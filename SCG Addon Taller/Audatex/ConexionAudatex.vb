Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Collections.Specialized

Public Class ConexionAudatex

    Public Sub New()
    End Sub

    '
    ' Llama una funcion para mandar a crear un expediente en Audatex
    '         
    Public Function AddExpedient(p_AdministradorExpediente As String, p_NumeroExpediente As String, p_NombreAsegurado As String,
                            p_ApellidoAsegurado As String, p_FechaAccidente As String, p_DescripcionAccidente As String,
                            p_Placas As String, p_Fabricante As String, p_AnioFabricacion As String, p_Poliza As String,
                            p_VIN As String, p_Registro As String, p_UsuarioAsignado As String, p_PoolAsignado As String,
                            p_Taller As String, p_CompaniaAsignada As String, p_CompaniaOrigen As String,
                            p_CreadoPor As String, p_NumeroCaso As String, p_TarifaCarroceria As String,
                            p_TarifaPintura As String, p_Operacion As String) As String

        Dim urlFormat As String = "http://localhost:55248/Audatex?" +
            "p_AdministradorExpediente='{0}'&p_NumeroExpediente='{1}'&p_NombreAsegurado='{2}'&p_ApellidoAsegurado='{3}'&p_FechaAccidente='{4}'&p_DescripcionAccidente='{5}'&" +
            "p_Placas='{6}'&p_Fabricante='{7}'&p_AnioFabricacion='{8}'&p_Poliza='{9}'&p_VIN='{10}'&p_Registro='{11}'&p_UsuarioAsignado='{12}'&p_PoolAsignado='{13}'&p_Taller='{14}'&" +
            "p_CompaniaAsignada='{15}'&p_CompaniaOrigen='{16}'&p_CreadoPor='{17}'&p_NumeroCaso='{18}'&p_TarifaCarroceria='{19}'&p_TarifaPintura='{20}'&p_Operacion='{21}'"

        Dim url As String = String.Format(urlFormat, p_AdministradorExpediente, p_NumeroExpediente, p_NombreAsegurado,
                                            p_ApellidoAsegurado, p_FechaAccidente, p_DescripcionAccidente,
                                            p_Placas, p_Fabricante, p_AnioFabricacion, p_Poliza,
                                            p_VIN, p_Registro, p_UsuarioAsignado, p_PoolAsignado,
                                            p_Taller, p_CompaniaAsignada, p_CompaniaOrigen,
                                            p_CreadoPor, p_NumeroCaso, p_TarifaCarroceria,
                                            p_TarifaPintura, p_Operacion)

        Try
            Dim myHttpWebRequest As HttpWebRequest = DirectCast(HttpWebRequest.Create(url), HttpWebRequest)
            myHttpWebRequest.Method = "POST"
            myHttpWebRequest.Headers.Add("CompanyDB", "SBO_SAIS")
            myHttpWebRequest.Headers.Add("UserName", "manager")
            myHttpWebRequest.Headers.Add("Password", "B1Admin")

            Dim data As Byte() = Encoding.ASCII.GetBytes("")
            myHttpWebRequest.ContentLength = data.Length
            Dim myHttpWebResponse As HttpWebResponse = DirectCast(myHttpWebRequest.GetResponse(), HttpWebResponse)
            Dim responseStream As Stream = myHttpWebResponse.GetResponseStream()
            Dim myStreamReader As New StreamReader(responseStream, Encoding.[Default])

            ' Almacena el mensaje de respuesta del WS
            Dim pageContent As String = myStreamReader.ReadToEnd()

            myStreamReader.Close()
            responseStream.Close()
            myHttpWebResponse.Close()

            Return pageContent

        Catch ex As Exception
            Dim mensaje As String = ex.Message
        End Try
    End Function

    '
    ' Llama una funcion para obtener información de un expediente de Audatex
    '         
    Public Sub GetExpedient(p_NumOT As String, p_Operacion As String, p_Evento As String, p_FechaIni As String, p_FechaFin As String)
        Dim urlFormat As String = "http://localhost:55248/Audatex?numOT='{0}'&operacion='{1}'&evento='{2}'&fechaIni='{3}'&fechaFin='{4}'"
        Dim url As String = String.Format(urlFormat, p_NumOT, p_Operacion, p_Evento, p_FechaIni, p_FechaFin)

        Dim webrequest As WebRequest = DirectCast(System.Net.WebRequest.Create(url), HttpWebRequest)
        webrequest.Headers.Add("CompanyDB", "SBO_SAIS")
        webrequest.Headers.Add("UserName", "manager")
        webrequest.Headers.Add("Password", "B1Admin")

        Try
            Dim response As WebResponse = webrequest.GetResponse()
            Dim reader As Stream = response.GetResponseStream()
            Dim myStreamReader As New StreamReader(reader, Encoding.[Default])

            ' Almacena el mensaje de respuesta del WS
            Dim pageContent As String = myStreamReader.ReadToEnd()

            Dim mensaje As String = ""

        Catch ex As Exception
            Dim mensaje As String = ex.Message
        End Try
    End Sub
End Class
