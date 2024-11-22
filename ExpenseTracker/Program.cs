using ExpenseTracker.Models;
using ExpenseTracker.Services;

// File path for saving/loading expenses
string filePath = Path.Combine(AppContext.BaseDirectory, "../../../expenses.json");
Console.WriteLine($"Using file path: {Path.GetFullPath(filePath)}");

// Create an instance of ExpenseService
var expenseService = new ExpenseService();

// Load expenses from the file
expenseService.LoadExpenses(filePath);

Console.WriteLine("Welcome to the Expense Tracker App!");

// Running the loop
bool isRunning = true;
while (isRunning)
{
    ShowMenu();
    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            AddExpense(expenseService); 
            break;
        case "2":
            DisplayExpenses(expenseService); 
            break;
        case "3":
            EditExpense(expenseService);
            break;
        case "4":
            DeleteExpense(expenseService);
            break;
        case "5":
            Console.WriteLine("Saving expenses and exiting...");
            expenseService.SaveExpenses(filePath);
            isRunning = false;
            break;
        default:
            Console.WriteLine("Invalid choice. Please select an option from the menu.");
            break;
    }
}

// Show Menu of options
void ShowMenu()
{
    Console.WriteLine("\n=====================");
    Console.WriteLine("Expense Tracker Menu");
    Console.WriteLine("=====================");
    Console.WriteLine("1. Add Expense");
    Console.WriteLine("2. View All Expenses");
    Console.WriteLine("3. Edit Expense");
    Console.WriteLine("4. Delete Expense");
    Console.WriteLine("5. Exit");
    Console.Write("=====================\nEnter your choice: ");
}

// Add an expense to list of expenses
void AddExpense(ExpenseService expenseService)
{
    try
    {
        Console.Write("Enter amount: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
        {
            Console.WriteLine("Invalid amount. Please enter a positive number");
            return;
        }

        Console.Write("Enter category: ");
        string? category = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(category))
        {
            Console.WriteLine("Category cannot be empty. Returning to the menu.");
            return;
        }

        Console.Write("Enter notes (optional): ");
        string? notes = Console.ReadLine();

        var newExpense = new Expense
        {
            Amount = amount,
            Category = category,
            Date = DateTime.Now,
            Notes = notes ?? string.Empty
        };

        expenseService.AddExpense(newExpense);
        Console.WriteLine("Expense added successfully!");
    }
    catch (Exception ex) 
    { 
        Console.WriteLine($"An error occured: {ex.Message}");
    }
}

// Display all expenses
void DisplayExpenses(ExpenseService expenseService)
{
    var expenses = expenseService.GetExpenses();
    if (expenses.Count == 0)
    {
        Console.WriteLine("No expenses recorded yet.");
        return;
    }

    Console.WriteLine("\nYour expenses:");
    foreach (var expense in expenses)
    {
        Console.WriteLine(expense);
    }
}

// helper function to select an Expense
//  used for EditExpense and Delete Expense
int SelectExpense(ExpenseService expenseService)
{
    var expenses = expenseService.GetExpenses();
    // Check if expenses is empty
    if (expenses.Count == 0)
    {
        Console.WriteLine("No expenses available");
        return -1;
    }

    // Displaying expenses
    Console.WriteLine("\nSelect an expense");
    for (int i = 0; i < expenses.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {expenses[i]}");
    }

    // Select an expense
    Console.Write("Enter the number of the expense: ");
    if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= expenses.Count) 
    {
        return selection - 1;
    }

    // Checking for invalid selection
    Console.WriteLine("Invalid selection. Please enter a valid number from the list.");
    return -1;
}

void EditExpense(ExpenseService expenseService)
{
    int index = SelectExpense(expenseService);
    if (index == -1)
    {
        Console.WriteLine("No expense selected. Returning to the menu.");
        return;
    }

    var expenses = expenseService.GetExpenses();
    var expense = expenses[index];

    Console.WriteLine($"\nEditing expense: {expense}");
    Console.WriteLine("Select a field to edit:");
    Console.WriteLine("1. Amount");
    Console.WriteLine("2. Category");
    Console.WriteLine("3. Notes");
    Console.Write("Enter your choice: ");
    string? choice = Console.ReadLine();

    switch(choice)
    {
        case "1":
            Console.Write("Enter new amount: ");
            if (decimal.TryParse(Console.ReadLine(),out decimal newAmount) && newAmount > 0)
            {
                Console.Write($"Are you sure you want to update the amount to {newAmount}? (yes/no): ");
                if (Console.ReadLine()?.ToLower() == "yes")
                {
                    expense.Amount = newAmount;
                    Console.WriteLine("Amount updated successfully!");
                }
                else
                {
                    Console.WriteLine("Amount update canceled.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Amount");
            }
            break;
        case "2":
            Console.Write("Enter new category: ");
            string? newCategory = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newCategory))
            {
                Console.Write($"Are you sure you want to update the category to '{newCategory}'? (yes/no): ");
                if (Console.ReadLine()?.ToLower() == "yes")
                {
                    expense.Category = newCategory;
                    Console.WriteLine("Category updated successfully!");
                }
                else
                {
                    Console.WriteLine("Category update canceled.");
                }
            }
            else
            {
                Console.WriteLine("Category cannot be empty. Returning to the menu.");
            }
            break;
        case "3":
            Console.Write("Enter new notes: ");
            string? newNotes = Console.ReadLine();
            Console.Write("Are you sure you want to update the notes? (yes/no): ");
            if (Console.ReadLine()?.ToLower() == "yes")
            {
                expense.Notes = newNotes ?? string.Empty;
                Console.WriteLine("Notes updated successfully!");
            }
            else
            {
                Console.WriteLine("Notes update canceled.");
            }
            break;
        default:
            Console.WriteLine("Invalid Choice");
            break;
    }
}

void DeleteExpense(ExpenseService expenseService)
{
    int index = SelectExpense(expenseService);
    if (index == -1)
    {
        Console.WriteLine("No expense selected. Returning to the menu.");
        return;
    }

    var expenses = expenseService.GetExpenses();
    var expense = expenses[index];

    Console.WriteLine($"Are you sure you want to delete this expense? {expense} (yes/no)");
    string? confirmation = Console.ReadLine();

    if (confirmation?.ToLower() == "yes")
    {
        expenseService.RemoveExpense(index);
        Console.WriteLine("Expense deleted successfully!");
    }
    else
    {
        Console.WriteLine("Deletion canceled.");
    }
}