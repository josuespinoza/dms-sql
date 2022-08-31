'Fecha: 29/04/2009
'Autor: Werner

Imports System.Configuration

''' <summary>
''' Permite ejecutar el addon de DMS y cambiar el idioma antes de su ejecución
''' </summary>
Public Class GestorAddon

    Private Const _lengua As String = "Lengua"

    Private _proceso As Process

    Public Sub New()

        _proceso = New Process()

    End Sub

    ''' <summary>
    ''' Ejecuta un addon enviando al exe los parámetros
    ''' </summary>
    ''' <param name="p_archivo">Nombre del archivo ejecutable</param>
    ''' <param name="p_argumentos">Lista de parámetros</param>
    Public Sub EjecutarAddon(ByVal p_archivo As String, ByVal p_argumentos As String)

        _proceso.StartInfo.FileName = p_archivo

        If p_argumentos.Length <> 0 Then

            _proceso.StartInfo.Arguments = p_argumentos

        End If

        _proceso.Start()

    End Sub

    ''' <summary>
    ''' Cambia el lenguaje de la aplicación
    ''' </summary>
    ''' <param name="p_archivoConfig">Nombre del archivo de configuración</param>
    ''' <param name="p_idioma">Idioma que se debe usar</param>
    Public Sub CambiarIdioma(ByVal p_archivoConfig As String, ByVal p_idioma As String)

        Dim configFile As New ExeConfigurationFileMap()

        configFile.ExeConfigFilename = p_archivoConfig

        Dim config As Configuration = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None)

        config.AppSettings.Settings.Remove(_lengua)

        config.AppSettings.Settings.Add(_lengua, p_idioma)

        config.Save(ConfigurationSaveMode.Full)

        ConfigurationManager.RefreshSection("appSettings")

    End Sub

End Class