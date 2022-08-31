Namespace SCGDataAccess
    Public Class RepuestosProveeduriaDataAdapter

        Implements IDataAdapter, IDisposable

#Region "Declaraciones"

        Private Const mc_strSPInsRepuestosProveeduria As String = "SCGTA_SP_InsRepuestosxOrden_Proveduria"
        Private Const mc_strSPUpdRepuestosProveeduria As String = "SCGTA_SP_UpdRepuestosxOrden_Proveduria"
        Private Const mc_strSPDelRepuestosProveeduria As String = "SCGTA_SP_DelRepuestosxOrden_Proveduria"
        Private Const mc_strSPSelRepuestosProveeduria As String = "SCGTA_SP_SelTrackingRepuestos"
        Private Const mc_strSPSelRegistroFuenteTrackingRepuestos As String = "SCGTA_SP_SelRegistroFuenteTrackingRepuestos"
        Private Const mc_strSCGTA_SP_SelTotalTrackingxRepuesto As String = "SCGTA_SP_SelTotalTrackingxRepuesto"
        Private Const mc_strEstaLlaveExiste As String = ""

        Private Const mc_strPkRepuestoxOrdenesdeCompraPro As String = "PkRepuestoxOrdenesdeCompraPro"
        Private Const mc_strNoRepuesto As String = "NoRepuesto"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strFechaSolicitud As String = "FechaSolicitud"
        Private Const mc_strFechaCompromiso As String = "FechaCompromiso"
        Private Const mc_strFechaEntrega As String = "FechaEntrega"
        Private Const mc_strCardCode As String = "CardCode"
        Private Const mc_strCantSolicitados As String = "CantSolicitados"
        Private Const mc_strCantSuministrados As String = "CantSuministrados"
        Private Const mc_strNoAdicional As String = "NoAdicional"
        Private Const mc_strNoOrdendeCompra As String = "NoOrdendeCompra"
        Private Const mc_strNoFactura As String = "NoFactura"
        Private Const mc_strCostoRepuesto As String = "CostoRepuesto"
        Private Const mc_strPrecioCompraReal As String = "PrecioCompraReal"
        Private Const mc_strPrecioCompraDesc As String = "MontoDesc"
        Private Const mc_strDescuento As String = "Descuento"
        Private Const mc_strObservaciones As String = "Observaciones"
        Private Const mc_strIdRepuestosxOrden As String = "IdRepuestosxOrden"
        Private Const mc_strNoLinea As String = "NoLinea"


        'TODO Agregar nombres de columnas de la tabla
        Private Const mc_str As String = ""
        Private Const mc_strB As String = ""
        Private Const mc_strP As String = ""
        Private Const mc_strC As String = ""

        'Declaracion de objetos de acceso a datos
        Private m_cnn As SqlClient.SqlConnection
        Private m_adp As SqlClient.SqlDataAdapter

        Private m_strConexion As String

        Private Const mc_strArroba As String = "@"

        Private Shared objDAConexion As New DAConexion

#End Region

#Region "Inicializar AnalisisDataAdapter"

        Public Sub New()
            Try
                'm_strConexion = conexion
                m_cnn = objDAConexion.ObtieneConexion  'New SqlClient.SqlConnection(conexion)
                m_adp = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        Public Sub New(ByVal conexion As String)
            Try
                m_strConexion = conexion
                m_cnn = New SqlClient.SqlConnection(conexion)
                m_adp = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub
#End Region

#Region "Implementaciones"
        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Overloads Function Fill(ByRef dataset As RepuestosProveduriaDataset, _
                                       ByVal IdRepuestoxOrden As Integer) As Integer
            Try
                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                'If UltimaFechaCompromiso Then

                '    m_adp.SelectCommand = CrearCmdSel(mc_strSPSelRegistroFuenteTrackingRepuestos)

                'Else

                m_adp.SelectCommand = CrearCmdSel(mc_strSPSelRepuestosProveeduria)

                'End If

                m_adp.SelectCommand.Connection = m_cnn

                'm_adp.SelectCommand.Parameters(mc_strArroba & mc_strNoRepuesto).Value = NoRepuesto
                'm_adp.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden

                m_adp.SelectCommand.Parameters(mc_strArroba & mc_strIdRepuestosxOrden).Value = IdRepuestoxOrden


                With dataset.SCGTA_TB_RepuestosxOrden_Proveduria

                    '.FechaCompromisoColumn.Expression = "IIf(max(FechaCompromiso)>system.datetime.now, CantSolicitados = 1, CantSolicitados = 0)"
                    .CantSolicitadosColumn.AllowDBNull = True
                    .CantSuministradosColumn.AllowDBNull = True
                    .CardCodeColumn.AllowDBNull = True
                    .CostoRepuestoColumn.AllowDBNull = True
                    .DescuentoColumn.AllowDBNull = True
                    .FechaCompromisoColumn.AllowDBNull = True
                    .FechaEntregaColumn.AllowDBNull = True
                    .FechaSolicitudColumn.AllowDBNull = True
                    .NoAdicionalColumn.AllowDBNull = True
                    .NoFacturaColumn.AllowDBNull = True
                    .NoOrdenColumn.AllowDBNull = True
                    .NoOrdendeCompraColumn.AllowDBNull = True
                    .NoRepuestoColumn.AllowDBNull = True
                    .PkRepuestoxOrdenesdeCompraProColumn.AllowDBNull = True
                    .MontoDescColumn.AllowDBNull = True
                    .PrecioCompraRealColumn.AllowDBNull = True

                    .NoFacturaColumn.ColumnMapping = MappingType.Hidden
                    .NoOrdenColumn.ColumnMapping = MappingType.Hidden
                    .NoOrdendeCompraColumn.ColumnMapping = MappingType.Hidden
                    .NoRepuestoColumn.ColumnMapping = MappingType.Hidden
                    .PkRepuestoxOrdenesdeCompraProColumn.ColumnMapping = MappingType.Hidden

                End With

                Call m_adp.Fill(dataset.SCGTA_TB_RepuestosxOrden_Proveduria)

                Return dataset.SCGTA_TB_RepuestosxOrden_Proveduria.Rows.Count

            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)
                Return -1
            Finally
                Call m_cnn.Close()
            End Try
        End Function

        Public Overloads Function Fill(ByRef dataset As RepuestosProveduriaDataset, _
                                       ByVal NoRepuesto As String, _
                                       ByVal NoOrden As String, _
                                       ByVal IdRepuestoxOrden As Integer) As Integer
            Try
                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If


                m_adp.SelectCommand = CrearCmdSel()
                'End If

                m_adp.SelectCommand.Connection = m_cnn

                m_adp.SelectCommand.Parameters(mc_strArroba & mc_strNoRepuesto).Value = NoRepuesto
                m_adp.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden
                m_adp.SelectCommand.Parameters(mc_strArroba & mc_strIdRepuestosxOrden).Value = IdRepuestoxOrden

                With dataset.SCGTA_TB_RepuestosxOrden_Proveduria

                    '.FechaCompromisoColumn.Expression = "IIf(max(FechaCompromiso)>system.datetime.now, CantSolicitados = 1, CantSolicitados = 0)"
                    .CantSolicitadosColumn.AllowDBNull = True
                    .CantSuministradosColumn.AllowDBNull = True
                    .CardCodeColumn.AllowDBNull = True
                    .CostoRepuestoColumn.AllowDBNull = True
                    .DescuentoColumn.AllowDBNull = True
                    .FechaCompromisoColumn.AllowDBNull = True
                    .FechaEntregaColumn.AllowDBNull = True
                    .FechaSolicitudColumn.AllowDBNull = True
                    .NoAdicionalColumn.AllowDBNull = True
                    .NoFacturaColumn.AllowDBNull = True
                    .NoOrdenColumn.AllowDBNull = True
                    .NoOrdendeCompraColumn.AllowDBNull = True
                    .NoRepuestoColumn.AllowDBNull = True
                    .PkRepuestoxOrdenesdeCompraProColumn.AllowDBNull = True
                    .MontoDescColumn.AllowDBNull = True
                    .PrecioCompraRealColumn.AllowDBNull = True

                    .NoFacturaColumn.ColumnMapping = MappingType.Hidden
                    .NoOrdenColumn.ColumnMapping = MappingType.Hidden
                    .NoOrdendeCompraColumn.ColumnMapping = MappingType.Hidden
                    .NoRepuestoColumn.ColumnMapping = MappingType.Hidden
                    .PkRepuestoxOrdenesdeCompraProColumn.ColumnMapping = MappingType.Hidden

                End With

                Call m_adp.Fill(dataset.SCGTA_TB_RepuestosxOrden_Proveduria)

                Return dataset.SCGTA_TB_RepuestosxOrden_Proveduria.Rows.Count

            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)
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


        Public Function Update1(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

        Public Function Update(ByVal dataSet As RepuestosProveduriaDataset) As Integer
            Dim m_trn As SqlClient.SqlTransaction

            Try

                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                m_trn = m_cnn.BeginTransaction
                m_adp.UpdateCommand = CrearCmdUpd()
                m_adp.InsertCommand = CrearCmdIns()
                m_adp.UpdateCommand.Connection = m_cnn
                m_adp.InsertCommand.Connection = m_cnn
                m_adp.UpdateCommand.Transaction = m_trn
                m_adp.InsertCommand.Transaction = m_trn

                Call m_adp.Update(dataSet.SCGTA_TB_RepuestosxOrden_Proveduria)

                Return dataSet.SCGTA_TB_RepuestosxOrden_Proveduria.Rows.Count
            Catch ex As SqlClient.SqlException
                Throw ex
                'MsgBox(ex.Message)

                If Not m_trn Is Nothing Then
                    Call m_trn.Rollback()
                End If
                Return -1
            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)

                If Not m_trn Is Nothing Then
                    Call m_trn.Rollback()
                End If
                Return -1
            Finally
                If Not m_trn Is Nothing Then
                    Call m_trn.Commit()
                    Call m_trn.Dispose()
                    m_trn = Nothing
                End If
                Call m_cnn.Close()
            End Try
        End Function

        Public Function Update(ByVal dataSet As DataSet) As Integer


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
                Throw ex
            End Try
        End Sub

        Public Sub InsertarLineaTracking(ByVal p_strEstadoNuevo As String, ByVal p_strEstadoActual As String, ByVal p_intCantidad As Integer, _
                                        ByVal p_intNoRepuesto As Integer, ByVal p_strNoOrden As String, ByVal p_strUsuario As String)
            'Se utiliza para insertar una nueva linea en la TBRepuestosXOrden_Proveeduria 
            'cuando se cambia el estado de un repuesto de modo manual
            Dim cmdInsertar As New SqlClient.SqlCommand
            Dim strObservaciones As String

            'strObservaciones = "Estado del repuesto cambiado manualmente por el usuario: " & p_strUsuario & "." & vbCrLf & p_intCantidad & " artículo(s) cambiado(s) del estado " & p_strEstadoActual & " al estado " & p_strEstadoNuevo

            Try
                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                cmdInsertar.CommandType = CommandType.StoredProcedure
                cmdInsertar.CommandText = mc_strSPInsRepuestosProveeduria
                cmdInsertar.Connection = m_cnn

                With cmdInsertar.Parameters

                    .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.VarChar, 50).Value = p_intNoRepuesto
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 25).Value = p_strNoOrden
                    .Add(mc_strArroba & mc_strNoFactura, SqlDbType.Int, 4).Value = 0
                    .Add(mc_strArroba & mc_strFechaSolicitud, SqlDbType.DateTime, 9).Value = Today
                    .Add(mc_strArroba & mc_strFechaCompromiso, SqlDbType.DateTime, 9).Value = System.DBNull.Value
                    .Add(mc_strArroba & mc_strFechaEntrega, SqlDbType.DateTime, 9).Value = System.DBNull.Value
                    .Add(mc_strArroba & mc_strCardCode, SqlDbType.VarChar, 15).Value = "0"
                    .Add(mc_strArroba & mc_strCantSolicitados, SqlDbType.Decimal, 9).Value = 0
                    .Add(mc_strArroba & mc_strCantSuministrados, SqlDbType.Decimal, 9).Value = 0
                    .Add(mc_strArroba & mc_strNoAdicional, SqlDbType.Int, 4).Value = 0
                    .Add(mc_strArroba & mc_strNoOrdendeCompra, SqlDbType.Int, 4).Value = 0
                    .Add(mc_strArroba & mc_strCostoRepuesto, SqlDbType.Decimal, 9).Value = 0
                    .Add(mc_strArroba & mc_strPrecioCompraReal, SqlDbType.Decimal, 9).Value = 0
                    .Add(mc_strArroba & mc_strPrecioCompraDesc, SqlDbType.Decimal, 9).Value = 0
                    .Add(mc_strArroba & mc_strDescuento, SqlDbType.Decimal, 9).Value = 0
                    .Add(mc_strArroba & mc_strObservaciones, SqlDbType.VarChar, 200).Value = strObservaciones
                    .Add(mc_strArroba & mc_strIdRepuestosxOrden, SqlDbType.Int, 4).Value = System.DBNull.Value

                End With

                cmdInsertar.ExecuteNonQuery()


            Catch ex As Exception
                Throw ex

            Finally
                m_cnn.Close()
            End Try

        End Sub
#End Region

#Region "Commands "
        Private Function CrearCmdIns() As SqlClient.SqlCommand

            Dim cmdIns As SqlClient.SqlCommand

            Try

                cmdIns = New SqlClient.SqlCommand(mc_strSPInsRepuestosProveeduria)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.VarChar, 50, mc_strNoRepuesto)
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 25, mc_strNoOrden)
                    .Add(mc_strArroba & mc_strNoFactura, SqlDbType.VarChar, 50, mc_strNoFactura)
                    .Add(mc_strArroba & mc_strFechaSolicitud, SqlDbType.DateTime, 9, mc_strFechaSolicitud)
                    .Add(mc_strArroba & mc_strFechaCompromiso, SqlDbType.DateTime, 9, mc_strFechaCompromiso)
                    .Add(mc_strArroba & mc_strFechaEntrega, SqlDbType.DateTime, 9, mc_strFechaEntrega)
                    .Add(mc_strArroba & mc_strCardCode, SqlDbType.VarChar, 15, mc_strCardCode)
                    .Add(mc_strArroba & mc_strCantSolicitados, SqlDbType.Decimal, 9, mc_strCantSolicitados)
                    .Add(mc_strArroba & mc_strCantSuministrados, SqlDbType.Decimal, 9, mc_strCantSuministrados)
                    .Add(mc_strArroba & mc_strNoAdicional, SqlDbType.Int, 4, mc_strNoAdicional)
                    .Add(mc_strArroba & mc_strNoOrdendeCompra, SqlDbType.VarChar, 50, mc_strNoOrdendeCompra)
                    .Add(mc_strArroba & mc_strCostoRepuesto, SqlDbType.Decimal, 9, mc_strCostoRepuesto)
                    .Add(mc_strArroba & mc_strPrecioCompraReal, SqlDbType.Decimal, 9, mc_strPrecioCompraReal)
                    .Add(mc_strArroba & mc_strPrecioCompraDesc, SqlDbType.Decimal, 9, mc_strPrecioCompraDesc)
                    .Add(mc_strArroba & mc_strDescuento, SqlDbType.Decimal, 9, mc_strDescuento)
                    .Add(mc_strArroba & mc_strObservaciones, SqlDbType.VarChar, 200, mc_strObservaciones)
                    .Add(mc_strArroba & mc_strIdRepuestosxOrden, SqlDbType.Int, 4, mc_strIdRepuestosxOrden)


                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            Finally
            End Try

        End Function

        Private Function CrearCmdDel() As SqlClient.SqlCommand

            Dim cmdDel As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try

                'cmdDel = New SqlClient.SqlCommand(mc_strSPDel)
                cmdDel.CommandType = CommandType.StoredProcedure

                With cmdDel.Parameters


                    'TODO agregar campos para el comando de borrado


                End With

                Return cmdDel
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearCmdUpd() As SqlClient.SqlCommand

            Dim cmdUpd As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try

                cmdUpd = New SqlClient.SqlCommand(mc_strSPUpdRepuestosProveeduria)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters

                    param = .Add(mc_strArroba & mc_strPkRepuestoxOrdenesdeCompraPro, SqlDbType.Int, 4, mc_strPkRepuestoxOrdenesdeCompraPro)
                    param.SourceVersion = DataRowVersion.Original

                    param = .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.VarChar, 50, mc_strNoRepuesto)
                    'param.SourceVersion = DataRowVersion.Original

                    param = .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 25, mc_strNoOrden)
                    'param.SourceVersion = DataRowVersion.Original

                    .Add(mc_strArroba & mc_strNoFactura, SqlDbType.VarChar, 50, mc_strNoFactura)
                    .Add(mc_strArroba & mc_strFechaSolicitud, SqlDbType.DateTime, 9, mc_strFechaSolicitud)
                    .Add(mc_strArroba & mc_strFechaCompromiso, SqlDbType.DateTime, 9, mc_strFechaCompromiso)
                    .Add(mc_strArroba & mc_strFechaEntrega, SqlDbType.DateTime, 9, mc_strFechaEntrega)
                    .Add(mc_strArroba & mc_strCardCode, SqlDbType.VarChar, 15, mc_strCardCode)
                    .Add(mc_strArroba & mc_strCantSolicitados, SqlDbType.Decimal, 9, mc_strCantSolicitados)
                    .Add(mc_strArroba & mc_strCantSuministrados, SqlDbType.Decimal, 9, mc_strCantSuministrados)
                    .Add(mc_strArroba & mc_strNoAdicional, SqlDbType.Int, 4, mc_strNoAdicional)
                    .Add(mc_strArroba & mc_strNoOrdendeCompra, SqlDbType.VarChar, 50, mc_strNoOrdendeCompra)
                    .Add(mc_strArroba & mc_strCostoRepuesto, SqlDbType.Decimal, 9, mc_strCostoRepuesto)
                    .Add(mc_strArroba & mc_strPrecioCompraReal, SqlDbType.Decimal, 9, mc_strPrecioCompraReal)
                    .Add(mc_strArroba & mc_strPrecioCompraDesc, SqlDbType.Decimal, 9, mc_strPrecioCompraDesc)
                    .Add(mc_strArroba & mc_strDescuento, SqlDbType.Decimal, 9, mc_strDescuento)
                    .Add(mc_strArroba & mc_strObservaciones, SqlDbType.VarChar, 200, mc_strObservaciones)
                    .Add(mc_strArroba & mc_strIdRepuestosxOrden, SqlDbType.Int, 4, mc_strIdRepuestosxOrden)


                End With

                Return cmdUpd
            Catch ex As Exception
                Throw ex
            Finally
            End Try

        End Function

        Private Function CrearCmdSel(ByVal strStoreProcedure As String) As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                cmdSel = New SqlClient.SqlCommand(strStoreProcedure)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    '.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
                    '.Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.VarChar, 20)

                    .Add(mc_strArroba & mc_strIdRepuestosxOrden, SqlDbType.Int, 4)


                End With

                Return cmdSel
            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearCmdSel() As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                cmdSel = New SqlClient.SqlCommand(mc_strSPSelRegistroFuenteTrackingRepuestos)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.VarChar, 20)
                    .Add(mc_strArroba & mc_strIdRepuestosxOrden, SqlDbType.Int, 4)

                End With

                Return cmdSel
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Shared Function DevuelveCantidadDeTracks(ByVal NoRepuesto As Integer, _
                                                 ByVal NoOrdenDetrabajo As String, _
                                                 ByRef CantidadDeRegistrosEnTracking As Integer, _
                                                 ByVal CadenaDeConexion As String) As Boolean
            Dim cmdSel As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter
            Dim m_cnnSCGTaller As SqlClient.SqlConnection

            Try
                If CadenaDeConexion <> "" Then
                    m_cnnSCGTaller = New SqlClient.SqlConnection(CadenaDeConexion)
                Else
                    m_cnnSCGTaller = objDAConexion.ObtieneConexion
                End If

                cmdSel = New SqlClient.SqlCommand(mc_strSCGTA_SP_SelTotalTrackingxRepuesto)
                cmdSel.CommandType = CommandType.StoredProcedure

                cmdSel.Connection = m_cnnSCGTaller

                If Not m_cnnSCGTaller Is Nothing _
                    And m_cnnSCGTaller.State = ConnectionState.Closed Then

                    Call m_cnnSCGTaller.Open()

                End If

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.Int, 4)

                End With

                cmdSel.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrdenDetrabajo

                cmdSel.Parameters(mc_strArroba & mc_strNoRepuesto).Value = NoRepuesto


                CantidadDeRegistrosEnTracking = CInt(cmdSel.ExecuteScalar)

            Catch ex As Exception
                Throw ex
                'Call MsgBox(ex.Message)

            Finally
                If Not m_cnnSCGTaller Is Nothing Then
                    Call m_cnnSCGTaller.Close()
                End If

            End Try

        End Function


#End Region


    End Class
End Namespace


