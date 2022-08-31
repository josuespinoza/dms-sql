Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.Financiamiento
Imports ICompany = SAPbobsCOM.ICompany

Public Class ConfiguracionFormularioConPermisos
    Inherits ConfiguracionFormulario
    Implements IUsaPermisos

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, menuFinanc As String)
        MyBase.New(application, companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLConfFinanc
        MenuPadre = menuFinanc
        NombreMenu = "Configuración"
        IdMenu = "SCGD_CFF"
        Titulo = My.Resources.Resource.TituloConfFinanc
        Posicion = 2
        FormType = "SCGD_CONF_FIN"

    End Sub

End Class
