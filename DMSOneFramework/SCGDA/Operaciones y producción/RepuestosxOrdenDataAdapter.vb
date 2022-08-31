

Namespace SCGDataAccess

Public Class RepuestosxOrdenDataAdapter
    Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_CodEstadoRep As String = "CodEstadoRep"
        Private Const mc_Costo As String = "Costo"
        Private Const mc_CantidadPendiente As String = "CantidadPendiente"
        Private Const mc_Componente As String = "Componente"
        Private Const mc_Bodega As String = "Bodega"
        Private Const mc_ID As String = "ID"
        Private Const mc_CodNuevo As String = "CodEstadoNuevo"
        Private Const mc_strPrecioAcordado As String = "PrecioAcordado"

        Private Const mc_LineNumOriginal As String = "LineNumOriginal"


        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_intnoActividad As String = "NoActividad"
        Private Const mc_intNoFase As String = "NoFase"
        Private Const mc_strEstado As String = "Estado"
        Private Const mc_strAdicional As String = "Adicional"
        Private Const mc_strNoRepuesto As String = "NoRepuesto"
        Private Const mc_intAdicional As String = "Adicional"
        Private Const mc_intPrimaryKey As String = "PrimaryKey"
        Private Const mc_strFacturada As String = "Facturada"
        Private Const mc_intNoFactura As String = "NoFactura"
        Private Const mc_strCadenaActividades As String = "cadena_actividades"
        Private Const mc_strLineNum As String = "LineNum"
        Private Const mc_strLineNumFather As String = "LineNumFather"
        Private Const mc_strTipo As String = "Tipo"
        Private Const mc_strEstadoTransf As String = "EstadoTransf"
        Private Const mc_strTrasladado As String = "Trasladado"
        Private Const mc_strCantidadLineasAnte As String = "CantidadLineasAnte"
        Private Const mc_strId As String = "ID"

        Private Const mc_strItemCodeEspecifico As String = "ItemCodeEspecifico"
        Private Const mc_strItemNameEspecifico As String = "ItemNameEspecifico"


        Private Const mc_strCosto As String = "Costo"

        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_strCantidad As String = "Cantidad"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_intCantidadPendiente As String = "CantidadPendiente"
        Private Const mc_strComponente As String = "Componente"

    Private m_adpAct As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDRep As String = "SCGTA_SP_UpdRepuestoXOrden"
        Private Const mc_strSCGTA_SP_SELRep As String = "SCGTA_SP_SELRepuestosxOrden"
        Private Const mc_strSCGTA_SP_SELRepbyFilters As String = "SCGTA_SP_SELRepuestosxOrdenByFilters"
        Private Const mc_strSCGTA_SP_DelRep As String = "SCGTA_SP_DELRepuestoXOrden"
        Private Const mc_strSCGTA_SP_InsRep As String = "SCGTA_SP_INSRepuestosxOrden"
        Private Const mc_strSCGTA_SP_SCGSelRepuestoxOrden As String = "SCGTA_SP_SCGSelRepuestoxOrden"
        Private Const mc_strSCGTA_SP_DELRepuestoXOrden As String = "SCGTA_SP_DELRepuestoXOrden"
        Private Const mc_strSCGTA_SP_DELRepuesto1XOrden As String = "SCGTA_SP_DelRepuesto1xOrden"
        Private Const mc_strSCGTA_SP_UpdRepuestoXOrdenCantidades As String = "UpdRepuestoXOrdenCantidades"
        Private Const mc_strSCGTA_SP_UpdRepuestoXOrden As String = "SCGTA_SP_UpdRepuestoXOrden"
        Private Const mc_strSCGTA_SP_UpdRepuestoXOrdenBodega As String = "SCGTA_SP_UpdRepuestoXOrdenBodega"
        Private Const mc_strSCGTA_SP_UpdRepuestoXOrdenPrecioAcordado As String = "SCGTA_SP_UpdRepuestoXOrdenPrecioAcordado"
        Private Const mc_strSCGTA_SP_SELVerificarEstadoRep As String = "SCGTA_SP_SELVerificarEstadoRep"
        Private Const mc_strSCGTA_SP_UPDEstadoRepuestos As String = "SCGTA_SP_UPDEstadoRepuesto"
        Private Const mc_strSCGTA_SP_UPDCantidadRepuestosXAjuste As String = "SCGTA_SP_UpdCantidadRepuestoXOrdenAjuste"

        'Cambio actualizar estado de los repuestos

        Private Const mc_strSCGTA_SP_SELRepuestosxOrdenAdicionales As String = "SCGTA_SP_SELRepuestosxOrdenAdicionales"

        'Valida repuestos o SE con cantidad solicitada
        Private Const mc_strSCGTA_SP_SELRepuestoSECantSolicitada As String = "SCGTA_SP_SELRepuestoSECantSolicitada"

        'Actualiza costo del repuesto
        Private Const mc_strSCGTA_SP_UPDCostoRepuestoxOrden As String = "SCGTA_SP_UPDCostoRepuestoxOrden"



        Private Const mc_strTipoItemGenerico As String = "TipoItemGenerico"
        Private Const mc_strTipoItemNoGenerico As String = "TipoItemNoGenerico"

        Private Const mc_strItemNoProcesado As String = "NoProcesado"
        Private Const mc_strItemNoTrasladado As String = "NoTrasladado"
        Private Const mc_strItemTrasladado As String = "ItemTrasladado"
        Private Const mc_strItemPendienteTraslado As String = "PendienteTraslado"
        Private Const mc_strItemSinDescripcion As String = "SinDescripcion"
        '' para documentos Draft
        Private Const mc_strItemPendienteBodega As String = "PendienteBodega"


        Private Const mc_strSCGTA_SP_InsRepuestoXOrden As String = "SCGTA_SP_INSRepuestosXOrden"
        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

        Private m_strConexion As String

        Private m_strGenerico As String = My.Resources.ResourceFrameWork.Generico
        Private m_strNoGenerico As String = My.Resources.ResourceFrameWork.NoGenerico

        Private m_strNoProcesado As String = My.Resources.ResourceFrameWork.NoProcesado
        Private m_strNoTrasladado As String = My.Resources.ResourceFrameWork.NoTrasladado
        Private m_strTrasladado As String = My.Resources.ResourceFrameWork.Trasladado
        Private m_strPendienteTaslado As String = My.Resources.ResourceFrameWork.PendienteTraslado
        Private m_strSinDescripcion As String = My.Resources.ResourceFrameWork.SinDescripcion
        Private m_strPendienteBodega As String = My.Resources.ResourceFrameWork.PendienteBodega


      


#End Region

#Region "Inicializa DataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpAct = New SqlClient.SqlDataAdapter
        End Sub

        Public Sub New(ByVal conexion As String)
            Try
                m_strConexion = conexion
                m_cnnSCGTaller = New SqlClient.SqlConnection(conexion)
                m_adpAct = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub


#End Region

#Region "Implementaciones .Net Framework"

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

            End Get
        End Property

#End Region

#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As RepuestosxOrdenDataset, _
                                        ByVal decNoOrden As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    Call m_cnnSCGTaller.Open()
                    'If m_cnnSCGTaller.ConnectionString = "" Then
                    '    m_cnnSCGTaller.ConnectionString = strConexionADO
                    'End If
                    'Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.SelectCommand = CrearSelectCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.SelectCommand.CommandTimeout = 480
                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = decNoOrden

                m_adpAct.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Fill(dataSet.SCGTA_TB_RepuestosxOrden)


            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As RepuestosxOrdenDataset, _
                                        ByVal decNoOrden As String, _
                                        ByVal cn As SqlClient.SqlConnection, _
                                        ByVal tran As SqlClient.SqlTransaction) As Integer

            Try

                'If m_cnnSCGTaller.State = ConnectionState.Closed Then
                '    Call m_cnnSCGTaller.Open()
                '    'If m_cnnSCGTaller.ConnectionString = "" Then
                '    '    m_cnnSCGTaller.ConnectionString = strConexionADO
                '    'End If
                '    'Call m_cnnSCGTaller.Open()
                'End If

                m_adpAct.SelectCommand = CrearSelectCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.SelectCommand.CommandTimeout = 480
                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = decNoOrden

                m_adpAct.SelectCommand.Connection = cn

                m_adpAct.SelectCommand.Transaction = tran

                Call m_adpAct.Fill(dataSet.SCGTA_TB_RepuestosxOrden)


            Catch ex As Exception

                Throw ex

                'Finally

                'Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As RepuestosxOrdenDataset, _
                                       ByVal decNoOrden As String, _
                                       ByVal intEstado As Integer, _
                                       ByVal intTipoArticulo As Integer, _
                                       ByVal intAdicional As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.SelectCommand = CrearSelectCommandByFilters()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.SelectCommand.CommandTimeout = 480
                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = decNoOrden

                If intEstado = -1 Then
                    m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_CodEstadoRep).Value = DBNull.Value
                Else
                    m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_CodEstadoRep).Value = intEstado
                End If

                If intAdicional = 0 Then
                    m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strAdicional).Value = intAdicional
                Else
                    m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strAdicional).Value = DBNull.Value
                End If

                If intTipoArticulo = 0 Then
                    m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strTipo).Value = DBNull.Value
                Else

                    m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strTipo).Value = intTipoArticulo
                End If

                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strTipoItemNoGenerico).Value = m_strNoGenerico
                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strTipoItemGenerico).Value = m_strGenerico

                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strItemNoProcesado).Value = m_strNoProcesado
                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strItemNoTrasladado).Value = m_strNoTrasladado
                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strItemTrasladado).Value = m_strTrasladado
                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strItemPendienteTraslado).Value = m_strPendienteTaslado
                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strItemPendienteBodega).Value = m_strPendienteBodega
                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strItemSinDescripcion).Value = m_strSinDescripcion


                m_adpAct.SelectCommand.Connection = m_cnnSCGTaller

                dataSet.SCGTA_TB_RepuestosxOrden.CheckColumn.DefaultValue = 0

                Call m_adpAct.Fill(dataSet.SCGTA_TB_RepuestosxOrden)


            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As RepuestosxOrdenDataset, _
                                       ByVal NoOrden As String, _
                                       ByVal NoRepuesto As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    Call m_cnnSCGTaller.Open()
                    'If m_cnnSCGTaller.ConnectionString = "" Then
                    '    m_cnnSCGTaller.ConnectionString = strConexionADO
                    'End If
                    'Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.SelectCommand = CrearSelectCommandRepuestoxOrden()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.SelectCommand.CommandTimeout = 480
                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden
                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strNoRepuesto).Value = NoRepuesto

                m_adpAct.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Fill(dataSet.SCGTA_TB_RepuestosxOrden)

                Return dataSet.SCGTA_TB_RepuestosxOrden.Rows.Count


            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)
                Return -1

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function



        'Verifica si existen Repuestos o Servicios Externos con cantidades solicitadas
        Public Function FillRepuestoSECantSolicitada(ByVal NoOrden As String) As Integer

            Try
                Dim sql_DataReader As SqlClient.SqlDataReader
                'Dim dataSetTemp As RepuestosxOrdenDataset
                'dataSetTemp = New RepuestosxOrdenDataset()

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.SelectCommand = CrearSelectCommandRepuestoSESolicitadaXOrden()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                'm_adpAct.SelectCommand.CommandTimeout = 480
                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden

                m_adpAct.SelectCommand.Connection = m_cnnSCGTaller

                'Call m_adpAct.Fill(dataSetTemp.SCGTA_TB_RepuestosxOrden)

                'Return dataSetTemp.SCGTA_TB_RepuestosxOrden.Rows.Count

                sql_DataReader = m_adpAct.SelectCommand.ExecuteReader()

                If sql_DataReader.Read Then
                    Return sql_DataReader.Item("total")
                Else
                    Return -1
                End If

            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)
                Return -1

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function




        Public Overloads Function FillRepuestosxOrdenAdicionales(ByVal dataSet As RepuestosxOrdenDataset, _
                                       ByVal NoOrden As String)

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    Call m_cnnSCGTaller.Open()
                    'If m_cnnSCGTaller.ConnectionString = "" Then
                    '    m_cnnSCGTaller.ConnectionString = strConexionADO
                    'End If
                    'Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.SelectCommand = CrearSelectCommandRepuestoAdicionalesxOrden()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.SelectCommand.CommandTimeout = 480
                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden

                m_adpAct.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Fill(dataSet.SCGTA_TB_RepuestosxOrden)



            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)


            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Function Delete(ByVal dataset As RepuestosxOrdenDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.UpdateCommand = CrearDeleteCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.UpdateCommand.CommandTimeout = 480
                m_adpAct.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Update(dataset.SCGTA_TB_RepuestosxOrden)

            Catch ex As Exception

                Throw ex

            Finally
                Call m_cnnSCGTaller.Close()

            End Try
        End Function

        Public Function DeleteRepuestosxOrden(ByRef dataset As RepuestosxOrdenDataset) As Integer
            Dim m_trn As SqlClient.SqlTransaction = Nothing

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_trn = m_cnnSCGTaller.BeginTransaction()

                m_adpAct.DeleteCommand = CrearDeleteCommandRxO()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.DeleteCommand.CommandTimeout = 480
                m_adpAct.DeleteCommand.Connection = m_cnnSCGTaller
                m_adpAct.DeleteCommand.Transaction = m_trn

                Call m_adpAct.Update(dataset.SCGTA_TB_RepuestosxOrden)
                Call m_trn.Commit()

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
        End Function

        Public Overloads Function Update(ByVal table As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable) As String

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
                m_adpAct.InsertCommand.Connection = m_cnnSCGTaller
                m_adpAct.UpdateCommand = CrearUpdateCommand()
                m_adpAct.UpdateCommand.Connection = m_cnnSCGTaller
                m_adpAct.DeleteCommand = CrearDeleteCommandRxO()
                m_adpAct.DeleteCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Update(table)


            Catch ex As Exception

                Throw ex

            Finally
                m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Overloads Function Update(ByVal table As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable, _
                                         ByRef cn As SqlClient.SqlConnection, _
                                         ByRef tran As SqlClient.SqlTransaction, _
                                         Optional ByVal blnIniciar As Boolean = False, _
                                         Optional ByVal blnTerminar As Boolean = False, _
                                         Optional ByVal blnActCantidad As Boolean = False) As String


            Try

                If blnIniciar Then
                    If m_cnnSCGTaller.State = ConnectionState.Closed Then
                        If m_cnnSCGTaller.ConnectionString = "" Then
                            m_cnnSCGTaller.ConnectionString = m_cnnSCGTaller.ConnectionString
                        End If
                        Call m_cnnSCGTaller.Open()
                        cn = m_cnnSCGTaller
                        tran = cn.BeginTransaction(IsolationLevel.ReadCommitted)
                    End If
                End If

                m_adpAct.InsertCommand = CreateInsertCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.InsertCommand.CommandTimeout = 480
                m_adpAct.InsertCommand.Connection = cn
                m_adpAct.InsertCommand.Transaction = tran
                If blnActCantidad Then
                    m_adpAct.UpdateCommand = CrearUpdateCommandCantidad()
                    'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                    m_adpAct.UpdateCommand.CommandTimeout=480
                Else
                    m_adpAct.UpdateCommand = CrearUpdateCommand()
                    'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                    m_adpAct.UpdateCommand.CommandTimeout = 480
                End If
                m_adpAct.UpdateCommand.Connection = cn
                m_adpAct.UpdateCommand.Transaction = tran
                m_adpAct.DeleteCommand = CrearDeleteCommandRxO()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.}
                m_adpAct.DeleteCommand.CommandTimeout = 480
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

        End Function

        ''**************************************Para documentos Draft*****************************'''

        Public Overloads Function UpdateDraft(ByVal table As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable, _
                                        ByRef cn As SqlClient.SqlConnection, _
                                        ByRef tran As SqlClient.SqlTransaction, _
                                        Optional ByVal blnIniciar As Boolean = False, _
                                         Optional ByVal blnTerminar As Boolean = False, _
                                         Optional ByVal blnActCantidad As Boolean = False) As String


            Try
                If blnIniciar Then
                    cn = New SqlClient.SqlConnection
                    If m_cnnSCGTaller.State = ConnectionState.Closed Then
                        If m_cnnSCGTaller.ConnectionString = "" Then
                            m_cnnSCGTaller.ConnectionString = strConexionADO
                        End If
                        Call m_cnnSCGTaller.Open()
                        cn = m_cnnSCGTaller
                        tran = cn.BeginTransaction(IsolationLevel.ReadUncommitted)
                    Else
                        cn = m_cnnSCGTaller
                        tran = cn.BeginTransaction(IsolationLevel.ReadUncommitted)
                    End If
                End If

                m_adpAct.InsertCommand = CreateInsertDraftCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.InsertCommand.CommandTimeout = 480
                m_adpAct.InsertCommand.Connection = cn
                m_adpAct.InsertCommand.Transaction = tran

                If blnActCantidad Then
                    m_adpAct.UpdateCommand = CrearUpdateCommandCantidad()
                    'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                    m_adpAct.UpdateCommand.CommandTimeout = 480
                Else
                    m_adpAct.UpdateCommand = CrearUpdateCommand()
                    'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                    m_adpAct.UpdateCommand.CommandTimeout = 480
                End If
                m_adpAct.UpdateCommand.Connection = cn
                m_adpAct.UpdateCommand.Transaction = tran
                m_adpAct.DeleteCommand = CrearDeleteCommandRxO()
                m_adpAct.DeleteCommand.Connection = cn
                m_adpAct.DeleteCommand.Transaction = tran

                Call m_adpAct.Update(table)
                If blnTerminar Then
                    tran.Commit()
                End If

                'tran.Commit()
            Catch ex As Exception

                Throw ex

            End Try

        End Function

        Public Overloads Function UpdateCodigoRepuesto(ByVal dataSet As RepuestosxOrdenDataset, _
                                                         Optional ByRef tran As SqlClient.SqlTransaction = Nothing) As String


            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()

                End If


                m_adpAct.UpdateCommand = CrearUpdateCommandCodigoRepuestoTransferenciaDraft()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.UpdateCommand.CommandTimeout = 480
                m_adpAct.UpdateCommand.Connection = m_cnnSCGTaller
                If Not tran Is Nothing Then m_adpAct.UpdateCommand.Transaction = tran
                Call m_adpAct.Update(dataSet.SCGTA_TB_RepuestosxOrden)


            Catch ex As Exception

                Throw ex

            Finally
                If tran IsNot Nothing Then
                    If tran.Connection Is Nothing Then
                        m_cnnSCGTaller.Close()
                    End If
                End If
            End Try

        End Function

        ''**************************************Para documentos Draft*****************************'''

        Public Overloads Function Update(ByVal dataSet As RepuestosxOrdenDataset) As String


            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.InsertCommand = CreateInsertCommandRepuestos(0)
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.InsertCommand.CommandTimeout = 480
                m_adpAct.InsertCommand.Connection = m_cnnSCGTaller
                m_adpAct.UpdateCommand = CrearUpdateCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.UpdateCommand.CommandTimeout = 480
                m_adpAct.UpdateCommand.Connection = m_cnnSCGTaller
                m_adpAct.DeleteCommand = CrearDeleteCommandRxO()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.DeleteCommand.CommandTimeout = 480
                m_adpAct.DeleteCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Update(dataSet.SCGTA_TB_RepuestosxOrden)


            Catch ex As Exception

                Throw ex

            Finally
                m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Update(ByRef p_dataTable As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable, ByVal intEstado As Integer) As String
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.UpdateCommand = CrearUpdateCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.UpdateCommand.CommandTimeout=480
                With m_adpAct.UpdateCommand
                    '.Parameters(mc_strArroba & mc_CodEstadoRep).Value = intEstado
                    'Modificado 10/07/06. Alejandra
                    .Parameters(mc_strArroba & mc_CodNuevo).Value = intEstado
                End With

                m_adpAct.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Update(p_dataTable)

            Catch ex As Exception

                Throw ex

            Finally
                m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function UpdateEliminar(ByRef p_dataTable As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable) As String
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.DeleteCommand = EliminarUpdateCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.DeleteCommand.CommandTimeout = 480
                m_adpAct.DeleteCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Update(p_dataTable)

            Catch ex As Exception

                Throw ex

            Finally
                m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Sub UpdateBodega(ByRef p_dtbRepuestos As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable)

            Dim cmmd As SqlClient.SqlCommand
            m_adpAct = New SqlClient.SqlDataAdapter
            Try

                cmmd = New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdRepuestoXOrdenBodega, m_cnnSCGTaller)

                With cmmd
                    .CommandType = CommandType.StoredProcedure
                    With .Parameters
                        .Add(mc_strArroba & mc_ID, SqlDbType.Int, 4, mc_ID) '.Value = p_drwRepuesto.ID
                        .Add(mc_strArroba & mc_Bodega, SqlDbType.Bit, 1, mc_Bodega) '.Value = p_drwRepuesto.Bodega
                    End With
                End With
                m_adpAct.UpdateCommand = cmmd
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.UpdateCommand.CommandTimeout = 480
                m_adpAct.Update(p_dtbRepuestos)

            Catch ex As Exception
                Throw ex
            Finally
                If Not IsNothing(m_cnnSCGTaller) Then
                    m_cnnSCGTaller.Close()
                End If
            End Try


        End Sub

        Public Overloads Sub UpdateCantidadXAjuste(ByVal p_Articulo As String, _
                                                    ByVal p_NoOrden As String, _
                                                    ByVal p_Cantidad As Decimal, _
                                                    ByVal p_LineNumOriginal As Integer)

            Dim cmdUpd As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    Call m_cnnSCGTaller.Open()
                End If

                cmdUpd = New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDCantidadRepuestosXAjuste, m_cnnSCGTaller)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters
                    .AddWithValue(mc_strArroba & "Articulo", p_Articulo)
                    .AddWithValue(mc_strArroba & mc_strNoOrden, p_NoOrden)
                    .AddWithValue(mc_strArroba & mc_strCantidad, p_Cantidad)
                    .AddWithValue(mc_strArroba & mc_LineNumOriginal, p_LineNumOriginal)

                End With

                cmdUpd.ExecuteNonQuery()


            Catch ex As SqlClient.SqlException
                MsgBox(ex.Message)
            Catch ex As Exception
                MsgBox(ex.Message)

            Finally

                Call m_cnnSCGTaller.Close()
            End Try


        End Sub

        'Public Overloads Sub UpdateCantidadXAjuste(ByVal p_Articulo As String, _
        '                                            ByVal p_NoOrden As String, _
        '                                            ByVal p_Cantidad As Decimal, _
        '                                            ByVal p_LineNumOriginal As Integer, _
        '                                            ByRef p_dtbRepuestos As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable)

        '    Dim cmmd As SqlClient.SqlCommand
        '    m_adpAct = New SqlClient.SqlDataAdapter
        '    Try

        '        cmmd = New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDCantidadRepuestosXAjuste, m_cnnSCGTaller)

        '        With cmmd
        '            .CommandType = CommandType.StoredProcedure
        '            With .Parameters
        '                .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.NVarChar, 100, mc_strNoRepuesto)
        '                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.NVarChar, 100, mc_strNoOrden)
        '                .Add(mc_strArroba & mc_strCantidad, SqlDbType.Decimal, 100, mc_strCantidad)
        '                .Add(mc_strArroba & mc_LineNumOriginal, SqlDbType.NVarChar, 100, mc_LineNumOriginal)
        '            End With
        '        End With
        '        m_adpAct.UpdateCommand = cmmd
        '        m_adpAct.Update(p_dtbRepuestos)

        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        If Not IsNothing(m_cnnSCGTaller) Then
        '            m_cnnSCGTaller.Close()
        '        End If
        '    End Try


        'End Sub



        Public Function UpdateCostoRepuesto(ByVal p_strNoOrden As String _
                                            , ByVal p_strNoRepuesto As String _
                                            , ByVal p_strLineNum As Integer _
                                            , ByVal p_strCosto As Decimal)

            Dim cmdUpd As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    Call m_cnnSCGTaller.Open()
                End If

                cmdUpd = New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDCostoRepuestoxOrden, m_cnnSCGTaller)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters
                    .AddWithValue(mc_strArroba & mc_strNoOrden, p_strNoOrden)
                    .AddWithValue(mc_strArroba & mc_strNoRepuesto, p_strNoRepuesto)
                    .AddWithValue(mc_strArroba & mc_strLineNum, p_strLineNum)
                    .AddWithValue(mc_strArroba & mc_strCosto, p_strCosto)

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
        


        'Public Overloads Sub UpdatePrecioAcordado(ByVal p_intID As Integer, ByVal p_dblPrecio As Double)

        '    Dim cmmd As SqlClient.SqlCommand
        '    m_adpAct = New SqlClient.SqlDataAdapter
        '    Try
        '        If m_cnnSCGTaller.State <> ConnectionState.Open Then
        '            m_cnnSCGTaller.Open()
        '        End If
        '        cmmd = New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdRepuestoXOrdenPrecioAcordado, m_cnnSCGTaller)

        '        With cmmd
        '            .CommandType = CommandType.StoredProcedure
        '            With .Parameters
        '                .Add(mc_strArroba & mc_ID, SqlDbType.Int, 4, mc_ID).Value = p_intID
        '                .Add(mc_strArroba & mc_strPrecioAcordado, SqlDbType.Decimal, 18, mc_strPrecioAcordado).Value = p_dblPrecio
        '            End With
        '        End With
        '        cmmd.ExecuteNonQuery()

        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        If Not IsNothing(m_cnnSCGTaller) Then
        '            m_cnnSCGTaller.Close()
        '        End If
        '    End Try

        'End Sub

        Public Function Inserta(ByVal dataset As RepuestosxOrdenDataset,
                                ByRef Transaction As SqlClient.SqlTransaction,
                                ByRef Conexion As SqlClient.SqlConnection) As String
            Try

                Dim Comando As SqlClient.SqlCommand
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                Comando = CreateInsertCommand()
                Comando.Transaction = Transaction
                m_adpAct.InsertCommand = Comando
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.InsertCommand.CommandTimeout = 480
                m_adpAct.InsertCommand.Connection = Conexion

                Call m_adpAct.Update(dataset.SCGTA_TB_RepuestosxOrden)

                'llama al rollback mas arriba
            Catch ex As Exception
                Throw
                'Finally
                '    m_cnnSCGTaller.Close()
            End Try

        End Function




        Public Function VerificarEstadoRepPend(ByVal p_intID As Integer) As Boolean
            Dim cmdVerificar As SqlClient.SqlCommand
            Dim intCont As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    Call m_cnnSCGTaller.Open()
                End If

                cmdVerificar = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELVerificarEstadoRep, m_cnnSCGTaller)

                cmdVerificar.CommandType = CommandType.StoredProcedure
                cmdVerificar.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_ID, SqlDbType.Int)).Value = p_intID

                intCont = cmdVerificar.ExecuteScalar

                If intCont = 0 Then
                    Return True
                Else
                    Return False
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




        Public Function UpdateCostoRepuestosXOrden(ByVal dataSet As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable, _
                                ByRef p_cnnConeccion As SqlClient.SqlConnection)

            Try
                If p_cnnConeccion.State = ConnectionState.Closed Then
                    Call p_cnnConeccion.Open()
                End If
                m_adpAct = New SqlClient.SqlDataAdapter


                m_adpAct.InsertCommand = CrearUpdateCommandCostoRepuesto()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.InsertCommand.CommandTimeout = 480
                m_adpAct.InsertCommand.Connection = p_cnnConeccion
                'm_adpAct.InsertCommand.Transaction = p_trnTransacion

                Call m_adpAct.Update(dataSet)

            Catch ex As Exception
                Throw ex

                'Finally
                'If Not IsNothing(m_cnnSCGTaller) Then
                '    If p_cnnConeccion.State = ConnectionState.Open Then
                '        p_cnnConeccion.Close()
                '    End If
                'End If

            End Try
        End Function





        Public Function UpdateCostoRepuestosXOrden(ByVal dataSet As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable)
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.UpdateCommand = CrearUpdateCommandCostoRepuesto()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.UpdateCommand.CommandTimeout = 480
                m_adpAct.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Update(dataSet)

            Catch ex As Exception

                Throw ex

            Finally
                m_cnnSCGTaller.Close()

            End Try
        End Function

#End Region

#Region "Creación de comandos"
        Private Function CrearSelectCommandRepuestoAdicionalesxOrden() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELRepuestosxOrdenAdicionales)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

            End With

            Return cmdSel


        End Function

        Private Function CrearSelectCommandRepuestoxOrden() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SCGSelRepuestoxOrden)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.Int, 4, mc_strNoRepuesto)

            End With

            Return cmdSel


        End Function


        ' Verifica si existen Repuestos o SE con cantidad solicitada
        Private Function CrearSelectCommandRepuestoSESolicitadaXOrden() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELRepuestoSECantSolicitada)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

            End With

            Return cmdSel


        End Function


        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELRep)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

            End With

            Return cmdSel


        End Function

        Private Function CrearSelectCommandByFilters() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELRepbyFilters)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                .Add(mc_strArroba & mc_CodEstadoRep, SqlDbType.Decimal, 5, mc_CodEstadoRep)
                .Add(mc_strArroba & mc_strAdicional, SqlDbType.Int, 1, mc_strAdicional)
                .Add(mc_strArroba & mc_strTipo, SqlDbType.Int, 1, mc_strTipo)
                .Add(mc_strArroba & mc_strTipoItemGenerico, SqlDbType.VarChar, 20)
                .Add(mc_strArroba & mc_strTipoItemNoGenerico, SqlDbType.VarChar, 20)
                .Add(mc_strArroba & mc_strItemNoProcesado, SqlDbType.VarChar, 20)
                .Add(mc_strArroba & mc_strItemNoTrasladado, SqlDbType.VarChar, 20)
                .Add(mc_strArroba & mc_strItemTrasladado, SqlDbType.VarChar, 20)
                .Add(mc_strArroba & mc_strItemPendienteTraslado, SqlDbType.VarChar, 20)
                .Add(mc_strArroba & mc_strItemPendienteBodega, SqlDbType.VarChar, 20)
                .Add(mc_strArroba & mc_strItemSinDescripcion, SqlDbType.VarChar, 20)

            End With

            Return cmdSel


        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                'Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDRep)
                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDEstadoRepuestos)

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters
                    '.Add(mc_strArroba & mc_CodEstadoRep, SqlDbType.Decimal) ', 5, mc_CodEstadoRep)
                    '.Add(mc_strArroba & mc_NoRepuesto, SqlDbType.Int, 4, mc_NoRepuesto)
                    '.Add(mc_strArroba & mc_NoOrden, SqlDbType.VarChar, 50, mc_NoOrden)
                    '.Add(mc_strArroba & mc_NoPiezaPrincipal, SqlDbType.Decimal, 9, mc_NoPiezaPrincipal)
                    '.Add(mc_strArroba & mc_NoSeccion, SqlDbType.Decimal, 5, mc_NoSeccion)
                    .Add(mc_strArroba & mc_CodEstadoRep, SqlDbType.Decimal, 5, mc_CodEstadoRep)
                    .Add(mc_strArroba & mc_CodNuevo, SqlDbType.Decimal)
                    .Add(mc_strArroba & mc_ID, SqlDbType.Decimal, 5, mc_ID)
                    '.Add(mc_strArroba & mc_strCantidad, SqlDbType.Int, 4, mc_strCantidad)


                End With

                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommandAjusteCantidad() As SqlClient.SqlCommand

            Try


                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDCantidadRepuestosXAjuste)

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters

                    .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.NVarChar, 100, mc_strNoRepuesto)
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.NVarChar, 100, mc_strNoOrden)
                    .Add(mc_strArroba & mc_strCantidad, SqlDbType.Decimal, 100, mc_strCantidad)
                    .Add(mc_strArroba & mc_LineNumOriginal, SqlDbType.NVarChar, 100, mc_LineNumOriginal)

                End With

                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommandCantidades() As SqlClient.SqlCommand

            Try

                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdRepuestoXOrdenCantidades)

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters

                    .Add(mc_strArroba & mc_CodEstadoRep, SqlDbType.Decimal, 5, mc_CodEstadoRep)
                    .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.Int, 4, mc_strNoRepuesto)
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    .Add(mc_strArroba & mc_CantidadPendiente, SqlDbType.Int, 4, mc_CantidadPendiente)
                    .Add(mc_strArroba & mc_strCantidad, SqlDbType.Int, 4, mc_strCantidad)


                End With

                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommandCantidad() As SqlClient.SqlCommand

            Try

                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdRepuestoXOrden)

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters

                    .Add(mc_strArroba & mc_ID, SqlDbType.Int, 4, mc_ID)
                    .Add(mc_strArroba & mc_strCantidad, SqlDbType.Decimal, 9, mc_strCantidad)
                    .Add(mc_strArroba & mc_strTrasladado, SqlDbType.Int, 4, mc_strTrasladado)
                    .Add(mc_strArroba & mc_strLineNum, SqlDbType.Int, 4, mc_strLineNum)
                    .Add(mc_strArroba & mc_strLineNumFather, SqlDbType.Int, 4, mc_strLineNumFather)
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.NVarChar, 50, mc_strNoOrden)
                    .Add(mc_strArroba & mc_strItemCodeEspecifico, SqlDbType.NVarChar, 50, mc_strItemCodeEspecifico)
                    .Add(mc_strArroba & mc_strItemNameEspecifico, SqlDbType.NVarChar, 100, mc_strItemNameEspecifico)

                End With

                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function EliminarUpdateCommand() As SqlClient.SqlCommand
            Try

                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_DELRepuestoXOrden)

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters

                    .Add(mc_strArroba & mc_ID, SqlDbType.Int, 4, mc_ID)

                End With

                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelRep)

            cmdUPD.CommandType = CommandType.StoredProcedure

            With cmdUPD.Parameters


                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.NVarChar, 50, mc_strNoOrden)

            End With

            Return cmdUPD

        End Function

        Private Function CrearDeleteCommandRxO() As SqlClient.SqlCommand

            Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_DELRepuesto1XOrden)

            cmdUPD.CommandType = CommandType.StoredProcedure

            With cmdUPD.Parameters

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.NVarChar, 50, mc_strNoOrden)
                .Add(mc_strArroba & mc_ID, SqlDbType.Int, 4, mc_ID)

            End With

            Return cmdUPD

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsRep)

            cmdIns.CommandType = CommandType.StoredProcedure

            With cmdIns.Parameters

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.VarChar, 20, mc_strNoRepuesto)

                .Add(mc_strArroba & mc_strCantidad, SqlDbType.Decimal, 9, mc_strCantidad)

                .Add(mc_strArroba & mc_strAdicional, SqlDbType.Int, 4, mc_strAdicional)

                .Add(mc_strArroba & mc_strLineNum, SqlDbType.Int, 4, mc_strLineNum)

                .Add(mc_strArroba & mc_strTipo, SqlDbType.Int, 4, "TipoArticulo") 'mc_strTipo

                .Add(mc_strArroba & mc_strEstadoTransf, SqlDbType.Int, 4, mc_strEstadoTransf) 'mc_strTipo

                .Add(mc_strArroba & mc_strLineNumFather, SqlDbType.Int, 4, mc_strLineNumFather)

                .Add(mc_strArroba & mc_strItemNameEspecifico, SqlDbType.NVarChar, 100, mc_strItemNameEspecifico)

                .Add(mc_strArroba & mc_strItemCodeEspecifico, SqlDbType.NVarChar, 50, mc_strItemCodeEspecifico)

                .Add(mc_strArroba & mc_strCantidadLineasAnte, SqlDbType.Int, 4, mc_strCantidadLineasAnte)

                .Add(mc_strArroba & mc_strId, SqlDbType.Int, 4, mc_strId).Direction = ParameterDirection.Output

                .Add("@RespondidoPor", SqlDbType.NVarChar, 80, "RespondidoPor")

                .Add(mc_strArroba & "Compra", SqlDbType.VarChar, 10, "Compra")
            End With

            Return cmdIns


        End Function

        ''***************************************Para Documentos Draft******************************************

        Private Function CreateInsertDraftCommand() As SqlClient.SqlCommand

            Dim cmdIns As New SqlClient.SqlCommand("SCGTA_SP_INSRepuestosxOrdenDocumentosDraft")

            cmdIns.CommandType = CommandType.StoredProcedure

            With cmdIns.Parameters

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.VarChar, 20, mc_strNoRepuesto)

                .Add(mc_strArroba & mc_strCantidad, SqlDbType.Decimal, 9, mc_strCantidad)

                .Add(mc_strArroba & mc_strAdicional, SqlDbType.Int, 4, mc_strAdicional)

                .Add(mc_strArroba & mc_strLineNum, SqlDbType.Int, 4, mc_strLineNum)

                .Add(mc_strArroba & mc_strTipo, SqlDbType.Int, 4, "TipoArticulo") 'mc_strTipo

                .Add(mc_strArroba & mc_strEstadoTransf, SqlDbType.Int, 4, mc_strEstadoTransf) 'mc_strTipo

                .Add(mc_strArroba & mc_strLineNumFather, SqlDbType.Int, 4, mc_strLineNumFather)

                .Add(mc_strArroba & mc_strItemNameEspecifico, SqlDbType.NVarChar, 100, mc_strItemNameEspecifico)

                .Add(mc_strArroba & mc_strItemCodeEspecifico, SqlDbType.NVarChar, 50, mc_strItemCodeEspecifico)

                .Add(mc_strArroba & mc_strCantidadLineasAnte, SqlDbType.Int, 4, mc_strCantidadLineasAnte)

                .Add(mc_strArroba & mc_strId, SqlDbType.Int, 4, mc_strId).Direction = ParameterDirection.Output

                .Add("@RespondidoPor", SqlDbType.NVarChar, 80, "RespondidoPor")

                .Add("@LineNumOriginal", SqlDbType.NVarChar, 80, "LineNumOriginal")

                .Add(mc_strArroba & "Compra", SqlDbType.VarChar, 10, "Compra")


            End With

            Return cmdIns


        End Function

        Private Function CrearUpdateCommandCodigoRepuestoTransferenciaDraft() As SqlClient.SqlCommand

            Try

                Dim cmdUPD As New SqlClient.SqlCommand("SCGTA_SP_UPDCodigoEstadoRepuestoDespuesTranferenciaDraft")

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters

                    .Add(mc_strArroba & "CodEstadoNuevo", SqlDbType.Int, 4, "CodEstadoRep")
                    .Add(mc_strArroba & mc_ID, SqlDbType.Int, 4, mc_ID)

                End With

                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try

        End Function



        Private Function CrearUpdateCommandCostoRepuesto() As SqlClient.SqlCommand

            Try

                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDCostoRepuestoxOrden)

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.VarChar, 50, mc_strNoRepuesto)
                    .Add(mc_strArroba & mc_strLineNum, SqlDbType.Int, 4, mc_strLineNum)
                    .Add(mc_strArroba & mc_strCosto, SqlDbType.Decimal, 18, mc_strCosto)

                End With

                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try

        End Function
        ''*************************************************************************************************************

        Private Function CreateInsertCommandRepuestos(ByVal IntNoAdicional As Integer) As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsRepuestoXOrden)
                cmdIns.CommandType = CommandType.StoredProcedure
                cmdIns.UpdatedRowSource = UpdateRowSource.Both

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                    .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.Int, 4, mc_strNoRepuesto)

                    .Add(mc_strArroba & mc_strCantidad, SqlDbType.Decimal, 9, mc_strCantidad)

                    .Add(mc_strArroba & mc_strAdicional, SqlDbType.Bit, 1, mc_strAdicional)

                    '.Add(mc_strArroba & mc_intCantidadPendiente, SqlDbType.Int, 4, mc_intCantidadPendiente)

                    .Add(mc_strArroba & mc_intCantidadPendiente, SqlDbType.Int, 4, mc_strCantidad)

                    'Agregado 05/07/06. Alejandra. La descripción del repuesto será agregada a la tabla
                    .Add(mc_strArroba & mc_strComponente, SqlDbType.VarChar, 100, mc_strComponente)

                End With


                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

    End Class

End Namespace