Namespace SCGDataAccess
    Public Class BodegasDataAdapter
        Implements IDataAdapter

#Region "Implementaciones"


        Public Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

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

        Public Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

#End Region

#Region "Declaraciones"

        Private m_adpBodegas As SqlClient.SqlDataAdapter
        Private m_cnnSCGTaller As SqlClient.SqlConnection
        Private objDAConexion As DAConexion

        'Procedimientos almacenados
        Private Const mc_strSCGTA_SP_UPDBodegas As String = "SCGTA_SP_UPDBodegas"
        Private Const mc_strSCGTA_SP_SELBodegas As String = "SCGTA_SP_SELBodegas"
        'Parametros de los procedimientos almacenados
        Private Const mc_strArroba As String = "@"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strNoBodega As String = "NoBodega"
        Private Const mc_strBodega As String = "NombreBodega"

#End Region

#Region "Inicializacion"

        Public Sub New()

            Call InicializaActividadesDataAdapter(m_cnnSCGTaller)

        End Sub

        Public Sub New(ByRef p_cnnSCGTaller As SqlClient.SqlConnection)

            Call InicializaActividadesDataAdapter(p_cnnSCGTaller)

        End Sub

        Private Sub InicializaActividadesDataAdapter(ByRef cnnTaller As SqlClient.SqlConnection)
            Try

                ' cnnTaller = New SqlClient.SqlConnection(conexion)
                objDAConexion = New DAConexion
                If cnnTaller Is Nothing Then
                    cnnTaller = objDAConexion.ObtieneConexion
                Else
                    m_cnnSCGTaller = cnnTaller
                End If

                m_adpBodegas = New SqlClient.SqlDataAdapter

            Catch ex As Exception

                MsgBox(ex.Message)

            Finally

            End Try

        End Sub
#End Region

#Region "Metodos"

        Public Sub ActualizarBodegas(ByVal p_strDescripcion As String, ByVal p_strNoBodega As String, ByVal p_strBodega As String)
            'Permite establecer cuáles serán las bodegas utilizadas por la sucursal para Refacciones, Servicios y Suministros

            Dim cmdUpd As New SqlClient.SqlCommand
            Dim intResultado As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                With cmdUpd
                    'Se le asigna una instancia de la conexion
                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = mc_strSCGTA_SP_UPDBodegas
                End With

                With cmdUpd
                    .Parameters.Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100)
                    .Parameters.Add(mc_strArroba & mc_strNoBodega, SqlDbType.VarChar, 10)
                    .Parameters.Add(mc_strArroba & mc_strBodega, SqlDbType.VarChar, 100)
                    .Parameters(mc_strArroba & mc_strDescripcion).Value = p_strDescripcion
                    .Parameters(mc_strArroba & mc_strNoBodega).Value = p_strNoBodega
                    .Parameters(mc_strArroba & mc_strBodega).Value = p_strBodega
                End With

                intResultado = cmdUpd.ExecuteNonQuery()


            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Sub

        Public Sub SeleccionarBodegas(ByRef p_strCodigo As String, ByRef p_strBodega As String, ByVal p_strDescripcion As String)
            'Permite seleccionar las bodegas que están siendo utilizadas por la sucursal para Refacciones, Servicios y Suministros

            Dim cmdSel As New SqlClient.SqlCommand
            Dim drdBodegas As SqlClient.SqlDataReader = Nothing


            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                With cmdSel
                    'Se le asigna una instancia de la conexion
                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = mc_strSCGTA_SP_SELBodegas

                End With

                With cmdSel
                    .Parameters.Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100)
                    .Parameters(mc_strArroba & mc_strDescripcion).Value = p_strDescripcion
                End With

                drdBodegas = cmdSel.ExecuteReader


                If drdBodegas.Read Then
                    p_strBodega = drdBodegas.Item(1)
                    p_strCodigo = drdBodegas.Item(0)

                    If p_strBodega = "-1" Then
                        p_strBodega = ""
                    End If

                    If p_strCodigo = "-1" Then
                        p_strCodigo = ""
                    End If

                End If



            Catch ex As Exception
                Throw ex
            Finally

                m_cnnSCGTaller.Close()
                drdBodegas.Close()
                drdBodegas = Nothing
            End Try
        End Sub

#End Region




    End Class
End Namespace
