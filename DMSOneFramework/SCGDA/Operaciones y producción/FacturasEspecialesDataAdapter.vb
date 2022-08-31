Imports DMSOneFramework.SCGDataAccess.DAConexion

Namespace SCGDataAccess

    Public Class FacturasEspecialesDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strDocEntry As String = "DocEntry"
        Private Const mc_strLineNum As String = "LineNum"
        Private Const mc_strItemCode As String = "ItemCode"
        Private Const mc_strItemName As String = "ItemName"
        Private Const mc_strEmpId As String = "EmpId"
        Private Const mc_strColaborador As String = "Colaborador"
        Private Const mc_strAsignar As String = "Asignar"

        Private m_adpFactEspecial As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_SelFactEspeciales As String = "SCGTA_SP_SelFactEspeciales"
        Private Const mc_strSCGTA_SP_UpdColaFactEspeciales As String = "SCGTA_SP_UpdColaFactEspeciales"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

#End Region

#Region "Constructor"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpFactEspecial = New SqlClient.SqlDataAdapter
        End Sub

        Public Sub New(ByVal strCadenaConexion As String)

            m_cnnSCGTaller = New SqlClient.SqlConnection(strCadenaConexion)

            m_adpFactEspecial = New SqlClient.SqlDataAdapter

        End Sub


#End Region

#Region "Implementaciones"

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
                Throw New NotImplementedException()
            End Get
        End Property

#End Region

#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As FacturasEspecialesDataset, _
                                       ByVal p_intDocEntry As Integer) As Integer

            Try
                m_adpFactEspecial.SelectCommand = CrearSelectCommand()

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpFactEspecial.SelectCommand.Parameters(mc_strArroba & mc_strDocEntry).Value = p_intDocEntry

                m_adpFactEspecial.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpFactEspecial.Fill(dataSet.FacturasEspecialesDataTable)

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As FacturasEspecialesDataset) As Integer

            Try
                m_adpFactEspecial.UpdateCommand = CrearUpdateCommand()

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpFactEspecial.UpdateCommand.Connection = m_cnnSCGTaller

                Return m_adpFactEspecial.Update(dataSet.FacturasEspecialesDataTable)

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

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelFactEspeciales)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strDocEntry, SqlDbType.Int)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdColaFactEspeciales)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    .Add(mc_strArroba & mc_strDocEntry, SqlDbType.Int, 4, mc_strDocEntry)
                    .Add(mc_strArroba & mc_strItemCode, SqlDbType.VarChar, 20, mc_strItemCode)
                    .Add(mc_strArroba & mc_strLineNum, SqlDbType.Int, 4, mc_strLineNum)
                    .Add(mc_strArroba & mc_strEmpId, SqlDbType.Int, 4, mc_strEmpId)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function

#End Region

    End Class

End Namespace