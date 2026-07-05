# Demo Video Script: Personal Finance Tracker

---

## Video Details
- **Recommended Length**: 3–5 minutes
- **Tone**: Calm, clear, professional
- **Audience**: University professor / evaluator

---

## Script

---

### [OPENING — 0:00 to 0:20]

*(Show your face or just screen record. Start with the application already open.)*

> "Hello everyone. My name is [Your Name], student ID [Your ID].
> In this video, I will demonstrate my university assignment project —
> the **Personal Finance Tracker**, a Windows Forms desktop application
> built using C# and .NET 8."

---

### [INTRODUCE THE UI — 0:20 to 0:45]

*(Pan the camera or mouse over the full application window.)*

> "As you can see, the application has a clean, modern layout.
> At the very top, there is a navy-blue header bar with the application title.
>
> Below that, we have **three summary cards**:
> - The green card shows **Total Income**
> - The red card shows **Total Expenses**
> - The blue card shows the **Net Balance**
>
> Right now, all values are zero because no transactions have been added yet."

---

### [ADD AN INCOME TRANSACTION — 0:45 to 1:30]

*(Click on the Amount field and type a value.)*

> "Let me add my first transaction. I will add a salary income.
>
> I will type **25,000** in the Amount field.
>
> *(Select Income radio button — already selected by default)*
> The Type is set to **Income** by default.
>
> *(Select Salary from Category drop-down)*
> I will select **Salary** from the Category drop-down.
>
> The date is already set to today's date.
>
> I will add a note: **'July salary'**.
>
> Now I will click **Add Transaction**."

*(Click the button. The transaction appears in the grid and the summary cards update.)*

> "As you can see, the transaction has been added to the table.
> The **Total Income** card now shows **৳ 25,000.00**.
> The **Net Balance** is also **৳ 25,000.00** — shown in a lighter color."

---

### [ADD AN EXPENSE TRANSACTION — 1:30 to 2:15]

> "Now let me add an expense.
>
> I will type **3,500** in the Amount field.
>
> *(Click Expense radio button)*
> I will select **Expense** as the type.
>
> *(Select Food from drop-down)*
> Category: **Food**.
>
> I will add a note: **'Monthly groceries'**.
>
> Click **Add Transaction**."

*(Transaction appears in the grid.)*

> "The expense has been added. Notice:
> - **Total Expenses** now shows **৳ 3,500.00**
> - **Net Balance** has automatically updated to **৳ 21,500.00**
>
> The **Type** column in the table shows 'Income' in green and 'Expense' in red
> for easy visual identification."

---

### [SHOW VALIDATION — 2:15 to 2:40]

> "The application also validates user input.
>
> Let me clear the amount field and click Add Transaction with nothing entered."

*(Click Add Transaction with an empty amount field.)*

> "A warning message appears: *'Please enter an amount.'*
>
> Now let me try entering an invalid value — I will type **'abc'**."

*(Type 'abc' and click Add.)*

> "Again a message box appears: *'Please enter a valid positive amount.'*
>
> Let me also try entering a **zero or negative value** — I will type **-100**."

*(Type -100 and click Add.)*

> "The validation catches this as well.
> Only valid, positive decimal numbers are accepted."

---

### [DELETE A TRANSACTION — 2:40 to 3:10]

> "Now let me demonstrate the delete feature.
>
> I will click on the expense row in the table to select it.
>
> *(Click the row.)*
> The full row is highlighted in blue.
>
> Now I will click the red **Delete Selected** button at the bottom."

*(A confirmation dialog appears.)*

> "A confirmation dialog asks: *'Are you sure you want to delete this transaction?'*
> I will click **Yes**."

*(Row disappears. Summary updates.)*

> "The transaction is removed from the table.
> The **Total Expenses** is back to zero, and the **Net Balance**
> has updated back to **৳ 25,000.00** automatically."

---

### [EXPLAIN DATA STORAGE — 3:10 to 3:30]

> "One important thing to note: **no database is used** in this application.
>
> All transaction data is stored in a simple C# list called `List<Transaction>`,
> which lives in memory while the application is running.
>
> The `Transaction` class has five properties: Date, Type, Category, Amount, and Notes.
>
> This design is intentionally simple and beginner-friendly,
> as required by the assignment."

---

### [CLOSING — 3:30 to 3:50]

> "That concludes the demonstration of my Personal Finance Tracker project.
>
> The application was built using:
> - C# with .NET 8
> - Windows Forms
> - No external libraries or databases
>
> All features work as required: adding transactions, validating input,
> displaying a real-time summary, and deleting selected transactions.
>
> Thank you for watching."

---

## Tips for Recording

- Use a screen recorder like **OBS Studio** (free) or the Windows built-in **Xbox Game Bar** (Win + G).
- Speak slowly and clearly.
- Rehearse the script once before recording.
- Make sure the application window is visible and not covered by other windows.
- Trim any silent pauses in the beginning or end of the video.

---

*Script prepared for CSE-3200 Assignment Demo — Personal Finance Tracker*
