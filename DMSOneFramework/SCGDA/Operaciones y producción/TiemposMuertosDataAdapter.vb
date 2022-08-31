Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Namespace SCGDataAccess

    Public Class TiemposMuertosDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

#Region "Variables"

        Private m_adpTiemposMuertos As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Dim objDAConexion As DAConexion

#End Region

#Region "Constantes"

        Private Const mc_strArroba As String = "@"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strNoFase As String = "NoFase"

        Private Const mc_strSCGTA_SP_UPDTiemposMuertosIniciarOrden As String = "SCGTA_SP_UPDTiemposMuertosIniciarOrden"
        Private Const mc_strSCGTA_SP_UPDTiemposMuertosIniciarFase As String = "SCGTA_SP_UPDTiemposMuertosIniciarFase"
        Private Const mc_strSCGTA_SP_UPDTiemposMuertosRechazarFase As String = "SCGTA_SP_UPDTiemposMuertosRechazarFase"
#End Region

#End Region

#Region "Inicializacion"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpTiemposMuertos = New SqlClient.SqlDataAdapter
        End Sub

#End Region

#Region "Implementaciones"

        Public Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Function FillSchema(ByVal dataSet As System.Data.DataSet, ByVal schemaType As System.Data.SchemaType) As System.Data.DataTable() Implements System.Data.IDataAdapter.FillSchema

        End Function

        Public Function GetFillParameters() As System.Data.IDataParameter() Implements System.Data.IDataAdapter.GetFillParameters

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

        Public Sub UPDTiemposMuertosIniciarOrden(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer)

            Dim cmdActualizar As SqlClient.SqlCommand

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                cmdActualizar = CrearCommandUpdIniciarOrden()

                cmdActualizar.Connection = m_cnnSCGTaller

                With cmdActualizar
                    .Parameters(mc_strArroba & mc_strNoOrden).Value = p_strNoOrden
                    .Parameters(mc_strArroba & mc_strNoFase).Value = p_intNoFase
                End With

                cmdActualizar.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex
            Finally

                If Not IsNothing(m_cnnSCGTaller) Then
                    If m_cnnSCGTaller.State = ConnectionState.Open Then
                        m_cnnSCGTaller.Close()
                    End If
                End If
            End Try

        End Sub

        Public Sub UPDTiemposMuertosIniciarFase(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer)

            Dim cmdActualizar As SqlClient.SqlCommand

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                cmdActualizar = CrearCommandUpdIniciarFase()

                cmdActualizar.Connection = m_cnnSCGTaller

                With cmdActualizar
                    .Parameters(mc_strArroba & mc_strNoOrden).Value = p_strNoOrden
                    .Parameters(mc_strArroba & mc_strNoFase).Value = p_intNoFase
                End With

                cmdActualizar.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex
            Finally

                If Not IsNothing(m_cnnSCGTaller) Then
                    If m_cnnSCGTaller.State = ConnectionState.Open Then
                        m_cnnSCGTaller.Close()
                    End If
                End If
            End Try

        End Sub

        Public Sub UPDTiemposMuertosRechazarFase(ByVal p_strNoOrden As String)

            Dim cmdActualizar As SqlClient.SqlCommand

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                cmdActualizar = CrearCommandUpdRechazarFase()

                cmdActualizar.Connection = m_cnnSCGTaller

                With cmdActualizar
                    .Parameters(mc_strArroba & mc_strNoOrden).Value = p_strNoOrden
                End With

                cmdActualizar.ExecuteNonQuery()


            Catch ex As Exception
                Throw ex
            Finally

                If Not IsNothing(m_cnnSCGTaller) Then
                    If m_cnnSCGTaller.State = ConnectionState.Open Then
                        m_cnnSCGTaller.Close()
                    End If
                End If
            End Try

        End Sub

#End Region

#Region "Comandos"

        Private Function CrearCommandUpdIniciarOrden() As SqlClient.SqlCommand

            Try

                Dim cmd As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDTiemposMuertosIniciarOrden)

                cmd.CommandType = CommandType.StoredProcedure

                With cmd.Parameters
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strNoFase, SqlDbType.Int, 4)
                End With

                Return cmd

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearCommandUpdIniciarFase() As SqlClient.SqlCommand

            Try

                Dim cmd As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDTiemposMuertosIniciarFase)

                cmd.CommandType = CommandType.StoredProcedure

                With cmd.Parameters
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strNoFase, SqlDbType.Int, 4)
                End With

                Return cmd

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearCommandUpdRechazarFase() As SqlClient.SqlCommand

            Try

                Dim cmd As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDTiemposMuertosRechazarFase)

                cmd.CommandType = CommandType.StoredProcedure

                With cmd.Parameters
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
                End With

                Return cmd

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region


    End Class
End Namespace