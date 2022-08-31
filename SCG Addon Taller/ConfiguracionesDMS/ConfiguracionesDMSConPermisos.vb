Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Partial Public Class ConfiguracionesDMSConPermisos
    Inherits ConfiguracionesDMS
    Implements IUsaPermisos

    Public Sub New()
        MyBase.New()
        MenuPadre = "SCGD_CFG"
        Nombre = "Configuraciones DMS"
        IdMenu = "SCGD_CDE"
        Posicion = 65
    End Sub
End Class
