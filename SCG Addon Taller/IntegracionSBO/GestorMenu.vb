'Fecha: 29/04/2009
'Autor: Werner
Imports DMS_Addon.ControlesSBO
Imports SCG.DMSOne.Framework.MenuManager
Imports SCG.SBOFramework.UI

''' <summary>
''' Permite agregar menus en SBO
''' </summary>
Public Class GestorMenu

    Private _sboApplication As SAPbouiCOM.Application

    Public Shared MenusManager As DmsOneMenusManager = New DmsOneMenusManager()

    <System.CLSCompliant(False)> _
    Public Sub New(ByRef p_sboApplication As SAPbouiCOM.Application)

        _sboApplication = p_sboApplication

    End Sub

    ''' <summary>
    ''' Agrega un menú en SBO
    ''' </summary>
    ''' <param name="idMenu">Id del menú</param>
    ''' <param name="nombreMenu">Nombre del menú</param>
    ''' <param name="imagen">Imágen del menú</param>
    Public Sub AgregarMenu(ByVal idMenu As String, ByVal nombreMenu As String, ByVal imagen As String)

        MenusManager.AddMenuEntry(New MenuEntry(idMenu, SAPbouiCOM.BoMenuType.mt_POPUP, nombreMenu, 30, False, True, imagen, "43520"))

    End Sub

    ''' <summary>
    ''' Agrega un submenú en SBO
    ''' </summary>
    ''' <param name="idMenu">Id del menú</param>
    ''' <param name="nombreMenu">Nombre del submenú</param>
    ''' <param name="imagen">Imágen del submenú</param>
    ''' <param name="idMenuPadre">Id del menú padre</param>
    ''' <param name="posicion">Posición dentro del submenu</param>
    Public Sub AgregarSubMenu(ByVal idMenu As String, ByVal nombreMenu As String, ByVal posicion As Integer, ByVal imagen As String, ByVal idMenuPadre As String, _
                                    Optional ByVal tipoMenu As Integer = 1)

        MenusManager.AddMenuEntry(New MenuEntry(idMenu, tipoMenu, nombreMenu, posicion, False, True, imagen, idMenuPadre))
        
    End Sub

    Public Sub AgregaSubMenu(ByVal datosMenu As IUsaMenu)
        AgregarSubMenu(datosMenu.IdMenu, datosMenu.Nombre, datosMenu.Posicion, Nothing, datosMenu.MenuPadre)
    End Sub

    Public Sub AgregaSubMenu(ByVal datosMenu As IUsaPermisos, Optional ByVal intTipo As Integer = 1)
        Dim nombreMenu As String
        If Utilitarios.MostrarMenu(datosMenu.IdMenu, _sboApplication.Company.UserName) Then
            nombreMenu = Utilitarios.PermisosMenu(datosMenu.IdMenu, _sboApplication.Language)
            datosMenu.Nombre = nombreMenu
            AgregarSubMenu(datosMenu.IdMenu, datosMenu.Nombre, datosMenu.Posicion, Nothing, datosMenu.MenuPadre, intTipo)
        End If
    End Sub


End Class
