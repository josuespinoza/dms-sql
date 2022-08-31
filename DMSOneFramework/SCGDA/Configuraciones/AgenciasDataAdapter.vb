Option Strict On
Option Explicit On 
Namespace SCGDataAccess
    Public Class AgenciasDataAdapter

        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strCodAgencia As String = "CodAgencia"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        Private m_adpAgencias As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDAgencias As String = "SCGTA_SP_UPDAgencias"
        Private Const mc_strSCGTA_SP_SELAgencias As String = "SCGTA_SP_SELAgencias"
        Private Const mc_strSCGTA_SP_INSAgencias As String = "SCGTA_SP_INSAgencias"
        Private Const mc_strSCGTA_SP_DelAgencia As String = "SCGTA_SP_DelAgencia"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion
        'Private mc_Conexion As String

#End Region


#Region "Inicializa AgenciasDataAdapter"

        'Public Sub New(ByVal gc_Conexion As String)
        '    Call InicializaAgenciasDataAdapter(m_cnnSCGTaller, gc_Conexion)
        'End Sub

        Public Sub New()
            Call InicializaAgenciasDataAdapter(m_cnnSCGTaller)
        End Sub

        Private Sub InicializaAgenciasDataAdapter(ByRef cnnTaller As SqlClient.SqlConnection)

            Try

                'cnnTaller = New SqlClient.SqlConnection(conexion)
                objDAConexion = New DAConexion
                cnnTaller = objDAConexion.ObtieneConexion
                m_adpAgencias = New SqlClient.SqlDataAdapter




            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                'Call cnnTaller.Close()
            End Try
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

        Public Overloads Function Fill(ByVal dataSet As AgenciasDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpAgencias.SelectCommand = CrearSelectCommand()
                m_adpAgencias.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adpAgencias.Fill(dataSet.SCGTA_TB_Agencias)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As AgenciasDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpAgencias.InsertCommand = CreateInsertCommand()
                m_adpAgencias.InsertCommand.Connection = m_cnnSCGTaller

                m_adpAgencias.UpdateCommand = CrearUpdateCommand()
                m_adpAgencias.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpAgencias.Update(dataSet.SCGTA_TB_Agencias)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Function Delete(ByVal dataset As AgenciasDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpAgencias.UpdateCommand = CrearDeleteCommand()
                m_adpAgencias.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpAgencias.Update(dataset.SCGTA_TB_Agencias)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELAgencias)
                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDAgencias)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strCodAgencia, SqlDbType.Int, 4, mc_strCodAgencia)
                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelAgencia)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strCodAgencia, SqlDbType.Int, 4, mc_strCodAgencia)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSAgencias)
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
