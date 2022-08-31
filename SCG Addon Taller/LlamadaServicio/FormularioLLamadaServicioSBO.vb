'Option Strict On
Option Explicit On

Imports DMS_Addon.Agendas
Imports DMSOneFramework
Imports System.Runtime.InteropServices
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.SCGDataAccess
Imports System.Data.SqlClient
Imports DMSOneFramework.CitasTableAdapters
Imports SCG_User_Interface.SCG_User_Interface
Imports SCG.WinFormsSAP

Namespace LlamadaServicio
    Public Class FormularioLLamadaServicioSBO
        Public Shared FormType As String = "60110"
        Private _sboCompany As SAPbobsCOM.Company
        Private WithEvents _sboApplication As Application
        Private _sboForm As Form
        Private _dstCitas As Citas = New Citas()

        Public Shared HuboError As Boolean = False

        Public Shared NombreTabla As String = "OSCL"
        Public Shared CFLVEhiculos As String = "SCGD_CFLVEH"
        Public Shared NombreTablaVehiculos As String = "SCGD_VEH"

        Public Shared UDF_Placa As String = "U_SCGD_Placa"
        Protected Shared ConnectionStringTaller As String = String.Empty
        Protected Shared ConnectionStringSBO As String = String.Empty

        Public Shared PaneVehiculos As Integer = 9
        Public Shared ItemPaneVehiculos As String = "SCGD_FDVeh"
        Public Shared ItemPaneHistorial As String = "158"
        Public Shared LabelPlaca As String = "SCGD_lbPla"
        Public Shared LabelMarca As String = "SCGD_lbMrc"
        Public Shared LabelEstilo As String = "SCGD_lbEst"
        Public Shared LabelVin As String = "SCGD_lbVin"
        Public Shared LabelNoUnidad As String = "SCGD_lbNoU"
        Public Shared LabelModelo As String = "SCGD_lbMod"
        Public Shared LabelAgenda As String = "SCGD_lbAge"
        Public Shared LabelFechaCita As String = "SCGD_lbFcC"
        Public Shared LabelCita As String = "SCGD_lbCit"
        Public Shared LabelRazonCita As String = "SCGD_lbRaz"
        Public Shared LabelGeneraCita As String = "SCGD_lbGeC"
        Public Shared LabelCotizacion As String = "SCGD_lbCot"

        Public Shared TxtPlaca As String = "SCGD_txPla"
        Public Shared TxtMarca As String = "SCGD_txMrc"
        Public Shared TxtEstilo As String = "SCGD_txEst"
        Public Shared TxtVin As String = "SCGD_txVin"
        Public Shared TxtNoUnidad As String = "SCGD_txNoU"
        Public Shared TxtModelo As String = "SCGD_txMod"
        Public Shared TxtCardCode As String = "14"
        Public Shared TxtCardName As String = "79"
        Public Shared TxtFechaCita As String = "SCGD_txFcC"
        Public Shared TxtHoraCita As String = "SCGD_txHrC"
        Public Shared TxtCita As String = "SCGD_txCit"
        Public Shared TxtObervaciones As String = "6"

        Public Shared CbAgenda As String = "SCGD_cbAge"
        Public Shared CbRazonCita As String = "SCGD_cbRaz"
        Public Shared CbGeneraCita As String = "SCGD_cbGeC"

        Public Shared TxtIdVehiculo As String = "SCGD_txIdV"
        Public Shared TxtCodMarca As String = "SCGD_txCoM"
        Public Shared TxtCodEstilo As String = "SCGD_txCoE"
        Public Shared TxtCodModelo As String = "SCGD_txCMd"
        Public Shared TxtIdCita As String = "SCGD_txIdC"
        Public Shared TxtCotizacion As String = "SCGD_txCot"

        Public Shared LinkCotizacion As String = "SCGD_lkCot"
        Public Shared LinkVehiculo As String = "SCGD_lkVeh"

        Public Shared BtnAgenda As String = "SCGD_btAge"

        Public Shared LabelReferencia As String = "68"

        Public dtVehiculo As New DatosVehiculo()

        Private WithEvents _frmAgendaDotNet As frmCalendarioAgenda

        <CLSCompliant(False)> _
        Public Sub New(ByVal sboApplication As Application, ByVal sboCompany As SAPbobsCOM.Company)
            _sboCompany = sboCompany
            _sboApplication = sboApplication

            If String.IsNullOrEmpty(ConnectionStringTaller) Then
                Utilitarios.DevuelveCadenaConexionBDTaller(_sboApplication, ConnectionStringTaller)
            End If

            If String.IsNullOrEmpty(ConnectionStringSBO) Then
                Configuracion.CrearCadenaDeconexion(_sboCompany.Server, _sboCompany.CompanyDB, ConnectionStringSBO)
            End If

        End Sub

        Public Sub CargaTabVehiculos()
            Dim sboItem As Item
            Dim sboItemNuevo As Item
            Dim sboFolder As Folder
            Dim sboLabel As StaticText
            Dim sboEdit As EditText
            Dim sboCombo As ComboBox
            Dim sboButton As Button
            Dim sboLink As LinkedButton
            Dim left As Integer
            Dim top As Integer
            Dim leftInicio As Integer
            Dim leftInicio2 As Integer = leftInicio + 230 + 20
            Dim topInicio As Integer

            ''Variables para el manejo de la posicion del linked button de la Unidad
            'Dim topLinkedU As Integer
            'Dim leftLinkedU As Integer

            sboItem = _sboForm.Items.Item(LabelReferencia)
            left = sboItem.Left
            top = sboItem.Top

            leftInicio = left
            topInicio = top

            'Agrega Tab
            sboItem = _sboForm.Items.Item(ItemPaneHistorial)
            sboItemNuevo = _sboForm.Items.Add(ItemPaneVehiculos, BoFormItemTypes.it_FOLDER)
            sboItemNuevo.Left = sboItem.Left + sboItem.Width
            sboItemNuevo.Width = 30
            sboItemNuevo.Top = sboItem.Top - 10
            sboItemNuevo.Height = sboItem.Height
            sboItemNuevo.AffectsFormMode = False

            sboFolder = DirectCast(sboItemNuevo.Specific, Folder)
            sboFolder.Caption = My.Resources.Resource.Vehiculo
            sboFolder.GroupWith(ItemPaneHistorial)

            'Label Placa
            sboItem = _sboForm.Items.Add(LabelPlaca, BoFormItemTypes.it_STATIC)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 100
            sboItem.Visible = True
            sboLabel = DirectCast(sboItem.Specific, StaticText)
            sboLabel.Caption = My.Resources.Resource.CapPlaca

            'EditText Placa
            left = left + 100
            sboItem = _sboForm.Items.Add(TxtPlaca, BoFormItemTypes.it_EDIT)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 130
            sboItem.Visible = True
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.DataBind.SetBound(True, NombreTabla, "U_SCGD_Placa")
            sboEdit.ChooseFromListUID = CFLVEhiculos
            sboEdit.ChooseFromListAlias = "U_Num_Plac"
            sboItem = _sboForm.Items.Item(LabelPlaca)
            sboItem.LinkTo = TxtPlaca

            ''Link
            'sboItem = _sboForm.Items.Add(LinkVehiculo, BoFormItemTypes.it_LINKED_BUTTON)
            'sboItem.Left = left - 15
            'sboItem.Height = 9
            'sboItem.FromPane = PaneVehiculos
            'sboItem.ToPane = PaneVehiculos
            'sboItem.Top = top + 2
            'sboItem.Width = 10
            'sboItem.Enabled = True
            'sboItem.Visible = True
            'sboItem.LinkTo = TxtNoUnidad
            '' sboItem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            'sboLink = DirectCast(sboItem.Specific, LinkedButton)
            ''Se guarda la posición en la que debe ir el linkedbutton para asignarla despues de crear el campo al que va asociado
            'topLinkedU = top
            'leftLinkedU = left


            top = top + 20

            'Label Marca
            left = leftInicio
            sboItem = _sboForm.Items.Add(LabelMarca, BoFormItemTypes.it_STATIC)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 100
            sboItem.Visible = True
            sboLabel = DirectCast(sboItem.Specific, StaticText)
            sboLabel.Caption = My.Resources.Resource.CapMarca

            'EditText Marca
            left = left + 100
            sboItem = _sboForm.Items.Add(TxtMarca, BoFormItemTypes.it_EDIT)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 130
            sboItem.Enabled = False
            sboItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            sboItem.Visible = True
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.DataBind.SetBound(True, NombreTabla, "U_SCGD_D_Marca")
            sboItem = _sboForm.Items.Item(LabelMarca)
            sboItem.LinkTo = TxtMarca

            top = top + 20

            'Label Estilo
            left = leftInicio
            sboItem = _sboForm.Items.Add(LabelEstilo, BoFormItemTypes.it_STATIC)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 100
            sboItem.Visible = True
            sboLabel = DirectCast(sboItem.Specific, StaticText)
            sboLabel.Caption = My.Resources.Resource.CapEstilo

            'EditText Estilo
            left = left + 100
            sboItem = _sboForm.Items.Add(TxtEstilo, BoFormItemTypes.it_EDIT)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 130
            sboItem.Enabled = False
            sboItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            sboItem.Visible = True
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.DataBind.SetBound(True, NombreTabla, "U_SCGD_D_Est")
            sboItem = _sboForm.Items.Item(LabelEstilo)
            sboItem.LinkTo = TxtEstilo

            top = top + 20

            'label Agenda
            left = leftInicio
            sboItem = _sboForm.Items.Add(LabelAgenda, BoFormItemTypes.it_STATIC)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 100
            sboItem.Visible = True
            sboLabel = DirectCast(sboItem.Specific, StaticText)
            sboLabel.Caption = My.Resources.Resource.Agenda

            'Combo Agenda
            left = left + 100
            sboItem = _sboForm.Items.Add(CbAgenda, BoFormItemTypes.it_COMBO_BOX)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 130
            sboItem.Enabled = True
            sboItem.DisplayDesc = True
            sboItem.Visible = True
            sboCombo = DirectCast(sboItem.Specific, ComboBox)
            CargaComboAgenda(sboCombo)
            sboCombo.DataBind.SetBound(True, NombreTabla, "U_SCGD_Agenda")
            If sboCombo.Selected Is Nothing Then sboCombo.Select(0, BoSearchKey.psk_Index)
            sboItem = _sboForm.Items.Item(LabelAgenda)
            sboItem.LinkTo = CbAgenda

            top = top + 20

            'label Razon
            left = leftInicio
            sboItem = _sboForm.Items.Add(LabelRazonCita, BoFormItemTypes.it_STATIC)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 100
            sboItem.Visible = True
            sboLabel = DirectCast(sboItem.Specific, StaticText)
            sboLabel.Caption = My.Resources.Resource.RazonCita

            'Combo Razon
            left = left + 100
            sboItem = _sboForm.Items.Add(CbRazonCita, BoFormItemTypes.it_COMBO_BOX)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 130
            sboItem.Enabled = True
            sboItem.DisplayDesc = True
            sboItem.Visible = True
            sboCombo = DirectCast(sboItem.Specific, ComboBox)
            CargaComboRazon(sboCombo)
            sboCombo.DataBind.SetBound(True, NombreTabla, "U_SCGD_Razon")
            If sboCombo.Selected Is Nothing Then sboCombo.Select(0, BoSearchKey.psk_Index)
            sboItem = _sboForm.Items.Item(LabelRazonCita)
            sboItem.LinkTo = CbRazonCita

            top = top + 20

            'Label Cotizacion
            left = leftInicio
            sboItem = _sboForm.Items.Add(LabelCotizacion, BoFormItemTypes.it_STATIC)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 100
            sboItem.Visible = True
            sboLabel = DirectCast(sboItem.Specific, StaticText)
            sboLabel.Caption = My.Resources.Resource.CapNoCotizacion

            'EditText Cotizacion
            left = left + 100
            sboItem = _sboForm.Items.Add(TxtCotizacion, BoFormItemTypes.it_EDIT)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 130
            sboItem.Enabled = False
            sboItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            sboItem.Visible = True
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.DataBind.SetBound(True, NombreTabla, "U_SCGD_Cotiz")
            sboItem = _sboForm.Items.Item(LabelCotizacion)
            sboItem.LinkTo = TxtCotizacion

            'Link
            sboItem = _sboForm.Items.Add(LinkCotizacion, BoFormItemTypes.it_LINKED_BUTTON)
            sboItem.Left = left - 15
            sboItem.FromPane = PaneVehiculos
            sboItem.Height = 9
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top + 2
            sboItem.Width = 10
            sboItem.Enabled = True
            sboLink = DirectCast(sboItem.Specific, LinkedButton)
            sboLink.LinkedObject = BoLinkedObject.lf_Quotation
            sboItem.LinkTo = TxtCotizacion
            'Segunda Columna

            top = topInicio

            'Label Modelo
            left = leftInicio2
            sboItem = _sboForm.Items.Add(LabelModelo, BoFormItemTypes.it_STATIC)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 100
            sboItem.Visible = True
            sboLabel = DirectCast(sboItem.Specific, StaticText)
            sboLabel.Caption = My.Resources.Resource.CapModelo

            'EditText Modelo
            left = left + 100
            sboItem = _sboForm.Items.Add(TxtModelo, BoFormItemTypes.it_EDIT)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 130
            sboItem.Enabled = False
            sboItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            sboItem.Visible = True
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.DataBind.SetBound(True, NombreTabla, "U_SCGD_D_Mod")
            sboItem = _sboForm.Items.Item(LabelModelo)
            sboItem.LinkTo = TxtModelo

            top = top + 20
            'Label VIN
            left = leftInicio2
            sboItem = _sboForm.Items.Add(LabelVin, BoFormItemTypes.it_STATIC)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 100
            sboItem.Visible = True
            sboLabel = DirectCast(sboItem.Specific, StaticText)
            sboLabel.Caption = My.Resources.Resource.CapVIN

            'EditText VIN
            left = left + 100
            sboItem = _sboForm.Items.Add(TxtVin, BoFormItemTypes.it_EDIT)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 130
            sboItem.Enabled = False
            sboItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            sboItem.Visible = True
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.DataBind.SetBound(True, NombreTabla, "U_SCGD_Vin")
            sboItem = _sboForm.Items.Item(LabelVin)
            sboItem.LinkTo = TxtVin

            top = top + 20
            'Label No Unidad
            left = leftInicio2
            sboItem = _sboForm.Items.Add(LabelNoUnidad, BoFormItemTypes.it_STATIC)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 100
            sboItem.Visible = True
            sboLabel = DirectCast(sboItem.Specific, StaticText)
            sboLabel.Caption = My.Resources.Resource.CapNoUnidad

            'EditText No Unidad
            left = left + 100
            sboItem = _sboForm.Items.Add(TxtNoUnidad, BoFormItemTypes.it_EDIT)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 130
            sboItem.Enabled = False
            sboItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            sboItem.Visible = True
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.DataBind.SetBound(True, NombreTabla, "U_SCGD_NoUn")
            sboItem = _sboForm.Items.Item(LabelNoUnidad)
            sboItem.LinkTo = TxtNoUnidad



            sboItem = _sboForm.Items.Add(LinkVehiculo, BoFormItemTypes.it_LINKED_BUTTON)
            sboItem.Left = left - 15
            sboItem.Height = 9
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top + 2
            sboItem.Width = 10
            sboItem.Enabled = True
            sboItem.Visible = True
            sboItem.LinkTo = TxtNoUnidad
            ' sboItem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            sboLink = DirectCast(sboItem.Specific, LinkedButton)



            top = top + 20
            'label Fecha Cita
            left = leftInicio2
            sboItem = _sboForm.Items.Add(LabelFechaCita, BoFormItemTypes.it_STATIC)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 100
            sboItem.Visible = True
            sboLabel = DirectCast(sboItem.Specific, StaticText)
            sboLabel.Caption = My.Resources.Resource.FechaCita

            'EditText Fecha Cita
            left = left + 100
            sboItem = _sboForm.Items.Add(TxtFechaCita, BoFormItemTypes.it_EDIT)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.LinkTo = TxtHoraCita
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 80
            sboItem.Visible = True
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.DataBind.SetBound(True, NombreTabla, "U_SCGD_FcCita")

            'EditText Hora Cita
            left = left + 80 + 2
            sboItem = _sboForm.Items.Add(TxtHoraCita, BoFormItemTypes.it_EDIT)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 48
            sboItem.Visible = True
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.DataBind.SetBound(True, NombreTabla, "U_SCGD_HrCita")

            sboItem = _sboForm.Items.Item(LabelFechaCita)
            sboItem.LinkTo = TxtFechaCita

            sboItem = _sboForm.Items.Item(TxtFechaCita)
            sboItem.LinkTo = TxtHoraCita

            'Boton Agenda
            left = left + 48 + 2
            sboItem = _sboForm.Items.Add(BtnAgenda, BoFormItemTypes.it_BUTTON)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 15
            sboItem.Height = _sboForm.Items.Item(CbAgenda).Height
            sboItem.Visible = True
            sboButton = DirectCast(sboItem.Specific, Button)
            sboButton.Caption = "..."

            top = top + 20
            'Label Cita
            left = leftInicio2
            sboItem = _sboForm.Items.Add(LabelCita, BoFormItemTypes.it_STATIC)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 100
            sboItem.Visible = True
            sboLabel = DirectCast(sboItem.Specific, StaticText)
            sboLabel.Caption = My.Resources.Resource.CapCita

            'EditText No Cita
            left = left + 100
            sboItem = _sboForm.Items.Add(TxtCita, BoFormItemTypes.it_EDIT)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 130
            sboItem.Enabled = False
            sboItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            sboItem.Visible = True
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.DataBind.SetBound(True, NombreTabla, "U_SCGD_NoCita")
            sboItem = _sboForm.Items.Item(LabelCita)
            sboItem.LinkTo = TxtCita

            top = top + 20

            'label Genera Cita
            left = leftInicio2
            sboItem = _sboForm.Items.Add(LabelGeneraCita, BoFormItemTypes.it_STATIC)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 100
            sboItem.Visible = True
            sboLabel = DirectCast(sboItem.Specific, StaticText)
            sboLabel.Caption = My.Resources.Resource.CapGeneraCita

            'Combo Genera Cita
            left = left + 100
            sboItem = _sboForm.Items.Add(CbGeneraCita, BoFormItemTypes.it_COMBO_BOX)
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Top = top
            sboItem.Left = left
            sboItem.Width = 130
            sboItem.Enabled = True
            sboItem.DisplayDesc = True
            sboItem.Visible = True
            sboCombo = DirectCast(sboItem.Specific, ComboBox)
            sboCombo.DataBind.SetBound(True, NombreTabla, "U_SCGD_GenCita")
            If sboCombo.Selected Is Nothing Then sboCombo.Select(0, BoSearchKey.psk_ByValue)
            sboItem = _sboForm.Items.Item(LabelGeneraCita)
            sboItem.LinkTo = CbGeneraCita


            ''''''''' Otros Campos Ocultos
            sboItem = _sboForm.Items.Add(TxtIdVehiculo, BoFormItemTypes.it_EDIT)
            sboItem.Visible = True
            sboItem.AffectsFormMode = True
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.DataBind.SetBound(True, NombreTabla, "U_SCGD_IdVeh")
            sboItem.Left = 900
            sboItem.Top = 10
            ''''
            sboItem = _sboForm.Items.Add(TxtCodMarca, BoFormItemTypes.it_EDIT)
            sboItem.Visible = False
            sboItem.AffectsFormMode = True
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.DataBind.SetBound(True, NombreTabla, "U_SCGD_Cod_Marca")
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Left = 900
            sboItem.Top = 10
            ''''
            sboItem = _sboForm.Items.Add(TxtCodModelo, BoFormItemTypes.it_EDIT)
            sboItem.Visible = False
            sboItem.AffectsFormMode = True
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.DataBind.SetBound(True, NombreTabla, "U_SCGD_Cod_Modelo")
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Left = 900
            sboItem.Top = 10
            ''''
            sboItem = _sboForm.Items.Add(TxtCodEstilo, BoFormItemTypes.it_EDIT)
            sboItem.Visible = False
            sboItem.AffectsFormMode = True
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.DataBind.SetBound(True, NombreTabla, "U_SCGD_Cod_Estilo")
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Left = 900
            sboItem.Top = 10
            ''''
            sboItem = _sboForm.Items.Add(TxtIdCita, BoFormItemTypes.it_EDIT)
            sboItem.Visible = False
            sboItem.AffectsFormMode = True
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.DataBind.SetBound(True, NombreTabla, "U_SCGD_Cita")
            sboItem.FromPane = PaneVehiculos
            sboItem.ToPane = PaneVehiculos
            sboItem.Left = 900
            sboItem.Top = 10
            ''''



        End Sub

        Public Sub DeshabilitaFechaCita()
            ''''''''' Si ya hay cita deshabilitar la fecha y la hora
            If DMS_Connector.Configuracion.ParamGenAddon.U_OT_SAP.Trim().Equals("Y") Then
                Exit Sub
            End If

            If _sboForm IsNot Nothing Then

                Dim cita As String = _sboForm.DataSources.DBDataSources.Item(NombreTabla).GetValue("U_SCGD_Cita", 0)
                Dim sboItem As Item
                If Not String.IsNullOrEmpty(cita) Then
                    sboItem = _sboForm.Items.Item(TxtHoraCita)
                    sboItem.Enabled = False
                    sboItem = _sboForm.Items.Item(TxtFechaCita)
                    sboItem.Enabled = False
                    sboItem = _sboForm.Items.Item(BtnAgenda)
                    sboItem.Enabled = False
                    sboItem = _sboForm.Items.Item(TxtPlaca)
                    sboItem.Enabled = False
                    sboItem = _sboForm.Items.Item(CbAgenda)
                    sboItem.Enabled = False
                    sboItem = _sboForm.Items.Item(CbRazonCita)
                    sboItem.Enabled = False
                    sboItem = _sboForm.Items.Item(CbGeneraCita)
                    sboItem.Enabled = False

                Else
                    sboItem = _sboForm.Items.Item(TxtHoraCita)
                    sboItem.Enabled = True
                    sboItem = _sboForm.Items.Item(TxtFechaCita)
                    sboItem.Enabled = True
                    sboItem = _sboForm.Items.Item(BtnAgenda)
                    sboItem.Enabled = True
                    sboItem = _sboForm.Items.Item(TxtPlaca)
                    sboItem.Enabled = True
                    sboItem = _sboForm.Items.Item(CbAgenda)
                    sboItem.Enabled = True
                    sboItem = _sboForm.Items.Item(CbRazonCita)
                    sboItem.Enabled = True
                    sboItem = _sboForm.Items.Item(CbGeneraCita)
                    sboItem.Enabled = True

                End If
            End If

        End Sub

        <CLSCompliant(False)> _
        Public Sub CargaComboRazon(ByVal cb As ComboBox)
            Dim adapterRazones As SCGTA_TB_RazonesCitaTableAdapter = New SCGTA_TB_RazonesCitaTableAdapter()
            Dim razonesTabla As Citas.SCGTA_TB_RazonesCitaDataTable
            adapterRazones.Connection.ConnectionString = ConnectionStringTaller
            adapterRazones.Fill(_dstCitas.SCGTA_TB_RazonesCita)
            razonesTabla = _dstCitas.SCGTA_TB_RazonesCita
            If razonesTabla.Rows.Count <> 0 Then
                For Each row As Citas.SCGTA_TB_RazonesCitaRow In razonesTabla.Rows
                    cb.ValidValues.Add(row.NoRazon.ToString(), row.Descripcion)
                Next
                '                cb.Select(0, BoSearchKey.psk_Index)
            End If
        End Sub

        Private Sub CargaComboAgenda(ByVal cb As ComboBox)
            Dim adapterAgendas As SCGTA_TB_AgendaTableAdapter = New SCGTA_TB_AgendaTableAdapter()
            Dim agendasTabla As Citas.SCGTA_TB_AgendaDataTable
            adapterAgendas.Connection.ConnectionString = ConnectionStringTaller
            adapterAgendas.Fill(_dstCitas.SCGTA_TB_Agenda)
            agendasTabla = _dstCitas.SCGTA_TB_Agenda
            If agendasTabla.Rows.Count <> 0 Then
                For Each row As Citas.SCGTA_TB_AgendaRow In agendasTabla.Rows
                    cb.ValidValues.Add(row.ID.ToString(), row.Agenda)
                Next
                '                cb.Select(0, BoSearchKey.psk_Index)
            End If

        End Sub

        Private Sub AgregaCFLVehiculos()
            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
            Dim oCons As SAPbouiCOM.Conditions
            Dim oCon As SAPbouiCOM.Condition

            oCFLs = _sboForm.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = DirectCast(_sboApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams), ChooseFromListCreationParams)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = NombreTablaVehiculos
            oCFLCreationParams.UniqueID = CFLVEhiculos
            oCFL = oCFLs.Add(oCFLCreationParams)

            oCons = oCFL.GetConditions()

            oCon = oCons.Add()
            oCon.Alias = "DocEntry"
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL

            'oCon = oCons.Add()
            'oCon.Alias = "DocEntry"
            'oCon.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL

            oCFL.SetConditions(oCons)
        End Sub

        <CLSCompliant(False)> _
        Public Sub ManejadorEventoLoad(ByVal FormUID As String, _
                                ByRef pVal As SAPbouiCOM.ItemEvent, _
                                ByRef BubbleEvent As Boolean)
            If DMS_Connector.Configuracion.ParamGenAddon.U_OT_SAP.Trim().Equals("Y") Then
                Exit Sub
            End If

            If FormType = pVal.FormTypeEx.ToString() Then
                If pVal.ActionSuccess Then
                    _sboForm = _sboApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                    AgregaCFLVehiculos()
                    CargaTabVehiculos()
                    _sboForm.PaneLevel = 1
                Else
                    _sboForm = Nothing
                End If
            End If
        End Sub

        <System.CLSCompliant(False)> _
        Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                                ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                ByRef BubbleEvent As Boolean)
            Dim sboItem As Item = _sboApplication.Forms.Item(FormUID).Items.Item(pVal.ItemUID)

            If DMS_Connector.Configuracion.ParamGenAddon.U_OT_SAP.Trim().Equals("Y") Then
                Exit Sub
            End If

            If sboItem.Enabled = False Then Return

            _sboForm = _sboApplication.Forms.ActiveForm

            If pVal.FormTypeEx.ToString() = FormType Then
                If pVal.ActionSuccess Then
                    Select Case pVal.ItemUID
                        Case ItemPaneVehiculos
                            _sboForm.PaneLevel = PaneVehiculos
                        Case BtnAgenda
                            Dim sboCombo As ComboBox
                            'Dim datosF As DatosFormularioPadre = New DatosFormularioPadre()

                            sboCombo = DirectCast(_sboForm.Items.Item(CbAgenda).Specific, ComboBox)
                            'CatchingEvents.m_oAgendas.CodigoAgenda = CInt(sboCombo.Selected.Value)

                            'datosF.SboFormID = pVal.FormUID
                            'datosF.Actualiza = True

                            'CatchingEvents.m_oAgendas.FormularioPadre = datosF
                            'FormularioAgendaSBO.Modal = True
                            'CatchingEvents.m_oAgendas.CargarFormulario()

                            Dim fecha As Date
                            fecha = DateTime.Now
                            Utilitarios.RetornaFechaFormatoRegional(fecha.ToString("yyyy-MM-dd"))

                            Dim descripcionAgenda As String = sboCombo.Selected.Description

                            Dim ptr As IntPtr = GetForegroundWindow()
                            Dim wrapper As New WindowWrapper(ptr)
                            _frmAgendaDotNet = New frmCalendarioAgenda(True, Date.Parse(fecha), descripcionAgenda, True)
                            _frmAgendaDotNet.ShowInTaskbar = False
                            _frmAgendaDotNet.ShowDialog(wrapper)

                        Case LinkVehiculo
                            Dim sboEdit As EditText
                            sboEdit = DirectCast(_sboForm.Items.Item(TxtIdVehiculo).Specific, EditText)
                            Dim idVeh As String = sboEdit.Value

                            sboEdit = DirectCast(_sboForm.Items.Item(TxtCardCode).Specific, EditText)
                            Dim cardCode As String = sboEdit.Value

                            Dim tipoVehiculo As String  =  String.Empty
                            Dim tipoCountVeh As Integer

                            If Not String.IsNullOrEmpty(idVeh) Then
                                If Not ValidarSiFormularioAbierto("DET", False) Then
                                    Dim m_oVehiculo As VehiculosCls = New VehiculosCls(_sboCompany, _sboApplication)
                                    m_oVehiculo.DibujarFormularioDetalleInformacionVehiculo(cardCode, _
                                                                                                 idVeh, _
                                                                                                 True, _
                                                                                                 tipoVehiculo, _
                                                                                                 tipoCountVeh, False, False, VehiculosCls.ModoFormulario.scgTaller)
                                End If
                            End If


                    End Select
                Else 'Before

                    If pVal.ItemUID = BtnAgenda Then
                        Dim sboCombo As ComboBox
                        
                        sboCombo = DirectCast(_sboForm.Items.Item(CbAgenda).Specific, ComboBox)

                        If String.IsNullOrEmpty(sboCombo.Value.ToString().Trim()) Then
                            BubbleEvent = False
                            _sboApplication.StatusBar.SetText(My.Resources.Resource.FaltaAgenda, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)

                        End If
                    End If

                    If pVal.ItemUID = "9" AndAlso (_sboForm.Mode = BoFormMode.fm_ADD_MODE Or _sboForm.Mode = BoFormMode.fm_UPDATE_MODE) Then
                        CreaCita(FormUID, pVal, BubbleEvent)

                    End If

                End If
            End If
        End Sub

        Private Function ValidarSiFormularioAbierto(ByVal strFormUID As String, _
                                          ByVal blnselectIfOpen As Boolean) As Boolean

            Dim intI As Integer = 0
            Dim blnFound As Boolean = False
            Dim frmForma As SAPbouiCOM.Form

            Dim a As Integer = _sboApplication.Forms.Count

            While (Not blnFound AndAlso intI < _sboApplication.Forms.Count)

                frmForma = _sboApplication.Forms.Item(intI)

                If frmForma.UniqueID = strFormUID Then
                    blnFound = True
                    If (blnselectIfOpen) Then
                        If Not (frmForma.Selected) Then
                            _sboApplication.Forms.Item(strFormUID).Select()
                        End If
                    End If
                Else

                    intI += 1
                End If

            End While

            If (blnFound) Then
                Return True
            Else
                Return False
            End If

        End Function

        <CLSCompliant(False)> _
        Public Sub ManejadorEventoChooseFromList(ByVal FormUID As String, _
                                          ByRef pVal As SAPbouiCOM.ItemEvent, _
                                          ByRef BubbleEvent As Boolean)
            _sboForm = _sboApplication.Forms.Item(pVal.FormUID)

            If DMS_Connector.Configuracion.ParamGenAddon.U_OT_SAP.Trim().Equals("Y") Then
                Exit Sub
            End If

            If pVal.FormTypeEx.ToString() = FormType Then
                If pVal.ActionSuccess = True Then
                    Dim sboEdit As EditText
                    Dim sboDataTable As SAPbouiCOM.DataTable
                    Dim sboCFLEvento As SAPbouiCOM.IChooseFromListEvent
                    sboCFLEvento = DirectCast(pVal, IChooseFromListEvent)
                    sboDataTable = sboCFLEvento.SelectedObjects()

                    If Not sboDataTable Is Nothing Then
                        If pVal.ItemUID = TxtPlaca Then

                            dtVehiculo.IdVehiculo = sboDataTable.GetValue("Code", 0).ToString()
                            dtVehiculo.CodMarca = sboDataTable.GetValue("U_Cod_Marc", 0).ToString()
                            dtVehiculo.DescMarca = sboDataTable.GetValue("U_Des_Marc", 0).ToString()
                            dtVehiculo.CodModelo = sboDataTable.GetValue("U_Cod_Mode", 0).ToString()
                            dtVehiculo.DescModelo = sboDataTable.GetValue("U_Des_Mode", 0).ToString()
                            dtVehiculo.Vin = sboDataTable.GetValue("U_Num_VIN", 0).ToString()
                            dtVehiculo.CodEstilo = sboDataTable.GetValue("U_Cod_Esti", 0).ToString()
                            dtVehiculo.DescEstilo = sboDataTable.GetValue("U_Des_Esti", 0).ToString()
                            dtVehiculo.NoUnidad = sboDataTable.GetValue("U_Cod_Unid", 0).ToString()
                            dtVehiculo.Placa = sboDataTable.GetValue("U_Num_Plac", 0).ToString()


                            sboEdit = DirectCast(_sboForm.Items.Item(TxtIdVehiculo).Specific, EditText)
                            sboEdit.String = dtVehiculo.IdVehiculo.ToString()

                            sboEdit = DirectCast(_sboForm.Items.Item(TxtCodMarca).Specific, EditText)
                            sboEdit.String = dtVehiculo.CodMarca

                            sboEdit = DirectCast(_sboForm.Items.Item(TxtMarca).Specific, EditText)
                            sboEdit.String = dtVehiculo.DescMarca

                            sboEdit = DirectCast(_sboForm.Items.Item(TxtCodModelo).Specific, EditText)
                            sboEdit.String = dtVehiculo.CodModelo

                            sboEdit = DirectCast(_sboForm.Items.Item(TxtModelo).Specific, EditText)
                            sboEdit.String = dtVehiculo.DescModelo

                            sboEdit = DirectCast(_sboForm.Items.Item(TxtVin).Specific, EditText)
                            sboEdit.String = dtVehiculo.Vin

                            sboEdit = DirectCast(_sboForm.Items.Item(TxtCodEstilo).Specific, EditText)
                            sboEdit.String = dtVehiculo.CodEstilo

                            sboEdit = DirectCast(_sboForm.Items.Item(TxtEstilo).Specific, EditText)
                            sboEdit.String = dtVehiculo.DescEstilo

                            sboEdit = DirectCast(_sboForm.Items.Item(TxtNoUnidad).Specific, EditText)
                            sboEdit.String = dtVehiculo.NoUnidad

                            '                            sboItem = _sboForm.Items.Item(TxtCardCode)
                            '                            sboEdit = DirectCast(sboItem.Specific, EditText)
                            '                            Dim valor As String = sboEdit.Value.ToString().Trim

                            Try
                                '                                If String.IsNullOrEmpty(valor) Then
                                '
                                '                                    sboItem = _sboForm.Items.Item(TxtCardCode)
                                '                                    sboEdit = DirectCast(sboItem.Specific, EditText)
                                '                                    sboEdit.Value = sboDataTable.GetValue("U_CardCode", 0).ToString()
                                '                                    sboItem = _sboForm.Items.Item(TxtCardName)
                                '                                    sboEdit = DirectCast(sboItem.Specific, EditText)
                                '                                    sboEdit.Value = sboDataTable.GetValue("U_CardName", 0).ToString()
                                '                                End If

                                sboEdit = DirectCast(_sboForm.Items.Item(TxtPlaca).Specific, EditText)
                                sboEdit.String = dtVehiculo.Placa


                            Catch ex As System.Runtime.InteropServices.COMException
                                If ex.Message <> "Item - Can't set value on item because the item can't get focus.  [66000-153]" Then Throw
                            End Try
                        End If
                    End If
                Else 'Before Action
                    If pVal.ItemUID = TxtPlaca Then
                        Dim sboItem As Item
                        Dim sboEditText As EditText
                        Dim valor As String = String.Empty

                        Dim oCons As SAPbouiCOM.Conditions
                        Dim oCon As SAPbouiCOM.Condition
                        Dim oCFL As SAPbouiCOM.ChooseFromList

                        oCFL = _sboForm.ChooseFromLists.Item(CFLVEhiculos)
                        oCons = oCFL.GetConditions()

                        sboItem = _sboForm.Items.Item(TxtCardCode)
                        sboEditText = DirectCast(sboItem.Specific, EditText)
                        valor = sboEditText.Value.ToString().Trim

                        oCon = oCons.Item(0)
                        If Not String.IsNullOrEmpty(valor) Then
                            oCon.Alias = "U_CardCode"
                            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCon.CondVal = valor
                        Else
                            oCon.Alias = "DocEntry"
                            oCon.Operation = BoConditionOperation.co_NOT_NULL
                        End If

                        oCFL.SetConditions(oCons)
                    End If
                End If
            End If
        End Sub

        Private Function ObtieneValorEditText(ByVal itemUID As String, ByVal sboForm As Form) As String
            Dim sboItem As Item
            Dim sboEditText As EditText

            sboItem = sboForm.Items.Item(itemUID)
            sboEditText = DirectCast(sboItem.Specific, EditText)
            Return sboEditText.Value
        End Function

        Public Function ObtieneValorDataSource(ByVal udf As String, ByVal sboForm As Form) As String
            Return sboForm.DataSources.DBDataSources.Item(NombreTabla).GetValue(udf, 0)
        End Function

        Private Function ObtieneValorCombo(ByVal itemUID As String, ByVal sboForm As Form) As String
            Dim sboItem As Item
            Dim sboCombo As ComboBox

            sboItem = sboForm.Items.Item(itemUID)
            sboCombo = DirectCast(sboItem.Specific, ComboBox)
            Return sboCombo.Selected.Value
        End Function

        Private creoCita As Boolean = False
        Private numLlamada As Integer
        Private transaccion As SqlTransaction = Nothing
        Private rowCita As Citas.SCGTA_TB_CitaRow
        Dim adapterCita As SCGTA_TB_CitaTableAdapter

        <CLSCompliant(False)> _
        Public Sub CreaCotizacion(ByVal BusinessObjectInfo As BusinessObjectInfo)

            Dim sboCotizacion As Documents

            If Not creoCita Then Return
            Dim dstConf As New ConfiguracionDataSet
            Dim adapterConfiguracion As ConfiguracionDataAdapter
            Dim serieCotizacion As String = String.Empty
            Dim articuloCita As String = String.Empty
            Dim razon As String = String.Empty
            Dim sboCombo As ComboBox

            Dim sboLlamadaServicio As ServiceCalls = DirectCast(_sboCompany.GetBusinessObject(BoObjectTypes.oServiceCalls), ServiceCalls)
            _sboForm = _sboApplication.Forms.ActiveForm

            sboCotizacion = DirectCast(_sboCompany.GetBusinessObject(BoObjectTypes.oQuotations), Documents)
            sboLlamadaServicio.Browser.GetByKeys(BusinessObjectInfo.ObjectKey)
            Try
                adapterConfiguracion = New ConfiguracionDataAdapter(ConnectionStringTaller)
                adapterConfiguracion.Fill(dstConf.SCGTA_TB_Configuracion)

                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, "IDSerieDocumentosCotizaciones", serieCotizacion)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, "ArticuloCita", articuloCita)

                sboCotizacion.CardCode = sboLlamadaServicio.CustomerCode
                Dim STRC As String = sboCotizacion.CardCode
                sboCotizacion.DocDate = Date.Now
                sboCotizacion.DocDueDate = Date.Now
                If Not String.IsNullOrEmpty(serieCotizacion) Then sboCotizacion.Series = CInt(serieCotizacion)

                'copiar datos vehiculo
                sboCotizacion.UserFields.Fields.Item("U_SCGD_LlSv").Value = sboLlamadaServicio.ServiceCallID
                sboCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = sboLlamadaServicio.UserFields.Fields.Item("U_SCGD_NoUn").Value
                sboCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = sboLlamadaServicio.UserFields.Fields.Item("U_SCGD_Placa").Value
                sboCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value = sboLlamadaServicio.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value
                sboCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value = sboLlamadaServicio.UserFields.Fields.Item("U_SCGD_D_Marca").Value
                sboCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value = sboLlamadaServicio.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value
                sboCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value = sboLlamadaServicio.UserFields.Fields.Item("U_SCGD_D_Est").Value
                sboCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value = sboLlamadaServicio.UserFields.Fields.Item("U_SCGD_Vin").Value
                sboCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value = sboLlamadaServicio.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value
                sboCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value = sboLlamadaServicio.UserFields.Fields.Item("U_SCGD_D_Mod").Value
                sboCotizacion.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value = sboLlamadaServicio.UserFields.Fields.Item("U_SCGD_IdVeh").Value

                sboCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = "No Iniciada"
                sboCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value = rowCita.NoConsecutivo
                sboCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value = rowCita.NoSerie

                'Informacion referente a la agenda
                Dim adpAgenda As SCGTA_TB_AgendaTableAdapter = New SCGTA_TB_AgendaTableAdapter()
                adpAgenda.Connection.ConnectionString = ConnectionStringTaller
                Dim agendaDataTable As Citas.SCGTA_TB_AgendaDataTable = adpAgenda.GetDataBy(rowCita.IDAgenda)
                If agendaDataTable.Rows.Count > 0 Then
                    Dim age As Citas.SCGTA_TB_AgendaRow = DirectCast(agendaDataTable.Rows(0), Citas.SCGTA_TB_AgendaRow)
                    If Not age.IsCodAsesorNull AndAlso age.CodAsesor <> 0 Then
                        sboCotizacion.DocumentsOwner = age.CodAsesor
                    End If
                    If Not age.IsArticuloCitaNull Then
                        articuloCita = age.ArticuloCita
                    End If
                End If
                '                If Not rowCita.IsempIdNull() AndAlso rowCita.empId <> -1 Then
                '                    sboCotizacion.DocumentsOwner = rowCita.empId
                '                End If

                sboCombo = DirectCast(_sboForm.Items.Item(CbRazonCita).Specific, ComboBox)
                razon = sboCombo.Selected.Description

                If Not (rowCita.Observaciones Is DBNull.Value) Then
                    If Not String.IsNullOrEmpty(rowCita.Observaciones) Then
                        sboCotizacion.Comments = My.Resources.Resource.CitaPorLLamada + vbNewLine + " (" + rowCita.Observaciones + ")" + vbNewLine + My.Resources.Resource.UsuarioLLamada + _sboCompany.UserName
                    Else
                        sboCotizacion.Comments = My.Resources.Resource.CitaPorLLamada + vbNewLine + My.Resources.Resource.UsuarioLLamada + _sboCompany.UserName
                    End If
                End If
                'agregar linea
                sboCotizacion.Lines.ItemCode = articuloCita
                sboCotizacion.Lines.Add()

                Dim codigoError As Integer

                _sboCompany.StartTransaction()

                codigoError = sboCotizacion.Add()

                Dim stre As String = _sboCompany.GetLastErrorDescription().ToString()

                If codigoError <> 0 Then

                    If codigoError = -5002 Then

                        _sboApplication.StatusBar.SetText(My.Resources.Resource.ErrorCode & codigoError & ": " & _sboCompany.GetLastErrorDescription() & "for the Quotation", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)

                    Else
                        _sboApplication.StatusBar.SetText(My.Resources.Resource.ErrorCode & codigoError & ": " & _sboCompany.GetLastErrorDescription(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)

                    End If

                    _sboCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
                    transaccion.Rollback()
                    If adapterCita.Connection.State <> ConnectionState.Closed Then adapterCita.Connection.Close()
                    Return
                End If

                Dim key As String = String.Empty
                _sboCompany.GetNewObjectCode(key)
                sboCotizacion.GetByKey(CInt(key))

                rowCita.NoCotizacion = sboCotizacion.DocEntry
                adapterCita.Update(_dstCitas.SCGTA_TB_Cita)

                'llenar udf llamda srvicio, quitat lo de abajo
                sboLlamadaServicio.UserFields.Fields.Item("U_SCGD_NoCita").Value = rowCita.NoCita
                sboLlamadaServicio.UserFields.Fields.Item("U_SCGD_Cita").Value = rowCita.NoConsecutivo
                sboLlamadaServicio.UserFields.Fields.Item("U_SCGD_GenCita").Value = 0
                sboLlamadaServicio.UserFields.Fields.Item("U_SCGD_Cotiz").Value = key

                codigoError = sboLlamadaServicio.Update()
                If codigoError <> 0 Then
                    _sboApplication.StatusBar.SetText(My.Resources.Resource.ErrorCode & codigoError & ": " &_sboCompany.GetLastErrorDescription(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    _sboCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
                    transaccion.Rollback()
                    If adapterCita.Connection.State <> ConnectionState.Closed Then adapterCita.Connection.Close()
                    Return
                End If


                _sboCompany.EndTransaction(BoWfTransOpt.wf_Commit)
                transaccion.Commit()
                If adapterCita.Connection.State <> ConnectionState.Closed Then adapterCita.Connection.Close()


                DeshabilitaFechaCita()
                rowCita = Nothing
                creoCita = False

            Catch ex As Exception
                If _sboCompany.InTransaction Then _sboCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
                transaccion.Rollback()
                If adapterCita.Connection.State <> ConnectionState.Closed Then adapterCita.Connection.Close()
                HuboError = True

                Call Utilitarios.ManejadorErrores(ex, _sboApplication)
                '_sboApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            End Try
        End Sub

        <CLSCompliant(False)> _
        Public Sub CreaCita(ByVal FormUID As String, _
                                                ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                ByRef BubbleEvent As Boolean)
            If Integer.Parse(_sboForm.DataSources.DBDataSources.Item(NombreTabla).GetValue("U_SCGD_GenCita", 0)) = 0 Then Return

            HuboError = False

            Dim err As String = String.Empty
            If ValidarDatosCita(err) = False Then
                BubbleEvent = False
                _sboApplication.StatusBar.SetText(err, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                creoCita = False

                Return

            End If

            adapterCita = New SCGTA_TB_CitaTableAdapter()
            transaccion = Nothing
            adapterCita.Connection.ConnectionString = ConnectionStringTaller
            _sboForm = _sboApplication.Forms.ActiveForm

            adapterCita.Fill(_dstCitas.SCGTA_TB_Cita)

            Dim idAgenda As Integer
            Dim idRazon As Integer
            Dim observaciones As String = String.Empty
            Dim fecha As String
            Dim hora As String
            Dim fechaHora As Date
            Dim cliente As String
            Dim idVehiculo As Integer
            Dim noUnidad As String
            Dim codUsuario As Integer
            Dim nombreUsuario As String
            'No se para qué es esto
            Dim fechaHoraEnHorario As Date


            cliente = ObtieneValorEditText(TxtCardCode, _sboForm)
            fecha = ObtieneValorEditText(TxtFechaCita, _sboForm)
            hora = ObtieneValorEditText(TxtHoraCita, _sboForm)
            fechaHora = Date.ParseExact(fecha & " " & hora, "yyyyMMdd HHmm", Nothing)
            idRazon = Integer.Parse(ObtieneValorCombo(CbRazonCita, _sboForm))
            observaciones = ObtieneValorEditText(TxtObervaciones, _sboForm)
            idVehiculo = Integer.Parse(ObtieneValorEditText(TxtIdVehiculo, _sboForm))
            noUnidad = ObtieneValorEditText(TxtNoUnidad, _sboForm)
            codUsuario = Integer.Parse(Utilitarios.EjecutarConsulta(String.Format("select userid from OUSR where USER_CODE = '{0}'", _sboCompany.UserName), _sboCompany.CompanyDB, _sboCompany.Server))
            nombreUsuario = _sboCompany.UserName
            idAgenda = Integer.Parse(ObtieneValorCombo(CbAgenda, _sboForm))
            '
            fechaHoraEnHorario = New Date(1900, 1, 1, fechaHora.Hour, fechaHora.Minute, 0)

            Dim _
                row As Citas.SCGTA_TB_CitaRow = _
                    _dstCitas.SCGTA_TB_Cita.AddSCGTA_TB_CitaRow(String.Empty, -999, fechaHora, idAgenda, _
                                                                 idRazon, _
                                                                 observaciones, True, cliente, _
                                                                 idVehiculo.ToString(), noUnidad, codUsuario, _
                                                                 nombreUsuario, _
                                                                 Nothing, fechaHoraEnHorario, String.Empty, String.Empty)
            'transaccion
            adapterCita.Connection.Open()
            transaccion = adapterCita.Connection.BeginTransaction()
            adapterCita.Transaccion = transaccion

            Try
                adapterCita.Update(_dstCitas.SCGTA_TB_Cita)
                
            Catch ex As SqlException
                _sboApplication.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                creoCita = False
                HuboError = True
                BubbleEvent = False
                transaccion.Rollback()
                adapterCita.Connection.Close()
                Return
            End Try

            If row.IDCita = -1 Then
                _sboApplication.StatusBar.SetText(My.Resources.Resource.ErrorCreandoCita, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                creoCita = False
                BubbleEvent = False
                transaccion.Rollback()
                adapterCita.Connection.Close()
                Return
            End If

            rowCita = row
            creoCita = True

        End Sub

        Public Function ValidarDatosCita(ByRef errorD As String) As Boolean
            _sboForm = _sboApplication.Forms.ActiveForm
            Dim sboItem As Item
            Dim sboCombo As ComboBox

            sboItem = _sboForm.Items.Item(CbAgenda)
            sboCombo = DirectCast(sboItem.Specific, ComboBox)
            If sboCombo.Selected Is Nothing Then
                errorD = My.Resources.Resource.FaltaAgenda
                Return False
            End If

            sboItem = _sboForm.Items.Item(CbRazonCita)
            sboCombo = DirectCast(sboItem.Specific, ComboBox)
            If sboCombo.Selected Is Nothing Then
                errorD = My.Resources.Resource.FaltaRazonCita
                Return False
            End If

            If String.IsNullOrEmpty(_sboForm.DataSources.DBDataSources.Item(NombreTabla).GetValue("U_SCGD_FcCita", 0)) Then
                errorD = My.Resources.Resource.ErrorFechaCita
                Return False
            End If
            If String.IsNullOrEmpty(_sboForm.DataSources.DBDataSources.Item(NombreTabla).GetValue("U_SCGD_HrCita", 0)) Then
                errorD = My.Resources.Resource.ErrorFechaCita
                Return False
            End If
            If String.IsNullOrEmpty(_sboForm.DataSources.DBDataSources.Item(NombreTabla).GetValue("U_SCGD_IdVeh", 0)) Then
                errorD = My.Resources.Resource.ErrorVehiculoCita
                Return False
            End If
            If Not String.IsNullOrEmpty(_sboForm.DataSources.DBDataSources.Item(NombreTabla).GetValue("U_SCGD_Cita", 0)) Then
                errorD = My.Resources.Resource.YaTieneCita
                Return False
            End If
            If String.IsNullOrEmpty(_sboForm.DataSources.DBDataSources.Item(NombreTabla).GetValue("subject", 0)) Then
                errorD = My.Resources.Resource.AsuntoCita
                Return False
            End If
            Dim cardCode As String = _sboForm.DataSources.DBDataSources.Item(NombreTabla).GetValue("customer", 0)
            If Not ValidarTipoCambioMonedaSistema() Then
                errorD = My.Resources.Resource.TipoCambioNoActualizado
                Return False
            End If
            If Not ValidarTipoCambioCliente(cardCode) Then
                errorD = My.Resources.Resource.TipoCambioNoActualizado
                Return False
            End If

            Return True
        End Function

        Private Function ValidarTipoCambioMonedaSistema() As Boolean

            Dim monedaSistema As String = String.Empty
            Dim monedaLocal As String = String.Empty
            Dim blsbo As New BLSBO.GlobalFunctionsSBO
            Dim utilitarioas As SCGDataAccess.Utilitarios = New SCGDataAccess.Utilitarios(ConnectionStringSBO)
            Dim tipoCambio As Double = 0

            blsbo.Set_Compania(_sboCompany)
            blsbo.MonedasSistema(monedaLocal, monedaSistema)
            If Trim(monedaLocal) <> Trim(monedaSistema) Then
                tipoCambio = blsbo.RetornarTipoCambioMoneda(monedaSistema, utilitarioas.CargarFechaHoraServidor(), ConnectionStringSBO, False)
                If tipoCambio = -1 Then Return False
            End If
            Return True
        End Function

        Private Function ValidarTipoCambioCliente(ByVal cardCode As String) As Boolean
            Dim monedaSistema As String = String.Empty
            Dim monedaLocal As String = String.Empty
            Dim blsbo As New BLSBO.GlobalFunctionsSBO
            Dim utilitarioas As SCGDataAccess.Utilitarios = New SCGDataAccess.Utilitarios(ConnectionStringSBO)
            Dim tipoCambio As Double = 0
            Dim bp As BusinessPartners

            bp = DirectCast(_sboCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners), BusinessPartners)
            bp.GetByKey(cardCode)

            blsbo.Set_Compania(_sboCompany)
            blsbo.MonedasSistema(monedaLocal, monedaSistema)
            monedaLocal = monedaLocal.Trim()
            monedaSistema = monedaSistema.Trim()

            If (bp.Currency <> "##" AndAlso bp.Currency <> monedaLocal) Then
                tipoCambio = blsbo.RetornarTipoCambioMoneda(bp.Currency, utilitarioas.CargarFechaHoraServidor(), ConnectionStringSBO, false)
                If tipoCambio = -1 Then Return False
            End If

            Return True

        End Function

        <DllImport("user32.dll", SetLastError:=True)> _
        Private Shared Function GetForegroundWindow() As IntPtr
        End Function

        Private Sub _frmAgendaDotNet_eFechaYHoraSeleccionada(ByVal p_dtFechaYHora As Date, ByVal p_strNombreAgenda As String, ByVal p_intCodigoAgenda As Integer) Handles _frmAgendaDotNet.eFechaYHoraSeleccionada

            Dim sboItem As Item
            Dim sboEdit As EditText
            Dim fechaCita As String
            Dim horaCita As String
            Dim minutosCita As String


            fechaCita = p_dtFechaYHora.ToString("yyyyMMdd")
            sboItem = _sboForm.Items.Item(TxtFechaCita)
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.Value = fechaCita

            sboItem = _sboForm.Items.Item(TxtHoraCita)
            sboEdit = DirectCast(sboItem.Specific, EditText)
            horaCita = p_dtFechaYHora.ToString("HH")
            minutosCita = p_dtFechaYHora.ToString("mm")
            horaCita = horaCita + minutosCita
            sboEdit.Value = horaCita


            _frmAgendaDotNet.Close()
            _frmAgendaDotNet = Nothing

        End Sub
    End Class


End Namespace
