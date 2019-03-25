Imports System.Net.Mail
Imports System.IO
Module Mail
    Public Sub SendEmail(ByVal Subject As String, ByVal body As String, ByVal From As String, ByVal ToWhom As String)
        Try
            'Create a new MailMessage object and specify the"From" and "To" addresses
            Dim Email As New System.Net.Mail.MailMessage("TheCompass@cornerstonesupport.com", ToWhom)
            Email.Subject = Subject
            Email.Body = body

            Dim mailClient As New System.Net.Mail.SmtpClient()
            'This object stores the authentication values
            Dim basicAuthenticationInfo As _
               New System.Net.NetworkCredential("thecompass", "winter79")
            'Put your own, or your ISPs, mail server name onthis next line
            mailClient.Host = "CSI-SBS"
            mailClient.UseDefaultCredentials = False
            mailClient.Credentials = basicAuthenticationInfo
            mailClient.Send(Email)
        Catch ex As Exception
            'MsgBox(ex.Message)

        End Try

    End Sub

  
    Public Sub SendHTMLemail(ToWhom As String, from As String, Subject As String, AttachmentString As String, ServerName As String, smtpUser As String, smtpPass As String, htmlBody As String)
        Dim EM As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage(from, ToWhom)
        Dim A As System.Net.Mail.Attachment = New System.Net.Mail.Attachment(AttachmentString)
        Dim RGen As Random = New Random()
        A.ContentId = RGen.Next(100000, 9999999).ToString()
        EM.Attachments.Add(A)
        EM.Subject = Subject
        EM.Body = htmlBody '"<body>" + txtBody.Text + "<br><img src='cid:" + A.ContentId + "'></body>"
        EM.IsBodyHtml = True
        Dim SC As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient(ServerName)
        SC.Send(EM)
    End Sub
    

End Module
