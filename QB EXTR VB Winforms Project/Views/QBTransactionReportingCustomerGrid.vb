Imports System.Windows.Forms
Imports System.Drawing
Imports QBTransactions.ViewModels

Public Class QBTransactionReportingCustomerGrid

    Dim vm As New QBTransactionReportingViewModel
    Dim customerList As New List(Of String)
    Dim taQBCustomerNames As New CSIAutomationDataSetTableAdapters.QBCustomerNamesTableAdapter
    Dim rowClicked As Integer
    Dim StatusText As String = ""

    Private Enum MaintenanceState
        WaitingToStart
        Started
    End Enum

    Private Target As DateTime
    Private state As MaintenanceState = MaintenanceState.WaitingToStart
    Private MaintenanceTime As New TimeSpan(1, 0, 0) ' 1:00 am
    Private WaitingInterval As New TimeSpan(6, 0, 0) ' 6 hours
    Private AllJobsCompleted As Boolean = False

    Private Sub mnuViewAll_Click(sender As Object, e As EventArgs) Handles mnuViewAll.Click
        LoadAllCustomerBalances()
    End Sub

    Private Sub mnuViewWatchList_Click(sender As Object, e As EventArgs) Handles mnuViewWatchList.Click
        LoadWatchListCompanies()
    End Sub

    Private Sub UpdateAllCustomers()
        Dim nowString As String = Now.ToString()
        Dim s As String = "Beginning UpdateAllCustomers Routine at " + nowString
        txtLog.Text = txtLog.Text + s + System.Environment.NewLine

        AllJobsCompleted = False
        For Each row As DataGridViewRow In dgvCustomers.Rows
            GetQbTransactionsForCustomerByDataRow(row)
        Next
        AllJobsCompleted = True

        nowString = Now.ToString()
        s = "Finished UpdateAllCustomers Routine at " + nowString
        txtLog.Text = txtLog.Text + Environment.NewLine
        txtLog.Text = txtLog.Text + s + System.Environment.NewLine

    End Sub



    Private Sub GetQbTransactionsByName(customer As String)
        vm.GetQBTransactionsByCustomer(customer)
    End Sub



    Private Sub QBTransactionReportingCustomerGrid_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Cursor = Cursors.WaitCursor
        'KillQuickbooks()
        Try
            ' Timer stuff
            '      Target = GetNextMaintenanceTarget(MaintenanceTime)

            '    dgvCustomers.Font = New Font(lblStatus.Font, FontStyle.Regular)

            LoadWatchListCompanies()

        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try
        Me.Cursor = Cursors.Default
    End Sub

    'Private Function GetNextMaintenanceTarget(ByVal time As TimeSpan) As DateTime
    '    Dim dt As DateTime = DateTime.Today.Add(time)
    '    If DateTime.Now > dt Then
    '        dt = dt.AddDays(1) ' already past target time for today, next start is tomorrow
    '    End If
    '    Return dt
    'End Function

    Private Sub LoadWatchListCompanies()
        lblStatus.Text = "Requesting Customer data, please wait ..."
        customerList = vm.GetWatchList()
        lblStatus.Text = ""
        If customerList Is Nothing Then
            MessageBox.Show("Unable to retrieve a list of Customer Deposit customers from QuickBooks.")
        Else
            PopulateGrid()
        End If

    End Sub

    Private Async Sub LoadAllCustomerBalances()
        lblStatus.Text = "Requesting Customer data, please wait ..."
        customerList = vm.GetCustomersWithCustomerDepositBalances()
        lblStatus.Text = ""
        If customerList Is Nothing Then
            MessageBox.Show("Unable to retrieve a list of Customer Deposit customers from QuickBooks.")
        Else
            PopulateGrid()
        End If

    End Sub

    Private Sub GetQbTransactionsForCustomerByDataRow(ByVal row As DataGridViewRow)
        If row Is Nothing Then Return
        txtLog.AppendText("Beginning Update for " + row.Cells("CustomerName").Value + " at " + Now + System.Environment.NewLine)

        Me.Cursor = Cursors.WaitCursor
        row.Cells("CurrentBalance").Value = ""
        row.Cells("DateLastPull").Value = ""
        row.Cells("NumberOfTransactions").Value = ""

        GetQbTransactionsByName(row.Cells("CustomerName").Value)
        Dim pullRow = vm.GetQBTransactionPullData(row.Cells("CustomerName").Value)
        If Not pullRow Is Nothing Then
            row.Cells("CurrentBalance").Value = pullRow.Balance
            row.Cells("DateLastPull").Value = pullRow.DatePulled
            row.Cells("NumberOfTransactions").Value = pullRow.NumberOfTransactions
            If (row.Cells("ActualCustomerBalance").Value <> row.Cells("CurrentBalance").Value) Then
                row.Cells("CurrentBalance").Style.BackColor = Color.Red
                row.Cells("CurrentBalance").Style.ForeColor = Color.Black
            Else
                row.Cells("CurrentBalance").Style.BackColor = row.Cells("DateLastPull").Style.BackColor
                row.Cells("CurrentBalance").Style.BackColor = Color.Black
            End If
        End If
        Me.Cursor = Cursors.Default
        txtLog.ScrollBars = ScrollBars.Vertical
        txtLog.AppendText("Finished Update for " + row.Cells("CustomerName").Value + " at " + Now + System.Environment.NewLine + System.Environment.NewLine)
    End Sub

    Private Async Sub PopulateGrid()
        Dim i As Integer = 0
        Try
            dgvCustomers.Rows.Clear()
            Dim pullRow As CSIAutomationDataSet.QBTransactionPullRow
            For Each customer As String In customerList
                Try
                    pullRow = vm.GetQBTransactionPullData(customer)
                    If pullRow Is Nothing Then
                        'If vm.GetActualCustomerBalance(customer) <> 0 Then
                        dgvCustomers.Rows.Add(False, customer, vm.GetActualCustomerBalance(customer), vm.GetActualDateCustomerBalance(customer),
                                                  vm.GetStaticBalanceDate(customer), vm.GetStaticBalance(customer), "",
                                                  "", -1, "View")
                        'End If
                    Else
                        ' If vm.GetActualCustomerBalance(customer) <> 0 Then
                        dgvCustomers.Rows.Add(False, customer, vm.GetActualCustomerBalance(customer), vm.GetActualDateCustomerBalance(customer),
                                                  vm.GetStaticBalanceDate(customer), vm.GetStaticBalance(customer), pullRow.DatePulled,
                                                  pullRow.Balance, (vm.GetActualCustomerBalance(customer) - pullRow.Balance), pullRow.NumberOfTransactions, "View")
                        ' End If
                    End If

                    If (dgvCustomers.Rows(i).Cells("ActualCustomerBalance").Value <> dgvCustomers.Rows(i).Cells("CurrentBalance").Value) Then
                        Me.dgvCustomers.Rows(i).Cells("CurrentBalance").Style.BackColor = Color.Red
                        Me.dgvCustomers.Rows(i).Cells("CurrentBalance").Style.ForeColor = Color.White
                    Else

                        Me.dgvCustomers.Rows(i).Cells("CurrentBalance").Style.BackColor = dgvCustomers.Rows(i).Cells("DateLastPull").Style.BackColor
                        Me.dgvCustomers.Rows(i).Cells("CurrentBalance").Style.ForeColor = Color.Black
                    End If

                Catch ex As Exception
                    'MsgBox(ex.Message)
                End Try
                i = dgvCustomers.Rows.Count - 1

            Next
        Catch ex As Exception
            ' MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnSelectAll_Click(sender As Object, e As EventArgs) Handles btnSelectAll.Click
        For Each row As DataGridViewRow In dgvCustomers.Rows
            row.Cells(0).Value = True
        Next
    End Sub

    Private Sub btnSelectNone_Click(sender As Object, e As EventArgs) Handles btnSelectNone.Click
        For Each row As DataGridViewRow In dgvCustomers.Rows
            row.Cells(0).Value = False
        Next
    End Sub

    Private Sub QBTransactionReportingCustomerGrid_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        End
    End Sub

    Private Sub btnCheckForNewTransactions_Click(sender As Object, e As EventArgs)
        For Each row As DataGridViewRow In dgvCustomers.Rows
            If row.Cells(0).Value = True Then
                GetQbTransactionsForCustomerByDataRow(row)
            End If
        Next
    End Sub

    Private Sub dgvCustomers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCustomers.CellContentClick
        If e.ColumnIndex = 9 Then
            Dim theRow As DataGridViewRow
            theRow = dgvCustomers.Rows(e.RowIndex)
            Dim custName As String = theRow.Cells("CustomerName").Value
            vm.CreateExcelReport(custName, theRow.Cells("StaticDate").Value)
        End If
        dgvCustomers.CurrentCell = Nothing
    End Sub

    Private Sub PreferredCompanyWatchListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PreferredCompanyWatchListToolStripMenuItem.Click
        Using wl As New CompanyWatchList
            wl.ShowDialog()
        End Using
    End Sub

    Private Sub RefreshToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RefreshToolStripMenuItem.Click
        PopulateGrid()
    End Sub

    Private Sub QuitQToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QuitQToolStripMenuItem.Click
        End
    End Sub

    Private Sub mnuToolCheckForNewTransactions_Click(sender As Object, e As EventArgs) Handles mnuToolCheckForNewTransactions.Click
        For Each row As DataGridViewRow In dgvCustomers.Rows
            If row.Cells(0).Value = True Then
                GetQbTransactionsForCustomerByDataRow(row)
            End If
        Next
        PopulateGrid()
    End Sub

    Private Sub contextMenuCheckForNewTransactions_Click(sender As Object, e As EventArgs) Handles contextMenuCheckForNewTransactions.Click
        GetQbTransactionsForCustomerByDataRow(dgvCustomers.Rows(rowClicked))
        'GetQbTransactionsByName(dgvCustomers.Rows(rowClicked).Cells(1).Value)
    End Sub

    Private Sub dgvCustomers_CellMouseEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCustomers.CellMouseEnter
        rowClicked = e.RowIndex
    End Sub


    Private Sub mnuRecheckRowsWithErrors_Click(sender As Object, e As EventArgs) Handles mnuRecheckRowsWithErrors.Click
        For Each row As DataGridViewRow In dgvCustomers.Rows
            If row.Cells("Difference").Value <> "0.0" Then
                GetQbTransactionsForCustomerByDataRow(row)
            End If
        Next
    End Sub

    Private Sub mnuCreateStatements_Click(sender As Object, e As EventArgs) Handles mnuCreateStatements.Click
        Dim stmtGen As New StatementGenerator

        For Each row As DataGridViewRow In dgvCustomers.Rows
            If row.Cells(0).Value = True Then
                stmtGen.lstCompanies.Items.Add(row.Cells(1).Value)
            End If
        Next
        stmtGen.ShowDialog()

    End Sub

    Private Sub mnuImportStaticBalances_Click(sender As Object, e As EventArgs) Handles mnuImportStaticBalances.Click
        Using importer As New ImportCustomerDepositAmountsFromExcel
            importer.ImportCustDepBalancesFromExcel()
        End Using


    End Sub

    Private Sub MarkALLInvoicesToBePrintedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MarkALLInvoicesToBePrintedToolStripMenuItem.Click
        Using qbSpecial As New QBSpecialProjects
            For Each row As DataGridViewRow In dgvCustomers.Rows
                If row.Cells(0).Value = True Then
                    qbSpecial.UpdateAllInvoicesSetToBePrintedToTrue(row.Cells(1).Value)
                End If
            Next
        End Using
    End Sub

    Private Sub SetupQBConnectivityToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SetupQBConnectivityToolStripMenuItem.Click
        SetupQBFunctionality()

    End Sub
End Class