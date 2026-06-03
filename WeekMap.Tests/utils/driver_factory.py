import os
from selenium import webdriver
from selenium.webdriver.chrome.options import Options


def create_driver():
    options = Options()
    if os.environ.get("CI"):
        options.add_argument("--headless")
        options.add_argument("--no-sandbox")
        options.add_argument("--disable-dev-shm-usage")
    return webdriver.Chrome(options=options)
