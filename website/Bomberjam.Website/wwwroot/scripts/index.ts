function sayHello(name: string): Promise<string> {
  return new Promise<string>(resolve => {
    window.setTimeout(() => {
      resolve('Hello ' + name);
    }, 1000);
  });
}

async function printHello(name: string) {
  const text = await sayHello(name);
  console.log(text);
}

async function main() {
  console.log('About to say hello world in one second');
  await printHello('world');
}

main().catch(console.log);