Imports System.Data
Namespace SCGDataAccess
    Public Class AccesoSBODataAdapter

        Implements IDataAdapter, IDisposable

        Private objDAConexion As DAConexion
        'Private cnnTaller As SqlClient.SqlConnection


#Region "Declaraciones"


        'TODO Agregar nombres de procedimeintos almacenados para una determinada tabla
        Private Const mc_strSPIns As String = ""
        Private Const mc_strSPUpd As String = ""
        Private Const mc_strSPDel As String = ""
        Private Const mc_strSPSel As String = ""
        Private Const mc_strEstaLlaveExiste As String = ""

        'TODO Agregar nombres de columnas de la tabla
        'Private Const mc_str As String = ""
        'Private Const mc_strB As String = ""
        'Private Const mc_strP As String = ""
        'Private Const mc_strC As String = ""

        'Declaracion de objetos de acceso a datos
        Private m_cnnConexion As SqlClient.SqlConnection
        Private m_adpAdapter As SqlClient.SqlDataAdapter

        Private m_strConexion As String

        ' Private mc_strArroba As String = "@"

#End Region

#Region "Inicializar AnalisisDataAdapter"

        Public Sub New()
            Try

                'Dim strCadena As String = ""
                'CrearCadenaDeconexion(strServidorSQL, strNombreBaseDatos, strCadena)

                Dim cnnTaller As New SqlClient.SqlConnection

                objDAConexion = New DAConexion
                cnnTaller = objDAConexion.ObtieneConexion

                m_cnnConexion = cnnTaller 'New SqlClient.SqlConnection(strCadena)
                m_adpAdapter = New SqlClient.SqlDataAdapter

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        'Public Function CrearCadenaDeconexion(ByVal ServidorSQL As String, _
        '                                       ByVal BasedeDatos As String, _
        '                                       ByRef p_strCadenaDeConexion As String) As Boolean

        '    'Dim strConectionString As String

        '    Try
        '        'Verifica si la conexión utiliza autenticación de windows
        '        'Si utiliza Windows Autentication crea el string sin el Usuario y Password
        '        'Si No envia el Usuario y Password de Conexión
        '        If My.Settings.UseTrusted = False Then
        '            p_strCadenaDeConexion = "Data Source=" & ServidorSQL & _
        '                                 ";Initial Catalog =" & BasedeDatos & ";" & _
        '                                 "Connect Timeout=60;" & _
        '                                 "connection reset=false;" & _
        '                                 "connection lifetime=5;" & _
        '                                 "enlist=true;" & _
        '                                 "min pool size=1;" & _
        '                                 "max pool size=100;" & _
        '                                 "Pooling=true;" & _
        '                                 "User ID=" & My.Settings.DBUser & ";" & _
        '                                 "pwd=" & My.Settings.DBPassword & ";" & _
        '                                 "Trusted_Connection=No"
        '        Else
        '            p_strCadenaDeConexion = "Data Source=" & ServidorSQL & _
        '                               ";Initial Catalog =" & BasedeDatos & ";" & _
        '                               "Connect Timeout=60;" & _
        '                               "connection reset=false;" & _
        '                               "connection lifetime=5;" & _
        '                               "enlist=true;" & _
        '                               "min pool size=1;" & _
        '                               "max pool size=100;" & _
        '                               "Pooling=true;" & _
        '                               "Trusted_Connection=Yes"
        '        End If



        '        'If oCompany.WinAuthentication Then
        '        '    strConectionString &= ";Trusted_Connection=Yes"
        '        'Else
        '        '    
        '        'End If
        '        Return True

        '    Catch ex As Exception
        '        Return False
        '    End Try
        'End Function



#End Region

#Region "Implementaciones"

        Public Overloads Function Fill(ByRef dstDataset As System.Data.DataSet, ByVal strConsulta As String) As Integer
            Try
                If m_cnnConexion.State = ConnectionState.Closed Then
                    Call m_cnnConexion.Open()
                End If

                m_adpAdapter.SelectCommand = CrearCmdSel(strConsulta)
                m_adpAdapter.SelectCommand.Connection = m_cnnConexion

                Call m_adpAdapter.Fill(dstDataset)

            Catch ex As Exception
                Return 1
            Finally
                Call m_cnnConexion.Close()
            End Try
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


        Public Function Update(ByVal dstDataSet As System.Data.DataSet) As Integer

            'Dim m_trnEncAnalisisxDetalle As SqlClient.SqlTransaction

            'Try
            '    Call m_cnnConexion.Open()


            '    m_trnEncAnalisisxDetalle = m_cnnConexion.BeginTransaction
            '    m_adpAdapter.UpdateCommand = CrearCmdUpd()
            '    m_adpAdapter.InsertCommand = CrearCmdIns()
            '    m_adpAdapter.UpdateCommand.Connection = m_cnnConexion
            '    m_adpAdapter.InsertCommand.Connection = m_cnnConexion
            '    m_adpAdapter.UpdateCommand.Transaction = m_trnEncAnalisisxDetalle
            '    m_adpAdapter.InsertCommand.Transaction = m_trnEncAnalisisxDetalle

            '    Call m_adpAdapter.Update(dstDataSet.SCGPL_TIPOS_MARCAS)

            '    Return dstDataSet.SCGPL_TIPOS_MARCAS.Rows.Count


            '    'Catch ex As SqlClient.SqlException
            '    '    If ex.Errors(0).Number = mc_strEstaLlaveExiste Then
            '    '        MsgBox("El nùmero de anàlisis : " & dataSet.Enc_Analisis(0).num_Analisis & " ya ha sido registrado", MsgBoxStyle.Information, "<SCG> Análisis")
            '    '    End If

            'Catch ex As Exception
            '    MsgBox(ex.Message)

            '    If Not m_trnEncAnalisisxDetalle Is Nothing Then
            '        Call m_trnEncAnalisisxDetalle.Rollback()
            '    End If

            'Finally
            '    If Not m_trnEncAnalisisxDetalle Is Nothing Then
            '        Call m_trnEncAnalisisxDetalle.Commit()
            '        Call m_trnEncAnalisisxDetalle.Dispose()
            '        m_trnEncAnalisisxDetalle = Nothing
            '    End If
            '    Call m_cnnConexion.Close()
            'End Try
        End Function


        Public Sub Dispose() Implements System.IDisposable.Dispose
            Try
                If m_cnnConexion.State = ConnectionState.Open Then
                    Call m_cnnConexion.Close()
                    Call m_cnnConexion.Dispose()
                    m_cnnConexion = Nothing
                End If

                If Not m_adpAdapter Is Nothing Then
                    Call m_adpAdapter.Dispose()
                    m_adpAdapter = Nothing
                End If
            Catch ex As Exception

            End Try
        End Sub
#End Region

#Region "Commands "
'        Private Function CrearCmdIns() As SqlClient.SqlCommand
'
'            Dim cmdIns As SqlClient.SqlCommand
'
'            Try
'
'                cmdIns = New SqlClient.SqlCommand(mc_strSPIns)
'                cmdIns.CommandType = CommandType.StoredProcedure
'
'                With cmdIns.Parameters
'
'                    'TODO agregar campos para el comando de insercion
'
'                End With
'
'                Return cmdIns
'            Catch ex As Exception
'            Finally
'            End Try
'
'        End Function

'        Private Function CrearCmdDel() As SqlClient.SqlCommand
'
'            Dim cmdDel As SqlClient.SqlCommand
'            Dim param As SqlClient.SqlParameter
'
'            Try
'
'                cmdDel = New SqlClient.SqlCommand(mc_strSPDel)
'                cmdDel.CommandType = CommandType.StoredProcedure
'
'                With cmdDel.Parameters
'
'
'                    'TODO agregar campos para el comando de borrado
'
'
'                End With
'
'                Return cmdDel
'            Catch ex As Exception
'
'            End Try
'
'        End Function

'        Private Function CrearCmdUpd() As SqlClient.SqlCommand
'
'            Dim cmdUpd As SqlClient.SqlCommand
'            Dim param As SqlClient.SqlParameter
'
'            Try
'
'                cmdUpd = New SqlClient.SqlCommand(mc_strSPUpd)
'                cmdUpd.CommandType = CommandType.StoredProcedure
'
'                With cmdUpd.Parameters
'
'
'                    'TODO agregar campos para el comando de actualizacion
'
'
'                End With
'
'                Return cmdUpd
'            Catch ex As Exception
'            Finally
'            End Try
'
'        End Function

        Private Function CrearCmdSel(ByVal strConsulta As String) As SqlClient.SqlCommand

            Dim cmdSel As SqlClient.SqlCommand
'            Dim param As SqlClient.SqlParameter

            cmdSel = New SqlClient.SqlCommand
            cmdSel.CommandType = CommandType.Text

            With cmdSel
                .CommandText = strConsulta
            End With

            Return cmdSel

        End Function

        Public Function Fill1(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Function Update1(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

#End Region
    End Class
End Namespace
