Imports System.Data.SqlClient
Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion

Namespace SCGDataAccess

    Public Class RendimientoDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

#Region "Constantes"
        Private Const mc_strSCGTA_SP_DuracionXFase As String = "SCGTA_SP_SELDuracionXFase"
        Private Const mc_strSCGTA_SP_Montos As String = "SCGTA_SP_SELMontoOtorgadoVsAcumulado"
        Private Const mc_strSCGTA_SP_SelRendimientoxOrden As String = "SCGTA_SP_RendimientoxOrden"

        Private Const mc_strArroba As String = "@"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strNoExpediente As String = "NoExpediente"
        Private Const mc_strCardCode As String = "CardCode"

#End Region

#Region "Variables"

        Private m_adpRendimiento As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Dim objDAConexion As DAConexion

#End Region

#End Region

#Region "Inicializa DataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpRendimiento = New SqlClient.SqlDataAdapter
        End Sub


#End Region

#Region "Implementaciones"

        Public Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

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

        Public Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function
#End Region

#Region "Implementaciones SCG"

        Public Sub CargarDuracionXFase(ByRef dtsDuracionXFase As DuracionXFaseDataset, ByVal NoOrden As String)
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                With m_adpRendimiento

                    .SelectCommand = CrearSelectCommand(NoOrden)


                    With .SelectCommand

                        .Connection = m_cnnSCGTaller
                        .CommandText = mc_strSCGTA_SP_DuracionXFase

                    End With

                End With

                m_adpRendimiento.Fill(dtsDuracionXFase.SCGTA_SP_SELDuracionXFase)

            Catch ex As Exception
                Throw ex
            Finally
                If Not m_cnnSCGTaller Is Nothing Then
                    m_cnnSCGTaller.Close()
                End If
            End Try
        End Sub

        Public Sub CargarMontos(ByRef dtsRendMontoReparacion As RendMontoReparacionDataset, ByVal NoOrden As String)
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                With m_adpRendimiento

                    .SelectCommand = CrearSelectCommand(NoOrden)


                    With .SelectCommand

                        .Connection = m_cnnSCGTaller
                        .CommandText = mc_strSCGTA_SP_Montos

                    End With

                End With

                m_adpRendimiento.Fill(dtsRendMontoReparacion.SCGTA_SP_SELMontoOtorgadoVsAcumulado)

            Catch ex As Exception
                Throw ex
            Finally
                If Not m_cnnSCGTaller Is Nothing Then
                    m_cnnSCGTaller.Close()
                End If
            End Try
        End Sub

        Public Function Fill(ByVal dataSet As RendimientoxOrdenDataset, _
                             ByVal NoOrden As String, _
                             ByVal CardCode As String, _
                             ByVal Expediente As Integer) As Integer
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                With m_adpRendimiento

                    .SelectCommand = CrearSelectCommandRendimientoxOrden()

                    With .SelectCommand

                        .Connection = m_cnnSCGTaller
                        .Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden
                        .Parameters(mc_strArroba & mc_strNoExpediente).Value = Expediente
                        .Parameters(mc_strArroba & mc_strCardCode).Value = CardCode

                    End With

                End With

                m_adpRendimiento.Fill(dataSet.SCGTA_SP_RendimientoxOrden)

                Return dataSet.SCGTA_SP_RendimientoxOrden.Rows.Count


            Catch ex As DivideByZeroException

                MsgBox(ex.Message)
            Catch ex As Exception

                MsgBox(ex.Message)
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function
#End Region

#Region "Creacion Comandos"

        Private Function CrearSelectCommand(ByVal NoOrden As String) As SqlCommand
            Dim cmdRendimiento As New SqlCommand

            With cmdRendimiento
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = NoOrden

            End With

            Return cmdRendimiento
        End Function

        Private Function CrearSelectCommandRendimientoxOrden() As SqlCommand

            Dim cmdRendimiento As New SqlCommand(mc_strSCGTA_SP_SelRendimientoxOrden)

            With cmdRendimiento

                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
                .Parameters.Add(mc_strArroba & mc_strCardCode, SqlDbType.VarChar, 15)
                .Parameters.Add(mc_strArroba & mc_strNoExpediente, SqlDbType.Int, 4)

            End With

            Return cmdRendimiento
        End Function

#End Region

    End Class
End Namespace
