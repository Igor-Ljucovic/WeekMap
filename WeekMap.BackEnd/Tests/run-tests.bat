@echo off
title Running WeekMap Tests
echo =============================
echo CHECKING BACKEND (https://localhost:7141)
echo =============================

:: Try to request the root or test endpoint to check actual responsiveness
powershell -Command "$r = try { Invoke-WebRequest https://localhost:7141 -UseBasicParsing -TimeoutSec 3 } catch { $null }; if ($r -eq $null) { exit 1 }"
if %errorlevel% neq 0 (
    echo Starting backend on port 7141...
    cd /d "E:\JOB\Git Hub my-projects\my-personal-projects\WeekMap\WeekMap.BackEnd"
    start "WeekMap_Backend" /min dotnet run --launch-profile "Test"
    
    echo Waiting for backend to respond...
    :backend_wait
    powershell -Command "$r = try { Invoke-WebRequest https://localhost:7141 -UseBasicParsing -TimeoutSec 1 } catch { $null }; if ($r -eq $null) { exit 1 }"
    if %errorlevel% neq 0 (
        timeout /t 1 >nul
        goto backend_wait
    )
    echo Backend is up!
) else (
    echo Backend already running and responsive.
)

echo =============================
echo CHECKING FRONTEND (http://localhost:3000)
echo =============================

powershell -Command "$r = try { Invoke-WebRequest http://localhost:3000 -UseBasicParsing -TimeoutSec 3 } catch { $null }; if ($r -eq $null) { exit 1 }"
if %errorlevel% neq 0 (
    echo Starting frontend on port 3000...
    cd /d "E:\JOB\Git Hub my-projects\my-personal-projects\WeekMap\WeekMap.FrontEnd"
    start "WeekMap_Frontend" npm start
) else (
    echo Frontend already running and responsive.
)

echo =============================
echo RUNNING PYTHON TESTS
echo =============================
cd /d "E:\JOB\Git Hub my-projects\my-personal-projects\WeekMap\WeekMap.Tests"
python main.py

echo =============================
echo ALL DONE
echo =============================
pause  - can you tell me what exactly this code does? I think it doesnt give enough time for frontend to start, so it does the tests before frontend is ready.