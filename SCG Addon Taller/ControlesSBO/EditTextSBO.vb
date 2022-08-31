Option Strict On
Option Explicit On

Imports SAPbouiCOM

Namespace ControlesSBO


    <CLSCompliant(False)> _
    Public Class EditTextSBO
        Inherits ControlSBO
        Implements ISBOBindable

        Protected _editText As IEditText

        Public Sub New(ByVal itemSBO As IItem, ByVal permitirBuscar As Boolean)
            MyBase.New(itemSBO, permitirBuscar)
        End Sub

        Protected Overrides Sub AsignaControlEspecifico()
            _editText = DirectCast(_itemSBO.Specific, IEditText)
        End Sub

        Public Sub AsignarBinding(ByVal tabla As String, ByVal columna As String) Implements ISBOBindable.AsignarBinding
            _editText.DataBind.Bind(UID:=tabla, columnUid:=columna)
        End Sub

        Public ReadOnly Property EditText() As IEditText
            Get
                Return _editText
            End Get
        End Property
    End Class
End Namespace