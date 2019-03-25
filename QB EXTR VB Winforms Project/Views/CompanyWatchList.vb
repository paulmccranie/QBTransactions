Imports System.Windows.Forms

Public Class CompanyWatchList
    Dim taNames As New CSIAutomationDataSetTableAdapters.QBCustomerNamesTableAdapter
    Dim taWatchList As New CSIAutomationDataSetTableAdapters.CompanyWatchListTableAdapter
    Dim dtWatchList As New CSIAutomationDataSet.CompanyWatchListDataTable

    Private Sub CompanyWatchList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim dt As New CSIAutomationDataSet.QBCustomerNamesDataTable
        taNames.FillByCustomersWithBalances(dt)

        For Each row As CSIAutomationDataSet.QBCustomerNamesRow In dt.Rows
            clbAllCompanies.Items.Add(row.CustomerName, ShouldBeChecked(row.CustomerName))
        Next
        clbAllCompanies.Sorted = True
        FillWatchList()
    End Sub

    Private Function ShouldBeChecked(ByVal customerName As String) As Boolean
        Try
            Return taWatchList.ShouldPullTransactions(customerName)
        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Sub clbAllCompanies_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles clbAllCompanies.ItemCheck
        If e.CurrentValue = CheckState.Checked Then
            taWatchList.RemoveFromWatchList(clbAllCompanies.Items(e.Index))
        Else
            AddToWatchList(clbAllCompanies.Items(e.Index))
        End If
        FillWatchList()
    End Sub

    Private Sub AddToWatchList(thecompanyName As String)
        If taWatchList.ShouldPullTransactions(thecompanyName) = True Then Return
        taWatchList.AddToWatchList(theCompanyName, True)
    End Sub

    Private Sub FillWatchList()
        lstWatchedCompanies.Items.Clear()
        taWatchList.GetActiveWatchList(dtWatchList)
        For Each companyWatchListRow As CSIAutomationDataSet.CompanyWatchListRow In dtWatchList.Rows
            lstWatchedCompanies.Items.Add(companyWatchListRow.CompanyName)
        Next
    End Sub

End Class