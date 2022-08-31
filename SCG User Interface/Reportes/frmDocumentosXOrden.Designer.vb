Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmDocumentosXOrden
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDocumentosXOrden))
            Me.Label1 = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.txtOrdendeTrabajo = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.btncerrar = New System.Windows.Forms.Button
            Me.btnBuscar = New System.Windows.Forms.Button
            Me.picRepuesto = New System.Windows.Forms.PictureBox
            CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'Label2
            '
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label2.Name = "Label2"
            '
            'txtOrdendeTrabajo
            '
            Me.txtOrdendeTrabajo.AceptaNegativos = False
            Me.txtOrdendeTrabajo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtOrdendeTrabajo.EstiloSBO = True
            resources.ApplyResources(Me.txtOrdendeTrabajo, "txtOrdendeTrabajo")
            Me.txtOrdendeTrabajo.MaxDecimales = 0
            Me.txtOrdendeTrabajo.MaxEnteros = 0
            Me.txtOrdendeTrabajo.Millares = False
            Me.txtOrdendeTrabajo.Name = "txtOrdendeTrabajo"
            Me.txtOrdendeTrabajo.ReadOnly = True
            Me.txtOrdendeTrabajo.Size_AdjustableHeight = 20
            Me.txtOrdendeTrabajo.TeclasDeshacer = True
            Me.txtOrdendeTrabajo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'btncerrar
            '
            resources.ApplyResources(Me.btncerrar, "btncerrar")
            Me.btncerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btncerrar.ForeColor = System.Drawing.Color.Black
            Me.btncerrar.Name = "btncerrar"
            '
            'btnBuscar
            '
            resources.ApplyResources(Me.btnBuscar, "btnBuscar")
            Me.btnBuscar.ForeColor = System.Drawing.Color.Black
            Me.btnBuscar.Name = "btnBuscar"
            '
            'picRepuesto
            '
            Me.picRepuesto.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.picRepuesto.BackgroundImage = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.picRepuesto.ErrorImage = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.picRepuesto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            resources.ApplyResources(Me.picRepuesto, "picRepuesto")
            Me.picRepuesto.Name = "picRepuesto"
            Me.picRepuesto.TabStop = False
            '
            'frmDocumentosXOrden
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.picRepuesto)
            Me.Controls.Add(Me.btncerrar)
            Me.Controls.Add(Me.btnBuscar)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.txtOrdendeTrabajo)
            Me.MaximizeBox = False
            Me.Name = "frmDocumentosXOrden"
            CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents txtOrdendeTrabajo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents btncerrar As System.Windows.Forms.Button
        Friend WithEvents btnBuscar As System.Windows.Forms.Button
        Friend WithEvents picRepuesto As System.Windows.Forms.PictureBox
    End Class
End Namespace