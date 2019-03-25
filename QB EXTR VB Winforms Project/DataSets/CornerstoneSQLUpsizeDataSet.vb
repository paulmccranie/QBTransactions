Partial Class CornerstoneSQLUpsizeDataSet
    

    Partial Class SecondaryDataTable

        Private Sub SecondaryDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me._secondary_streetColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

    Partial Class ReferencesDataTable

        Private Sub ReferencesDataTable_ReferencesRowChanging(ByVal sender As System.Object, ByVal e As ReferencesRowChangeEvent) Handles Me.ReferencesRowChanging

        End Sub

    End Class

    Partial Class PresidentDataTable

        Private Sub PresidentDataTable_PresidentRowChanging(ByVal sender As System.Object, ByVal e As PresidentRowChangeEvent) Handles Me.PresidentRowChanging

        End Sub

    End Class

End Class
