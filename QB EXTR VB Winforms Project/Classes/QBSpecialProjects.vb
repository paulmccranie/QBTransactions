Imports System.Xml
Imports System.IO

Public Class QBSpecialProjects
    Implements IDisposable
    Public Property CustomerName As String
    Private Property qbSearcher As New nsoftware.IBizQB.Objsearch
    Public Property Errors As Boolean = False
    Public Property DepositBalance As Decimal
    Public Successful As Boolean = False
    Public AsOfDate As Date = Date.Parse("1/1/2014")
    Dim taTransactionRecord As New CSIAutomationDataSetTableAdapters.QBTransactionTableAdapter

    Public Sub UpdateAllInvoicesSetToBePrintedToTrue(custName As String)
        CustomerName = custName
        InitializeQBSearcher()
        GetInvoiceTransactions()
    End Sub

    Private Function InitializeQBSearcher() As nsoftware.IBizQB.Objsearch
        Dim Searcher1 As New nsoftware.IBizQB.Objsearch
        Try
            KillQuickbooks()
            TheCurrentQBFile = "Q:\Cornerstone.qbw"
            Searcher1.QBXMLVersion = "6.0"
            Searcher1.QBConnectionString = " CompanyFile= """ + TheCurrentQBFile + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " User= """ + My.Settings.ThisApplicationName + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " Password= """ + "billing" + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " ApplicationName= """ + My.Settings.ThisApplicationName + """"
            Searcher1.QBConnectionMode = nsoftware.IBizQB.CustomerQBConnectionModes.cmDontCare
            Searcher1.TransactionDateStart = Date.Parse("1/1/2014")
            Searcher1.TransactionDateEnd = Date.Parse("1/1/2015")
            'Searcher1.ModifiedDateStart = My.Settings.StaticBalanceDate.AddDays(-1)
            'Searcher1.ModifiedDateEnd = Today.AddDays(1)
            Searcher1.EntityName = CustomerName
            Searcher1.OpenQBConnection()

        Catch ex As Exception
            Return Nothing
        End Try
        Return Searcher1

    End Function

    Private Sub GetInvoiceTransactions()
        Try
            Me.qbSearcher.QueryType = nsoftware.IBizQB.ObjsearchQueryTypes.qtInvoiceSearch
            'Do the Search
            Try
                Me.qbSearcher.Search()
            Catch ex As Exception

            Finally
                'KillQuickbooks()
            End Try
            Dim invoice As New nsoftware.IBizQB.Invoice
            For i As Integer = 0 To Me.qbSearcher.ResultCount - 1
                invoice.QBResponseAggregate = qbSearcher.ResultAggregate(i)
                invoice.IsToBePrinted = True
                invoice.Update()
            Next

        Catch ex As Exception
            Me.Errors = True
        End Try
    End Sub


    Public Sub Dispose() Implements System.IDisposable.Dispose
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
    End Sub
End Class
