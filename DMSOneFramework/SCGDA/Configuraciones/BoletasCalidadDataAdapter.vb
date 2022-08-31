Namespace SCGDataAccess

    Public Class BoletasCalidadDataAdapter

        Implements IDataAdapter

#Region "Implementaciones"


        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Function FillSchema(ByVal dataSet As System.Data.DataSet, ByVal schemaType As System.Data.SchemaType) As System.Data.DataTable() Implements System.Data.IDataAdapter.FillSchema
            Return Nothing
        End Function

        Public Function GetFillParameters() As System.Data.IDataParameter() Implements System.Data.IDataAdapter.GetFillParameters
            Return Nothing
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
                Return Nothing
            End Get
        End Property

        Public Overloads Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function


#End Region


#Region "Declaraciones"

        Private Const mc_intNoLista As String = "NoLista"
        Private Const mc_intNoCondicion As String = "NoCondicion"
        Private Const mc_intNoFase As String = "NoFase"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        Private m_adpActividad As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDCondicionesCalidad As String = "SCGTA_SP_UPDCondicionesCalidad"
        Private Const mc_strSCGTA_SP_SELCondicionesCalidad As String = "SCGTA_SP_SELCondicionesCalidad"
        Private Const mc_strSCGTA_SP_SELCondicionesCalidadxFase As String = "SCGTA_SP_SELCondicionesCalidadxFase"
        Private Const mc_strSCGTA_SP_INSCondicionesCalidad As String = "SCGTA_SP_INSCondicionesCalidad"
        Private Const mc_strSCGTA_SP_DELCondicionesCalidad As String = "SCGTA_SP_DELCondicionesCalidad"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Dim objDAConexion As DAConexion

#End Region


#Region "Inicializa BoletasCalidadDataAdapter"


        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpActividad = New SqlClient.SqlDataAdapter
        End Sub



#End Region


#Region "Implementaciones SCG"


        Public Overloads Function Fill(ByVal dataSet As BoletasCalidadDataset, ByVal intNoFase As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If
                m_adpActividad.SelectCommand = CrearSelectCommand()

                m_adpActividad.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = intNoFase

                m_adpActividad.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpActividad.Fill(dataSet.SCGTA_TB_CondicionesxLista)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Overloads Function Update(ByVal dataSet As BoletasCalidadDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpActividad.InsertCommand = CreateInsertCommand()
                m_adpActividad.InsertCommand.Connection = m_cnnSCGTaller

                m_adpActividad.UpdateCommand = CrearUpdateCommand()
                m_adpActividad.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpActividad.Update(dataSet.SCGTA_TB_CondicionesxLista)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function


        Public Function Delete(ByVal dataset As BoletasCalidadDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpActividad.UpdateCommand = CrearDeleteCommand()
                m_adpActividad.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpActividad.Update(dataset.SCGTA_TB_CondicionesxLista)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELCondicionesCalidadxFase)

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

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDCondicionesCalidad)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 9, mc_intNoFase)

                    .Add(mc_strArroba & mc_intNoLista, SqlDbType.Int, 9, mc_intNoLista)

                    .Add(mc_strArroba & mc_intNoCondicion, SqlDbType.Int, 9, mc_intNoCondicion)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DELCondicionesCalidad)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoCondicion, SqlDbType.Int, 9, mc_intNoCondicion)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSCondicionesCalidad)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters


                    .Add(mc_strArroba & mc_intNoLista, SqlDbType.Int, 9, mc_intNoLista)

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
