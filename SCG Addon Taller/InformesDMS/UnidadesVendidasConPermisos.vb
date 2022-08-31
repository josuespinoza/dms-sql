Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM

Public Class UnidadesVendidasConPermisos
    Inherits UnidadesVendidas
    Implements IUsaPermisos


    Public Sub New(ByVal companySbo As SAPbobsCOM.Company, ByVal application As Application, p_menuInformesDMS As String, p_strUISCGD_RptUnidadesVend As String)
        MyBase.New(application, companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFrmReporteUnidadesVendidas
        MenuPadre = p_menuInformesDMS
        Nombre = My.Resources.Resource.MenuUnidadesVendidas
        IdMenu = p_strUISCGD_RptUnidadesVend
        Titulo = My.Resources.Resource.MenuUnidadesVendidas
        Posicion = 3
        FormType = p_strUISCGD_RptUnidadesVend
        DireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        UsuarioBd = CatchingEvents.DBUser
        ContraseñaBd = CatchingEvents.DBPassword
    End Sub

End Class
