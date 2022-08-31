Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports ICompany = SAPbobsCOM.ICompany

Public Class CargarPanelCitasConPermisos
    Inherits CargarPanelCitas
    Implements IUsaPermisos

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_menuCitas As String, ByVal p_strUISCGD_CargPanelCitas As String)
        MyBase.New(application, companySbo)
        NombreXml = Windows.Forms.Application.StartupPath & My.Resources.Resource.XMLFormularioCargaPanelCitas
        MenuPadre = p_menuCitas
        Nombre = "Panel Citas"
        IdMenu = p_strUISCGD_CargPanelCitas
        Titulo = My.Resources.Resource.TituloCargaPanelCitas
        Posicion = 3
        FormType = p_strUISCGD_CargPanelCitas
        StrDireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        StrUsuarioBD = CatchingEvents.DBUser
        StrContraseñaBD = CatchingEvents.DBPassword
    End Sub

End Class
