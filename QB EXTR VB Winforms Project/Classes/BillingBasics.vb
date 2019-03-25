Imports System.Drawing
Imports System.Xml
Imports nsoftware.IBizQB

Module BillingBasics

    Public TheCustomerID As Long
    Public TheServiceID As Long
    Public TheCornerstoneQBFile As String
    Public TheIFIQBFile As String
    Public TheCurrentQBFile As String
    Public TheCompanyToBeAssigned As Integer
    Public TheUserName As String
    Public TheUserInitials As String
    Public theUserPosition As String

    'Grid Fonts
    Public theGridFont As New Font("Trebuchet", 9, FontStyle.Regular)
    Public theBoldGridFont As New Font("Trebuchet", 9, FontStyle.Bold)
    Public theItalicGridFont As New Font("Trebuchet", 9, FontStyle.Italic)
    Public theLabelFont As New Font("Trebuchet", 7, FontStyle.Regular)
    Public panelBackColor = Color.FromArgb(173, 162, 132)


    Private thePrimaryBackColor As Color = Color.DarkSlateGray
    Private theprimaryForeColor As Color = Color.PaleGoldenrod

    Public CompanyInfoVerified As Boolean = False

    Public CrystalReportsInstalled As Boolean = False
    Public CrystalReportsFilePath As String

    Public gIFIFile As String
    Public gCornerstoneFile As String

    Public gCheckQBconnectionOnStartup As Boolean

    Public theCustomerList As nsoftware.IBizQB.Objsearch
    Public checksList As nsoftware.IBizQB.Objsearch
    Public theVendorList As nsoftware.IBizQB.Objsearch

    Public gCustomerList As nsoftware.IBizQB.Objsearch
    Public gchecksList As nsoftware.IBizQB.Objsearch
    Public gVendorList As nsoftware.IBizQB.Objsearch


    Public Sub SetupGlobalLists()
        gCustomerList = GetCustomers(TheCurrentQBFile)
        'gVendorList = GetVendors(TheCurrentQBFile)
        'gchecksList = GetChecks(TheCurrentQBFile)

    End Sub

    Public Sub SetupQBFunctionality()
        Dim CornerstoneConnectivityOK As Boolean
        Dim Bill1 As New nsoftware.IBizQB.Bill

        MsgBox("Make sure Cornerstone QB file is open before continuing.")

        ' Quickbooks
        Bill1.QBXMLVersion = "6.0"
        Bill1.QBConnectionString = " CompanyFile= """ + TheCurrentQBFile + """"
        Bill1.QBConnectionString = Bill1.QBConnectionString + " User= """ + My.Settings.ThisApplicationName + """"
        Bill1.QBConnectionString = Bill1.QBConnectionString + " Password= """ + "79Billing" + """"
        Bill1.QBConnectionString = Bill1.QBConnectionString + " ApplicationName= """ + My.Settings.ThisApplicationName + """"
        Bill1.QBConnectionMode = nsoftware.IBizQB.CustomerQBConnectionModes.cmDontCare
        Try
            Bill1.OpenQBConnection()
            CornerstoneConnectivityOK = True
        Catch ex As Exception
            MsgBox(ex.Message)

            CornerstoneConnectivityOK = False
            Exit Sub

        End Try
        KillQuickbooks()

        If CornerstoneConnectivityOK Then
            MsgBox("Cornerstone QB FILE OK")
        Else
            MsgBox("Cornerstone QB FILE NOT WORKING")
        End If
    End Sub

    Public Function GetCustomers(ByVal theQBFileLocation As String) As nsoftware.IBizQB.Objsearch '
        Try
            Return CreateObjSearcher(nsoftware.IBizQB.ObjsearchQueryTypes.qtCustomerSearch, "", TheCurrentQBFile, 0, "", "")
        Catch ex As Exception
            ' MsgBox(ex.Message)

        End Try

        Return Nothing


    End Function

    Public Sub MarkCustomersWithDepositsAsSuch()
        Dim theCustomer As New nsoftware.IBizQB.Customer
        Dim custSearcher As New nsoftware.IBizQB.Objsearch
        custSearcher = GetCustomers(TheCurrentQBFile)

        theCustomer.QBXMLVersion = "6.0"
        theCustomer.QBConnectionString = " CompanyFile= """ + TheCurrentQBFile + """"
        theCustomer.QBConnectionString = theCustomer.QBConnectionString + " User= """ + My.Settings.ThisApplicationName + """"
        theCustomer.QBConnectionString = theCustomer.QBConnectionString + " Password= """ + "Billing1" + """"
        theCustomer.QBConnectionString = theCustomer.QBConnectionString + " ApplicationName= """ + My.Settings.ThisApplicationName + """"
        theCustomer.QBConnectionMode = nsoftware.IBizQB.CustomerQBConnectionModes.cmDontCare


        theCustomer.OpenQBConnection()

        For i As Integer = 0 To custSearcher.ResultCount - 1
            theCustomer.QBResponseAggregate = custSearcher.ResultAggregate(i)
            If CustomerHasBalance(theCustomer.CustomerName) Then
                theCustomer.CustomerTypeName = "Customer Deposit"
                Try
                    theCustomer.Update()
                Catch ex As Exception
                    MsgBox(ex.Message)

                End Try

            End If
        Next
        'theCustomer.CloseQBConnection()
        KillQuickbooks()
    End Sub

    Private Function CustomerHasBalance(theCustName As String) As Boolean
        Dim ta As New CSIAutomationDataSetTableAdapters.QBCustomerNamesTableAdapter
        If ta.GetCustomerDepositAmount(theCustName) <> 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function GetCustomerNames(ByVal QBFileLocation As String) As List(Of String)
        Dim Searcher1 = CreateObjSearcher(nsoftware.IBizQB.ObjsearchQueryTypes.qtCustomerSearch, "", QBFileLocation, 0, "", "")
        Dim customerNameList As New List(Of String)
        Dim customerCount As Integer

        customerCount = Searcher1.ResultCount

        For i As Integer = 0 To Searcher1.ResultCount - 1
            Dim theCustomerName As New nsoftware.IBizQB.Customer
            theCustomerName.QBResponseAggregate = Searcher1.ResultAggregate(i)

            If theCustomerName.CustomerName = "" Then
                EmailToPaul("We have a blank QB Customer Name, the COMPANY name is " & theCustomerName.CompanyName, "see subject")
            Else
                customerNameList.Add(theCustomerName.CustomerName)
            End If
            'If theCustomerName.CompanyName = "" Then
            '    customerNameList.Add(theCustomerName.CustomerName)
            'Else
            '    customerNameList.Add(theCustomerName.CompanyName)
            'End If
        Next
        Return customerNameList
    End Function

    Public Function GetCustomerWithCustDepositNames(ByVal QBFileLocation As String) As List(Of String)
        Dim Searcher1 = CreateObjSearcher(nsoftware.IBizQB.ObjsearchQueryTypes.qtCustomerSearch, "", QBFileLocation, 0, "", "")
        Dim customerNameList As New List(Of String)
        Dim customerCount As Integer

        customerCount = Searcher1.ResultCount

        For i As Integer = 0 To Searcher1.ResultCount - 1
            Dim theCustomerName As New nsoftware.IBizQB.Customer
            theCustomerName.QBResponseAggregate = Searcher1.ResultAggregate(i)
            If theCustomerName.CustomerTypeName = "Customer Deposit" Then
                If theCustomerName.CompanyName = "" Then
                    customerNameList.Add(theCustomerName.CustomerName)
                Else
                    customerNameList.Add(theCustomerName.CompanyName)
                End If
            End If
        Next

        Return customerNameList

    End Function


    Public Function GetVendors(ByVal theQBFileLocation As String) As nsoftware.IBizQB.Objsearch '
        Try
            Return CreateObjSearcher(nsoftware.IBizQB.ObjsearchQueryTypes.qtVendorSearch, "", TheCurrentQBFile, 0, "", "")
        Catch ex As Exception

        End Try

        Return Nothing
    End Function

    Public Function GetVendorsNames(ByVal QBFileLocation As String) As List(Of String)
        Dim Searcher1 = CreateObjSearcher(nsoftware.IBizQB.ObjsearchQueryTypes.qtVendorSearch, "", My.Settings.TheQBFileLocation, 0, "", "")
        Dim vendorNameList As New List(Of String)
        Dim vendorCount As Integer

        vendorCount = Searcher1.ResultCount

        For i As Integer = 0 To Searcher1.ResultCount - 1
            Dim theVendorXML As String = Searcher1.ResultAggregate(i)
            Dim theVendorName As String = xmlGetValueByNode(theVendorXML, "//VendorRet/Name")

            'Dim xmlDoc As XDocument = XDocument.Parse(theVendorXML)
            'Dim vName As String = xmlDoc.Element("Name")
            vendorNameList.Add(theVendorName)
        Next

        Return vendorNameList

    End Function
    Public Function GetVendorsNames(ByVal QBFileLocation As String, ByVal isStateVendor As Boolean) As List(Of String)
        'This function will only return the vendors that are state vendors, one in which filing fees are the expense
        Dim Searcher1 = CreateObjSearcher(nsoftware.IBizQB.ObjsearchQueryTypes.qtVendorSearch, "", My.Settings.TheQBFileLocation, 0, "", "")
        Dim vendorNameList As New List(Of String)
        Dim vendorCount As Integer

        vendorCount = Searcher1.ResultCount
        'Main.UpdateStatusTextBox("Vendor count: " & vendorCount, 'Main.ParagraphPos.Middle)

        For i As Integer = 0 To Searcher1.ResultCount - 1
            Dim theVendorXML As String = Searcher1.ResultAggregate(i)
            Dim newVendor As New nsoftware.IBizQB.Vendor
            newVendor.QBResponseAggregate = Searcher1.ResultAggregate(i)

            If isStateVendor Then
                If InStr(newVendor.VendorTypeName, "State Vendor") Then
                    ''Main.UpdateStatusTextBox("Adding: " & newVendor.VendorName, 'Main.ParagraphPos.Middle)
                    vendorNameList.Add(newVendor.VendorName)
                End If
            Else
                vendorNameList.Add(newVendor.VendorName)
            End If
        Next
        'Main.UpdateStatusTextBox("Returning vendorList to database function.", 'Main.ParagraphPos.Middle)

        Return vendorNameList
    End Function

    Public Sub MarkStateVendors(theVendorList As nsoftware.IBizQB.Objsearch)

    End Sub

    Public Sub MarkStateVendorsAsSuch()
        Dim Searcher1 = CreateObjSearcher(nsoftware.IBizQB.ObjsearchQueryTypes.qtVendorSearch, "", TheCurrentQBFile, 0, "", "")
        Dim theVendor As New nsoftware.IBizQB.Vendor

        theVendor.QBXMLVersion = "6.0"
        theVendor.QBConnectionString = " CompanyFile= """ + TheCurrentQBFile + """"
        theVendor.QBConnectionString = Searcher1.QBConnectionString + " User= """ + My.Settings.ThisApplicationName + """"
        theVendor.QBConnectionString = Searcher1.QBConnectionString + " Password= """ + "billing" + """"
        theVendor.QBConnectionString = Searcher1.QBConnectionString + " ApplicationName= """ + My.Settings.ThisApplicationName + """"
        theVendor.QBConnectionMode = nsoftware.IBizQB.CustomerQBConnectionModes.cmDontCare
        theVendor.OpenQBConnection()

        Dim ta As New CSIAutomationDataSetTableAdapters.StateVendorsTableAdapter

        For i As Integer = 0 To Searcher1.ResultCount - 1

            theVendor.QBResponseAggregate = Searcher1.ResultAggregate(i)
            If ta.StateVendorExists(theVendor.VendorName) Then
                theVendor.VendorTypeName = "State Vendor"
                theVendor.Update()
            End If

        Next
        theVendor.CloseQBConnection()
        KillQuickbooks()
        MsgBox("Done adding vendors.")

    End Sub
    Public Function GetVendorDetails(ByVal QBFileLocation As String, ByVal theVendorName As String) As String
        Dim VendorXML As String
        Dim Searcher1 = CreateObjSearcher(nsoftware.IBizQB.ObjsearchQueryTypes.qtVendorSearch, theVendorName, TheCurrentQBFile, 0, "", "")
        VendorXML = Searcher1.ResultAggregate(0)

        Return VendorXML

    End Function


    Public Function xmlGetValueByNode(ByVal theText As String, ByVal theNode As String) As String
        Dim xmlDoc As XmlDocument = New XmlDocument
        xmlDoc.LoadXml(theText)

        'Dim xmlNodeList = xmlDoc.SelectNodes("//CustomerRef/FullName")
        Dim xmlNodeList = xmlDoc.SelectNodes(theNode)

        If xmlNodeList IsNot Nothing Then
            For Each xmlNode As XmlNode In xmlNodeList
                Return xmlNode.InnerText
            Next
        Else
            Return ""
        End If

        Return ""

    End Function

    Public Function GetQBCustomerByName(ByVal TheCustomerName As String) As nsoftware.IBizQB.Customer
        Dim custSearch As New nsoftware.IBizQB.Objsearch
        custSearch = CreateObjSearcher(ObjsearchQueryTypes.qtCustomerSearch, TheCustomerName, TheCurrentQBFile, False, "", "")
        Dim qbCust As New nsoftware.IBizQB.Customer
        Try
            qbCust.QBResponseAggregate = custSearch.ResultAggregate(0)
            Return qbCust
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    Public Function CreateObjSearcher(ByVal queryType As nsoftware.IBizQB.ObjsearchQueryTypes, ByVal theNameString As String, ByVal QBFileLocation As String, ByVal DateRange As Boolean, ByVal DateStart As String, ByVal DateEnd As String) As nsoftware.IBizQB.Objsearch
        Dim Searcher1 As New nsoftware.IBizQB.Objsearch
        Try
            TheCurrentQBFile = "Q:\Cornerstone.qbw"
            Searcher1.QBXMLVersion = "6.0"
            Searcher1.QBConnectionString = " CompanyFile= """ + TheCurrentQBFile + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " User= """ + My.Settings.ThisApplicationName + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " Password= """ + "Billing1" + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " ApplicationName= """ + My.Settings.ThisApplicationName + """"
            Searcher1.QBConnectionMode = nsoftware.IBizQB.CustomerQBConnectionModes.cmDontCare

            Searcher1.QueryType = queryType
            Searcher1.OpenQBConnection()


            'Is there a name search string?
            If theNameString <> "" Then Searcher1.EntityName = theNameString

            'Is there a date range?
            If DateRange Then
                Searcher1.TransactionDateStart = DateStart
                Searcher1.TransactionDateEnd = DateEnd
            End If


            'Do the search
            Searcher1.Search()
            'Searcher1.CloseQBConnection()
            ' KillQuickbooks()
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
        Return Searcher1
    End Function



    Public Function CheckQBconnection(ByVal theQBfile As String) As Boolean
        Dim CornerstoneConnectivityOK As Boolean
        Dim Bill1 As New nsoftware.IBizQB.Bill

        Try
            ' Quickbooks
            Bill1.QBXMLVersion = "6.0"
            Bill1.QBConnectionString = " CompanyFile= """ + TheCurrentQBFile + """"
            Bill1.QBConnectionString = Bill1.QBConnectionString + " User= """ + My.Settings.ThisApplicationName + """"
            Bill1.QBConnectionString = Bill1.QBConnectionString + " Password= """ + "Billing1" + """"
            Bill1.QBConnectionString = Bill1.QBConnectionString + " ApplicationName= """ + My.Settings.ThisApplicationName + """"
            Bill1.QBConnectionMode = nsoftware.IBizQB.CustomerQBConnectionModes.cmDontCare

        Catch ex As Exception
            'MsgBox("There was a problem setting the QB Properties for Cornerstone.")
        End Try

        Try
            Bill1.OpenQBConnection()
            CornerstoneConnectivityOK = True
        Catch ex As Exception
            CornerstoneConnectivityOK = False
        End Try

        'KillQuickbooks()

        Return CornerstoneConnectivityOK


    End Function

    Public Sub SetQBFileLocation(ByVal WhichQBFile As String)
        My.Settings.TheQBFileLocation = WhichQBFile
        My.Settings.Save()
    End Sub

    Public Function GetQBFileLocation(ByVal WhichQBFile As String)
        Try
            Return My.Settings.TheQBFileLocation
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Sub KillQuickbooks()
        Dim pList() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcessesByName("qbw32")
        For Each proc As System.Diagnostics.Process In pList
            Try
                proc.Kill()
            Catch ex As Exception

            End Try

        Next

        Dim p2List() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcessesByName("WerFault")
        'For Each proc As System.Diagnostics.Process In pList
        '    Try
        '        proc.Kill()
        '    Catch ex As Exception

        '    End Try

        'Next

    End Sub

    Public Function verifyQBConnectivity(ByVal theQBfile As String)
        Dim testerObject As New nsoftware.IBizQB.Objsearch
        Try
            testerObject = CreateObjSearcherWithoutKillingQuickBooks(nsoftware.IBizQB.ObjsearchQueryTypes.qtCustomerSearch, "", TheCurrentQBFile, 0, "", "")
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function



    Public Function CreateObjSearcherWithoutKillingQuickBooks(ByVal queryType As nsoftware.IBizQB.ObjsearchQueryTypes, ByVal theNameString As String, ByVal QBFileLocation As String, ByVal DateRange As Boolean, ByVal DateStart As String, ByVal DateEnd As String) As nsoftware.IBizQB.Objsearch
        Dim Searcher1 As New nsoftware.IBizQB.Objsearch
        Try
            Searcher1.QBXMLVersion = "6.0"
            Searcher1.QBConnectionString = " CompanyFile= """ + TheCurrentQBFile + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " User= """ + My.Settings.ThisApplicationName + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " Password= """ + "Billing1" + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " ApplicationName= """ + My.Settings.ThisApplicationName + """"
            Searcher1.QBConnectionMode = nsoftware.IBizQB.CustomerQBConnectionModes.cmDontCare

            Searcher1.QueryType = queryType
            Searcher1.OpenQBConnection()


            'Is there a name search string?
            If theNameString <> "" Then Searcher1.EntityName = theNameString

            'Is there a date range?
            If DateRange Then
                Searcher1.TransactionDateStart = DateStart
                Searcher1.TransactionDateEnd = DateEnd
            End If


            'Do the search
            Searcher1.Search()

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
        Return Searcher1
    End Function

    Public Function VerifyQBItem(theItemName As String)

        Dim Searcher1 As New nsoftware.IBizQB.Objsearch
        Try
            Searcher1.QBXMLVersion = "6.0"
            Searcher1.QBConnectionString = " CompanyFile= """ + TheCurrentQBFile + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " User= """ + My.Settings.ThisApplicationName + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " Password= """ + "Billing1" + """"
            Searcher1.QBConnectionString = Searcher1.QBConnectionString + " ApplicationName= """ + My.Settings.ThisApplicationName + """"
            Searcher1.QBConnectionMode = nsoftware.IBizQB.CustomerQBConnectionModes.cmDontCare

            Searcher1.QueryType = ObjsearchQueryTypes.qtItemSearch
            Searcher1.NameContains = theItemName
            Searcher1.OpenQBConnection()
            Searcher1.Search()


            If Searcher1.ResultCount > 0 Then
                Return True
            Else
                Return (AddQBItem(theItemName))

            End If

        Catch ex As Exception
            Return False
        End Try


    End Function


    Public Function AddQBItem(theItemName As String)
        Dim itemSearcher As New nsoftware.IBizQB.Item
        itemSearcher.QBXMLVersion = "6.0"
        itemSearcher.QBConnectionString = " CompanyFile= """ + TheCurrentQBFile + """"
        itemSearcher.QBConnectionString = itemSearcher.QBConnectionString + " User= """ + My.Settings.ThisApplicationName + """"
        itemSearcher.QBConnectionString = itemSearcher.QBConnectionString + " Password= """ + "Billing1" + """"
        itemSearcher.QBConnectionString = itemSearcher.QBConnectionString + " ApplicationName= """ + My.Settings.ThisApplicationName + """"
        itemSearcher.QBConnectionMode = nsoftware.IBizQB.CustomerQBConnectionModes.cmDontCare

        itemSearcher.ItemType = ItemItemTypes.itService
        itemSearcher.ItemName = Left(theItemName, 31)
        itemSearcher.AccountName = "Initial Licensing Work:Comprehensive - Partial"

        Try
            itemSearcher.Add()
            Return True
        Catch ex As Exception
            MsgBox(ex.Message)

            Return False
        End Try

    End Function
    Public Function GetAllQBCustomers(ByVal theQBFileLocation As String) As nsoftware.IBizQB.Objsearch '
        Try
            Return CreateObjSearcher(nsoftware.IBizQB.ObjsearchQueryTypes.qtCustomerSearch, "", TheCurrentQBFile, 0, "", "")
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try

        Return Nothing


    End Function


End Module
