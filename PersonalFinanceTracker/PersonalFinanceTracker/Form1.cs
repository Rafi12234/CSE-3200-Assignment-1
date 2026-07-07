// Form1.cs
// Main form of the Personal Finance Tracker application.
// All UI is built entirely in code (no .Designer.cs needed).
// Data is stored in-memory using List<Transaction>. No database is used.
//
// Compatible with: .NET Framework 4.8 + Visual Studio 2019/2022
// ─────────────────────────────────────────────────────────────────────────

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace PersonalFinanceTracker
{
    public partial class Form1 :  Form
    {
        // ─────────────────────────────────────────────────────────────
        //  IN-MEMORY STORAGE
        //  All transactions are kept in this list during runtime.
        //  When the application closes, data is lost (no database).
        // ─────────────────────────────────────────────────────────────
        private List<Transaction> transactions = new List<Transaction>();

        // ─────────────────────────────────────────────────────────────
        //  UI CONTROL REFERENCES
        //  Declared as fields so every method can access them easily.
        // ─────────────────────────────────────────────────────────────

        // Summary card labels (updated whenever data changes)
        private Label lblIncomeAmount  = new Label();
        private Label lblExpenseAmount = new Label();
        private Label lblBalanceAmount = new Label();

        // Input controls inside the left "Add Transaction" panel
        private TextBox        txtAmount   = new TextBox();
        private ComboBox       cboCategory = new ComboBox();
        private RadioButton    rbIncome    = new RadioButton();
        private RadioButton    rbExpense   = new RadioButton();
        private DateTimePicker dtpDate     = new DateTimePicker();
        private TextBox        txtNotes    = new TextBox();

        // DataGridView that lists all transactions
        private DataGridView dgvTransactions = new DataGridView();

        // ─────────────────────────────────────────────────────────────
        //  CONSTRUCTOR
        // ─────────────────────────────────────────────────────────────
        public Form1()
        {
            // Configure the form itself
            this.Text            = "Personal Finance Tracker";
            this.Size            = new Size(900, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;  // Non-resizable
            this.MaximizeBox     = false;
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.BackColor       = Color.FromArgb(240, 242, 245); // Light gray background
            this.Font            = new Font("Segoe UI", 9f);

            // Build every control on the form
            InitializeCustomComponents();
        }

        // ─────────────────────────────────────────────────────────────
        //  SECTION 1 — BUILD THE ENTIRE UI
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Creates and places every UI control on the form.
        /// Called once from the constructor.
        /// </summary>
        private void InitializeCustomComponents()
        {
            // ── Header / Title bar ─────────────────────────────────────
            Panel pnlHeader = new Panel();
            pnlHeader.Location  = new Point(0, 0);
            pnlHeader.Size      = new Size(900, 50);
            pnlHeader.BackColor = Color.FromArgb(30, 55, 100);

            Label lblTitle = new Label();
            lblTitle.Text      = "  Personal Finance Tracker";
            lblTitle.Font      = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize  = false;
            lblTitle.Location  = new Point(12, 10);
            lblTitle.Size      = new Size(500, 30);

            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            // ── Build the three sections ───────────────────────────────
            BuildSummaryCards();   // Top area: 3 summary cards
            BuildInputPanel();     // Left side: Add Transaction panel
            BuildGridPanel();      // Right side: DataGridView + Delete button
        }

        // ─────────────────────────────────────────────────────────────
        //  BUILD SUMMARY CARDS (top area, below header)
        // ─────────────────────────────────────────────────────────────
        private void BuildSummaryCards()
        {
            // Container panel for all three cards
            Panel pnlCards = new Panel();
            pnlCards.Location  = new Point(10, 58);
            pnlCards.Size      = new Size(870, 88);
            pnlCards.BackColor = Color.Transparent;

            int cardW = 278;
            int gap   = 18;

            // ─ Card 1: Total Income (green) ─
            Panel cardIncome = CreateCard("Total Income", Color.FromArgb(39, 174, 96), out lblIncomeAmount);
            cardIncome.Location = new Point(0, 0);

            // ─ Card 2: Total Expenses (red) ─
            Panel cardExpense = CreateCard("Total Expenses", Color.FromArgb(192, 57, 43), out lblExpenseAmount);
            cardExpense.Location = new Point(cardW + gap, 0);

            // ─ Card 3: Net Balance (blue) ─
            Panel cardBalance = CreateCard("Net Balance", Color.FromArgb(41, 128, 185), out lblBalanceAmount);
            cardBalance.Location = new Point((cardW + gap) * 2, 0);

            pnlCards.Controls.Add(cardIncome);
            pnlCards.Controls.Add(cardExpense);
            pnlCards.Controls.Add(cardBalance);
            this.Controls.Add(pnlCards);

            // Display initial zero values
            UpdateSummary();
        }

        /// <summary>
        /// Helper that creates one summary card panel with a title and amount label.
        /// </summary>
        private Panel CreateCard(string title, Color bgColor, out Label amountLabel)
        {
            Panel card = new Panel();
            card.Size      = new Size(278, 84);
            card.BackColor = bgColor;

            Label lblTitle = new Label();
            lblTitle.Text      = title;
            lblTitle.Font      = new Font("Segoe UI", 9f, FontStyle.Regular);
            lblTitle.ForeColor = Color.FromArgb(210, 235, 255);
            lblTitle.AutoSize  = false;
            lblTitle.Location  = new Point(14, 10);
            lblTitle.Size      = new Size(250, 18);

            amountLabel = new Label();
            amountLabel.Text      = FormatCurrency(0);
            amountLabel.Font      = new Font("Segoe UI", 17f, FontStyle.Bold);
            amountLabel.ForeColor = Color.White;
            amountLabel.AutoSize  = false;
            amountLabel.Location  = new Point(14, 32);
            amountLabel.Size      = new Size(250, 40);

            card.Controls.Add(lblTitle);
            card.Controls.Add(amountLabel);

            return card;
        }

        // ─────────────────────────────────────────────────────────────
        //  BUILD INPUT PANEL (left side — "Add Transaction")
        // ─────────────────────────────────────────────────────────────
        private void BuildInputPanel()
        {
            // White card panel on the left
            Panel pnlInput = new Panel();
            pnlInput.Location  = new Point(10, 154);
            pnlInput.Size      = new Size(272, 406);
            pnlInput.BackColor = Color.White;
            pnlInput.BorderStyle = BorderStyle.FixedSingle;

            // Panel heading
            Label lblHeading = new Label();
            lblHeading.Text      = "Add Transaction";
            lblHeading.Font      = new Font("Segoe UI", 11f, FontStyle.Bold);
            lblHeading.ForeColor = Color.FromArgb(30, 55, 100);
            lblHeading.AutoSize  = false;
            lblHeading.Location  = new Point(12, 12);
            lblHeading.Size      = new Size(240, 24);
            pnlInput.Controls.Add(lblHeading);

            // Divider line
            Panel divider = new Panel();
            divider.Location  = new Point(12, 38);
            divider.Size      = new Size(240, 2);
            divider.BackColor = Color.FromArgb(220, 225, 235);
            pnlInput.Controls.Add(divider);

            // Vertical position tracker inside the panel
            int y        = 48;
            int labelH   = 16;
            int ctrlH    = 26;
            int gap      = 5;
            int rowGap   = 10;

            // ── Amount ──────────────────────────────────────
            pnlInput.Controls.Add(MakeLabel("Amount (in ৳)", 12, y));
            y += labelH + gap;

            txtAmount.Location    = new Point(12, y);
            txtAmount.Size        = new Size(244, ctrlH);
            txtAmount.Font        = new Font("Segoe UI", 10f);
            txtAmount.BorderStyle = BorderStyle.FixedSingle;
            pnlInput.Controls.Add(txtAmount);
            y += ctrlH + rowGap;

            // ── Type — Radio Buttons ─────────────────────────
            pnlInput.Controls.Add(MakeLabel("Transaction Type", 12, y));
            y += labelH + gap;

            rbIncome.Text      = "Income";
            rbIncome.Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            rbIncome.Location  = new Point(12, y);
            rbIncome.Size      = new Size(88, 22);
            rbIncome.Checked   = true;                             // Default: Income
            rbIncome.ForeColor = Color.FromArgb(39, 174, 96);

            rbExpense.Text      = "Expense";
            rbExpense.Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            rbExpense.Location  = new Point(108, y);
            rbExpense.Size      = new Size(88, 22);
            rbExpense.ForeColor = Color.FromArgb(192, 57, 43);

            pnlInput.Controls.Add(rbIncome);
            pnlInput.Controls.Add(rbExpense);
            y += 22 + rowGap;

            // ── Category ─────────────────────────────────────
            pnlInput.Controls.Add(MakeLabel("Category", 12, y));
            y += labelH + gap;

            cboCategory.Location      = new Point(12, y);
            cboCategory.Size          = new Size(244, ctrlH);
            cboCategory.Font          = new Font("Segoe UI", 10f);
            cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategory.FlatStyle     = FlatStyle.Flat;
            // Add category options
            cboCategory.Items.AddRange(new string[] { "Salary", "Freelance", "Food", "Transport", "Utilities", "Other" });
            cboCategory.SelectedIndex = 0;  // Default: Salary
            pnlInput.Controls.Add(cboCategory);
            y += ctrlH + rowGap;

            // ── Date ─────────────────────────────────────────
            pnlInput.Controls.Add(MakeLabel("Date", 12, y));
            y += labelH + gap;

            dtpDate.Location = new Point(12, y);
            dtpDate.Size     = new Size(244, ctrlH);
            dtpDate.Font     = new Font("Segoe UI", 10f);
            dtpDate.Format   = DateTimePickerFormat.Short;
            dtpDate.Value    = DateTime.Today;  // Default: today's date
            pnlInput.Controls.Add(dtpDate);
            y += ctrlH + rowGap;

            // ── Notes ────────────────────────────────────────
            pnlInput.Controls.Add(MakeLabel("Notes (optional)", 12, y));
            y += labelH + gap;

            txtNotes.Location    = new Point(12, y);
            txtNotes.Size        = new Size(244, 54);
            txtNotes.Font        = new Font("Segoe UI", 9.5f);
            txtNotes.BorderStyle = BorderStyle.FixedSingle;
            txtNotes.Multiline   = true;
            pnlInput.Controls.Add(txtNotes);
            y += 54 + rowGap + 2;

            // ── Add Transaction Button (blue) ──────────────────
            Button btnAdd = new Button();
            btnAdd.Text      = "+  Add Transaction";
            btnAdd.Font      = new Font("Segoe UI", 10f, FontStyle.Bold);
            btnAdd.Location  = new Point(12, y);
            btnAdd.Size      = new Size(244, 36);
            btnAdd.BackColor = Color.FromArgb(30, 55, 150);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Cursor    = Cursors.Hand;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += (s, e) => AddTransaction();

            // Hover: slightly lighter blue
            btnAdd.MouseEnter += (s, e) => { btnAdd.BackColor = Color.FromArgb(50, 80, 180); };
            btnAdd.MouseLeave += (s, e) => { btnAdd.BackColor = Color.FromArgb(30, 55, 150); };

            pnlInput.Controls.Add(btnAdd);

            this.Controls.Add(pnlInput);
        }

        // ─────────────────────────────────────────────────────────────
        //  BUILD GRID PANEL (right side — DataGridView + Delete button)
        // ─────────────────────────────────────────────────────────────
        private void BuildGridPanel()
        {
            // White card panel on the right
            Panel pnlGrid = new Panel();
            pnlGrid.Location    = new Point(292, 154);
            pnlGrid.Size        = new Size(592, 406);
            pnlGrid.BackColor   = Color.White;
            pnlGrid.BorderStyle = BorderStyle.FixedSingle;

            // Panel heading
            Label lblHeading = new Label();
            lblHeading.Text      = "Transaction History";
            lblHeading.Font      = new Font("Segoe UI", 11f, FontStyle.Bold);
            lblHeading.ForeColor = Color.FromArgb(30, 55, 100);
            lblHeading.AutoSize  = false;
            lblHeading.Location  = new Point(12, 12);
            lblHeading.Size      = new Size(400, 24);
            pnlGrid.Controls.Add(lblHeading);

            // Divider line
            Panel divider = new Panel();
            divider.Location  = new Point(12, 38);
            divider.Size      = new Size(560, 2);
            divider.BackColor = Color.FromArgb(220, 225, 235);
            pnlGrid.Controls.Add(divider);

            // ── DataGridView ──────────────────────────────────────────
            dgvTransactions.Location               = new Point(12, 46);
            dgvTransactions.Size                   = new Size(562, 312);
            dgvTransactions.ReadOnly               = true;          // Cells are not editable
            dgvTransactions.AllowUserToAddRows     = false;         // No blank new-row at bottom
            dgvTransactions.AllowUserToDeleteRows  = false;         // Delete handled by button only
            dgvTransactions.SelectionMode          = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.MultiSelect            = false;         // One row at a time
            dgvTransactions.AutoSizeColumnsMode    = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTransactions.BorderStyle            = BorderStyle.None;
            dgvTransactions.BackgroundColor        = Color.White;
            dgvTransactions.GridColor              = Color.FromArgb(220, 225, 235);
            dgvTransactions.RowHeadersVisible      = false;
            dgvTransactions.Font                   = new Font("Segoe UI", 9.5f);
            dgvTransactions.CellBorderStyle        = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTransactions.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvTransactions.ColumnHeadersHeight = 36;

            // Header row styling
            dgvTransactions.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 55, 100);
            dgvTransactions.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTransactions.ColumnHeadersDefaultCellStyle.Font      =
                new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvTransactions.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            dgvTransactions.EnableHeadersVisualStyles = false;

            // Row styling
            dgvTransactions.RowsDefaultCellStyle.BackColor            = Color.White;
            dgvTransactions.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 251);
            dgvTransactions.DefaultCellStyle.SelectionBackColor       = Color.FromArgb(173, 214, 240);
            dgvTransactions.DefaultCellStyle.SelectionForeColor       = Color.FromArgb(20, 20, 20);
            dgvTransactions.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
            dgvTransactions.RowTemplate.Height = 30;

            // Add columns — these match the Transaction properties
            dgvTransactions.Columns.Add(CreateColumn("Date",     "Date",     18));
            dgvTransactions.Columns.Add(CreateColumn("Type",     "Type",     15));
            dgvTransactions.Columns.Add(CreateColumn("Category", "Category", 20));
            dgvTransactions.Columns.Add(CreateColumn("Amount",   "Amount",   22));
            dgvTransactions.Columns.Add(CreateColumn("Notes",    "Notes",    25));

            // Alignment for specific columns
            dgvTransactions.Columns["Date"].DefaultCellStyle.Alignment     =
                DataGridViewContentAlignment.MiddleCenter;
            dgvTransactions.Columns["Type"].DefaultCellStyle.Alignment     =
                DataGridViewContentAlignment.MiddleCenter;
            dgvTransactions.Columns["Category"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            dgvTransactions.Columns["Amount"].DefaultCellStyle.Alignment   =
                DataGridViewContentAlignment.MiddleRight;

            // Color the "Type" cell text: green for Income, red for Expense
            dgvTransactions.CellFormatting += DgvTransactions_CellFormatting;

            pnlGrid.Controls.Add(dgvTransactions);

            // ── Delete Selected Button (red) ──────────────────────────
            Button btnDelete = new Button();
            btnDelete.Text      = "   Delete Selected Transaction";
            btnDelete.Font      = new Font("Segoe UI", 10f, FontStyle.Bold);
            btnDelete.Location  = new Point(12, 366);
            btnDelete.Size      = new Size(562, 36);
            btnDelete.BackColor = Color.FromArgb(192, 57, 43);
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Cursor    = Cursors.Hand;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += (s, e) => DeleteSelectedTransaction();

            // Hover: slightly lighter red
            btnDelete.MouseEnter += (s, e) => { btnDelete.BackColor = Color.FromArgb(220, 75, 55); };
            btnDelete.MouseLeave += (s, e) => { btnDelete.BackColor = Color.FromArgb(192, 57, 43); };

            pnlGrid.Controls.Add(btnDelete);

            this.Controls.Add(pnlGrid);
        }

        // ─────────────────────────────────────────────────────────────
        //  SECTION 2 — BUSINESS LOGIC METHODS
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Validates the input fields and adds a new Transaction to the in-memory list.
        /// Called when the "Add Transaction" button is clicked.
        /// </summary>
        private void AddTransaction()
        {
            // ── Step 1: Validate Amount ────────────────────────────────

            // Check if the amount text box is empty
            if (string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                MessageBox.Show(
                    "Please enter an amount.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtAmount.Focus();
                return; // Stop here — do not add the transaction
            }

            decimal amount;

            // Check if the text can be parsed as a decimal number
            if (!decimal.TryParse(txtAmount.Text.Trim(), out amount))
            {
                MessageBox.Show(
                    "Please enter a valid positive amount.\n\nExample: 5000 or 1500.50",
                    "Invalid Amount",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtAmount.Focus();
                return; // Stop here — do not add the transaction
            }

            // Check if the amount is greater than zero
            if (amount <= 0)
            {
                MessageBox.Show(
                    "Please enter a valid positive amount.\nAmount must be greater than 0.",
                    "Invalid Amount",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtAmount.Focus();
                return; // Stop here — do not add the transaction
            }

            // ── Step 2: Create the Transaction object ──────────────────

            Transaction newTransaction = new Transaction();
            newTransaction.Date     = dtpDate.Value.Date;
            newTransaction.Type     = rbIncome.Checked ? "Income" : "Expense";
            newTransaction.Category = cboCategory.SelectedItem != null
                                        ? cboCategory.SelectedItem.ToString()
                                        : "Other";
            newTransaction.Amount   = amount;
            newTransaction.Notes    = txtNotes.Text.Trim();

            // ── Step 3: Add to in-memory list ──────────────────────────
            transactions.Add(newTransaction);

            // ── Step 4: Refresh the grid and summary ───────────────────
            RefreshTransactionGrid();
            UpdateSummary();

            // ── Step 5: Reset input fields ─────────────────────────────
            ClearInputFields();
        }

        /// <summary>
        /// Removes the selected transaction from the list and refreshes the UI.
        /// Called when the "Delete Selected" button is clicked.
        /// </summary>
        private void DeleteSelectedTransaction()
        {
            // Check if a row is selected
            if (dgvTransactions.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a transaction to delete.",
                    "No Selection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // Ask for confirmation before deleting
            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete this transaction?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return; // User cancelled — do nothing

            // Get the row index of the selected row
            int selectedIndex = dgvTransactions.SelectedRows[0].Index;

            // Remove the corresponding transaction from the in-memory list
            transactions.RemoveAt(selectedIndex);

            // Refresh the grid and summary
            RefreshTransactionGrid();
            UpdateSummary();
        }

        /// <summary>
        /// Clears the DataGridView and repopulates it from the transactions list.
        /// </summary>
        private void RefreshTransactionGrid()
        {
            // Remove all current rows from the grid
            dgvTransactions.Rows.Clear();

            // Add a new row for each transaction in the list
            foreach (Transaction t in transactions)
            {
                dgvTransactions.Rows.Add(
                    t.Date.ToString("dd/MM/yyyy"),  // Format: 01/07/2026
                    t.Type,                          // "Income" or "Expense"
                    t.Category,                      // e.g. "Salary"
                    FormatCurrency(t.Amount),        // e.g. "৳ 5,000.00"
                    t.Notes                          // Optional note text
                );
            }
        }

        /// <summary>
        /// Recalculates Total Income, Total Expenses, and Net Balance,
        /// then updates the three summary card labels.
        /// </summary>
        private void UpdateSummary()
        {
            // Sum all transactions where Type is "Income"
            decimal totalIncome = 0m;
            foreach (Transaction t in transactions)
                if (t.Type == "Income")
                    totalIncome += t.Amount;

            // Sum all transactions where Type is "Expense"
            decimal totalExpense = 0m;
            foreach (Transaction t in transactions)
                if (t.Type == "Expense")
                    totalExpense += t.Amount;

            // Net Balance = Income - Expenses
            decimal netBalance = totalIncome - totalExpense;

            // Update the labels in the summary cards
            lblIncomeAmount.Text  = FormatCurrency(totalIncome);
            lblExpenseAmount.Text = FormatCurrency(totalExpense);
            lblBalanceAmount.Text = FormatCurrency(netBalance);

            // Color: green for positive/zero balance, red for negative
            lblBalanceAmount.ForeColor = (netBalance >= 0)
                ? Color.FromArgb(190, 255, 190)  // Light green
                : Color.FromArgb(255, 160, 160); // Light red
        }

        /// <summary>
        /// Resets all input controls back to their default values.
        /// Called after a transaction is successfully added.
        /// </summary>
        private void ClearInputFields()
        {
            txtAmount.Text            = string.Empty;         // Empty amount
            txtNotes.Text             = string.Empty;         // Empty notes
            cboCategory.SelectedIndex = 0;                    // Back to Salary
            rbIncome.Checked          = true;                 // Back to Income
            dtpDate.Value             = DateTime.Today;       // Back to today
            txtAmount.Focus();                                // Focus for next entry
        }

        // ─────────────────────────────────────────────────────────────
        //  SECTION 3 — HELPER / UTILITY METHODS
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Formats a decimal as Bangladeshi Taka currency.
        /// Example: 12000.50 → "৳ 12,000.50"
        /// </summary>
        private string FormatCurrency(decimal amount)
        {
            return string.Format("৳ {0:N2}", amount);
        }

        /// <summary>
        /// Creates a small bold label with consistent styling.
        /// Used for field titles inside the input panel.
        /// </summary>
        private Label MakeLabel(string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text      = text;
            lbl.Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(80, 85, 105);
            lbl.AutoSize  = false;
            lbl.Location  = new Point(x, y);
            lbl.Size      = new Size(244, 16);
            return lbl;
        }

        /// <summary>
        /// Creates a DataGridView text column with specified name and fill weight.
        /// </summary>
        private DataGridViewTextBoxColumn CreateColumn(string name, string header, int fillWeight)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name        = name;
            col.HeaderText  = header;
            col.FillWeight  = fillWeight;
            col.SortMode    = DataGridViewColumnSortMode.NotSortable;
            return col;
        }

        /// <summary>
        /// Event handler: colors the "Type" column text green for Income, red for Expense.
        /// This fires every time a cell is painted.
        /// </summary>
        private void DgvTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Only apply to the "Type" column
            if (dgvTransactions.Columns[e.ColumnIndex].Name == "Type" && e.Value != null)
            {
                string typeValue = e.Value.ToString();

                if (typeValue == "Income")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(39, 174, 96);  // Green
                    e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                }
                else if (typeValue == "Expense")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(192, 57, 43); // Red
                    e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                }
            }
        }
    }
}
