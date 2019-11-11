::@echo off

:: To gather all the console output/errors

::MedocUpdates.exe -minimize >> stdouterr.log
MedocUpdates.exe -minimize 1>> stdouterr.log 2>>&1
exit