Imports SAPbouiCOM
Imports SAPbobsCOM
Imports SCG.DMSOne.Framework.MenuManager
Imports System.Globalization
Imports SCG.SBOFramework
Imports System.Xml
Imports System.IO
Imports System.Collections.Generic
Imports System.Reflection
Imports SCG.SBOFramework.UI
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Windows.Forms
Imports RestSharp
Imports Newtonsoft.Json.Linq
Imports DMS_Connector.Business_Logic.DataContract
Imports DMS_Connector.Business_Logic.DataContract.Integracion

Module InterfaceJohnDeere_DTFAPITest
    Private WithEvents oApplication As SAPbouiCOM.Application
    Private oCompany As SAPbobsCOM.Company
    Private n As NumberFormatInfo

    Enum Accion
        CargarArchivo
        DescargarArchivo
    End Enum
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        Try
            oApplication = DMS_Connector.Company.ApplicationSBO
            oCompany = DMS_Connector.Company.CompanySBO
            n = DIHelper.GetNumberFormatInfo(oCompany)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ManejaInterfaceJohnDeere_DTFAPI(ByRef p_oForm As SAPbouiCOM.Form, ByRef p_strAccion As String)
        Dim oInterfazJohnDeere As InterfazJohnDeereDC
        Try
            oInterfazJohnDeere = New InterfazJohnDeereDC()
            CargarConfiguracion(p_oForm, oInterfazJohnDeere)
            GetAccessToken(oInterfazJohnDeere)

            Select Case p_strAccion
                Case Accion.CargarArchivo
                    If Not String.IsNullOrEmpty(oInterfazJohnDeere.RutaArchivoCarga) Then
                        PUTLoadFile(oInterfazJohnDeere)
                    Else
                        oApplication.StatusBar.SetText("Debe seleccionar un archivo para realizar la carga", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End If
                Case Accion.DescargarArchivo
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub GetAccessToken(ByRef p_oInterfazJohnDeereDC As InterfazJohnDeereDC)
        Dim client As RestClient
        Dim request As RestRequest
        Dim response As IRestResponse
        Dim resp As JObject
        Try
            ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)

            client = New RestClient(p_oInterfazJohnDeereDC.TokenURL)
            request = New RestRequest(Method.POST)
            request.AddHeader("cache-control", "no-cache")
            request.AddHeader("content-type", "application/x-www-form-urlencoded")
            request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&scope=dtf:dbs:file:read dtf:dbs:file:write&client_id=" + p_oInterfazJohnDeereDC.ClientID + "&client_secret=" + p_oInterfazJohnDeereDC.Secret, ParameterType.RequestBody)
            response = client.Execute(request)
            resp = JObject.Parse(response.Content)
            p_oInterfazJohnDeereDC.AccessToken = resp.SelectToken("access_token").ToString()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub PUTLoadFile(ByRef p_oInterfazJohnDeereDC As InterfazJohnDeereDC)
        Dim client As RestClient
        Dim request As RestRequest
        Dim response As IRestResponse
        Dim resp As JObject
        Try
            client = New RestClient(p_oInterfazJohnDeereDC.BaseURL)
            client.Timeout = -1
            request = New RestRequest(Method.PUT)
            request.AddHeader("Authorization", "Bearer " & p_oInterfazJohnDeereDC.AccessToken)
            response = client.Execute(request)

            resp = JObject.Parse(response.Content)
            _txtResul.AsignaValorUserDataSource(resp.Item("message").ToString())

            oApplication.StatusBar.SetText("Process Complete", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub CargarConfiguracion(ByRef p_oForm As SAPbouiCOM.Form, ByRef p_oInterfazJohnDeereDC As InterfazJohnDeereDC)
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim dsInformation As DBDataSource
        Try
            If Not p_oForm Is Nothing Then
                p_oForm.DataSources.DBDataSources.Add("@SCGD_JD")
                dsInformation = p_oForm.DataSources.DBDataSources.Item("@SCGD_JD")

                oConditions = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                oCondition = oConditions.Add()
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "Code"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "JD1"
                oCondition.BracketCloseNum = 1

                dsInformation.Query(oConditions)
                For index As Integer = 0 To dsInformation.Size - 1
                    If Not String.IsNullOrEmpty(dsInformation.GetValue("U_TokenURL", index)) Then p_oInterfazJohnDeereDC.TokenURL = dsInformation.GetValue("U_TokenURL", index).ToString().Trim()
                    If Not String.IsNullOrEmpty(dsInformation.GetValue("U_ClientID", index)) Then p_oInterfazJohnDeereDC.ClientID = dsInformation.GetValue("U_ClientID", index).ToString().Trim()
                    If Not String.IsNullOrEmpty(dsInformation.GetValue("U_Secret", index)) Then p_oInterfazJohnDeereDC.Secret = dsInformation.GetValue("U_Secret", index).ToString().Trim()
                    If Not String.IsNullOrEmpty(dsInformation.GetValue("U_BaseURL", index)) Then p_oInterfazJohnDeereDC.BaseURL = dsInformation.GetValue("U_BaseURL", index).ToString().Trim()
                    If Not String.IsNullOrEmpty(dsInformation.GetValue("U_DAcc", index)) Then p_oInterfazJohnDeereDC.DealerAccount = dsInformation.GetValue("U_DAcc", index).ToString().Trim()
                Next
                If Not String.IsNullOrEmpty(_txtRutaC.ObtieneValorUserDataSource()) Then
                    p_oInterfazJohnDeereDC.RutaArchivoCarga = _txtRutaC.ObtieneValorUserDataSource().ToString()
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
End Module
