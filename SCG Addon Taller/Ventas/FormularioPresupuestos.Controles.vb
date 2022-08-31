Option Strict On
Option Explicit On

Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Namespace Ventas

    Partial Class FormularioPresupuestos
        'matriz de unidades para presupuestso
        Private _mtxPresUnidades As ControlesSBO.MatrixSBO
        Private _mtxPresupuestos As ControlesSBO.MatrixSBO
        Private _cbClsf1 As ControlesSBO.ComboBoxSBO
        Private _txtCode As ControlesSBO.EditTextSBO
        Private _txtAno As ControlesSBO.EditTextSBO
        Private _cbMesIni As ControlesSBO.ComboBoxSBO
        Private _cbDash As ControlesSBO.ComboBoxSBO
        'Private _cbClsf2 As ControlesSBO.ComboBoxSBO
        'Private _cbClsf3 As ControlesSBO.ComboBoxSBO
        Private _cbClsf4 As ControlesSBO.ComboBoxSBO
        Private _cbMarca As ControlesSBO.ComboBoxSBO
        Private _lblMes1 As ControlesSBO.StaticTextSBO
        Private _lblMes2 As ControlesSBO.StaticTextSBO
        Private _lblMes3 As ControlesSBO.StaticTextSBO
        Private _lblMes4 As ControlesSBO.StaticTextSBO
        Private _lblMes5 As ControlesSBO.StaticTextSBO
        Private _lblMes6 As ControlesSBO.StaticTextSBO
        Private _lblMes7 As ControlesSBO.StaticTextSBO
        Private _lblMes8 As ControlesSBO.StaticTextSBO
        Private _lblMes9 As ControlesSBO.StaticTextSBO
        Private _lblMes10 As ControlesSBO.StaticTextSBO
        Private _lblMes11 As ControlesSBO.StaticTextSBO
        Private _lblMes12 As ControlesSBO.StaticTextSBO
        Private _txtMes1 As ControlesSBO.EditTextSBO
        Private _txtMes2 As ControlesSBO.EditTextSBO
        Private _txtMes3 As ControlesSBO.EditTextSBO
        Private _txtMes4 As ControlesSBO.EditTextSBO
        Private _txtMes5 As ControlesSBO.EditTextSBO
        Private _txtMes6 As ControlesSBO.EditTextSBO
        Private _txtMes7 As ControlesSBO.EditTextSBO
        Private _txtMes8 As ControlesSBO.EditTextSBO
        Private _txtMes9 As ControlesSBO.EditTextSBO
        Private _txtMes10 As ControlesSBO.EditTextSBO
        Private _txtMes11 As ControlesSBO.EditTextSBO
        Private _txtMes12 As ControlesSBO.EditTextSBO
        Private _btCalc As ControlesSBO.ButtonSBO

        Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles
            If _sboForm IsNot Nothing Then
                _mtxPresupuestos = New ControlesSBO.MatrixSBO(_sboForm.Items.Item("mtxPresp"), True)
                'Manejo de la matriz de unidades para presupuestos
                _mtxPresUnidades = New ControlesSBO.MatrixSBO(_sboForm.Items.Item("mtxUnidad"), True)

                _cbClsf1 = New ControlesSBO.ComboBoxSBO(_sboForm.Items.Item("cbClasf1"), True)
                '_cbClsf2 = New ControlesSBO.ComboBoxSBO(_sboForm.Items.Item("cbClasf2"), True)
                '_cbClsf3 = New ControlesSBO.ComboBoxSBO(_sboForm.Items.Item("cbClasf3"), True)
                _cbClsf4 = New ControlesSBO.ComboBoxSBO(_sboForm.Items.Item("cbClasf4"), True)
                _cbMarca = New ControlesSBO.ComboBoxSBO(_sboForm.Items.Item("cbMarca"), True)
                _cbMesIni = New ControlesSBO.ComboBoxSBO(_sboForm.Items.Item("cbMesIni"), True)
                _cbDash = New ControlesSBO.ComboBoxSBO(_sboForm.Items.Item("cbDash"), True)
                _txtAno = New ControlesSBO.EditTextSBO(_sboForm.Items.Item("txtAno"), True)
                _txtCode = New ControlesSBO.EditTextSBO(_sboForm.Items.Item("txtCode"), True)
                _lblMes1 = New ControlesSBO.StaticTextSBO(_sboForm.Items.Item("lblMes1"), False)
                _lblMes2 = New ControlesSBO.StaticTextSBO(_sboForm.Items.Item("lblMes2"), False)
                _lblMes3 = New ControlesSBO.StaticTextSBO(_sboForm.Items.Item("lblMes3"), False)
                _lblMes4 = New ControlesSBO.StaticTextSBO(_sboForm.Items.Item("lblMes4"), False)
                _lblMes5 = New ControlesSBO.StaticTextSBO(_sboForm.Items.Item("lblMes5"), False)
                _lblMes6 = New ControlesSBO.StaticTextSBO(_sboForm.Items.Item("lblMes6"), False)
                _lblMes7 = New ControlesSBO.StaticTextSBO(_sboForm.Items.Item("lblMes7"), False)
                _lblMes8 = New ControlesSBO.StaticTextSBO(_sboForm.Items.Item("lblMes8"), False)
                _lblMes9 = New ControlesSBO.StaticTextSBO(_sboForm.Items.Item("lblMes9"), False)
                _lblMes10 = New ControlesSBO.StaticTextSBO(_sboForm.Items.Item("lblMes10"), False)
                _lblMes11 = New ControlesSBO.StaticTextSBO(_sboForm.Items.Item("lblMes11"), False)
                _lblMes12 = New ControlesSBO.StaticTextSBO(_sboForm.Items.Item("lblMes12"), False)
                _txtMes1 = New ControlesSBO.EditTextSBO(_sboForm.Items.Item("txtMes1"), False)
                _txtMes2 = New ControlesSBO.EditTextSBO(_sboForm.Items.Item("txtMes2"), False)
                _txtMes3 = New ControlesSBO.EditTextSBO(_sboForm.Items.Item("txtMes3"), False)
                _txtMes4 = New ControlesSBO.EditTextSBO(_sboForm.Items.Item("txtMes4"), False)
                _txtMes5 = New ControlesSBO.EditTextSBO(_sboForm.Items.Item("txtMes5"), False)
                _txtMes6 = New ControlesSBO.EditTextSBO(_sboForm.Items.Item("txtMes6"), False)
                _txtMes7 = New ControlesSBO.EditTextSBO(_sboForm.Items.Item("txtMes7"), False)
                _txtMes8 = New ControlesSBO.EditTextSBO(_sboForm.Items.Item("txtMes8"), False)
                _txtMes9 = New ControlesSBO.EditTextSBO(_sboForm.Items.Item("txtMes9"), False)
                _txtMes10 = New ControlesSBO.EditTextSBO(_sboForm.Items.Item("txtMes10"), False)
                _txtMes11 = New ControlesSBO.EditTextSBO(_sboForm.Items.Item("txtMes11"), False)
                _txtMes12 = New ControlesSBO.EditTextSBO(_sboForm.Items.Item("txtMes12"), False)
                _btCalc = New ControlesSBO.ButtonSBO(_sboForm.Items.Item("btCalc"), False)

                'manejo de presupuestos para montos 
                _mtxPresupuestos.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                _mtxPresupuestos.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_Add Or BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False)
                'manejo de presupuestos para unidades 
                _mtxPresUnidades.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                _mtxPresUnidades.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_Add Or BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False)

                _cbMarca.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                _cbMarca.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_Add Or BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True)
                _cbClsf1.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                _cbClsf1.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_Add Or BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True)
                '_cbClsf2.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                '_cbClsf2.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_Add Or BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True)
                '_cbClsf3.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                '_cbClsf3.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_Add Or BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True)
                _cbClsf4.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                _cbClsf4.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_Add Or BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True)
                _cbMesIni.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                _cbMesIni.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_Add Or BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True)
                _txtAno.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                _txtAno.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_Add Or BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True)
                _txtCode.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                _txtCode.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True)

                _btCalc.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                _btCalc.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_False)
                _btCalc.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_Add, BoModeVisualBehavior.mvb_False)

                Inicializado = True
            End If
        End Sub
    End Class

End Namespace