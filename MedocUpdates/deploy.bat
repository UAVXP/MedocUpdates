::@echo off

cd bin\Release

copy *.exe D:\Clouds\Net\MedocUpdates
copy *.dll D:\Clouds\Net\MedocUpdates

copy *.pdb D:\Clouds\Net\MedocUpdates
copy *.exe.config D:\Clouds\Net\MedocUpdates
copy *.xml D:\Clouds\Net\MedocUpdates

mkdir D:\Clouds\Net\MedocUpdates\lang
copy lang\*.json D:\Clouds\Net\MedocUpdates\lang
pause