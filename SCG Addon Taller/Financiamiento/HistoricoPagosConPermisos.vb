Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.Financiamiento
Imports ICompany = SAPbobsCOM.ICompany

Public Class HistoricoPagosConPermisos
    Inherits HistoricoPagos
    Implements IUsaPermisos

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        MyBase.New(application, companySbo)
        NombreXml = System.Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLHistoricoPagos
        MenuPadre = "SCGD_RPF"
        IdMenu = "SCGD_HPF"
        Titulo = My.Resources.Resource.TituloHistoricoPagos
        Posicion = 2
        FormType = "SCGD_HIST_PAGOS"
        StrDireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        StrUsuarioBD = CatchingEvents.DBUser
        StrContraseñaBD = CatchingEvents.DBPassword
    End Sub

End Class
