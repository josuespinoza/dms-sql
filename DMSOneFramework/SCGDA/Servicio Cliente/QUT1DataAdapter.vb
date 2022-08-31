Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion

Namespace SCGDataAccess

    Public Class QUT1DataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_strNoCita As String = "NoCita"
        Private Const mc_strNoCotizacion As String = "NoCotizacion"
        Private Const mc_strNoConsecutivo As String = "NoConsecutivo"
        Private Const mc_strCardCode As String = "CardCode"
        Private Const mc_strIDVehiculo As String = "IDVehiculo"
        Private Const mc_strNoVehiculo As String = "NoVehiculo"
        Private Const mc_strNoSerie As String = "NoSerie"
        Private Const mc_strValidaRepPendientes As String = "ValidaRepPendientes"

        'Declaracion de las constantes con el nombre de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_SELQUT1 As String = "SCGTA_SP_SELQUT1"

        Private m_adpQUT1 As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Dim objDAConexion As DAConexion


#End Region

#Region "Inicializa ClientesDataAdapter"

        Public Sub New()

            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion
            m_adpQUT1 = New SqlClient.SqlDataAdapter

        End Sub


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

#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As QUT1Dataset, ByVal p_intNoCotizacion As Integer, ByVal p_strValidaRepPendientes As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpQUT1.SelectCommand = CrearSelectCommand()
                m_adpQUT1.SelectCommand.Connection = m_cnnSCGTaller

                '-------------------------------------Se cargan los parámetros----------------------------------------
                m_adpQUT1.SelectCommand.Parameters.Item(mc_strArroba + mc_strNoCotizacion).Value = p_intNoCotizacion
                m_adpQUT1.SelectCommand.Parameters.Item(mc_strArroba + mc_strValidaRepPendientes).Value = p_strValidaRepPendientes

                Call m_adpQUT1.Fill(dataSet.QUT1)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELQUT1)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strNoCotizacion, SqlDbType.Int, 4, mc_strNoCotizacion)
                    .Add(mc_strArroba & mc_strValidaRepPendientes, SqlDbType.VarChar, 10, mc_strValidaRepPendientes)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

    End Class

End Namespace