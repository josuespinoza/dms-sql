Namespace SCGDataAccess


Public Class RazonesSuspensionDataAdapter

    Implements IDataAdapter


#Region "Inicializa RazonesSuspensionDataAdapter"
        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpSuspension = New SqlClient.SqlDataAdapter
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
        Private Const mc_intIDRazon As String = "IDRazon"
        Private Const mc_intNoFase As String = "NoFase"
        Private Const mc_strDescripcion As String = "Razon"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        'Objeto de adapter.
        Private m_adpSuspension As SqlClient.SqlDataAdapter

        'Constantes con los nombres de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_UPDRazonesSuspension As String = "SCGTA_SP_UPDRazonesSuspension"
        Private Const mc_strSCGTA_SP_SELRazonesSuspension As String = "SCGTA_SP_SELRazonesSuspension"
        Private Const mc_strSCGTA_SP_INSRazonesSuspension As String = "SCGTA_SP_INSRazonesSuspension"
        Private Const mc_strSCGTA_SP_DELRazonesSuspension As String = "SCGTA_SP_DELRazonesSuspension"

        'Objeto de conexión
        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion

#End Region


#Region "Implementaciones SCG"


        Public Overloads Function Fill(ByVal dataSet As RazonesSuspensionDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If
                m_adpSuspension.SelectCommand = CrearSelectCommand()

                m_adpSuspension.SelectCommand.Connection = m_cnnSCGTaller

                'm_adpSuspension.SelectCommand.Parameters(mc_strArroba & mc_intNoFase).Value = intFaseProduccion

                Call m_adpSuspension.Fill(dataSet.SCGTA_TB_RazonSuspension)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function

        Public Overloads Function Fill(ByRef dataReader As SqlClient.SqlDataReader) As Integer

            Dim cmdRazones As New SqlClient.SqlCommand

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If
                cmdRazones = CrearSelectCommand()

                cmdRazones.Connection = m_cnnSCGTaller

                dataReader = cmdRazones.ExecuteReader

            Catch ex As Exception

                Throw ex

            Finally
                'Agregado
                'Call m_cnnSCGTaller.Close()

            End Try


        End Function

        Public Overloads Function Update(ByVal dataSet As RazonesSuspensionDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If


                m_adpSuspension.InsertCommand = CreateInsertCommand()
                m_adpSuspension.InsertCommand.Connection = m_cnnSCGTaller

                m_adpSuspension.UpdateCommand = CrearUpdateCommand()
                m_adpSuspension.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpSuspension.Update(dataSet.SCGTA_TB_RazonSuspension)

            Catch ex As Exception

                Throw ex

            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Function Delete(ByVal dataset As RazonesSuspensionDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpSuspension.DeleteCommand = CrearDeleteCommand()

                m_adpSuspension.DeleteCommand.Connection = m_cnnSCGTaller

                Call m_adpSuspension.Update(dataset.SCGTA_TB_RazonSuspension)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELRazonesSuspension)

                cmdSel.CommandType = CommandType.StoredProcedure
                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_intIDRazon, SqlDbType.Int, 4, mc_intIDRazon)

                End With
                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDRazonesSuspension)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intIDRazon, SqlDbType.Int, 9, mc_intIDRazon)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DELRazonesSuspension)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intIDRazon, SqlDbType.Int, 9, mc_intIDRazon)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSRazonesSuspension)

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
