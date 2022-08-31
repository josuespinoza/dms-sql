Imports System.Collections.Generic

Public Class LicenseComponent
    Public Property Tipo As String
    Public Property Descripcion As String
    Public Property Cantidad As Integer
    Public Property Formularios As Dictionary(Of String, String)


    Sub New(ByVal Tipo As String, ByVal Descripcion As String, ByVal Cantidad As Integer)
        Try
            Me.Tipo = Tipo
            Me.Descripcion = Descripcion
            Me.Cantidad = Cantidad
            Formularios = New Dictionary(Of String, String)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub AgregarFormulario(ByVal UniqueID As String, ByVal DescripcionFormulario As String)
        Try
            If Not Formularios.ContainsKey(UniqueID) Then
                If Not String.IsNullOrEmpty(UniqueID) AndAlso Not String.IsNullOrEmpty(DescripcionFormulario) Then
                    Formularios.Add(UniqueID, DescripcionFormulario)
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
End Class