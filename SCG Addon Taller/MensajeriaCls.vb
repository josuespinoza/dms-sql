Imports System.Collections.Generic
Imports System.Linq
Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon
Imports DMS_Connector.Business_Logic.DataContract.Configuracion.Mensajeria
'Imports DMSOneFramework.SCGBusinessLogic

Public Class MensajeriaCls

#Region "Declaraciones"

    Private SBO_Application As SAPbouiCOM.Application
    Private m_adpMensajeria As MensajeriaSBOTallerDataAdapter
    Private m_ocompany As SAPbobsCOM.Company

    Private Const mc_strUsaMensajeriaXCentroCosto As String = "UsaMensajeriaXCentroCosto"



    Public Enum RecibeMensaje
        EncargadoTaller = 0
        Bodeguero = 1
        Asesor = 2
        EncargadoRepuestos = 3
    End Enum

    Public Enum TipoMensaje
        scgPeticionRepuestos = 1
        scgPeticionSuministros = 2
        scgDevolucionRepuestos = 3
        scgDevolucionSuministros = 4
    End Enum

#End Region

#Region "Constructor"

    Public Sub New(ByVal p_SBO_Application As SAPbouiCOM.Application, ByVal p_ocompany As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Application
        m_ocompany = p_ocompany


    End Sub

#End Region

    'Comentario Prueba CheckIn
    Public Sub CreaMensajeSBO_DMS(ByVal p_strMensaje As String, ByVal p_strOT As String _
                                , ByVal p_intNoCotizacion As Integer, ByVal p_destinatario As RecibeMensaje, ByVal p_CodEmpleado As Integer _
                                , ByVal p_strNoVisita As String _
                                , Optional ByVal p_strIdSucursal As String = "")

        Try
            Dim strCadenaConexion As String = String.Empty
            If Not String.IsNullOrEmpty(p_strIdSucursal) Then
                Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, p_strIdSucursal, strCadenaConexion)
            Else
                Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, strCadenaConexion)
            End If


            'Verifico el valor de la propiedad UsaMensajeriaXCentroCosto, para saber si usa la mensajeria por centro de costo
            Dim adpConf As New ConfiguracionDataAdapter(strCadenaConexion)
            Dim dstConf As New ConfiguracionDataSet
            Dim objUtilitariosCls As New Utilitarios
            Dim blnUsaMensajeriaXCentroCosto As Boolean = False

            adpConf.Fill(dstConf)
            m_adpMensajeria = New MensajeriaSBOTallerDataAdapter(strCadenaConexion)

            If objUtilitariosCls.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, mc_strUsaMensajeriaXCentroCosto, "") Then
                blnUsaMensajeriaXCentroCosto = True
            Else
                blnUsaMensajeriaXCentroCosto = False
            End If


            If blnUsaMensajeriaXCentroCosto = True Then
                m_adpMensajeria.InsertarMensajeSBO_DMSXCentroCosto(m_ocompany, p_strMensaje, p_strOT, p_intNoCotizacion, p_destinatario, p_CodEmpleado, p_strNoVisita)
            Else
                m_adpMensajeria.InsertarMensajeSBO_DMS(p_strMensaje, p_strOT, p_intNoCotizacion, p_destinatario, p_CodEmpleado, p_strNoVisita)
            End If




        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub CreaMensajeSBO_SBOCotizacion(ByVal p_strMensaje As String,
                                            ByVal p_strDocEntry As String,
                                            ByVal p_strNoOrden As String,
                                            ByVal p_intTipoMensaje As MensajeriaSBOTallerDataAdapter.TipoMensaje,
                                            ByRef p_blnDraft As Boolean,
                                            ByVal p_oForm As SAPbouiCOM.Form,
                                            ByVal p_strDtConsulta As String,
                                            Optional ByVal p_strIdSucursal As String = "",
                                            Optional ByVal strRolCode As String = "", _
                                            Optional ByVal p_bNewUpdate As Boolean = False,
                                            Optional ByVal p_blnConf_TallerEnSAP As Boolean = False,
                                            Optional ByVal Asesor As String = "")

        Try
            Dim strCadenaConexion As String = String.Empty
            Dim dtConsulta As SAPbouiCOM.DataTable
            Dim strConsultaMensajeriaCC As String = " select U_MsjXCC from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}' "
            Dim strConsultaMensajeriaCCForm As String = String.Empty
            Dim strMensajeriaXCC As String = String.Empty
            Dim objUtilitariosCls As New Utilitarios
            Dim blnUsaMensajeriaXCentroCosto As Boolean = False

            If Not p_blnConf_TallerEnSAP Then
                If Not String.IsNullOrEmpty(p_strIdSucursal) Then
                    Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, p_strIdSucursal, strCadenaConexion)
                Else
                    Utilitarios.DevuelveCadenaConexionBDTaller(SBO_Application, strCadenaConexion)
                End If
                'Verifico el valor de la propiedad UsaMensajeriaXCentroCosto, para saber si usa la mensajeria por centro de costo
                Dim adpConf As New ConfiguracionDataAdapter(strCadenaConexion)
                Dim dstConf As New ConfiguracionDataSet


                adpConf.Fill(dstConf)
                m_adpMensajeria = New MensajeriaSBOTallerDataAdapter(strCadenaConexion)

                dtConsulta = p_oForm.DataSources.DataTables.Item("dtConsulta")

                strConsultaMensajeriaCCForm = String.Format(strConsultaMensajeriaCC, p_strIdSucursal)
                dtConsulta.ExecuteQuery(strConsultaMensajeriaCCForm)
                strMensajeriaXCC = dtConsulta.GetValue(0, 0)

                If Not String.IsNullOrEmpty(strMensajeriaXCC) Then
                    If strMensajeriaXCC = "Y" Then
                        blnUsaMensajeriaXCentroCosto = True
                    Else
                        blnUsaMensajeriaXCentroCosto = False
                    End If
                Else
                    blnUsaMensajeriaXCentroCosto = False
                End If
            End If

            If blnUsaMensajeriaXCentroCosto = True Then
                m_adpMensajeria.CreaMensajeSBO_SBO_CotizacionXCentroCosto(p_strMensaje, p_strDocEntry, m_ocompany, p_strNoOrden, p_intTipoMensaje, p_blnDraft)
            Else
                If p_blnConf_TallerEnSAP Then
                    If Not String.IsNullOrEmpty(strRolCode) AndAlso strRolCode <> "-1" Then 'strRolCode = Utilitarios.RolesMensajeria.EncargadoProduccion.ToString().Trim() Then
                        'CreaMensajeSBO(p_strMensaje, p_strDocEntry, m_ocompany, p_strNoOrden, p_blnDraft, strRolCode, p_strIdSucursal, p_oForm, "dtConsulta", p_bNewUpdate, Asesor)
                        CreaMensajeSBO(p_strMensaje, p_strDocEntry, m_ocompany, p_strNoOrden, p_blnDraft, strRolCode, p_strIdSucursal, p_bNewUpdate, Asesor)
                    End If
                Else
                    m_adpMensajeria.CreaMensajeSBO_SBO_Cotizacion(p_strMensaje, p_strDocEntry, m_ocompany, p_strNoOrden, p_intTipoMensaje, p_blnDraft, Asesor)
                End If
            End If


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    ''' <summary>
    ''' Envio de Mensajería En SAP
    ''' </summary>
    ''' <param name="p_strMensaje">Mensaje a enviar</param>
    ''' <param name="p_strDocEntry">DocEntry del documento que se creó o modificó</param>
    ''' <param name="p_ocompany">Company</param>
    ''' <param name="p_strNoOrden">Numero de OT</param>
    ''' <param name="blnDraft">Indica si es docuemtto Draft</param>
    ''' <param name="strRolCode"> Id de rol de mensajería</param>
    ''' <param name="strIdSuc">Id sucursal donde se produjo el movimiento que se va a notificar</param>
    ''' <param name="p_oForm">Formulario de Origen</param>
    ''' <param name="p_strLocalDT">Nombre de Data Table Local</param>
    ''' <param name="p_bNewUpdate">Indica si es un documento nuevo</param>
    Public Sub CreaMensajeSBO(ByVal p_strMensaje As String, ByVal p_strDocEntry As String, ByVal p_ocompany As SAPbobsCOM.Company, _
                              ByVal p_strNoOrden As String, _
                              ByVal blnDraft As Boolean, ByVal strRolCode As String, ByVal strIdSuc As String, _
                              Optional ByVal p_bNewUpdate As Boolean = False, Optional ByVal Asesor As String = "")
        Dim oMsg As SAPbobsCOM.Messages
        Dim intResultado As Integer
        Dim strError As String = String.Empty
        Dim intError As Integer
        Dim intindiceUsuarios As Integer
        Dim linea As Mensajeria_Lineas
        Dim lstLineas As List(Of Mensajeria_Lineas)

        Try
            If DMS_Connector.Configuracion.ConfMensajeria.Any(Function(x) x.U_IdSuc = strIdSuc AndAlso x.U_IdRol = strRolCode) Then
                lstLineas = DMS_Connector.Configuracion.ConfMensajeria.First(Function(x) x.U_IdSuc = strIdSuc AndAlso x.U_IdRol = strRolCode).Mensajeria_Lineas

                If lstLineas.Count >= 1 Then
                    If Not String.IsNullOrEmpty(lstLineas(0).U_Usr_UsrName) Then
                        Select Case strRolCode
                            Case Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoProduccion)
                                'Crea el mensaje
                                If p_bNewUpdate Then
                                    oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                    oMsg.MessageText = p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
                                    oMsg.Subject = oMsg.MessageText 'p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
                                Else
                                    If blnDraft Then
                                        oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                        oMsg.MessageText = String.Format(My.Resources.Resource.MensajeTransferenciaBorradorOTSAP, p_strDocEntry, p_strNoOrden)
                                        oMsg.Subject = oMsg.MessageText
                                    Else
                                        oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                        oMsg.MessageText = p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
                                        oMsg.Subject = p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
                                    End If
                                End If

                                For intindiceUsuarios = 0 To lstLineas.Count - 1
                                    linea = lstLineas(intindiceUsuarios)
                                    oMsg.Recipients.Add()
                                    oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
                                    oMsg.Recipients.UserCode = linea.U_Usr_UsrName.Trim()
                                    oMsg.Recipients.NameTo = linea.U_Usr_UsrName.Trim()
                                    oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
                                Next

                                'verifica que el documento creado sea un draft
                                If Not p_bNewUpdate Then
                                    If Not blnDraft AndAlso Not String.IsNullOrEmpty(p_strDocEntry) Then
                                        oMsg.AddDataColumn(My.Resources.Resource.MensajeFavorRevisar, My.Resources.Resource.Traslado & "," & My.Resources.Resource.Referencia & ": " & CStr(p_strDocEntry), SAPbobsCOM.BoObjectTypes.oStockTransfer, CStr(p_strDocEntry))
                                    End If
                                End If

                                intResultado = oMsg.Add()
                                If (intResultado <> 0) Then
                                    p_ocompany.GetLastError(intError, strError)
                                    Throw New ExceptionsSBO(intError, strError)
                                End If
                            Case Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoRepuestos), Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoSuministros)
                                'Crea el mensaje
                                If blnDraft Then
                                    oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                    oMsg.MessageText = String.Format(My.Resources.Resource.MensajeTransferenciaBorradorOTSAP + " " + Asesor, p_strDocEntry, p_strNoOrden)
                                    oMsg.Subject = oMsg.MessageText
                                Else
                                    oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                    oMsg.MessageText = p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
                                    oMsg.Subject = p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
                                End If
                                For intindiceUsuarios = 0 To lstLineas.Count - 1
                                    linea = lstLineas(intindiceUsuarios)
                                    oMsg.Recipients.Add()
                                    oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
                                    oMsg.Recipients.UserCode = linea.U_Usr_UsrName.Trim()
                                    oMsg.Recipients.NameTo = linea.U_Usr_UsrName.Trim()
                                    oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES

                                Next
                                'verifica que el documento creado sea un draft
                                If Not p_bNewUpdate Then
                                    If Not blnDraft AndAlso Not String.IsNullOrEmpty(p_strDocEntry) Then
                                        oMsg.AddDataColumn(My.Resources.Resource.MensajeFavorRevisar, My.Resources.Resource.Traslado & "," & My.Resources.Resource.Referencia & ": " & p_strDocEntry, SAPbobsCOM.BoObjectTypes.oStockTransfer, p_strDocEntry)
                                    End If
                                End If

                                intResultado = oMsg.Add()
                                If (intResultado <> 0) Then
                                    p_ocompany.GetLastError(intError, strError)
                                    Throw New ExceptionsSBO(intError, strError)
                                End If
                            Case Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoSOE)
                                'Crea el mensaje
                                oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                oMsg.MessageText = p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
                                oMsg.Subject = oMsg.MessageText

                                For intindiceUsuarios = 0 To lstLineas.Count - 1
                                    linea = lstLineas(intindiceUsuarios)
                                    oMsg.Recipients.Add()
                                    oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
                                    oMsg.Recipients.UserCode = linea.U_Usr_UsrName.Trim()
                                    oMsg.Recipients.NameTo = linea.U_Usr_UsrName.Trim()
                                    oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
                                Next

                                intResultado = oMsg.Add()
                                If (intResultado <> 0) Then
                                    p_ocompany.GetLastError(intError, strError)
                                    'Throw New ExceptionsSBO(intError, strError)
                                End If
                            Case Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoCompras)
                                'Crea el mensaje
                                oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                                oMsg.MessageText = p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
                                oMsg.Subject = oMsg.MessageText

                                For intindiceUsuarios = 0 To lstLineas.Count - 1
                                    linea = lstLineas(intindiceUsuarios)
                                    oMsg.Recipients.Add()
                                    oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
                                    oMsg.Recipients.UserCode = linea.U_Usr_UsrName.Trim()
                                    oMsg.Recipients.NameTo = linea.U_Usr_UsrName.Trim()
                                    oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
                                Next

                                intResultado = oMsg.Add()
                                If (intResultado <> 0) Then
                                    p_ocompany.GetLastError(intError, strError)
                                End If
                        End Select
                    Else
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If
            Else
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ' ''' <summary>
    ' ''' Envio de Mensajería En SAP
    ' ''' </summary>
    ' ''' <param name="p_strMensaje">Mensaje a enviar</param>
    ' ''' <param name="p_strDocEntry">DocEntry del documento que se creó o modificó</param>
    ' ''' <param name="p_ocompany">Company</param>
    ' ''' <param name="p_strNoOrden">Numero de OT</param>
    ' ''' <param name="blnDraft">Indica si es docuemtto Draft</param>
    ' ''' <param name="strRolCode"> Id de rol de mensajería</param>
    ' ''' <param name="strIdSuc">Id sucursal donde se produjo el movimiento que se va a notificar</param>
    ' ''' <param name="p_oForm">Formulario de Origen</param>
    ' ''' <param name="p_strLocalDT">Nombre de Data Table Local</param>
    ' ''' <param name="p_bNewUpdate">Indica si es un documento nuevo</param>
    'Public Sub CreaMensajeSBO(ByVal p_strMensaje As String, ByVal p_strDocEntry As String, ByVal p_ocompany As SAPbobsCOM.Company, _
    '                          ByVal p_strNoOrden As String, _
    '                          ByVal blnDraft As Boolean, ByVal strRolCode As String, ByVal strIdSuc As String, ByVal p_oForm As SAPbouiCOM.Form, _
    '                          ByVal p_strLocalDT As String, Optional ByVal p_bNewUpdate As Boolean = False, Optional ByVal Asesor As String = "")
    '    'Crea mensaje en SAP para el bodeguero sobre creacion de un documento de traslado
    '    Try
    '        Dim oMsg As SAPbobsCOM.Messages
    '        Dim dtConsulta As SAPbouiCOM.DataTable
    '        Dim intResultado As Integer
    '        Dim strError As String = String.Empty
    '        Dim intError As Integer
    '        Dim intindiceUsuarios As Integer
    '        Dim query As String

    '        query = "select l.U_EmpCode code, l.U_Usr_Name name, l.U_Usr_UsrName userId " & _
    '                "from [@SCGD_CONF_MSJ] m " & _
    '                    "inner join  [@SCGD_CONF_MSJLN] l on m.DocEntry=l.DocEntry " & _
    '                "where m.U_IdRol = '{0}' and m.U_IdSuc = '{1}' "

    '        query = String.Format(query, strRolCode, strIdSuc)
    '        If String.IsNullOrEmpty(p_strLocalDT) Then
    '            dtConsulta = p_oForm.DataSources.DataTables.Item("dtConsulta")
    '        Else
    '            dtConsulta = p_oForm.DataSources.DataTables.Item(p_strLocalDT)
    '        End If

    '        dtConsulta.ExecuteQuery(query)

    '        If dtConsulta.Rows.Count >= 1 Then
    '            If Not String.IsNullOrEmpty(dtConsulta.GetValue("userId", 0).ToString) Then
    '                Select Case strRolCode
    '                    Case Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoProduccion)
    '                        'Crea el mensaje
    '                        If p_bNewUpdate Then
    '                            oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
    '                            oMsg.MessageText = p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
    '                            oMsg.Subject = oMsg.MessageText 'p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
    '                        Else
    '                            If blnDraft Then
    '                                oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
    '                                oMsg.MessageText = String.Format(My.Resources.Resource.MensajeTransferenciaBorradorOTSAP, p_strDocEntry, p_strNoOrden)
    '                                oMsg.Subject = oMsg.MessageText
    '                            Else
    '                                oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
    '                                oMsg.MessageText = p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
    '                                oMsg.Subject = p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
    '                            End If
    '                        End If

    '                        For intindiceUsuarios = 0 To dtConsulta.Rows.Count - 1
    '                            oMsg.Recipients.Add()
    '                            oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
    '                            oMsg.Recipients.UserCode = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
    '                            oMsg.Recipients.NameTo = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
    '                            oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
    '                        Next

    '                        'verifica que el documento creado sea un draft
    '                        If Not p_bNewUpdate Then
    '                            If Not blnDraft AndAlso Not String.IsNullOrEmpty(p_strDocEntry) Then
    '                                oMsg.AddDataColumn(My.Resources.Resource.MensajeFavorRevisar, My.Resources.Resource.Traslado & "," & My.Resources.Resource.Referencia & ": " & CStr(p_strDocEntry), SAPbobsCOM.BoObjectTypes.oStockTransfer, CStr(p_strDocEntry))
    '                            End If
    '                        End If

    '                        intResultado = oMsg.Add()
    '                        If (intResultado <> 0) Then
    '                            p_ocompany.GetLastError(intError, strError)
    '                            Throw New ExceptionsSBO(intError, strError)
    '                        End If
    '                    Case Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoRepuestos), Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoSuministros)
    '                        'Crea el mensaje
    '                        If blnDraft Then
    '                            oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
    '                            oMsg.MessageText = String.Format(My.Resources.Resource.MensajeTransferenciaBorradorOTSAP + " " + Asesor, p_strDocEntry, p_strNoOrden)
    '                            oMsg.Subject = oMsg.MessageText
    '                        Else
    '                            oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
    '                            oMsg.MessageText = p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
    '                            oMsg.Subject = p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
    '                        End If

    '                        For intindiceUsuarios = 0 To dtConsulta.Rows.Count - 1
    '                            oMsg.Recipients.Add()
    '                            oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
    '                            oMsg.Recipients.UserCode = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
    '                            oMsg.Recipients.NameTo = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
    '                            oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES

    '                        Next
    '                        'verifica que el documento creado sea un draft
    '                        If Not p_bNewUpdate Then
    '                            If Not blnDraft AndAlso Not String.IsNullOrEmpty(p_strDocEntry) Then
    '                                oMsg.AddDataColumn(My.Resources.Resource.MensajeFavorRevisar, My.Resources.Resource.Traslado & "," & My.Resources.Resource.Referencia & ": " & p_strDocEntry, SAPbobsCOM.BoObjectTypes.oStockTransfer, p_strDocEntry)
    '                            End If
    '                        End If

    '                        intResultado = oMsg.Add()
    '                        If (intResultado <> 0) Then
    '                            p_ocompany.GetLastError(intError, strError)
    '                            Throw New ExceptionsSBO(intError, strError)
    '                        End If
    '                    Case Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoSOE)
    '                        'Crea el mensaje
    '                        oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
    '                        oMsg.MessageText = p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
    '                        oMsg.Subject = oMsg.MessageText

    '                        For intindiceUsuarios = 0 To dtConsulta.Rows.Count - 1
    '                            oMsg.Recipients.Add()
    '                            oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
    '                            oMsg.Recipients.UserCode = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
    '                            oMsg.Recipients.NameTo = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
    '                            oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
    '                        Next

    '                        intResultado = oMsg.Add()
    '                        If (intResultado <> 0) Then
    '                            p_ocompany.GetLastError(intError, strError)
    '                            'Throw New ExceptionsSBO(intError, strError)
    '                        End If
    '                    Case Convert.ToUInt32(Utilitarios.RolesMensajeria.EncargadoCompras)
    '                        'Crea el mensaje
    '                        oMsg = p_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
    '                        oMsg.MessageText = p_strMensaje & " " & My.Resources.Resource.OT & ": " & p_strNoOrden
    '                        oMsg.Subject = oMsg.MessageText

    '                        For intindiceUsuarios = 0 To dtConsulta.Rows.Count - 1
    '                            oMsg.Recipients.Add()
    '                            oMsg.Recipients.SetCurrentLine(intindiceUsuarios)
    '                            oMsg.Recipients.UserCode = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
    '                            oMsg.Recipients.NameTo = dtConsulta.GetValue("userId", intindiceUsuarios).ToString().Trim()
    '                            oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
    '                        Next

    '                        intResultado = oMsg.Add()
    '                        If (intResultado <> 0) Then
    '                            p_ocompany.GetLastError(intError, strError)
    '                        End If
    '                End Select
    '            Else
    '                Exit Sub
    '            End If
    '        Else
    '            Exit Sub
    '        End If

    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Sub


End Class
