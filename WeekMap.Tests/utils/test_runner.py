import time
import requests
from utils import config


def cleanup_test_database():
    try:
        response = requests.delete("https://localhost:7141/api/test-tools/cleanup-all", verify=False)
        status = "successful" if response.status_code == 200 else "unsuccessful"
        print(f"Database cleanup {status}, HTTP response status code: {response.status_code}\n")
    except Exception as e:
        print(f"Database cleanup failed: {e}\n")


def run_tests(*test_functions):
    print("=" * config.PRINT_SEPERATOR_LENGTH)
    print("RUNNING PYTHON TESTS")
    print("=" * config.PRINT_SEPERATOR_LENGTH)

    start_time = time.time()

    cleanup_test_database()
    for test_func in test_functions:
        test_func()
    cleanup_test_database()

    elapsed_seconds = time.time() - start_time
    print(f"Test completed in {elapsed_seconds:.2f} seconds")

    print("=" * config.PRINT_SEPERATOR_LENGTH)
    print("ALL DONE")
    print("=" * config.PRINT_SEPERATOR_LENGTH)
