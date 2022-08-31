Option Strict On
Option Explicit On

Namespace SCGDataAccess

    Public Class ConfCatalogoRepXMarcaDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strID As String = "ID"
        Private Const mc_strCodMarca As String = "CodMarca"
        Private Const mc_strServidor As String = "Servidor"
        Private Const mc_strUsuarioServidor As String = "UsuarioServidor"
        Private Const mc_strUsuarioSBO As String = "UsuarioSBO"
        Private Const mc_strPasswordServidor As String = "PasswordServidor"
        Private Const mc_strPasswordSBO As String = "PasswordSBO"
        Private Const mc_strCompañia As String = "Compañia"
        Private Const mc_strBDCompañia As String = "BDCompañia"
        Private Const mc_strCodAlmacen As String = "CodAlmacen"
        Private Const mc_strCodListaPrecio As String = "CodListaPrecio"
        Private Const mc_strNombAlmacen As String = "NombAlmacen"
        Private Const mc_strNombListaPrecios As String = "NombListaPrecios"
        Private Const mc_strDescMarca As String = "DescMarca"
        Private Const mc_strCardCodeProveedor As String = "CardCodeProveedor"
        Private Const mc_strCardNameProveedor As String = "CardNameProveedor"

        Private m_adpConfCatalogos As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_SelConfRepuestoXMarca As String = "SCGTA_SP_SelConfRepuestoXMarca"
        Private Const mc_strSCGTA_SP_InsConfRepuestoXMarca As String = "SCGTA_SP_InsConfRepuestoXMarca"
        Private Const mc_strSCGTA_SP_UpdConfRepuestoXMarca As String = "SCGTA_SP_UpdConfRepuestoXMarca"
        Private Const mc_strSCGTA_SP_DelConfRepuestoXMarca As String = "SCGTA_SP_DelConfRepuestoXMarca"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion

#End Region

#Region "Inicializa RazonesCitaDataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpConfCatalogos = New SqlClient.SqlDataAdapter
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

        Public Overloads Function Fill(ByVal dataSet As ConfCatalogoRepXMarcaDataset, _
                                       Optional ByVal p_intID As Integer = -1, _
                                       Optional ByVal p_strCodMarca As String = "") As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpConfCatalogos.SelectCommand = CrearSelectCommand()

                If p_intID <> -1 Then
                    m_adpConfCatalogos.SelectCommand.Parameters.Item(mc_strArroba + mc_strID).Value = p_intID
                Else
                    m_adpConfCatalogos.SelectCommand.Parameters.Item(mc_strArroba + mc_strID).Value = DBNull.Value
                End If

                If p_strCodMarca <> "" Then
                    m_adpConfCatalogos.SelectCommand.Parameters.Item(mc_strArroba + mc_strCodMarca).Value = p_strCodMarca
                End If

                m_adpConfCatalogos.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adpConfCatalogos.Fill(dataSet.SCGTA_TB_ConfCatalogoRepxMarca)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        'Public Overloads Function Fill(ByRef datareader As SqlClient.SqlDataReader, Optional ByVal p_intID As Integer = -1) As Integer

        '    Try

        '        If m_cnnSCGTaller.State = ConnectionState.Closed Then
        '            If m_cnnSCGTaller.ConnectionString = "" Then
        '                m_cnnSCGTaller.ConnectionString = strConexionADO
        '            End If
        '            Call m_cnnSCGTaller.Open()
        '        End If

        '        m_adpConfCatalogos.SelectCommand = CrearSelectCommand()
        '        If p_intID <> -1 Then
        '            m_adpConfCatalogos.SelectCommand.Parameters.Item(mc_strArroba + mc_strID).Value = p_intID
        '        End If
        '        m_adpConfCatalogos.SelectCommand.Connection = m_cnnSCGTaller
        '        datareader = m_adpConfCatalogos.SelectCommand.ExecuteReader()

        '    Catch ex As Exception

        '        Throw ex

        '    Finally

        '        'Agregado
        '        'Call m_cnnSCGTaller.Close()

        '    End Try

        'End Function

        Public Overloads Function Update(ByVal dataSet As ConfCatalogoRepXMarcaDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If


                m_adpConfCatalogos.InsertCommand = CreateInsertCommand()
                m_adpConfCatalogos.InsertCommand.Connection = m_cnnSCGTaller

                m_adpConfCatalogos.UpdateCommand = CrearUpdateCommand()
                m_adpConfCatalogos.UpdateCommand.Connection = m_cnnSCGTaller

                m_adpConfCatalogos.DeleteCommand = CrearDeleteCommand()
                m_adpConfCatalogos.DeleteCommand.Connection = m_cnnSCGTaller

                Call m_adpConfCatalogos.Update(dataSet.SCGTA_TB_ConfCatalogoRepxMarca)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As ConfCatalogoRepXMarcaDataset, _
                                         ByRef Coneccion As SqlClient.SqlConnection, _
                                         ByRef Transaccion As SqlClient.SqlTransaction) As Integer

            Try


                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                Coneccion = m_cnnSCGTaller
                Transaccion = Coneccion.BeginTransaction()

                m_adpConfCatalogos.InsertCommand = CreateInsertCommand()
                m_adpConfCatalogos.InsertCommand.Transaction = Transaccion
                m_adpConfCatalogos.InsertCommand.Connection = Coneccion

                m_adpConfCatalogos.UpdateCommand = CrearUpdateCommand()
                m_adpConfCatalogos.UpdateCommand.Transaction = Transaccion
                m_adpConfCatalogos.UpdateCommand.Connection = Coneccion

                m_adpConfCatalogos.DeleteCommand = CrearDeleteCommand()
                m_adpConfCatalogos.DeleteCommand.Transaction = Transaccion
                m_adpConfCatalogos.DeleteCommand.Connection = Coneccion

                Call m_adpConfCatalogos.Update(dataSet.SCGTA_TB_ConfCatalogoRepxMarca)

            Catch ex As Exception

                Throw ex

            Finally

                'Agregado
                'Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Function Delete(ByVal dataset As ConfCatalogoRepXMarcaDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpConfCatalogos.UpdateCommand = CrearDeleteCommand()
                m_adpConfCatalogos.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpConfCatalogos.Update(dataset.SCGTA_TB_ConfCatalogoRepxMarca)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelConfRepuestoXMarca)
                cmdSel.CommandType = CommandType.StoredProcedure
                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID)
                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.NVarChar, 8, mc_strCodMarca)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdConfRepuestoXMarca)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID)
                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.VarChar, 8, mc_strCodMarca)
                    .Add(mc_strArroba & mc_strServidor, SqlDbType.VarChar, 50, mc_strServidor)
                    .Add(mc_strArroba & mc_strUsuarioServidor, SqlDbType.NVarChar, 50, mc_strUsuarioServidor)
                    .Add(mc_strArroba & mc_strUsuarioSBO, SqlDbType.NVarChar, 50, mc_strUsuarioSBO)
                    .Add(mc_strArroba & mc_strPasswordServidor, SqlDbType.NVarChar, 50, mc_strPasswordServidor)
                    .Add(mc_strArroba & mc_strPasswordSBO, SqlDbType.NVarChar, 50, mc_strPasswordSBO)
                    .Add(mc_strArroba & mc_strCompañia, SqlDbType.NVarChar, 150, mc_strCompañia)
                    .Add(mc_strArroba & mc_strBDCompañia, SqlDbType.NVarChar, 150, mc_strBDCompañia)
                    .Add(mc_strArroba & mc_strCodAlmacen, SqlDbType.NVarChar, 8, mc_strCodAlmacen)
                    .Add(mc_strArroba & mc_strCodListaPrecio, SqlDbType.Int, 4, mc_strCodListaPrecio)
                    .Add(mc_strArroba & mc_strNombAlmacen, SqlDbType.NVarChar, 100, mc_strNombAlmacen)
                    .Add(mc_strArroba & mc_strNombListaPrecios, SqlDbType.NVarChar, 100, mc_strNombListaPrecios)
                    .Add(mc_strArroba & mc_strCardCodeProveedor, SqlDbType.NVarChar, 20, mc_strCardCodeProveedor)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelConfRepuestoXMarca)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsConfRepuestoXMarca)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.VarChar, 8, mc_strCodMarca)
                    .Add(mc_strArroba & mc_strServidor, SqlDbType.VarChar, 50, mc_strServidor)
                    .Add(mc_strArroba & mc_strUsuarioServidor, SqlDbType.NVarChar, 50, mc_strUsuarioServidor)
                    .Add(mc_strArroba & mc_strUsuarioSBO, SqlDbType.NVarChar, 50, mc_strUsuarioSBO)
                    .Add(mc_strArroba & mc_strPasswordServidor, SqlDbType.NVarChar, 50, mc_strPasswordServidor)
                    .Add(mc_strArroba & mc_strPasswordSBO, SqlDbType.NVarChar, 50, mc_strPasswordSBO)
                    .Add(mc_strArroba & mc_strCompañia, SqlDbType.NVarChar, 150, mc_strCompañia)
                    .Add(mc_strArroba & mc_strBDCompañia, SqlDbType.NVarChar, 150, mc_strBDCompañia)
                    .Add(mc_strArroba & mc_strCodAlmacen, SqlDbType.NVarChar, 8, mc_strCodAlmacen)
                    .Add(mc_strArroba & mc_strCodListaPrecio, SqlDbType.Int, 4, mc_strCodListaPrecio)
                    .Add(mc_strArroba & mc_strNombAlmacen, SqlDbType.NVarChar, 100, mc_strNombAlmacen)
                    .Add(mc_strArroba & mc_strNombListaPrecios, SqlDbType.NVarChar, 100, mc_strNombListaPrecios)
                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID).Direction = ParameterDirection.Output
                    .Add(mc_strArroba & mc_strCardCodeProveedor, SqlDbType.NVarChar, 20, mc_strCardCodeProveedor)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try

        End Function



#End Region

    End Class

End Namespace