# Overview

I created Employee Manager to practice C# syntax and learn how to organize a console program with classes and methods. It lets a user add employees, edit their information, and display their hourly pay and estimated pay for a 40-hour week.

My goal was to connect variables, expressions, conditionals, loops, functions, and classes in a small working program. The program also reads and writes employee records as JSON so they can be used again after restarting.

[Software Demo Video](https://youtu.be/BcK7MVcfosk)

# Development Environment

The project uses the .NET 10 SDK and runs from the macOS terminal. It can be opened in Visual Studio Code or another C# editor.

The program uses C# and the built-in .NET libraries. System.Text.Json converts employee records to and from JSON. No external packages are required.

From the project folder, run:

```sh
dotnet run
```

Choose an option from the menu. Editing requires an employee ID shown by the display option. Choose **Save and quit** to save your changes. The program loads saved records automatically on startup. The data file is `employees.json` beside the compiled program in `bin/Debug/net10.0/` when using the command above. It is excluded from GitHub. Use fictional employee details for demonstrations.

# Useful Websites

- [C# Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [System.Text.Json Documentation](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview)
- [.NET CLI Documentation](https://learn.microsoft.com/en-us/dotnet/core/tools/)

# Future Work

- Add an option to delete an employee.
- Allow searches by name or department.
- Allow users to enter weekly hours instead of assuming 40 hours.
