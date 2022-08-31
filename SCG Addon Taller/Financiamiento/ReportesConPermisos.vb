Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.Financiamiento

Public Class ReportesConPermisos
    Inherits Reportes
    Implements IUsaPermisos

    Public Sub New(menuFinanc As String)
        MyBase.New()
        MenuPadre = menuFinanc
        Nombre = "Reportes Financiamiento"
        IdMenu = "SCGD_RPF"
        Posicion = 3
    End Sub

End Class
