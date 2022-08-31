Option Strict On
Option Explicit On

Imports SAPbouiCOM

Namespace ControlesSBO
    Public Class StaticTextSBO
        Inherits ControlSBO

        Protected _staticText As IStaticText

        Public Sub New(ByVal itemSBO As IItem, ByVal permitirBuscar As Boolean)
            MyBase.New(itemSBO, permitirBuscar)
        End Sub

        Protected Overrides Sub AsignaControlEspecifico()
            _staticText = DirectCast(_itemSBO.Specific, IStaticText)
        End Sub

        Public ReadOnly Property StaticText() As IStaticText
            Get
                Return _staticText
            End Get
        End Property
    End Class
End Namespace