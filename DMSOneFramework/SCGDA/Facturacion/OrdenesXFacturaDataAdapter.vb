Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Namespace SCGDataAccess
    Public Class OrdenesXFacturaDataAdapter
        Implements IDataAdapter

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

            m_adpRFactura = New SqlClient.SqlDataAdapter
        End Sub


#End Region

#Region "Variables"

        Private m_adpRFactura As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Dim objDAConexion As DAConexion

#End Region

#Region "Constantes"

        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_intdocnum As String = "docnum"
        Private Const mc_intdoctotal As String = "doctotal"
        Private Const mc_strnoorden As String = "noorden"
        Private Const mc_strestado As String = "est_fac"
        Private Const mc_strArroba As String = "@"
        Private Const mc_strFacturada As String = "Facturada"



        'Declaracion de las constantes con el nombre de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_SelOrdenesXFactura As String = "SCGTA_SP_SelOrdenesXFactura"
     

#End Region

#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As OrdenesxFacturaDataset, ByVal ORDEN As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpRFactura.SelectCommand = CrearSelectCommand()

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado
                If ORDEN = "" Then
                    m_adpRFactura.SelectCommand.Parameters(mc_strArroba & mc_strnoorden).Value = System.DBNull.Value
                Else
                    m_adpRFactura.SelectCommand.Parameters(mc_strArroba & mc_strnoorden).Value = ORDEN
                End If


                m_adpRFactura.SelectCommand.Connection = m_cnnSCGTaller


                Call m_adpRFactura.Fill(dataSet.SCGTA_SP_SelOrdenesXFactura)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function
#End Region

#Region "Creación de comandos"

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelOrdenesXFactura)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strnoorden, SqlDbType.VarChar, 50, mc_strnoorden)


            End With

            Return cmdSel

        End Function
#End Region

    End Class
End Namespace
