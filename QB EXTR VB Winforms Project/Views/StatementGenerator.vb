Imports QBTransactions
Imports QBTransactions.ViewModels

Public Class StatementGenerator

    Dim vm As New QBTransactionReportingViewModel

    Private Sub contextMenuRemoveCompanyFromList_Click(sender As Object, e As EventArgs) Handles contextMenuRemoveCompanyFromList.Click
        'Remove selected company from list

    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click

        ' Is it Custom or Monthly

        For Each lstCompany As String In lstCompanies.Items
            Dim startDate As Date = dtStart.Value
            Dim endDate As Date = dtEnd.Value

            Dim startBalance As Decimal = vm.GetBalanceAsOfCertainDay(lstCompany, startDate)
            Dim endBalance As Decimal = vm.GetBalanceAsOfCertainDay(lstCompany, endDate)
            Dim currentBalance As Decimal = vm.GetActualCustomerBalance(lstCompany)

            Dim excelReporter As New QBCustomerDeposit
            excelReporter.CreateExcelReport(lstCompany, endDate)


            ' Which Format, excel or pdf?


            'Create the statement


            ' Package it up and deliver

        Next



    End Sub

    Private Sub StatementGenerator_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtStart.Value = DateAdd("m", -1, DateSerial(Year(Today), Month(Today), 1))
        dtEnd.Value = DateAdd("m", 0, DateSerial(Year(Today), Month(Today), 0))
    End Sub

End Class