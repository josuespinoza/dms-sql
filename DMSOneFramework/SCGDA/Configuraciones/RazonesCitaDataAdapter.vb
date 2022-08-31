Option Strict On
Option Explicit On 
Namespace SCGDataAccess

Public Class RazonesCitaDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strNoRazon As String = "NoRazon"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        Private m_adpRazones As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDRazones As String = "SCGTA_SP_UPDRazonesCita"
        Private Const mc_strSCGTA_SP_SELRazones As String = "SCGTA_SP_SELRazonesCita"
        Private Const mc_strSCGTA_SP_INSRazones As String = "SCGTA_SP_INSRazonesCita"
        Private Const mc_strSCGTA_SP_DelRazones As String = "SCGTA_SP_DelRazonesCita"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion

#End Region


#Region "Inicializa RazonesCitaDataAdapter"
        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpRazones = New SqlClient.SqlDataAdapter
        End Sub

#End Region


#Region "Implementaciones . Net Framework"

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
                Return Nothing
            End Get
        End Property


#End Region

#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As RazonesCitaDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpRazones.SelectCommand = CrearSelectCommand()
                m_adpRazones.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adpRazones.Fill(dataSet.SCGTA_TB_RazonesCita)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Fill(ByRef datareader As SqlClient.SqlDataReader, Optional ByVal p_intEstado As Integer = -1) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpRazones.SelectCommand = CrearSelectCommand()
                If p_intEstado <> -1 Then
                    m_adpRazones.SelectCommand.Parameters.Item(mc_strArroba + mc_strEstadoLogico).Value = p_intEstado
                End If
                m_adpRazones.SelectCommand.Connection = m_cnnSCGTaller
                datareader = m_adpRazones.SelectCommand.ExecuteReader()

            Catch ex As Exception
                Throw ex
            Finally
                'Agregado
                'Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As RazonesCitaDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If


                m_adpRazones.InsertCommand = CreateInsertCommand()
                m_adpRazones.InsertCommand.Connection = m_cnnSCGTaller

                m_adpRazones.UpdateCommand = CrearUpdateCommand()
                m_adpRazones.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpRazones.Update(dataSet.SCGTA_TB_RazonesCita)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Function Delete(ByVal dataset As RazonesCitaDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpRazones.UpdateCommand = CrearDeleteCommand()
                m_adpRazones.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpRazones.Update(dataset.SCGTA_TB_RazonesCita)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELRazones)
                cmdSel.CommandType = CommandType.StoredProcedure
                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNoRazon, SqlDbType.Int, 4, mc_strNoRazon)
                    .Add(mc_strArroba & mc_strEstadoLogico, SqlDbType.Bit, 2, mc_strEstadoLogico)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDRazones)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoRazon, SqlDbType.Int, 4, mc_strNoRazon)
                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)
                    .Add(mc_strArroba & mc_strEstadoLogico, SqlDbType.Bit, 2, mc_strEstadoLogico)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelRazones)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoRazon, SqlDbType.Int, 4, mc_strNoRazon)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSRazones)
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