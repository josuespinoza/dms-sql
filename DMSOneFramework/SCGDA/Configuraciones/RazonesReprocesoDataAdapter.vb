Imports DMSOneFramework.SCGDataAccess.DAConexion


Namespace SCGDataAccess
    Public Class RazonesReprocesoDataAdapter
        Implements IDataAdapter


#Region "Inicializa RazonesReprocesoDataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpReproceso = New SqlClient.SqlDataAdapter
        End Sub

#End Region


#Region "Implementaciones .Net Framework"

        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

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

        Public Overloads Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

#End Region


#Region "Declaraciones"

        'Constantes con los nombres de las columnas.
        Private Const mc_intNoReproceso As String = "NoReproceso"
        Private Const mc_intNoFase As String = "NoFase"
        Private Const mc_strDescripcion As String = "Razon"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        'Objeto de adapter.
        Private m_adpReproceso As SqlClient.SqlDataAdapter

        'Constantes con los nombres de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_UPDRazonesReproceso As String = "SCGTA_SP_UPDRazonesReproceso"
        Private Const mc_strSCGTA_SP_SELRazonesReproceso As String = "SCGTA_SP_SELRazonesReproceso"
        Private Const mc_strSCGTA_SP_INSRazonesReproceso As String = "SCGTA_SP_INSRazonesReproceso"
        Private Const mc_strSCGTA_SP_DELRazonesReproceso As String = "SCGTA_SP_DELRazonesReproceso"

        'Objeto de conexión
        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion
#End Region


#Region "Implementaciones SCG"


        Public Overloads Function Fill(ByVal dataSet As RazonesReprocesoDataset, ByVal intFaseProduccion As Integer) As Integer

            Try


                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpReproceso.SelectCommand = CrearSelectCommand()

                m_adpReproceso.SelectCommand.Connection = m_cnnSCGTaller

                m_adpReproceso.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = intFaseProduccion

                Call m_adpReproceso.Fill(dataSet.SCGTA_TB_Reproceso)

            Catch ex As Exception

                Throw ex
            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function


        Public Overloads Function Update(ByVal dataSet As RazonesReprocesoDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpReproceso.InsertCommand = CreateInsertCommand()
                m_adpReproceso.InsertCommand.Connection = m_cnnSCGTaller

                m_adpReproceso.UpdateCommand = CrearUpdateCommand()
                m_adpReproceso.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpReproceso.Update(dataSet.SCGTA_TB_Reproceso)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Function Delete(ByVal dataset As RazonesReprocesoDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpReproceso.UpdateCommand = CrearDeleteCommand()

                m_adpReproceso.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpReproceso.Update(dataset.SCGTA_TB_Reproceso)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELRazonesReproceso)

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

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDRazonesReproceso)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    '.Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 9, mc_intNoFase)

                    .Add(mc_strArroba & mc_intNoReproceso, SqlDbType.Int, 9, mc_intNoReproceso)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DELRazonesReproceso)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoReproceso, SqlDbType.Int, 9, mc_intNoReproceso)
                    '.Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 4, mc_intNoFase)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSRazonesReproceso)

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
