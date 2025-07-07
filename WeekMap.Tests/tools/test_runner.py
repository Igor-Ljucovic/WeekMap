import time
import requests
from tests.RegisterTest import test_registration_scenario
from tools import config

def cleanup_test_database():
    try:
        response = requests.delete("https://localhost:7141/api/test-tools/cleanup-all", verify=False)
        print(f"Database cleanup complete, HTTP response status code: {response.status_code}\n")
    except Exception as e:
        print(f"Database cleanup failed: {e}\n")

def run_tests():

    print("=" * config.PRINT_SEPERATOR_LENGTH)
    print("RUNNING PYTHON TESTS")
    print("=" * config.PRINT_SEPERATOR_LENGTH)

    start_time = time.time()

    cleanup_test_database()
    test_registration_scenario()
    cleanup_test_database()

    elapsed_seconds = time.time() - start_time
    print(f"Test completed in {elapsed_seconds:.2f} seconds")

    print("=" * config.PRINT_SEPERATOR_LENGTH)
    print("ALL DONE")
    print("=" * config.PRINT_SEPERATOR_LENGTH)
