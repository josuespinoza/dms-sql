Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.Financiamiento
Imports ICompany = SAPbobsCOM.ICompany

Public Class EstadosCuentaConPermisos
    Inherits EstadoCuentas
    Implements IUsaPermisos

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        MyBase.New(application, companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLEstadoCuentas
        MenuPadre = "SCGD_RPF"
        IdMenu = "SCGD_ECF"
        Titulo = My.Resources.Resource.TituloEstadosCuenta
        Posicion = 1
        FormType = "SCGD_EST_CUENTA"
        StrDireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        StrUsuarioBD = CatchingEvents.DBUser
        StrContraseñaBD = CatchingEvents.DBPassword
    End Sub

End Class
