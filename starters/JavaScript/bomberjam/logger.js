const fs = require('fs');

const format = (args) => {
  return args.map(x => typeof x !== 'string' ? x.toString() : x).join(' ');
}

class Logger {
  constructor() {
    this.file = null;
  }

  setup(filename) {
    if (!this.file)
      this.file = fs.createWriteStream(filename, {flags: 'w'});
  }

  debug(...args) {
    if (this.file)
      this.file.write(`DEBUG: ${format(args)}\n`);
  }

  info(...args) {
    if (this.file)
      this.file.write(`INFO: ${format(args)}\n`);
  }

  warn(...args) {
    if (this.file)
      this.file.write(`WARN: ${format(args)}\n`);
  }

  error(...args) {
    if (this.file)
      this.file.write(`ERROR: ${format(args)}\n`);
  }

  close() {
    if (this.file) {
      this.file.end();
      this.file = null;
    }
  }
}

module.exports = Logger;