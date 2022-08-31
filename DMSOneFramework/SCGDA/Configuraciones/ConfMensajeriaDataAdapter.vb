Option Strict On
Option Explicit On

Namespace SCGDataAccess

    Public Class ConfMensajeriaDataAdapter
        Implements IDataAdapter


#Region "Declaraciones"
        Private Const mc_strIdConfMensajeria As String = "IdConfMensajeria"
        Private Const mc_strCodCentroCosto As String = "CodCentroCosto"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEncargadoAccesorio As String = "EncargadoAccesorio"
        Private Const mc_strEncargadoRepuesto As String = "EncargadoRepuesto"
        Private Const mc_strEncargadoSuministro As String = "EncargadoSuministro"
        Private Const mc_strEncargadoServicio As String = "EncargadoServicio"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"
        Private Const mc_strTipoEncargado As String = "TipoEncargado"

        Private m_adpConfMensajeria As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDConfMensajeria As String = "SCGTA_SP_UPDConfMensajeria"
        Private Const mc_strSCGTA_SP_SELConfMensajeria As String = "SCGTA_SP_SELConfMensajeria"
        Private Const mc_strSCGTA_SP_INSConfMensajeria As String = "SCGTA_SP_INSConfMensajeria"
        Private Const mc_strSCGTA_SP_DELConfMensajeria As String = "SCGTA_SP_DELConfMensajeria"
        Private Const mc_strSCGTA_SP_SELConfMensajeriaXCodCentroCosto As String = "SCGTA_SP_SELConfMensajeriaXCodCentroCosto"
        Private Const mc_strSCGTA_SP_SELConfMensajeriaXCodCentroCostoXEncargado As String = "SCGTA_SP_SELConfMensajeriaXCodCentroCostoXEncargado"


        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Private objDAConexion As DAConexion
#End Region


#Region "Inicializa AgenciasDataAdapter"

        Public Sub New()
            Call InicializaConfMensajeriaDataAdapter(m_cnnSCGTaller)
        End Sub

        Private Sub InicializaConfMensajeriaDataAdapter(ByRef cnnTaller As SqlClient.SqlConnection)
            Try
                objDAConexion = New DAConexion
                cnnTaller = objDAConexion.ObtieneConexion
                m_adpConfMensajeria = New SqlClient.SqlDataAdapter

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally

            End Try
        End Sub

#End Region

#Region "Implementaciones .Net Framework"
        Public Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Function FillSchema(ByVal dataSet As System.Data.DataSet, ByVal schemaType As System.Data.SchemaType) As System.Data.DataTable() Implements System.Data.IDataAdapter.FillSchema

        End Function

        Public Function GetFillParameters() As System.Data.IDataParameter() Implements System.Data.IDataAdapter.GetFillParameters

        End Function

        Public Property MissingMappingAction As System.Data.MissingMappingAction Implements System.Data.IDataAdapter.MissingMappingAction
            Get

            End Get
            Set(ByVal value As System.Data.MissingMappingAction)

            End Set
        End Property

        Public Property MissingSchemaAction As System.Data.MissingSchemaAction Implements System.Data.IDataAdapter.MissingSchemaAction
            Get

            End Get
            Set(ByVal value As System.Data.MissingSchemaAction)

            End Set
        End Property

        Public ReadOnly Property TableMappings As System.Data.ITableMappingCollection Implements System.Data.IDataAdapter.TableMappings
            Get

            End Get
        End Property

        Public Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function
#End Region


#Region "Implementaciones SCG"

        Public Overloads Function FillConfMensajeria(ByVal dataSet As ConfMensajeriaDataSet) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpConfMensajeria.SelectCommand = CrearSelectCommand()
                m_adpConfMensajeria.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adpConfMensajeria.Fill(dataSet.SCGTA_TB_ConfiguracionMensajeria)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function
        Public Overloads Function FillXCodCentroCosto(ByVal dataSet As ConfMensajeriaDataSet, ByRef CodCentroCosto As Integer) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpConfMensajeria.SelectCommand = CrearSelectCommandXCodCentroCosto()
                m_adpConfMensajeria.SelectCommand.Parameters.Item(mc_strArroba & mc_strCodCentroCosto).Value = CodCentroCosto
                m_adpConfMensajeria.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adpConfMensajeria.Fill(dataSet.SCGTA_TB_ConfiguracionMensajeria)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function


        'Public Overloads Function FillXCodCentroCostoXTipoEncargado(ByVal dataSet As ConfMensajeriaDataSet, ByRef CodCentroCosto As Integer) As Integer

        '    Try
        '        If m_cnnSCGTaller.State = ConnectionState.Closed Then
        '            If m_cnnSCGTaller.ConnectionString = "" Then
        '                m_cnnSCGTaller.ConnectionString = strConexionADO
        '            End If
        '            m_cnnSCGTaller.Open()
        '        End If

        '        m_adpConfMensajeria.SelectCommand = CrearSelectCommandXCodCentroCosto()
        '        m_adpConfMensajeria.SelectCommand.Parameters.Item(mc_strArroba & mc_strCodCentroCosto).Value = CodCentroCosto
        '        'm_adpConfMensajeria.SelectCommand.Parameters.Item(mc_strArroba & mc_strTipoEncargado).Value = TipoEncargado
        '        m_adpConfMensajeria.SelectCommand.Connection = m_cnnSCGTaller
        '        Call m_adpConfMensajeria.Fill(dataSet.SCGTA_TB_ConfiguracionMensajeria)

        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        Call m_cnnSCGTaller.Close()
        '    End Try

        'End Function
        Public Overloads Function UpdateConfMensajeria(ByVal dataSet As ConfMensajeriaDataSet) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpConfMensajeria.InsertCommand = CreateInsertCommand()
                m_adpConfMensajeria.InsertCommand.Connection = m_cnnSCGTaller

                m_adpConfMensajeria.UpdateCommand = CrearUpdateCommand()
                m_adpConfMensajeria.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpConfMensajeria.Update(dataSet.SCGTA_TB_ConfiguracionMensajeria)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Function Delete(ByVal dataset As ConfMensajeriaDataSet, ByVal IdConfMensajeria As Integer) As Integer
            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpConfMensajeria.UpdateCommand = CrearDeleteCommand()
                m_adpConfMensajeria.UpdateCommand.Parameters.Item(mc_strArroba & mc_strIdConfMensajeria).Value = IdConfMensajeria
                m_adpConfMensajeria.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpConfMensajeria.Update(dataset.SCGTA_TB_ConfiguracionMensajeria)

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
                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELConfMensajeria)
                cmdSel.CommandType = CommandType.StoredProcedure
                Return cmdSel
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function CrearSelectCommandXCodCentroCosto() As SqlClient.SqlCommand
            Try
                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELConfMensajeriaXCodCentroCosto)
                cmdSel.CommandType = CommandType.StoredProcedure
                With cmdSel.Parameters
                    .Add(mc_strArroba & mc_strCodCentroCosto, SqlDbType.Int, 4, mc_strCodCentroCosto)
                End With
                Return cmdSel
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        'Private Function CrearSelectCommandXCodCentroCostoXTipoEncargado() As SqlClient.SqlCommand
        '    Try
        '        Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELConfMensajeriaXCodCentroCostoXEncargado)
        '        cmdSel.CommandType = CommandType.StoredProcedure
        '        With cmdSel.Parameters
        '            .Add(mc_strArroba & mc_strCodCentroCosto, SqlDbType.Int, 4, mc_strCodCentroCosto)
        '            .Add(mc_strArroba & mc_strTipoEncargado, SqlDbType.VarChar, 50, mc_strTipoEncargado)
        '        End With
        '        Return cmdSel
        '    Catch ex As Exception
        '        Throw ex
        '    End Try
        'End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand
            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDConfMensajeria)
                cmdIns.CommandType = CommandType.StoredProcedure
                With cmdIns.Parameters
                    .Add(mc_strArroba & mc_strIdConfMensajeria, SqlDbType.Int, 4, mc_strIdConfMensajeria)
                    .Add(mc_strArroba & mc_strCodCentroCosto, SqlDbType.Int, 4, mc_strCodCentroCosto)
                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.NVarChar, 100, mc_strDescripcion)
                    .Add(mc_strArroba & mc_strEncargadoAccesorio, SqlDbType.NVarChar, 300, mc_strEncargadoAccesorio)
                    .Add(mc_strArroba & mc_strEncargadoRepuesto, SqlDbType.NVarChar, 300, mc_strEncargadoRepuesto)
                    .Add(mc_strArroba & mc_strEncargadoSuministro, SqlDbType.NVarChar, 300, mc_strEncargadoSuministro)
                    .Add(mc_strArroba & mc_strEncargadoServicio, SqlDbType.NVarChar, 300, mc_strEncargadoServicio)
                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function CrearDeleteCommand() As SqlClient.SqlCommand
            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DELConfMensajeria)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters
                    .Add(mc_strArroba & mc_strIdConfMensajeria, SqlDbType.Int, 4, mc_strIdConfMensajeria)
                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try
                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSConfMensajeria)
                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters
                    .Add(mc_strArroba & mc_strCodCentroCosto, SqlDbType.Int, 4, mc_strCodCentroCosto)
                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.NVarChar, 100, mc_strDescripcion)
                    .Add(mc_strArroba & mc_strEncargadoAccesorio, SqlDbType.NVarChar, 300, mc_strEncargadoAccesorio)
                    .Add(mc_strArroba & mc_strEncargadoRepuesto, SqlDbType.NVarChar, 300, mc_strEncargadoRepuesto)
                    .Add(mc_strArroba & mc_strEncargadoSuministro, SqlDbType.NVarChar, 300, mc_strEncargadoSuministro)
                    .Add(mc_strArroba & mc_strEncargadoServicio, SqlDbType.NVarChar, 300, mc_strEncargadoServicio)
                End With

                Return cmdIns
            Catch ex As Exception
                Throw ex
            End Try

        End Function



#End Region

    End Class
End Namespace