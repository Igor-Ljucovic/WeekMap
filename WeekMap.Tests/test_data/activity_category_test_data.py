from utils.base_test_data import BaseTestData
from typing import List, Dict, Any


class ActivityCategoryTestData(BaseTestData):
    @property
    def test_data(self) -> List[Dict[str, Any]]:
        return [
            {
                "name": "",
                "color": "CB49A3",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "name": "someCategoryName",
                "color": "",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "name": "wrongColorCharacters",
                "color": "badclr",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "name": "name with spaces",
                "color": "123123",
                "expected_success": True,
                "expected_result": {"status": "ok", "code": 200}
            },
            {
                "name": "7_character_color",
                "color": "1231231",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "name": "hashtagColors",
                "color": "#A1236D",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "name": "categoryyy",
                "color": "000000",
                "expected_success": True,
                "expected_result": {"status": "ok", "code": 200}
            },
            {
                "name": "catafasafa",
                "color": "B999BB",
                "expected_success": True,
                "expected_result": {"status": "ok", "code": 200}
            },
            {
                "name": "user© EMOJI",
                "color": "9BBB99",
                "expected_success": True,
                "expected_result": {"status": "ok", "code": 200}
            },
            {
                "name": "W"*49,
                "color": "B999BB",
                "expected_success": True,
                "expected_result": {"status": "ok", "code": 200}
            },
            {
                "name": "W"*50,
                "color": "B999BB",
                "expected_success": True,
                "expected_result": {"status": "ok", "code": 200}
            },
            # the one below should be false, and it is, but for the wrong reasons, the html element
            # only allows for max 50 characters, so 51 characters will become 50, and so it will fail because
            # the added element isn't the same, but the category will actually still be added, it will just
            # contain the first 50 characters instead of the original 51 characters
            # {
            #     "name": "W"*51,
            #     "color": "B999BB",
            #     "expected_success": False,
            #     "expected_result": {"status": "bad request", "code": 400}
            # },
            {
                "name": "weird characters &#%*^%#%_-`'",
                "color": "ABC123",
                "expected_success": True,
                "expected_result": {"status": "ok", "code": 200}
            }
        ]
