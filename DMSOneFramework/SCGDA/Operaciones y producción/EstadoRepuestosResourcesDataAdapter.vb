Namespace SCGDataAccess
    Public Class EstadoRepuestosResourcesDataAdapter

        Implements IDataAdapter, IDisposable

#Region "Declaraciones"


        'TODO Agregar nombres de procedimeintos almacenados para una determinada tabla
        Private Const mc_strSPIns As String = ""
        Private Const mc_strSPUpd As String = ""
        Private Const mc_strSPDel As String = ""
        Private Const mc_strSPSel As String = ""
        Private Const mc_strEstaLlaveExiste As String = ""
        Private Const mc_strArroba As String = "@"
        Private Const mc_strCultura As String = "Cultura"


        ' Private m_adpAdapter As SqlClient.SqlDataAdapter
        Private objDAConexion As DAConexion
        Private m_cnnSCGTaller As SqlClient.SqlConnection
        Private m_adpEstRepResources As SqlClient.SqlDataAdapter
        Private Const SCGTA_SP_SelRetornaEstadoRepuestos As String = "SCGTA_SP_SelRetornaEstadoRepuestos"
        Private Const mc_strTodasColumnas As String = "TodasColumnas"


#End Region

#Region "Inicializar AnalisisDataAdapter"

        Public Sub New(ByVal strCadenaConexion As String)
            Try
                m_cnnSCGTaller = New SqlClient.SqlConnection(strCadenaConexion)
                m_adpEstRepResources = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion
            m_adpEstRepResources = New SqlClient.SqlDataAdapter
        End Sub


#End Region

#Region "Implementaciones"
        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Overloads Function Fill(ByRef dstDataset As EstadoRepuestosResourcesDataset) As Integer
            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpEstRepResources.SelectCommand = CrearCmdSel()
                m_adpEstRepResources.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpEstRepResources.Fill(dstDataset.SCGTA_TB_EstadoRepuestoResources)

            Catch ex As Exception
                MsgBox(ex.Message)
                Return 1
            Finally
                Call m_cnnSCGTaller.Close()
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

            End Get
        End Property

        Public Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

        Public Sub Dispose() Implements System.IDisposable.Dispose
            Try
                If m_cnnSCGTaller.State = ConnectionState.Open Then
                    Call m_cnnSCGTaller.Close()
                    Call m_cnnSCGTaller.Dispose()
                    m_cnnSCGTaller = Nothing
                End If

                If Not m_adpEstRepResources Is Nothing Then
                    Call m_adpEstRepResources.Dispose()
                    m_adpEstRepResources = Nothing
                End If
            Catch ex As Exception

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
                Return Nothing
            Finally
            End Try

        End Function

        Private Function CrearCmdDel() As SqlClient.SqlCommand

            Dim cmdDel As SqlClient.SqlCommand
            'Dim param As SqlClient.SqlParameter

            Try

                cmdDel = New SqlClient.SqlCommand(mc_strSPDel)
                cmdDel.CommandType = CommandType.StoredProcedure

                With cmdDel.Parameters


                    'TODO agregar campos para el comando de borrado


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

                cmdUpd = New SqlClient.SqlCommand(mc_strSPUpd)
                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters


                    'TODO agregar campos para el comando de actualizacion


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
                cmdSel = New SqlClient.SqlCommand
                ' cmdSel.CommandType = CommandType.Text

                With cmdSel
                    .CommandText = "Select codigo as codigo, Descripcion, Cultura from SCGTA_TB_EstadoRepuestoResources union select codestadorep as codigo, descripcion, '' as Cultura from  SCGTA_TB_EstadoRepuesto"
                End With

                Return cmdSel
            Catch ex As Exception
                Return Nothing
            End Try
        End Function
#End Region


    End Class
End Namespace
