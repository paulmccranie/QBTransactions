Imports System.Windows.Forms
Imports x = Microsoft.Office.Interop.Excel


Public Class ExcelExporter
    Friend Function Export(ByRef dgv As DataGridView) As Boolean
        Export = False
        Try
            Dim xlApp As x.Application, xlWorkBook As x.Workbook, xlWorkSheet As x.Worksheet
            Dim MissingValue As Object = System.Reflection.Missing.Value

            xlApp = New x.Application
            xlWorkBook = xlApp.Workbooks.Add(MissingValue)
            xlWorkSheet = CType(xlWorkBook.Sheets.Item(1), Microsoft.Office.Interop.Excel.Worksheet)

            'get all visible solumns in display index order
            Dim ColNames As List(Of String) = (From col As DataGridViewColumn In dgv.Columns.Cast(Of DataGridViewColumn)() _
                                                Where col.Visible = True _
                                                Order By col.DisplayIndex _
                                                Select col.Name).ToList

            'get headers
            Dim colcount = 0
            For Each s In ColNames
                colcount += 1
                xlWorkSheet.Cells(1, colcount) = dgv.Columns.Item(s).HeaderText
            Next

            'get values
            For rowcount = 0 To dgv.Rows.Count - 1  'for each row
                colcount = 0
                For Each s In ColNames 'for each column
                    colcount += 1
                    xlWorkSheet.Cells(rowcount + 2, colcount) = dgv.Rows(rowcount).Cells(s).Value
                    'xlWorkSheet.Cells(rowcount + 2, colcount) = dgv.Rows(rowcount).Cells(s).FormattedValue
                Next s
            Next

            xlApp.Visible = True

            'xlWorkSheet.SaveAs(Path)
            'xlWorkBook.Close()
            'xlApp.Quit()
            'ReleaseObject(xlWorkSheet)
            'ReleaseObject(xlWorkBook)
            'ReleaseObject(xlApp)
            Return True
        Catch ex As Exception
        End Try
    End Function

    Shared Sub ReleaseObject(ByVal o As Object)
        Try
            Runtime.InteropServices.Marshal.ReleaseComObject(o)
            o = Nothing
        Catch ex As Exception
            o = Nothing
        Finally
#If DEBUG Then
            GC.Collect()
#End If
        End Try
    End Sub
End Class

