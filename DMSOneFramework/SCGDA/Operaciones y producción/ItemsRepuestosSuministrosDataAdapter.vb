Imports DMSOneFramework.SCGDataAccess.DAConexion

Namespace SCGDataAccess

    Public Class ItemsRepuestosSuministrosDataAdapter
        Implements IDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private m_adpItems As SqlClient.SqlDataAdapter



        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

#Region "Constructor"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpItems = New SqlClient.SqlDataAdapter
        End Sub

        Public Sub New(ByVal strCadenaConexion As String)

            m_cnnSCGTaller = New SqlClient.SqlConnection(strCadenaConexion)

            m_adpItems = New SqlClient.SqlDataAdapter

        End Sub


#End Region

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
            Set(ByVal value As System.Data.MissingMappingAction)

            End Set
        End Property

        Public Property MissingSchemaAction() As System.Data.MissingSchemaAction Implements System.Data.IDataAdapter.MissingSchemaAction
            Get

            End Get
            Set(ByVal value As System.Data.MissingSchemaAction)

            End Set
        End Property

        Public ReadOnly Property TableMappings() As System.Data.ITableMappingCollection Implements System.Data.IDataAdapter.TableMappings
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

        Public Overloads Function Fill_ItemsRepuestosSuministros(ByVal dataSet As ItemsRepuestosSuministrosDataset, ByVal p_orden As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                ' m_adpItems = New SqlDataAdapter

                m_adpItems.SelectCommand = Me.CrearSelectCommandItemsRepuestosSuministros(p_orden)
                m_adpItems.SelectCommand.Connection = m_cnnSCGTaller

                Fill_ItemsRepuestosSuministros = m_adpItems.Fill(dataSet.ItemsRepuestosSuministros)


            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Private Function CrearSelectCommandItemsRepuestosSuministros(ByVal p_Orden As String) As SqlClient.SqlCommand
            Try

                Dim cmdSel As New SqlClient.SqlCommand("SCGTA_SP_SELItemsRespuestoSuministros")

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & "NoOrden", SqlDbType.VarChar, 80).Value = p_Orden


                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try
        End Function
    End Class
End Namespace

