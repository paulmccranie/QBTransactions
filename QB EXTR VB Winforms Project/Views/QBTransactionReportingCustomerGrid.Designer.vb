<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class QBTransactionReportingCustomerGrid
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvCustomers = New System.Windows.Forms.DataGridView()
        Me.Applies = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.CustomerName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ActualCustomerBalance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ActualBalanceDatePulled = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StaticDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ConfirmedBalance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DateLastPull = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CurrentBalance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Difference = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NumberOfTransactions = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.contextMenuCheckForNewTransactions = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.CompanyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PreferredCompanyWatchListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewWatchList = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.RefreshToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ActionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuToolCheckForNewTransactions = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRecheckRowsWithErrors = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuCreateStatements = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuImportStaticBalances = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.QuitQToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SpecialToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MarkALLInvoicesToBePrintedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.btnSelectNone = New System.Windows.Forms.Button()
        Me.btnSelectAll = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.QBToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetupQBConnectivityToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VerifyQBConnectivityToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.dgvCustomers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvCustomers
        '
        Me.dgvCustomers.AllowUserToAddRows = False
        Me.dgvCustomers.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        Me.dgvCustomers.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvCustomers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCustomers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Applies, Me.CustomerName, Me.ActualCustomerBalance, Me.ActualBalanceDatePulled, Me.StaticDate, Me.ConfirmedBalance, Me.DateLastPull, Me.CurrentBalance, Me.Difference, Me.NumberOfTransactions})
        Me.dgvCustomers.ContextMenuStrip = Me.ContextMenuStrip1
        Me.dgvCustomers.Location = New System.Drawing.Point(12, 67)
        Me.dgvCustomers.Name = "dgvCustomers"
        Me.dgvCustomers.RowHeadersVisible = False
        Me.dgvCustomers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvCustomers.Size = New System.Drawing.Size(684, 388)
        Me.dgvCustomers.TabIndex = 0
        '
        'Applies
        '
        Me.Applies.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Applies.Frozen = True
        Me.Applies.HeaderText = ""
        Me.Applies.Name = "Applies"
        Me.Applies.Width = 60
        '
        'CustomerName
        '
        Me.CustomerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.CustomerName.Frozen = True
        Me.CustomerName.HeaderText = "Customer Name"
        Me.CustomerName.Name = "CustomerName"
        Me.CustomerName.ReadOnly = True
        Me.CustomerName.Width = 104
        '
        'ActualCustomerBalance
        '
        Me.ActualCustomerBalance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        DataGridViewCellStyle2.Format = "C2"
        Me.ActualCustomerBalance.DefaultCellStyle = DataGridViewCellStyle2
        Me.ActualCustomerBalance.HeaderText = "Actual Customer Deposit Balance"
        Me.ActualCustomerBalance.Name = "ActualCustomerBalance"
        Me.ActualCustomerBalance.ReadOnly = True
        Me.ActualCustomerBalance.Width = 148
        '
        'ActualBalanceDatePulled
        '
        Me.ActualBalanceDatePulled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ActualBalanceDatePulled.DefaultCellStyle = DataGridViewCellStyle3
        Me.ActualBalanceDatePulled.HeaderText = "Date That Actual Balance Was Updated"
        Me.ActualBalanceDatePulled.Name = "ActualBalanceDatePulled"
        Me.ActualBalanceDatePulled.ReadOnly = True
        Me.ActualBalanceDatePulled.Width = 148
        '
        'StaticDate
        '
        Me.StaticDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.StaticDate.DefaultCellStyle = DataGridViewCellStyle4
        Me.StaticDate.HeaderText = "Static Date of Confirmed Balance"
        Me.StaticDate.Name = "StaticDate"
        Me.StaticDate.ReadOnly = True
        Me.StaticDate.Width = 147
        '
        'ConfirmedBalance
        '
        Me.ConfirmedBalance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle5.Format = "C2"
        DataGridViewCellStyle5.NullValue = "0"
        Me.ConfirmedBalance.DefaultCellStyle = DataGridViewCellStyle5
        Me.ConfirmedBalance.HeaderText = "Confirmed Balance"
        Me.ConfirmedBalance.Name = "ConfirmedBalance"
        Me.ConfirmedBalance.ReadOnly = True
        Me.ConfirmedBalance.Width = 118
        '
        'DateLastPull
        '
        Me.DateLastPull.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        DataGridViewCellStyle6.Format = "d"
        DataGridViewCellStyle6.NullValue = Nothing
        Me.DateLastPull.DefaultCellStyle = DataGridViewCellStyle6
        Me.DateLastPull.HeaderText = "Date of Last Pulled Transactions"
        Me.DateLastPull.Name = "DateLastPull"
        Me.DateLastPull.ReadOnly = True
        Me.DateLastPull.Width = 177
        '
        'CurrentBalance
        '
        Me.CurrentBalance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        DataGridViewCellStyle7.Format = "C2"
        DataGridViewCellStyle7.NullValue = "0"
        Me.CurrentBalance.DefaultCellStyle = DataGridViewCellStyle7
        Me.CurrentBalance.HeaderText = "Balance As Of Last Pull (Calculated)"
        Me.CurrentBalance.Name = "CurrentBalance"
        Me.CurrentBalance.ReadOnly = True
        Me.CurrentBalance.Width = 138
        '
        'Difference
        '
        Me.Difference.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle8.Format = "C2"
        DataGridViewCellStyle8.NullValue = Nothing
        Me.Difference.DefaultCellStyle = DataGridViewCellStyle8
        Me.Difference.HeaderText = "Amount Difference"
        Me.Difference.Name = "Difference"
        Me.Difference.Width = 118
        '
        'NumberOfTransactions
        '
        Me.NumberOfTransactions.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        Me.NumberOfTransactions.HeaderText = "Number of Transactions"
        Me.NumberOfTransactions.Name = "NumberOfTransactions"
        Me.NumberOfTransactions.ReadOnly = True
        Me.NumberOfTransactions.Width = 140
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.contextMenuCheckForNewTransactions})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(224, 26)
        '
        'contextMenuCheckForNewTransactions
        '
        Me.contextMenuCheckForNewTransactions.Name = "contextMenuCheckForNewTransactions"
        Me.contextMenuCheckForNewTransactions.Size = New System.Drawing.Size(223, 22)
        Me.contextMenuCheckForNewTransactions.Text = "Check For New Transactions"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ActionsToolStripMenuItem, Me.ViewToolStripMenuItem, Me.CompanyToolStripMenuItem, Me.SpecialToolStripMenuItem, Me.QBToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.ShowItemToolTips = True
        Me.MenuStrip1.Size = New System.Drawing.Size(708, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'CompanyToolStripMenuItem
        '
        Me.CompanyToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PreferredCompanyWatchListToolStripMenuItem})
        Me.CompanyToolStripMenuItem.Name = "CompanyToolStripMenuItem"
        Me.CompanyToolStripMenuItem.Size = New System.Drawing.Size(71, 20)
        Me.CompanyToolStripMenuItem.Text = "Company"
        '
        'PreferredCompanyWatchListToolStripMenuItem
        '
        Me.PreferredCompanyWatchListToolStripMenuItem.Name = "PreferredCompanyWatchListToolStripMenuItem"
        Me.PreferredCompanyWatchListToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.PreferredCompanyWatchListToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.PreferredCompanyWatchListToolStripMenuItem.Text = "Preferred Company Watch List"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuViewWatchList, Me.mnuViewAll, Me.ToolStripSeparator1, Me.RefreshToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'mnuViewWatchList
        '
        Me.mnuViewWatchList.Name = "mnuViewWatchList"
        Me.mnuViewWatchList.Size = New System.Drawing.Size(185, 22)
        Me.mnuViewWatchList.Text = "View Watch List Only"
        '
        'mnuViewAll
        '
        Me.mnuViewAll.Name = "mnuViewAll"
        Me.mnuViewAll.Size = New System.Drawing.Size(185, 22)
        Me.mnuViewAll.Text = "View All Companies"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(182, 6)
        '
        'RefreshToolStripMenuItem
        '
        Me.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem"
        Me.RefreshToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.RefreshToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.RefreshToolStripMenuItem.Text = "Refresh"
        '
        'ActionsToolStripMenuItem
        '
        Me.ActionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuToolCheckForNewTransactions, Me.mnuRecheckRowsWithErrors, Me.ToolStripSeparator2, Me.mnuCreateStatements, Me.ToolStripSeparator3, Me.mnuImportStaticBalances, Me.ToolStripSeparator4, Me.QuitQToolStripMenuItem})
        Me.ActionsToolStripMenuItem.Name = "ActionsToolStripMenuItem"
        Me.ActionsToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.ActionsToolStripMenuItem.Text = "File"
        '
        'mnuToolCheckForNewTransactions
        '
        Me.mnuToolCheckForNewTransactions.Name = "mnuToolCheckForNewTransactions"
        Me.mnuToolCheckForNewTransactions.Size = New System.Drawing.Size(306, 22)
        Me.mnuToolCheckForNewTransactions.Text = "Check For New Transactions"
        '
        'mnuRecheckRowsWithErrors
        '
        Me.mnuRecheckRowsWithErrors.Name = "mnuRecheckRowsWithErrors"
        Me.mnuRecheckRowsWithErrors.Size = New System.Drawing.Size(306, 22)
        Me.mnuRecheckRowsWithErrors.Text = "Recheck Companies with Incorrect Balances"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(303, 6)
        '
        'mnuCreateStatements
        '
        Me.mnuCreateStatements.Name = "mnuCreateStatements"
        Me.mnuCreateStatements.Size = New System.Drawing.Size(306, 22)
        Me.mnuCreateStatements.Text = "Create Statements"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(303, 6)
        '
        'mnuImportStaticBalances
        '
        Me.mnuImportStaticBalances.Name = "mnuImportStaticBalances"
        Me.mnuImportStaticBalances.Size = New System.Drawing.Size(306, 22)
        Me.mnuImportStaticBalances.Text = "Import Static Balance Report"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(303, 6)
        '
        'QuitQToolStripMenuItem
        '
        Me.QuitQToolStripMenuItem.Name = "QuitQToolStripMenuItem"
        Me.QuitQToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Q), System.Windows.Forms.Keys)
        Me.QuitQToolStripMenuItem.Size = New System.Drawing.Size(306, 22)
        Me.QuitQToolStripMenuItem.Text = "Quit"
        '
        'SpecialToolStripMenuItem
        '
        Me.SpecialToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MarkALLInvoicesToBePrintedToolStripMenuItem})
        Me.SpecialToolStripMenuItem.Name = "SpecialToolStripMenuItem"
        Me.SpecialToolStripMenuItem.Size = New System.Drawing.Size(56, 20)
        Me.SpecialToolStripMenuItem.Text = "Special"
        '
        'MarkALLInvoicesToBePrintedToolStripMenuItem
        '
        Me.MarkALLInvoicesToBePrintedToolStripMenuItem.Name = "MarkALLInvoicesToBePrintedToolStripMenuItem"
        Me.MarkALLInvoicesToBePrintedToolStripMenuItem.Size = New System.Drawing.Size(243, 22)
        Me.MarkALLInvoicesToBePrintedToolStripMenuItem.Text = "Mark ALL Invoices To Be Printed"
        '
        'txtLog
        '
        Me.txtLog.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLog.Location = New System.Drawing.Point(12, 461)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.Size = New System.Drawing.Size(684, 67)
        Me.txtLog.TabIndex = 6
        '
        'btnSelectNone
        '
        Me.btnSelectNone.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectNone.Location = New System.Drawing.Point(113, 40)
        Me.btnSelectNone.Name = "btnSelectNone"
        Me.btnSelectNone.Size = New System.Drawing.Size(81, 21)
        Me.btnSelectNone.TabIndex = 2
        Me.btnSelectNone.Text = "Select None"
        Me.btnSelectNone.UseVisualStyleBackColor = True
        '
        'btnSelectAll
        '
        Me.btnSelectAll.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectAll.Location = New System.Drawing.Point(26, 40)
        Me.btnSelectAll.Name = "btnSelectAll"
        Me.btnSelectAll.Size = New System.Drawing.Size(81, 21)
        Me.btnSelectAll.TabIndex = 1
        Me.btnSelectAll.Text = "Select All"
        Me.btnSelectAll.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Teal
        Me.lblStatus.Location = New System.Drawing.Point(304, 48)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(78, 16)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Status Label"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'QBToolStripMenuItem
        '
        Me.QBToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SetupQBConnectivityToolStripMenuItem, Me.VerifyQBConnectivityToolStripMenuItem})
        Me.QBToolStripMenuItem.Name = "QBToolStripMenuItem"
        Me.QBToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.QBToolStripMenuItem.Text = "QB"
        '
        'SetupQBConnectivityToolStripMenuItem
        '
        Me.SetupQBConnectivityToolStripMenuItem.Name = "SetupQBConnectivityToolStripMenuItem"
        Me.SetupQBConnectivityToolStripMenuItem.Size = New System.Drawing.Size(193, 22)
        Me.SetupQBConnectivityToolStripMenuItem.Text = "Setup QB Connectivity"
        '
        'VerifyQBConnectivityToolStripMenuItem
        '
        Me.VerifyQBConnectivityToolStripMenuItem.Name = "VerifyQBConnectivityToolStripMenuItem"
        Me.VerifyQBConnectivityToolStripMenuItem.Size = New System.Drawing.Size(193, 22)
        Me.VerifyQBConnectivityToolStripMenuItem.Text = "Verify QB Connectivity"
        '
        'QBTransactionReportingCustomerGrid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(708, 540)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.dgvCustomers)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.btnSelectNone)
        Me.Controls.Add(Me.btnSelectAll)
        Me.Controls.Add(Me.lblStatus)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "QBTransactionReportingCustomerGrid"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "QB Transaction Reporting"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvCustomers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvCustomers As System.Windows.Forms.DataGridView
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnSelectNone As System.Windows.Forms.Button
    Friend WithEvents btnSelectAll As System.Windows.Forms.Button
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents CompanyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PreferredCompanyWatchListToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RefreshToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ActionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents QuitQToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuToolCheckForNewTransactions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents contextMenuCheckForNewTransactions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuViewWatchList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuViewAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuRecheckRowsWithErrors As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuCreateStatements As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Applies As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents CustomerName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ActualCustomerBalance As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ActualBalanceDatePulled As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents StaticDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ConfirmedBalance As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DateLastPull As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CurrentBalance As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Difference As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NumberOfTransactions As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents mnuImportStaticBalances As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SpecialToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MarkALLInvoicesToBePrintedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents QBToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetupQBConnectivityToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents VerifyQBConnectivityToolStripMenuItem As Windows.Forms.ToolStripMenuItem
End Class
