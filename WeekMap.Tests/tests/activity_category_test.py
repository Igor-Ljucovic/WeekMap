from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.support import expected_conditions as ec
from utils import auth_actions, console_helpers, base_test
from test_data.activity_category_test_data import ActivityCategoryTestData
import re


class ActivityCategoryTest(base_test.BaseTest):
    def run_test_case(self, driver, case):
        def is_valid_hex_color(hex_color):
            # we need this because the color is inputted by choosing it from a graphical element, so we can't
            # actually change it in the DOM, so we are going to test the color like this instead
            return re.fullmatch(r"[0-9a-fA-F]{6}", hex_color) is not None

        if not is_valid_hex_color(case['color']):
            print(f"[ERROR] Invalid hex color: {case['color']}")
            return "Failure"
        try:
            WebDriverWait(driver, 5).until(ec.presence_of_element_located((By.NAME, "name")))

            driver.find_element(By.NAME, "name").clear()
            driver.find_element(By.NAME, "name").send_keys(case['name'])

            # needed because the color is inputted by choosing it from a graphical element
            driver.execute_script("""
                const colorInput = document.getElementsByName('color')[0];
                colorInput.value = arguments[0];
                colorInput.dispatchEvent(new Event('input', { bubbles: true }));
                colorInput.dispatchEvent(new Event('change', { bubbles: true }));
            """, f"#{case['color']}")

            driver.find_element(By.CSS_SELECTOR, 'form button[type="submit"]').click()

            WebDriverWait(driver, 3).until(lambda d: d.find_element(By.ID, "category-message").text.strip() != "")

            result_text = driver.find_element(By.ID, "category-message").text.strip().lower()
            if "success" in result_text:
                try:
                    WebDriverWait(driver, 7).until(
                        lambda d: any(
                            case["name"] in el.text
                            for el in d.find_elements(By.CSS_SELECTOR, "#categories-list li")
                        )
                    )
                    # only if both the message and the DOM list agree - success
                    return "Success"
                except TimeoutException:
                    # message said success but list never updated - failure
                    return "Failure"
            elif "failure" in result_text:
                return "Failure"
            else:
                return "Unknown"

        except TimeoutException:
            return "Failure"

    def run_all_test_cases(self):
        try:
            driver = webdriver.Chrome()
            auth_actions.register_user(driver)
            auth_actions.login_user(driver)

            driver.get("http://localhost:3000/activity-categories")

            successful_test_results = 0
            test_results = ""
            actd = ActivityCategoryTestData()
            cases = actd.test_data

            for case in cases:
                result = self.run_test_case(driver, case)
                expected = "Success" if case["expected_success"] else "Failure"
                total_result = "Passed" if result == expected else "Failed"
                if total_result == "Passed":
                    successful_test_results += 1
                line = f"{total_result} (expected: {expected}, got: {result}) - " \
                       f"Name: {case['name']}, Color: {case['color']}\n"
                test_results += console_helpers.colorize_based_on_message_success(line, total_result == "Passed")

            print(test_results)
            final_summary = f"Activity category test results: {successful_test_results}/{len(cases)}\n"
            print(console_helpers.colorize_based_on_message_success(
                final_summary, successful_test_results == len(cases)))
        finally:
            driver.quit()
