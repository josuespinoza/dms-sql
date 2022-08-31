Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion

Namespace SCGDataAccess
    Public Class ColaboradorDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

#Region "Constantes"

        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_intcolaborador As String = "CodColaborador"
        Private Const mc_intNoFase As String = "NoFase"
        Private Const mc_inttiempohoras As String = "TiempoHoras"
        Private Const mc_intFechainicio As String = "FechaInicio"
        Private Const mc_intFechafin As String = "FechaFin"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_indicador As String = "indicador"
        Private Const mc_horas As String = "horas"
        Private Const mc_razon As String = "NoRazon"
        Private Const mc_strValidarSuspendidas As String = "ValidarSuspendidas"
        Private Const mc_strMostrarSoloSintiempo As String = "MostrarSoloSintiempo"
        'Private Const mc_str_TotalUnidadTiempo As String = "TotalUnidadTiempo"
        Private Const mc_bolReAsignado As String = "ReAsignado"


        'Declaracion de las constantes con el nombre de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_UpdColaboraFechainicio As String = "SCGTA_SP_UpdColaboraXFechainicio"
        Private Const mc_strSCGTA_SP_UpdColaboraFechafin As String = "SCGTA_SP_UpdColaboraXFechaFIN"
        Private Const mc_strSCGTA_SP_UpdCalculaFecha As String = "SCGTA_SP_CalculaFechaDiferencia"

        Private Const mc_strSCGTA_SP_SELColabora As String = "SCGTA_SP_SelColabora"
        Private Const mc_strSCGTA_SP_SELColaboraInd As String = "SCGTA_SP_SelColaboraInd"
        Private Const mc_strSCGTA_SP_InsColabora As String = "SCGTA_SP_INSColabora"
        Private Const mc_strSCGTA_SP_INSColaboraConVerificacion As String = "SCGTA_SP_INSColaboraConVerificacion"

        Private Const mc_strSCGTA_SP_UPDIniciaColabora As String = "SCGTA_SP_UPDIniciaColabora"
        Private Const mc_strSCGTA_SP_DelColabora As String = "SCGTA_SP_DelColabora"
        Private Const mc_strSCGTA_SP_UpdColaboraSuspendido As String = "SCGTA_SP_UPDSuspendeColabora"
        Private Const mc_strSCGTA_SP_UpdColaboraSuspenderFase As String = "SCGTA_SP_UPDSuspendeFase"
        Private Const mc_strSCGTA_SP_UPDFinalizarFaseCol As String = "SCGTA_SP_UPDFinalizarFaseCol"
        Private Const mc_strSCGTA_SP_UpdColaboraFinalizado As String = "SCGTA_SP_UPDFinalizarColabora"
        Private Const mc_strSCGTA_SP_DELColaborador As String = "SCGTA_SP_DELColaborador"
        Private Const mc_strSCGTA_SP_SELColaboradorIniXOrden As String = "SCGTA_SP_SELColabIniciadoXOrden"
        Private Const mc_strSCGTA_SP_InsColabora2 As String = "SCGTA_SP_INSColaboraXSegundaVez"
        Private Const mc_strSCGTA_SP_SelColabIniSusp As String = "SCGTA_SP_SELColabIniciadosOSuspendidos"
        Private Const mc_strSCGTA_SP_SelColabIni As String = "SCGTA_SP_SELColabIniciados"
        Private Const mc_strSCGTA_SP_SelCountColAsig As String = "SCGTA_SP_SelCountColAsig"
        Private Const mc_str_SCGTA_SP_UPDFinalizarColaboraHorasDigitadas As String = "SCGTA_SP_UPDFinalizarColaboraHorasDigitadas"

        Private Const mc_strSCGTA_SP_SelControlColaboradorxActividad As String = "SCGTA_SP_SelControlColaboradorxActividad"

        'Actualiza el campo ReAsignar colaborador
        Private Const mc_strSCGTA_SP_UPDColaboradorReAsignar As String = "SCGTA_SP_UPDColaboradorReAsignar"

        ''Produccion
        Private Const mccol_intID As String = "ID"
        Private Const mccol_intEmpId As String = "EmpID"
        Private Const mccol_dtFechaInicio As String = "FechaInicio"
        Private Const mccol_dtFechaFin As String = "FechaFin"
        Private Const mccol_intReproceso As String = "Reproceso"
        Private Const mccol_dblCosto As String = "Costo"
        Private Const mccol_dblTiempoHoras As String = "TiempoHoras"
        Private Const mccol_strNoOrden As String = "NoOrden"
        Private Const mccol_intNoFase As String = "NoFase"
        Private Const mccol_strEmpNombre As String = "EmpNombre"
        Private Const mccol_strEstado As String = "Estado"
        Private Const mccol_intReferencia As String = "Referencia"
        Private Const mccol_intNoRazon As String = "NoRazon"
        Private Const mccol_strProceso As String = "Proceso"
        Private Const mccol_strFecha As String = "Fecha"
        Private Const mccol_strIDActividad As String = "IDActividad"
        Private Const mccol_strEsTiempoDigitado As String = "EsTotalDigitado"
        Private Const mccol_strTotalDigitado As String = "TiempoDigitado"
        Private Const mccol_strTiempoHoras As String = "TiempoHoras"
        Private Const mccol_dblCostoEstandar As String = "CostoEstandar"
        Private Const mccol_strTableName As String = "SCGTA_TB_CONTROLCOLABORADOR"


        Private Const mc_strArroba As String = "@"

        Private m_adpAct As SqlClient.SqlDataAdapter

#End Region

#Region "Variables"

        Private m_adpColabora As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Dim objDAConexion As DAConexion

#End Region

#End Region

#Region "Inicializa DataAdapter"

        Public Sub New(Optional ByVal blnIniciar As Boolean = True)
            If blnIniciar Then
                objDAConexion = New DAConexion
                m_cnnSCGTaller = objDAConexion.ObtieneConexion
            End If
            m_adpColabora = New SqlClient.SqlDataAdapter
        End Sub


#End Region

#Region "Implementaciones"

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


        Public Overloads Function Fill(ByVal dataSet As ColaboradorDataset, ByVal ORDEN As String, ByVal NoFase As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpColabora.SelectCommand = CrearSelectCommandInd()

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado
                If ORDEN = "" Then
                    m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = System.DBNull.Value
                Else
                    m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = ORDEN
                End If


                If NoFase = 0 Then
                    m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = System.DBNull.Value
                Else
                    m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = NoFase
                End If

                dataSet.SCGTA_TB_ControlColaborador.CheckColumn.DefaultValue = 0
                dataSet.SCGTA_TB_ControlColaborador.ReprocesoColumn.DefaultValue = 0


                m_adpColabora.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpColabora.Fill(dataSet.SCGTA_TB_ControlColaborador)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As ColaboradorDataset, ByVal ORDEN As String, ByVal NoFase As Integer, ByVal Indicador As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpColabora.SelectCommand = CrearSelectCommandInd()

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado
                If ORDEN = "" Then
                    m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = System.DBNull.Value
                Else
                    m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = ORDEN
                End If


                If NoFase = 0 Then
                    m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = System.DBNull.Value
                Else
                    m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = NoFase
                End If

                m_adpColabora.SelectCommand.Parameters(mc_strArroba & "Indicador").Value = Indicador

                dataSet.SCGTA_TB_ControlColaborador.CheckColumn.DefaultValue = 0
                dataSet.SCGTA_TB_ControlColaborador.ReprocesoColumn.DefaultValue = 0

                m_adpColabora.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpColabora.Fill(dataSet.SCGTA_TB_ControlColaborador)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As ColaboradorDataset, ByVal ORDEN As String, ByVal NoFase As Integer, ByVal Indicador As Integer, ByVal bolSoloSinTiempo As Boolean) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpColabora.SelectCommand = CrearSelectCommandSoloSintiempo()

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado
                If ORDEN = "" Then
                    m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = System.DBNull.Value
                Else
                    m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = ORDEN
                End If


                If NoFase = 0 Then
                    m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = System.DBNull.Value
                Else
                    m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = NoFase
                End If

                m_adpColabora.SelectCommand.Parameters(mc_strArroba & "Indicador").Value = Indicador

                dataSet.SCGTA_TB_ControlColaborador.CheckColumn.DefaultValue = 0
                dataSet.SCGTA_TB_ControlColaborador.ReprocesoColumn.DefaultValue = 0

                m_adpColabora.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpColabora.Fill(dataSet.SCGTA_TB_ControlColaborador)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function VerificarColPendi(ByVal p_strNoOrden As String, _
                                                    ByVal p_intNoFase As Integer, _
                                                    Optional ByVal p_intValidarSuspendidas As Integer = 0) As Boolean
            Dim objSqlCommand As SqlClient.SqlCommand
            Dim intCount As Integer
            Dim blnResult As Boolean

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                objSqlCommand = New SqlClient.SqlCommand("SCGTA_SP_SelVerificarFaseColPendi", m_cnnSCGTaller)
                objSqlCommand.CommandType = CommandType.StoredProcedure

                objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_intNoFase, SqlDbType.Int)).Value = p_intNoFase
                objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden
                If p_intValidarSuspendidas <> 0 Then
                    objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strValidarSuspendidas, SqlDbType.VarChar, 50)).Value = p_intValidarSuspendidas
                End If


                intCount = CInt(objSqlCommand.ExecuteScalar)

                If intCount <> 0 Then
                    blnResult = True
                Else
                    blnResult = False
                End If

                Return blnResult

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try
        End Function

        Public Overloads Function Update(ByVal p_DTColaborador As ColaboradorDataset.SCGTA_TB_ControlColaboradorDataTable) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpColabora.InsertCommand = CreateInsertCommand()
                m_adpColabora.InsertCommand.Connection = m_cnnSCGTaller

                'Call m_adpColabora.Update(dataSet.SCGTA_TB_ControlColaborador)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function UpdateIniciar(ByVal p_DTColaborador As ColaboradorDataset.SCGTA_TB_ControlColaboradorDataTable, ByVal p_strProceso As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpColabora.UpdateCommand = CreateUpdateIniciarCommand()
                m_adpColabora.UpdateCommand.Connection = m_cnnSCGTaller

                m_adpColabora.UpdateCommand.Parameters(mc_strArroba & mccol_strProceso).Value = p_strProceso

                Call m_adpColabora.Update(p_DTColaborador)

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function UpdateSuspender(ByRef p_DTColaborador As ColaboradorDataset.SCGTA_TB_ControlColaboradorDataTable, _
                                                  ByVal norazon As Integer, ByVal p_strProceso As String, _
                                                  ByVal dtFecha As Date) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpColabora.UpdateCommand = CreateUpdateSuspenderCommand()
                m_adpColabora.UpdateCommand.Connection = m_cnnSCGTaller

                With m_adpColabora.UpdateCommand
                    .Parameters(mc_strArroba & mccol_intNoRazon).Value = norazon
                    .Parameters(mc_strArroba & mccol_strProceso).Value = p_strProceso
                    If dtFecha <> Nothing Then
                        .Parameters(mc_strArroba & mccol_strFecha).Value = dtFecha
                    Else
                        .Parameters(mc_strArroba & mccol_strFecha).Value = DBNull.Value
                    End If

                End With

                Call m_adpColabora.Update(p_DTColaborador)

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function UpdateFinalizar(ByRef p_DTColaborador As ColaboradorDataset.SCGTA_TB_ControlColaboradorDataTable, ByVal p_strProceso As String) As Integer
            Dim intResult As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpColabora.UpdateCommand = CreateUpdateFinalizarCommand()
                m_adpColabora.UpdateCommand.Connection = m_cnnSCGTaller

                m_adpColabora.UpdateCommand.Parameters(mc_strArroba & mccol_strProceso).Value = p_strProceso

                intResult = m_adpColabora.Update(p_DTColaborador)

                Return intResult

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function UpdateFinalizar(ByRef p_DTColaborador As ColaboradorDataset.SCGTA_TB_ControlColaboradorDataTable, ByVal p_strProceso As String, ByVal bolTiempoDigitado As Boolean) As Integer
            Dim intResult As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpColabora.UpdateCommand = CreateUpdateFinalizarCommandTiempoDigitado(bolTiempoDigitado)
                m_adpColabora.UpdateCommand.Connection = m_cnnSCGTaller

                m_adpColabora.UpdateCommand.Parameters(mc_strArroba & mccol_strProceso).Value = p_strProceso

                intResult = m_adpColabora.Update(p_DTColaborador)

                Return intResult

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function


        Public Overloads Function UpdateFinalizar(ByRef p_DTColaborador As ColaboradorDataset.SCGTA_TB_ControlColaboradorDataTable, ByVal p_strProceso As String, _
                                                  ByRef p_cnConeccion As SqlClient.SqlConnection, ByRef p_tnnTransaccion As SqlClient.SqlTransaction) As Integer
            Dim intResult As Integer

            Try
                If p_cnConeccion Is Nothing Then
                    p_cnConeccion = New SqlClient.SqlConnection
                End If
                If p_cnConeccion.State = ConnectionState.Closed Then
                    If p_cnConeccion.ConnectionString = "" Then
                        p_cnConeccion.ConnectionString = strConexionADO
                    End If
                    Call p_cnConeccion.Open()
                    p_tnnTransaccion = p_cnConeccion.BeginTransaction
                End If

                m_adpColabora.UpdateCommand = CreateUpdateFinalizarCommand()
                m_adpColabora.UpdateCommand.Connection = p_cnConeccion
                m_adpColabora.UpdateCommand.Transaction = p_tnnTransaccion

                m_adpColabora.UpdateCommand.Parameters(mc_strArroba & mccol_strProceso).Value = p_strProceso

                intResult = m_adpColabora.Update(p_DTColaborador)

                Return intResult

            Catch ex As Exception
                Throw ex
            Finally
                'm_cnnSCGTaller.Close()
            End Try

        End Function

        Public Function Actualizar(ByVal dataset As ColaboradorDataset) As Integer
            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpColabora.UpdateCommand = CrearUpdateCommand()
                m_adpColabora.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpColabora.Update(dataset.SCGTA_TB_ControlColaborador)

            Catch ex As Exception
                Throw ex
            Finally

                Call m_cnnSCGTaller.Close()

            End Try
        End Function

        Public Function InsertarNuevo(ByVal dataset As ColaboradorDataset) As Integer
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpColabora.InsertCommand = CrearInsertNuevoCommand()
                m_adpColabora.InsertCommand.Connection = m_cnnSCGTaller

                Call m_adpColabora.Update(dataset.SCGTA_TB_ControlColaborador)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
        End Function

        Public Function InsertarNuevo(ByVal dataset As ColaboradorDataset, _
                                      ByVal p_cnnConeccion As SqlClient.SqlConnection, _
                                      ByRef p_trnTransaccion As SqlClient.SqlTransaction) As Integer
            Try

                m_adpColabora.InsertCommand = CrearInsertNuevoCommandConVerif()
                m_adpColabora.InsertCommand.Connection = p_cnnConeccion
                m_adpColabora.InsertCommand.Transaction = p_trnTransaccion

                Call m_adpColabora.Update(dataset.SCGTA_TB_ControlColaborador)

            Catch ex As Exception
                Throw ex
                'Finally
                'Call m_cnnSCGTaller.Close()
            End Try
        End Function

        Public Function ActualizarFin(ByVal dataset As ColaboradorDataset) As Integer
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If
                m_adpColabora.UpdateCommand = CrearUpdateCommandFin()
                m_adpColabora.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpColabora.Update(dataset.SCGTA_TB_ControlColaborador)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()

            End Try
        End Function

'        Public Function Actualizahoras(ByVal dataset As ColaboradorDataset, ByVal fechai As DateTime, ByVal fechaf As DateTime)
'            Dim strLlave As String
'
'            Try
'
'                If m_cnnSCGTaller.State = ConnectionState.Closed Then
'                    If m_cnnSCGTaller.ConnectionString = "" Then
'                        m_cnnSCGTaller.ConnectionString = strConexionADO
'                    End If
'                    Call m_cnnSCGTaller.Open()
'                End If
'
'                m_adpColabora.UpdateCommand = CrearUpdateCommandHoras()
'                m_adpColabora.UpdateCommand.Connection = m_cnnSCGTaller
'
'                Call m_adpColabora.Update(dataset.SCGTA_TB_ControlColaborador)
'
'
'            Catch ex As Exception
'                Throw ex
'            Finally
'                Call m_cnnSCGTaller.Close()
'            End Try
'
'        End Function

        Public Function ActualizarSuspendido(ByVal dataset As ColaboradorDataset) As Integer
            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpColabora.UpdateCommand = CrearUpdateCommandSuspendido()
                m_adpColabora.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpColabora.Update(dataset.SCGTA_TB_ControlColaborador)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function
        '''''''''''''''''''''''''''''''''
        Public Sub EliminarColaborador(ByVal dataset As ColaboradorDataset)
            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpColabora.DeleteCommand = CrearDeleteColaborador()
                m_adpColabora.DeleteCommand.Connection = m_cnnSCGTaller
                'dataset.SCGTA_TB_ControlColaborador.Rows
                Call m_adpColabora.Update(dataset.SCGTA_TB_ControlColaborador)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
        End Sub
        '''''''''''''''''''''''''''''''''''''
        Public Sub SelColaboradorIniciadoXOrden(ByVal dataSet As ColaboradorDataset, ByVal NoOrden As String)
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpColabora.SelectCommand = CrearSelectCmdColabIniciados()



                m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden



                dataSet.SCGTA_TB_ControlColaborador.CheckColumn.DefaultValue = 0
                dataSet.SCGTA_TB_ControlColaborador.ReprocesoColumn.DefaultValue = 0

                m_adpColabora.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpColabora.Fill(dataSet.SCGTA_TB_ControlColaborador)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try
        End Sub

        Public Sub InsertColabXSegundaVez(ByVal dataset As ColaboradorDataset, ByVal strProceso As String)
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpColabora.InsertCommand = CrearInsCmd()
                m_adpColabora.InsertCommand.Connection = m_cnnSCGTaller
                m_adpColabora.InsertCommand.Parameters(mc_strArroba & mccol_strProceso).Value = strProceso

                Call m_adpColabora.Update(dataset.SCGTA_TB_ControlColaborador)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Sub

        Public Sub SelColaboradoresAFinalizar(ByVal dataSet As ColaboradorDataset, ByVal Orden As String, ByVal NoFase As Integer)
            'Selecciona los colaboradores de una fase que estan iniciados o suspendidos 
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpColabora.SelectCommand = CrearSelCmdColabIniciadosOSuspendidos()

                m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = Orden
                m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = NoFase

                dataSet.SCGTA_TB_ControlColaborador.CheckColumn.DefaultValue = 0
                'dataSet.SCGTA_TB_ControlColaborador.ReprocesoColumn.DefaultValue = 0

                m_adpColabora.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpColabora.Fill(dataSet.SCGTA_TB_ControlColaborador)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try
        End Sub

        Public Sub SelColabIniciadosXOrdenXFase(ByVal dataSet As ColaboradorDataset, ByVal Orden As String, ByVal NoFase As Integer)
            'Selecciona los colaboradores de una fase y una orden especifica que estan iniciados
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpColabora.SelectCommand = CrearSelCmdColabIniciados()

                m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = Orden
                m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = NoFase


                m_adpColabora.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpColabora.Fill(dataSet.SCGTA_TB_ControlColaborador)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try
        End Sub

        Public Function VerificarColAsig(ByVal p_strNoOrden As String) As Integer
            Dim cmdColaboradoresAsig As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelCountColAsig, m_cnnSCGTaller)
            Dim intResult As Integer

            With cmdColaboradoresAsig

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden

            End With

            intResult = CInt(cmdColaboradoresAsig.ExecuteScalar)

            Return intResult

        End Function

        Public Function VerificarColAsig(ByVal p_strNoOrden As String, _
                                         ByRef p_cnConeccion As SqlClient.SqlConnection, _
                                         ByRef p_tnnTransaccion As SqlClient.SqlTransaction) As Integer

            Dim cmdColaboradoresAsig As SqlClient.SqlCommand
            Dim intResult As Integer

            If p_cnConeccion Is Nothing Then
                p_cnConeccion = New SqlClient.SqlConnection
            End If
            If p_cnConeccion.State = ConnectionState.Closed Then
                p_cnConeccion.ConnectionString = strConectionString
                p_cnConeccion.Open()
                p_tnnTransaccion = p_cnConeccion.BeginTransaction
            End If

            cmdColaboradoresAsig = New SqlClient.SqlCommand(mc_strSCGTA_SP_SelCountColAsig, p_cnConeccion, p_tnnTransaccion)

            With cmdColaboradoresAsig

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden

            End With

            intResult = CInt(cmdColaboradoresAsig.ExecuteScalar)

            Return intResult

        End Function




        'Public Function UpdateReAsignarColaborador(ByRef p_DTColaborador As ColaboradorDataset.SCGTA_TB_ControlColaboradorDataTable, ByVal p_intIDActividad As Integer)


        '    Try

        '        If m_cnnSCGTaller.State = ConnectionState.Closed Then
        '            If m_cnnSCGTaller.ConnectionString = "" Then
        '                m_cnnSCGTaller.ConnectionString = strConexionADO
        '            End If
        '            Call m_cnnSCGTaller.Open()
        '        End If

        '        m_adpColabora.UpdateCommand = CrearUpdateCommandReAsignar()
        '        m_adpColabora.UpdateCommand.Connection = m_cnnSCGTaller

        '        m_adpColabora.UpdateCommand.Parameters(mc_strArroba & mccol_strIDActividad).Value = p_intIDActividad

        '        m_adpColabora.Update(p_DTColaborador)

        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        m_cnnSCGTaller.Close()
        '    End Try

        'End Function


        Public Overloads Function UpdateReAsignarColaborador(ByVal p_intIDActividad As Integer) As Boolean
            Dim objSqlCommand As SqlClient.SqlCommand
            Dim intCount As Integer
            Dim blnResult As Boolean

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                objSqlCommand = New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDColaboradorReAsignar, m_cnnSCGTaller)
                objSqlCommand.CommandType = CommandType.StoredProcedure

                objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mccol_strIDActividad, SqlDbType.Int)).Value = p_intIDActividad
               
                intCount = CInt(objSqlCommand.ExecuteScalar)

                If intCount <> 0 Then
                    blnResult = True
                Else
                    blnResult = False
                End If

                Return blnResult

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try
        End Function

#End Region

#Region "Creación de comandos"

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELColabora)



            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 9, mc_intNoFase)



            End With

            Return cmdSel

        End Function

        Private Function CrearSelectCommandInd() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELColaboraInd)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)

                .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 9)

                .Add(mc_strArroba & "Indicador", SqlDbType.Int, 9)

            End With

            Return cmdSel

        End Function

        Private Function CrearSelectCommandSoloSintiempo() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELColaboraInd)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)

                .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 9)

                .Add(mc_strArroba & "Indicador", SqlDbType.Int, 9)

                .Add(mc_strArroba & mc_strMostrarSoloSintiempo, SqlDbType.Int, 1)

                .Item(mc_strArroba & mc_strMostrarSoloSintiempo).Value = 1


            End With

            Return cmdSel

        End Function

        Private Function CrearSelCmdColabIniciadosOSuspendidos() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelColabIniSusp)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)

                .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 9)


            End With

            Return cmdSel
        End Function

        Private Function CrearSelCmdColabIniciados() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelColabIni)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)

                .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 9)


            End With

            Return cmdSel
        End Function
        Private Function CrearSelectCmdColabIniciados() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELColaboradorIniXOrden)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)

            End With

            Return cmdSel
        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsColabora)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                    .Add(mc_strArroba & mc_intcolaborador, SqlDbType.Int, 4, mc_intcolaborador)

                    .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 9, mc_intNoFase)

                    .Add(mc_strArroba & mc_inttiempohoras, SqlDbType.Int, 4, mc_inttiempohoras)

                End With


                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CreateUpdateIniciarCommand() As SqlClient.SqlCommand
            Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDIniciaColabora)

            cmdIns.CommandType = CommandType.StoredProcedure

            With cmdIns.Parameters
                .Add(mc_strArroba & mccol_intID, SqlDbType.Int, 4, mccol_intID)
                .Add(mc_strArroba & mccol_strProceso, SqlDbType.VarChar, 15)
                .Add(mc_strArroba & mccol_dtFechaInicio, SqlDbType.DateTime, 9, mccol_dtFechaInicio)
                .Add(mc_strArroba & mccol_dtFechaFin, SqlDbType.DateTime, 9, mccol_dtFechaFin)
            End With

            Return cmdIns

        End Function

        Private Function CreateUpdateSuspenderCommand() As SqlClient.SqlCommand
            Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdColaboraSuspendido)

            cmdIns.CommandType = CommandType.StoredProcedure

            With cmdIns.Parameters

                .Add(mc_strArroba & mccol_intID, SqlDbType.Int, 4, mccol_intID)
                .Add(mc_strArroba & mccol_intNoRazon, SqlDbType.Int, 4)
                .Add(mc_strArroba & mccol_strProceso, SqlDbType.VarChar, 15)
                .Add(mc_strArroba & mccol_strFecha, SqlDbType.DateTime, 9)

                .Add(mc_strArroba & mccol_dblCosto, SqlDbType.Decimal, 9, mccol_dblCosto).Direction = ParameterDirection.InputOutput
                .Add(mc_strArroba & mccol_dblTiempoHoras, SqlDbType.Decimal, 9, mccol_dblTiempoHoras).Direction = ParameterDirection.InputOutput

                .Item(mc_strArroba & mccol_dblCosto).Precision = 15
                .Item(mc_strArroba & mccol_dblCosto).Scale = 4

                .Item(mc_strArroba & mccol_dblTiempoHoras).Precision = 15
                .Item(mc_strArroba & mccol_dblTiempoHoras).Scale = 4


            End With

            Return cmdIns

        End Function

        Private Function CreateUpdateFinalizarCommand() As SqlClient.SqlCommand
            Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdColaboraFinalizado)

            cmdIns.CommandType = CommandType.StoredProcedure

            With cmdIns.Parameters

                .Add(mc_strArroba & mccol_intID, SqlDbType.Int, 4, mccol_intID)
                .Add(mc_strArroba & mccol_strProceso, SqlDbType.VarChar, 15)
                .Add(mc_strArroba & mccol_dblCosto, SqlDbType.Decimal, 9, mccol_dblCosto).Direction = ParameterDirection.InputOutput
                .Add(mc_strArroba & mccol_dblTiempoHoras, SqlDbType.Decimal, 9, mccol_dblTiempoHoras).Direction = ParameterDirection.InputOutput
                .Add(mc_strArroba & mccol_dtFechaFin, SqlDbType.DateTime, 9, mccol_dtFechaFin)
                .Add(mc_strArroba & mccol_dblCostoEstandar, SqlDbType.Decimal, 9, mccol_dblCostoEstandar).Direction = ParameterDirection.InputOutput

            End With

            With cmdIns.Parameters(mc_strArroba & mccol_dblCosto)
                .Precision = 15
                .Scale = 4
            End With

            With cmdIns.Parameters(mc_strArroba & mccol_dblTiempoHoras)
                .Precision = 15
                .Scale = 4
            End With

            With cmdIns.Parameters(mc_strArroba & mccol_dblCostoEstandar)
                .Precision = 15
                .Scale = 4
            End With

            Return cmdIns

        End Function

        Private Function CreateUpdateFinalizarCommandTiempoDigitado(ByVal bolTiempoDigitado As Boolean) As SqlClient.SqlCommand
            Dim cmdIns As New SqlClient.SqlCommand(mc_str_SCGTA_SP_UPDFinalizarColaboraHorasDigitadas)

            cmdIns.CommandType = CommandType.StoredProcedure

            With cmdIns.Parameters

                .Add(mc_strArroba & mccol_intID, SqlDbType.Int, 4, mccol_intID)
                .Add(mc_strArroba & mccol_strProceso, SqlDbType.VarChar, 15)
                .Add(mc_strArroba & mccol_dblCosto, SqlDbType.Decimal, 9, mccol_dblCosto).Direction = ParameterDirection.InputOutput
                .Add(mc_strArroba & mccol_dblTiempoHoras, SqlDbType.Decimal, 9, mccol_dblTiempoHoras).Direction = ParameterDirection.InputOutput
                .Add(mc_strArroba & mccol_dtFechaFin, SqlDbType.DateTime, 9, mccol_dtFechaFin)
                .Add(mc_strArroba & mccol_strTotalDigitado, SqlDbType.Decimal, 12, mccol_strTiempoHoras)
                .Add(mc_strArroba & mccol_dblCostoEstandar, SqlDbType.Decimal, 9, mccol_dblCostoEstandar).Direction = ParameterDirection.InputOutput
                '.Item(mc_strArroba & mccol_strTotalDigitado).Value = dblTiempo

                '.Item(mc_strArroba & mccol_strTotalDigitado).Value = dblTiempoDigitado

            End With

            With cmdIns.Parameters(mc_strArroba & mccol_dblCosto)
                .Precision = 15
                .Scale = 4
            End With

            With cmdIns.Parameters(mc_strArroba & mccol_dblTiempoHoras)
                .Precision = 15
                .Scale = 4
            End With

            With cmdIns.Parameters(mc_strArroba & mccol_dblCostoEstandar)
                .Precision = 15
                .Scale = 4
            End With

            Return cmdIns

        End Function

        Private Function CrearInsertNuevoCommand() As SqlClient.SqlCommand
            Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsColabora)

            cmdIns.CommandType = CommandType.StoredProcedure

            With cmdIns.Parameters
                .Add(mc_strArroba & mccol_intNoFase, SqlDbType.Int, 4, mccol_intNoFase)
                .Add(mc_strArroba & mccol_strNoOrden, SqlDbType.VarChar, 50, mccol_strNoOrden)
                .Add(mc_strArroba & mccol_intReproceso, SqlDbType.Bit, 1, mccol_intReproceso)
                .Add(mc_strArroba & mccol_intEmpId, SqlDbType.Int, 4, mccol_intEmpId)
                .Add(mc_strArroba & mccol_dblTiempoHoras, SqlDbType.Float, 5, mccol_dblTiempoHoras)
                .Add(mc_strArroba & mccol_strEstado, SqlDbType.VarChar, 20, mccol_strEstado)
                .Add(mc_strArroba & mccol_dblCosto, SqlDbType.Float, 9, mccol_dblCosto)
                .Add(mc_strArroba & mccol_strIDActividad, SqlDbType.Int, 4, mccol_strIDActividad)
                .Add(mc_strArroba & mccol_dtFechaInicio, SqlDbType.DateTime, 9, mccol_dtFechaInicio)
                .Add(mc_strArroba & mccol_dtFechaFin, SqlDbType.DateTime, 9, mccol_dtFechaFin)
            End With

            Return cmdIns
        End Function

        Private Function CrearInsertNuevoCommandConVerif() As SqlClient.SqlCommand
            Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSColaboraConVerificacion)

            cmdIns.CommandType = CommandType.StoredProcedure

            With cmdIns.Parameters
                .Add(mc_strArroba & mccol_intNoFase, SqlDbType.Int, 4, mccol_intNoFase)
                .Add(mc_strArroba & mccol_strNoOrden, SqlDbType.VarChar, 50, mccol_strNoOrden)
                .Add(mc_strArroba & mccol_intReproceso, SqlDbType.Bit, 1, mccol_intReproceso)
                .Add(mc_strArroba & mccol_intEmpId, SqlDbType.Int, 4, mccol_intEmpId)
                .Add(mc_strArroba & mccol_dblTiempoHoras, SqlDbType.Float, 5, mccol_dblTiempoHoras)
                .Add(mc_strArroba & mccol_strEstado, SqlDbType.VarChar, 20, mccol_strEstado)
                .Add(mc_strArroba & mccol_dblCosto, SqlDbType.Float, 9, mccol_dblCosto)
                .Add(mc_strArroba & mccol_dblCostoEstandar, SqlDbType.Float, 9, mccol_dblCostoEstandar)
                .Add(mc_strArroba & mccol_strIDActividad, SqlDbType.Int, 4, mccol_strIDActividad)
            End With

            Return cmdIns
        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdColaboraFechainicio)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters


                    .Add(mc_strArroba & mc_indicador, SqlDbType.Int, 4, mc_indicador)


                    .Add(mc_strArroba & mc_intFechainicio, SqlDbType.DateTime, 8, mc_intFechainicio)

                End With


                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommandFin() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdColaboraFechafin)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters


                    .Add(mc_strArroba & mc_indicador, SqlDbType.Int, 4, mc_indicador)



                    .Add(mc_strArroba & mc_intFechafin, SqlDbType.DateTime, 8, mc_intFechafin)

                End With


                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommandHoras() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdCalculaFecha)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters


                    .Add(mc_strArroba & mc_intFechainicio, SqlDbType.DateTime, 8, mc_intFechainicio)

                    .Add(mc_strArroba & mc_intFechafin, SqlDbType.DateTime, 8, mc_intFechafin)

                    .Add(mc_strArroba & mc_indicador, SqlDbType.Int, 4, mc_indicador)
                End With


                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommandSuspendido() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdColaboraSuspendido)

                cmdIns.CommandType = CommandType.StoredProcedure
                With cmdIns.Parameters


                    .Add(mc_strArroba & mc_indicador, SqlDbType.Int, 4, mc_indicador)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearDeleteColaborador() As SqlClient.SqlCommand

            Try
                Dim cmdDelete As New SqlClient.SqlCommand(mc_strSCGTA_SP_DELColaborador)

                cmdDelete.CommandType = CommandType.StoredProcedure

                With cmdDelete.Parameters
                    .Add(mc_strArroba & mccol_intID, SqlDbType.Int, 4, mccol_intID)
                    '.Add(mc_strArroba & mccol_strEstado, SqlDbType.NVarChar, 20, mccol_strEstado)

                End With

                Return cmdDelete

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearInsCmd() As SqlClient.SqlCommand

            Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsColabora2)

            cmdIns.CommandType = CommandType.StoredProcedure

            With cmdIns.Parameters
                .Add(mc_strArroba & mccol_intNoFase, SqlDbType.Int, 4, mccol_intNoFase)
                .Add(mc_strArroba & mccol_strNoOrden, SqlDbType.VarChar, 50, mccol_strNoOrden)
                .Add(mc_strArroba & mccol_intReproceso, SqlDbType.Bit, 1, mccol_intReproceso)
                .Add(mc_strArroba & mccol_intEmpId, SqlDbType.Int, 4, mccol_intEmpId)
                .Add(mc_strArroba & mccol_dblTiempoHoras, SqlDbType.Float, 5, mccol_dblTiempoHoras)
                .Add(mc_strArroba & mccol_strEstado, SqlDbType.VarChar, 20, mccol_strEstado)
                .Add(mc_strArroba & mccol_dblCosto, SqlDbType.Float, 9, mccol_dblCosto)
                .Add(mc_strArroba & mccol_dtFechaInicio, SqlDbType.SmallDateTime, 8, mccol_dtFechaInicio)
                .Add(mc_strArroba & mccol_dtFechaFin, SqlDbType.SmallDateTime, 8, mccol_dtFechaFin)
                .Add(mc_strArroba & mccol_intReferencia, SqlDbType.Int, 4, mccol_intReferencia)
                .Add(mc_strArroba & mccol_intNoRazon, SqlDbType.Int, 4, mccol_intNoRazon)
                .Add(mc_strArroba & mccol_strProceso, SqlDbType.VarChar, 15)
            End With

            Return cmdIns
        End Function

        Public Function CargarDuracionEstandar(ByRef m_dstCol As DMSOneFramework.ColaboradorDataset, ByVal g_intUnidadTiempo As Integer, ByVal m_dblValorUnidadTiempo As Double) As Integer
            Try
                Dim intFila As Integer
                Dim strIDActividad As String
                Dim intDuracionEstandar As Decimal
                Dim cmdCommand As New SqlClient.SqlCommand
                cmdCommand.Connection = New SqlClient.SqlConnection(strConectionString)
                If cmdCommand.Connection.State = ConnectionState.Closed Then
                    cmdCommand.Connection.Open()
                End If

                For intFila = 0 To m_dstCol.SCGTA_TB_ControlColaborador.Rows.Count - 1
                    strIDActividad = m_dstCol.SCGTA_TB_ControlColaborador.Item(intFila)("ID")
                    cmdCommand.CommandText = "Exec SCGTA_SP_SelTiempoEstandar " & strIDActividad
                    intDuracionEstandar = cmdCommand.ExecuteScalar
                    If g_intUnidadTiempo = -1 Then
                        m_dstCol.SCGTA_TB_ControlColaborador.Rows(intFila)("TiempoEstandar") = intDuracionEstandar
                    Else
                        m_dstCol.SCGTA_TB_ControlColaborador.Rows(intFila)("TiempoEstandar") = Math.Round(intDuracionEstandar / m_dblValorUnidadTiempo, 4)
                    End If

                Next

                cmdCommand.Connection.Close()

                Return 0
            Catch ex As Exception
                Return -1
            End Try
        End Function



        Private Function CrearUpdateCommandReAsignar() As SqlClient.SqlCommand

            Try

                Dim cmdUpdate As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDColaboradorReAsignar)

                cmdUpdate.CommandType = CommandType.StoredProcedure

                With cmdUpdate.Parameters


                    .Add(mc_strArroba & mccol_strIDActividad, SqlDbType.Int, 9, mccol_strIDActividad)

                End With


                Return cmdUpdate

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "Procedimientos"

        Public Sub SuspenderFase(ByVal p_intNoFase As Integer, ByVal p_strOrden As String, ByVal p_intNoSuspension As Integer, ByVal p_strProceso As String)
            Dim cmdSuspender As New SqlClient.SqlCommand

            Try

                With cmdSuspender
                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = mc_strSCGTA_SP_UpdColaboraSuspenderFase
                    .Parameters.Add(mc_strArroba & mccol_intNoFase, SqlDbType.Int).Value = p_intNoFase
                    .Parameters.Add(mc_strArroba & mccol_strNoOrden, SqlDbType.NVarChar, 50).Value = p_strOrden
                    .Parameters.Add(mc_strArroba & mccol_intNoRazon, SqlDbType.Int).Value = p_intNoSuspension
                    .Parameters.Add(mc_strArroba & mccol_strProceso, SqlDbType.VarChar, 15).Value = p_strProceso
                End With

                cmdSuspender.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub FinalizarFase(ByVal p_intNoFase As Integer, ByVal p_strOrden As String, ByVal p_strProceso As String)
            Dim cmdFinalizar As New SqlClient.SqlCommand

            Try

                With cmdFinalizar
                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = mc_strSCGTA_SP_UPDFinalizarFaseCol
                    .Parameters.Add(mc_strArroba & mccol_intNoFase, SqlDbType.Int).Value = p_intNoFase
                    .Parameters.Add(mc_strArroba & mccol_strNoOrden, SqlDbType.NVarChar, 50).Value = p_strOrden
                    .Parameters.Add(mc_strArroba & mccol_strProceso, SqlDbType.VarChar, 15).Value = p_strProceso

                End With

                cmdFinalizar.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex
            End Try
        End Sub




        Public Sub SelControlColaboradorxActividad(ByVal dataSet As ColaboradorDataset, ByVal p_strNoOrden As String, ByVal p_intIDActividad As Integer)
            'Selecciona los colaboradores de una fase que estan iniciados o suspendidos 
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpColabora.SelectCommand = CrearSelControlColaboradorxActividad()

                m_adpColabora.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = p_strNoOrden
                m_adpColabora.SelectCommand.Parameters(mc_strArroba & mccol_strIDActividad).Value = p_intIDActividad

                'dataSet.SCGTA_TB_ControlColaborador.CheckColumn.DefaultValue = 0
                'dataSet.SCGTA_TB_ControlColaborador.ReprocesoColumn.DefaultValue = 0

                m_adpColabora.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpColabora.Fill(dataSet.SCGTA_TB_ControlColaborador)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try
        End Sub

        Private Function CrearSelControlColaboradorxActividad() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelControlColaboradorxActividad)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)

                .Add(mc_strArroba & mccol_strIDActividad, SqlDbType.Int, 9)


            End With

            Return cmdSel
        End Function
#End Region

    End Class
End Namespace
