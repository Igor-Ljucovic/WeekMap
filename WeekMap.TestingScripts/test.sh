#!/bin/bash

read -p "Enter mode (start/test): " MODE

# Escape Windows path and run the PowerShell script
powershell.exe -ExecutionPolicy Bypass -File "test.ps1" "$MODE"

# Keep the terminal open (optional)
read -p "Press Enter to exit..."
