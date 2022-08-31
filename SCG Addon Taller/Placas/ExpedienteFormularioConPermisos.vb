Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.Placas
Imports ICompany = SAPbobsCOM.ICompany

Public Class ExpedienteFormularioConPermisos
    Inherits ExpedienteFormulario
    Implements IUsaPermisos

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_menuPlacas As String)
        MyBase.New(application, companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLPlacas
        MenuPadre = p_menuPlacas
        Nombre = "Placas"
        IdMenu = "SCGD_PLC"
        Titulo = My.Resources.Resource.TituloPlacas
        Posicion = 1
        FormType = "SCGD_PLACAS"
        DireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        UsuarioBD = CatchingEvents.DBUser
        ContraseñaBD = CatchingEvents.DBPassword
    End Sub

End Class
