'Imports TallerFramework
'Imports TallerFramework.SCGDataAccess.DAConexion
Namespace SCGDataAccess

    Public Class CalendarioAgendaDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_intNoCita As String = "NoCita"
        Private Const mc_intCardCode As String = "CardCode"
        Private Const mc_intCardName As String = "CardName"
        Private Const mc_dtFecha As String = "Fecha"
        Private Const mc_strIDAgenda As String = "IDAgenda"
        Private Const mc_dtHoraIni As String = "HoraIni"
        Private Const mc_dtHoraFin As String = "HoraFin"

        Private Const mc_strSCGTA_SP_SelClienteFromCita As String = "SCGTA_SP_SelClienteFromCita"
        Private Const mc_strSCGTA_SP_SELNoCitasByFecha As String = "SCGTA_SP_SELNoCitasByFecha"

        Private m_adpClienteFromCita As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Dim objDAConexion As DAConexion

#End Region

#Region "Inicializa ClientesDataAdapter"

        Public Sub New()

            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion
            m_adpClienteFromCita = New SqlClient.SqlDataAdapter

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
                Return Nothing
            End Get
        End Property

#End Region

#Region "Implementaciones SCG"

        Public Sub AbrirConexion()

            If m_cnnSCGTaller.State = ConnectionState.Closed Then
                m_cnnSCGTaller.Open()
            End If

        End Sub

        Public Sub CerrarConexion()

            If m_cnnSCGTaller.State = ConnectionState.Open Then
                m_cnnSCGTaller.Close()
            End If

        End Sub

        Public Sub GetInfoClientesFromCitas(ByVal p_strNoCita As String, ByRef p_strCardCode As String, ByRef p_strCardName As String, ByRef p_dtHoraIni As Date, ByRef p_dtHoraFin As Date)
            Dim cmdSelect As SqlClient.SqlCommand
            Dim drdResult As SqlClient.SqlDataReader

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If

                cmdSelect = CrearSelectCommandClientFromCitas(p_strNoCita)

                cmdSelect.Connection = m_cnnSCGTaller

                drdResult = cmdSelect.ExecuteReader

                If drdResult.Read Then
                    p_strCardCode = drdResult.Item(mc_intCardCode)
                    p_strCardName = drdResult.Item(mc_intCardName)
                    p_dtHoraIni = drdResult.Item(mc_dtHoraIni)
                    p_dtHoraFin = drdResult.Item(mc_dtHoraFin)
                Else
                    p_strCardCode = ""
                    p_strCardName = ""
                    p_dtHoraIni = Nothing
                    p_dtHoraFin = Nothing
                End If

                drdResult.Close()

            Catch ex As Exception

                Throw ex

            Finally

                If Not IsNothing(m_cnnSCGTaller) Then
                    If m_cnnSCGTaller.State = ConnectionState.Open Then
                        m_cnnSCGTaller.Close()
                    End If
                End If

            End Try
        End Sub

        Public Function GetCodsCitas(ByVal p_dtFecha As Date, ByVal p_intIDAgenda As Integer) As String
            Dim cmdSelect As SqlClient.SqlCommand
            Dim drdResult As SqlClient.SqlDataReader
            Dim strNoCita As String = ""

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If

                cmdSelect = CrearSelectCommandCodigosCitas(p_dtFecha, p_intIDAgenda)

                cmdSelect.Connection = m_cnnSCGTaller

                drdResult = cmdSelect.ExecuteReader

                While drdResult.Read
                    strNoCita &= drdResult.Item(mc_intNoCita) & ","
                End While

                drdResult.Close()

                If strNoCita <> "" Then
                    strNoCita = strNoCita.Substring(0, strNoCita.Length - 1)
                End If

                Return strNoCita

            Catch ex As Exception
                Throw ex
            Finally

                If Not IsNothing(m_cnnSCGTaller) Then
                    If m_cnnSCGTaller.State = ConnectionState.Open Then
                        m_cnnSCGTaller.Close()
                    End If
                End If

            End Try
        End Function

#End Region

#Region "Creación de comandos"

        Private Function CrearSelectCommandClientFromCitas(ByVal p_strNoCita As String) As SqlClient.SqlCommand
            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelClienteFromCita)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                .Add(mc_strArroba & mc_intNoCita, SqlDbType.NVarChar, 20).Value = p_strNoCita

            End With

            Return cmdSel

        End Function

        Private Function CrearSelectCommandCodigosCitas(ByVal p_dtFecha As Date, ByVal p_intIDAgenda As Integer) As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELNoCitasByFecha)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                .Add(mc_strArroba & mc_dtFecha, SqlDbType.DateTime).Value = p_dtFecha

                .Add(mc_strArroba & mc_strIDAgenda, SqlDbType.Int).Value = p_intIDAgenda

            End With

            Return cmdSel

        End Function

#End Region

    End Class

End Namespace
