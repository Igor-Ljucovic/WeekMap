from colorama import init, Fore

init(autoreset=True)


def colorize_based_on_message_success(message, success):
    return (Fore.GREEN if success else Fore.RED) + message


def dict_to_str(d):
    return ", ".join(f"{key}: {value}" for key, value in d.items())
