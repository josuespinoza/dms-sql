
Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports ICompany = SAPbobsCOM.ICompany

Public Class AsociacionArticuloxEspecifConPermisos
    Inherits AsociacionArticuloxEspecific
    Implements IUsaPermisos

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        MyBase.New(application, companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioAsocArtixEspecif
        MenuPadre = "15872"
        Nombre = "ArticuloxEspecificacion"
        IdMenu = "SCGD_AAE"
        Titulo = My.Resources.Resource.TituloFormularioAsocArtixEspecif
        Posicion = 2
        FormType = "SCGD_ASOC_AXE"
    End Sub

End Class
