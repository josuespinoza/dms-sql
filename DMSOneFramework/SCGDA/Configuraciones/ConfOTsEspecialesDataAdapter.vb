Option Strict On
Option Explicit On

Namespace SCGDataAccess

    Public Class ConfOTsEspecialesDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strID As String = "ID"
        Private Const mc_strIDTipoOrden As String = "IDTipoOrden"
        Private Const mc_strIDAsesor As String = "IDAsesor"
        Private Const mc_strCardCodeCliente As String = "CardCodeCliente"
        Private Const mc_strUsaListaPrecios As String = "UsaListaPrecios"

        Private m_adpConf As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_SelConfOTsEspeciales As String = "SCGTA_SP_SelConfOTsEspeciales"
        Private Const mc_strSCGTA_SP_InsConfOTsEspeciales As String = "SCGTA_SP_InsConfOTsEspeciales"
        Private Const mc_strSCGTA_SP_DelConfOTsEspeciales As String = "SCGTA_SP_DelConfOTsEspeciales"
        Private Const mc_strSCGTA_SP_UpdConfOTsEspeciales As String = "SCGTA_SP_UpdConfOTsEspeciales"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion

#End Region


#Region "Inicializa RazonesCitaDataAdapter"
        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpConf = New SqlClient.SqlDataAdapter
        End Sub

#End Region


#Region "Implementaciones . Net Framework"

        Private Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Private Overloads Function Update(ByVal dataset As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

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


#End Region

#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As ConfOrdenesEspeciales, _
                                       Optional ByVal p_intIdTipoOrden As Integer = -1) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpConf.SelectCommand = CrearSelectCommand()

                If p_intIdTipoOrden <> -1 Then

                    m_adpConf.SelectCommand.Parameters.Item(mc_strIDTipoOrden).Value = p_intIdTipoOrden

                End If

                m_adpConf.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adpConf.Fill(dataSet.SCGTA_TB_ConfOrdenesEspeciales)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Fill(ByRef datareader As SqlClient.SqlDataReader, _
                                       Optional ByVal p_intIdTipoOrden As Integer = -1) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpConf.SelectCommand = CrearSelectCommand()
                m_adpConf.SelectCommand.Connection = m_cnnSCGTaller


                If p_intIdTipoOrden <> -1 Then

                    m_adpConf.SelectCommand.Parameters.Item(mc_strIDTipoOrden).Value = p_intIdTipoOrden

                End If

                datareader = m_adpConf.SelectCommand.ExecuteReader()

            Catch ex As Exception
                Throw ex

            Finally
                'Agregado
                'Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As ConfOrdenesEspeciales, _
                                         ByRef cnConeccion As SqlClient.SqlConnection, _
                                         ByRef tnTransaccion As SqlClient.SqlTransaction) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                    cnConeccion = m_cnnSCGTaller
                    tnTransaccion = cnConeccion.BeginTransaction
                End If


                m_adpConf.InsertCommand = CreateInsertCommand()
                m_adpConf.InsertCommand.Connection = cnConeccion
                m_adpConf.InsertCommand.Transaction = tnTransaccion

                m_adpConf.UpdateCommand = CrearUpdateCommand()
                m_adpConf.UpdateCommand.Connection = cnConeccion
                m_adpConf.UpdateCommand.Transaction = tnTransaccion

                m_adpConf.DeleteCommand = CrearUpdateCommand()
                m_adpConf.DeleteCommand.Connection = cnConeccion
                m_adpConf.DeleteCommand.Transaction = tnTransaccion

                Call m_adpConf.Update(dataSet.SCGTA_TB_ConfOrdenesEspeciales)

            Catch ex As Exception

                Throw ex

            Finally
                'Agregado
                'Call m_cnnSCGTaller.Close()

            End Try

        End Function

#End Region

#Region "Creacion de Comandos del DataAdapter"

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelConfOTsEspeciales)
                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strIDTipoOrden, SqlDbType.Int, 4)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdConfOTsEspeciales)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID)
                    .Add(mc_strArroba & mc_strIDAsesor, SqlDbType.Int, 4, mc_strIDAsesor)
                    .Add(mc_strArroba & mc_strCardCodeCliente, SqlDbType.NVarChar, 20, mc_strCardCodeCliente)
                    .Add(mc_strArroba & mc_strUsaListaPrecios, SqlDbType.Bit, 2, mc_strUsaListaPrecios)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelConfOTsEspeciales)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Function CrearDeleteOTEspecial(ByVal p_ID As Integer) As SqlClient.SqlCommand


            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    Call m_cnnSCGTaller.Open()
                End If

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelConfOTsEspeciales, m_cnnSCGTaller)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters
                    .AddWithValue(mc_strArroba & "ID", p_ID)

                End With

                cmdIns.ExecuteNonQuery()


                Return cmdIns

            Catch ex As Exception
                Throw ex

            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsConfOTsEspeciales)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID).Direction = ParameterDirection.Output
                    .Add(mc_strArroba & mc_strIDAsesor, SqlDbType.Int, 4, mc_strIDAsesor)
                    .Add(mc_strArroba & mc_strIDTipoOrden, SqlDbType.Int, 4, mc_strIDTipoOrden)
                    .Add(mc_strArroba & mc_strCardCodeCliente, SqlDbType.NVarChar, 20, mc_strCardCodeCliente)
                    .Add(mc_strArroba & mc_strUsaListaPrecios, SqlDbType.Bit, 2, mc_strUsaListaPrecios)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try

        End Function



#End Region


    End Class

End Namespace