@echo off
set botFolder=%1

mkdir empty
robocopy .\empty .\published /MIR /NFL /NDL /NJH /NJS /NC /NS /NP
rmdir empty

robocopy .\%botFolder% .\published /MIR /NFL /NDL /NJH /NJS /NC /NS /NP /XD venv scripts .idea logs __pycache__ /XF replay.json README
@echo on