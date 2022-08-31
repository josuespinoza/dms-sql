Namespace SCGDataAccess

    Public Class SeccionesDataAdapter

        Implements IDataAdapter


#Region "Declaraciones"

        Private Const mc_intNoSeccion As String = "NoSeccion"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        Private m_adpSeccion As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDSeccion As String = "SCGTA_SP_UPDSeccion"
        Private Const mc_strSCGTA_SP_SELSecciones As String = "SCGTA_SP_SELSecciones"
        Private Const mc_strSCGTA_SP_INSSeccion As String = "SCGTA_SP_INSSeccion"
        Private Const mc_strSCGTA_SP_DELSeccion As String = "SCGTA_SP_DELSeccion"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion


#End Region

#Region "Inicializa SeccionesDataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpSeccion = New SqlClient.SqlDataAdapter
        End Sub

#End Region

#Region "Implementaciones"

        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Overloads Function FillSchema(ByVal dataSet As System.Data.DataSet, ByVal schemaType As System.Data.SchemaType) As System.Data.DataTable() Implements System.Data.IDataAdapter.FillSchema

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


#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As SeccionesDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpSeccion.SelectCommand = CrearSelectCommand()

                m_adpSeccion.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpSeccion.Fill(dataSet.SCGTA_TB_Seccion)

            Catch ex As Exception

                Throw ex
            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Overloads Function Update(ByVal dataSet As SeccionesDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If


                m_adpSeccion.InsertCommand = CreateInsertCommand()
                m_adpSeccion.InsertCommand.Connection = m_cnnSCGTaller

                m_adpSeccion.UpdateCommand = CrearUpdateCommand()
                m_adpSeccion.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpSeccion.Update(dataSet.SCGTA_TB_Seccion)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Function Delete(ByVal dataset As SeccionesDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpSeccion.UpdateCommand = CrearDeleteCommand()
                m_adpSeccion.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpSeccion.Update(dataset.SCGTA_TB_Seccion)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELSecciones)

                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDSeccion)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoSeccion, SqlDbType.Int, 9, mc_intNoSeccion)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DELSeccion)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoSeccion, SqlDbType.Int, 9, mc_intNoSeccion)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSSeccion)

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
