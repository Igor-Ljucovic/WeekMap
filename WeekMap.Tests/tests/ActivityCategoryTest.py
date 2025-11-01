from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.support import expected_conditions as ec
from tests import activity_category_test_cases
from colorama import init, Fore
import re

# CONFIGURATIONS
init(autoreset=True)


def registration_dict_element_to_str(dict_element):
    return f"({dict_element['username']}, {dict_element['email']}, {dict_element['password']})"


def colorize_based_on_message_success(message, success):
    return (Fore.GREEN if success else Fore.RED) + message


# these 2 methods should probably be moved to a seperate file, as well as text colorization, dict elem to str,
# there should be 2 folders, 1 for test cases, 1 for tests
def register_user(driver, username, email, password):
    driver.get("http://localhost:3000/register")
    driver.find_element(By.ID, "username").send_keys(username)
    driver.find_element(By.ID, "email").send_keys(email)
    driver.find_element(By.ID, "password").send_keys(password)
    driver.find_element(By.ID, "confirmPassword").send_keys(password)
    driver.find_element(By.TAG_NAME, "form").submit()
    try:
        WebDriverWait(driver, 7).until(lambda d: d.find_element(By.ID, "registration-message").text.strip() != "")
        message = driver.find_element(By.ID, "registration-message").text
        return message
    except TimeoutException:
        return "Timed out waiting for the registration message"


def login_user(driver, email, password):
    driver.get("http://localhost:3000/register")
    login_link = WebDriverWait(driver, 5).until(ec.element_to_be_clickable((By.LINK_TEXT, "Log in")))
    login_link.click()

    WebDriverWait(driver, 5).until(ec.presence_of_element_located((By.ID, "email")))

    driver.find_element(By.ID, "email").send_keys(email)
    driver.find_element(By.ID, "password").send_keys(password)

    login_button = driver.find_element(By.XPATH, '//button[text()="Log in"]')
    login_button.click()

    try:
        WebDriverWait(driver, 7).until(lambda d: d.find_element(By.ID, "login-message").text.strip() != "")
        message = driver.find_element(By.ID, "login-message").text
        return message
    except TimeoutException:
        return "Timed out waiting for the login message"


def add_category(driver, name, color):

    def is_valid_hex_color(hex_color):
        # we need this because the JavaScript script will accept any string as a color
        return re.fullmatch(r"[0-9a-fA-F]{6}", hex_color) is not None

    if not is_valid_hex_color(color):
        print(f"[ERROR] Invalid hex color: {color}")
        return "Failure"

    try:
        WebDriverWait(driver, 5).until(ec.presence_of_element_located((By.NAME, "name")))

        driver.find_element(By.NAME, "name").clear()
        driver.find_element(By.NAME, "name").send_keys(name)

        driver.execute_script("""
            const colorInput = document.getElementsByName('color')[0];
            colorInput.value = arguments[0];
            colorInput.dispatchEvent(new Event('input', { bubbles: true }));
            colorInput.dispatchEvent(new Event('change', { bubbles: true }));
        """, f"#{color}")

        driver.find_element(By.CSS_SELECTOR, 'form button[type="submit"]').click()

        WebDriverWait(driver, 3).until(lambda d: d.find_element(By.ID, "category-message").text.strip() != "")

        result_text = driver.find_element(By.ID, "category-message").text.strip().lower()
        if "success" in result_text:
            return "Success"
        elif "failure" in result_text:
            return "Failure"
        else:
            return "Unknown"

    except TimeoutException:
        return "Failure"


def test_activity_category_scenario():
    driver = webdriver.Chrome()
    username = "someUser1"
    email = "someUser1@gmail.com"
    password = "StrongPassword123"

    try:
        register_user(driver, username, email, password)
        login_user(driver, email, password)

        driver.get("http://localhost:3000/activity-categories")

        successful_test_results = 0
        test_results = ""
        cases = activity_category_test_cases.get_test_cases()

        for case in cases:
            result = add_category(driver, case["name"], case["color"])
            expected = "Success" if case["is_valid"] else "Failure"
            total_result = "Passed" if result == expected else "Failed"
            if total_result == "Passed":
                successful_test_results += 1
            line = f"{total_result} (expected: {expected}, got: {result}) - " \
                   f"Name: {case['name']}, Color: {case['color']}\n"
            test_results += colorize_based_on_message_success(line, total_result == "Passed")

        print(test_results)
        final_summary = f"Activity Category test results: {successful_test_results}/{len(cases)}\n"
        print(colorize_based_on_message_success(final_summary, successful_test_results == len(cases)))
    finally:
        driver.quit()
