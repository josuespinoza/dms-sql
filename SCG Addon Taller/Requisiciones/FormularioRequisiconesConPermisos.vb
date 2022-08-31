Imports DMS_Addon.ControlesSBO
Imports SCG.Requisiciones
Imports SAPbouiCOM
Imports SCG.Requisiciones.UI
Imports ICompany = SAPbobsCOM.ICompany

Namespace Requisiciones
    Public Class FormularioRequisiconesConPermisos
        Inherits FormularioRequisiciones
        Implements IUsaPermisos

        Public Sub New(ByVal applicationSBO As Application, ByVal companySBO As ICompany, ByVal requisicion As Requisicion)
            MyBase.New(applicationSBO, companySBO, requisicion)
            NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormularioRequisiciones
            MenuPadre = "43540"
            Nombre = My.Resources.Resource.TituloFormularioRequisicones
            IdMenu = "SCGD_REQ"
            BDUser = CatchingEvents.DBUser
            BDPass = CatchingEvents.DBPassword
            DireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim, "\")
            Titulo = My.Resources.Resource.TituloFormularioRequisicones
            Posicion = 1
        End Sub
    End Class
End Namespace