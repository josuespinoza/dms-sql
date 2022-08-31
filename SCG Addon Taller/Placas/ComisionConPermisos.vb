Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.Placas
Imports ICompany = SAPbobsCOM.ICompany

Public Class ComisionConPermisos
    Inherits Comision
    Implements IUsaPermisos

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        MyBase.New(application, companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLComisionPlacas
        MenuPadre = "SCGD_RPP"
        IdMenu = "SCGD_COP"
        Titulo = My.Resources.Resource.TituloComisionPlacas
        Posicion = 3
        FormType = "SCGD_COM_PLC"
        DireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        UsuarioBD = CatchingEvents.DBUser
        ContraseñaBD = CatchingEvents.DBPassword
    End Sub

End Class
