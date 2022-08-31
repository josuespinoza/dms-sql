Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion


Public Class LineasSolicitudOTEspecialDataAdapter
    Implements IDataAdapter


#Region "Inicializa ClientesDataAdapter"

    Public Sub New()

        objDAConexion = New DAConexion
        m_cnnSCGTaller = objDAConexion.ObtieneConexion
        m_adpSOT = New SqlClient.SqlDataAdapter

    End Sub


#End Region


#Region "Declaraciones"


    'Declaración de las constantes con el nombre de las columnas del Dataset.
    Private Const mc_strDocEntry As String = "DocEntry"
    Private Const mc_strItemCode As String = "U_ItemCode"
    Private Const mc_strIdRepuesto As String = "U_IdRxO"
    Private Const mc_strNoOrden As String = "NOORDEN"

    'Declaracion de las constantes con el nombre de los procedimientos almacenados
    Private Const mc_strSCGTA_SP_SEL_LineasSolOTEsp As String = "SCGTA_SP_SEL_LineasSolOTEspecial"

    Private m_adpSOT As SqlClient.SqlDataAdapter

    Private m_cnnSCGTaller As SqlClient.SqlConnection

    Private Const mc_strArroba As String = "@"

    Dim objDAConexion As DAConexion
#End Region


#Region "Implementaciones SCG"

    Public Overloads Function Fill(ByVal dataSet As LineasSolOTEspecialDataSet, ByVal p_strNoOrden As String) As Integer

        Try

            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                If m_cnnSCGTaller.ConnectionString = "" Then
                    m_cnnSCGTaller.ConnectionString = strConexionADO
                End If
                m_cnnSCGTaller.Open()
            End If

            'Creacion del comando
            m_adpSOT.SelectCommand = CrearSelectCommand()
            m_adpSOT.SelectCommand.Connection = m_cnnSCGTaller

            '-------------------------------------Se cargan los parámetros----------------------------------------
            m_adpSOT.SelectCommand.Parameters.Item(mc_strArroba + mc_strNoOrden).Value = p_strNoOrden

            Call m_adpSOT.Fill(dataSet.LineasSolicitudOTEspecial)

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

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SEL_LineasSolOTEsp)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.NVarChar, 200, "mc_strNoOrden")

            End With

            Return cmdSel

        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region


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
End Class
