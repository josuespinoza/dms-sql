Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Namespace SCGDataAccess
    Public Class CitasDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_strNoCita As String = "NoCita"
        Private Const mc_strIDCita As String = "IDCita"
        Private Const mc_strIDAgenda As String = "IDAgenda"
        Private Const mc_strNoCotizacion As String = "NoCotizacion"
        Private Const mc_strNoConsecutivo As String = "NoConsecutivo"
        Private Const mc_strPlaca As String = "Placa"
        Private Const mc_strFechayHora As String = "FechayHora"
        Private Const mc_strFechayHoraEnHorario As String = "FechayHoraEnHorario"
        Private Const mc_strIDRazon As String = "IDRazon"
        Private Const mc_strObservaciones As String = "Observaciones"
        Private Const mc_strCardCode As String = "CardCode"
        Private Const mc_strIDVehiculo As String = "IDVehiculo"
        Private Const mc_strNoVehiculo As String = "NoVehiculo"
        Private Const mc_strNoSerie As String = "NoSerie"
        Private Const mc_strCreadaPor As String = "CreadaPor"
        Private Const mc_strempId As String = "empId"

        Private Const mc_strCodModelo As String = "CodModelo"
        Private Const mc_strCodMarca As String = "CodMarca"
        Private Const mc_strCodEstilo As String = "CodEstilo"
        Private Const mc_strFechaIni As String = "FechaIni"
        Private Const mc_strFechaFin As String = "FechaFin"


        'Declaracion de las constantes con el nombre de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_SELCitas As String = "SCGTA_SP_SELCita"
        Private Const mc_strSCGTA_SP_INSCitas As String = "SCGTA_SP_INSCita"
        Private Const mc_strSCGTA_SP_UpdCitas As String = "SCGTA_SP_UPDCita"
        Private Const mc_strSCGTA_SP_DelCitas As String = "SCGTA_SP_DELCita"

        'Private Const mc_strSCGTA_SP_SelCitasCorreo As String = "SCGTA_SP_SelCitaCorreos"


        Private m_adpCita As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Dim objDAConexion As DAConexion

#End Region

#Region "Inicializa ClientesDataAdapter"

        Public Sub New()

            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion
            m_adpCita = New SqlClient.SqlDataAdapter

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

        Public Overloads Function Update(ByVal dataSet As CitasDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpCita.InsertCommand = CreateInsertCommand()
                m_adpCita.InsertCommand.Connection = m_cnnSCGTaller

                m_adpCita.UpdateCommand = CrearUpdateCommand()
                m_adpCita.UpdateCommand.Connection = m_cnnSCGTaller

                m_adpCita.DeleteCommand = CrearDeleteCommand()
                m_adpCita.DeleteCommand.Connection = m_cnnSCGTaller

                Update = m_adpCita.Update(dataSet.SCGTA_TB_Citas)

            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Update(ByRef dataSet As CitasDataset, _
                                         ByRef cnConection As SqlClient.SqlConnection, _
                                         ByRef tnTransaction As SqlClient.SqlTransaction, _
                                         Optional ByVal p_blnIniciarTransaccion As Boolean = False) As Integer

            Try

                If p_blnIniciarTransaccion Then
                    If m_cnnSCGTaller.State = ConnectionState.Closed Then
                        If m_cnnSCGTaller.ConnectionString = "" Then
                            m_cnnSCGTaller.ConnectionString = strConexionADO
                        End If
                        m_cnnSCGTaller.Open()
                    End If
                    cnConection = m_cnnSCGTaller
                    tnTransaction = cnConection.BeginTransaction()
                End If

                m_adpCita.InsertCommand = CreateInsertCommand()
                m_adpCita.InsertCommand.Connection = cnConection
                m_adpCita.InsertCommand.Transaction = tnTransaction

                m_adpCita.UpdateCommand = CrearUpdateCommand()
                m_adpCita.UpdateCommand.Connection = cnConection
                m_adpCita.UpdateCommand.Transaction = tnTransaction

                Update = m_adpCita.Update(dataSet.SCGTA_TB_Citas)

            Catch ex As Exception

                Throw ex

            Finally

                'm_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As CitasDataset, ByVal p_dtFechaIni As Date, _
                                        ByVal p_dtFechaFin As Date, Optional ByVal p_strNoCita As String = "", _
                                        Optional ByVal p_strCardCode As String = "", Optional ByVal p_intIDAgenda As Integer = 0, _
                                        Optional ByVal p_strCodMarca As String = "", Optional ByVal p_strCodEstilo As String = "", _
                                        Optional ByVal p_strCodModelo As String = "", Optional ByVal p_strPlaca As String = "", _
                                        Optional ByVal p_strNoVehiculo As String = "", Optional ByVal p_intIDCita As Integer = 0, _
                                        Optional ByVal p_intIDCotizacion As Integer = 0) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpCita.SelectCommand = CrearSelectCommand()
                m_adpCita.SelectCommand.Connection = m_cnnSCGTaller

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado
                With m_adpCita.SelectCommand.Parameters
                    If p_dtFechaIni <> Nothing Then
                        .Item(mc_strArroba + mc_strFechaIni).Value = p_dtFechaIni
                    Else
                        .Item(mc_strArroba + mc_strFechaIni).Value = DBNull.Value
                    End If

                    If p_dtFechaFin <> Nothing Then
                        .Item(mc_strArroba + mc_strFechaFin).Value = p_dtFechaFin
                    Else
                        .Item(mc_strArroba + mc_strFechaFin).Value = DBNull.Value
                    End If

                    If p_strNoCita <> "" Then
                        .Item(mc_strArroba + mc_strNoCita).Value = p_strNoCita
                    End If

                    If p_strCardCode <> "" Then
                        .Item(mc_strArroba + mc_strCardCode).Value = p_strCardCode
                    End If

                    If p_strCodMarca <> "" Then
                        .Item(mc_strArroba + mc_strCodMarca).Value = p_strCodMarca
                    End If

                    If p_strCodModelo <> "" Then
                        .Item(mc_strArroba + mc_strCodModelo).Value = p_strCodModelo
                    End If

                    If p_strNoVehiculo <> "" Then
                        .Item(mc_strArroba + mc_strNoVehiculo).Value = p_strNoVehiculo
                    End If

                    If p_strCodEstilo <> "" Then
                        .Item(mc_strArroba + mc_strCodEstilo).Value = p_strCodEstilo
                    End If

                    If p_strPlaca <> "" Then
                        .Item(mc_strArroba + mc_strPlaca).Value = p_strPlaca
                    End If

                    If p_intIDCita <> 0 Then
                        .Item(mc_strArroba + mc_strIDCita).Value = p_intIDCita
                    End If

                    If p_intIDAgenda <> 0 Then
                        .Item(mc_strArroba + mc_strIDAgenda).Value = p_intIDAgenda
                    End If

                    If p_intIDCotizacion <> 0 Then
                        .Item(mc_strArroba + mc_strNoCotizacion).Value = p_intIDCotizacion
                    End If

                End With

                Call m_adpCita.Fill(dataSet.SCGTA_TB_Citas)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Function Delete(ByVal dataset As CitasDataset) As Integer

            Try

                m_adpCita.DeleteCommand = CrearDeleteCommand()

                m_adpCita.DeleteCommand.Connection = m_cnnSCGTaller

                Call m_adpCita.Update(dataset.SCGTA_TB_Citas)

            Catch ex As Exception

                Throw ex

            Finally
                m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As CitasDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpCita.SelectCommand = CrearSelectCommand()

                m_adpCita.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpCita.Fill(dataSet.SCGTA_TB_Citas)

                Return dataSet.SCGTA_TB_Citas.Rows.Count

            Catch ex As Exception

                Throw ex
                Return -1

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

#End Region

#Region "Creación de comandos"



        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdCitas)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strIDCita, SqlDbType.Int, 4, mc_strIDCita)

                    .Add(mc_strArroba & mc_strObservaciones, SqlDbType.NVarChar, 150, mc_strObservaciones)

                    .Add(mc_strArroba & mc_strFechayHora, SqlDbType.DateTime, 8, mc_strFechayHora)

                    .Add(mc_strArroba & mc_strIDRazon, SqlDbType.Int, 8, mc_strIDRazon)

                    .Add(mc_strArroba & mc_strNoCotizacion, SqlDbType.Int, 4, mc_strNoCotizacion)

                    .Add(mc_strArroba & mc_strempId, SqlDbType.Int, 4, mc_strempId)

                    .Add(mc_strArroba & "CodTecnico", SqlDbType.Int, 8, "CodTecnico")

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelCitas)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strIDCita, SqlDbType.Int, 4, mc_strIDCita)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSCitas)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoCotizacion, SqlDbType.Int, 4, mc_strNoCotizacion)

                    .Add(mc_strArroba & mc_strIDAgenda, SqlDbType.Int, 4, mc_strIDAgenda)

                    .Add(mc_strArroba & mc_strIDCita, SqlDbType.Int, 4, mc_strIDCita).Direction = ParameterDirection.Output

                    .Add(mc_strArroba & mc_strObservaciones, SqlDbType.VarChar, 150, mc_strObservaciones)

                    .Add(mc_strArroba & mc_strFechayHora, SqlDbType.DateTime, 8, mc_strFechayHora)

                    .Add(mc_strArroba & mc_strFechayHoraEnHorario, SqlDbType.DateTime, 8, mc_strFechayHoraEnHorario)

                    .Add(mc_strArroba & mc_strNoCita, SqlDbType.NVarChar, 12, mc_strNoCita).Direction = ParameterDirection.Output

                    .Add(mc_strArroba & mc_strIDRazon, SqlDbType.Int, 8, mc_strIDRazon)

                    .Add(mc_strArroba & mc_strCardCode, SqlDbType.VarChar, 20, mc_strCardCode)

                    .Add(mc_strArroba & mc_strIDVehiculo, SqlDbType.NVarChar, 8, mc_strIDVehiculo)

                    .Add(mc_strArroba & mc_strNoVehiculo, SqlDbType.NVarChar, 20, mc_strNoVehiculo)

                    .Add(mc_strArroba & mc_strNoSerie, SqlDbType.NVarChar, 7, mc_strNoSerie).Direction = ParameterDirection.Output

                    .Add(mc_strArroba & mc_strNoConsecutivo, SqlDbType.NVarChar, 4, mc_strNoConsecutivo).Direction = ParameterDirection.Output

                    .Add(mc_strArroba & mc_strCreadaPor, SqlDbType.NVarChar, 20, mc_strCreadaPor)

                    .Add(mc_strArroba & mc_strempId, SqlDbType.Int, 8, mc_strempId)

                    .Add(mc_strArroba & "CodTecnico", SqlDbType.Int, 8, "CodTecnico")


                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELCitas)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strNoCita, SqlDbType.NVarChar, 12, mc_strNoCita)

                    .Add(mc_strArroba & mc_strIDCita, SqlDbType.Int, 4, mc_strIDCita)

                    .Add(mc_strArroba & mc_strFechaIni, SqlDbType.DateTime, 8, mc_strFechaIni)

                    .Add(mc_strArroba & mc_strFechaFin, SqlDbType.DateTime, 8, mc_strFechaFin)

                    .Add(mc_strArroba & mc_strPlaca, SqlDbType.NVarChar, 20, mc_strPlaca)

                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.NVarChar, 8, mc_strCodMarca)

                    .Add(mc_strArroba & mc_strCodEstilo, SqlDbType.NVarChar, 8, mc_strCodEstilo)

                    .Add(mc_strArroba & mc_strCodModelo, SqlDbType.NVarChar, 8, mc_strCodModelo)

                    .Add(mc_strArroba & mc_strCardCode, SqlDbType.NVarChar, 20, mc_strCardCode)

                    .Add(mc_strArroba & mc_strIDAgenda, SqlDbType.Int, 4, mc_strIDAgenda)

                    .Add(mc_strArroba & mc_strNoVehiculo, SqlDbType.NVarChar, 20, mc_strNoVehiculo)

                    '.Add(mc_strArroba & "CodTecnico", SqlDbType.Int, 8, "CodTecnico")

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function

#End Region

    End Class
End Namespace
