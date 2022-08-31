'Agregado 04/11/2010: Clase para manejar funciones en salidas de mercancía
Imports SAPbobsCOM
Imports SAPbouiCOM

Public Class SalidasMercancia

    Private SBO_Application As SAPbouiCOM.Application
    Private SBO_Company As SAPbobsCOM.Company

    Private m_oCVenta As ContratoVentasCls

    Public strNumeroSalida As String = String.Empty
    Dim strNumeroCV As String
    Dim m_strNumeroOT As String = String.Empty
    Dim m_strEsDraft As String = String.Empty

    Public Sub New(ByVal p_SBO_Application As SAPbouiCOM.Application, ByVal m_oCompany As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Application
        SBO_Company = m_oCompany

    End Sub

    Public Sub ManejoEventoLoad(ByRef pVal As SAPbouiCOM.ItemEvent)
        Dim oForm As SAPbouiCOM.Form
        Try
            oForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

            Call AgregaComponentes(oForm)

            Call LigaFormulario(oForm)

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Sub

    Public Sub ManejarEstado(ByRef oForm As SAPbouiCOM.Form)

        Try

            oForm.Items.Item("SCGD_txtCV").Enabled = False

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Private Sub LigaFormulario(ByVal oForm As SAPbouiCOM.Form)

        Dim oitem As SAPbouiCOM.Item
        Dim oedit As SAPbouiCOM.EditText

        Try

            oitem = oForm.Items.Item("SCGD_txtCV")
            oedit = DirectCast(oitem.Specific, SAPbouiCOM.EditText)
            oedit.DataBind.SetBound(True, "OIGE", "U_SCGD_NoContrato")

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Private Sub AgregaComponentes(ByVal oForm As SAPbouiCOM.Form)

        Dim oitem As SAPbouiCOM.Item
        Dim oLabel As SAPbouiCOM.StaticText
        Dim oEdit As SAPbouiCOM.EditText
        Dim oButtonLink As SAPbouiCOM.LinkedButton

        Try
            oitem = oForm.Items.Add("SCGD_txtCV", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oitem.Left = 420
            oitem.Top = 53
            oitem.Width = 82
            oitem.Height = 14
            oitem.Enabled = False
            oEdit = oitem.Specific

            oitem = oForm.Items.Add("SCGD_lbCV", SAPbouiCOM.BoFormItemTypes.it_STATIC)
            oitem.Left = 308
            oitem.Top = 53
            oitem.Width = 101
            oitem.Height = 14
            oitem.LinkTo = "SCGD_txtCV"
            oLabel = oitem.Specific
            oLabel.Caption = My.Resources.Resource.ConVenta

            oitem = oForm.Items.Add("SCGD_lkCV", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON)
            oitem.Left = 400
            oitem.Top = 56
            oitem.Width = 20
            oitem.Height = 9
            oitem.LinkTo = "SCGD_txtCV"
            oButtonLink = oitem.Specific
            'oButtonLink.LinkedObjectType = "SCG_CVT"

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent, _
                                        ByRef BubbleEvent As Boolean)
        Dim oForm As SAPbouiCOM.Form
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Try
            m_oCVenta = New ContratoVentasCls(SBO_Company, SBO_Application)
            oForm = SBO_Application.Forms.GetForm(pval.FormTypeEx, pval.FormTypeCount)

            If oForm IsNot Nothing Then
                If pval.BeforeAction Then
                    Select Case pval.FormMode
                        Case SAPbouiCOM.BoFormMode.fm_ADD_MODE
                            If pval.ItemUID = "1" Then
                                If Not String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("OIGE").GetValue("U_SCGD_NoContrato", 0)) Then
                                    strNumeroCV = oForm.DataSources.DBDataSources.Item("OIGE").GetValue("U_SCGD_NoContrato", 0)
                                    strNumeroCV = strNumeroCV.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("OIGE").GetValue("U_SCGD_Numero_OT", 0)) Then
                                    m_strNumeroOT = oForm.DataSources.DBDataSources.Item("OIGE").GetValue("U_SCGD_Numero_OT", 0)
                                    m_strNumeroOT = m_strNumeroOT.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oForm.DataSources.DBDataSources.Item("OIGE").GetValue("U_SCGD_Draft", 0)) Then
                                    m_strEsDraft = oForm.DataSources.DBDataSources.Item("OIGE").GetValue("U_SCGD_Draft", 0)
                                    m_strEsDraft = m_strEsDraft.Trim()
                                End If
                            End If
                        Case SAPbouiCOM.BoFormMode.fm_UPDATE_MODE

                    End Select
                    If pval.ItemUID = "SCGD_lkCV" Then
                        m_oCVenta.m_blnCargoManejarEstados = True
                    End If
                ElseIf pval.ActionSuccess Then
                    Select Case pval.FormMode
                        Case SAPbouiCOM.BoFormMode.fm_ADD_MODE
                            If pval.ItemUID = "1" Then
                                'Valida si existe una factura interna que se deba ligar
                                If m_strEsDraft = "Y" Then
                                    ManejaDraftFacturaInterna(oForm, strNumeroSalida, m_strNumeroOT)
                                End If

                                If Not String.IsNullOrEmpty(strNumeroCV) Then
                                    oCompanyService = SBO_Company.GetCompanyService()
                                    oGeneralService = oCompanyService.GetGeneralService(ContratoVentasCls.NombreUDO)
                                    oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                                    oGeneralParams.SetProperty("DocEntry", strNumeroCV)
                                    oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                                    oGeneralData.SetProperty("U_SCGD_NoSalida", strNumeroSalida)
                                    oGeneralService.Update(oGeneralData)
                                End If
                            End If
                        Case SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                            'Implementar aquí operaciones en modo actualizar
                        Case SAPbouiCOM.BoFormMode.fm_OK_MODE

                    End Select
                    If pval.ItemUID = "SCGD_lkCV" Then
                        strNumeroCV = oForm.DataSources.DBDataSources.Item("OIGE").GetValue("U_SCGD_NoContrato", 0)
                        strNumeroCV = strNumeroCV.Trim()
                        If Not ValidarSiFormularioAbierto(ContratoVentasCls.FormType, False) Then
                            Call m_oCVenta.DibujarFormularioContratoVentas("", False)
                            Call m_oCVenta.CargarContrato(strNumeroCV, ContratoVentasCls.FormType)
                            Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item(ContratoVentasCls.FormType), False)
                        Else
                            SBO_Application.Forms.Item(ContratoVentasCls.FormType).Select()
                        End If
                        m_oCVenta.m_blnCargoManejarEstados = False
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Function ValidarSiFormularioAbierto(ByVal strFormUID As String, _
                                                ByVal blnselectIfOpen As Boolean) As Boolean

        Dim intI As Integer = 0
        Dim blnFound As Boolean = False
        Dim frmForma As SAPbouiCOM.Form

        Try

            Dim a As Integer = SBO_Application.Forms.Count

            While (Not blnFound AndAlso intI < SBO_Application.Forms.Count)

                frmForma = SBO_Application.Forms.Item(intI)

                If frmForma.UniqueID = strFormUID Then
                    blnFound = True
                    If (blnselectIfOpen) Then
                        If Not (frmForma.Selected) Then
                            SBO_Application.Forms.Item(strFormUID).Select()
                        End If
                    End If
                Else

                    intI += 1
                End If

            End While

            If (blnFound) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try      

    End Function


    Private Sub ManejaDraftFacturaInterna(ByRef p_oForm As SAPbouiCOM.Form, ByRef p_strNumeroSalida As String, ByRef p_strNumeroOT As String)
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Dim strDocEntryFI As String = String.Empty
        Try
            If Not String.IsNullOrEmpty(p_strNumeroOT) Then
                strDocEntryFI = ValidaFacturaInterna(p_strNumeroOT)
                If Not String.IsNullOrEmpty(strDocEntryFI) Then
                    oCompanyService = SBO_Company.GetCompanyService()
                    oGeneralService = oCompanyService.GetGeneralService("SCGD_FAC_INT")
                    oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                    oGeneralParams.SetProperty("DocEntry", strDocEntryFI)
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                    oGeneralData.SetProperty("U_No_Sal", p_strNumeroSalida)
                    oGeneralService.Update(oGeneralData)
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Function ValidaFacturaInterna(ByRef p_strNumeroOT As String) As String
        Dim oForm As SAPbouiCOM.Form
        Dim creationPackage As SAPbouiCOM.FormCreationParams
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim dsFacturaInterna As DBDataSource
        Try
            If Not String.IsNullOrEmpty(p_strNumeroOT) Then
                creationPackage = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
                creationPackage.FormType = "FacturaInterna"
                creationPackage.ObjectType = ""

                oForm = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(creationPackage)
                oForm.DataSources.DBDataSources.Add("@SCGD_FACTURAINTERNA")
                dsFacturaInterna = oForm.DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA")

                oConditions = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                oCondition = oConditions.Add
                oCondition.Alias = "U_No_OT"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = p_strNumeroOT

                dsFacturaInterna.Query(oConditions)

                If dsFacturaInterna.Size > 0 Then
                    If Not String.IsNullOrEmpty(dsFacturaInterna.GetValue("DocEntry", 0)) Then
                        Return dsFacturaInterna.GetValue("DocEntry", 0).ToString.Trim()
                    End If
                End If
            End If
            Return String.Empty
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return String.Empty
        End Try
    End Function

End Class
