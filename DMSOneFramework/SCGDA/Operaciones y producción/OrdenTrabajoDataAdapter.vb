Imports DMSOneFramework.SCGDataAccess.DAConexion

Namespace SCGDataAccess
    Public Class OrdenTrabajoDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strNoOrden_Orig As String = "NoOrden_Orig"
        Private Const mc_intNoFase As String = "NoFaseActual"
        Private Const mc_intNoVisita As String = "NoVisita"

        Private Const mc_intTipoOrden As String = "CodTipoOrden"
        Private Const mc_strCodModelo As String = "CodModelo"

        Private Const mc_DatFechaApertura As String = "Fecha_apertura"
        Private Const mc_DatFechaCompromiso As String = "Fecha_compromiso"
        Private Const mc_DatFechaCierre As String = "Fecha_cierre"
        Private Const mc_strFechaAperturaini As String = "APERTURAINI"
        Private Const mc_strFechaCompromisoini As String = "COMPROMISOINI"
        Private Const mc_strFechaCierreini As String = "CIERREINI"
        Private Const mc_strFechaAperturafin As String = "APERTURAFIN"
        Private Const mc_strFechaCompromisofin As String = "COMPROMISOFIN"
        Private Const mc_strFechaCierrefin As String = "CIERREFIN"

        Private Const mc_strEstado As String = "Estado"
        Private Const mc_strOrden As String = "NoOrden"
        Private Const mc_intAsesor As String = "Asesor"
        Private Const mc_intNoCotizacion As String = "NoCotizacion"
        Private Const mc_strObservacion As String = "Observacion"
        Private Const mc_strClienteFacturar As String = "ClienteFacturar"
        Private Const mc_strMontoReparacion As String = "MontoReparacion"

        Private Const mc_strPlaca As String = "Placa"
        Private Const mc_strCodMarca As String = "CodMarca"
        Private Const mc_strCodEstilo As String = "CodEstilo"
        Private Const mc_intCono As String = "Cono"
        Private Const mc_NoVehiculo As String = "NoVehiculo"
        Private Const mc_strCardCode As String = "CardCode"
        Private Const mc_strCardName As String = "CardName"

        Private Const mc_strIDEstadoWeb As String = "IDEstadoWeb"
        Private Const mc_strKilometraje As String = "Kilometraje"

        Private Const mc_strCostoAlmacenamiento As String = "CostoAlmacenamiento"

        Private m_adpOrden As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDOrden As String = "SCGTA_SP_UpdOrdenTrabajo"
        Private Const mc_strSCGTA_SP_UPDOrden2 As String = "SCGTA_SP_UpdOrdenTrabajo2"
        Private Const mc_strSCGTA_SP_UPDOrdenTrabajoSimple As String = "SCGTA_SP_UPDOrdenTrabajoSimple"
        Private Const mc_strSCGTA_SP_SELOrden As String = "SCGTA_SP_SELORDENTRABAJOFILTRO"
        Private Const mc_strSCGTA_SP_SELORDENESBYNoVisita As String = "SCGTA_SP_SELORDENESBYNoVisita"
        Private Const mc_strSCGTA_SP_INSOrden As String = "SCGTA_SP_INSOrdenTrabajo"
        Private Const mc_strSCGTA_SP_DelOrden As String = "SCGTA_SP_DELOrdenTrabajo"
        Private Const mc_strSCGTA_SP_SELOrdenFacturacion As String = "SCGTA_SP_SelOrdenEncabezado"
        Private Const mc_strSCGTA_SP_SelNombreempleado As String = "SCGTA_SP_SELDEVUELVENOMBRE"
        Private Const mc_strSCGTA_SP_SelMontoIns As String = "SCGTA_SP_SELORDEN"
        Private Const mc_strSCGTA_SP_SelManoOFacturada As String = "SCGTA_SP_SELMontoManoObraFact"
        Private Const mc_strSCGTA_SP_UpdManoOFacturada As String = "SCGTA_SP_UPDMontoManoObraFact"
        Private Const mc_strSCGTA_SP_SelOrdenxoOrdn As String = "SCGTA_SP_SelOrdenxoOrdn"
        Private Const mc_strSCGTA_TB_UpdOrden As String = "SCGTA_TB_UpdOrden"
        Private Const mc_strSCGTA_SP_UpdEstadoCerrada As String = "SCGTA_SP_UPDEstadoOrdenCerrada"
        Private Const mc_strSCGTA_SP_SelOrdenProcSusp As String = "SCGTA_SP_SelOrdenesProcesoYSuspendidas"
        Private Const mc_strSCGTA_SP_UPDCostoAlmaceOrden As String = "SCGTA_SP_UPDCostoAlmaceOrden"
        Private Const mc_strSCGTA_SP_SELOrdenTrabajoByNumero As String = "SCGTA_SP_SELOrdenTrabajoByNumero"
        Private Const mc_strSCGTA_SP_UPDFecSyncItemsOrden As String = "SCGTA_SP_UPDFecSyncItemsOrden"

        Private Const mc_strSCGTA_SP_UpdEstadoWeb As String = "SCGTA_SP_UpdEstadoWeb"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

        Private mc_strIfNoExpediente As String = "IfNoExpediente"
        Private mc_strIfNoCono As String = "IfNoCono"
        Private mc_strIfPlaca As String = "IfPlaca"

        ''para factura interna, series y lotes 
        Private m_adpSeriesLotes As SqlClient.SqlDataAdapter

#End Region

#Region "Constructor"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpOrden = New SqlClient.SqlDataAdapter
        End Sub

        Public Sub New(ByVal strCadenaConexion As String)

            m_cnnSCGTaller = New SqlClient.SqlConnection(strCadenaConexion)

            m_adpOrden = New SqlClient.SqlDataAdapter

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

        Public Overloads Function Fill(ByVal dataSet As OrdenTrabajoDataset, _
                                       ByVal m_strorden As String, _
                                       ByVal valos As Boolean) As Integer

            Try
                m_adpOrden.SelectCommand = CrearSelectCommandOrden()

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.SelectCommand.CommandTimeout = 480
                m_adpOrden.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = m_strorden

                m_adpOrden.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpOrden.Fill(dataSet.SCGTA_TB_Orden)

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As OrdenTrabajoDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If


                m_adpOrden.SelectCommand = CrearSelectCommand()

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.SelectCommand.CommandTimeout = 480

                m_adpOrden.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpOrden.Fill(dataSet.SCGTA_TB_Orden)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        ''**********************Documentos Draft**************************

        Public Overloads Function Fill_x_OrdenTrabajo(ByVal dataSet As RepuestosxOrdenDataset, ByVal p_NoOrden As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpOrden.SelectCommand = Me.CreateSelectCommandOT(p_NoOrden)

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.SelectCommand.CommandTimeout = 480

                dataSet.EnforceConstraints = False
                Fill_x_OrdenTrabajo = m_adpOrden.Fill(dataSet.SCGTA_TB_RepuestosxOrden)


            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function


        ''**********************Documentos Draft**************************

        Public Overloads Function Fill(ByVal dataSet As OrdenTrabajoDataset, ByVal intNoVisita As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpOrden.SelectCommand = CrearSelectCommand()

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.SelectCommand.CommandTimeout = 480


                'If intNoVisita <> 0 Then

                m_adpOrden.SelectCommand.Parameters.Add(mc_strArroba & mc_intNoVisita, SqlDbType.Int, 4, mc_intNoVisita).Value = intNoVisita

                'End If
                m_adpOrden.SelectCommand.Parameters(mc_strArroba & mc_intNoVisita).Value = intNoVisita

                m_adpOrden.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpOrden.Fill(dataSet.SCGTA_TB_Orden)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Fill(ByRef p_dataSet As OrdenTrabajoDataset, ByVal p_strNoOrden As String, _
                                       ByVal p_strNoVehiculo As String, ByVal p_strPlaca As String, ByVal p_intNoVisita As Integer, _
                                      ByVal p_strCono As String, ByVal p_strEstado As String, ByVal p_strMarca As String, _
                                      ByVal p_strEstilo As String, ByVal p_strModelo As String, ByVal p_dtAperturaini As Date, _
                                      ByVal p_dtCompromisoini As Date, ByVal p_dtCierreini As Date, ByVal p_dtAperturafin As Date, _
                                      ByVal p_dtCompromisofin As Date, ByVal p_dtCierrefin As Date) As Integer

            Try

                'Call m_cnnSCGTaller.Open()

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpOrden.SelectCommand = CrearSelectCommand()

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 240.
                m_adpOrden.SelectCommand.CommandTimeout = 480

                With m_adpOrden.SelectCommand.Parameters

                    If p_strNoOrden <> "" Then
                        .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden).Value = p_strNoOrden
                    End If

                    If p_strNoVehiculo <> "" Then
                        .Add(mc_strArroba & mc_NoVehiculo, SqlDbType.VarChar, 20, mc_NoVehiculo).Value = p_strNoVehiculo
                    End If

                    If p_strPlaca <> "" Then
                        .Add(mc_strArroba & mc_strPlaca, SqlDbType.VarChar, 20, mc_strPlaca).Value = p_strPlaca
                    End If

                    If p_intNoVisita <> 0 Then
                        .Add(mc_strArroba & mc_intNoVisita, SqlDbType.Int, 4, mc_intNoVisita).Value = p_intNoVisita
                    End If

                    If p_strCono <> "" Then
                        .Add(mc_strArroba & mc_intCono, SqlDbType.NVarChar, 50, mc_intCono).Value = p_strCono
                    End If

                    If p_strEstado <> "" Then
                        .Add(mc_strArroba & mc_strEstado, SqlDbType.VarChar, 50, mc_strEstado).Value = p_strEstado
                    End If

                    If p_strMarca <> "" Then
                        .Add(mc_strArroba & mc_strCodMarca, SqlDbType.VarChar, 8, mc_strCodMarca).Value = p_strMarca
                    End If

                    If p_strEstilo <> "" Then
                        .Add(mc_strArroba & mc_strCodEstilo, SqlDbType.VarChar, 8, mc_strCodEstilo).Value = p_strEstilo
                    End If

                    If p_strModelo <> "" Then
                        .Add(mc_strArroba & mc_strCodModelo, SqlDbType.VarChar, 8, mc_strCodModelo).Value = p_strModelo
                    End If

                    If p_dtAperturafin <> Nothing And p_dtAperturaini <> Nothing Then

                        .Add(mc_strArroba & mc_strFechaAperturafin, SqlDbType.DateTime, 8, mc_strFechaAperturafin).Value = p_dtAperturafin
                        .Add(mc_strArroba & mc_strFechaAperturaini, SqlDbType.DateTime, 8, mc_strFechaAperturaini).Value = p_dtAperturaini

                    End If

                    If p_dtCierrefin <> Nothing And p_dtCierreini <> Nothing Then

                        .Add(mc_strArroba & mc_strFechaCierrefin, SqlDbType.DateTime, 8, mc_strFechaCierrefin).Value = p_dtCierrefin
                        .Add(mc_strArroba & mc_strFechaCierreini, SqlDbType.DateTime, 8, mc_strFechaCierreini).Value = p_dtCierreini

                    End If

                    If p_dtCompromisofin <> Nothing And p_dtCompromisoini <> Nothing Then

                        .Add(mc_strArroba & mc_strFechaCompromisofin, SqlDbType.DateTime, 8, mc_strFechaCompromisofin).Value = p_dtCompromisofin
                        .Add(mc_strArroba & mc_strFechaCompromisoini, SqlDbType.DateTime, 8, mc_strFechaCompromisoini).Value = p_dtCompromisoini

                    End If

                End With

                'Se comenta esta línea que estaba creada previamente. Erick Sanabria Bravo  
                'm_adpOrden.SelectCommand.CommandTimeout = 0

                m_adpOrden.SelectCommand.Connection = m_cnnSCGTaller

                'MsgBox(m_adpOrden.SelectCommand.CommandTimeout)

                Call m_adpOrden.Fill(p_dataSet.SCGTA_TB_Orden)




            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As OrdenTrabajoDataset, ByVal m_strorden As String, _
                                       Optional ByVal p_intConeccionNueva As Integer = 0) As Integer

            Try
                Dim cnConeccionLocal As New SqlClient.SqlConnection
                m_adpOrden.SelectCommand = CrearSelectCommand()
                If p_intConeccionNueva = 0 Then
                    If m_cnnSCGTaller.State = ConnectionState.Closed Then
                        If m_cnnSCGTaller.ConnectionString = "" Then
                            m_cnnSCGTaller.ConnectionString = strConexionADO
                        End If
                        m_cnnSCGTaller.Open()
                    End If
                Else
                    cnConeccionLocal.ConnectionString = m_cnnSCGTaller.ConnectionString
                    cnConeccionLocal.Open()
                End If
                m_adpOrden.SelectCommand.Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.NVarChar, 20).Value = m_strorden
                If p_intConeccionNueva = 0 Then
                    m_adpOrden.SelectCommand.Connection = m_cnnSCGTaller
                Else
                    m_adpOrden.SelectCommand.Connection = cnConeccionLocal
                End If


                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.SelectCommand.CommandTimeout = 480

                Call m_adpOrden.Fill(dataSet.SCGTA_TB_Orden)

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Sub UpdateEstadoWeb(ByVal strNoOrden As String, ByVal intIDEstadoWeb As Integer)
            Dim cmd As New SqlClient.SqlCommand
            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If
                With cmd
                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = mc_strSCGTA_SP_UpdEstadoWeb
                    .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = strNoOrden
                    .Parameters.Add(mc_strArroba & mc_strIDEstadoWeb, SqlDbType.Int).Value = intIDEstadoWeb
                End With
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
        End Sub

        Public Overloads Function Update(ByVal dataSet As OrdenDataset) As String

            'este string devuelve el numero de orden que se creo en la Base de Datos
            Dim strNoOrden As String

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpOrden.UpdateCommand = CrearUpdateCommand()

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.UpdateCommand.CommandTimeout = 480

                m_adpOrden.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpOrden.Update(dataSet.SCGTA_TB_Orden)

                strNoOrden = m_adpOrden.InsertCommand.Parameters(mc_strArroba & mc_strOrden).Value

                If strNoOrden.Length > 0 Then
                    Update = strNoOrden
                Else
                    Update = ""
                End If

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As OrdenTrabajoDataset) As String

            Try


                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If


                m_adpOrden.InsertCommand = CreateInsertCommand()

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.InsertCommand.CommandTimeout = 480

                m_adpOrden.InsertCommand.Connection = m_cnnSCGTaller

                Call m_adpOrden.Update(dataSet.SCGTA_TB_Orden)

                Return m_adpOrden.InsertCommand.Parameters.Item(mc_strArroba & mc_strNoOrden).Value

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function

        Public Overloads Sub Update(ByVal dataSet As OrdenTrabajoDataset, _
                                         ByRef cn As SqlClient.SqlConnection, _
                                         ByRef tran As SqlClient.SqlTransaction, _
                                         Optional ByVal blnIniciar As Boolean = False, _
                                         Optional ByVal blnTerminar As Boolean = False) ' As String

            Try

                If blnIniciar Then
                    If m_cnnSCGTaller.State = ConnectionState.Closed Then
                        If m_cnnSCGTaller.ConnectionString = "" Then
                            m_cnnSCGTaller.ConnectionString = strConexionADO
                        End If
                        m_cnnSCGTaller.Open()
                        cn = m_cnnSCGTaller
                        tran = cn.BeginTransaction
                    End If
                End If

                m_adpOrden.InsertCommand = CreateInsertCommand()

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.InsertCommand.CommandTimeout = 480

                m_adpOrden.InsertCommand.Connection = cn
                m_adpOrden.InsertCommand.Transaction = tran

                Call m_adpOrden.Update(dataSet.SCGTA_TB_Orden)

                'tran.Commit()

                'Return m_adpOrden.InsertCommand.Parameters.Item(mc_strArroba & mc_strNoOrden).Value

            Catch ex As Exception

                Throw ex
                '            Finally
                '                m_cnnSCGTaller.Close()
                '
            End Try


        End Sub

        Public Overloads Function Update(ByVal dataSet As OrdenTrabajoDataset, _
                                         ByVal bandera As Boolean) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpOrden.UpdateCommand = CrearUpdateCommand3()

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.UpdateCommand.CommandTimeout = 480

                m_adpOrden.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpOrden.Update(dataSet.SCGTA_TB_Orden)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function

        Public Overloads Function Actualizar(ByVal dataSet As OrdenTrabajoDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpOrden.UpdateCommand = CrearActualizarCommand()

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.UpdateCommand.CommandTimeout = 480

                m_adpOrden.UpdateCommand.Connection = m_cnnSCGTaller

                Return m_adpOrden.Update(dataSet.SCGTA_TB_Orden)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function

        Public Overloads Function Actualizar(ByVal dataSet As OrdenTrabajoDataset, _
                                             ByRef cnConeccion As SqlClient.SqlConnection, _
                                             ByRef tnTransaccion As SqlClient.SqlTransaction) As Integer

            Try
                If cnConeccion Is Nothing Then
                    cnConeccion = New SqlClient.SqlConnection
                End If
                If cnConeccion.State = ConnectionState.Closed Then
                    If cnConeccion.ConnectionString = "" Then
                        cnConeccion.ConnectionString = strConexionADO
                    End If
                    cnConeccion.Open()
                    tnTransaccion = cnConeccion.BeginTransaction
                End If

                m_adpOrden.UpdateCommand = CrearActualizarCommand()

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.UpdateCommand.CommandTimeout = 480

                m_adpOrden.UpdateCommand.Connection = cnConeccion
                m_adpOrden.UpdateCommand.Transaction = tnTransaccion

                Return m_adpOrden.Update(dataSet.SCGTA_TB_Orden)

            Catch ex As Exception

                Throw ex

            Finally
                'Call cnConeccion.Close()

            End Try


        End Function

        Public Overloads Function Insert(ByVal dataSet As OrdenTrabajoDataset) As String

            'este string devuelve el numero de orden que se creo en la Base de Datos
            Dim strNoOrden As String

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpOrden.InsertCommand = CreateInsertCommand()


                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.InsertCommand.CommandTimeout = 480

                m_adpOrden.InsertCommand.Connection = m_cnnSCGTaller

                Call m_adpOrden.Update(dataSet.SCGTA_TB_Orden)

                strNoOrden = m_adpOrden.InsertCommand.Parameters(mc_strArroba & mc_strOrden).Value

                'Se retorna el numero de orden en caso de la insercion un string vacio para el update.
                If strNoOrden.Length > 0 Then
                    Insert = strNoOrden
                Else
                    Insert = ""
                End If

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try


        End Function

        Public Function DevuelveNombre(ByVal dataset As OrdenTrabajoDataset, ByVal p_strcardcode As String) As String
'            Dim nombre As String
            Dim RNombre As SqlClient.SqlDataReader

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpOrden.SelectCommand = CrearSelectCommandNombre()

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.SelectCommand.CommandTimeout = 480


                If p_strcardcode = "" Then
                    m_adpOrden.SelectCommand.Parameters(mc_strArroba & mc_strCardCode).Value = System.DBNull.Value
                Else
                    m_adpOrden.SelectCommand.Parameters(mc_strArroba & mc_strCardCode).Value = p_strcardcode
                End If


                m_adpOrden.SelectCommand.Connection = m_cnnSCGTaller
                RNombre = m_adpOrden.SelectCommand.ExecuteReader

                If RNombre.Read Then
                    Return RNombre("CardName")
                Else
                    Return ""
                End If


            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Function DevuelveMonto(ByVal p_strnoorden As String) As Decimal
'            Dim nombre As String
            Dim RNombre As SqlClient.SqlDataReader

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpOrden.SelectCommand = CrearSelectCommandMonto()

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.SelectCommand.CommandTimeout = 480

                If p_strnoorden = "" Then
                    m_adpOrden.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = System.DBNull.Value
                Else
                    m_adpOrden.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = p_strnoorden
                End If


                m_adpOrden.SelectCommand.Connection = m_cnnSCGTaller
                RNombre = m_adpOrden.SelectCommand.ExecuteReader

                If RNombre.Read Then

                    ''Sólo Mano de Obra
                    'If IsDBNull(RNombre("MontoManoObraINS")) Then
                    '    Return 0
                    'Else
                    '    Return RNombre("MontoManoObraINS")
                    'End If

                    ''''''''''''''''''''''''''''''''''''''''''''''''
                    ''Mano de Obra + Adicionales
                    If IsDBNull(RNombre("MontoMOTotal")) Then
                        Return 0
                    Else
                        Return RNombre("MontoMOTotal")
                    End If

                Else
                    Return ""
                End If


            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Actualizar(ByVal dataSet As OrdenDataset) As String

            'Preguntar quien usa este método y para que lo usa. (Jonathan Vargas)

'            Dim strLlave As String

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpOrden.InsertCommand = CreateInsertCommand()

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.InsertCommand.CommandTimeout = 480

                m_adpOrden.InsertCommand.Connection = m_cnnSCGTaller

                m_adpOrden.UpdateCommand = CrearUpdateCommand()
                m_adpOrden.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpOrden.Update(dataSet.SCGTA_TB_Orden)


            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
            Return String.Empty
        End Function

        Public Function SelManoObraFacturada(ByVal NoOrden As String) As Decimal
            'Agregado 24/05/06. Alejandra. Permite seleccionar el MontoManoObraFacturado en la Tabla Orden

            Dim cmd As New SqlClient.SqlCommand
            Dim decManoObra As Decimal

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                With cmd
                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = mc_strSCGTA_SP_SelManoOFacturada
                    .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = NoOrden
                End With

                decManoObra = cmd.ExecuteScalar
                Return decManoObra

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Sub UpdManoObraFacturada(ByVal NoOrden As String, ByVal Monto As Decimal)

            'Agregado 24/05/06. Alejandra. Permite actualizar el campo MontoManoObraFacturado en la Tabla Orden
            Dim cmd As New SqlClient.SqlCommand
'            Dim decManoObra As Decimal


            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                With cmd
                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = mc_strSCGTA_SP_UpdManoOFacturada
                    .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = NoOrden
                End With

                cmd.ExecuteNonQuery()


            Catch ex As Exception
                Throw ex

            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Sub

        Public Sub EstablecerEstadoOrdenCerrada(ByVal p_decNoExpediente As Decimal)

            'Establece a 'Cerrada' el Estado de la Orden
            Dim cmd As New SqlClient.SqlCommand


            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                With cmd
                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = mc_strSCGTA_SP_UpdEstadoCerrada
                    .Parameters.Add(mc_strArroba & mc_intNoVisita, SqlDbType.Decimal, 9).Value = p_decNoExpediente
                End With

                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex

            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Sub

        Public Sub SelOrdenesProcesoYSuspendidas(ByVal p_dstOrden As OrdenTrabajoDataset)
            'Selecciona de la tabla Ordenes aquellas con estado En Proceso o Suspendida
            Try
                m_adpOrden.SelectCommand = CmdOrdenesProcesoYSuspendidas()

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpOrden.SelectCommand.Connection = m_cnnSCGTaller

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.SelectCommand.CommandTimeout = 480

                Call m_adpOrden.Fill(p_dstOrden.SCGTA_TB_Orden)

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Sub

        Public Sub ActualizarCostoAlmacen(ByVal p_decCosto As Decimal, ByVal p_strNoOrden As String)
            Dim cmdOrden As New SqlClient.SqlCommand

            With cmdOrden
                .Connection = m_cnnSCGTaller
                .CommandType = CommandType.StoredProcedure
                .CommandText = mc_strSCGTA_SP_UPDCostoAlmaceOrden
                .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden

                .Parameters.Add(mc_strArroba & mc_strCostoAlmacenamiento, SqlDbType.Decimal, 9).Value = p_decCosto
                .Parameters(mc_strArroba & mc_strCostoAlmacenamiento).Scale = 4
                .Parameters(mc_strArroba & mc_strCostoAlmacenamiento).Precision = 15

            End With

            cmdOrden.ExecuteNonQuery()

            cmdOrden.Connection.Close()

        End Sub

        Public Sub SelOrden(ByRef p_dataSet As OrdenTrabajoDataset, ByVal p_strNoOrden As String)

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpOrden.SelectCommand = New SqlClient.SqlCommand

                'Erick Sanabria Bravo. 04.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.SelectCommand.CommandTimeout = 480

                m_adpOrden.SelectCommand.CommandType = CommandType.StoredProcedure
                m_adpOrden.SelectCommand.CommandText = mc_strSCGTA_SP_SELOrden 'mc_strSCGTA_SP_SELOrdenTrabajoByNumero
                m_adpOrden.SelectCommand.Connection = m_cnnSCGTaller

                With m_adpOrden.SelectCommand.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden).Value = p_strNoOrden

                End With


                Call m_adpOrden.Fill(p_dataSet.SCGTA_TB_Orden)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try


        End Sub

        Public Sub UpdFechaSyncItemsOrden(ByVal conexion As SqlClient.SqlConnection, ByVal trans As SqlClient.SqlTransaction, ByVal NoOrden As String)

            Dim cmd As New SqlClient.SqlCommand

            Try
                If conexion.State = ConnectionState.Closed Then
                    Call conexion.Open()
                End If

                With cmd
                    .Connection = conexion
                    .Transaction = trans
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = mc_strSCGTA_SP_UPDFecSyncItemsOrden
                    .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = NoOrden
                End With

                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex

            Finally
                'Call conexion.Close()
            End Try

        End Sub

        Public Function SelCodigoTecnico(ByVal p_CodTecnico As Integer) As String

            Dim cmd As New SqlClient.SqlCommand

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                With cmd
                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "SCGTA_SP_DescTecnico"
                    .Parameters.Add(mc_strArroba & "CodTecnico", SqlDbType.VarChar, 50).Value = p_CodTecnico
                End With

                Return cmd.ExecuteScalar()

            Catch ex As Exception
                Throw ex

            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

#End Region

#Region "Creación de comandos"

        Private Function CrearSelectCommandNombre() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelNombreempleado)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strCardCode, SqlDbType.VarChar, 50, mc_strCardCode)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearSelectCommandMonto() As SqlClient.SqlCommand
            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelMontoIns)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELOrden)

                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearSelectCommandOrden() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELOrdenFacturacion)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearSelectCommandxNoOrden() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelOrdenxoOrdn)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        'Private Function CrearSelectCommandByNoExpediente() As SqlClient.SqlCommand

        '    Try

        '        Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELOrden)

        '        cmdSel.CommandType = CommandType.StoredProcedure

        '        With cmdSel.Parameters

        '            'Parametros o criterios de búsqueda 
        '            .Add(mc_strArroba & mc_intNoVisita, SqlDbType.Decimal, 9, mc_intNoVisita)

        '        End With

        '        Return cmdSel

        '    Catch ex As Exception
        '        Throw ex
        '    End Try

        'End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSOrden)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strOrden, SqlDbType.VarChar, 50, mc_strOrden)

                    '.Item(mc_strArroba & mc_strOrden).Direction = ParameterDirection.Output

                    .Add(mc_strArroba & mc_intTipoOrden, SqlDbType.Int, 4, mc_intTipoOrden)

                    .Add(mc_strArroba & mc_intNoVisita, SqlDbType.Int, 4, mc_intNoVisita)

                    .Add(mc_strArroba & mc_strClienteFacturar, SqlDbType.NVarChar, 15, mc_strClienteFacturar)

                    .Add(mc_strArroba & mc_intAsesor, SqlDbType.Int, 4, mc_intAsesor)

                    .Add(mc_strArroba & mc_strObservacion, SqlDbType.VarChar, 1000, mc_strObservacion)

                    .Add(mc_strArroba & mc_strMontoReparacion, SqlDbType.Int, 4, mc_strMontoReparacion)

                    .Add(mc_strArroba & mc_intNoCotizacion, SqlDbType.Int, 4, mc_intNoCotizacion)

                    .Add(mc_strArroba & "CodTecnico", SqlDbType.Int, 4000, "CodTecnico")

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDOrden)

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters


                    .Add(mc_strArroba & mc_strOrden, SqlDbType.VarChar, 50, mc_strOrden)
                    .Item(mc_strArroba & mc_strOrden).Direction = ParameterDirection.Output

                    .Add(mc_strArroba & mc_intTipoOrden, SqlDbType.Int, 4, mc_intTipoOrden)

                    .Add(mc_strArroba & mc_intNoVisita, SqlDbType.Int, 4, mc_intNoVisita)

                    .Add(mc_strArroba & mc_DatFechaApertura, SqlDbType.DateTime, 8, mc_DatFechaApertura)

                    .Add(mc_strArroba & mc_strObservacion, SqlDbType.VarChar, 500, mc_strObservacion)


                End With

                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        'Private Function CrearUpdateCommand2() As SqlClient.SqlCommand
        '    'Solo actuliza el porcentaje de prorrateo
        '    Try

        '        Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDOrden2)

        '        cmdUPD.CommandType = CommandType.StoredProcedure

        '        With cmdUPD.Parameters

        '            .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

        '        End With

        '        Return cmdUPD

        '    Catch ex As Exception
        '        Throw ex
        '    End Try


        'End Function

        Private Function CrearUpdateCommand3() As SqlClient.SqlCommand
            'Solo actuliza el porcentaje de prorrateo
            Try

                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_TB_UpdOrden)
                Dim param As SqlClient.SqlParameter

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters

                    '.Add(mc_strArroba & mc_strOrden, SqlDbType.VarChar, 50, mc_strOrden)

                    param = .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    param.SourceVersion = DataRowVersion.Original

                    .Add(mc_strArroba & mc_NoVehiculo, SqlDbType.Int, 9, mc_NoVehiculo)

                    .Add(mc_strArroba & mc_intTipoOrden, SqlDbType.Int, 4, mc_intTipoOrden)

                    .Add(mc_strArroba & mc_intNoVisita, SqlDbType.Int, 4, mc_intNoVisita)

                    .Add(mc_strArroba & mc_DatFechaApertura, SqlDbType.DateTime, 8, mc_DatFechaApertura)

                    '.Add(mc_strArroba & mc_intPaneles, SqlDbType.Decimal, 9, mc_intPaneles)

                    .Add(mc_strArroba & mc_strObservacion, SqlDbType.VarChar, 1000, mc_strObservacion)

                    '.Add(mc_strArroba & mc_strPrioridad, SqlDbType.VarChar, 50, mc_strPrioridad)

                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.Int, 9, mc_strCodMarca)

                    '.Add(mc_strArroba & mc_intTiempoAprobado, SqlDbType.Int, 4, mc_intTiempoAprobado)

                    '.Add(mc_strArroba & mc_intProrrateo, SqlDbType.Int, 4, mc_intProrrateo)

                    '.Add(mc_strArroba & mc_decPorcentaje, SqlDbType.Decimal, 15, mc_decPorcentaje)

                    .Add(mc_strArroba & mc_strCodModelo, SqlDbType.Decimal, 9, mc_strCodModelo)

                    .Add(mc_strArroba & mc_strPlaca, SqlDbType.VarChar, 50, mc_strPlaca)

                    '.Add(mc_strArroba & mc_decMontoManoObra, SqlDbType.Decimal, 9, mc_decMontoManoObra)

                    '.Add(mc_strArroba & mc_decMontoMaterialesPintura, SqlDbType.Decimal, 9, mc_decMontoMaterialesPintura)

                    '.Add(mc_strArroba & mc_decMontoMaterialesTaller, SqlDbType.Decimal, 9, mc_decMontoMaterialesTaller)

                    '.Add(mc_strArroba & mc_decMontoSuministros, SqlDbType.Decimal, 9, mc_decMontoSuministros)

                    '.Add(mc_strArroba & mc_decValorReal, SqlDbType.Decimal, 9, mc_decValorReal)

                    '.Add(mc_strArroba & mc_strIndCompra, SqlDbType.Int, 4, mc_strIndCompra)

                    '.Add(mc_strArroba & mc_intIndAsegurada, SqlDbType.Int, 4, mc_intIndAsegurada)

                    '.Add(mc_strArroba & mc_strNoOrdenRef, SqlDbType.VarChar, 50, mc_strNoOrdenRef)


                End With

                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearActualizarCommand() As SqlClient.SqlCommand
            'Solo actuliza el porcentaje de prorrateo
            Try

                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDOrdenTrabajoSimple)

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                    .Add(mc_strArroba & mc_strEstado, SqlDbType.VarChar, 50, mc_strEstado)

                    .Add(mc_strArroba & mc_DatFechaCompromiso, SqlDbType.SmallDateTime, 8, mc_DatFechaCompromiso)

                    .Add(mc_strArroba & mc_DatFechaCierre, SqlDbType.SmallDateTime, 8, mc_DatFechaCierre)

                    .Add(mc_strArroba & mc_strObservacion, SqlDbType.VarChar, 1000, mc_strObservacion)

                    .Add(mc_strArroba & "CodTecnico", SqlDbType.Int, 4000, "CodTecnico")

                    .Add(mc_strArroba & mc_strIDEstadoWeb, SqlDbType.Int, 4, mc_strIDEstadoWeb)

                End With

                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CmdOrdenesProcesoYSuspendidas() As SqlClient.SqlCommand
            Try

                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelOrdenProcSusp)

                cmdUPD.CommandType = CommandType.StoredProcedure



                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        ''************Para documentos Draft*******************

        Public Function CreateSelectCommandOT(ByVal p_NoOrden As String) As SqlClient.SqlCommand


            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                'm_adpOrden.SelectCommand.Connection = m_cnnSCGTaller
                Dim strConsulta As String = "SELECT SCGTA_TB_Orden.NoOrden, SCGTA_TB_RepuestosxOrden.ID, SCGTA_TB_RepuestosxOrden.NoRepuesto, SCGTA_TB_RepuestosxOrden.LineNum, " & _
                                            "SCGTA_TB_RepuestosxEstado.IdRepuestosxOrden, SCGTA_TB_RepuestosxEstado.CodEstadoRep, SCGTA_TB_RepuestosxEstado.Cantidad, SCGTA_TB_RepuestosxOrden.Adicional, SCGTA_TB_RepuestosxOrden.LineNumOriginal " & _
                                            "FROM  SCGTA_TB_Orden INNER JOIN " & _
                                            "SCGTA_TB_RepuestosxOrden ON SCGTA_TB_Orden.NoOrden = SCGTA_TB_RepuestosxOrden.NoOrden INNER JOIN " & _
                                            "SCGTA_TB_RepuestosxEstado ON SCGTA_TB_RepuestosxOrden.ID = SCGTA_TB_RepuestosxEstado.IdRepuestosxOrden " & _
                                            "WHERE SCGTA_TB_Orden.NoOrden = '" & p_NoOrden & "'"

                m_adpOrden.SelectCommand = m_cnnSCGTaller.CreateCommand

                '                Dim SelectCmd As SqlClient.SqlCommand = New SqlClient.SqlCommand("SELECT NoOrden, CodTipoOrden, NoVisita, Estado, NoCotizacion, ClienteFacturar, Asesor FROM SCGTA_TB_Orden WHERE NoOrden = '" & p_NoOrden & "'", m_cnnSCGTaller)
                Dim SelectCmd As SqlClient.SqlCommand = New SqlClient.SqlCommand(strConsulta, m_cnnSCGTaller)

                m_adpOrden.SelectCommand = SelectCmd

                CreateSelectCommandOT = SelectCmd


            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

       


#End Region

    End Class
End Namespace