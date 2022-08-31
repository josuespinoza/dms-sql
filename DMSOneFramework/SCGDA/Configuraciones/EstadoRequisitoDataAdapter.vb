Namespace SCGDataAccess
    Public Class EstadoRequisitoDataAdapter

        Implements IDataAdapter


#Region "Declaraciones"

        Private Const mc_intCodEstadoReq As String = "CodEstadoReq"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        Private m_adp As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDEstadoReq As String = "SCGTA_SP_UPDEstadoReq"
        Private Const mc_strSCGTA_SP_SELEstadosReq As String = "SCGTA_SP_SELEstadoReq"
        Private Const mc_strSCGTA_SP_INSEstadoReq As String = "SCGTA_SP_INSEstadoReq"
        Private Const mc_strSCGTA_SP_DELEstadoReq As String = "SCGTA_SP_DELEstadoReq"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Dim objDAConexion As DAConexion

#End Region


#Region "Inicializa EstadoRequisitosDataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adp = New SqlClient.SqlDataAdapter
        End Sub

#End Region


#Region "Implementaciones"


#Region "Metodos IDataAdarpter"


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


        Public Overloads Function Fill(ByVal dataSet As EstadoRequisitoDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adp.SelectCommand = CrearSelectCommand()

                m_adp.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adp.Fill(dataSet.SCGTA_TB_EstadoReq)

            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Overloads Function Update(ByVal dataSet As EstadoRequisitoDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adp.InsertCommand = CreateInsertCommand()
                m_adp.InsertCommand.Connection = m_cnnSCGTaller

                m_adp.UpdateCommand = CrearUpdateCommand()
                m_adp.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adp.Update(dataSet.SCGTA_TB_EstadoReq)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function


        Public Function Delete(ByVal dataset As EstadoRequisitoDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adp.UpdateCommand = CrearDeleteCommand()
                m_adp.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adp.Update(dataset.SCGTA_TB_EstadoReq)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELEstadosReq)

                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)

            End Try

        End Function


        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDEstadoReq)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intCodEstadoReq, SqlDbType.Int, 9, mc_intCodEstadoReq)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)

            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DELEstadoReq)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intCodEstadoReq, SqlDbType.Int, 9, mc_intCodEstadoReq)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)

            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSEstadoReq)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)

            End Try

        End Function


#End Region



    End Class
End Namespace
