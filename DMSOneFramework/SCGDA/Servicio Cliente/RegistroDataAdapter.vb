Imports DMSOneFramework.SCGDataAccess.DAConexion
Namespace SCGDataAccess

Public Class RegistroDataAdapter
    Implements IDataAdapter

#Region "Declaraciones"
    'Constantes de los nombre de las columnas
    Private Const mc_intNoRegistro As String = "NoRegistro"
    Private Const mc_intNoExpediente As String = "NoExpediente"
    Private Const mc_strpersonacliente As String = "Persona_cliente"
    Private Const mc_strpersonataller As String = "Persona_taller"
    Private Const mc_observacion As String = "Observacion"
    Private Const mc_fecha As String = "Fecha"
    Private Const mc_hora As String = "Hora"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"
        Private Const mc_NombreCliente = "NombreCliente"

        Private Const mc_Codcliente As String = "Codcliente"
        Private Const mc_NoOrden As String = "NoOrden"
        'Private Const mc_strDestinatario As String = "Destinatario"
        'Private Const mc_strDestinatarioID As String = "DestinatarioID"
        Private Const mc_strUsuario As String = "Usuario"
        Private Const mc_strCompania As String = "Compania"
        Private Const mc_strAplicacion As String = "Aplicacion"




        Private m_adpRegistro As SqlClient.SqlDataAdapter

    'Constantes de los nombres de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_UPDRegistro As String = "SCGTA_SP_UPDRegistro"
        Private Const mc_strSCGTA_SP_SELRegistro As String = "SCGTA_SP_SELRegistro"
        Private Const mc_strSCGTA_SP_INSRegistro As String = "SCGTA_SP_INSRegistro"
        Private Const mc_strSCGTA_SP_DELRegistro As String = "SCGTA_SP_DELRegistro"
        Private Const mc_strSCGTA_SP_SELRegistroFiltro As String = "SCGTA_SP_SELRegistroFiltro"
        Private Const mc_strSCGTA_SP_SELNoMensajes As String = "SCGTA_SP_SELNoMensajes"
        Private Const mc_strSCGTA_SP_SELMensajes As String = "SCGTA_SP_SELMensajes"
        Private Const mc_strSCGTA_SP_UPDMensajesLeidos As String = "SCGTA_SP_UPDNoMensajesNuevos"

    Private m_cnnSCGTaller As SqlClient.SqlConnection

    Private Const mc_strArroba As String = "@"
    Private objDAConexion As DAConexion


#End Region

#Region "Inicializa RequisitosDataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpRegistro = New SqlClient.SqlDataAdapter
        End Sub

#End Region

#Region "Implementacion .Net Framework"
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

        'Metodo utilizado para la seleccion de requisitos que se cargan en el dataset
        Public Overloads Function Fill(ByVal dataSet As RegistroDataset) As Integer

            Try
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If
                m_adpRegistro.SelectCommand = CrearSelectCommand()
                m_adpRegistro.SelectCommand.Connection = m_cnnSCGTaller
                Call m_adpRegistro.Fill(dataSet.SCGTA_TB_Registro)

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As RegistroDataset, ByVal NoExpediente As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpRegistro.SelectCommand = CrearSelectCommandFiltro()

                If NoExpediente = -1 Then
                    m_adpRegistro.SelectCommand.Parameters(mc_strArroba & mc_intNoExpediente).Value = System.DBNull.Value
                Else
                    m_adpRegistro.SelectCommand.Parameters(mc_strArroba & mc_intNoExpediente).Value = NoExpediente
                End If

                m_adpRegistro.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpRegistro.Fill(dataSet.SCGTA_TB_Registro)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        'metodo utilizado para la inserción y modificación de datos.
        Public Overloads Function Update(ByVal dataSet As RegistroDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpRegistro.InsertCommand = CreateInsertCommand()
                m_adpRegistro.InsertCommand.Connection = m_cnnSCGTaller

                m_adpRegistro.UpdateCommand = CrearUpdateCommand()
                m_adpRegistro.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpRegistro.Update(dataSet.SCGTA_TB_Registro)

            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        'Metodo utilizado para la eliminación lógica de los requisitos (es un update del estado lógico).
        Public Function Delete(ByVal dataset As RegistroDataset, ByVal codregistro As Integer, ByVal noexpedient As Integer) As Integer
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpRegistro.UpdateCommand = CrearDeleteCommand()
                m_adpRegistro.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpRegistro.Update(dataset.SCGTA_TB_Registro)

            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function
        'Agregado 14/06/06. Alejandra. Se utiliza para determinar si hay mensajes nuevos para un usuario
        Public Function HayMensajesNuevos(ByVal usuario As String, ByVal compania As String, ByVal aplicacion As String) As Boolean

            Try
                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELNoMensajes)
                Dim blnMensajes As Boolean
                Dim intNoMensajes As Integer


                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel
                    .Parameters.Add(mc_strArroba & mc_strUsuario, SqlDbType.VarChar, 15)
                    .Parameters.Add(mc_strArroba & mc_strCompania, SqlDbType.VarChar, 50)
                    .Parameters.Add(mc_strArroba & mc_strAplicacion, SqlDbType.VarChar, 50)
                    .Parameters(mc_strArroba & mc_strUsuario).Value = usuario
                    .Parameters(mc_strArroba & mc_strCompania).Value = compania
                    .Parameters(mc_strArroba & mc_strAplicacion).Value = aplicacion
                End With

                cmdSel.Connection = m_cnnSCGTaller

                intNoMensajes = cmdSel.ExecuteScalar

                If intNoMensajes > 0 Then
                    blnMensajes = True
                Else
                    blnMensajes = False

                End If

                Return blnMensajes

            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function
        'Agregado 14/06/06. Alejandra. Se utiliza para seleccionar los mensajes nuevos para un usuario
        Public Sub SeleccionarRegistros(ByVal dataset As RegistroDataset, ByVal usuario As String _
                                        , ByVal compania As String, ByVal aplicacion As String)
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpRegistro.SelectCommand = CrearSelectCommandRegistros()


                m_adpRegistro.SelectCommand.Parameters(mc_strArroba & mc_strUsuario).Value = usuario
                m_adpRegistro.SelectCommand.Parameters(mc_strArroba & mc_strCompania).Value = compania
                m_adpRegistro.SelectCommand.Parameters(mc_strArroba & mc_strAplicacion).Value = aplicacion

                m_adpRegistro.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpRegistro.Fill(dataset.SCGTA_TB_Registro)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Sub

        'Agregado 16/06/06. Alejandra. Se utiliza para marcar un mensaje como leido por el usuario
        Public Sub MarcarComoLeido(ByVal usuario As String, ByVal compania As String, ByVal aplicacion As String, ByVal noRegistro As Long)

            Try
                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDMensajesLeidos)


                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel
                    .Parameters.Add(mc_strArroba & mc_strUsuario, SqlDbType.VarChar, 15)
                    .Parameters.Add(mc_strArroba & mc_strCompania, SqlDbType.VarChar, 50)
                    .Parameters.Add(mc_strArroba & mc_strAplicacion, SqlDbType.VarChar, 50)
                    .Parameters.Add(mc_strArroba & mc_intNoRegistro, SqlDbType.BigInt, 8)
                    .Parameters(mc_strArroba & mc_strUsuario).Value = usuario
                    .Parameters(mc_strArroba & mc_strCompania).Value = compania
                    .Parameters(mc_strArroba & mc_strAplicacion).Value = aplicacion
                    .Parameters(mc_strArroba & mc_intNoRegistro).Value = noRegistro

                End With

                cmdSel.Connection = m_cnnSCGTaller
                cmdSel.ExecuteNonQuery()


            Catch ex As Exception
                Throw ex

            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Sub

#End Region

#Region "Creación de comandos"


        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELRegistro)

                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDRegistro)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoRegistro, SqlDbType.Int, 9, mc_intNoRegistro)

                    .Add(mc_strArroba & mc_intNoExpediente, SqlDbType.Int, 9, mc_intNoExpediente)

                    .Add(mc_strArroba & mc_strpersonacliente, SqlDbType.VarChar, 100, mc_strpersonacliente)

                    .Add(mc_strArroba & mc_strpersonataller, SqlDbType.VarChar, 100, mc_strpersonataller)

                    .Add(mc_strArroba & mc_observacion, SqlDbType.VarChar, 5000, mc_observacion)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DELRegistro)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoRegistro, SqlDbType.Int, 9, mc_intNoRegistro)

                    .Add(mc_strArroba & mc_intNoExpediente, SqlDbType.Int, 9, mc_intNoExpediente)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSRegistro)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_intNoExpediente, SqlDbType.Int, 9, mc_intNoExpediente)

                    .Add(mc_strArroba & mc_strpersonacliente, SqlDbType.VarChar, 100, mc_strpersonacliente)

                    .Add(mc_strArroba & mc_strpersonataller, SqlDbType.VarChar, 100, mc_strpersonataller)

                    .Add(mc_strArroba & mc_observacion, SqlDbType.VarChar, 500, mc_observacion)

                    .Add(mc_strArroba & mc_fecha, SqlDbType.SmallDateTime, 4, mc_fecha)

                    .Add(mc_strArroba & mc_hora, SqlDbType.VarChar, 12, mc_hora)
                    ''''''''''''''''''''''
                    .Add(mc_strArroba & mc_intNoRegistro, SqlDbType.BigInt, 8, mc_intNoRegistro).Direction = ParameterDirection.Output

                    ''''''''''''''

                    


                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearSelectCommandFiltro() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELRegistroFiltro)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_intNoExpediente, SqlDbType.Int, 9, mc_intNoExpediente)

                    '.Add(mc_strArroba & mc_NoOrden, SqlDbType.VarChar, 20, mc_NoOrden)

                    '.Add(mc_strArroba & mc_NombreCliente, SqlDbType.VarChar, 100, mc_NombreCliente)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearSelectCommandRegistros() As SqlClient.SqlCommand
            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELMensajes)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    .Add(mc_strArroba & mc_strUsuario, SqlDbType.VarChar, 15)
                    .Add(mc_strArroba & mc_strCompania, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strAplicacion, SqlDbType.VarChar, 50)
                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try
        End Function

#End Region


    End Class

End Namespace
