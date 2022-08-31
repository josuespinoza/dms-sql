Namespace SCGDataAccess


Public Class ListasxOrdenDataAdapter

        Implements IDataAdapter

#Region "Inicializa ListasxOrdenDataAdapter"

        Public Sub New()

            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion
            m_adpListasxOrden = New SqlClient.SqlDataAdapter

        End Sub


#End Region

#Region "Implementaciones"

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

#Region "Declaraciones"

        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_intNoFase As String = "NoFase"
        Private Const mc_intNoLista As String = "NoLista"
        Private Const mc_strObservaciones As String = "Observaciones"

        Private m_adpListasxOrden As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_InsListasxOrden As String = "SCGTA_SP_InsListasxOrden"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion

#End Region

        Public Overloads Function Fill(ByVal dataSet As FasesXOrdenDataset, ByVal strnoorden As String) As Integer



        End Function

        Public Overloads Function Update(ByVal dataSet As ListasxOrdenDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpListasxOrden.InsertCommand = CreateInsertCommand()
                m_adpListasxOrden.InsertCommand.Connection = m_cnnSCGTaller

                Call m_adpListasxOrden.Update(dataSet.SCGTA_TB_ListasxOrden)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Function Delete(ByVal dataset As FasesXOrdenDataset) As Integer


        End Function


#Region "Creación de comandos"


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsListasxOrden)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                    .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 4, mc_intNoFase)

                    .Add(mc_strArroba & mc_intNoLista, SqlDbType.Int, 4, mc_intNoLista)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
                Return Nothing
            End Try

        End Function


        Private Function CrearSelectCommand() As SqlClient.SqlCommand
            Return Nothing
        End Function


        Private Function CrearUpdateCommand() As SqlClient.SqlCommand
            Return Nothing
        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand
            Return Nothing
        End Function


#End Region


End Class
End Namespace