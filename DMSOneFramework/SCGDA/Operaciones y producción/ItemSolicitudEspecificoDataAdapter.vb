Option Strict On
Option Explicit On

Namespace SCGDataAccess

    Public Class ItemSolicitudEspecificoDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strID As String = "ID"
        Private Const mc_strIDSolicitud As String = "IDSolicitud"
        Private Const mc_strItemCodeGenerico As String = "ItemCodeGenerico"
        Private Const mc_strCantidad As String = "Cantidad"
        Private Const mc_strObservaciones As String = "Observaciones"
        Private Const mc_strItemName As String = "ItemName"
        Private Const mc_strLineNum As String = "LineNum"
        Private Const mc_strPrecioAcordado As String = "PrecioAcordado"
        Private Const mc_strIDEmpleado As String = "IDEmpleado"
        Private Const mc_strNombreEmpleado As String = "NombreEmpleado"
        Private Const mc_strCodEspecifico As String = "CodEspecifico"
        Private Const mc_strNomEspecifico As String = "NomEspecifico"
        Private Const mc_strSinExistencia As String = "SinExistencia"
        Private Const mc_strIngresoPE As String = "IngresoPE"
        Private Const mc_strTransaccionNula As String = "TransaccionNula"
        Private Const mc_strFreeTxt As String = "FreeTxt"



        Private m_adpConfCatalogos As SqlClient.SqlDataAdapter


        Private Const mc_strSCGTA_SP_SelItemSolicitudEspecifico As String = "SCGTA_SP_SelItemSolicitudEspecifico"
        Private Const mc_strSCGTa_SP_InsItemSolicitudEspecifico As String = "SCGTa_SP_InsItemSolicitudEspecifico"
        Private Const mc_strSCGTa_SP_UpdItemSolicitudEspecifico As String = "SCGTa_SP_UpdItemSolicitudEspecifico"

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

        Public Overloads Function Fill(ByVal dataSet As ItemSolicitudEspecificoDataset, Optional ByVal p_intID As Integer = -1) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpConfCatalogos.SelectCommand = CrearSelectCommand()

                If p_intID <> -1 Then
                    m_adpConfCatalogos.SelectCommand.Parameters.Item(mc_strArroba + mc_strIDSolicitud).Value = p_intID
                    'Else
                    'm_adpConfCatalogos.SelectCommand.Parameters.Item(mc_strArroba + mc_strIDSolicitud).Value = DBNull.Value
                End If

                m_adpConfCatalogos.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adpConfCatalogos.Fill(dataSet.SCGTA_SP_SelItemSolicitudEspecifico)

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
        '        'Agregado 02072010
        '        'Call m_cnnSCGTaller.Close()

        '    End Try

        'End Function


        Public Overloads Function UpdateItemEspecifico(ByVal dataSet As ItemSolicitudEspecificoDataset, _
                                                       ByRef p_cnConeccion As SqlClient.SqlConnection, _
                                                       ByRef p_tnTransaccion As SqlClient.SqlTransaction) As Integer
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If
                p_cnConeccion = m_cnnSCGTaller
                p_tnTransaccion = p_cnConeccion.BeginTransaction()

                m_adpConfCatalogos.InsertCommand = CreateInsertCommand()
                m_adpConfCatalogos.InsertCommand.Connection = m_cnnSCGTaller
                m_adpConfCatalogos.InsertCommand.Transaction = p_tnTransaccion

                m_adpConfCatalogos.UpdateCommand = CreateUpdateCommand()
                m_adpConfCatalogos.UpdateCommand.Connection = m_cnnSCGTaller
                m_adpConfCatalogos.UpdateCommand.Transaction = p_tnTransaccion

                Call m_adpConfCatalogos.Update(dataSet.SCGTA_SP_SelItemSolicitudEspecifico)


                p_tnTransaccion.Commit()
                Call m_cnnSCGTaller.Close()

            Catch ex As Exception
                p_tnTransaccion.Rollback()
                Call m_cnnSCGTaller.Close()
                Throw ex
            Finally
                'Agregado 02072010
                'Call m_cnnSCGTaller.Close()


            End Try

        End Function


        Public Overloads Function Update(ByVal dataSet As ItemSolicitudEspecificoDataset, _
                                         ByRef p_cnConeccion As SqlClient.SqlConnection, _
                                         ByRef p_tnTransaccion As SqlClient.SqlTransaction) As Integer

            Try

                m_adpConfCatalogos.InsertCommand = CreateInsertCommand()
                m_adpConfCatalogos.InsertCommand.Connection = p_cnConeccion
                m_adpConfCatalogos.InsertCommand.Transaction = p_tnTransaccion

                m_adpConfCatalogos.UpdateCommand = CreateUpdateCommand()
                m_adpConfCatalogos.UpdateCommand.Connection = p_cnConeccion
                m_adpConfCatalogos.UpdateCommand.Transaction = p_tnTransaccion

                Call m_adpConfCatalogos.Update(dataSet.SCGTA_SP_SelItemSolicitudEspecifico)

            Catch ex As Exception

                Throw ex

            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As ItemSolicitudEspecificoDataset, _
                                         ByRef p_cnConeccion As SqlClient.SqlConnection, _
                                         ByRef p_tnTransaccion As SqlClient.SqlTransaction, _
                                         ByVal p_FinalizarTrans As Boolean) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpConfCatalogos.InsertCommand = CreateInsertCommand()
                m_adpConfCatalogos.InsertCommand.Connection = p_cnConeccion
                m_adpConfCatalogos.InsertCommand.Transaction = p_tnTransaccion

                m_adpConfCatalogos.UpdateCommand = CreateUpdateCommand()
                m_adpConfCatalogos.UpdateCommand.Connection = p_cnConeccion
                m_adpConfCatalogos.UpdateCommand.Transaction = p_tnTransaccion

                Call m_adpConfCatalogos.Update(dataSet.SCGTA_SP_SelItemSolicitudEspecifico)

                If p_FinalizarTrans Then

                    p_tnTransaccion.Commit()
                    Call m_cnnSCGTaller.Close()

                End If

            Catch ex As Exception
                p_tnTransaccion.Rollback()
                Call m_cnnSCGTaller.Close()
                Throw ex

            End Try

        End Function

#End Region

#Region "Creacion de Comandos del DataAdapter"

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelItemSolicitudEspecifico)
                cmdSel.CommandType = CommandType.StoredProcedure
                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strIDSolicitud, SqlDbType.Int, 4, mc_strIDSolicitud)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTa_SP_InsItemSolicitudEspecifico)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strIDSolicitud, SqlDbType.Int, 4, mc_strIDSolicitud)
                    .Add(mc_strArroba & mc_strLineNum, SqlDbType.Int, 4, mc_strLineNum)
                    .Add(mc_strArroba & mc_strIDEmpleado, SqlDbType.Int, 4, mc_strIDEmpleado)
                    .Add(mc_strArroba & mc_strItemCodeGenerico, SqlDbType.VarChar, 20, mc_strItemCodeGenerico)
                    .Add(mc_strArroba & mc_strCantidad, SqlDbType.Decimal, 9, mc_strCantidad)
                    .Add(mc_strArroba & mc_strObservaciones, SqlDbType.NVarChar, 150, mc_strObservaciones)
                    .Add(mc_strArroba & mc_strNombreEmpleado, SqlDbType.NVarChar, 250, mc_strNombreEmpleado)
                    .Add(mc_strArroba & mc_strCodEspecifico, SqlDbType.NVarChar, 20, mc_strCodEspecifico)
                    .Add(mc_strArroba & mc_strNomEspecifico, SqlDbType.NVarChar, 100, mc_strNomEspecifico)
                    .Add(mc_strArroba & mc_strSinExistencia, SqlDbType.Int, 4, mc_strSinExistencia)
                    .Add(mc_strArroba & "Nuevo", SqlDbType.Int, 4, "Nuevo")
                    .Add(mc_strArroba & mc_strIngresoPE, SqlDbType.Int, 4, mc_strIngresoPE)
                    .Add(mc_strArroba & mc_strTransaccionNula, SqlDbType.Int, 4, mc_strTransaccionNula)
                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID).Direction = ParameterDirection.Output

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CreateUpdateCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTa_SP_UpdItemSolicitudEspecifico)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID)
                    .Add(mc_strArroba & mc_strLineNum, SqlDbType.Int, 4, mc_strLineNum)
                    .Add(mc_strArroba & mc_strPrecioAcordado, SqlDbType.Decimal, 18, mc_strPrecioAcordado)
                    .Add(mc_strArroba & mc_strCodEspecifico, SqlDbType.NVarChar, 20, mc_strCodEspecifico)
                    .Add(mc_strArroba & mc_strNomEspecifico, SqlDbType.NVarChar, 100, mc_strNomEspecifico)
                    .Add(mc_strArroba & mc_strSinExistencia, SqlDbType.Int, 4, mc_strSinExistencia)
                    .Add(mc_strArroba & "Nuevo", SqlDbType.Int, 4, "Nuevo")
                    .Add(mc_strArroba & mc_strIngresoPE, SqlDbType.Int, 4, mc_strIngresoPE)
                    .Add(mc_strArroba & mc_strTransaccionNula, SqlDbType.Int, 4, mc_strTransaccionNula)
                    '.Add(mc_strArroba & mc_strFreeTxt, SqlDbType.NVarChar, 150, mc_strFreeTxt)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try

        End Function



#End Region

    End Class

End Namespace