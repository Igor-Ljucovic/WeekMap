import urllib3
import os
import sys
from tools.project_setup import setup_project
from tools.test_runner import run_tests
from tests.RegisterTest import test_registration_scenario
from tests.ActivityCategoryTest import test_activity_category_scenario

if __name__ == "__main__":
    urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
    command = sys.argv[1] if len(sys.argv) > 1 else ""

    if command in ["start", "test", "activitycategory"]:
        urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
        setup_project()

    if command == "test":
        run_tests(test_registration_scenario, test_activity_category_scenario)
    elif command == "activitycategory":
        run_tests(test_activity_category_scenario)
    else:
        script_name = os.path.splitext(os.path.basename(sys.argv[0]))[0]
        print("Usage:")
        print(f"start               # Start only backend and frontend")
        print(f"test                # Run all tests")
        print(f"activitycategory    # Run activity category tests")
        print(f"(try 'python {script_name}.py ...' if '{script_name}' doesn't work)")
        print(f"('{script_name}' - name of the python file that runs tests, try a different name if it doesn't work)")
