# Calculator App

## Overview
Calculator App is a feature-rich Windows desktop calculator application built with WPF (.NET). It provides standard calculation functionality with additional advanced features like memory operations, digit grouping, and keyboard support.

![Calculator App Screenshot](/CalculatorAppDemo.png)
## Features

### Calculation Operations
- Basic arithmetic operations: addition, subtraction, multiplication, division
- Advanced operations: square, square root, inverse (1/x), percentage
- Sign toggling (+/-)
- Result calculation with equals (=)

### Memory Functions
- Memory Store (MS): Save current value to memory
- Memory Recall (MR): Retrieve the saved value
- Memory Add (M+): Add current value to memory
- Memory Subtract (M-): Subtract current value from memory
- Memory Clear (MC): Clear the memory
- Memory List (M>): View all stored memory values

### User Interface Features
- Dark theme interface
- Digit grouping option (toggle in File menu)
- Cut/Copy/Paste support
- Keyboard navigation and input support
- Error handling for operations like division by zero

## System Requirements
- Windows operating system
- .NET 8.0 Runtime

## Installation
1. Download the application from the releases page
2. Extract the ZIP file to your preferred location
3. Run `calculator-app.exe` to start the application

## Building from Source
1. Clone the repository
2. Open the solution in Visual Studio 2022 or later
3. Build the solution using the Build menu
4. Run the application using the Debug menu or by pressing F5

## Keyboard Shortcuts
- Numbers: 0-9 keys or numpad
- Decimal point: . (period) or numpad decimal
- Operations: +, -, *, / keys
- Equals: Enter key
- Clear: Escape key
- Backspace: Backspace key

## Settings
The application saves user preferences (like digit grouping) in a JSON settings file located in the application directory.

## Development
The application is built using:
- C# programming language
- WPF (Windows Presentation Foundation)
- .NET 8.0 framework

## Project Structure
- `MainWindow.xaml` & `MainWindow.xaml.cs`: Main application UI and event handlers
- `CalculatorLogic.cs`: Core calculation functionality
- `MemoryLogic.cs`: Memory-related operations
- `CalculatorSettings.cs`: User settings management
