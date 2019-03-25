
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data.SqlTypes
Imports System.Threading
Imports System.Text.RegularExpressions.Regex
Imports System.Text.RegularExpressions


Module Module1
    Public theAZNVID As Integer
    Public TheRoutineID As Integer
    Public WeAreDebugging As Boolean = False
    Public CheckRequestStatusCode As String

   
    Public Sub UpdateTheSIAudit()
        Dim theSITA As New SIAuditTableAdapters.rmServiceItemTableAdapter
        Dim SIds As New SIAudit.rmServiceItemDataTable
        Dim theSIAuditTA As New SIAuditTableAdapters.SIAuditTableAdapter

        'The record fields
        Dim DateCreated As Date
        Dim DateAC As Date
        Dim DateUC As Date
        Dim DateAS As Date
        'Dim DateSat As Date
        Dim SQLNulldate As SqlDateTime = SqlDateTime.Null
        'Dim SQLNull As Date = SQLNulldate.ToSqlString

        theSIAuditTA.DeleteAll()
        theSITA.GetSIidsFORALLOLitemsUNSAT(SIds)

        Dim theSIreader As DataTableReader = SIds.CreateDataReader


        While theSIreader.Read
            ' SEt the Date Created, set to null if null
            Try
                DateCreated = theSITA.GetDateCreated(theSIreader.Item(0))
            Catch ex As Exception
                DateCreated = SqlDateTime.Null
            End Try


            Try
                DateAC = Format(theSITA.GetDateCreatedBasedOnTextString(theSIreader.Item(0), "%AT CLIENT%"), "Short Date")
            Catch ex As Exception
                DateAC = SqlDateTime.Null
            End Try


            Try
                DateUC = Format(theSITA.GetDateCreatedBasedOnTextString(theSIreader.Item(0), "%UNDER REVIEW%"), "Short Date")
            Catch ex As Exception
                DateUC = SqlDateTime.Null
            End Try


            Try
                DateAS = Format(theSITA.GetDateCreatedBasedOnTextString(theSIreader.Item(0), "%AT STATE%"), "Short Date")
            Catch ex As Exception
                DateAS = SqlDateTime.Null
            End Try


            'need algorithm, take the most recent date - and set the others to null

            If theSITA.GETACstatus(theSIreader.Item(0)) = True Then
                DateUC = SqlDateTime.Null
                DateAS = SqlDateTime.Null
            End If
            If theSITA.GETUCstatus(theSIreader.Item(0)) = True Then
                DateAC = SqlDateTime.Null
                DateAS = SqlDateTime.Null
            End If
            If theSITA.GETASstatus(theSIreader.Item(0)) = True Then
                DateUC = SqlDateTime.Null
                DateAC = SqlDateTime.Null
            End If

            theSIAuditTA.AddNewSIauditItem(CInt(theSIreader.Item(0)), DateAC, DateUC, DateAS)
        End While

        'now for every item marked 1/1/1900, set it to null

        theSIAuditTA.CleanAC()
        theSIAuditTA.CleanUC()
        theSIAuditTA.CleanAS()
    End Sub
    Public Sub FindLostExpDatesOnServiceItems(ByVal theID As Integer, ByVal theRoutine As String)
        'The purpose of this sub is to cycle through each client and determine
        ' if a client has renewal items and yet, is not marked a Renewal Client

        'There's 2 cases
        '1) Client has renewal items assigned to it AND is not marked as a Renewal Client
        '2) Client is marked renewal, but branch(es) are NOT marked as Renewal

        'These are the standard values needed
        Dim AutomationNotifiedTableAdapter As New CornerstoneSQLUpsizeDataSetTableAdapters.AutomationNotifiedTableAdapter
        Dim AutomationNotified As New CornerstoneSQLUpsizeDataSet.AutomationNotifiedDataTable
        Dim aRow As DataRow
        Dim TheNotifiedName As String = ""
        Dim TheNotifiedEmail As String = ""
        Dim TheNotifiedString As String = ""

        AutomationNotifiedTableAdapter.GetToBeNotifiedList(AutomationNotified, theID)
        For Each aRow In AutomationNotified
            TheNotifiedName = aRow.Item("NotifiedName").ToString
            TheNotifiedEmail = aRow.Item("NotifiedEmail").ToString
        Next

        'Cycle through the CompanyRecordsets and see if there are nonRenewal clients with Renewal Project Items
        Dim CompanyDS As New CornerstoneSQLUpsizeDataSet.rmServiceItemDataTable
        Dim taCompany As New CornerstoneSQLUpsizeDataSetTableAdapters.rmServiceItemTableAdapter

        taCompany.GetStatusAndSINameWithNoExpDatePotential(CompanyDS)

        TheNotifiedString = "The following Agencies/ProjectItems records MAY require attention as they continue to have no expiration date assigned even though there appears to be activity." + vbCrLf + vbCrLf + vbCrLf + vbCrLf

        Dim theRow As DataRow
        For Each theRow In CompanyDS
            TheNotifiedString += theRow.Item("Status").ToString & "    " & theRow.Item("Name") + vbCrLf + vbCrLf
        Next
        Mail.SendEmail("Possible Project Item Issues", TheNotifiedString, "The Compass", "vbeavers@cornerstonesupport.com")
    End Sub

    Public Sub CheckForUnCheckedRenewalClients(ByVal theID As Integer, ByVal theRoutine As String)
        'The purpose of this sub is to cycle through each client and determine
        ' if a client has renewal items and yet, is not marked a Renewal Client

        'There's 2 cases
        '1) Client has renewal items assigned to it AND is not marked as a Renewal Client
        '2) Client is marked renewal, but branch(es) are NOT marked as Renewal

        'These are the standard values needed
        Dim AutomationNotifiedTableAdapter As New CornerstoneSQLUpsizeDataSetTableAdapters.AutomationNotifiedTableAdapter
        Dim AutomationNotified As New CornerstoneSQLUpsizeDataSet.AutomationNotifiedDataTable
        Dim aRow As DataRow
        Dim TheNotifiedName As String = ""
        Dim TheNotifiedEmail As String = ""
        Dim TheNotifiedString As String = ""

        AutomationNotifiedTableAdapter.GetToBeNotifiedList(AutomationNotified, theID)
        For Each aRow In AutomationNotified
            TheNotifiedName = aRow.Item("NotifiedName").ToString
            TheNotifiedEmail = aRow.Item("NotifiedEmail").ToString
        Next

        'Cycle through the CompanyRecordsets and see if there are nonRenewal clients with Renewal Project Items
        Dim CompanyDS As New CornerstoneSQLUpsizeDataSet.CompanyDataTable
        Dim taCompany As New CornerstoneSQLUpsizeDataSetTableAdapters.CompanyTableAdapter

        taCompany.NonRenewalClients_GetAll(CompanyDS)

        Dim theRow As DataRow
        For Each theRow In CompanyDS
            If taCompany.Count_RenewalItemsAssignedToClient(theRow.Item(0)) Then
                TheNotifiedString += theRow.Item("Status").ToString & " has renewal items, but is not a renewal client" + vbCrLf + vbCrLf
            End If
        Next
        Mail.SendEmail("Unchecked Renewal Clients", TheNotifiedString, "The Compass", "vbeavers@cornerstonesupport.com")
    End Sub
   

    Public Sub EmailToPaul(theSubject As String, ByVal theString As String)
        Try
            Dim email As New System.Net.Mail.MailMessage

            'Assign the From parameter
            email.From = New System.Net.Mail.MailAddress("donotreply@cornerstonesupport.com")



            'assign the subject and body
            email.Subject = theSubject & "  - " & Now.Date.ToString
            'email.Body = "Dear " & TheNotifiedName & ", " & vbCr & vbCr & vbTab & "Attached are the 10 Day Letters for this time period: " & Now.ToShortDateString & vbCr & vbCr & "Sincerely," & vbCr & vbCr & "The Compass" & vbCr & "System Generated" & vbCr & "Cornerstone Support, Inc." & vbCr
            email.Body = theString
            email.To.Add("pmccranie@cornerstonesupport.com")
            'setup the mail server parameters
            Dim mailClient As New System.Net.Mail.SmtpClient()
            'This object stores the authentication values
            Dim basicAuthenticationInfo As _
               New System.Net.NetworkCredential("thecompass", "winter79")
            'Put your own, or your ISPs, mail server name onthis next line
            mailClient.Host = "CSI-SBS"
            mailClient.UseDefaultCredentials = False
            mailClient.Credentials = basicAuthenticationInfo

            ' Finally = We're going to send the email
            mailClient.Send(email)

            'clear out the vars
            email = Nothing
            mailClient = Nothing
        Catch ex As Exception

        End Try

    End Sub
   


End Module
