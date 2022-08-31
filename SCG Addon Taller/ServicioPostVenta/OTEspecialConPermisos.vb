Imports SCG.ServicioPostVenta.CreaciónOTEspecial
Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.ServicioPostVenta
Imports ICompany = SAPbobsCOM.ICompany

Public Class OTEspecialConPermisos
    Inherits OTEspecial

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        MyBase.New(application, companySbo)
    End Sub

End Class
