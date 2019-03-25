Imports System.Windows.Forms

Public Class CustomerDepositProcessor
    Public QBCustomersWithDepositBalances As New List(Of String)
    Dim ErrorList As New List(Of String)
    Dim SuccessList As New List(Of String)

    Public Sub New()
        Try
            QBCustomersWithDepositBalances = Me.GetCustomersWithCustomerDepositBalances()
        Catch ex As Exception
        End Try

    End Sub

    Public Sub GetCustomerDepositBalancesForAll()

        Dim qbCust As qbCustomer
        Try
            For Each custName As String In Me.QBCustomersWithDepositBalances
                qbCust = New qbCustomer(custName)
                qbCust.GetCustomerDepositBalance()

                If qbCust.Successful Then
                    SuccessList.Add(custName)
                Else
                    ErrorList.Add(custName)
                End If
            Next
            EmailToPaul("QB Transaction pull.", CreateTransactionPullSummary)
            MessageBox.Show("Done.")
        Catch ex As Exception

        End Try

    End Sub

    Private Function CreateTransactionPullSummary() As String
        Dim summary As String
        summary = "This list was generated " + Now + vbCrLf
        summary += vbCrLf

        For Each success As String In SuccessList
            summary += success + vbCrLf
        Next

        summary += vbCrLf + vbCrLf
        summary += "Failures" + vbCrLf
        For Each failure As String In ErrorList
            summary += failure + vbCrLf
        Next
        Return summary
    End Function


    Private Function GetCustomersWithCustomerDepositBalances() As List(Of String)
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
            ' 'Main.UpdateStatusTextBox("Function: GetCustomersWithCustomerDeposotiBlances:" & ex.Message, 'Main.ParagraphPos.Middle)
        End Try
        Return customerNameList
    End Function

End Class
