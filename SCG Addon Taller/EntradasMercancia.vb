'Agregado 13/12/2010: Clase para manejar funciones en entradas de mercancía
Imports SAPbobsCOM
Imports SAPbouiCOM

Public Class EntradasMercancia

    Private SBO_Application As SAPbouiCOM.Application
    Private SBO_Company As SAPbobsCOM.Company

    Private m_oCVenta As ContratoVentasCls

    Public strNumeroEntrada As String = String.Empty
    Dim strNumeroCV As String
    Dim strLineaRevertidos As String

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

    Private Sub LigaFormulario(ByVal oForm As SAPbouiCOM.Form)

        Dim oitem As SAPbouiCOM.Item
        Dim oedit As SAPbouiCOM.EditText

        Try

            oitem = oForm.Items.Item("SCGD_txtCV")
            oedit = DirectCast(oitem.Specific, SAPbouiCOM.EditText)
            Dim s As String = oForm.DataSources.DBDataSources.Item(3).TableName
            oedit.DataBind.SetBound(True, "OIGN", "U_SCGD_NoContrato")

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

    Public Sub ManejarEstado(ByRef oForm As SAPbouiCOM.Form)

        Try

            oForm.Items.Item("SCGD_txtCV").Enabled = False

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Public Sub ManejadorEventoItemPressed(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oForm As SAPbouiCOM.Form

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim oChild As SAPbobsCOM.GeneralData
        Dim oChildren As SAPbobsCOM.GeneralDataCollection

        Try

            m_oCVenta = New ContratoVentasCls(SBO_Company, SBO_Application)

            oForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

            If pVal.ItemUID = "SCGD_lkCV" AndAlso pVal.ActionSuccess Then

                strNumeroCV = oForm.DataSources.DBDataSources.Item("OIGN").GetValue("U_SCGD_NoContrato", 0)

                strNumeroCV = strNumeroCV.Trim()

                If Not ValidarSiFormularioAbierto(ContratoVentasCls.FormType, False) Then

                    Call m_oCVenta.DibujarFormularioContratoVentas("", False)
                    Call m_oCVenta.CargarContrato(strNumeroCV, ContratoVentasCls.FormType)
                    Utilitarios.FormularioSoloLectura(SBO_Application.Forms.Item(ContratoVentasCls.FormType), False)

                Else

                    SBO_Application.Forms.Item(ContratoVentasCls.FormType).Select()

                End If

                m_oCVenta.m_blnCargoManejarEstados = False

            ElseIf pVal.ItemUID = "SCGD_lkCV" AndAlso pVal.ActionSuccess Then

                m_oCVenta.m_blnCargoManejarEstados = True

            ElseIf pVal.ItemUID = "1" AndAlso oForm.Mode = BoFormMode.fm_ADD_MODE AndAlso pVal.BeforeAction Then

                strNumeroCV = oForm.DataSources.DBDataSources.Item("OIGN").GetValue("U_SCGD_NoContrato", 0)

                strNumeroCV = strNumeroCV.Trim()

            ElseIf pVal.ItemUID = "1" AndAlso oForm.Mode = BoFormMode.fm_ADD_MODE AndAlso pVal.ActionSuccess Then

                'strNumeroSalida = oForm.DataSources.DBDataSources.Item("OIGE").GetValue("DocNum", 0)

                If Not String.IsNullOrEmpty(strNumeroCV) Then

                    oCompanyService = SBO_Company.GetCompanyService()
                    oGeneralService = oCompanyService.GetGeneralService("SCGD_ContRevertir")
                    oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                    strLineaRevertidos = Utilitarios.EjecutarConsulta("Select DocEntry from [@SCGD_CV_REVERTIR] where U_NumC = " & strNumeroCV, SBO_Company.CompanyDB, SBO_Company.Server)
                    oGeneralParams.SetProperty("DocEntry", strLineaRevertidos)
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                    oChildren = oGeneralData.Child("SCGD_CV_REVERLINEA")
                    oChild = oChildren.Item(0)
                    oChild.SetProperty("U_SCGD_EntMerc", strNumeroEntrada)
                    oGeneralService.Update(oGeneralData)

                End If

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
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

End Class
