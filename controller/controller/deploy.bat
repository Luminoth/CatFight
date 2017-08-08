@echo off

set NG=ng
set BUILDARGS=build --aot --prod
set DISTDIR=dist
set CONTROLLER_SRC=%DISTDIR%\index.html

if [%1] == [] goto usage
if [%2] == [] goto usage

@echo Cleaning %DISTDIR%...

del /F /Q %DISTDIR% 2>nul

@echo Building, this may take a moment...

call %NG% %BUILDARGS%

@echo Copying controller asset %CONTROLLER_SRC% to %1...

xcopy /Y %CONTROLLER_SRC% %1

@echo Copying controller assets from %DISTDIR% to %2...

xcopy /Y /E /I %DISTDIR%\assets %2\assets
xcopy /Y %DISTDIR%\favicon.ico %2
xcopy /Y %DISTDIR%\*.css %2
xcopy /Y %DISTDIR%\*.js %2

goto :eof

:usage
@echo Usage: %0 ^<controller asset path^> ^<controller assets path^>
exit /B 1
