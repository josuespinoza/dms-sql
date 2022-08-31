Option Strict On
Option Explicit On 

Namespace SCGDataAccess

    Public Class ClaseDataAdapter

        Implements IDataAdapter

#Region "Implementaciones"

        Private Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

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

        Private Overloads Function Update(ByVal dataset As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

#End Region


#Region "Declaraciones"

        Private Const mc_strCodClase As String = "CodClase"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        Private m_adpClase As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDClase As String = "SCGTA_SP_UPDClase"
        Private Const mc_strSCGTA_SP_SELClases As String = "SCGTA_SP_SELClases"
        Private Const mc_strSCGTA_SP_INSClase As String = "SCGTA_SP_INSClase"
        Private Const mc_strSCGTA_SP_DelClase As String = "SCGTA_SP_DelClase"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion


#End Region


#Region "Inicializa ClaseDataAdapter"


        Public Sub New()
            Call InicializaClasesDataAdapter(m_cnnSCGTaller)
        End Sub

        Private Sub InicializaClasesDataAdapter(ByRef cnnTaller As SqlClient.SqlConnection)

            Try

                'cnnTaller = New SqlClient.SqlConnection(conexion)
                objDAConexion = New DAConexion
                cnnTaller = objDAConexion.ObtieneConexion
                m_adpClase = New SqlClient.SqlDataAdapter


            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                'Call cnnTaller.Close()
            End Try
        End Sub

#End Region


#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As ClasesVehiculoDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpClase.SelectCommand = CrearSelectCommand()
                m_adpClase.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adpClase.Fill(dataSet.SCGTA_TB_Clase)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As ClasesVehiculoDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpClase.InsertCommand = CreateInsertCommand()
                m_adpClase.InsertCommand.Connection = m_cnnSCGTaller

                m_adpClase.UpdateCommand = CrearUpdateCommand()
                m_adpClase.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpClase.Update(dataSet.SCGTA_TB_Clase)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Function Delete(ByVal dataset As ClasesVehiculoDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If


                m_adpClase.UpdateCommand = CrearDeleteCommand()
                m_adpClase.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpClase.Update(dataset.SCGTA_TB_Clase)

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


                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELClases)
                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDClase)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strCodClase, SqlDbType.Int, 4, mc_strCodClase)
                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelClase)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strCodClase, SqlDbType.Int, 4, mc_strCodClase)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSClase)
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