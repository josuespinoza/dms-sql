Option Strict On
Option Explicit On 

Namespace SCGDataAccess

    Public Class ProcesosDataAdapter

        Implements IDataAdapter

#Region "Implementaciones"

        Private Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

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

        Private Overloads Function Update(ByVal dataset As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

#End Region


#Region "Declaraciones"

        Private Const mc_strNoProceso As String = "NoProceso"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strNoGrupo As String = "NoGrupo"
        Private Const mc_strNuevo As String = "Nuevo"
        Private Const mc_strPintar As String = "Pintar"

        Private m_adpProcesos As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDProcesos As String = "SCGTA_SP_UPDProcesos"
        Private Const mc_strSCGTA_SP_SELProcesos As String = "SCGTA_SP_SELProcesosGrupo"
        Private Const mc_strSCGTA_SP_INSProcesos As String = "SCGTA_SP_INSProcesos"
        Private Const mc_strSCGTA_SP_DELProcesos As String = "SCGTA_SP_DelProcesos"
        Private Const mc_strSCGTA_SP_SELProcesosNewNoGrupo As String = "SCGTA_SP_SELProcesosNewNoGrupo"
        Private Const mc_strSCGTA_SP_SELProcesosAsoc As String = "SCGTA_SP_SELProcesosAsoc"
        Private Const mc_strSCGTA_SP_SELProcesosEspecific As String = "SCGTA_SP_SELProcesosEspecific"

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
                m_adpProcesos = New SqlClient.SqlDataAdapter


            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                'Call cnnTaller.Close()
            End Try
        End Sub

#End Region


#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As ProcesosGrupoDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpProcesos.SelectCommand = CrearSelectCommand()
                m_adpProcesos.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adpProcesos.Fill(dataSet.SCGTA_VW_ProcesosGroup)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As ProcesosGrupoDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpProcesos.InsertCommand = CreateInsertCommand()
                m_adpProcesos.InsertCommand.Connection = m_cnnSCGTaller

                m_adpProcesos.UpdateCommand = CrearUpdateCommand()
                m_adpProcesos.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpProcesos.Update(dataSet.SCGTA_VW_ProcesosGroup)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Function Delete(ByVal dataset As ProcesosGrupoDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If


                m_adpProcesos.DeleteCommand = CrearDeleteCommand()
                m_adpProcesos.DeleteCommand.Connection = m_cnnSCGTaller

                Call m_adpProcesos.Update(dataset.SCGTA_VW_ProcesosGroup)

            Catch ex As Exception

                Throw ex

            Finally
                Call m_cnnSCGTaller.Close()
            End Try


        End Function

        Public Function GetNewNoGrupo() As Integer
            Dim cmdProcesos As SqlClient.SqlCommand
            Dim intResult As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                cmdProcesos = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELProcesosNewNoGrupo, m_cnnSCGTaller)
                cmdProcesos.CommandType = CommandType.StoredProcedure

                intResult = CInt(cmdProcesos.ExecuteScalar)

                Return intResult

            Catch ex As Exception
                Throw ex
            Finally

                If Not IsNothing(m_cnnSCGTaller) Then
                    If m_cnnSCGTaller.State = ConnectionState.Open Then
                        m_cnnSCGTaller.Close()
                    End If
                End If

            End Try

        End Function

        Public Function GetBoolProcesoExiste(ByVal p_intNoGrupo As Integer) As Boolean
            Dim cmdProcesos As SqlClient.SqlCommand
            Dim intResult As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                cmdProcesos = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELProcesosAsoc, m_cnnSCGTaller)
                cmdProcesos.CommandType = CommandType.StoredProcedure

                cmdProcesos.Parameters.Add(mc_strArroba & mc_strNoGrupo, SqlDbType.Int, 4).Value = p_intNoGrupo

                intResult = CInt(cmdProcesos.ExecuteScalar)

                If intResult = 0 Then
                    Return False
                Else
                    Return True
                End If

            Catch ex As Exception
                Throw ex
            Finally

                If Not IsNothing(m_cnnSCGTaller) Then
                    If m_cnnSCGTaller.State = ConnectionState.Open Then
                        m_cnnSCGTaller.Close()
                    End If
                End If

            End Try

        End Function

        Public Function GetNoProcesoByNoGrupoNuevoPintar(ByVal p_intNoGrupo As Integer, ByVal p_blnNuevo As Boolean, ByVal p_blnPintar As Boolean) As Integer
            Dim cmdProcesos As SqlClient.SqlCommand
            Dim intResult As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                cmdProcesos = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELProcesosEspecific, m_cnnSCGTaller)
                cmdProcesos.CommandType = CommandType.StoredProcedure

                cmdProcesos.Parameters.Add(mc_strArroba & mc_strNoGrupo, SqlDbType.Int, 4).Value = p_intNoGrupo
                cmdProcesos.Parameters.Add(mc_strArroba & mc_strNuevo, SqlDbType.Int, 4).Value = p_blnNuevo
                cmdProcesos.Parameters.Add(mc_strArroba & mc_strPintar, SqlDbType.Int, 4).Value = p_blnPintar

                intResult = CInt(cmdProcesos.ExecuteScalar)

                Return intResult

            Catch ex As Exception
                Throw ex
            Finally

                If Not IsNothing(m_cnnSCGTaller) Then
                    If m_cnnSCGTaller.State = ConnectionState.Open Then
                        m_cnnSCGTaller.Close()
                    End If
                End If

            End Try

        End Function

#End Region


#Region "Creacion de Comandos del DataAdapter"

        Private Function CrearSelectCommand() As SqlClient.SqlCommand
            Try
                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELProcesos)
                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel
            Catch ex As Exception
                Throw ex
            End Try

            

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand
            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDProcesos)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoGrupo, SqlDbType.Int, 4, mc_strNoGrupo)
                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

            

        End Function

        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DELProcesos)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoGrupo, SqlDbType.Int, 4, mc_strNoGrupo)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

            

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSProcesos)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoGrupo, SqlDbType.Int, 4, mc_strNoGrupo)
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