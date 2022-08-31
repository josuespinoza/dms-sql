Imports DMS_Addon.ControlesSBO
Imports SCG.ServicioPostVenta
Imports SAPbobsCOM

Public Class ListadoSolicitudEspecificosConPermisos
    Inherits ListadoSolicitudEspecificos
    Implements IUsaPermisos

    Public Sub New(ByVal applicationSBO As SAPbouiCOM.Application, ByVal companySBO As ICompany, ByVal p_strMenuListadoSolEsp As String)
        MyBase.New(applicationSBO, companySBO)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLListadoSolEsp
        MenuPadre = "SCGD_GOV"
        Nombre = My.Resources.Resource.TituloListadoSolEsp
        IdMenu = p_strMenuListadoSolEsp
        Posicion = 200
        FormType = p_strMenuListadoSolEsp
    End Sub
End Class
