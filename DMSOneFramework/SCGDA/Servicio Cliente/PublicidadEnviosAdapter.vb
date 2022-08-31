Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGDataAccess

Namespace SCGDataAccess
    Public Class PublicidadEnviosAdapter

        Implements IDataAdapter, IDisposable

#Region "Declaraciones"

        'Encabezado
        Private Const mc_strSPInsEnvioPublicidad As String = "SCGTA_SP_InsEnvioPublicidad"
        Private Const mc_strSPUpdEnvioPublicidad As String = "SCGTA_SP_UpdEnvioPublicidad"
        Private Const mc_strSPDelEnvioPublicidad As String = "SCGTA_SP_DelEnvioPublicidad"
        Private Const mc_strSPSelEnvioPublicidad As String = "SCGTA_SP_SelEnvioPublicidad"
        Private Const mc_strSelEnvioPublicidadMasiva As String = "SCGTA_SP_SelEnvioPublicidadMasiva"
        Private Const mc_strEstaLlaveExiste As String = ""

        Private Const mc_strIdEnvioPublicidad As String = "IdEnvioPublicidad"
        Private Const mc_strEtiquetaPublicidad As String = "EtiquetaPublicidad"
        Private Const mc_strHoraEnvio As String = "HoraEnvio"
        Private Const mc_strFechaEnvio As String = "FechaEnvio"
        Private Const mc_strAsunto As String = "Asunto"
        Private Const mc_strMaterial As String = "Material"
        Private Const mc_strDetalle As String = "Detalle"
        Private Const mc_strEnviado As String = "Enviado"

        'Detalle
        Private Const mc_strSPInsDetalleEnvioPublicidad As String = "SCGTA_SP_InsDetalleEnvioPublicidad"
        Private Const mc_strSPUpdDetalleEnvioPublicidad As String = "SCGTA_SP_UpdDetalleEnvioPublicidad"
        Private Const mc_strSPDelDetalleEnvioPublicidad As String = "SCGTA_SP_DelDetalleEnvioPublicidad"

        Private Const mc_strCardCode As String = "CardCode"


        'Config Correo

        Private Const mc_strSPInsConfigCorreo As String = "SCGTA_SP_InsConfiguracionDeCorreo"
        Private Const mc_strSPUpdConfigCorreo As String = "SCGTA_SP_UpdConfiguracionDeCorreo"
        Private Const mc_strSPSelConfigCorreo As String = "SCGTA_SP_SelConfigCorreo"




        Private Const mc_strIdConfigCorreo As String = "IdConfigCorreo"
        Private Const mc_strServidorDeCorreo As String = "ServidorDeCorreo"
        Private Const mc_strDireccionCorreoEnvia As String = "DireccionCorreoEnvia"
        Private Const mc_strUsuarioSMTP As String = "UsuarioSMTP"
        Private Const mc_strPasswordSMTP As String = "PasswordSMTP"
        Private Const mc_strPuerto As String = "Puerto"
        Private Const mc_strUsaSSL As String = "UsaSSL"

        'Declaracion de objetos de acceso a datos
        Private m_cnn As SqlClient.SqlConnection
        Private m_adp As SqlClient.SqlDataAdapter
        Private m_adpDetalleEnvioPublicidad As SqlClient.SqlDataAdapter

        Private m_strConexion As String

        Private mc_strArroba As String = "@"

        Private objDAConexion As DAConexion

#End Region

#Region "Inicializar AnalisisDataAdapter"

        'Public Sub New(ByVal conexion As String)
        '    Try
        '        m_strConexion = conexion
        '        m_cnn = New SqlClient.SqlConnection(conexion)
        '        m_adp = New SqlClient.SqlDataAdapter
        '    Catch ex As Exception
        '        MsgBox(ex.Message)
        '    End Try
        'End Sub

        Public Sub New()

            objDAConexion = New DAConexion
            m_cnn = objDAConexion.ObtieneConexion
            m_adp = New SqlClient.SqlDataAdapter
            m_adpDetalleEnvioPublicidad = New SqlClient.SqlDataAdapter

        End Sub
#End Region

#Region "Implementaciones"
        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Overloads Function Fill(ByRef dataset As PublicidadEnvioDataset, _
                                       ByVal idEnvioPublicidad As Integer, _
                                       ByVal Enviado As Integer) As Integer
            Try
                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                m_adp.SelectCommand = CrearCmdSel()
                m_adp.SelectCommand.Connection = m_cnn

                m_adp.SelectCommand.Parameters(mc_strArroba & mc_strIdEnvioPublicidad).Value = idEnvioPublicidad
                m_adp.SelectCommand.Parameters(mc_strArroba & mc_strEnviado).Value = Enviado

                If m_adp.TableMappings.Count = 0 Then
                    m_adp.TableMappings.Add("Table", dataset.SCGTA_TB_EnvioPublicidad.TableName)
                    m_adp.TableMappings.Add("Table1", dataset.SCGTA_TB_DetalleEnvioPublicidad.TableName)
                End If

                Call m_adp.Fill(dataset)

            Catch ex As Exception
                MsgBox(ex.Message)
                Return 1
            Finally
                Call m_cnn.Close()
            End Try
        End Function

        Public Overloads Function Fill(ByRef dataset As ConfigServidorCorreoDataset) As Integer
            Try
                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                m_adp.SelectCommand = CrearCmdSelServidorDeCorreo()
                m_adp.SelectCommand.Connection = m_cnn

                Call m_adp.Fill(dataset.SCGTA_TB_ConfiguracionDeCorreo)

            Catch ex As Exception
                MsgBox(ex.Message)
                Return 1
            Finally
                Call m_cnn.Close()
            End Try
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

        Public Function Update(ByVal dataSet As DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

        Public Function Update(ByVal dataSet As PublicidadEnvioDataset) As Integer

            Dim m_trn As SqlClient.SqlTransaction = Nothing

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

                m_adpDetalleEnvioPublicidad.UpdateCommand = CrearCmdUpdDetalle()
                m_adpDetalleEnvioPublicidad.UpdateCommand.Connection = m_cnn
                m_adpDetalleEnvioPublicidad.UpdateCommand.Transaction = m_trn


                m_adpDetalleEnvioPublicidad.InsertCommand = CrearCmdInsDetalle()
                m_adpDetalleEnvioPublicidad.InsertCommand.Connection = m_cnn
                m_adpDetalleEnvioPublicidad.InsertCommand.Transaction = m_trn

                m_adpDetalleEnvioPublicidad.DeleteCommand = CrearCmdDelDetalle()
                m_adpDetalleEnvioPublicidad.DeleteCommand.Connection = m_cnn
                m_adpDetalleEnvioPublicidad.DeleteCommand.Transaction = m_trn


                Call m_adp.Update(dataSet.SCGTA_TB_EnvioPublicidad)
                Call m_adpDetalleEnvioPublicidad.Update(dataSet.SCGTA_TB_DetalleEnvioPublicidad)

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

        Public Function Update(ByVal dataSet As ConfigServidorCorreoDataset) As Integer

            Dim m_trn As SqlClient.SqlTransaction = Nothing

            Try

                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                m_trn = m_cnn.BeginTransaction
                m_adp.UpdateCommand = CrearCmdUpdServidorCorreo()
                m_adp.InsertCommand = CrearCmdInsServidorCorreo()
                m_adp.UpdateCommand.Connection = m_cnn
                m_adp.InsertCommand.Connection = m_cnn
                m_adp.UpdateCommand.Transaction = m_trn
                m_adp.InsertCommand.Transaction = m_trn

                Call m_adp.Update(dataSet.SCGTA_TB_ConfiguracionDeCorreo)

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
#End Region

#Region "Commands "
     

        Private Function CrearCmdInsServidorCorreo() As SqlClient.SqlCommand

            Dim cmdIns As SqlClient.SqlCommand
           
            Try

                cmdIns = New SqlClient.SqlCommand(mc_strSPInsConfigCorreo)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strServidorDeCorreo, SqlDbType.VarChar, 50, mc_strServidorDeCorreo)
                    .Add(mc_strArroba & mc_strDireccionCorreoEnvia, SqlDbType.VarChar, 50, mc_strDireccionCorreoEnvia)
                    .Add(mc_strArroba & mc_strUsuarioSMTP, SqlDbType.VarChar, 50, mc_strUsuarioSMTP)
                    .Add(mc_strArroba & mc_strPasswordSMTP, SqlDbType.VarChar, 20, mc_strPasswordSMTP)
                    .Add(mc_strArroba & mc_strPuerto, SqlDbType.VarChar, 20, mc_strPuerto)
                    .Add(mc_strArroba & mc_strUsaSSL, SqlDbType.VarChar, 20, mc_strUsaSSL)
                 
                End With

                Return cmdIns
            Catch ex As Exception
                MsgBox(ex.Message)
                Return Nothing
            Finally
            End Try

        End Function

        Private Function CrearCmdUpdServidorCorreo() As SqlClient.SqlCommand

            Dim cmdUpd As SqlClient.SqlCommand
            'Dim param As SqlClient.SqlParameter

            Try

                cmdUpd = New SqlClient.SqlCommand(mc_strSPUpdConfigCorreo)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters

                    .Add(mc_strArroba & mc_strServidorDeCorreo, SqlDbType.VarChar, 50, mc_strServidorDeCorreo)
                    .Add(mc_strArroba & mc_strDireccionCorreoEnvia, SqlDbType.VarChar, 50, mc_strDireccionCorreoEnvia)
                    .Add(mc_strArroba & mc_strUsuarioSMTP, SqlDbType.VarChar, 50, mc_strUsuarioSMTP)
                    .Add(mc_strArroba & mc_strPasswordSMTP, SqlDbType.VarChar, 20, mc_strPasswordSMTP)
                    .Add(mc_strArroba & mc_strIdConfigCorreo, SqlDbType.VarChar, 20, mc_strIdConfigCorreo)
                    .Add(mc_strArroba & mc_strPuerto, SqlDbType.VarChar, 20, mc_strPuerto)
                    .Add(mc_strArroba & mc_strUsaSSL, SqlDbType.VarChar, 20, mc_strUsaSSL)

                End With

                Return cmdUpd
            Catch ex As Exception
                Return Nothing
            Finally
            End Try

        End Function

        Private Function CrearCmdSelServidorDeCorreo() As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand
            'Dim param As SqlClient.SqlParameter

            Try
                cmdSel = New SqlClient.SqlCommand(mc_strSPSelConfigCorreo)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

               
                End With

                Return cmdSel
            Catch ex As Exception
                Return Nothing
            End Try

        End Function


        Private Function CrearCmdIns() As SqlClient.SqlCommand

            Dim cmdIns As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try

                cmdIns = New SqlClient.SqlCommand(mc_strSPInsEnvioPublicidad)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strEtiquetaPublicidad, SqlDbType.VarChar, 50, mc_strEtiquetaPublicidad)
                    .Add(mc_strArroba & mc_strHoraEnvio, SqlDbType.DateTime, 8, mc_strHoraEnvio)
                    .Add(mc_strArroba & mc_strFechaEnvio, SqlDbType.DateTime, 8, mc_strFechaEnvio)
                    .Add(mc_strArroba & mc_strAsunto, SqlDbType.VarChar, 50, mc_strAsunto)
                    .Add(mc_strArroba & mc_strMaterial, SqlDbType.VarChar, 500, mc_strMaterial)
                    .Add(mc_strArroba & mc_strDetalle, SqlDbType.VarChar, 4000, mc_strDetalle)
                    .Add(mc_strArroba & mc_strEnviado, SqlDbType.Int, 4, mc_strEnviado)
                    param = .Add(mc_strArroba & mc_strIdEnvioPublicidad, SqlDbType.Int, 4, mc_strIdEnvioPublicidad)
                    param.Direction = ParameterDirection.Output
                    '.Add(mc_strArroba & mc_strCardCode, SqlDbType.VarChar, 15, mc_strCardCode)
                End With

                Return cmdIns
            Catch ex As Exception
                MsgBox(ex.Message)
                Return Nothing
            Finally
            End Try

        End Function

        Private Function CrearCmdDel() As SqlClient.SqlCommand

            Dim cmdDel As SqlClient.SqlCommand
            'Dim param As SqlClient.SqlParameter

            Try

                cmdDel = New SqlClient.SqlCommand(mc_strSPDelEnvioPublicidad)
                cmdDel.CommandType = CommandType.StoredProcedure

                With cmdDel.Parameters


                    .Add(mc_strArroba & mc_strIdEnvioPublicidad, SqlDbType.VarChar, 50, mc_strIdEnvioPublicidad)


                End With


                Return cmdDel
            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Private Function CrearCmdUpd() As SqlClient.SqlCommand

            Dim cmdUpd As SqlClient.SqlCommand
            'Dim param As SqlClient.SqlParameter

            Try

                cmdUpd = New SqlClient.SqlCommand(mc_strSPUpdEnvioPublicidad)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters

                    .Add(mc_strArroba & mc_strEtiquetaPublicidad, SqlDbType.VarChar, 50, mc_strEtiquetaPublicidad)
                    .Add(mc_strArroba & mc_strHoraEnvio, SqlDbType.DateTime, 8, mc_strHoraEnvio)
                    .Add(mc_strArroba & mc_strFechaEnvio, SqlDbType.DateTime, 8, mc_strFechaEnvio)
                    .Add(mc_strArroba & mc_strAsunto, SqlDbType.VarChar, 50, mc_strAsunto)
                    .Add(mc_strArroba & mc_strMaterial, SqlDbType.VarChar, 500, mc_strMaterial)
                    .Add(mc_strArroba & mc_strDetalle, SqlDbType.VarChar, 4000, mc_strDetalle)
                    .Add(mc_strArroba & mc_strEnviado, SqlDbType.Int, 4, mc_strEnviado)
                    .Add(mc_strArroba & mc_strIdEnvioPublicidad, SqlDbType.Int, 4, mc_strIdEnvioPublicidad)
                    '.Add(mc_strArroba & mc_strCardCode, SqlDbType.VarChar, 15, mc_strCardCode)

                End With

                Return cmdUpd
            Catch ex As Exception
                Return Nothing
            Finally
            End Try

        End Function

        Private Function CrearCmdInsDetalle() As SqlClient.SqlCommand

            Dim cmdIns As SqlClient.SqlCommand


            Try

                cmdIns = New SqlClient.SqlCommand(mc_strSPInsDetalleEnvioPublicidad)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strIdEnvioPublicidad, SqlDbType.Int, 4, mc_strIdEnvioPublicidad)
                    .Add(mc_strArroba & mc_strCardCode, SqlDbType.VarChar, 15, mc_strCardCode)


                End With

                Return cmdIns
            Catch ex As Exception
                Return Nothing
            Finally
            End Try

        End Function

        Private Function CrearCmdDelDetalle() As SqlClient.SqlCommand

            Dim cmdDel As SqlClient.SqlCommand
            'Dim param As SqlClient.SqlParameter

            Try

                cmdDel = New SqlClient.SqlCommand(mc_strSPDelDetalleEnvioPublicidad)
                cmdDel.CommandType = CommandType.StoredProcedure

                With cmdDel.Parameters


                    .Add(mc_strArroba & mc_strIdEnvioPublicidad, SqlDbType.VarChar, 50, mc_strIdEnvioPublicidad)


                End With


                Return cmdDel
            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Private Function CrearCmdUpdDetalle() As SqlClient.SqlCommand

            Dim cmdUpd As SqlClient.SqlCommand
            'Dim param As SqlClient.SqlParameter

            Try

                cmdUpd = New SqlClient.SqlCommand(mc_strSPUpdDetalleEnvioPublicidad)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters

                    .Add(mc_strArroba & mc_strCardCode, SqlDbType.VarChar, 15, mc_strCardCode)

                End With

                Return cmdUpd
            Catch ex As Exception
                Return Nothing
            Finally
            End Try

        End Function


        Private Function CrearCmdSel() As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand
            'Dim param As SqlClient.SqlParameter

            Try
                cmdSel = New SqlClient.SqlCommand(mc_strSPSelEnvioPublicidad)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strIdEnvioPublicidad, SqlDbType.Int, 4, mc_strIdEnvioPublicidad)
                    .Add(mc_strArroba & mc_strEnviado, SqlDbType.Int, 50, mc_strEnviado)

                End With

                Return cmdSel
            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Private Function CrearCmdSelEnvioMasivoPublicidad() As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand
            'Dim param As SqlClient.SqlParameter

            Try
                cmdSel = New SqlClient.SqlCommand(mc_strSelEnvioPublicidadMasiva)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    '.Add(mc_strArroba & mc_strIdEnvioPublicidad, SqlDbType.VarChar, 50, mc_strIdEnvioPublicidad)

                End With

                Return cmdSel
            Catch ex As Exception
                Return Nothing
            End Try

        End Function



#End Region


    End Class
End Namespace


