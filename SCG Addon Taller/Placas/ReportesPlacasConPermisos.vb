Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.Placas

Public Class ReportesPlacasConPermisos
    Inherits Reportes
    Implements IUsaPermisos

    Public Sub New(p_menuPlacas As String)
        MyBase.New()
        MenuPadre = p_menuPlacas
        Nombre = "Reportes Placas"
        IdMenu = "SCGD_RPP"
        Posicion = 3
    End Sub

End Class
