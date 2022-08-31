Option Strict On
Option Explicit On 

Namespace SCGDataAccess

    Public Class CentroCostoDataAdapter

        Implements IDataAdapter


#Region "Implementaciones"


        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

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
                Return Nothing
            End Get
        End Property

        Public Overloads Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

#End Region


#Region "Declaraciones"

        Private Const mc_strCodCentroCosto As String = "CodCentroCosto"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        Private m_adpCentroCosto As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDCentroCosto As String = "SCGTA_SP_UPDCentrosCosto"
        Private Const mc_strSCGTA_SP_SELCentroCostos As String = "SCGTA_SP_SELCentrosCosto"
        Private Const mc_strSCGTA_SP_INSCentroCosto As String = "SCGTA_SP_INSCentrosCosto"
        Private Const mc_strSCGTA_SP_DelCentroCosto As String = "SCGTA_SP_DelCentrosCosto"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion


#End Region


#Region "Inicializa CentroCostoDataAdapter"


        Public Sub New()
            Call InicializaAgenciasDataAdapter(m_cnnSCGTaller)
        End Sub

        Private Sub InicializaAgenciasDataAdapter(ByRef cnnTaller As SqlClient.SqlConnection)

            Try

                'cnnTaller = New SqlClient.SqlConnection(conexion)
                objDAConexion = New DAConexion
                cnnTaller = objDAConexion.ObtieneConexion
                m_adpCentroCosto = New SqlClient.SqlDataAdapter


            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                'Call cnnTaller.Close()
            End Try
        End Sub


#End Region


#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As CentroCostoDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpCentroCosto.SelectCommand = CrearSelectCommand()
                m_adpCentroCosto.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adpCentroCosto.Fill(dataSet.SCGTA_TB_CentroCosto)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As CentroCostoDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpCentroCosto.InsertCommand = CreateInsertCommand()
                m_adpCentroCosto.InsertCommand.Connection = m_cnnSCGTaller

                m_adpCentroCosto.UpdateCommand = CrearUpdateCommand()
                m_adpCentroCosto.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpCentroCosto.Update(dataSet.SCGTA_TB_CentroCosto)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Function Delete(ByVal dataset As CentroCostoDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpCentroCosto.UpdateCommand = CrearDeleteCommand()
                m_adpCentroCosto.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpCentroCosto.Update(dataset.SCGTA_TB_CentroCosto)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try


        End Function

        Public Sub CargaCentrosCostoByReader(ByRef p_drdCC As SqlClient.SqlDataReader)

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpCentroCosto.SelectCommand = CrearSelectCommand()
                m_adpCentroCosto.SelectCommand.Connection = m_cnnSCGTaller

                p_drdCC = m_adpCentroCosto.SelectCommand.ExecuteReader(CommandBehavior.CloseConnection)


            Catch ex As Exception
                Throw ex

            Finally
                'Agregado
                'Call m_cnnSCGTaller.Close()
            End Try

        End Sub

#End Region


#Region "Creacion de Comandos del DataAdapter"


        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELCentroCostos)
                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDCentroCosto)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strCodCentroCosto, SqlDbType.Int, 4, mc_strCodCentroCosto)
                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)
                    .Add("@NormaReparto", SqlDbType.NVarChar, 8, "NormaReparto")

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelCentroCosto)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strCodCentroCosto, SqlDbType.Int, 4, mc_strCodCentroCosto)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSCentroCosto)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)
                    .Add("@NormaReparto", SqlDbType.NVarChar, 8, "NormaReparto")

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try

        End Function


#End Region

    End Class

End Namespace