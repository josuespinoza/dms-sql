<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DetalleOtrosGastos
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DetalleOtrosGastos))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.txtConcepto = New NEWTEXTBOX.NEWTEXTBOX_CTRL
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.DocEntryDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DocNumDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.FechaDocDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.FechaContDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LineNumDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DscriptionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CommentsDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PrecioConDescuentoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.UNoOTDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.UConceptDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ConceptoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TipoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.m_bsrcOtrosGastos = New System.Windows.Forms.BindingSource(Me.components)
        Me.m_dstOtrosGastos = New DMSOneFramework.OtrosGastosDataSet
        Me.txtTotal = New NEWTEXTBOX.NEWTEXTBOX_CTRL
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.m_bsrcOtrosGastos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.m_dstOtrosGastos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtConcepto
        '
        Me.txtConcepto.AceptaNegativos = False
        resources.ApplyResources(Me.txtConcepto, "txtConcepto")
        Me.txtConcepto.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.txtConcepto.EstiloSBO = True
        Me.txtConcepto.ForeColor = System.Drawing.Color.Black
        Me.txtConcepto.MaxDecimales = 0
        Me.txtConcepto.MaxEnteros = 0
        Me.txtConcepto.Millares = False
        Me.txtConcepto.Name = "txtConcepto"
        Me.txtConcepto.ReadOnly = True
        Me.txtConcepto.Size_AdjustableHeight = 20
        Me.txtConcepto.TeclasDeshacer = True
        Me.txtConcepto.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.Label2.Name = "Label2"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.DataGridView1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        resources.ApplyResources(Me.DataGridView1, "DataGridView1")
        Me.DataGridView1.AutoGenerateColumns = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DocEntryDataGridViewTextBoxColumn, Me.DocNumDataGridViewTextBoxColumn, Me.FechaDocDataGridViewTextBoxColumn, Me.FechaContDataGridViewTextBoxColumn, Me.LineNumDataGridViewTextBoxColumn, Me.DscriptionDataGridViewTextBoxColumn, Me.CommentsDataGridViewTextBoxColumn, Me.PrecioConDescuentoDataGridViewTextBoxColumn, Me.UNoOTDataGridViewTextBoxColumn, Me.UConceptDataGridViewTextBoxColumn, Me.ConceptoDataGridViewTextBoxColumn, Me.TipoDataGridViewTextBoxColumn})
        Me.DataGridView1.DataSource = Me.m_bsrcOtrosGastos
        Me.DataGridView1.GridColor = System.Drawing.SystemColors.Control
        Me.DataGridView1.Name = "DataGridView1"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridView1.RowHeadersVisible = False
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(44, Byte), Integer))
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.DataGridView1.RowsDefaultCellStyle = DataGridViewCellStyle4
        '
        'DocEntryDataGridViewTextBoxColumn
        '
        Me.DocEntryDataGridViewTextBoxColumn.DataPropertyName = "DocEntry"
        resources.ApplyResources(Me.DocEntryDataGridViewTextBoxColumn, "DocEntryDataGridViewTextBoxColumn")
        Me.DocEntryDataGridViewTextBoxColumn.Name = "DocEntryDataGridViewTextBoxColumn"
        '
        'DocNumDataGridViewTextBoxColumn
        '
        Me.DocNumDataGridViewTextBoxColumn.DataPropertyName = "DocNum"
        resources.ApplyResources(Me.DocNumDataGridViewTextBoxColumn, "DocNumDataGridViewTextBoxColumn")
        Me.DocNumDataGridViewTextBoxColumn.Name = "DocNumDataGridViewTextBoxColumn"
        '
        'FechaDocDataGridViewTextBoxColumn
        '
        Me.FechaDocDataGridViewTextBoxColumn.DataPropertyName = "FechaDoc"
        resources.ApplyResources(Me.FechaDocDataGridViewTextBoxColumn, "FechaDocDataGridViewTextBoxColumn")
        Me.FechaDocDataGridViewTextBoxColumn.Name = "FechaDocDataGridViewTextBoxColumn"
        '
        'FechaContDataGridViewTextBoxColumn
        '
        Me.FechaContDataGridViewTextBoxColumn.DataPropertyName = "FechaCont"
        resources.ApplyResources(Me.FechaContDataGridViewTextBoxColumn, "FechaContDataGridViewTextBoxColumn")
        Me.FechaContDataGridViewTextBoxColumn.Name = "FechaContDataGridViewTextBoxColumn"
        '
        'LineNumDataGridViewTextBoxColumn
        '
        Me.LineNumDataGridViewTextBoxColumn.DataPropertyName = "LineNum"
        resources.ApplyResources(Me.LineNumDataGridViewTextBoxColumn, "LineNumDataGridViewTextBoxColumn")
        Me.LineNumDataGridViewTextBoxColumn.Name = "LineNumDataGridViewTextBoxColumn"
        '
        'DscriptionDataGridViewTextBoxColumn
        '
        Me.DscriptionDataGridViewTextBoxColumn.DataPropertyName = "Dscription"
        resources.ApplyResources(Me.DscriptionDataGridViewTextBoxColumn, "DscriptionDataGridViewTextBoxColumn")
        Me.DscriptionDataGridViewTextBoxColumn.Name = "DscriptionDataGridViewTextBoxColumn"
        '
        'CommentsDataGridViewTextBoxColumn
        '
        Me.CommentsDataGridViewTextBoxColumn.DataPropertyName = "Comments"
        resources.ApplyResources(Me.CommentsDataGridViewTextBoxColumn, "CommentsDataGridViewTextBoxColumn")
        Me.CommentsDataGridViewTextBoxColumn.Name = "CommentsDataGridViewTextBoxColumn"
        '
        'PrecioConDescuentoDataGridViewTextBoxColumn
        '
        Me.PrecioConDescuentoDataGridViewTextBoxColumn.DataPropertyName = "PrecioConDescuento"
        DataGridViewCellStyle2.Format = "N2"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.PrecioConDescuentoDataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle2
        resources.ApplyResources(Me.PrecioConDescuentoDataGridViewTextBoxColumn, "PrecioConDescuentoDataGridViewTextBoxColumn")
        Me.PrecioConDescuentoDataGridViewTextBoxColumn.Name = "PrecioConDescuentoDataGridViewTextBoxColumn"
        '
        'UNoOTDataGridViewTextBoxColumn
        '
        Me.UNoOTDataGridViewTextBoxColumn.DataPropertyName = "U_NoOT"
        resources.ApplyResources(Me.UNoOTDataGridViewTextBoxColumn, "UNoOTDataGridViewTextBoxColumn")
        Me.UNoOTDataGridViewTextBoxColumn.Name = "UNoOTDataGridViewTextBoxColumn"
        '
        'UConceptDataGridViewTextBoxColumn
        '
        Me.UConceptDataGridViewTextBoxColumn.DataPropertyName = "U_Concept"
        resources.ApplyResources(Me.UConceptDataGridViewTextBoxColumn, "UConceptDataGridViewTextBoxColumn")
        Me.UConceptDataGridViewTextBoxColumn.Name = "UConceptDataGridViewTextBoxColumn"
        '
        'ConceptoDataGridViewTextBoxColumn
        '
        Me.ConceptoDataGridViewTextBoxColumn.DataPropertyName = "Concepto"
        resources.ApplyResources(Me.ConceptoDataGridViewTextBoxColumn, "ConceptoDataGridViewTextBoxColumn")
        Me.ConceptoDataGridViewTextBoxColumn.Name = "ConceptoDataGridViewTextBoxColumn"
        '
        'TipoDataGridViewTextBoxColumn
        '
        Me.TipoDataGridViewTextBoxColumn.DataPropertyName = "Tipo"
        resources.ApplyResources(Me.TipoDataGridViewTextBoxColumn, "TipoDataGridViewTextBoxColumn")
        Me.TipoDataGridViewTextBoxColumn.Name = "TipoDataGridViewTextBoxColumn"
        '
        'm_bsrcOtrosGastos
        '
        Me.m_bsrcOtrosGastos.DataMember = "SCGTA_VW_OtrosGastos"
        Me.m_bsrcOtrosGastos.DataSource = Me.m_dstOtrosGastos
        '
        'm_dstOtrosGastos
        '
        Me.m_dstOtrosGastos.DataSetName = "OtrosGastosDataSet"
        Me.m_dstOtrosGastos.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'txtTotal
        '
        Me.txtTotal.AceptaNegativos = False
        Me.txtTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.txtTotal.EstiloSBO = True
        resources.ApplyResources(Me.txtTotal, "txtTotal")
        Me.txtTotal.ForeColor = System.Drawing.Color.Black
        Me.txtTotal.MaxDecimales = 0
        Me.txtTotal.MaxEnteros = 0
        Me.txtTotal.Millares = False
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.ReadOnly = True
        Me.txtTotal.Size_AdjustableHeight = 20
        Me.txtTotal.TeclasDeshacer = True
        Me.txtTotal.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.Label4.Name = "Label4"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.SCG_User_Interface.My.Resources.Resources.triangulo1
        resources.ApplyResources(Me.PictureBox1, "PictureBox1")
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.TabStop = False
        '
        'DetalleOtrosGastos
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtTotal)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.txtConcepto)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Name = "DetalleOtrosGastos"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.m_bsrcOtrosGastos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.m_dstOtrosGastos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents txtConcepto As NEWTEXTBOX.NEWTEXTBOX_CTRL
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents m_bsrcOtrosGastos As System.Windows.Forms.BindingSource
    Friend WithEvents m_dstOtrosGastos As DMSOneFramework.OtrosGastosDataSet
    Friend WithEvents txtTotal As NEWTEXTBOX.NEWTEXTBOX_CTRL
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents DocEntryDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DocNumDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FechaDocDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FechaContDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LineNumDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DscriptionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CommentsDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PrecioConDescuentoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UNoOTDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UConceptDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ConceptoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TipoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
