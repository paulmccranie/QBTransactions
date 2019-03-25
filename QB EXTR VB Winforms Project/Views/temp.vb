Public Class temp

Private Sub btnRunExcelForATG_Click( sender As Object,  e As EventArgs) Handles btnRunExcelForATG.Click
     Dim vm As New   QBTransactionReportingViewModel 
        vm.CreateExcelReport("ATG Credit, LLC", "1/1/2014")
End Sub
End Class