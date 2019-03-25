Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Excel

Module Excel
    Public Sub ImportStateVendors()
        Try
            Dim excelApp As New Microsoft.Office.Interop.Excel.Application


            Dim workbookPath As String = "C:\Users\Paul\Desktop\s.xlsx"
            Dim remWB As Microsoft.Office.Interop.Excel.Workbook = excelApp.Workbooks.Open(workbookPath)
            Dim excelWorksheet As Microsoft.Office.Interop.Excel.Worksheet = CType(remWB.Worksheets(1), Microsoft.Office.Interop.Excel.Worksheet)
            Dim myDataSetTA As New CSIAutomationDataSetTableAdapters.StateVendorsTableAdapter

            For i = 1 To 311
                myDataSetTA.Insert(excelWorksheet.Range("A" & i.ToString).Value)
            Next


            excelWorksheet = Nothing
            remWB.Close()
            excelApp.Quit()
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try


    End Sub
End Module
