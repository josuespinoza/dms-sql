Imports DMS_Addon.ControlesSBO
Imports SCG.Requisiciones
Imports SAPbouiCOM
Imports ICompany = SAPbobsCOM.ICompany

Namespace Requisiciones
    Public Class ListadoRequisicionesConPermisos
        Inherits ListadoRequisiciones
        Implements IUsaPermisos
        
        Public Sub New(ByVal applicationSBO As Application, ByVal companySBO As ICompany)
            MyBase.New(applicationSBO, companySBO)
            NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormListadoRequisiciones
            MenuPadre = "43540"
            Nombre = My.Resources.Resource.TituloFormListadoRequisicones
            IdMenu = "SCGD_LRQ"
            Titulo = My.Resources.Resource.TituloFormListadoRequisicones
            Posicion = 2
        End Sub

    End Class
End Namespace
