Imports DMS_Addon.ControlesSBO
Imports System.Collections.Generic
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports System.Timers

Partial Public Class AgendaSuspension
    Implements IUsaPermisos

#Region "Variables"

    Private m_blnFlagTimer As Boolean = False
    Shared m_oTimer As System.Timers.Timer

#End Region


#Region "Metodos"


    Private Sub HandlerTimer(ByVal sender As Object, ByVal e As Timers.ElapsedEventArgs)

        If m_blnFlagTimer Then
            _applicationSbo.RemoveWindowsMessage(SAPbouiCOM.BoWindowsMessageType.bo_WM_TIMER, True)
        End If
    End Sub

    Public Sub IniciaTimer()

        m_blnFlagTimer = True

        m_oTimer = New System.Timers.Timer()
        AddHandler m_oTimer.Elapsed, New ElapsedEventHandler(AddressOf HandlerTimer)
        m_oTimer.Interval = 60000
        m_oTimer.Enabled = True

    End Sub

    Private Sub FinalizaTimer()
        m_blnFlagTimer = False

        m_oTimer.Enabled = False
        m_oTimer.Stop()
        m_oTimer.Dispose()

    End Sub
#End Region





End Class
