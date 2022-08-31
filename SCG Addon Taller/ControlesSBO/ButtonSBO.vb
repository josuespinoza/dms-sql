Option Strict On
Option Explicit On

Imports SAPbouiCOM

Namespace ControlesSBO


    <CLSCompliant(False)> _
    Public Class ButtonSBO
        Inherits ControlSBO

        Private _button As IButton

        Public Sub New(ByVal itemSBO As IItem, ByVal permitirBuscar As Boolean)
            MyBase.New(itemSBO, permitirBuscar)
        End Sub

        Protected Overrides Sub AsignaControlEspecifico()
            _button = DirectCast(ItemSBO.Specific, IButton)
        End Sub

        Public ReadOnly Property Button() As IButton
            Get
                Return _button
            End Get
        End Property
    End Class
End Namespace