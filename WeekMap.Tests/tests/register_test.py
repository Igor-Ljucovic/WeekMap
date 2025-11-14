from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.common.exceptions import TimeoutException
from utils import console_helpers, base_test
from test_data.register_test_data import RegisterTestData


class RegisterTest(base_test.BaseTest):
    def run_test_case(self, driver, case):
        driver.get("http://localhost:3000/register")
        driver.find_element(By.ID, "username").send_keys(case["username"])
        driver.find_element(By.ID, "email").send_keys(case["email"])
        driver.find_element(By.ID, "password").send_keys(case["password"])
        driver.find_element(By.ID, "confirmPassword").send_keys(case["password"])
        driver.find_element(By.TAG_NAME, "form").submit()
        try:
            # wait at most 7 seconds until a new "p" tag appeas in the DOM
            WebDriverWait(driver, 7).until(lambda d: d.find_element(By.ID, "registration-message").text.strip() != "")
            message = driver.find_element(By.ID, "registration-message").text
            return message
        except TimeoutException:
            return "Timed out waiting for the registration message"

    def run_all_test_cases(self):
        successful_registration_message = 'Registration successful!'
        test_results = ""
        successful_test_results = 0
        rtd = RegisterTestData()
        cases = rtd.test_data
        driver = webdriver.Chrome()
        try:
            for case in cases:
                registration_message = self.run_test_case(driver, case)
                expected_result = "Success" if (case["expected_success"]) else "Failure"
                result = "Success" if (registration_message == successful_registration_message) else "Failure"
                total_result = "Passed" if (expected_result == result) else "Failed"
                if expected_result == result:
                    successful_test_results += 1
                case_result = f"{total_result} (expected: {expected_result}, got: {result}) - " \
                              f"{console_helpers.dict_to_str(case)} - {registration_message}\n"
                test_results += console_helpers.colorize_based_on_message_success(case_result,
                                                                                  expected_result == result)
        finally:
            driver.quit()

        print(test_results)
        registration_test_results = f"Registration test results: {successful_test_results}/{len(cases)}\n"
        print(console_helpers.colorize_based_on_message_success(registration_test_results,
                                                                successful_test_results == len(cases)))
