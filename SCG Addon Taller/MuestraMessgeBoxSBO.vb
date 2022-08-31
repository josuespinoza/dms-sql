
Imports SAPbouiCOM

Public Class MuestraMessgeBoxSBO
    Private _sboApplication As SAPbouiCOM.Application

    Public Sub New(ByVal sboApplication As Application)
        _sboApplication = sboApplication
    End Sub

    Public Function MessageBxPreg(ByVal mensaje As String) As Boolean
        Return _sboApplication.MessageBox(Text:=mensaje, Btn1Caption:=My.Resources.Resource.Si, Btn2Caption:="No", Btn3Caption:="") = 1
    End Function

    Public Sub MessageBxExc(ByVal mensaje As String)
        _sboApplication.MessageBox(Text:=mensaje, Btn1Caption:="Ok")
    End Sub

End Class
