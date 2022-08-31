Imports DMSOneFramework.SCGDataAccess.DAConexion

Namespace SCGDataAccess

    Public Class FaseXOrdenEstadosDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private m_adpFaseXOrdenEstados As SqlClient.SqlDataAdapter
        Private m_cnnSCGTaller As SqlClient.SqlConnection
        Private objDAConexion As DAConexion

#Region "Constantes"

        Private Const mc_strArroba As String = "@"

        ''Fields
        Private Const m_strNoFase As String = "NoFase"
        Private Const m_strNoOrden As String = "NoOrden"

        ''Stores Procedures
        Private Const m_strSelectFaseXOrdeBYKey As String = "SCGTA_SP_SELFaseXOrdenEstados"
        Private Const m_strSCGTA_SP_UPDIniciaFase As String = "SCGTA_SP_UPDIniciaFase"
        Private Const m_strSCGTA_SP_UPDSuspendeFaseEstado As String = "SCGTA_SP_UPDSuspendeFaseEstado"
        Private Const m_strSCGTA_SP_UPDFinalizaFaseEstado As String = "SCGTA_SP_UPDFinalizaFaseEstado"
        Private Const m_strSCGTA_SP_SELVerificarOrdenXSuspender As String = "SCGTA_SP_SELVerificarOrdenXSuspender"
        Private Const m_strSCGTA_SP_FechaFinSuspension As String = "SCGTA_SP_FinSuspensionXFase"
        Private Const m_strSCGTA_SP_UPDCostoPanel As String = "SCGTA_SP_UPDCostoPromedioPanel"
        Private Const m_strSCGTA_SP_INSRechazo As String = "SCGTA_SP_INSRechazo"
        Private Const m_strSCGTA_SP_SELEsEstadoFaseRechazo As String = "SCGTA_SP_SELEsEstadoFaseRechazada"

#End Region

#End Region

#Region "Constructor"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpFaseXOrdenEstados = New SqlClient.SqlDataAdapter

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

        Public Overloads Function Fill(ByRef p_Dataset As FaseXOrdenEstadosDataset, ByVal p_intNoFase As Integer, ByVal p_strNoOrden As String) As Integer

            m_adpFaseXOrdenEstados.SelectCommand = CrearSelectByKeyCommand()

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpFaseXOrdenEstados.SelectCommand.Parameters(mc_strArroba & m_strNoFase).Value = p_intNoFase
                m_adpFaseXOrdenEstados.SelectCommand.Parameters(mc_strArroba & m_strNoOrden).Value = p_strNoOrden

                m_adpFaseXOrdenEstados.SelectCommand.Connection = m_cnnSCGTaller

                m_adpFaseXOrdenEstados.Fill(p_Dataset.SCGTA_TB_FasesxOrden_Estados)

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function IniciarFase(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer) As Integer
            Dim objSqlCommand As SqlClient.SqlCommand
            Dim intUpdateResult As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                objSqlCommand = New SqlClient.SqlCommand(m_strSCGTA_SP_UPDIniciaFase, m_cnnSCGTaller)
                objSqlCommand.CommandType = CommandType.StoredProcedure

                objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoFase, SqlDbType.Int)).Value = p_intNoFase
                objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden

                intUpdateResult = objSqlCommand.ExecuteNonQuery()

                Return intUpdateResult

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try
        End Function

        Public Overloads Function SuspenderFase(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer, _
                                                      ByRef p_cnConeccion As SqlClient.SqlConnection, ByRef p_tnnTransaccion As SqlClient.SqlTransaction) As Integer
            Dim objSqlCommand As SqlClient.SqlCommand

            Try

                If p_cnConeccion.State = ConnectionState.Closed Then
                    If p_cnConeccion.ConnectionString = "" Then
                        p_cnConeccion.ConnectionString = strConexionADO
                    End If
                    p_cnConeccion.Open()
                    p_tnnTransaccion = p_cnConeccion.BeginTransaction
                End If

                objSqlCommand = New SqlClient.SqlCommand(m_strSCGTA_SP_UPDSuspendeFaseEstado, p_cnConeccion, p_tnnTransaccion)
                objSqlCommand.CommandType = CommandType.StoredProcedure

                objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoFase, SqlDbType.Int)).Value = p_intNoFase
                objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden

                objSqlCommand.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex
            Finally
                'm_cnnSCGTaller.Close()
            End Try
        End Function

        Public Overloads Function SuspenderFase(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer) As Integer
            Dim objSqlCommand As SqlClient.SqlCommand

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                objSqlCommand = New SqlClient.SqlCommand(m_strSCGTA_SP_UPDSuspendeFaseEstado, m_cnnSCGTaller)
                objSqlCommand.CommandType = CommandType.StoredProcedure

                objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoFase, SqlDbType.Int)).Value = p_intNoFase
                objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden

                objSqlCommand.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try
        End Function

        Public Overloads Function FinalizarFase(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer) As Integer
            Dim objSqlCommand As SqlClient.SqlCommand

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                objSqlCommand = New SqlClient.SqlCommand(m_strSCGTA_SP_UPDFinalizaFaseEstado, m_cnnSCGTaller)
                objSqlCommand.CommandType = CommandType.StoredProcedure

                objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoFase, SqlDbType.Int)).Value = p_intNoFase
                objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden

                objSqlCommand.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try
        End Function

        Public Overloads Function FinalizarFase(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer, _
                                                ByRef p_cnConeccion As SqlClient.SqlConnection, ByRef p_tnnTransacion As SqlClient.SqlTransaction) As Integer
            Dim objSqlCommand As SqlClient.SqlCommand

            Try

                If p_cnConeccion.State = ConnectionState.Closed Then
                    If p_cnConeccion.ConnectionString = "" Then
                        p_cnConeccion.ConnectionString = strConexionADO
                    End If
                    p_cnConeccion.Open()
                    p_tnnTransacion = p_cnConeccion.BeginTransaction
                End If

                objSqlCommand = New SqlClient.SqlCommand(m_strSCGTA_SP_UPDFinalizaFaseEstado, p_cnConeccion, p_tnnTransacion)
                objSqlCommand.CommandType = CommandType.StoredProcedure

                objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoFase, SqlDbType.Int)).Value = p_intNoFase
                objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden

                objSqlCommand.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex
            Finally
                'm_cnnSCGTaller.Close()
            End Try
        End Function

        Public Overloads Function VerificarOrdenXSuspender(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer) As Boolean
            Dim objSqlCommand As SqlClient.SqlCommand
            Dim objSQLReader As SqlClient.SqlDataReader
            Dim intFases As Integer
            Dim intSuspendidas As Integer
            Dim blnResult As Boolean

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                objSqlCommand = New SqlClient.SqlCommand(m_strSCGTA_SP_SELVerificarOrdenXSuspender, m_cnnSCGTaller)
                objSqlCommand.CommandType = CommandType.StoredProcedure

                objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoFase, SqlDbType.Int)).Value = p_intNoFase
                objSqlCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden

                objSQLReader = objSqlCommand.ExecuteReader()

                If objSQLReader.Read Then
                    intFases = objSQLReader.Item("Fases")
                    intSuspendidas = objSQLReader.Item("Suspendidas")
                End If

                If intFases = intSuspendidas Then
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

        Public Overloads Function Update(ByVal dataSet As OrdenDataset) As String

            ''este string devuelve el numero de orden que se creo en la Base de Datos
            'Dim strNoOrden As String

            'Try

            '    If m_cnnSCGTaller.State = ConnectionState.Closed Then
            '        If m_cnnSCGTaller.ConnectionString = "" Then
            '            m_cnnSCGTaller.ConnectionString = strConexionADO
            '        End If
            '        m_cnnSCGTaller.Open()
            '    End If

            '    m_adpOrden.UpdateCommand = CrearUpdateCommand()
            '    m_adpOrden.UpdateCommand.Connection = m_cnnSCGTaller

            '    Call m_adpOrden.Update(dataSet.SCGTA_TB_Orden)

            '    strNoOrden = m_adpOrden.InsertCommand.Parameters(mc_strArroba & mc_strOrden).Value

            '    If strNoOrden.Length > 0 Then
            '        Update = strNoOrden
            '    Else
            '        Update = ""
            '    End If

            'Catch ex As Exception
            '    Throw ex
            'Finally
            '    Call m_cnnSCGTaller.Close()
            'End Try
             Throw New NotImplementedException()
        End Function

        Public Overloads Function Actualizar(ByVal dataSet As OrdenTrabajoDataset) As String

            'Try

            '    If m_cnnSCGTaller.State = ConnectionState.Closed Then
            '        If m_cnnSCGTaller.ConnectionString = "" Then
            '            m_cnnSCGTaller.ConnectionString = strConexionADO
            '        End If
            '        m_cnnSCGTaller.Open()
            '    End If

            '    m_adpOrden.UpdateCommand = CrearActualizarCommand()

            '    m_adpOrden.UpdateCommand.Connection = m_cnnSCGTaller

            '    Call m_adpOrden.Update(dataSet.SCGTA_TB_Orden)

            'Catch ex As Exception

            '    Throw ex

            'Finally

            '    Call m_cnnSCGTaller.Close()

            'End Try
            Return ""

        End Function

        Public Overloads Function Insert(ByVal dataSet As OrdenTrabajoDataset) As String

            'este string devuelve el numero de orden que se creo en la Base de Datos
            'Dim strNoOrden As String

            'Try

            '    If m_cnnSCGTaller.State = ConnectionState.Closed Then
            '        If m_cnnSCGTaller.ConnectionString = "" Then
            '            m_cnnSCGTaller.ConnectionString = strConexionADO
            '        End If
            '        m_cnnSCGTaller.Open()
            '    End If

            '    m_adpOrden.InsertCommand = CreateInsertCommand()
            '    m_adpOrden.InsertCommand.Connection = m_cnnSCGTaller

            '    Call m_adpOrden.Update(dataSet.SCGTA_TB_Orden)

            '    strNoOrden = m_adpOrden.InsertCommand.Parameters(mc_strArroba & mc_strOrden).Value

            '    'Se retorna el numero de orden en caso de la insercion un string vacio para el update.
            '    If strNoOrden.Length > 0 Then
            '        Insert = strNoOrden
            '    Else
            '        Insert = ""
            '    End If

            'Catch ex As Exception
            '    Throw ex
            'Finally
            '    Call m_cnnSCGTaller.Close()
            'End Try

             Throw New NotImplementedException()
        End Function

        Public Sub EstablecerFinSuspension(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer)
            'Agregado 29/06/06. Alejandra. Establece la fecha en que finaliza la suspensión en caso de que una fase
            'pase del estado Suspendida a Iniciada, o del estado Suspendida a Finalizada

            Dim objCommand As SqlClient.SqlCommand

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                objCommand = New SqlClient.SqlCommand(m_strSCGTA_SP_FechaFinSuspension, m_cnnSCGTaller)
                objCommand.CommandType = CommandType.StoredProcedure

                objCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoFase, SqlDbType.Int)).Value = p_intNoFase
                objCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden

                objCommand.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Sub

        Public Sub ActualizarCostoPromedioPanel(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer)
            'Agregado 08/08/06. Alejandra. Actualiza el Costo promedio por panel en TB_FasesXOrden cuando se 
            'finaliza una fase

            Dim objCommand As SqlClient.SqlCommand

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                objCommand = New SqlClient.SqlCommand(m_strSCGTA_SP_UPDCostoPanel, m_cnnSCGTaller)
                objCommand.CommandType = CommandType.StoredProcedure

                objCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoFase, SqlDbType.Int)).Value = p_intNoFase
                objCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden

                objCommand.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try
        End Sub

        Public Sub RechazarFase(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer)
            'Agregado 17/08/06. Alejandra. Agrega la fase rechazada a la  tabla Rechazos

            Dim objCommand As SqlClient.SqlCommand

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                objCommand = New SqlClient.SqlCommand(m_strSCGTA_SP_INSRechazo, m_cnnSCGTaller)
                objCommand.CommandType = CommandType.StoredProcedure

                objCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoFase, SqlDbType.Int, 4)).Value = p_intNoFase
                objCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden

                objCommand.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Sub

        Public Function EsEstadoFaseRechazo(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer) As Integer
            'Agregado 22/08/06. Alejandra. Determina si el ultimo estado de la fase es Rechazo, en caso afirmativo
            'devuelve un 1, de lo contrario devuelve un 0

            Dim objCommand As SqlClient.SqlCommand
            Dim intRechazo As Integer


            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                objCommand = New SqlClient.SqlCommand(m_strSCGTA_SP_SELEsEstadoFaseRechazo, m_cnnSCGTaller)
                objCommand.CommandType = CommandType.StoredProcedure

                objCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoFase, SqlDbType.Int, 4)).Value = p_intNoFase
                objCommand.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & m_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden

                intRechazo = objCommand.ExecuteScalar
                Return intRechazo

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

#End Region

#Region "Creación de comandos"

        Private Function CrearSelectByKeyCommand() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(m_strSelectFaseXOrdeBYKey)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                .Add(mc_strArroba & m_strNoOrden, SqlDbType.VarChar, 50, m_strNoOrden)
                .Add(mc_strArroba & m_strNoFase, SqlDbType.Int, 4, m_strNoFase)

            End With

            Return cmdSel

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            'Try

            '    Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSOrden)

            '    cmdIns.CommandType = CommandType.StoredProcedure

            '    With cmdIns.Parameters

            '        .Add(mc_strArroba & mc_strOrden, SqlDbType.VarChar, 50, mc_strOrden)

            '        .Item(mc_strArroba & mc_strOrden).Direction = ParameterDirection.Output

            '        .Add(mc_strArroba & mc_NoVehiculo, SqlDbType.Int, 9, mc_NoVehiculo)

            '        .Add(mc_strArroba & mc_intTipoOrden, SqlDbType.Int, 4, mc_intTipoOrden)

            '        .Add(mc_strArroba & mc_intNoExpediente, SqlDbType.Int, 4, mc_intNoExpediente)

            '        .Add(mc_strArroba & mc_DatFechaApertura, SqlDbType.DateTime, 8, mc_DatFechaApertura)

            '        .Add(mc_strArroba & mc_intPaneles, SqlDbType.Int, 4, mc_intPaneles)

            '        .Add(mc_strArroba & mc_strObservacion, SqlDbType.VarChar, 1000, mc_strObservacion)

            '        .Add(mc_strArroba & mc_strPrioridad, SqlDbType.VarChar, 50, mc_strPrioridad)

            '        .Add(mc_strArroba & mc_intCodMarca, SqlDbType.Int, 9, mc_intCodMarca)

            '        .Add(mc_strArroba & mc_intTiempoAprobado, SqlDbType.Int, 4, mc_intTiempoAprobado)

            '        .Add(mc_strArroba & mc_intProrrateo, SqlDbType.Int, 4, mc_intProrrateo)

            '        .Add(mc_strArroba & mc_decPorcentaje, SqlDbType.Decimal, 15, mc_decPorcentaje)

            '        .Add(mc_strArroba & mc_intCodModelo, SqlDbType.Decimal, 9, mc_intCodModelo)

            '        .Add(mc_strArroba & mc_strPlaca, SqlDbType.VarChar, 50, mc_strPlaca)

            '    End With

            '    Return cmdIns

            'Catch ex As Exception

            'End Try
            Throw New NotImplementedException()
        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            'Try

            '    Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDOrden)

            '    cmdUPD.CommandType = CommandType.StoredProcedure

            '    With cmdUPD.Parameters


            '        .Add(mc_strArroba & mc_strOrden, SqlDbType.VarChar, 50, mc_strOrden)
            '        .Item(mc_strArroba & mc_strOrden).Direction = ParameterDirection.Output

            '        .Add(mc_strArroba & mc_intTipoOrden, SqlDbType.Int, 4, mc_intTipoOrden)

            '        .Add(mc_strArroba & mc_intNoExpediente, SqlDbType.Int, 4, mc_intNoExpediente)

            '        .Add(mc_strArroba & mc_DatFechaApertura, SqlDbType.DateTime, 8, mc_DatFechaApertura)

            '        .Add(mc_strArroba & mc_intPaneles, SqlDbType.Int, 4, mc_intPaneles)

            '        .Add(mc_strArroba & mc_strObservacion, SqlDbType.VarChar, 500, mc_strObservacion)

            '        .Add(mc_strArroba & mc_strPrioridad, SqlDbType.VarChar, 50, mc_strPrioridad)


            '    End With

            '    Return cmdUPD

            'Catch ex As Exception

            'End Try

            Throw New NotImplementedException()
        End Function

#End Region

    End Class

End Namespace