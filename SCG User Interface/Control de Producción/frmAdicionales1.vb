Option Compare Text

Imports DMSOneFramework
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGDataAccess

Namespace SCG_User_Interface

    Public Class frmAdicionales1
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents SubBusRecep As Buscador.SubBuscador
        Friend WithEvents EPPrueba As System.Windows.Forms.ErrorProvider
        Friend WithEvents btnEliminar As System.Windows.Forms.Button
        Friend WithEvents btnAgregar As System.Windows.Forms.Button
        Friend WithEvents btnGuardarCerrar As System.Windows.Forms.Button
        Friend WithEvents btnCerrar As System.Windows.Forms.Button
        Friend WithEvents btnSolicitudEspecificos As System.Windows.Forms.Button
        Friend WithEvents dtgRepuestosYActiv As System.Windows.Forms.DataGridView
        Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CheckDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents ItemCodeDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ItemNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Fase As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Tipo As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Duracion As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ObservacionesDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Stock As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Currency As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Precio As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents LineNum As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDEmpleado As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NombreEmpleado As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents btnColaborador As System.Windows.Forms.DataGridViewImageColumn
        Friend WithEvents DataGridViewImageColumn1 As System.Windows.Forms.DataGridViewImageColumn
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim dtsItemsDiseño As DMSOneFramework.ItemsSAPDataset
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAdicionales1))
            Me.SubBusRecep = New Buscador.SubBuscador()
            Me.EPPrueba = New System.Windows.Forms.ErrorProvider(Me.components)
            Me.btnSolicitudEspecificos = New System.Windows.Forms.Button()
            Me.btnGuardarCerrar = New System.Windows.Forms.Button()
            Me.btnEliminar = New System.Windows.Forms.Button()
            Me.btnAgregar = New System.Windows.Forms.Button()
            Me.dtgRepuestosYActiv = New System.Windows.Forms.DataGridView()
            Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.CheckDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.ItemCodeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ItemNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Fase = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Tipo = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Duracion = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ObservacionesDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Stock = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Currency = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Precio = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.LineNum = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.IDEmpleado = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.NombreEmpleado = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.btnColaborador = New System.Windows.Forms.DataGridViewImageColumn()
            Me.btnCerrar = New System.Windows.Forms.Button()
            Me.DataGridViewImageColumn1 = New System.Windows.Forms.DataGridViewImageColumn()
            dtsItemsDiseño = New DMSOneFramework.ItemsSAPDataset()
            CType(dtsItemsDiseño, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.EPPrueba, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dtgRepuestosYActiv, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'dtsItemsDiseño
            '
            dtsItemsDiseño.DataSetName = "ItemsSAPDataset"
            dtsItemsDiseño.Locale = New System.Globalization.CultureInfo("en-US")
            dtsItemsDiseño.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'SubBusRecep
            '
            Me.SubBusRecep.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.SubBusRecep.Barra_Titulo = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.SubBusRecep.ConsultarDBPorFiltrado = False
            Me.SubBusRecep.Criterios = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.SubBusRecep.Criterios_Ocultos = 0
            Me.SubBusRecep.Criterios_OcultosEx = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.SubBusRecep.IN_DataTable = Nothing
            resources.ApplyResources(Me.SubBusRecep, "SubBusRecep")
            Me.SubBusRecep.MultiSeleccion = False
            Me.SubBusRecep.Name = "SubBusRecep"
            Me.SubBusRecep.SQL_Cnn = Nothing
            Me.SubBusRecep.Tabla = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.SubBusRecep.Titulos = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.SubBusRecep.Where = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            '
            'EPPrueba
            '
            Me.EPPrueba.ContainerControl = Me
            '
            'btnSolicitudEspecificos
            '
            resources.ApplyResources(Me.btnSolicitudEspecificos, "btnSolicitudEspecificos")
            Me.btnSolicitudEspecificos.ForeColor = System.Drawing.Color.Black
            Me.btnSolicitudEspecificos.Name = "btnSolicitudEspecificos"
            '
            'btnGuardarCerrar
            '
            resources.ApplyResources(Me.btnGuardarCerrar, "btnGuardarCerrar")
            Me.btnGuardarCerrar.ForeColor = System.Drawing.Color.Black
            Me.btnGuardarCerrar.Name = "btnGuardarCerrar"
            '
            'btnEliminar
            '
            resources.ApplyResources(Me.btnEliminar, "btnEliminar")
            Me.btnEliminar.ForeColor = System.Drawing.Color.Maroon
            Me.btnEliminar.Name = "btnEliminar"
            '
            'btnAgregar
            '
            resources.ApplyResources(Me.btnAgregar, "btnAgregar")
            Me.btnAgregar.ForeColor = System.Drawing.Color.Maroon
            Me.btnAgregar.Name = "btnAgregar"
            '
            'dtgRepuestosYActiv
            '
            Me.dtgRepuestosYActiv.AllowUserToAddRows = False
            Me.dtgRepuestosYActiv.AllowUserToDeleteRows = False
            resources.ApplyResources(Me.dtgRepuestosYActiv, "dtgRepuestosYActiv")
            Me.dtgRepuestosYActiv.AutoGenerateColumns = False
            Me.dtgRepuestosYActiv.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgRepuestosYActiv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgRepuestosYActiv.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.CheckDataGridViewCheckBoxColumn, Me.ItemCodeDataGridViewTextBoxColumn, Me.ItemNameDataGridViewTextBoxColumn, Me.Cantidad, Me.Fase, Me.Tipo, Me.Duracion, Me.ObservacionesDataGridViewTextBoxColumn, Me.Stock, Me.Currency, Me.Precio, Me.LineNum, Me.IDEmpleado, Me.NombreEmpleado, Me.btnColaborador})
            Me.dtgRepuestosYActiv.DataMember = "SCGTA_TB_ItemsSAP"
            Me.dtgRepuestosYActiv.DataSource = dtsItemsDiseño
            Me.dtgRepuestosYActiv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
            Me.dtgRepuestosYActiv.Name = "dtgRepuestosYActiv"
            '
            'ID
            '
            Me.ID.DataPropertyName = "ID"
            resources.ApplyResources(Me.ID, "ID")
            Me.ID.Name = "ID"
            '
            'CheckDataGridViewCheckBoxColumn
            '
            Me.CheckDataGridViewCheckBoxColumn.DataPropertyName = "Check"
            Me.CheckDataGridViewCheckBoxColumn.HeaderText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.CheckDataGridViewCheckBoxColumn.Name = "CheckDataGridViewCheckBoxColumn"
            resources.ApplyResources(Me.CheckDataGridViewCheckBoxColumn, "CheckDataGridViewCheckBoxColumn")
            '
            'ItemCodeDataGridViewTextBoxColumn
            '
            Me.ItemCodeDataGridViewTextBoxColumn.DataPropertyName = "ItemCode"
            resources.ApplyResources(Me.ItemCodeDataGridViewTextBoxColumn, "ItemCodeDataGridViewTextBoxColumn")
            Me.ItemCodeDataGridViewTextBoxColumn.Name = "ItemCodeDataGridViewTextBoxColumn"
            Me.ItemCodeDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ItemNameDataGridViewTextBoxColumn
            '
            Me.ItemNameDataGridViewTextBoxColumn.DataPropertyName = "ItemName"
            resources.ApplyResources(Me.ItemNameDataGridViewTextBoxColumn, "ItemNameDataGridViewTextBoxColumn")
            Me.ItemNameDataGridViewTextBoxColumn.Name = "ItemNameDataGridViewTextBoxColumn"
            Me.ItemNameDataGridViewTextBoxColumn.ReadOnly = True
            '
            'Cantidad
            '
            Me.Cantidad.DataPropertyName = "Cantidad"
            resources.ApplyResources(Me.Cantidad, "Cantidad")
            Me.Cantidad.Name = "Cantidad"
            '
            'Fase
            '
            Me.Fase.DataPropertyName = "Fase"
            resources.ApplyResources(Me.Fase, "Fase")
            Me.Fase.Name = "Fase"
            '
            'Tipo
            '
            Me.Tipo.DataPropertyName = "TipoArticulo"
            resources.ApplyResources(Me.Tipo, "Tipo")
            Me.Tipo.Name = "Tipo"
            '
            'Duracion
            '
            Me.Duracion.DataPropertyName = "Duracion"
            resources.ApplyResources(Me.Duracion, "Duracion")
            Me.Duracion.Name = "Duracion"
            '
            'ObservacionesDataGridViewTextBoxColumn
            '
            Me.ObservacionesDataGridViewTextBoxColumn.DataPropertyName = "Observaciones"
            resources.ApplyResources(Me.ObservacionesDataGridViewTextBoxColumn, "ObservacionesDataGridViewTextBoxColumn")
            Me.ObservacionesDataGridViewTextBoxColumn.Name = "ObservacionesDataGridViewTextBoxColumn"
            '
            'Stock
            '
            Me.Stock.DataPropertyName = "Stock"
            resources.ApplyResources(Me.Stock, "Stock")
            Me.Stock.Name = "Stock"
            Me.Stock.ReadOnly = True
            '
            'Currency
            '
            Me.Currency.DataPropertyName = "Currency"
            resources.ApplyResources(Me.Currency, "Currency")
            Me.Currency.Name = "Currency"
            Me.Currency.ReadOnly = True
            '
            'Precio
            '
            Me.Precio.DataPropertyName = "PrecioAcordado"
            resources.ApplyResources(Me.Precio, "Precio")
            Me.Precio.Name = "Precio"
            '
            'LineNum
            '
            Me.LineNum.DataPropertyName = "LineNum"
            resources.ApplyResources(Me.LineNum, "LineNum")
            Me.LineNum.Name = "LineNum"
            '
            'IDEmpleado
            '
            Me.IDEmpleado.DataPropertyName = "IDEmpleado"
            resources.ApplyResources(Me.IDEmpleado, "IDEmpleado")
            Me.IDEmpleado.Name = "IDEmpleado"
            Me.IDEmpleado.ReadOnly = True
            '
            'NombreEmpleado
            '
            Me.NombreEmpleado.DataPropertyName = "NombreEmpleado"
            resources.ApplyResources(Me.NombreEmpleado, "NombreEmpleado")
            Me.NombreEmpleado.Name = "NombreEmpleado"
            Me.NombreEmpleado.ReadOnly = True
            '
            'btnColaborador
            '
            Me.btnColaborador.HeaderText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.btnColaborador.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.btnColaborador.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch
            Me.btnColaborador.Name = "btnColaborador"
            Me.btnColaborador.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
            resources.ApplyResources(Me.btnColaborador, "btnColaborador")
            '
            'btnCerrar
            '
            resources.ApplyResources(Me.btnCerrar, "btnCerrar")
            Me.btnCerrar.ForeColor = System.Drawing.Color.Black
            Me.btnCerrar.Name = "btnCerrar"
            '
            'DataGridViewImageColumn1
            '
            Me.DataGridViewImageColumn1.HeaderText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.DataGridViewImageColumn1.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.DataGridViewImageColumn1.Name = "DataGridViewImageColumn1"
            Me.DataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
            '
            'frmAdicionales1
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.dtgRepuestosYActiv)
            Me.Controls.Add(Me.btnSolicitudEspecificos)
            Me.Controls.Add(Me.btnCerrar)
            Me.Controls.Add(Me.btnGuardarCerrar)
            Me.Controls.Add(Me.btnEliminar)
            Me.Controls.Add(Me.btnAgregar)
            Me.Controls.Add(Me.SubBusRecep)
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Name = "frmAdicionales1"
            CType(dtsItemsDiseño, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.EPPrueba, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dtgRepuestosYActiv, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region "Constructor"


        Public Sub New(ByVal TipoArticulo As enTipoArticulo, _
                       ByVal strNoOrden As String, _
                       ByVal intNoCotizacion As Integer, _
                       ByRef blnAgregaAdicional As Boolean, _
                       ByVal p_strNoVisita As String)

            MyBase.New()

            InitializeComponent()

            m_strNoOrden = strNoOrden
            m_enTipoArticulo = TipoArticulo
            m_intNoCotizacion = intNoCotizacion
            m_strNoVisita = p_strNoVisita
            'm_blnAgregaAdicional = blnAgregaAdicional

        End Sub

#End Region

#Region "Declaraciones"

        Private m_strNoOrden As String
        Private m_intNoCotizacion As Integer
        Private m_dtsItems As New DMSOneFramework.ItemsSAPDataset
        Private objfrmCtrlCliente As frmCtrlInformacionClientes
        Private m_enTipoArticulo As enTipoArticulo
        Private m_strTituloForm As String
        Private m_strTituloBuscador As String
        Private m_blnAgregaAdicional As Boolean
        Private m_intIDListaPrecios As Integer
        Private m_intCodeEspecific As Integer

        Private m_strNoVisita As String
        Private m_intNoSolicitud As Integer

        Private m_adpActRep As SqlClient.SqlDataAdapter

        Public m_cnnSCGTaller As SqlClient.SqlConnection
        Dim objDAConexion As DAConexion

        Dim m_strCodigoEstilo As String
        Dim m_strCodigoModelo As String
        Dim m_strUsaAsocxEspecif As String
        Dim m_strEspecifVehi As String
        Dim DtConf As System.Data.DataTable
        Dim m_strUsaFilSer As String
        Dim m_strUsaFilRep As String

        Private objUtilitarios2 As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

        Public Enum enTipoArticulo
            Repuesto = 1
            Servicio
            Suministro
            ServicioExterno
        End Enum

        Private m_intIDItem As Integer
        Private DATemp As DMSOneFramework.SCGDataAccess.DAConexion



#End Region

#Region "Constantes"
        Private Const strUsaListaCliente As String = "UsaListaPreciosCliente"
        Private Const strListaPrecios As String = "ListaPrecios"
#End Region
#Region "Procedimientos"



        Private Sub EstiloGridRepuesto()

            Try

                If m_enTipoArticulo = enTipoArticulo.Servicio Then
                    dtgRepuestosYActiv.Columns.Item("Duracion").Visible = True
                    dtgRepuestosYActiv.Columns.Item("Stock").Visible = False
                Else
                    dtgRepuestosYActiv.Columns.Item("Duracion").Visible = False
                    dtgRepuestosYActiv.Columns.Item("Stock").Visible = True

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try

        End Sub

        Private Sub AsignarEmpleadoSeleccionado(ByVal p_dtbItems As DataTable)
            Dim drwCurrent As DataRow
            Dim drwItem As ItemsSAPDataset.SCGTA_TB_ItemsSAPRow

            If p_dtbItems.Rows.Count > 0 Then
                drwCurrent = p_dtbItems.Rows.Item(0)

                With drwCurrent

                    drwItem = m_dtsItems.SCGTA_TB_ItemsSAP.FindByID(m_intIDItem)
                    drwItem.Item("IDEmpleado") = .Item(0)
                    If .Item(1) IsNot DBNull.Value Then
                        drwItem.Item("NombreEmpleado") = .Item(1)
                    End If

                End With

            End If
        End Sub


        Private Sub LoadItemsToGrid(ByVal p_dtbItems As DataTable)
            Dim drwCurrent As DataRow
            Dim strItemCode As String = ""
            Dim strItemName As String = ""
            Dim intFase As Integer
            Dim intDuracion As Integer
            Dim strTipoArticulo As String = ""
            Dim dblStock As Double
            Dim dblPrecio As Double
            Dim strTipoMoneda As String = ""


            For Each drwCurrent In p_dtbItems.Rows


                With drwCurrent

                    strItemCode = .Item("ItemCode")
                    If .Item(1) IsNot DBNull.Value Then
                        strItemName = .Item("Itemname")
                    End If
                    intFase = IIf(Not .Item("U_SCGD_T_FASE") Is DBNull.Value, .Item("U_SCGD_T_FASE"), 0)
                    intDuracion = IIf(Not .Item("U_SCGD_Duracion") Is DBNull.Value, .Item("U_SCGD_Duracion"), 0)
                    strTipoArticulo = IIf(Not .Item("U_SCGD_TipoArticulo") Is DBNull.Value, .Item("U_SCGD_TipoArticulo"), 0)
                    dblStock = IIf(Not .Item("Column1") Is DBNull.Value, .Item("Column1"), 0)
                    strTipoMoneda = IIf(Not .Item("Currency") Is DBNull.Value, .Item("Currency"), 0)
                    dblPrecio = IIf(Not .Item("Price") Is DBNull.Value, .Item("Price"), 0)

                End With


                If strTipoMoneda = "0" Then
                    strTipoMoneda = ""
                End If

                Call GuardaItem(strItemCode, strItemName, intFase, intDuracion, strTipoArticulo, dblStock, strTipoMoneda, dblPrecio)

            Next
            EstiloGridRepuesto()
        End Sub

        Private Sub GuardaItem(ByVal p_strItemCode As String, _
                               ByVal p_strItemName As String, _
                               ByVal p_intFase As Integer, _
                               ByVal p_intDuracion As Integer, _
                               ByVal p_strTipoarticulo As String, _
                               ByVal p_dblStock As Double, _
                               ByVal p_strTipoMoneda As String, _
                               ByVal p_dlbPrecio As Double)

            Dim drwItem As DMSOneFramework.ItemsSAPDataset.SCGTA_TB_ItemsSAPRow
            Dim strDescripcion As String = ""
            Dim dblTiempoMinutos As Double
            Dim CambiaPrecio As Integer = 0
            Dim bandera As Boolean = False
            
            CambiaPrecio = Utilitarios.EjecutarConsulta(String.Format("SELECT Valor FROM SCGTA_TB_Configuracion WHERE Propiedad = 'PermiteCambioPrecio'"),
                                                        strConexionADO)


            Try

                drwItem = m_dtsItems.SCGTA_TB_ItemsSAP.NewSCGTA_TB_ItemsSAPRow

                CargarUnidadesTiempoConfigurada(strDescripcion, dblTiempoMinutos)

                With drwItem
                    .Check = False
                    .ItemCode = p_strItemCode
                    .ItemName = p_strItemName
                    .Cantidad = 1
                    .Fase = p_intFase
                    .TipoArticulo = p_strTipoarticulo
                    .Duracion = p_intDuracion / IIf(dblTiempoMinutos <> 0, dblTiempoMinutos, 1)
                    .Stock = p_dblStock
                    .PrecioAcordado = p_dlbPrecio
                    .Currency = p_strTipoMoneda
                End With

                m_dtsItems.SCGTA_TB_ItemsSAP.Rows.Add(drwItem)

                With m_dtsItems.SCGTA_TB_ItemsSAP.DefaultView
                    .AllowDelete = False

                    'If Cambiar el precio = True
                    If CambiaPrecio > 0 Then
                        bandera = True
                        .AllowEdit = True
                    Else
                        bandera = False
                    End If

                    .AllowNew = False
                End With

                dtgRepuestosYActiv.DataSource = m_dtsItems.SCGTA_TB_ItemsSAP

                If bandera = False Then
                    dtgRepuestosYActiv.Columns("Precio").ReadOnly = True
                End If


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try

        End Sub

        Private Sub CargarPinturaGrid()

            With m_dtsItems.SCGTA_TB_ItemsSAP.DefaultView
                .AllowDelete = False
                .AllowEdit = True
                .AllowNew = False
            End With

            dtgRepuestosYActiv.DataSource = m_dtsItems.SCGTA_TB_ItemsSAP
        End Sub

        Private Sub EliminarItems()
            Dim drwItem As DMSOneFramework.ItemsSAPDataset.SCGTA_TB_ItemsSAPRow
            Dim intContID As Integer

            For intContID = m_dtsItems.SCGTA_TB_ItemsSAP.Rows.Count - 1 To 0 Step -1
                drwItem = m_dtsItems.SCGTA_TB_ItemsSAP.Rows(intContID)
                If drwItem.Check Then
                    drwItem.Delete()
                End If
            Next
        End Sub

        Private Function IngresaRepuestosxOrdenenBD(ByVal dstRepuestos As RepuestosxOrdenDataset,
                                                    ByRef Transaction As SqlClient.SqlTransaction,
                                                    ByRef Conexion As SqlClient.SqlConnection) As Boolean
            Try
                Dim adpActRep As New DMSOneFramework.SCGDataAccess.RepuestosxOrdenDataAdapter

                If dstRepuestos.SCGTA_TB_RepuestosxOrden.Rows.Count > 0 Then
                    Call adpActRep.Inserta(dstRepuestos, Transaction, Conexion)
                End If

            Catch ex As Exception
                Throw
                'MsgBox(ex.Message)
            End Try

        End Function

        Private Function GuardaRepuestos(ByRef dtbRepuestosxOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable, _
                                   ByVal dtbitemSAP As ItemsSAPDataset.SCGTA_TB_ItemsSAPDataTable, _
                                   ByVal strNoOrden As String) As Boolean

            Try
                Dim drwRepuesto As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
                Dim drwItemSap As ItemsSAPDataset.SCGTA_TB_ItemsSAPRow
                Dim strNombreEmpleado As String
                Dim intIDEmpleado As Integer
                Dim intLineNum As Integer
                Dim blnActulizaCotizacion As Boolean = True
                Dim strShipToCode As String = String.Empty

                'Ciclo para validar si existe tipo de cambio para los items
                For Each drwItemSap In dtbitemSAP.Rows
                    If ValidarMonedaItems(drwItemSap.Currency.ToString.Trim()) = False Then
                        Return False
                    End If
                Next

                MetodosCompartidosSBOCls.IniciarCotizacion(m_intNoCotizacion)

                strShipToCode = Utilitarios.EjecutarConsulta(
                    String.Format(
                        "select ShipToDef from OCRD crd with (nolock) inner join OQUT qut with (nolock) on crd.CardCode = qut.CardCode where qut.DocEntry = '{0}'",
                        m_intNoCotizacion), strConexionSBO).Trim()

                For Each drwItemSap In dtbitemSAP.Rows

                    If drwItemSap.RowState <> DataRowState.Deleted Then

                        If Not drwItemSap.IsIDEmpleadoNull Then
                            intIDEmpleado = drwItemSap.IDEmpleado
                        Else
                            intIDEmpleado = -1
                        End If
                        If Not drwItemSap.IsNombreEmpleadoNull Then
                            strNombreEmpleado = drwItemSap.NombreEmpleado
                        Else
                            strNombreEmpleado = ""
                        End If
                        drwRepuesto = dtbRepuestosxOrden.NewSCGTA_TB_RepuestosxOrdenRow

                        If IsDBNull(drwItemSap("PrecioAcordado")) Then
                            drwItemSap.PrecioAcordado = 0
                        End If

                        If drwItemSap.PrecioAcordado = 0 Then
                            If drwItemSap.TipoArticulo = 1 Then
                                intLineNum = MetodosCompartidosSBOCls.AgregarItemCotizacion(m_intNoCotizacion, drwItemSap.ItemCode, drwItemSap.Cantidad, drwItemSap.Observaciones, g_strImpRepuestos, , , , , intIDEmpleado, strNombreEmpleado, , strShipToCode)
                            Else
                                intLineNum = MetodosCompartidosSBOCls.AgregarItemCotizacion(m_intNoCotizacion, drwItemSap.ItemCode, drwItemSap.Cantidad, drwItemSap.Observaciones, g_strImpServiciosExternos, , , , , intIDEmpleado, strNombreEmpleado, , strShipToCode)
                            End If
                        Else
                            If drwItemSap.TipoArticulo = 1 Then
                                intLineNum = MetodosCompartidosSBOCls.AgregarItemCotizacion(m_intNoCotizacion, drwItemSap.ItemCode, drwItemSap.Cantidad, drwItemSap.Observaciones, g_strImpRepuestos, drwItemSap.PrecioAcordado, drwItemSap.Currency, , , intIDEmpleado, strNombreEmpleado, , strShipToCode)
                            Else
                                intLineNum = MetodosCompartidosSBOCls.AgregarItemCotizacion(m_intNoCotizacion, drwItemSap.ItemCode, drwItemSap.Cantidad, drwItemSap.Observaciones, g_strImpServiciosExternos, drwItemSap.PrecioAcordado, drwItemSap.Currency, , , intIDEmpleado, strNombreEmpleado, , strShipToCode)
                            End If
                        End If
                        With drwRepuesto

                            .NoOrden = strNoOrden
                            .NoRepuesto = drwItemSap.ItemCode
                            .Cantidad = drwItemSap.Cantidad
                            .Adicional = 1
                            .TipoArticulo = drwItemSap.TipoArticulo
                            .LineNum = intLineNum
                            .LineNumOriginal = intLineNum
                        End With

                        Call dtbRepuestosxOrden.AddSCGTA_TB_RepuestosxOrdenRow(drwRepuesto)

                    End If

                Next drwItemSap

                'ACTUALIZA obj cotizacion
                MetodosCompartidosSBOCls.ActualizarCotizacion()
                Return True



            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
                Return False
            End Try

        End Function

        Private Function GuardaActividades(ByRef dtbActividadesxOrden As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable, _
                                           ByVal dtbitemSAP As ItemsSAPDataset.SCGTA_TB_ItemsSAPDataTable, _
                                           ByVal strNoOrden As String) As Boolean

            Try
                Dim drwActividadesxOrden As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow
                Dim drwItemSap As ItemsSAPDataset.SCGTA_TB_ItemsSAPRow
                Dim intLineNum As Integer
                Dim strNombreEmpleado As String
                Dim intIDEmpleado As Integer
                
                'Ciclo para validar si existe tipo de cambio para los items
                For Each drwItemSap In dtbitemSAP.Rows
                    If ValidarMonedaItems(drwItemSap.Currency.ToString.Trim()) = False Then
                        Return False
                    End If
                Next

                MetodosCompartidosSBOCls.IniciarCotizacion(m_intNoCotizacion)

                Dim strShipToCode As String = String.Empty
                strShipToCode = Utilitarios.EjecutarConsulta(
                    String.Format(
                        "select ShipToDef from OCRD crd with (nolock) inner join OQUT qut with (nolock) on crd.CardCode = qut.CardCode where qut.DocEntry = '{0}'",
                        m_intNoCotizacion), strConexionSBO).Trim()

                For Each drwItemSap In dtbitemSAP.Rows

                    If drwItemSap.RowState <> DataRowState.Deleted Then

                        If Not drwItemSap.IsIDEmpleadoNull Then
                            intIDEmpleado = drwItemSap.IDEmpleado

                            If ValidarColaborador(intIDEmpleado) Then
                                Exit Function
                            End If

                        Else
                            intIDEmpleado = -1
                        End If
                        If Not drwItemSap.IsNombreEmpleadoNull Then
                            strNombreEmpleado = drwItemSap.NombreEmpleado
                        Else
                            strNombreEmpleado = ""
                        End If

                        drwActividadesxOrden = dtbActividadesxOrden.NewSCGTA_TB_ActividadesxOrdenRow
                        
                        ''*****************************************************************************
                        If IsDBNull(drwItemSap("PrecioAcordado")) Then
                            drwItemSap.PrecioAcordado = 0
                        End If
                        
                        If drwItemSap.PrecioAcordado = 0 Then
                            intLineNum = MetodosCompartidosSBOCls.AgregarItemCotizacion(m_intNoCotizacion, drwItemSap.ItemCode, drwItemSap.Cantidad, drwItemSap.Observaciones, g_strImpServicios, , , , , intIDEmpleado, strNombreEmpleado, drwItemSap.Duracion, strShipToCode)
                        Else
                            intLineNum = MetodosCompartidosSBOCls.AgregarItemCotizacion(m_intNoCotizacion, drwItemSap.ItemCode, drwItemSap.Cantidad, drwItemSap.Observaciones, g_strImpServicios, drwItemSap.PrecioAcordado, drwItemSap.Currency, , , intIDEmpleado, strNombreEmpleado, drwItemSap.Duracion, strShipToCode)
                        End If
                        With drwActividadesxOrden

                            .NoOrden = strNoOrden
                            .NoFase = drwItemSap.Fase
                            .NoActividad = drwItemSap.ItemCode
                            .Adicional = 1
                            .Duracion = drwItemSap.Duracion
                            .LineNum = intLineNum
                            .Cantidad = drwItemSap.Cantidad
                        End With

                        Call dtbActividadesxOrden.AddSCGTA_TB_ActividadesxOrdenRow(drwActividadesxOrden)

                    End If

                Next drwItemSap

                MetodosCompartidosSBOCls.ActualizarCotizacion()
                Return True

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
                Return False
            End Try

        End Function


        Private Function ValidarColaborador(ByVal IDColaborador As String) As Boolean
            Dim strCosteoServicios As String

            Try

                'Validar salario del colaborador

                strCosteoServicios = objUtilitarios2.TraerConfiguracionServicios()

                If strCosteoServicios <> "0" Then

                    If objUtilitarios2.TraerSalarioColaborador(IDColaborador) Then
                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.ValidarSalarioEnServicio + IDColaborador)
                        Return True

                    Else
                        Return False

                    End If

                End If


            Catch ex As Exception
                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Function


        Private Function IngresaActividadesxOrdenenBD(ByVal dstActividades As ActividadesXFaseDataset,
                                                      ByRef Transaction As SqlClient.SqlTransaction,
                                                      ByRef Conexion As SqlClient.SqlConnection) As Boolean
            Try
                Dim adpfasesxOrden As New DMSOneFramework.SCGDataAccess.ActividadesXFaseDataAdapter

                If dstActividades.SCGTA_TB_ActividadesxOrden.Rows.Count > 0 Then

                    Call adpfasesxOrden.Inserta(dstActividades, Transaction, Conexion)

                End If

            Catch ex As Exception
                Throw
                'MsgBox(ex.Message)
            End Try

        End Function

        Private Function GuardaSuministros(ByRef dtbSuministrosxOrden As SuministrosDataset.SCGTA_VW_SuministrosDataTable, _
                                          ByVal dtbitemSAP As ItemsSAPDataset.SCGTA_TB_ItemsSAPDataTable, _
                                          ByVal strNoOrden As String) As Boolean

            Try
                Dim drwsuministrosxOrden As SuministrosDataset.SCGTA_VW_SuministrosRow
                Dim drwItemSap As ItemsSAPDataset.SCGTA_TB_ItemsSAPRow
                Dim intLineNum As Integer
                Dim strNombreEmpleado As String
                Dim intIDEmpleado As Integer
                Dim blnActulizaCotizacion As Boolean = True

                '*****************Manejo Multimoneda'''''''''''''''''''''''''''''
                'Dim PreciosinConvert As Decimal = 0
                'Dim decCodArticulo As String = ""
                'Dim strCurrencyArticulo As String = ""
                'Dim PrecioConvertido As Decimal = 0

                'Dim decTipoCambioCotizacion As Decimal = 0
                'Dim strTipoCambioCotizacion As String = 0

                'Dim strMonedaCotizacion As String = ""
                'Dim strFechaCotizacion As String = ""

                'Dim strMonedaSistema As String = ""
                'Dim strMonedaLocal As String = ""

                'Dim decTipoCambioMS As String = 0
                'Dim strTipoCambioMS As String = ""
                '****************************************************************

                'Ciclo para validar si existe tipo de cambio para los items
                For Each drwItemSap In dtbitemSAP.Rows
                    If ValidarMonedaItems(drwItemSap.Currency.ToString.Trim()) = False Then
                        Return False
                    End If
                Next

                MetodosCompartidosSBOCls.IniciarCotizacion(m_intNoCotizacion)

                Dim strShipToCode As String = String.Empty
                strShipToCode = Utilitarios.EjecutarConsulta(
                    String.Format(
                        "select ShipToDef from OCRD crd with (nolock) inner join OQUT qut with (nolock) on crd.CardCode = qut.CardCode where qut.DocEntry = '{0}'",
                        m_intNoCotizacion), strConexionSBO).Trim()

                    For Each drwItemSap In dtbitemSAP.Rows

                    If drwItemSap.RowState <> DataRowState.Deleted Then

                        If Not drwItemSap.IsIDEmpleadoNull Then
                            intIDEmpleado = drwItemSap.IDEmpleado
                        Else
                            intIDEmpleado = -1
                        End If
                        If Not drwItemSap.IsNombreEmpleadoNull Then
                            strNombreEmpleado = drwItemSap.NombreEmpleado
                        Else
                            strNombreEmpleado = ""
                        End If

                        drwsuministrosxOrden = dtbSuministrosxOrden.NewSCGTA_VW_SuministrosRow
                        
                        '*****************************************************************************
                        If IsDBNull(drwItemSap("PrecioAcordado")) Then
                            drwItemSap.PrecioAcordado = 0
                        End If
                        
                        If drwItemSap.PrecioAcordado = 0 Then
                            intLineNum = MetodosCompartidosSBOCls.AgregarItemCotizacion(m_intNoCotizacion, drwItemSap.ItemCode, drwItemSap.Cantidad, drwItemSap.Observaciones, g_strImpSuministros, , , , , intIDEmpleado, strNombreEmpleado, , strShipToCode)
                        Else
                            intLineNum = MetodosCompartidosSBOCls.AgregarItemCotizacion(m_intNoCotizacion, drwItemSap.ItemCode, drwItemSap.Cantidad, drwItemSap.Observaciones, g_strImpSuministros, drwItemSap.PrecioAcordado, drwItemSap.Currency, , , intIDEmpleado, strNombreEmpleado, , strShipToCode)
                        End If

                        'intLineNum = MetodosCompartidosSBOCls.AgregarItemCotizacion(m_intNoCotizacion, drwItemSap.ItemCode, drwItemSap.Cantidad, drwItemSap.Observaciones, g_strImpSuministros, , , , intIDEmpleado, strNombreEmpleado)

                        With drwsuministrosxOrden

                            .NoOrden = strNoOrden
                            .Cantidad = drwItemSap.Cantidad
                            .Adicional = 1
                            .NoSuministro = drwItemSap.ItemCode
                            .LineNum = intLineNum
                            .LineNumOriginal = intLineNum
                        End With

                        Call dtbSuministrosxOrden.AddSCGTA_VW_SuministrosRow(drwsuministrosxOrden)

                    End If

                Next drwItemSap

                MetodosCompartidosSBOCls.ActualizarCotizacion()
                Return True

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
                Return False
            End Try

        End Function

        Private Function IngresaSuministrosxOrdenenBD(ByVal dstSuministros As SuministrosDataset,
                                                      ByRef Transaction As SqlClient.SqlTransaction,
                                                      ByRef Conexion As SqlClient.SqlConnection) As Boolean
            Try
                Dim adpSuministrosxOrden As New DMSOneFramework.SCGDataAccess.SuministrosDataAdapter

                If dstSuministros.SCGTA_VW_Suministros.Rows.Count > 0 Then

                    Call adpSuministrosxOrden.Inserta(dstSuministros, Transaction, Conexion)

                End If

            Catch ex As Exception
                Throw
                'MsgBox(ex.Message)
            End Try

        End Function

        Private Function CalculaNoActiSig(ByRef p_dstActividad As DMSOneFramework.ActividadesXFaseDataset) As Integer

            Dim drwActXFace As DMSOneFramework.ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow
            Dim intMaxID As Integer = 0

            For Each drwActXFace In p_dstActividad.SCGTA_TB_ActividadesxOrden.Rows
                If drwActXFace.NoActividad >= intMaxID Then
                    intMaxID = drwActXFace.NoActividad
                End If
            Next

            Return intMaxID + 1

        End Function

        'Private Sub CambiarTamanioColums()
        '    Const intGridSize As Integer = 456
        '    Const intCheckSize As Integer = 30
        '    Const intItemCodeSize As Integer = 90
        '    Const intItemNameSize As Integer = 272
        '    Const intCantidadSize As Integer = 60

        '    Dim intResult As Integer

        '    intResult = Math.Round((intItemNameSize * dtgRepuestosYActiv.Size.Width) / intGridSize)
        '    dtgRepuestosYActiv.TableStyles(0).GridColumnStyles("ItemName").Width = intResult

        '    intResult = Math.Round((intItemCodeSize * dtgRepuestosYActiv.Size.Width) / intGridSize)
        '    dtgRepuestosYActiv.TableStyles(0).GridColumnStyles("ItemName").Width += (intResult - intItemCodeSize)

        '    intResult = Math.Round((intCantidadSize * dtgRepuestosYActiv.Size.Width) / intGridSize)
        '    dtgRepuestosYActiv.TableStyles(0).GridColumnStyles("ItemName").Width += (intResult - intCantidadSize)

        '    intResult = Math.Round((intCheckSize * dtgRepuestosYActiv.Size.Width) / intGridSize)
        '    dtgRepuestosYActiv.TableStyles(0).GridColumnStyles("ItemName").Width += (intResult - intCheckSize)

        'End Sub

        Private Function GenerarSolicitudEspecificos() As Boolean

            Dim drwSolicitudEspecificos As SolicitudEspecificosDataset.SCGTA_SP_SelSolicitudEspecificoRow
            Dim dtsSolicitudEspecificos As New SolicitudEspecificosDataset
            Dim adpSolicitudEspecificos As New SolicitudEspecificosDataAdapter

            Dim drwItemsSolicitud As ItemSolicitudEspecificoDataset.SCGTA_SP_SelItemSolicitudEspecificoRow
            Dim dtsItemsSolicitud As New ItemSolicitudEspecificoDataset
            Dim adpItemsSolicitud As New ItemSolicitudEspecificoDataAdapter

            Dim drwItemsSAP As DMSOneFramework.ItemsSAPDataset.SCGTA_TB_ItemsSAPRow

            Dim cnConection As SqlClient.SqlConnection = Nothing
            Dim tnTransation As SqlClient.SqlTransaction = Nothing

            Try

                drwSolicitudEspecificos = dtsSolicitudEspecificos.SCGTA_SP_SelSolicitudEspecifico.NewSCGTA_SP_SelSolicitudEspecificoRow
                drwSolicitudEspecificos.ID = 1
                drwSolicitudEspecificos.NoOrden = m_strNoOrden
                drwSolicitudEspecificos.SolicitadoPor = G_strUsuarioAplicacion

                dtsSolicitudEspecificos.SCGTA_SP_SelSolicitudEspecifico.AddSCGTA_SP_SelSolicitudEspecificoRow(drwSolicitudEspecificos)
                adpSolicitudEspecificos.Update(dtsSolicitudEspecificos, cnConection, tnTransation, False)

                For Each drwItemsSAP In m_dtsItems.SCGTA_TB_ItemsSAP

                    drwItemsSolicitud = Nothing
                    drwItemsSolicitud = dtsItemsSolicitud.SCGTA_SP_SelItemSolicitudEspecifico.NewSCGTA_SP_SelItemSolicitudEspecificoRow

                    drwItemsSolicitud.ItemCodeGenerico = drwItemsSAP.ItemCode
                    drwItemsSolicitud.Cantidad = drwItemsSAP.Cantidad
                    drwItemsSolicitud.Observaciones = drwItemsSAP.Observaciones
                    drwItemsSolicitud.IDSolicitud = drwSolicitudEspecificos.ID
                    drwItemsSolicitud.LineNum = drwItemsSAP.LineNum
                    If Not drwItemsSAP.IsIDEmpleadoNull Then
                        drwItemsSolicitud.IDEmpleado = drwItemsSAP.IDEmpleado
                    End If
                    If Not drwItemsSAP.IsNombreEmpleadoNull Then
                        drwItemsSolicitud.NombreEmpleado = drwItemsSAP.NombreEmpleado
                    End If
                    dtsItemsSolicitud.SCGTA_SP_SelItemSolicitudEspecifico.AddSCGTA_SP_SelItemSolicitudEspecificoRow(drwItemsSolicitud)

                Next
                adpItemsSolicitud.Update(dtsItemsSolicitud, cnConection, tnTransation)

                tnTransation.Commit()
                cnConection.Close()

                m_intNoSolicitud = drwSolicitudEspecificos.ID

                MessageBox.Show(My.Resources.ResourceUI.MensajeSeGeneroLaSolicitud & " " & CStr(drwSolicitudEspecificos.ID))

                Call EnviarMensaje()

                Return True

            Catch ex As Exception

                If cnConection.State = ConnectionState.Open Then

                    tnTransation.Rollback()
                    cnConection.Close()

                End If

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
                Return False

            Finally
                'Agregado 05072010
                Call cnConection.Close()

            End Try

        End Function

        Private Sub EnviarMensaje()

            Dim m_adpMensajeria As MensajeriaSBOTallerDataAdapter
            Dim strConsultaAsesor As String = " Select T1.firstName + ' ' + T1.lastName  From [OQUT] T0 ,[OHEM] T1 Where T0.[OwnerCode] = T1.[empID] and T0.U_SCGD_Numero_OT = '" + m_strNoOrden + "' "
            Dim Asesor As String
            m_adpMensajeria = New MensajeriaSBOTallerDataAdapter(strConexionADO)
            Asesor = Utilitarios.EjecutarConsulta(strConsultaAsesor, strConexionSBO)
            m_adpMensajeria.InsertarMensajeSBO_DMS(My.Resources.ResourceUI.MensajeNuevaSolicitudEspecificos + " " + Asesor, m_strNoOrden, m_intNoCotizacion, MensajeriaSBOTallerDataAdapter.RecibeMensaje.EncargadoRepuestos, 0, m_strNoVisita, 2, m_intNoSolicitud)

        End Sub

        Public Function GenerarSolicitudDesdeAfuera(ByVal p_dtsRepuestosXOrden As RepuestosxOrdenDataset) As Boolean

            Dim drwRepuestos As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
            Dim drwItems As ItemsSAPDataset.SCGTA_TB_ItemsSAPRow
            Dim blnGenerar As Boolean = False

            For Each drwRepuestos In p_dtsRepuestosXOrden.SCGTA_TB_RepuestosxOrden.Rows
                If drwRepuestos.Check And drwRepuestos.Generico = 2 And drwRepuestos.IsItemCodeEspecificoNull Then

                    drwItems = m_dtsItems.SCGTA_TB_ItemsSAP.NewSCGTA_TB_ItemsSAPRow
                    drwItems.ItemCode = drwRepuestos.NoRepuesto
                    drwItems.ItemName = drwRepuestos.Itemname
                    drwItems.Observaciones = drwRepuestos.Observaciones
                    drwItems.PrecioAcordado = drwRepuestos.PrecioAcordado
                    drwItems.Cantidad = drwRepuestos.Cantidad
                    drwItems.LineNum = drwRepuestos.LineNum
                    m_dtsItems.SCGTA_TB_ItemsSAP.AddSCGTA_TB_ItemsSAPRow(drwItems)
                    blnGenerar = True
                Else
                    drwRepuestos.RejectChanges()
                End If
            Next
            If blnGenerar Then
                Call GenerarSolicitudEspecificos()
            Else
                MessageBox.Show(My.Resources.ResourceUI.MensajeDebeSeleccionarRefaccionGenerica)
            End If

        End Function

        Public Function ObtenerListaP_OTEspecial(ByVal p_strNoOrden As String) As String
            Dim IntCodTipoOrden As Integer
            Dim bolUsaListaPrecios As Boolean = False
            Dim strCodCliente As String = String.Empty
            Dim intListaPrecios As Integer
            Dim cmdConsult As SqlClient.SqlCommand
            Dim cmdConsultaCliente As SqlClient.SqlCommand
            Dim drdReader As SqlClient.SqlDataReader
            Dim objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConexionADO)



            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpActRep = New SqlClient.SqlDataAdapter
            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                cmdConsult = CreateSelectCommandTipoOrden(p_strNoOrden)

                cmdConsult.Connection = m_cnnSCGTaller

                IntCodTipoOrden = cmdConsult.ExecuteScalar





                cmdConsult = CreateSelectCommandOrdenesEspeciales(IntCodTipoOrden)

                cmdConsult.Connection = m_cnnSCGTaller

                drdReader = cmdConsult.ExecuteReader

                If Not (drdReader) Is Nothing Then

                    While drdReader.Read

                        bolUsaListaPrecios = drdReader.Item("UsaListaPrecios")
                        If bolUsaListaPrecios Then strCodCliente = drdReader.Item("CardCodeCliente")
                    End While

                    drdReader.Close()

                    If bolUsaListaPrecios Then


                        cmdConsult = CreateSelectCommandListaPreciosCliente(strCodCliente)

                        cmdConsult.Connection = m_cnnSCGTaller

                        intListaPrecios = cmdConsult.ExecuteScalar

                        If intListaPrecios < 0 Then
                            Return String.Empty
                        Else
                            Return CStr(intListaPrecios)
                        End If



                    Else
                        'Poner aqui llamada a configuracion para ver si utiliza lista de precios
                        Dim adpConfig As New ConfiguracionDataAdapter
                        Dim dstConfig As New ConfiguracionDataSet
                        Dim blnUsaListaClientes As Boolean = False
                        Dim strCardCodeCliente As String = ""
                        adpConfig.Fill(dstConfig)

                        If ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracionValorBooleano(dstConfig.SCGTA_TB_Configuracion, strUsaListaCliente, blnUsaListaClientes) Then

                            'Consulta CardCode Cliente a Facturar
                            cmdConsultaCliente = CreateSelectCommandCardCodeCliente(p_strNoOrden)

                            cmdConsultaCliente.Connection = m_cnnSCGTaller

                            strCardCodeCliente = cmdConsultaCliente.ExecuteScalar

                            If String.IsNullOrEmpty(strCardCodeCliente) Then
                                intListaPrecios = 0
                            Else
                                cmdConsult = CreateSelectCommandListaPreciosCliente(strCardCodeCliente)

                                cmdConsult.Connection = m_cnnSCGTaller

                                intListaPrecios = cmdConsult.ExecuteScalar

                                If intListaPrecios < 0 Then
                                    Return String.Empty
                                Else
                                    Return CStr(intListaPrecios)
                                End If

                            End If


                        End If



                    End If

                Else

                    Return String.Empty

                End If





            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex

            Finally

                If Not IsNothing(m_cnnSCGTaller) Then
                    If m_cnnSCGTaller.State = ConnectionState.Open Then
                        m_cnnSCGTaller.Close()
                    End If
                End If

            End Try

            Return String.Empty
        End Function
        Private Function CreateSelectCommandTipoOrden(ByVal p_strNoOrden As String) As SqlClient.SqlCommand

            Try
                Dim cmdSelCodTipoOrden As New SqlClient.SqlCommand("Select CodTipoOrden from SCGTA_TB_Orden with (nolock) where NoOrden=" & "'" & p_strNoOrden & "'")

                cmdSelCodTipoOrden.CommandType = CommandType.Text

                Return cmdSelCodTipoOrden

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try



        End Function

        Private Function CreateSelectCommandCardCodeCliente(ByVal p_strNoOrden As String) As SqlClient.SqlCommand

            Try
                Dim cmdSelCardCodeCliente As New SqlClient.SqlCommand("Select ClienteFacturar from SCGTA_TB_Orden with (nolock) where NoOrden=" & "'" & p_strNoOrden & "'")

                cmdSelCardCodeCliente.CommandType = CommandType.Text

                Return cmdSelCardCodeCliente

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try



        End Function

        Private Function CreateSelectCommandOrdenesEspeciales(ByVal p_intCodTipoOrden As Integer) As SqlClient.SqlCommand

            Try
                Dim cmdSelUsaListaPrecios As New SqlClient.SqlCommand("Select CardCodeCliente,UsaListaPrecios from SCGTA_TB_ConfOrdenesEspeciales with (nolock) where IDTipoOrden=" & p_intCodTipoOrden)

                cmdSelUsaListaPrecios.CommandType = CommandType.Text

                Return cmdSelUsaListaPrecios

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try

        End Function

        Private Function CreateSelectCommandListaPreciosCliente(ByVal p_strCodCliente As String) As SqlClient.SqlCommand

            Try
                Dim cmdSelListaPreciosCliente As New SqlClient.SqlCommand("Select ListNum from SCGTA_VW_Clientes with (nolock) where CardCode=" & "'" & p_strCodCliente & "'")

                cmdSelListaPreciosCliente.CommandType = CommandType.Text

                Return cmdSelListaPreciosCliente

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try

        End Function

#End Region

#Region "Eventos"


        Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
            Dim m_dtArticulos As System.Data.DataTable
            Dim m_strConsultaArticulos As String = "  Select U_ItemCode from [@SCGD_ARTXESP] where U_TipoArt = '{0}' "
            Dim m_strFiltroMod As String = " and U_CodMod = '{0}' "
            Dim m_strFiltroArt As String = " and U_CodEsti = '{0}' "
            Dim m_bTieneArt As Boolean = False



            Try

                DATemp = New DMSOneFramework.SCGDataAccess.DAConexion

                'With SubBusRecep
                '    .SQL_Cnn = DATemp.ObtieneConexion
                '    .Barra_Titulo = m_strTituloBuscador
                '    .Titulos = "Código,Descripcion,Fase,Duracion,Tipo Articulo"
                '    .Criterios = "itemcode,itemname,Tipo_Fase,Duracion,U_TipoArticulo"
                '    .Criterios_Ocultos = 0
                '    .Criterios_OcultosEx = "3,4,5"
                '    .MultiSeleccion = True
                '    .Tabla = "SCGTA_VW_OITM"
                '    .Where = "U_TipoArticulo=" & CStr(m_enTipoArticulo)
                '    .Activar_Buscador(sender)
                'End With

                With SubBusRecep
                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = m_strTituloBuscador

                    'ListP.Price
                    '"Código,Descripcion,Fase,Duracion,Tipo Articulo,Stock,Precio"

                    If m_strUsaAsocxEspecif.Equals("Y") Then

                        Select Case m_enTipoArticulo
                            Case enTipoArticulo.Repuesto
                                If m_strUsaFilRep.Equals("Y") Then

                                    If m_strEspecifVehi.Equals("M") Then
                                        m_strFiltroMod = String.Format(m_strFiltroMod, m_strCodigoModelo)
                                        m_strConsultaArticulos = String.Format(m_strConsultaArticulos, 1)
                                        m_strConsultaArticulos = m_strConsultaArticulos & m_strFiltroMod
                                        m_dtArticulos = Utilitarios.EjecutarConsultaDataTable(m_strConsultaArticulos, strConexionSBO)

                                        If m_dtArticulos.Rows.Count > 0 Then
                                            m_bTieneArt = True
                                        End If
                                    Else
                                        m_strFiltroArt = String.Format(m_strFiltroArt, m_strCodigoEstilo)
                                        m_strConsultaArticulos = String.Format(m_strConsultaArticulos, 1)
                                        m_strConsultaArticulos = m_strConsultaArticulos & m_strFiltroArt
                                        m_dtArticulos = Utilitarios.EjecutarConsultaDataTable(m_strConsultaArticulos, strConexionSBO)

                                        If m_dtArticulos.Rows.Count > 0 Then
                                            m_bTieneArt = True
                                        End If
                                    End If

                                End If


                            Case enTipoArticulo.Servicio
                                If m_strUsaFilRep.Equals("Y") Then

                                    If m_strEspecifVehi.Equals("M") Then
                                        m_strFiltroMod = String.Format(m_strFiltroMod, m_strCodigoModelo)
                                        m_strConsultaArticulos = String.Format(m_strConsultaArticulos, 2)
                                        m_strConsultaArticulos = m_strConsultaArticulos & m_strFiltroMod
                                        m_dtArticulos = Utilitarios.EjecutarConsultaDataTable(m_strConsultaArticulos, strConexionSBO)

                                        If m_dtArticulos.Rows.Count > 0 Then
                                            m_bTieneArt = True
                                        End If
                                    Else
                                        m_strFiltroArt = String.Format(m_strFiltroArt, m_strCodigoEstilo)
                                        m_strConsultaArticulos = String.Format(m_strConsultaArticulos, 2)
                                        m_strConsultaArticulos = m_strConsultaArticulos & m_strFiltroArt
                                        m_dtArticulos = Utilitarios.EjecutarConsultaDataTable(m_strConsultaArticulos, strConexionSBO)

                                        If m_dtArticulos.Rows.Count > 0 Then
                                            m_bTieneArt = True
                                        End If
                                    End If

                                End If


                        End Select

                    End If



                    If m_strUsaAsocxEspecif.Equals("Y") Then

                        If m_bTieneArt Then
                            .Titulos = My.Resources.ResourceUI.CodigoArticulo & "," & My.Resources.ResourceUI.Descripcion & _
                            "," & My.Resources.ResourceUI.Fase & "," & My.Resources.ResourceUI.Duracion & _
                            "," & My.Resources.ResourceUI.TipoArticulo & "," & My.Resources.ResourceUI.Stock & _
                            "," & My.Resources.ResourceUI.TipoMoneda & _
                            "," & My.Resources.ResourceUI.Precio & "," & My.Resources.ResourceUI.CodEstilo & _
                            "," & My.Resources.ResourceUI.Estilo & "," & My.Resources.ResourceUI.CodModelo & _
                            "," & My.Resources.ResourceUI.Modelo & "," & My.Resources.ResourceUI.CodigoBarras


                            .Criterios = "TOP 50 SCGTA_VW_OITM.itemcode,SCGTA_VW_OITM.itemname,U_SCGD_T_Fase,U_SCGD_Duracion,U_SCGD_TipoArticulo,(SCGTA_VW_OITW.OnHand - SCGTA_VW_OITW.IsCommited),ListP.Currency, ListP.Price, Esti.Code, Esti.Name, Mode.Code, Mode.Name,SCGTA_VW_OITM.CodeBars"
                        Else
                            .Titulos = My.Resources.ResourceUI.CodigoArticulo & "," & My.Resources.ResourceUI.Descripcion & _
                         "," & My.Resources.ResourceUI.Fase & "," & My.Resources.ResourceUI.Duracion & _
                         "," & My.Resources.ResourceUI.TipoArticulo & "," & My.Resources.ResourceUI.Stock & _
                         "," & My.Resources.ResourceUI.TipoMoneda & "," & My.Resources.ResourceUI.Precio & "," & My.Resources.ResourceUI.CodigoBarras

                            .Criterios = "TOP 50 SCGTA_VW_OITM.itemcode, SCGTA_VW_OITM.itemname, " + _
                                "U_SCGD_T_Fase, U_SCGD_Duracion, " + _
                                "U_SCGD_TipoArticulo, (SCGTA_VW_OITW.OnHand - SCGTA_VW_OITW.IsCommited), " + _
                                "ListP.Currency, ListP.Price,SCGTA_VW_OITM.CodeBars "
                        End If
                        
                    Else

                        .Titulos = My.Resources.ResourceUI.CodigoArticulo & "," & My.Resources.ResourceUI.Descripcion & _
                            "," & My.Resources.ResourceUI.Fase & "," & My.Resources.ResourceUI.Duracion & _
                            "," & My.Resources.ResourceUI.TipoArticulo & "," & My.Resources.ResourceUI.Stock & _
                            "," & My.Resources.ResourceUI.TipoMoneda & "," & My.Resources.ResourceUI.Precio & "," & My.Resources.ResourceUI.CodigoBarras

                        .Criterios = "TOP 50 SCGTA_VW_OITM.itemcode, SCGTA_VW_OITM.itemname, " + _
                            "U_SCGD_T_Fase, U_SCGD_Duracion, " + _
                            "U_SCGD_TipoArticulo, (SCGTA_VW_OITW.OnHand - SCGTA_VW_OITW.IsCommited), " + _
                            "ListP.Currency, ListP.Price,SCGTA_VW_OITM.CodeBars"

                    End If

                    .Criterios_Ocultos = 0
                    .Criterios_OcultosEx = "3,4,5"
                    .MultiSeleccion = True

                    Select Case m_enTipoArticulo
                        Case enTipoArticulo.Repuesto


                            If m_strUsaAsocxEspecif.Equals("Y") Then

                                If m_strUsaFilRep.Equals("Y") Then

                                    If m_bTieneArt Then

                                        .Tabla = "SCGTA_VW_OITM " & _
                                   "Inner join SCGTA_VW_OITW on SCGTA_VW_OITM.itemCode = SCGTA_VW_OITW.itemCode " & _
                                   "INNER JOIN SCGTA_VW_ARTXESP AS AXESP ON SCGTA_VW_OITM.ItemCode = AXESP.U_ItemCode " & _
                                   "INNER JOIN SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode " & _
                                   "INNER JOIN SCGTA_TB_ConfBodegasXCentroCosto AS BXCC ON SCGTA_VW_OITM.U_SCGD_CodCtroCosto = BXCC.IDCentroCosto " & _
                                   "AND SCGTA_VW_OITW.WhsCode  = BXCC.Repuestos COLLATE database_default " & _
                                   "LEFT OUTER JOIN SCGTA_VW_Estilos as Esti on AXESP.U_CodEsti = Esti.Code " & _
                                   "LEFT OUTER JOIN SCGTA_VW_Modelos as Mode on AXESP.U_CodMod = Mode.Code "

                                        If m_strEspecifVehi.Equals("E") Then

                                            .Where = "AXESP.U_TipoArt=" & CStr(m_enTipoArticulo) & _
                                                "and PriceList=" & CStr(m_intIDListaPrecios) & " and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y' " & _
                                                "and AXESP.U_CodEsti = '" & (m_strCodigoEstilo) & "'"

                                        ElseIf m_strEspecifVehi.Equals("M") Then

                                            .Where = "AXESP.U_TipoArt=" & CStr(m_enTipoArticulo) & _
                                                " and PriceList=" & CStr(m_intIDListaPrecios) & " and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y'" & _
                                                " and AXESP.U_CodMod ='" & (m_strCodigoModelo) & "'"

                                        Else

                                            .Where = "AXESP.U_TipoArt=" & CStr(m_enTipoArticulo) & _
                                                "and PriceList=" & CStr(m_intIDListaPrecios) & " and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y' "

                                        End If

                                    Else

                                        .Titulos = My.Resources.ResourceUI.CodigoArticulo & "," & My.Resources.ResourceUI.Descripcion & _
                                        "," & My.Resources.ResourceUI.Fase & "," & My.Resources.ResourceUI.Duracion & _
                                        "," & My.Resources.ResourceUI.TipoArticulo & "," & My.Resources.ResourceUI.Stock & _
                                        "," & My.Resources.ResourceUI.TipoMoneda & "," & My.Resources.ResourceUI.Precio & "," & My.Resources.ResourceUI.CodigoBarras

                                        .Criterios = "TOP 50 SCGTA_VW_OITM.itemcode, SCGTA_VW_OITM.itemname, " + _
                                            "U_SCGD_T_Fase, U_SCGD_Duracion, " + _
                                            "U_SCGD_TipoArticulo, (SCGTA_VW_OITW.OnHand - SCGTA_VW_OITW.IsCommited), " + _
                                            "ListP.Currency, ListP.Price,SCGTA_VW_OITM.CodeBars"

                                        .Tabla = "SCGTA_VW_OITM " & _
                                       "Inner join SCGTA_VW_OITW on SCGTA_VW_OITM.itemCode = SCGTA_VW_OITW.itemCode " & _
                                       "INNER JOIN SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode " & _
                                       "INNER JOIN SCGTA_TB_ConfBodegasXCentroCosto AS BXCC ON SCGTA_VW_OITM.U_SCGD_CodCtroCosto = BXCC.IDCentroCosto " & _
                                       "AND SCGTA_VW_OITW.WhsCode  = BXCC.Repuestos COLLATE database_default "

                                        .Where = " U_SCGD_TipoArticulo=" & CStr(m_enTipoArticulo) & _
                                            " and PriceList=" & CStr(m_intIDListaPrecios) & " and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y' "
                                    End If

                                Else
                                    .Tabla = "SCGTA_VW_OITM " & _
                                "Inner join SCGTA_VW_OITW on SCGTA_VW_OITM.itemCode = SCGTA_VW_OITW.itemCode " & _
                                "INNER JOIN SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode " & _
                                "INNER JOIN SCGTA_TB_ConfBodegasXCentroCosto AS BXCC ON SCGTA_VW_OITM.U_SCGD_CodCtroCosto = BXCC.IDCentroCosto " & _
                                "AND SCGTA_VW_OITW.WhsCode  = BXCC.Repuestos COLLATE database_default "

                                    .Where = " U_SCGD_TipoArticulo=" & CStr(m_enTipoArticulo) & _
                                        " and PriceList=" & CStr(m_intIDListaPrecios) & " and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y' "
                                End If


                            Else

                                .Tabla = "SCGTA_VW_OITM " & _
                                "Inner join SCGTA_VW_OITW on SCGTA_VW_OITM.itemCode = SCGTA_VW_OITW.itemCode " & _
                                "INNER JOIN SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode " & _
                                "INNER JOIN SCGTA_TB_ConfBodegasXCentroCosto AS BXCC ON SCGTA_VW_OITM.U_SCGD_CodCtroCosto = BXCC.IDCentroCosto " & _
                                "AND SCGTA_VW_OITW.WhsCode  = BXCC.Repuestos COLLATE database_default "

                                .Where = " U_SCGD_TipoArticulo=" & CStr(m_enTipoArticulo) & _
                                    " and PriceList=" & CStr(m_intIDListaPrecios) & " and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y' "

                            End If

                        Case enTipoArticulo.ServicioExterno

                           

                            .Tabla = "SCGTA_VW_OITM " & _
                                "Inner join SCGTA_VW_OITW on SCGTA_VW_OITM.itemCode = SCGTA_VW_OITW.itemCode " & _
                                "INNER JOIN SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode " & _
                                "INNER JOIN SCGTA_TB_ConfBodegasXCentroCosto AS BXCC ON SCGTA_VW_OITM.U_SCGD_CodCtroCosto = BXCC.IDCentroCosto " & _
                                "AND SCGTA_VW_OITW.WhsCode  = BXCC.ServiciosEx COLLATE database_default "

                            If g_blnServiciosExternosInventariables Then
                                .Where = " U_SCGD_TipoArticulo=" & CStr(m_enTipoArticulo) & _
                                    " and PriceList=" & CStr(m_intIDListaPrecios) & " and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y'  and U_SCGD_Generico is not null "

                            Else
                                .Where = " U_SCGD_TipoArticulo=" & CStr(m_enTipoArticulo) & _
                                    " and PriceList=" & CStr(m_intIDListaPrecios) & " and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'N'  and U_SCGD_Generico is not null "

                            End If

                            '' End If

                            .Criterios_OcultosEx = "3,4,5,6"

                        Case enTipoArticulo.Suministro

                       

                            .Tabla = "SCGTA_VW_OITM " & _
                                "Inner join SCGTA_VW_OITW on SCGTA_VW_OITM.itemCode = SCGTA_VW_OITW.itemCode " & _
                                "INNER JOIN SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode " & _
                                "INNER JOIN SCGTA_TB_ConfBodegasXCentroCosto AS BXCC ON SCGTA_VW_OITM.U_SCGD_CodCtroCosto = BXCC.IDCentroCosto " & _
                                "AND SCGTA_VW_OITW.WhsCode  = BXCC.Suministros COLLATE database_default "

                            .Where = " U_SCGD_TipoArticulo=" & CStr(m_enTipoArticulo) & _
                                " and PriceList=" & CStr(m_intIDListaPrecios) & " and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y'"

                            ''End If

                        Case enTipoArticulo.Servicio

                            If m_strUsaAsocxEspecif.Equals("Y") Then

                                If m_bTieneArt Then


                                    .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Descripcion & _
                                        "," & My.Resources.ResourceUI.Fase & "," & My.Resources.ResourceUI.Duracion & _
                                        "," & My.Resources.ResourceUI.TipoArticulo & "," & My.Resources.ResourceUI.Stock & _
                                        "," & My.Resources.ResourceUI.TipoMoneda & _
                                        "," & My.Resources.ResourceUI.Precio & "," & My.Resources.ResourceUI.CodEstilo & _
                                        "," & My.Resources.ResourceUI.Estilo & "," & My.Resources.ResourceUI.CodModelo & _
                                        "," & My.Resources.ResourceUI.Modelo & "," & My.Resources.ResourceUI.CodigoBarras

                                    If m_strUsaFilSer.Equals("Y") Then

                                        .Criterios = "TOP 50 SCGTA_VW_OITM.itemcode,itemname,U_SCGD_T_Fase,SXESP.U_Duracion,U_SCGD_TipoArticulo,(OnHand - IsCommited),ListP.Currency, ListP.Price,Esti.Code,Esti.Name,Mode.Code,Mode.Name,SCGTA_VW_OITM.CodeBars"

                                        .Tabla = "SCGTA_VW_OITM " & _
                                                "inner join SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode " & _
                                                "INNER JOIN SCGTA_VW_ARTXESP AS SXESP ON SCGTA_VW_OITM.ItemCode = SXESP.U_ItemCode " & _
                                                "LEFT OUTER JOIN SCGTA_VW_Estilos as Esti on SXESP.U_CodEsti = Esti.Code " & _
                                                "LEFT OUTER JOIN SCGTA_VW_Modelos as Mode on SXESP.U_CodMod = Mode.Code "

                                        If m_strEspecifVehi.Equals("E") Then

                                            .Where = "SXESP.U_TipoArt=" & CStr(m_enTipoArticulo) & " and PriceList=" & CStr(m_intIDListaPrecios) & " and SellItem = 'Y' and InvntItem = 'N' and U_SCGD_T_Fase is not null " & _
                                                "and SXESP.U_CodEsti = '" & (m_strCodigoEstilo) & "'"

                                        ElseIf m_strEspecifVehi.Equals("M") Then

                                            .Where = "SXESP.U_TipoArt=" & CStr(m_enTipoArticulo) & " and PriceList=" & CStr(m_intIDListaPrecios) & " and SellItem = 'Y' and InvntItem = 'N' and U_SCGD_T_Fase is not null " & _
                                                "and SXESP.U_CodMod = '" & (m_strCodigoModelo) & "'"

                                        Else

                                            .Where = "SXESP.U_TipoArt==" & CStr(m_enTipoArticulo) & " and PriceList=" & CStr(m_intIDListaPrecios) & " and SellItem = 'Y' and InvntItem = 'N' and U_SCGD_T_Fase is not null "

                                        End If
                                    Else
                                        .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Descripcion & _
                                        "," & My.Resources.ResourceUI.Fase & "," & My.Resources.ResourceUI.Duracion & _
                                        "," & My.Resources.ResourceUI.TipoArticulo & "," & My.Resources.ResourceUI.Stock & _
                                        "," & My.Resources.ResourceUI.TipoMoneda & "," & My.Resources.ResourceUI.Precio & "," & My.Resources.ResourceUI.CodigoBarras

                                        '"Código,Descripcion,Fase,Duracion,Tipo Articulo,Stock,Precio"

                                        .Criterios = "TOP 50 SCGTA_VW_OITM.itemcode,itemname,U_SCGD_T_Fase,U_SCGD_Duracion,U_SCGD_TipoArticulo,(OnHand - IsCommited),ListP.Currency,ListP.Price,SCGTA_VW_OITM.CodeBars"

                                        .Tabla = "SCGTA_VW_OITM " & _
                                                "inner join SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode "

                                        .Where = "U_SCGD_TipoArticulo=" & CStr(m_enTipoArticulo) & " and PriceList=" & CStr(m_intIDListaPrecios) & " and SellItem = 'Y' and InvntItem = 'N' and U_SCGD_T_Fase is not null "
                                    End If
                                Else
                                    .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Descripcion & _
                                   "," & My.Resources.ResourceUI.Fase & "," & My.Resources.ResourceUI.Duracion & _
                                   "," & My.Resources.ResourceUI.TipoArticulo & "," & My.Resources.ResourceUI.Stock & _
                                   "," & My.Resources.ResourceUI.TipoMoneda & "," & My.Resources.ResourceUI.Precio & "," & My.Resources.ResourceUI.CodigoBarras

                                    '"Código,Descripcion,Fase,Duracion,Tipo Articulo,Stock,Precio"

                                    .Criterios = "TOP 50 SCGTA_VW_OITM.itemcode,itemname,U_SCGD_T_Fase,U_SCGD_Duracion,U_SCGD_TipoArticulo,(OnHand - IsCommited),ListP.Currency,ListP.Price,SCGTA_VW_OITM.CodeBars"

                                    .Tabla = "SCGTA_VW_OITM " & _
                                            "inner join SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode "

                                    .Where = "U_SCGD_TipoArticulo=" & CStr(m_enTipoArticulo) & " and PriceList=" & CStr(m_intIDListaPrecios) & " and SellItem = 'Y' and InvntItem = 'N' and U_SCGD_T_Fase is not null "
                                End If

                            Else

                                .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Descripcion & _
                                    "," & My.Resources.ResourceUI.Fase & "," & My.Resources.ResourceUI.Duracion & _
                                    "," & My.Resources.ResourceUI.TipoArticulo & "," & My.Resources.ResourceUI.Stock & _
                                    "," & My.Resources.ResourceUI.TipoMoneda & "," & My.Resources.ResourceUI.Precio & "," & My.Resources.ResourceUI.CodigoBarras

                                '"Código,Descripcion,Fase,Duracion,Tipo Articulo,Stock,Precio"

                                .Criterios = "TOP 50 SCGTA_VW_OITM.itemcode,itemname,U_SCGD_T_Fase,U_SCGD_Duracion,U_SCGD_TipoArticulo,(OnHand - IsCommited),ListP.Currency,ListP.Price,SCGTA_VW_OITM.CodeBars"

                                .Tabla = "SCGTA_VW_OITM " & _
                                        "inner join SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode "

                                .Where = "U_SCGD_TipoArticulo=" & CStr(m_enTipoArticulo) & " and PriceList=" & CStr(m_intIDListaPrecios) & " and SellItem = 'Y' and InvntItem = 'N' and U_SCGD_T_Fase is not null "

                            End If

                            .Criterios_OcultosEx = "3,5,6"

                    End Select
                    .ConsultarDBPorFiltrado = True
                    .Activar_Buscador(sender)
                End With

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub


        Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
            Try
                EliminarItems()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnGuardarCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardarCerrar.Click

            Dim dstRepuestosxOrden As New RepuestosxOrdenDataset

            Dim dstActividadesxOrden As New ActividadesXFaseDataset
            Dim dstSuministrosxOrden As New SuministrosDataset
            ' Dim intLineNum As Integer
            Dim tnTransation As SqlClient.SqlTransaction = Nothing

            'Dim adpMensajeria As New MensajeriaSBOTallerDataAdapter

            Try
                m_blnAgregaAdicional = False

                'inicia transaccion de SAP
                MetodosCompartidosSBOCls.IniciaTransaccion()

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    'abro conexion
                    m_cnnSCGTaller.Open()
                End If

                'inicia transaccion de DMS
                tnTransation = m_cnnSCGTaller.BeginTransaction()

                'recorro items agregados 
                If m_dtsItems.SCGTA_TB_ItemsSAP.Rows.Count > 0 Then

                    Select Case m_enTipoArticulo

                        Case enTipoArticulo.Repuesto
                            'Inserta en SAP
                            If GuardaRepuestos(dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden, _
                                               m_dtsItems.SCGTA_TB_ItemsSAP, _
                                                m_strNoOrden) Then
                                'inserta en DMS
                                Call IngresaRepuestosxOrdenenBD(dstRepuestosxOrden, tnTransation, m_cnnSCGTaller)
                                g_AgregaAdicionales = True
                            End If

                        Case enTipoArticulo.Servicio
                            'Inserta en SAP
                            If GuardaActividades(dstActividadesxOrden.SCGTA_TB_ActividadesxOrden, _
                                                    m_dtsItems.SCGTA_TB_ItemsSAP, _
                                                    m_strNoOrden) Then
                                'inserta en DMS
                                Call IngresaActividadesxOrdenenBD(dstActividadesxOrden, tnTransation, m_cnnSCGTaller)
                                g_AgregaAdicionales = True

                            End If

                        Case enTipoArticulo.Suministro
                            'Inserta en SAP
                            If GuardaSuministros(dstSuministrosxOrden.SCGTA_VW_Suministros, _
                                                 m_dtsItems.SCGTA_TB_ItemsSAP, _
                                                 m_strNoOrden) Then
                                'inserta en DMS
                                Call IngresaSuministrosxOrdenenBD(dstSuministrosxOrden, tnTransation, m_cnnSCGTaller)
                                g_AgregaAdicionales = True

                            End If

                        Case enTipoArticulo.ServicioExterno
                            'Inserta en SAP
                            If GuardaRepuestos(dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden, _
                                               m_dtsItems.SCGTA_TB_ItemsSAP, _
                                               m_strNoOrden) Then
                                'inserta en DMS
                                Call IngresaRepuestosxOrdenenBD(dstRepuestosxOrden, tnTransation, m_cnnSCGTaller)
                                g_AgregaAdicionales = True
                            End If

                    End Select

                    'If blnAgregaAdicional Then
                    '    'Genera mensaje en SBO para el asesor
                    '    adpMensajeria.CreaMensajeDMS_SBO_Cotizacion("Cotización actualizada desde la orden de trabajo", MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_strNoOrden)
                    'End If

                End If

                'manejo de transacciones 
                If g_AgregaAdicionales Then
                    MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Commit)
                    tnTransation.Commit()
                Else
                    MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                    tnTransation.Rollback()
                End If

                Me.Close()

            Catch ex As Exception
                MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                tnTransation.Rollback()
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            Finally
                If m_cnnSCGTaller.State = ConnectionState.Open Then
                    'cierro conexion
                    m_cnnSCGTaller.Close()
                End If
            End Try
        End Sub

        'Private Sub dtgRepuestosYActiv_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        '    Try

        '        Call G_CancelarEditColumnDataGrid(Me, dtgRepuestosYActiv)

        '    Catch ex As Exception
        '        SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
        '    End Try
        'End Sub

        'Private Sub dtgRepuestosYActiv_Resize(ByVal sender As Object, ByVal e As System.EventArgs)
        '    Try
        '        If dtgRepuestosYActiv.TableStyles.Count <> 0 Then
        '            CambiarTamanioColums()
        '        End If
        '    Catch ex As Exception
        '        SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
        '    End Try
        'End Sub

        Private Sub picConfCliente_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                Dim Forma_Nueva As Form
                Dim blnExisteForm As Boolean


                For Each Forma_Nueva In Me.MdiParent.MdiChildren

                    If Forma_Nueva.Name = "frmCtrlInformacionClientes" Then
                        blnExisteForm = True
                    End If

                Next

                If Not blnExisteForm Then

                    objfrmCtrlCliente = New frmCtrlInformacionClientes
                    objfrmCtrlCliente.MdiParent = Me.MdiParent
                    objfrmCtrlCliente.Show()

                End If


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub frmAdicionales1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim DtConf As System.Data.DataTable
            Const c_strListaPrecios As String = "ListaPrecios"
            Dim strValorRetorno As String = ""

            'With m_dtsItems.SCGTA_TB_ItemsSAP.DefaultView
            '    .AllowDelete = False
            '    .AllowEdit = True
            '    .AllowNew = False
            'End With


            'Cargar Configuración de Asociación de Artículos por Especificación
            DtConf = Utilitarios.EjecutarConsultaDataTable("Select U_UsaAXEV,U_EspVehic,U_UsaFilSer,U_UsaFilRep from [@SCGD_ADMIN]", strConexionSBO)
            m_strUsaAsocxEspecif = DtConf.Rows(0)("U_UsaAXEV").ToString().Trim()           ''Utilitarios.EjecutarConsulta("Select U_UsaAXEV from [@SCGD_ADMIN]", strConexionSBO)
            m_strEspecifVehi = DtConf.Rows(0)("U_EspVehic").ToString().Trim() ''Utilitarios.EjecutarConsulta("Select U_EspVehic from [@SCGD_ADMIN]", strConexionSBO)
            m_strUsaFilRep = DtConf.Rows(0)("U_UsaFilRep").ToString().Trim()
            m_strUsaFilSer = DtConf.Rows(0)("U_UsaFilSer").ToString().Trim()

            '***


            If m_enTipoArticulo = enTipoArticulo.Repuesto Then

                btnSolicitudEspecificos.Visible = g_blnCatalogosExternos

            Else

                btnSolicitudEspecificos.Visible = False

            End If


            dtgRepuestosYActiv.DataSource = m_dtsItems.SCGTA_TB_ItemsSAP

            '++++++++++++++

            m_strCodigoEstilo = Utilitarios.EjecutarConsulta(String.Format("Select U_SCGD_Cod_Estilo from OQUT where U_SCGD_Numero_OT = '{0}'", m_strNoOrden), strConexionSBO)
            m_strCodigoModelo = Utilitarios.EjecutarConsulta(String.Format("Select U_SCGD_Cod_Modelo from OQUT where U_SCGD_Numero_OT = '{0}'", m_strNoOrden), strConexionSBO)

            '++++++++++++++

            ''Revisa si la OT es especial, y carga la lista de precios según está configurada
            strValorRetorno = ObtenerListaP_OTEspecial(m_strNoOrden)

            If String.IsNullOrEmpty(strValorRetorno) Then
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, c_strListaPrecios, strValorRetorno)
            End If

            If Not String.IsNullOrEmpty(strValorRetorno) Then
                m_intIDListaPrecios = CInt(strValorRetorno)
            End If

            Select Case m_enTipoArticulo

                Case enTipoArticulo.Repuesto
                    Me.Text = My.Resources.ResourceUI.TituloRefaccionesAdicinales
                    m_strTituloBuscador = My.Resources.ResourceUI.busBarraTituloBuscadorRefacciones

                Case enTipoArticulo.Servicio
                    Me.Text = My.Resources.ResourceUI.TituloServiciosAdicionales
                    m_strTituloBuscador = My.Resources.ResourceUI.busBarraTituloBuscadorServicios

                Case enTipoArticulo.Suministro
                    Me.Text = My.Resources.ResourceUI.TituloSuministrosAdicionales
                    m_strTituloBuscador = My.Resources.ResourceUI.busBarraTitulosBuscadorSuministros

                Case enTipoArticulo.ServicioExterno

                    Me.Text = My.Resources.ResourceUI.TituloServiciosExternosAdicionales
                    m_strTituloBuscador = My.Resources.ResourceUI.busBarraTituloServiciosExternosAdicionales

            End Select
            Call EstiloGridRepuesto()

        End Sub

        Private Sub SubBusRecep_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles SubBusRecep.AppAceptar
            Select Case sender.Name

                Case btnAgregar.Name
                    LoadItemsToGrid(SubBusRecep.OUT_DataTable)

                Case dtgRepuestosYActiv.Name
                    AsignarEmpleadoSeleccionado(SubBusRecep.OUT_DataTable)
            End Select
        End Sub

        Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
            Call Me.Close()
        End Sub

        Private Sub btnSolicitudEspecificos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSolicitudEspecificos.Click

            If ValidarItemsGrid() = True Then
                If MessageBox.Show(My.Resources.ResourceUI.PreguntaDeseaGenerarSolicitudEspecificos, "DMS One", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    If GenerarSolicitudEspecificos() Then
                        Me.Close()
                    End If

                End If
            End If


        End Sub

        Function ValidarItemsGrid() As Boolean
            If Not (m_dtsItems.SCGTA_TB_ItemsSAP.Rows.Count > 0) Then
                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeTenerunItem)
                Return False
            Else
                Return True
            End If

        End Function

        Private Sub dtgRepuestosYActiv_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgRepuestosYActiv.CellClick

            Try

                If e.ColumnIndex = 14 And dtgRepuestosYActiv.CurrentRow IsNot Nothing Then

                    DATemp = New DMSOneFramework.SCGDataAccess.DAConexion

                    m_intIDItem = CInt(dtgRepuestosYActiv.CurrentRow.Cells(0).Value)
                    SubBusRecep = Nothing
                    SubBusRecep = New Buscador.SubBuscador()

                    SubBusRecep.SQL_Cnn = DATemp.ObtieneConexion
                    SubBusRecep.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorEmpleados

                    SubBusRecep.Titulos = My.Resources.ResourceUI.EmpId & "," & My.Resources.ResourceUI.NombreEmpleado

                    '"Código, Nombre,Stock,Precio"
                    SubBusRecep.Criterios = "empID, (firstName + ' ' + lastName) Nombre"
                    SubBusRecep.Tabla = "SCGTA_VW_OHEM"
                    SubBusRecep.Where = ""
                    'm_objBuscadorItems.Top = 500
                    SubBusRecep.ConsultarDBPorFiltrado = False
                    SubBusRecep.Activar_Buscador(sender)
                Else

                    m_intIDItem = 0

                End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Function ValidarMonedaItems(ByVal strCurrency As String) As Boolean

            Dim objBLSBO As New BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal = 0
            Dim strMonedaLocal As String = String.Empty

            strMonedaLocal = objBLSBO.RetornarMonedaLocal

            'Valida la Moneda del Item
            If strCurrency <> strMonedaLocal And strCurrency <> "" Then
                decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(strCurrency, Today, strConectionString, True)
            End If

            If decTipoCambio = -1 Then
                MsgBox(My.Resources.ResourceUI.MensajeErrorTipoCambioME + strCurrency + My.Resources.ResourceUI.ParaLaFecha + Today)
                Return False
            End If

            Return True

        End Function

#End Region
    End Class

End Namespace
