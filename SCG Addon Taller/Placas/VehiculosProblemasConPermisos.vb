Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.Placas
Imports ICompany = SAPbobsCOM.ICompany

Public Class VehiculosProblemasConPermisos
    Inherits VehiculosProblemas
    Implements IUsaPermisos

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        MyBase.New(application, companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLVehiculosProblemasPlacas
        MenuPadre = "SCGD_RPP"
        IdMenu = "SCGD_PPL"
        Titulo = My.Resources.Resource.TituloVehiculosProblemasPlacas
        Posicion = 4
        FormType = "SCGD_VEH_PROB"
        DireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        UsuarioBD = CatchingEvents.DBUser
        ContraseñaBD = CatchingEvents.DBPassword
    End Sub

End Class
