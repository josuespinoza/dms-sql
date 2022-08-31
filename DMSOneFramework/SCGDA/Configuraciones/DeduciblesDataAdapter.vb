Option Strict On
Option Explicit On 

Namespace SCGDataAccess
    Public Class DeduciblesDataAdapter

        Implements IDataAdapter



#Region "Declaraciones"

        Private Const mc_strCodDeducible As String = "CodDeducible"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"
        Private Const mc_strInfraseguro As String = "ChkInfraseguro"

        Private m_adp As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDDeducibles As String = "SCGTA_SP_UPDDeducibles"
        Private Const mc_strSCGTA_SP_SELDeducible As String = "SCGTA_SP_SELDeducible"
        Private Const mc_strSCGTA_SP_INSDeducibles As String = "SCGTA_SP_INSDeducibles"
        Private Const mc_strSCGTA_SP_DELDeducible As String = "SCGTA_SP_DELDeducible"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Dim objDAConexion As DAConexion

#End Region


#Region "Inicializa DeduciblesDataAdapter"
        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adp = New SqlClient.SqlDataAdapter
        End Sub


#End Region


#Region "Implementaciones"


        Public Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

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

        Public Overloads Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function


#End Region


#Region "Implementaciones SCG"


        Public Overloads Function Fill(ByVal dataSet As DeducibleDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adp.SelectCommand = CrearSelectCommand()
                m_adp.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adp.Fill(dataSet.SCGTA_TB_Deducible)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function


        Public Overloads Function Update(ByVal dataSet As DeducibleDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adp.InsertCommand = CreateInsertCommand()
                m_adp.InsertCommand.Connection = m_cnnSCGTaller

                m_adp.UpdateCommand = CrearUpdateCommand()
                m_adp.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adp.Update(dataSet.SCGTA_TB_Deducible)

            Catch ex As Exception

                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Function Delete(ByVal dataset As DeducibleDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adp.UpdateCommand = CrearDeleteCommand()
                m_adp.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adp.Update(dataset.SCGTA_TB_Deducible)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELDeducible)

                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDDeducibles)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strCodDeducible, SqlDbType.Int, 9, mc_strCodDeducible)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)
                    'Infraseguro se agregò como parámetro del SP. Alejandra 22/05/06
                    .Add(mc_strArroba & mc_strInfraseguro, SqlDbType.Bit, 1, mc_strInfraseguro)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DELDeducible)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strCodDeducible, SqlDbType.Int, 9, mc_strCodDeducible)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSDeducibles)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)
                    'Infraseguro se agregó como parámetro del SP. Alejandra 22/05/06
                    .Add(mc_strArroba & mc_strInfraseguro, SqlDbType.Bit, 1, mc_strInfraseguro)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


#End Region



    End Class
End Namespace
