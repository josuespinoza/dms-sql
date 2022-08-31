Option Strict On
Option Explicit On

Imports DMS_Addon.ControlesSBO
Imports System.Collections.Generic
Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Namespace Ventas
    <CLSCompliant(False)> _
    Public Class FormularioPresupuestos
        Implements SCG.SBOFramework.UI.IFormularioSBO, IUsaPermisos

#Region "Properties"
        Private _formType As String = "SCGD_PRESUP"

        Private _nombreXml As String

        Private _sboCompany As SAPbobsCOM.Company
        Private _sboForm As IForm
        Private WithEvents _sboApplication As SAPbouiCOM.Application

        Private _idMenu As String = "SCGD_MPS"

        Private _menuPadre As String = "2048"

        Private _posicion As Integer = 0

        Private _nombreMenu As String

        Protected Shared ConnectionStringSBO As String = String.Empty

        Dim _configuracionesClasificacion1 As Dictionary(Of String, ConfiguracionPresupuesto)
        Dim _marcasPresupuestos As Dictionary(Of String, MarcasPresupuestos)

        Private _inicializado As Boolean

        Private _titulo As String

        Public ReadOnly Property ApplicationSBO() As IApplication Implements IFormularioSBO.ApplicationSBO
            Get
                Return _sboApplication
            End Get
        End Property

        Public ReadOnly Property CompanySBO() As SAPbobsCOM.ICompany Implements IFormularioSBO.CompanySBO
            Get
                Return _sboCompany
            End Get
        End Property

        Public Property FormType() As String Implements IFormularioSBO.FormType
            Get
                Return _formType
            End Get
            Set(ByVal value As String)
                _formType = value
            End Set
        End Property

        Public Property NombreXML() As String Implements IFormularioSBO.NombreXml
            Get
                Return _nombreXml
            End Get
            Set(ByVal value As String)
                _nombreXml = value
            End Set
        End Property

        Public Property Titulo() As String Implements IFormularioSBO.Titulo
            Get
                Return _titulo
            End Get
            Set(ByVal value As String)
                _titulo = value
            End Set
        End Property

        Public Property FormularioSBO() As IForm Implements IFormularioSBO.FormularioSBO
            Get
                Return _sboForm
            End Get
            Set(ByVal value As IForm)
                _sboForm = value
            End Set
        End Property


        Private _updated As Boolean
        Public Property Updated() As Boolean
            Get
                Return _updated
            End Get
            Set(ByVal value As Boolean)
                _updated = value
            End Set
        End Property

#End Region

        Public Sub New(ByVal sboApplication As Application, ByVal sboCompany As SAPbobsCOM.Company)
            _sboCompany = sboCompany
            _sboApplication = sboApplication
            MenuPadre = "SCGD_CTT"
            If String.IsNullOrEmpty(ConnectionStringSBO) Then
                Configuracion.CrearCadenaDeconexion(_sboCompany.Server, _sboCompany.CompanyDB, ConnectionStringSBO)
            End If

            _nombreMenu = My.Resources.Resource.TituloMenuPresupuestos
            _nombreXml = My.Resources.Resource.XMLFormularioPresupuestos

        End Sub

        Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario
            If _sboForm IsNot Nothing Then
                CargaComboClasificacion1(True)
                CargaComboMarcasPresupuesto(True)
                _cbMesIni.ComboBox.Select(0, BoSearchKey.psk_Index)
            End If
        End Sub

        Public Property Inicializado() As Boolean Implements IFormularioSBO.Inicializado
            Get
                Return _inicializado
            End Get
            Set(ByVal value As Boolean)
                _inicializado = value
            End Set
        End Property

        Public Property IdMenu() As String Implements IUsaMenu.IdMenu
            Get
                Return _idMenu
            End Get
            Set(ByVal value As String)
                _idMenu = value
            End Set
        End Property

        Public Property MenuPadre() As String Implements IUsaMenu.MenuPadre
            Get
                Return _menuPadre
            End Get
            Set(ByVal value As String)
                _menuPadre = value
            End Set
        End Property

        Public Property Posicion() As Integer Implements IUsaMenu.Posicion
            Get
                Return _posicion
            End Get
            Set(ByVal value As Integer)
                _posicion = value
            End Set
        End Property

        Public Property Nombre() As String Implements IUsaMenu.Nombre
            Get
                Return _nombreMenu
            End Get
            Set(ByVal value As String)
                _nombreMenu = value
            End Set
        End Property

        Protected Overridable Sub CargaComboClasificacion1(ByVal seleccionarPrimero As Boolean)
            Dim valoresValidos As List(Of ControlesSBO.ValorValidoSBO)
            Dim mn As ManejadorClasificacionPresupuesto = New ManejadorClasificacionPresupuesto(ConnectionStringSBO)
            _configuracionesClasificacion1 = mn.CargaConfiguracionesClasificacion1()

            valoresValidos = New List(Of ControlesSBO.ValorValidoSBO)(_configuracionesClasificacion1.Count)
            For Each keyValuePair As KeyValuePair(Of String, ConfiguracionPresupuesto) In _configuracionesClasificacion1
                valoresValidos.Add( _
                                    New ControlesSBO.ValorValidoSBO _
                                       With {.Value = keyValuePair.Value.Code, .Description = keyValuePair.Value.Name})
 _
            Next
            _cbClsf1.CargaValoresValidos(valoresValidos, seleccionarPrimero)
        End Sub

        Protected Overridable Sub CargaComboMarcasPresupuesto(ByVal seleccionarPrimero As Boolean)
            Dim valoresValidos As List(Of ControlesSBO.ValorValidoSBO)
            Dim mn As ManejadorMarcasPresupuestos = New ManejadorMarcasPresupuestos(ConnectionStringSBO)
            _marcasPresupuestos = mn.CargaMarcasPresupuestos()

            valoresValidos = New List(Of ControlesSBO.ValorValidoSBO)(_marcasPresupuestos.Count)
            For Each keyValuePair As KeyValuePair(Of String, MarcasPresupuestos) In _marcasPresupuestos
                valoresValidos.Add( _
                                    New ControlesSBO.ValorValidoSBO _
                                       With {.Value = keyValuePair.Value.Code, .Description = keyValuePair.Value.Name})
 _
            Next
            _cbMarca.CargaValoresValidos(valoresValidos, seleccionarPrimero)
        End Sub

        'Private Sub _sboApplication_FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean) Handles _sboApplication.FormDataEvent
        '    If _sboForm Is Nothing OrElse Not Inicializado Then Return
        '    If BusinessObjectInfo.FormTypeEx = FormType Then

        '        If BusinessObjectInfo.EventType = BoEventTypes.et_FORM_DATA_ADD AndAlso BusinessObjectInfo.BeforeAction Then

        '        End If
        '        If BusinessObjectInfo.EventType = BoEventTypes.et_FORM_DATA_UPDATE AndAlso BusinessObjectInfo.BeforeAction Then

        '        End If
        '        If BusinessObjectInfo.EventType = BoEventTypes.et_FORM_DATA_LOAD AndAlso BusinessObjectInfo.ActionSuccess Then

        '            'CalculaTotales()
        '        End If
        '    End If
        'End Sub

        Protected Overridable Sub AsignaMeses()
            If _sboForm IsNot Nothing Then
                _sboForm.Freeze(True)

                Dim c As CultureInfo = New CultureInfo(Utilitarios.CargarCulturaActual())
                Dim mes As Integer = CInt(_sboForm.DataSources.DBDataSources.Item(0).GetValue("U_MesIn", 0))

                _lblMes1.StaticText.Caption = StrConv(c.DateTimeFormat.GetMonthName(mes), VbStrConv.ProperCase)
                mes = mes + 1
                If mes = 13 Then mes = 1
                _lblMes2.StaticText.Caption = StrConv(c.DateTimeFormat.GetMonthName(mes), VbStrConv.ProperCase)
                mes = mes + 1
                If mes = 13 Then mes = 1
                _lblMes3.StaticText.Caption = StrConv(c.DateTimeFormat.GetMonthName(mes), VbStrConv.ProperCase)
                mes = mes + 1
                If mes = 13 Then mes = 1
                _lblMes4.StaticText.Caption = StrConv(c.DateTimeFormat.GetMonthName(mes), VbStrConv.ProperCase)
                mes = mes + 1
                If mes = 13 Then mes = 1
                _lblMes5.StaticText.Caption = StrConv(c.DateTimeFormat.GetMonthName(mes), VbStrConv.ProperCase)
                mes = mes + 1
                If mes = 13 Then mes = 1
                _lblMes6.StaticText.Caption = StrConv(c.DateTimeFormat.GetMonthName(mes), VbStrConv.ProperCase)
                mes = mes + 1
                If mes = 13 Then mes = 1
                _lblMes7.StaticText.Caption = StrConv(c.DateTimeFormat.GetMonthName(mes), VbStrConv.ProperCase)
                mes = mes + 1
                If mes = 13 Then mes = 1
                _lblMes8.StaticText.Caption = StrConv(c.DateTimeFormat.GetMonthName(mes), VbStrConv.ProperCase)
                mes = mes + 1
                If mes = 13 Then mes = 1
                _lblMes9.StaticText.Caption = StrConv(c.DateTimeFormat.GetMonthName(mes), VbStrConv.ProperCase)
                mes = mes + 1
                If mes = 13 Then mes = 1
                _lblMes10.StaticText.Caption = StrConv(c.DateTimeFormat.GetMonthName(mes), VbStrConv.ProperCase)
                mes = mes + 1
                If mes = 13 Then mes = 1
                _lblMes11.StaticText.Caption = StrConv(c.DateTimeFormat.GetMonthName(mes), VbStrConv.ProperCase)
                mes = mes + 1
                If mes = 13 Then mes = 1
                _lblMes12.StaticText.Caption = StrConv(c.DateTimeFormat.GetMonthName(mes), VbStrConv.ProperCase)
                mes = mes + 1

                _sboForm.Freeze(False)
            End If
        End Sub

        ''' <summary>
        ''' Pinta los totales y campos de texto de las matrices
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub CalculaTotales()
            Dim totalColumnasMontos(11) As Decimal
            Dim totalFilasMontos(_mtxPresupuestos.Matrix.RowCount - 1) As Decimal
            'Manejo de matriz de unidades 
            Dim totalColumnasUnidades(11) As Decimal
            Dim totalFilasUnidades(_mtxPresUnidades.Matrix.RowCount - 1) As Decimal

            Dim n As NumberFormatInfo = Utilitarios.GetNumberFomatInfo(_sboCompany)

            'carga totales por columnas y filas para las matrices
            For rows As Integer = 1 To _mtxPresupuestos.Matrix.RowCount
                For cols As Integer = 1 To 12
                    totalFilasMontos(rows - 1) = totalFilasMontos(rows - 1) + Decimal.Parse(_mtxPresupuestos.ObtieneValorColumnaEditText("colMes" & cols, rows), n)
                    totalColumnasMontos(cols - 1) = totalColumnasMontos(cols - 1) + Decimal.Parse(_mtxPresupuestos.ObtieneValorColumnaEditText("colMes" & cols, rows), n)
                    totalFilasUnidades(rows - 1) = totalFilasUnidades(rows - 1) + Decimal.Parse(_mtxPresUnidades.ObtieneValorColumnaEditText("Col_Mes" & cols, rows), n)
                    totalColumnasUnidades(cols - 1) = totalColumnasUnidades(cols - 1) + Decimal.Parse(_mtxPresUnidades.ObtieneValorColumnaEditText("Col_Mes" & cols, rows), n)
                Next
            Next

            'pinta los totales en las matrices
            For rows As Integer = 0 To totalFilasMontos.Length - 1
                _mtxPresupuestos.AsignaValorColumnaEditText(totalFilasMontos(rows).ToString(n), "colTotal", rows + 1)
                _mtxPresUnidades.AsignaValorColumnaEditText(totalFilasUnidades(rows).ToString(n), "Col_Total", rows + 1)
            Next

            Dim totalMontos As Decimal = 0
            Dim totalUnidades As Decimal = 0
            For columnas As Integer = 0 To totalColumnasMontos.Length - 1
                totalMontos = totalMontos + totalColumnasMontos(columnas)
                totalUnidades = totalUnidades + totalColumnasUnidades(columnas)

                DirectCast(_sboForm.Items.Item("txtMes" & (columnas + 1)).Specific, EditText).Value = totalColumnasMontos(columnas).ToString(n)
                DirectCast(_sboForm.Items.Item("txtMes2_" & (columnas + 1)).Specific, EditText).Value = totalColumnasUnidades(columnas).ToString(n)
            Next
            DirectCast(_sboForm.Items.Item("txtTotal").Specific, EditText).Value = totalMontos.ToString(n)
            DirectCast(_sboForm.Items.Item("txtTotal2").Specific, EditText).Value = totalUnidades.ToString(n)
            Updated = True

        End Sub

        Public Sub ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

            If Not Inicializado OrElse String.IsNullOrEmpty(pVal.ItemUID) Then Return

            If pVal.BeforeAction Then

                Select Case FormularioSBO.Mode
                    Case BoFormMode.fm_ADD_MODE

                        Select Case pVal.EventType

                            Case BoEventTypes.et_CLICK

                                Select Case pVal.ItemUID
                                    Case "1"
                                        If _cbClsf1.ComboBox.Selected Is Nothing Then
                                            _sboApplication.StatusBar.SetText(My.Resources.Resource.CamposRequeridos, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                                            BubbleEvent = False
                                        Else
                                            Dim configuracionPresupuesto As ConfiguracionPresupuesto = _configuracionesClasificacion1(_cbClsf1.ComboBox.Selected.Value)
                                            Dim mn As ManejadorClasificacionPresupuesto = New ManejadorClasificacionPresupuesto(ConnectionStringSBO)
                                            Dim clasificacionPresupuestos As List(Of ClasificacionPresupuesto)

                                            If _cbClsf1.ComboBox.Selected.Value = "1" Then
                                                Dim marcaPresupuesto As MarcasPresupuestos = _marcasPresupuestos(_cbMarca.ComboBox.Selected.Value)
                                                clasificacionPresupuestos = mn.CargaClasificacion(String.Format("SELECT TOP 1000 [Code] ,[Name] FROM [@SCGD_CONF_ART_VENTA] with (nolock) where code = '{0}' ORDER BY Name", marcaPresupuesto.Code))
                                            Else
                                                clasificacionPresupuestos = mn.CargaClasificacion("SELECT SlpCode AS Code, SlpName AS Name FROM OSLP")
                                            End If

                                            If Not clasificacionPresupuestos Is Nothing Then

                                                Dim manejadorCode As ManejadorCode = New ManejadorCode(ConnectionStringSBO)
                                                Dim count As Integer = 0

                                                If clasificacionPresupuestos.Count <> 0 Then _mtxPresupuestos.Matrix.AddRow(clasificacionPresupuestos.Count)
                                                If clasificacionPresupuestos.Count <> 0 Then _mtxPresUnidades.Matrix.AddRow(clasificacionPresupuestos.Count)

                                                For Each clasificacionPresupuesto As ClasificacionPresupuesto In clasificacionPresupuestos
                                                    count = count + 1
                                                    With _mtxPresupuestos.Matrix
                                                        _mtxPresupuestos.AsignaValorColumnaEditText(clasificacionPresupuesto.Code, "colIdClasf", count)
                                                        _mtxPresupuestos.AsignaValorColumnaEditText(clasificacionPresupuesto.Name, "colClasf", count)
                                                    End With

                                                    With _mtxPresUnidades.Matrix
                                                        _mtxPresUnidades.AsignaValorColumnaEditText(clasificacionPresupuesto.Code, "Col_IdClas", count)
                                                        _mtxPresUnidades.AsignaValorColumnaEditText(clasificacionPresupuesto.Name, "Col_Clasf", count)
                                                    End With
                                                Next
                                                Dim code As String = manejadorCode.ObtieneCode()
                                                _sboForm.DataSources.DBDataSources.Item("@SCGD_PRESUPUESTOS").SetValue("Code", 0, code)
                                                _mtxPresupuestos.Matrix.FlushToDataSource()
                                                _mtxPresUnidades.Matrix.FlushToDataSource()
                                            Else
                                                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorConfigPresupuestos, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                                BubbleEvent = False
                                            End If
                                        End If
                                End Select

                        End Select

                    Case BoFormMode.fm_UPDATE_MODE, BoFormMode.fm_EDIT_MODE, BoFormMode.fm_OK_MODE

                        If FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE Then
                            If Updated = False Then
                                CalculaTotales()
                                Updated = True
                            End If
                            _mtxPresupuestos.Matrix.FlushToDataSource()
                            _mtxPresUnidades.Matrix.FlushToDataSource()
                        End If

                        Select Case pVal.EventType

                            Case BoEventTypes.et_KEY_DOWN

                                Select Case pVal.ItemUID
                                    Case "mtxPresp", "mtxUnidad"
                                        Updated = False
                                End Select

                        End Select

                End Select

            ElseIf pVal.ActionSuccess Then

                Select Case pVal.EventType
                    Case BoEventTypes.et_COMBO_SELECT

                        Select Case pVal.ItemUID
                            Case _cbMesIni.ItemSBO.UniqueID
                                AsignaMeses()
                            Case _cbClsf1.ItemSBO.UniqueID
                                If _cbClsf1.ComboBox.Selected.Value = "1" Then
                                    _cbMarca.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                                Else
                                    _cbMarca.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                                End If
                        End Select

                    Case BoEventTypes.et_CLICK
                        Select Case pVal.ItemUID
                            Case _btCalc.ItemSBO.UniqueID
                                CalculaTotales()
                        End Select

                    Case BoEventTypes.et_FORM_DATA_LOAD
                        AsignaMeses()
                End Select

            End If

        End Sub

        Public Sub ManejadorEventoLoad(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
            Try
                If Not pVal.BeforeAction Then
                    Dim query As String = "Select MAX(Cast(ISNULL(Code, 0) as int)) +1 from [@SCGD_PRESUPUESTOS] with (nolock)"
                    Dim strNextCode As String = Utilitarios.EjecutarConsulta(query, _sboApplication.Company.DatabaseName, _sboApplication.Company.ServerName)
                    If Not String.IsNullOrEmpty(strNextCode) Then
                        _txtCode.EditText.Value = strNextCode
                    End If
                End If
            Catch ex As Exception
                Utilitarios.ManejadorErrores(ex, _sboApplication)
            End Try
        End Sub

        Private Sub _sboApplication_MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean) Handles _sboApplication.MenuEvent
            If Not Inicializado Then Return
            If _sboApplication.Forms.ActiveForm.TypeEx = Me.FormType Then
                If pVal.MenuUID = "1282" OrElse pVal.MenuUID = "1281" Then
                    AsignaMeses()
                    If (pVal.BeforeAction = False) AndAlso pVal.MenuUID = "1282" Then
                        ManejadorEventoLoad(pVal, BubbleEvent)
                    End If
                End If
            End If
        End Sub
    End Class

End Namespace