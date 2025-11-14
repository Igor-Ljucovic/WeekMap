import urllib3
import os
import sys
from utils.project_setup import setup_project
from utils.test_runner import run_tests
from tests.register_test import RegisterTest
from tests.activity_category_test import ActivityCategoryTest

if __name__ == "__main__":
    urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
    command = sys.argv[1] if len(sys.argv) > 1 else ""

    if command in ["start", "test", "activitycategory", "register"]:
        urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
        setup_project()

    if command == "test":
        rt = RegisterTest()
        act = ActivityCategoryTest()
        run_tests(rt.run_all_test_cases, act.run_all_test_cases)
    elif command == "register":
        rt = RegisterTest()
        run_tests(rt.run_all_test_cases)
    elif command == "activitycategory":
        act = ActivityCategoryTest()
        run_tests(act.run_all_test_cases)
    else:
        script_name = os.path.splitext(os.path.basename(sys.argv[0]))[0]
        print("Usage:")
        print(f"start               # Start only backend and frontend")
        print(f"test                # Run all tests")
        print(f"register            # Run register tests")
        print(f"activitycategory    # Run activity category tests")
        print(f"(try 'python {script_name}.py ...' if '{script_name}' doesn't work)")
        print(f"('{script_name}' - name of the python file that runs tests, try a different name if it doesn't work)")
