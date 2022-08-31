<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditarElementoCita
    Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEditarElementoCita))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.textBoxMarca = New System.Windows.Forms.TextBox
        Me.textBoxModelo = New System.Windows.Forms.TextBox
        Me.textBoxEstilo = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.dateTimePickerFechaUS = New System.Windows.Forms.DateTimePicker
        Me.dateTimePickerFechaPS = New System.Windows.Forms.DateTimePicker
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonCancelar = New System.Windows.Forms.Button
        Me.textBoxCliente = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.textBoxVin = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'textBoxMarca
        '
        resources.ApplyResources(Me.textBoxMarca, "textBoxMarca")
        Me.textBoxMarca.Name = "textBoxMarca"
        '
        'textBoxModelo
        '
        resources.ApplyResources(Me.textBoxModelo, "textBoxModelo")
        Me.textBoxModelo.Name = "textBoxModelo"
        '
        'textBoxEstilo
        '
        resources.ApplyResources(Me.textBoxEstilo, "textBoxEstilo")
        Me.textBoxEstilo.Name = "textBoxEstilo"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'dateTimePickerFechaUS
        '
        resources.ApplyResources(Me.dateTimePickerFechaUS, "dateTimePickerFechaUS")
        Me.dateTimePickerFechaUS.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dateTimePickerFechaUS.Name = "dateTimePickerFechaUS"
        '
        'dateTimePickerFechaPS
        '
        resources.ApplyResources(Me.dateTimePickerFechaPS, "dateTimePickerFechaPS")
        Me.dateTimePickerFechaPS.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dateTimePickerFechaPS.Name = "dateTimePickerFechaPS"
        '
        'ButtonAceptar
        '
        resources.ApplyResources(Me.ButtonAceptar, "ButtonAceptar")
        Me.ButtonAceptar.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonAceptar.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.ButtonAceptar.Name = "ButtonAceptar"
        '
        'ButtonCancelar
        '
        resources.ApplyResources(Me.ButtonCancelar, "ButtonCancelar")
        Me.ButtonCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancelar.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.ButtonCancelar.Name = "ButtonCancelar"
        '
        'textBoxCliente
        '
        resources.ApplyResources(Me.textBoxCliente, "textBoxCliente")
        Me.textBoxCliente.Name = "textBoxCliente"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'textBoxVin
        '
        resources.ApplyResources(Me.textBoxVin, "textBoxVin")
        Me.textBoxVin.Name = "textBoxVin"
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'frmEditarElementoCita
        '
        Me.AcceptButton = Me.ButtonAceptar
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonCancelar
        Me.Controls.Add(Me.ButtonCancelar)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.dateTimePickerFechaPS)
        Me.Controls.Add(Me.dateTimePickerFechaUS)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.textBoxModelo)
        Me.Controls.Add(Me.textBoxCliente)
        Me.Controls.Add(Me.textBoxEstilo)
        Me.Controls.Add(Me.textBoxVin)
        Me.Controls.Add(Me.textBoxMarca)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label2)
        Me.Name = "frmEditarElementoCita"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents textBoxMarca As System.Windows.Forms.TextBox
    Friend WithEvents textBoxModelo As System.Windows.Forms.TextBox
    Friend WithEvents textBoxEstilo As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents dateTimePickerFechaUS As System.Windows.Forms.DateTimePicker
    Friend WithEvents dateTimePickerFechaPS As System.Windows.Forms.DateTimePicker
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonCancelar As System.Windows.Forms.Button
    Friend WithEvents textBoxCliente As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents textBoxVin As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
End Class
