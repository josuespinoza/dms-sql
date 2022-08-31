Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.Financiamiento
Imports ICompany = SAPbobsCOM.ICompany

Public Class SaldosConPermisos
    Inherits Saldos
    Implements IUsaPermisos

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        MyBase.New(application, companySbo)
        NombreXml = System.Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLSaldos
        MenuPadre = "SCGD_RPF"
        IdMenu = "SCGD_SAF"
        Titulo = My.Resources.Resource.TituloSaldos
        Posicion = 4
        FormType = "SCGD_SALDOS"
        StrDireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        StrUsuarioBD = CatchingEvents.DBUser
        StrContraseñaBD = CatchingEvents.DBPassword
    End Sub

End Class
