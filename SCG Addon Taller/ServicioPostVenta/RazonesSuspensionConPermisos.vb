Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.ServicioPostVenta
Imports ICompany = SAPbobsCOM.ICompany

Public Class RazonesSuspensionConPermisos
    Inherits RazonesSuspension

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        MyBase.New(application, companySbo)
        NombreXml = Environment.CurrentDirectory + My.Resources.Resource.frmRazonesSuspension
        FormType = "SCGD_RAZO"
    End Sub

End Class
