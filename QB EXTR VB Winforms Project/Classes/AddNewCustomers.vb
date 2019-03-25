Module AddNewCustomers

    Dim taQBcust As New CSIAutomationDataSetTableAdapters.QBCustomerNamesTableAdapter

    Public Sub AddAnyNewCustomersToDatabase()
        Dim customerList As New List(Of String)
        customerList = GetCustomerNames(TheCurrentQBFile)

        For Each cName As String In customerList
            If taQBcust.GetCountByCustName(cName) = 0 Then
                'Main.UpdateStatusTextBox("Adding New Customer: " & cName, 'Main.ParagraphPos.Middle)
                Dim qbCustEntry As New qbCustomer(cName)
                qbCustEntry.GetCustomerDepositBalance()
                taQBcust.Insert(cName, qbCustEntry.DepositBalance, Now)
            End If
        Next
        KillQuickbooks()

    End Sub
End Module
