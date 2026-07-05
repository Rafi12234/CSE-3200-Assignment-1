// Transaction.cs
// This file defines the Transaction model/class.
// Each transaction object holds one income or expense record.

using System;

namespace PersonalFinanceTracker
{
    /// <summary>
    /// Represents a single financial transaction (income or expense).
    /// </summary>
    public class Transaction
    {
        /// <summary>The date the transaction was recorded.</summary>
        public DateTime Date { get; set; }

        /// <summary>"Income" or "Expense".</summary>
        public string Type { get; set; }

        /// <summary>Category such as Salary, Food, Transport, etc.</summary>
        public string Category { get; set; }

        /// <summary>The monetary amount of this transaction.</summary>
        public decimal Amount { get; set; }

        /// <summary>Optional notes or description for this transaction.</summary>
        public string Notes { get; set; }

        // Constructor: initialize string properties to empty strings
        public Transaction()
        {
            Type     = string.Empty;
            Category = string.Empty;
            Notes    = string.Empty;
        }
    }
}
