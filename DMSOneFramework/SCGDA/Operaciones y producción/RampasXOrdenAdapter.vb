Namespace SCGDataAccess

    Public Class RampasXOrdenAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strID As String = "ID"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strIDRampa As String = "IDRampa"
        Private Const mc_strFechaHora As String = "FechaHora"
        Private Const mc_strDuracion As String = "Duracion"

        Private m_adpRampasXOrden As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_SelRampas As String = "SCGTA_SP_SelRampas"
        Private Const mc_strSCGTA_SP_SelRampasXOrdenByNoOrden As String = "SCGTA_SP_SelRampasXOrdenByNoOrden"
        Private Const mc_strSCGTA_SP_InsRampasXOrden As String = "SCGTA_SP_InsRampasXOrden"
        Private Const mc_strSCGTA_SP_DelRampasXOrden As String = "SCGTA_SP_DelRampasXOrden"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion

#End Region

#Region "Inicializa RampasXOrdenAdapter"

        Public Sub New()

            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpRampasXOrden = New SqlClient.SqlDataAdapter

        End Sub

#End Region

#Region "Implementaciones .Net Framework"

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

        'Public Overloads Function GetRampasToReader() As SqlClient.SqlDataReader
        '    Dim cmdRampas As SqlClient.SqlCommand
        '    Dim drdRampas As SqlClient.SqlDataReader

        '    Try

        '        If m_cnnSCGTaller.State = ConnectionState.Closed Then
        '            If m_cnnSCGTaller.ConnectionString = "" Then
        '                m_cnnSCGTaller.ConnectionString = strConexionADO
        '            End If
        '            Call m_cnnSCGTaller.Open()
        '        End If

        '        cmdRampas = CrearSelectRampasCommand()

        '        With cmdRampas

        '            .Connection = m_cnnSCGTaller
        '            drdRampas = .ExecuteReader(CommandBehavior.CloseConnection)

        '        End With

        '        Return drdRampas

        '    Catch ex As Exception
        '        Throw ex

        '    Finally
        '        'Agregado
        '        Call m_cnnSCGTaller.Close()
        '    End Try

        'End Function

        Public Overloads Sub CargaRampasXOrdenByNoOrden(ByRef p_dstRampas As RampasXOrdenDataset, ByVal p_strNoOrden As String)
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpRampasXOrden.SelectCommand = CrearSelectRampasXOrdenByNoOrdenCommand()

                With m_adpRampasXOrden.SelectCommand
                    .Connection = m_cnnSCGTaller
                    .Parameters(mc_strArroba & mc_strNoOrden).Value = p_strNoOrden
                End With

                m_adpRampasXOrden.Fill(p_dstRampas.SCGTA_TB_RampasXOrden)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
        End Sub

        Public Overloads Function InsertRampasXOrden(ByRef p_dstRampasXOrden As RampasXOrdenDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpRampasXOrden.InsertCommand = CreateInsertRampaXOrdenCommand()
                m_adpRampasXOrden.InsertCommand.Connection = m_cnnSCGTaller

                Call m_adpRampasXOrden.Update(p_dstRampasXOrden.SCGTA_TB_RampasXOrden)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Function DeleteRampasXOrden(ByRef p_dstRampasXOrden As RampasXOrdenDataset) As Integer
            Dim cmdRampas As SqlClient.SqlCommand

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                cmdRampas = CrearDeleteRampaXOrdenCommand()

                cmdRampas.Connection = m_cnnSCGTaller

                m_adpRampasXOrden.DeleteCommand = cmdRampas

                m_adpRampasXOrden.Update(p_dstRampasXOrden.SCGTA_TB_RampasXOrden)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try


        End Function

#End Region

#Region "Creacion de Comandos del DataAdapter"

        Private Function CrearSelectRampasCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelRampas)
                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearSelectRampasXOrdenByNoOrdenCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelRampasXOrdenByNoOrden)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearDeleteRampaXOrdenCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelRampasXOrden)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strID, SqlDbType.VarChar, 50, mc_strID)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CreateInsertRampaXOrdenCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsRampasXOrden)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)
                    .Add(mc_strArroba & mc_strIDRampa, SqlDbType.Int, 4, mc_strIDRampa)
                    .Add(mc_strArroba & mc_strFechaHora, SqlDbType.DateTime, 8, mc_strFechaHora)
                    .Add(mc_strArroba & mc_strDuracion, SqlDbType.Decimal, 9, mc_strDuracion)

                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try

        End Function



#End Region

    End Class

End Namespace

