Namespace SCGDataAccess
    Public Class TramiteDataAdapter

        Implements IDataAdapter, IDisposable

#Region "Declaraciones"

        'TODO Agregar nombres de procedimeintos almacenados para una determinada tabla
        Private Const mc_strSPIns As String = ""
        Private Const mc_strSPUpd As String = ""
        Private Const mc_strSPDel As String = ""
        Private Const mc_strSPSelTramite As String = "SCGTA_SP_Tramites"
        Private Const mc_strEstaLlaveExiste As String = ""

        'TODO Agregar nombres de columnas de la tabla
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strCono As String = "Cono"
        Private Const mc_strPlaca As String = "Placa"
        Private Const mc_strMarca As String = "Marca"
        Private Const mc_strEstadoOrden As String = "EstadoOrden"
        Private Const mc_strEstadoTram As String = "EstadoTram "

        'Declaracion de objetos de acceso a datos

        Private m_adp As SqlClient.SqlDataAdapter

        Private m_strConexion As String

        Private mc_strArroba As String = "@"
        Dim objDAConexion As DAConexion
        Private m_cnnSCGTaller As SqlClient.SqlConnection

#End Region

#Region "Inicializar AnalisisDataAdapter"

        Public Sub New()
            Try
                objDAConexion = New DAConexion
                m_cnnSCGTaller = objDAConexion.ObtieneConexion
                m_adp = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub
#End Region

#Region "Implementaciones"
        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Overloads Function Fill(ByRef dataset As TramitesDataset, _
                                       ByVal NoOrden As String, _
                                       ByVal Cono As String, _
                                       ByVal Placa As String, _
                                       ByVal Marca As String, _
                                       ByVal EstadoOrden As String, _
                                       ByVal EstadoTramite As String) As Integer
            Try


                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adp.SelectCommand = CrearCmdSel()
                m_adp.SelectCommand.Connection = m_cnnSCGTaller

                If NoOrden <> vbNullString Then
                    m_adp.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden
                Else
                    m_adp.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = System.Convert.DBNull
                End If

                If Cono <> vbNullString Then
                    m_adp.SelectCommand.Parameters(mc_strArroba & mc_strCono).Value = Cono
                Else
                    m_adp.SelectCommand.Parameters(mc_strArroba & mc_strCono).Value = System.Convert.DBNull
                End If


                If Placa <> vbNullString Then
                    m_adp.SelectCommand.Parameters(mc_strArroba & mc_strPlaca).Value = Placa
                Else
                    m_adp.SelectCommand.Parameters(mc_strArroba & mc_strPlaca).Value = System.Convert.DBNull
                End If

                If Marca <> vbNullString Then

                    m_adp.SelectCommand.Parameters(mc_strArroba & mc_strMarca).Value = Marca
                Else

                    m_adp.SelectCommand.Parameters(mc_strArroba & mc_strMarca).Value = System.Convert.DBNull
                End If

                If EstadoOrden <> vbNullString Then
                    m_adp.SelectCommand.Parameters(mc_strArroba & mc_strEstadoOrden).Value = EstadoOrden
                Else
                    m_adp.SelectCommand.Parameters(mc_strArroba & mc_strEstadoOrden).Value = System.Convert.DBNull
                End If

                If EstadoTramite <> vbNullString Then
                    m_adp.SelectCommand.Parameters(mc_strArroba & mc_strEstadoTram).Value = EstadoTramite
                Else
                    m_adp.SelectCommand.Parameters(mc_strArroba & mc_strEstadoTram).Value = System.Convert.DBNull
                End If

                Call m_adp.Fill(dataset.Tramites)

            Catch ex As Exception
                Throw ex
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

        Public Function Update(ByVal dataSet As TramitesDataset) As Integer

            Dim m_trn As SqlClient.SqlTransaction

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                Call m_cnnSCGTaller.Open()


                m_trn = m_cnnSCGTaller.BeginTransaction
                m_adp.UpdateCommand = CrearCmdUpd()
                m_adp.InsertCommand = CrearCmdIns()
                m_adp.UpdateCommand.Connection = m_cnnSCGTaller
                m_adp.InsertCommand.Connection = m_cnnSCGTaller
                m_adp.UpdateCommand.Transaction = m_trn
                m_adp.InsertCommand.Transaction = m_trn

                Call m_adp.Update(dataSet)



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

                If Not m_adp Is Nothing Then
                    Call m_adp.Dispose()
                    m_adp = Nothing
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region

#Region "Commands "
        Private Function CrearCmdIns() As SqlClient.SqlCommand

            Dim cmdIns As SqlClient.SqlCommand

            Try

                cmdIns = New SqlClient.SqlCommand(mc_strSPIns)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    'TODO agregar campos para el comando de insercion

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            Finally
            End Try

        End Function

        Private Function CrearCmdDel() As SqlClient.SqlCommand

            Dim cmdDel As SqlClient.SqlCommand

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

        Private Function CrearCmdUpd() As SqlClient.SqlCommand

            Dim cmdUpd As SqlClient.SqlCommand

            Try

                cmdUpd = New SqlClient.SqlCommand(mc_strSPUpd)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters



                    'TODO agregar campos para el comando de actualizacion


                End With

                Return cmdUpd
            Catch ex As Exception
                Throw ex
            Finally
            End Try

        End Function

        Private Function CrearCmdSel() As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand

            Try
                cmdSel = New SqlClient.SqlCommand(mc_strSPSelTramite)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strCono, SqlDbType.Int, 4)
                    .Add(mc_strArroba & mc_strPlaca, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strMarca, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strEstadoOrden, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strEstadoTram, SqlDbType.VarChar, 50)

                End With

                Return cmdSel
            Catch ex As Exception
                Throw ex
            End Try

        End Function
#End Region



    End Class
End Namespace

