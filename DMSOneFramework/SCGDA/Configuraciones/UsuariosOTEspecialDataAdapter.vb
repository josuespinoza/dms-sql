Option Strict On
Option Explicit On

Namespace SCGDataAccess
    Public Class UsuariosOTEspecialDataAdapter

        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strID As String = "ID"
        Private Const mc_strIDConfOTEspecial As String = "IDConfOTEspecial"
        Private Const mc_strUsuario As String = "Usuario"

        Private m_adpConfUsuariosOTEspecial As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_SelConfUsuariosOTEspecial As String = "SCGTA_SP_SelConfUsuariosOTEspecial"
        Private Const mc_strSCGTA_SP_InsConfUsuariosOTEspecial As String = "SCGTA_SP_InsConfUsuariosOTEspecial"
        Private Const mc_strSCGTA_SP_DelConfUsuariosOTEspecial As String = "SCGTA_SP_DelConfUsuariosOTEspecial"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Dim objDAConexion As DAConexion


#End Region

#Region "Inicializa TipoOrdenDataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpConfUsuariosOTEspecial = New SqlClient.SqlDataAdapter

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

            End Get
        End Property


#End Region

#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As UsuariosOTEspecialDataset, ByVal p_intIDConf As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpConfUsuariosOTEspecial.SelectCommand = CrearSelectCommand()

                m_adpConfUsuariosOTEspecial.SelectCommand.Connection = m_cnnSCGTaller

                m_adpConfUsuariosOTEspecial.SelectCommand.Parameters.Item(mc_strArroba & mc_strIDConfOTEspecial).Value = p_intIDConf

                Call m_adpConfUsuariosOTEspecial.Fill(dataSet.SCGTA_TB_ConfUsuariosConfOTEspecial)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Fill(ByRef datareader As SqlClient.SqlDataReader, ByVal p_intIDConf As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpConfUsuariosOTEspecial.SelectCommand = CrearSelectCommand()

                m_adpConfUsuariosOTEspecial.SelectCommand.Connection = m_cnnSCGTaller

                m_adpConfUsuariosOTEspecial.SelectCommand.Parameters.Item(mc_strArroba & mc_strIDConfOTEspecial).Value = p_intIDConf

                datareader = m_adpConfUsuariosOTEspecial.SelectCommand.ExecuteReader()

            Catch ex As Exception

                Throw ex

            Finally
                'Agregado
                'Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As UsuariosOTEspecialDataset, _
                                         ByRef cnConeccion As SqlClient.SqlConnection, _
                                         ByRef tnTransaccion As SqlClient.SqlTransaction) As Integer

            Try

                'If m_cnnSCGTaller.State = ConnectionState.Closed Then
                '    If m_cnnSCGTaller.ConnectionString = "" Then
                '        m_cnnSCGTaller.ConnectionString = strConexionADO
                '    End If
                '    Call m_cnnSCGTaller.Open()
                'End If

                m_adpConfUsuariosOTEspecial.InsertCommand = CreateInsertCommand()
                m_adpConfUsuariosOTEspecial.InsertCommand.Connection = cnConeccion
                m_adpConfUsuariosOTEspecial.InsertCommand.Transaction = tnTransaccion

                m_adpConfUsuariosOTEspecial.UpdateCommand = CrearDeleteCommand()
                m_adpConfUsuariosOTEspecial.UpdateCommand.Connection = cnConeccion
                m_adpConfUsuariosOTEspecial.UpdateCommand.Transaction = tnTransaccion

                m_adpConfUsuariosOTEspecial.DeleteCommand = CrearDeleteCommand()
                m_adpConfUsuariosOTEspecial.DeleteCommand.Connection = cnConeccion
                m_adpConfUsuariosOTEspecial.DeleteCommand.Transaction = tnTransaccion

                Call m_adpConfUsuariosOTEspecial.Update(dataSet.SCGTA_TB_ConfUsuariosConfOTEspecial)

            Catch ex As Exception

                Throw ex

            Finally
                'Agregado
                'Call m_cnnSCGTaller.Close()

            End Try

        End Function

#End Region

#Region "Creación de comandos"

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelConfUsuariosOTEspecial)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strIDConfOTEspecial, SqlDbType.Int, 4)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelConfUsuariosOTEspecial)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsConfUsuariosOTEspecial)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    '.Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID)
                    .Add(mc_strArroba & mc_strIDConfOTEspecial, SqlDbType.Int, 4, mc_strIDConfOTEspecial)
                    .Add(mc_strArroba & mc_strUsuario, SqlDbType.VarChar, 100, mc_strUsuario)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function
#End Region

    End Class

End Namespace
