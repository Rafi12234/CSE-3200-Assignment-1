# Design Report: Personal Finance Tracker

---

## Project Title
**Personal Finance Tracker** — A Windows Forms Desktop Application

---

## Objective
The objective of this project is to build a simple, beginner-friendly personal finance management tool as a Windows Forms desktop application. It allows a user to record and manage their daily income and expense transactions, view a summary of their financial situation, and maintain a clear transaction history — all without requiring any internet connection or database.

---

## Tools and Technology Used

| Component       | Details                            |
|-----------------|------------------------------------|
| Language        | C# (.NET 8)                        |
| UI Framework    | Windows Forms (WinForms)           |
| IDE             | Visual Studio 2022                 |
| Data Storage    | In-memory `List<Transaction>`      |
| External Packages | None (no NuGet packages used)   |
| Database        | None                               |

---

## Main Features

1. **Add Transaction** — The user can enter an amount, select a category (Salary, Freelance, Food, Transport, Utilities, Other), choose the type (Income or Expense), pick a date, and optionally add notes.
2. **Transaction Table** — All transactions are displayed in a clean, readable DataGridView with columns: Date, Type, Category, Amount, and Notes.
3. **Summary Dashboard** — Three summary cards at the top always show the live Total Income, Total Expenses, and Net Balance.
4. **Delete Transaction** — Any selected row can be removed from the list and the summary updates immediately.
5. **Input Validation** — The application validates the amount field and shows a clear error message if the input is empty or invalid.

---

## UI Design Explanation

The form is a fixed 900×600 pixel window that cannot be resized, keeping the layout consistent.

- **Header Bar**: A deep navy-blue bar spans the top with the application title.
- **Summary Cards** (below header): Three colored panels display financial totals. Income card is green, Expenses card is red, and Balance card is blue. The Net Balance text turns light-red when negative.
- **Left Panel** ("Add Transaction"): A white card panel on the left side (270px wide) holds all input controls stacked vertically: Amount TextBox, Type Radio Buttons, Category ComboBox, DateTimePicker, Notes TextBox, and a blue "Add Transaction" button.
- **Right Panel** ("Transaction History"): A white card panel on the right (584px wide) contains the DataGridView and a red "Delete Selected" button at the bottom.
- **Color Coding**: Income rows display the word "Income" in green; Expense rows display it in red. This gives instant visual feedback.
- **Font**: Segoe UI is used throughout for a modern, clean appearance.

---

## Data Storage Explanation

No database, file, or external storage is used. All transaction data is stored in a single C# generic list:

```csharp
private List<Transaction> transactions = new List<Transaction>();
```

Each item in this list is a `Transaction` object with five properties: `Date`, `Type`, `Category`, `Amount`, and `Notes`. This list lives in memory for the duration of the application session. When the application is closed, all data is lost — this is an intentional design choice to keep the project simple.

---

## Validation Explanation

When the user clicks **Add Transaction**, the following checks are performed in order:

1. **Empty Check** — If the Amount field is blank, a warning MessageBox appears.
2. **Parse Check** — If the text cannot be converted to a decimal number, an error MessageBox appears.
3. **Positive Check** — If the amount is zero or negative, an error MessageBox appears.

Only if all three checks pass is the transaction created and added to the list.

---

## Limitations

- **No Persistence**: Data is lost when the application is closed. There is no save/load functionality.
- **Single User**: The application is designed for one user on one machine.
- **No Editing**: Existing transactions cannot be edited; they must be deleted and re-added.
- **No Export**: There is no option to export data to Excel or PDF.
- **No Charts**: There are no graphs or visual charts for spending analysis.

---

## Conclusion

The Personal Finance Tracker successfully meets all the assignment requirements. It provides a functional, clean, and beginner-understandable Windows Forms application that demonstrates core C# concepts including: class design, list manipulation, LINQ queries, event-driven programming, and Windows Forms UI construction. The project uses no external dependencies, no database, and follows a clear, maintainable code structure that is easy for any student to read and understand.

---

*Submitted as part of CSE-3200 Assignment — University Submission*
