Namespace SCGDataAccess

Public Class FaseProduccionDataAdapter

    Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_intNoCentroCosto As String = "CodCentroCosto"
        Private Const mc_intNoFase As String = "NoFase"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"
        Private Const mc_strUnidad As String = "Unidad"

        Private m_adpFases As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDFase As String = "SCGTA_SP_UpdFasesProduccion"
        Private Const mc_strSCGTA_SP_SELFase As String = "SCGTA_SP_SELFasesProduccion"
        Private Const mc_strSCGTA_SP_SELALLFase As String = "SCGTA_SP_SELALLFasesProduccion"
        Private Const mc_strSCGTA_SP_INSFase As String = "SCGTA_SP_InsFasesProduccion"
        Private Const mc_strSCGTA_SP_DelFase As String = "SCGTA_SP_DelFasesProduccion"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

#End Region


#Region "Inicializa FaseProduccionDataAdapter"
        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpFases = New SqlClient.SqlDataAdapter
        End Sub

#End Region


#Region "Implementaciones .Net Framework"

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
                Return Nothing
            End Get
        End Property
#End Region


#Region "Implementaciones SCG"


        Public Overloads Function Fill(ByVal dataSet As FaseProduccionDataset, ByVal intCentroCosto As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpFases.SelectCommand = CrearSelectCommand()

                m_adpFases.SelectCommand.Connection = m_cnnSCGTaller

                m_adpFases.SelectCommand.Parameters(mc_strArroba & mc_intNoCentroCosto).Value = intCentroCosto

                Call m_adpFases.Fill(dataSet.SCGTA_TB_FasesProduccion)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try


        End Function


        Public Overloads Function Fill(ByVal dataSet As FaseProduccionDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpFases.SelectCommand = CrearSelectCommand2()

                m_adpFases.SelectCommand.Connection = m_cnnSCGTaller


                Call m_adpFases.Fill(dataSet.SCGTA_TB_FasesProduccion)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Overloads Function Update(ByVal dataSet As FaseProduccionDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpFases.InsertCommand = CreateInsertCommand()
                m_adpFases.InsertCommand.Connection = m_cnnSCGTaller

                m_adpFases.UpdateCommand = CrearUpdateCommand()
                m_adpFases.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpFases.Update(dataSet.SCGTA_TB_FasesProduccion)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function


        Public Function Delete(ByVal dataset As FaseProduccionDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpFases.UpdateCommand = CrearDeleteCommand()
                m_adpFases.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpFases.Update(dataset.SCGTA_TB_FasesProduccion)

            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()

            End Try

        End Function

#End Region


#Region "Creación de comandos"


        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELFase)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_intNoCentroCosto, SqlDbType.Int, 4, mc_intNoCentroCosto)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearSelectCommand2() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELALLFase)

                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDFase)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 9, mc_intNoFase)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                    .Add(mc_strArroba & mc_strUnidad, SqlDbType.SmallInt, 2, mc_strUnidad)

                    .Add(mc_strArroba & mc_intNoCentroCosto, SqlDbType.SmallInt, 2, mc_intNoCentroCosto)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelFase)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 4, mc_intNoFase)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSFase)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoCentroCosto, SqlDbType.Int, 4, mc_intNoCentroCosto)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                    .Add(mc_strArroba & mc_strUnidad, SqlDbType.SmallInt, 2, mc_strUnidad)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


#End Region




End Class

End Namespace