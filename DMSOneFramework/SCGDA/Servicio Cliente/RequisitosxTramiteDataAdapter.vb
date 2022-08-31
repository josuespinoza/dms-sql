Namespace SCGDataAccess
    Public Class RequisitosxTramiteDataAdapter

        Implements IDataAdapter, IDisposable

#Region "Declaraciones"

        'TODO Agregar nombres de procedimeintos almacenados para una determinada tabla
        Private Const mc_strSPInsTramites As String = "SCGTA_SP_InsTramite"
        Private Const mc_strSPInsRequisitosxTramites As String = "SCGTA_SP_InsRequisitosxTramite"
        Private Const mc_strSPUpd As String = "SCGTA_SP_UpdRequisitosxTramite"
        Private Const mc_strSPUpdTramite As String = "SCGTA_SP_UpdTramite"
        Private Const mc_strSPDel As String = ""
        Private Const mc_strSPSelRequisitos As String = "SCGTA_SP_SelRequisitos1"
        Private Const mc_strSPSelTramiteyRequisitos As String = "SCGTA_SP_CargaTramiteExistente"
        Private Const mc_strEstaLlaveExiste As String = ""
        Private Const mc_strSPSelEstadoReq As String = "SCGTA_SP_SELEstadoReq"

        'Columnas de la Tabla de Tramites
        Private Const mc_strNoTramite As String = "NoTramite"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strCodEstadoTra As String = "CodEstadoTra"

        'Columna de la tabla de requisitos
        Private Const mc_strNoRequisito As String = "NoRequisito"
        Private Const mc_strCodEstadoReq As String = "CodEstadoReq"
        Private Const mc_strTipoRequisito As String = "TipoRequisito"
        Private Const mc_strRequerido As String = "Requerido"
        Private Const mc_strFecha As String = "Fecha"

        Private Const mc_strC As String = ""

        'Declaracion de objetos de acceso a datos
        Private m_cnnSCGTaller As SqlClient.SqlConnection
        Private m_adpTramite As SqlClient.SqlDataAdapter
        Private m_adpRequisitosxTramite As SqlClient.SqlDataAdapter

        Private m_strConexion As String

        Private mc_strArroba As String = "@"
        Dim objDAConexion As DAConexion


#End Region

#Region "Inicializar RequisitosxTramiteDataAdapter"

        Public Sub New()
            Try
                objDAConexion = New DAConexion
                m_cnnSCGTaller = objDAConexion.ObtieneConexion
                m_adpTramite = New SqlClient.SqlDataAdapter
                m_adpRequisitosxTramite = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
        End Sub
#End Region

#Region "Implementaciones"
        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Overloads Function Fill(ByRef dataset As RequesitoxtramiteDataset) As Integer
            Try
                Call m_cnnSCGTaller.Open()

                m_adpTramite.SelectCommand = CrearCmdSel()
                m_adpTramite.SelectCommand.Connection = m_cnnSCGTaller

                dataset.RequisitosPorTramite.CodEstadoReqColumn.AllowDBNull = True
                dataset.RequisitosPorTramite.Estado_RequisitoColumn.AllowDBNull = True
                dataset.RequisitosPorTramite.FechaColumn.AllowDBNull = True
                dataset.RequisitosPorTramite.FechaColumn.DefaultValue = System.DateTime.Now
                dataset.RequisitosPorTramite.RequeridoColumn.DefaultValue = True
                dataset.RequisitosPorTramite.RequisitoColumn.AllowDBNull = True



                dataset.RequisitosPorTramite.NoRequisitoColumn.ColumnMapping = MappingType.Hidden
                dataset.RequisitosPorTramite.CodEstadoReqColumn.ColumnMapping = MappingType.Hidden

                Call m_adpTramite.Fill(dataset.RequisitosPorTramite)

            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)
                Return 1
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
        End Function


        Public Overloads Function Fill(ByRef datatable As RequisitosyTramitesDataset.SCGTA_TB_RequisitosxTramiteDataTable) As Integer
            Try
                Call m_cnnSCGTaller.Open()

                m_adpTramite.SelectCommand = CrearCmdSel()
                m_adpTramite.SelectCommand.Connection = m_cnnSCGTaller

                datatable.NoOrdenColumn.AllowDBNull = True
                datatable.NoTramiteColumn.AllowDBNull = True
                datatable.CodEstadoReqColumn.AllowDBNull = True
                datatable.Estado_RequisitoColumn.AllowDBNull = True
                datatable.FechaColumn.AllowDBNull = True
                datatable.RequisitoColumn.AllowDBNull = True
                datatable.TipoRequisitoColumn.AllowDBNull = True
                datatable.RequeridoColumn.AllowDBNull = True
                datatable.NoRequisitoColumn.AllowDBNull = True

                datatable.FechaColumn.DefaultValue = System.DateTime.Now
                datatable.RequeridoColumn.DefaultValue = True
                ''''''''''''''''
                datatable.CheckColumn.DefaultValue = False

                datatable.NoRequisitoColumn.ColumnMapping = MappingType.Hidden
                datatable.CodEstadoReqColumn.ColumnMapping = MappingType.Hidden
                datatable.NoTramiteColumn.ColumnMapping = MappingType.Hidden
                datatable.NoOrdenColumn.ColumnMapping = MappingType.Hidden
                datatable.TipoRequisitoColumn.ColumnMapping = MappingType.Hidden

                Call m_adpTramite.Fill(datatable)

            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)
                Return 1
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
        End Function

        Public Overloads Function Fill(ByRef dataset As RequisitosyTramitesDataset, _
                                       ByVal NoTramite As String, _
                                       ByVal NoOrden As String) As Integer
            Try
                Call m_cnnSCGTaller.Open()

                m_adpTramite.SelectCommand = CrearCmdSelTramitesyRequisitos()
                m_adpTramite.SelectCommand.Connection = m_cnnSCGTaller

                dataset.SCGTA_TB_RequisitosxTramite.CodEstadoReqColumn.AllowDBNull = True
                dataset.SCGTA_TB_RequisitosxTramite.Estado_RequisitoColumn.AllowDBNull = True
                dataset.SCGTA_TB_RequisitosxTramite.FechaColumn.AllowDBNull = True
                dataset.SCGTA_TB_RequisitosxTramite.FechaColumn.DefaultValue = System.DateTime.Now
                dataset.SCGTA_TB_RequisitosxTramite.RequeridoColumn.DefaultValue = True
                dataset.SCGTA_TB_RequisitosxTramite.RequisitoColumn.AllowDBNull = True
                ''''''''''''''''
                dataset.SCGTA_TB_RequisitosxTramite.CheckColumn.DefaultValue = False

                dataset.SCGTA_TB_RequisitosxTramite.NoRequisitoColumn.ColumnMapping = MappingType.Hidden
                dataset.SCGTA_TB_RequisitosxTramite.CodEstadoReqColumn.ColumnMapping = MappingType.Hidden
                dataset.SCGTA_TB_RequisitosxTramite.NoTramiteColumn.ColumnMapping = MappingType.Hidden
                dataset.SCGTA_TB_RequisitosxTramite.NoOrdenColumn.ColumnMapping = MappingType.Hidden
                dataset.SCGTA_TB_RequisitosxTramite.TipoRequisitoColumn.ColumnMapping = MappingType.Hidden


                m_adpTramite.SelectCommand.Parameters(mc_strArroba & mc_strNoTramite).Value = NoTramite
                m_adpTramite.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden
                m_adpTramite.TableMappings.Add("Table", dataset.SCGTA_TB_Tramite.TableName)
                m_adpTramite.TableMappings.Add("Table1", dataset.SCGTA_TB_RequisitosxTramite.TableName)
                Call m_adpTramite.Fill(dataset)

            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)
                Return 1
            Finally
                Call m_cnnSCGTaller.Close()
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

        Public Function Update(ByVal dataSet As RequisitosyTramitesDataset) As Integer

            Dim m_trn As SqlClient.SqlTransaction
            Try
                Call m_cnnSCGTaller.Open()

                m_trn = m_cnnSCGTaller.BeginTransaction

                m_adpTramite.InsertCommand = CrearCmdInsTramite()
                m_adpTramite.InsertCommand.Connection = m_cnnSCGTaller

                m_adpRequisitosxTramite.InsertCommand = CrearCmdInsRequisitosxTramite()
                m_adpRequisitosxTramite.InsertCommand.Connection = m_cnnSCGTaller

                m_adpRequisitosxTramite.UpdateCommand = CrearCmdUpdRequisitosxTramite()
                m_adpRequisitosxTramite.UpdateCommand.Connection = m_cnnSCGTaller

                m_adpTramite.UpdateCommand = CrearCmdUpdTramite()
                m_adpTramite.UpdateCommand.Connection = m_cnnSCGTaller

                m_adpRequisitosxTramite.UpdateCommand.Transaction = m_trn
                m_adpTramite.UpdateCommand.Transaction = m_trn
                m_adpTramite.InsertCommand.Transaction = m_trn
                m_adpRequisitosxTramite.InsertCommand.Transaction = m_trn

                Call m_adpTramite.Update(dataSet.SCGTA_TB_Tramite)
                Call m_adpRequisitosxTramite.Update(dataSet.SCGTA_TB_RequisitosxTramite)

            Catch ex As SqlClient.SqlException
                Throw ex
                'MsgBox(ex.Message)
            Catch ex As Exception
                Throw ex
                'MsgBox(ex.Message)

                If Not m_trn Is Nothing Then
                    Call m_trn.Rollback()
                End If

            Finally
                If Not m_trn Is Nothing Then
                    Call m_trn.Commit()
                    Call m_trn.Dispose()
                    m_trn = Nothing
                End If
                Call m_cnnSCGTaller.Close()
            End Try
        End Function


        Public Sub Dispose() Implements System.IDisposable.Dispose
            Try
                If m_cnnSCGTaller.State = ConnectionState.Open Then
                    Call m_cnnSCGTaller.Close()
                    Call m_cnnSCGTaller.Dispose()
                    m_cnnSCGTaller = Nothing
                End If

                If Not m_adpTramite Is Nothing Then
                    Call m_adpTramite.Dispose()
                    m_adpTramite = Nothing
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub SelEstadoRequisitos(ByRef p_intCodigo As Integer, ByRef p_strDescripcion As String)
            'Selecciona el estado de los requisitos, el primero que aparezca en la tabla
            Dim cmd As New SqlClient.SqlCommand
            Dim drd As SqlClient.SqlDataReader


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
                    .CommandText = mc_strSPSelEstadoReq
                End With

                drd = cmd.ExecuteReader

                If drd.Read Then
                    p_intCodigo = drd.Item(0)
                    p_strDescripcion = drd.Item(1)
                End If


            Catch ex As Exception
                Throw ex


            Finally
                drd.Close()
                m_cnnSCGTaller.Close()

            End Try
        End Sub
#End Region

#Region "Commands "
        Private Function CrearCmdInsTramite() As SqlClient.SqlCommand

            Dim cmdIns As SqlClient.SqlCommand

            Try
                cmdIns = New SqlClient.SqlCommand(mc_strSPInsTramites)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoTramite, SqlDbType.VarChar, 35, mc_strNoTramite)
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    .Add(mc_strArroba & mc_strCodEstadoTra, SqlDbType.Int, 4, mc_strCodEstadoTra)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            Finally
            End Try

        End Function

        Private Function CrearCmdInsRequisitosxTramite() As SqlClient.SqlCommand

            Dim cmdDel As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                cmdDel = New SqlClient.SqlCommand(mc_strSPInsRequisitosxTramites)
                cmdDel.CommandType = CommandType.StoredProcedure

                With cmdDel.Parameters

                    .Add(mc_strArroba & mc_strNoTramite, SqlDbType.VarChar, 35, mc_strNoTramite)
                    .Add(mc_strArroba & mc_strNoRequisito, SqlDbType.Int, 4, mc_strNoRequisito)
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    .Add(mc_strArroba & mc_strCodEstadoReq, SqlDbType.Int, 4, mc_strCodEstadoReq)

                    .Add(mc_strArroba & mc_strTipoRequisito, SqlDbType.Char, 2, mc_strTipoRequisito)
                    .Add(mc_strArroba & mc_strRequerido, SqlDbType.Bit, 1, mc_strRequerido)
                    .Add(mc_strArroba & mc_strFecha, SqlDbType.SmallDateTime, 4, mc_strFecha)
                End With

                Return cmdDel
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearCmdUpdRequisitosxTramite() As SqlClient.SqlCommand

            Dim cmdUpd As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                cmdUpd = New SqlClient.SqlCommand(mc_strSPUpd)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters

                    param = .Add(mc_strArroba & mc_strNoTramite, SqlDbType.VarChar, 35, mc_strNoTramite)
                    param.SourceVersion = DataRowVersion.Original

                    param = .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    param.SourceVersion = DataRowVersion.Original

                    param = .Add(mc_strArroba & mc_strNoRequisito, SqlDbType.Int, 4, mc_strNoRequisito)
                    param.SourceVersion = DataRowVersion.Original

                    param = .Add(mc_strArroba & mc_strTipoRequisito, SqlDbType.Char, 2, mc_strTipoRequisito)
                    param.SourceVersion = DataRowVersion.Original

                    .Add(mc_strArroba & mc_strCodEstadoReq, SqlDbType.Int, 4, mc_strCodEstadoReq)

                    .Add(mc_strArroba & mc_strRequerido, SqlDbType.Bit, 1, mc_strRequerido)

                    .Add(mc_strArroba & mc_strFecha, SqlDbType.SmallDateTime, 4, mc_strFecha)

                End With

                Return cmdUpd
            Catch ex As Exception
                Throw ex
            Finally
            End Try

        End Function

        Private Function CrearCmdUpdTramite() As SqlClient.SqlCommand

            Dim cmdUpd As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                cmdUpd = New SqlClient.SqlCommand(mc_strSPUpdTramite)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters

                    param = .Add(mc_strArroba & mc_strNoTramite, SqlDbType.VarChar, 35, mc_strNoTramite)
                    param.SourceVersion = DataRowVersion.Original

                    param = .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    param.SourceVersion = DataRowVersion.Original
                
                    .Add(mc_strArroba & mc_strCodEstadoTra, SqlDbType.Int, 4, mc_strCodEstadoTra)

                End With

                Return cmdUpd
            Catch ex As Exception
                Throw ex
            Finally
            End Try

        End Function

        Private Function CrearCmdSel() As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                cmdSel = New SqlClient.SqlCommand(mc_strSPSelRequisitos)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters


                End With

                Return cmdSel
            Catch ex As Exception
                Throw ex
            End Try

        End Function
        Private Function CrearCmdSelTramitesyRequisitos() As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                cmdSel = New SqlClient.SqlCommand(mc_strSPSelTramiteyRequisitos)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNoTramite, SqlDbType.VarChar, 35)
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
                End With

                Return cmdSel
            Catch ex As Exception
                Throw ex
            End Try


        End Function
#End Region


    End Class

End Namespace
