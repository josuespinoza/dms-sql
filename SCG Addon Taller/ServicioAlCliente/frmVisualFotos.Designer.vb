<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVisualFotos
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    ' <System.Diagnostics.DebuggerNonUserCode()> _
    ' Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    '     Try
    '         If disposing AndAlso components IsNot Nothing Then
    '            components.Dispose()
    '        End If
    '    Finally
    '        MyBase.Dispose(disposing)
    '    End Try
    'End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVisualFotos))
        Me.BackPanel = New System.Windows.Forms.Panel()
        Me.label1 = New System.Windows.Forms.Label()
        Me.bntCopia = New System.Windows.Forms.Button()
        Me.btnAleja = New System.Windows.Forms.Button()
        Me.btnAcerca = New System.Windows.Forms.Button()
        Me.lblNombreAgenda = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BackPanel
        '
        resources.ApplyResources(Me.BackPanel, "BackPanel")
        Me.BackPanel.BackColor = System.Drawing.Color.White
        Me.BackPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.BackPanel.Name = "BackPanel"
        '
        'label1
        '
        resources.ApplyResources(Me.label1, "label1")
        Me.label1.Name = "label1"
        '
        'bntCopia
        '
        Me.bntCopia.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.bntCopia, "bntCopia")
        Me.bntCopia.Image = Global.DMS_Addon.My.Resources.Resources.copiar
        Me.bntCopia.Name = "bntCopia"
        Me.bntCopia.UseVisualStyleBackColor = False
        '
        'btnAleja
        '
        resources.ApplyResources(Me.btnAleja, "btnAleja")
        Me.btnAleja.Image = Global.DMS_Addon.My.Resources.Resources.zoom_menos
        Me.btnAleja.Name = "btnAleja"
        Me.btnAleja.UseVisualStyleBackColor = True
        '
        'btnAcerca
        '
        resources.ApplyResources(Me.btnAcerca, "btnAcerca")
        Me.btnAcerca.Image = Global.DMS_Addon.My.Resources.Resources.zoom_mas
        Me.btnAcerca.Name = "btnAcerca"
        Me.btnAcerca.UseVisualStyleBackColor = True
        '
        'lblNombreAgenda
        '
        resources.ApplyResources(Me.lblNombreAgenda, "lblNombreAgenda")
        Me.lblNombreAgenda.Name = "lblNombreAgenda"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.PictureBox1)
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.Name = "Panel1"
        '
        'PictureBox1
        '
        Me.PictureBox1.Cursor = System.Windows.Forms.Cursors.Hand
        resources.ApplyResources(Me.PictureBox1, "PictureBox1")
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.TabStop = False
        '
        'frmVisualFotos
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblNombreAgenda)
        Me.Controls.Add(Me.bntCopia)
        Me.Controls.Add(Me.btnAleja)
        Me.Controls.Add(Me.btnAcerca)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.BackPanel)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmVisualFotos"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Panel1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BackPanel As System.Windows.Forms.Panel
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents bntCopia As System.Windows.Forms.Button
    Friend WithEvents btnAleja As System.Windows.Forms.Button
    Friend WithEvents btnAcerca As System.Windows.Forms.Button
    Friend WithEvents lblNombreAgenda As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class
