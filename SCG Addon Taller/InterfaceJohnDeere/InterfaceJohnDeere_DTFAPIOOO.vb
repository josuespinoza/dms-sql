Imports RestSharp
Public Class InterfaceJohnDeere_DTFAPIOOO

    Public Sub ManejaInterfaceJohnDeere_DTFAPI()
        Try
            Dim client1 As RestClient = New RestClient("https://servicesext.deere.com/dtfapi/dbs/dealer/282585/files")
            client1.Timeout = -1
            Dim request1 As RestRequest = New RestRequest(Method.GET)
            request1.AddHeader("Authorization", "Bearer eyJraWQiOiI2Q3QzMTFLd2N2a180QzkyOXJhbVNGbFNma042Nlh2QkhJdU9iUGtaX3pNIiwiYWxnIjoiUlMyNTYifQ.eyJ2ZXIiOjEsImp0aSI6IkFULlJVOWF4Z0wxLUtrNk9mYjQ5aUJzYk5pSlNmUlNzTUFINWM5YlVCa0lwZ1UiLCJpc3MiOiJodHRwczovL3Nzby5qb2huZGVlcmUuY29tL29hdXRoMi9hdXM5azBmYjhrVWpHOFM1WjF0NyIsImF1ZCI6ImNoYW5uZWwtY2FwIiwiaWF0IjoxNjUwNTU2Mzc0LCJleHAiOjE2NTA1NTk5NzQsImNpZCI6IjBvYWx2Ym1rM2k4MHd5dE9SMXQ3Iiwic2NwIjpbImR0ZjpkYnM6ZmlsZTp3cml0ZSIsImR0ZjpkYnM6ZmlsZTpyZWFkIl0sInN1YiI6IjBvYWx2Ym1rM2k4MHd5dE9SMXQ3In0.bbTDafcF-netqnzKuV7iPitQE54qRW90BXw4zjQfUBOGSwwrq3DdAzwT4gBARL18My0h1Fv_SAJ1g2mwnADdrEdSCaeSDkuOZlrSxsa7pIleNSOG39fX9igw4uizxXCc-NE8ND5s9JOD8xhK8gVtpgDXk0m3gnchFVviIqiMrBK9B-CRuFMvgzQc0GHTw4d78zXGo5bGLbcsG6XG6ly8i9O-A8bGtv0MPr6SmibjfIBpjjHYoId6bnLftx0G3mHSIYAwQVs2iQW3ZAW2xWXv1HrCYVkbD9phCJWCZPCy5ulcjyFV_XsvVSPGOvOfqfQRmaAbrz8w89bDxcBCwEwpEA")
            Dim response As IRestResponse = client1.Execute(request1)
            Console.WriteLine(response.Content)
        Catch ex As Exception

        End Try
    End Sub
End Class
