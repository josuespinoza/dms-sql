Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmConfCatalogoRepxMarca

#Region "Declaraciones"

#Region "Objetos"

        Private WithEvents m_objBuscador As Buscador.SubBuscador
        Private WithEvents m_objBuscadorProveedores As New Buscador.SubBuscador

#End Region

#Region "Acceso a datos"

        Private m_drdMarcas As SqlClient.SqlDataReader

        Private m_dstConfCatalogos As ConfCatalogoRepXMarcaDataset
        Private m_adpConfCatalogos As ConfCatalogoRepXMarcaDataAdapter
        Private m_adpProveedores As ProveedorXMarcaDatasetTableAdapters.SCGTB_TA_ProveedorXMarcaTableAdapter

        Private m_drwConfCatalogos As ConfCatalogoRepXMarcaDataset.SCGTA_TB_ConfCatalogoRepxMarcaRow

        Private m_oCompania As SAPbobsCOM.Company

        Private m_dicCompañias As New Generic.Dictionary(Of String, String)


#End Region

#Region "Eventos"

        Friend Event FinalizoProcesamiento()

#End Region

#Region "Enums"

        Private Enum enumValidarDatos

            scgValidarSoloServidor = 0
            scgValidarServidorYCompañia = 1
            scgValidarTodo = 2

        End Enum

#End Region

#End Region

#Region "Constructor"

        Public Sub New(ByRef p_dstConfCatalogos As ConfCatalogoRepXMarcaDataset)

            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()

            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
            m_dstConfCatalogos = p_dstConfCatalogos

        End Sub

        Public Sub New(ByVal p_drwConfRepuestosXMarca As ConfCatalogoRepXMarcaDataset.SCGTA_TB_ConfCatalogoRepxMarcaRow, _
                       ByRef p_dstConfCatalogos As ConfCatalogoRepXMarcaDataset)

            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()

            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
            m_drwConfCatalogos = p_drwConfRepuestosXMarca
            m_dstConfCatalogos = p_dstConfCatalogos


        End Sub

#End Region

#Region "Eventos"

        Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
            Try

                Me.Close()

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub frmConfCatalogoRepxMarca_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                Call CargarCatalogos()
                Call MostrarDatosPantalla()

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub picCompañia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picCompañia.Click

            Try

                Call CargarCompañias()

            Catch ex As Exception

                If ex.Message = "Connection to SBO-Common has failed" Then
                    MessageBox.Show(My.Resources.ResourceUI.MensajeProblemaCargarCompanias)
                Else
                    ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                    'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
                End If
            End Try


        End Sub


        Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
            Try

                If Guardar() Then
                    Me.Close()
                End If

            Catch ex As SqlClient.SqlException

                'Dim serrSQLError As SqlClient.SqlError
                If ex.Number = 2601 Then
                    MessageBox.Show(My.Resources.ResourceUI.MensajeConfiguracionMarcaExiste)
                Else
                    ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                    'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
                End If

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub
        Private Sub picAlmacen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picAlmacen.Click

            Try
                If ValidarDatos(enumValidarDatos.scgValidarServidorYCompañia) Then
                    m_objBuscador = Nothing
                    m_objBuscador = New Buscador.SubBuscador
                    With m_objBuscador
                        .SQL_Cnn = CrearObjetoConeccion(txtServidor.Text, m_dicCompañias.Item(cboCompañia.Text), txtUsuarioServidor.Text, txtPasswordServidor.Text)
                        .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorAlmacenes
                        .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Almacen
                        .Criterios = "WhsCode,WhsName"
                        .Criterios_OcultosEx = ""
                        .Where = ""
                        .MultiSeleccion = False
                        .Tabla = "OWHS"
                        .Activar_Buscador(sender)
                    End With
                End If
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub picListaPrecios_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picListaPrecios.Click

            Try
                If ValidarDatos(enumValidarDatos.scgValidarServidorYCompañia) Then
                    m_objBuscador = Nothing
                    m_objBuscador = New Buscador.SubBuscador
                    With m_objBuscador
                        .SQL_Cnn = CrearObjetoConeccion(txtServidor.Text, m_dicCompañias.Item(cboCompañia.Text), txtUsuarioServidor.Text, txtPasswordServidor.Text)
                        .Barra_Titulo = My.Resources.ResourceUI.busBarraTitulosBuscadorListasPrecios
                        .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Descripcion
                        .Criterios = "ListNum,ListName"
                        .Criterios_OcultosEx = ""
                        .Where = ""
                        .MultiSeleccion = False
                        .Tabla = "OPLN"
                        .Activar_Buscador(sender)
                        '.Activar_Buscador(sender)
                    End With
                End If
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub m_objBuscador_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_objBuscador.AppAceptar, m_objBuscadorProveedores.AppAceptar

            Select Case sender.name

                Case picAlmacen.Name
                    txtAlmacen.Text = Arreglo_Campos(1)
                    txtAlmacen.Tag = Arreglo_Campos(0)

                Case picListaPrecios.Name
                    txtListaPrecios.Text = Arreglo_Campos(1)
                    txtListaPrecios.Tag = Arreglo_Campos(0)

                Case btnAgregarAct.Name

                    Dim Proveedor As ProveedorXMarcaDataset.SCGTB_TA_ProveedorXMarcaRow
                    Proveedor = ProveedorXMarca.SCGTB_TA_ProveedorXMarca.NewSCGTB_TA_ProveedorXMarcaRow
                    Proveedor.IDCatalogoRepxMarca = 1
                    Proveedor.CardCodeProveedor = Arreglo_Campos(0)
                    Proveedor.CardNameProveedor = Arreglo_Campos(1)
                    ProveedorXMarca.SCGTB_TA_ProveedorXMarca.AddSCGTB_TA_ProveedorXMarcaRow(Proveedor)

            End Select

        End Sub

        Private Sub picProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarAct.Click
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscadorProveedores.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscadorProveedores.Barra_Titulo = My.Resources.ResourceUI.busBarraTitulosBuscadorProveedores

                m_objBuscadorProveedores.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre

                m_objBuscadorProveedores.Criterios = "CardCode, CardName"
                m_objBuscadorProveedores.Tabla = "SCGTA_VW_Proveedores"
                m_objBuscadorProveedores.Where = ""
                m_objBuscadorProveedores.Activar_Buscador(sender)

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

#End Region

#Region "Métodos"

        Private Function ValidarDatos(ByVal p_intTipoValidacion As enumValidarDatos) As Boolean

            Dim blnDatosCorrectos As Boolean = True

            If cboMarcas.SelectedIndex <= -1 And p_intTipoValidacion >= 2 Then

                epConfRepXMarca.SetError(cboMarcas, My.Resources.ResourceUI.MensajeDebeSeleccionarMarca)
                epConfRepXMarca.SetIconAlignment(cboMarcas, ErrorIconAlignment.BottomLeft)
                blnDatosCorrectos = False

            End If

            If txtServidor.Text = "" And p_intTipoValidacion >= 0 Then

                epConfRepXMarca.SetError(txtServidor, My.Resources.ResourceUI.MensajeIngresarNombreServidor)
                epConfRepXMarca.SetIconAlignment(txtServidor, ErrorIconAlignment.BottomLeft)
                blnDatosCorrectos = False

            End If

            If txtUsuarioServidor.Text = "" And p_intTipoValidacion >= 0 Then

                epConfRepXMarca.SetError(txtUsuarioServidor, My.Resources.ResourceUI.MensajeDebeIngresarNombreServidor)
                epConfRepXMarca.SetIconAlignment(txtUsuarioServidor, ErrorIconAlignment.BottomLeft)
                blnDatosCorrectos = False

            End If

            If cboCompañia.SelectedIndex <= -1 And p_intTipoValidacion >= 1 Then

                epConfRepXMarca.SetError(cboCompañia, My.Resources.ResourceUI.MensajeDebeSeleccionarCompania)
                epConfRepXMarca.SetIconAlignment(cboCompañia, ErrorIconAlignment.BottomLeft)
                blnDatosCorrectos = False

            End If

            If txtUsuarioSBO.Text = "" And p_intTipoValidacion >= 1 Then

                epConfRepXMarca.SetError(txtUsuarioSBO, My.Resources.ResourceUI.MensajeDebeSeleccionarUsuario)
                epConfRepXMarca.SetIconAlignment(txtUsuarioSBO, ErrorIconAlignment.BottomLeft)
                blnDatosCorrectos = False

            End If

            If txtPasswordSBO.Text = "" And p_intTipoValidacion >= 1 Then

                epConfRepXMarca.SetError(txtPasswordSBO, My.Resources.ResourceUI.MensajeDebeSeleccionarContrasena)
                epConfRepXMarca.SetIconAlignment(txtPasswordSBO, ErrorIconAlignment.BottomLeft)
                blnDatosCorrectos = False

            End If

            If txtAlmacen.Text = "" And p_intTipoValidacion >= 2 Then

                epConfRepXMarca.SetError(txtAlmacen, My.Resources.ResourceUI.MensajeDebeSeleccionarAlmacen)
                epConfRepXMarca.SetIconAlignment(txtAlmacen, ErrorIconAlignment.BottomLeft)
                blnDatosCorrectos = False

            End If

            If txtListaPrecios.Text = "" And p_intTipoValidacion >= 2 Then

                epConfRepXMarca.SetError(txtListaPrecios, My.Resources.ResourceUI.MensajeDebeSeleccionarListaPrecios)
                epConfRepXMarca.SetIconAlignment(txtListaPrecios, ErrorIconAlignment.BottomLeft)
                blnDatosCorrectos = False

            End If

            'If txtProveedor.Tag = "" And p_intTipoValidacion >= 2 Then

            '    epConfRepXMarca.SetError(txtProveedor, My.Resources.ResourceUI.MensajeDebeSeleccionarProveedor)
            '    epConfRepXMarca.SetIconAlignment(txtProveedor, ErrorIconAlignment.BottomLeft)
            '    blnDatosCorrectos = False

            'End If

            Return blnDatosCorrectos

        End Function

        Private Function Guardar() As Boolean

            Dim Coneccion As New SqlClient.SqlConnection
            Dim Transaccion As SqlClient.SqlTransaction = Nothing
            Dim Proveedor As ProveedorXMarcaDataset.SCGTB_TA_ProveedorXMarcaRow
            Dim blnDatosGuardados As Boolean = True
            Dim Configuracion As ConfCatalogoRepXMarcaDataset.SCGTA_TB_ConfCatalogoRepxMarcaRow

            Try


                If ValidarDatos(enumValidarDatos.scgValidarTodo) Then

                    If m_drwConfCatalogos Is Nothing Then
                        m_dstConfCatalogos = Nothing
                        m_dstConfCatalogos = New ConfCatalogoRepXMarcaDataset

                        m_drwConfCatalogos = m_dstConfCatalogos.SCGTA_TB_ConfCatalogoRepxMarca.NewSCGTA_TB_ConfCatalogoRepxMarcaRow

                        Call PasarDatosADataRow()

                        m_dstConfCatalogos.SCGTA_TB_ConfCatalogoRepxMarca.AddSCGTA_TB_ConfCatalogoRepxMarcaRow(m_drwConfCatalogos)

                    Else

                        Call PasarDatosADataRow()

                    End If

                    If Coneccion.State = ConnectionState.Closed Then
                        If Coneccion.ConnectionString = "" Then
                            Coneccion.ConnectionString = strConexionADO
                        End If
                        Call Coneccion.Open()
                    End If

                    m_adpConfCatalogos = New ConfCatalogoRepXMarcaDataAdapter
                    m_adpProveedores = New ProveedorXMarcaDatasetTableAdapters.SCGTB_TA_ProveedorXMarcaTableAdapter
                    m_adpConfCatalogos.Update(m_dstConfCatalogos, Coneccion, Transaccion)
                    ' m_adpConfCatalogos.Update(m_dstConfCatalogos)
                    m_adpProveedores.Connection = Coneccion
                    m_adpProveedores.SetTransaction(Transaccion)
                    For Each Configuracion In m_dstConfCatalogos.SCGTA_TB_ConfCatalogoRepxMarca.Rows
                        If Configuracion.CodMarca = cboMarcas.SelectedValue Then
                            For Each Proveedor In ProveedorXMarca.SCGTB_TA_ProveedorXMarca.Rows
                                If Proveedor.RowState <> DataRowState.Deleted Then
                                    Proveedor.IDCatalogoRepxMarca = Configuracion.ID
                                End If
                            Next
                        End If
                    Next

                    m_adpProveedores.Update(ProveedorXMarca)
                    If Coneccion.State = ConnectionState.Open Then
                        Transaccion.Commit()
                        Coneccion.Close()

                    End If

                    RaiseEvent FinalizoProcesamiento()

                Else
                    blnDatosGuardados = False
                End If
                Return blnDatosGuardados
            Catch ex As Exception

                If Coneccion.State <> ConnectionState.Closed Then
                    Coneccion.Close()
                End If

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

                Throw

            Finally
                'Agregado 05072010
                Coneccion.Close()

            End Try

        End Function

        Private Sub PasarDatosADataRow()

            m_drwConfCatalogos.BDCompañia = m_dicCompañias.Item(cboCompañia.Text)
            m_drwConfCatalogos.CodAlmacen = txtAlmacen.Tag
            m_drwConfCatalogos.CodListaPrecio = txtListaPrecios.Tag
            m_drwConfCatalogos.CodMarca = cboMarcas.SelectedValue
            m_drwConfCatalogos.Compañia = cboCompañia.Text
            m_drwConfCatalogos.DescMarca = cboMarcas.Text
            m_drwConfCatalogos.NombAlmacen = txtAlmacen.Text
            m_drwConfCatalogos.NombListaPrecios = txtListaPrecios.Text
            m_drwConfCatalogos.PasswordSBO = txtPasswordSBO.Text
            m_drwConfCatalogos.PasswordServidor = txtPasswordServidor.Text
            m_drwConfCatalogos.Servidor = txtServidor.Text
            m_drwConfCatalogos.UsuarioSBO = txtUsuarioSBO.Text
            m_drwConfCatalogos.UsuarioServidor = txtUsuarioServidor.Text

        End Sub

        Private Sub CargarCompañias()

            If ValidarDatos(enumValidarDatos.scgValidarSoloServidor) Then

                Dim oRecordSet As SAPbobsCOM.Recordset

                '// Once the Server property of the Company is set
                '// you can query for a list of companies to choose from
                '// This method returns a Recordset object

                If txtServidor.Text <> "" Then

                    If txtUsuarioServidor.Text <> "" Then
                        m_oCompania = Nothing
                        m_oCompania = New SAPbobsCOM.Company
                        m_oCompania.Server = txtServidor.Text
                        m_oCompania.DbUserName = txtUsuarioServidor.Text
                        m_oCompania.DbPassword = txtPasswordServidor.Text

                        oRecordSet = m_oCompania.GetCompanyList

                        '// The returned Recordset contains the following four fields:
                        '// dbName - represents the database name
                        '// cmpName - represents the company name
                        '// versStr - represents the version number of the company database
                        '// dbUser - represents the database owner

                        '// Go through the Recordset and extract the dbname
                        cboCompañia.Items.Clear()
                        m_dicCompañias.Clear()
                        Do Until oRecordSet.EoF = True
                            '// The first field (dbName) of each record
                            m_dicCompañias.Add(oRecordSet.Fields.Item(1).Value() + "(" & oRecordSet.Fields.Item(0).Value() & ")", oRecordSet.Fields.Item(0).Value())
                            cboCompañia.Items.Add(oRecordSet.Fields.Item(1).Value() + "(" & oRecordSet.Fields.Item(0).Value() & ")")
                            '// Move the record pointer to the next row
                            oRecordSet.MoveNext()
                        Loop

                    End If

                End If

            End If

        End Sub

        Private Sub MostrarDatosPantalla()

            If m_drwConfCatalogos IsNot Nothing Then
                cboMarcas.Text = m_drwConfCatalogos.DescMarca

                txtServidor.Text = m_drwConfCatalogos.Servidor
                txtUsuarioServidor.Text = m_drwConfCatalogos.UsuarioServidor
                txtPasswordServidor.Text = m_drwConfCatalogos.PasswordServidor

                Call CargarCompañias()
                cboCompañia.Text = m_drwConfCatalogos.Compañia
                txtUsuarioSBO.Text = m_drwConfCatalogos.UsuarioSBO
                txtPasswordSBO.Text = m_drwConfCatalogos.PasswordSBO

                txtAlmacen.Text = m_drwConfCatalogos.NombAlmacen
                txtListaPrecios.Text = m_drwConfCatalogos.NombListaPrecios
                txtAlmacen.Tag = m_drwConfCatalogos.CodAlmacen
                txtListaPrecios.Tag = m_drwConfCatalogos.CodListaPrecio

                Call CargarProveedores()

            End If
        End Sub

        Private Sub CargarProveedores()
            Dim Coneccion As New SqlClient.SqlConnection

            If Coneccion.State = ConnectionState.Closed Then
                If Coneccion.ConnectionString = "" Then
                    Coneccion.ConnectionString = strConexionADO
                End If
                Call Coneccion.Open()
            End If

            m_adpProveedores = New ProveedorXMarcaDatasetTableAdapters.SCGTB_TA_ProveedorXMarcaTableAdapter
            m_adpProveedores.Connection = Coneccion
            m_adpProveedores.Fill(ProveedorXMarca.SCGTB_TA_ProveedorXMarca, m_drwConfCatalogos.ID)

        End Sub

        Private Function CrearObjetoConeccion(ByVal p_strServidor As String, _
                                              ByVal p_strDatabase As String, _
                                              ByVal p_strUsuarioBD As String, _
                                              ByVal p_strPasswordBD As String) As SqlClient.SqlConnection

            Dim cnConection As New SqlClient.SqlConnection
            Dim strConectionString As String

            strConectionString = "Data Source=" & p_strServidor & _
                                 ";Initial Catalog =" & p_strDatabase & ";" & _
                                 "Connect Timeout=180;" & _
                                 "connection reset=false;" & _
                                 "connection lifetime=5;" & _
                                 "enlist=true;" & _
                                 "min pool size=1;" & _
                                 "max pool size=100;" & _
                                 "Pooling=true;" & _
                                 "User ID=" & p_strUsuarioBD & ";" & _
                                 "pwd=" & p_strPasswordBD & ";" & _
                                 "Trusted_Connection=No"

            cnConection.ConnectionString = strConectionString
            cnConection.Open()
            Return cnConection

        End Function

        Private Sub CargarCatalogos()

            Utilitarios.CargarCombosMarcasVehiculos(cboMarcas)

        End Sub

#End Region



    End Class

End Namespace