<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProgramacionCitasAutom
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProgramacionCitasAutom))
        Me.txtSalida = New System.Windows.Forms.TextBox
        Me.btnGenerarCitas = New System.Windows.Forms.Button
        Me.AgendaPropuestaCitas1 = New SCG.UX.Windows.CitasAutomaticas.AgendaPropuestaCitas
        Me.SuspendLayout()
        '
        'txtSalida
        '
        resources.ApplyResources(Me.txtSalida, "txtSalida")
        Me.txtSalida.Name = "txtSalida"
        Me.txtSalida.ReadOnly = True
        '
        'btnGenerarCitas
        '
        Me.btnGenerarCitas.ForeColor = System.Drawing.Color.Black
        resources.ApplyResources(Me.btnGenerarCitas, "btnGenerarCitas")
        Me.btnGenerarCitas.Name = "btnGenerarCitas"
        '
        'AgendaPropuestaCitas1
        '
        Me.AgendaPropuestaCitas1.AdministradorPropuestasCitas = Nothing
        Me.AgendaPropuestaCitas1.AgendaActual = Nothing
        Me.AgendaPropuestaCitas1.BackColor = System.Drawing.SystemColors.Control
        Me.AgendaPropuestaCitas1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.AgendaPropuestaCitas1, "AgendaPropuestaCitas1")
        Me.AgendaPropuestaCitas1.Name = "AgendaPropuestaCitas1"
        '
        'frmProgramacionCitasAutom
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnGenerarCitas)
        Me.Controls.Add(Me.txtSalida)
        Me.Controls.Add(Me.AgendaPropuestaCitas1)
        Me.Name = "frmProgramacionCitasAutom"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents AgendaPropuestaCitas1 As SCG.UX.Windows.CitasAutomaticas.AgendaPropuestaCitas
    Friend WithEvents txtSalida As System.Windows.Forms.TextBox
    Friend WithEvents btnGenerarCitas As System.Windows.Forms.Button
End Class
