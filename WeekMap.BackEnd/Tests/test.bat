@echo off
set /p MODE=Enter mode (start/test): 
powershell -ExecutionPolicy Bypass -File "test.ps1" %MODE%
pause