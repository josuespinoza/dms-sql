Imports DMSOneFramework.SCGDataAccess.DAConexion

Namespace SCGDataAccess
    Public Class ActividadesRepuestosDataAdapter
        Implements IDataAdapter


#Region "Declaraciones"


        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_intNoSeccion As String = "NoSeccion"
        Private Const mc_intNoPiezaPrincipal As String = "NoPiezaPrincipal"
        Private Const mc_intNoFase As String = "NoFase"
        Private Const mc_intNoRepuesto As String = "NoRepuesto"
        Private Const mc_intNoActividad As String = "NoActividad"
        Private Const mc_intCantidad As String = "Cantidad"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_bitAdicional As String = "Adicional"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_intNoAdicional As String = "NoAdicional"
        Private Const mc_intCantidadPendiente As String = "CantidadPendiente"
        Private Const mc_dtFecha_Solicitud As String = "Fecha_Solicitud"
        Private Const mc_blnComprarRepuesto As String = "Comprar"
        Private Const mc_strComponente As String = "Componente"

        'Declaracion de las constantes con el nombre de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_UpdActRep As String = "SCGTA_SP_UpdActividadesRepuestos"
        Private Const mc_strSCGTA_SP_SELActRep As String = "SCGTA_SP_SelActividadesRepuestos"

        ''para seleccionar el ultimo numero de solicitud de respuestos adicionales
        'Private Const mc_strSCGTA_SP_SELUltimoNoAdicionalRep As String = "SCGTA_SP_SELUltimoNoAdicionalRepuestos"
        ''para seleccionar el ultimo numero de solicitud de actividades adicionales
        'Private Const mc_strSCGTA_SP_SELUltimoNoAdicionalAct As String = "SCGTA_SP_SELUltimoNoAdicionalActividades"

        'para seleccionar el ultimo numero de solicitud de adicionales
        Private Const mc_strSCGTA_SP_SELUltimoNoAdicional As String = "SCGTA_SP_SELUltimoNoAdicional"

        Private Const mc_strSCGTA_SP_InsRepuestoXOrden As String = "SCGTA_SP_INSRepuestosXOrden"
        Private Const mc_strSCGTA_SP_InsActividadesXOrden As String = "SCGTA_SP_INSActividadesXOrden"
        Private Const mc_strSCGTA_SP_DelActRep As String = "SCGTA_SP_DelActividadesRepuestos"

        Private m_adpActRep As SqlClient.SqlDataAdapter

        Public m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Dim objDAConexion As DAConexion
#End Region

#Region "Inicializa DataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpActRep = New SqlClient.SqlDataAdapter
        End Sub


#End Region

#Region "Implementaciones"

        Private Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Private Overloads Function Update(ByVal dataset As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

        Public Function FillSchema(ByVal dataSet As System.Data.DataSet, ByVal schemaType As System.Data.SchemaType) As System.Data.DataTable() Implements System.Data.IDataAdapter.FillSchema
            Return Nothing
        End Function

        Public Function GetFillParameters() As System.Data.IDataParameter() Implements System.Data.IDataAdapter.GetFillParameters
            Return Nothing
        End Function

        Public Property MissingMappingAction() As System.Data.MissingMappingAction Implements System.Data.IDataAdapter.MissingMappingAction

            Get

            End Get

            Set(ByVal Value As System.Data.MissingMappingAction)

            End Set
        End Property

        Public Property MissingSchemaAction() As System.Data.MissingSchemaAction Implements System.Data.IDataAdapter.MissingSchemaAction
            Get

            End Get
            Set(ByVal Value As System.Data.MissingSchemaAction)

            End Set
        End Property

        Public ReadOnly Property TableMappings() As System.Data.ITableMappingCollection Implements System.Data.IDataAdapter.TableMappings
            Get
                Throw New NotImplementedException()
            End Get
        End Property

#End Region

#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As RepuestosxOrdenDataset, ByVal NoSeccion As Integer, ByVal NoPiezaPrincipal As Integer, ByVal NoFase As Integer, _
                                       ByVal NoRepuesto As Integer, ByVal NoActividad As Integer, ByVal Cantidad As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpActRep.SelectCommand = CrearSelectCommand()

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado
                If NoSeccion = 0 Then
                    m_adpActRep.SelectCommand.Parameters(mc_strArroba & mc_intNoSeccion).Value = System.DBNull.Value
                Else
                    m_adpActRep.SelectCommand.Parameters(mc_strArroba & mc_intNoSeccion).Value = NoSeccion
                End If

                If NoPiezaPrincipal = 0 Then
                    m_adpActRep.SelectCommand.Parameters(mc_strArroba & mc_intNoPiezaPrincipal).Value = System.DBNull.Value
                Else
                    m_adpActRep.SelectCommand.Parameters(mc_strArroba & mc_intNoPiezaPrincipal).Value = NoPiezaPrincipal

                End If

                If NoFase = 0 Then
                    m_adpActRep.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = System.DBNull.Value

                Else
                    m_adpActRep.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = NoFase

                End If

                If NoRepuesto = 0 Then
                    m_adpActRep.SelectCommand.Parameters(mc_strArroba & mc_intNoRepuesto).Value = System.DBNull.Value

                Else
                    m_adpActRep.SelectCommand.Parameters(mc_strArroba & mc_intNoRepuesto).Value = NoRepuesto

                End If

                If NoActividad = 0 Then
                    m_adpActRep.SelectCommand.Parameters(mc_strArroba & mc_intNoActividad).Value = System.DBNull.Value

                Else
                    m_adpActRep.SelectCommand.Parameters(mc_strArroba & mc_intNoActividad).Value = NoActividad
                End If

                If Cantidad = 0 Then
                    m_adpActRep.SelectCommand.Parameters(mc_strArroba & mc_intCantidad).Value = System.DBNull.Value

                Else
                    m_adpActRep.SelectCommand.Parameters(mc_strArroba & mc_intCantidad).Value = Cantidad

                End If

                m_adpActRep.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpActRep.Fill(dataSet.SCGTA_TB_RepuestosxOrden)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        ' ------------------------------------------------------------------------------
        ' Nombre: InsertRepuestos
        '
        ' Descripcion: inserta los repuestos por una orden dada 
        '
        ' Parametros: dataSet. contienen los datarows con los repuestos
        '               blnSolicitudAdicional: Boolean. indica si la insercion es para
        '               solicitud de repuesto adicional o es creando la orden
        '
        ' Logica Especial: si es una solicitud adicional se realiza una consulta
        '                  previa para saber cual es el ultimo adicional para esta orden.
        '                en caso contrario se le coloca un cero a la columna de "NoAdicional".
        '
        ' Fecha:         18-04-06    
        ' Desarrollador: Dorian Alvarado m.       '
        '
        ' comentario: 
        ' -------------------------------------------------------------------------------
        Public Overloads Function InsertRepuestos(ByVal dataSet As DMSOneFramework.RepuestosxOrdenDataset, _
                                                  ByVal blnSolicitudAdicional As Boolean) As Integer

            Dim trnInsertarRepuestos As SqlClient.SqlTransaction =  Nothing


            Try
                'conexion
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If


                'Valida si existe por lo menos un data row
                If dataSet.SCGTA_TB_RepuestosxOrden.Rows.Count > 0 Then


                    With m_adpActRep

                        ''Verifica si es solicitud adicional 
                        'If blnSolicitudAdicional Then

                        '    'Crea select command para ultimo numero de actividad, pasa primer data row para 
                        '    'obtener numero de orden
                        '    .SelectCommand = CreateSelectCommandUltimoNoAdicionaldRepuestos(dataSet.SCGTA_TB_RepuestosxOrden.Rows(0))
                        '    .SelectCommand.Connection = m_cnnSCGTaller


                        '    '***MANEJO DE LA TRANSACCION
                        '    'Crea la transaccion para la seleccion del numero adicionlales
                        '    'y la posterior insercion de los repuestos
                        '    trnInsertarRepuestos = m_cnnSCGTaller.BeginTransaction
                        '    .SelectCommand.Transaction = trnInsertarRepuestos


                        '    'ejecuta la consulta del numero de actividad
                        '    IntNoAdicionalActual = .SelectCommand.ExecuteScalar

                        'Else

                        '    IntNoAdicionalActual = 0
                        'End If



                        'Crea el Insert command de acuerdo al numero de adicional proporcionado
                        .InsertCommand = CreateInsertCommandRepuestos(0)


                        'TRANSACCION CON INSERT COMMAND
                        'verifica que no se haya creado una transaccion antes de crear otra
                        If trnInsertarRepuestos Is Nothing Then

                            trnInsertarRepuestos = m_cnnSCGTaller.BeginTransaction

                        End If

                        .InsertCommand.Transaction = trnInsertarRepuestos


                        .InsertCommand.Connection = m_cnnSCGTaller

                        Call m_adpActRep.Update(dataSet.SCGTA_TB_RepuestosxOrden)

                        Call trnInsertarRepuestos.Commit()

                    End With

                End If

                Return 0

            Catch ex As Exception

                If Not trnInsertarRepuestos Is Nothing Then
                    Call trnInsertarRepuestos.Rollback()
                End If
                Throw ex
            Finally

                trnInsertarRepuestos = Nothing

                m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function InsertRepuestos(ByVal dataSet As DMSOneFramework.RepuestosxOrdenDataset, _
                                                  ByVal blnSolicitudAdicional As Boolean, ByVal p_intNoAdicional As Integer, _
                                                  ByRef p_intCantRep As Integer) As Integer

            Dim trnInsertarRepuestos As SqlClient.SqlTransaction =  Nothing

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If


                If dataSet.SCGTA_TB_RepuestosxOrden.Rows.Count > 0 Then


                    With m_adpActRep

                        .InsertCommand = CreateInsertCommandRepuestos(p_intNoAdicional)

                        If trnInsertarRepuestos Is Nothing Then

                            trnInsertarRepuestos = m_cnnSCGTaller.BeginTransaction
                        End If

                        .InsertCommand.Transaction = trnInsertarRepuestos

                        .InsertCommand.Connection = m_cnnSCGTaller

                        Call m_adpActRep.Update(dataSet.SCGTA_TB_RepuestosxOrden)

                        p_intCantRep = dataSet.SCGTA_TB_RepuestosxOrden.Rows.Count

                        Call trnInsertarRepuestos.Commit()

                    End With

                End If

            Catch ex As Exception

                Throw ex

                If Not trnInsertarRepuestos Is Nothing Then
                    Call trnInsertarRepuestos.Rollback()
                End If

            Finally

                trnInsertarRepuestos = Nothing

                m_cnnSCGTaller.Close()

            End Try

        End Function

        ' ------------------------------------------------------------------------------
        ' Nombre: InsertActividad
        '
        ' Descripcion: inserta los actividades por una orden dada 
        '
        ' Parametros: dataSet. contienen los datarows con los repuestos
        '               blnSolicitudAdicional: Boolean. indica si la insercion es para
        '               solicitud de repuesto adicional o es creando la orden
        '
        ' Logica Especial: si es una solicitud adicional se realiza una consulta
        '                  previa para saber cual es el ultimo adicional para esta orden.
        '                en caso contrario se le coloca un cero a la columna de "NoAdicional".
        '
        ' Fecha:         18-04-06    
        ' Desarrollador: Dorian Alvarado m.       '
        '
        ' comentario: 
        ' -------------------------------------------------------------------------------
        Public Overloads Function InsertActividad(ByVal dataSet As DMSOneFramework.ActividadesXFaseDataset, _
                                                   ByVal blnSolicitudAdicional As Boolean) As Integer


            'Dim IntNoAdicionalActual As Integer
            Dim trnInsertarActividades As SqlClient.SqlTransaction =  Nothing

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                'Valida si existe por lo menos un data row
                If dataSet.SCGTA_TB_ActividadesxOrden.Rows.Count > 0 Then





                    With m_adpActRep

                        'If blnSolicitudAdicional Then

                        '    'Crea select command para ultimo numero de actividad, pasa primer data row para 
                        '    'obtener numero de orden
                        '    .SelectCommand = CreateSelectCommandUltimoNoAdicionaldActividades(dataSet.SCGTA_TB_ActividadesxOrden.Rows(0))
                        '    .SelectCommand.Connection = m_cnnSCGTaller


                        '    '***MANEJO DE LA TRANSACCION
                        '    'Crea la transaccion para la seleccion del numero adicionlales
                        '    'y la posterior insercion de los actividades
                        '    trnInsertarActividades = m_cnnSCGTaller.BeginTransaction

                        '    .SelectCommand.Transaction = trnInsertarActividades

                        '    'ejecuta la consulta del numero de actividad
                        '    IntNoAdicionalActual = .SelectCommand.ExecuteScalar

                        'End If

                        .InsertCommand = CreateInsertCommandActividades(0)
                        .InsertCommand.Connection = m_cnnSCGTaller

                        'TRANSACCION CON INSERT COMMAND
                        'verifica que no se haya creado una transaccion antes de crear otra
                        If trnInsertarActividades Is Nothing Then

                            trnInsertarActividades = m_cnnSCGTaller.BeginTransaction
                        End If

                        .InsertCommand.Transaction = trnInsertarActividades


                        'Actualiza
                        Call m_adpActRep.Update(dataSet.SCGTA_TB_ActividadesxOrden)

                        'Confirma la transacccion
                        Call trnInsertarActividades.Commit()

                    End With
                End If

            Catch ex As Exception

                If Not trnInsertarActividades Is Nothing Then

                    Call trnInsertarActividades.Rollback()
                End If

                Throw ex

            Finally

                trnInsertarActividades = Nothing
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function InsertActividad(ByVal dataSet As DMSOneFramework.ActividadesXFaseDataset, _
                                                   ByVal blnSolicitudAdicional As Boolean, ByVal p_intNoAdicional As Integer) As Integer

            Dim trnInsertarActividades As SqlClient.SqlTransaction =  Nothing

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                If dataSet.SCGTA_TB_ActividadesxOrden.Rows.Count > 0 Then

                    With m_adpActRep

                        .InsertCommand = CreateInsertCommandActividades(p_intNoAdicional)
                        .InsertCommand.Connection = m_cnnSCGTaller

                        If trnInsertarActividades Is Nothing Then

                            trnInsertarActividades = m_cnnSCGTaller.BeginTransaction
                        End If

                        .InsertCommand.Transaction = trnInsertarActividades


                        Call m_adpActRep.Update(dataSet.SCGTA_TB_ActividadesxOrden)

                        Call trnInsertarActividades.Commit()

                    End With
                End If

            Catch ex As Exception

                If Not trnInsertarActividades Is Nothing Then

                    Call trnInsertarActividades.Rollback()
                End If

                Throw ex

            Finally

                trnInsertarActividades = Nothing
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Function DeleteRepuesto(ByVal dataset As DMSOneFramework.RepuestosxOrdenDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpActRep.UpdateCommand = CreateDeleteCommand()
                m_adpActRep.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpActRep.Update(dataset.SCGTA_TB_RepuestosxOrden)

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Sub GetNoAdicionalXOrden(ByVal p_strNoOrden As String, ByRef p_intNoAdicional As Integer, _
                                        ByRef p_dtFecha As DateTime)

            Dim IntNoAdicionalActual As Integer
            Dim cmdConsult As SqlClient.SqlCommand

            Dim objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConexionADO)

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                cmdConsult = CreateSelectCommandUltimoNoAdicional(p_strNoOrden)

                cmdConsult.Connection = m_cnnSCGTaller

                IntNoAdicionalActual = cmdConsult.ExecuteScalar

                p_intNoAdicional = IntNoAdicionalActual
                p_dtFecha = objUtilitarios.CargarFechaHoraServidor

            Catch ex As Exception

                Throw ex

            Finally

                If Not IsNothing(m_cnnSCGTaller) Then
                    If m_cnnSCGTaller.State = ConnectionState.Open Then
                        m_cnnSCGTaller.Close()
                    End If
                End If

            End Try
        End Sub

#End Region

#Region "Creación de comandos"


        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELActRep)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_intNoSeccion, SqlDbType.Int, 9, mc_intNoSeccion)

                    .Add(mc_strArroba & mc_intNoPiezaPrincipal, SqlDbType.Int, 9, mc_intNoPiezaPrincipal)

                    .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 9, mc_intNoFase)

                    .Add(mc_strArroba & mc_intNoRepuesto, SqlDbType.Int, 9, mc_intNoRepuesto)

                    .Add(mc_strArroba & mc_intNoActividad, SqlDbType.Int, 9, mc_intNoActividad)

                    .Add(mc_strArroba & mc_intCantidad, SqlDbType.Int, 9, mc_intCantidad)


                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        '-----------------------------------------------------------------------------------
        ' Nombre: CreateSelectCommandUltimoNoActividadRepuestos  
        '
        ' Descripcion: configurar un SqlCommand para que consulte el ultimo 
        '              noAdicional. Que sera asignado los repuestos por orden en la insercion
        '              A los parametros se le asignan de una vez los valores por medio de un dataRow
        '
        ' Parametros:  drwRepuesto tipo SCGTA_TB_RepuestosxOrdenRow
        '               contiene los valores que se le van a asignar a los parametros

        '           
        ' Dorian Alvarado Murillo 25-04-06
        '-----------------------------------------------------------------------------------
        'Private Function CreateSelectCommandUltimoNoAdicionaldRepuestos(ByVal drwRepuesto As TallerFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow) As SqlClient.SqlCommand

        '    Try
        '        'Utiliza la constante del procedimiento almacenado
        '        'para seleccionar el ultimo numero de solicitud de repuestos adicionales
        '        Dim cmdSelNoAdicional As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELUltimoNoAdicionalRep)

        '        cmdSelNoAdicional.CommandType = CommandType.StoredProcedure

        '        With cmdSelNoAdicional.Parameters

        '            'agrega parametros y valores de consulta
        '            .Add(mc_strArroba & mc_strNoOrden, _
        '            SqlDbType.VarChar, 50, mc_strNoOrden).Value = drwRepuesto.NoOrden

        '            .Add(mc_strArroba & mc_bitAdicional, _
        '            SqlDbType.Bit, 1, mc_bitAdicional).Value = drwRepuesto.Adicional

        '        End With

        '        Return cmdSelNoAdicional

        '    Catch ex As Exception
        '        Throw ex
        '    End Try



        'End Function

        Private Function CreateSelectCommandUltimoNoAdicional(ByVal p_strNoOrden As String) As SqlClient.SqlCommand

            Try
                Dim cmdSelNoAdicional As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELUltimoNoAdicional)

                cmdSelNoAdicional.CommandType = CommandType.StoredProcedure

                With cmdSelNoAdicional.Parameters
                    .Add(mc_strArroba & mc_strNoOrden, _
                    SqlDbType.VarChar, 50, mc_strNoOrden).Value = p_strNoOrden
                End With

                Return cmdSelNoAdicional

            Catch ex As Exception
                Throw ex
            End Try



        End Function

        '-----------------------------------------------------------------------------------
        ' Nombre: CreateSelectCommandUltimoNoAdicionaldActividades  
        '
        ' Descripcion: configurar un SqlCommand para que consulte el ultimo 
        '              noAdicional. Que sera asignado a las actividades por orden en la insercion
        '              A los parametros se le asignan de una vez los valores por medio de un dataRow
        '
        ' Parametros:  drwActividad tipo SCGTA_TB_ActividadesxOrdenRow
        '               contiene los valores que se le van a asignar a los parametros
        '           
        ' Dorian Alvarado Murillo 26-04-06
        '-----------------------------------------------------------------------------------
        'Private Function CreateSelectCommandUltimoNoAdicionaldActividades(ByVal drwActividad As _
        '                    TallerFramework.ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow) As SqlClient.SqlCommand

        '    Try

        '        'Asigna el nombre del procedimiento segun la constante
        '        'para seleccionar el ultimo numero de solicitud de repuestos adicionales
        '        Dim cmdSelNoAdicional As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELUltimoNoAdicionalAct)

        '        cmdSelNoAdicional.CommandType = CommandType.StoredProcedure

        '        With cmdSelNoAdicional.Parameters

        '            'agrega parametros y valores de consulta
        '            .Add(mc_strArroba & mc_strNoOrden, _
        '            SqlDbType.VarChar, 50, mc_strNoOrden).Value = drwActividad.NoOrden

        '            .Add(mc_strArroba & mc_bitAdicional, _
        '            SqlDbType.Bit, 1, mc_bitAdicional).Value = drwActividad.Adicional

        '        End With

        '        Return cmdSelNoAdicional

        '    Catch ex As Exception
        '        Throw ex
        '    End Try



        'End Function


        Private Function CreateInsertCommandRepuestos(ByVal IntNoAdicional As Integer) As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsRepuestoXOrden)
                cmdIns.CommandType = CommandType.StoredProcedure
                cmdIns.UpdatedRowSource = UpdateRowSource.Both

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                    .Add(mc_strArroba & mc_intNoSeccion, SqlDbType.Decimal, 9, mc_intNoSeccion)

                    .Add(mc_strArroba & mc_intNoPiezaPrincipal, SqlDbType.Decimal, 9, mc_intNoPiezaPrincipal)

                    .Add(mc_strArroba & mc_intNoRepuesto, SqlDbType.Int, 4, mc_intNoRepuesto)

                    .Add(mc_strArroba & mc_intCantidad, SqlDbType.Int, 4, mc_intCantidad)

                    .Add(mc_strArroba & mc_bitAdicional, SqlDbType.Bit, 1, mc_bitAdicional)

                    '.Add(mc_strArroba & mc_intCantidadPendiente, SqlDbType.Int, 4, mc_intCantidadPendiente)

                    .Add(mc_strArroba & mc_intCantidadPendiente, SqlDbType.Int, 4, mc_intCantidad)

                    'agrega un parametro y su valor. que fue consultado desde la base de datos
                    .Add(mc_strArroba & mc_intNoAdicional, _
                        SqlDbType.Int, 4).Value = IntNoAdicional

                    .Add(mc_strArroba & mc_dtFecha_Solicitud, SqlDbType.DateTime, 8, mc_dtFecha_Solicitud)

                    .Add(mc_strArroba & mc_blnComprarRepuesto, SqlDbType.Bit, 1, mc_blnComprarRepuesto)

                    'Agregado 05/07/06. Alejandra. La descripción del repuesto será agregada a la tabla
                    .Add(mc_strArroba & mc_strComponente, SqlDbType.VarChar, 100, mc_strComponente)

                End With


                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        '-----------------------------------------------------------------------------------
        ' Nombre: CreateInsertCommandActividades  
        '
        ' Descripcion: Configurar un SqlCommand para la insersion de una actividad adicional.
        '              Se agrego a este procedimiento un parametro de entrada.
        '              Al Sqlcommand de insercion se le agregaron los parametros:
        '               1. "NoAdicional" indica el solicitud de adicionales por actividad
        '               2. "Fecha_Solicitud" : igual para todo el conjunto de actividades
        '                   que se insertan en un bloque
        '
        '               3.NoRepuesto.
        '               4.NoPiezaPrincipal.
        '               5.NoSeccion.
        '               Los tres valores van a servir de referencia a la tabla repuestos
        '               para saber cuales repuestos estan ligados a una actividad.
        '
        '
        ' Parametros:  intNoAdicional: Entero. indica el numero de adicional 
        '               con que se insertaran un conjunto de actividades adicionales
        '              .nuevo parametro
        '
        ' Retorna: SqlClient.SqlCommand 
        '           
        ' Dorian Alvarado Murillo 26-04-06. 
        ' Comentario : Modificador del procedimiento. no estaba documentado anteriormente
        '-----------------------------------------------------------------------------------
        Private Function CreateInsertCommandActividades(ByVal intNoAdicional As Integer) As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsActividadesXOrden)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                    .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 4, mc_intNoFase)

                    .Add(mc_strArroba & mc_bitAdicional, SqlDbType.Bit, 1, mc_bitAdicional)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 200, mc_strDescripcion)

                    'se el agrega el valor que se trae por parametro. 
                    'Importante: no tiene columna origen.
                    .Add(mc_strArroba & mc_intNoAdicional, _
                           SqlDbType.Int).Value = intNoAdicional

                    'fecha de la solicitud
                    .Add(mc_strArroba & mc_dtFecha_Solicitud, SqlDbType.DateTime, 8, mc_dtFecha_Solicitud)

                    'Valores que referencian a la tabla repuestos
                    .Add(mc_strArroba & mc_intNoRepuesto, SqlDbType.Int, 4, mc_intNoRepuesto)
                    .Add(mc_strArroba & mc_intNoPiezaPrincipal, SqlDbType.Int, 4, mc_intNoPiezaPrincipal)
                    .Add(mc_strArroba & mc_intNoSeccion, SqlDbType.Int, 5, mc_intNoSeccion)


                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CreateUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdActRep)

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters
                End With

                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CreateDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelActRep)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters


                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


#End Region

    End Class

End Namespace