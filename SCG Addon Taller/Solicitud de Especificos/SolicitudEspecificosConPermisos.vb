Imports DMS_Addon.ControlesSBO
Imports SCG.ServicioPostVenta

Public Class SolicitudEspecificosConPermisos
    Inherits SolicitudEspecificos
    Implements IUsaPermisos

    Public Sub New(ByVal applicationSBO As SAPbouiCOM.Application, ByVal companySBO As SAPbobsCOM.ICompany, ByVal p_strMenuSolEsp As String)
        MyBase.New(applicationSBO, companySBO)
        DBUser = CatchingEvents.DBUser
        DBPassword = CatchingEvents.DBPassword
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLSolicitudEspecificos
        MenuPadre = "SCGD_GOV"
        Nombre = My.Resources.Resource.TituloSolEsp
        IdMenu = p_strMenuSolEsp
        Posicion = 201
        FormType = p_strMenuSolEsp
    End Sub

End Class
