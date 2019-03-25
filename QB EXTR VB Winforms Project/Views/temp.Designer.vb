<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class temp
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnRunExcelForATG = New System.Windows.Forms.Button()
        Me.SuspendLayout
        '
        'btnRunExcelForATG
        '
        Me.btnRunExcelForATG.Location = New System.Drawing.Point(220, 333)
        Me.btnRunExcelForATG.Name = "btnRunExcelForATG"
        Me.btnRunExcelForATG.Size = New System.Drawing.Size(324, 63)
        Me.btnRunExcelForATG.TabIndex = 0
        Me.btnRunExcelForATG.Text = "Create Excel Report for ATG"
        Me.btnRunExcelForATG.UseVisualStyleBackColor = true
        '
        'temp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(756, 462)
        Me.Controls.Add(Me.btnRunExcelForATG)
        Me.Name = "temp"
        Me.Text = "temp"
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents btnRunExcelForATG As System.Windows.Forms.Button
End Class
