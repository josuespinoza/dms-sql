Option Strict On
Option Explicit On 
Namespace SCGDataAccess
    Public Class MarcaDataAdapter
        Implements IDataAdapter
#Region "Declaraciones"

        Private Const mc_strCodMarca As String = "CodMarca"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        Private m_adpMarca As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDMarca As String = "SCGTA_SP_UpdMarcasAutos"
        Private Const mc_strSCGTA_SP_SELMarca As String = "SCGTA_SP_SELMarcasAutos"
        Private Const mc_strSCGTA_SP_INSMarca As String = "SCGTA_SP_InsMarcasAutos"
        Private Const mc_strSCGTA_SP_DelMarca As String = "SCGTA_SP_DelMarcasAutos"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion

#End Region


#Region "Inicializa MarcaDataAdapter"
        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpMarca = New SqlClient.SqlDataAdapter
        End Sub

#End Region


#Region "Implementaciones .Net Framework"

        Private Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Private Overloads Function Update(ByVal dataset As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

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


#End Region

#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As MarcaDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpMarca.SelectCommand = CrearSelectCommand()
                m_adpMarca.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adpMarca.Fill(dataSet.SCGTA_TB_Marca)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function CargaMarcasdeVehiculo(ByRef datareader As SqlClient.SqlDataReader) As Boolean

            'Dim cmdMarcasdeVehiculo As SqlClient.SqlCommand

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpMarca.SelectCommand = CrearSelectCommand()
                m_adpMarca.SelectCommand.Connection = m_cnnSCGTaller
                datareader = m_adpMarca.SelectCommand.ExecuteReader(CommandBehavior.CloseConnection)


                Return True
            Catch ex As Exception
                Throw ex
            Finally
                'Agregado
                'Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As MarcaDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpMarca.InsertCommand = CreateInsertCommand()
                m_adpMarca.InsertCommand.Connection = m_cnnSCGTaller

                m_adpMarca.UpdateCommand = CrearUpdateCommand()
                m_adpMarca.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpMarca.Update(dataSet.SCGTA_TB_Marca)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Function Delete(ByVal dataset As MarcaDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpMarca.UpdateCommand = CrearDeleteCommand()
                m_adpMarca.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpMarca.Update(dataset.SCGTA_TB_Marca)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try


        End Function


#End Region

#Region "Creacion de Comandos del DataAdapter"

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELMarca)
                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDMarca)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.Int, 4, mc_strCodMarca)
                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelMarca)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.Int, 4, mc_strCodMarca)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSMarca)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try

        End Function



#End Region

    End Class
End Namespace