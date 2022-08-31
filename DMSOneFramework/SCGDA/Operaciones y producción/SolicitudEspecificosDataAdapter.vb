Option Strict On
Option Explicit On

Namespace SCGDataAccess

    Public Class SolicitudEspecificosDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strID As String = "ID"
        Private Const mc_strFechaSolicitud As String = "FechaSolicitud"
        Private Const mc_strSolicitadoPor As String = "SolicitadoPor"
        Private Const mc_strRespondidoPor As String = "RespondidoPor"
        Private Const mc_strFechaRespuesta As String = "FechaRespuesta"
        Private Const mc_strEstado As String = "Estado"
        Private Const mc_strPrecioTotal As String = "PrecioTotal"
        Private Const mc_strCurrencySoliEspecifico As String = "Currency"
        Private Const mc_strDescEstado As String = "DescEstado"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strPlaca As String = "Placa"
        Private Const mc_strIDVehiculo As String = "IDVehiculo"
        Private Const mc_strCodMarca As String = "CodMarca"
        Private Const mc_strDescMarca As String = "DescMarca"
        Private Const mc_strCodEstilo As String = "CodEstilo"
        Private Const mc_strDescEstilo As String = "DescEstilo"
        Private Const mc_strCodModelo As String = "CodModelo"
        Private Const mc_strDescModelo As String = "DescModelo"
        Private Const mc_strNoVisita As String = "NoVisita"
        Private Const mc_strFecha_apertura As String = "Fecha_apertura"
        Private Const mc_strFecha_compromiso As String = "Fecha_compromiso"
        Private Const mc_strCodTipoOrden As String = "CodTipoOrden"
        Private Const mc_strNoCotizacion As String = "NoCotizacion"
        Private Const mc_strTipoDesc As String = "TipoDesc"
        Private Const mc_strObservacion As String = "Observacion"
        Private Const mc_strNoVehiculo As String = "NoVehiculo"
        Private Const mc_strHora_Comp As String = "Hora_Comp"
        Private Const mc_strFecha_Comp As String = "Fecha_Comp"
        Private Const mc_strFechaSolicitudIni As String = "FechaSolicitudIni"
        Private Const mc_strFechaSolicitudFin As String = "FechaSolicitudFin"
        Private Const mc_strFechaRespuestaIni As String = "FechaRespuestaIni"
        Private Const mc_strFechaRespuestaFin As String = "FechaRespuestaFin"

        Private m_adpSolicitudEspecificos As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_SelSolicitudEspecifico As String = "SCGTA_SP_SelSolicitudEspecifico"
        Private Const mc_strSCGTA_Sp_InsSolicitudEspecifico As String = "SCGTA_Sp_InsSolicitudEspecifico"
        Private Const mc_strSCGTA_Sp_UpdSolicitudEspecifico As String = "SCGTA_Sp_UpdSolicitudEspecifico"
        Private Const mc_strSCGTA_Sp_DelSolicitudEspecifico As String = "SCGTA_Sp_DelSolicitudEspecifico"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion

#End Region

#Region "Constructor"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpSolicitudEspecificos = New SqlClient.SqlDataAdapter
        End Sub

#End Region

#Region "Implementaciones . Net Framework"

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

        Public Overloads Function Fill(ByVal dataSet As SolicitudEspecificosDataset, _
                                       Optional ByVal p_intID As Integer = -1, _
                                       Optional ByVal p_strNoOrden As String = "", _
                                       Optional ByVal p_strNoVehiculo As String = "", _
                                       Optional ByVal p_strPlaca As String = "", _
                                       Optional ByVal p_intNoVisita As Integer = -1, _
                                       Optional ByVal p_intEstado As Integer = -1, _
                                       Optional ByVal p_strCodMarca As String = "", _
                                       Optional ByVal p_strCodEstilo As String = "", _
                                       Optional ByVal p_strCodModelo As String = "", _
                                       Optional ByVal p_dtFechaSolicitudIni As Date = Nothing, _
                                       Optional ByVal p_dtFechaSolicitudFin As Date = Nothing, _
                                       Optional ByVal p_dtFechaRespuestaini As Date = Nothing, _
                                       Optional ByVal p_dtFecharespuestaFin As Date = Nothing, _
                                       Optional ByRef p_cnConeccion As SqlClient.SqlConnection = Nothing, _
                                       Optional ByRef p_tnTransaccion As SqlClient.SqlTransaction = Nothing, _
                                       Optional ByRef p_blnUsarConeccionParametro As Boolean = False) As Integer

            Try
                If p_blnUsarConeccionParametro Then
                    If p_cnConeccion Is Nothing Then
                        p_cnConeccion = New SqlClient.SqlConnection
                    End If
                    'If p_cnConeccion.State <> ConnectionState.Closed Then
                    '    p_cnConeccion.Close()
                    'End If
                    If p_cnConeccion.State = ConnectionState.Closed Then
                        If p_cnConeccion.ConnectionString = "" Then
                            p_cnConeccion.ConnectionString = strConexionADO
                        End If
                        Call p_cnConeccion.Open()
                        p_tnTransaccion = p_cnConeccion.BeginTransaction

                    End If
                Else
                    If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                        m_cnnSCGTaller.Close()
                    End If
                    If m_cnnSCGTaller.State = ConnectionState.Closed Then
                        If m_cnnSCGTaller.ConnectionString = "" Then
                            m_cnnSCGTaller.ConnectionString = strConexionADO
                        End If
                        Call m_cnnSCGTaller.Open()
                    End If
                End If
                m_adpSolicitudEspecificos.SelectCommand = CrearSelectCommand()

                With m_adpSolicitudEspecificos.SelectCommand.Parameters

                    If p_intID <> -1 Then
                        .Item(mc_strArroba & mc_strID).Value = p_intID
                    End If

                    If p_strNoOrden <> "" Then
                        .Item(mc_strArroba & mc_strNoOrden).Value = p_strNoOrden
                    End If

                    If p_strNoVehiculo <> "" Then
                        .Item(mc_strArroba & mc_strNoVehiculo).Value = p_strNoVehiculo
                    End If
                    If p_strPlaca <> "" Then
                        .Item(mc_strArroba & mc_strPlaca).Value = p_strPlaca
                    End If
                    If p_intNoVisita <> -1 Then
                        .Item(mc_strArroba & mc_strNoVisita).Value = p_intNoVisita
                    End If
                    If p_intEstado <> -1 Then
                        .Item(mc_strArroba & mc_strEstado).Value = p_intEstado
                    End If
                    If p_strCodMarca <> "" Then
                        .Item(mc_strArroba & mc_strCodMarca).Value = p_strCodMarca
                    End If
                    If p_strCodEstilo <> "" Then
                        .Item(mc_strArroba & mc_strCodEstilo).Value = p_strCodEstilo
                    End If
                    If p_strCodModelo <> "" Then
                        .Item(mc_strArroba & mc_strCodModelo).Value = p_strCodModelo
                    End If
                    If p_dtFechaSolicitudIni <> Nothing And p_dtFechaSolicitudFin <> Nothing Then
                        .Item(mc_strArroba & mc_strFechaSolicitudIni).Value = p_dtFechaSolicitudIni
                        .Item(mc_strArroba & mc_strFechaSolicitudFin).Value = p_dtFechaSolicitudFin
                    End If
                    If p_dtFechaRespuestaini <> Nothing And p_dtFecharespuestaFin <> Nothing Then
                        .Item(mc_strArroba & mc_strFechaRespuestaIni).Value = p_dtFechaRespuestaini
                        .Item(mc_strArroba & mc_strFechaRespuestaFin).Value = p_dtFecharespuestaFin
                    End If

                End With
                If p_blnUsarConeccionParametro Then
                    m_adpSolicitudEspecificos.SelectCommand.Connection = p_cnConeccion
                    m_adpSolicitudEspecificos.SelectCommand.Transaction = p_tnTransaccion
                Else
                    m_adpSolicitudEspecificos.SelectCommand.Connection = m_cnnSCGTaller
                End If
                Call m_adpSolicitudEspecificos.Fill(dataSet.SCGTA_SP_SelSolicitudEspecifico)

            Catch ex As Exception
                Throw ex
            Finally
                If Not p_blnUsarConeccionParametro Then
                    'Call p_cnConeccion.Close()
                    'Call m_cnnSCGTaller.Close()
                End If
            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As SolicitudEspecificosDataset, ByRef cnConeccion As SqlClient.SqlConnection, _
                                         ByRef tnTransaccion As SqlClient.SqlTransaction, _
                                         Optional ByVal p_blnFinalizarTransaccion As Boolean = True) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If
                cnConeccion = m_cnnSCGTaller
                tnTransaccion = cnConeccion.BeginTransaction()

                m_adpSolicitudEspecificos.InsertCommand = CreateInsertCommand()
                m_adpSolicitudEspecificos.InsertCommand.Connection = m_cnnSCGTaller
                m_adpSolicitudEspecificos.InsertCommand.Transaction = tnTransaccion

                m_adpSolicitudEspecificos.UpdateCommand = CrearUpdateCommand()
                m_adpSolicitudEspecificos.UpdateCommand.Connection = m_cnnSCGTaller
                m_adpSolicitudEspecificos.UpdateCommand.Transaction = tnTransaccion

                m_adpSolicitudEspecificos.DeleteCommand = CrearDeleteCommand()
                m_adpSolicitudEspecificos.DeleteCommand.Connection = m_cnnSCGTaller
                m_adpSolicitudEspecificos.DeleteCommand.Transaction = tnTransaccion

                Call m_adpSolicitudEspecificos.Update(dataSet.SCGTA_SP_SelSolicitudEspecifico)

                If p_blnFinalizarTransaccion Then

                    tnTransaccion.Commit()
                    Call m_cnnSCGTaller.Close()

                End If

            Catch ex As Exception

                tnTransaccion.Rollback()
                Call m_cnnSCGTaller.Close()
                Throw ex
            Finally
                'Agregado 02072010
                'Call m_cnnSCGTaller.Close()


            End Try

        End Function

#End Region

#Region "Creacion de Comandos del DataAdapter"

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelSolicitudEspecifico)
                cmdSel.CommandType = CommandType.StoredProcedure
                With cmdSel.Parameters


                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4)
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.NVarChar, 50)
                    .Add(mc_strArroba & mc_strNoVehiculo, SqlDbType.NVarChar, 20)
                    .Add(mc_strArroba & mc_strPlaca, SqlDbType.NVarChar, 20)
                    .Add(mc_strArroba & mc_strNoVisita, SqlDbType.Int, 20)
                    .Add(mc_strArroba & mc_strEstado, SqlDbType.Int, 4)
                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.NVarChar, 8)
                    .Add(mc_strArroba & mc_strCodEstilo, SqlDbType.NVarChar, 8)
                    .Add(mc_strArroba & mc_strCodModelo, SqlDbType.NVarChar, 8)
                    .Add(mc_strArroba & mc_strFechaSolicitudIni, SqlDbType.DateTime, 8)
                    .Add(mc_strArroba & mc_strFechaSolicitudFin, SqlDbType.DateTime, 8)
                    .Add(mc_strArroba & mc_strFechaRespuestaIni, SqlDbType.DateTime, 8)
                    .Add(mc_strArroba & mc_strFechaRespuestaFin, SqlDbType.DateTime, 8)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_Sp_UpdSolicitudEspecifico)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID)
                    .Add(mc_strArroba & mc_strRespondidoPor, SqlDbType.NVarChar, 50, mc_strRespondidoPor)
                    .Add(mc_strArroba & mc_strEstado, SqlDbType.SmallInt, 2, mc_strEstado)
                    With .Add(mc_strArroba & mc_strPrecioTotal, SqlDbType.Decimal)
                        .Precision = 15
                        .Scale = 2
                        .SourceColumn = mc_strPrecioTotal
                    End With
                    .Add(mc_strArroba & mc_strCurrencySoliEspecifico, SqlDbType.NVarChar, 5, "DocCur")
                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_Sp_DelSolicitudEspecifico)

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
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_Sp_InsSolicitudEspecifico)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strSolicitadoPor, SqlDbType.NVarChar, 50, mc_strSolicitadoPor)
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.NVarChar, 20, mc_strNoOrden)
                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID).Direction = ParameterDirection.Output

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try

        End Function



#End Region

    End Class

End Namespace
