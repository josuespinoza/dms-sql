Imports DMSOneFramework.SCGDataAccess.DAConexion
Namespace SCGDataAccess
    Public Class PiezaPrincipalDataAdapter
        Implements IDataAdapter

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

        Private Const mc_intNoPieza As String = "NoPiezaPrincipal"
        Private Const mc_intNoSeccion As String = "NoSeccion"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"

        Private m_adpPiezaPrincipal As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDPieza As String = "SCGTA_SP_UpdPiezaPrincipal"
        Private Const mc_strSCGTA_SP_SELPieza As String = "SCGTA_SP_SELPiezasPrincipales"
        Private Const mc_strSCGTA_SP_SELPieza2 As String = "SCGTA_SP_SELPiezasPrincipales2"
        Private Const mc_strSCGTA_SP_INSPieza As String = "SCGTA_SP_INSPiezaPrincipal"
        Private Const mc_strSCGTA_SP_INSPiezaPrincipal As String = "SCGTA_SP_InsPiezasPrincipales"
        Private Const mc_strSCGTA_SP_SelPiezaPrincipal = "SCGTA_SP_SelPiezasPrincipales1"


        Private Const mc_strSCGTA_SP_DelPieza As String = "SCGTA_SP_DELPiezaPrincipal"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion


#End Region


#Region "Inicializa PiezaPrincipalDataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpPiezaPrincipal = New SqlClient.SqlDataAdapter
        End Sub

#End Region


#Region "Implementaciones SCG"


        Public Overloads Function Fill(ByVal dataSet As PiezaPrincipalDataset, ByVal intFaseProduccion As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                
                m_adpPiezaPrincipal.SelectCommand = CrearSelectCommand()

                m_adpPiezaPrincipal.SelectCommand.Connection = m_cnnSCGTaller

                m_adpPiezaPrincipal.SelectCommand.Parameters(mc_strArroba & mc_intNoSeccion).Value = intFaseProduccion

                Call m_adpPiezaPrincipal.Fill(dataSet.SCGTA_TB_PiezaPrincipal)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function

        Public Overloads Function Fill2(ByVal dataSet As PiezaPrincipalDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If


                m_adpPiezaPrincipal.SelectCommand = CrearSelectCommand2()

                m_adpPiezaPrincipal.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpPiezaPrincipal.Fill(dataSet.SCGTA_TB_PiezaPrincipal)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function

        Public Overloads Function Fill3(ByVal dataSet As PiezaPrincipalDataset, _
                                        ByVal NoSeccion As Integer, _
                                        ByVal NoPiezaPrincipal As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If


                m_adpPiezaPrincipal.SelectCommand = CrearSelectCommand3()

                m_adpPiezaPrincipal.SelectCommand.Parameters(mc_strArroba & mc_intNoSeccion).Value = NoSeccion
                m_adpPiezaPrincipal.SelectCommand.Parameters(mc_strArroba & mc_intNoPieza).Value = NoPiezaPrincipal


                m_adpPiezaPrincipal.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpPiezaPrincipal.Fill(dataSet.SCGTA_TB_PiezaPrincipal)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function



        Public Overloads Function Update(ByVal dataSet As PiezaPrincipalDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpPiezaPrincipal.InsertCommand = CreateInsertCommand()
                m_adpPiezaPrincipal.InsertCommand.Connection = m_cnnSCGTaller

                m_adpPiezaPrincipal.UpdateCommand = CrearUpdateCommand()
                m_adpPiezaPrincipal.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpPiezaPrincipal.Update(dataSet.SCGTA_TB_PiezaPrincipal)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Function Delete(ByVal dataset As PiezaPrincipalDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpPiezaPrincipal.UpdateCommand = CrearDeleteCommand()
                m_adpPiezaPrincipal.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpPiezaPrincipal.Update(dataset.SCGTA_TB_PiezaPrincipal)

            Catch ex As Exception

                Throw

            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Overloads Function Insert(ByVal dataSet As PiezaPrincipalDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpPiezaPrincipal.InsertCommand = CreateInsertCommand1()
                m_adpPiezaPrincipal.InsertCommand.Connection = m_cnnSCGTaller

                Call m_adpPiezaPrincipal.Update(dataSet.SCGTA_TB_PiezaPrincipal)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELPieza)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_intNoSeccion, SqlDbType.Int, 9, mc_intNoSeccion)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Private Function CrearSelectCommand2() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELPieza2)

                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearSelectCommand3() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelPiezaPrincipal)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_intNoSeccion, SqlDbType.Int, 9, mc_intNoSeccion)

                    .Add(mc_strArroba & mc_intNoPieza, SqlDbType.Int, 9, mc_intNoPieza)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDPieza)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoPieza, SqlDbType.Int, 4, mc_intNoPieza)
                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)


                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelPieza)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoPieza, SqlDbType.Int, 4, mc_intNoPieza)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSPieza)

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

        Private Function CreateInsertCommand1() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSPiezaPrincipal)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoSeccion, SqlDbType.Int, 9, mc_intNoSeccion)

                    .Add(mc_strArroba & mc_intNoPieza, SqlDbType.Int, 9, mc_intNoPieza)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

                    .Add(mc_strArroba & mc_strEstadoLogico, SqlDbType.Bit, 1, mc_strEstadoLogico)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


#End Region

    End Class
End Namespace