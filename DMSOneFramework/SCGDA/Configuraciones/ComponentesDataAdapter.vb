Namespace SCGDataAccess
    Public Class ComponentesDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strNoRepuesto As String = "NoRepuesto"
        Private Const mc_strNopieza As String = "NoPiezaPrincipal"
        Private Const mc_NoSeccion As String = "NoSeccion"
        Private Const mc_CodiMitchell As String = "CodigoMitchell"
        Private Const mc_strComponente As String = "Componente"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"
        Private Const mc_strCodigoMitchell As String = "CodigoMitchell"
        Private Const mc_strCodEstilo As String = "CodEstilo"
        Private Const mc_strCodMarca As String = "CodMarca"
        Private Const mc_strCodModelo As String = "CodModelo"

        Private m_adpComponente As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDComponente As String = "SCGTA_SP_UpdComponentes"
        Private Const mc_strSCGTA_SP_SELComponente As String = "SCGTA_SP_SELComponentes"
        Private Const mc_strSCGTA_SP_SELComponente2 As String = "SCGTA_SP_SELComponentes2"
        Private Const mc_strSCGTA_SP_SELComponente3 As String = "SCGTA_SP_SELComponentes3"
        Private Const mc_strSCGTA_SP_SELComponente5 As String = "SCGTA_SP_SELComponentes5"
        Private Const mc_strSCGTA_SP_SELComponente6 As String = "SCGTA_SP_SELComponentes6"
        Private Const mc_strSCGTA_SP_INSComponente As String = "SCGTA_SP_INSComponentes"
        Private Const mc_strSCGTA_SP_DelComponente As String = "SCGTA_SP_DELComponentes"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

#End Region

#Region "Inicializa ComponentesDataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpComponente = New SqlClient.SqlDataAdapter
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

#Region "Implementaciones SCG"


        Public Overloads Function Fill2(ByVal dataSet As ComponentesDataset, ByVal intPiezaPrincipal As Integer) As Integer

            Try


                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpComponente.SelectCommand = CrearSelectCommand2()

                m_adpComponente.SelectCommand.Connection = m_cnnSCGTaller

                'm_adpComponente.SelectCommand.Parameters(mc_strArroba & mc_intNopieza).Value = intPiezaPrincipal

                Call m_adpComponente.Fill(dataSet.SCGTA_TB_Repuestos)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function

        Public Overloads Function Fill(ByVal dataSet As ComponentesDataset, ByVal intNoRepuesto As Integer) As Integer

            Try


                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpComponente.SelectCommand = CrearSelectCommand()

                m_adpComponente.SelectCommand.Connection = m_cnnSCGTaller

                m_adpComponente.SelectCommand.Parameters(mc_strArroba & mc_strNopieza).Value = intNoRepuesto

                Call m_adpComponente.Fill(dataSet.SCGTA_TB_Repuestos)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function

        Public Overloads Function Fill(ByVal dataSet As ComponentesDataset, ByVal codMitchell As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpComponente.SelectCommand = CrearSelectCommand3()

                m_adpComponente.SelectCommand.Connection = m_cnnSCGTaller

                m_adpComponente.SelectCommand.Parameters(mc_strArroba & mc_strCodigoMitchell).Value = codMitchell

                Call m_adpComponente.Fill(dataSet.SCGTA_TB_Repuestos)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function

        Public Overloads Function Fill(ByVal dataSet As ComponentesDataset, _
                                       ByVal CodMarca As Integer, _
                                       ByVal CodModelo As Integer, _
                                       ByVal CodEstilo As Integer, _
                                       ByVal NoSeccion As Integer, _
                                       ByVal NoPiezaPrincipal As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpComponente.SelectCommand = CrearSelectCommand5()

                m_adpComponente.SelectCommand.Connection = m_cnnSCGTaller

                m_adpComponente.SelectCommand.Parameters(mc_strArroba & mc_strCodMarca).Value = CodMarca
                m_adpComponente.SelectCommand.Parameters(mc_strArroba & mc_strCodModelo).Value = CodModelo
                m_adpComponente.SelectCommand.Parameters(mc_strArroba & mc_strCodEstilo).Value = CodEstilo
                m_adpComponente.SelectCommand.Parameters(mc_strArroba & mc_NoSeccion).Value = NoSeccion
                m_adpComponente.SelectCommand.Parameters(mc_strArroba & mc_strNopieza).Value = NoPiezaPrincipal


                Call m_adpComponente.Fill(dataSet.SCGTA_TB_Repuestos)

            Catch ex As Exception

                Throw ex
                'MsgBox(ex.Message)

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function

        Public Overloads Function Fill(ByVal dataSet As ComponentesDataset, _
                                       ByVal NoPiezaPrincipal As Integer, _
                                       ByVal PorPieza As Boolean) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpComponente.SelectCommand = CrearSelectCommand6()

                m_adpComponente.SelectCommand.Connection = m_cnnSCGTaller

                m_adpComponente.SelectCommand.Parameters(mc_strArroba & mc_strNopieza).Value = NoPiezaPrincipal

                Call m_adpComponente.Fill(dataSet.SCGTA_TB_Repuestos)

            Catch ex As Exception

                Throw ex
                'MsgBox(ex.Message)

            Finally

                Call m_cnnSCGTaller.Close()

            End Try


        End Function


        Public Overloads Function Update(ByVal dataSet As ComponentesDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpComponente.InsertCommand = CreateInsertCommand()
                m_adpComponente.InsertCommand.Connection = m_cnnSCGTaller

                m_adpComponente.UpdateCommand = CrearUpdateCommand()
                m_adpComponente.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpComponente.Update(dataSet.SCGTA_TB_Repuestos)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Function Delete(ByVal dataset As ComponentesDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpComponente.UpdateCommand = CrearDeleteCommand()
                m_adpComponente.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpComponente.Update(dataset.SCGTA_TB_Repuestos)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELComponente)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNopieza, SqlDbType.Int, 9, mc_strNopieza)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearSelectCommand2() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELComponente2)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    '.Add(mc_strArroba & mc_intNopieza, SqlDbType.Int, 9, mc_intNopieza)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearSelectCommand3() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELComponente3)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strCodigoMitchell, SqlDbType.VarChar, 100, mc_strCodigoMitchell)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearSelectCommand5() As SqlClient.SqlCommand

            Try
                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELComponente5)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNopieza, SqlDbType.Int, 9)

                    .Add(mc_strArroba & mc_NoSeccion, SqlDbType.Int, 9)

                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.Int, 9)

                    .Add(mc_strArroba & mc_strCodModelo, SqlDbType.Int, 9)

                    .Add(mc_strArroba & mc_strCodEstilo, SqlDbType.Int, 9)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function CrearSelectCommand6() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELComponente6)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strNopieza, SqlDbType.Int, 9)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand


            Dim param As SqlClient.SqlParameter

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDComponente)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    param = .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.Int, 9, mc_strNoRepuesto)
                    param.SourceVersion = DataRowVersion.Original

                    param = .Add(mc_strArroba & mc_strNopieza, SqlDbType.Int, 9, mc_strNopieza)
                    param.SourceVersion = DataRowVersion.Original

                    param = .Add(mc_strArroba & mc_NoSeccion, SqlDbType.Int, 9, mc_NoSeccion)
                    param.SourceVersion = DataRowVersion.Original

                    .Add(mc_strArroba & mc_CodiMitchell, SqlDbType.Int, 9, mc_CodiMitchell)

                    .Add(mc_strArroba & mc_strComponente, SqlDbType.VarChar, 100, mc_strComponente)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelComponente)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.Int, 9, mc_strNoRepuesto)

                    .Add(mc_strArroba & mc_strNopieza, SqlDbType.Int, 9, mc_strNopieza)

                    .Add(mc_strArroba & mc_NoSeccion, SqlDbType.Int, 9, mc_NoSeccion)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSComponente)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters


                    .Add(mc_strArroba & mc_strNopieza, SqlDbType.Int, 9, mc_strNopieza)

                    .Add(mc_strArroba & mc_NoSeccion, SqlDbType.Int, 9, mc_NoSeccion)

                    .Add(mc_strArroba & mc_CodiMitchell, SqlDbType.Int, 9, mc_CodiMitchell)

                    .Add(mc_strArroba & mc_strComponente, SqlDbType.VarChar, 100, mc_strComponente)

                    .Add(mc_strArroba & mc_strCodEstilo, SqlDbType.Int, 4, mc_strCodEstilo)

                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.Int, 4, mc_strCodMarca)

                    .Add(mc_strArroba & mc_strCodModelo, SqlDbType.Int, 4, mc_strCodModelo)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


#End Region

    End Class
End Namespace
