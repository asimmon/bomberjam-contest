<?php

namespace bomberjam;

class Logger
{
    /** @var resource */
    private $file;

    public function __construct()
    {
        $this->file = null;
    }

    /**
     * @param $filename string
     */
    public function setup($filename)
    {
        if (is_null($this->file))
            $this->file = fopen($filename, 'w');
    }

    /**
     * @param $text string
     */
    public function debug($text)
    {
        $this->write('DEBUG', $text);
    }

    /**
     * @param $text string
     */
    public function info($text)
    {
        $this->write('INFO', $text);
    }

    /**
     * @param $text string
     */
    public function warn($text)
    {
        $this->write('WARN', $text);
    }

    /**
     * @param $text string
     */
    public function error($text)
    {
        $this->write('WARN', $text);
    }

    private function write($level, $text)
    {
        if (!is_null($this->file)) {
            fwrite($this->file, $level . ': ' . $text . PHP_EOL);
        }
    }

    public function close()
    {
        if (!is_null($this->file)) {
            fclose($this->file);
            $this->file = null;
        }
    }

    public function __destruct()
    {
        $this->close();
    }
}