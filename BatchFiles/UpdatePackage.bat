@ECHO OFF
COLOR 2

SETLOCAL ENABLEDELAYEDEXPANSION ENABLEEXTENSIONS

SET NPPDir=

REM This will find the greatest package number
FOR /D %%D IN ("C:\Users\Public\Anypay[21.110.0032]*") DO (
	SET NPPDir=%%D
)

RMDIR ..\Package\Anypay\_project /S /Q
RMDIR ..\Package\Anypay\ReportsCustomized /S /Q
RMDIR ..\Package\Anypay\ReportsDefault /S /Q
RMDIR ..\Package\Anypay /S /Q
XCOPY !NPPDir!\_project ..\Package\Anypay\_project\ /S /Q /E /Y
XCOPY !NPPDir!\ReportsCustomized ..\Package\Anypay\ReportsCustomized\ /S /Q /E /Y
XCOPY !NPPDir!\ReportsDefault ..\Package\Anypay\ReportsDefault\ /S /Q /E /Y

PAUSE