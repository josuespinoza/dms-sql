Option Strict On
Option Explicit On 
Namespace SCGDataAccess
    Public Class TipoOrdenDataAdapter

        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strCodTipoOrden As String = "CodTipoOrden"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strCodCentroCosto As String = "CodCentroCosto"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        Private m_adpTipoOrden As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UpdTipoOrden As String = "SCGTA_SP_UPDTipoOrden"
        Private Const mc_strSCGTA_SP_SELTipoOrden As String = "SCGTA_SP_SELTipoOrden"
        Private Const mc_strSCGTA_SP_INSTipoOrden As String = "SCGTA_SP_INSTipoOrden"
        Private Const mc_strSCGTA_SP_DelTipoOrden As String = "SCGTA_SP_DelTipoOrden"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Dim objDAConexion As DAConexion


#End Region


#Region "Inicializa TipoOrdenDataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpTipoOrden = New SqlClient.SqlDataAdapter
        End Sub

#End Region


#Region "Implementaciones .Net Framework"

        Private Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Private Overloads Function Update(ByVal dataset As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

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


#End Region

#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As TipoOrdenDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpTipoOrden.SelectCommand = CrearSelectCommand()

                m_adpTipoOrden.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpTipoOrden.Fill(dataSet.SCGTA_TB_TipoOrden)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Fill(ByRef datareader As SqlClient.SqlDataReader) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpTipoOrden.SelectCommand = CrearSelectCommand()

                m_adpTipoOrden.SelectCommand.Connection = m_cnnSCGTaller

                datareader = m_adpTipoOrden.SelectCommand.ExecuteReader()

            Catch ex As Exception

                Throw ex

            Finally
                'Agregado
                'Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As TipoOrdenDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpTipoOrden.InsertCommand = CreateInsertCommand()
                m_adpTipoOrden.InsertCommand.Connection = m_cnnSCGTaller

                m_adpTipoOrden.UpdateCommand = CrearUpdateCommand()
                m_adpTipoOrden.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpTipoOrden.Update(dataSet.SCGTA_TB_TipoOrden)

            Catch ex As Exception

                Throw ex

            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Function Delete(ByVal dataset As TipoOrdenDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpTipoOrden.UpdateCommand = CrearDeleteCommand()
                m_adpTipoOrden.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpTipoOrden.Update(dataset.SCGTA_TB_TipoOrden)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELTipoOrden)

                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdTipoOrden)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strCodTipoOrden, SqlDbType.VarChar, 9, mc_strCodTipoOrden)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                    .Add(mc_strArroba & mc_strCodCentroCosto, SqlDbType.Int, 4, mc_strCodCentroCosto)


                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelTipoOrden)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strCodTipoOrden, SqlDbType.Int, 4, mc_strCodTipoOrden)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSTipoOrden)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)
                    .Add(mc_strArroba & mc_strCodCentroCosto, SqlDbType.Int, 4, mc_strCodCentroCosto)


                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


#End Region


    End Class
End Namespace
