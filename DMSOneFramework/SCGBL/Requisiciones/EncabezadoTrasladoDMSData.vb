Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports System.Runtime.CompilerServices
Imports SCG.Requisiciones
Imports System.Runtime.Serialization
Imports System.Text
Imports System.Xml.Serialization

Namespace SCGBL.Requisiciones

    Public Class EncabezadoTrasladoDMSData

        Public Marca As String
        Public Estilo As String
        Public Modelo As String
        Public Placa As String
        Public Vin As String
        Public NumCotizacion As Integer
        Public TipoTransferencia As Integer
        Public NumCotizacionOrigen As Integer
        Public Serie As String
    End Class

    Public Module DataExtensionMethods
        <Extension()> _
        Public Function Serialize(ByVal obj As EncabezadoTrasladoDMSData) As String
            Dim xmlSerializer As XmlSerializer = New XmlSerializer(obj.GetType)
            Dim sb As StringWriter = New StringWriter()
            xmlSerializer.Serialize(sb, obj)
            Return sb.ToString
        End Function

        <Extension()> _
        Public Function Deserialize(ByVal data As String) As EncabezadoTrasladoDMSData
            Dim xmlSerializer As XmlSerializer = New XmlSerializer(GetType(EncabezadoTrasladoDMSData))
            Dim sr As StringReader = New StringReader(data)
            Return xmlSerializer.Deserialize(sr)
        End Function
    End Module
End Namespace