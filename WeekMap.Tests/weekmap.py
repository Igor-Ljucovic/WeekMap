import urllib3
import os
import sys
from tools.project_setup import setup_project
from tools.test_runner import run_tests

if __name__ == "__main__":
    urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
    command = sys.argv[1] if len(sys.argv) > 1 else ""

    if command == "test" or command == "start" or command == "":
        urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
        setup_project()

        if command == "test" or command == "":
            run_tests()
    else:
        script_name = os.path.splitext(os.path.basename(sys.argv[0]))[0]
        print("Usage:")
        print(f"{script_name} start   # Start only backend and frontend")
        print(f"{script_name} test    # Run all tests")
        print(f"(try 'python {script_name}.py ...' if '{script_name}' doesn't work)")
        print(f"('{script_name}' is the name of the python file that runs tests, try a different name if it doesn't work)")