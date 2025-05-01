import random
import string
import re
import time

def make_case(username, email, password, is_valid):
    return {"username": username, "email": email, "password": password, "is_valid": is_valid}

# Doesn't matter if is_valid is False or True, it will get overriden anyway
edge_cases = [
    # Replaced emojis with symbols in BMP
    make_case("user©", "emoji@test.com", "Password123", False),
    make_case("user511", "emoji@tmmail.com", "Password123", False),
    make_case("user255", "email@test.com", "P@ssw©rd123", False),

    # Replaced non-English characters
    make_case("polzovatel", "user@test.com", "Password123", False),
    make_case("user735", "yonghu@ceshi.com", "Password123", False),
    make_case("user764", "user764@test.com", "Parol123", False),

    # Empty fields
    make_case("", "user@test.com", "Password123", False),
    make_case("user243", "", "Password123", False),
    make_case("user265", "user@test.com", "", False),

    # Too short
    make_case("us", "user@test.com", "Pass12A", False),
    make_case("user89", "u@t.com", "P1!", False),

    # Too long
    make_case("u" * 26, "user@test.com", "Password123", False),
    make_case("user176", "user@test.com", "A1" + "!" * 63, False),
    make_case("user932", "a" * 100 + "@test.com", "Password123", False),

    # No uppercase or digit in password
    make_case("user123", "user@test.com", "password", False),
    make_case("user123", "user@test.com", "PASSWORD", False),
    make_case("user123", "user@test.com", "password!", False),

    # Valid edge case (borderline valid)
    make_case("usr", "x@x.com", "Abcdefg1", False),

    # All valid with underscore and symbols
    make_case("user_name", "emaillllll@domain.com", "GoodPass123!", False),

    # Unicode punctuation or non-english characters (selenium doesn't support non-BPM characters like emojis)
    make_case("user569", "email569@domain.com", "pass§word1A", False),
    make_case("user876", "email876@domain.com", "µPassword123", False),
  # make_case("user876\U0001F525", "email900@domain.com", "Password123456", False)
]

def random_string(length, include_underscore=False, include_punctuation=False):
    chars = string.ascii_letters + string.digits

    if include_punctuation:
        chars += string.punctuation
    elif include_underscore:
        chars += "_"

    return "".join(random.choices(chars, k=length))

def is_registration_valid(username, password, email):
    password_has_upper = any(c.isupper() for c in password)
    password_has_digit = any(c.isdigit() for c in password)
    is_username_valid = len(username) < 3 and username.isalnum()
    is_password_valid = len(password) < 8 and password_has_upper and password_has_digit and password.isalnum()
    is_email_valid = ('@' in email and len(email.split('@')) == 2 and any(c.isalpha() for c in email.split('@')[1]))

    return is_username_valid and is_password_valid and is_email_valid

def generate_test_cases(random_user_cases, include_edge_cases):
    cases = []
    for _ in range(random_user_cases):
        # generate random alphanumeric string that is 0-15 characters long
        username = random_string(random.randint(0, 15))
        email = f"{random_string(random.randint(0, 15))}@test.com"
        password = random_string(random.randint(0, 15))
        cases.append(make_case(username, email, password, False))

    if (include_edge_cases):
        cases.extend(edge_cases)

    for i, case in enumerate(cases):
        username = case["username"]
        email = case["email"]
        password = case["password"]
        is_valid = case["is_valid"]

        password_has_upper = any(c.isupper() for c in password)
        password_has_digit = any(c.isdigit() for c in password)
        # username can only contain alphabet letters, numbers and _
        is_username_valid =  len(username) >= 3 and len(username) <= 25 \
                             and re.match(r'^[a-zA-Z0-9_]+$', username)
        # password can only contain alphabet letters, numbers and symbols (no emojis)
        is_password_valid = len(password) >= 8 and len(password) <= 64 and password_has_upper and password_has_digit \
                            and re.match(r'^[\x21-\x7E]+$', password)
        is_email_valid = '@' in email and len(email.split('@')) == 2 and \
                         any(c.isalpha() for c in email.split('@')[0]) and any(c.isalpha() for c in email.split('@')[1])

        is_valid = is_username_valid and is_password_valid and is_email_valid
        cases[i] = make_case(username, email, password, is_valid)

    # The following cases need to be added manually because Python doesn't have access to the SSMS database directly,
    # so it can't check for the unique username and unique email constraints
    cases.append(make_case("gfsdwerwe54", "asdaasfqd@gmail.com", "TAsras53467", True))
    cases.append(make_case("gfsdwerwe54", "ittgh@gmail.com", "TAsras53467", False))
    cases.append(make_case("afaasgasga", "jhgjhgjhgjhg@gmail.com", "TAsras53467", True))
    cases.append(make_case("yeryery0429", "jhgjhgjhgjhg@gmail.com", "TAsras53467", False))

    return cases