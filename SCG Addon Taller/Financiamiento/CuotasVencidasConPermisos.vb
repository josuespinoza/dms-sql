Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.Financiamiento
Imports ICompany = SAPbobsCOM.ICompany

Public Class CuotasVencidasConPermisos
    Inherits CuotasVencidas
    Implements IUsaPermisos

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        MyBase.New(application, companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLCuotasVencidas
        MenuPadre = "SCGD_RPF"
        IdMenu = "SCGD_CVF"
        Titulo = My.Resources.Resource.TituloCuotasVencidas
        Posicion = 3
        FormType = "SCGD_VENCIDAS"
        StrDireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        StrUsuarioBD = CatchingEvents.DBUser
        StrContraseñaBD = CatchingEvents.DBPassword
    End Sub

End Class
