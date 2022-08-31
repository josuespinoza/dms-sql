Option Strict Off
Option Explicit On

Imports System.Windows.Forms
Imports DMS_Addon


Module SubMain

    <STAThread()> _
    <CLSCompliant(False)> _
    Public Sub Main()

        Try
            Dim oCatchingEvents As CatchingEvents
            'Dim appCurrent As New ApplicationServices.ApplicationBase
            'Dim ProcesosMismoNombre() As Process
            'Dim currentProcess As Process = Process.GetCurrentProcess
            'ProcesosMismoNombre = Process.GetProcessesByName(appCurrent.Info.AssemblyName)
            'For Each Proceso As Process In ProcesosMismoNombre
            '    If Proceso.Id <> currentProcess.Id AndAlso Proceso.SessionId = currentProcess.SessionId Then
            '        Proceso.Kill()
            '        Proceso.WaitForExit()
            '        While Not Proceso.HasExited
            '            Application.DoEvents()
            '        End While
            '    End If
            'Next
            oCatchingEvents = New CatchingEvents(My.Settings.DireccionConfiguracion, My.Settings.IDFormMarcaEstiloModelo, My.Settings.FiltroFormsCFL)
            Call Application.Run()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Application.Exit()
        End Try
    End Sub
    
End Module