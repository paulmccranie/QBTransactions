Public Class CustDepositOnly

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim qbprocessor As New CustomerDepositProcessor
        qbprocessor.GetCustomerDepositBalancesForAll()

    End Sub
End Class