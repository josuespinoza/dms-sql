Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmConfMensajeria
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP


#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

#End Region

#Region "Declaraciones"

        Friend Event RetornaDatos()


        Private m_adpConfMensajeria As ConfMensajeriaDataAdapter
        Private m_dstConfMensajeria As New ConfMensajeriaDataSet
        Private m_dstConfMensajeriaXCentroCosto As New ConfMensajeriaDataSet

        '-- Constantes que guardan el nombre de las columnas 
        Private mc_strIdConfMensajeria As String = "IdConfMensajeria"
        Private mc_strCodCentroCosto As String = "CodCentroCosto"
        Private mc_strDescripcion As String = "Descripcion"
        Private mc_strEncargadoAccesorio As String = "EncargadoAccesorio"
        Private mc_strEncargadoRepuesto As String = "EncargadoRepuesto"
        Private mc_strEncargadoSuministro As String = "EncargadoSuministro"
        Private mc_strEncargadoServicio As String = "EncargadoServicio"
        Private mc_strEstadoLogico As String = "EstadoLogico"
        Private mc_strCentroCosto As String = "CentroCosto"

        '-- Nombre de la constante de la tabla donde se consultan las fases de producción
        Private mc_strTableName As String = "SCGTA_TB_ConfiguracionMensajeria"

        '-- Se inicializa un objeto tipo Utilitarios que recibe el string de conexión con el objetivo de usarla en funciones como carga combos.
        Private objUtilitarios As New Utilitarios(strConectionString)

        '-- Tipo de inserción que se va a relizar si un update o un insert si el tipo de inserción es 1 es un insert de un nuevo objeto si no es un 2 es un update.
        Private intTipoInsercion As Integer

        Private drw As ConfMensajeriaDataSet.SCGTA_TB_ConfiguracionMensajeriaRow

        Private m_BuConfiguracion As Buscador.SubBuscador
        'Private intFaseProduccion As Integer

        Private intCodCentroCostoActual As Integer = 0
#End Region

#Region "Eventos"

        Private Sub frmConfMensajeria_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                m_BuConfiguracion = New Buscador.SubBuscador

                AddHandler m_BuConfiguracion.AppAceptar, _
                AddressOf m_BuSeries_AppAceptar

                cargar()

                'Carga los centros de costo en el combo
                objUtilitarios.CargarCombos(Me.cboCentroCosto, 2)

                'Se ocultan los botones del toolbar que no se van utilizar
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Visible = True
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Exportar).Visible = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Imprimir).Visible = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Buscar).Visible = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cancelar).Visible = False

                'Se inicializan los botones eliminar y guardar inhabilitados ya que no se puede almacenar nada vacio ni eliminar si no esta un row seleccionado
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = False

                'cajas de texto
                cboCentroCosto.Enabled = False
                txtEncargadoAcc.Enabled = False
                txtEncargadoRep.Enabled = False
                txtEncargadoSum.Enabled = False
                txtEncargadoSer.Enabled = False
                picEncargadoAcc.Enabled = False
                picEncargadoRep.Enabled = False
                picEncargadoSum.Enabled = False
                picEncargadoSer.Enabled = False
            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub CargarBuscadordeUsuariosSBO(ByVal buUsuariosSBO As Buscador.SubBuscador, _
                                             ByVal Sender As Object)
            Dim DATemp As DMSOneFramework.SCGDataAccess.DAConexion
            Try
                DATemp = New DMSOneFramework.SCGDataAccess.DAConexion

                With buUsuariosSBO
                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarratituloBuscadorusuariosSBO
                    .Titulos = My.Resources.ResourceUI.Usuario & "," & My.Resources.ResourceUI.Nombre & "," & My.Resources.ResourceUI.Sucursal
                    .Criterios = "User_Code,U_Name, Sucursal"
                    .Criterios_OcultosEx = ""
                    .MultiSeleccion = False
                    .Tabla = "SCGTA_VW_OUSR"
                    .Where = ""
                    .Activar_Buscador(Sender)
                End With

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            End Try
        End Sub
        Private Sub ScgToolBar1_Click_Guardar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Guardar
            Try
                guardar()
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub ScgToolBar1_Click_Eliminar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Eliminar
            Try
                Eliminar()
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub Eliminar()
            Dim intIdConfMensajeria As Integer
            Try
                If txtIdConfMensajeria.Text <> String.Empty Then
                    If objSCGMSGBox.msgPregunta(My.Resources.ResourceUI.MensajeEliminarConfMensajeria) = MsgBoxResult.Yes Then
                        intIdConfMensajeria = CInt(txtIdConfMensajeria.Text)
                        drw = m_dstConfMensajeria.SCGTA_TB_ConfiguracionMensajeria.FindByIdConfMensajeria(intIdConfMensajeria)
                        drw.EstadoLogico = 0
                        m_adpConfMensajeria.Delete(m_dstConfMensajeria, intIdConfMensajeria)

                        cargar()
                        LimpiarCampos()
                        cboCentroCosto.Enabled = False
                        txtEncargadoAcc.Enabled = False
                        txtEncargadoRep.Enabled = False
                        txtEncargadoSum.Enabled = False
                        txtEncargadoSer.Enabled = False
                        picEncargadoAcc.Enabled = False
                        picEncargadoRep.Enabled = False
                        picEncargadoSum.Enabled = False
                        picEncargadoSer.Enabled = False
                        ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = False
                        ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = False
                    End If
                End If

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            End Try

        End Sub

        Private Sub ScgToolBar1_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Cerrar
            Try
                Me.Close()

                m_dstConfMensajeria.Dispose()
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub ScgToolBar1_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Nuevo
            Try
                intTipoInsercion = 1
                LimpiarCampos()
                cboCentroCosto.Enabled = True
                txtEncargadoAcc.Enabled = True
                txtEncargadoRep.Enabled = True
                txtEncargadoSum.Enabled = True
                txtEncargadoSer.Enabled = True
                picEncargadoAcc.Enabled = True
                picEncargadoRep.Enabled = True
                picEncargadoSum.Enabled = True
                picEncargadoSer.Enabled = True
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub frmConfMensajeria_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
            Try
                If Asc(e.KeyChar) = Keys.Escape Then Me.Close()
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            End Try
        End Sub

        Private Sub picEncargadoAcc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picEncargadoAcc.Click
            Call CargarBuscadordeUsuariosSBO(m_BuConfiguracion, sender)
        End Sub
        Private Sub picEncargadoRep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picEncargadoRep.Click
            Call CargarBuscadordeUsuariosSBO(m_BuConfiguracion, sender)
        End Sub

        Private Sub picEncargadoSum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picEncargadoSum.Click
            Call CargarBuscadordeUsuariosSBO(m_BuConfiguracion, sender)
        End Sub

        Private Sub picEncargadoSer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picEncargadoSer.Click
            Call CargarBuscadordeUsuariosSBO(m_BuConfiguracion, sender)
        End Sub
        Private Sub dtgConfMensajeria_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgConfMensajeria.CellClick

            Try
                LimpiarCampos()
                If e.RowIndex >= 0 Then
                    cboCentroCosto.Enabled = True
                    txtEncargadoAcc.Enabled = True
                    txtEncargadoRep.Enabled = True
                    txtEncargadoSum.Enabled = True
                    txtEncargadoSer.Enabled = True
                    picEncargadoAcc.Enabled = True
                    picEncargadoRep.Enabled = True
                    picEncargadoSum.Enabled = True
                    picEncargadoSer.Enabled = True
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = True
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = True

                    Busca_Item_Combo(cboCentroCosto, dtgConfMensajeria.Rows.Item(e.RowIndex).Cells.Item(mc_strCodCentroCosto).Value)
                    txtEncargadoAcc.Text = dtgConfMensajeria.Rows.Item(e.RowIndex).Cells.Item(mc_strEncargadoAccesorio).Value
                    txtEncargadoRep.Text = dtgConfMensajeria.Rows.Item(e.RowIndex).Cells.Item(mc_strEncargadoRepuesto).Value
                    txtEncargadoSum.Text = dtgConfMensajeria.Rows.Item(e.RowIndex).Cells.Item(mc_strEncargadoSuministro).Value
                    txtEncargadoSer.Text = dtgConfMensajeria.Rows.Item(e.RowIndex).Cells.Item(mc_strEncargadoServicio).Value
                    txtIdConfMensajeria.Text = dtgConfMensajeria.Rows.Item(e.RowIndex).Cells.Item(mc_strIdConfMensajeria).Value

                    intCodCentroCostoActual = dtgConfMensajeria.Rows.Item(e.RowIndex).Cells.Item(mc_strCodCentroCosto).Value
                    intTipoInsercion = 2

                Else
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = False
                    ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = False

                End If


            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            End Try
        End Sub

#End Region

#Region "Métodos"

        Private Sub cargar()
            Try

                'Inicializa el DataAdapter con la conexión
                m_adpConfMensajeria = New SCGDataAccess.ConfMensajeriaDataAdapter

                'Inicializa el DataAdapter con la conexión
                m_dstConfMensajeria = New ConfMensajeriaDataSet

                Call m_adpConfMensajeria.FillConfMensajeria(m_dstConfMensajeria)

                With m_dstConfMensajeria.SCGTA_TB_ConfiguracionMensajeria.DefaultView
                    .AllowDelete = True
                    .AllowEdit = True
                    .AllowNew = True
                End With
                dtgConfMensajeria.DataSource = Nothing
                'Carga el datagrid con el dataset en memoria
                dtgConfMensajeria.DataSource = m_dstConfMensajeria.SCGTA_TB_ConfiguracionMensajeria

                EstiloGrid()

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try



        End Sub

        Private Function ConsultarXCentroCosto(ByRef CodCentroCosto As Integer) As Integer
            Try

                'Inicializa el DataAdapter con la conexión
                m_adpConfMensajeria = New SCGDataAccess.ConfMensajeriaDataAdapter

                'Inicializa el DataAdapter con la conexión
                m_dstConfMensajeriaXCentroCosto = New ConfMensajeriaDataSet

                Call m_adpConfMensajeria.FillXCodCentroCosto(m_dstConfMensajeriaXCentroCosto, CodCentroCosto)

                Return CInt(m_dstConfMensajeriaXCentroCosto.SCGTA_TB_ConfiguracionMensajeria.Count())

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try



        End Function

        Private Sub EstiloGrid()

            'IdConfMensajeria
            dtgConfMensajeria.Columns(mc_strIdConfMensajeria).Visible = False
            dtgConfMensajeria.Columns(mc_strIdConfMensajeria).DisplayIndex = 0
            'CodCentroCosto
            dtgConfMensajeria.Columns(mc_strCodCentroCosto).Visible = False
            dtgConfMensajeria.Columns(mc_strCodCentroCosto).DisplayIndex = 1
            'Descripcion
            dtgConfMensajeria.Columns(mc_strDescripcion).Visible = False

            'EncargadoAccesorio
            dtgConfMensajeria.Columns(mc_strEncargadoAccesorio).HeaderText = My.Resources.ResourceUI.EncargadoAccesorio
            dtgConfMensajeria.Columns(mc_strEncargadoAccesorio).DisplayIndex = 3
            dtgConfMensajeria.Columns(mc_strEncargadoAccesorio).Width = 170
            'EncargadoRepuesto
            dtgConfMensajeria.Columns(mc_strEncargadoRepuesto).HeaderText = My.Resources.ResourceUI.EncargadoRepuesto
            dtgConfMensajeria.Columns(mc_strEncargadoRepuesto).DisplayIndex = 4
            dtgConfMensajeria.Columns(mc_strEncargadoRepuesto).Width = 170
            'EncargadoSuministro
            dtgConfMensajeria.Columns(mc_strEncargadoSuministro).HeaderText = My.Resources.ResourceUI.EncargadoSuministro
            dtgConfMensajeria.Columns(mc_strEncargadoSuministro).DisplayIndex = 5
            dtgConfMensajeria.Columns(mc_strEncargadoSuministro).Width = 170

            'EncargadoServicios
            dtgConfMensajeria.Columns(mc_strEncargadoServicio).HeaderText = My.Resources.ResourceUI.EncargadoServicio
            dtgConfMensajeria.Columns(mc_strEncargadoServicio).DisplayIndex = 6
            dtgConfMensajeria.Columns(mc_strEncargadoServicio).Width = 170
            'EstadoLogico
            dtgConfMensajeria.Columns(mc_strEstadoLogico).Visible = False
            'CentroCosto
            dtgConfMensajeria.Columns(mc_strCentroCosto).HeaderText = My.Resources.ResourceUI.CentroCosto
            dtgConfMensajeria.Columns(mc_strCentroCosto).DisplayIndex = 2

        End Sub


        Private Sub m_BuSeries_AppAceptar(ByVal Campo_Llave As String, _
                                         ByVal Arreglo_Campos As System.Collections.ArrayList, _
                                         ByVal sender As Object)

            If Not m_BuConfiguracion.OUT_DataTable Is Nothing _
                AndAlso m_BuConfiguracion.OUT_DataTable.Rows.Count > 0 Then

                Select Case sender.name
                    Case picEncargadoAcc.Name
                        If Trim(txtEncargadoAcc.Text) = String.Empty Then
                            txtEncargadoAcc.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                            txtEncargadoAcc.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        Else
                            txtEncargadoAcc.Text = txtEncargadoAcc.Text & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                            txtEncargadoAcc.Tag = txtEncargadoAcc.Tag & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        End If

                    Case picEncargadoRep.Name
                        If Trim(txtEncargadoRep.Text) = String.Empty Then
                            txtEncargadoRep.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                            txtEncargadoRep.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        Else
                            txtEncargadoRep.Text = txtEncargadoRep.Text & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                            txtEncargadoRep.Tag = txtEncargadoRep.Tag & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        End If

                    Case picEncargadoSum.Name
                        If Trim(txtEncargadoSum.Text) = String.Empty Then
                            txtEncargadoSum.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                            txtEncargadoSum.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        Else
                            txtEncargadoSum.Text = txtEncargadoSum.Text & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                            txtEncargadoSum.Tag = txtEncargadoSum.Tag & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        End If

                    Case picEncargadoSer.Name
                        If Trim(txtEncargadoSer.Text) = String.Empty Then
                            txtEncargadoSer.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                            txtEncargadoSer.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        Else
                            txtEncargadoSer.Text = txtEncargadoSer.Text & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                            txtEncargadoSer.Tag = txtEncargadoSer.Tag & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        End If
                End Select

            End If

        End Sub


        Public Function Busca_Codigo_Texto(ByVal strTempItem As String, Optional ByVal blnGetCodigo As Boolean = True) As String

            '------------------------------------------------ Documentación SCG -----------------------------------------------------------
            '-- Busca el texto en el string enviado....si usas true busca el de la derecha y si usas falses busca el de la izquierda
            '------------------------------------------------------------------------------------------------------------------------------------

            Dim strCod_Item_Comp As String = ""
            Dim strTemp As String = ""
            Dim intCharCont As Integer
            Dim strTextoNoCodigo As String = ""

            strTemp = ""
            strCod_Item_Comp = ""

            If strTempItem <> "" Then
                For intCharCont = strTempItem.Length - 1 To 0 Step -1
                    If Char.IsWhiteSpace(strTempItem.Chars(intCharCont)) Then
                        Exit For
                    End If
                    strTemp = strTemp & strTempItem.Chars(intCharCont)
                Next
                If strTempItem.Length > 0 Then
                    strTextoNoCodigo = strTempItem.Substring(0, strTempItem.Length - (strTempItem.Length - intCharCont)).Trim
                End If
                For intCharCont = strTemp.Length - 1 To 0 Step -1
                    strCod_Item_Comp = strCod_Item_Comp & strTemp.Chars(intCharCont)
                Next

                If blnGetCodigo Then
                    Return strCod_Item_Comp
                Else
                    Return strTextoNoCodigo
                End If
            Else
                Return ""
            End If

        End Function

        Public Sub Busca_Item_Combo(ByRef Combo As ComboBox, ByVal Cod_Item As String)

            Dim intItemCont As Integer
            Dim strTempItem As String
            Dim strCod_Item_Comp As String
            Dim blnExiste As Boolean

            With Combo

                If .Items.Count <> 0 Then
                    blnExiste = False
                    For intItemCont = 0 To .Items.Count - 1
                        strTempItem = .Items(intItemCont)
                        strCod_Item_Comp = Busca_Codigo_Texto(strTempItem)
                        If Cod_Item = strCod_Item_Comp Then
                            blnExiste = True
                            Exit For
                        End If
                    Next
                    If blnExiste Then
                        .Text = .Items(intItemCont)
                    End If
                End If

            End With

        End Sub

        Private Sub guardar()
            Dim intIdConfMensajeria As Integer
            Try
                If cboCentroCosto.Text <> vbNullString Then

                    If intTipoInsercion = 1 Then 'Es una nueva fase de producción.

                        '-- Crea un objeto Datarow del objeto Dataset Fase
                        Dim drwConfMensajeria As ConfMensajeriaDataSet.SCGTA_TB_ConfiguracionMensajeriaRow

                        'se declara un nuevo row
                        drwConfMensajeria = m_dstConfMensajeria.SCGTA_TB_ConfiguracionMensajeria.NewRow()

                        '-- Carga el row con los datos adecuados.
                        drwConfMensajeria.CodCentroCosto = CInt(Busca_Codigo_Texto(cboCentroCosto.SelectedItem, True))
                        drwConfMensajeria.Descripcion = ""
                        drwConfMensajeria.EncargadoAccesorio = txtEncargadoAcc.Text
                        drwConfMensajeria.EncargadoRepuesto = txtEncargadoRep.Text
                        drwConfMensajeria.EncargadoSuministro = txtEncargadoSum.Text
                        drwConfMensajeria.EncargadoServicio = txtEncargadoSer.Text

                        If ConsultarXCentroCosto(CInt(Busca_Codigo_Texto(cboCentroCosto.SelectedItem, True))) > 0 Then
                            MessageBox.Show(My.Resources.ResourceUI.MensajeCodCentroCostoExiste)
                        Else
                            '-- Inserta el row en el Dataset 
                            m_dstConfMensajeria.SCGTA_TB_ConfiguracionMensajeria.AddSCGTA_TB_ConfiguracionMensajeriaRow(drwConfMensajeria)

                            'Actualiza la base de datos todos los cambios hechos en el el dataset.
                            m_adpConfMensajeria.UpdateConfMensajeria(m_dstConfMensajeria)

                            cargar()
                            LimpiarCampos()
                        End If


                    ElseIf intTipoInsercion = 2 Then
                        intIdConfMensajeria = CInt(txtIdConfMensajeria.Text)
                        drw = m_dstConfMensajeria.SCGTA_TB_ConfiguracionMensajeria.FindByIdConfMensajeria(intIdConfMensajeria)
                        '-- Carga el row con los datos adecuados.
                        drw.CodCentroCosto = CInt(Busca_Codigo_Texto(cboCentroCosto.SelectedItem, True))
                        drw.Descripcion = ""
                        drw.EncargadoAccesorio = txtEncargadoAcc.Text
                        drw.EncargadoRepuesto = txtEncargadoRep.Text
                        drw.EncargadoSuministro = txtEncargadoSum.Text
                        drw.EncargadoServicio = txtEncargadoSer.Text

                        If drw.CodCentroCosto = intCodCentroCostoActual Then
                            'Se modifica en la base de datos mediante los metodos de la capa de negocios.
                            m_adpConfMensajeria.UpdateConfMensajeria(m_dstConfMensajeria)

                            cargar()
                            LimpiarCampos()

                            cboCentroCosto.Enabled = False
                            txtEncargadoAcc.Enabled = False
                            txtEncargadoRep.Enabled = False
                            txtEncargadoSum.Enabled = False
                            txtEncargadoSer.Enabled = False
                            picEncargadoAcc.Enabled = False
                            picEncargadoRep.Enabled = False
                            picEncargadoSum.Enabled = False
                            picEncargadoSer.Enabled = False
                            ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = False
                            ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = False
                        Else
                            If ConsultarXCentroCosto(CInt(Busca_Codigo_Texto(cboCentroCosto.SelectedItem, True))) > 0 Then
                                MessageBox.Show(My.Resources.ResourceUI.MensajeCodCentroCostoExiste)
                            Else
                                'Se modifica en la base de datos mediante los metodos de la capa de negocios.
                                m_adpConfMensajeria.UpdateConfMensajeria(m_dstConfMensajeria)

                                cargar()
                                LimpiarCampos()
                                cboCentroCosto.Enabled = False
                                txtEncargadoAcc.Enabled = False
                                txtEncargadoRep.Enabled = False
                                txtEncargadoSum.Enabled = False
                                txtEncargadoSer.Enabled = False
                                picEncargadoAcc.Enabled = False
                                picEncargadoRep.Enabled = False
                                picEncargadoSum.Enabled = False
                                picEncargadoSer.Enabled = False
                                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = False
                                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = False
                            End If
                        End If
                        
                        End If
                Else
                    objSCGMSGBox.msgRequeridos()
                End If

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub LimpiarCampos()
            Try
                txtIdConfMensajeria.Text = String.Empty
                objUtilitarios.CargarCombos(Me.cboCentroCosto, 2)
                txtEncargadoAcc.Text = String.Empty
                txtEncargadoRep.Text = String.Empty
                txtEncargadoSum.Text = String.Empty
                txtEncargadoSer.Text = String.Empty
                intCodCentroCostoActual = 0
            Catch ex As Exception

            End Try
        End Sub

#End Region



    End Class
End Namespace
