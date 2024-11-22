using System;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        public decimal Amount { get; set; } // Expense amount
        public string Category { get; set; } = string.Empty; // e.g., Food, Transport
        public DateTime Date {  get; set; }
        public string Notes { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Date.ToShortDateString()} - {Category}: ${Amount:F2} ({Notes})";
        }
    }
}