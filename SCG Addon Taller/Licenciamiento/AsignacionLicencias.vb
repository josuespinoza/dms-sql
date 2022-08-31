Imports System.Collections.Generic

Public Class AsignacionLicencias

    Public AsignacionPorTipo As Dictionary(Of String, Integer)
    Public AsignacionPorUsuario As Dictionary(Of String, List(Of String))

    Sub New()
        Try

            AsignacionPorTipo = New Dictionary(Of String, Integer)
            AsignacionPorUsuario = New Dictionary(Of String, List(Of String))
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub AsignarLicencia(ByVal IDUsuario As String, ByVal TipoLicencia As String)
        Dim ListaLicenciasUsuario As List(Of String)
        Try
            ListaLicenciasUsuario = New List(Of String)
            If AsignacionPorTipo.ContainsKey(TipoLicencia) Then
                AsignacionPorTipo.Item(TipoLicencia) += 1
            Else
                AsignacionPorTipo.Add(TipoLicencia, 1)
            End If

            If AsignacionPorUsuario.ContainsKey(IDUsuario) Then
                ListaLicenciasUsuario = AsignacionPorUsuario.Item(IDUsuario)
                If Not ListaLicenciasUsuario.Contains(TipoLicencia) Then
                    ListaLicenciasUsuario.Add(TipoLicencia)
                End If
            Else
                ListaLicenciasUsuario.Add(TipoLicencia)
                AsignacionPorUsuario.Add(IDUsuario, ListaLicenciasUsuario)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub RemoverAsignacion(ByVal IDUsuario As String, ByVal TipoLicencia As String)
        Try
            If AsignacionPorTipo.ContainsKey(TipoLicencia) Then
                AsignacionPorTipo.Item(TipoLicencia) -= 1
            Else
                AsignacionPorTipo.Add(TipoLicencia, 0)
            End If

            If AsignacionPorUsuario.ContainsKey(IDUsuario) Then
                If AsignacionPorUsuario.Item(IDUsuario) IsNot Nothing Then
                    If AsignacionPorUsuario.Item(IDUsuario).Contains(TipoLicencia) Then
                        AsignacionPorUsuario.Item(IDUsuario).Remove(TipoLicencia)
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

End Class
