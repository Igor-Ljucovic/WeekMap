import subprocess
import socket
import time
from utils import config


def is_port_open(port):
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as sock:
        sock.settimeout(1)
        return sock.connect_ex(('127.0.0.1', port)) == 0


def wait_for_service(port, name):
    print(f"Waiting for {name} to respond...")
    while not is_port_open(port):
        time.sleep(1)
    print(f"{name} is up!\n")


def start_backend():
    if is_port_open(config.BACKEND_PORT):
        print("Backend already running and responsive.\n")
    else:
        print(f"Starting backend on port {config.BACKEND_PORT}...")
        subprocess.Popen(
            ['cmd', '/c', 'dotnet run --launch-profile Test'],
            cwd=config.BACKEND_PATH,
            creationflags=subprocess.CREATE_NEW_CONSOLE
        )
        wait_for_service(config.BACKEND_PORT, "Backend")


def start_frontend():
    if is_port_open(config.FRONTEND_PORT):
        print("Frontend already running and responsive.\n")
    else:
        print(f"Starting frontend on port {config.FRONTEND_PORT}...")
        subprocess.Popen(
            ['cmd', '/c', 'npm start'],
            cwd=config.FRONTEND_PATH,
            creationflags=subprocess.CREATE_NEW_CONSOLE
        )
        wait_for_service(config.FRONTEND_PORT, "Frontend")


def setup_project():
    print("=" * config.PRINT_SEPERATOR_LENGTH)
    print(f"CHECKING BACKEND (https://localhost:{config.BACKEND_PORT})")
    print("=" * config.PRINT_SEPERATOR_LENGTH)
    start_backend()

    print("=" * config.PRINT_SEPERATOR_LENGTH)
    print(f"CHECKING FRONTEND (http://localhost:{config.FRONTEND_PORT})")
    print("=" * config.PRINT_SEPERATOR_LENGTH)
    start_frontend()
