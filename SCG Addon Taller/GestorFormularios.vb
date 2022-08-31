Option Strict On
Option Explicit On

Imports DMS_Addon.ControlesSBO
Imports System.Net
Imports System.IO
Imports SAPbouiCOM
Imports System.Xml
Imports SCG.SBOFramework.UI

<CLSCompliant(False)> _
Public Class GestorFormularios
    Private _sboApplication As Application

    Public Sub New(ByVal sboApplication As Application)
        _sboApplication = sboApplication
    End Sub

    Public Function FormularioAbierto(ByVal formulario As IFormularioSBO, ByVal activarSiEstaAbierto As Boolean) As Boolean
        Dim sboForm As Form

        For indice As Integer = 0 To _sboApplication.Forms.Count - 1
            sboForm = _sboApplication.Forms.Item(indice)
            If sboForm.TypeEx = formulario.FormType Then
                If activarSiEstaAbierto Then sboForm.Select()
                Return True
            End If
        Next
        Return False

    End Function

    Public Function CargaFormulario(ByVal formulario As IFormularioSBO) As SAPbouiCOM.Form

        Dim fcp As FormCreationParams

        fcp = _
            DirectCast(_sboApplication.CreateObject(BoCreatableObjectType.cot_FormCreationParams),  _
                FormCreationParams)
        fcp.FormType = formulario.FormType
        Dim sboForm As Form = CargarDesdeXML(fcp, formulario)
        formulario.FormularioSBO = sboForm
        formulario.Inicializado = False
        formulario.InicializarControles()
        formulario.InicializaFormulario()

        Return sboForm

    End Function

    Private Function CargarDesdeXML(ByVal fcp As FormCreationParams, ByVal formulario As IFormularioSBO) As SAPbouiCOM.Form
        fcp.XmlData = File.ReadAllText(formulario.NombreXml)
        Return _sboApplication.Forms.AddEx(fcp)
    End Function

End Class