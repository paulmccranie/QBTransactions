Imports Microsoft.Office.Interop
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles

Public Class ImportCustomerDepositAmountsFromExcel
    Implements IDisposable
    Dim theCSIQBTA As New CSIAutomationDataSetTableAdapters.QBCustomerNamesTableAdapter


    Public Sub ImportCustDepBalancesFromExcel()
        Dim theQBCompanyName As String = "Start"
        Dim theQBCompanyCustomerDepositBalance As Decimal = 0.0
        Dim excelApp As New Excel.Application()
        Dim excelBook As Excel.Workbook
        Dim excelWorksheet As Excel.Worksheet
        Dim openDialog As New OpenFileDialog
        Dim SuccessList As New List(Of AgencyAndAmountData)
        Dim FailList As New List(Of AgencyAndAmountData)
        Dim PreviousCompanyName As String = ""


        Try
            openDialog.ShowDialog()

            Dim theExcelPath As String = openDialog.FileName

            If theExcelPath = "" Then Exit Sub ' make sure that canceling the dialog causes exit

            Dim ExcelRowCounter As Integer = 2

            excelBook = excelApp.Workbooks.Open(theExcelPath) '"C:\Users\Paul\Desktop\MassNMLSTransition.xls")
            excelWorksheet = CType(excelBook.Worksheets(2), Excel.Worksheet)

            Dim asofDate As Date = InputBox("What is the As Of Date?")

            Dim taStatic As New CSIAutomationDataSetTableAdapters.CustomerDepositBalanceStaticTableAdapter
            taStatic.DeleteAll()

            'Run down the list and when you hit a blank, exit out
            While theQBCompanyName <> "TOTALBOGUS_NEVAGONNAHAPPEN"
                theQBCompanyName = excelWorksheet.Range("B" & ExcelRowCounter).Value
                theQBCompanyCustomerDepositBalance = excelWorksheet.Range("J" & ExcelRowCounter).Value

                If theQBCompanyName Is Nothing Then theQBCompanyName = ""

                If theQBCompanyName.Contains("Genpact") Then
                    theExcelPath = "Boge"
                End If
                If theQBCompanyName.Contains("Total ") Then

                    Try
                        theQBCompanyName = theQBCompanyName.Replace("Total ", "")
                        taStatic.AddNewStaticBalance(theQBCompanyName, theQBCompanyCustomerDepositBalance, asofDate)
                        SuccessList.Add(New AgencyAndAmountData(theQBCompanyName, theQBCompanyCustomerDepositBalance, "Success"))

                    Catch ex As Exception
                        FailList.Add(New AgencyAndAmountData(theQBCompanyName, theQBCompanyCustomerDepositBalance, ex.Message))
                    End Try

                End If

                If excelWorksheet.Range("A" & ExcelRowCounter).Value = "TOTAL" Then Exit While
                ExcelRowCounter += 1

                If ExcelRowCounter > 6000 Then
                    MsgBox("We're over 6000")
                    Exit Sub
                End If
            End While

            ' excelBook.Close()
            'excelApp = Nothing
            ReportSummary(SuccessList, FailList)

        Catch ex As Exception
            MsgBox(ex.Message)
            excelApp.Quit()
        Finally

        End Try
        excelApp.Quit()

        '   Dim pList() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcessesByName("excel")
        'For Each proc As System.Diagnostics.Process In pList
        '    Try
        '        proc.Kill()
        '    Catch ex As Exception
        '    End Try
        'Next

    End Sub

    Private Sub ReportSummary(ByVal successList As List(Of AgencyAndAmountData), ByVal FailList As List(Of AgencyAndAmountData))
        Dim xlApp As Excel.Application, xlWorkBook As Excel.Workbook, xlWorkSheet As Excel.Worksheet
        'Dim MissingValue As Object = System.Reflection.Missing.Value

        Try
            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Add()
            xlWorkSheet = CType(xlWorkBook.Sheets.Item(1), Microsoft.Office.Interop.Excel.Worksheet)

            xlWorkSheet.Cells(1, 1) = "Agency"
            xlWorkSheet.Cells(1, 2) = "Deposit Amount"
            xlWorkSheet.Cells(1, 3) = "Reason"

            Dim row As Integer = 1

            For Each aData As AgencyAndAmountData In FailList
                xlWorkSheet.Cells(row, 1) = aData.AgencyName
                xlWorkSheet.Cells(row, 2) = aData.AgencyAmount
                xlWorkSheet.Cells(row, 3) = aData.Reason
                row += 1
            Next

            row += 3

            For Each aData As AgencyAndAmountData In successList
                xlWorkSheet.Cells(row, 1) = aData.AgencyName
                xlWorkSheet.Cells(row, 2) = aData.AgencyAmount
                xlWorkSheet.Cells(row, 3) = aData.Reason
                row += 1
            Next

            xlWorkBook.Worksheets(1).Columns("A:C").AutoFit()
            xlApp.Visible = True
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose

    End Sub
End Class


Public Class AgencyAndAmountData
    Public AgencyName As String
    Public AgencyAmount As Decimal
    Public Reason As String

    Public Sub New(ByVal agencyName As String, ByVal agencyAmount As Decimal, ByVal reason As String)
        Me.AgencyName = agencyName
        Me.AgencyAmount = agencyAmount
        Me.Reason = reason
    End Sub

End Class