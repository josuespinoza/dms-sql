Namespace SCGDataAccess
    Public Class RegNotaCreditoDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_intnotacredito As String = "NotaCredito"
        Private Const mc_intNoFactura As String = "NoFactura"
        Private Const mc_datFecha As String = "Fecha_Anula"

        Private m_adpAnula As SqlClient.SqlDataAdapter

      
        Private Const mc_strSCGTA_SP_InsRegNC As String = "SCGTA_SP_INSRegNota_Credito"
      

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

#End Region

#Region "Implementaciones"

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


#Region "Inicializa DataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpAnula = New SqlClient.SqlDataAdapter
        End Sub

#End Region

#Region "Implementaciones SCG"

        Public Function Inserta(ByVal dataset As RegNotaCreditoDataset) As String

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAnula.InsertCommand = CreateInsertCommand()
                m_adpAnula.InsertCommand.Connection = m_cnnSCGTaller

                Call m_adpAnula.Update(dataset.SCGTA_TB_RegNotas_Credito)


            Catch ex As Exception

                Throw ex

            Finally
                m_cnnSCGTaller.Close()

            End Try
            Return String.Empty
        End Function

#End Region


#Region "Creación de comandos"
        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsRegNC)

            cmdIns.CommandType = CommandType.StoredProcedure

            With cmdIns.Parameters

                .Add(mc_strArroba & mc_intnotacredito, SqlDbType.Int, 4, mc_intnotacredito)

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                .Add(mc_strArroba & mc_intNoFactura, SqlDbType.Int, 4, mc_intNoFactura)

                .Add(mc_strArroba & mc_datFecha, SqlDbType.DateTime, 8, mc_datFecha)


            End With

            Return cmdIns

        End Function
#End Region

    End Class
End Namespace