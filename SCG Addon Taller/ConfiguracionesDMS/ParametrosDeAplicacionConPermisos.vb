Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports ICompany = SAPbobsCOM.ICompany

Public Class ParametrosDeAplicacionConPermisos
    Inherits ParametrosDeAplicacion
    Implements IUsaPermisos

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_strUISCGD_FormParamAplicacion As String)
        MyBase.New(application, companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioParametrosDeAplicacion
        MenuPadre = "SCGD_CDE"
        Nombre = "Parametros de Aplicacion"
        IdMenu = p_strUISCGD_FormParamAplicacion
        Titulo = "Parametros de Aplicacion"
        Posicion = 1
        FormType = p_strUISCGD_FormParamAplicacion
        StrDireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        StrUsuarioBD = CatchingEvents.DBUser
        StrContraseñaBD = CatchingEvents.DBPassword
    End Sub
End Class
