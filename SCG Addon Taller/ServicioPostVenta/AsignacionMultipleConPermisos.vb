Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.ServicioPostVenta
Imports ICompany = SAPbobsCOM.ICompany

Public Class AsignacionMultipleConPermisos
    Inherits AsignacionMultipleOT

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        MyBase.New(application, companySbo, 0)
        NombreXml = Environment.CurrentDirectory + My.Resources.Resource.frmAsignacionMultiple
        FormType = "SCGD_ASIM"
    End Sub

End Class
