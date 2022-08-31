Imports System.Data.SqlClient

Namespace SCGDataAccess
    Public Class RepuestosxEstadoDataAdapter

        Implements IDataAdapter, IDisposable

#Region "Declaraciones"

        'TODO Agregar nombres de procedimeintos almacenados para una determinada tabla
        Private Const mc_strSPInsRepuestosxEstado As String = "SCGTA_SP_InsRepuestosxEstado"
        Private Const mc_strSPUpdUpdRepuestosxEstado As String = "SCGTA_SP_UpdRepuestosxEstado"
        Private Const mc_strSPDelRepuestosxEstado As String = "SCGTA_SP_DelRepuestosxEstado"
        Private Const mc_strSPSelRepuetosxEstado As String = "SCGTA_SP_SelEstadoxRepuestos"
        Private Const mc_strSPSelRepuetosxEstadoxOrden As String = "SCGTA_SP_SelEstadoxRepuestosxOrden"
        Private Const mc_strSPSCGTA_SP_SelCountItemsPendientes As String = "SCGTA_SP_SelCountItemsPendientes"
        Private Const mc_strEstaLlaveExiste As String = ""


        'Cambios estado repuestos en las ot, para las requisiciones
        Private Const mc_strSCGTA_SP_ActualizaEstadoRepuestosRequisiciones As String = "SCGTA_SP_ActualizaEstadoRepuestosRequisiciones"

        Private Const mc_lineNumOr As String = "lineNumOr"
        Private Const mc_numOrden As String = "numOrden"
        Private Const mc_numRep As String = "numRep"
        Private Const mc_cantPendiente As String = "cantPendiente"
        Private Const mc_cantRecibida As String = "cantRecibida"
        Private Const mc_decCosto As String = "Costo"
        Private Const mc_cantNuevaRep As String = "cantNuevaRep"




        Private Const mc_strIdRepuestosxOrden As String = "IdRepuestosxOrden"
        Private Const mc_strCodEstadoRep As String = "CodEstadoRep"
        Private Const mc_strCantidad As String = "Cantidad"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strNoRepuesto As String = "NoRepuesto"
        Private Const mc_strCodEstado As String = "CodEstado"
        Private Const mc_strNoLinea As String = "Nolinea"
        'Private Const mc_strNoRepuesto As String = "NoRepuesto"

        'Declaracion de objetos de acceso a datos
        Private m_cnn As SqlClient.SqlConnection
        Private m_adp As SqlClient.SqlDataAdapter

        Private m_strConexion As String

        Private mc_strArroba As String = "@"
        Private Shared objDAConexion As New DAConexion
#End Region

#Region "Inicializar AnalisisDataAdapter"

        Public Sub New(ByVal conexion As String)
            Try
                m_strConexion = conexion
                m_cnn = New SqlClient.SqlConnection(conexion)
                m_adp = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        Public Sub New()
            Try
                'm_strConexion = conexion
                m_cnn = objDAConexion.ObtieneConexion  'New SqlClient.SqlConnection(conexion)
                m_adp = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        Public Sub New(ByVal blnConectado As Boolean)
            Try

                m_adp = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region

#Region "Implementaciones"
        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Overloads Function Fill(ByRef dataset As EstadoxRepuestosDataset, _
                                     ByVal NoOrden As String) As Integer
            Try

                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                m_adp.SelectCommand = CrearCmdSelxOrden()
                m_adp.SelectCommand.Connection = m_cnn

                m_adp.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden


                Call m_adp.Fill(dataset.SCGTA_TB_RepuestosxEstado)

                Return dataset.SCGTA_TB_RepuestosxEstado.Rows.Count

            Catch ex As Exception
                MsgBox(ex.Message)
                Return -1
            Finally
                Call m_cnn.Close()
            End Try
        End Function

        Public Overloads Function Fill(ByRef dataset As EstadoxRepuestosDataset, _
                                       ByVal NoOrden As String, _
                                       ByVal NoRepuesto As String, _
                                       ByVal CodEstado As Integer, _
                                       ByVal IdRepuestoxOrden As Integer) As Integer
            Try

                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                m_adp.SelectCommand = CrearCmdSel()
                m_adp.SelectCommand.Connection = m_cnn

                If NoOrden <> "" Then
                    m_adp.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden
                End If
                m_adp.SelectCommand.Parameters(mc_strArroba & mc_strNoRepuesto).Value = NoRepuesto
                m_adp.SelectCommand.Parameters(mc_strArroba & mc_strCodEstado).Value = CodEstado
                m_adp.SelectCommand.Parameters(mc_strArroba & mc_strIdRepuestosxOrden).Value = IdRepuestoxOrden

                Call m_adp.Fill(dataset.SCGTA_TB_RepuestosxEstado)

                Return dataset.SCGTA_TB_RepuestosxEstado.Rows.Count

            Catch ex As Exception
                MsgBox(ex.Message)
                Return -1
            Finally
                Call m_cnn.Close()
            End Try
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

        Public Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

        Public Function UpdateCantidadRepuestos( _
                                                   ByVal p_lineNumOr As Integer _
                                                  , ByVal p_numOrden As String _
                                                  , ByVal p_numRep As String _
                                                  , ByVal p_cantPendiente As Decimal _
                                                  , ByVal p_cantRecibida As Decimal _
                                                  , ByVal p_cantidadNueva As Decimal)

            Dim cmdUpd As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                cmdUpd = New SqlClient.SqlCommand(mc_strSCGTA_SP_ActualizaEstadoRepuestosRequisiciones, m_cnn)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters
                    .AddWithValue(mc_strArroba & mc_lineNumOr, p_lineNumOr)
                    .AddWithValue(mc_strArroba & mc_numOrden, p_numOrden)
                    .AddWithValue(mc_strArroba & mc_numRep, p_numRep)
                    .AddWithValue(mc_strArroba & mc_cantPendiente, p_cantPendiente)
                    .AddWithValue(mc_strArroba & mc_cantRecibida, p_cantRecibida)
                    .AddWithValue(mc_strArroba & mc_cantNuevaRep, p_cantidadNueva)

                End With

                cmdUpd.ExecuteNonQuery()


            Catch ex As SqlClient.SqlException
                MsgBox(ex.Message)
            Catch ex As Exception
                MsgBox(ex.Message)

            Finally

                Call m_cnn.Close()
            End Try
        End Function

        Public Function UpdateRepuestosXEstadoRequisiciones( _
                                                   ByVal p_lineNumOr As Integer _
                                                  , ByVal p_numOrden As String _
                                                  , ByVal p_numRep As String _
                                                  , ByVal p_cantPendiente As Decimal _
                                                  , ByVal p_cantRecibida As Decimal)

            Dim cmdUpd As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                cmdUpd = New SqlClient.SqlCommand(mc_strSCGTA_SP_ActualizaEstadoRepuestosRequisiciones, m_cnn)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters
                    .AddWithValue(mc_strArroba & mc_lineNumOr, p_lineNumOr)
                    .AddWithValue(mc_strArroba & mc_numOrden, p_numOrden)
                    .AddWithValue(mc_strArroba & mc_numRep, p_numRep)
                    .AddWithValue(mc_strArroba & mc_cantPendiente, p_cantPendiente)
                    .AddWithValue(mc_strArroba & mc_cantRecibida, p_cantRecibida)


                End With

                cmdUpd.ExecuteNonQuery()


            Catch ex As SqlClient.SqlException
                MsgBox(ex.Message)
            Catch ex As Exception
                MsgBox(ex.Message)

            Finally

                Call m_cnn.Close()
            End Try
        End Function
        Public Function UpdateRepuestosXEstadoReq(ByVal dataSet As EstadoxRepuestosDataset _
                                                  , ByVal p_lineNumOr As Integer _
                                                  , ByVal p_numOrden As String _
                                                  , ByVal p_numRep As String _
                                                  , ByVal p_cantPendiente As Decimal _
                                                  , ByVal p_cantRecibida As Decimal)
            Dim m_trn As SqlClient.SqlTransaction

            Try
                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                m_trn = m_cnn.BeginTransaction
                m_adp.UpdateCommand = CrearCmdUpdRepuestosXEstadoReq()

                m_adp.UpdateCommand.Connection = m_cnn



                m_adp.UpdateCommand.Parameters(mc_strArroba & mc_lineNumOr).Value = p_lineNumOr
                m_adp.UpdateCommand.Parameters(mc_strArroba & mc_numOrden).Value = p_numOrden
                m_adp.UpdateCommand.Parameters(mc_strArroba & mc_numRep).Value = p_numRep
                m_adp.UpdateCommand.Parameters(mc_strArroba & mc_cantPendiente).Value = p_cantPendiente
                m_adp.UpdateCommand.Parameters(mc_strArroba & mc_cantRecibida).Value = p_cantRecibida

                m_adp.UpdateCommand.Transaction = m_trn

                Call m_adp.Update(dataSet.SCGTA_TB_RepuestosxEstado)





            Catch ex As SqlClient.SqlException
                MsgBox(ex.Message)
            Catch ex As Exception
                MsgBox(ex.Message)

                If Not m_trn Is Nothing Then
                    Call m_trn.Rollback()
                End If

            Finally
                If Not m_trn Is Nothing Then
                    Call m_trn.Commit()
                    Call m_trn.Dispose()
                    m_trn = Nothing
                End If
                Call m_cnn.Close()
            End Try
        End Function



        Public Function Update(ByVal dataSet As EstadoxRepuestosDataset)
            Dim m_trn As SqlClient.SqlTransaction

            Try
                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                m_trn = m_cnn.BeginTransaction
                m_adp.UpdateCommand = CrearCmdUpd()
                m_adp.InsertCommand = CrearCmdIns()
                m_adp.DeleteCommand = CrearCmdDel()
                m_adp.UpdateCommand.Connection = m_cnn
                m_adp.InsertCommand.Connection = m_cnn
                m_adp.DeleteCommand.Connection = m_cnn

                m_adp.DeleteCommand.Transaction = m_trn
                m_adp.UpdateCommand.Transaction = m_trn
                m_adp.InsertCommand.Transaction = m_trn

                Call m_adp.Update(dataSet.SCGTA_TB_RepuestosxEstado)


            Catch ex As SqlClient.SqlException
                MsgBox(ex.Message)
            Catch ex As Exception
                MsgBox(ex.Message)

                If Not m_trn Is Nothing Then
                    Call m_trn.Rollback()
                End If

            Finally
                If Not m_trn Is Nothing Then
                    Call m_trn.Commit()
                    Call m_trn.Dispose()
                    m_trn = Nothing
                End If
                Call m_cnn.Close()
            End Try
        End Function

        Public Function Update(ByVal dataSet As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable, _
                                ByRef p_cnnConeccion As SqlClient.SqlConnection, _
                                ByRef p_trnTransacion As SqlClient.SqlTransaction, _
                                ByVal p_blnEstadoPendiente As Boolean,
                                Optional ByVal p_blnIniciaTransaccion As Boolean = False)

            Try

                If p_blnIniciaTransaccion Then
                    p_cnnConeccion = New SqlClient.SqlConnection
                    If m_cnn.State = ConnectionState.Closed Then
                        If m_cnn.ConnectionString = "" Then
                            m_cnn.ConnectionString = strConexionADO
                        End If
                        Call m_cnn.Open()
                        p_cnnConeccion = m_cnn
                        p_trnTransacion = p_cnnConeccion.BeginTransaction(IsolationLevel.ReadCommitted)
                    Else
                        p_cnnConeccion = m_cnn
                        p_trnTransacion = p_cnnConeccion.BeginTransaction(IsolationLevel.ReadCommitted)
                    End If
                End If

                m_adp = New SqlClient.SqlDataAdapter

                If p_blnEstadoPendiente Then
                    m_adp.InsertCommand = CrearCmdUpdEstado()
                Else
                    m_adp.InsertCommand = CrearCmdUpdAdicional()
                End If

                m_adp.InsertCommand.Connection = p_cnnConeccion
                m_adp.InsertCommand.Transaction = p_trnTransacion

                Call m_adp.Update(dataSet)

            Catch ex As Exception
                Throw ex

            End Try
        End Function

        Public Sub Dispose() Implements System.IDisposable.Dispose
            Try
                If m_cnn.State = ConnectionState.Open Then
                    Call m_cnn.Close()
                    Call m_cnn.Dispose()
                    m_cnn = Nothing
                End If

                If Not m_adp Is Nothing Then
                    Call m_adp.Dispose()
                    m_adp = Nothing
                End If
            Catch ex As Exception

            End Try
        End Sub

        Public Function ValidarItemsPendientes(ByVal p_strNoOrden As String) As Integer
            Dim cmdItemsPendientes As New SqlClient.SqlCommand(mc_strSPSCGTA_SP_SelCountItemsPendientes, m_cnn)
            Dim intResult As Integer

            With cmdItemsPendientes

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden

            End With

            intResult = CInt(cmdItemsPendientes.ExecuteScalar)

            Return intResult

        End Function

#End Region

#Region "Commands "

        Private Function CrearCmdIns() As SqlClient.SqlCommand

            Dim cmdIns As SqlClient.SqlCommand

            Try

                cmdIns = New SqlClient.SqlCommand(mc_strSPInsRepuestosxEstado)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strIdRepuestosxOrden, SqlDbType.Int, 4, mc_strIdRepuestosxOrden)
                    .Add(mc_strArroba & mc_strCodEstadoRep, SqlDbType.Decimal, 5, mc_strCodEstadoRep)
                    .Add(mc_strArroba & mc_strCantidad, SqlDbType.Decimal, 9, mc_strCantidad)


                End With

                Return cmdIns
            Catch ex As Exception
            Finally
            End Try

        End Function

        Private Function CrearCmdDel() As SqlClient.SqlCommand

            Dim cmdDel As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try

                cmdDel = New SqlClient.SqlCommand(mc_strSPDelRepuestosxEstado)
                cmdDel.CommandType = CommandType.StoredProcedure

                With cmdDel.Parameters

                    .Add(mc_strArroba & mc_strIdRepuestosxOrden, SqlDbType.Int, 4, mc_strIdRepuestosxOrden)
                    .Add(mc_strArroba & mc_strCodEstadoRep, SqlDbType.Decimal, 5, mc_strCodEstadoRep)


                End With

                Return cmdDel
            Catch ex As Exception

            End Try

        End Function

        Private Function CrearCmdUpdRepuestosXEstadoReq() As SqlClient.SqlCommand

            Dim cmdUpd As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try

                cmdUpd = New SqlClient.SqlCommand(mc_strSCGTA_SP_ActualizaEstadoRepuestosRequisiciones)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters
                    .Add(mc_strArroba & mc_lineNumOr, SqlDbType.Int, 4)
                    .Add(mc_strArroba & mc_numOrden, SqlDbType.NVarChar, 30)
                    .Add(mc_strArroba & mc_numRep, SqlDbType.NVarChar, 100)
                    .Add(mc_strArroba & mc_cantPendiente, SqlDbType.Decimal, 19)
                    .Add(mc_strArroba & mc_cantRecibida, SqlDbType.Decimal, 19)


                End With

                Return cmdUpd
            Catch ex As Exception
            Finally
            End Try

        End Function


        Private Function CrearCmdUpd() As SqlClient.SqlCommand

            Dim cmdUpd As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try

                cmdUpd = New SqlClient.SqlCommand(mc_strSPUpdUpdRepuestosxEstado)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters

                    .Add(mc_strArroba & mc_strIdRepuestosxOrden, SqlDbType.Int, 4, mc_strIdRepuestosxOrden)
                    .Add(mc_strArroba & mc_strCodEstadoRep, SqlDbType.Decimal, 5, mc_strCodEstadoRep)
                    .Add(mc_strArroba & mc_strCantidad, SqlDbType.Decimal, 9, mc_strCantidad)

                End With

                Return cmdUpd
            Catch ex As Exception
            Finally
            End Try

        End Function

        Private Function CrearCmdUpdEstado() As SqlClient.SqlCommand

            Dim cmdUpd As SqlClient.SqlCommand
            'Dim param As SqlClient.SqlParameter

            Try

                cmdUpd = New SqlClient.SqlCommand("SCGTA_SP_UpdSetEstadoRepuestoRecibido")
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters

                    .Add(mc_strArroba & "LineNum", SqlDbType.Int, 4, "LineNum")
                    .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.NVarChar, 20, mc_strNoRepuesto)
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.NVarChar, 20, mc_strNoOrden)
                    .Add(mc_strArroba & mc_strCantidad, SqlDbType.Decimal, 9, mc_strCantidad)

                End With

                Return cmdUpd
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearCmdUpdAdicional() As SqlClient.SqlCommand

            Dim cmdUpd As SqlClient.SqlCommand
            'Dim param As SqlClient.SqlParameter

            Try

                cmdUpd = New SqlClient.SqlCommand("SCGTA_SP_UpdSetEstadoRepuestoAdicionalRecibido")
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters

                    .Add(mc_strArroba & "LineNum", SqlDbType.Int, 4, "LineNum")
                    .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.NVarChar, 20, mc_strNoRepuesto)
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.NVarChar, 20, mc_strNoOrden)
                    .Add(mc_strArroba & mc_strCantidad, SqlDbType.Decimal, 9, mc_strCantidad)

                End With

                Return cmdUpd
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearCmdSel() As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                cmdSel = New SqlClient.SqlCommand(mc_strSPSelRepuetosxEstado)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 20)
                    .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.VarChar, 20)
                    .Add(mc_strArroba & mc_strCodEstado, SqlDbType.Int, 4)
                    .Add(mc_strArroba & mc_strIdRepuestosxOrden, SqlDbType.Int, 4)

                End With

                Return cmdSel
            Catch ex As Exception

                MsgBox(ex.Message)
            End Try

        End Function

        Private Function CrearCmdSelxOrden() As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                cmdSel = New SqlClient.SqlCommand(mc_strSPSelRepuetosxEstadoxOrden)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 20)

                End With

                Return cmdSel
            Catch ex As Exception

                MsgBox(ex.Message)
            End Try

        End Function
#End Region


    End Class
End Namespace

