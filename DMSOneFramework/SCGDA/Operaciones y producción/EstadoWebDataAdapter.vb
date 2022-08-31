Imports System.Data.SqlClient

Namespace SCGDataAccess
    Public Class EstadoWebDataAdapter
        Implements IDataAdapter, IDisposable

#Region "Declaraciones"

        'TODO Agregar nombres de procedimeintos almacenados para una determinada tabla
        Private Const mc_strSPSCGTA_SP_SelEstadoWeb As String = "SCGTA_SP_SelEstadoWeb"


        'Declaracion de objetos de acceso a datos
        Private m_cnn As SqlClient.SqlConnection
        Private m_adp As SqlClient.SqlDataAdapter

        Private m_strConexion As String

        Private mc_strArroba As String = "@"
        Private Shared objDAConexion As New DAConexion

        Private m_cnnSCGTaller As SqlClient.SqlConnection

#End Region


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



#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region


#Region "Contructor"
        Public Sub New()
            Try
                objDAConexion = New DAConexion
                m_cnnSCGTaller = objDAConexion.ObtieneConexion
                m_adp = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub



#End Region

#Region "Implementaciones"
        Public Overloads Function FillEstadoWeb(ByRef dataset As EstadoWebDataset) As Integer
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adp.SelectCommand = SelEstadoWeb()
                m_adp.SelectCommand.Connection = m_cnnSCGTaller



                Call m_adp.Fill(dataset.SCGTA_TB_EstadoWeb)

                Return dataset.SCGTA_TB_EstadoWeb.Rows.Count

            Catch ex As Exception
                MsgBox(ex.Message)
                Return -1
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
        End Function

        'Public Overloads Function UpdateEstadoWeb(ByVal dataSet As AgenciasDataset) As Integer

        '    Try
        '        If m_cnnSCGTaller.State = ConnectionState.Closed Then
        '            If m_cnnSCGTaller.ConnectionString = "" Then
        '                m_cnnSCGTaller.ConnectionString = strConexionADO
        '            End If
        '            m_cnnSCGTaller.Open()
        '        End If

        '        m_adpAgencias.InsertCommand = CreateInsertCommand()
        '        m_adpAgencias.InsertCommand.Connection = m_cnnSCGTaller

        '        m_adpAgencias.UpdateCommand = CrearUpdateCommand()
        '        m_adpAgencias.UpdateCommand.Connection = m_cnnSCGTaller

        '        Call m_adpAgencias.Update(dataSet.SCGTA_TB_Agencias)

        '    Catch ex As Exception

        '        Throw ex

        '    Finally

        '        Call m_cnnSCGTaller.Close()

        '    End Try

        'End Function

#End Region
#Region "Commands "

        Private Function SelEstadoWeb() As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand
            Dim param As SqlClient.SqlParameter

            Try
                cmdSel = New SqlClient.SqlCommand(mc_strSPSCGTA_SP_SelEstadoWeb)
                cmdSel.CommandType = CommandType.StoredProcedure



                Return cmdSel
            Catch ex As Exception

                MsgBox(ex.Message)
            End Try

        End Function

        'Private Function CrearUpdateCommand() As SqlClient.SqlCommand

        '    Try
        '        Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDAgencias)
        '        cmdIns.CommandType = CommandType.StoredProcedure

        '        With cmdIns.Parameters

        '            .Add(mc_strArroba & mc_strCodAgencia, SqlDbType.Int, 4, mc_strCodAgencia)
        '            .Add(mc_strArroba & mc_strDescripcion, SqlDbType.VarChar, 100, mc_strDescripcion)

        '        End With

        '        Return cmdIns
        '    Catch ex As Exception
        '        Throw ex
        '    End Try

        'End Function
#End Region
    End Class
End Namespace


