Imports DMSOneFramework

Namespace SCGDataAccess
    Public Class CostosxOrdenDataAdapter

        Implements IDataAdapter, IDisposable

#Region "Declaraciones"

        'TODO Agregar nombres de procedimeintos almacenados para una determinada tabla
        Private Const mc_strSPIns As String = ""
        Private Const mc_strSPUpdCostosxOrden As String = "SCGTA_SP_UpdCostosxOrden"
        Private Const mc_strSPDel As String = ""
        Private Const mc_strSPSelCostosxOrden As String = "SCGTA_SP_SelCostosxOrden"
        Private Const mc_strEstaLlaveExiste As String = ""

        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strCostoSuministro As String = "CostoSuministro"
        Private Const mc_strCostoRepuesto As String = "CostoRepuesto"
        Private Const mc_strCostoSuministroPintura As String = "CostoSuministroPintura"

        'Declaracion de objetos de acceso a datos
        Private m_cnn As SqlClient.SqlConnection
        Private m_adp As SqlClient.SqlDataAdapter

        Private m_strConexion As String

        Private mc_strArroba As String = "@"

        Dim objDAConexion As DAConexion

#End Region

#Region "Inicializar AnalisisDataAdapter"

        Public Sub New(ByVal conexion As String)
            Try
                m_strConexion = conexion
                m_cnn = New SqlClient.SqlConnection(conexion)
                m_adp = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        'Public Sub New()
        '    objDAConexion = New DAConexion
        '    m_cnnSCGTaller = objDAConexion.ObtieneConexion

        '    m_adpColabora = New SqlClient.SqlDataAdapter
        'End Sub
#End Region

#Region "Implementaciones"
        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Overloads Function Fill(ByRef dataset As CostosxOrdenDataset, _
                                        ByVal NoOrden As String) As Integer
            Try

                Call m_cnn.Open()

                m_adp.SelectCommand = CrearCmdSel()
                m_adp.SelectCommand.Connection = m_cnn

                m_adp.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden

                Call m_adp.Fill(dataset.SCGTA_SP_SelCostosxOrden)

            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)
                Return 1
            Finally
                Call m_cnn.Close()
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

        Public Function Update(ByVal dataSet As CostosxOrdenDataset) As Integer

            Dim m_trn As SqlClient.SqlTransaction = Nothing

            Try
                Call m_cnn.Open()


                m_trn = m_cnn.BeginTransaction
                m_adp.UpdateCommand = CrearCmdUpd()
                m_adp.InsertCommand = CrearCmdIns()
                m_adp.UpdateCommand.Connection = m_cnn
                m_adp.InsertCommand.Connection = m_cnn
                m_adp.UpdateCommand.Transaction = m_trn
                m_adp.InsertCommand.Transaction = m_trn

                Call m_adp.Update(dataSet.SCGTA_SP_SelCostosxOrden)


            Catch ex As SqlClient.SqlException

            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)

                If Not m_trn Is Nothing Then
                    Call m_trn.Rollback()
                End If

            Finally
                If Not m_trn Is Nothing Then
                    Call m_trn.Commit()
                    Call m_trn.Dispose()
                    m_trn = Nothing
                End If
                Call m_cnn.Close()
            End Try
        End Function


        Public Sub Dispose() Implements System.IDisposable.Dispose
            Try
                If m_cnn.State = ConnectionState.Open Then
                    Call m_cnn.Close()
                    Call m_cnn.Dispose()
                    m_cnn = Nothing
                End If

                If Not m_adp Is Nothing Then
                    Call m_adp.Dispose()
                    m_adp = Nothing
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region

#Region "Commands "
        Private Function CrearCmdIns() As SqlClient.SqlCommand

            Dim cmdIns As SqlClient.SqlCommand

            Try

                cmdIns = New SqlClient.SqlCommand(mc_strSPIns)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    'TODO agregar campos para el comando de insercion

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            Finally
            End Try

        End Function

        Private Function CrearCmdDel() As SqlClient.SqlCommand

            Dim cmdDel As SqlClient.SqlCommand

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
            Dim param As SqlClient.SqlParameter

            Try

                cmdUpd = New SqlClient.SqlCommand(mc_strSPUpdCostosxOrden)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters

                    param = .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    param.SourceVersion = DataRowVersion.Original
                    .Add(mc_strArroba & mc_strCostoSuministro, SqlDbType.Decimal, 9, mc_strCostoSuministro)
                    .Add(mc_strArroba & mc_strCostoSuministroPintura, SqlDbType.Decimal, 9, mc_strCostoSuministroPintura)
                    .Add(mc_strArroba & mc_strCostoRepuesto, SqlDbType.Decimal, 9, mc_strCostoRepuesto)

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
                cmdSel = New SqlClient.SqlCommand(mc_strSPSelCostosxOrden)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                End With

                Return cmdSel
            Catch ex As Exception
                Throw ex
            End Try

        End Function
#End Region


    End Class

End Namespace
