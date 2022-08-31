Option Strict On
Option Explicit On 
Namespace SCGDataAccess


Public Class ProveedoresDataAdapter
        Implements IDataAdapter



#Region "Declaraciones"

        Private Const mc_strCodAgencia As String = "NoProveedor"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        Private m_adpProveedores As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDProveedores As String = "SCGTA_SP_UPDProveedoresGrua"
        Private Const mc_strSCGTA_SP_SELProveedores As String = "SCGTA_SP_SELProveedoresGrua"
        Private Const mc_strSCGTA_SP_INSProveedores As String = "SCGTA_SP_INSProveedoresGrua"
        Private Const mc_strSCGTA_SP_DelProveedores As String = "SCGTA_SP_DelProveedoresGrua"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion

#End Region


#Region "Inicializa proveedoresDataAdapter"
        Public Sub New()
            Call InicializaProveedoresDataAdapter(m_cnnSCGTaller)
        End Sub

        Private Sub InicializaProveedoresDataAdapter(ByRef cnnTaller As SqlClient.SqlConnection)

            Try

                objDAConexion = New DAConexion
                cnnTaller = objDAConexion.ObtieneConexion
                m_adpProveedores = New SqlClient.SqlDataAdapter


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

        Public Overloads Function Fill(ByVal dataSet As ProveedoresDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpProveedores.SelectCommand = CrearSelectCommand()
                m_adpProveedores.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adpProveedores.Fill(dataSet.SCGTA_TB_ProveedoresGrua)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As ProveedoresDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpProveedores.InsertCommand = CreateInsertCommand()
                m_adpProveedores.InsertCommand.Connection = m_cnnSCGTaller

                m_adpProveedores.UpdateCommand = CrearUpdateCommand()
                m_adpProveedores.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpProveedores.Update(dataSet.SCGTA_TB_ProveedoresGrua)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Function Delete(ByVal dataset As ProveedoresDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpProveedores.UpdateCommand = CrearDeleteCommand()
                m_adpProveedores.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpProveedores.Update(dataset.SCGTA_TB_ProveedoresGrua)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELProveedores)
                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDProveedores)
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

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelProveedores)

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
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSProveedores)
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