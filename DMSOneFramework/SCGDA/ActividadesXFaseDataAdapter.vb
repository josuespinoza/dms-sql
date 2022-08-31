
Namespace SCGDataAccess

    Public Class ActividadesXFaseDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_intnoActividad As String = "NoActividad"
        Private Const mc_intNoFase As String = "NoFase"
        Private Const mc_strEstado As String = "Estado"
        Private Const mc_strAdicional As String = "Adicional"
        Private Const mc_strNoRepuesto As String = "NoRepuesto"
        Private Const mc_decNoPiezaPrincipal As String = "NoPiezaPrincipal"
        Private Const mc_decNoSeccion As String = "NoSeccion"
        Private Const mc_intAdicional As String = "Adicional"
        Private Const mc_intPrimaryKey As String = "PrimaryKey"
        Private Const mc_strFacturada As String = "Facturada"
        Private Const mc_intNoFactura As String = "NoFactura"
        Private Const mc_strId As String = "ID"
        Private Const mc_strDuracion As String = "Duracion"
        Private Const mc_strCadenaActividades As String = "cadena_actividades"
        Private Const mc_strSCGTA_SP_UPDActividadXFase As String = "SCGTA_SP_UPDActividadXFase"


        Private Const mc_intNoSeccion As String = "NoSeccion"
        Private Const mc_intNoPiezaPrincipal As String = "NoPiezaPrincipal"
        'Private Const mc_intNoFase As String = "NoFase"
        Private Const mc_intNoRepuesto As String = "NoRepuesto"
        'Private Const mc_intNoActividad As String = "NoActividad"
        Private Const mc_intCantidad As String = "Cantidad"
        'Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_bitAdicional As String = "Adicional"
        Private Const mc_strDescripcion As String = "Descripcion"

        Private Const mc_strLineNum As String = "LineNum"
        Private Const mc_strLineNumFather As String = "LineNumFather"
        Private Const mc_intNoAdicional As String = "NoAdicional"
        Private Const mc_intCantidadPendiente As String = "CantidadPendiente"
        Private Const mc_dtFecha_Solicitud As String = "Fecha_Solicitud"
        'Private Const mc_blnComprarRepuesto As String = "Comprar"
        Private Const mc_strComponente As String = "Componente"


        Private m_adpAct As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDAct As String = "SCGTA_SP_UpdActividadesXFase"
        Private Const mc_strSCGTA_SP_UPDActCantidad As String = "SCGTA_SP_UpdActividadXOrden"
        Private Const mc_strSCGTA_SP_UPDActEstado As String = "SCGTA_SP_UpdActividadesXFaseEstado"
        Private Const mc_strSCGTA_SP_SELAct As String = "SCGTA_SP_SELActividadesXFase"
        Private Const mc_strSCGTA_SP_SELActByFilters As String = "SCGTA_SP_SELActividadesXFaseByFilters"
        Private Const mc_strSCGTA_SP_SELActByFiltersFactura As String = "SCGTA_SP_SELActividadesXFaseFactura"
        Private Const mc_strSCGTA_SP_SELActByOrden As String = "SCGTA_SP_SELActividadesXFaseByOrden"
        Private Const mc_strSCGTA_SP_DelAct As String = "SCGTA_SP_DELActividadesXFase"
        Private Const mc_strSCGTA_SP_InsAct As String = "SCGTA_SP_INSActividadesXOrden"
        Private Const mc_strSCGTA_SP_DELActividadXOrden As String = "SCGTA_SP_DELActividadXOrden"
        Private Const mc_strSCGTA_SP_UPDFacturaActividad As String = "SGCTA_SP_UPDFactura_Actividades"
        Private Const mc_strSCGTA_SP_UPDReversarFactura As String = "SCGTA_SP_ReversaManoO_Factura"
        Private Const mc_strSCGTA_SP_InsActividadesXOrden As String = "SCGTA_SP_INSActividadesXOrden"
        Private Const mc_strSCGTA_SP_UPDCantidadActividadesXOrden As String = "SCGTA_SP_UPDCantidadActividadesXOrden"

        Private Const mc_strSCGTA_SP_SelActividadesXOrdenToReader As String = "SCGTA_SP_SelACtividadesXOrdenToReader"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

#End Region

#Region "Inicializa DataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpAct = New SqlClient.SqlDataAdapter
        End Sub

        Public Sub New(ByVal strCadenaConexion As String)

            Dim cnConexion As New SqlClient.SqlConnection(strCadenaConexion)

            m_cnnSCGTaller = cnConexion

            m_adpAct = New SqlClient.SqlDataAdapter

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

        Public Overloads Function Fill(ByVal dataSet As ActividadesXFaseDataset, ByVal decNoOrden As String, ByVal intFase As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpAct.SelectCommand = CrearSelectCommand()

                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = decNoOrden

                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = intFase


                m_adpAct.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Fill(dataSet.SCGTA_TB_ActividadesxOrden)


            Catch ex As Exception
                Throw ex
            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function FillbyFiltersFactura(ByVal dataSet As ActividadesXFaseDataset, ByVal decNoOrden As String, ByVal intFase As Integer, ByVal intAdicional As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpAct.SelectCommand = CrearSelectbyFiltersCommandFactura()

                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = decNoOrden

                If intFase = 0 Then
                    m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = DBNull.Value
                Else
                    m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = intFase
                End If

                If intAdicional = "" Then
                    m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strFacturada).Value = DBNull.Value
                Else
                    m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strFacturada).Value = intAdicional
                End If

                m_adpAct.SelectCommand.Connection = m_cnnSCGTaller

                dataSet.SCGTA_TB_ActividadesxOrden.CheckColumn.DefaultValue = 0



                Call m_adpAct.Fill(dataSet.SCGTA_TB_ActividadesxOrden)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function FillbyFilters(ByVal dataSet As ActividadesXFaseDataset, _
                                                ByVal decNoOrden As String, _
                                                ByVal intFase As Integer, _
                                                ByVal intAdicional As Integer, _
                                                Optional ByVal cn As SqlClient.SqlConnection = Nothing, _
                                                Optional ByVal tran As SqlClient.SqlTransaction = Nothing) As Integer

            Try

                If cn Is Nothing Then

                    If m_cnnSCGTaller.State = ConnectionState.Closed Then
                        If m_cnnSCGTaller.ConnectionString = "" Then
                            m_cnnSCGTaller.ConnectionString = strConexionADO
                        End If
                        m_cnnSCGTaller.Open()
                    End If
                End If

                m_adpAct.SelectCommand = CrearSelectbyFiltersCommand()

                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = decNoOrden

                If intFase = 0 Then
                    m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = DBNull.Value
                Else
                    m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = intFase
                End If

                If intAdicional = 1 Then
                    m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_intAdicional).Value = DBNull.Value
                Else
                    m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_intAdicional).Value = intAdicional
                End If

                If cn Is Nothing Then
                    m_adpAct.SelectCommand.Connection = m_cnnSCGTaller
                Else
                    m_adpAct.SelectCommand.Connection = cn
                    m_adpAct.SelectCommand.Transaction = tran
                End If

                dataSet.SCGTA_TB_ActividadesxOrden.CheckColumn.DefaultValue = 0

                Call m_adpAct.Fill(dataSet.SCGTA_TB_ActividadesxOrden)

            Catch ex As Exception

                Throw ex

            Finally
                If cn Is Nothing Then
                    Call m_cnnSCGTaller.Close()
                End If

            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As ActividadesXFaseDataset, ByVal decNoOrden As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpAct.SelectCommand = CrearSelectCommandByOrden()

                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = decNoOrden

                m_adpAct.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Fill(dataSet.SCGTA_TB_ActividadesxOrden)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Function Delete(ByVal dataset As ActividadesXFaseDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.UpdateCommand = CrearDeleteCommand()
                m_adpAct.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Update(dataset.SCGTA_TB_ActividadesxOrden)

            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try
        End Function

        Public Overloads Function Update(ByVal table As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable) As String


            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.InsertCommand = CreateInsertCommand()
                m_adpAct.InsertCommand.Connection = m_cnnSCGTaller

                m_adpAct.UpdateCommand = CrearUpdateCommand()
                m_adpAct.UpdateCommand.Connection = m_cnnSCGTaller
                m_adpAct.DeleteCommand = CrearUpdateEliminarCommand()
                m_adpAct.DeleteCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Update(table)


            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try
            Return String.Empty
        End Function

        'Public Overloads Function Update(ByVal table As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable) As String


        '    Try
        '        If m_cnnSCGTaller.State = ConnectionState.Closed Then
        '            If m_cnnSCGTaller.ConnectionString = "" Then
        '                m_cnnSCGTaller.ConnectionString = strConexionADO
        '            End If
        '            Call m_cnnSCGTaller.Open()
        '        End If

        '        m_adpAct.InsertCommand = CreateInsertCommand()
        '        m_adpAct.InsertCommand.Connection = m_cnnSCGTaller

        '        m_adpAct.UpdateCommand = CrearUpdateCommand()
        '        m_adpAct.UpdateCommand.Connection = m_cnnSCGTaller
        '        m_adpAct.DeleteCommand = CrearUpdateEliminarCommand()
        '        m_adpAct.DeleteCommand.Connection = m_cnnSCGTaller

        '        Call m_adpAct.Update(table)


        '    Catch ex As Exception

        '        Throw ex
        '    Finally
        '        m_cnnSCGTaller.Close()
        '    End Try

        'End Function

        Public Overloads Function Update(ByVal table As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable, _
                                         ByRef cn As SqlClient.SqlConnection, _
                                         ByRef tran As SqlClient.SqlTransaction, _
                                         Optional ByVal blnIniciar As Boolean = False, _
                                         Optional ByVal blnTerminar As Boolean = False, _
                                         Optional ByVal blnActualizarCantidad As Boolean = False) As String


            Try
                If blnIniciar Then
                    cn = New SqlClient.SqlConnection
                    If m_cnnSCGTaller.State = ConnectionState.Closed Then
                        If m_cnnSCGTaller.ConnectionString = "" Then
                            m_cnnSCGTaller.ConnectionString = strConexionADO
                        End If
                        Call m_cnnSCGTaller.Open()
                        cn = m_cnnSCGTaller
                        tran = cn.BeginTransaction(IsolationLevel.ReadCommitted)
                    Else
                        cn = m_cnnSCGTaller
                        tran = cn.BeginTransaction(IsolationLevel.ReadCommitted)
                    End If
                End If

                m_adpAct.InsertCommand = CreateInsertCommand()
                m_adpAct.InsertCommand.Connection = cn
                m_adpAct.InsertCommand.Transaction = tran
                If blnActualizarCantidad Then
                    m_adpAct.UpdateCommand = CrearUpdateCommandCantidades()
                Else
                    m_adpAct.UpdateCommand = CrearUpdateCommand()
                End If
                m_adpAct.UpdateCommand.Connection = cn
                m_adpAct.UpdateCommand.Transaction = tran
                m_adpAct.DeleteCommand = CrearUpdateEliminarCommand()
                m_adpAct.DeleteCommand.Connection = cn
                m_adpAct.DeleteCommand.Transaction = tran

                Call m_adpAct.Update(table)
                If blnTerminar Then
                    tran.Commit()
                End If


            Catch ex As Exception

                Throw ex
            Finally
                'm_cnnSCGTaller.Close()
            End Try
            Return String.Empty
        End Function

        Public Overloads Function Update(ByVal dataSet As ActividadesXFaseDataset) As String


            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.InsertCommand = CreateInsertCommandActividades(0)
                m_adpAct.InsertCommand.Connection = m_cnnSCGTaller

                m_adpAct.UpdateCommand = CrearUpdateCommand()
                m_adpAct.UpdateCommand.Connection = m_cnnSCGTaller
                m_adpAct.DeleteCommand = CrearUpdateEliminarCommand()
                m_adpAct.DeleteCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Update(dataSet.SCGTA_TB_ActividadesxOrden)


            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try
            Return String.Empty
        End Function

        Public Overloads Function Update(ByVal p_DataTable As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable, ByVal strEstado As String) As String

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.UpdateCommand = CrearUpdateEstadoCommand()

                With m_adpAct.UpdateCommand
                    .Parameters(mc_strArroba & mc_strEstado).Value = strEstado
                End With

                m_adpAct.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Update(p_DataTable)

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

            Return String.Empty

        End Function

        Public Overloads Function UpdateEliminar(ByVal p_DataTable As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable) As String

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.DeleteCommand = CrearUpdateEliminarCommand()

                m_adpAct.DeleteCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Update(p_DataTable)

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try
            Return String.Empty
        End Function

        'Public Overloads Function UpdateEliminar(ByVal p_DataTable As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable, _
        '                                          Byval optional p_blnIniciarTransaccion as Boolean = false, _
        '                                          Byref Optional p_tnTransaccion as SqlClient.SqlTransaction, _
        '                                          Byref Optional p_cnConeccion as SqlClient.SqlConnection) As String

        '    Try

        '        If m_cnnSCGTaller.State = ConnectionState.Closed Then
        '            If m_cnnSCGTaller.ConnectionString = "" Then
        '                m_cnnSCGTaller.ConnectionString = strConexionADO
        '            End If
        '            Call m_cnnSCGTaller.Open()
        '        End If

        '        m_adpAct.DeleteCommand = CrearUpdateEliminarCommand()

        '        m_adpAct.DeleteCommand.Connection = m_cnnSCGTaller

        '        Call m_adpAct.Update(p_DataTable)

        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        m_cnnSCGTaller.Close()
        '    End Try

        'End Function

        Public Overloads Function UpdateEliminarActividadesxOrden(ByRef Dataset As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable) As String

            Dim m_trn As SqlClient.SqlTransaction =  Nothing
            Try


                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_trn = m_cnnSCGTaller.BeginTransaction

                m_adpAct.DeleteCommand = CrearUpdateEliminarCommand()
                m_adpAct.DeleteCommand.Connection = m_cnnSCGTaller
                m_adpAct.DeleteCommand.Transaction = m_trn

                Call m_adpAct.Update(Dataset)
                m_trn.Commit()

            Catch ex As Exception
                If Not m_trn Is Nothing Then
                    Call m_trn.Rollback()
                End If
            Finally
                Call m_cnnSCGTaller.Close()
                If Not m_trn Is Nothing Then
                    Call m_trn.Dispose()
                    m_trn = Nothing
                End If
            End Try
            Return String.Empty
        End Function

        Public sub UpdateTiempoEstandarActividades(ByVal p_strNoOrden As String, _
                                ByVal p_dblTiempo As Double, ByVal p_intIdActividad As Integer)

            Dim cmdActXFase As SqlClient.SqlCommand

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                cmdActXFase = New SqlClient.SqlCommand

                With cmdActXFase
                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = mc_strSCGTA_SP_UPDActividadXFase

                    .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden

                    With .Parameters.Add(mc_strArroba & mc_strDuracion, SqlDbType.Decimal)
                        .Precision = 15
                        .Scale = 2
                        .Value = p_dblTiempo
                    End With

                    .Parameters.Add(mc_strArroba & mc_strId, SqlDbType.Int).Value = p_intIdActividad

                End With

                cmdActXFase.ExecuteNonQuery()

            Catch ex As Exception

                Throw ex

            Finally
                m_cnnSCGTaller.Close()
            End Try

        End sub

        Public Function Inserta(ByVal dataset As ActividadesXFaseDataset,
                                ByRef Transaction As SqlClient.SqlTransaction,
                                ByRef Conexion As SqlClient.SqlConnection) As String

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.InsertCommand = CreateInsertCommand()
                m_adpAct.InsertCommand.Transaction = Transaction
                m_adpAct.InsertCommand.Connection = Conexion

                Call m_adpAct.Update(dataset.SCGTA_TB_ActividadesxOrden)

                'llama al rollback afuera
            Catch ex As Exception
                Throw
            End Try
            Return String.Empty
        End Function

        Public Function Actualizar(ByVal CadenaActividades As String, ByVal intFactura As Integer) As Integer

'            Dim nombre As String
            Dim RNombre As SqlClient.SqlDataReader

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpAct.UpdateCommand = UpdateCommandActividades()

                If CadenaActividades = "" Then
                    m_adpAct.UpdateCommand.Parameters(mc_strArroba & mc_strCadenaActividades).Value = System.DBNull.Value
                Else
                    m_adpAct.UpdateCommand.Parameters(mc_strArroba & mc_strCadenaActividades).Value = CadenaActividades
                End If

                If intFactura = 0 Then
                    m_adpAct.UpdateCommand.Parameters(mc_strArroba & mc_intNoFactura).Value = System.DBNull.Value
                Else
                    m_adpAct.UpdateCommand.Parameters(mc_strArroba & mc_intNoFactura).Value = intFactura
                End If


                m_adpAct.UpdateCommand.Connection = m_cnnSCGTaller
                RNombre = m_adpAct.UpdateCommand.ExecuteReader

                If RNombre.Read Then
                    Return 1
                Else
                    Return 0
                End If


            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
        End Function

        Public Function Reversar(ByVal intFactura As Integer) As Integer

'            Dim nombre As String
            Dim RNombre As SqlClient.SqlDataReader

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpAct.UpdateCommand = UpdateCommandReversarFactura()


                If intFactura = 0 Then
                    m_adpAct.UpdateCommand.Parameters(mc_strArroba & mc_intNoFactura).Value = System.DBNull.Value
                Else
                    m_adpAct.UpdateCommand.Parameters(mc_strArroba & mc_intNoFactura).Value = intFactura
                End If


                m_adpAct.UpdateCommand.Connection = m_cnnSCGTaller
                RNombre = m_adpAct.UpdateCommand.ExecuteReader

                If RNombre.Read Then
                    Return 1
                Else
                    Return 0
                End If


            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
        End Function

        Public Function GetActividadesByNoFaseToReader(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer) As SqlClient.SqlDataReader
            Dim drdActividades As SqlClient.SqlDataReader =  Nothing
            Dim cmdActividades As SqlClient.SqlCommand

            Try

                cmdActividades = New SqlClient.SqlCommand(mc_strSCGTA_SP_SelActividadesXOrdenToReader, m_cnnSCGTaller)

                With cmdActividades

                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden
                    .Parameters.Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 4).Value = p_intNoFase

                    drdActividades = .ExecuteReader(CommandBehavior.CloseConnection)

                End With

                Return drdActividades

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ActualizarCantidadActXOrden(ByVal p_strNoOrden As String, ByVal p_strNoActividad As String, _
                ByVal p_intLineNum As Integer, ByVal p_intNoFase As Integer, ByVal p_dblDuracion As Double, _
                ByVal p_dblCantidad As Double) As Integer

            Dim cmdActividadesXOrden As New SqlClient.SqlCommand
            Dim intResult As Integer = 0

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If

                    m_cnnSCGTaller.Open()

                End If

                With cmdActividadesXOrden

                    .CommandText = mc_strSCGTA_SP_UPDCantidadActividadesXOrden
                    .CommandType = CommandType.StoredProcedure
                    .Connection = m_cnnSCGTaller

                    .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden
                    .Parameters.Add(mc_strArroba & mc_intnoActividad, SqlDbType.VarChar, 50).Value = p_strNoActividad
                    .Parameters.Add(mc_strArroba & mc_strLineNum, SqlDbType.Int).Value = p_intLineNum
                    .Parameters.Add(mc_strArroba & mc_intNoFase, SqlDbType.Int).Value = p_intNoFase

                    With .Parameters.Add(mc_strArroba & mc_strDuracion, SqlDbType.Decimal)
                        .Precision = 15
                        .Scale = 2
                        .Value = p_dblDuracion
                    End With

                    .Parameters.Add(mc_strArroba & mc_intCantidad, SqlDbType.Float).Value = p_dblCantidad

                End With

                intResult = cmdActividadesXOrden.ExecuteNonQuery

                Return intResult

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try
        End Function

#End Region

#Region "Creación de comandos"

        Private Function UpdateCommandActividades() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDFacturaActividad)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strCadenaActividades, SqlDbType.VarChar, 250, mc_strCadenaActividades)

                    .Add(mc_strArroba & mc_intNoFactura, SqlDbType.Int, 4, mc_intNoFactura)
                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function UpdateCommandReversarFactura() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDReversarFactura)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_intNoFactura, SqlDbType.Int, 4, mc_intNoFactura)
                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELAct)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 5, mc_intNoFase)

            End With

            Return cmdSel



        End Function

        Private Function CrearSelectbyFiltersCommand() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELActByFilters)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 5, mc_intNoFase)
                .Add(mc_strArroba & mc_intAdicional, SqlDbType.Int, 5, mc_intAdicional)

            End With

            Return cmdSel

        End Function

        Private Function CrearSelectbyFiltersCommandFactura() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELActByFiltersFactura)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 5, mc_intNoFase)
                .Add(mc_strArroba & mc_strFacturada, SqlDbType.VarChar, 50, mc_strFacturada)

            End With

            Return cmdSel

        End Function

        Private Function CrearSelectCommandByOrden() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELActByOrden)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

            End With

            Return cmdSel

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDAct)

            cmdUPD.CommandType = CommandType.StoredProcedure

            With cmdUPD.Parameters


                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 4, mc_intNoFase)

                .Add(mc_strArroba & mc_strEstado, SqlDbType.VarChar, 100, mc_strEstado)

                .Add(mc_strArroba & mc_intnoActividad, SqlDbType.Int, 5, mc_intnoActividad)

            End With

            Return cmdUPD

        End Function

        Private Function CrearUpdateCommandCantidades() As SqlClient.SqlCommand

            Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDActCantidad)

            cmdUPD.CommandType = CommandType.StoredProcedure

            With cmdUPD.Parameters


                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                .Add(mc_strArroba & mc_intnoActividad, SqlDbType.NVarChar, 20, mc_intnoActividad)

                .Add(mc_strArroba & mc_intCantidad, SqlDbType.Float, 4, mc_intCantidad)

                .Add(mc_strArroba & mc_strLineNum, SqlDbType.Int, 4, mc_strLineNum)

                .Add(mc_strArroba & mc_strLineNumFather, SqlDbType.Int, 4, mc_strLineNumFather)

                .Add(mc_strArroba & mc_strId, SqlDbType.Int, 4, mc_strId)



            End With

            Return cmdUPD

        End Function

        Private Function CrearUpdateEstadoCommand() As SqlClient.SqlCommand

            Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDActEstado)

            cmdUPD.CommandType = CommandType.StoredProcedure

            With cmdUPD.Parameters

                .Add(mc_strArroba & mc_strEstado, SqlDbType.VarChar, 100)
                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 4, mc_intNoFase)
                .Add(mc_strArroba & mc_intnoActividad, SqlDbType.Int, 5, mc_intnoActividad)

            End With

            Return cmdUPD

        End Function

        Private Function CrearUpdateEliminarCommand() As SqlClient.SqlCommand

            Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_DELActividadXOrden)

            cmdUPD.CommandType = CommandType.StoredProcedure

            With cmdUPD.Parameters

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 4, mc_intNoFase)
                .Add(mc_strArroba & mc_strId, SqlDbType.Int, 5, mc_strId)
                .Add(mc_strArroba & mc_strDuracion, SqlDbType.Decimal, 15, mc_strDuracion)

            End With

            Return cmdUPD

        End Function

        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelAct)

            cmdUPD.CommandType = CommandType.StoredProcedure

            With cmdUPD.Parameters


                .Add(mc_strArroba & mc_intPrimaryKey, SqlDbType.Int, 4, mc_intPrimaryKey)

            End With

            Return cmdUPD

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsAct)

            cmdIns.CommandType = CommandType.StoredProcedure

            With cmdIns.Parameters

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 4, mc_intNoFase)

                .Add(mc_strArroba & mc_intnoActividad, SqlDbType.VarChar, 50, mc_intnoActividad)

                .Add(mc_strArroba & mc_intAdicional, SqlDbType.Int, 5, mc_intAdicional)

                .Add(mc_strArroba & mc_strDuracion, SqlDbType.Decimal, 9, mc_strDuracion)

                .Add(mc_strArroba & mc_strLineNum, SqlDbType.Int, 4, mc_strLineNum)

                .Add(mc_strArroba & mc_intCantidad, SqlDbType.Float, 9, mc_intCantidad)

                .Add(mc_strArroba & mc_strId, SqlDbType.Int, 4, mc_strId).Direction = ParameterDirection.Output

                .Add(mc_strArroba & mc_strLineNumFather, SqlDbType.Int, 4, mc_strLineNumFather)

            End With

            Return cmdIns

        End Function

        Private Function CreateInsertCommandActividades(ByVal intNoAdicional As Integer) As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsActividadesXOrden)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                    .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 4, mc_intNoFase)

                    .Add(mc_strArroba & mc_bitAdicional, SqlDbType.Bit, 1, mc_bitAdicional)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 500, mc_strDescripcion)

                    'se el agrega el valor que se trae por parametro. 
                    'Importante: no tiene columna origen.
                    .Add(mc_strArroba & mc_intNoAdicional, _
                           SqlDbType.Int).Value = intNoAdicional

                    'fecha de la solicitud
                    .Add(mc_strArroba & mc_dtFecha_Solicitud, SqlDbType.DateTime, 8, mc_dtFecha_Solicitud)

                    'Valores que referencian a la tabla repuestos
                    .Add(mc_strArroba & mc_intNoRepuesto, SqlDbType.Int, 4, mc_intNoRepuesto)
                    .Add(mc_strArroba & mc_intNoPiezaPrincipal, SqlDbType.Int, 4, mc_intNoPiezaPrincipal)
                    .Add(mc_strArroba & mc_intNoSeccion, SqlDbType.Int, 5, mc_intNoSeccion)


                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

    End Class

End Namespace
