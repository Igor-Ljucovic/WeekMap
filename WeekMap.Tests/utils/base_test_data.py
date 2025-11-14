from abc import ABC, abstractmethod
from typing import List, Dict, Any


class BaseTestData(ABC):
    _REQUIRED_KEYS = ["expected_success", "expected_result"]
    _EXPECTED_RESULT_KEYS = ["status", "code"]

    def __init__(self):
        self.validate_data()

    @property
    @abstractmethod
    def test_data(self) -> List[Dict[str, Any]]:
        pass

    def validate_data(self):
        data = self.test_data

        if not isinstance(data, list):
            raise TypeError("test_data must be a list.")

        for i, item in enumerate(data):
            if not isinstance(item, dict):
                raise TypeError(f"Item at index {i} in test_data must be a dictionary.")

            for key in self._REQUIRED_KEYS:
                if key not in item:
                    raise ValueError(f"Dictionary at index {i} in test_data is missing the required key: '{key}'.")

            if not isinstance(item["expected_success"], bool):
                raise TypeError(f"Value for 'expected_success' at index {i} must be a boolean.")

            expected_result = item["expected_result"]
            if not isinstance(expected_result, dict):
                raise TypeError(f"'expected_result' at index {i} must be a dictionary.")

            for key in self._EXPECTED_RESULT_KEYS:
                if key not in expected_result:
                    raise ValueError(f"'expected_result' at index {i} is missing required key: '{key}'.")

            if not isinstance(expected_result["status"], str):
                raise TypeError(f"'status' in expected_result at index {i} must be a string.")

            if not isinstance(expected_result["code"], int):
                raise TypeError(f"'code' in expected_result at index {i} must be an integer.")
