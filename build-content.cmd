echo off
cls

if not exist tools\Lunt.Make\Lake.exe ( 
	echo Installing Lunt Make...
	"tools\nuget.exe" "install" "Lunt.Make" "-OutputDirectory" "tools" "-ExcludeVersion" "-NonInteractive"
	echo.
)

cd tools/Lunt.Make
Lake.exe -i="../../assets/" -p="../../src/Surface.Pipeline/bin/Debug" -o="../../src/Surface/bin/Debug/data" -v=diagnostic -rebuild "../../assets/build.xml"

pause
