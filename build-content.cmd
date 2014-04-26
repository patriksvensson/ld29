echo off
cls

cd tools
Lunt.exe -i="../assets/" -o="../src/Surface/bin/Debug/data" -v=diagnostic "../assets/build.xml"

pause
