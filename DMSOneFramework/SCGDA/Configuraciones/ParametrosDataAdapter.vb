Imports System.Data.SqlClient
Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion

Namespace SCGDataAccess
    Public Class ParametrosDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

#Region "Constantes"

        Private Const mc_strSCGTA_SP_SELParam As String = "SCGTA_SP_SELParametrosGenerales"
        Private Const mc_strSCGTA_SP_UPDParam As String = "SCGTA_SP_UPDParametrosGenerales"
        Private Const mc_strCompania As String = "Compania"
        Private Const mc_strBaseDatos As String = "SCGDatabase"
        Private Const mc_strNombreParam As String = "NombreParametro"
        Private Const mc_strValor As String = "Valor"
        Private Const mc_strArroba As String = "@"

#End Region

#Region "Variables"

        Private m_adpParametros As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Dim objDAConexion As DAConexion

#End Region

#End Region

#Region "Inicializacion"

        Public Sub New()

            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpParametros = New SqlClient.SqlDataAdapter

        End Sub

#End Region

#Region "Implementaciones"
        Public Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

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

        Public Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function
#End Region

#Region "Implementaciones SCG"

        Public Sub CargarParametros(ByRef p_dstParam As ParametrosDataset, ByVal p_strCompania As String, ByVal p_strBaseDatos As String)

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpParametros.SelectCommand = CrearSelectCommand()
                m_adpParametros.SelectCommand.Connection = m_cnnSCGTaller

                With m_adpParametros.SelectCommand
                    .Parameters(mc_strArroba & mc_strCompania).Value = p_strCompania
                    .Parameters(mc_strArroba & mc_strBaseDatos).Value = p_strBaseDatos
                End With

                m_adpParametros.Fill(p_dstParam.SCGTA_SP_SELParametrosGenerales)

            Catch ex As Exception
                Throw ex
            Finally
                If Not m_cnnSCGTaller Is Nothing Then
                    m_cnnSCGTaller.Close()
                End If
            End Try
        End Sub

        Public Function ActualizarParametros(ByRef p_dstParam As ParametrosDataset, ByVal p_strCompania As String, ByVal p_strBaseDatos As String) As Integer

            Dim intResult As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpParametros.UpdateCommand = CrearUpdateCommand()
                m_adpParametros.UpdateCommand.Connection = m_cnnSCGTaller

                With m_adpParametros.UpdateCommand
                    .Parameters(mc_strArroba & mc_strCompania).Value = p_strCompania
                    .Parameters(mc_strArroba & mc_strBaseDatos).Value = p_strBaseDatos
                End With

                intResult = m_adpParametros.Update(p_dstParam.SCGTA_SP_SELParametrosGenerales)

                Return intResult

            Catch ex As Exception
                Throw ex
            Finally

                If Not m_cnnSCGTaller Is Nothing Then
                    m_cnnSCGTaller.Close()
                End If

            End Try
        End Function

#End Region

#Region "Comandos"

        Private Function CrearSelectCommand() As SqlClient.SqlCommand
            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELParam)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    .Add(mc_strArroba & mc_strCompania, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strBaseDatos, SqlDbType.VarChar, 50) 
                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmd As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDParam)

                cmd.CommandType = CommandType.StoredProcedure

                With cmd.Parameters
                    .Add(mc_strArroba & mc_strCompania, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strBaseDatos, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strNombreParam, SqlDbType.VarChar, 80, mc_strNombreParam)
                    .Add(mc_strArroba & mc_strValor, SqlDbType.VarChar, 200, mc_strValor)
                End With

                Return cmd

            Catch ex As Exception
                Throw ex
            End Try


        End Function

#End Region

    End Class
End Namespace