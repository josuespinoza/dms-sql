Namespace SCGDataAccess
    Public Class ConfBodegasXCCDataAdapter
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

        Private m_adpBodegasXCC As SqlClient.SqlDataAdapter
        Private m_cnnSCGTaller As SqlClient.SqlConnection
        Private objDAConexion As DAConexion

        'Procedimientos almacenados
        Private Const mc_strSCGTA_SP_SELBodegasLista As String = "SCGTA_SP_SELBodegasLista"
        Private Const mc_strSCGTA_SP_SELCentrosCosto As String = "SCGTA_SP_SELCentrosCosto"
        Private Const mc_strSCGTA_SP_SELBodegasXCC As String = "SCGTA_SP_SELBodegasXCC"
        Private Const mc_strSCGTA_SP_INSBodegasXCC As String = "SCGTA_SP_INSBodegasXCC"
        Private Const mc_strSCGTA_SP_UPDBodegasXCC As String = "SCGTA_SP_UPDBodegasXCC"
        Private Const mc_strSCGTA_SP_DELBodegasXCC As String = "SCGTA_SP_DELBodegasXCC"

        'Parametros de los procedimientos almacenados
        Private Const mc_strArroba As String = "@"
        Private Const mc_strWhsName As String = "WhsName"
        Private Const mc_strWhsCode As String = "WhsCode"

        Private Const mc_strIDCentroCosto As String = "IDCentroCosto"
        Private Const mc_strIDCentroCostoNew As String = "IDCentroCostoNew"
        Private Const mc_strRepuestos As String = "Repuestos"
        Private Const mc_strServicios As String = "Servicios"
        Private Const mc_strSuministros As String = "Suministros"
        Private Const mc_strServiciosEX As String = "ServiciosEX"
        Private Const mc_strProceso As String = "Proceso"

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

                objDAConexion = New DAConexion
                If cnnTaller Is Nothing Then
                    cnnTaller = objDAConexion.ObtieneConexion
                Else
                    m_cnnSCGTaller = cnnTaller
                End If

                m_adpBodegasXCC = New SqlClient.SqlDataAdapter

            Catch ex As Exception

                Throw ex

            Finally

            End Try

        End Sub

#End Region

#Region "Metodos"

        Public Sub FillBodegasLista(ByRef p_dstBodegas As BodegasSBODataset)
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                With Me.m_adpBodegasXCC

                    .SelectCommand = SelectCommandBodegasLista(m_cnnSCGTaller, mc_strSCGTA_SP_SELBodegasLista)

                End With

                m_adpBodegasXCC.Fill(p_dstBodegas.SCGTA_VW_Bodegas)

            Catch ex As Exception
                Throw ex
            Finally
                If m_cnnSCGTaller.State = ConnectionState.Open Then
                    m_cnnSCGTaller.Close()
                End If
            End Try
        End Sub

        Public Sub FillCentrosCostoLista(ByRef p_dstCentrosCosto As CentroCostoDataset)
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                With Me.m_adpBodegasXCC

                    .SelectCommand = SelectCommandCentrosCostoLista(m_cnnSCGTaller, mc_strSCGTA_SP_SELCentrosCosto)

                End With

                m_adpBodegasXCC.Fill(p_dstCentrosCosto.SCGTA_TB_CentroCosto)

            Catch ex As Exception
                Throw ex
            Finally
                If m_cnnSCGTaller.State = ConnectionState.Open Then
                    m_cnnSCGTaller.Close()
                End If
            End Try
        End Sub

        Public Sub FillBXCC(ByRef p_dstBodegasXCC As ConfBodegasXCentroCostoDataSet) 'Bodegas X Centros de Costo
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                With Me.m_adpBodegasXCC

                    .SelectCommand = SelectCommandBodegasXCC(m_cnnSCGTaller, mc_strSCGTA_SP_SELBodegasXCC)

                End With

                m_adpBodegasXCC.Fill(p_dstBodegasXCC.SCGTA_SP_SelConfBodegasXCentroCosto)

            Catch ex As Exception
                Throw ex
            Finally
                If m_cnnSCGTaller.State = ConnectionState.Open Then
                    m_cnnSCGTaller.Close()
                End If
            End Try
        End Sub

        Public Sub UpdateBXCC(ByRef p_dstBodegasXCC As ConfBodegasXCentroCostoDataSet) 'Bodegas X Centros de Costo
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                With Me.m_adpBodegasXCC

                    .InsertCommand = InsertCommandBodegasXCC(m_cnnSCGTaller, mc_strSCGTA_SP_INSBodegasXCC)
                    .UpdateCommand = UpdateCommandBodegasXCC(m_cnnSCGTaller, mc_strSCGTA_SP_UPDBodegasXCC)
                    .DeleteCommand = DeleteCommandBodegasXCC(m_cnnSCGTaller, mc_strSCGTA_SP_DELBodegasXCC)

                End With

                m_adpBodegasXCC.Update(p_dstBodegasXCC.SCGTA_SP_SelConfBodegasXCentroCosto)

            Catch ex As Exception
                Throw ex
            Finally
                If m_cnnSCGTaller.State = ConnectionState.Open Then
                    m_cnnSCGTaller.Close()
                End If
            End Try
        End Sub

        Private Function SelectCommandBodegasLista(ByVal p_cnnSCGTaller As SqlClient.SqlConnection, _
                                        ByVal p_strSCGTA_SP_SelBodegasLista As String) As SqlClient.SqlCommand

            Dim cmdBodegas As New SqlClient.SqlCommand

            With cmdBodegas
                .CommandText = p_strSCGTA_SP_SelBodegasLista
                .CommandType = CommandType.StoredProcedure
                .Connection = p_cnnSCGTaller
            End With

            Return cmdBodegas

        End Function

        Private Function SelectCommandCentrosCostoLista(ByVal p_cnnSCGTaller As SqlClient.SqlConnection, _
                                        ByVal p_strSCGTA_SP_SelCeontrosCostoLista As String) As SqlClient.SqlCommand

            Dim cmdBodegas As New SqlClient.SqlCommand

            With cmdBodegas
                .CommandText = p_strSCGTA_SP_SelCeontrosCostoLista
                .CommandType = CommandType.StoredProcedure
                .Connection = p_cnnSCGTaller
            End With

            Return cmdBodegas

        End Function

        Private Function SelectCommandBodegasXCC(ByVal p_cnnSCGTaller As SqlClient.SqlConnection, _
                                        ByVal p_strSCGTA_SP_SELBodegasXCC As String) As SqlClient.SqlCommand

            Dim cmdBodegasXCC As New SqlClient.SqlCommand

            With cmdBodegasXCC
                .CommandText = p_strSCGTA_SP_SELBodegasXCC
                .CommandType = CommandType.StoredProcedure
                .Connection = p_cnnSCGTaller
            End With

            Return cmdBodegasXCC

        End Function

        Private Function InsertCommandBodegasXCC(ByVal p_cnnSCGTaller As SqlClient.SqlConnection, _
                                        ByVal p_strSCGTA_SP_INSBodegasXCC As String) As SqlClient.SqlCommand

            Dim cmdBodegasXCC As New SqlClient.SqlCommand

            With cmdBodegasXCC
                .CommandText = p_strSCGTA_SP_INSBodegasXCC
                .CommandType = CommandType.StoredProcedure
                .Connection = p_cnnSCGTaller

                .Parameters.Add(mc_strArroba & mc_strIDCentroCosto, SqlDbType.Int, 4, mc_strIDCentroCosto)
                .Parameters.Add(mc_strArroba & mc_strRepuestos, SqlDbType.NVarChar, 8, mc_strRepuestos)
                .Parameters.Add(mc_strArroba & mc_strServicios, SqlDbType.NVarChar, 8, mc_strServicios)
                .Parameters.Add(mc_strArroba & mc_strSuministros, SqlDbType.NVarChar, 8, mc_strSuministros)
                .Parameters.Add(mc_strArroba & mc_strServiciosEX, SqlDbType.NVarChar, 8, mc_strServiciosEX)
                .Parameters.Add(mc_strArroba & mc_strProceso, SqlDbType.NVarChar, 8, mc_strProceso)

            End With

            Return cmdBodegasXCC

        End Function

        Private Function UpdateCommandBodegasXCC(ByVal p_cnnSCGTaller As SqlClient.SqlConnection, _
                                        ByVal p_strSCGTA_SP_UPDBodegasXCC As String) As SqlClient.SqlCommand

            Dim cmdBodegasXCC As New SqlClient.SqlCommand

            With cmdBodegasXCC

                .CommandText = p_strSCGTA_SP_UPDBodegasXCC
                .CommandType = CommandType.StoredProcedure
                .Connection = p_cnnSCGTaller

                With .Parameters.Add(mc_strArroba & mc_strIDCentroCosto, SqlDbType.Int, 4, mc_strIDCentroCosto)
                    .SourceVersion = DataRowVersion.Original
                End With
                .Parameters.Add(mc_strArroba & mc_strRepuestos, SqlDbType.NVarChar, 8, mc_strRepuestos)
                .Parameters.Add(mc_strArroba & mc_strServicios, SqlDbType.NVarChar, 8, mc_strServicios)
                .Parameters.Add(mc_strArroba & mc_strSuministros, SqlDbType.NVarChar, 8, mc_strSuministros)
                .Parameters.Add(mc_strArroba & mc_strServiciosEX, SqlDbType.NVarChar, 8, mc_strServiciosEX)
                .Parameters.Add(mc_strArroba & mc_strProceso, SqlDbType.NVarChar, 8, mc_strProceso)
                .Parameters.Add(mc_strArroba & mc_strIDCentroCostoNew, SqlDbType.Int, 4, mc_strIDCentroCosto)

            End With

            Return cmdBodegasXCC

        End Function

        Private Function DeleteCommandBodegasXCC(ByVal p_cnnSCGTaller As SqlClient.SqlConnection, _
                                        ByVal p_strSCGTA_SP_DELBodegasXCC As String) As SqlClient.SqlCommand

            Dim cmdBodegasXCC As New SqlClient.SqlCommand

            With cmdBodegasXCC

                .CommandText = p_strSCGTA_SP_DELBodegasXCC
                .CommandType = CommandType.StoredProcedure
                .Connection = p_cnnSCGTaller

                .Parameters.Add(mc_strArroba & mc_strIDCentroCosto, SqlDbType.Int, 4, mc_strIDCentroCosto)

            End With

            Return cmdBodegasXCC

        End Function

#End Region

    End Class

End Namespace