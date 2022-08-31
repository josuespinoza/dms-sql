Imports System.Collections.Generic
Imports System.Xml
Public Class License
    Private HardwareKey As String
    Private IdLicencia As String
    Private AddOn As String
    Private FechaInicio As DateTime
    Public Property FechaVencimiento As DateTime
    Private UsaHardwareKey As Boolean
    Public LicenseComponents As List(Of LicenseComponent)

    Sub New(ByRef Documento As XmlDocument)
        Try
            LicenseComponents = New List(Of LicenseComponent)
            LeerArchivoLicencias(Documento)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub LeerArchivoLicencias(ByRef Documento As XmlDocument)
        Dim oXmlElement As XmlElement
        Dim NodeList As XmlNodeList
        Dim IdTipoLicencia As String = String.Empty
        Dim NombreTipoLicencia As String = String.Empty
        Dim Cantidad As Integer = 0
        Dim UsaHardwareKey As Boolean = False
        Dim nsmgr As XmlNamespaceManager
        Dim UniqueID As String = String.Empty
        Dim DscFormulario As String = String.Empty
        Try
            oXmlElement = Documento.DocumentElement
            nsmgr = New XmlNamespaceManager(Documento.NameTable)
            nsmgr.AddNamespace("Default", "http://tempuri.org/SCGLicencia.xsd")
            NodeList = oXmlElement.SelectNodes("//Default:Lic", nsmgr)
            For Each Nodo As XmlNode In NodeList
                AddOn = Nodo.Item("NombreSistema").InnerText
                If AddOn = "SCG DMS One" Then
                    HardwareKey = Nodo.Item("HardwareKey").InnerText
                    UsaHardwareKey = Nodo.Item("usaHardwareKey").InnerText
                    FechaInicio = DateTime.Parse(Nodo.Item("FechaInicio").InnerText)
                    FechaVencimiento = DateTime.Parse(Nodo.Item("FechaFinal").InnerText)

                    IdTipoLicencia = Nodo.Item("IdTipoLicencia").InnerText
                    NombreTipoLicencia = Nodo.Item("NombreTipoLicencia").InnerText
                    Cantidad = Nodo.Item("Cantidad").InnerText
                    LicenseComponents.Add(New LicenseComponent(IdTipoLicencia, NombreTipoLicencia, Cantidad))
                End If
            Next

            For Each Componente As LicenseComponent In LicenseComponents
                NodeList = oXmlElement.SelectNodes("//Default:Opc", nsmgr)
                For Each Nodo As XmlNode In NodeList
                    IdTipoLicencia = Nodo.Item("IdTipoLicencia").InnerText
                    UniqueID = Nodo.Item("CodigoOpcion").InnerText
                    DscFormulario = Nodo.Item("NombreFrmOpcion").InnerText
                    If Not String.IsNullOrEmpty(IdTipoLicencia) AndAlso Componente.Tipo = IdTipoLicencia Then
                        Componente.AgregarFormulario(UniqueID, DscFormulario)
                    End If
                Next
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

End Class
