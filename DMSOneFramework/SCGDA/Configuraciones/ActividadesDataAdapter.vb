Namespace SCGDataAccess
    Public Class ActividadesDataAdapter

        Implements IDataAdapter


#Region "Declaraciones"

        Private Const mc_intNoActividad As String = "NoActividad"
        Private Const mc_intNoFase As String = "NoFase"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        Private m_adpActividad As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDActividad As String = "SCGTA_SP_UpdActividades"
        Private Const mc_strSCGTA_SP_SELActividad As String = "SCGTA_SP_SELActividades"
        Private Const mc_strSCGTA_SP_INSActividad As String = "SCGTA_SP_INSActividades"
        Private Const mc_strSCGTA_SP_DelActividad As String = "SCGTA_SP_DELActividad"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

#End Region


#Region "Inicializa ActividadesDataAdapter"

        'Public Sub New(ByVal gc_Conexion As String)

        '    Call InicializaActividadesDataAdapter(m_cnnSCGTaller, gc_Conexion)

        'End Sub

        Public Sub New()

            Call InicializaActividadesDataAdapter(m_cnnSCGTaller)

        End Sub
        Private Sub InicializaActividadesDataAdapter(ByRef cnnTaller As SqlClient.SqlConnection)
            Try

                ' cnnTaller = New SqlClient.SqlConnection(conexion)
                objDAConexion = New DAConexion
                cnnTaller = objDAConexion.ObtieneConexion

                m_adpActividad = New SqlClient.SqlDataAdapter

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
#End Region


#Region "Implementaciones SCG"


        Public Overloads Function Fill(ByVal dataSet As ActividadesDataset, ByVal intFaseProduccion As Integer) As Integer

            Try

                'Call m_cnnSCGTaller.Open()

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpActividad.SelectCommand = CrearSelectCommand()

                m_adpActividad.SelectCommand.Connection = m_cnnSCGTaller

                m_adpActividad.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = intFaseProduccion

                Call m_adpActividad.Fill(dataSet.SCGTA_TB_Actividades)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function


        Public Overloads Function Update(ByVal dataSet As ActividadesDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpActividad.InsertCommand = CreateInsertCommand()
                m_adpActividad.InsertCommand.Connection = m_cnnSCGTaller

                m_adpActividad.UpdateCommand = CrearUpdateCommand()
                m_adpActividad.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpActividad.Update(dataSet.SCGTA_TB_Actividades)

            Catch ex As Exception

                Throw ex
            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Function Delete(ByVal dataset As ActividadesDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpActividad.UpdateCommand = CrearDeleteCommand()
                m_adpActividad.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpActividad.Update(dataset.SCGTA_TB_Actividades)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELActividad)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 9, mc_intNoFase)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDActividad)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 9, mc_intNoFase)

                    .Add(mc_strArroba & mc_intNoActividad, SqlDbType.Int, 9, mc_intNoActividad)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelActividad)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoActividad, SqlDbType.Int, 9, mc_intNoActividad)
                    .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 4, mc_intNoFase)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSActividad)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 9, mc_intNoFase)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


#End Region


    End Class
End Namespace
