Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmReporteItemsXOrden
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReporteItemsXOrden))
            Me.txtNumeroOT = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label4 = New System.Windows.Forms.Label
            Me.btncerrar = New System.Windows.Forms.Button
            Me.btnBuscar = New System.Windows.Forms.Button
            Me.GroupBox1 = New System.Windows.Forms.GroupBox
            Me.picRepuesto = New System.Windows.Forms.PictureBox
            Me.Panel2 = New System.Windows.Forms.Panel
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.GroupBox2 = New System.Windows.Forms.GroupBox
            Me.chkOtros = New System.Windows.Forms.CheckBox
            Me.chkServicios = New System.Windows.Forms.CheckBox
            Me.chkSuminstros = New System.Windows.Forms.CheckBox
            Me.chkPaquetes = New System.Windows.Forms.CheckBox
            Me.chkServiciosExternos = New System.Windows.Forms.CheckBox
            Me.chkRepuestos = New System.Windows.Forms.CheckBox
            Me.Panel3 = New System.Windows.Forms.Panel
            Me.Panel4 = New System.Windows.Forms.Panel
            Me.SubBOTs = New Buscador.SubBuscador
            Me.GroupBox1.SuspendLayout()
            CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.GroupBox2.SuspendLayout()
            Me.SuspendLayout()
            '
            'txtNumeroOT
            '
            Me.txtNumeroOT.AceptaNegativos = False
            Me.txtNumeroOT.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNumeroOT.EstiloSBO = True
            resources.ApplyResources(Me.txtNumeroOT, "txtNumeroOT")
            Me.txtNumeroOT.MaxDecimales = 0
            Me.txtNumeroOT.MaxEnteros = 0
            Me.txtNumeroOT.Millares = False
            Me.txtNumeroOT.Name = "txtNumeroOT"
            Me.txtNumeroOT.Size_AdjustableHeight = 20
            Me.txtNumeroOT.TeclasDeshacer = True
            Me.txtNumeroOT.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label4
            '
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.Name = "Label4"
            '
            'btncerrar
            '
            resources.ApplyResources(Me.btncerrar, "btncerrar")
            Me.btncerrar.ForeColor = System.Drawing.Color.Black
            Me.btncerrar.Name = "btncerrar"
            '
            'btnBuscar
            '
            resources.ApplyResources(Me.btnBuscar, "btnBuscar")
            Me.btnBuscar.ForeColor = System.Drawing.Color.Black
            Me.btnBuscar.Name = "btnBuscar"
            '
            'GroupBox1
            '
            Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
            Me.GroupBox1.Controls.Add(Me.picRepuesto)
            Me.GroupBox1.Controls.Add(Me.Panel2)
            Me.GroupBox1.Controls.Add(Me.Panel1)
            Me.GroupBox1.Controls.Add(Me.lblLine1)
            Me.GroupBox1.Controls.Add(Me.txtNumeroOT)
            Me.GroupBox1.Controls.Add(Me.Label4)
            resources.ApplyResources(Me.GroupBox1, "GroupBox1")
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.TabStop = False
            '
            'picRepuesto
            '
            Me.picRepuesto.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.picRepuesto, "picRepuesto")
            Me.picRepuesto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.picRepuesto.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.picRepuesto.Name = "picRepuesto"
            Me.picRepuesto.TabStop = False
            '
            'Panel2
            '
            Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.Panel2, "Panel2")
            Me.Panel2.Name = "Panel2"
            '
            'Panel1
            '
            Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.Panel1, "Panel1")
            Me.Panel1.Name = "Panel1"
            '
            'lblLine1
            '
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.Name = "lblLine1"
            '
            'GroupBox2
            '
            Me.GroupBox2.BackColor = System.Drawing.SystemColors.Control
            Me.GroupBox2.Controls.Add(Me.chkOtros)
            Me.GroupBox2.Controls.Add(Me.chkServicios)
            Me.GroupBox2.Controls.Add(Me.chkSuminstros)
            Me.GroupBox2.Controls.Add(Me.chkPaquetes)
            Me.GroupBox2.Controls.Add(Me.chkServiciosExternos)
            Me.GroupBox2.Controls.Add(Me.chkRepuestos)
            Me.GroupBox2.Controls.Add(Me.Panel3)
            Me.GroupBox2.Controls.Add(Me.Panel4)
            resources.ApplyResources(Me.GroupBox2, "GroupBox2")
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.TabStop = False
            '
            'chkOtros
            '
            resources.ApplyResources(Me.chkOtros, "chkOtros")
            Me.chkOtros.Checked = True
            Me.chkOtros.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkOtros.Name = "chkOtros"
            Me.chkOtros.UseVisualStyleBackColor = True
            '
            'chkServicios
            '
            resources.ApplyResources(Me.chkServicios, "chkServicios")
            Me.chkServicios.Checked = True
            Me.chkServicios.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkServicios.Name = "chkServicios"
            Me.chkServicios.UseVisualStyleBackColor = True
            '
            'chkSuminstros
            '
            resources.ApplyResources(Me.chkSuminstros, "chkSuminstros")
            Me.chkSuminstros.Checked = True
            Me.chkSuminstros.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkSuminstros.Name = "chkSuminstros"
            Me.chkSuminstros.UseVisualStyleBackColor = True
            '
            'chkPaquetes
            '
            resources.ApplyResources(Me.chkPaquetes, "chkPaquetes")
            Me.chkPaquetes.Checked = True
            Me.chkPaquetes.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkPaquetes.Name = "chkPaquetes"
            Me.chkPaquetes.UseVisualStyleBackColor = True
            '
            'chkServiciosExternos
            '
            resources.ApplyResources(Me.chkServiciosExternos, "chkServiciosExternos")
            Me.chkServiciosExternos.Checked = True
            Me.chkServiciosExternos.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkServiciosExternos.Name = "chkServiciosExternos"
            Me.chkServiciosExternos.UseVisualStyleBackColor = True
            '
            'chkRepuestos
            '
            resources.ApplyResources(Me.chkRepuestos, "chkRepuestos")
            Me.chkRepuestos.Checked = True
            Me.chkRepuestos.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkRepuestos.Name = "chkRepuestos"
            Me.chkRepuestos.UseVisualStyleBackColor = True
            '
            'Panel3
            '
            Me.Panel3.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.Panel3, "Panel3")
            Me.Panel3.Name = "Panel3"
            '
            'Panel4
            '
            Me.Panel4.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.Panel4, "Panel4")
            Me.Panel4.Name = "Panel4"
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
            'frmReporteItemsXOrden
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.SubBOTs)
            Me.Controls.Add(Me.GroupBox1)
            Me.Controls.Add(Me.GroupBox2)
            Me.Controls.Add(Me.btnBuscar)
            Me.Controls.Add(Me.btncerrar)
            Me.MaximizeBox = False
            Me.Name = "frmReporteItemsXOrden"
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).EndInit()
            Me.GroupBox2.ResumeLayout(False)
            Me.GroupBox2.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents txtNumeroOT As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents btncerrar As System.Windows.Forms.Button
        Friend WithEvents btnBuscar As System.Windows.Forms.Button
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents chkServicios As System.Windows.Forms.CheckBox
        Friend WithEvents chkSuminstros As System.Windows.Forms.CheckBox
        Friend WithEvents chkPaquetes As System.Windows.Forms.CheckBox
        Friend WithEvents chkServiciosExternos As System.Windows.Forms.CheckBox
        Friend WithEvents chkRepuestos As System.Windows.Forms.CheckBox
        Friend WithEvents Panel3 As System.Windows.Forms.Panel
        Friend WithEvents Panel4 As System.Windows.Forms.Panel
        Friend WithEvents chkOtros As System.Windows.Forms.CheckBox
        Friend WithEvents picRepuesto As System.Windows.Forms.PictureBox
        Friend WithEvents SubBOTs As Buscador.SubBuscador
    End Class
End Namespace
