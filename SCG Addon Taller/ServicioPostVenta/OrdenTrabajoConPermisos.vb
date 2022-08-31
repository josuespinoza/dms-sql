Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.ServicioPostVenta
Imports ICompany = SAPbobsCOM.ICompany

Public Class OrdenTrabajoConPermisos
    Inherits OrdenTrabajo
    Implements IUsaPermisos

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByRef p_AsignacionMultipleConPermisos As AsignacionMultipleOT, ByRef p_RazonesSuspensionConPermisos As RazonesSuspensionConPermisos, ByRef p_FinalizaActividad As FinalizaActividad, ByRef p_TrackingRepuestos As TrackingRepuestos, ByRef p_DocumentoCompraConPermisos As DocumentoCompraConPermisos, ByRef p_BuscadorProveedoresConPermisos As BuscadorProveedoresConPermisos, ByRef p_TrackSolEspecificos As TrackingSolEspecificos)
        MyBase.New(application, companySbo, p_AsignacionMultipleConPermisos, p_RazonesSuspensionConPermisos, p_FinalizaActividad, p_TrackingRepuestos, p_DocumentoCompraConPermisos, p_BuscadorProveedoresConPermisos, p_TrackSolEspecificos)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.xmlOT
        MenuPadre = "SCGD_GOV"
        Nombre = My.Resources.Resource.TituloOT
        IdMenu = "SCGD_OTR"
        Posicion = 8
        FormType = "SCGD_ORDT"
        PasswordBD = CatchingEvents.DBPassword
    End Sub

End Class


