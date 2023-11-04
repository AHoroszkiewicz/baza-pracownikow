using ConsoleControls;
using baza_pracownikow.Models;

Control mainScreen = new Control() { X = 0, Y = 0, Width = Console.WindowWidth, Height = Console.WindowHeight };

Control currentScreen = mainScreen;

var employeeRegister = new ListBox() { X = 1, Y = 1, Width = 20, Height = 10, Label = "Employees", Parent = mainScreen };
var addEmployeeButton = new Button() { X = 22, Y = 1, Width = 8, Label = "Add", Parent = mainScreen };
var editEmployeeButton = new Button() { X = 22, Y = 2, Width = 8, Label = "Edit", Parent = mainScreen };
var deleteEmployeeButton = new Button() { X = 22, Y = 3, Width = 8, Label = "Delete", Parent = mainScreen };
var searchEmployeeButton = new Button() { X = 22, Y = 4, Width = 8, Label = "Search", Parent = mainScreen };

addEmployeeButton.Click += (s, a) =>
{
	var employeeAddingScreen = new ListBox() { X = 0, Y = 0, Width = Console.WindowWidth, Height = Console.WindowHeight };
	var firstNameTextBox = new TextBox() { X = 1, Y = 1, Width = 20, Label = "First name", Parent = employeeAddingScreen };
	var lastNameTextBox = new TextBox() { X = 1, Y = 4, Width = 20, Label = "Last name", Parent = employeeAddingScreen };
	var peselTextBox = new TextBox() { X = 1, Y = 7, Width = 20, Label = "PESEL", Parent = employeeAddingScreen };
	var confirmButton = new Button() { X = 1, Y = 10, Width = 8, Label = "Confirm", Parent = employeeAddingScreen };
	var cancelButton = new Button() { X = 10, Y = 10, Width = 8, Label = "Cancel", Parent = employeeAddingScreen };

	confirmButton.Click += (s, a) =>
	{
		var employee = new Employee()
		{
			FirstName = firstNameTextBox.Text,
			LastName = lastNameTextBox.Text,
			PESEL = peselTextBox.Text,
		};
		employeeRegister.AddItem(employee);
		currentScreen.CleanUp();
		currentScreen = mainScreen;
		currentScreen.Redraw();

	};

	cancelButton.Click += (s, a) =>
	{
		currentScreen.CleanUp();
		currentScreen = mainScreen;
		currentScreen.Redraw();
	};

	currentScreen.CleanUp();
	currentScreen = employeeAddingScreen;
	currentScreen.Redraw();
};

deleteEmployeeButton.Click += (s, a) =>
{
    var employeeDeleteScreen = new ListBox() { X = 0, Y = 0, Width = Console.WindowWidth, Height = Console.WindowHeight };
	var informationLabel = new Button() { X = 1, Y = 1, Width = 80, Label = $"Please confirm you want to delete `{employeeRegister.SelectedItem}`" };
	employeeDeleteScreen.UnfocusableChildren.Add(informationLabel);
    var confirmButton = new Button() { X = 1, Y = 10, Width = 8, Label = "Confirm", Parent = employeeDeleteScreen };
    var cancelButton = new Button() { X = 10, Y = 10, Width = 8, Label = "Cancel", Parent = employeeDeleteScreen };

    confirmButton.Click += (s, a) =>
    {
        employeeRegister.RemoveItem(employeeRegister.SelectedItem);
        currentScreen.CleanUp();
        currentScreen = mainScreen;
        currentScreen.Redraw();

    };

    cancelButton.Click += (s, a) =>
    {
        currentScreen.CleanUp();
        currentScreen = mainScreen;
        currentScreen.Redraw();
    };
};

currentScreen.Redraw();

while (true)
{
    var key = Console.ReadKey(true);
	if (key.Key == ConsoleKey.Escape)
		break;
	currentScreen.HandleKey(key);
}