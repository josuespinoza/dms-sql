Namespace SCGDataAccess

    Public Class SuspensionesxOrdenDataAdapter

        Implements IDataAdapter, IDisposable

#Region "Declaraciones"

        'TODO Agregar nombres de procedimeintos almacenados para una determinada tabla
        Private Const mc_strSPInsSuspensionesxOrden As String = "SCGTA_SP_InsSuspensionesxOrden"
        Private Const mc_strSPUpd As String = ""
        Private Const mc_strSPDel As String = ""
        Private Const mc_strSPSelSuspensionesxOrden As String = "SCGTA_SP_SelSuspensionesxOrden"
        Private Const mc_strEstaLlaveExiste As String = ""

        'TODO Agregar nombres de columnas de la tabla
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strNoSuspension As String = "NoSuspension"
        Private Const mc_strNoSuspensionxOrden As String = "NoSuspensionxOrden"
        Private Const mc_strFecha As String = "Fecha"
        Private Const mc_strFechaFin As String = "FechaFin"
        Private Const mc_strRazon As String = "Razon"
        Private Const mc_strNoColaborador As String = "NoColaborador"
        Private Const mc_strIndividual As String = "Individual"
        Private Const mc_strCodRazon As String = "CodRazon"
        Private Const mc_strNoFase As String = "NoFase"

        Private Const mc_strCompania As String = "Compania"
        Private Const mc_strAplicacion As String = "Aplicacion"

        'Declaracion de objetos de acceso a datos
        Private m_cnnSCGTaller As SqlClient.SqlConnection
        Private m_adpSuspensionesxOrden As SqlClient.SqlDataAdapter

        Private m_strConexion As String

        Private Const mc_strArroba As String = "@"

        Dim objDAConexion As DAConexion
#End Region

#Region "Inicializar AnalisisDataAdapter"

        Public Sub New()
            
                Try
                    objDAConexion = New DAConexion
                    m_cnnSCGTaller = objDAConexion.ObtieneConexion
                    m_adpSuspensionesxOrden = New SqlClient.SqlDataAdapter
                Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
        End Sub
#End Region

#Region "Implementaciones"
        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Overloads Function Fill(ByRef dataset As SuspensionesxOrdenDataset, _
                                       ByVal NoOrden As String, _
                                       ByVal Nofase As Integer, _
                                       ByVal Compania As String, _
                                       ByVal Aplicacion As String) As Integer
            Try

                Call m_cnnSCGTaller.Open()

                m_adpSuspensionesxOrden.SelectCommand = CrearCmdSel()
                m_adpSuspensionesxOrden.SelectCommand.Connection = m_cnnSCGTaller

                m_adpSuspensionesxOrden.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden
                'm_adpSuspensionesxOrden.SelectCommand.Parameters(mc_strArroba & mc_strCompania).Value = Compania
                'm_adpSuspensionesxOrden.SelectCommand.Parameters(mc_strArroba & mc_strAplicacion).Value = Aplicacion
                m_adpSuspensionesxOrden.SelectCommand.Parameters(mc_strArroba & mc_strNofase).Value = CStr(Nofase)

                Call m_adpSuspensionesxOrden.Fill(dataset.SCGTA_TB_SuspensionesxOrden)

            Catch ex As Exception
                Throw ex
                Return 1
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
        End Function

        Public Function FillSchema(ByVal dataSet As System.Data.DataSet, ByVal schemaType As System.Data.SchemaType) As System.Data.DataTable() Implements System.Data.IDataAdapter.FillSchema

        End Function

        Public Function GetFillParameters() As System.Data.IDataParameter() Implements System.Data.IDataAdapter.GetFillParameters

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

            End Get
        End Property

        Public Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

        Public Function Update(ByVal dataSet As SuspensionesxOrdenDataset) As Integer

            Dim m_trn As SqlClient.SqlTransaction = Nothing
            Dim intCodSuspensionResult As Integer

            Try
                Call m_cnnSCGTaller.Open()

                m_trn = m_cnnSCGTaller.BeginTransaction

                With m_adpSuspensionesxOrden
                    .InsertCommand = CrearCmdIns()
                    .InsertCommand.Connection = m_cnnSCGTaller
                    .InsertCommand.Transaction = m_trn

                    Call .Update(dataSet.SCGTA_TB_SuspensionesxOrden)

                    intCodSuspensionResult = .InsertCommand.Parameters(mc_strArroba & mc_strNoSuspensionxOrden).Value

                End With

                Return intCodSuspensionResult

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

                If Not m_adpSuspensionesxOrden Is Nothing Then
                    Call m_adpSuspensionesxOrden.Dispose()
                    m_adpSuspensionesxOrden = Nothing
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region

#Region "Commands "
        Private Function CrearCmdIns() As SqlClient.SqlCommand

            Dim cmdIns As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try

                cmdIns = New SqlClient.SqlCommand(mc_strSPInsSuspensionesxOrden)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    param = .Add(mc_strArroba & mc_strNoSuspensionxOrden, SqlDbType.Int, 4, mc_strNoSuspensionxOrden)
                    param.Direction = ParameterDirection.Output

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    .Add(mc_strArroba & mc_strNoFase, SqlDbType.Int, 4, mc_strNoFase)
                    .Add(mc_strArroba & mc_strRazon, SqlDbType.VarChar, 500, mc_strRazon)
                    .Add(mc_strArroba & mc_strFecha, SqlDbType.DateTime, 8, mc_strFecha)
                    .Add(mc_strArroba & mc_strIndividual, SqlDbType.Int, 4, mc_strIndividual)
                    .Add(mc_strArroba & mc_strCodRazon, SqlDbType.Int, 4, mc_strCodRazon)

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

        Private Function CrearCmdSel() As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand

            Try
                cmdSel = New SqlClient.SqlCommand(mc_strSPSelSuspensionesxOrden)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
                    '.Add(mc_strArroba & mc_strCompania, SqlDbType.VarChar, 50)
                    '.Add(mc_strArroba & mc_strAplicacion, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strNofase, SqlDbType.Int, 4)

                End With

                Return cmdSel
            Catch ex As Exception
                Throw ex
            End Try

        End Function
#End Region


    End Class
End Namespace

