Namespace SCGDataAccess

Public Class ServiciosGruaDataAdapter

    Implements IDataAdapter

#Region "Declaraciones"

        'Declaración de los nombres de las columnas que van a formar el Dataset. 
        'Se deben llamar exactamente igual que en el Store Procedure
        Private Const mc_strNoServicio As String = "NoServicio"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strNoExpediente As String = "NoExpediente"
        Private Const mc_strCono As String = "Cono"
        Private Const mc_strCodMarca As String = "CodMarca"
        Private Const mc_strNoProveedor As String = "NoProveedor"
        Private Const mc_strProveedor As String = "Proveedor"
        Private Const mc_strPlaca As String = "Placa"
        Private Const mc_strCodServicio As String = "CodServicio"
        Private Const mc_strServicio As String = "Servicio"
        Private Const mc_strFacturable As String = "Facturable"
        Private Const mc_strFecha As String = "Fecha"
        Private Const mc_strHora As String = "Hora"
        Private Const mc_strDetalle As String = "Detalle"

        Private Const mc_strCliente As String = "Cliente"
        Private Const mc_intMarca As String = "CodMarca"


        Private m_adpServicios As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDServicioGrua As String = "SCGTA_SP_UpdServicioGrua"
        Private Const mc_strSCGTA_SP_SELServicioGrua As String = "SCGTA_SP_SELServicioGrua"
        Private Const mc_strSCGTA_SP_SELServicioGruaMaestro As String = "SCGTA_SP_SELServicioGMaestro"
        Private Const mc_strSCGTA_SP_INSServicioGrua As String = "SCGTA_SP_INSServicioGrua"
        Private Const mc_strSCGTA_SP_DelServicioGrua As String = "SCGTA_SP_DELServicioGrua"
        Private Const mc_strSCGTA_SP_SelServicioGruaByNoExpediente As String = "SCGTA_SP_SELServicioGruaByNoExpediente"

    Private m_cnnSCGTaller As SqlClient.SqlConnection

    Private Const mc_strArroba As String = "@"
    Private objDAConexion As DAConexion

#End Region


#Region "Inicializa ServiciosGruaDataAdapter"


        Public Sub New()

            Call InicializaServiciosGruaDataAdapter(m_cnnSCGTaller)

        End Sub

        Private Sub InicializaServiciosGruaDataAdapter(ByRef cnnTaller As SqlClient.SqlConnection)
            Try

                ' cnnTaller = New SqlClient.SqlConnection(conexion)
                objDAConexion = New DAConexion
                cnnTaller = objDAConexion.ObtieneConexion

                m_adpServicios = New SqlClient.SqlDataAdapter

            Catch ex As Exception

                MsgBox(ex.Message)

            Finally

            End Try

        End Sub

#End Region


#Region "Implementaciones .Net Framework"

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


        Public Overloads Function Fill(ByVal dataSet As ServiciosGruaDataset, ByVal strNoOrden As String, ByVal intNoExpediente As Integer, ByVal strPlaca As String, ByVal intCono As Integer, ByVal CodMarca As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpServicios.SelectCommand = CrearSelectCommand()

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado
                If strNoOrden = "" Then
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = System.DBNull.Value
                Else
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = strNoOrden
                End If


                If intNoExpediente = "" Then
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strNoExpediente).Value = System.DBNull.Value
                Else
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strNoExpediente).Value = intNoExpediente
                End If


                If strPlaca = "" Then
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strPlaca).Value = System.DBNull.Value
                Else
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strPlaca).Value = strPlaca
                End If


                If intCono = "" Then
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strCono).Value = System.DBNull.Value
                Else
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strCono).Value = intCono
                End If


                If CodMarca = "" Then
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strCodMarca).Value = System.DBNull.Value
                Else
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strCodMarca).Value = CodMarca
                End If


                m_adpServicios.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpServicios.Fill(dataSet.SCGTA_TB_ServicioGrua)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function

        Public Overloads Function Fill(ByVal dataSet As ServiciosGruaDataset, ByVal intNoExpediente As Integer, ByVal strPlaca As String, ByVal intCono As Integer, ByVal CodMarca As Integer, ByVal intproveedor As Integer, ByVal intCodServicio As Integer, ByVal strcodcliente As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If
                'Creacion del comando
                m_adpServicios.SelectCommand = CrearSelectCommandMaestro()

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado
                If strcodcliente = "" Then
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strCliente).Value = System.DBNull.Value
                Else
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strCliente).Value = strcodcliente
                End If


                If intNoExpediente = 0 Then
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strNoExpediente).Value = System.DBNull.Value
                Else
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strNoExpediente).Value = intNoExpediente
                End If


                If strPlaca = "" Then
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strPlaca).Value = System.DBNull.Value
                Else
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strPlaca).Value = strPlaca
                End If


                If intCono = 0 Then
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strCono).Value = System.DBNull.Value
                Else
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strCono).Value = intCono
                End If


                If CodMarca = 0 Then
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strCodMarca).Value = System.DBNull.Value
                Else
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strCodMarca).Value = CodMarca
                End If

                If intCodServicio = 0 Then
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strCodServicio).Value = System.DBNull.Value
                Else
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strCodServicio).Value = intCodServicio
                End If

                If intproveedor = 0 Then
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strNoProveedor).Value = System.DBNull.Value
                Else
                    m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strNoProveedor).Value = intproveedor
                End If



                m_adpServicios.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpServicios.Fill(dataSet.SCGTA_TB_ServicioGrua)

            Catch ex As Exception

                Throw ex
            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function


        Public Overloads Function Fill(ByVal dataSet As ServiciosGruaDataset, ByVal decNoExpediente As Decimal) As Integer

            Try


                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpServicios.SelectCommand = CrearSelectCommandByNoExpediente()

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Carga el valor del NoExpediente en los parametros de busqueda               
                m_adpServicios.SelectCommand.Parameters(mc_strArroba & mc_strNoExpediente).Value = decNoExpediente

                m_adpServicios.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpServicios.Fill(dataSet.SCGTA_TB_ServicioGrua)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function


        Public Overloads Function Update(ByVal dataSet As ServiciosGruaDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpServicios.InsertCommand = CreateInsertCommand()
                m_adpServicios.InsertCommand.Connection = m_cnnSCGTaller

                m_adpServicios.UpdateCommand = CrearUpdateCommand()
                m_adpServicios.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpServicios.Update(dataSet.SCGTA_TB_ServicioGrua)

            Catch ex As Exception

                Throw ex

            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Function Delete(ByVal dataset As ServiciosGruaDataset, ByVal decNoServicio As Integer, ByVal decnoexpediente As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpServicios.UpdateCommand = CrearDeleteCommand()

                m_adpServicios.UpdateCommand.Connection = m_cnnSCGTaller

                m_adpServicios.UpdateCommand.Parameters(mc_strArroba & mc_strNoServicio).Value = decNoServicio

                m_adpServicios.UpdateCommand.Parameters(mc_strArroba & mc_strNoExpediente).Value = decnoexpediente

                Call m_adpServicios.Update(dataset.SCGTA_TB_ServicioGrua)

            Catch ex As Exception

                Throw ex

            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function





#End Region


#Region "Creación de comandos"


        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELServicioGrua)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.Int, 9, mc_strNoOrden)
                    .Add(mc_strArroba & mc_strNoExpediente, SqlDbType.Int, 9, mc_strNoExpediente)
                    .Add(mc_strArroba & mc_strPlaca, SqlDbType.Int, 9, mc_strPlaca)
                    .Add(mc_strArroba & mc_strCono, SqlDbType.Int, 9, mc_strCono)
                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.Int, 9, mc_strCodMarca)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearSelectCommandMaestro() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELServicioGruaMaestro)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strCliente, SqlDbType.VarChar, 15, mc_strCliente)
                    .Add(mc_strArroba & mc_strPlaca, SqlDbType.VarChar, 20, mc_strPlaca)
                    .Add(mc_strArroba & mc_strCono, SqlDbType.Int, 9, mc_strCono)
                    .Add(mc_strArroba & mc_strCodServicio, SqlDbType.Int, 9, mc_strCodServicio)
                    .Add(mc_strArroba & mc_strNoExpediente, SqlDbType.Int, 9, mc_strNoExpediente)
                    .Add(mc_strArroba & mc_strNoProveedor, SqlDbType.Int, 5, mc_strNoProveedor)
                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.Int, 9, mc_strCodMarca)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Private Function CrearSelectCommandByNoExpediente() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelServicioGruaByNoExpediente)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNoExpediente, SqlDbType.Decimal, 9, mc_strNoExpediente)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function



        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDServicioGrua)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoServicio, SqlDbType.Int, 9, mc_strNoServicio)

                    .Add(mc_strArroba & mc_strNoExpediente, SqlDbType.Int, 9, mc_strNoExpediente)

                    .Add(mc_strArroba & mc_strNoProveedor, SqlDbType.Int, 9, mc_strNoProveedor)

                    .Add(mc_strArroba & mc_strCodServicio, SqlDbType.Int, 9, mc_strCodServicio)

                    .Add(mc_strArroba & mc_strFacturable, SqlDbType.Bit, 1, mc_strFacturable)

                    .Add(mc_strArroba & mc_strFecha, SqlDbType.DateTime, 8, mc_strFecha)

                    .Add(mc_strArroba & mc_strHora, SqlDbType.VarChar, 15, mc_strHora)

                    .Add(mc_strArroba & mc_strDetalle, SqlDbType.VarChar, 500, mc_strDetalle)



                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelServicioGrua)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoServicio, SqlDbType.Int, 9, mc_strNoServicio)

                    .Add(mc_strArroba & mc_strNoExpediente, SqlDbType.Int, 9, mc_strNoExpediente)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try



        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSServicioGrua)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters


                    .Add(mc_strArroba & mc_strNoExpediente, SqlDbType.Int, 9, mc_strNoExpediente)

                    .Add(mc_strArroba & mc_strNoProveedor, SqlDbType.Int, 9, mc_strNoProveedor)

                    .Add(mc_strArroba & mc_strCodServicio, SqlDbType.Int, 9, mc_strCodServicio)

                    .Add(mc_strArroba & mc_strFacturable, SqlDbType.Bit, 1, mc_strFacturable)

                    .Add(mc_strArroba & mc_strFecha, SqlDbType.DateTime, 8, mc_strFecha)

                    .Add(mc_strArroba & mc_strHora, SqlDbType.VarChar, 15, mc_strHora)

                    .Add(mc_strArroba & mc_strDetalle, SqlDbType.VarChar, 500, mc_strDetalle)
                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


#End Region



    End Class

End Namespace