Namespace SCG_User_Interface

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmRptOrdenGarantia
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptOrdenGarantia))
            Me.optFord = New System.Windows.Forms.RadioButton
            Me.optVW = New System.Windows.Forms.RadioButton
            Me.GroupBox1 = New System.Windows.Forms.GroupBox
            Me.picRepuesto = New System.Windows.Forms.PictureBox
            Me.txtNoOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label2 = New System.Windows.Forms.Label
            Me.Label1 = New System.Windows.Forms.Label
            Me.btnAceptar = New System.Windows.Forms.Button
            Me.btnCerrar = New System.Windows.Forms.Button
            Me.SubBOTs = New Buscador.SubBuscador
            Me.SubReportsOrdenes = New ComponenteCristalReport.SubReportView
            Me.GroupBox1.SuspendLayout()
            CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'optFord
            '
            resources.ApplyResources(Me.optFord, "optFord")
            Me.optFord.Checked = True
            Me.optFord.Name = "optFord"
            Me.optFord.TabStop = True
            Me.optFord.UseVisualStyleBackColor = True
            '
            'optVW
            '
            resources.ApplyResources(Me.optVW, "optVW")
            Me.optVW.Name = "optVW"
            Me.optVW.UseVisualStyleBackColor = True
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.picRepuesto)
            Me.GroupBox1.Controls.Add(Me.txtNoOrden)
            Me.GroupBox1.Controls.Add(Me.Label2)
            Me.GroupBox1.Controls.Add(Me.Label1)
            Me.GroupBox1.Controls.Add(Me.optFord)
            Me.GroupBox1.Controls.Add(Me.optVW)
            resources.ApplyResources(Me.GroupBox1, "GroupBox1")
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.TabStop = False
            '
            'picRepuesto
            '
            Me.picRepuesto.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.picRepuesto, "picRepuesto")
            Me.picRepuesto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.picRepuesto.Name = "picRepuesto"
            Me.picRepuesto.TabStop = False
            '
            'txtNoOrden
            '
            Me.txtNoOrden.AceptaNegativos = False
            Me.txtNoOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoOrden.EstiloSBO = True
            resources.ApplyResources(Me.txtNoOrden, "txtNoOrden")
            Me.txtNoOrden.MaxDecimales = 0
            Me.txtNoOrden.MaxEnteros = 0
            Me.txtNoOrden.Millares = False
            Me.txtNoOrden.Name = "txtNoOrden"
            Me.txtNoOrden.Size_AdjustableHeight = 20
            Me.txtNoOrden.TeclasDeshacer = True
            Me.txtNoOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'Label1
            '
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'btnAceptar
            '
            resources.ApplyResources(Me.btnAceptar, "btnAceptar")
            Me.btnAceptar.Name = "btnAceptar"
            Me.btnAceptar.UseVisualStyleBackColor = True
            '
            'btnCerrar
            '
            resources.ApplyResources(Me.btnCerrar, "btnCerrar")
            Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCerrar.Name = "btnCerrar"
            Me.btnCerrar.UseVisualStyleBackColor = True
            '
            'SubBOTs
            '
            Me.SubBOTs.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.SubBOTs.Barra_Titulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBOTs.ConsultarDBPorFiltrado = False
            Me.SubBOTs.Criterios = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBOTs.Criterios_Ocultos = 0
            Me.SubBOTs.Criterios_OcultosEx = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBOTs.IN_DataTable = Nothing
            resources.ApplyResources(Me.SubBOTs, "SubBOTs")
            Me.SubBOTs.MultiSeleccion = False
            Me.SubBOTs.Name = "SubBOTs"
            Me.SubBOTs.SQL_Cnn = Nothing
            Me.SubBOTs.Tabla = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBOTs.Titulos = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBOTs.Where = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'SubReportsOrdenes
            '
            Me.SubReportsOrdenes.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.SubReportsOrdenes, "SubReportsOrdenes")
            Me.SubReportsOrdenes.Name = "SubReportsOrdenes"
            Me.SubReportsOrdenes.P_BarraTitulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubReportsOrdenes.P_CompanyName = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubReportsOrdenes.P_DataBase = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubReportsOrdenes.P_Filename = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubReportsOrdenes.P_NCopias = 0
            Me.SubReportsOrdenes.P_Owner = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubReportsOrdenes.P_ParArray = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubReportsOrdenes.P_Password = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubReportsOrdenes.P_Server = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubReportsOrdenes.P_User = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubReportsOrdenes.P_WorkFolder = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'frmRptOrdenGarantia
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.CancelButton = Me.btnCerrar
            Me.Controls.Add(Me.GroupBox1)
            Me.Controls.Add(Me.btnAceptar)
            Me.Controls.Add(Me.btnCerrar)
            Me.Controls.Add(Me.SubBOTs)
            Me.Controls.Add(Me.SubReportsOrdenes)
            Me.MaximizeBox = False
            Me.Name = "frmRptOrdenGarantia"
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents optFord As System.Windows.Forms.RadioButton
        Friend WithEvents optVW As System.Windows.Forms.RadioButton
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents txtNoOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents btnAceptar As System.Windows.Forms.Button
        Friend WithEvents btnCerrar As System.Windows.Forms.Button
        Friend WithEvents picRepuesto As System.Windows.Forms.PictureBox
        Friend WithEvents SubBOTs As Buscador.SubBuscador
        Friend WithEvents SubReportsOrdenes As ComponenteCristalReport.SubReportView
    End Class

End Namespace
