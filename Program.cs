using System.Globalization;
using System.Text.Json;

class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Department { get; set; } = "";
    public decimal HourlyPay { get; set; }

    public decimal WeeklyPay()
    {
        return HourlyPay * 40;
    }
}

class Program
{
    static readonly string DataFile = Path.Combine(AppContext.BaseDirectory, "employees.json");
    static List<Employee> employees = new();

    static void Main()
    {
        LoadEmployees();
        bool running = true;

        // Keep showing the menu until the user chooses to quit.
        while (running)
        {
            Console.WriteLine("\nEmployee Manager");
            Console.WriteLine("1. Add employee");
            Console.WriteLine("2. Edit employee");
            Console.WriteLine("3. Display employees");
            Console.WriteLine("4. Save and quit");
            Console.Write("Choose an option: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddEmployee();
                    break;
                case "2":
                    EditEmployee();
                    break;
                case "3":
                    DisplayEmployees();
                    break;
                case "4":
                    running = !SaveEmployees();
                    break;
                case null:
                    SaveEmployees();
                    running = false;
                    break;
                default:
                    Console.WriteLine("Please choose 1, 2, 3, or 4.");
                    break;
            }
        }
    }

    static void AddEmployee()
    {
        // Use the highest existing ID so each employee gets a different number.
        int nextId = employees.Count == 0 ? 1 : employees.Max(e => e.Id) + 1;
        Employee employee = new Employee
        {
            Id = nextId,
            Name = ReadText("Name: "),
            Department = ReadText("Department: "),
            HourlyPay = ReadPay()
        };
        employees.Add(employee);
        Console.WriteLine($"Added employee #{employee.Id}.");
    }

    static void EditEmployee()
    {
        Console.Write("Employee ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Enter a whole number for the ID.");
            return;
        }

        Employee? employee = employees.Find(e => e.Id == id);
        if (employee == null)
        {
            Console.WriteLine("Employee not found.");
            return;
        }

        Console.WriteLine($"Editing {employee.Name}. Enter the updated information.");
        employee.Name = ReadText("Name: ");
        employee.Department = ReadText("Department: ");
        employee.HourlyPay = ReadPay();
        Console.WriteLine("Employee updated.");
    }

    static void DisplayEmployees()
    {
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees yet.");
            return;
        }

        foreach (Employee employee in employees)
        {
            Console.WriteLine($"#{employee.Id}: {employee.Name} | {employee.Department}");
            Console.WriteLine($"Hourly pay: ${employee.HourlyPay:F2} | 40-hour weekly pay: ${employee.WeeklyPay():F2}");
        }
    }

    static string ReadText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = Console.ReadLine()?.Trim()
                ?? throw new EndOfStreamException("Input ended before the employee was complete.");
            if (value.Length > 0)
                return value;
            Console.WriteLine("This field cannot be blank.");
        }
    }

    static decimal ReadPay()
    {
        while (true)
        {
            Console.Write("Hourly pay (for example, 18.50): ");
            string input = Console.ReadLine()
                ?? throw new EndOfStreamException("Input ended before the employee was complete.");
            if (decimal.TryParse(input, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out decimal pay) && pay >= 0 && pay <= 1000000)
                return pay;
            Console.WriteLine("Enter a number from 0 to 1000000 using a decimal point.");
        }
    }

    static void LoadEmployees()
    {
        if (!File.Exists(DataFile))
            return;
        try
        {
            // JSON lets the program save the list and read it back next time.
            employees = JsonSerializer.Deserialize<List<Employee>>(File.ReadAllText(DataFile))
                ?? new List<Employee>();
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException or JsonException)
        {
            Console.WriteLine($"Could not load employees: {error.Message}");
            Console.WriteLine("The file will be kept. Fix or remove it before running again.");
            Environment.Exit(1);
        }
    }

    static bool SaveEmployees()
    {
        try
        {
            string json = JsonSerializer.Serialize(employees, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(DataFile, json);
            Console.WriteLine("Employees saved.");
            return true;
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException)
        {
            Console.WriteLine($"Could not save employees: {error.Message}");
            return false;
        }
    }
}
