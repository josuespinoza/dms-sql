'Agregado 15072010

Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports ManipuladorClienteDLL

Namespace SCGCommon

    Public Module ManejoErroresEH

        Public Sub ManejoErrores(ByVal ex As Exception, ByVal nombreCompañia As String, ByVal cultura As String, ByVal form As IWin32Window, ByVal metodoMessageBoxPregunta As MetodoMessageBoxPregunta, ByVal metodoMessageBoxExc As MetodoMessageBoxExclamacion, ByVal skin As TipoSkin)

            Dim oErrorHandler As New ManipuladorClienteDLL.ManipuladorExcepciones("SCG DMS One", nombreCompañia)
            Dim path As String = Application.StartupPath & "\"
            Dim mensaje As String

            If metodoMessageBoxExc Is Nothing OrElse metodoMessageBoxPregunta Is Nothing Then
                mensaje = oErrorHandler.ManipularExcepcion(ex, cultura, path)
            Else
                mensaje = oErrorHandler.ManipularExcepcion(ex, cultura, path, metodoMessageBoxPregunta, metodoMessageBoxExc)
            End If

            Dim oError As ManipuladorClienteDLL.MensajeDeError

            Dim detalleMensaje As string = ex.Message

            Dim comEx As COMException = TryCast(ex, COMException)

            If comEx isnot Nothing then
                detalleMensaje = String.Format("{0}: {1}", comEx.ErrorCode, comEx.Message)
            End If

            Dim customSBOEx As ExceptionsSBO = TryCast(ex, ExceptionsSBO)

            If customSBOEx isnot Nothing then
                detalleMensaje = String.Format("{0}: {1}", customSBOEx.Codigo, customSBOEx.Message)
            End If

            If Not String.IsNullOrEmpty(mensaje) Then
                oError = New ManipuladorClienteDLL.MensajeDeError(mensaje, detalleMensaje, skin)
            Else
                oError = New ManipuladorClienteDLL.MensajeDeError(ex.Message, detalleMensaje, skin)
            End If

            If form Is Nothing Then
                oError.ShowDialog()
            Else
                oError.ShowDialog(form)
            End If

        End Sub

        Public Sub ManejoErrores(ByRef ex As Exception, ByVal nombreCompañia As String, ByVal tipoSkin As Integer)
            ManejoErrores(ex, nombreCompañia, System.Threading.Thread.CurrentThread.CurrentUICulture.IetfLanguageTag, Nothing, Nothing, Nothing, tipoSkin)
        End Sub

    End Module

End Namespace
