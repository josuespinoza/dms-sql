Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Namespace SCGDataAccess
    Public Class AgendaDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"


        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_strID As String = "ID"
        Private Const mc_strDescripcion As String = "Agenda"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"
        Private Const mc_strCargaAgenda As String = "CargaAgenda"
        Private Const mc_strIntervaloCitas As String = "IntervaloCitas"
        Private Const mc_strAbreviatura As String = "Abreviatura"
        Private Const mc_strCodTecnico As String = "CodTecnico"
        Private Const mc_strCodAsesor As String = "CodAsesor"
        Private Const mc_strRazonCita As String = "RazonCita"
        Private Const mc_strArticuloCita As String = "ArticuloCita"


        'Declaracion de las constantes con el nombre de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_SELAgenda As String = "SCGTA_SP_SelAgenda"
        Private Const mc_strSCGTA_SP_INSAgenda As String = "SCGTA_SP_InsAgenda"
        Private Const mc_strSCGTA_SP_UpdAgenda As String = "SCGTA_SP_UpdAgenda"


        Private m_adpAgenda As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Dim objDAConexion As DAConexion


#End Region

#Region "Inicializa Configuracion"

        Public Sub New()

            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion
            m_adpAgenda = New SqlClient.SqlDataAdapter

        End Sub

        Public Sub New(ByVal conexion As String)
            Try

                m_cnnSCGTaller = New SqlClient.SqlConnection(conexion)
                m_adpAgenda = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub


#End Region

#Region "Implementaciones"

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


        Public Overloads Function Update(ByVal dataSet As AgendaDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpAgenda.InsertCommand = CreateInsertCommand()
                m_adpAgenda.InsertCommand.Connection = m_cnnSCGTaller

                m_adpAgenda.UpdateCommand = CrearUpdateCommand()
                m_adpAgenda.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpAgenda.Update(dataSet.SCGTA_TB_Agendas)

            Catch ex As Exception

                MsgBox(ex.Message)
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As AgendaDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpAgenda.SelectCommand = CrearSelectCommand()


                m_adpAgenda.SelectCommand.Parameters.Item(mc_strArroba + "CargaAgenda").Value = True

                m_adpAgenda.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpAgenda.Fill(dataSet.SCGTA_TB_Agendas)

            Catch ex As Exception

                MsgBox(ex.Message)

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Fill(ByRef datareader As SqlClient.SqlDataReader, Optional ByVal p_intEstado As Integer = -1) As Integer

            Try

                Dim cmdCommand As SqlClient.SqlCommand
                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()

                End If

                cmdCommand = CrearSelectCommand()
                If p_intEstado <> -1 Then
                    cmdCommand.Parameters.Item(mc_strArroba + mc_strEstadoLogico).Value = p_intEstado
                End If
                cmdCommand.Connection = m_cnnSCGTaller

                datareader = cmdCommand.ExecuteReader

            Catch ex As Exception

                Throw ex

            Finally
                'Agregado
                'Call m_cnnSCGTaller.Close()

            End Try

        End Function

#End Region

#Region "Creación de comandos"

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdAgenda)


                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.NVarChar, 50, mc_strDescripcion)

                    .Add(mc_strArroba & mc_strEstadoLogico, SqlDbType.Bit, 1, mc_strEstadoLogico)

                    .Add(mc_strArroba & mc_strIntervaloCitas, SqlDbType.Int, 4, mc_strIntervaloCitas)

                    .Add(mc_strArroba & mc_strAbreviatura, SqlDbType.NVarChar, 3, mc_strAbreviatura)

                    .Add(mc_strArroba & mc_strCodTecnico, SqlDbType.NVarChar, 3, mc_strCodTecnico)

                    .Add(mc_strArroba & mc_strCodAsesor, SqlDbType.NVarChar, 3, mc_strCodAsesor)

                    .Add(mc_strArroba & mc_strRazonCita, SqlDbType.NVarChar, 3, mc_strRazonCita)

                    .Add(mc_strArroba & mc_strArticuloCita, SqlDbType.NVarChar, 20, mc_strArticuloCita)

                End With

                Return cmdIns

            Catch ex As Exception
                MsgBox(ex.Message)
                Return Nothing
            End Try

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSAgenda)

            cmdIns.CommandType = CommandType.StoredProcedure

            With cmdIns.Parameters

                .Add(mc_strArroba & mc_strDescripcion, SqlDbType.NVarChar, 100, mc_strDescripcion)

                .Add(mc_strArroba & mc_strIntervaloCitas, SqlDbType.Int, 4, mc_strIntervaloCitas)

                .Add(mc_strArroba & mc_strAbreviatura, SqlDbType.NVarChar, 3, mc_strAbreviatura)

                .Add(mc_strArroba & mc_strCodTecnico, SqlDbType.Int, 3, mc_strCodTecnico)

                .Add(mc_strArroba & mc_strCodAsesor, SqlDbType.Int, 3, mc_strCodAsesor)

                .Add(mc_strArroba & mc_strRazonCita, SqlDbType.Int, 3, mc_strRazonCita)

                .Add(mc_strArroba & mc_strArticuloCita, SqlDbType.NVarChar, 20, mc_strArticuloCita)

            End With

            Return cmdIns

        End Function

        Private Function CrearSelectCommand(Optional ByVal p_blnToReader As Boolean = False) As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand


            cmdSel = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELAgenda)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID)

                .Add(mc_strArroba & mc_strEstadoLogico, SqlDbType.Bit, 1, mc_strEstadoLogico)

                .Add(mc_strArroba & mc_strCargaAgenda, SqlDbType.Bit, 1, mc_strCargaAgenda)

            End With

            Return cmdSel

            'Catch ex As Exception
            '    MsgBox(ex.Message)
            'End Try


        End Function

#End Region

    End Class
End Namespace
