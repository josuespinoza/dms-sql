Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.Placas
Imports ICompany = SAPbobsCOM.ICompany

Public Class GrupoFormularioConPermisos
    Inherits GrupoPlacasFormulario
    Implements IUsaPermisos

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_menuPlacas As String)
        MyBase.New(application, companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLGrupoPlacas
        MenuPadre = p_menuPlacas
        Nombre = "Grupo_Placas"
        IdMenu = "SCGD_PLG"
        Titulo = My.Resources.Resource.TituloGrupoPlacas
        Posicion = 2
        FormType = "SCGD_GRUPO_PLACAS"
        UsuarioBD = CatchingEvents.DBUser
        ContraseñaBD = CatchingEvents.DBPassword
    End Sub
End Class
