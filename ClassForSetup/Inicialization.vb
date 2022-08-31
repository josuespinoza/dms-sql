Module Inicialization

    Friend Const mc_NombreBaseDatosSCGConfg As String = "SCGConfiguracion.bak"
    Friend Const mc_NombreBaseDatosSCGDMS As String = "SCGDMSOneDemoES_MX.bak"
    Friend Const mc_NombreBaseDatosSBO As String = "SBODemoES_MX.bak"
    Friend Const mc_RutaArchivoRespaldo As String = "DBs"

    Friend Const mc_strSQLAddonUser As String = "SCGAddon"
    Friend Const mc_strSQLAddonPass As String = "scgadmin"

    Friend m_strPFF As String
    Friend m_strFolderDestino As String
    Friend m_strFolderOrigen As String

    Friend m_NombreServidor As String = "127.0.0.1"
    Friend m_AutenticacionWindows As Boolean = True

    Friend m_UsuarioServidor As String = "sa"
    Friend m_ContraseñaServidor As String = "scg"
    Friend m_pathBDServidor As String = ""

End Module
