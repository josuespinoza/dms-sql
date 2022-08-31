Imports System
Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Namespace SCGDataAccess

    Public Class Utilitarios

#Region "Constantes Privadas"

        Private Const mc_str As String = ""
        Private Const mc_strSelInfraseguro As String = "SCGTA_SP_SELDeducibleInfraseguro"
        Private Const mc_strSelEstadoFase As String = "SCGTA_SP_SELEstadoFase"
        Private Const mc_strSelHayFasesIniciadas As String = "SCGTA_SP_SELHayFasesIniciadas"
        Private Const mc_strSCGTA_SP_SelEmpByID As String = "SCGTA_SP_SelEmpByID"
        Private Const mc_strSCGTA_SP_SELContarLineasPaquete As String = "SCGTA_SP_SELContarLineasPaquete"

        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strNoFase As String = "NoFase"
        Private Const mc_strEmpID As String = "EmpID"
        Private Const mc_strNombre As String = "Nombre"
        Private Const mc_strCodPaquete As String = "CodPaquete"
        Private Const mc_strCantidad As String = "Cantidad"
        Private Const mc_strArroba As String = "@"

        'Procedimientos almacenados
        Private Const mc_strSCGTA_SP_UPDOcupacion As String = "SCGTA_SP_UPDOcupacionMaxPatio"

        'Constantes referentes a las bodegas de SAP
        Private Const mc_strRefacciones As String = "Bodega de Repuestos"
        Private Const mc_strServicios As String = "Bodega de Servicios Externos"
        Private Const mc_strSuministros As String = "Bodega de Suministros"

#End Region

#Region "Constantes Publicas"

        Public Const GC_strEstadoVisita_Proceso As String = "Proceso"
        Public Const GC_strEstadoVisita_Suspendido As String = "Suspendido"
        Public Const GC_strEstadoVisita_Finalizado As String = "Finalizado"
        Public Const GC_strEstadoVisita_Entregado As String = "Entregado"

        Public Const GC_strTextMonedoSys As String = "USD"

#End Region

#Region "Enums Publicos"

        Public Enum RolesMensajeria
            EncargadoRepuestos = 1
            EncargadoProduccion = 2
            EncargadoSolEspec = 3
            EncargadoCompras = 4
            EncargadoSOE = 5
            EncargadoSuministros = 6
        End Enum

        Public Enum GEnum_EstadoVisita
            dmsProceso = 1
            dmsSuspendido = 2
            dmsFinalizada = 3
            dmsEntregado = 4
        End Enum

        Public Enum GEnum_EstadoOrden
            dmsNoIniciada = 1
            dmsProceso = 2
            dmsSuspendida = 3
            dmsFinalizada = 4
        End Enum

#End Region

#Region "Declaraciones"

        Private strSQL As String

        Private cmdConsultar As SqlCommand
        Private cmdConsulta As SqlCommand

        Private Shared m_cnnSCGTaller As SqlClient.SqlConnection

        Private m_adpAgencias As SqlClient.SqlDataAdapter

        Private cmdSeleccion As SqlCommand

        Private cmdSeleccionar As SqlCommand

        Private Shared objDAConexion As New DAConexion

        Private m_adpBodegas As DMSOneFramework.SCGDataAccess.BodegasDataAdapter

        Shared m_strNameSucursal As String

        '------------------------------------------------

#End Region


#Region "Constructor"

        Public Sub New(ByVal gc_Conexion As String)

            '------------- Se inicializa la conexión en el momento que se levanta la clase.
            ' Call InicializaUtilitarios(m_cnnSCGTaller, gc_Conexion)

            Call InicializaUtilitarios(gc_Conexion)
        End Sub

        'constructor 
        'Public Sub New(ByVal company As String, ByVal servidor As String, ByVal DB As String,
        '               ByVal userDB As String, ByVal passDB As String)
        '    _CompanyL = company
        '    _ServerL = servidor
        '    _DBSBOL = DB
        '    _UserDBL = userDB
        '    _PassDBL = passDB

        'End Sub

#End Region

#Region "SubClases"

        Public Class G_ItemCombo

#Region "Declaraciones"

            Private strValor As String
            Private strDescripcion As String
            Public Const mc_Descripcion As String = "Descripcion"
            Public Const mc_Valor As String = "Valor"

#End Region

#Region "Constructor"

            Public Sub New(ByVal p_strDescripcion As String, ByVal p_strValor As String)
                MyBase.New()
                Me.strValor = p_strValor
                Me.strDescripcion = p_strDescripcion
            End Sub

#End Region

#Region "Propiedades"

            Public ReadOnly Property Valor() As String
                Get
                    Return strValor
                End Get
            End Property

            Public ReadOnly Property Descripcion() As String
                Get
                    Return strDescripcion
                End Get
            End Property

            Public Overrides Function ToString() As String
                Return Me.strDescripcion & Space(100) & Me.strValor
            End Function

#End Region

        End Class


#End Region

#Region "Procedimientos"

        ' Private Sub InicializaUtilitarios(ByRef cnnTaller As SqlClient.SqlConnection, ByVal conexion As String)

        Private Sub InicializaUtilitarios(ByVal conexion As String)
            Try


                m_cnnSCGTaller = New SqlClient.SqlConnection(conexion)
                m_strNameSucursal = m_cnnSCGTaller.Database.Trim()
                m_adpBodegas = New DMSOneFramework.SCGDataAccess.BodegasDataAdapter(m_cnnSCGTaller)
                m_adpAgencias = New SqlClient.SqlDataAdapter

                cmdConsultar = m_cnnSCGTaller.CreateCommand()
                cmdConsulta = m_cnnSCGTaller.CreateCommand()

                cmdSeleccionar = m_cnnSCGTaller.CreateCommand()

            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)

            Finally

                ' Call cnnTaller.Close()

            End Try

        End Sub

        Shared Function obtieneIDsucursal() As String
            Try
                Return EjecutarConsulta(String.Format(" Select Code From [@SCGD_SUCURSALES] with (nolock) Where U_BDSucursal = '{0}' ", m_strNameSucursal), strConexionSBO).Trim
            Catch ex As Exception
                Return String.Empty
            End Try


        End Function

        Public Function CargarCombos(ByVal combo As ComboBox, ByVal intCaso As Integer, Optional ByVal Nooptional As Integer = 0, Optional ByVal p_strIdSucursal As String = "") As ComboBox

            '--------------------------------------------- Documentación SCG ------------------------------------------------------
            'Carga cualquier tipo de combo según el criterio en el case.....si se quiere un caso más solo se debe agregar
            'el case + el string SQL Server.
            '----------------------------------------------------------------------------------------------------------------------------
            'Dim drd As SqlDataReader

            Select Case intCaso

                Case Is = 1 'Fases de producción
                    strSQL = "Select NoFase, Descripcion from SCGTA_TB_FasesProduccion with(nolock) where EstadoLogico = 1 Order By Descripcion"

                Case Is = 2 'Centros de costo.
                    '-- String de consulta / tiene un distinc porque pueden existir los mismos centros de costo para varias fases de producción.
                    strSQL = "Select CodCentroCosto,descripcion from SCGTA_TB_CentroCosto with(nolock) where EstadoLogico = 1"

                Case Is = 3 'Marcas de autos
                    'String de consulta para traer todas las marcas de vehiculos registrados.
                    strSQL = "Select CodMarca, Descripcion from SCGTA_TB_Marca with(nolock) where EstadoLogico = 1 Order By Descripcion"

                Case Is = 4 'Estilos de automovil
                    strSQL = "Select codEstilo, descripcion from SCGTA_TB_Estilo with(nolock) where EstadoLogico = 1"

                Case Is = 5 'Clases de automovil
                    strSQL = "Select codClase, descripcion from SCGTA_TB_Clase with(nolock) where EstadoLogico = 1 Order By Descripcion"

                Case Is = 6 'Colores de automovil
                    strSQL = "Select codColor, descripcion from SCGTA_TB_Color with(nolock) where EstadoLogico = 1 Order By Descripcion"

                Case Is = 7 'Secciones '
                    strSQL = "Select NoSeccion,Descripcion from SCGTA_TB_Seccion with(nolock) where EstadoLogico = 1  "

                Case Is = 8 'Piezas 
                    strSQL = "Select NopiezaPrincipal, Descripcion from SCGTA_TB_PiezaPrincipal with(nolock) where EstadoLogico = 1"

                Case Is = 9 'Estados Expediente
                    strSQL = "Select CodEstadoExp, Descripcion from SCGTA_TB_EstadosExpediente with(nolock) where EstadoLogico = 1"

                Case Is = 10 'Tipo Orden
                    strSQL = "Select CodTipoOrden, Descripcion from SCGTA_TB_TipoOrden with(nolock) where EstadoLogico = 1"

                Case Is = 11 ' Cobertura
                    strSQL = "Select CodCobertura, Descripcion from SCGTA_TB_Coberturas with(nolock) where EstadoLogico =1"

                Case Is = 12 'deducibles
                    strSQL = "Select CodDeducible, Descripcion from SCGTA_TB_Deducible with(nolock) where EstadoLogico = 1"

                Case Is = 13 'Actividades
                    strSQL = "Select NoActividad, Descripcion from SCGTA_TB_Actividades with(nolock) where EstadoLogico = 1"

                Case Is = 14 'Estado de lo repuestos
                    ' strSQL = "Select codEstadoRep,Descripcion from SCGTA_TB_EstadoRepuesto "
                    strSQL = "EXEC SCGTA_SP_SelRetornaEstadoRepuestos '" & System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString & "'"



                Case Is = 15 'Actividades con repuestos
                    strSQL = "Select distinct A.NoActividad, A.Descripcion from SCGTA_TB_Actividades A with(nolock) inner join SCGTA_TB_ACTIVIDADESXORDEN AXO with(nolock) ON A.NOACTIVIDAD= AXO.NOACTIVIDAD  where EstadoLogico = 1 and axo.nofase= " & Nooptional

                Case Is = 16 'Empleados por Fase
                    strSQL = "SELECT COD_EMPLEADO, Nombre FROM SCGTA_VW_EMPLEADOS with(nolock) where branch = " & p_strIdSucursal & " and U_SCGD_T_FASE <> ''"

                Case Is = 17 'Actividades por fase
                    strSQL = "Select NoActividad, Descripcion from SCGTA_TB_Actividades with(nolock) where NoFase = " & Nooptional

                Case Is = 18 'Modelo de autos
                    strSQL = "Select CodModelo, Descripcion from SCGTA_TB_MODELO with(nolock) Where EstadoLogico=1"

                Case Is = 19 'Razones de Cita
                    strSQL = "Select NoRazon, Descripcion from SCGTA_TB_RazonesCita with(nolock) Where EstadoLogico=1"

                Case Is = 20 'Servicios 
                    strSQL = "Select CodServicio, Descripcion from SCGTA_TB_Servicios with(nolock)"

                Case Is = 21 'Proveedor
                    strSQL = "Select NoProveedor, Descripcion from SCGTA_TB_ProveedoresGrua with(nolock) Where EstadoLogico=1"

                Case Is = 22 'Agencias
                    strSQL = "Select CodAgencia,Descripcion from SCGTA_TB_Agencias with(nolock) Where EstadoLogico=1"

                Case Is = 24 'Estado de Trámite"
                    strSQL = "Select CodEstadoTra,Descripcion From SCGTA_TB_EstadoTramite with(nolock) Where EstadoLogico=1"

                Case Is = 25 'Estado de Requisitos"
                    strSQL = "Select CodEstadoReq, Descripcion From SCGTA_TB_EstadoReq with(nolock) Where EstadoLogico=1"

                Case Is = 27 'Reprocesos"
                    strSQL = "SELECT NoReproceso,Razon,NoFase,EstadoLogico FROM SCGTA_TB_Reproceso with(nolock) Where EstadoLogico=1"

                Case Is = 28 'Proveedores
                    strSQL = "SELECT CardCode,CardName FROM SCGTA_VW_Proveedores with(nolock)"

                Case Is = 29 'Estado de Repuestos
                    'strSQL = "Select codEstadoRep,Descripcion From dbo.SCGTA_TB_EstadoRepuesto"
                    strSQL = "EXEC SCGTA_SP_SelRetornaEstadoRepuestos '" & System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString & "'"

                Case Is = 30 'Estilos del Vehiculo
                    strSQL = "SELECT Code, Name FROM SCGTA_VW_Estilos with(nolock)"


                Case Is = 230 'Estado Web
                    'strSQL = "Select codEstadoRep,Descripcion From dbo.SCGTA_TB_EstadoRepuesto"
                    strSQL = "EXEC SCGTA_SP_SelRetornaEstadoRepuestos '" & System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString & "'"

            End Select

            'Se abre la conexion

            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar
                    '----Correccion de errores. Dorian 03-04-06
                    'Asigna la instancia de la conexion al command
                    .Connection = m_cnnSCGTaller
                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL

                    'drd = cmdConsultar.ExecuteReader()
                End With

                Using drd As SqlDataReader = cmdConsultar.ExecuteReader

                    combo.Items.Clear()

                    'Se carga el combo con los valores que estan en el datareader mediante el siguiente ciclo.
                    While drd.Read

                        'Funcion que ingresa los valores en el combo.
                        CargarValorCombo(combo, Trim(drd.Item(1)), Trim(drd.Item(0)), True)

                    End While
                End Using

                'Se limpia el combo en caso de que traiga valores ya configurados anteriormente.


            Catch ex As Exception
                Throw ex

            Finally
                'Agregado 02072010
                'Se cierra la conexion
                ' drd.Close()
                m_cnnSCGTaller.Close()
            End Try




        End Function


        Public Function RetornaDataTable(ByVal consulta As String) As DataTable
            ' Dim drd As SqlDataReader
            Dim dt As New DataTable

            strSQL = consulta

            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar
                    '----Correccion de errores. Dorian 03-04-06
                    'Asigna la instancia de la conexion al command
                    .Connection = m_cnnSCGTaller
                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL

                    Using drd As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader
                        dt.Load(drd)
                    End Using
                    'drd = cmdConsultar.ExecuteReader()
                End With

                'drd.fillfrom()

                Return dt

            Catch ex As Exception
                Throw ex
            Finally
                'Se cierra la conexion
                'drd.Close()
                m_cnnSCGTaller.Close()
            End Try
        End Function


        '********************************************************************************************
        'Agregado 29/02/2012: Agregar configuración validación de tiempo estándar



        Public Function TraerValorTiempo() As Boolean

            'Dim drd As SqlDataReader

            Dim valor As String

            strSQL = "SELECT [Valor] FROM [SCGTA_TB_Configuracion] with (nolock) where Propiedad = 'UsaVTiempoEstandar'"

            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar
                    '----Correccion de errores. Dorian 03-04-06
                    'Asigna la instancia de la conexion al command
                    .Connection = m_cnnSCGTaller
                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    ' drd = cmdConsultar.ExecuteReader()
                End With

                Using drd As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader
                    While drd.Read

                        valor = drd.Item(0).ToString()

                        Exit While

                    End While
                End Using

                If valor = "1" Then

                    'drd.Close()
                    'm_cnnSCGTaller.Close()
                    Return True

                Else
                    Return False
                End If

                ''If drd.Item(0).ToString = "1" Then
                ''    drd.Close()
                ''    m_cnnSCGTaller.Close()
                ''    Return True

                ''Else
                'drd.Close()
                'm_cnnSCGTaller.Close()



            Catch ex As Exception
                Throw ex

            Finally
                'Agregado 02072010
                'Se cierra la conexion
                'drd.Close()
                m_cnnSCGTaller.Close()
            End Try




        End Function

        Public Function TraerValorFiltros() As Boolean

            ' Dim drd As SqlDataReader

            Dim valor As String

            strSQL = "SELECT TOP 1000 [Valor] FROM [SCGTA_TB_Configuracion] with(nolock) where Propiedad = 'UsaFiltCliente'"

            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar
                    '----Correccion de errores. Dorian 03-04-06
                    'Asigna la instancia de la conexion al command
                    .Connection = m_cnnSCGTaller
                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    'drd = cmdConsultar.ExecuteReader()
                End With

                Using drd As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader
                    While drd.Read

                        valor = drd.Item(0).ToString()

                        Exit While

                    End While
                End Using


                If valor = "1" Then

                    'drd.Close()
                    'm_cnnSCGTaller.Close()
                    Return True

                Else
                    Return False
                End If

                ''If drd.Item(0).ToString = "1" Then
                ''    drd.Close()
                ''    m_cnnSCGTaller.Close()
                ''    Return True

                ''Else
                'drd.Close()
                'm_cnnSCGTaller.Close()



            Catch ex As Exception
                Throw ex

            Finally
                'Agregado 02072010
                'Se cierra la conexion
                'drd.Close()
                m_cnnSCGTaller.Close()
            End Try




        End Function

        Public Function TraerNiveles() As Integer
            Dim strValorLeido As String

            ' Dim drd As SqlDataReader

            Dim m_intEstadoSuperior As Integer

            strSQL = "Select Max(U_Nivel) from [SCGTA_VW_NIVELES_PV] with(nolock)"

            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar
                    '----Correccion de errores. Dorian 03-04-06
                    'Asigna la instancia de la conexion al command
                    .Connection = m_cnnSCGTaller
                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    'drd = cmdConsultar.ExecuteReader()
                End With

                Using drd As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader

                    While drd.Read

                        strValorLeido = drd.Item(0).ToString()

                        Exit While

                    End While

                End Using



                If IsNumeric(strValorLeido) Then
                    m_intEstadoSuperior = CInt(strValorLeido)
                Else
                    m_intEstadoSuperior = 2
                End If


            Catch ex As Exception
                Throw ex

            Finally
                'Agregado 02072010
                'Se cierra la conexion
                ' drd.Close()
                m_cnnSCGTaller.Close()
            End Try

            Return m_intEstadoSuperior

        End Function

        Public Function TraerTipoCosto() As Boolean

            'Dim drd As SqlDataReader

            Dim valor2 As String

            strSQL = "Select Valor from SCGTA_TB_Configuracion with(nolock) where Propiedad = 'TipoCosto'"

            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar
                    '----Correccion de errores. Dorian 03-04-06
                    'Asigna la instancia de la conexion al command
                    .Connection = m_cnnSCGTaller
                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    'drd = cmdConsultar.ExecuteReader()
                End With

                Using drd As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader
                    While drd.Read

                        valor2 = drd.Item(0).ToString()

                        Exit While

                    End While
                End Using

            Catch ex As Exception
                Throw ex

            Finally
                'Agregado 02072010
                'Se cierra la conexion
                '                drd.Close()
                m_cnnSCGTaller.Close()

            End Try

            If valor2.Trim() <> "1" Then

                Return True

            Else
                Return False

            End If

        End Function

        'retorna valor para manejo o no de citas a clientes inactivos 
        Public Function CitasClientesInactivos() As Boolean
            Dim valorRetorno As String
            strSQL = "Select Valor from SCGTA_TB_Configuracion with(nolock) where Propiedad = 'CitasAClientesInactivos'"

            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then m_cnnSCGTaller.Open()
            Try
                With cmdConsultar
                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    valorRetorno = cmdConsultar.ExecuteScalar()
                End With

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try
            If valorRetorno.Trim() = "1" Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function TraerTipoCompra() As Boolean

            'Dim drd As SqlDataReader

            Dim valor2 As String

            strSQL = "Select Valor from SCGTA_TB_Configuracion with (nolock) where Propiedad = 'TipoCompra'"

            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar
                    '----Correccion de errores. Dorian 03-04-06
                    'Asigna la instancia de la conexion al command
                    .Connection = m_cnnSCGTaller
                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    'drd = cmdConsultar.ExecuteReader()
                End With
                Using drd As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader

                    While drd.Read

                        valor2 = drd.Item(0).ToString()

                        Exit While

                    End While

                End Using

            Catch ex As Exception
                Throw ex

            Finally
                'Agregado 02072010
                'Se cierra la conexion
                ' drd.Close()
                m_cnnSCGTaller.Close()

            End Try

            If valor2.Trim() = "2" Then

                'Configuracion Tipo Oferta de Compra
                Return True

            Else

                'Configuracion Tipo Orden de Compra
                Return False

            End If

        End Function



        Public Function TraerEmail(ByVal CodCliente As String) As String

            'Dim drd As SqlDataReader

            Dim valor2 As String

            strSQL = "SELECT E_Mail FROM [SCGTA_VW_Clientes] with(nolock) where CardCode = '" & CodCliente & "'"

            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar
                    '----Correccion de errores. Dorian 03-04-06
                    'Asigna la instancia de la conexion al command
                    .Connection = m_cnnSCGTaller
                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    'drd = cmdConsultar.ExecuteReader()
                End With
                Using drd As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader
                    While drd.Read

                        valor2 = drd.Item(0).ToString()

                        Exit While

                    End While

                End Using

            Catch ex As Exception
                Throw ex

            Finally
                'Agregado 02072010
                'Se cierra la conexion
                'drd.Close()
                m_cnnSCGTaller.Close()
            End Try

            If valor2 Is Nothing Then

                Return Nothing
                'Return "----"

            Else
                Return valor2
            End If



        End Function

        Public Function TraerConfiguracionServicios() As String

            ' Dim drd As SqlDataReader

            Dim valor2 As String

            strSQL = "Select Valor from SCGTA_TB_Configuracion with(nolock) where Propiedad = 'CosteoServicios'"

            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar
                    '----Correccion de errores. Dorian 03-04-06
                    'Asigna la instancia de la conexion al command
                    .Connection = m_cnnSCGTaller
                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    ' drd = cmdConsultar.ExecuteReader()
                End With

                Using drd As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader
                    While drd.Read

                        valor2 = drd.Item(0).ToString()

                        Exit While

                    End While

                End Using

            Catch ex As Exception
                Throw ex

            Finally
                'Agregado 02072010
                'Se cierra la conexion
                '                drd.Close()
                m_cnnSCGTaller.Close()
            End Try

            If valor2 Is Nothing Then

                Return Nothing
                'Return "----"

            Else
                Return valor2
            End If



        End Function

        Public Function TraerSalarioColaborador(ByVal IDColaborador As String) As Boolean

            ' Dim drd As SqlDataReader

            Dim valor2 As String

            strSQL = "select U_SCGD_sALXHORA from SCGTA_VW_OHEM with(nolock) where empID = '" & IDColaborador & "'"

            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar
                    '----Correccion de errores. Dorian 03-04-06
                    'Asigna la instancia de la conexion al command
                    .Connection = m_cnnSCGTaller
                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    'drd = cmdConsultar.ExecuteReader()
                End With
                Using drd As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader
                    While drd.Read

                        valor2 = drd.Item(0).ToString()

                        Exit While

                    End While
                End Using

            Catch ex As Exception
                Throw ex

            Finally
                'Agregado 02072010
                'Se cierra la conexion
                'drd.Close()
                m_cnnSCGTaller.Close()
            End Try

            If IsNumeric(valor2) Then
                If CDec(valor2) <= 0 Then

                    Return True
                    'Return "----"

                Else
                    Return False
                End If


            Else

                Return True

            End If


        End Function

        Public Function ObtenerNombreCuenta(ByVal NumCuenta As String) As String

            ' Dim drd As SqlDataReader

            Dim valor As String

            strSQL = "SELECT ACCTNAME FROM [SCGTA_VW_OACT] with(nolock) where ACCTCODE = '" & NumCuenta & "'"

            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar
                    '----Correccion de errores. Dorian 03-04-06
                    'Asigna la instancia de la conexion al command
                    .Connection = m_cnnSCGTaller
                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    'drd = cmdConsultar.ExecuteReader()
                End With
                Using drd As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader
                    While drd.Read

                        valor = drd.Item(0).ToString().Trim()

                        Exit While

                    End While

                End Using

            Catch ex As Exception
                Throw ex

            Finally
                'Agregado 02072010
                'Se cierra la conexion
                ' drd.Close()
                m_cnnSCGTaller.Close()
            End Try

            If valor Is Nothing Then

                Return Nothing
                'Return "----"

            Else
                Return valor
            End If



        End Function


        Public Function CargarDeducibleInfraseguro(ByVal combo As ComboBox) As ComboBox
            'Agregado el 22/05/06.  Alejandra

            'Dim drd As SqlDataReader
            Dim strConsulta As String
            Dim strValor As String


            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsulta

                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = mc_strSelInfraseguro
                    ' drd = cmdConsulta.ExecuteReader()
                End With

                'Se limpia el combo en caso de que traiga valores ya configurados anteriormente.
                combo.Items.Clear()

                Using drd As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader
                    'Se carga el combo con los valores Descripcion, CheckInfraseguro, CodDeducible.
                    While drd.Read
                        'Ingresa los valores en el combo.
                        strValor = Trim(drd.Item(1)) & Space(100) & Trim(drd.Item(2) & Space(1) & Trim(drd.Item(0)))
                        combo.Items.Add(strValor)

                    End While
                End Using


            Catch ex As Exception
                Throw ex
            Finally
                'Ágregado 02072010 
                'Se cierra la conexion
                'drd.Close()
                m_cnnSCGTaller.Close()
            End Try


        End Function


        '----------------------------------------------------------------------------
        'Nombre: CargarFechaHoraServidor.
        'Descripcion: Obtiene la fecha  y hora actual consultada desde la base de datos 
        '             del servidor
        '
        'Parametros: ninguno
        '
        'Dorian Alvarado M.      07-04-06
        '-----------------------------------------------------------------------------
        Public Function CargarFechaHoraServidor() As DateTime

            'Dim drdFechaHoraServidor As SqlDataReader

            '-- Carga los modelos de autos para una marca específica.
            ' Dim strNombreProcedimiento As String = "SCGTA_SP_SelFechaServidor"


            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar

                    'Se le asigna una instancia de la coneccion -- Dorian 04-04-06
                    .Connection = m_cnnSCGTaller
                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = "Select GetDate()" 'strNombreProcedimiento
                End With
                Using drdFechaHoraServidor As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader
                    'drdFechaHoraServidor = cmdConsultar.ExecuteReader

                    If drdFechaHoraServidor.Read() Then

                        Return CDate(drdFechaHoraServidor.Item(0))

                    End If
                End Using

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
                'drdFechaHoraServidor.Close()
                'drdFechaHoraServidor = Nothing
            End Try

        End Function


        Shared Sub CargarCombosMarcasVehiculos(ByRef p_cboMarca As ComboBox)
            Dim adpMarcas As DMSOneFramework.SCGDataAccess.MarcaDataAdapter
            Dim drdMarcas As SqlDataReader

            Try

                adpMarcas = New DMSOneFramework.SCGDataAccess.MarcaDataAdapter

                adpMarcas.CargaMarcasdeVehiculo(drdMarcas)

                CargarComboSourceByReader(p_cboMarca, drdMarcas)

                drdMarcas.Close()

            Catch ex As Exception

                Throw ex

            Finally

                'Agregado 02072010
                If drdMarcas IsNot Nothing Then
                    If Not drdMarcas.IsClosed Then
                        Call drdMarcas.Close()
                    End If
                End If

                m_cnnSCGTaller.Close()

            End Try

        End Sub

        Shared Sub CargarComboEstilosVehiculos(ByRef p_cboEstilos As ComboBox, ByVal p_strCodMarca As String)
            Dim adpEstilos As DMSOneFramework.SCGDataAccess.EstiloDataAdapter
            Dim drdEstilos As SqlDataReader

            Try

                adpEstilos = New DMSOneFramework.SCGDataAccess.EstiloDataAdapter

                adpEstilos.CargaEstilosdeVehiculo(drdEstilos, p_strCodMarca)

                CargarComboSourceByReader(p_cboEstilos, drdEstilos)

                drdEstilos.Close()

            Catch ex As Exception
                Throw ex
            Finally
                'Agregado 01072010
                If drdEstilos IsNot Nothing Then
                    If Not drdEstilos.IsClosed Then
                        Call drdEstilos.Close()
                    End If
                End If
            End Try
        End Sub

        Shared Sub CargarComboModelosVehiculos(ByRef p_cboModelos As ComboBox, ByVal p_strCodEstilo As String)
            Dim adpModelos As DMSOneFramework.SCGDataAccess.ModelosDataAdapter
            Dim drdModelos As SqlDataReader

            Try

                adpModelos = New DMSOneFramework.SCGDataAccess.ModelosDataAdapter

                adpModelos.CargaModelosdeVehiculo(drdModelos, p_strCodEstilo)

                CargarComboSourceByReader(p_cboModelos, drdModelos)

                drdModelos.Close()

            Catch ex As Exception
                Throw ex
            Finally
                'Agregado 02072010
                If drdModelos IsNot Nothing Then
                    If Not drdModelos.IsClosed Then
                        Call drdModelos.Close()
                    End If
                End If
            End Try
        End Sub

        Shared Sub CargarComboCentrosdeCosto(ByRef p_cboCC As ComboBox)
            Dim adpCentrosCosto As DMSOneFramework.SCGDataAccess.CentroCostoDataAdapter
            Dim drdCentrosCosto As SqlDataReader

            Try

                adpCentrosCosto = New DMSOneFramework.SCGDataAccess.CentroCostoDataAdapter

                adpCentrosCosto.CargaCentrosCostoByReader(drdCentrosCosto)

                CargarComboSourceByReader(p_cboCC, drdCentrosCosto)

                drdCentrosCosto.Close()

            Catch ex As Exception
                Throw ex
            Finally
                'Agregado 01072010
                If drdCentrosCosto IsNot Nothing Then
                    If Not drdCentrosCosto.IsClosed Then
                        Call drdCentrosCosto.Close()
                    End If
                End If
            End Try
        End Sub

        Public Function cargarCombosSoloModelos(ByVal cboModelo As ComboBox, ByVal intCodMarca As Integer) As ComboBox
            'Dim drd As SqlDataReader
            Dim strSQL As String

            Try
                'Se abre la conexion
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If

                strSQL = "Select Descripcion from SCGTA_TB_Modelo with(nolock) where EstadoLogico = 1 and codMarca = '" & intCodMarca & "' "

                With cmdConsultar

                    'Instancia la conexion para el command --Dorian
                    .Connection = m_cnnSCGTaller

                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    'drd = cmdConsultar.ExecuteReader()
                End With

                'Se limpia el combo en caso de que traiga valores ya configurados anteriormente.
                cboModelo.Items.Clear()

                Using drd As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader
                    'Se carga el combo con los valores que estan en el datareader mediante el siguiente ciclo.
                    While drd.Read
                        'Funcion que ingresa los valores en el combo.
                        cboModelo.Items.Add(Trim(drd.Item(0)))
                    End While
                End Using



            Catch ex As Exception
                Throw ex
            Finally
                'Se cierra la conexion
                'drd.Close()

                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Function retornaNoSeccion(ByVal intNoPieza As Integer) As Integer

            Dim intNoSeccion
            ' Dim drdSeccion As SqlDataReader

            Try

                strSQL = "Select NoSeccion from SCGTA_TB_PiezaPrincipal with(nolock) where NoPiezaPrincipal = " & intNoPieza & ""

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If

                With cmdConsultar
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    'drdSeccion = cmdConsultar.ExecuteReader()
                End With

                Using drdSeccion As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader
                    While drdSeccion.Read
                        intNoSeccion = Trim(drdSeccion.Item(0))
                    End While

                End Using

                Return intNoSeccion

            Catch ex As Exception
                Throw ex
            Finally
                'drdSeccion.Close()

                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Function retornaCodMarca(ByVal strDescripcionMarca As String) As Integer

            Dim intCodMarca
            'Dim drdMarca As SqlDataReader

            Try

                strSQL = "Select CodMarca from SCGTA_TB_Marca with(nolock) where Descripcion = '" & strDescripcionMarca & "'"

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If

                With cmdConsultar
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    'drdMarca = cmdConsultar.ExecuteReader()
                End With

                Using drdMarca As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader

                    While drdMarca.Read
                        intCodMarca = Trim(drdMarca.Item(0))
                    End While

                End Using

                Return intCodMarca

            Catch ex As Exception
                Throw ex
            Finally
                'drdMarca.Close()
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Function retornaPiezaSeccion(ByVal intNoPieza As Integer) As String

            Dim strNombre As String

            Dim drdSeccion As SqlDataReader

            Try

                strSQL = "SELECT  T0.Descripcion  + ',' + T1.Descripcion FROM SCGTA_TB_PiezaPrincipal T0 with(nolock) inner join SCGTA_TB_Seccion T1 with(nolock) on T1.NoSeccion=T0.NoSeccion Where T0.NoPiezaPrincipal=" & intNoPieza & ""


                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If

                With cmdConsultar
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    drdSeccion = cmdConsultar.ExecuteReader()
                End With

                While drdSeccion.Read
                    strNombre = Trim(drdSeccion.Item(0))

                End While

                Return strNombre

            Catch ex As Exception
                Throw ex
            Finally
                drdSeccion.Close()
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Function retornaCodigoEstilo(ByVal strNoOrden As String) As Integer
            Dim cmdretornaCodigoEstilo As SqlCommand

            Dim strNombre As String

            Dim drwOrden As SqlDataReader

            Try

                cmdretornaCodigoEstilo = New SqlCommand

                strSQL = "SELECT T0.CodEstilo FROM SCGTA_TB_Vehiculo T0 with(nolock) INNER JOIN SCGTA_TB_Orden T1 with(nolock) ON T0.NoVehiculo = T1.NoVehiculo WHERE T1.NoOrden = '" & strNoOrden & "'"

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If

                With cmdretornaCodigoEstilo
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    .Connection = m_cnnSCGTaller
                End With

                drwOrden = cmdretornaCodigoEstilo.ExecuteReader()

                While drwOrden.Read
                    strNombre = Trim(drwOrden.Item(0))
                End While

                Return strNombre

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try


        End Function

        Public Function CargarComboFaseXOrden(ByVal combo As ComboBox, ByVal strOrden As String) As ComboBox

            'Dim drd As SqlDataReader

            Try

                strSQL = "SELECT FXC.NoFase,F.Descripcion  " & _
                        "FROM SCGTA_TB_FASESXORDEN FXC with(nolock) INNER JOIN SCGTA_TB_FASESproduccion F with(nolock) " & _
                        "	ON FXC.NoFase = F.NoFase  " & _
                        "WHERE NoOrden='" & strOrden & "' and EstadoLogico='True'"

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If

                With cmdConsultar

                    'Instancia la conexion para el command --Dorian
                    .Connection = m_cnnSCGTaller

                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    'drd = cmdConsultar.ExecuteReader()
                End With

                combo.Items.Clear()

                Using drd As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader

                    While drd.Read
                        CargarValorCombo(combo, Trim(drd.Item(1)), Trim(drd.Item(0)), True)
                    End While

                End Using



            Catch ex As Exception
                Throw ex
            Finally
                'drd.Close()
                m_cnnSCGTaller.Close()
            End Try
        End Function

        Public Function CargarComboExpediente(ByVal combo As ComboBox, ByVal intNoExpediente As Decimal) As ComboBox

            '--------------------------------------------- Documentación SCG ------------------------------------------------------
            'Carga cualquier tipo de combo según el criterio en el case.....si se quiere un caso más solo se debe agregar
            'el case + el string SQL Server.
            '----------------------------------------------------------------------------------------------------------------------------

            Dim drd As SqlDataReader

            strSQL = "Select NoOrden, NoExpediente from SCGTA_TB_Orden with(nolock) where NoExpediente = '" & intNoExpediente & "'"

            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar

                    'Instancia la conexion para el command --Dorian
                    .Connection = m_cnnSCGTaller

                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    drd = cmdConsultar.ExecuteReader()

                End With

                'Se limpia el combo en caso de que traiga valores ya configurados anteriormente.
                combo.Items.Clear()

                'Se carga el combo con los valores que estan en el datareader mediante el siguiente ciclo.
                While drd.Read

                    'Funcion que ingresa los valores en el combo.
                    CargarValorCombo(combo, drd.Item(0), drd.Item(1), True)

                End While

            Catch ex As Exception
                Throw ex
            Finally
                'Se cierra la conexion

                '--! Jonathan Vargas V.

                m_cnnSCGTaller.Close()
                drd.Close()

            End Try

        End Function

        Public Function CargarComboRazones(ByVal combo As ComboBox, ByVal NoFase As String) As ComboBox

            '--------------------------------------------- Documentación SCG ------------------------------------------------------
            'Carga cualquier tipo de combo según el criterio en el case.....si se quiere un caso más solo se debe agregar
            'el case + el string SQL Server.
            '----------------------------------------------------------------------------------------------------------------------------

            Dim drd As SqlDataReader

            strSQL = "SELECT NoReproceso, Razon FROM SCGTA_TB_Reproceso with(nolock) Where EstadoLogico=1 and NoFase=" & NoFase


            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar

                    'Instancia la conexion para el command --Dorian
                    .Connection = m_cnnSCGTaller

                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    drd = cmdConsultar.ExecuteReader()

                End With

                'Se limpia el combo en caso de que traiga valores ya configurados anteriormente.
                combo.Items.Clear()

                'Se carga el combo con los valores que estan en el datareader mediante el siguiente ciclo.
                While drd.Read

                    'Funcion que ingresa los valores en el combo.
                    CargarValorCombo(combo, drd.Item(1), drd.Item(0), True)

                End While

            Catch ex As Exception
                Throw ex

            Finally
                'Se cierra la conexion
                m_cnnSCGTaller.Close()
                drd.Close()

                '--! Jonathan Vargas V.
            End Try


        End Function

        Public Function CargarComboSuspensiones(ByVal combo As ComboBox, ByVal NoFase As String) As ComboBox

            '--------------------------------------------- Documentación SCG ------------------------------------------------------
            'Carga cualquier tipo de combo según el criterio en el case.....si se quiere un caso más solo se debe agregar
            'el case + el string SQL Server.
            '----------------------------------------------------------------------------------------------------------------------------

            Dim drd As SqlDataReader

            strSQL = "SELECT NoSuspension,Razon FROM SCGTA_TB_Suspension with(nolock) Where Estadologico=1 and NoFase=" & NoFase


            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar

                    'Instancia la conexion para el command --Dorian
                    .Connection = m_cnnSCGTaller
                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    drd = cmdConsultar.ExecuteReader()

                End With

                'Se limpia el combo en caso de que traiga valores ya configurados anteriormente.
                combo.Items.Clear()

                'Se carga el combo con los valores que estan en el datareader mediante el siguiente ciclo.
                While drd.Read

                    'Funcion que ingresa los valores en el combo.
                    CargarValorCombo(combo, drd.Item(1), drd.Item(0), True)

                End While

            Catch ex As Exception
                Throw ex
            Finally
                'Se cierra la conexion
                m_cnnSCGTaller.Close()
                drd.Close()

                '--! Jonathan Vargas V.
            End Try


        End Function

        'Public Function OrdenProrrateada(ByVal decNoExpediente As Decimal, ByVal decCantPaneles As Decimal, ByVal strNoOrden As String) As Decimal
        '    'Modificado 07/08/06. Alejandra. Recibe como nuevo parametro el NoOrden.
        '    'Este método se usa cuando se crea la orden para saber si es una orden prorrateada o no.
        '    'Devuelve el porcentaje de prorrateo. 
        '    'En caso que sea la primer orden que se crea para un expediente devuelve un 100%

        '    Dim dstOrdenes As OrdenTrabajoDataset
        '    Dim adpOrdenes As OrdenTrabajoDataAdapter
        '    Dim drwOrden As OrdenTrabajoDataset.SCGTA_TB_OrdenRow
        '    Dim decCantTotalPaneles As Decimal
        '    Dim decPorcentajeProrrateo As Decimal

        '    adpOrdenes = New SCGDataAccess.OrdenTrabajoDataAdapter
        '    dstOrdenes = New OrdenTrabajoDataset
        '    drwOrden = dstOrdenes.SCGTA_TB_Orden.NewRow()

        '    'Se traen todas las ordenes asociadas al expediente que se desea meter la nueva orden
        '    Call adpOrdenes.Fill(dstOrdenes, decNoExpediente)

        '    If dstOrdenes.SCGTA_TB_Orden.Rows.Count > 0 Then

        '        'Se obtiene el TOTAL de paneles ya registrados para el expediente.
        '        For Each drwOrden In dstOrdenes.SCGTA_TB_Orden
        '            'Agregado 07/08/06. Alejandra. Si la orden ya està creada pero se actualizó el numero de paneles
        '            'no debe sumar la cantidad anterior de páneles para esa órden, sólo debe sumar la cantidad nueva
        '            If drwOrden.NoOrden <> strNoOrden Then
        '                decCantTotalPaneles = decCantTotalPaneles + drwOrden.Paneles
        '            End If
        '            ' decCantTotalPaneles = decCantTotalPaneles + drwOrden.Paneles
        '        Next drwOrden

        '        'Se suma a la cantidad de paneles ya existente la cantidad por registrar
        '        decCantTotalPaneles = decCantTotalPaneles + decCantPaneles

        '        'Se actualiza el porcentaje de prorrateo para cada orden asociada al expediente
        '        For Each drwOrden In dstOrdenes.SCGTA_TB_Orden

        '            drwOrden.PorcentajeProrrateo = (drwOrden.Paneles * 100) / decCantTotalPaneles

        '            drwOrden.Prorrateo = 1

        '        Next drwOrden

        '        'Se actualiza en Base de datos los porcentajes de prorrateo
        '        adpOrdenes.Update(dstOrdenes)

        '        'Se estima el porcentaje de prorrateo para la nueva orden
        '        decPorcentajeProrrateo = (decCantPaneles * 100) / decCantTotalPaneles

        '        'Se retorna el porcentaje de prorrateo
        '        Return decPorcentajeProrrateo

        '    Else

        '        decPorcentajeProrrateo = 100
        '        Return decPorcentajeProrrateo

        '    End If



        '    '--! Jonathan Vargas V.
        'End Function

        Public Function CargarComboModelos(ByVal combo As ComboBox, ByVal codMarca As Integer) As ComboBox


            '--------------------------------------------- Documentación SCG ------------------------------------------------------
            ' Carga el combo de modelos de automovil, este no se incluyó en la funcion general de cargacombos ya que este
            ' recibe por valor el codigo de la marca asociada
            '----------------------------------------------------------------------------------------------------------------------------

            Dim dr As SqlDataReader

            '-- Carga los modelos de autos para una marca específica.
            strSQL = "Select codModelo, descripcion from SCGTA_TB_Modelo with(nolock) where EstadoLogico = 1 AND codMarca = " & codMarca & "Order By descripcion"


            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdConsultar
                    'Se le asigna una instancia de la coneccion -- Dorian 04-04-06
                    .Connection = m_cnnSCGTaller
                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    dr = cmdConsultar.ExecuteReader()

                End With

                'Se limpia el combo en caso de que traiga valores ya configurados anteriormente.
                combo.Items.Clear()

                'Se carga el combo con los valores que estan en el datareader mediante el siguiente ciclo.
                While dr.Read

                    'Funcion que ingresa los valores en el combo.
                    CargarValorCombo(combo, dr.Item(1), dr.Item(0), True)

                End While


            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
                dr.Close()
            End Try

            '--! Jonathan Vargas V.
        End Function

        Public Function CargarComboActividades(ByVal combo As ComboBox, ByVal NoFase As Integer) As ComboBox

            '--------------------------------------------- Documentación SCG ------------------------------------------------------
            ' Carga el las actividades en un combo con base en el Numero de la fase que le envien
            ' Cada actividad está asociada a una única Fase de Producción.
            '----------------------------------------------------------------------------------------------------------------------------

            Dim drActividades As SqlDataReader

            '-- Carga los modelos de autos para una marca específica.
            strSQL = "Select NoActividad, descripcion from SCGTA_TB_Actividades with(nolock) where EstadoLogico = 1 AND NoFase = " & NoFase


            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If


            Try

                With cmdConsultar

                    'Instancia la conexion para el command --Dorian
                    .Connection = m_cnnSCGTaller

                    'El comando de ejecucion va a ser tipo text ya que la consulta es simple y es mas trabajo hacer un Store Procedure para esto.
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    .Connection = m_cnnSCGTaller
                    drActividades = cmdConsultar.ExecuteReader
                End With

                'Se limpia el combo en caso de que traiga valores ya configurados anteriormente.
                combo.Items.Clear()


                If Not IsNothing(drActividades) Then
                    'Se carga el combo con los valores que estan en el datareader mediante el siguiente ciclo.
                    While drActividades.Read

                        'Funcion que ingresa los valores en el combo.
                        CargarValorCombo(combo, drActividades.Item(1), drActividades.Item(0), True)

                    End While
                End If

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

            '--! Jonathan Vargas V.

        End Function

        Public Function DevuelveSeccion(ByVal codpieza As Integer) As Integer

            'Función que devuelve el numero de sección para una pieza principal
            Dim drd As SqlDataReader
            Dim n As Integer
            strSQL = "Select NoSeccion from SCGTA_TB_PiezaPrincipal with(nolock) where NoPiezaPrincipal = " & codpieza

            m_cnnSCGTaller.Open()

            Try
                With cmdConsultar

                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    drd = cmdConsultar.ExecuteReader()

                End With

                If drd.Read Then

                    n = drd.Item(0)
                    drd.Close()
                    m_cnnSCGTaller.Close()
                    Return n
                Else
                    Return -1
                End If

            Catch ex As Exception
                Throw ex

            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Function DevuelveTiempoAprobado(ByVal codorden As String, ByVal fase As Integer) As Decimal

            'Función que devuelve el numero de sección para una pieza principal
            ' Dim drd As SqlDataReader
            Dim n As Decimal

            strSQL = "Select DuracionHorasAprobadas from SCGTA_TB_FasesXOrden with(nolock) where NoOrden = '" & codorden & "' and nofase = " & CStr(fase)

            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try
                With cmdConsultar

                    'asigna la conexion
                    .Connection = m_cnnSCGTaller

                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    'drd = cmdConsultar.ExecuteReader()

                End With

                Using drd As SqlClient.SqlDataReader = cmdConsultar.ExecuteReader
                    If drd.Read Then

                        If Not drd.Item(0) Is DBNull.Value Then
                            n = drd.Item(0)
                        Else
                            n = 0
                        End If
                        'drd.Close()
                        m_cnnSCGTaller.Close()
                        Return n
                    Else
                        'drd.Close()
                        m_cnnSCGTaller.Close()
                        Return 0
                    End If
                End Using


            Catch ex As Exception
                Throw ex

            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Sub CargarValorCombo(ByRef p_objCombo As ComboBox, ByVal p_strValorVisible As String, ByVal p_strValorInvisible As String, ByVal blnDerecha As Boolean)

            '-------------------------------------------- Documentacion SCG --------------------------------------------------
            'Sirve para cargar los combos con los valores que se hayan en los datareaders que consultan 
            'la Base de Datos usualmente esta función se manda a llamar desde un ciclo.
            '-----------------------------------------------------------------------------------------------------------------------

            Dim strValor As String

            If blnDerecha Then

                strValor = p_strValorVisible & Space(100) & p_strValorInvisible

            Else

                strValor = p_strValorInvisible & "- " & p_strValorVisible

            End If

            p_objCombo.Items.Add(strValor)




        End Sub

        Public Function DevuelveNoExpediente(ByVal codorden As String) As Integer

            'Función que devuelve el numero de expediente para una orden
            Dim drd As SqlDataReader
            Dim n As Integer
            strSQL = "Select noexpediente from SCGTA_TB_Orden with(nolock) where NoOrden = '" & codorden & "'"

            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try
                With cmdConsultar
                    .Connection = m_cnnSCGTaller

                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    drd = cmdConsultar.ExecuteReader()

                End With

                If drd.Read Then

                    n = drd.Item(0)
                    drd.Close()
                    m_cnnSCGTaller.Close()
                    Return n
                Else
                    drd.Close()
                    m_cnnSCGTaller.Close()
                    Return 0
                End If

            Catch ex As Exception
                Throw ex

            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Function obtieneNombreFaseProduccion(ByVal intNoFase As Integer) As String

            Dim drdFaseProduccion As SqlDataReader
            Dim strFase As String

            Try

                strSQL = "Select Descripcion from SCGTA_TB_FasesProduccion with(nolock) where NoFase = '" & intNoFase & "'"

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If


                With cmdConsultar

                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    drdFaseProduccion = cmdConsultar.ExecuteReader()

                End With

                If drdFaseProduccion.Read Then

                    strFase = drdFaseProduccion.Item(0)
                    drdFaseProduccion.Close()
                    m_cnnSCGTaller.Close()

                    Return strFase
                Else
                    drdFaseProduccion.Close()
                    m_cnnSCGTaller.Close()
                    strFase = My.Resources.ResourceFrameWork.FaseNoAsociada
                    Return strFase
                End If

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try


        End Function

        Public Function obtieneEquivalenciaFaseProduccionLapOp(ByVal intLapOP As Integer) As Integer

            Dim drdFaseProduccion As SqlDataReader
            Dim intFase As String

            Try

                strSQL = "Select NoFase from SCGTB_TA_FaseProduccionxLapOp with(nolock) where LapOp = '" & intLapOP & "'"

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If


                With cmdConsultar

                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    drdFaseProduccion = cmdConsultar.ExecuteReader()

                End With

                If drdFaseProduccion.Read Then

                    intFase = drdFaseProduccion.Item(0)
                    drdFaseProduccion.Close()
                    m_cnnSCGTaller.Close()
                    Return intFase

                Else
                    drdFaseProduccion.Close()
                    m_cnnSCGTaller.Close()
                    intFase = "1"
                    Return intFase

                End If

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try


        End Function

        Public Function CargaValoresHorarios() As SqlDataReader
            Dim drdDatos As SqlDataReader

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                Else
                    m_cnnSCGTaller.Close()
                    m_cnnSCGTaller.Open()
                End If

                With cmdConsultar
                    .CommandType = CommandType.Text
                    .CommandText = "SELECT * FROM SCGTA_TB_HorarioTaller with(nolock)"
                    .Connection = m_cnnSCGTaller
                    drdDatos = cmdConsultar.ExecuteReader(CommandBehavior.CloseConnection)
                End With

                Return drdDatos

            Catch ex As Exception
                Throw ex
            Finally
                'Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Sub CargaValorRango(ByRef p_strRango As String)
            Try

                Dim adtConfiguracion As New ConfiguracionDataAdapter
                Dim dtbConfiguracion As New ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable

                adtConfiguracion.Fill(dtbConfiguracion)
                adtConfiguracion.DevuelveValorDeParametosConfiguracion(dtbConfiguracion, "RangoRampas", p_strRango)


            Catch ex As Exception

                Throw ex

            End Try

        End Sub

        Public Sub CargaValoresHorarios(ByVal dtFechaIni As Date, ByVal dtFechaFin As Date, ByVal dblTotalHoras As Double, ByVal dblTiempoDescanso As Double)
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If

                With cmdConsultar
                    .Parameters.Add("@FechaIni", SqlDbType.DateTime).Value = dtFechaIni
                    .Parameters.Add("@FechaFin", SqlDbType.DateTime).Value = dtFechaFin
                    .Parameters.Add("@TotalHoras", SqlDbType.Decimal).Value = dblTotalHoras
                    .Parameters.Add("@TiempoDescanso", SqlDbType.Decimal).Value = dblTiempoDescanso
                    .CommandType = CommandType.Text
                    .CommandText = "INSERT INTO SCGTA_TB_HorarioTaller (FechaIni,FechaFin,TotalHoras,TiempoDescanso) VALUES (@FechaIni,@FechaFin,@TotalHoras,@TiempoDescanso)"
                    .Connection = m_cnnSCGTaller
                    .ExecuteNonQuery()
                End With


            Catch ex As Exception
                Throw ex
            Finally

                If m_cnnSCGTaller.State = ConnectionState.Open Then
                    m_cnnSCGTaller.Close()
                End If

            End Try

        End Sub

        Public Sub ModificarValoresHorarios(ByVal dtFechaIni As Date, ByVal dtFechaFin As Date, ByVal dblTotalHoras As Double, ByVal dblTiempoDescanso As Double)
            Try
                Dim strCommandText As String

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If

                With cmdConsultar
                    .Parameters.Add("@FechaIni", SqlDbType.DateTime).Value = dtFechaIni
                    .Parameters.Add("@FechaFin", SqlDbType.DateTime).Value = dtFechaFin
                    .Parameters.Add("@TotalHoras", SqlDbType.Decimal).Value = dblTotalHoras
                    .Parameters.Add("@TiempoDescanso", SqlDbType.Decimal).Value = dblTiempoDescanso
                    .CommandType = CommandType.Text
                    .Connection = m_cnnSCGTaller
                    .CommandText = "UPDATE SCGTA_TB_HorarioTaller SET FechaIni=@FechaIni,FechaFin=@FechaFin,TotalHoras=@TotalHoras,TiempoDescanso=@TiempoDescanso"
                    cmdConsultar.ExecuteNonQuery()

                End With

            Catch ex As Exception
                Throw ex
            Finally

                If m_cnnSCGTaller.State = ConnectionState.Open Then
                    m_cnnSCGTaller.Close()
                End If

            End Try

        End Sub

        Shared Sub CargarComboSourceByReader(ByRef p_objComboBox As ComboBox, ByVal p_drdListaDatos As SqlClient.SqlDataReader)
            Dim alstItemsCombo As New ArrayList

            If Not IsNothing(CType(p_objComboBox.DataSource, ArrayList)) Then
                CType(p_objComboBox.DataSource, ArrayList).Clear()
            End If

            p_objComboBox.DataSource = Nothing

            If Not IsNothing(p_drdListaDatos) Then

                While p_drdListaDatos.Read
                    alstItemsCombo.Add(New G_ItemCombo(p_drdListaDatos.Item(1), p_drdListaDatos.Item(0)))
                End While

                If alstItemsCombo.Count <> 0 Then

                    p_objComboBox.DataSource = alstItemsCombo
                    p_objComboBox.DisplayMember = G_ItemCombo.mc_Descripcion
                    p_objComboBox.ValueMember = G_ItemCombo.mc_Valor

                End If

            End If

        End Sub

        Public Function obtenerNombreUsuario(ByVal usuario As String, ByVal compania As String, ByVal aplicacion As String) As String

            Dim strNombreUsuario As String
            Dim strNombreProcedimiento As String = "SCGTA_SP_SELNombreUsuario"
            Dim c_strArroba As String = "@"
            Dim c_strUsuario As String = "Usuario"
            Dim c_strCompania As String = "Compania"
            Dim c_strAplicacion As String = "Aplicacion"

            Dim cmdSeleccionar As New SqlClient.SqlCommand

            'Se abre la conexion
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

            Try

                With cmdSeleccionar
                    'Se le asigna una instancia de la conexion
                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = strNombreProcedimiento
                End With

                With cmdSeleccionar
                    .Parameters.Add(c_strArroba & c_strUsuario, SqlDbType.NVarChar, 50, c_strUsuario)
                    .Parameters.Add(c_strArroba & c_strCompania, SqlDbType.NVarChar, 50, c_strCompania)
                    .Parameters.Add(c_strArroba & c_strAplicacion, SqlDbType.NVarChar, 50, c_strAplicacion)
                    .Parameters(c_strArroba & c_strUsuario).Value = usuario
                    .Parameters(c_strArroba & c_strCompania).Value = compania
                    .Parameters(c_strArroba & c_strAplicacion).Value = aplicacion

                End With

                strNombreUsuario = cmdSeleccionar.ExecuteScalar

                If Not IsNothing(strNombreUsuario) Then

                    Return strNombreUsuario

                End If

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Function Busca_Codigo_Texto(ByVal strTempItem As String, Optional ByVal blnGetCodigo As Boolean = True) As String
            'Agregado en Utilitarios el 17/05/06

            '------------------------------------------------ Documentación SCG -----------------------------------------------------------
            '-- Busca el texto en el string enviado....si usas true busca el de la derecha y si usas falses busca el de la izquierda
            '------------------------------------------------------------------------------------------------------------------------------------

            Dim strCod_Item_Comp As String
            Dim strTemp As String
            Dim intCharCont As Integer
            Dim strTextoNoCodigo As String

            strTemp = ""
            strCod_Item_Comp = ""
            Try


                If strTempItem <> "" Then

                    For intCharCont = strTempItem.Length - 1 To 0 Step -1
                        If strTempItem.Chars(intCharCont).IsWhiteSpace(strTempItem.Chars(intCharCont)) Then
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

                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Sub Busca_Item_Combo(ByRef Combo As ComboBox, ByVal Cod_Item As String)
            'Agregado en Utilitarios el 17/05/06
            Dim intItemCont As Integer
            Dim strTempItem As String
            Dim strCod_Item_Comp As String
            Dim blnExiste As Boolean
            Dim ind As Integer
            Try


                With Combo

                    If .Items.Count <> 0 Then
                        blnExiste = False
                        For intItemCont = 0 To .Items.Count - 1
                            strTempItem = .Items(intItemCont)
                            strCod_Item_Comp = Busca_Codigo_Texto(strTempItem)
                            If Cod_Item = strCod_Item_Comp Then
                                ind = intItemCont
                                blnExiste = True
                                Exit For
                            End If
                        Next

                        If blnExiste Then
                            Combo.SelectedIndex = ind
                        End If
                    End If

                End With
            Catch ex As Exception
                Throw ex
            End Try

        End Sub

        Public Function ReaderFasesProd(ByVal strOrden As String) As SqlClient.SqlDataReader
            'Agregado 03/07/06. Alejandra. Devuelve un dataReader con las fases de produccion
            Dim drd As SqlDataReader
            Dim cmdConsulta As New SqlClient.SqlCommand
            Dim strSQL As String


            Try

                strSQL = "SELECT F.Descripcion, FXC.NoFase  " & _
                        "FROM SCGTA_TB_FASESXORDEN FXC with(nolock) INNER JOIN SCGTA_TB_FASESproduccion F with(nolock) " & _
                        "	ON FXC.NoFase = F.NoFase and F.EstadoLogico = 1" & _
                        "WHERE NoOrden='" & strOrden & "'"

                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If
                m_cnnSCGTaller.Open()
                With cmdConsulta

                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    drd = cmdConsulta.ExecuteReader(CommandBehavior.CloseConnection)
                End With

                Return drd

            Catch ex As Exception
                Throw ex
            Finally
                'm_cnnSCGTaller.Close()
            End Try
        End Function

        Public Function retornaEstadoFase(ByVal strNoOrden As String, ByVal intNoFase As Integer) As String
            'Agregado 03/07/06. Alejandra. Devuelve el estado de una fase para una determinada orden
            Dim strEstado As String
            Dim cmd As New SqlClient.SqlCommand


            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If

                With cmd

                    .CommandType = CommandType.StoredProcedure
                    .CommandText = mc_strSelEstadoFase
                    .Connection = m_cnnSCGTaller

                    .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.NVarChar, 50)
                    .Parameters.Add(mc_strArroba & mc_strNoFase, SqlDbType.Int, 4)
                    .Parameters(mc_strArroba & mc_strNoOrden).Value = strNoOrden
                    .Parameters(mc_strArroba & mc_strNoFase).Value = intNoFase

                End With

                strEstado = cmd.ExecuteScalar
                Return strEstado

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try
        End Function

        Public Function retornaEstadoFase(ByVal strNoOrden As String, ByVal intNoFase As Integer, _
                                          ByRef p_cnConeccion As SqlClient.SqlConnection, _
                                          ByRef p_tnnTransaccion As SqlClient.SqlTransaction) As String


            Dim strEstado As String
            Dim cmd As New SqlClient.SqlCommand


            Try
                If p_cnConeccion Is Nothing Then
                    p_cnConeccion = New SqlClient.SqlConnection
                End If
                If p_cnConeccion.State = ConnectionState.Closed Then
                    If p_cnConeccion.ConnectionString = "" Then
                        p_cnConeccion.ConnectionString = m_cnnSCGTaller.ConnectionString
                    End If
                    p_cnConeccion.Open()
                    p_tnnTransaccion = p_cnConeccion.BeginTransaction
                End If

                With cmd

                    .CommandType = CommandType.StoredProcedure
                    .CommandText = mc_strSelEstadoFase
                    .Connection = p_cnConeccion
                    .Transaction = p_tnnTransaccion

                    .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.NVarChar, 50)
                    .Parameters.Add(mc_strArroba & mc_strNoFase, SqlDbType.Int, 4)
                    .Parameters(mc_strArroba & mc_strNoOrden).Value = strNoOrden
                    .Parameters(mc_strArroba & mc_strNoFase).Value = intNoFase

                End With

                strEstado = cmd.ExecuteScalar
                Return strEstado

            Catch ex As Exception
                Throw ex
            Finally
                'Call p_cnConeccion.Close()
                'm_cnnSCGTaller.Close()
            End Try
        End Function

        Public Function HayFasesIniciadas(ByVal strNoOrden As String) As Integer
            'Agregado 17/08/06. Alejandra. Determina si hay fases iniciadas para la orden de trabajo
            Dim intFases As String
            Dim cmd As New SqlClient.SqlCommand

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If

                With cmd

                    .CommandType = CommandType.StoredProcedure
                    .CommandText = mc_strSelHayFasesIniciadas
                    .Connection = m_cnnSCGTaller

                    .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.NVarChar, 50)
                    .Parameters(mc_strArroba & mc_strNoOrden).Value = strNoOrden

                End With

                intFases = cmd.ExecuteScalar
                Return intFases

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Shared Function GetPostingPeriod(ByVal p_dtFecha As Date) As String

            Dim reader As SqlDataReader

            Dim myCommand As New SqlCommand("SCGTA_SP_SELPOSTPERIOD")
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Connection = New SqlConnection
            myCommand.Connection = objDAConexion.ObtieneConexion()

            myCommand.Parameters.Add(New SqlParameter("@Fecha", SqlDbType.DateTime))
            myCommand.Parameters.Item("@Fecha").Value = p_dtFecha

            reader = myCommand.ExecuteReader
            If reader.HasRows Then
                reader.Read()
                GetPostingPeriod = reader.GetString(0)
            Else
                GetPostingPeriod = ""
            End If

            myCommand.Connection.Close()
            myCommand.Connection.Dispose()
        End Function

        Public Function GF_CargarIDBodegaRep() As String

            Dim strCodigoBodega As String
            Dim strNombreBodega As String

            Try
                m_adpBodegas.SeleccionarBodegas(strCodigoBodega, strNombreBodega, mc_strRefacciones)
                Return strCodigoBodega

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Function GF_CargarIDBodegaSum() As String

            Dim strCodigoBodega As String
            Dim strNombreBodega As String

            Try
                m_adpBodegas.SeleccionarBodegas(strCodigoBodega, strNombreBodega, mc_strSuministros)
                Return strCodigoBodega

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GF_CargarIDBodegaSer() As String

            Dim strCodigoBodega As String
            Dim strNombreBodega As String

            Try
                m_adpBodegas.SeleccionarBodegas(strCodigoBodega, strNombreBodega, mc_strServicios)
                Return strCodigoBodega

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Sub CerrarConexionPendiente()
            If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                m_cnnSCGTaller.Close()
            End If
        End Sub

        Public Function GetEmpNombre(ByVal p_intEmpID As Integer) As String
            Dim drdEmpleados As SqlClient.SqlDataReader
            Dim strReturn As String

            With cmdConsulta

                .CommandText = mc_strSCGTA_SP_SelEmpByID
                .CommandType = CommandType.StoredProcedure
                .Connection.Open()

                .Parameters.Add(mc_strArroba & mc_strEmpID, SqlDbType.Int, 4).Value = p_intEmpID

                drdEmpleados = .ExecuteReader(CommandBehavior.CloseConnection)

            End With

            If drdEmpleados.Read Then
                strReturn = drdEmpleados.Item(mc_strNombre)
            End If

            drdEmpleados.Close()

        End Function

        Public Function CantidadLineasPaquetes(ByVal p_strCodigoPaquete As String) As Integer

            'Dim intCantidadLineas As String
            'Dim drdCantidad As SqlClient.SqlDataReader
            'If cmdConsulta IsNot Nothing Then
            '    cmdConsulta.Dispose()
            '    cmdConsulta = Nothing

            '    cmdConsulta = m_cnnSCGTaller.CreateCommand()
            'End If
            'With cmdConsulta

            '    .CommandText = mc_strSCGTA_SP_SELContarLineasPaquete
            '    .CommandType = CommandType.StoredProcedure
            '    If .Connection.State = ConnectionState.Closed Then
            '        .Connection.Open()
            '    End If

            '    .Parameters.Add(mc_strArroba & mc_strCodPaquete, SqlDbType.NVarChar, 20).Value = p_strCodigoPaquete
            '    .Parameters.Add(mc_strArroba & mc_strCantidad, SqlDbType.Int, 4).Direction = ParameterDirection.Output

            '    .ExecuteNonQuery()
            '    intCantidadLineas = .Parameters.Item(mc_strArroba & mc_strCantidad).Value

            'End With
            Return -1

        End Function

        Public Function ObtenerItemsCotizaRepetidosByItemCode(ByVal p_intDocEntry As Integer, _
                            ByVal p_intLineNum As Integer, ByVal p_strItemCode As String) As Cotizacion_LineasDataset

            Dim dstCotizacionLineas As New Cotizacion_LineasDataset
            Dim adpCotizacionLineas As New SqlDataAdapter
            Try
                With cmdConsulta

                    .CommandText = "SELECT DocEntry " & _
                                         ",LineNum " & _
                                         ",ItemCode " & _
                                         ",Quantity " & _
                                         ",OpenQty " & _
                                         ",U_SCGD_IdRepxOrd " & _
                                         ",U_SCGD_Aprobado " & _
                                         ",U_SCGD_Traslad " & _
                                         ",U_SCGD_CodEspecifico " & _
                                    "FROM SCGTA_VW_QUT1 with(nolock) " & _
                                    "WHERE DocEntry = @DocEntry " & _
                                        "AND ItemCode = @ItemCode " & _
                                    "ORDER BY LineNum"

                    .CommandType = CommandType.Text
                    .Connection = m_cnnSCGTaller

                    With .Parameters
                        .Add("@DocEntry", SqlDbType.Int).Value = p_intDocEntry
                        ''.Add("@LineNum", SqlDbType.Int).Value = p_LineNum
                        .Add("@ItemCode", SqlDbType.VarChar, 20).Value = p_strItemCode
                    End With

                End With

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    cmdConsulta.Connection.Open()
                End If

                adpCotizacionLineas.SelectCommand = cmdConsulta

                adpCotizacionLineas.Fill(dstCotizacionLineas.Cotizacion_Lineas)

                m_cnnSCGTaller.Close()
            Catch ex As Exception
                Throw
            End Try
            Return dstCotizacionLineas

        End Function

        Public Function ObtenerConfiguracionCompañia() As String

            Dim strConfiguracion As String = ""
            Dim drdOADM As SqlClient.SqlDataReader

            With cmdConsulta

                .CommandText = "Select Country from SCGTA_VW_OADM with(nolock)"
                .CommandType = CommandType.Text
                .Connection = m_cnnSCGTaller
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    .Connection.Open()
                End If

                drdOADM = .ExecuteReader(CommandBehavior.CloseConnection)

            End With

            If drdOADM.Read Then
                strConfiguracion = drdOADM.GetString(0)
            End If

            drdOADM.Close()
            Return strConfiguracion

        End Function

        Shared Function ObtenerMonedaSistema() As String

            Dim cmdAsignarConsultaEjecutar As New SqlClient.SqlCommand
            Dim strRetorno As String

            cmdAsignarConsultaEjecutar.Connection = m_cnnSCGTaller
            cmdAsignarConsultaEjecutar.CommandType = CommandType.Text



            cmdAsignarConsultaEjecutar.CommandText = "Select syscurrncy from SCGTA_VW_OADM with(nolock)"
            cmdAsignarConsultaEjecutar.CommandType = CommandType.Text
            cmdAsignarConsultaEjecutar.Connection = m_cnnSCGTaller
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                cmdAsignarConsultaEjecutar.Connection.Open()
            End If

            strRetorno = cmdAsignarConsultaEjecutar.ExecuteScalar()


            Return strRetorno

        End Function

        Shared Function ObtenerMonedaLocal() As String

            Dim cmdAsignarConsultaEjecutar As New SqlClient.SqlCommand
            Dim strRetorno As String

            cmdAsignarConsultaEjecutar.Connection = m_cnnSCGTaller
            cmdAsignarConsultaEjecutar.CommandType = CommandType.Text



            cmdAsignarConsultaEjecutar.CommandText = "Select maincurncy from SCGTA_VW_OADM with(nolock)"
            cmdAsignarConsultaEjecutar.CommandType = CommandType.Text
            cmdAsignarConsultaEjecutar.Connection = m_cnnSCGTaller
            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                cmdAsignarConsultaEjecutar.Connection.Open()
            End If

            strRetorno = cmdAsignarConsultaEjecutar.ExecuteScalar()



            Return strRetorno

        End Function

        Shared Sub DestruirObjeto(ByRef objDocumento As Object)
            If Not objDocumento Is Nothing Then
                'Destruyo el Objeto - Error HRESULT  
                System.Runtime.InteropServices.Marshal.ReleaseComObject(objDocumento)
                objDocumento = Nothing
            End If
        End Sub

        Shared Function AsignarEmpleado(ByVal p_intNoCotizacion As Integer, _
                                                      ByVal p_strIDEmpleado As String, _
                                                      ByVal p_intLineNum As Integer, _
                                                      ByVal p_strNombreEmpleado As String) As Boolean
            Try

                Dim cmdAsignarEmpleado As New SqlClient.SqlCommand
                Dim intResult As Integer

                cmdAsignarEmpleado.Connection = m_cnnSCGTaller
                cmdAsignarEmpleado.CommandType = CommandType.Text

                cmdAsignarEmpleado.CommandText = "Update dbo.SCGTA_VW_OQUT_QUT1 set U_SCGD_EmpAsig ='" & p_strIDEmpleado & _
                "', U_SCGD_NombEmpleado='" & p_strNombreEmpleado & "' where DocEntry =" & p_intNoCotizacion & _
                " and Linenum = " & p_intLineNum

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If

                intResult = cmdAsignarEmpleado.ExecuteNonQuery()

            Catch ex As Exception
                Return False
            Finally

                If m_cnnSCGTaller.State = ConnectionState.Open Then
                    m_cnnSCGTaller.Close()
                End If

            End Try

        End Function

        Shared Function RetornaDescripcionOT(ByVal intTipoOrden As Integer) As String
            Try

                Dim cmdAsignarEmpleado As New SqlClient.SqlCommand
                cmdAsignarEmpleado.Connection = m_cnnSCGTaller
                cmdAsignarEmpleado.CommandType = CommandType.Text

                cmdAsignarEmpleado.CommandText = "select Descripcion from SCGTA_TB_TipoOrden with(nolock) where CodTipoOrden =" & intTipoOrden

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If

                RetornaDescripcionOT = cmdAsignarEmpleado.ExecuteScalar

            Catch ex As Exception
                Return False
            Finally

                If m_cnnSCGTaller.State = ConnectionState.Open Then
                    m_cnnSCGTaller.Close()
                End If

            End Try

        End Function

        Shared Function RetornaDescripcionCuentaContable(ByVal strTipoOrden As String) As String
            Try

                Dim cmdAsignarEmpleado As New SqlClient.SqlCommand
                cmdAsignarEmpleado.Connection = m_cnnSCGTaller
                cmdAsignarEmpleado.CommandType = CommandType.Text

                cmdAsignarEmpleado.CommandText = "select AcctName from SCGTA_VW_OACT with(nolock) where AcctCode ='" & strTipoOrden & "'"

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If

                RetornaDescripcionCuentaContable = cmdAsignarEmpleado.ExecuteScalar

            Catch ex As Exception
                Return False
            Finally

                If m_cnnSCGTaller.State = ConnectionState.Open Then
                    m_cnnSCGTaller.Close()
                End If

            End Try

        End Function

        'Shared Function ValidarCodigoUnidad(ByVal p_strCodigoUnidad As String) As String
        '    Try

        '        Dim cmValidarCodUnid As New SqlClient.SqlCommand
        '        cmValidarCodUnid.Connection = m_cnnSCGTaller
        '        cmValidarCodUnid.CommandType = CommandType.Text

        '        cmValidarCodUnid.CommandText = "select IdVehiculo from SCGTA_VW_Vehiculos2 where NoVehiculo ='" & p_strCodigoUnidad & "'"

        '        If m_cnnSCGTaller.State = ConnectionState.Closed Then
        '            m_cnnSCGTaller.Open()
        '        End If

        '        ValidarCodigoUnidad = cmValidarCodUnid.ExecuteScalar

        '    Catch ex As Exception
        '        Return False
        '    Finally

        '        If m_cnnSCGTaller.State = ConnectionState.Open Then
        '            m_cnnSCGTaller.Close()
        '        End If

        '    End Try

        'End Function



        'Shared Function BuscarDocNum(ByVal strDocEntry As String, ByVal TipoDocumento As SAPbobsCOM.BoObjectTypes) As String
        '    Try

        '        Dim cmdBuscarDocNum As New SqlClient.SqlCommand
        '        Dim strTablaABuscar As String
        '        cmdBuscarDocNum.Connection = m_cnnSCGTaller
        '        cmdBuscarDocNum.CommandType = CommandType.Text


        '        If TipoDocumento = SAPbobsCOM.BoObjectTypes.oPurchaseOrders Then
        '            strTablaABuscar = "SCGTA_VW_OPOR"
        '        End If

        '        cmdBuscarDocNum.CommandText = "Select DocNum from " & strTablaABuscar & _
        '        " where DocEntry = " & strDocEntry


        '        If m_cnnSCGTaller.State = ConnectionState.Closed Then
        '            m_cnnSCGTaller.Open()
        '        End If

        '        BuscarDocNum = cmdBuscarDocNum.ExecuteNonQuery()

        '    Catch ex As Exception
        '        Return False
        '    Finally

        '        If m_cnnSCGTaller.State = ConnectionState.Open Then
        '            m_cnnSCGTaller.Close()
        '        End If

        '    End Try

        'End Function

#End Region

        Public Shared Function EjecutarConsulta(ByRef p_strConsulta As String, _
                                 ByRef p_strConexion As String) As String

            'Dim drdResultadoConsulta As SqlClient.SqlDataReader
            'Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
            Dim strConectionString As String = ""
            ' Dim cn_Coneccion As New SqlClient.SqlConnection
            Dim strValor As String = ""

            Try
                Using cn_Coneccion As New SqlClient.SqlConnection(p_strConexion)

                    ' cn_Coneccion.ConnectionString = p_strConexion
                    cn_Coneccion.Open()

                    Using cmdEjecutarConsulta As New SqlClient.SqlCommand

                        cmdEjecutarConsulta.Connection = cn_Coneccion

                        cmdEjecutarConsulta.CommandType = CommandType.Text
                        cmdEjecutarConsulta.CommandText = p_strConsulta

                        Using drdResultadoConsulta As SqlClient.SqlDataReader = cmdEjecutarConsulta.ExecuteReader

                            'drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()
                            Do While drdResultadoConsulta.Read
                                If drdResultadoConsulta.Item(0) IsNot DBNull.Value Then
                                    strValor = drdResultadoConsulta.Item(0)
                                    Exit Do
                                End If
                            Loop

                        End Using

                    End Using

                End Using

            Catch
                Throw
            Finally
                'drdResultadoConsulta.Close()
                'cmdEjecutarConsulta.Connection.Close()
            End Try
            Return strValor

        End Function

        ''' <summary>
        ''' Retorna Monedas de los articulos enviados
        ''' </summary>
        ''' <param name="p_strConsulta">Consulta a ejecutar</param>
        ''' <param name="p_strConexion">Conexion</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function DevuelveMonedasCodigos(ByRef p_strConsulta As String, _
                                 ByRef p_strConexion As String) As List(Of String)

            Dim drdResultadoConsulta As SqlClient.SqlDataReader
            Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
            Dim strConectionString As String = ""
            Dim cn_Coneccion As New SqlClient.SqlConnection
            Dim strValor As String = ""
            Dim contador As Integer = 0
            Dim lsMonedas As List(Of String) = New List(Of String)

            Try
                cn_Coneccion.ConnectionString = p_strConexion
                cn_Coneccion.Open()

                cmdEjecutarConsulta.Connection = cn_Coneccion

                cmdEjecutarConsulta.CommandType = CommandType.Text
                cmdEjecutarConsulta.CommandText = p_strConsulta
                drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()
                While drdResultadoConsulta.Read

                    If IsDBNull(drdResultadoConsulta.Item(contador)) Then
                        'lsMonedas.Add("Null")
                    Else
                        lsMonedas.Add(drdResultadoConsulta.Item(contador))
                    End If

                    contador += contador
                End While
            Catch
                Throw
            Finally
                drdResultadoConsulta.Close()
                cmdEjecutarConsulta.Connection.Close()
            End Try
            Return lsMonedas

        End Function

        ''' <summary>
        ''' Obtiene el próximo DocEntry
        ''' </summary>
        ''' <param name="udoID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ObtieneAutoKey(ByVal udoID As String, ByVal cadenaConexion As String) As Integer
            Dim consulta As String = String.Format("select autokey from SCGTA_VW_ONNM with(nolock) where [ObjectCode] = '{0}'", udoID)
            Dim autoKey As Integer = Integer.Parse(EjecutarConsulta(consulta, cadenaConexion))
            Return autoKey
        End Function

        Public Shared Function DevuelveCodIndicadores(ByVal LineId As String, ByVal company As String,
                                               ByVal server As String, ByVal db As String,
                                               ByVal userdb As String, ByVal passdb As String) As String

            'valor asignado
            Dim Indicador As String = ""

            Try
                'Acceso local a BD
                Dim daCnx As New DAConexion
                Dim compania As New SCGCommon.clsCompany()

                compania.Server = server
                compania.Company = company
                compania.DataBase = db
                compania.DBUser = userdb
                compania.UserName = userdb
                compania.DBPassword = passdb
                compania.Password = passdb

                'devuelve el vindicador para ese tipo de documento
                Indicador = Utilitarios.EjecutarConsulta("SELECT U_Cod_Ind FROM [dbo].[@SCGD_ADMIN8] with(nolock) WHERE LineId = '" & LineId & "'", objDAConexion.CadenaConexionSBO(compania))

                If Not String.IsNullOrEmpty(Indicador) Then
                    Return Indicador
                End If

                Return ""

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Shared Function RetornaFechaFormatoRegional(ByVal FechaSinFormato As String) As String

            'Obtengo la Formato de fecha y el separador de la configuracion global de la maquina
            Dim FormatoFecha As String = ""
            Dim SeparadorFecha As String = ""
            Dim dt As Date

            FormatoFecha = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern
            SeparadorFecha = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator

            If Not String.IsNullOrEmpty(FormatoFecha) _
                And Not String.IsNullOrEmpty(SeparadorFecha) _
                And Not String.IsNullOrEmpty(FechaSinFormato) Then

                'doy formato a fecha el string 
                dt = CDate(FechaSinFormato)
                'fecha a retornar formateada
                Dim dtFechaFormateada As Date
                'convierto el string a fecha ya formateada
                dtFechaFormateada = Date.ParseExact(dt, FormatoFecha, Nothing)
                'la formateo de modo yyyyMMdd
                dtFechaFormateada = New Date(dtFechaFormateada.Year, dtFechaFormateada.Month, dtFechaFormateada.Day)

                Dim strFechaFormateada As String = ""
                strFechaFormateada = String.Format(dtFechaFormateada.Year & "{0}" & dtFechaFormateada.Month & "{0}" & dtFechaFormateada.Day, SeparadorFecha)

                'retorno fecha formateada
                Return strFechaFormateada
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' Manejo de multimoneda para los precios de las solicitudes de especificos, Servicios, Refacciones y Servicios Externos
        ''' </summary>
        ''' <param name="PreciosinConvert">Precio sin convertir</param>
        ''' <param name="strMonedaCotizacion">Moneda de la cotizacion</param>
        ''' <param name="decTipoCambioCotizacion">Tipo de cambio de la cotizacion</param>
        ''' <remarks></remarks>
        Shared Function ManejoMultimonedaPrecios(ByVal decPrecioSinConvertir As Decimal,
                                                  ByVal strMonedaCotizacion As String,
                                                  ByVal decTipoCambioCotizacion As Decimal,
                                                  ByVal decTipoCambioMS As Decimal,
                                                  ByVal decCodArticulo As String,
                                                  ByVal strFechaCotizacion As String,
                                                  ByVal strMonedaLocal As String,
                                                  ByVal strMonedaSistema As String,
                                                  ByRef strCurrencyArticulo As String)

            Dim strTipoCambioME As String = ""
            Dim decTipoCambioME As Decimal = 0

            'Variables para manejo de Errores
            Dim intError As Integer = -2
            Dim strVacio As String = ""

            Try

                'Tipo de Cambio de la Moneda Extranjera segun Fecha contable
                If decTipoCambioME = 0 And strCurrencyArticulo <> strMonedaLocal And strCurrencyArticulo <> strMonedaSistema Then
                    If Not String.IsNullOrEmpty(strFechaCotizacion) And Not String.IsNullOrEmpty(strCurrencyArticulo) Then
                        strTipoCambioME = Utilitarios.EjecutarConsulta(
                                                            String.Format("select Rate from SCGTA_VW_ORTT with(nolock) where RateDate = '{0}' and Currency = '{1}'",
                                                                           strFechaCotizacion.ToString.Trim(), strCurrencyArticulo.ToString.Trim()),
                                                           strConexionADO)

                        If Not String.IsNullOrEmpty(strTipoCambioME) Then decTipoCambioME = Decimal.Parse(strTipoCambioME)
                    End If
                End If

                If Not String.IsNullOrEmpty(decPrecioSinConvertir.ToString.Trim()) Then

                    Select Case strCurrencyArticulo

                        'Si el item no tiene Moneda en la lista de precios
                        Case strVacio
                            Return decPrecioSinConvertir

                            'Moneda Local
                        Case strMonedaLocal
                            Select Case strMonedaCotizacion

                                Case strMonedaLocal
                                    Return decPrecioSinConvertir

                                Case strMonedaSistema
                                    If Not String.IsNullOrEmpty(decTipoCambioCotizacion.ToString.Trim()) _
                                        And decTipoCambioCotizacion <> 0 Then

                                        Return decPrecioSinConvertir / decTipoCambioCotizacion

                                    End If

                                Case Else
                                    If Not String.IsNullOrEmpty(decTipoCambioCotizacion.ToString.Trim()) _
                                        And decTipoCambioCotizacion <> 0 Then

                                        Return decPrecioSinConvertir / decTipoCambioCotizacion

                                    End If
                            End Select

                            'Moneda Sistema
                        Case strMonedaSistema

                            Select Case strMonedaCotizacion

                                Case strMonedaLocal
                                    If Not String.IsNullOrEmpty(decTipoCambioMS.ToString.Trim()) _
                                        And decTipoCambioMS <> 0 Then
                                        Return decPrecioSinConvertir * decTipoCambioMS
                                    Else
                                        Return intError
                                    End If

                                Case strMonedaSistema
                                    Return decPrecioSinConvertir

                                Case Else
                                    If Not String.IsNullOrEmpty(decTipoCambioMS.ToString.Trim()) _
                                        And Not String.IsNullOrEmpty(decTipoCambioCotizacion.ToString.Trim()) _
                                        And decTipoCambioCotizacion <> 0 And decTipoCambioMS <> 0 _
                                        Then
                                        Return (decPrecioSinConvertir * decTipoCambioMS) / decTipoCambioCotizacion
                                    Else
                                        Return intError
                                    End If
                            End Select

                            'Moneda Extranjera
                        Case Else

                            Select Case strMonedaCotizacion

                                Case strMonedaLocal
                                    If Not String.IsNullOrEmpty(decTipoCambioME) _
                                        And decTipoCambioME <> 0 Then
                                        Return decPrecioSinConvertir * decTipoCambioME
                                    Else
                                        Return intError + -2
                                    End If

                                Case strMonedaSistema
                                    If Not String.IsNullOrEmpty(decTipoCambioME.ToString.Trim()) _
                                      And Not String.IsNullOrEmpty(decTipoCambioCotizacion.ToString.Trim()) _
                                      And decTipoCambioME <> 0 And decTipoCambioCotizacion <> 0 _
                                      Then
                                        Return (decPrecioSinConvertir * decTipoCambioME) / decTipoCambioCotizacion
                                    Else
                                        Return intError + -2
                                    End If

                                Case Else
                                    Return decPrecioSinConvertir
                            End Select

                    End Select

                    'FIN DEL IF
                End If

            Catch ex As Exception

            End Try

        End Function

        'Public Function CargarComboEstadoWeb(ByVal combo As ComboBox, ByVal p_strValorVisible As String, ByVal p_strValorInvisible As String) As ComboBox

        '    Try


        '        'Se limpia el combo en caso de que traiga valores ya configurados anteriormente.
        '        combo.Items.Clear()

        '        'Se carga el combo con los valores que estan en el datareader mediante el siguiente ciclo.
        '        While drd.Read

        '            'Funcion que ingresa los valores en el combo.
        '            CargarValorCombo(combo, drd.Item(1), drd.Item(0), True)

        '        End While

        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        'Se cierra la conexion
        '        m_cnnSCGTaller.Close()
        '        drd.Close()

        '        '--! Jonathan Vargas V.
        '    End Try


        'End Function





        ''' <summary>
        ''' devuelve el valor de la propiedad en la tabla de configuracion
        ''' </summary>
        ''' <param name="dtbConfiguracion"></param>
        ''' <param name="strPropiedad"></param>
        ''' <param name="strValor"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DevuelveValorDeParametosConfiguracion(ByVal dtbConfiguracion As DMSOneFramework.ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable, _
                                                                        ByVal strPropiedad As String, _
                                                                        ByRef strValor As String) As Boolean

            Dim drwConfiguracion As DMSOneFramework.ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow

            Try

                drwConfiguracion = dtbConfiguracion.FindByPropiedad(strPropiedad)
                strValor = ""
                If Not drwConfiguracion Is Nothing _
                   AndAlso drwConfiguracion.Valor <> "" Then
                    If drwConfiguracion.Valor = 1 Then
                        Return True
                    Else
                        Return False
                    End If
                    'strValor = drwConfiguracion.Valor
                Else
                    Return False
                End If

            Catch ex As Exception
                Throw
            End Try

        End Function


        ''' <summary>
        ''' devuelve el valor de la propiedad en la tabla de configuracion
        ''' </summary>
        ''' <param name="dtbConfiguracion"></param>
        ''' <param name="strPropiedad"></param>
        ''' <param name="strValor"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DevuelveValorDeParametosConfiguracionValor(ByVal dtbConfiguracion As DMSOneFramework.ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable, _
                                                                        ByVal strPropiedad As String, _
                                                                        ByRef strValor As String)

            Dim drwConfiguracion As DMSOneFramework.ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow

            Try

                drwConfiguracion = dtbConfiguracion.FindByPropiedad(strPropiedad)
                strValor = ""
                If Not drwConfiguracion Is Nothing _
                   AndAlso drwConfiguracion.Valor <> "" Then

                    strValor = drwConfiguracion.Valor

                End If

            Catch ex As Exception
                Throw
            End Try

        End Function

        Public Shared Function DevuelveCodeProyecto(ByVal NumeroOT As String, ByVal company As String,
                                                   ByVal server As String, ByVal db As String,
                                                   ByVal userdb As String, ByVal passdb As String) As String

            Dim strConsultaProyectos As String = "select U_SCGD_Proyec from OQUT with(nolock) where U_SCGD_Numero_OT = '{0}'"
            Dim strProyecto As String = String.Empty

            Try
                'Acceso local a BD
                Dim daCnx As New DAConexion
                Dim compania As New SCGCommon.clsCompany()

                compania.Server = server
                compania.Company = company
                compania.DataBase = db
                compania.DBUser = userdb
                compania.UserName = userdb
                compania.DBPassword = passdb
                compania.Password = passdb

                'devuelve el vindicador para ese tipo de documento
                strProyecto = Utilitarios.EjecutarConsulta(String.Format(strConsultaProyectos, NumeroOT), objDAConexion.CadenaConexionSBO(compania))

                If Not String.IsNullOrEmpty(strProyecto) Then
                    Return strProyecto
                End If

                Return ""

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Shared Function EjecutarConsultaDataTable(ByRef p_strConsulta As String, _
                                     ByRef p_strConectionString As String) As System.Data.DataTable

            Dim drdResultadoConsulta As SqlClient.SqlDataReader = Nothing
            Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
            Dim cn_Coneccion As New SqlClient.SqlConnection
            Dim dt As New System.Data.DataTable
            Try
                'objDAConexion.CadenaConexionSBO(compania)
                'Configuracion.CrearCadenaDeconexion(p_strServerName, p_strDatabaseName, strConectionString)
                cn_Coneccion.ConnectionString = p_strConectionString
                cn_Coneccion.Open()

                cmdEjecutarConsulta.Connection = cn_Coneccion

                cmdEjecutarConsulta.CommandType = CommandType.Text
                cmdEjecutarConsulta.CommandText = p_strConsulta
                drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()
                dt.Load(drdResultadoConsulta)
            Catch
                Throw
            Finally
                If Not drdResultadoConsulta Is Nothing Then drdResultadoConsulta.Close()
                cmdEjecutarConsulta.Connection.Close()
            End Try
            Return dt

        End Function

        Shared Sub CreaMensajeSBO(p_strMensaje As String, p_strDocEntry As String, p_ocompany As SAPbobsCOM.Company, p_strNoOrden As String, blnDraft As Boolean, p_strRolCode As String, strIdSuc As String, p_oForm As SAPbouiCOM.Form, p_strLocalDT As String, p_bNewUpdate As SqlBoolean, pRol As RolesMensajeria, p_FinishOT As Boolean, Optional _aplicationSBO As SAPbouiCOM.Application = Nothing)
            'Crea mensaje en SAP para el bodeguero sobre creacion de un documento de traslado
            Try
                Dim oMsg As SAPbobsCOM.Messages
                Dim dtConsulta As SAPbouiCOM.DataTable
                Dim intResultado As Integer
                Dim strError As String
                Dim intError As Integer
                Dim intindiceUsuarios As Integer
                Dim query As String = String.Empty
                'Dim rolEncargadoProduccion As String = (CInt(RolesMensajeria.EncargadoProduccion)).ToString()
                'Dim rolEncargadoBodega As String = (CInt(RolesMensajeria.EncargadoRepuestos)).ToString()
                'Dim rolEncargadoCompras As String = (CInt(RolesMensajeria.EncargadoCompras)).ToString()
                'Dim rolEncargadoSOE As String = (CInt(RolesMensajeria.EncargadoSOE)).ToString()
                'Dim rolEncargadoSolEspec As String = (CInt(RolesMensajeria.EncargadoSolEspec)).ToString()

                query = "select l.U_EmpCode code, l.U_Usr_Name name, l.U_Usr_UsrName userId " & _
                  "from [@SCGD_CONF_MSJ] m with(nolock) " & _
                      "inner join [@SCGD_CONF_MSJLN] l with(nolock) on m.DocEntry=l.DocEntry " & _
                  "where m.U_IdRol = '{0}' and m.U_IdSuc = '{1}' "

                query = String.Format(query, p_strRolCode, strIdSuc)

                If String.IsNullOrEmpty(p_strLocalDT) Then
                    dtConsulta = p_oForm.DataSources.DataTables.Item("dtConsulta")
                Else
                    If ValidaExisteDataTable(p_oForm, p_strLocalDT) Then
                        dtConsulta = p_oForm.DataSources.DataTables.Item(p_strLocalDT)
                    Else
                        dtConsulta = p_oForm.DataSources.DataTables.Add(p_strLocalDT)
                    End If

                End If

                dtConsulta.ExecuteQuery(query)

                If dtConsulta.Rows.Count >= 1 Then
                    If Not String.IsNullOrEmpty(dtConsulta.GetValue("userId", 0).ToString) Then
                        Select Case p_strRolCode
                            Case RolesMensajeria.EncargadoProduccion
                            Case Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoProduccion)
                                'Crea el mensaje
                                If p_bNewUpdate Then
                                    oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                    oMsg.MessageText = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                                    oMsg.Subject = oMsg.MessageText 'p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                                Else
                                    If blnDraft Then
                                        oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                        oMsg.MessageText = String.Format(My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOTSAP, p_strDocEntry, p_strNoOrden)
                                        oMsg.Subject = oMsg.MessageText
                                    Else
                                        oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                        oMsg.MessageText = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                                        oMsg.Subject = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                                    End If
                                End If

                                For intindiceUsuarios = 0 To dtConsulta.Rows.Count - 1
                                    oMsg.Recipients.Add()
                                    oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
                                    oMsg.Recipients.UserCode = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
                                    oMsg.Recipients.NameTo = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
                                    oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
                                Next

                                'verifica que el documento creado sea un draft
                                If Not p_bNewUpdate Then
                                    If Not blnDraft Then
                                        oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.Traslado & "," & My.Resources.ResourceFrameWork.Referencia & ": " & CStr(p_strDocEntry), SAPbobsCOM.BoObjectTypes.oStockTransfer, CStr(p_strDocEntry))
                                    End If
                                End If

                                intResultado = oMsg.Add()
                                If (intResultado <> 0) Then
                                    p_ocompany.GetLastError(intError, strError)
                                    Throw New ExceptionsSBO(intError, strError)
                                End If
                            Case Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoRepuestos), Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoSuministros)
                                'Crea el mensaje
                                If blnDraft Then
                                    oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                    oMsg.MessageText = String.Format(My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOTSAP, p_strDocEntry, p_strNoOrden)
                                    oMsg.Subject = oMsg.MessageText
                                Else
                                    oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                    oMsg.MessageText = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                                    oMsg.Subject = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                                End If

                                For intindiceUsuarios = 0 To dtConsulta.Rows.Count - 1
                                    oMsg.Recipients.Add()
                                    oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
                                    oMsg.Recipients.UserCode = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
                                    oMsg.Recipients.NameTo = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
                                    oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
                                Next
                                'verifica que el documento creado sea un draft
                                If Not p_bNewUpdate Then
                                    If Not blnDraft Then
                                        oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.Traslado & "," & My.Resources.ResourceFrameWork.Referencia & ": " & p_strDocEntry, SAPbobsCOM.BoObjectTypes.oStockTransfer, p_strDocEntry)
                                    End If
                                End If

                                intResultado = oMsg.Add()
                                If (intResultado <> 0) Then
                                    p_ocompany.GetLastError(intError, strError)
                                    Throw New ExceptionsSBO(intError, strError)
                                End If
                            Case Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoSOE)
                                'Crea el mensaje
                                oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                oMsg.MessageText = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                                oMsg.Subject = oMsg.MessageText

                                For intindiceUsuarios = 0 To dtConsulta.Rows.Count - 1
                                    oMsg.Recipients.Add()
                                    oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
                                    oMsg.Recipients.UserCode = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
                                    oMsg.Recipients.NameTo = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
                                    oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
                                Next

                                intResultado = oMsg.Add()
                                If (intResultado <> 0) Then
                                    p_ocompany.GetLastError(intError, strError)
                                    'Throw New ExceptionsSBO(intError, strError)
                                End If
                            Case Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoCompras)
                                'Crea el mensaje
                                oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                oMsg.MessageText = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                                oMsg.Subject = oMsg.MessageText

                                For intindiceUsuarios = 0 To dtConsulta.Rows.Count - 1
                                    oMsg.Recipients.Add()
                                    oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
                                    oMsg.Recipients.UserCode = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
                                    oMsg.Recipients.NameTo = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
                                    oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
                                Next

                                intResultado = oMsg.Add()
                                If (intResultado <> 0) Then
                                    p_ocompany.GetLastError(intError, strError)
                                End If
                        End Select
                    Else
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If

            Catch ex As Exception
                Throw ex
            End Try

        End Sub


        ''' <summary>
        ''' Obtiene formato en BD y retorna un string con la fecha formateada
        ''' </summary>
        ''' <param name="p_dtFecha">Fecha obtenida de interfaz</param>
        ''' <param name="p_strNombreServidor">Servidor de BD</param>
        ''' <param name="p_strDBUser">Usuario de BD</param>
        ''' <param name="p_strDBPass">Password de BD</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function RetornaFechaFormatoDB(ByVal p_dtFecha As Date, ByVal p_strNombreServidor As String, ByVal p_strDBUser As String, ByVal p_strDBPass As String, Optional ByVal p_usaHora As Boolean = False) As String

            'Obtengo la Formato de fecha y el separador de la configuracion global de la maquina
            Dim SeparadorFecha As String = String.Empty
            Dim SeparadorHora As String = String.Empty
            'fecha a retornar formateada
            Dim strFechaFormateada As String = String.Empty
            Dim FormatoServer As String = String.Empty
            Dim dtUserOptions As System.Data.DataTable

            'Const strConsultaFormatoSQL As String = "select dateformat from syslanguages where langid = (select value from master..sysconfigures where comment = 'default language')"
            Const strConsultaFormatoSQL As String = "dbcc useroptions"

            Dim strDia As String = String.Empty
            Dim strMes As String = String.Empty
            Dim strAno As String = String.Empty
            Dim strHora As String = String.Empty
            Dim strMinutos As String = String.Empty
            Dim strSeg As String = String.Empty

            Dim strConexion As String = String.Empty
            Dim oCompany As New SCGCommon.clsCompany()

            SeparadorFecha = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator
            SeparadorHora = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.TimeSeparator

            If Not String.IsNullOrEmpty(p_strNombreServidor) _
                And Not String.IsNullOrEmpty(SeparadorFecha) _
                And Not String.IsNullOrEmpty(p_dtFecha) Then

                strMes = String.Format("{0:D2}", p_dtFecha.Month)
                strDia = String.Format("{0:D2}", p_dtFecha.Day)
                strAno = p_dtFecha.Year.ToString()

                strHora = String.Format("{0:D2}", p_dtFecha.Hour)
                strMinutos = String.Format("{0:D2}", p_dtFecha.Minute)
                strSeg = String.Format("{0:D2}", p_dtFecha.Second)
                
                oCompany.Server = p_strNombreServidor
                oCompany.Company = COMPANY
                oCompany.DataBase = "master"
                oCompany.DBUser = p_strDBUser
                oCompany.UserName = p_strDBUser
                oCompany.DBPassword = p_strDBPass
                oCompany.Password = p_strDBPass

                strConexion = objDAConexion.CadenaConexionSBO(oCompany)
                dtUserOptions = EjecutarConsultaDataTable(strConsultaFormatoSQL, strConexion)
                FormatoServer = dtUserOptions.Rows.Item(2).Item(1).ToString()

                Select Case FormatoServer
                    Case "dmy"
                        strFechaFormateada = String.Format(strDia & "{0}" & strMes & "{0}" & strAno, SeparadorFecha)
                    Case "dym"
                        strFechaFormateada = String.Format(strDia & "{0}" & strAno & "{0}" & strMes, SeparadorFecha)
                    Case "mdy"
                        strFechaFormateada = String.Format(strMes & "{0}" & strDia & "{0}" & strAno, SeparadorFecha)
                    Case "myd"
                        strFechaFormateada = String.Format(strMes & "{0}" & strAno & "{0}" & strDia, SeparadorFecha)
                    Case "ymd"
                        strFechaFormateada = String.Format(strAno & "{0}" & strMes & "{0}" & strDia, SeparadorFecha)
                    Case "ydm"
                        strFechaFormateada = String.Format(strAno & "{0}" & strDia & "{0}" & strMes, SeparadorFecha)
                End Select
                If p_usaHora = True Then
                    strFechaFormateada = String.Format(strFechaFormateada & " " & strHora & "{0}" & strMinutos & "{0}" & strSeg, SeparadorHora)
                End If

                'retorno fecha formateada
                Return strFechaFormateada
            End If

            Return Nothing
        End Function

        'Valida DT
        Shared Function ValidaExisteDataTable(ByRef p_form As SAPbouiCOM.Form, ByVal strDtName As String) As Boolean
            Dim ExisteDataTable As Boolean = False
            If p_form.DataSources.DataTables.Count > 0 Then
                For i As Integer = 0 To p_form.DataSources.DataTables.Count - 1
                    If p_form.DataSources.DataTables.Item(i).UniqueID = strDtName Then
                        ExisteDataTable = True
                    End If
                Next
            End If
            Return ExisteDataTable
        End Function

    End Class

End Namespace
