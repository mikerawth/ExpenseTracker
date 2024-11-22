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
                var json = JsonSerializer.Serialize(_expenses, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
                Console.WriteLine("Expenses saved successfully.");
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
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var expenses = JsonSerializer.Deserialize<List<Expense>>(json);
                    if (expenses != null)
                    {
                        _expenses.Clear();
                        _expenses.AddRange(expenses);
                        Console.WriteLine("Expenses loaded successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load expenses: {ex.Message}");
            }
        }
    }
}
