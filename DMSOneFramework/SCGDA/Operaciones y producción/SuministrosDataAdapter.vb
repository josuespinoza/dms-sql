Imports System.Data.SqlClient
Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion

Namespace SCGDataAccess

    Public Class SuministrosDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

#Region "Constantes"
        Private Const mc_intcode As String = "ItemCode"
        Private Const mc_strdescripcion As String = "Dscription"
        Private Const mc_intcantidad As String = "Quantity"
        Private Const mc_fecha As String = "DocDate"
        Private Const mc_intmonto As String = "Monto"
        Private Const mc_intempid As String = "U_Empid"
        Private Const mc_intOT As String = "U_OT"
        Private Const mc_strnombre As String = "Nombre"
        Private Const mc_intfase As String = "U_T_fase"
        Private Const mc_strcentro As String = "CentroCosto"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_intNoCentroCosto As String = "NoCentroCosto"
        Private Const mc_strFacturada As String = "facturada"
        Private Const mc_strCadenasuministros As String = "cadena_suministros"
        Private Const mc_strFacturable As String = "u_factura"
        Private Const mc_strLineNum As String = "LineNum"
        Private Const mc_strLineNumFather As String = "LineNumFather"
        Private Const mc_intNoFactura As String = "NoFactura"
        Private Const mc_strAdicional As String = "Adicional"
        Private Const mc_strCantidad As String = "Cantidad"
        Private Const mc_strNoSuministro As String = "NoSuministro"
        Private Const mc_strID As String = "ID"
        Private Const mc_strBodega As String = "Bodega"

        'Declaracion de las constantes con el nombre de los procedimientos almacenados

        Private Const mc_strSCGTA_TB_DElSuministros As String = "SCGTA_TB_DElSuministros"
        Private Const mc_strSCGTA_SP_INSSuministrosxOrden As String = "SCGTA_SP_INSSuministrosxOrden"
        Private Const mc_strSCGTA_SP_SelSuministros As String = "SCGTA_SP_SelSuministrosxOrden"
        Private Const mc_strSCGTA_SP_SelSuministrosSalida As String = "SCGTA_SP_SelSuministrosSalida"
        Private Const mc_strSCGTA_SP_SelSuministrosEntrada As String = "SCGTA_SP_SelSuministrosEntrada"
        Private Const mc_strSCGTA_SP_SelSuministrosFull As String = "SCGTA_SP_SelSuministrosFull"
        Private Const mc_strSCGTA_SP_SelMontoIns As String = "SCGTA_SP_SELORDEN"
        Private Const mc_strSCGTA_SP_SelSuministrosFactura As String = "SCGTA_SP_SelSuministrosFactura"
        Private Const mc_strSCGTA_SP_UPDFacturaSuministro As String = "SCGTA_SP_UPDFactura_Suministros"
        Private Const mc_strSCGTA_SP_SelSuministrosFullFactura As String = "SCGTA_SP_SelSuministrosFullFactura"
        Private Const mc_strSCGTA_SP_SelSuministrosSalidaFacturables As String = "SCGTA_SP_SelSuministrosSalidaFacturables"
        Private Const mc_strSCGTA_SP_SelSuministrosEntradaFacturables As String = "SCGTA_SP_SelSuministrosEntradaFacturables"
        Private Const mc_strSCGTA_SP_UpdSuministroXOrden As String = "SCGTA_SP_UpdSuministroPorOrden"
        Private Const mc_strSCGTA_SP_UpdSuministroXOrdenBodega As String = "SCGTA_SP_UpdSuministroXOrdenBodega"


        'Cambio en la forma de actualizar los suministros provenientes de requisiciones, no utiliza querys adapter
        Private Const mc_strSCGTA_SP_ActualizaCantidadSuministroRequisiciones As String = "SCGTA_SP_ActualizaCantidadSuministroRequisiciones"

        Private Const mc_lineNumOr As String = "lineNumOr"
        Private Const mc_numOrden As String = "numOrden"
        Private Const mc_numSum As String = "numSum"
        Private Const mc_cantRecibida As String = "cantRecibida"

        Private m_cnn As SqlClient.SqlConnection

        Private Const mc_strTipoItemGenerico As String = "TipoItemGenerico"
        Private Const mc_strTipoItemNoGenerico As String = "TipoItemNoGenerico"

        Private Const mc_strItemNoProcesado As String = "NoProcesado"
        Private Const mc_strItemNoTrasladado As String = "NoTrasladado"
        Private Const mc_strItemTrasladado As String = "ItemTrasladado"
        Private Const mc_strItemPendienteTraslado As String = "PendienteTraslado"
        Private Const mc_strItemSinDescripcion As String = "SinDescripcion"

        '---------------------------------para documentos drafts--------------------------------------
        Private Const mc_strItemPendientBodega As String = "PendienteBodega"
        '---------------------------------------------------------------------------------------------

        Private m_strGenerico As String = My.Resources.ResourceFrameWork.Generico
        Private m_strNoGenerico As String = My.Resources.ResourceFrameWork.NoGenerico

        Private m_strNoProcesado As String = My.Resources.ResourceFrameWork.NoProcesado
        Private m_strNoTrasladado As String = My.Resources.ResourceFrameWork.NoTrasladado
        Private m_strTrasladado As String = My.Resources.ResourceFrameWork.Trasladado
        Private m_strPendienteTaslado As String = My.Resources.ResourceFrameWork.PendienteTraslado
        Private m_strSinDescripcion As String = My.Resources.ResourceFrameWork.SinDescripcion
        Private m_strPendienteBodega As String = My.Resources.ResourceFrameWork.PendienteBodega


        'Private m_adpOrden As SqlClient.SqlDataAdapter

        Private Const mc_strArroba As String = "@"

        Private m_adpAct As New SqlDataAdapter

#End Region

#Region "Variables"

        Private m_adpSuministros As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Dim objDAConexion As DAConexion

#End Region

#End Region

#Region "Inicializa DataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpSuministros = New SqlClient.SqlDataAdapter
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

#End Region

#Region "Implementaciones SCG"


        Public Overloads Function Fill(ByRef dataSet As SuministrosDataset, _
                                       ByVal ORDEN As String, _
                                       ByVal Nolinea As Integer, _
                                       ByVal adicional As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If
                m_adpSuministros = New SqlDataAdapter
                'Creacion del comando
                m_adpSuministros.SelectCommand = CrearSelectCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpSuministros.SelectCommand.CommandTimeout = 480
                m_adpSuministros.SelectCommand.Connection = m_cnnSCGTaller

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado
                If ORDEN = "" Then
                    m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = System.DBNull.Value
                Else
                    m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = ORDEN
                End If

                If Nolinea = -1 Then
                    m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strLineNum).Value = System.DBNull.Value
                Else
                    m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strLineNum).Value = Nolinea
                End If

                If adicional = -1 Then
                    m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strAdicional).Value = System.DBNull.Value
                Else
                    m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strAdicional).Value = adicional
                End If

                '''''
                m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strTipoItemNoGenerico).Value = m_strNoGenerico
                m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strTipoItemGenerico).Value = m_strGenerico

                m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strItemNoProcesado).Value = m_strNoProcesado
                m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strItemNoTrasladado).Value = m_strNoTrasladado
                m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strItemTrasladado).Value = m_strTrasladado
                m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strItemPendienteTraslado).Value = m_strPendienteTaslado
                m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strItemPendientBodega).Value = m_strPendienteBodega
                m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strItemSinDescripcion).Value = m_strSinDescripcion

                '''''

                m_adpSuministros.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpSuministros.Fill(dataSet.SCGTA_VW_Suministros)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Fill(ByRef dataSet As SuministrosDataset, _
                                               ByVal ORDEN As String, _
                                               ByVal Nolinea As Integer, _
                                               ByVal adicional As Integer, _
                                                ByVal cn As SqlClient.SqlConnection, _
                                                ByVal tran As SqlClient.SqlTransaction) As Integer

            Try

                m_adpSuministros = New SqlDataAdapter
                'Creacion del comando
                m_adpSuministros.SelectCommand = CrearSelectCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpSuministros.SelectCommand.CommandTimeout = 480
                m_adpSuministros.SelectCommand.Connection = cn
                m_adpSuministros.SelectCommand.Transaction = tran

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado
                If ORDEN = "" Then
                    m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = System.DBNull.Value
                Else
                    m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = ORDEN
                End If

                If Nolinea = -1 Then
                    m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strLineNum).Value = System.DBNull.Value
                Else
                    m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strLineNum).Value = Nolinea
                End If

                If adicional = -1 Then
                    m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strAdicional).Value = System.DBNull.Value
                Else
                    m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strAdicional).Value = adicional
                End If


                'm_adpSuministros.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpSuministros.Fill(dataSet.SCGTA_VW_Suministros)

            Catch ex As Exception

                Throw ex

                'Finally

                '    Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Sub CargarSuministros(ByRef dtsSuministros As SuministrosFullDataset, ByVal strOrden As String, ByVal intCentroCosto As Integer)
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                With m_adpSuministros

                    .SelectCommand = CrearCommandSuministros(strOrden, intCentroCosto)
                    'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                    .SelectCommand.CommandTimeout = 480
                    dtsSuministros.SCGTA_SP_SelSuministrosFull.CheckColumn.DefaultValue = 0


                    With .SelectCommand

                        .Connection = m_cnnSCGTaller
                        .CommandText = mc_strSCGTA_SP_SelSuministrosFull

                    End With

                End With

                m_adpSuministros.Fill(dtsSuministros.SCGTA_SP_SelSuministrosFull)

            Catch ex As Exception
                Throw ex

            Finally
                If Not m_cnnSCGTaller Is Nothing Then
                    m_cnnSCGTaller.Close()
                End If
            End Try
        End Sub


        Public Function UpdateSuministrosXEstadoRequisiciones( _
                                                  ByVal p_lineNumOr As Integer _
                                                 , ByVal p_numOrden As String _
                                                 , ByVal p_numSum As String _
                                                 , ByVal p_cantRecibida As Decimal)

            Dim cmdUpd As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    Call m_cnnSCGTaller.Open()
                End If

                cmdUpd = New SqlClient.SqlCommand(mc_strSCGTA_SP_ActualizaCantidadSuministroRequisiciones, m_cnnSCGTaller)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters
                    .AddWithValue(mc_strArroba & mc_lineNumOr, p_lineNumOr)
                    .AddWithValue(mc_strArroba & mc_numOrden, p_numOrden)
                    .AddWithValue(mc_strArroba & mc_numSum, p_numSum)
                    .AddWithValue(mc_strArroba & mc_cantRecibida, p_cantRecibida)

                End With

                cmdUpd.ExecuteNonQuery()


            Catch ex As SqlClient.SqlException
                MsgBox(ex.Message)
            Catch ex As Exception
                MsgBox(ex.Message)

            Finally

                Call m_cnnSCGTaller.Close()
            End Try
        End Function

        'Public Function CargarSuministrosFacturables(ByRef dtsSuministros As SuministrosFullDataset, ByVal strOrden As String, ByVal intCentroCosto As Integer, ByVal facturable As String) As SqlClient.SqlDataReader
        '    'Si facturable = 1 se muestran los articulos facturables, 
        '    'Si facturable = 2 se muestran los articulos no facturables
        '    Try

        '        If m_cnnSCGTaller.State = ConnectionState.Closed Then
        '            If m_cnnSCGTaller.ConnectionString = "" Then
        '                m_cnnSCGTaller.ConnectionString = strConexionADO
        '            End If
        '            m_cnnSCGTaller.Open()
        '        End If

        '        With m_adpSuministros

        '            .SelectCommand = CrearCommandSuministrosFacturables(strOrden, intCentroCosto, facturable)

        '            dtsSuministros.SCGTA_SP_SelSuministrosFull.CheckColumn.DefaultValue = 0


        '            With .SelectCommand

        '                .Connection = m_cnnSCGTaller
        '                .CommandText = mc_strSCGTA_SP_SelSuministrosFullFactura

        '            End With

        '        End With

        '        m_adpSuministros.Fill(dtsSuministros.SCGTA_SP_SelSuministrosFull)

        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        If Not m_cnnSCGTaller Is Nothing Then
        '            m_cnnSCGTaller.Close()
        '        End If
        '    End Try
        'End Function


        'Public Function CargarSuministrosFactura(ByRef dtsSuministros As SuministrosFullDataset, ByVal strOrden As String, ByVal intCentroCosto As Integer, ByVal strFactura As String) As SqlClient.SqlDataReader
        '    Try

        '        If m_cnnSCGTaller.State = ConnectionState.Closed Then
        '            If m_cnnSCGTaller.ConnectionString = "" Then
        '                m_cnnSCGTaller.ConnectionString = strConexionADO
        '            End If
        '            m_cnnSCGTaller.Open()
        '        End If

        '        With m_adpSuministros

        '            .SelectCommand = CrearCommandSuministrosF(strOrden, intCentroCosto, strFactura)

        '            dtsSuministros.SCGTA_SP_SelSuministrosFull.CheckColumn.DefaultValue = 0
        '            'dtsSuministros.SCGTA_SP_SelSuministrosFull.U_KitReparColumn.DefaultValue = 0

        '            With .SelectCommand

        '                .Connection = m_cnnSCGTaller
        '                .CommandText = mc_strSCGTA_SP_SelSuministrosFactura

        '            End With

        '        End With

        '        m_adpSuministros.Fill(dtsSuministros.SCGTA_SP_SelSuministrosFull)

        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        If Not m_cnnSCGTaller Is Nothing Then
        '            m_cnnSCGTaller.Close()
        '        End If
        '    End Try
        'End Function

        Public Function DevuelveMonto(ByVal p_strnoorden As String) As String

            Dim RNombre As SqlClient.SqlDataReader

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpSuministros.SelectCommand = CrearSelectCommandMonto()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpSuministros.SelectCommand.CommandTimeout = 480

                If p_strnoorden = "" Then
                    m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = System.DBNull.Value
                Else
                    m_adpSuministros.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = p_strnoorden
                End If


                m_adpSuministros.SelectCommand.Connection = m_cnnSCGTaller
                RNombre = m_adpSuministros.SelectCommand.ExecuteReader

                If RNombre.Read Then
                    If IsDBNull(RNombre("MontoSuministros")) Then
                        Return 0
                    Else
                        Return RNombre("MontoSuministros")
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

        Public Overloads Function Update(ByVal table As SuministrosDataset.SCGTA_VW_SuministrosDataTable, _
                                         ByRef cn As SqlClient.SqlConnection, _
                                         ByRef tran As SqlClient.SqlTransaction, _
                                         Optional ByVal blnIniciar As Boolean = False, _
                                         Optional ByVal blnTerminar As Boolean = False) As Integer

            Try

                If blnIniciar Then
                    If m_cnnSCGTaller.State = ConnectionState.Closed Then
                        If m_cnnSCGTaller.ConnectionString = "" Then
                            m_cnnSCGTaller.ConnectionString = strConectionString
                        End If
                        m_cnnSCGTaller.Open()
                        cn = m_cnnSCGTaller
                        tran = cn.BeginTransaction(IsolationLevel.ReadCommitted)
                    End If
                End If
                m_adpSuministros = New SqlDataAdapter
                m_adpSuministros.InsertCommand = CreateInsertCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpSuministros.InsertCommand.CommandTimeout = 480
                m_adpSuministros.InsertCommand.Connection = cn
                m_adpSuministros.InsertCommand.Transaction = tran
                m_adpSuministros.UpdateCommand = CrearUpdateCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpSuministros.UpdateCommand.CommandTimeout = 480
                m_adpSuministros.UpdateCommand.Connection = cn
                m_adpSuministros.UpdateCommand.Transaction = tran
                m_adpSuministros.DeleteCommand = CreateDeleteCommandSuministros()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpSuministros.DeleteCommand.CommandTimeout = 480
                m_adpSuministros.DeleteCommand.Connection = cn
                m_adpSuministros.DeleteCommand.Transaction = tran


                Call m_adpSuministros.Update(table)
                If blnTerminar Then
                    tran.Commit()
                End If

            Catch ex As Exception

                Throw ex
            Finally
                'm_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Update(ByVal table As SuministrosDataset.SCGTA_VW_SuministrosDataTable) As Integer

            Try


                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpSuministros = New SqlDataAdapter
                m_adpSuministros.InsertCommand = CreateInsertCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpSuministros.InsertCommand.CommandTimeout = 480
                m_adpSuministros.InsertCommand.Connection = m_cnnSCGTaller
                m_adpSuministros.UpdateCommand = CrearUpdateCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpSuministros.UpdateCommand.CommandTimeout = 480
                m_adpSuministros.InsertCommand.Connection = m_cnnSCGTaller
                Call m_adpSuministros.Update(table)


            Catch ex As Exception

                Throw ex

            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Sub UpdateBodega(ByRef p_dtbSuministros As DMSOneFramework.SuministrosDataset.SCGTA_VW_SuministrosDataTable)
            Dim cmmd As SqlClient.SqlCommand
            m_adpAct = New SqlClient.SqlDataAdapter
            Try

                cmmd = New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdSuministroXOrdenBodega, m_cnnSCGTaller)

                With cmmd
                    .CommandType = CommandType.StoredProcedure
                    With .Parameters
                        .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID) '.Value = p_drwRepuesto.ID
                        .Add(mc_strArroba & mc_strBodega, SqlDbType.Bit, 1, mc_strBodega) '.Value = p_drwRepuesto.Bodega
                    End With
                End With
                m_adpAct.UpdateCommand = cmmd
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.UpdateCommand.CommandTimeout = 480
                m_adpAct.Update(p_dtbSuministros)

            Catch ex As Exception
                Throw ex
            Finally
                If Not IsNothing(m_cnnSCGTaller) Then
                    m_cnnSCGTaller.Close()
                End If
            End Try


        End Sub

        Public Function Actualizar(ByVal CadenaSuministros As String, ByVal strorden As String) As Integer

            Dim RNombre As SqlClient.SqlDataReader = Nothing

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpSuministros.UpdateCommand = UpdateCommandSuministros()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpSuministros.UpdateCommand.CommandTimeout = 480

                If CadenaSuministros = "" Then
                    m_adpSuministros.UpdateCommand.Parameters(mc_strArroba & mc_strCadenasuministros).Value = System.DBNull.Value
                Else
                    m_adpSuministros.UpdateCommand.Parameters(mc_strArroba & mc_strCadenasuministros).Value = CadenaSuministros
                End If

                If strorden = "" Then
                    m_adpSuministros.UpdateCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = System.DBNull.Value
                Else
                    m_adpSuministros.UpdateCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = strorden
                End If

                m_adpSuministros.UpdateCommand.Connection = m_cnnSCGTaller
                RNombre = m_adpSuministros.UpdateCommand.ExecuteReader

                If RNombre.Read Then
                    Return 1
                Else
                    Return 0
                End If


            Catch ex As Exception
                Throw ex
            Finally
                ' Se cierra la conexión
                RNombre.Close()
                Call m_cnnSCGTaller.Close()
            End Try
        End Function


        Public Sub CargarDevoluciones(ByRef dtsSuministros As SuministrosXOrdenDataset, ByVal strOrden As String, ByVal intCentroCosto As Integer)
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                With m_adpSuministros

                    .SelectCommand = CrearCommandSuministros(strOrden, intCentroCosto)
                    'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                    .SelectCommand.CommandTimeout = 480
                    With .SelectCommand

                        .Connection = m_cnnSCGTaller
                        .CommandText = mc_strSCGTA_SP_SelSuministrosEntrada

                    End With

                End With

                m_adpSuministros.Fill(dtsSuministros.SCGTA_SP_SelSuministrosEntradaSalida)

            Catch ex As Exception
                Throw ex
            Finally
                If Not m_cnnSCGTaller Is Nothing Then
                    m_cnnSCGTaller.Close()
                End If
            End Try
        End Sub

        'Public Function CargarDevolucionesFacturables(ByRef dtsSuministros As SuministrosXOrdenDataset, ByVal strOrden As String, ByVal intCentroCosto As Integer, ByVal facturable As String) As SqlClient.SqlDataReader
        '    'Si facturable = 1 se muestran los articulos de devolución facturables, 
        '    'Si facturable = 2 se muestran los articulos de devolución no facturables
        '    Try

        '        If m_cnnSCGTaller.State = ConnectionState.Closed Then
        '            If m_cnnSCGTaller.ConnectionString = "" Then
        '                m_cnnSCGTaller.ConnectionString = strConexionADO
        '            End If
        '            m_cnnSCGTaller.Open()
        '        End If

        '        With m_adpSuministros

        '            .SelectCommand = CrearCommandSuministrosFacturables(strOrden, intCentroCosto, facturable)

        '            With .SelectCommand

        '                .Connection = m_cnnSCGTaller
        '                .CommandText = mc_strSCGTA_SP_SelSuministrosEntradaFacturables

        '            End With

        '        End With

        '        m_adpSuministros.Fill(dtsSuministros.SCGTA_SP_SelSuministrosEntradaSalida)

        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        If Not m_cnnSCGTaller Is Nothing Then
        '            m_cnnSCGTaller.Close()
        '        End If
        '    End Try
        'End Function

        Public Function Inserta(ByVal dataset As SuministrosDataset,
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
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.InsertCommand.CommandTimeout = 480
                m_adpAct.InsertCommand.Transaction = Transaction
                m_adpAct.InsertCommand.Connection = Conexion

                Call m_adpAct.Update(dataset.SCGTA_VW_Suministros)

            Catch ex As Exception
                Throw
            End Try

        End Function

        Public Function EliminarSuministros(ByVal dataset As SuministrosDataset) As String
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.DeleteCommand = CreateDeleteCommandSuministros()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.DeleteCommand.CommandTimeout = 480
                m_adpAct.DeleteCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Update(dataset.SCGTA_VW_Suministros)

            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

#End Region

#Region "Creación de comandos"


        Private Function CreateDeleteCommandSuministros() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_TB_DElSuministros)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function UpdateCommandSuministros() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDFacturaSuministro)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strCadenasuministros, SqlDbType.VarChar, 250, mc_strCadenasuministros)
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelSuministros)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                .Add(mc_strArroba & mc_strLineNum, SqlDbType.Int, 4, mc_strLineNum)

                .Add(mc_strArroba & mc_strAdicional, SqlDbType.Int, 4, mc_strAdicional)


                .Add(mc_strArroba & mc_strTipoItemGenerico, SqlDbType.VarChar, 20)
                .Add(mc_strArroba & mc_strTipoItemNoGenerico, SqlDbType.VarChar, 20)
                .Add(mc_strArroba & mc_strItemNoProcesado, SqlDbType.VarChar, 20)
                .Add(mc_strArroba & mc_strItemNoTrasladado, SqlDbType.VarChar, 20)
                .Add(mc_strArroba & mc_strItemTrasladado, SqlDbType.VarChar, 20)
                .Add(mc_strArroba & mc_strItemPendienteTraslado, SqlDbType.VarChar, 20)
                '-------------para documentos drafts-----------------------------
                .Add(mc_strArroba & mc_strItemPendientBodega, SqlDbType.VarChar, 20)
                '----------------------------------------------------------------
                .Add(mc_strArroba & mc_strItemSinDescripcion, SqlDbType.VarChar, 20)

            End With

            Return cmdSel

        End Function

        Private Function CrearCommandSuministros(ByVal strOrden As String, ByVal intCentroCosto As Integer) As SqlCommand
            Dim cmdSuministros As New SqlCommand

            With cmdSuministros
                .CommandType = CommandType.StoredProcedure
                If strOrden.Trim = "" Then
                    .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = DBNull.Value
                Else
                    .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = strOrden
                End If
                If intCentroCosto = 0 Then
                    .Parameters.Add(mc_strArroba & mc_intNoCentroCosto, SqlDbType.Int, 4).Value = DBNull.Value
                Else
                    .Parameters.Add(mc_strArroba & mc_intNoCentroCosto, SqlDbType.Int, 4).Value = intCentroCosto
                End If
            End With

            Return cmdSuministros
        End Function

        'Private Function CrearCommandSuministrosFacturables(ByVal strOrden As String, ByVal intCentroCosto As Integer, ByVal facturable As String) As SqlCommand
        '    Dim cmdSuministros As New SqlCommand

        '    With cmdSuministros
        '        .CommandType = CommandType.StoredProcedure
        '        If strOrden.Trim = "" Then
        '            .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = DBNull.Value
        '        Else
        '            .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = strOrden
        '        End If
        '        If intCentroCosto = 0 Then
        '            .Parameters.Add(mc_strArroba & mc_intNoCentroCosto, SqlDbType.Int, 4).Value = DBNull.Value
        '        Else
        '            .Parameters.Add(mc_strArroba & mc_intNoCentroCosto, SqlDbType.Int, 4).Value = intCentroCosto
        '        End If
        '        .Parameters.Add(mc_strArroba & mc_strFacturable, SqlDbType.VarChar, 10).Value = facturable
        '    End With

        '    Return cmdSuministros
        'End Function

        'Private Function CrearCommandSuministrosF(ByVal strOrden As String, ByVal intCentroCosto As Integer, ByVal factur As String) As SqlCommand
        '    Dim cmdSuministros As New SqlCommand

        '    With cmdSuministros
        '        .CommandType = CommandType.StoredProcedure
        '        If strOrden.Trim = "" Then
        '            .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = DBNull.Value
        '        Else
        '            .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = strOrden
        '        End If
        '        If intCentroCosto = 0 Then
        '            .Parameters.Add(mc_strArroba & mc_intNoCentroCosto, SqlDbType.Int, 4).Value = DBNull.Value
        '        Else
        '            .Parameters.Add(mc_strArroba & mc_intNoCentroCosto, SqlDbType.Int, 4).Value = intCentroCosto
        '        End If
        '        If factur = "" Then
        '            .Parameters.Add(mc_strArroba & mc_strFacturada, SqlDbType.VarChar, 50).Value = DBNull.Value
        '        Else
        '            .Parameters.Add(mc_strArroba & mc_strFacturada, SqlDbType.VarChar, 50).Value = factur
        '        End If
        '    End With

        '    Return cmdSuministros
        'End Function

        Private Function CreateInsertCommand() As SqlCommand

            Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSSuministrosxOrden)

            cmdIns.CommandType = CommandType.StoredProcedure

            With cmdIns.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                .Add(mc_strArroba & mc_strLineNum, SqlDbType.Int, 4, mc_strLineNum)

                .Add(mc_strArroba & mc_strAdicional, SqlDbType.Int, 4, mc_strAdicional)

                .Add(mc_strArroba & mc_strCantidad, SqlDbType.Decimal, 9, mc_strCantidad)

                .Add(mc_strArroba & mc_strNoSuministro, SqlDbType.NVarChar, 50, mc_strNoSuministro)

                .Add(mc_strArroba & mc_strLineNumFather, SqlDbType.Int, 4, mc_strLineNumFather)

                .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID).Direction = ParameterDirection.Output


                .Add(mc_strArroba & "LineNumOriginal", SqlDbType.Int, 80, "LineNumOriginal")

            End With

            Return cmdIns

        End Function

        Private Overloads Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdSuministroXOrden)

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters

                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID)
                    .Add(mc_strArroba & mc_strCantidad, SqlDbType.Decimal, 9, mc_strCantidad)
                    .Add(mc_strArroba & mc_strLineNum, SqlDbType.Int, 4, mc_strLineNum)
                    .Add(mc_strArroba & mc_strLineNumFather, SqlDbType.Int, 4, mc_strLineNumFather)
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.NVarChar, 50, mc_strNoOrden)

                End With

                Return cmdUPD

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

        '**********************************
        Public Overloads Function Fill_Suministros(ByVal dataSet As SuministrosDataset, ByVal p_orden As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpSuministros = New SqlDataAdapter

                m_adpSuministros.SelectCommand = Me.CrearSelectCommandSuministros(p_orden)
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpSuministros.SelectCommand.CommandTimeout = 480
                m_adpSuministros.SelectCommand.Connection = m_cnnSCGTaller

                Fill_Suministros = m_adpSuministros.Fill(dataSet.SCGTA_TB_SuministroxOrden)


            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Private Function CrearSelectCommandSuministros(ByVal p_Orden As String) As SqlClient.SqlCommand
            Try

                Dim cmdSel As New SqlClient.SqlCommand("SCGTA_SP_CargarSuministrosxOrden")

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & "NoOrden", SqlDbType.VarChar, 80).Value = p_Orden


                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try
        End Function

#End Region

    End Class
End Namespace
