Option Strict On

Imports System.Drawing
Imports DMS_Addon.My.Resources
Imports System.Globalization
Imports System.Collections.Generic
Imports DMS_Addon.LlamadaServicio
Imports System.IO
Imports SAPbouiCOM
Imports System.Xml
Imports Company = SAPbobsCOM.Company
Imports DMSOneFramework
Imports DMSOneFramework.CitasTableAdapters

Namespace Agendas
    <CLSCompliant(False)> _
    Public Class FormularioAgendaSBO
        Private _sboCompany As SAPbobsCOM.Company
        Private WithEvents _sboApplication As Application
        Private _sboForm As Form

        Public Const IdMenuAgendas As String = "SCGD_AGN"
        Public Const FormType As String = "SCGD_AGE"
        Public Shared HoraInicioTaller As String = String.Empty
        Public Shared HoraFinTaller As String = String.Empty
        Public Shared ConnectionStringTaller As String = String.Empty
        Public Shared ConnectionStringSBO As String = String.Empty
        Public Shared txtFechaID As String = "txtFecha"
        Public Shared txtAgendaID As String = "txtAgenda"
        Public Shared Modal As Boolean
        Public Shared ActualizandoFormularioPadre As Boolean = False
        Private _listaCitas As List(Of DatosCita) = New List(Of DatosCita)(10)

        Private Const _IdMenuServicio As String = "3584"
        Private _dataSetCitas As Citas = New Citas()
        Private _codigoAgenda As Integer = 1
        Private _intervalo As Single
        Private _areaCitas As Rectangle
        Private _puntoInicio As Rectangle
        Private _listaLabelsFechas(6) As Item
        Private _resultadoFormulario As Nullable(Of Date) = Nothing
        Private _fechaInicioLabels As Date
        Private _backColorAntiguo As Integer
        Private _formularioPadre As DatosFormularioPadre

        <CLSCompliant(False)> _
        Public Sub New(ByRef p_SBO_Aplication As Application, ByRef p_oCompania As Company)
            _sboApplication = p_SBO_Aplication
            _sboCompany = p_oCompania
            InicializaDatosIniciales()
        End Sub

        Private Sub InicializaDatosIniciales()
            _areaCitas.Width = 90
            _areaCitas.Height = 15

            _puntoInicio.X = 75
            _puntoInicio.Y = 55

            _formularioPadre = New DatosFormularioPadre()

            If String.IsNullOrEmpty(ConnectionStringTaller) Then
                Utilitarios.DevuelveCadenaConexionBDTaller(_sboApplication, ConnectionStringTaller)
            End If

            If String.IsNullOrEmpty(ConnectionStringSBO) Then
                Configuracion.CrearCadenaDeconexion(_sboCompany.Server, _sboCompany.CompanyDB, ConnectionStringSBO)
            End If
        End Sub

        Public Sub AgregaMenu()

            Dim oMenus As Menus
            Dim oMenuItem As MenuItem

            oMenus = _sboApplication.Menus

            Dim oCreationPackage As MenuCreationParams
            oCreationPackage = _
                CType(_sboApplication.CreateObject(BoCreatableObjectType.cot_MenuCreationParams), MenuCreationParams)

            oMenuItem = _sboApplication.Menus.Item(_IdMenuServicio)
            oMenus = oMenuItem.SubMenus
            
            oCreationPackage.Position = 1
            oCreationPackage.Type = BoMenuType.mt_STRING
            oCreationPackage.UniqueID = IdMenuAgendas
            oCreationPackage.String = Resource.Agenda


        End Sub

        Public Sub CargarFormulario()

            'si no se ha cargado el horario del taller, cargarlo
            If (String.IsNullOrEmpty(HoraInicioTaller)) Then
                Dim adpHorario As SCGTA_TB_HorarioTallerTableAdapter = New SCGTA_TB_HorarioTallerTableAdapter()
                adpHorario.Connection.ConnectionString = ConnectionStringTaller
                adpHorario.Fill(_dataSetCitas.SCGTA_TB_HorarioTaller)
                If (_dataSetCitas.SCGTA_TB_HorarioTaller.Rows.Count = 0) Then
                    _sboApplication.StatusBar.SetText(Resource.SinHorario, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return
                End If
                Dim _
                    row As Citas.SCGTA_TB_HorarioTallerRow = _
                        DirectCast(_dataSetCitas.SCGTA_TB_HorarioTaller.Rows(0), Citas.SCGTA_TB_HorarioTallerRow)
                HoraInicioTaller = row.FechaIni.TimeOfDay.TotalHours.ToString()
                HoraFinTaller = row.FechaFin.TimeOfDay.TotalHours.ToString()
            End If

            Dim fcp As FormCreationParams

            fcp = _
                DirectCast(_sboApplication.CreateObject(BoCreatableObjectType.cot_FormCreationParams),  _
                    FormCreationParams)
            fcp.FormType = FormType

            CargarDesdeXML(fcp)
            AgregaDataSource()

            ReDim _listaLabelsFechas(6)
            _listaCitas.Clear()
            Dim d As Date = ObtieneFechaTextBox()

            CargaNombreAgenda()
            DibujaFechas(d)
            CargaCitas()

            _resultadoFormulario = Nothing
        End Sub

        Private Sub AgregaDataSource()
            If _sboForm IsNot Nothing Then

                With _sboForm

                    Dim dataTable As DataTable = .DataSources.DataTables.Add("AAA")
                    dataTable.Columns.Add(UID:="FecHoy", ColFieldType:=BoFieldsType.ft_Date)
                    dataTable.Rows.Add(1)
                    dataTable.SetValue(Column:="FecHoy", rowIndex:=0, Value:=Date.Today)

                    Dim txtItem As Item = _sboForm.Items.Item(txtFechaID)
                    Dim txtSpecific As EditText = DirectCast(txtItem.Specific, EditText)

                    txtSpecific.DataBind.Bind(UID:="AAA", columnUid:="FecHoy")

                End With

            End If
        End Sub

        Private Function CargarDesdeXML(ByVal fcp As FormCreationParams) As String
            Dim adapterUTAgenda As SBO_SCG_AGENDACITATableAdapter = New SBO_SCG_AGENDACITATableAdapter()
            adapterUTAgenda.CadenaConexion = ConnectionStringSBO
            adapterUTAgenda.FillBy(_dataSetCitas.SBO_SCG_AGENDACITA, _codigoAgenda.ToString())

            Dim adapterAgendas As SCGTA_TB_AgendaTableAdapter = New SCGTA_TB_AgendaTableAdapter()
            adapterAgendas.Connection.ConnectionString = ConnectionStringTaller
            adapterAgendas.FillBy(_dataSetCitas.SCGTA_TB_Agenda, _codigoAgenda)
            Dim _
                rowAgenda As Citas.SCGTA_TB_AgendaRow = _
                    DirectCast(_dataSetCitas.SCGTA_TB_Agenda.Rows(0), Citas.SCGTA_TB_AgendaRow)

            Dim rowSboAgenda As Citas.SBO_SCG_AGENDACITARow = Nothing
            If (_dataSetCitas.SBO_SCG_AGENDACITA.Rows.Count <> 0) Then _
                rowSboAgenda = DirectCast(_dataSetCitas.SBO_SCG_AGENDACITA.Rows(0), Citas.SBO_SCG_AGENDACITARow)
            If _
                Not rowSboAgenda Is Nothing AndAlso Not String.IsNullOrEmpty(rowSboAgenda.U_XMLData) AndAlso _
                rowSboAgenda.U_HoraIn = HoraInicioTaller AndAlso rowSboAgenda.U_HoraFin = HoraFinTaller AndAlso _
                rowSboAgenda.U_Interv = rowAgenda.IntervaloCitas Then
                fcp.XmlData = rowSboAgenda.U_XMLData
                _intervalo = rowSboAgenda.U_Interv
                _sboForm = _sboApplication.Forms.AddEx(fcp)
                Return fcp.XmlData
            Else
                Dim strPath As String = System.Windows.Forms.Application.StartupPath & "\" & My.Resources.Resource.XMLAgenda
                Dim readAllText As String = File.ReadAllText(strPath)
                fcp.XmlData = readAllText
                _sboForm = _sboApplication.Forms.AddEx(fcp)

                DibujaAgenda(CSng(rowAgenda.IntervaloCitas / 60))

                If rowSboAgenda Is Nothing Then
                    _dataSetCitas.SBO_SCG_AGENDACITA.AddSBO_SCG_AGENDACITARow(_codigoAgenda.ToString(), rowAgenda.Agenda, rowAgenda.IntervaloCitas, _
                                                                          _sboForm.GetAsXML(), HoraInicioTaller, _
                                                                          HoraFinTaller)
                Else
                    rowSboAgenda.U_HoraIn = HoraInicioTaller
                    rowSboAgenda.U_HoraFin = HoraFinTaller
                    rowSboAgenda.U_Interv = rowAgenda.IntervaloCitas
                    _intervalo = rowSboAgenda.U_Interv
                    rowSboAgenda.U_XMLData = _sboForm.GetAsXML()
                End If

                adapterUTAgenda.Update(_dataSetCitas.SBO_SCG_AGENDACITA)
                Return readAllText
            End If

        End Function

        Private Sub DibujaAgenda(ByVal intervalo As Single)
            _sboForm.Freeze(True)


            Dim fecha As Date = Date.Now
            Dim hI As Single = CSng(HoraInicioTaller)
            Dim hF As Single = CSng(HoraFinTaller)

            Dim rectItem As Item = Nothing
            Dim diaItem As Item = Nothing
            Dim diaLabel As StaticText
            Dim yaEstanLasHoras As Boolean = False

            For x As Integer = 0 To 6
                Dim cont As Integer = 0

                For y As Single = hI To hF Step intervalo
                    rectItem = _sboForm.Items.Add("SCGD_rc" & x & cont, BoFormItemTypes.it_EDIT)
                    rectItem.Enabled = False
                    rectItem.Left = _puntoInicio.X + (_areaCitas.Width + 5) * x
                    rectItem.Width = _areaCitas.Width
                    rectItem.Height = _areaCitas.Height
                    rectItem.Top = _puntoInicio.Y + (_areaCitas.Height + 5) * cont

                    If Not yaEstanLasHoras Then
                        diaItem = _sboForm.Items.Add("SCGD_lb" & x & cont, BoFormItemTypes.it_STATIC)
                        diaItem.Left = _puntoInicio.X - 60
                        diaItem.Top = _puntoInicio.Y + (_areaCitas.Height + 5) * cont
                        diaItem.Width = 50
                        diaItem.TextStyle = 2
                        diaLabel = DirectCast(diaItem.Specific, StaticText)
                        Dim h As Integer
                        Dim m As Integer
                        h = CInt(Math.Floor(y))
                        m = Math.Abs(CInt((h - y) * 60))
                        If m = 60 Then
                            m = 0
                            h = h + 1
                        End If
                        Dim _
                            d As Date = _
                                New Date(fecha.Year, fecha.Month, fecha.Day, h, _
                                          m, 0)

                        diaLabel.Caption = d.ToString("hh:mm tt")
                    End If
                    cont = cont + 1
                Next
                yaEstanLasHoras = True
            Next
            If Not rectItem Is Nothing Then
                _sboForm.Height = rectItem.Top + rectItem.Height + 60
                _sboForm.Width = rectItem.Left + rectItem.Width + 60
            End If

            _sboForm.Freeze(False)
        End Sub

        Public Sub DibujaFechas(ByVal fecha As DateTime)
            Dim diaItem As Item = Nothing
            Dim diaLabel As StaticText
            Dim formato As String

            _sboForm.Freeze(True)
            formato = Utilitarios.ObtieneFormatoFecha(_sboApplication, _sboCompany)

            For x As Integer = 0 To 6
                If _listaLabelsFechas(x) Is Nothing Then
                    diaItem = _sboForm.Items.Add("SCGD_lb" & x, BoFormItemTypes.it_STATIC)
                    _listaLabelsFechas(x) = diaItem
                    _listaLabelsFechas(x).Left = _puntoInicio.X + (_areaCitas.Width + 5) * x + 3
                    _listaLabelsFechas(x).Top = _puntoInicio.Y - 15
                    _listaLabelsFechas(x).TextStyle = 2
                End If
                diaLabel = DirectCast(_listaLabelsFechas(x).Specific, StaticText)
                diaLabel.Caption = fecha.AddDays(x).ToString(formato)
            Next
            _sboForm.Freeze(False)

        End Sub

        Public Property CodigoAgenda() As Integer
            Get
                Return _codigoAgenda
            End Get
            Set(ByVal value As Integer)
                _codigoAgenda = value
            End Set
        End Property

        <CLSCompliant(False)> _
        Public Sub ManejadorEventoItemClicked(ByVal FormUID As String, _
                                              ByRef pVal As SAPbouiCOM.ItemEvent, _
                                              ByRef BubbleEvent As Boolean)
            Dim pos As Integer = -1

            Dim stringUID As String = pVal.ItemUID.ToString()
            If (pVal.ActionSuccess AndAlso stringUID.StartsWith("SCGD_rc")) Then

                '                For citasActuales As Integer = 0 To _listaCitas.Count - 1
                '                    dc = _listaCitas(citasActuales)
                '                    If (dc.sboItem.UniqueID = pVal.ItemUID) Then
                '                        _sboApplication.StatusBar.SetText(My.Resources.Resource.ExisteCitaEnFecha, BoMessageTime.bmt_Short,  BoStatusBarMessageType.smt_Error)
                '                        _resultadoFormulario = Nothing
                '                        Return
                '                    End If
                '                Next
                'si llega a este punto es pq no hay cita en esa fecha
                Dim x As Integer = Integer.Parse(stringUID(7))
                Dim y As Integer = CInt(stringUID.Remove(0, 8))
                _resultadoFormulario = ObtieneFechaDeXY(x, y)
                If _resultadoFormulario < Date.Now Then
                    _sboApplication.StatusBar.SetText(My.Resources.Resource.FechaInvalidaCita, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    _resultadoFormulario = Nothing
                    Return
                End If
                If _formularioPadre.Actualiza Then
                    ActualizaFormularioPadre()
                End If
                _sboForm.Close()
            End If

        End Sub

        <CLSCompliant(False)> _
        Public Sub ManejadorEventoKeyDown(ByVal FormUID As String, _
                                              ByRef pVal As SAPbouiCOM.ItemEvent, _
                                              ByRef BubbleEvent As Boolean)
            If pVal.ItemUID = txtFechaID AndAlso pVal.CharPressed = 13 Then
                Dim d As Date = ObtieneFechaTextBox()
                DibujaFechas(d)
                CargaCitas()
            End If
        End Sub

        Private Function ObtieneFechaTextBox() As Date
            Dim txtItem As Item = _sboForm.Items.Item(txtFechaID)
            Dim txtSpecific As EditText = DirectCast(txtItem.Specific, EditText)
            Dim a As String = txtSpecific.Value
            Dim formato As String = Utilitarios.ObtieneFormatoFecha(_sboApplication, _sboCompany)
            If String.IsNullOrEmpty(a) Then
                txtSpecific.Value = Date.Now.ToString(formato)
                a = txtSpecific.Value
            End If
            Return DateTime.ParseExact(a, "yyyyMMdd", Nothing)
        End Function

        Private Sub CargaCitas()
            Dim adapterCitas As SCGTA_TB_CitaTableAdapter = New SCGTA_TB_CitaTableAdapter()
            adapterCitas.Connection.ConnectionString = ConnectionStringTaller

            _sboForm.Freeze(True)
            Dim citaItem As Item
            Dim citaText As EditText
            For Each datoCita As DatosCita In _listaCitas
                citaItem = datoCita.sboItem
                citaItem.Description = String.Empty
                citaText = DirectCast(citaItem.Specific, EditText)
                citaText.Value = Nothing
                citaText.BackColor = _backColorAntiguo
            Next
            _listaCitas.Clear()

            Dim fInicio As Date = ObtieneFechaTextBox()
            _fechaInicioLabels = fInicio
            Dim fFin As Date = fInicio.AddDays(7)
            adapterCitas.FillBy(_dataSetCitas.SCGTA_TB_Cita, _codigoAgenda, fInicio, fFin)
            If (_dataSetCitas.SCGTA_TB_Cita.Rows.Count <> 0) Then
                For Each rowCita As Citas.SCGTA_TB_CitaRow In _dataSetCitas.SCGTA_TB_Cita.Rows
                    Dim posicionX As Integer = rowCita.FechayHora.Subtract(fInicio).Days
                    Dim posicionY As Integer = 0

                    Dim hI As Single = CSng(HoraInicioTaller)
                    Dim hF As Single = CSng(HoraFinTaller)

                    For y As Single = hI To hF Step _intervalo / 60
                        If rowCita.FechayHora.TimeOfDay.TotalHours < y + _intervalo / 60 Then Exit For
                        posicionY = posicionY + 1
                    Next

                    Dim dc As DatosCita = Nothing
                    For Each cita As DatosCita In _listaCitas
                        If cita.posX = posicionX AndAlso cita.posY = posicionY Then 'Ya está
                            dc = cita
                            Exit For
                        End If
                    Next

                    If dc IsNot Nothing Then 'ya está
                        dc.sboItem.Description &= System.Environment.NewLine + "---" + System.Environment.NewLine + String.Format(My.Resources.Resource.TTipAgenda, System.Environment.NewLine, rowCita.NoCita, rowCita.NoCotizacion, rowCita.FechayHora.ToString("hh:mm tt"))
                    Else 'no está
                        citaItem = _sboForm.Items.Item("SCGD_rc" & posicionX & posicionY)
                        citaText = DirectCast(citaItem.Specific, EditText)
                        citaText.Value = rowCita.NoCita
                        _backColorAntiguo = citaText.BackColor
                        citaText.BackColor = Color.YellowGreen.ToArgb()
                        citaItem.Description = String.Format(My.Resources.Resource.TTipAgenda, System.Environment.NewLine, rowCita.NoCita, rowCita.NoCotizacion, rowCita.FechayHora.ToString("hh:mm tt"))
                        dc = New DatosCita()
                        dc.sboItem = citaItem
                        dc.posX = posicionX
                        dc.posY = posicionY
                        dc.IdCita = rowCita.IDCita
                        _listaCitas.Add(dc)
                    End If

                Next
            End If
            _sboForm.Freeze(False)
        End Sub

        Public Function ObtieneFechaDeXY(ByVal x As Integer, ByVal y As Integer) As Date
            Dim hI As Single = CSng(HoraInicioTaller)
            Dim hF As Single = CSng(HoraFinTaller)
            Dim hora As Single = 0

            Dim cantY As Integer = 0
            For hora = hI To hF Step _intervalo / 60

                If y = cantY Then Exit For
                cantY = cantY + 1
            Next

            Dim h As Integer
            Dim m As Integer
            h = CInt(Math.Floor(hora))
            m = Math.Abs(CInt((h - hora) * 60))
            If m = 60 Then
                m = 0
                h = h + 1
            End If

            Dim resultado As Date = New Date(_fechaInicioLabels.Year, _fechaInicioLabels.Month, _fechaInicioLabels.Day, h, _
                                          m, 0)
            Return resultado.AddDays(x)

        End Function

        Public Sub CargaNombreAgenda()
            Dim sboItem As Item
            Dim sboText As EditText

            Dim adapterAgenda As SCGTA_TB_AgendaTableAdapter = New SCGTA_TB_AgendaTableAdapter()
            adapterAgenda.Connection.ConnectionString = ConnectionStringTaller
            adapterAgenda.FillBy(_dataSetCitas.SCGTA_TB_Agenda, _codigoAgenda)

            _sboForm.Freeze(True)

            sboItem = _sboForm.Items.Item(txtAgendaID)
            sboText = DirectCast(sboItem.Specific, EditText)
            sboText.Value = _dataSetCitas.SCGTA_TB_Agenda.Rows(0).Item("Agenda").ToString()

            _sboForm.Freeze(False)
        End Sub

        Public Property FormularioPadre() As DatosFormularioPadre
            Get
                Return _formularioPadre
            End Get
            Set(ByVal value As DatosFormularioPadre)
                _formularioPadre = value
            End Set
        End Property

        Public Sub ActualizaFormularioPadre()
            Dim sboItem As Item
            Dim sboEdit As EditText
            Dim sboFormPadre As Form
            Dim formato As String
            Dim hora As String
            Dim minutos As String
            
            formato = Utilitarios.ObtieneFormatoFecha(_sboApplication, _sboCompany)

            ActualizandoFormularioPadre = True

            sboFormPadre = _sboApplication.Forms.Item(_formularioPadre.SboFormID)
            sboItem = sboFormPadre.Items.Item(FormularioLLamadaServicioSBO.TxtFechaCita)
            sboEdit = DirectCast(sboItem.Specific, EditText)
            sboEdit.Value = _resultadoFormulario.Value.ToString("yyyyMMdd")

            sboItem = sboFormPadre.Items.Item(FormularioLLamadaServicioSBO.TxtHoraCita)

            sboEdit = DirectCast(sboItem.Specific, EditText)
            hora = _resultadoFormulario.Value.ToString("HH")
            minutos = _resultadoFormulario.Value.ToString("mm")
            hora = hora + minutos
            'sboEdit.Value = _resultadoFormulario.Value.ToString("HH:mm")
            sboEdit.Value = hora
            Dim a As String = sboEdit.Value


            ActualizandoFormularioPadre = False
        End Sub
    End Class
End Namespace
