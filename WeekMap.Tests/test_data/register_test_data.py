from utils.base_test_data import BaseTestData
from typing import List, Dict, Any


class RegisterTestData(BaseTestData):
    @property
    def test_data(self) -> List[Dict[str, Any]]:
        return [
            {
                "username": "user©",
                "email": "emoji@test.com",
                "password": "Password123",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "username": "user511",
                "email": "tm@tmmail.com",
                "password": "Password123",
                "expected_success": True,
                "expected_result": {"status": "ok", "code": 200}
            },
            {
                "username": "user255",
                "email": "email@test.com",
                "password": "P@ssw©rd123",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "username": "polzovatel",
                "email": "user@test.com",
                "password": "Password123",
                "expected_success": True,
                "expected_result": {"status": "ok", "code": 200}
            },
            {
                "username": "user735",
                "email": "yonghu@ceshi.com",
                "password": "Password123",
                "expected_success": True,
                "expected_result": {"status": "ok", "code": 200}
            },
            {
                "username": "user764",
                "email": "user764@test.com",
                "password": "Parol123",
                "expected_success": True,
                "expected_result": {"status": "ok", "code": 200}
            },
            {
                "username": "",
                "email": "user@test.com",
                "password": "Password123",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "username": "user243",
                "email": "",
                "password": "Password123",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "username": "user265",
                "email": "user@test.com",
                "password": "",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "username": "userrrr878",
                "email": "user@test.com",
                "password": "Pass12A",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "username": "user89",
                "email": "u@t.com",
                "password": "P1!",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "username": "u" * 26,
                "email": "user@test.com",
                "password": "Password123",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "username": "user176",
                "email": "user@test.com",
                "password": "A1" + "!" * 63,
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "username": "user932",
                "email": "a" * 100 + "@test.com",
                "password": "Password123",
                "expected_success": True,
                "expected_result": {"status": "ok", "code": 200}
            },
            {
                "username": "user753",
                "email": "user53@test.com",
                "password": "password",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "username": "user754",
                "email": "user54@test.com",
                "password": "PASSWORD",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "username": "user755",
                "email": "user55@test.com",
                "password": "password!",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "username": "usr",
                "email": "x@x.com",
                "password": "Abcdefg1",
                "expected_success": True,
                "expected_result": {"status": "ok", "code": 200}
            },
            {
                "username": "user_name",
                "email": "emaillllll@domain.com",
                "password": "GoodPass123!",
                "expected_success": True,
                "expected_result": {"status": "ok", "code": 200}
            },
            {
                "username": "user569",
                "email": "email569@domain.com",
                "password": "pass§word1A",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
            {
                "username": "user876",
                "email": "email876@domain.com",
                "password": "µPassword123",
                "expected_success": False,
                "expected_result": {"status": "bad request", "code": 400}
            },
        ]
