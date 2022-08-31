Imports DMSOneFramework.SCGDataAccess.DAConexion
Namespace SCGDataAccess
    Public Class DestinatariosDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"
        Private m_adpDestinatarios As SqlClient.SqlDataAdapter
        Private m_cnnSCGTaller As SqlClient.SqlConnection
        Private objDAConexion As DAConexion

        'Parametros de los procedimientos
        Private Const mc_strArroba As String = "@"
        Private Const mc_intNoRegistro As String = "NoRegistro"
        Private Const mc_intDestinatarioID As String = "DestinatarioID"
        Private Const mc_blnLeido As String = "Mensaje_leido"
        Private Const mc_strCompania As String = "Compania"
        Private Const mc_strAplicacion As String = "Aplicacion"

        'Procedimientos
        Private Const mc_strSCGTA_SP_INSDestinatario As String = "SCGTA_SP_INSDestinatario"
        Private Const mc_strSCGTA_SP_SELDestinatario As String = "SCGTA_SP_SELDestinatario"
#End Region

#Region "Inicializacion"

        Public Sub New()

            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion
            m_adpDestinatarios = New SqlClient.SqlDataAdapter

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
                Throw New NotImplementedException()
            End Get
        End Property

        Public Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function
#End Region

#Region "Implementaciones SCG"
        Public Overloads Function Actualizar(ByVal dataSet As DestinatariosDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpDestinatarios.InsertCommand = CrearInsertCommand()
                m_adpDestinatarios.InsertCommand.Connection = m_cnnSCGTaller

                'm_adpDestinatarios.UpdateCommand = CrearUpdateCommand()
                'm_adpDestinatarios.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpDestinatarios.Update(dataSet.SCGTA_TB_DestinaXRegistro)

            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Sub SeleccionarDestinatarios(ByVal dataSet As DestinatariosDataset, ByVal noRegistro As Long _
                                           , ByVal compania As String, ByVal aplicacion As String)

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpDestinatarios.SelectCommand = CrearSelectCommand()
                m_adpDestinatarios.SelectCommand.Parameters(mc_strArroba & mc_intNoRegistro).Value = noRegistro
                m_adpDestinatarios.SelectCommand.Parameters(mc_strArroba & mc_strCompania).Value = compania
                m_adpDestinatarios.SelectCommand.Parameters(mc_strArroba & mc_strAplicacion).Value = aplicacion

                m_adpDestinatarios.SelectCommand.Connection = m_cnnSCGTaller

                m_adpDestinatarios.Fill(dataSet.SCGTA_TB_DestinaXRegistro)

            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Sub

#Region "Comandos"
        Private Function CrearInsertCommand() As SqlClient.SqlCommand
            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSDestinatario)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoRegistro, SqlDbType.BigInt, 8, mc_intNoRegistro)

                    .Add(mc_strArroba & mc_intDestinatarioID, SqlDbType.Int, 4, mc_intDestinatarioID)

                    .Add(mc_strArroba & mc_blnLeido, SqlDbType.Bit, 1, mc_blnLeido)


                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


'        Private Function CrearUpdateCommand() As SqlClient.SqlCommand
'
'        End Function

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELDestinatario)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    .Add(mc_strArroba & mc_intNoRegistro, SqlDbType.BigInt, 8, mc_intNoRegistro)
                    .Add(mc_strArroba & mc_strCompania, SqlDbType.VarChar, 50, mc_strCompania)
                    .Add(mc_strArroba & mc_strAplicacion, SqlDbType.VarChar, 50, mc_strAplicacion)
                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function
#End Region

#End Region

    End Class
End Namespace
