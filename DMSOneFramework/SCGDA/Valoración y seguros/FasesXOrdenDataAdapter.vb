Namespace SCGDataAccess
    Public Class FasesXOrdenDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_intNoFase As String = "NoFase"
        Private Const mc_intcostomanoobra As String = "CostoManoObra"

        Private Const mc_intCostoPromedioPanel As String = "CostoPromedioPanel"
        Private Const mc_decDuracionhoras As String = "DuracionHoras"
        Private Const mc_intduracionhorasaprobadas As String = "DuracionHorasAprobadas"
        Private Const mc_intCantidadHoraManoObra As String = "CantidadHoraManoObra"
        Private Const mc_strDescripcion As String = "Descripcion"


        Private m_adpFases As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_SELFasesTiempoAprobado As String = "SCGTA_SP_SELFasesTiempoAprobado"
        Private Const mc_strSCGTA_SP_UPDFases As String = "SCGTA_SP_UPDFasesXOrden"
        Private Const mc_strSCGTA_SP_SELFases As String = "SCGTA_SP_SELFasesXOrden"
        Private Const mc_strSCGTA_SP_INSFases As String = "SCGTA_SP_INSFasesXOrden"
        Private Const mc_strSCGTA_SP_DelFases As String = "SCGTA_SP_DelFasesXOrden"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion
        'Private mc_Conexion As String

#End Region


#Region "Inicializa FasesxOrdenDataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpFases = New SqlClient.SqlDataAdapter
        End Sub


#End Region


#Region "Implementaciones"

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
                Throw New NotImplementedException()
            End Get
        End Property


#End Region


#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As FasesXOrdenDataset, ByVal strnoorden As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpFases.SelectCommand = CrearSelectCommand()

                dataSet.SCGTA_TB_FasesxOrden.CostoManoObraColumn.AllowDBNull = True
                dataSet.SCGTA_TB_FasesxOrden.CostoPromedioPanelColumn.AllowDBNull = True
                dataSet.SCGTA_TB_FasesxOrden.DescripcionColumn.AllowDBNull = True
                dataSet.SCGTA_TB_FasesxOrden.DuracionHorasAprobadasColumn.AllowDBNull = True
                dataSet.SCGTA_TB_FasesxOrden.DuracionHorasColumn.AllowDBNull = True
                dataSet.SCGTA_TB_FasesxOrden.CantidadHoraManoObraColumn.AllowDBNull = True


                If strnoorden = "" Then
                    m_adpFases.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = System.DBNull.Value
                Else
                    m_adpFases.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = strnoorden
                End If
                m_adpFases.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adpFases.Fill(dataSet.SCGTA_TB_FasesxOrden)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As FasesXOrdenDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpFases.InsertCommand = CreateInsertCommand()
                m_adpFases.InsertCommand.Connection = m_cnnSCGTaller

                m_adpFases.UpdateCommand = CrearUpdateCommand()
                m_adpFases.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpFases.Update(dataSet.SCGTA_TB_FasesxOrden)

            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Update2(ByVal dataSet As FasesXOrdenDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpFases.InsertCommand = CreateInsertCommand()
                m_adpFases.InsertCommand.Connection = m_cnnSCGTaller

                m_adpFases.UpdateCommand = CrearUpdateCommand()

                m_adpFases.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpFases.Update(dataSet.SCGTA_TB_FasesxOrden)

            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Function Delete(ByVal dataset As FasesXOrdenDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpFases.UpdateCommand = CrearDeleteCommand()
                m_adpFases.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpFases.Update(dataset.SCGTA_TB_FasesxOrden)

            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()

            End Try


        End Function

        Public Function DeleteFasesxOrden(ByRef dataset As FasesXOrdenDataset) As Integer
            Dim m_trn As SqlClient.SqlTransaction = Nothing
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_trn = m_cnnSCGTaller.BeginTransaction

                m_adpFases.DeleteCommand = CrearDeleteCommand()
                m_adpFases.DeleteCommand.Connection = m_cnnSCGTaller
                m_adpFases.DeleteCommand.Transaction = m_trn

                Call m_adpFases.Update(dataset.SCGTA_TB_FasesxOrden)

                Call m_trn.Commit()
            Catch ex As Exception
                If Not m_trn Is Nothing Then
                    Call m_trn.Rollback()
                End If
            Finally
                Call m_cnnSCGTaller.Close()
                If Not m_trn Is Nothing Then
                    Call m_trn.Dispose()
                    m_trn = Nothing
                End If
            End Try


        End Function

        Public Function GetTiempoTotalAsignado(ByVal p_strNoOrden As String) As Decimal
            Dim cmdFasesXOrden As SqlClient.SqlCommand
            Dim decTiempoAprobado As Decimal

            Try

                cmdFasesXOrden = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELFasesTiempoAprobado, m_cnnSCGTaller)

                With cmdFasesXOrden

                    .CommandType = CommandType.StoredProcedure

                    .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden

                    decTiempoAprobado = CDec(.ExecuteScalar)

                End With

                Return decTiempoAprobado

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

#End Region


#Region "Creación de comandos"


        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELFases)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 


                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDFases)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                    .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 4, mc_intNoFase)

                    .Add(mc_strArroba & mc_intduracionhorasaprobadas, SqlDbType.Decimal, 6, mc_intduracionhorasaprobadas)


                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelFases)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                    .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 4, mc_intNoFase)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSFases)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                    .Add(mc_strArroba & mc_intNoFase, SqlDbType.Int, 4, mc_intNoFase)

                    .Add(mc_strArroba & mc_intduracionhorasaprobadas, SqlDbType.Decimal, 6, mc_intduracionhorasaprobadas)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function



#End Region


    End Class
End Namespace
