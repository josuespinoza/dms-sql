Namespace SCGDataAccess

    Public Class ActividadesXProcesoDataAdapter
        Implements IDataAdapter

#Region "Implementaciones"
        Public Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

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
#End Region

#Region "Declaraciones"

        ''Objetos
        Private m_adpActividadesXProceso As SqlClient.SqlDataAdapter
        Private m_cnnSCGTaller As SqlClient.SqlConnection
        Private objDAConexion As DAConexion

        ''Parametros
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strNoProceso As String = "NoProceso"
        Private Const mc_strNoActividad As String = "NoActividad"
        Private Const mc_strNoFase As String = "NoFase"
        Private Const mc_strNuevo As String = "Nuevo"
        Private Const mc_strPintar As String = "Pintar"
        Private Const mc_strId As String = "ID"

        ''Store Procedures
        Private Const mc_strSCGTA_SP_INSActividadesXProceso As String = "SCGTA_SP_INSActividadesXProceso"
        Private Const mc_strSCGTA_SP_INSProceso As String = "SCGTA_SP_INSProceso"
        Private Const mc_strSCGTA_SP_SELActividadesXProceso As String = "SCGTA_SP_SELActividadesXProceso"
        Private Const mc_strSCGTA_SP_DELActividadesXProceso As String = "SCGTA_SP_DELActividadesXProceso"
        Private Const mc_strSCGTA_SP_UPDActividadesXProceso As String = "SCGTA_SP_UPDActividadesXProceso"
        Private Const mc_strSCGTA_SP_SELVWActividadesXProceso As String = "SCGTA_SP_SELVWActividadesXProceso"
        Private Const mc_strSCGTA_SP_SELProcesosByChecks As String = "SCGTA_SP_SELProcesosByChecks"

        Private Const mc_strArroba As String = "@"

#End Region

#Region "Inicializa ActividadesXProcesoDataAdapter"

        Public Sub New()
            Call InicializaActividadesXProcesoDataAdapter(m_cnnSCGTaller)
        End Sub

        Private Sub InicializaActividadesXProcesoDataAdapter(ByRef cnnTaller As SqlClient.SqlConnection)

            Try


                objDAConexion = New DAConexion
                cnnTaller = objDAConexion.ObtieneConexion
                m_adpActividadesXProceso = New SqlClient.SqlDataAdapter


            Catch ex As Exception
                MsgBox(ex.Message)

            End Try
        End Sub

#End Region

#Region "Implementaciones SCG"

        Public Function InsActividadYProceso(ByRef p_Dataset As ActividadesXProcesoDataset, _
                                             ByVal p_strDescripcion As String,ByVal p_intProyectoNuevo AS Integer) As Integer

            Dim cmdProcesos As SqlClient.SqlCommand
            Dim intIDProceso As Integer
            Dim trnActividadesXProcesos As SqlClient.SqlTransaction =  Nothing


            Try


                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                trnActividadesXProcesos = m_cnnSCGTaller.BeginTransaction


                'TODO crear logica para instanciar el dataadapter
                m_adpActividadesXProceso.InsertCommand = CrearActXProcInsertCommand()
                m_adpActividadesXProceso.InsertCommand.Connection = m_cnnSCGTaller
                ' delete
                m_adpActividadesXProceso.DeleteCommand = CrearActXProcDeleteCommand()
                m_adpActividadesXProceso.DeleteCommand.Connection = m_cnnSCGTaller
                'update
                m_adpActividadesXProceso.UpdateCommand = CrearActXProcUpdateCommand()
                m_adpActividadesXProceso.UpdateCommand.Connection = m_cnnSCGTaller

                m_adpActividadesXProceso.InsertCommand.Transaction = trnActividadesXProcesos
                m_adpActividadesXProceso.DeleteCommand.Transaction = trnActividadesXProcesos
                m_adpActividadesXProceso.UpdateCommand.Transaction = trnActividadesXProcesos

                If p_intProyectoNuevo = 0 Then

                    'Prepara el objeto comando para ingresar el proceso
                    cmdProcesos = CrearProcesoInsertCommand()
                    cmdProcesos.Connection = m_cnnSCGTaller
                    cmdProcesos.Parameters(mc_strArroba & mc_strDescripcion).Value = p_strDescripcion
                    cmdProcesos.Transaction = trnActividadesXProcesos

                    cmdProcesos.ExecuteNonQuery()

                    intIDProceso = cmdProcesos.Parameters(mc_strArroba & mc_strNoProceso).Value()
                Else
                    intIDProceso = p_intProyectoNuevo
                End If


                'TODO llamar al metodo que asigna el id del proceso a las actividades

                AsignaActividadesXProceso(p_Dataset.SCGTA_TB_ActividadesXProcesos, intIDProceso)

                'm_adpActividadesXProceso.InsertCommand.Parameters(mc_strArroba & mc_strNoProceso).Value = intIDProceso

                Call m_adpActividadesXProceso.Update(p_Dataset.SCGTA_TB_ActividadesXProcesos)

                Call trnActividadesXProcesos.Commit()

                Return intIDProceso

            Catch ex As Exception

                If Not trnActividadesXProcesos Is Nothing Then
                    trnActividadesXProcesos.Rollback()
                End If
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try
        End Function

        Public Function InsActividadYProceso(ByRef p_Dataset As ActividadesXProcesoDataset) As Integer
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpActividadesXProceso.InsertCommand = CrearActXProcInsertCommand()
                m_adpActividadesXProceso.InsertCommand.Connection = m_cnnSCGTaller

                m_adpActividadesXProceso.DeleteCommand = CrearActXProcDeleteCommand()
                m_adpActividadesXProceso.DeleteCommand.Connection = m_cnnSCGTaller

                m_adpActividadesXProceso.Update(p_Dataset.SCGTA_TB_ActividadesXProcesos)

            Catch ex As Exception

                Throw ex

            Finally

                m_cnnSCGTaller.Close()

            End Try
        End Function

        Public Sub SeleccActividadesXProceso(ByRef p_Dataset As ActividadesXProcesoDataset, ByVal p_intNoProceso As Integer)
            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpActividadesXProceso.SelectCommand = CrearSelectCommand()

                m_adpActividadesXProceso.SelectCommand.Parameters(mc_strArroba & mc_strNoProceso).Value = p_intNoProceso

                m_adpActividadesXProceso.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpActividadesXProceso.Fill(p_Dataset.SCGTA_TB_ActividadesXProcesos)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
        End Sub

        'Revisar si en la vista ya existe un determinado proceso
        Public Function ExisteProceso(ByVal NoProceso As Integer, ByVal nuevo As Integer, ByVal pintar As Integer) As Boolean

            Dim cmdProceso As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELVWActividadesXProceso)
            Dim trnActividadesXProcesos As SqlClient.SqlTransaction =  Nothing
            Dim blnResultado As Boolean


            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                trnActividadesXProcesos = m_cnnSCGTaller.BeginTransaction

                cmdProceso.CommandType = CommandType.StoredProcedure

                With cmdProceso.Parameters

                    .Add(mc_strArroba & mc_strNoProceso, SqlDbType.Int, 4)
                    .Add(mc_strArroba & mc_strNuevo, SqlDbType.Bit, 1)
                    .Add(mc_strArroba & mc_strPintar, SqlDbType.Bit, 1)
                End With

                cmdProceso.Connection = m_cnnSCGTaller
                cmdProceso.Parameters(mc_strArroba & mc_strNoProceso).Value = NoProceso
                cmdProceso.Parameters(mc_strArroba & mc_strNuevo).Value = nuevo
                cmdProceso.Parameters(mc_strArroba & mc_strPintar).Value = pintar
                cmdProceso.Transaction = trnActividadesXProcesos

                blnResultado = cmdProceso.ExecuteScalar()


                Call trnActividadesXProcesos.Commit()

                Return blnResultado

            Catch ex As Exception
                If Not trnActividadesXProcesos Is Nothing Then
                    trnActividadesXProcesos.Rollback()
                End If
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        'Public Function ListaProcedimientos(ByVal p_blnNuevo As Boolean, ByVal p_blnPintar As Boolean) As SqlClient.SqlDataReader
        '    Dim cmdProceso As SqlClient.SqlCommand
        '    Dim drdTmpProcesos As SqlClient.SqlDataReader

        '    Try
        '        If m_cnnSCGTaller.State = ConnectionState.Closed Then
        '            If m_cnnSCGTaller.ConnectionString = "" Then
        '                m_cnnSCGTaller.ConnectionString = strConexionADO
        '            End If
        '            m_cnnSCGTaller.Open()
        '        End If

        '        cmdProceso = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELProcesosByChecks, m_cnnSCGTaller)
        '        cmdProceso.CommandType = CommandType.StoredProcedure

        '        With cmdProceso.Parameters

        '            .Add(mc_strArroba & mc_strNuevo, SqlDbType.Bit, 1).Value = p_blnNuevo
        '            .Add(mc_strArroba & mc_strPintar, SqlDbType.Bit, 1).Value = p_blnPintar

        '        End With

        '        drdTmpProcesos = cmdProceso.ExecuteReader(CommandBehavior.CloseConnection)

        '        Return drdTmpProcesos

        '    Catch ex As Exception
        '        Throw ex

        '    Finally

        '        'Call m_cnnSCGTaller.Close()
        '    End Try

        'End Function

#End Region

#Region "CreacionComandos"

        Private Function CrearProcesoInsertCommand() As SqlClient.SqlCommand
            Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSProceso)

            cmdIns.CommandType = CommandType.StoredProcedure

            cmdIns.Connection = m_cnnSCGTaller

            With cmdIns.Parameters

                .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100)
                .Add(mc_strArroba & mc_strNoProceso, SqlDbType.Int, 4).Direction = ParameterDirection.Output


            End With

            Return cmdIns

        End Function

        Private Function CrearActXProcInsertCommand() As SqlClient.SqlCommand
            Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSActividadesXProceso)

            cmdIns.CommandType = CommandType.StoredProcedure

            cmdIns.Connection = m_cnnSCGTaller

            With cmdIns.Parameters

                .Add(mc_strArroba & mc_strNoProceso, SqlDbType.Int, 4, mc_strNoProceso)
                .Add(mc_strArroba & mc_strNoActividad, SqlDbType.Int, 4, mc_strNoActividad)
                .Add(mc_strArroba & mc_strNoFase, SqlDbType.Int, 4, mc_strNoFase)

            End With

            Return cmdIns

        End Function

        Private Function CrearSelectCommand() As SqlClient.SqlCommand
            Dim cmdSelec As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELActividadesXProceso)

            cmdSelec.CommandType = CommandType.StoredProcedure

            cmdSelec.Connection = m_cnnSCGTaller

            With cmdSelec.Parameters

                .Add(mc_strArroba & mc_strNoProceso, SqlDbType.Int, 4)

            End With

            Return cmdSelec

        End Function

        Private Function CrearActXProcDeleteCommand() As SqlClient.SqlCommand

            Dim cmdDelete As New SqlClient.SqlCommand(mc_strSCGTA_SP_DELActividadesXProceso)

            cmdDelete.CommandType = CommandType.StoredProcedure

            cmdDelete.Connection = m_cnnSCGTaller

            With cmdDelete.Parameters

                .Add(mc_strArroba & mc_strNoProceso, SqlDbType.Int, 4, mc_strNoProceso)
                .Add(mc_strArroba & mc_strNoActividad, SqlDbType.Int, 4, mc_strNoActividad)
                .Add(mc_strArroba & mc_strNoFase, SqlDbType.Int, 4, mc_strNoFase)

            End With

            Return cmdDelete

        End Function

        Private Sub AsignaActividadesXProceso(ByRef p_dtbActividadesxProceso As ActividadesXProcesoDataset.SCGTA_TB_ActividadesXProcesosDataTable, _
                                              ByVal p_idProceso As Integer)

            Try

                Dim drwActividadesXProceso As ActividadesXProcesoDataset.SCGTA_TB_ActividadesXProcesosRow
                If Not p_dtbActividadesxProceso Is Nothing AndAlso p_idProceso > 0 Then

                    For Each drwActividadesXProceso In p_dtbActividadesxProceso.Rows


                        If drwActividadesXProceso.RowState = DataRowState.Added Then
                            drwActividadesXProceso.NoProceso = p_idProceso
                        End If

                    Next drwActividadesXProceso

                End If

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
            End Try

        End Sub

        Private Function CrearActXProcUpdateCommand() As SqlClient.SqlCommand

            Dim cmdUpd As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDActividadesXProceso)

            cmdUpd.CommandType = CommandType.StoredProcedure

            cmdUpd.Connection = m_cnnSCGTaller

            With cmdUpd.Parameters

                .Add(mc_strArroba & mc_strId, SqlDbType.Int, 4, mc_strId)
                .Add(mc_strArroba & mc_strNuevo, SqlDbType.Bit, 1, mc_strNuevo)
                .Add(mc_strArroba & mc_strPintar, SqlDbType.Bit, 1, mc_strPintar)

            End With

            Return cmdUpd

        End Function

#End Region

    End Class
End Namespace


