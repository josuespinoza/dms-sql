Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.Financiamiento
Imports ICompany = SAPbobsCOM.ICompany

Public Class PrestamoFormularioConPermisos
    Inherits PrestamoFormulario
    Implements IUsaPermisos

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, menuFinanc As String)
        MyBase.New(application, companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLPrestamo
        MenuPadre = menuFinanc
        NombreMenu = "Préstamo"
        IdMenu = "SCGD_PRT"
        Titulo = My.Resources.Resource.TituloPrestamo
        Posicion = 1
        FormType = "SCGD_PRESTAMOS"
        StrDireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        StrUsuarioBD = CatchingEvents.DBUser
        StrContraseñaBD = CatchingEvents.DBPassword
    End Sub
End Class