Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports ICompany = SAPbobsCOM.ICompany

Public Class BusquedasCitasConPermisos
    Inherits BusquedasCitas
    Implements IUsaPermisos

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_menuCitas As String, ByVal p_mc_strUISCGD_BusqCitas As String)
        MyBase.New(application, companySbo)
        NombreXml = System.Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLBusquedaCitas
        MenuPadre = p_menuCitas
        Nombre = "Citas"
        IdMenu = p_mc_strUISCGD_BusqCitas
        Titulo = My.Resources.Resource.TituloCitas
        Posicion = 1
        FormType = p_mc_strUISCGD_BusqCitas
        StrDireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        StrUsuarioBD = CatchingEvents.DBUser
        StrContraseñaBD = CatchingEvents.DBPassword
    End Sub

End Class
