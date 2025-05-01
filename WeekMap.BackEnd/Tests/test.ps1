param(
    [Parameter(Mandatory = $false)]
    [string]$Command = "start"
)

# CONFIGURATIONS
$BACKEND_PORT = 7141
$FRONTEND_PORT = 3000
$BACKEND_PATH = "E:\JOB\Git Hub my-projects\my-personal-projects\WeekMap\WeekMap.BackEnd"
$FRONTEND_PATH = "E:\JOB\Git Hub my-projects\my-personal-projects\WeekMap\WeekMap.FrontEnd"
$PYTHON_SCRIPT = "E:\JOB\Git Hub my-projects\my-personal-projects\WeekMap\WeekMap.Tests\weekmap.py"

function Test-PortOpen {
    param($port)
    try {
        $tcp = New-Object Net.Sockets.TcpClient
        $tcp.Connect("127.0.0.1", $port)
        $tcp.Close()
        return $true
    } catch {
        return $false
    }
}

function Wait-ForService {
    param($port)
    while (-not (Test-PortOpen $port)) {
        Start-Sleep -Seconds 1
    }
}

function Start-Backend {
    if (-not (Test-PortOpen $BACKEND_PORT)) {
        Start-Process "cmd.exe" "/c dotnet run --launch-profile Test" -WorkingDirectory $BACKEND_PATH | Out-Null
        Wait-ForService -port $BACKEND_PORT
    }
}

function Start-Frontend {
    if (-not (Test-PortOpen $FRONTEND_PORT)) {
        Start-Process "cmd.exe" "/c npm start" -WorkingDirectory $FRONTEND_PATH | Out-Null
        Wait-ForService -port $FRONTEND_PORT
    }
}

# MAIN EXECUTION
$null = Start-Backend
$null = Start-Frontend

# Run Python script with forwarded command (start or test)
python "`"$PYTHON_SCRIPT`"" $Command
