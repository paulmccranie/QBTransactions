Imports QBTransactions
Imports QBTransactions.ViewModels
Imports QBTransLibrary2019

Public Class QBTransactionReportingViewModel : Implements IDisposable

    Dim TransactionQBCustomerBalance As New QBCustomerDeposit

    Dim staticBalance As CustomerDepositBalanceStatic
    Dim taQBCustomerName As New CSIAutomationDataSetTableAdapters.QBCustomerNamesTableAdapter
    Dim dtQBCustomerName As New CSIAutomationDataSet.QBCustomerNamesDataTable
    Dim rowQBCustomerName As CSIAutomationDataSet.QBCustomerNamesRow
    Dim taQBTransaction As New CSIAutomationDataSetTableAdapters.QBTransactionTableAdapter
    Dim taWatchList As New CSIAutomationDataSetTableAdapters.CompanyWatchListTableAdapter


    Public Function GetCustomersWithCustomerDepositBalances() As List(Of String)
        Dim customerNameList As New List(Of String)
        Try
            TheCurrentQBFile = "Q:\Cornerstone.qbw"
            Dim Searcher1 = CreateObjSearcher(nsoftware.IBizQB.ObjsearchQueryTypes.qtCustomerSearch, "", TheCurrentQBFile, 0, "", "")
            Dim customerCount As Integer

            customerCount = Searcher1.ResultCount

            For i As Integer = 0 To Searcher1.ResultCount - 1
                Dim theCustomerName As New nsoftware.IBizQB.Customer
                theCustomerName.QBResponseAggregate = Searcher1.ResultAggregate(i)
                If theCustomerName.CustomerTypeName = "Customer Deposit" Then
                    If theCustomerName.CompanyName = "" Then
                        customerNameList.Add(theCustomerName.CustomerName)
                    Else
                        If theCustomerName.CustomerName <> "" Then
                            customerNameList.Add(theCustomerName.CustomerName)
                        Else
                            customerNameList.Add(theCustomerName.CompanyName)
                        End If
                    End If
                End If
            Next

        Catch ex As Exception
            Return Nothing
        End Try
        Return customerNameList
    End Function

    Public Function GetWatchList() As List(Of String)
        Try

            Dim dtWatchList As New CSIAutomationDataSet.CompanyWatchListDataTable
            taWatchList.GetActiveWatchList(dtWatchList)
            Dim customerList As New List(Of String)
            For Each row As CSIAutomationDataSet.CompanyWatchListRow In dtWatchList.Rows
                customerList.Add(row.Item("CompanyName"))
            Next
            Return customerList
        Catch ex As Exception

        End Try
        Return Nothing
    End Function

    Public Function GetStaticBalanceDate(custName As String) As String
        Try
            staticBalance = TransactionQBCustomerBalance.GetStaticDateAndBalance(custName)
            If staticBalance Is Nothing Then Return ""
            Return staticBalance.AsOfDate.ToShortDateString()
        Catch ex As Exception
            Return ""
        End Try


    End Function

    Public Function GetStaticBalance(custName As String) As String
        Try
            staticBalance = TransactionQBCustomerBalance.GetStaticDateAndBalance(custName)
            If staticBalance Is Nothing Then Return ""
            Return staticBalance.Balance.ToString()
        Catch ex As Exception
            Return ""
        End Try

    End Function

    Public Function GetDateLastPull(custName As String) As String
        Try
            Return TransactionQBCustomerBalance.GetDateOfLastPull(custName)
        Catch ex As Exception
            Return ""
        End Try

    End Function

    Public Function GetActualCustomerBalance(ByVal customer As String) As String
        Try
            Return taQBCustomerName.GetCustomerDepositAmount(customer).ToString()
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function GetActualDateCustomerBalance(ByVal customer As String) As String
        Try
            Return taQBCustomerName.GetDateOfLastCustDepUpdateByCustName(customer).ToString()
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Sub GetQBTransactionsByCustomer(custName As String)

        Using qbCust As New qbCustomer(custName)
            qbCust.GetCustomerDepositBalance()
        End Using



        KillQuickbooks()
        Dim staticBalance As Decimal = 0.0
        Try
            staticBalance = GetStaticBalance(custName)
        Catch ex As Exception
            staticBalance = 0
        End Try

        Dim custBalance As Decimal = 0.0
        Try
            custBalance = taQBTransaction.GetDepositTotal(custName)
        Catch ex As Exception
            custBalance = 0.0
        End Try

        Try
            AddQBTransactionPull(custName, custBalance + staticBalance, GetNumberOfTransactionsForCustomer(custName))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Function GetNumberOfTransactionsForCustomer(custName As String) As Integer
        Return taQBTransaction.GetNumberOfTransactionsForCustomer(custName)
    End Function

    Public Function GetCalculatedBalanceBasedOnLastTransactionPull(custName As String) As Decimal
        Try
            Return taQBTransaction.GetDepositTotal(custName) + GetStaticBalance(custName)
        Catch ex As Exception
            Return 0.0
        End Try

    End Function

    Public Function GetLastDateQBTransactionsWerePulled(custName As String) As String
        Try
            Return taQBTransaction.DateLastTransaction(custName).ToString()
        Catch ex As Exception
            Return ""
        End Try

    End Function

    Public Sub RefreshQBTransactions(custName As String)
        Dim qbCust As qbCustomer
        qbCust = New qbCustomer(custName)
        qbCust.GetCustomerDepositBalance()
    End Sub

    Public Sub CreateExcelReport(ByVal custName As String, ByVal AsOfDate As String)
        Try
            Dim excelReporter As New QBCustomerDeposit
            If AsOfDate = "" Then AsOfDate = "1/1/2015"
            excelReporter.CreateExcelReport(custName, AsOfDate)
        Catch ex As Exception

        End Try

    End Sub

    Public Sub AddQBTransactionPull(custName As String, theBalance As Decimal, numberOfTransactions As Integer)
        Try
            Dim taPull As New CSIAutomationDataSetTableAdapters.QBTransactionPullTableAdapter

            'Delete any old one
            taPull.DeleteByCustomer(custName)
            'Add the new one
            taPull.Insert(custName, Today, theBalance, numberOfTransactions)

            taPull.Dispose()
        Catch ex As Exception

        End Try

    End Sub

    Public Function GetQBTransactionPullData(custName As String) As CSIAutomationDataSet.QBTransactionPullRow
        Try
            Dim taPull As New CSIAutomationDataSetTableAdapters.QBTransactionPullTableAdapter
            Dim dtPull As New CSIAutomationDataSet.QBTransactionPullDataTable
            Try
                taPull.GetPullDataByCustomer(dtPull, custName)
                Return dtPull.Item(0)
            Catch ex As Exception
                Return Nothing
            End Try
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    Public Function GetBalanceAsOfCertainDay(companyName As String, asofDate As Date) As Decimal
        Dim taQBT As New CSIAutomationDataSetTableAdapters.QBTransactionTableAdapter

        If asofDate.AddDays(-1) < My.Settings.StaticBalanceDate Then
            asofDate = My.Settings.StaticBalanceDate
        Else
            asofDate = asofDate.AddDays(-1)
        End If

        Try
            Dim StaticBalance As String = GetStaticBalance(companyName)
            If StaticBalance = "" Then
                Return CDec(taQBT.GetBalanceForDatePeriod(companyName, My.Settings.StaticBalanceDate.AddDays(-1), asofDate.AddDays(-1)))
            Else
                Return CDec(StaticBalance + taQBT.GetBalanceForDatePeriod(companyName, My.Settings.StaticBalanceDate.AddDays(-1), asofDate.AddDays(-1)))
            End If
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
        Return 0
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
