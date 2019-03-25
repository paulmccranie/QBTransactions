<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StatementGenerator
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
        Me.components = New System.ComponentModel.Container()
        Me.dtStart = New System.Windows.Forms.DateTimePicker()
        Me.dtEnd = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.radioMonthly = New System.Windows.Forms.RadioButton()
        Me.radioCustom = New System.Windows.Forms.RadioButton()
        Me.groupCustom = New System.Windows.Forms.GroupBox()
        Me.lstCompanies = New System.Windows.Forms.ListBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.radioPDF = New System.Windows.Forms.RadioButton()
        Me.radioExcel = New System.Windows.Forms.RadioButton()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.contextMenuRemoveCompanyFromList = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1.SuspendLayout
        Me.groupCustom.SuspendLayout
        Me.GroupBox2.SuspendLayout
        Me.ContextMenuStrip1.SuspendLayout
        Me.SuspendLayout
        '
        'dtStart
        '
        Me.dtStart.Location = New System.Drawing.Point(127, 21)
        Me.dtStart.Name = "dtStart"
        Me.dtStart.Size = New System.Drawing.Size(200, 20)
        Me.dtStart.TabIndex = 0
        '
        'dtEnd
        '
        Me.dtEnd.Location = New System.Drawing.Point(127, 62)
        Me.dtEnd.Name = "dtEnd"
        Me.dtEnd.Size = New System.Drawing.Size(200, 20)
        Me.dtEnd.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = true
        Me.Label1.Location = New System.Drawing.Point(39, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Start"
        '
        'Label2
        '
        Me.Label2.AutoSize = true
        Me.Label2.Location = New System.Drawing.Point(42, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(26, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "End"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.groupCustom)
        Me.GroupBox1.Controls.Add(Me.radioCustom)
        Me.GroupBox1.Controls.Add(Me.radioMonthly)
        Me.GroupBox1.Location = New System.Drawing.Point(391, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(619, 248)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = false
        Me.GroupBox1.Text = "Time Frame"
        '
        'radioMonthly
        '
        Me.radioMonthly.AutoSize = true
        Me.radioMonthly.Location = New System.Drawing.Point(64, 39)
        Me.radioMonthly.Name = "radioMonthly"
        Me.radioMonthly.Size = New System.Drawing.Size(62, 17)
        Me.radioMonthly.TabIndex = 0
        Me.radioMonthly.TabStop = true
        Me.radioMonthly.Text = "Monthly"
        Me.radioMonthly.UseVisualStyleBackColor = true
        '
        'radioCustom
        '
        Me.radioCustom.AutoSize = true
        Me.radioCustom.Location = New System.Drawing.Point(64, 75)
        Me.radioCustom.Name = "radioCustom"
        Me.radioCustom.Size = New System.Drawing.Size(60, 17)
        Me.radioCustom.TabIndex = 1
        Me.radioCustom.TabStop = true
        Me.radioCustom.Text = "Custom"
        Me.radioCustom.UseVisualStyleBackColor = true
        '
        'groupCustom
        '
        Me.groupCustom.Controls.Add(Me.Label1)
        Me.groupCustom.Controls.Add(Me.Label2)
        Me.groupCustom.Controls.Add(Me.dtStart)
        Me.groupCustom.Controls.Add(Me.dtEnd)
        Me.groupCustom.Location = New System.Drawing.Point(120, 109)
        Me.groupCustom.Name = "groupCustom"
        Me.groupCustom.Size = New System.Drawing.Size(400, 100)
        Me.groupCustom.TabIndex = 2
        Me.groupCustom.TabStop = false
        '
        'lstCompanies
        '
        Me.lstCompanies.ContextMenuStrip = Me.ContextMenuStrip1
        Me.lstCompanies.FormattingEnabled = true
        Me.lstCompanies.Location = New System.Drawing.Point(1, 2)
        Me.lstCompanies.Name = "lstCompanies"
        Me.lstCompanies.Size = New System.Drawing.Size(341, 628)
        Me.lstCompanies.TabIndex = 5
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.radioPDF)
        Me.GroupBox2.Controls.Add(Me.radioExcel)
        Me.GroupBox2.Location = New System.Drawing.Point(392, 279)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(618, 152)
        Me.GroupBox2.TabIndex = 6
        Me.GroupBox2.TabStop = false
        Me.GroupBox2.Text = "Format"
        '
        'radioPDF
        '
        Me.radioPDF.AutoSize = true
        Me.radioPDF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.radioPDF.Image = Global.QBTransactionExtractor.My.Resources.Resources.PDFicon
        Me.radioPDF.Location = New System.Drawing.Point(397, 25)
        Me.radioPDF.Name = "radioPDF"
        Me.radioPDF.Size = New System.Drawing.Size(122, 108)
        Me.radioPDF.TabIndex = 1
        Me.radioPDF.TabStop = true
        Me.radioPDF.UseVisualStyleBackColor = true
        '
        'radioExcel
        '
        Me.radioExcel.AutoSize = true
        Me.radioExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.radioExcel.Image = Global.QBTransactionExtractor.My.Resources.Resources.excel
        Me.radioExcel.Location = New System.Drawing.Point(119, 29)
        Me.radioExcel.Name = "radioExcel"
        Me.radioExcel.Size = New System.Drawing.Size(114, 100)
        Me.radioExcel.TabIndex = 0
        Me.radioExcel.TabStop = true
        Me.radioExcel.UseVisualStyleBackColor = true
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(789, 517)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(221, 34)
        Me.btnGenerate.TabIndex = 7
        Me.btnGenerate.Text = "Generate"
        Me.btnGenerate.UseVisualStyleBackColor = true
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.contextMenuRemoveCompanyFromList})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(170, 26)
        '
        'contextMenuRemoveCompanyFromList
        '
        Me.contextMenuRemoveCompanyFromList.Name = "contextMenuRemoveCompanyFromList"
        Me.contextMenuRemoveCompanyFromList.Size = New System.Drawing.Size(169, 22)
        Me.contextMenuRemoveCompanyFromList.Text = "Remove From List"
        '
        'StatementGenerator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1080, 629)
        Me.Controls.Add(Me.btnGenerate)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.lstCompanies)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "StatementGenerator"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "StatementGenerator"
        Me.GroupBox1.ResumeLayout(false)
        Me.GroupBox1.PerformLayout
        Me.groupCustom.ResumeLayout(false)
        Me.groupCustom.PerformLayout
        Me.GroupBox2.ResumeLayout(false)
        Me.GroupBox2.PerformLayout
        Me.ContextMenuStrip1.ResumeLayout(false)
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents dtStart As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtEnd As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents groupCustom As System.Windows.Forms.GroupBox
    Friend WithEvents radioCustom As System.Windows.Forms.RadioButton
    Friend WithEvents radioMonthly As System.Windows.Forms.RadioButton
    Friend WithEvents lstCompanies As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents radioExcel As System.Windows.Forms.RadioButton
    Friend WithEvents radioPDF As System.Windows.Forms.RadioButton
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents contextMenuRemoveCompanyFromList As System.Windows.Forms.ToolStripMenuItem
End Class
