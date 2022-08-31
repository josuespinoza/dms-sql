Namespace SCGDataAccess


    Public Class PaquetesxOrdenDataAdapter
        Implements IDataAdapter


        Private Const mc_strSCGTA_SP_SelPaquetes As String = "SCGTA_SP_SelPaquetesOrden"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

        Private m_strConexion As String
        Private Const mc_strNoOrden As String = "NoOrden"


        Private m_adpAct As SqlClient.SqlDataAdapter



        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpAct = New SqlClient.SqlDataAdapter
        End Sub

        Public Sub New(ByVal conexion As String)
            Try
                m_strConexion = conexion
                m_cnnSCGTaller = New SqlClient.SqlConnection(conexion)
                m_adpAct = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub


        Public Overloads Function Fill(ByVal dataSet As PaquetesDataSet, ByVal decNoOrden As String) As Integer
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    Call m_cnnSCGTaller.Open()
                    'If m_cnnSCGTaller.ConnectionString = "" Then
                    '    m_cnnSCGTaller.ConnectionString = strConexionADO
                    'End If
                    'Call m_cnnSCGTaller.Open()
                End If

                m_adpAct.SelectCommand = CrearSelectCommandPaquete()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpAct.SelectCommand.CommandTimeout = 480
                m_adpAct.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = decNoOrden

                m_adpAct.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpAct.Fill(dataSet._PaquetesDataSet)


            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Private Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Function FillSchema(ByVal dataSet As System.Data.DataSet, ByVal schemaType As System.Data.SchemaType) As System.Data.DataTable() Implements System.Data.IDataAdapter.FillSchema

        End Function

        Public Function GetFillParameters() As System.Data.IDataParameter() Implements System.Data.IDataAdapter.GetFillParameters

        End Function

        Public Property MissingMappingAction As System.Data.MissingMappingAction Implements System.Data.IDataAdapter.MissingMappingAction
            Get

            End Get
            Set(ByVal value As System.Data.MissingMappingAction)

            End Set
        End Property

        Public Property MissingSchemaAction As System.Data.MissingSchemaAction Implements System.Data.IDataAdapter.MissingSchemaAction
            Get

            End Get
            Set(ByVal value As System.Data.MissingSchemaAction)

            End Set
        End Property

        Public ReadOnly Property TableMappings As System.Data.ITableMappingCollection Implements System.Data.IDataAdapter.TableMappings
            Get

            End Get
        End Property

        Public Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

        Private Function CrearSelectCommandPaquete() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelPaquetes)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

            End With

            Return cmdSel


        End Function
    End Class

End Namespace
