Public Class QBCustomerList
    Public Property TheQBLocation As String
    Public Property CustomerList As List(Of String)

    Public Sub CreateCustomerList()

        Dim Searcher1 As New nsoftware.IBizQB.Objsearch
        Try
            Searcher1.QBXMLVersion = "6.0"
            Searcher1.QBConnectionString = " CompanyFile= """ + TheQBLocation + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " User= """ + my.settings.ThisApplicationName + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " Password= """ + "billing" + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " ApplicationName= """ + my.settings.ThisApplicationName + """"
            Searcher1.QBConnectionMode = nsoftware.IBizQB.CustomerQBConnectionModes.cmDontCare

            Searcher1.QueryType = nsoftware.IBizQB.ObjsearchQueryTypes.qtCustomerSearch
            

            'Do the search
            Searcher1.Search()

            'now create the customer list
            For i = 0 To Searcher1.ResultCount - 1
                Dim qbc As New nsoftware.IBizQB.Customer
                qbc.QBResponseAggregate = Searcher1.ResultAggregate(i)
                CustomerList.Add(qbc.CustomerName)
            Next

            Searcher1 = Nothing

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub


    Public Sub WriteCustomerListToDatabase()
        Dim ta As New CSIAutomationDataSetTableAdapters.QBCustomerNamesTableAdapter

        For Each cName As String In CustomerList
            Dim depAmount = ta.GetCustomerDepositAmount(cName)
            ta.Insert(cName, depAmount, Today)
        Next
    End Sub

End Class
