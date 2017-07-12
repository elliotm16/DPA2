<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Menu
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
        Me.btnSetUpDB = New System.Windows.Forms.Button()
        Me.btnForm2 = New System.Windows.Forms.Button()
        Me.btnForm3 = New System.Windows.Forms.Button()
        Me.btnForm4 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnSetUpDB
        '
        Me.btnSetUpDB.Location = New System.Drawing.Point(12, 12)
        Me.btnSetUpDB.Name = "btnSetUpDB"
        Me.btnSetUpDB.Size = New System.Drawing.Size(120, 30)
        Me.btnSetUpDB.TabIndex = 0
        Me.btnSetUpDB.Text = "Set up database"
        Me.btnSetUpDB.UseVisualStyleBackColor = True
        '
        'btnForm2
        '
        Me.btnForm2.Location = New System.Drawing.Point(12, 48)
        Me.btnForm2.Name = "btnForm2"
        Me.btnForm2.Size = New System.Drawing.Size(120, 30)
        Me.btnForm2.TabIndex = 1
        Me.btnForm2.Text = "Form2"
        Me.btnForm2.UseVisualStyleBackColor = True
        '
        'btnForm3
        '
        Me.btnForm3.Location = New System.Drawing.Point(12, 84)
        Me.btnForm3.Name = "btnForm3"
        Me.btnForm3.Size = New System.Drawing.Size(120, 30)
        Me.btnForm3.TabIndex = 2
        Me.btnForm3.Text = "Form3"
        Me.btnForm3.UseVisualStyleBackColor = True
        '
        'btnForm4
        '
        Me.btnForm4.Location = New System.Drawing.Point(12, 120)
        Me.btnForm4.Name = "btnForm4"
        Me.btnForm4.Size = New System.Drawing.Size(120, 30)
        Me.btnForm4.TabIndex = 3
        Me.btnForm4.Text = "Form4"
        Me.btnForm4.UseVisualStyleBackColor = True
        '
        'Menu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.btnForm4)
        Me.Controls.Add(Me.btnForm3)
        Me.Controls.Add(Me.btnForm2)
        Me.Controls.Add(Me.btnSetUpDB)
        Me.Name = "Menu"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnSetUpDB As System.Windows.Forms.Button
    Friend WithEvents btnForm2 As System.Windows.Forms.Button
    Friend WithEvents btnForm3 As System.Windows.Forms.Button
    Friend WithEvents btnForm4 As System.Windows.Forms.Button

End Class
