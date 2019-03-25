Imports System.Xml
Imports System.Xml.Serialization
Imports System.IO
Imports System.Text
Imports System.Threading



Module CustomerDeposit
    Dim DepositTotals As New Decimal
    Dim theCustomerName As String 'customer Name
    Dim taTransactionRecord As New CSIAutomationDataSetTableAdapters.QBTransactionTableAdapter
    Dim tempCompanyListToAudit As New List(Of String)

    'use this bit to exit out quickly if the customer has no invoices with "customer Deposit" transactions
    Dim HasCustomerDepositTransactions As Boolean = False


    Private Sub UpdateCustomerDepositList()
        Try
            Dim taCust As New CSIAutomationDataSetTableAdapters.QBCustomerNamesTableAdapter
            'First - clear all existing transactions!
            'taTransactionRecord.DeleteAllTransactions()

            'for the purpose of the update timer on the customerdepositWorksheet form
            'taTransactionRecord.Insert("Update", "Update", Today, "Update", "Update", 0.0, "Update")

            'GetCustomerListFromQB
            Dim custList As New List(Of String)
            custList = GetCustomerWithCustDepositNames(TheCurrentQBFile)

            'InsertNewCustomers
            For Each cName As String In custList
                theCustomerName = cName
                taTransactionRecord.UpdateCompanyBeingUpdated(theCustomerName)
                DepositTotals = 0
                Try
                    If taCust.GetCountByCustName(theCustomerName) = 0 Then
                        'add it
                        Dim depAmount = GetCustomerDepositAmount(theCustomerName)
                        Dim taTrans As New CSIAutomationDataSetTableAdapters.QBTransactionTableAdapter
                        taCust.Insert(theCustomerName, taTrans.GetDepositTotal(theCustomerName), Today)
                        Try
                            'Main.UpdateStatusTextBox("Updating New Customer with Cust Dep. Balance: " & theCustomerName & " with a balance of " & depAmount, 'Main.ParagraphPos.Middle)
                        Catch ex As Exception

                        End Try
                    Else
                        'the customer exists - just update the Customer Deposit amount
                        Dim theCustDepAmount = GetCustomerDepositAmount(theCustomerName)
                        'Update the database
                        Dim ta As New CSIAutomationDataSetTableAdapters.QBCustomerNamesTableAdapter
                        Dim taTrans As New CSIAutomationDataSetTableAdapters.QBTransactionTableAdapter
                        ta.UpdateDepositAmountWithDate(taTrans.GetDepositTotal(theCustomerName), theCustomerName)

                        Try
                            'Main.UpdateStatusTextBox("Updating: " & theCustomerName & " with a balance of " & taTrans.GetDepositTotal(theCustomerName), 'Main.ParagraphPos.Middle)
                        Catch ex As Exception

                        End Try
                    End If
                Catch ex As Exception
                End Try
            Next
        Catch ex As Exception
        End Try
        taTransactionRecord.NotUpdating()
        KillQuickbooks()

    End Sub



    'Need to be able to match up Compass customers with QB customers

    Public Function GetCustomerDepositAmount(ByVal theCustomerName As String) As Decimal
        Try
            'Delete The Current Transactions for the Company
            taTransactionRecord.DeleteAllByCompanyName(theCustomerName)

            TheCurrentQBFile = "Q:\Cornerstone.qbw"

            KillQuickbooks()
            ''Main.UpdateStatusTextBox("Getting the CUSTOMER DEPOSIT BALANCE for " & theCustomerName, ParagraphPos.Middle)
            Dim Searcher1 As New nsoftware.IBizQB.Objsearch
            Searcher1.QBXMLVersion = "6.0"
            Searcher1.QBConnectionString = " CompanyFile= """ + TheCurrentQBFile + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " User= """ + My.Settings.ThisApplicationName + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " Password= """ + "billing" + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " ApplicationName= """ + My.Settings.ThisApplicationName + """"
            Searcher1.QBConnectionMode = nsoftware.IBizQB.CustomerQBConnectionModes.cmDontCare
            Searcher1.QueryType = nsoftware.IBizQB.ObjsearchQueryTypes.qtInvoiceSearch
            Searcher1.EntityName = theCustomerName
            Searcher1.TransactionDateStart = "1/1/2014"

            'Search Invoices
            Try
                Searcher1.Search()
                'Searcher1.CloseQBConnection()
            Catch ex As Exception

            Finally
                KillQuickbooks()
            End Try



            '  KillQuickbooks()
            ''Main.UpdateStatusTextBox("Retrieving Invoices for " & theCustomerName & ". There are " & Searcher1.ResultCount.ToString, ParagraphPos.Middle)

            For i As Integer = 0 To Searcher1.ResultCount - 1

                Dim myAccount As New nsoftware.IBizQB.Invoice
                myAccount.QBResponseAggregate = Searcher1.ResultAggregate(i)
                DoInvoices(myAccount.QBResponseAggregate.ToString)

            Next


            'Search Journal Entries
            Searcher1.QueryType = nsoftware.IBizQB.ObjsearchQueryTypes.qtJournalEntrySearch
            Searcher1.EntityName = theCustomerName

            Try
                Searcher1.Search()
                'Searcher1.CloseQBConnection()
            Catch ex As Exception

            Finally
                KillQuickbooks()
            End Try

            ' 'Main.UpdateStatusTextBox("Retrieving Journal Entries for " & theCustomerName & ". There are " & Searcher1.ResultCount.ToString, ParagraphPos.Middle)

            For i As Integer = 0 To Searcher1.ResultCount - 1
                tryParseJournalEntries(Searcher1.ResultAggregate(i).ToString, theCustomerName)
            Next



            'Search Checks
            Searcher1.QueryType = nsoftware.IBizQB.ObjsearchQueryTypes.qtCheckSearch
            Searcher1.EntityName = theCustomerName

            Try
                Searcher1.Search()
                ' Searcher1.CloseQBConnection()
            Catch ex As Exception

            Finally
                KillQuickbooks()
            End Try

            ''Main.UpdateStatusTextBox("Retrieving Checks for " & theCustomerName & ". There are " & Searcher1.ResultCount.ToString, ParagraphPos.Middle)
            'C:\Users\pmccranie\Desktop\Base VS Projects\4) Compass Automator\CompassAutomator\Classes\ExcelExporter.vb
            For i As Integer = 0 To Searcher1.ResultCount - 1
                tryParseChecks(Searcher1.ResultAggregate(i).ToString, theCustomerName)
            Next



            'Search Deposits

            Searcher1.QueryType = nsoftware.IBizQB.ObjsearchQueryTypes.qtDepositSearch
            Searcher1.EntityName = theCustomerName
            Try
                Searcher1.Search()
                ' Searcher1.CloseQBConnection()
            Catch ex As Exception

            Finally
                KillQuickbooks()
            End Try
            ' 'Main.UpdateStatusTextBox("Retrieving Deposits for " & theCustomerName & ". There are " & Searcher1.ResultCount.ToString, ParagraphPos.Middle)

            For i As Integer = 0 To Searcher1.ResultCount - 1
                tryParseDeposits(Searcher1.ResultAggregate(i).ToString, theCustomerName)
            Next


            'Searcher1.CloseQBConnection()

            ' 'Main.UpdateStatusTextBox("Retrieving Total for " & theCustomerName & ". It is:  " & DepositTotals.ToString, ParagraphPos.Middle)
            Return DepositTotals

            KillQuickbooks()

        Catch ex As Exception

        Finally

        End Try

    End Function

    Private Sub tryParseChecks(ByVal theString As String, ByVal theCustomerName As String)
        Try
            Dim dateTransaction As Date
            Dim transactionNumber As String
            Dim xmlDoc As New XmlDocument
            Dim dataNodeList As XmlNodeList
            Dim dataNode As XmlNode
            Dim je As New JournalEntryClass


            je.Company = theCustomerName
            je.Account = ""
            je.Amount = ""
            je.DebitCredit = "Neither"
            je.IsCustomerDepositAccount = False
            je.ListID = ""
            je.Memo = ""

            xmlDoc.LoadXml(theString)

            Dim TimeNode As XmlNodeList = xmlDoc.GetElementsByTagName("TimeCreated")
            Dim detailTimeNode As XmlNode = TimeNode.Item(0)
            dateTransaction = detailTimeNode.InnerText

            Dim transactionNode As XmlNodeList = xmlDoc.GetElementsByTagName("TxnNumber")
            Dim detailtransactionNode As XmlNode = transactionNode.Item(0)
            transactionNumber = detailtransactionNode.InnerText


            dataNodeList = xmlDoc.GetElementsByTagName("Amount")
            je.Amount = dataNodeList.Item(0).InnerText

            dataNodeList = xmlDoc.GetElementsByTagName("ExpenseLineRet")
            dataNode = dataNodeList.Item(0)
            Dim childDataNode As XmlNodeList = dataNode.ChildNodes
            For Each childDetailDataNode As XmlNode In childDataNode
                Select Case childDetailDataNode.Name
                    Case "AccountRef"
                        Dim accountNodes As XmlNodeList = childDetailDataNode.ChildNodes
                        For Each accountNode As XmlNode In accountNodes
                            Select Case accountNode.Name
                                Case "FullName"
                                    je.Account = accountNode.InnerText
                            End Select
                        Next

                End Select
            Next


            If je.Account.Contains("Customer Deposit") Then
                DepositTotals -= je.Amount
                taTransactionRecord.Insert(je.Company, "Check", dateTransaction, transactionNumber, je.Memo, je.Amount * -1, theString,"","")
            End If

        Catch ex As Exception
        End Try

    End Sub





    Private Sub tryParseDeposits(ByVal theString As String, ByVal theCustomerName As String)
        Try
            Dim dateTransaction As Date
            Dim transactionNumber As String
            Dim xmlDoc As New XmlDocument
            Dim dataNodeList As XmlNodeList
            Dim je As New JournalEntryClass
            Dim checkList As New List(Of JournalEntryClass)

            xmlDoc.LoadXml(theString)

            Dim TimeNode As XmlNodeList = xmlDoc.GetElementsByTagName("TimeCreated")
            Dim detailTimeNode As XmlNode = TimeNode.Item(0)
            dateTransaction = detailTimeNode.InnerText

            Dim transactionNode As XmlNodeList = xmlDoc.GetElementsByTagName("TxnNumber")
            Dim detailtransactionNode As XmlNode = transactionNode.Item(0)
            transactionNumber = detailtransactionNode.InnerText

            'Get a list of deposits
            dataNodeList = xmlDoc.GetElementsByTagName("DepositLineRet")

            'Create a je for each deposit, we'll filter for company name before we apply
            For Each depositNode As XmlNode In dataNodeList
                je = New JournalEntryClass
                je.Company = theCustomerName
                je.Account = ""
                je.Amount = ""
                je.DebitCredit = "Neither"
                je.IsCustomerDepositAccount = False
                je.ListID = ""
                je.Memo = ""

                'Process all children
                Dim depositChildNodes As XmlNodeList = depositNode.ChildNodes
                For Each depositChildNode As XmlNode In depositChildNodes
                    Select Case depositChildNode.Name
                        Case "EntityRef"
                            Dim EntityNode As XmlNodeList = depositChildNode.ChildNodes
                            For Each EntityDetailNode As XmlNode In EntityNode
                                Select Case EntityDetailNode.Name
                                    Case "FullName"
                                        je.Company = EntityDetailNode.InnerText
                                End Select
                            Next
                        Case "AccountRef"
                            Dim accountNodes As XmlNodeList = depositChildNode.ChildNodes
                            For Each accountNode As XmlNode In accountNodes
                                Select Case accountNode.Name
                                    Case "FullName"
                                        je.Account = accountNode.InnerText
                                End Select
                            Next
                        Case "Amount"
                            je.Amount = depositChildNode.InnerText
                    End Select
                Next
                checkList.Add(je)
            Next


            For Each theJE As JournalEntryClass In checkList
                If theJE.Company = theCustomerName And theJE.Account.Contains("Customer Deposit") Then
                    DepositTotals += theJE.Amount
                    taTransactionRecord.Insert(theCustomerName, "Deposit", dateTransaction, transactionNumber, theJE.Memo, theJE.Amount, theString,"","")
                End If
            Next

        Catch ex As Exception
            ' MsgBox("Deposits Routine: " & ex.Message)
        End Try


    End Sub

    Private Sub DoInvoices(ByVal theString As String)
        Try
            Dim xmlDoc As New XmlDocument
            Dim InvoiceNodes As XmlNodeList
            Dim baseInvoiceNode As XmlNode
            Dim dateTransaction As Date
            Dim transactionNumber As String
            Dim theCompanyName As String = ""
            Dim je As JournalEntryClass
            Dim invoiceList As New List(Of JournalEntryClass)

            Using reader As XmlReader = XmlReader.Create(New StringReader(theString))
                reader.ReadToFollowing("TimeCreated")
                dateTransaction = reader.ReadElementContentAsDateTime()
                reader.ReadToFollowing("TxnNumber")
                transactionNumber = reader.ReadElementContentAsString()
                reader.Close()
            End Using

            xmlDoc.LoadXml(theString)

            'Get the Company Name
            Dim CompanyNode As XmlNodeList = xmlDoc.GetElementsByTagName("CustomerRef")
            For Each theCompanyNode As XmlNode In CompanyNode
                For Each companyChildNode In theCompanyNode.ChildNodes
                    Select Case companyChildNode.Name
                        Case "FullName"
                            theCompanyName = companyChildNode.InnerText
                    End Select
                Next

            Next

            InvoiceNodes = xmlDoc.GetElementsByTagName("InvoiceLineRet")


            For Each baseInvoiceNode In InvoiceNodes
                je = New JournalEntryClass
                je.Company = theCompanyName
                je.DebitCredit = "Neither"
                je.ListID = ""
                je.Account = ""
                je.Amount = ""
                je.Memo = ""
                je.IsCustomerDepositAccount = False




                For Each childNode As XmlNode In baseInvoiceNode
                    Select Case childNode.Name
                        Case "Desc"
                            je.Memo += childNode.InnerText + System.Environment.NewLine
                        Case "Amount"
                            je.Amount = childNode.InnerText
                        Case "ItemRef"
                            For Each childInvoiceNode As XmlNode In childNode
                                Select Case childInvoiceNode.Name
                                    Case "FullName"
                                        je.Account = childInvoiceNode.InnerText
                                End Select
                            Next
                    End Select
                Next
                invoiceList.Add(je)

            Next

            For Each theJe As JournalEntryClass In invoiceList
                If theJe.Account.Contains("Customer Deposit") Then
                    HasCustomerDepositTransactions = True
                    DepositTotals += theJe.Amount
                    taTransactionRecord.Insert(theJe.Company, "Invoice", dateTransaction, transactionNumber, theJe.Memo, theJe.Amount, theString,"","")
                End If

            Next
            xmlDoc = Nothing

        Catch ex As Exception
              MsgBox("Invoice Routine: " & ex.Message)

        End Try


    End Sub


    Private Sub tryParseJournalEntries(ByVal theString As String, ByVal theCustomerName As String)
        Try
            Dim theJournalEntries As New List(Of JournalEntryClass)
            Dim theTransactionDate As String = ""
            Dim theTransactionNumber As String = ""

            Dim xmlDoc As New XmlDocument
            Dim DebitEntries As XmlNodeList
            Dim CreditEntries As XmlNodeList

            Dim DebitEntry As XmlNode
            Dim CreditEntry As XmlNode

            Dim baseDebitEntries As XmlNodeList
            Dim baseCreditEntries As XmlNodeList

            Dim baseEntryNode As XmlNode
            Dim theCompanyName As String = ""
            Dim jeCredit As New JournalEntryClass
            Dim je As New JournalEntryClass

            Dim validCreditEntry As Boolean
            Dim validDebitEntry As Boolean

            'Get the basic info first
            Dim wrapperReader As XmlReader = XmlReader.Create(New StringReader(theString))
            Try
                wrapperReader.ReadToFollowing("TimeCreated")
                theTransactionDate = wrapperReader.ReadElementContentAsString()
                wrapperReader.ReadToFollowing("TxnNumber")
                theTransactionNumber = wrapperReader.ReadElementContentAsString
            Catch ex As Exception

            End Try
            Try
                wrapperReader.ReadToFollowing("RefNumber")
                theTransactionNumber = wrapperReader.ReadElementContentAsString()

            Catch ex As Exception

            End Try
            wrapperReader.Close()


            xmlDoc.LoadXml(theString)

            DebitEntries = xmlDoc.GetElementsByTagName("JournalDebitLine")
            CreditEntries = xmlDoc.GetElementsByTagName("JournalCreditLine")

            Dim theEntryNumber As Integer = 0
            Dim theNumberOfEntriesToProcess = DebitEntries.Count
            If CreditEntries.Count > theNumberOfEntriesToProcess Then theNumberOfEntriesToProcess = CreditEntries.Count

            While theEntryNumber < theNumberOfEntriesToProcess
                Try
                    DebitEntry = DebitEntries.Item(theEntryNumber)
                    validDebitEntry = True
                    If theEntryNumber >= DebitEntries.Count Then
                        validDebitEntry = False
                    End If
                Catch ex As Exception
                    validDebitEntry = False
                End Try

                Try
                    CreditEntry = CreditEntries.Item(theEntryNumber)
                    validCreditEntry = True
                    If theEntryNumber >= CreditEntries.Count Then
                        validCreditEntry = False
                    End If
                Catch ex As Exception
                    validCreditEntry = False
                End Try

                If validCreditEntry Then
                    jeCredit = New JournalEntryClass
                    jeCredit.Company = theCustomerName
                    jeCredit.DebitCredit = "Credit"
                    jeCredit.Memo = ""

                    baseCreditEntries = CreditEntry.ChildNodes
                    For Each baseEntryNode In baseCreditEntries

                        Select Case baseEntryNode.Name
                            Case "AccountRef"
                                Dim accounRefDetails As XmlNodeList = baseEntryNode.ChildNodes
                                For Each detailNode As XmlNode In accounRefDetails
                                    Select Case detailNode.Name
                                        Case "FullName"
                                            jeCredit.Account = detailNode.InnerText
                                        Case Else
                                    End Select
                                Next
                            Case "EntityRef"
                                Dim entityRefDetails As XmlNodeList = baseEntryNode.ChildNodes
                                For Each detailNode As XmlNode In entityRefDetails
                                    Select Case detailNode.Name
                                        Case "FullName"
                                            jeCredit.Company = detailNode.InnerText
                                            theCompanyName = jeCredit.Company
                                    End Select
                                Next

                            Case "Memo"
                                jeCredit.Memo = baseEntryNode.InnerText
                            Case "Amount"
                                jeCredit.Amount = baseEntryNode.InnerText
                            Case Else
                        End Select

                    Next
                    theJournalEntries.Add(jeCredit)
                End If

                If validDebitEntry Then
                    je = New JournalEntryClass
                    je.DebitCredit = "Debit"
                    je.Company = theCompanyName

                    je.Memo = ""

                    baseDebitEntries = DebitEntry.ChildNodes
                    For Each baseEntryNode In baseDebitEntries
                        Select Case baseEntryNode.Name
                            Case "AccountRef"
                                Dim accounRefDetails As XmlNodeList = baseEntryNode.ChildNodes
                                For Each detailNode As XmlNode In accounRefDetails
                                    Select Case detailNode.Name
                                        Case "FullName"
                                            je.Account = detailNode.InnerText
                                        Case "ListID"
                                            je.ListID = detailNode.InnerText
                                    End Select
                                Next
                            Case "EntityRef"
                                Dim entityRefDetails As XmlNodeList = baseEntryNode.ChildNodes
                                For Each detailNode As XmlNode In entityRefDetails
                                    Select Case detailNode.Name
                                        Case "FullName"
                                            je.Company = detailNode.InnerText
                                            theCompanyName = je.Company
                                            If jeCredit.Company = "" Then jeCredit.Company = theCompanyName
                                    End Select
                                Next
                            Case "Memo"
                                je.Memo = baseEntryNode.InnerText
                            Case "Amount"
                                je.Amount = baseEntryNode.InnerText
                            Case Else
                        End Select

                    Next
                    theJournalEntries.Add(je)
                End If


                theEntryNumber += 1
            End While

            'Now - I have two lists - DebitEntries and CreditEntries
            ' process them 

            'Processing journal entries
            For Each de As JournalEntryClass In theJournalEntries
                If de.Account.Contains("Customer Deposit") Then
                    If de.DebitCredit = "Debit" Then
                        'see if the transaction already exists
                        ' If taTransactionRecord.DoesTransactionExist(de.Company, "Journal", theTransactionDate, de.Memo, theTransactionNumber, de.Amount * -1) = 0 Then
                        If de.Company = theCustomerName Then
                            DepositTotals -= de.Amount
                            taTransactionRecord.Insert(de.Company, "Journal", theTransactionDate, theTransactionNumber, de.Memo, de.Amount * -1, theString,"","")
                        End If

                    Else
                        'see if the transaction already exists
                        'If taTransactionRecord.DoesTransactionExist(de.Company, "Journal", theTransactionDate, de.Memo, theTransactionNumber, de.Amount) = 0 Then
                        If de.Company = theCustomerName Then
                            DepositTotals += de.Amount
                            taTransactionRecord.Insert(de.Company, "Journal", theTransactionDate, theTransactionNumber, de.Memo, de.Amount, theString,"","")
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            ' MsgBox("JE Routine: " & ex.Message)

        End Try

    End Sub


    Public Class JournalEntryClass
        Public Property Company As String
        Public Property Account As String
        Public Property Amount As String
        Public Property DebitCredit As String
        Public Property IsCustomerDepositAccount As Boolean
        Public Property Memo As String
        Public Property ListID As String

    End Class

    Public Function GetCompanyNameConflicts() As DataSet
        Dim theCustTable As New DataTable("CustConflicts")

        Dim Customers As nsoftware.IBizQB.Objsearch
        Customers = GetCustomers(TheCurrentQBFile)
        Dim cust As New nsoftware.IBizQB.Customer

        Dim dcClientName As System.Data.DataColumn

        dcClientName = New System.Data.DataColumn("CompanyName", Type.GetType("System.String"))
        theCustTable.Columns.Add(dcClientName)

        dcClientName = New System.Data.DataColumn("CustomerName", Type.GetType("System.String"))
        theCustTable.Columns.Add(dcClientName)


        For i As Integer = 0 To Customers.ResultCount - 1
            cust.QBResponseAggregate = Customers.ResultAggregate(i)
            If Len(cust.CompanyName) > 0 And Len(cust.CustomerName) > 0 Then
                If cust.CompanyName <> cust.CustomerName Then
                    'MsgBox(cust.CompanyName & " does not match " & cust.CustomerName)
                    Dim nr As DataRow
                    nr = theCustTable.NewRow
                    nr.Item(0) = cust.CompanyName
                    nr.Item(1) = cust.CustomerName
                    theCustTable.Rows.Add(nr)

                End If
            End If

        Next

        Dim ds As New DataSet
        ds.Merge(theCustTable)
        Return ds
    End Function

End Module
