Imports System.Xml
Imports System.IO

Public Class qbCustomer
    Implements IDisposable
    Public Property CustomerName As String
    Private Property qbSearcher As New nsoftware.IBizQB.Objsearch
    Public Property Errors As Boolean = False
    Public Property DepositBalance As Decimal
    Public Successful As Boolean = False
    Public AsOfDate As Date = Date.Parse("1/1/2014")
    Dim taTransactionRecord As New CSIAutomationDataSetTableAdapters.QBTransactionTableAdapter


    Public Sub New(custName As String)
        CustomerName = custName
    End Sub

    Public Sub SetupQBCustomer(theCustName As String)
        CustomerName = theCustName
    End Sub


    Public Sub GetCustomerDepositBalance()
        Try
            'Delete The Current Transactions for the Company
            taTransactionRecord.DeleteAllByCompanyName(CustomerName)

            'QB Transaction Commands
            Me.qbSearcher = InitializeQBSearcher()
            If qbSearcher Is Nothing Then
                Successful = False
            End If

            Me.GetInvoiceTransactions()

            ' qbSearcher = New nsoftware.IBizQB.Objsearch
            ' qbSearcher = InitializeQBSearcher()

            Me.GetJournalEntryTransactions()

            'qbSearcher = New nsoftware.IBizQB.Objsearch
            'qbSearcher = InitializeQBSearcher()

            Me.GetDepositTransactions()

            'qbSearcher = New nsoftware.IBizQB.Objsearch
            'qbSearcher = InitializeQBSearcher()

            Me.GetCheckTransactions()

            ' qbSearcher = New nsoftware.IBizQB.Objsearch
            'qbSearcher = InitializeQBSearcher()

            Me.GetCreditMemoTransactions()

            'Get rid of dupes
            taTransactionRecord.DeleteDupes()
            Successful = True

        Catch ex As Exception
            Successful = False
        End Try

    End Sub



#Region "Transaction Commands"


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
            Searcher1.TransactionDateStart = My.Settings.StaticBalanceDate.AddDays(-1)
            Searcher1.TransactionDateEnd = Today.AddDays(1)
            'Searcher1.ModifiedDateStart = My.Settings.StaticBalanceDate.AddDays(-1)
            'Searcher1.ModifiedDateEnd = Today.AddDays(1)
            Searcher1.EntityName = CustomerName
            Searcher1.OpenQBConnection()

        Catch ex As Exception
            Return Nothing
        End Try
        Return Searcher1

    End Function

    'Invoices
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
            Dim myAccount As New nsoftware.IBizQB.Invoice
            For i As Integer = 0 To Me.qbSearcher.ResultCount - 1

                myAccount.QBResponseAggregate = Me.qbSearcher.ResultAggregate(i)
                DoInvoices(myAccount.QBResponseAggregate.ToString)
            Next
            myAccount = Nothing

            'If DialogsOK Then MsgBox("Invoicing OK")
        Catch ex As Exception
            Me.Errors = True
        End Try
    End Sub

    Private Sub GetJournalEntryTransactions()
        Try
            Me.qbSearcher.QueryType = nsoftware.IBizQB.ObjsearchQueryTypes.qtJournalEntrySearch
            'Do the Search
            Try
                Me.qbSearcher.Search()
            Catch ex As Exception

            Finally
                'KillQuickbooks()
            End Try

            For i As Integer = 0 To Me.qbSearcher.ResultCount - 1
                Dim myAccount As New nsoftware.IBizQB.Journalentry
                myAccount.QBResponseAggregate = Me.qbSearcher.ResultAggregate(i)
                DoJournalEntries(myAccount.QBResponseAggregate.ToString)
            Next
        Catch ex As Exception
            Errors = True
        End Try
    End Sub

    Private Sub GetDepositTransactions()
        Try
            Me.qbSearcher.QueryType = nsoftware.IBizQB.ObjsearchQueryTypes.qtDepositSearch

            'Do the Search
            Try
                Me.qbSearcher.Search()
            Catch ex As Exception

            Finally
                'KillQuickbooks()
            End Try

            For i As Integer = 0 To Me.qbSearcher.ResultCount - 1
                Dim myAccount As New nsoftware.IBizQB.Deposit
                myAccount.QBResponseAggregate = Me.qbSearcher.ResultAggregate(i)
                DoDeposits(myAccount.QBResponseAggregate.ToString)
            Next
        Catch ex As Exception
            Errors = True
        End Try
    End Sub

    Private Sub GetCheckTransactions()
        Try
            Me.qbSearcher.QueryType = nsoftware.IBizQB.ObjsearchQueryTypes.qtCheckSearch
            'Do the Search
            Try
                Me.qbSearcher.Search()
            Catch ex As Exception

            Finally
                'KillQuickbooks()
            End Try


            For i As Integer = 0 To Me.qbSearcher.ResultCount - 1
                Dim myAccount As New nsoftware.IBizQB.Check
                myAccount.QBResponseAggregate = Me.qbSearcher.ResultAggregate(i)
                DoChecks(myAccount.QBResponseAggregate.ToString)
            Next
        Catch ex As Exception
            Errors = True
        End Try
    End Sub

    Private Sub GetCreditMemoTransactions()
        Try
            Me.qbSearcher.QueryType = nsoftware.IBizQB.ObjsearchQueryTypes.qtCreditMemoSearch
            'Do the Search
            Try
                Me.qbSearcher.Search()
            Catch ex As Exception

            Finally
                'KillQuickbooks()
            End Try


            For i As Integer = 0 To Me.qbSearcher.ResultCount - 1
                Dim myAccount As New nsoftware.IBizQB.Check
                myAccount.QBResponseAggregate = Me.qbSearcher.ResultAggregate(i)
                DoCreditMemos(myAccount.QBResponseAggregate.ToString)
            Next
        Catch ex As Exception
            Errors = True
        End Try
    End Sub



#End Region

#Region "QB Transaction Parsing and Adding"

    Private Sub DoInvoices(ByVal theString As String)
        Try
            Dim xmlDoc As New XmlDocument
            Dim InvoiceNodes As XmlNodeList
            Dim baseInvoiceNode As XmlNode
            Dim dateTransaction As Date
            Dim transactionNumber As String
            Dim theCompanyName As String = ""
            Dim InvoiceNumber As String = ""
            Dim je As QBTransactionClass
            Dim invoiceList As New List(Of QBTransactionClass)

            Using reader As XmlReader = XmlReader.Create(New StringReader(theString))
                reader.ReadToFollowing("TxnNumber")
                transactionNumber = reader.ReadElementContentAsString()
                reader.ReadToFollowing("TxnDate")
                dateTransaction = reader.ReadElementContentAsDateTime()

            End Using

            Using reader As XmlReader = XmlReader.Create(New StringReader(theString))
                reader.ReadToFollowing("RefNumber")
                InvoiceNumber = reader.ReadElementContentAsInt().ToString()
                reader.Close()
            End Using

            ' Filtering Old Ones out
            If dateTransaction < My.Settings.StaticBalanceDate Then Exit Sub

            If transactionNumber = "292563" Then
                Dim s As String = "Time to take a breather"
            End If
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
                je = New QBTransactionClass
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
                            je.Memo = childNode.InnerText
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

            For Each theJe As QBTransactionClass In invoiceList
                If theJe.Account.Contains("Customer Deposit") Then
                    taTransactionRecord.Insert(theJe.Company, "Invoice", dateTransaction, transactionNumber, theJe.Memo, theJe.Amount, theString, "", InvoiceNumber)
                End If

            Next
            xmlDoc = Nothing
            invoiceList = Nothing

        Catch ex As Exception
            Errors = True
        End Try
    End Sub


    Private Sub DoJournalEntries(ByVal theString As String)
        Try
            Dim theJournalEntries As New List(Of QBTransactionClass)
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
            Dim jeCredit As New QBTransactionClass
            Dim je As New QBTransactionClass

            Dim validCreditEntry As Boolean
            Dim validDebitEntry As Boolean

            'Get the basic info first
            Dim wrapperReader As XmlReader = XmlReader.Create(New StringReader(theString))
            Try
                wrapperReader.ReadToFollowing("TxnNumber")
                theTransactionNumber = wrapperReader.ReadElementContentAsString
                Using newWrapper As XmlReader = XmlReader.Create(New StringReader(theString))
                    newWrapper.ReadToFollowing("TxnDate")
                    theTransactionDate = wrapperReader.ReadElementContentAsString()
                End Using


            Catch ex As Exception

            End Try
            Try
                Using newWrapper As XmlReader = XmlReader.Create(New StringReader(theString))
                    newWrapper.ReadToFollowing("RefNumber")
                    theTransactionNumber = wrapperReader.ReadElementContentAsString()
                End Using


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
                    jeCredit = New QBTransactionClass
                    jeCredit.Company = CustomerName
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
                    je = New QBTransactionClass
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
            For Each de As QBTransactionClass In theJournalEntries
                If de.Account.Contains("Customer Deposit") Then
                    If de.DebitCredit = "Debit" Then
                        'see if the transaction already exists
                        ' If taTransactionRecord.DoesTransactionExist(de.Company, "Journal", theTransactionDate, de.Memo, theTransactionNumber, de.Amount * -1) = 0 Then
                        If de.Company = CustomerName And Date.Parse(theTransactionDate) >= AsOfDate Then
                            taTransactionRecord.Insert(de.Company, "Journal", theTransactionDate, theTransactionNumber, de.Memo, de.Amount * -1, theString, "", "")
                        End If

                    Else
                        'see if the transaction already exists
                        'If taTransactionRecord.DoesTransactionExist(de.Company, "Journal", theTransactionDate, de.Memo, theTransactionNumber, de.Amount) = 0 Then
                        If de.Company = CustomerName And Date.Parse(theTransactionDate) >= AsOfDate Then
                            taTransactionRecord.Insert(de.Company, "Journal", theTransactionDate, theTransactionNumber, de.Memo, de.Amount, theString, "", "")
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            Errors = True
        End Try

    End Sub


    Private Sub DoDeposits(ByVal theString As String)
        Try
            Dim dateTransaction As Date
            Dim transactionNumber As String
            Dim xmlDoc As New XmlDocument
            Dim dataNodeList As XmlNodeList
            Dim je As New QBTransactionClass
            Dim checkList As New List(Of QBTransactionClass)

            xmlDoc.LoadXml(theString)

            Dim TimeNode As XmlNodeList = xmlDoc.GetElementsByTagName("TxnDate")
            Dim detailTimeNode As XmlNode = TimeNode.Item(0)
            dateTransaction = detailTimeNode.InnerText

            Dim transactionNode As XmlNodeList = xmlDoc.GetElementsByTagName("TxnNumber")
            Dim detailtransactionNode As XmlNode = transactionNode.Item(0)
            transactionNumber = detailtransactionNode.InnerText

            'Get a list of deposits
            dataNodeList = xmlDoc.GetElementsByTagName("DepositLineRet")

            'Create a je for each deposit, we'll filter for company name before we apply
            For Each depositNode As XmlNode In dataNodeList
                je = New QBTransactionClass
                je.Company = CustomerName
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


            For Each theJE As QBTransactionClass In checkList
                If theJE.Company = CustomerName And theJE.Account.Contains("Customer Deposit") And dateTransaction >= AsOfDate Then
                    taTransactionRecord.Insert(CustomerName, "Deposit", dateTransaction, transactionNumber, theJE.Memo, Math.Abs(CDec(theJE.Amount)), theString, "", "")
                End If
            Next

        Catch ex As Exception
            Errors = True
        End Try


    End Sub


    Private Sub DoChecks(ByVal theString As String)
        Try
            Dim dateTransaction As Date
            Dim transactionNumber As String
            Dim xmlDoc As New XmlDocument
            Dim dataNodeList As XmlNodeList
            Dim dataNode As XmlNode
            Dim je As New QBTransactionClass


            je.Company = CustomerName
            je.Account = ""
            je.Amount = ""
            je.DebitCredit = "Neither"
            je.IsCustomerDepositAccount = False
            je.ListID = ""
            je.Memo = ""

            xmlDoc.LoadXml(theString)

            Dim TimeNode As XmlNodeList = xmlDoc.GetElementsByTagName("TxnDate")
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


            If je.Account.Contains("Customer Deposit") And dateTransaction >= AsOfDate Then
                taTransactionRecord.Insert(je.Company, "Check", dateTransaction, transactionNumber, je.Memo, je.Amount * -1, theString, "", "")
            End If

        Catch ex As Exception
            Errors = True
        End Try

    End Sub

    Private Sub DoCreditMemos(ByVal theString As String)
        Try
            Dim dateTransaction As Date
            Dim transactionNumber As String
            Dim xmlDoc As New XmlDocument
            Dim dataNodeList As XmlNodeList
            ' Dim dataNode As XmlNode
            Dim je As New QBTransactionClass


            je.Company = CustomerName
            je.Account = ""
            je.Amount = ""
            je.DebitCredit = "Neither"
            je.IsCustomerDepositAccount = False
            je.ListID = ""
            je.Memo = ""

            xmlDoc.LoadXml(theString)

            Dim TimeNode As XmlNodeList = xmlDoc.GetElementsByTagName("TxnDate")
            Dim detailTimeNode As XmlNode = TimeNode.Item(0)
            dateTransaction = detailTimeNode.InnerText

            Dim transactionNode As XmlNodeList = xmlDoc.GetElementsByTagName("TxnNumber")
            Dim detailtransactionNode As XmlNode = transactionNode.Item(0)
            transactionNumber = detailtransactionNode.InnerText


            dataNodeList = xmlDoc.GetElementsByTagName("Amount")
            je.Amount = dataNodeList.Item(0).InnerText

            Dim doc As XDocument = XDocument.Parse(theString)

            Try
                Dim creditmemos As List(Of XElement) = doc.Descendants("CreditMemoLineRet").ToList()
                For Each creditmemo As XElement In creditmemos
                    Try
                        If creditmemo.Descendants("ItemRef").Descendants("FullName").First().Value = "Customer Deposit" And dateTransaction >= AsOfDate Then
                            je.Amount = creditmemo.Descendants("Amount").First().Value
                            je.Memo = creditmemo.Descendants("Desc").First().Value
                            taTransactionRecord.Insert(je.Company, "Credit Memo", dateTransaction, transactionNumber, je.Memo, je.Amount * -1, theString, "", "")
                        End If
                    Catch ex As Exception

                    End Try


                Next
            Catch ex As Exception

            End Try
        Catch ex As Exception
            Errors = True
        End Try
    End Sub

#End Region

    Public Sub Dispose() Implements System.IDisposable.Dispose
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
    End Sub
End Class

Public Class QBTransactionClass
    Public Property Company As String
    Public Property Account As String
    Public Property Amount As String
    Public Property DebitCredit As String
    Public Property IsCustomerDepositAccount As Boolean
    Public Property Memo As String
    Public Property ListID As String

End Class