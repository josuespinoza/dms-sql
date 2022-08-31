Imports DMSOneFramework.SCGCommon

Namespace SCGDataAccess
    Public Class MensajeriaSBOTallerDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"


        'Prueba carga documentos SAP
        Private m_oStockTransfer As SAPbobsCOM.StockTransfer

        'Private m_oCompany As SAPbobsCOM.Company
        Private Const mc_strCodCentroCosto As String = "U_SCGD_CodCtroCosto"
        Private m_adpConfMensajeria As ConfMensajeriaDataAdapter
        Private m_dstConfMensajeria As ConfMensajeriaDataSet
        'Private m_dstConfMensajeriaXCentroCosto As New ConfMensajeriaDataSet


        'Declaracion de objetos de acceso a datos
        Private m_cnn As SqlClient.SqlConnection
        Private m_adp As SqlClient.SqlDataAdapter
        Private objDAConexion As New DAConexion

        Private m_strConexion As String

        'Constantes
        Private Const mc_strEncargadoBodega As String = "EncargadoBodega"
        Private Const mc_strEncargadoSuministros As String = "EncargadoSuministros"
        Private Const mc_strEncargadoTaller As String = "EncargadoProduccion"
        Private Const mc_strEncargadoRepuestos As String = "EncargadoRepuestos"
        'Se incluye en mensajeria por centro costo
        Private Const mc_strEncargadoServicios As String = "EncargadoServicio"

        'Parametros de Stored procedures
        Private Const mc_strArroba As String = "@"
        Private Const mc_strDetalle As String = "Detalle"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strNoCotizacion As String = "NoCotizacion"
        Private Const mc_strNoMensaje As String = "NoMensaje"
        Private Const mc_strDestinatario As String = "Destinatario"
        Private Const mc_strUsuario As String = "Usuario"
        Private Const mc_strCompania As String = "Compania"
        Private Const mc_strAplicacion As String = "Aplicacion"
        Private Const mc_strCodSucursal As String = "CodSucursal"
        Private Const mc_strEmpID As String = "EmpID"
        Private Const mc_strDocNum As String = "DocNum"
        Private Const mc_strNoVisita As String = "NoVisita"
        Private Const mc_strTipoMensaje As String = "TipoMensaje"
        Private Const mc_strNoSolicitud As String = "NoSolicitud"


        'Nombre Stored procedures
        Private Const mc_strINSMensaje As String = "SCGTA_SP_INSMensajeSBO_DMS"
        Private Const mc_strINSDestinatario As String = "SCGTA_SP_INSDestinaMensajeSBO_DMS"
        Private Const mc_strSELMensajesNoLeidos As String = "SCGTA_SP_SELNumMensajesNoLeidosXUsuario"
        Private Const mc_strSELMensajes As String = "SCGTA_SP_SELMensajes"
        Private Const mc_strUPDMensajesLeidos As String = "SCGTA_SP_UPDMensajesLeidos"
        Private Const mc_strSELCodigoUsuario As String = "SCGTA_SP_SELCodigoUsuario"
        Private Const mc_strSELIdAsesor As String = "SCGTA_SP_SELIdAsesor"
        Private Const mc_strSELDocEntryCotizacion As String = "SCGTA_SP_SELDocEntryCotizacion"
        Private Const mc_strSELDocEntryOrdenCompra As String = "SCGTA_SP_SELDocEntryOrdenCompra"
        Private Const mc_strSELDocEntryOfertaCompra As String = "SCGTA_SP_SELDocEntryOfertaCompra"

        Private m_objCompany As SAPbobsCOM.Company

        Private m_HashTableUsuarios As New Hashtable

        Public Enum RecibeMensaje
            EncargadoTaller = 0
            Bodeguero = 1
            Asesor = 2
            EncargadoRepuestos = 3
            EncargadoSuministros
        End Enum

        Public Enum TipoMensaje
            scgPeticionRepuestos = 1
            scgPeticionSuministros = 2
            scgDevolucionRepuestos = 3
            scgDevolucionSuministros = 4
        End Enum

#End Region

#Region "Inicializaciones"

        Public Sub New(ByVal conexion As String)
            Try
                m_strConexion = conexion
                m_cnn = New SqlClient.SqlConnection(conexion)
                m_adp = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        Public Sub New()
            Try
                m_cnn = objDAConexion.ObtieneConexion  'New SqlClient.SqlConnection(conexion)
                m_strConexion = m_cnn.ConnectionString
                m_adp = New SqlClient.SqlDataAdapter

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
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
            Set(ByVal value As System.Data.MissingMappingAction)

            End Set
        End Property

        Public Property MissingSchemaAction() As System.Data.MissingSchemaAction Implements System.Data.IDataAdapter.MissingSchemaAction
            Get

            End Get
            Set(ByVal value As System.Data.MissingSchemaAction)

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
        Public Sub InsertarMensajeSBO_DMSXCentroCosto(ByVal p_ocompany As SAPbobsCOM.Company, ByVal p_strMensaje As String, ByVal p_strOT As String, ByVal p_intNoCotizacion As Integer _
                                        , ByVal p_destinatario As RecibeMensaje, ByVal p_intCodEmpleado As Integer _
                                        , ByVal p_strNoVisita As String, Optional ByVal p_intTipoMensaje As Integer = 1, Optional ByVal p_intNoSolicitud As Integer = -1)
            'Inserta el texto del mensaje
            Try
                Dim cmd As New SqlClient.SqlCommand
                Dim intNoMensaje As Integer
                Dim strDestinatario As String
                Dim strDestinatariosMensaje() As String
                Dim intIndice As Integer
                Dim strCentroCosto As String = String.Empty
                Dim blnConfCentroCosto As Boolean = False


                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = mc_strINSMensaje
                cmd.Connection = m_cnn

                With cmd.Parameters
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strOT
                    .Add(mc_strArroba & mc_strNoCotizacion, SqlDbType.Int, 4).Value = p_intNoCotizacion
                    .Add(mc_strArroba & mc_strDetalle, SqlDbType.VarChar, 5000).Value = p_strMensaje
                    .Add(mc_strArroba & mc_strNoVisita, SqlDbType.VarChar, 10).Value = p_strNoVisita
                    .Add(mc_strArroba & mc_strTipoMensaje, SqlDbType.VarChar, 10).Value = p_intTipoMensaje
                    .Add(mc_strArroba & mc_strNoSolicitud, SqlDbType.VarChar, 10).Value = p_intNoSolicitud
                End With

                intNoMensaje = cmd.ExecuteScalar

                strCentroCosto = RecorreLineasCotizacion(p_intNoCotizacion, strCentroCosto, p_ocompany)

                If strCentroCosto <> "" Then
                    'strArregloCentroCosto = strCentroCosto.Split(",")
                    blnConfCentroCosto = True
                Else
                    blnConfCentroCosto = False
                End If


                If blnConfCentroCosto = True Then
                    If intNoMensaje > 0 Then
                        'Obtiene el codigo de la persona a quien va dirigido el mensaje
                        'strDestinatario = ObtieneCodigoDestinatario(p_destinatario, p_intCodEmpleado)
                        strDestinatario = ConsultarMensajeriaXCentroCosto(strCentroCosto, mc_strEncargadoServicios)


                        If strDestinatario <> "" Then
                            strDestinatariosMensaje = Split(strDestinatario, ",")

                            Dim intIndicearreglo As Integer
                            For intIndicearreglo = 0 To strDestinatariosMensaje.Length - 1
                                strDestinatariosMensaje(intIndicearreglo) = Trim(strDestinatariosMensaje(intIndicearreglo))
                            Next


                            'Inserta el destinatario a quien va dirigido el mensaje

                            For intIndice = 0 To strDestinatariosMensaje.Length - 1
                                'InsertarDestinatarioMensajeSBO_DMS(intNoMensaje, strDestinatario)
                                InsertarDestinatarioMensajeSBO_DMS(intNoMensaje, Trim(strDestinatariosMensaje(intIndice)))
                            Next
                        End If

                    End If
                End If





            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try
        End Sub


        Public Sub InsertarMensajeSBO_DMS(ByVal p_strMensaje As String, ByVal p_strOT As String, ByVal p_intNoCotizacion As Integer _
                                        , ByVal p_destinatario As RecibeMensaje, ByVal p_intCodEmpleado As Integer _
                                        , ByVal p_strNoVisita As String, Optional ByVal p_intTipoMensaje As Integer = 1, Optional ByVal p_intNoSolicitud As Integer = -1)
            'Inserta el texto del mensaje
            Try
                Dim cmd As New SqlClient.SqlCommand
                Dim intNoMensaje As Integer
                Dim strDestinatario As String
                Dim strDestinatariosMensaje() As String
                Dim intIndice As Integer

                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = mc_strINSMensaje
                cmd.Connection = m_cnn

                With cmd.Parameters
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strOT
                    .Add(mc_strArroba & mc_strNoCotizacion, SqlDbType.Int, 4).Value = p_intNoCotizacion
                    .Add(mc_strArroba & mc_strDetalle, SqlDbType.VarChar, 5000).Value = p_strMensaje
                    .Add(mc_strArroba & mc_strNoVisita, SqlDbType.VarChar, 10).Value = p_strNoVisita
                    .Add(mc_strArroba & mc_strTipoMensaje, SqlDbType.VarChar, 10).Value = p_intTipoMensaje
                    .Add(mc_strArroba & mc_strNoSolicitud, SqlDbType.VarChar, 10).Value = p_intNoSolicitud
                End With

                intNoMensaje = cmd.ExecuteScalar

                If intNoMensaje > 0 Then
                    'Obtiene el codigo de la persona a quien va dirigido el mensaje
                    strDestinatario = ObtieneCodigoDestinatario(p_destinatario, p_intCodEmpleado)
                    If strDestinatario <> "" Then
                        strDestinatariosMensaje = Split(strDestinatario, ",")

                        Dim intIndicearreglo As Integer
                        For intIndicearreglo = 0 To strDestinatariosMensaje.Length - 1
                            strDestinatariosMensaje(intIndicearreglo) = Trim(strDestinatariosMensaje(intIndicearreglo))
                        Next


                        'Inserta el destinatario a quien va dirigido el mensaje

                        For intIndice = 0 To strDestinatariosMensaje.Length - 1
                            'InsertarDestinatarioMensajeSBO_DMS(intNoMensaje, strDestinatario)
                            InsertarDestinatarioMensajeSBO_DMS(intNoMensaje, Trim(strDestinatariosMensaje(intIndice)))
                        Next
                    End If

                End If

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try
        End Sub

        Private Sub InsertarDestinatarioMensajeSBO_DMS(ByVal p_intNoMensaje As Long, ByVal p_strDestinatario As String)
            Try
                Dim cmd As New SqlClient.SqlCommand

                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = mc_strINSDestinatario
                cmd.Connection = m_cnn

                With cmd.Parameters
                    .Add(mc_strArroba & mc_strNoMensaje, SqlDbType.BigInt, 8).Value = p_intNoMensaje
                    .Add(mc_strArroba & mc_strDestinatario, SqlDbType.VarChar, 50).Value = p_strDestinatario
                End With

                cmd.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try
        End Sub

        'Se utiliza para determinar si hay mensajes nuevos para un usuario
        Public Function HayMensajesNuevos(ByVal usuario As String, ByVal compania As String, ByVal aplicacion As String, ByVal idSucursal As String) As Boolean

            Try
                Dim cmdSel As New SqlClient.SqlCommand
                Dim blnMensajes As Boolean
                Dim intNoMensajes As Integer


                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                cmdSel.CommandType = CommandType.StoredProcedure
                cmdSel.CommandText = mc_strSELMensajesNoLeidos


                With cmdSel
                    .Parameters.Add(mc_strArroba & mc_strUsuario, SqlDbType.VarChar, 15)
                    .Parameters.Add(mc_strArroba & mc_strCompania, SqlDbType.VarChar, 50)
                    .Parameters.Add(mc_strArroba & mc_strAplicacion, SqlDbType.VarChar, 50)
                    .Parameters.Add(mc_strArroba & mc_strCodSucursal, SqlDbType.VarChar, 8)

                    .Parameters(mc_strArroba & mc_strUsuario).Value = usuario
                    .Parameters(mc_strArroba & mc_strCompania).Value = compania
                    .Parameters(mc_strArroba & mc_strAplicacion).Value = aplicacion
                    .Parameters(mc_strArroba & mc_strCodSucursal).Value = idSucursal
                End With

                cmdSel.Connection = m_cnn

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
                m_cnn.Close()
            End Try

        End Function

        'Se utiliza para seleccionar los mensajes nuevos para un usuario
        Public Sub SeleccionarMensajes(ByVal dataset As DestinaXMensajeSBODMSDataSet, ByVal usuario As String _
                                        , ByVal compania As String, ByVal aplicacion As String, ByVal idSucursal As Integer)
            Try

                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                'Creacion del comando
                m_adp.SelectCommand = CrearSelectCommandMensajes()


                m_adp.SelectCommand.Parameters(mc_strArroba & mc_strUsuario).Value = usuario
                m_adp.SelectCommand.Parameters(mc_strArroba & mc_strCompania).Value = compania
                m_adp.SelectCommand.Parameters(mc_strArroba & mc_strAplicacion).Value = aplicacion
                m_adp.SelectCommand.Parameters(mc_strArroba & mc_strCodSucursal).Value = idSucursal

                m_adp.SelectCommand.Connection = m_cnn

                dataset.SCGTA_TB_MensajesSBO_DMS.CheckColumn.DefaultValue = False


                Call m_adp.Fill(dataset.SCGTA_TB_MensajesSBO_DMS)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnn.Close()

            End Try

        End Sub

        'Se utiliza para marcar un mensaje como leido por el usuario
        Public Sub MarcarComoLeido(ByVal dataset As DestinaXMensajeSBODMSDataSet, ByVal usuario As String, ByVal compania As String _
                                    , ByVal aplicacion As String, ByVal idSucursal As Integer)

            Try

                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                m_adp.UpdateCommand = New SqlClient.SqlCommand
                m_adp.UpdateCommand.CommandType = CommandType.StoredProcedure
                m_adp.UpdateCommand.CommandText = mc_strUPDMensajesLeidos

                With m_adp.UpdateCommand
                    .Parameters.Add(mc_strArroba & mc_strUsuario, SqlDbType.VarChar, 15).Value = usuario
                    .Parameters.Add(mc_strArroba & mc_strCompania, SqlDbType.VarChar, 50).Value = compania
                    .Parameters.Add(mc_strArroba & mc_strAplicacion, SqlDbType.VarChar, 50).Value = aplicacion
                    .Parameters.Add(mc_strArroba & mc_strCodSucursal, SqlDbType.VarChar, 8).Value = idSucursal
                    .Parameters.Add(mc_strArroba & mc_strNoMensaje, SqlDbType.BigInt, 8, mc_strNoMensaje)

                End With

                m_adp.UpdateCommand.Connection = m_cnn
                m_adp.Update(dataset.SCGTA_TB_MensajesSBO_DMS)

            Catch ex As Exception
                Throw ex

            Finally
                m_cnn.Close()
            End Try

        End Sub

        Private Function ObtieneCodigoDestinatario(ByVal p_destinatario As RecibeMensaje, ByVal p_intCodEmpleado As Integer) As String

            Dim strCodigo As String = ""

            Try


                Select Case p_destinatario
                    Case RecibeMensaje.EncargadoTaller
                        strCodigo = ObtieneCodigoEncargadoTaller()

                    Case RecibeMensaje.Bodeguero
                        strCodigo = ObtieneCodigoBodeguero()

                    Case RecibeMensaje.Asesor
                        strCodigo = ObtieneCodigoAsesor(p_intCodEmpleado)

                    Case RecibeMensaje.EncargadoRepuestos
                        strCodigo = ObtieneCodigoEncargadoRepuestos()
                    Case RecibeMensaje.EncargadoSuministros
                        strCodigo = ObtieneCodigoEncargadoSuministros()

                End Select



            Catch ex As Exception
                MsgBox(ex.Message)

            End Try

            Return strCodigo

        End Function

        Private Function ObtieneCodigoAsesor(ByVal p_intCodEmpleado As Integer) As String
            Try

                Dim cmdSel As New SqlClient.SqlCommand
                Dim strCodigo As String

                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                cmdSel.CommandType = CommandType.StoredProcedure
                cmdSel.CommandText = mc_strSELCodigoUsuario

                With cmdSel.Parameters
                    .Add(mc_strArroba & mc_strEmpID, SqlDbType.Int, 4).Value = p_intCodEmpleado
                End With

                cmdSel.Connection = m_cnn

                strCodigo = cmdSel.ExecuteScalar()

                Return strCodigo
            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnn.Close()

            End Try
        End Function

        Private Function ObtieneCodigoBodeguero() As String
            Try
                Dim strCodigo As String =  String.Empty
                Dim adpConf As New ConfiguracionDataAdapter '(m_strConexion)
                Dim dstConf As New ConfiguracionDataSet

                adpConf.Fill(dstConf)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, mc_strEncargadoBodega, strCodigo)


                Return strCodigo


            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try

            Return String.Empty
        End Function

        Private Function ObtieneCodigoEncargadoRepuestos() As String
            Try

                Dim strCodigo As String =  String.Empty
                Dim adpConf As New ConfiguracionDataAdapter(m_strConexion)
                Dim dstConf As New ConfiguracionDataSet

                adpConf.Fill(dstConf)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, mc_strEncargadoRepuestos, strCodigo)

                Return strCodigo

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try
            Return String.Empty
        End Function

        Private Function ObtieneCodigoEncargadoSuministros() As String
            Try

                Dim strCodigo As String =  String.Empty
                Dim adpConf As New ConfiguracionDataAdapter(m_strConexion)
                Dim dstConf As New ConfiguracionDataSet

                adpConf.Fill(dstConf)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, mc_strEncargadoSuministros, strCodigo)

                Return strCodigo

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try
            Return String.Empty

        End Function

        Private Function ObtieneCodigoEncargadoTaller() As String
            Try

                Dim strCodigo As String =  String.Empty
                Dim adpConf As New ConfiguracionDataAdapter(m_strConexion)
                Dim dstConf As New ConfiguracionDataSet

                adpConf.Fill(dstConf)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, mc_strEncargadoTaller, strCodigo)


                Return strCodigo

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try
            Return String.Empty
        End Function


#Region "Mensajes DMS a SBO"

        'Enviar mensaje al asesor por el numero de orden
        Public Sub CreaMensajeDMS_SBO_Cotizacion(ByVal p_strMensaje As String, ByVal p_strAsunto As String, ByVal p_intDestinatario As RecibeMensaje, ByVal p_strNoOrden As String)
            Try
                m_objCompany = G_objCompany
                Dim oMsg As SAPbobsCOM.Messages

                Dim intResultado As Integer
                Dim strError As String =  String.Empty
                Dim intError As Integer
                Dim strCodigoUsuario As String
                Dim intIdAsesor As Integer
                Dim intDocEntry As Integer
                Dim intIndice As Integer
                Dim strArregloUsuarios() As String

                If p_intDestinatario = RecibeMensaje.Asesor Then
                    'obtiene el id del asesor a partir del numero de orden
                    intIdAsesor = ObtieneIDAsesor(p_strNoOrden)
                End If

                'Obtiene el codigo de usuario a quien va dirigido el mensaje
                strCodigoUsuario = ObtieneCodigoDestinatario(p_intDestinatario, intIdAsesor)
                strArregloUsuarios = Split(strCodigoUsuario, ",")

                Dim intIndicearreglo As Integer
                For intIndicearreglo = 0 To strArregloUsuarios.Length - 1
                    strArregloUsuarios(intIndicearreglo) = Trim(strArregloUsuarios(intIndicearreglo))
                Next

                'Obtiene el DocEntry de la Cotizacion a la que pertenece la orden
                intDocEntry = ObtieneDocEntryCotizacion(p_strNoOrden)

                If (strCodigoUsuario <> "") Then
                    'Crea el mensaje
                    oMsg = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                    oMsg.MessageText = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                    oMsg.Subject = p_strAsunto & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden

                    m_HashTableUsuarios.Clear()

                    For intIndice = 0 To strArregloUsuarios.Length - 1

                        If Not m_HashTableUsuarios.ContainsKey(Trim(strArregloUsuarios(intIndice))) Then

                            m_HashTableUsuarios.Add(Trim(strArregloUsuarios(intIndice)), Trim(strArregloUsuarios(intIndice)))

                            oMsg.Recipients.Add()
                            oMsg.Recipients.SetCurrentLine(intIndice)
                            oMsg.Recipients.UserCode = Trim(strArregloUsuarios(intIndice))
                            oMsg.Recipients.NameTo = Trim(strArregloUsuarios(intIndice))
                            oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES

                        End If
                        
                    Next
                    oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.Cotizacion & "," & My.Resources.ResourceFrameWork.Referencia & ": " & intDocEntry, SAPbobsCOM.BoObjectTypes.oQuotations, CStr(intDocEntry))

                    intResultado = oMsg.Add()
                    If (intResultado <> 0) Then
                        m_objCompany.GetLastError(intError, strError)
                        Throw New ExceptionsSBO(intError, strError)
                        'MsgBox("Error:" + Str(lngError) + "," + strError)
                    End If
                    If Not oMsg Is Nothing Then
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oMsg)
                        oMsg = Nothing
                    End If

                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        'Enviar mensaje a un destinatario ya conocido
        Public Sub CreaMensajeDMS_SBO_Cotizacion(ByVal p_strMensaje As String, ByVal p_strDestinatario As String, ByVal p_strNoOrden As String)
            Try
                m_objCompany = G_objCompany
                Dim oMsg As SAPbobsCOM.Messages

                Dim intResultado As Integer
                Dim strError As String =  String.Empty
                Dim intError As Integer
'                Dim intIdAsesor As Integer
                Dim intDocEntry As Integer
                Dim strArregloDestinatario() As String
                Dim intIndice As Integer


                'Obtiene el DocEntry de la Cotizacion a la que pertenece la orden
                intDocEntry = ObtieneDocEntryCotizacion(p_strNoOrden)

                If (p_strDestinatario <> "") Then

                    strArregloDestinatario = Split(p_strDestinatario, ",")

                    Dim intIndicearreglo As Integer
                    For intIndicearreglo = 0 To strArregloDestinatario.Length - 1
                        strArregloDestinatario(intIndicearreglo) = Trim(strArregloDestinatario(intIndicearreglo))
                    Next

                    'Crea el mensaje
                    oMsg = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                    oMsg.MessageText = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                    oMsg.Subject = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden

                    m_HashTableUsuarios.Clear()

                    For intIndice = 0 To strArregloDestinatario.Length - 1

                        If Not m_HashTableUsuarios.ContainsKey(Trim(strArregloDestinatario(intIndice))) Then

                            m_HashTableUsuarios.Add(Trim(strArregloDestinatario(intIndice)), Trim(strArregloDestinatario(intIndice)))

                            oMsg.Recipients.Add()
                            oMsg.Recipients.SetCurrentLine(intIndice)
                            oMsg.Recipients.UserCode = Trim(strArregloDestinatario(intIndice))
                            oMsg.Recipients.NameTo = Trim(strArregloDestinatario(intIndice))
                            oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES

                        End If

                        
                    Next
                    oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.Cotizacion & "," & My.Resources.ResourceFrameWork.Referencia & ": " & intDocEntry, SAPbobsCOM.BoObjectTypes.oQuotations, CStr(intDocEntry))

                    intResultado = oMsg.Add()
                    If (intResultado <> 0) Then
                        m_objCompany.GetLastError(intError, strError)
                        Throw New ExceptionsSBO(intError, strError)
                    End If



                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub


        'Enviar mensaje al asesor por el numero de cotización
        Public Sub CreaMensajeDMS_SBO_Cotizacion(ByVal p_strMensaje As String, ByVal p_intDestinatario As RecibeMensaje, ByVal p_intNoCituzacion As Integer, Optional ByVal blnSolicitud As Boolean = False)
            Try
                m_objCompany = G_objCompany
                Dim oMsg As SAPbobsCOM.Messages

                Dim intResultado As Integer
                Dim strError As String = String.Empty
                Dim intError As Integer
                Dim strCodigoUsuario As String
                Dim intIdAsesor As Integer
                Dim strArregloUsuarios As String()
                Dim intindice As Integer
                Dim tempSubject As String

                If p_intDestinatario = RecibeMensaje.Asesor Then
                    'obtiene el id del asesor a partir del numero de orden
                    intIdAsesor = ObtieneIDAsesor(p_intNoCituzacion)
                End If

                'Obtiene el codigo de usuario a quien va dirigido el mensaje
                strCodigoUsuario = ObtieneCodigoDestinatario(p_intDestinatario, intIdAsesor)
                strArregloUsuarios = Split(strCodigoUsuario, ",")

                Dim intIndicearreglo As Integer
                For intIndicearreglo = 0 To strArregloUsuarios.Length - 1
                    strArregloUsuarios(intIndicearreglo) = Trim(strArregloUsuarios(intIndicearreglo))
                Next


                If (strCodigoUsuario <> "") Then

                    'Crea el mensaje
                    oMsg = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                    oMsg.MessageText = p_strMensaje
                    'Se realiza la validación del tamaño del subject, ya que SAP solo soporta un tamaño de 50 caracteres
                    If (p_strMensaje.Length > 50) Then
                        tempSubject = p_strMensaje.Remove(49)
                        p_strMensaje = tempSubject
                        oMsg.Subject = p_strMensaje
                    Else
                        oMsg.Subject = p_strMensaje
                    End If


                    m_HashTableUsuarios.Clear()

                    For intindice = 0 To strArregloUsuarios.Length - 1

                        If Not m_HashTableUsuarios.ContainsKey(Trim(strArregloUsuarios(intindice))) Then

                            m_HashTableUsuarios.Add(Trim(strArregloUsuarios(intindice)), Trim(strArregloUsuarios(intindice)))

                            oMsg.Recipients.Add()
                            oMsg.Recipients.SetCurrentLine(intindice)
                            oMsg.Recipients.UserCode = Trim(strArregloUsuarios(intindice))
                            oMsg.Recipients.NameTo = Trim(strArregloUsuarios(intindice))
                            oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES

                        End If

                    Next

                    If Not blnSolicitud Then
                        oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.Cotizacion & "," & My.Resources.ResourceFrameWork.Referencia & ": " & p_intNoCituzacion, SAPbobsCOM.BoObjectTypes.oQuotations, CStr(p_intNoCituzacion))
                    End If


                    intResultado = oMsg.Add()
                    If (intResultado <> 0) Then
                        m_objCompany.GetLastError(intError, strError)
                        Throw New ExceptionsSBO(intError, strError)
                    End If



                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub


        ' For intindice = 0 To strCadenaMensajes.Length - 1
        ''there are two recipients in this message
        '                        oMsg.Recipients.Add()

        ''set values for the first recipients
        '                        oMsg.Recipients.SetCurrentLine(intindice)
        '                        Select Case strConexionEstablecida
        '                            Case "AUTOMOTRIZ"
        '                                oMsg.Recipients.UserCode = Trim(strCadenaMensajes(intindice)) '"manager" 'strUsuarioMensajeNotaCreda
        '                                oMsg.Recipients.NameTo = Trim(strCadenaMensajes(intindice)) '"manager" 'strUsuarioMensajeNotaCredClienteSKODA
        '                        End Select
        '                        oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
        '                    Next


        'Enviar mensaje al asesor por el numero de cotización
        Public Sub CreaMensajeDMS_SBO_Cotizacion(ByVal p_strMensaje As String, ByVal p_strDestinatario As String, ByVal p_intNoCotizacion As Integer)
            Try
                m_objCompany = G_objCompany
                Dim oMsg As SAPbobsCOM.Messages

                Dim intResultado As Integer
                Dim strError As String = String.Empty
                Dim intError As Integer
                Dim strArregloUsuarios() As String
                Dim intIndice As Integer
                Dim tempSubject As String
                ' Dim intIdAsesor As Integer

                If (p_strDestinatario <> "") Then

                    'Crea el mensaje
                    oMsg = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                    oMsg.MessageText = p_strMensaje
                    'oMsg.Subject = p_strMensaje
                    'Se realiza la validación del tamaño del subject, ya que SAP solo soporta un tamaño de 50 caracteres
                    If (p_strMensaje.Length > 50) Then
                        tempSubject = p_strMensaje.Remove(49)
                        p_strMensaje = tempSubject
                        oMsg.Subject = p_strMensaje
                    Else
                        oMsg.Subject = p_strMensaje
                    End If




                    strArregloUsuarios = Split(p_strDestinatario, ",")

                    Dim intIndicearreglo As Integer
                    For intIndicearreglo = 0 To strArregloUsuarios.Length - 1
                        strArregloUsuarios(intIndicearreglo) = Trim(strArregloUsuarios(intIndicearreglo))
                    Next

                    m_HashTableUsuarios.Clear()

                    For intIndice = 0 To strArregloUsuarios.Length - 1

                        If Not m_HashTableUsuarios.ContainsKey((strArregloUsuarios(intIndice))) Then

                            m_HashTableUsuarios.Add(Trim(strArregloUsuarios(intIndice)), Trim(strArregloUsuarios(intIndice)))

                            oMsg.Recipients.Add()
                            oMsg.Recipients.SetCurrentLine(intIndice)
                            oMsg.Recipients.UserCode = Trim(strArregloUsuarios(intIndice))
                            oMsg.Recipients.NameTo = Trim(strArregloUsuarios(intIndice))
                            oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES

                        End If
                        
                    Next

                    oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.Cotizacion & "," & My.Resources.ResourceFrameWork.Referencia & ": " & p_intNoCotizacion, SAPbobsCOM.BoObjectTypes.oQuotations, CStr(p_intNoCotizacion))

                    intResultado = oMsg.Add()
                    If (intResultado <> 0) Then
                        m_objCompany.GetLastError(intError, strError)
                        Throw New ExceptionsSBO(intError, strError)

                    End If


                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        ' Metodo utilizado para la funcionalidad de mensajería por centro de costo
        Public Sub CreaMensajeDMS_SBO_TransferenciaXCancelacionXCentroCosto(ByVal p_strMensaje As String, ByVal Destinatario As RecibeMensaje, ByVal p_strNumeroOrden As String, ByVal p_strDocEntry As String, ByVal blnDraft As Boolean)
            Try

                m_objCompany = G_objCompany
                Dim oMsg As SAPbobsCOM.Messages

                Dim intResultado As Integer
                Dim strError As String = String.Empty
                Dim intError As Integer
                Dim strCodigoUsuario As String = String.Empty
                Dim strArregloUsuarios() As String
                Dim intIndice As Integer


                ' Dim intIdAsesor As Integer
                Dim strArregloDocEntry As String()
                Dim strArregloCentroCosto As String()
                Dim strCentroCosto As String = ""
                Dim intCont As Integer
                Dim blnConfCentroCosto As Boolean = False
                Dim intDocEntry As Integer


                strArregloDocEntry = p_strDocEntry.Split(",")
                'For intCont = 0 To (strArregloDocEntry.Length) - 1
                '    strCentroCosto = RecorreLineasDocumentoTransferencia(strArregloDocEntry(intCont), strCentroCosto, m_objCompany, blnDraft)
                'Next

                'If strCentroCosto <> "" Then
                '    strArregloCentroCosto = strCentroCosto.Split(",")
                '    blnConfCentroCosto = False
                'Else
                '    blnConfCentroCosto = True
                'End If




                For intDocEntry = 0 To (strArregloDocEntry.Length) - 1
                    strCentroCosto = String.Empty
                    strCentroCosto = RecorreLineasDocumentoTransferencia(strArregloDocEntry(intDocEntry), strCentroCosto, m_objCompany, blnDraft)
                    If strCentroCosto <> "" Then
                        'strArregloCentroCosto = strCentroCosto.Split(",")
                        blnConfCentroCosto = True
                    Else
                        blnConfCentroCosto = False
                    End If

                    If blnConfCentroCosto = True Then

                        strCodigoUsuario = String.Empty
                        'Crea el mensaje
                        oMsg = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                        oMsg.MessageText = p_strMensaje
                        oMsg.Subject = p_strMensaje

                        Select Case Destinatario
                            Case RecibeMensaje.EncargadoSuministros
                                'strCodigoUsuario = ObtieneCodigoBodegueroSBOSUM(True)
                                strCodigoUsuario = ConsultarMensajeriaXCentroCosto(strCentroCosto, mc_strEncargadoSuministros)
                            Case RecibeMensaje.EncargadoRepuestos
                                'strCodigoUsuario = ObtieneCodigoBodeguero()
                                strCodigoUsuario = ConsultarMensajeriaXCentroCosto(strCentroCosto, mc_strEncargadoRepuestos)
                        End Select

                        strArregloUsuarios = Split(strCodigoUsuario, ",")

                        Dim intIndicearreglo As Integer
                        For intIndicearreglo = 0 To strArregloUsuarios.Length - 1
                            strArregloUsuarios(intIndicearreglo) = Trim(strArregloUsuarios(intIndicearreglo))
                        Next

                        m_HashTableUsuarios.Clear()

                        For intIndice = 0 To strArregloUsuarios.Length - 1

                            If Not m_HashTableUsuarios.ContainsKey(Trim(strArregloUsuarios(intIndice))) Then

                                m_HashTableUsuarios.Add(Trim(strArregloUsuarios(intIndice)), Trim(strArregloUsuarios(intIndice)))

                                oMsg.Recipients.Add()
                                oMsg.Recipients.SetCurrentLine(intIndice)
                                oMsg.Recipients.UserCode = Trim(strArregloUsuarios(intIndice))
                                oMsg.Recipients.NameTo = Trim(strArregloUsuarios(intIndice))
                                oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES

                            End If
                            
                        Next
                        oMsg.Subject = String.Format(My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOT, strArregloDocEntry(intDocEntry), p_strNumeroOrden)
                        oMsg.MessageText = oMsg.Subject + vbNewLine + My.Resources.ResourceFrameWork.MensajeTransferenciaCancelacion
                        '                oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.Cotizacion & "," & My.Resources.ResourceFrameWork.Referencia & ": " & p_intDocEntry, SAPbobsCOM.BoObjectTypes.oStockTransfer, CStr(p_intDocEntry))
                        intResultado = oMsg.Add()
                        If (intResultado <> 0) Then
                            m_objCompany.GetLastError(intError, strError)
                            Throw New ExceptionsSBO(intError, strError)
                        End If
                    End If


                Next

                If Not oMsg Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oMsg)
                    oMsg = Nothing
                End If
                
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub


        Public Sub CreaMensajeDMS_SBO_TransferenciaXCancelacion(ByVal p_strMensaje As String, ByVal Destinatario As RecibeMensaje, ByVal p_strNumeroOrden As String, ByVal p_intDocEntry As Integer)
            Try

                m_objCompany = G_objCompany
                Dim oMsg As SAPbobsCOM.Messages

                Dim intResultado As Integer
                Dim strError As String = String.Empty
                Dim intError As Integer
                Dim strCodigoUsuario As String = String.Empty
                Dim strArregloUsuarios() As String
                Dim intIndice As Integer

                'Crea el mensaje
                oMsg = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                oMsg.MessageText = p_strMensaje
                oMsg.Subject = p_strMensaje

                Select Case Destinatario
                    Case RecibeMensaje.Bodeguero, RecibeMensaje.EncargadoSuministros
                        strCodigoUsuario = ObtieneCodigoBodegueroSBOSUM(True)
                    Case RecibeMensaje.EncargadoRepuestos
                        strCodigoUsuario = ObtieneCodigoBodeguero()
                End Select

                If Not String.IsNullOrEmpty(strCodigoUsuario) Then

                    strArregloUsuarios = Split(strCodigoUsuario, ",")

                    Dim intIndicearreglo As Integer
                    For intIndicearreglo = 0 To strArregloUsuarios.Length - 1
                        strArregloUsuarios(intIndicearreglo) = Trim(strArregloUsuarios(intIndicearreglo))
                    Next

                    m_HashTableUsuarios.Clear()

                    For intIndice = 0 To strArregloUsuarios.Length - 1

                        If Not m_HashTableUsuarios.ContainsKey(Trim(strArregloUsuarios(intIndice))) Then

                            m_HashTableUsuarios.Add(Trim(strArregloUsuarios(intIndice)), Trim(strArregloUsuarios(intIndice)))

                            oMsg.Recipients.Add()
                            oMsg.Recipients.SetCurrentLine(intIndice)
                            oMsg.Recipients.UserCode = Trim(strArregloUsuarios(intIndice))
                            oMsg.Recipients.NameTo = Trim(strArregloUsuarios(intIndice))
                            oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES

                        End If

                    Next
                    oMsg.Subject = String.Format(My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOT, p_intDocEntry, p_strNumeroOrden)
                    oMsg.MessageText = oMsg.Subject + vbNewLine + My.Resources.ResourceFrameWork.MensajeTransferenciaCancelacion

                    intResultado = oMsg.Add()
                    If (intResultado <> 0) Then
                        m_objCompany.GetLastError(intError, strError)
                        Throw New ExceptionsSBO(intError, strError)
                    End If

                    If Not oMsg Is Nothing Then
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oMsg)
                        oMsg = Nothing
                    End If

                End If

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        Public Sub CreaMensajeDMS_SBO_OfertaCompra(ByVal p_strMensaje As String, ByVal p_intDocNum As Integer, ByVal p_strNoOrden As String)
            Try
                m_objCompany = G_objCompany
                Dim oMsg As SAPbobsCOM.Messages

                Dim intResultado As Integer
                Dim strError As String = String.Empty
                Dim intError As Integer
                Dim strCodigoUsuario As String
                Dim intDocEntry As Integer
                Dim strUsuariosMensaje As String()
                Dim intindice As Integer


                'Obtiene el codigo del bodeguero. Los mensajes de órdenes de compra solo se le envían a él.
                strCodigoUsuario = ObtieneCodigoBodeguero()
                strUsuariosMensaje = Split(strCodigoUsuario, ",")

                Dim intIndicearreglo As Integer
                For intIndicearreglo = 0 To strUsuariosMensaje.Length - 1
                    strUsuariosMensaje(intIndicearreglo) = Trim(strUsuariosMensaje(intIndicearreglo))
                Next



                'Obtiene el DocEntry a partir del DocNum de la orden de compra
                intDocEntry = ObtieneDocEntryOfertaCompra(p_intDocNum)

                If (strCodigoUsuario <> "") Then

                    'Crea el mensaje
                    oMsg = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                    oMsg.MessageText = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                    oMsg.Subject = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden

                    m_HashTableUsuarios.Clear()

                    For intindice = 0 To (strUsuariosMensaje.Length - 1)

                        If Not m_HashTableUsuarios.ContainsKey(Trim(strUsuariosMensaje(intindice))) Then

                            m_HashTableUsuarios.Add(Trim(strUsuariosMensaje(intindice)), Trim(strUsuariosMensaje(intindice)))

                            oMsg.Recipients.Add()
                            oMsg.Recipients.SetCurrentLine(intindice)
                            oMsg.Recipients.UserCode = Trim(strUsuariosMensaje(intindice)) 'strCodigoUsuario
                            oMsg.Recipients.NameTo = Trim(strUsuariosMensaje(intindice)) 'strCodigoUsuario
                            oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES

                        End If
                        
                    Next
                    ' oMsg.AddDataColumn("Favor Revisar", "Orden de compra,  Referencia: " & intDocEntry, SAPbobsCOM.BoObjectTypes.oPurchaseOrders, CStr(intDocEntry))
                    oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.OfertaCompra & "," & My.Resources.ResourceFrameWork.Referencia & ": " & intDocEntry, 540000006, CStr(intDocEntry))
                    intResultado = oMsg.Add()
                    If (intResultado <> 0) Then
                        m_objCompany.GetLastError(intError, strError)
                        Throw New ExceptionsSBO(intError, strError)
                        'MsgBox("Error:" + Str(lngError) + "," + strError)
                    End If

                    

                End If

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        Public Sub CreaMensajeDMS_SBO_OrdenCompra(ByVal p_strMensaje As String, ByVal p_intDocNum As Integer, ByVal p_strNoOrden As String)
            Try
                m_objCompany = G_objCompany
                Dim oMsg As SAPbobsCOM.Messages

                Dim intResultado As Integer
                Dim strError As String =  String.Empty
                Dim intError As Integer
                Dim strCodigoUsuario As String
                Dim intDocEntry As Integer
                Dim strUsuariosMensaje As String()
                Dim intindice As Integer


                'Obtiene el codigo del bodeguero. Los mensajes de órdenes de compra solo se le envían a él.
                strCodigoUsuario = ObtieneCodigoBodeguero()
                strUsuariosMensaje = Split(strCodigoUsuario, ",")

                Dim intIndicearreglo As Integer
                For intIndicearreglo = 0 To strUsuariosMensaje.Length - 1
                    strUsuariosMensaje(intIndicearreglo) = Trim(strUsuariosMensaje(intIndicearreglo))
                Next



                'Obtiene el DocEntry a partir del DocNum de la orden de compra
                intDocEntry = ObtieneDocEntryOrdenCompra(p_intDocNum)

                If (strCodigoUsuario <> "") Then

                    'Crea el mensaje
                    oMsg = m_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                    oMsg.MessageText = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                    oMsg.Subject = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden

                    m_HashTableUsuarios.Clear()

                    For intindice = 0 To (strUsuariosMensaje.Length - 1)

                        If Not m_HashTableUsuarios.ContainsKey(Trim(strUsuariosMensaje(intindice))) Then

                            m_HashTableUsuarios.Add(Trim(strUsuariosMensaje(intindice)), Trim(strUsuariosMensaje(intindice)))

                            oMsg.Recipients.Add()
                            oMsg.Recipients.SetCurrentLine(intindice)
                            oMsg.Recipients.UserCode = Trim(strUsuariosMensaje(intindice)) 'strCodigoUsuario
                            oMsg.Recipients.NameTo = Trim(strUsuariosMensaje(intindice)) 'strCodigoUsuario
                            oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES

                        End If
                        
                    Next
                    ' oMsg.AddDataColumn("Favor Revisar", "Orden de compra,  Referencia: " & intDocEntry, SAPbobsCOM.BoObjectTypes.oPurchaseOrders, CStr(intDocEntry))
                    oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.OrdenCompra & "," & My.Resources.ResourceFrameWork.Referencia & ": " & intDocEntry, SAPbobsCOM.BoObjectTypes.oPurchaseOrders, CStr(intDocEntry))
                    intResultado = oMsg.Add()
                    If (intResultado <> 0) Then
                        m_objCompany.GetLastError(intError, strError)
                        Throw New ExceptionsSBO(intError, strError)
                        'MsgBox("Error:" + Str(lngError) + "," + strError)
                    End If



                End If

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        Private Function ObtieneIDAsesor(ByVal p_strNoOrden As String) As Integer
            'Obtiene el ID del asesor de la orden
            Try
                Dim intIDAsesor As Integer
                Dim cmdSel As New SqlClient.SqlCommand

                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                cmdSel.CommandType = CommandType.StoredProcedure
                cmdSel.CommandText = mc_strSELIdAsesor
                cmdSel.Connection = m_cnn

                With cmdSel.Parameters
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden
                End With

                intIDAsesor = cmdSel.ExecuteScalar()

                Return intIDAsesor

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try

        End Function

        Private Function ObtieneIDAsesor(ByVal p_intNoCotizacion As Integer) As Integer
            'Obtiene el ID del asesor de la orden
            Try
                Dim intIDAsesor As Integer
                Dim cmdSel As New SqlClient.SqlCommand

                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                cmdSel.CommandType = CommandType.Text
                cmdSel.CommandText = "Select OwnerCode from SCGTA_VW_OQUT Where DocEntry = " & p_intNoCotizacion
                cmdSel.Connection = m_cnn

                intIDAsesor = cmdSel.ExecuteScalar()

                Return intIDAsesor

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try

        End Function

        Private Function ObtieneDocEntryCotizacion(ByVal p_strNoOrden As String) As Integer
            'Obtiene el DocEntry de la cotización de la orden
            Try
                Dim intDocEntry As Integer
                Dim cmdSel As New SqlClient.SqlCommand

                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                cmdSel.CommandType = CommandType.StoredProcedure
                cmdSel.CommandText = mc_strSELDocEntryCotizacion
                cmdSel.Connection = m_cnn

                With cmdSel.Parameters
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden
                End With

                intDocEntry = cmdSel.ExecuteScalar()

                Return intDocEntry

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try
        End Function


        Private Function ObtieneDocEntryOfertaCompra(ByVal p_intDocNum As Integer) As Integer
            'Obtiene el DocEntry de la orden de compra a partir del DocNum
            Try
                Dim intDocEntry As Integer
                Dim cmdSel As New SqlClient.SqlCommand

                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                cmdSel.CommandType = CommandType.StoredProcedure
                cmdSel.CommandText = mc_strSELDocEntryOfertaCompra
                cmdSel.Connection = m_cnn

                With cmdSel.Parameters
                    .Add(mc_strArroba & mc_strDocNum, SqlDbType.Int).Value = p_intDocNum
                End With

                intDocEntry = cmdSel.ExecuteScalar()

                Return intDocEntry

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try
        End Function

        Private Function ObtieneDocEntryOrdenCompra(ByVal p_intDocNum As Integer) As Integer
            'Obtiene el DocEntry de la orden de compra a partir del DocNum
            Try
                Dim intDocEntry As Integer
                Dim cmdSel As New SqlClient.SqlCommand

                If m_cnn.State = ConnectionState.Closed Then
                    Call m_cnn.Open()
                End If

                cmdSel.CommandType = CommandType.StoredProcedure
                cmdSel.CommandText = mc_strSELDocEntryOrdenCompra
                cmdSel.Connection = m_cnn

                With cmdSel.Parameters
                    .Add(mc_strArroba & mc_strDocNum, SqlDbType.Int).Value = p_intDocNum
                End With

                intDocEntry = cmdSel.ExecuteScalar()

                Return intDocEntry

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try
        End Function

#End Region

#Region "Mensajes SBO a SBO"

        Public Sub CreaMensajeSBO_SBO_CotizacionXCentroCosto(ByVal p_strMensaje As String, ByVal p_strDocEntry As String, ByVal p_ocompany As SAPbobsCOM.Company, ByVal p_strNoOrden As String, ByVal p_intTipoMensaje As MensajeriaSBOTallerDataAdapter.TipoMensaje, ByVal blnDraft As Boolean)
            'Crea mensaje en SAP para el bodeguero sobre creacion de un documento de traslado
            Try
                'Dim m_objCompany As New SAPbobsCOM.Company
                Dim oMsg As SAPbobsCOM.Messages

                Dim intResultado As Integer
                Dim strError As String = String.Empty
                Dim intError As Integer
                Dim strCodigoUsuarioREP As String = String.Empty
                Dim strCodigoUsuarioSUM As String = String.Empty

                Dim strArreglo As String()
                Dim strArregloUsuariosREP() As String = Nothing
                Dim strArregloUsuariosSUM() As String = Nothing
                Dim intIndice As Integer
                Dim intindiceUsuarios As Integer
                Dim intIndicearreglo As Integer

                Dim strNombreAsesor As String = String.Empty



                Dim strArregloDocEntry As String()
                Dim strArregloCentroCosto As String()
                Dim strCentroCosto As String = ""
                Dim intCont As Integer
                Dim blnConfCentroCosto As Boolean = False

                strArregloDocEntry = p_strDocEntry.Split(",")
                For intCont = 0 To (strArregloDocEntry.Length) - 1
                    strCentroCosto = RecorreLineasDocumentoTransferencia(strArregloDocEntry(intCont), strCentroCosto, p_ocompany, blnDraft)
                Next

                If strCentroCosto <> "" Then
                    'strArregloCentroCosto = strCentroCosto.Split(",")
                    blnConfCentroCosto = False
                Else
                    blnConfCentroCosto = True
                End If


                If blnConfCentroCosto <> True Then

                    'Obtiene el codigo del bodeguero
                    Select Case p_intTipoMensaje
                        Case TipoMensaje.scgPeticionRepuestos
                            'strCodigoUsuarioREP = ObtieneCodigoBodegueroSBO()
                            strCodigoUsuarioREP = ConsultarMensajeriaXCentroCosto(strCentroCosto, mc_strEncargadoRepuestos)
                            strArregloUsuariosREP = Split(strCodigoUsuarioREP, ",")

                            For intIndicearreglo = 0 To strArregloUsuariosREP.Length - 1
                                strArregloUsuariosREP(intIndicearreglo) = Trim(strArregloUsuariosREP(intIndicearreglo))
                            Next

                        Case TipoMensaje.scgPeticionSuministros
                            'strCodigoUsuarioSUM = ObtieneCodigoBodegueroSBOSUM()
                            strCodigoUsuarioSUM = ConsultarMensajeriaXCentroCosto(strCentroCosto, mc_strEncargadoSuministros)
                            strArregloUsuariosSUM = Split(strCodigoUsuarioSUM, ",")

                            For intIndicearreglo = 0 To strArregloUsuariosSUM.Length - 1
                                strArregloUsuariosSUM(intIndicearreglo) = Trim(strArregloUsuariosSUM(intIndicearreglo))
                            Next

                        Case TipoMensaje.scgDevolucionRepuestos
                            'strCodigoUsuarioREP = ObtieneCodigoBodegueroSBO()
                            strCodigoUsuarioREP = ConsultarMensajeriaXCentroCosto(strCentroCosto, mc_strEncargadoRepuestos)
                            strArregloUsuariosREP = Split(strCodigoUsuarioREP, ",")

                            For intIndicearreglo = 0 To strArregloUsuariosREP.Length - 1
                                strArregloUsuariosREP(intIndicearreglo) = Trim(strArregloUsuariosREP(intIndicearreglo))
                            Next

                        Case TipoMensaje.scgDevolucionSuministros

                            'strCodigoUsuarioSUM = ObtieneCodigoBodegueroSBOSUM()
                            strCodigoUsuarioSUM = ConsultarMensajeriaXCentroCosto(strCentroCosto, mc_strEncargadoSuministros)
                            strArregloUsuariosSUM = Split(strCodigoUsuarioSUM, ",")

                            For intIndicearreglo = 0 To strArregloUsuariosSUM.Length - 1
                                strArregloUsuariosSUM(intIndicearreglo) = Trim(strArregloUsuariosSUM(intIndicearreglo))
                            Next

                    End Select

                    strArreglo = p_strDocEntry.Split(",")


                    If (strCodigoUsuarioREP <> "") Then

                        'Crea el mensaje
                        If blnDraft Then

                            oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                            oMsg.MessageText = String.Format(My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOT, p_strDocEntry, p_strNoOrden)
                            oMsg.Subject = oMsg.MessageText

                        Else
                            oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                            oMsg.MessageText = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                            oMsg.Subject = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden

                        End If

                        m_HashTableUsuarios.Clear()

                        For intindiceUsuarios = 0 To strArregloUsuariosREP.Length - 1

                            If Not m_HashTableUsuarios.ContainsKey(Trim(strArregloUsuariosREP(intindiceUsuarios))) Then

                                m_HashTableUsuarios.Add(Trim(strArregloUsuariosREP(intindiceUsuarios)), Trim(strArregloUsuariosREP(intindiceUsuarios)))

                                oMsg.Recipients.Add()
                                oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
                                oMsg.Recipients.UserCode = Trim(strArregloUsuariosREP(intindiceUsuarios))
                                oMsg.Recipients.NameTo = Trim(strArregloUsuariosREP(intindiceUsuarios))
                                oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES

                            End If
                            
                        Next

                        

                        For intIndice = 0 To (strArreglo.Length) - 1

                            'verifica que el documento creado sea un draft
                            If blnDraft Then
                                'oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.DocumentoBorrador & "," & My.Resources.ResourceFrameWork.Referencia & ": " & CStr(strArreglo(intIndice)), SAPbobsCOM.BoObjectTypes.oDrafts, CStr(strArreglo(intIndice)))
                            Else
                                oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.Traslado & "," & My.Resources.ResourceFrameWork.Referencia & ": " & CStr(strArreglo(intIndice)), SAPbobsCOM.BoObjectTypes.oStockTransfer, CStr(strArreglo(intIndice)))
                            End If

                        Next

                        intResultado = oMsg.Add()
                        If (intResultado <> 0) Then
                            p_ocompany.GetLastError(intError, strError)
                            Throw New ExceptionsSBO(intError, strError)
                        End If


                    End If
                    If (strCodigoUsuarioSUM <> "") Then

                        'Crea el mensaje
                        If blnDraft Then
                            oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                            oMsg.MessageText = String.Format(My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOT, p_strDocEntry, p_strNoOrden)
                            oMsg.Subject = oMsg.MessageText

                        Else
                            oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                            oMsg.MessageText = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                            oMsg.Subject = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden

                        End If

                        m_HashTableUsuarios.Clear()

                        For intindiceUsuarios = 0 To strArregloUsuariosSUM.Length - 1

                            If Not m_HashTableUsuarios.ContainsKey(Trim(strArregloUsuariosSUM(intindiceUsuarios))) Then

                                m_HashTableUsuarios.Add(Trim(strArregloUsuariosSUM(intindiceUsuarios)), Trim(strArregloUsuariosSUM(intindiceUsuarios)))

                                oMsg.Recipients.Add()
                                oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
                                oMsg.Recipients.UserCode = Trim(strArregloUsuariosSUM(intindiceUsuarios))
                                oMsg.Recipients.NameTo = Trim(strArregloUsuariosSUM(intindiceUsuarios))
                                oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES

                            End If

                            For intIndice = 0 To (strArreglo.Length) - 1
                                If blnDraft Then
                                    ''''oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.DocumentoBorrador & "," & My.Resources.ResourceFrameWork.Referencia & ": " & CStr(strArreglo(intIndice)),SAPbobsCOM.BoObjectTypes. ) ', SAPbobsCOM.BoObjectTypes.oDrafts, CStr(strArreglo(intIndice)))
                                Else
                                    'oMsg.AddDataColumn("Favor Revisar", "Traslado,  Referencia: " & CStr(strArreglo(intIndice)), SAPbobsCOM.BoObjectTypes.oStockTransfer, CStr(strArreglo(intIndice)))
                                    oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.Traslado & "," & My.Resources.ResourceFrameWork.Referencia & ": " & CStr(strArreglo(intIndice)), SAPbobsCOM.BoObjectTypes.oStockTransfer, CStr(strArreglo(intIndice)))
                                End If
                            Next

                            ' oMsg.AddDataColumn("Favor Revisar", "Cotización1,  Referencia: " & p_intDocEntry, SAPbobsCOM.BoObjectTypes.oQuotations, CStr(p_intDocEntry))

                            intResultado = oMsg.Add()
                            If (intResultado <> 0) Then
                                p_ocompany.GetLastError(intError, strError)
                                Throw New ExceptionsSBO(intError, strError)
                                'MsgBox("Error:" + Str(lngError) + "," + strError)
                            End If
                        Next

                    End If
                End If

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub


        Public Sub CreaMensajeSBO_SBO_Cotizacion(ByVal p_strMensaje As String, ByVal p_strDocEntry As String, ByVal p_ocompany As SAPbobsCOM.Company, ByVal p_strNoOrden As String, ByVal p_intTipoMensaje As MensajeriaSBOTallerDataAdapter.TipoMensaje, ByVal blnDraft As Boolean, Optional ByVal Asesor As String = "")
            'Crea mensaje en SAP para el bodeguero sobre creacion de un documento de traslado
            Try
                'Dim m_objCompany As New SAPbobsCOM.Company
                Dim oMsg As SAPbobsCOM.Messages

                Dim intResultado As Integer
                Dim strError As String = String.Empty
                Dim intError As Integer
                Dim strCodigoUsuarioREP As String = String.Empty
                Dim strCodigoUsuarioSUM As String = String.Empty

                Dim strArreglo As String()
                Dim strArregloUsuariosREP() As String = Nothing
                Dim strArregloUsuariosSUM() As String = Nothing
                Dim intIndice As Integer
                Dim intindiceUsuarios As Integer
                Dim intIndicearreglo As Integer

                Dim strNombreAsesor As String = String.Empty

                'Obtiene el codigo del bodeguero
                Select Case p_intTipoMensaje
                    Case TipoMensaje.scgPeticionRepuestos
                        strCodigoUsuarioREP = ObtieneCodigoBodegueroSBO()
                        strArregloUsuariosREP = Split(strCodigoUsuarioREP, ",")

                        For intIndicearreglo = 0 To strArregloUsuariosREP.Length - 1
                            strArregloUsuariosREP(intIndicearreglo) = Trim(strArregloUsuariosREP(intIndicearreglo))
                        Next

                    Case TipoMensaje.scgPeticionSuministros
                        strCodigoUsuarioSUM = ObtieneCodigoBodegueroSBOSUM()
                        strArregloUsuariosSUM = Split(strCodigoUsuarioSUM, ",")

                        For intIndicearreglo = 0 To strArregloUsuariosSUM.Length - 1
                            strArregloUsuariosSUM(intIndicearreglo) = Trim(strArregloUsuariosSUM(intIndicearreglo))
                        Next

                    Case TipoMensaje.scgDevolucionRepuestos
                        strCodigoUsuarioREP = ObtieneCodigoBodegueroSBO()
                        strArregloUsuariosREP = Split(strCodigoUsuarioREP, ",")

                        For intIndicearreglo = 0 To strArregloUsuariosREP.Length - 1
                            strArregloUsuariosREP(intIndicearreglo) = Trim(strArregloUsuariosREP(intIndicearreglo))
                        Next

                    Case TipoMensaje.scgDevolucionSuministros

                        strCodigoUsuarioSUM = ObtieneCodigoBodegueroSBOSUM()
                        strArregloUsuariosSUM = Split(strCodigoUsuarioSUM, ",")

                        For intIndicearreglo = 0 To strArregloUsuariosSUM.Length - 1
                            strArregloUsuariosSUM(intIndicearreglo) = Trim(strArregloUsuariosSUM(intIndicearreglo))
                        Next

                End Select

                strArreglo = p_strDocEntry.Split(",")


                If (strCodigoUsuarioREP <> "") Then

                    'Crea el mensaje
                    If blnDraft Then

                        Select Case p_intTipoMensaje
                            Case TipoMensaje.scgPeticionRepuestos, TipoMensaje.scgPeticionRepuestos
                                oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                oMsg.MessageText = String.Format(My.Resources.ResourceFrameWork.MensajeTraslado & " - " & My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOT, p_strDocEntry, p_strNoOrden, " " & Asesor)
                                oMsg.Subject = oMsg.MessageText
                            Case TipoMensaje.scgDevolucionRepuestos, TipoMensaje.scgDevolucionSuministros
                                oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                oMsg.MessageText = String.Format(My.Resources.ResourceFrameWork.Devolucion & " - " & My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOT, p_strDocEntry, p_strNoOrden, " " & Asesor)
                                oMsg.Subject = oMsg.MessageText
                        End Select

                    Else
                        oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                        oMsg.MessageText = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                        oMsg.Subject = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden

                    End If


                    m_HashTableUsuarios.Clear()

                    For intindiceUsuarios = 0 To strArregloUsuariosREP.Length - 1

                        If Not m_HashTableUsuarios.ContainsKey(Trim(strArregloUsuariosREP(intindiceUsuarios))) Then

                            m_HashTableUsuarios.Add(Trim(strArregloUsuariosREP(intindiceUsuarios)), Trim(strArregloUsuariosREP(intindiceUsuarios)))

                            oMsg.Recipients.Add()
                            oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
                            oMsg.Recipients.UserCode = Trim(strArregloUsuariosREP(intindiceUsuarios))
                            oMsg.Recipients.NameTo = Trim(strArregloUsuariosREP(intindiceUsuarios))
                            oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES

                        End If

                    Next

                    For intIndice = 0 To (strArreglo.Length) - 1

                        'verifica que el documento creado sea un draft
                        If blnDraft Then
                            'oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.DocumentoBorrador & "," & My.Resources.ResourceFrameWork.Referencia & ": " & CStr(strArreglo(intIndice)), SAPbobsCOM.BoObjectTypes.oDrafts, CStr(strArreglo(intIndice)))
                        Else
                            oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.Traslado & "," & My.Resources.ResourceFrameWork.Referencia & ": " & CStr(strArreglo(intIndice)), SAPbobsCOM.BoObjectTypes.oStockTransfer, CStr(strArreglo(intIndice)))
                        End If

                    Next

                    intResultado = oMsg.Add()
                    If (intResultado <> 0) Then
                        p_ocompany.GetLastError(intError, strError)
                        Throw New ExceptionsSBO(intError, strError)
                    End If


                End If
                If (strCodigoUsuarioSUM <> "") Then

                    'Crea el mensaje
                    If blnDraft Then

                        Select Case p_intTipoMensaje
                            Case TipoMensaje.scgPeticionSuministros
                                oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                oMsg.MessageText = String.Format(My.Resources.ResourceFrameWork.MensajeTraslado & " - " & My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOT, p_strDocEntry, p_strNoOrden)
                                oMsg.Subject = oMsg.MessageText
                            Case TipoMensaje.scgDevolucionSuministros
                                oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                oMsg.MessageText = String.Format(My.Resources.ResourceFrameWork.Devolucion & " - " & My.Resources.ResourceFrameWork.MensajeTransferenciaBorradorOT, p_strDocEntry, p_strNoOrden)
                                oMsg.Subject = oMsg.MessageText
                        End Select

                    Else
                        oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                        oMsg.MessageText = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden
                        oMsg.Subject = p_strMensaje & " " & My.Resources.ResourceFrameWork.OT & ": " & p_strNoOrden

                    End If

                    m_HashTableUsuarios.Clear()

                    For intindiceUsuarios = 0 To strArregloUsuariosSUM.Length - 1

                        If Not m_HashTableUsuarios.ContainsKey(Trim(strArregloUsuariosSUM(intindiceUsuarios))) Then

                            m_HashTableUsuarios.Add(Trim(strArregloUsuariosSUM(intindiceUsuarios)), Trim(strArregloUsuariosSUM(intindiceUsuarios)))

                            oMsg.Recipients.Add()
                            oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
                            oMsg.Recipients.UserCode = Trim(strArregloUsuariosSUM(intindiceUsuarios))
                            oMsg.Recipients.NameTo = Trim(strArregloUsuariosSUM(intindiceUsuarios))
                            oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES

                        End If


                        For intIndice = 0 To (strArreglo.Length) - 1
                            If blnDraft Then
                                ''''oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.DocumentoBorrador & "," & My.Resources.ResourceFrameWork.Referencia & ": " & CStr(strArreglo(intIndice)),SAPbobsCOM.BoObjectTypes. ) ', SAPbobsCOM.BoObjectTypes.oDrafts, CStr(strArreglo(intIndice)))
                            Else
                                'oMsg.AddDataColumn("Favor Revisar", "Traslado,  Referencia: " & CStr(strArreglo(intIndice)), SAPbobsCOM.BoObjectTypes.oStockTransfer, CStr(strArreglo(intIndice)))
                                oMsg.AddDataColumn(My.Resources.ResourceFrameWork.MensajeFavorRevisar, My.Resources.ResourceFrameWork.Traslado & "," & My.Resources.ResourceFrameWork.Referencia & ": " & CStr(strArreglo(intIndice)), SAPbobsCOM.BoObjectTypes.oStockTransfer, CStr(strArreglo(intIndice)))
                            End If
                        Next

                        ' oMsg.AddDataColumn("Favor Revisar", "Cotización1,  Referencia: " & p_intDocEntry, SAPbobsCOM.BoObjectTypes.oQuotations, CStr(p_intDocEntry))

                        intResultado = oMsg.Add()
                        If (intResultado <> 0) Then
                            p_ocompany.GetLastError(intError, strError)
                            Throw New ExceptionsSBO(intError, strError)
                            'MsgBox("Error:" + Str(lngError) + "," + strError)
                        End If
                    Next

                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub
        Private Function ObtieneCodigoBodegueroSBO() As String

            Try

                Dim strCodigo As String =  String.Empty
                Dim adpConf As New ConfiguracionDataAdapter(m_strConexion) 'Como se llama desde el addon necesita la cadena de conexion correcta
                Dim dstConf As New ConfiguracionDataSet

                adpConf.Fill(dstConf)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, mc_strEncargadoBodega, strCodigo)


                Return strCodigo

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try
            Return String.Empty
        End Function

        Overloads Function ObtieneCodigoBodegueroSBOSUM() As String

            Try

                Dim strCodigo As String =  String.Empty
                Dim adpConf As New ConfiguracionDataAdapter(m_strConexion) 'Como se llama desde el addon necesita la cadena de conexion correcta
                Dim dstConf As New ConfiguracionDataSet

                adpConf.Fill(dstConf)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, mc_strEncargadoSuministros, strCodigo)


                Return strCodigo

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try
            Return String.Empty
        End Function


        Overloads Function ObtieneCodigoBodegueroSBOSUM(ByVal DesdeDMS As Boolean) As String

            Try
                Dim adpConf As ConfiguracionDataAdapter


                Dim strCodigo As String =  String.Empty
                If DesdeDMS = True Then
                    adpConf = New ConfiguracionDataAdapter '(m_strConexion)
                Else
                    adpConf = New ConfiguracionDataAdapter(m_strConexion)
                End If

                Dim dstConf As New ConfiguracionDataSet

                adpConf.Fill(dstConf)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, mc_strEncargadoSuministros, strCodigo)


                Return strCodigo

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                Call m_cnn.Close()
            End Try
            Return String.Empty
        End Function




       

        Private Function ConsultarMensajeriaXCentroCosto(ByRef strCodCentroCosto As String, ByRef TipoEncargado As String) As String
            Try
                Dim strArregloCentroCosto As String()
                Dim intCont As Integer
                Dim strArregloEncargado As String()
                Dim strArregloEncargadoTemp As String()
                Dim blnInsertar As Boolean = False
                Dim intIndice As Integer
                Dim intIndiceTemp As Integer
                'Inicializa el DataAdapter con la conexión
                m_adpConfMensajeria = New SCGDataAccess.ConfMensajeriaDataAdapter
                'Inicializa el DataAdapter con la conexión
                m_dstConfMensajeria = New ConfMensajeriaDataSet
                '-- Crea un objeto Datarow del objeto Dataset Fase
                Dim drwConfMensajeria As ConfMensajeriaDataSet.SCGTA_TB_ConfiguracionMensajeriaRow


                strArregloCentroCosto = strCodCentroCosto.Split(",")

                Dim strEncargado As String = String.Empty
                Dim strEncargadoTemp As String = String.Empty

                Call m_adpConfMensajeria.FillConfMensajeria(m_dstConfMensajeria)
                'Call m_adpConfMensajeria.FillXCodCentroCosto(m_dstConfMensajeria, CodCentroCosto)

                For Each drwConfMensajeria In m_dstConfMensajeria.SCGTA_TB_ConfiguracionMensajeria
                    For intCont = 0 To (strArregloCentroCosto.Length) - 1
                        If strArregloCentroCosto(intCont) = drwConfMensajeria.CodCentroCosto Then
                            Select Case TipoEncargado
                                Case mc_strEncargadoRepuestos
                                    strEncargadoTemp = drwConfMensajeria.EncargadoRepuesto
                                    strEncargado = validarExisteEncargado(strEncargado, strEncargadoTemp)
                                Case mc_strEncargadoSuministros
                                    strEncargadoTemp = drwConfMensajeria.EncargadoSuministro
                                    strEncargado = validarExisteEncargado(strEncargado, strEncargadoTemp)
                                Case mc_strEncargadoServicios ' Nota servicios esta como encargadio taller
                                    strEncargadoTemp = drwConfMensajeria.EncargadoServicio
                                    strEncargado = validarExisteEncargado(strEncargado, strEncargadoTemp)
                            End Select
                        End If
                    Next
                Next

                Return strEncargado
            Catch ex As Exception
                ' ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
                Throw ex
            End Try
        End Function



        Private Function validarExisteEncargado(ByRef strEncargado As String, ByRef strEncargadoTemp As String) As String

            Try
                'Dim strArregloCentroCosto As String()
                'Dim intCont As Integer
                Dim strArregloEncargado As String()
                Dim strArregloEncargadoTemp As String()
                Dim blnInsertar As Boolean = False
                Dim intIndice As Integer
                Dim intIndiceTemp As Integer
                'Dim strEncargado As String = String.Empty
                'Dim strEncargadoTemp As String = String.Empty


                If strEncargado <> "" AndAlso strEncargadoTemp <> "" Then
                    strArregloEncargado = strEncargado.Split(",")
                    strArregloEncargadoTemp = strEncargadoTemp.Split(",")

                    For intIndiceTemp = 0 To (strArregloEncargadoTemp.Length) - 1
                        blnInsertar = False
                        For intIndice = 0 To (strArregloEncargado.Length) - 1
                            If Trim(CStr(strArregloEncargadoTemp(intIndiceTemp))) = Trim(CStr(strArregloEncargado(intIndice))) Then
                                blnInsertar = True
                            End If
                        Next

                        If blnInsertar <> True Then
                            strEncargado &= ","
                            strEncargado &= strArregloEncargadoTemp(intIndiceTemp) 'PONER MUCHO OJO A ESTE PUNTO, antes estaba strEncargadoTemp por lo  tanto me estaba duplicando todo
                        End If
                    Next

                ElseIf (strEncargado = "" AndAlso strEncargadoTemp <> "") Then
                    strEncargado &= strEncargadoTemp
                End If

                Return strEncargado
            Catch ex As Exception
                Throw ex
            End Try

        End Function
#End Region



#End Region

#Region "Comandos"

        Private Function CrearSelectCommandMensajes() As SqlClient.SqlCommand
            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSELMensajes)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    .Add(mc_strArroba & mc_strUsuario, SqlDbType.VarChar, 15)
                    .Add(mc_strArroba & mc_strCompania, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strAplicacion, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strCodSucursal, SqlDbType.VarChar, 8)
                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try
        End Function

#End Region

        'Prueba mensajería
#Region "Cargar Objetos de SAP"

        Private Function CargarTransferenciaStock(ByVal p_StockTransfer As Integer, ByVal p_ocompany As SAPbobsCOM.Company) As SAPbobsCOM.StockTransfer

            Try
                m_oStockTransfer = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer)

                If m_oStockTransfer.GetByKey(p_StockTransfer) Then

                    Return m_oStockTransfer

                End If

            Catch ex As Exception
                'Call Utilitarios.ManejadorErrores(ex, SBO_Application)
                Throw ex

            End Try
            Return Nothing
        End Function

        'Private Function CargarTransferenciaStockDraft(ByVal p_StockTransfer As Integer, ByVal p_ocompany As SAPbobsCOM.Company) As SAPbobsCOM.StockTransfer

        'Try
        '    'Dim strCadenaConexion As String = String.Empty
        '    'Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, strCadenaConexion)

        '    Dim strConectionString As String = DAConexion.ConnectionString

        '    Dim objUtilitarios As New SCGDataAccess.Utilitarios(strConectionString)
        '    Dim dstCotizacionLineas As Cotizacion_LineasDataset
        '    Dim drwCotizacionLinea As Cotizacion_LineasDataset.Cotizacion_LineasRow

        '    Dim intContLineas As Integer
        '    Dim decCantidadAnterior As Integer = 0

        '    dstCotizacionLineas = objUtilitarios.ObtenerItemsCotizaRepetidosByItemCode(p_intDocEntry, p_intLineNum, p_strItemCode)

        '    For intContLineas = 0 To dstCotizacionLineas.Cotizacion_Lineas.Rows.Count - 1

        '        drwCotizacionLinea = dstCotizacionLineas.Cotizacion_Lineas.Rows(intContLineas)

        '        If drwCotizacionLinea.LineNum < p_intLineNum Then

        '            If drwCotizacionLinea.U_SCGD_Aprobado = 1 AndAlso (drwCotizacionLinea.U_SCGD_Traslad = 0 Or _
        '                    drwCotizacionLinea.U_SCGD_Traslad = 3) Then

        '                decCantidadAnterior += drwCotizacionLinea.Quantity

        '            End If

        '        Else

        '            Exit For

        '        End If

        '    Next

        'Catch ex As Exception
        '    'Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        '    Throw ex

        'End Try
        'Return Nothing
        'End Function


        Private Function RecorreLineasDocumentoTransferencia(ByVal p_DocEntry As Integer, ByRef strCentroCosto As String, ByVal p_ocompany As SAPbobsCOM.Company, ByVal blnDraft As Boolean) As String

            Try
                Dim objStockTransfer As SAPbobsCOM.StockTransfer
                Dim objStockTransferLines As SAPbobsCOM.StockTransfer_Lines


                'Dim strCentroCosto As String = ""
                Dim strTempCentroCosto As String
                Dim strArregloCentroCosto As String()
                Dim intIndice As Integer
                Dim bolInsertar As Boolean = False

                objStockTransfer = CargarTransferenciaStock(CInt(p_DocEntry), p_ocompany)
                objStockTransferLines = objStockTransfer.Lines
                For i As Integer = 0 To objStockTransferLines.Count - 1
                    objStockTransferLines.SetCurrentLine(i)
                    With objStockTransferLines

                        strTempCentroCosto = DevuelveValorItem(.ItemCode, mc_strCodCentroCosto, p_ocompany)

                        If strCentroCosto <> "" AndAlso strTempCentroCosto <> "" Then
                            strArregloCentroCosto = strCentroCosto.Split(",")

                            For intIndice = 0 To (strArregloCentroCosto.Length) - 1
                                If CStr(strArregloCentroCosto(intIndice)) = strTempCentroCosto Then
                                    bolInsertar = True
                                End If
                            Next

                            If bolInsertar <> True Then
                                strCentroCosto &= ","
                                strCentroCosto &= strTempCentroCosto
                            End If

                        ElseIf (strCentroCosto = "" AndAlso strTempCentroCosto <> "") Then
                            strCentroCosto &= strTempCentroCosto
                        End If

                    End With
                Next

                Return strCentroCosto
            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function DevuelveValorItem(ByVal strItemcode As String, _
                                       ByVal strUDfName As String, ByVal p_ocompany As SAPbobsCOM.Company) As String

            Dim oItemArticulo As SAPbobsCOM.IItems
            Dim valorUDF As String

            oItemArticulo = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            oItemArticulo.GetByKey(strItemcode)
            valorUDF = oItemArticulo.UserFields.Fields.Item(strUDfName).Value

            Return valorUDF

        End Function


        Private Function RecorreLineasCotizacion(ByVal p_DocEntryCotizacion As Integer, ByRef strCentroCosto As String, ByVal p_ocompany As SAPbobsCOM.Company) As String
            Try
                Dim m_oCotizacion As SAPbobsCOM.Documents
                Dim m_oLineasCotizacion As SAPbobsCOM.Document_Lines
                Dim strItemCode As String = String.Empty

                'm_oCotizacion = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

                'If m_oCotizacion.GetByKey(p_DocEntryCotizacion) Then

                '    m_oLineasCotizacion = m_oCotizacion.Lines

                '    For indice As Integer = 0 To m_oLineasCotizacion.Count - 1

                '        m_oLineasCotizacion.SetCurrentLine(indice)
                '        strItemCode = m_oLineasCotizacion.ItemCode(indice)
                '        'm_oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 2
                '    Next

                'End If

                Dim strTempCentroCosto As String
                Dim strArregloCentroCosto As String()
                Dim intIndice As Integer
                Dim bolInsertar As Boolean = False


                m_oCotizacion = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

                If m_oCotizacion.GetByKey(p_DocEntryCotizacion) Then
                    m_oLineasCotizacion = m_oCotizacion.Lines
                    'objStockTransfer = CargarTransferenciaStock(CInt(p_DocEntry), p_ocompany)
                    'objStockTransferLines = objStockTransfer.Lines
                    For i As Integer = 0 To m_oLineasCotizacion.Count - 1
                        m_oLineasCotizacion.SetCurrentLine(i)
                        With m_oLineasCotizacion

                            strTempCentroCosto = DevuelveValorItem(.ItemCode, mc_strCodCentroCosto, p_ocompany)

                            If strCentroCosto <> "" AndAlso strTempCentroCosto <> "" Then
                                strArregloCentroCosto = strCentroCosto.Split(",")

                                For intIndice = 0 To (strArregloCentroCosto.Length) - 1
                                    If CStr(strArregloCentroCosto(intIndice)) = strTempCentroCosto Then
                                        bolInsertar = True
                                    End If
                                Next

                                If bolInsertar <> True Then
                                    strCentroCosto &= ","
                                    strCentroCosto &= strTempCentroCosto
                                End If

                            ElseIf (strCentroCosto = "" AndAlso strTempCentroCosto <> "") Then
                                strCentroCosto &= strTempCentroCosto
                            End If

                        End With
                    Next


                End If

                Return strCentroCosto

            Catch ex As Exception
                Throw ex
            End Try
        End Function


#End Region
    End Class
End Namespace
