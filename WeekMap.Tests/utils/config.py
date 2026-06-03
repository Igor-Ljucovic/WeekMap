import os

BACKEND_PORT = int(os.environ.get("BACKEND_PORT", 7141))
BACKEND_PROTOCOL = os.environ.get("BACKEND_PROTOCOL", "https")
BACKEND_BASE_URL = f"{BACKEND_PROTOCOL}://localhost:{BACKEND_PORT}"
FRONTEND_PORT = int(os.environ.get("FRONTEND_PORT", 3000))
BACKEND_PATH = os.environ.get("BACKEND_PATH", r"E:\JOB\git-hub-projects\WeekMap\WeekMap.BackEnd.ASP.NET")
FRONTEND_PATH = os.environ.get("FRONTEND_PATH", r"E:\JOB\git-hub-projects\WeekMap\WeekMap.FrontEnd.REACT")

PRINT_SEPERATOR_LENGTH = 44
