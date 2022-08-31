Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports System.Data.SqlClient

Namespace SCGDataAccess
    Public Class RampasDataAdapter
        Implements IDataAdapter
#Region "Declaraciones"


        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_strIDRampa As String = "IDRampa"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"
        Private Const mc_strEstado As String = "Estado"
        Private Const mc_strFecha As String = "Fecha"
        Private Const mc_strHoraInicioProd As String = "HoraInicioProd"
        Private Const mc_strHoraFinProd As String = "HoraFinProd"
        Private Const mc_strRango As String = "Rango"
        Private Const mc_strEtiqueta As String = "Etiqueta"



        'Declaracion de las constantes con el nombre de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_SELRampas As String = "SCGTA_SP_SelRampas"
        Private Const mc_strSCGTA_SP_SelRampasXFecha As String = "SCGTA_SP_SelRampasXFecha"
        Private Const mc_strSCGTA_SP_SelOcupacionRampas As String = "SCGTA_SP_SelOcupacionRampas"
        Private Const mc_strSCGTA_SP_INSRampas As String = "SCGTA_SP_InsRampas"
        Private Const mc_strSCGTA_SP_UpdRampas As String = "SCGTA_SP_UpdRampas"


        Private m_adpRampas As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Dim objDAConexion As DAConexion


#End Region

#Region "Inicializa Configuracion"

        Public Sub New()

            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion
            m_adpRampas = New SqlClient.SqlDataAdapter

        End Sub

        Public Sub New(ByVal conexion As String)
            Try

                m_cnnSCGTaller = New SqlClient.SqlConnection(conexion)
                m_adpRampas = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub


#End Region

#Region "Implementaciones"

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


        Public Overloads Function Update(ByVal dataSet As RampasDataSet) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpRampas.InsertCommand = CreateInsertCommand()
                m_adpRampas.InsertCommand.Connection = m_cnnSCGTaller

                m_adpRampas.UpdateCommand = CrearUpdateCommand()
                m_adpRampas.UpdateCommand.Connection = m_cnnSCGTaller

                'm_adpRampas.DeleteCommand = CrearDeleteCommand()
                'm_adpRampas.DeleteCommand.Connection = m_cnnSCGTaller

                Call m_adpRampas.Update(dataSet.SCGTA_TB_Rampas)

            Catch ex As Exception

                MsgBox(ex.Message)
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As RampasDataSet) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpRampas.SelectCommand = CrearSelectCommand()

                m_adpRampas.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpRampas.Fill(dataSet.SCGTA_TB_Rampas)

            Catch ex As Exception

                MsgBox(ex.Message)

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Fill(ByVal datFecha As Date) As SqlClient.SqlDataReader

            Try

                Dim drdRampas As SqlClient.SqlDataReader
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpRampas.SelectCommand = CrearSelectCommandXFecha()

                m_adpRampas.SelectCommand.Connection = m_cnnSCGTaller

                m_adpRampas.SelectCommand.Parameters(mc_strArroba & mc_strFecha).Value = datFecha

                ' m_adpRampas.SelectCommand.Parameters(mc_strArroba & mc_strEstado).Value = DBNull.Value

                drdRampas = m_adpRampas.SelectCommand.ExecuteReader

                Return drdRampas

            Catch ex As Exception

                MsgBox(ex.Message)

            Finally
                

            End Try
            Return Nothing
        End Function

        Public Function GetRampasOcupadas(ByVal p_datHoraIni As Date, _
                                            ByVal p_datHoraFin As Date, _
                                            ByVal p_intRango As Integer) As SqlDataReader

            Try

                Dim drdRampas As SqlClient.SqlDataReader
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpRampas.SelectCommand = CrearSelectCommandOcupacion()

                m_adpRampas.SelectCommand.Connection = m_cnnSCGTaller

                m_adpRampas.SelectCommand.Parameters(mc_strArroba & mc_strHoraInicioProd).Value = p_datHoraIni

                m_adpRampas.SelectCommand.Parameters(mc_strArroba & mc_strHoraFinProd).Value = p_datHoraFin

                m_adpRampas.SelectCommand.Parameters(mc_strArroba & mc_strRango).Value = p_intRango

                ' m_adpRampas.SelectCommand.Parameters(mc_strArroba & mc_strEstado).Value = DBNull.Value

                drdRampas = m_adpRampas.SelectCommand.ExecuteReader

                Return drdRampas

            Catch ex As Exception

                MsgBox(ex.Message)

            Finally
                'Agregado
                'Call m_cnnSCGTaller.Close()

            End Try
            Return Nothing
        End Function

#End Region

#Region "Creación de comandos"



        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdRampas)


                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strIDRampa, SqlDbType.Int, 4, mc_strIDRampa)



                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.NVarChar, 100, mc_strDescripcion)



                    .Add(mc_strArroba & mc_strEstadoLogico, SqlDbType.Bit, 1, mc_strEstadoLogico)

                End With

                Return cmdIns

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            Return Nothing
        End Function


        'Private Function CrearDeleteCommand() As SqlClient.SqlCommand

        '    Try

        '        Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelConfiguracion)

        '        cmdIns.CommandType = CommandType.StoredProcedure

        '        With cmdIns.Parameters

        '            .Add(mc_strArroba & mc_strPropiedad, SqlDbType.NVarChar, 50, mc_strPropiedad)
        '        End With

        '        Return cmdIns

        '    Catch ex As Exception
        '        MsgBox(ex.Message)
        '    End Try

        'End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            'Try

            Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSRampas)

            cmdIns.CommandType = CommandType.StoredProcedure

            With cmdIns.Parameters

                '.Add(mc_strArroba & mc_strIDRampa, SqlDbType.Int, 4, mc_strIDRampa)

                .Add(mc_strArroba & mc_strDescripcion, SqlDbType.NVarChar, 100, mc_strDescripcion)

                .Add(mc_strArroba & mc_strEstadoLogico, SqlDbType.Bit, 1, mc_strEstadoLogico)

            End With

            Return cmdIns

            'Catch ex As Exception
            '    MsgBox(ex.Message)
            'End Try

        End Function

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            'Try

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELRampas)

            cmdSel.CommandType = CommandType.StoredProcedure

            'With cmdSel.Parameters

            '    'Parametros o criterios de búsqueda 


            'End With

            Return cmdSel

            'Catch ex As Exception
            '    MsgBox(ex.Message)
            'End Try


        End Function

        Private Function CrearSelectCommandXFecha() As SqlClient.SqlCommand

            'Try

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelRampasXFecha)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                .Add(mc_strArroba & mc_strFecha, SqlDbType.DateTime, 8, mc_strFecha)
                '.Add(mc_strArroba & mc_strEstado, SqlDbType.SmallInt, 2, mc_strEstado)


            End With

            Return cmdSel

            'Catch ex As Exception
            '    MsgBox(ex.Message)
            'End Try


        End Function

        Private Function CrearSelectCommandOcupacion() As SqlClient.SqlCommand

            'Try

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelOcupacionRampas)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                .Add(mc_strArroba & mc_strHoraInicioProd, SqlDbType.DateTime, 8, mc_strHoraInicioProd)
                .Add(mc_strArroba & mc_strHoraFinProd, SqlDbType.DateTime, 8, mc_strHoraFinProd)
                .Add(mc_strArroba & mc_strRango, SqlDbType.Int, 4, mc_strRango)
                '.Add(mc_strArroba & mc_strEstado, SqlDbType.SmallInt, 2, mc_strEstado)


            End With

            Return cmdSel

            'Catch ex As Exception
            '    MsgBox(ex.Message)
            'End Try


        End Function

#End Region

    End Class
End Namespace
