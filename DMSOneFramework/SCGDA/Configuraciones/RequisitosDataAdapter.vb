Imports DMSOneFramework.SCGDataAccess.DAConexion
Namespace SCGDataAccess
    Public Class RequisitosDataAdapter

        Implements IDataAdapter


#Region "Implementaciones .Net Framework"

        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Function FillSchema(ByVal dataSet As System.Data.DataSet, ByVal schemaType As System.Data.SchemaType) As System.Data.DataTable() Implements System.Data.IDataAdapter.FillSchema

        End Function

        Public Function GetFillParameters() As System.Data.IDataParameter() Implements System.Data.IDataAdapter.GetFillParameters

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

        Public Overloads Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

#End Region


#Region "Declaraciones"

        'Constantes de los nombre de las columnas
        Private Const mc_strNoRequisito As String = "NoRequisito"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        Private m_adpRequisito As SqlClient.SqlDataAdapter

        'Constantes de los nombres de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_UPDRequisitos As String = "SCGTA_SP_UPDRequisitos"
        Private Const mc_strSCGTA_SP_SELRequisitos As String = "SCGTA_SP_SELRequisitos"
        Private Const mc_strSCGTA_SP_INSRequisitos As String = "SCGTA_SP_INSRequisitos"
        Private Const mc_strSCGTA_SP_DELRequisitos As String = "SCGTA_SP_DELRequisitos"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

#End Region


#Region "Inicializa RequisitosDataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpRequisito = New SqlClient.SqlDataAdapter
        End Sub

#End Region


#Region "Implementaciones SCG"

        'Metodo utilizado para la seleccion de requisitos que se cargan en el dataset
        Public Overloads Function Fill(ByVal dataSet As RequisitosDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If
                m_adpRequisito.SelectCommand = CrearSelectCommand()
                m_adpRequisito.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adpRequisito.Fill(dataSet.SCGTA_TB_Requisitos)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        'metodo utilizado para la inserción y modificación de datos.
        Public Overloads Function Update(ByVal dataSet As RequisitosDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpRequisito.InsertCommand = CreateInsertCommand()
                m_adpRequisito.InsertCommand.Connection = m_cnnSCGTaller

                m_adpRequisito.UpdateCommand = CrearUpdateCommand()
                m_adpRequisito.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpRequisito.Update(dataSet.SCGTA_TB_Requisitos)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        'Metodo utilizado para la eliminación lógica de los requisitos (es un update del estado lógico).
        Public Function Delete(ByVal dataset As RequisitosDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpRequisito.UpdateCommand = CrearDeleteCommand()
                m_adpRequisito.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpRequisito.Update(dataset.SCGTA_TB_Requisitos)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELRequisitos)

                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDRequisitos)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoRequisito, SqlDbType.Int, 9, mc_strNoRequisito)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DELRequisitos)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoRequisito, SqlDbType.Int, 9, mc_strNoRequisito)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSRequisitos)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

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