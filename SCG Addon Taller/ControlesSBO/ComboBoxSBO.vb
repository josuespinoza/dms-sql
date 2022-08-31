Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports SAPbouiCOM

Namespace ControlesSBO


    <CLSCompliant(False)> _
    Public Class ComboBoxSBO
        Inherits ControlSBO
        Implements ISBOBindable

        Private _comboBox As IComboBox

        Public Sub New(ByVal itemSBO As IItem, ByVal permitirBuscar As Boolean)
            MyBase.New(itemSBO, permitirBuscar)
        End Sub

        Public Sub AsignarBinding(ByVal tabla As String, ByVal columna As String) Implements ISBOBindable.AsignarBinding
            _comboBox.DataBind.Bind(UID:=tabla, columnUid:=columna)
        End Sub

        Protected Overrides Sub AsignaControlEspecifico()
            _comboBox = DirectCast(_itemSBO.Specific, IComboBox)
        End Sub

        Public Overridable Sub CargaValoresValidos(ByVal valoresValidos As IEnumerable(Of ValorValidoSBO), Optional ByVal seleccionarPrimero As Boolean = True)
            If _comboBox IsNot Nothing Then
                For Each valorValido As ValorValidoSBO In valoresValidos
                    _comboBox.ValidValues.Add(valorValido.Value, valorValido.Description)
                Next
                If _comboBox.ValidValues.Count <> 0 AndAlso seleccionarPrimero Then _comboBox.Select(0, BoSearchKey.psk_Index)
            End If
        End Sub

        Public ReadOnly Property ComboBox() As IComboBox
            Get
                Return _comboBox
            End Get
        End Property

    End Class
End Namespace