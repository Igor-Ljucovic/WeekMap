from abc import ABC, abstractmethod
from typing import Dict, Any
from selenium.webdriver.remote.webdriver import WebDriver


class BaseTest(ABC):

    @property
    @abstractmethod
    def run_all_test_cases(self) -> None:
        pass

    @property
    @abstractmethod
    def run_test_case(self, driver: WebDriver, case: Dict[str, Any], *args) -> str:
        if not isinstance(case, dict):
            raise TypeError("case must be a dictionary")

        if not isinstance(driver, WebDriver):
            raise TypeError("driver must be a Selenium WebDriver instance")
