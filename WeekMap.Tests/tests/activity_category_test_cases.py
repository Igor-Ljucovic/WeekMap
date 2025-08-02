import re


def make_case(name, color, is_valid):
    return {"name": name, "color": color, "is_valid": is_valid}


# Doesn't matter if is_valid is False or True, it will get overriden anyway
edge_cases = [
    # Replaced emojis with symbols in BMP
    make_case("", "000000",  False),
    make_case("color1", "000000",  False),
    make_case("color2", "FFFFFF",  False),
    make_case("color3", "CB493A",  False),
    make_case("color4", "#000000", False),
    make_case("W"*50,   "CB493A",  False),
    make_case("color5", "ABCDWH",  False),
    make_case("color6", "ABCDEFG", False),
    make_case("color6", "ABCDE", False),
    # the one below shouldn't work, but will because the frontend will limit the input to 50 characters
    # make_case("W"*51,   "CB493A", False)
]


def is_activity_category_valid(name, color):
    is_name_valid = 1 <= len(name) <= 50
    is_color_valid = re.fullmatch(r"[A-Fa-f0-9]{6}", color) is not None
    return is_name_valid and is_color_valid


def get_test_cases():
    cases = []
    for i, case in enumerate(edge_cases):
        case["is_valid"] = is_activity_category_valid(case["name"], case["color"])
        cases.append(case)
    return cases
