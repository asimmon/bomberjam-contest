import collections
import errno
import fnmatch
import logging
import os
import os.path
import re
import subprocess

import util

try:
    from server_info import server_info
    MEMORY_LIMIT = server_info.get('memory_limit', 1500)
except ImportError:
    server_info = None
    MEMORY_LIMIT = 1500

BOT = "MyBot"
# Which file is used to override the detected language?
LANGUAGE_FILE = "LANGUAGE"
SAFEPATH = re.compile('[a-zA-Z0-9_.$-]+$')


class CD(object):
    """
    A context manager that enters a given directory and restores the old one.
    """
    def __init__(self, new_dir):
        self.new_dir = new_dir

    def __enter__(self):
        self.org_dir = os.getcwd()
        os.chdir(self.new_dir)
        return self.new_dir

    def __exit__(self, exc_type, exc_val, exc_tb):
        os.chdir(self.org_dir)


def safeglob(pattern):
    safepaths = []
    for root, dirs, files in os.walk("."):
        files = fnmatch.filter(files, pattern)
        for fname in files:
            if SAFEPATH.match(fname):
                safepaths.append(os.path.join(root, fname))
    return safepaths


def safeglob_multi(patterns):
    safepaths = []
    for pattern in patterns:
        safepaths.extend(safeglob(pattern))
    return safepaths


def nukeglob(pattern):
    paths = safeglob(pattern)
    for path in paths:
        try:
            os.unlink(path)
        except OSError as e:
            if e.errno != errno.ENOENT and e.errno != errno.EACCES:
                raise


def tree(dir_path):
    _run_cmd("tree -pufiag .", dir_path)


def _run_cmd(cmd, working_dir, timelimit=10, envvars=''):
    """Run a compilation command in an isolated sandbox. Returns the value of stdout as well as any errors that occurred."""
    absolute_working_dir = os.path.abspath(working_dir)
    cmd = 'sudo -H -iu bot_compilation ' + envvars + ' bash -c "cd ' + absolute_working_dir + ' && ' + cmd + '"'
    logging.info("> %s" % cmd)
    process = subprocess.Popen(cmd, cwd=working_dir, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE, stdin=subprocess.PIPE)

    try:
        raw_stdout, raw_strerr = process.communicate(timeout=timelimit)
        str_stdout = raw_stdout.decode("utf-8").strip()
        arr_stdout = str_stdout.split("\n") if not str_stdout.isspace() and str_stdout != "" else []

        str_stderr = raw_strerr.decode("utf-8").strip()
        arr_stderr = str_stderr.split("\n") if not str_stderr.isspace() and str_stderr != "" else []

        if str_stdout is not None and len(str_stdout) > 0:
            logging.info(str_stdout)

        if str_stderr is not None and len(str_stderr) > 0:
            logging.info(str_stderr)
    except subprocess.TimeoutExpired:
        arr_stdout = []
        arr_stderr = ["Compilation timed out while executing command %s" % cmd]

    # Clean up any processes that didn't exit cleanly
    util.kill_processes_as("bot_compilation")

    if len(arr_stdout) > 0:
        logging.info("> stdout: " + "\n".join(arr_stdout))

    if len(arr_stderr) > 0:
        logging.info("> stderr: " + "\n".join(arr_stderr))

    return arr_stdout, arr_stderr, process.returncode


def check_path(path, errors):
    print(path)
    if not os.path.exists(path):
        errors.append("Output file " + str(os.path.basename(path)) + " was not created.")
        return False
    else:
        return True


class Compiler(object):
    """A way to compile an uploaded bot."""
    def compile(self, bot_dir, globs, errors, timelimit):
        raise NotImplementedError


class NoneCompiler(Compiler):
    """A compiler that does nothing."""
    def __init__(self, language):
        self.language = language

    def __str__(self):
        return "NoneCompiler: %s" % self.language

    def compile(self, bot_dir, globs, errors, timelimit):
        return True


class ChmodCompiler(Compiler):
    """A compiler that simply sets the executable flag on a file."""
    def __init__(self, language):
        self.language = language

    def __str__(self):
        return "ChmodCompiler: %s" % self.language

    def compile(self, bot_dir, globs, errors, timelimit):
        with CD(bot_dir):
            for f in safeglob_multi(globs):
                try:
                    os.chmod(f, 0o755)
                except Exception as e:
                    errors.append("Error chmoding %s - %s\n" % (f, e))
        return True


class ExternalCompiler(Compiler):
    """
    A compiler that calls an external process.
    """
    def __init__(self, args, separate=False, out_files=[], out_ext=None, trailing_args=[]):
        self.args = args
        # Arguments that must follow the filename (e.g. `-lm` for C compilers)
        self.trailing_args = trailing_args
        self.separate = separate
        self.out_files = out_files
        self.out_ext = out_ext
        self.stderr_re = re.compile("WARNING: IPv4 forwarding is disabled")

    def __str__(self):
        return "ExternalCompiler: %s" % (' '.join(self.args),)

    def compile(self, bot_dir, globs, errors, timelimit):
        with CD(bot_dir):
            if len(globs) > 0:
                logging.debug("Searching for files matching: " + ", ".join(globs))
            files = safeglob_multi(globs)
            if len("".join(globs)) != 0 and len(files) == 0:
                # no files to compile
                return True

        try:
            if self.separate:
                for filename in files:
                    logging.debug("Handling file: " + filename)
                    cmdline = " ".join(self.args + [filename] + self.trailing_args)
                    cmd_out, cmd_errors, returncode = _run_cmd(cmdline, bot_dir, timelimit)
                    cmd_errors = self.cmd_error_filter(cmd_out, cmd_errors, returncode)
                    if not cmd_errors:
                        for ofile in self.out_files:
                            check_path(os.path.join(bot_dir, ofile), cmd_errors)
                        if self.out_ext:
                            oname = os.path.splitext(filename)[0] + self.out_ext
                            check_path(os.path.join(bot_dir, oname), cmd_errors)
                        if cmd_errors:
                            cmd_errors += cmd_out
                    if cmd_errors:
                        errors += cmd_errors
                        return False
            else:
                if len(files) > 0:
                    logging.debug("Handling files: " + ", ".join(files))
                cmdline = " ".join(self.args + files + self.trailing_args)
                cmd_out, cmd_errors, returncode = _run_cmd(cmdline, bot_dir, timelimit)
                cmd_errors = self.cmd_error_filter(cmd_out, cmd_errors, returncode)
                if not cmd_errors:
                    for ofile in self.out_files:
                        check_path(os.path.join(bot_dir, ofile), cmd_errors)
                    if self.out_ext:
                        for filename in files:
                            oname = os.path.splitext(filename)[0] + self.out_ext
                            check_path(os.path.join(bot_dir, oname), cmd_errors)
                    if cmd_errors:
                        cmd_errors += cmd_out
                if cmd_errors:
                    errors += cmd_errors
                    return False
        except:
            pass
        return True

    def cmd_error_filter(self, cmd_out, cmd_errors, returncode):
        cmd_errors = [line for line in cmd_errors if line is not None and self.stderr_re.search(line)]
        return cmd_errors


class ErrorFilterCompiler(ExternalCompiler):
    def __init__(self, args, separate=False, out_files=[], out_ext=None, stdout_is_error=False, skip_stdout=0, filter_stdout=None, filter_stderr=None):
        ExternalCompiler.__init__(self, args, separate, out_files, out_ext)
        self.stdout_is_error = stdout_is_error
        self.skip_stdout = skip_stdout;
        if filter_stdout is None:
            self.stdout_re = None
        else:
            self.stdout_re = re.compile(filter_stdout)
        if filter_stderr is None:
            self.stderr_re = None
        else:
            self.stderr_re = re.compile(filter_stderr)

    def __str__(self):
        return "ErrorFilterCompiler: %s" % (' '.join(self.args),)

    def cmd_error_filter(self, cmd_out, cmd_errors, returncode):
        cmd_errors = ExternalCompiler.cmd_error_filter(self, cmd_out, cmd_errors, returncode)

        if self.skip_stdout > 0:
            del cmd_out[:self.skip_stdout]
        # Somehow there are None values in the output
        if self.stdout_re is not None:
            cmd_out = [line for line in cmd_out if line is not None and self.stdout_re.search(line)]
        if self.stderr_re is not None:
            cmd_errors = [line for line in cmd_errors if line is not None and self.stderr_re.search(line)]
        if self.stdout_is_error:
            cmd_errors = [line for line in cmd_out if line is not None and self.stderr_re.search(line)]

        return cmd_errors


PYTHON_EXT_COMPILER = '''"from distutils.core import setup
from distutils.extension import read_setup_file
setup(ext_modules = read_setup_file('setup_exts'), script_args = ['-q', 'build_ext', '-i'])"'''

# Arrays of compiler commands to run for each language.
comp_args = {
    "C#": [
        ["dotnet", "restore"],
        ["dotnet", "build", "--no-restore", "--nologo", "-c", "Release", "-o", "./__dist__/", "MyBot.csproj"]
    ],
    "Java": [
        ["javac", "-cp", "'.:*'", "-encoding", "UTF-8", "-J-Xmx%sm" % MEMORY_LIMIT]
    ],
    "Python": [
        ["python3.9", "-c", PYTHON_EXT_COMPILER]
    ]
}

Language = collections.namedtuple("Language", [
    'name',
    'input',
    'output',
    'command_func',
    'envvars_func',
    'nukeglobs',
    'compilers'])

# The actual languages supported.
languages = (
    Language(
        "C#",
        BOT + ".csproj",
        f"./__dist__/{BOT}.dll",
        lambda bot_dir: "dotnet ./__dist__/MyBot.dll",
        lambda bot_dir: "",
        [f"./__dist__/{BOT}.dll"],
        [
            ([], ExternalCompiler(comp_args["C#"][0])),
            ([], ErrorFilterCompiler(comp_args["C#"][1], filter_stderr="(: error (CS|MSB)|Build FAILED)", stdout_is_error=True))
        ]
    ),
    Language(
        "Java",
        BOT + ".java",
        BOT + ".java",
        lambda bot_dir: "java -cp \".:*\" MyBot",
        lambda bot_dir: "",
        ["*.class"],
        [(["*.java"], ErrorFilterCompiler(comp_args["Java"][0], filter_stderr=": error:", out_files=["MyBot.class"]))]
    ),
    Language(
        "JavaScript",
        BOT + ".js",
        BOT + ".js",
        lambda bot_dir: "node MyBot.js",
        lambda bot_dir: "",
        [],
        [(["*.js"], NoneCompiler("JavaScript"))]
    ),
    Language(
        "Python",
        BOT + ".py",
        BOT + ".py",
        lambda bot_dir: "PYTHONPATH=__botpythonpackages__ bash -c python3.9 MyBot.py",
        lambda bot_dir: f"PIP_TARGET={bot_dir}/__botpythonpackages__",
        ["*.pyc"],
        [
            (["*.py"], NoneCompiler("Python")),
            (["setup_exts"], ErrorFilterCompiler(comp_args["Python"][0], separate=True, filter_stderr='-Wstrict-prototypes'))
        ]
    )
)


def compile_function(language, bot_dir, timelimit):
    with CD(bot_dir):
        for glob in language.nukeglobs:
            logging.debug("Removing any existing files matching: " + glob)
            nukeglob(glob)

    errors = []
    for globs, compiler in language.compilers:
        try:
            if not compiler.compile(bot_dir, globs, errors, timelimit):
                return False, errors
        except Exception as ex:
            errors.append("Compiler %s failed with: %s" % (compiler, ex))
            return False, errors

    compiled_bot_file = os.path.join(bot_dir, language.output)
    return check_path(compiled_bot_file, errors), errors


_MANY_LANG_FOUND = """Found multiple MyBot.* entry files:
%s"""

_LANG_NOT_FOUND = """Did not find a recognized MyBot.* entry file. Please add one of the following filenames to your zip file:
%s"""


def detect_language(bot_dir):
    with CD(bot_dir):
        detected_langs = [
            lang for lang in languages if os.path.exists(lang.input)
        ]

        if len(detected_langs) > 1:
            return None, [_MANY_LANG_FOUND % '\n'.join([dl.input for dl in detected_langs])]
        elif len(detected_langs) == 0:
            return None, [_LANG_NOT_FOUND % ('\n'.join(lg.name + ': ' + lg.input for lg in languages))]
        else:
            return detected_langs[0], None


def detect_language_file(bot_dir):
    with CD(bot_dir):
        try:
            with open(LANGUAGE_FILE, 'r') as lang_file:
                print("detected %s file" % LANGUAGE_FILE)
                language_name = lang_file.readline().strip()

                if not language_name:
                    return None
                else:
                    return language_name
        except IOError:
            return None


def get_run_cmd(submission_dir):
    with CD(submission_dir):
        if os.path.exists('run.sh'):
            with open('run.sh') as f:
                for line in f:
                    if line[0] != '#':
                        return line.rstrip('\r\n')


def get_run_lang(submission_dir):
    with CD(submission_dir):
        if os.path.exists('run.sh'):
            with open('run.sh') as f:
                for line in f:
                    if line[0] == '#':
                        return line[1:-1]


INSTALL_ERROR_START = "Possible errors running install.sh. stdout output as follows:"
INSTALL_ERROR_MID = "End of install.sh stdout. stderr as follows:"
INSTALL_ERROR_END = "End of install.sh output."


def truncate_errors(install_stdout, install_errors, language_detection_errors,
                    compile_errors, max_error_len=10*1024):
    """
    Combine lists of errors into a single list under a maximum length.
    """
    install_stdout = install_stdout or []
    install_errors = install_errors or []
    language_detection_errors = language_detection_errors or []
    compile_errors = compile_errors or []

    all_errors = install_stdout + install_errors + language_detection_errors + compile_errors
    result = []

    if sum(len(line) for line in all_errors) <= max_error_len:
        if install_stdout or install_errors:
            result.append(INSTALL_ERROR_START)
            result.extend(install_stdout)
            result.append(INSTALL_ERROR_MID)
            result.extend(install_errors)
            result.append(INSTALL_ERROR_END)
        result.extend(language_detection_errors)
        result.extend(compile_errors)
        return result

    def bound_errors(source, bound):
        total_length = sum(len(line) for line in source)
        if total_length <= bound:
            return total_length, source

        length = 0
        current = 0
        result = []
        # Take 1/3 from start of errors
        while current < len(source) and (
                length == 0 or
                length + len(source[current]) < bound // 3):
            result.append(source[current])
            length += len(source[current])
            current += 1

        if current < len(source):
            result.append("...(output truncated)...")

            end_errors = []
            end = current
            current = -1
            while current >= -(len(source) - end) and (
                    len(end_errors) == 0 or
                    length + len(source[current])) < bound:
                end_errors.append(source[current])
                length += len(source[current])
                current -= 1
            result.extend(reversed(end_errors))

        return length, result

    remaining_length = max_error_len
    if install_stdout or install_errors:
        result.append(INSTALL_ERROR_START)
        used, lines = bound_errors(install_stdout, 0.2 * max_error_len)
        remaining_length -= used
        result.extend(lines)
        result.append(INSTALL_ERROR_MID)
        used, lines = bound_errors(install_errors,
                                   max(0.3 * max_error_len,
                                       0.5 * max_error_len - used))
        remaining_length -= used
        result.extend(lines)
        result.append(INSTALL_ERROR_END)

    _, lines = bound_errors(language_detection_errors + compile_errors, remaining_length)
    result.extend(lines)
    return result


def compile_anything(bot_dir, install_time_limit=300, compile_time_limit=300, max_error_len=10 * 1024):
    install_stdout = []
    install_errors = []

    logging.info("Detecting language...")
    detected_language, language_errors = detect_language(bot_dir)

    if not detected_language:
        return "Unknown", truncate_errors(install_stdout, install_errors, language_errors, [], max_error_len)
    else:
        logging.info("Detected language: %s" % detected_language.name)

    tree(bot_dir)

    if os.path.exists(os.path.join(bot_dir, "install.sh")):
        envvars = detected_language.envvars_func(bot_dir)
        install_stdout, install_errors, _ = _run_cmd("bash ./install.sh", bot_dir, install_time_limit, envvars)

    logging.debug("Compiling...")
    compiled, compile_errors = compile_function(detected_language, bot_dir, compile_time_limit)
    if not compiled or compile_errors:
        return detected_language.name, truncate_errors(install_stdout, install_errors, language_errors, compile_errors, max_error_len)

    tree(bot_dir)

    run_cmd = detected_language.command_func(bot_dir)
    run_filename = os.path.join(bot_dir, 'run.sh')
    logging.debug("Compilation done, writing run.sh file at '%s' with command '%s'" % (run_filename, run_cmd))

    try:
        with open(run_filename, 'wb') as f:
            f.write(('# Language: %s\n%s\n' % (detected_language.name, run_cmd)).encode('utf-8'))
    except Exception as ex:
        compile_errors.append("Could not write command file '%s': %s" % (run_filename, ex))
        return detected_language.name, truncate_errors(install_stdout, install_errors, language_errors, compile_errors, max_error_len)

    return detected_language.name, None
