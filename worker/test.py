import backend
import util
import compiler
import sys


# sh main.sh && sudo -H -iu bot_compilation bash -c "cd && pwd && dotnet new console -n MyBot -o MyBot" && python3 test.py "/home/bot_compilation/MyBot" && ls -la "/home/bot_compilation/MyBot"


if __name__ == "__main__":
    util.setup_logging()
    # backend.upload_bot(1, 'C:\\Users\\simmo\\Desktop\\bot-123-0.zip')
    # backend.download_bot(1, 'C:\\Users\\simmo\\Desktop\\')
    # backend.send_compilation_result(1, False, 'C#', '')
    # task = backend.get_next_task()
    # if task is not None:
    #     backend.mark_task_started(task['id'])
    #     backend.mark_task_finished(task['id'])
    if len(sys.argv) > 1:
        workingPath = sys.argv[1]
        compiler.compile_anything(sys.argv[1])
