Option Strict On
Option Explicit On 
Namespace SCGDataAccess

    Public Class EstiloDataAdapter

        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strCodEstilo As String = "CodEstilo"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"
        Private Const mc_strCodModelo As String = "CodModelo"
        Private Const mc_strCodMarca As String = "CodMarca"

        Private m_adpEstilo As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDEstilo As String = "SCGTA_SP_UPDEstilo"
        Private Const mc_strSCGTA_SP_SELEstilo As String = "SCGTA_SP_SELEstilo"
        Private Const mc_strSCGTA_SP_INSEstilo As String = "SCGTA_SP_INSEstilo"
        Private Const mc_strSCGTA_SP_DelEstilo As String = "SCGTA_SP_DelEstilo"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion


#End Region


#Region "Inicializa EstiloDataAdapter"


        Public Sub New()
            Call InicializaEstiloDataAdapter(m_cnnSCGTaller)
        End Sub

        Private Sub InicializaEstiloDataAdapter(ByRef cnnTaller As SqlClient.SqlConnection)

            Try

                'cnnTaller = New SqlClient.SqlConnection(conexion)
                objDAConexion = New DAConexion
                cnnTaller = objDAConexion.ObtieneConexion
                m_adpEstilo = New SqlClient.SqlDataAdapter


            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                'Call cnnTaller.Close()
            End Try
        End Sub

#End Region


#Region "Implementaciones"

        Private Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

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
                Throw New NotImplementedException()
            End Get
        End Property

        Private Overloads Function Update(ByVal dataset As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

#End Region


#Region "Implementaciones SCG"


        'Public Overloads Function Fill(ByVal dataSet As EstiloDataset) As Integer

        '    Try
        '        If m_cnnSCGTaller.State = ConnectionState.Closed Then
        '            If m_cnnSCGTaller.ConnectionString = "" Then
        '                m_cnnSCGTaller.ConnectionString = strConexionADO
        '            End If
        '            m_cnnSCGTaller.Open()
        '        End If

        '        m_adpEstilo.SelectCommand = CrearSelectCommand()
        '        m_adpEstilo.SelectCommand.Connection = m_cnnSCGTaller
        '        Call m_adpEstilo.Fill(dataSet.SCGTA_TB_Estilo)

        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        Call m_cnnSCGTaller.Close()
        '    End Try

        'End Function

        Public Function CargaEstilosdeVehiculo(ByRef datareader As SqlClient.SqlDataReader, ByVal strCodMarca As String) As Boolean

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpEstilo.SelectCommand = CrearSelectCommand()
                m_adpEstilo.SelectCommand.Parameters.Item(mc_strArroba & mc_strCodMarca).Value = strCodMarca
                m_adpEstilo.SelectCommand.Connection = m_cnnSCGTaller
                datareader = m_adpEstilo.SelectCommand.ExecuteReader(CommandBehavior.CloseConnection)


                Return True
            Catch ex As Exception
                Throw ex
            Finally
                'Agregado
                'Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As EstiloDataset, ByVal decCodMarca As Decimal, ByVal decCodModelo As Decimal) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpEstilo.SelectCommand = CrearSelectCommand()

                m_adpEstilo.SelectCommand.Connection = m_cnnSCGTaller

                m_adpEstilo.SelectCommand.Parameters(mc_strArroba & mc_strCodMarca).Value = decCodMarca

                m_adpEstilo.SelectCommand.Parameters(mc_strArroba & mc_strCodModelo).Value = decCodModelo

                Call m_adpEstilo.Fill(dataSet.SCGTA_TB_Estilo)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function



        Public Overloads Function Update(ByVal dataSet As EstiloDataset, ByVal decCodMarca As Decimal, ByVal decCodModelo As Decimal) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpEstilo.InsertCommand = CreateInsertCommand()
                m_adpEstilo.InsertCommand.Connection = m_cnnSCGTaller

                m_adpEstilo.UpdateCommand = CrearUpdateCommand()
                m_adpEstilo.UpdateCommand.Connection = m_cnnSCGTaller

                m_adpEstilo.UpdateCommand.Parameters(mc_strArroba & mc_strCodMarca).Value = decCodMarca
                m_adpEstilo.UpdateCommand.Parameters(mc_strArroba & mc_strCodModelo).Value = decCodModelo

                Call m_adpEstilo.Update(dataSet.SCGTA_TB_Estilo)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Function Delete(ByVal dataset As EstiloDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpEstilo.UpdateCommand = CrearDeleteCommand()
                m_adpEstilo.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpEstilo.Update(dataset.SCGTA_TB_Estilo)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try


        End Function


#End Region


#Region "Creacion de Comandos del DataAdapter"


        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELEstilo)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.NVarChar, 8, mc_strCodMarca)
                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDEstilo)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strCodEstilo, SqlDbType.Int, 4, mc_strCodEstilo)
                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)
                    .Add(mc_strArroba & mc_strCodModelo, SqlDbType.Decimal, 9, mc_strCodModelo)
                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.Decimal, 9, mc_strCodMarca)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelEstilo)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strCodEstilo, SqlDbType.Int, 4, mc_strCodEstilo)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSEstilo)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)
                    .Add(mc_strArroba & mc_strCodModelo, SqlDbType.Decimal, 9, mc_strCodModelo)
                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.Decimal, 9, mc_strCodMarca)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try

        End Function


#End Region


    End Class

End Namespace