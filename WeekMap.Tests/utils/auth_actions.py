from selenium.common import TimeoutException
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
from selenium.webdriver.support import expected_conditions as ec

_username = "someUser1"
_email = "someUser1@gmail.com"
_password = "StrongPassword123"


def register_user(driver, username=_username, email=_email, password=_password):
    driver.get("http://localhost:3000/register")
    WebDriverWait(driver, 30).until(ec.element_to_be_clickable((By.ID, "username"))).send_keys(username)
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


def login_user(driver, email=_email, password=_password):
    driver.get("http://localhost:3000/login")

    email_field = WebDriverWait(driver, 30).until(ec.element_to_be_clickable((By.ID, "email")))
    email_field.send_keys(email)

    password_field = WebDriverWait(driver, 30).until(ec.element_to_be_clickable((By.ID, "password")))
    password_field.send_keys(password)

    login_button = WebDriverWait(driver, 30).until(ec.element_to_be_clickable((By.XPATH, '//button[text()="Log in"]')))
    login_button.click()

    try:
        # wait 7 seconds before the test fails
        WebDriverWait(driver, 7).until(lambda d: d.find_element(By.ID, "login-message").text.strip() != "")
        message = driver.find_element(By.ID, "login-message").text
        return message
    except TimeoutException:
        return "Timed out waiting for the login message"
