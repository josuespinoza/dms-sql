Option Strict On
Option Explicit On

Imports SAPbouiCOM

Namespace ControlesSBO


    <CLSCompliant(False)> _
    Public MustInherit Class ControlSBO

        Protected _itemSBO As IItem

        Protected Sub New(ByVal itemSBO As IItem, ByVal permitirBuscar As Boolean)
            _itemSBO = itemSBO
            AsignaControlEspecifico()
            PermitirModoBuscar(permitirBuscar)
        End Sub

        Protected MustOverride Sub AsignaControlEspecifico()

        Protected Overridable Sub PermitirModoBuscar(ByVal permitirBuscar As Boolean)

            If permitirBuscar Then
                _itemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            Else
                _itemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            End If

        End Sub

        Public Property ItemSBO() As IItem
            Get
                Return _itemSBO
            End Get
            Set(ByVal value As IItem)
                _itemSBO = value
            End Set
        End Property

    End Class
End Namespace