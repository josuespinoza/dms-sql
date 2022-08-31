Imports DMSOneFramework.SCGDataAccess.DAConexion
Namespace SCGDataAccess
    Public Class ListaAplicadaxOrdenDataAdapter

        Implements IDataAdapter, IDisposable

#Region "Declaraciones"

        'TODO Agregar nombres de procedimeintos almacenados para una determinada tabla
        Private Const mc_strSPInsListasxOrden As String = "SCGTA_SP_InsListasxOrden"
        Private Const mc_strSPInsListasAplicada As String = "SCGTA_SP_InsListaAplicada"

        Private Const mc_strSPUpdListasxOrden As String = "SCGTA_SP_UpdListasxOrden"
        Private Const mc_strSPUpdListasAplicada As String = "SCGTA_SP_UpdListaAplicada"

        Private Const mc_strSPSelListadexOrdenFase As String = "SCGTA_SP_SelListadexOrdenFase"
        Private Const mc_strSPSelListadexOrdenFaseNueva As String = "SCGTA_SP_SELCondicionesCalidadxFase"


        Private Const mc_strEstaLlaveExiste As String = ""
        Private Const mc_strSPDel As String = ""
        'TODO Agregar nombres de columnas de la tabla
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strNoLista As String = "NoLista"
        Private Const mc_strNoFase As String = "NoFase"
        Private Const mc_strObservaciones As String = "Observaciones"
        Private Const mc_strUsuarioModifico As String = "UsuarioModifico"

        Private Const mc_strNoCondicion As String = "NoCondicion"
        Private Const mc_strEstado As String = "Estado"

        'Declaracion de objetos de acceso a datos
        Private m_cnn As SqlClient.SqlConnection

        Private m_adpListaXOrden As SqlClient.SqlDataAdapter
        Private m_adpListaAplicada As SqlClient.SqlDataAdapter

        Private m_strConexion As String

        Private mc_strArroba As String = "@"

        Dim objDAConexion As DAConexion

#End Region

#Region "Inicializar AnalisisDataAdapter"

        Public Sub New()
            Try
                objDAConexion = New DAConexion
                m_cnn = objDAConexion.ObtieneConexion
                m_adpListaAplicada = New SqlClient.SqlDataAdapter
                m_adpListaXOrden = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try
        End Sub
#End Region

#Region "Implementaciones"
        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Overloads Function Fill(ByRef dataset As ListaAplicadaxOrdenDataset, _
                                       ByVal NOFase As Integer, _
                                       ByVal NoOrden As String, _
                                       ByVal NoLista As Integer) As Integer
            Try
                Call m_cnn.Open()

                m_adpListaXOrden.SelectCommand = CrearCmdSelListasxOrden()
                m_adpListaXOrden.SelectCommand.Connection = m_cnn

                m_adpListaXOrden.SelectCommand.Parameters(mc_strArroba & mc_strNoFase).Value = NOFase
                m_adpListaXOrden.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden
                m_adpListaXOrden.SelectCommand.Parameters(mc_strArroba & mc_strNoLista).Value = NoLista

                m_adpListaXOrden.TableMappings.Add("Table", dataset.SCGTA_TB_ListasxOrden.TableName)
                m_adpListaXOrden.TableMappings.Add("Table1", dataset.SCGTB_TA_ListaAplicada.TableName)

                dataset.SCGTB_TA_ListaAplicada.EstadoColumn.DefaultValue = True


                Call m_adpListaXOrden.Fill(dataset)

            Catch ex As Exception
                Throw ex
                Return 1
            Finally
                Call m_cnn.Close()
            End Try
        End Function

        Public Overloads Function Fill(ByRef datatable As ListaAplicadaxOrdenDataset.SCGTB_TA_ListaAplicadaDataTable, _
                                       ByVal NOFase As Integer) As Integer
            Try
                Call m_cnn.Open()

                m_adpListaXOrden.SelectCommand = CrearCmdSelListasCondicionesxFase()
                m_adpListaXOrden.SelectCommand.Connection = m_cnn

                datatable.NoFaseColumn.AllowDBNull = True
                datatable.DescripcionColumn.AllowDBNull = True
                datatable.EstadoColumn.AllowDBNull = True
                datatable.NoOrdenColumn.AllowDBNull = True
                datatable.NoListaColumn.AllowDBNull = True
                datatable.NoCondicionColumn.AllowDBNull = True

                'datatable.EstadoColumn.DefaultValue = True
                datatable.EstadoColumn.DefaultValue = False

                m_adpListaXOrden.SelectCommand.Parameters(mc_strArroba & mc_strNoFase).Value = NOFase

                Call m_adpListaXOrden.Fill(datatable)

            Catch ex As Exception
                Throw ex
                Return 1
            Finally
                Call m_cnn.Close()
            End Try
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

        Public Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

        Public Function Update(ByVal dataSet As ListaAplicadaxOrdenDataset) As Integer

            Dim m_trn As SqlClient.SqlTransaction =  Nothing

            Try

                Call m_cnn.Open()

                m_trn = m_cnn.BeginTransaction

                m_adpListaXOrden.InsertCommand = CrearCmdInsListasxOrden()
                m_adpListaXOrden.UpdateCommand = CrearCmdUpdListasxOrden()

                m_adpListaAplicada.InsertCommand = CrearCmdInsListasAplicada()
                m_adpListaAplicada.UpdateCommand = CrearCmdUpdListasAplicada()

                m_adpListaXOrden.UpdateCommand.Connection = m_cnn
                m_adpListaXOrden.InsertCommand.Connection = m_cnn

                m_adpListaAplicada.UpdateCommand.Connection = m_cnn
                m_adpListaAplicada.InsertCommand.Connection = m_cnn

                m_adpListaAplicada.UpdateCommand.Transaction = m_trn
                m_adpListaAplicada.InsertCommand.Transaction = m_trn

                m_adpListaXOrden.UpdateCommand.Transaction = m_trn
                m_adpListaXOrden.InsertCommand.Transaction = m_trn

                Call m_adpListaXOrden.Update(dataSet.SCGTA_TB_ListasxOrden)
                Call m_adpListaAplicada.Update(dataSet.SCGTB_TA_ListaAplicada)


            Catch ex As SqlClient.SqlException
                Throw ex
            Catch ex As Exception
                Throw ex
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


        Public Sub Dispose() Implements System.IDisposable.Dispose
            Try
                If m_cnn.State = ConnectionState.Open Then
                    Call m_cnn.Close()
                    Call m_cnn.Dispose()
                    m_cnn = Nothing
                End If

                If Not m_adpListaAplicada Is Nothing Then
                    Call m_adpListaAplicada.Dispose()
                    m_adpListaAplicada = Nothing
                End If

                If Not m_adpListaXOrden Is Nothing Then
                    Call m_adpListaXOrden.Dispose()
                    m_adpListaXOrden = Nothing
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region

#Region "Commands "
        Private Function CrearCmdInsListasxOrden() As SqlClient.SqlCommand

            Dim cmdIns As SqlClient.SqlCommand

            Try

                cmdIns = New SqlClient.SqlCommand(mc_strSPInsListasxOrden)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    .Add(mc_strArroba & mc_strNoLista, SqlDbType.Int, 4, mc_strNoLista)
                    .Add(mc_strArroba & mc_strNoFase, SqlDbType.Int, 4, mc_strNoFase)
                    .Add(mc_strArroba & mc_strObservaciones, SqlDbType.VarChar, 50, mc_strObservaciones)

                    'SE AGREGO EL USUARIO DEL SISTEMA
                    .Add(mc_strArroba & mc_strUsuarioModifico, SqlDbType.VarChar, 50, mc_strUsuarioModifico)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            Finally
            End Try

        End Function

        Private Function CrearCmdInsListasAplicada() As SqlClient.SqlCommand

            Dim cmdIns As SqlClient.SqlCommand

            Try

                cmdIns = New SqlClient.SqlCommand(mc_strSPInsListasAplicada)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    .Add(mc_strArroba & mc_strNoLista, SqlDbType.Int, 4, mc_strNoLista)
                    .Add(mc_strArroba & mc_strNoFase, SqlDbType.Int, 4, mc_strNoFase)
                    .Add(mc_strArroba & mc_strNoCondicion, SqlDbType.Int, 4, mc_strNoCondicion)
                    .Add(mc_strArroba & mc_strEstado, SqlDbType.Bit, 1, mc_strEstado)

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

                cmdDel = New SqlClient.SqlCommand(mc_strSPDel)
                cmdDel.CommandType = CommandType.StoredProcedure

                With cmdDel.Parameters


                    'TODO agregar campos para el comando de borrado


                End With

                Return cmdDel
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearCmdUpdListasxOrden() As SqlClient.SqlCommand

            Dim cmdUpd As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try

                cmdUpd = New SqlClient.SqlCommand(mc_strSPUpdListasxOrden)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters


                    param = .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    param.SourceVersion = DataRowVersion.Original

                    param = .Add(mc_strArroba & mc_strNoLista, SqlDbType.Int, 4, mc_strNoLista)
                    param.SourceVersion = DataRowVersion.Original

                    param = .Add(mc_strArroba & mc_strNoFase, SqlDbType.Int, 4, mc_strNoFase)
                    param.SourceVersion = DataRowVersion.Original

                    .Add(mc_strArroba & mc_strObservaciones, SqlDbType.VarChar, 50, mc_strObservaciones)

                    'SE AGREGO EL USUARIO DEL SISTEMA
                    .Add(mc_strArroba & mc_strUsuarioModifico, SqlDbType.VarChar, 50, mc_strUsuarioModifico)



                End With

                Return cmdUpd
            Catch ex As Exception
                Throw ex
            Finally
            End Try

        End Function

        Private Function CrearCmdUpdListasAplicada() As SqlClient.SqlCommand

            Dim cmdUpd As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try

                cmdUpd = New SqlClient.SqlCommand(mc_strSPUpdListasAplicada)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters


                    param = .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    param.SourceVersion = DataRowVersion.Original

                    param = .Add(mc_strArroba & mc_strNoLista, SqlDbType.Int, 4, mc_strNoLista)
                    param.SourceVersion = DataRowVersion.Original

                    param = .Add(mc_strArroba & mc_strNoFase, SqlDbType.Int, 4, mc_strNoFase)
                    param.SourceVersion = DataRowVersion.Original

                    param = .Add(mc_strArroba & mc_strNoCondicion, SqlDbType.Int, 4, mc_strNoCondicion)
                    param.SourceVersion = DataRowVersion.Original

                    .Add(mc_strArroba & mc_strEstado, SqlDbType.Bit, 1, mc_strEstado)


                End With

                Return cmdUpd
            Catch ex As Exception
                Throw ex
            Finally
            End Try

        End Function

        Private Function CrearCmdSelListasxOrden() As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                cmdSel = New SqlClient.SqlCommand(mc_strSPSelListadexOrdenFase)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    param = .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    param.SourceVersion = DataRowVersion.Original

                    param = .Add(mc_strArroba & mc_strNoLista, SqlDbType.Int, 4, mc_strNoLista)
                    param.SourceVersion = DataRowVersion.Original

                    param = .Add(mc_strArroba & mc_strNoFase, SqlDbType.Int, 4, mc_strNoFase)
                    param.SourceVersion = DataRowVersion.Original

                End With

                Return cmdSel
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearCmdSelListasCondicionesxFase() As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                cmdSel = New SqlClient.SqlCommand(mc_strSPSelListadexOrdenFaseNueva)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    param = .Add(mc_strArroba & mc_strNoFase, SqlDbType.Int, 4, mc_strNoFase)
                    param.SourceVersion = DataRowVersion.Original

                End With

                Return cmdSel
            Catch ex As Exception
                Throw ex
            End Try

        End Function
#End Region


    End Class
End Namespace


