Namespace SCGDataAccess
    Public Class ModelosDataAdapter
        Implements IDataAdapter


#Region "Declaraciones"

        Private Const mc_intNoModelo As String = "CodModelo"
        Private Const mc_strCodEstilo As String = "CodEstilo"
        Private Const mc_intNoMarca As String = "CodMarca"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        Private m_adpModelo As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDModelo As String = "SCGTA_SP_UpdModelosMarca"
        Private Const mc_strSCGTA_SP_SELModelo As String = "SCGTA_SP_SELModelo"
        Private Const mc_strSCGTA_SP_INSModelo As String = "SCGTA_SP_InsModelosMarca"
        Private Const mc_strSCGTA_SP_DelModelo As String = "SCGTA_SP_DelModelosMarca"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

#End Region


#Region "Inicializa ModelosDataAdapter"
        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpModelo = New SqlClient.SqlDataAdapter
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


        Public Function CargaModelosdeVehiculo(ByRef datareader As SqlClient.SqlDataReader, ByVal strCodEstilo As String) As Boolean

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpModelo.SelectCommand = CrearSelectCommand()
                m_adpModelo.SelectCommand.Parameters.Item(mc_strArroba & mc_strCodEstilo).Value = strCodEstilo
                m_adpModelo.SelectCommand.Connection = m_cnnSCGTaller

                datareader = m_adpModelo.SelectCommand.ExecuteReader(CommandBehavior.CloseConnection)


                Return True
            Catch ex As Exception
                Throw ex
            Finally
                'Agregado
                'Call m_cnnSCGTaller.Close()
            End Try

        End Function


        Public Overloads Function Fill(ByVal dataSet As ModelosDataset, ByVal intNoMarca As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpModelo.SelectCommand = CrearSelectCommand()

                m_adpModelo.SelectCommand.Connection = m_cnnSCGTaller

                m_adpModelo.SelectCommand.Parameters(mc_strArroba & mc_intNoMarca).Value = intNoMarca

                Call m_adpModelo.Fill(dataSet.SCGTA_TB_Modelo)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function


        Public Overloads Function Update(ByVal dataSet As ModelosDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpModelo.InsertCommand = CreateInsertCommand()
                m_adpModelo.InsertCommand.Connection = m_cnnSCGTaller

                m_adpModelo.UpdateCommand = CrearUpdateCommand()
                m_adpModelo.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpModelo.Update(dataSet.SCGTA_TB_Modelo)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Function Delete(ByVal dataset As ModelosDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpModelo.UpdateCommand = CrearDeleteCommand()
                m_adpModelo.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpModelo.Update(dataset.SCGTA_TB_Modelo)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

#End Region


#Region "Creación de comandos"


        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELModelo)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strCodEstilo, SqlDbType.NVarChar, 8, mc_strCodEstilo)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDModelo)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoMarca, SqlDbType.Int, 9, mc_intNoMarca)

                    .Add(mc_strArroba & mc_intNoModelo, SqlDbType.Int, 9, mc_intNoModelo)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelModelo)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoMarca, SqlDbType.Int, 9, mc_intNoMarca)
                    .Add(mc_strArroba & mc_intNoModelo, SqlDbType.Int, 4, mc_intNoModelo)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSModelo)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 9, mc_strDescripcion)

                    .Add(mc_strArroba & mc_intNoMarca, SqlDbType.Int, 100, mc_intNoMarca)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


#End Region



    End Class
End Namespace