using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public class ExpenseService
    {
        private readonly List<Expense> _expenses = new();

        // Add new expense
        public void AddExpense(Expense expense)
        {
            ArgumentNullException.ThrowIfNull(expense);

            if (expense.Amount <= 0)
            {
                throw new ArgumentException("Expense amount must be greater than 0.", nameof(expense));
            }
            if (string.IsNullOrWhiteSpace(expense.Category))
            {
                throw new ArgumentException("Expense category cannot be empty.", nameof(expense));
            }

            _expenses.Add(expense);
        }

        // Get expenses
        public List<Expense> GetExpenses()
        { 
            return _expenses;
        }

        // Remove an expense
        public void RemoveExpense(int index)
        {
            if (index < 0 || index >= _expenses.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Invalid expense index.");
            _expenses.RemoveAt(index);
        }

        // Save expenses to a JSON file
        public void SaveExpenses(string filePath)
        {
            try
            {
                // Ensure the directory exists
                var directory = Path.GetDirectoryName(filePath);
                if (directory != null && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonSerializer.Serialize(_expenses, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
                Console.WriteLine($"Expenses saved successfully. Total: {_expenses.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save expenses: {ex.Message}");
            }
        }

        // Load expenses from a JSON file
        public void LoadExpenses(string filePath)
        {
            try
            {
                Console.WriteLine($"Reading file: {filePath}");
                var json = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    Console.WriteLine("The file is empty. No expenses to load.");
                    return;
                }

                Console.WriteLine($"Loaded JSON:\n{json}");

                var expenses = JsonSerializer.Deserialize<List<Expense>>(json);
                if (expenses != null && expenses.Count > 0)
                {
                    _expenses.Clear();
                    _expenses.AddRange(expenses);
                    Console.WriteLine($"Expenses loaded successfully. Total: {_expenses.Count}");
                }
                else
                {
                    Console.WriteLine("No valid expenses found in the JSON.");
                }
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"Failed to parse expenses from file: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load expenses: {ex.Message}");
            }
        }
    }
}
