from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.common.exceptions import TimeoutException
from tests import register_test_cases
from colorama import init, Fore

# CONFIGURATIONS
init(autoreset=True)


def registration_dict_element_to_str(dict_element):
    return f"({dict_element['username']}, {dict_element['email']}, {dict_element['password']})"


def colorize_based_on_message_success(message, success):
    return (Fore.GREEN if success else Fore.RED) + message


def register_user(driver, case):
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


def test_registration_scenario():
    successful_registration_message = 'Registration successful!'
    test_results = ""
    successful_test_results = 0
    cases = register_test_cases.generate_test_cases(10, True)
    driver = webdriver.Chrome()
    try:
        for case in cases:
            registration_message = register_user(driver, case)
            expected_result = "Success" if (case["is_valid"]) else "Failure"
            result = "Success" if (registration_message == successful_registration_message) else "Failure"
            total_result = "Passed" if (expected_result == result) else "Failed"
            if expected_result == result:
                successful_test_results += 1
            case_result = f"{total_result} (expected: {expected_result}, got: {result}) - " \
                          f"{registration_dict_element_to_str(case)} - {registration_message}\n"
            test_results += colorize_based_on_message_success(case_result, expected_result == result)
    finally:
        driver.quit()

    print(test_results)
    registration_test_results = f"Registration test results: {successful_test_results}/{len(cases)}\n"
    print(colorize_based_on_message_success(registration_test_results, successful_test_results == len(cases)))
