Namespace SCGDataAccess
    Public Class Expediente_ImagenesDataAdapter

        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_Id As String = "Id"
        Private Const mc_IdVisita As String = "IdVisita"
        Private Const mc_Tipo As String = "Tipo"

        Private m_adpExpe_Img As SqlClient.SqlDataAdapter

        'Private Const mc_strSCGTA_SP_UPDActividad As String = "SCGTA_SP_UpdActividades"
        Private Const mc_strSCGTA_SP_SELExpe_Img As String = "SCGTA_SP_SELExpediente_Imagenes"
        'Private Const mc_strSCGTA_SP_INSActividad As String = "SCGTA_SP_INSActividades"
        'Private Const mc_strSCGTA_SP_DelActividad As String = "SCGTA_SP_DELActividad"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

#End Region

#Region "Inicializa Expediente_ImgDataAdapter"

        'Public Sub New(ByVal gc_Conexion As String)

        '    Call InicializaActividadesDataAdapter(m_cnnSCGTaller, gc_Conexion)

        'End Sub

        Public Sub New()

            Call InicializaExpe_ImgDataAdapter(m_cnnSCGTaller)

        End Sub
        Private Sub InicializaExpe_ImgDataAdapter(ByRef cnnTaller As SqlClient.SqlConnection)
            Try

                ' cnnTaller = New SqlClient.SqlConnection(conexion)
                objDAConexion = New DAConexion
                cnnTaller = objDAConexion.ObtieneConexion

                m_adpExpe_Img = New SqlClient.SqlDataAdapter

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
#End Region

#Region "Implementaciones SCG"


        Public Overloads Function Fill(ByRef dataSet As Visita_ImagenesDataset, ByVal intExpediente As Integer, ByVal intTipo As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpExpe_Img.SelectCommand = CrearSelectCommand()

                m_adpExpe_Img.SelectCommand.Connection = m_cnnSCGTaller

                m_adpExpe_Img.SelectCommand.Parameters(mc_strArroba & mc_IdVisita).Value = intExpediente
                m_adpExpe_Img.SelectCommand.Parameters(mc_strArroba & mc_Tipo).Value = intTipo

                Call m_adpExpe_Img.Fill(dataSet.SCGTA_TB_Exped_Img)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function



#End Region

#Region "Creación de comandos"


        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELExpe_Img)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_IdVisita, SqlDbType.Int, 9, mc_IdVisita)
                    .Add(mc_strArroba & mc_Tipo, SqlDbType.Int, 4, mc_Tipo)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function


        'Private Function CrearUpdateCommand() As SqlClient.SqlCommand

        '    Try

        '        Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDActividad)

        '        cmdIns.CommandType = CommandType.StoredProcedure

        '        With cmdIns.Parameters

        '            .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 9, mc_intNoFase)

        '            .Add(mc_strArroba & mc_intNoActividad, SqlDbType.Int, 9, mc_intNoActividad)

        '            .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

        '        End With

        '        Return cmdIns

        '    Catch ex As Exception

        '    End Try

        'End Function


        'Private Function CrearDeleteCommand() As SqlClient.SqlCommand

        '    Try

        '        Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelActividad)

        '        cmdIns.CommandType = CommandType.StoredProcedure

        '        With cmdIns.Parameters

        '            .Add(mc_strArroba & mc_intNoActividad, SqlDbType.Int, 9, mc_intNoActividad)
        '            .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 4, mc_intNoFase)

        '        End With

        '        Return cmdIns

        '    Catch ex As Exception

        '    End Try

        'End Function


        'Private Function CreateInsertCommand() As SqlClient.SqlCommand

        '    Try

        '        Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSActividad)

        '        cmdIns.CommandType = CommandType.StoredProcedure

        '        With cmdIns.Parameters

        '            .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 9, mc_intNoFase)

        '            .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

        '        End With

        '        Return cmdIns

        '    Catch ex As Exception

        '    End Try

        'End Function


#End Region

    End Class
End Namespace