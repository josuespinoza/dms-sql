Namespace SCGDataAccess
    Public Class ReprocesosxOrdenDataAdapter

        Implements IDataAdapter, IDisposable

#Region "Declaraciones"

        'TODO Agregar nombres de procedimeintos almacenados para una determinada tabla
        Private Const mc_strSPInsReprocesosxOrden As String = "SCGTA_SP_InsReprocesosxOrden"
        Private Const mc_strSPUpd As String = ""
        Private Const mc_strSPDel As String = ""
        Private Const mc_strSPSelReprocesosxOrden As String = "SCGTA_SP_SelReprocesosxOrden"
        Private Const mc_strSPSelCantidadReprocesosxOrden As String = "SCGTA_SP_SelCantidadReprocesosxOrden"
        Private Const mc_strEstaLlaveExiste As String = ""

        'TODO Agregar nombres de columnas de la tabla
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strNoReproceso As String = "NoReproceso"
        Private Const mc_strNoReprocesoxOrden As String = "NoReprocesoxOrden"
        Private Const mc_strFecha As String = "Fecha"
        Private Const mc_strObservacion As String = "Observacion"
        Private Const mc_strNoColaborador As String = "NoColaborador"
        Private Const mc_strTiempoManoObra As String = "TiempoManoObra"
        Private Const mc_strCosto As String = "Costo"
        Private Const mc_strFechaFin As String = "FechaFin"
        Private Const mc_strNoFase As String = "NoFase"

        Private Const mc_strCompania As String = "Compania"
        Private Const mc_strAplicacion As String = "Aplicacion"


        'Declaracion de objetos de acceso a datos
        Private m_cnnSCGTaller As SqlClient.SqlConnection
        Private m_adpProcesosxOrden As SqlClient.SqlDataAdapter

        Private m_strConexion As String

        Private mc_strArroba As String = "@"
        Dim objDAConexion As DAConexion
#End Region

#Region "Inicializar AnalisisDataAdapter"

        Public Sub New()
            Try
                objDAConexion = New DAConexion
                m_cnnSCGTaller = objDAConexion.ObtieneConexion
                m_adpProcesosxOrden = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                m_cnnSCGTaller.Close()
            End Try
        End Sub
#End Region

#Region "Procedimientos de consulta fuera de la interfase"

        ' ------------------------------------------------------------------------------
        ' Nombre: ConsultarCantidadReprocesosPorFase
        '
        ' Descripcion: retorna el numero reprocesos por una orden y una fase que son 
        '               pasadas por parametro.
        '              
        ' Parametros:  intFase: numero de fase.   NoOrden: nuemro de orden.
        '
        ' Logica Especial: 1. Abre el objeto conexion 
        '                  2. Crea el SelectCommand por medio de una funcion CrearCmdSelCantidadReprocesosxOrden
        '                  3. Asigna los parametros y ejecuta el Query que devuelve un entero.
        '                  4. Cierra conexion 
        ' 
        ' 25-04-06    Dorian Alvarado m.
        ' -------------------------------------------------------------------------------
        Public Function ConsultarCantidadReprocesosPorFase(ByVal intFase As Integer, _
                                            ByVal strNoOrden As String) As Integer

            Dim cmdCantidadReprocesosPorFase As SqlClient.SqlCommand

            Try

                'validacion antes de utilizar y abrir la conexion
                If Not m_cnnSCGTaller Is Nothing Then

                    If m_cnnSCGTaller.State = ConnectionState.Closed Then

                        Call m_cnnSCGTaller.Open()
                    End If
                End If

                'crea el objeto command para la seleccion
                cmdCantidadReprocesosPorFase = CrearCmdSelCantidadReprocesosxOrden()

                With cmdCantidadReprocesosPorFase

                    cmdCantidadReprocesosPorFase.Connection = m_cnnSCGTaller

                    .Parameters(mc_strArroba & mc_strNoOrden).Value = strNoOrden
                    .Parameters(mc_strArroba & mc_strNoFase).Value = intFase

                End With

                'ejecuta y retorna la consulta
                ConsultarCantidadReprocesosPorFase = cmdCantidadReprocesosPorFase.ExecuteScalar

            Catch ex As Exception
                Throw ex
                Return 1
            Finally

                'validacion antes de utilizar y cerrar la conexion
                If Not m_cnnSCGTaller Is Nothing Then

                    If m_cnnSCGTaller.State = ConnectionState.Open Then

                        Call m_cnnSCGTaller.Close()
                    End If
                End If

            End Try



        End Function
#End Region
#Region "Implementaciones"
        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Overloads Function Fill(ByRef dataset As ReprocesosxOrdenDataset, _
                                       ByVal NoOrden As String, _
                                       ByVal NoFase As Integer, _
                                       ByVal Compania As String, _
                                       ByVal Aplicacion As String) As Integer
            Try

                Call m_cnnSCGTaller.Open()

                m_adpProcesosxOrden.SelectCommand = CrearCmdSelReprocesosxOrden()
                m_adpProcesosxOrden.SelectCommand.Connection = m_cnnSCGTaller

                m_adpProcesosxOrden.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden
                m_adpProcesosxOrden.SelectCommand.Parameters(mc_strArroba & mc_strCompania).Value = Compania
                m_adpProcesosxOrden.SelectCommand.Parameters(mc_strArroba & mc_strAplicacion).Value = Aplicacion
                m_adpProcesosxOrden.SelectCommand.Parameters(mc_strArroba & mc_strNoFase).Value = NoFase

                Call m_adpProcesosxOrden.Fill(dataset.SCGTA_TB_ReprocesosxOrden)

            Catch ex As Exception
                Throw ex
                Return 1
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
        End Function

        Public Function FillSchema(ByVal dataSet As System.Data.DataSet, ByVal schemaType As System.Data.SchemaType) As System.Data.DataTable() Implements System.Data.IDataAdapter.FillSchema
            Throw New NotImplementedException()
        End Function

        Public Function GetFillParameters() As System.Data.IDataParameter() Implements System.Data.IDataAdapter.GetFillParameters
            Throw New NotImplementedException()
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

        Public Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

        Public Function Update(ByVal dataSet As ReprocesosxOrdenDataset) As Integer

            Dim m_trn As SqlClient.SqlTransaction = Nothing

            Try
                Call m_cnnSCGTaller.Open()

                m_trn = m_cnnSCGTaller.BeginTransaction
                m_adpProcesosxOrden.InsertCommand = CrearCmdInsProcesosxOrden()
                m_adpProcesosxOrden.InsertCommand.Connection = m_cnnSCGTaller
                m_adpProcesosxOrden.InsertCommand.Transaction = m_trn

                Call m_adpProcesosxOrden.Update(dataSet.SCGTA_TB_ReprocesosxOrden)


            Catch ex As SqlClient.SqlException
                Throw ex
            Catch ex As Exception
                Throw ex

                If Not m_trn Is Nothing Then
                    Call m_trn.Rollback()
                End If

            Finally
                If Not m_trn Is Nothing Then
                    Call m_trn.Commit()
                    Call m_trn.Dispose()
                    m_trn = Nothing
                End If
                Call m_cnnSCGTaller.Close()
            End Try
        End Function


        Public Sub Dispose() Implements System.IDisposable.Dispose
            Try
                If m_cnnSCGTaller.State = ConnectionState.Open Then
                    Call m_cnnSCGTaller.Close()
                    Call m_cnnSCGTaller.Dispose()
                    m_cnnSCGTaller = Nothing
                End If

                If Not m_adpProcesosxOrden Is Nothing Then
                    Call m_adpProcesosxOrden.Dispose()
                    m_adpProcesosxOrden = Nothing
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region

#Region "Commands "
        Private Function CrearCmdInsProcesosxOrden() As SqlClient.SqlCommand

            Dim cmdIns As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try

                cmdIns = New SqlClient.SqlCommand(mc_strSPInsReprocesosxOrden)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    .Add(mc_strArroba & mc_strNoReproceso, SqlDbType.Decimal, 5, mc_strNoReproceso)

                    param = .Add(mc_strArroba & mc_strNoReprocesoxOrden, SqlDbType.Decimal, 9, mc_strNoReprocesoxOrden)
                    param.Direction = ParameterDirection.Output

                    .Add(mc_strArroba & mc_strFecha, SqlDbType.SmallDateTime, 4, mc_strFecha)
                    .Add(mc_strArroba & mc_strObservacion, SqlDbType.VarChar, 500, mc_strObservacion)
                    .Add(mc_strArroba & mc_strNoColaborador, SqlDbType.VarChar, 15, mc_strNoColaborador)
                    .Add(mc_strArroba & mc_strTiempoManoObra, SqlDbType.Int, 4, mc_strTiempoManoObra)
                    .Add(mc_strArroba & mc_strCosto, SqlDbType.SmallMoney, 5, mc_strCosto)
                    .Add(mc_strArroba & mc_strFechaFin, SqlDbType.SmallDateTime, 4, mc_strFechaFin)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            Finally
            End Try

        End Function

        Private Function CrearCmdDel() As SqlClient.SqlCommand

            Dim cmdDel As SqlClient.SqlCommand
            'Dim param As SqlClient.SqlParameter

            Try

                cmdDel = New SqlClient.SqlCommand(mc_strSPDel)
                cmdDel.CommandType = CommandType.StoredProcedure

                With cmdDel.Parameters


                    'TODO agregar campos para el comando de borrado


                End With

                Return cmdDel
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearCmdUpd() As SqlClient.SqlCommand

            Dim cmdUpd As SqlClient.SqlCommand
            'Dim param As SqlClient.SqlParameter

            Try

                cmdUpd = New SqlClient.SqlCommand(mc_strSPUpd)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters


                    'TODO agregar campos para el comando de actualizacion


                End With

                Return cmdUpd
            Catch ex As Exception
                Throw ex
            Finally
            End Try

        End Function

        Private Function CrearCmdSelReprocesosxOrden() As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand
            'Dim param As SqlClient.SqlParameter

            Try
                cmdSel = New SqlClient.SqlCommand(mc_strSPSelReprocesosxOrden)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strNoFase, SqlDbType.Int, 4)
                    .Add(mc_strArroba & mc_strCompania, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strAplicacion, SqlDbType.VarChar, 50)

                End With

                Return cmdSel
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        ' ------------------------------------------------------------------------------
        ' Nombre: ConsultarCantidadReprocesosPorFase
        '
        ' Descripcion: cRea el objeto command basado utilizando el nombre de 
        '              el procedimiento almacenado que esta en una constante global. y
        '              nombres de las columnas 
        '
        ' Parametros:          
        ' Logica Especial:          
        ' 26-04-06    Dorian Alvarado m.
        ' -------------------------------------------------------------------------------
        Private Function CrearCmdSelCantidadReprocesosxOrden() As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand
            'Dim param As SqlClient.SqlParameter

            Try

                'crea el objeto comamnd y coloca las propiedades
                cmdSel = New SqlClient.SqlCommand(mc_strSPSelCantidadReprocesosxOrden)
                cmdSel.CommandType = CommandType.StoredProcedure

                'agrega  parametros Numero de orden y fase
                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strNoFase, SqlDbType.Int, 4)

                End With

                Return cmdSel
            Catch ex As Exception
                Throw ex
            End Try

        End Function
#End Region


    End Class
End Namespace

