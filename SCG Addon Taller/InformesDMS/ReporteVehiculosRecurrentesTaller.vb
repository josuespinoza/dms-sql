Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports DMSOneFramework


Public Class ReporteVehiculosRecurrentesTaller : Implements IUsaPermisos, IFormularioSBO

#Region "Declaraciones"
    Private _FormType As String
    Private _NombreXml As String
    Private _Titulo As String
    Private _FormularioSBO As IForm
    Private _Inicializado As Boolean
    Private _ApplicationSBO As IApplication
    Private _CompanySBO As SAPbobsCOM.ICompany
    Private _IdMenu As String
    Private _MenuPadre As String
    Private _Posicion As Integer
    Private _Nombre As String

    ''Variables Globales
    Private oDbSCGD_MARCA As SAPbouiCOM.DBDataSource
    Private oDbSCGD_ESTILO As SAPbouiCOM.DBDataSource
    Private oDbSCGD_MODELO As SAPbouiCOM.DBDataSource

    Private oComboMarca As SAPbouiCOM.ComboBox
    Private oComboEstilo As SAPbouiCOM.ComboBox
    Private oComboModelo As SAPbouiCOM.ComboBox

    'Conection
    Private m_strConectionString As String
    Dim m_cn_Coneccion As New SqlClient.SqlConnection
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon


#End Region

#Region "Propiedades"

    Public ReadOnly Property ApplicationSBO As SAPbouiCOM.IApplication Implements SCG.SBOFramework.UI.IFormularioSBO.ApplicationSBO
        Get
            Return _ApplicationSBO
        End Get
    End Property

    Public ReadOnly Property CompanySBO As SAPbobsCOM.ICompany Implements SCG.SBOFramework.UI.IFormularioSBO.CompanySBO
        Get
            Return _CompanySBO
        End Get
    End Property

    Public Property FormType As String Implements SCG.SBOFramework.UI.IFormularioSBO.FormType
        Get
            Return _FormType
        End Get
        Set(value As String)
            _FormType = value
        End Set
    End Property

    Public Property FormularioSBO As SAPbouiCOM.IForm Implements SCG.SBOFramework.UI.IFormularioSBO.FormularioSBO
        Get
            Return _FormularioSBO
        End Get
        Set(value As SAPbouiCOM.IForm)
            _FormularioSBO = value
        End Set
    End Property

    Public Property Inicializado As Boolean Implements SCG.SBOFramework.UI.IFormularioSBO.Inicializado
        Get
            Return _Inicializado
        End Get
        Set(value As Boolean)
            _Inicializado = value
        End Set
    End Property

    Public Property NombreXml As String Implements SCG.SBOFramework.UI.IFormularioSBO.NombreXml
        Get
            Return _NombreXml
        End Get
        Set(value As String)
            _NombreXml = value
        End Set
    End Property

    Public Property Titulo As String Implements SCG.SBOFramework.UI.IFormularioSBO.Titulo
        Get
            Return _Titulo
        End Get
        Set(value As String)
            _Titulo = value
        End Set
    End Property

    Public Property IdMenu As String Implements SCG.SBOFramework.UI.IUsaMenu.IdMenu
        Get
            Return _IdMenu
        End Get
        Set(value As String)
            _IdMenu = value
        End Set
    End Property

    Public Property MenuPadre As String Implements SCG.SBOFramework.UI.IUsaMenu.MenuPadre
        Get
            Return _MenuPadre
        End Get
        Set(value As String)
            _MenuPadre = value
        End Set
    End Property

    Public Property Nombre As String Implements SCG.SBOFramework.UI.IUsaMenu.Nombre
        Get
            Return _Nombre
        End Get
        Set(value As String)
            _Nombre = value
        End Set
    End Property

    Public Property Posicion As Integer Implements SCG.SBOFramework.UI.IUsaMenu.Posicion
        Get
            Return _Posicion
        End Get
        Set(value As Integer)
            _Posicion = value
        End Set
    End Property

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Contructor para la aplicacion
    ''' </summary>
    ''' <param name="p_Application"></param>
    ''' <param name="p_CompanySbo"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub New(ByVal p_Application As Application, ByVal p_CompanySbo As SAPbobsCOM.ICompany, ByVal mc_strUISCGD_FormSCGD_RVRT As String)
        _CompanySBO = p_CompanySbo
        _ApplicationSBO = p_Application
        NombreXml = System.Windows.Forms.Application.StartupPath & My.Resources.Resource.XMLReporteVehiculosRecurrentesTaller
        MenuPadre = "SCGD_IND"
        Nombre = My.Resources.Resource.txtVehiculosRecurrentesTaller
        IdMenu = mc_strUISCGD_FormSCGD_RVRT
        Titulo = My.Resources.Resource.txtVehiculosRecurrentesTaller
        Posicion = 41
        FormType = mc_strUISCGD_FormSCGD_RVRT
    End Sub

#End Region

#Region "Eventos"
    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario
    End Sub
    ''' <summary>
    ''' Inicia los DbDataSource y los los valores del combo Marca
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        Try
            FormularioSBO.Freeze(True)
            ''DESHABILITA COMBOS
            FormularioSBO.Items.Item("cboEstilo").Enabled = False
            FormularioSBO.Items.Item("cboModelo").Enabled = False
            ''DBDATASOURCE
            oDbSCGD_MARCA = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_MARCA")
            oDbSCGD_ESTILO = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_ESTILO")
            oDbSCGD_MODELO = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_MODELO")
            ''COMBO
            oComboMarca = DirectCast(FormularioSBO.Items.Item("cboMarca").Specific, SAPbouiCOM.ComboBox)
            oComboEstilo = DirectCast(FormularioSBO.Items.Item("cboEstilo").Specific, SAPbouiCOM.ComboBox)
            oComboModelo = DirectCast(FormularioSBO.Items.Item("cboModelo").Specific, SAPbouiCOM.ComboBox)
            ''CARGA COMBO MARCA
            CargarValidValues(oDbSCGD_MARCA, oComboMarca)

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
        End Sub

    ''' <summary>
    ''' Maneja Evento de combo para cargar los valores relacionados al estilo o la marca
    ''' </summary>
    ''' <param name="p_Val"></param>
    ''' <param name="p_bubbleEvent"></param>
    ''' <remarks></remarks>
    Public Sub ManejadorEventoComboSelected(p_Val As ItemEvent, p_bubbleEvent As Boolean)
        Dim strCode As String
        Try
            Select Case p_Val.ItemUID
                Case "cboMarca"
                    If oComboMarca.Selected IsNot Nothing Then
                        FormularioSBO.Items.Item("cboEstilo").Enabled = True
                        FormularioSBO.Items.Item("cboModelo").Enabled = False
                        FormularioSBO.DataSources.UserDataSources.Item("udEstilo").Value = ""
                        FormularioSBO.DataSources.UserDataSources.Item("udModelo").Value = ""
                        strCode = oComboMarca.Selected.Value
                        CargarValidValues(oDbSCGD_ESTILO, oComboEstilo, strCode)
                    Else
                        FormularioSBO.Items.Item("cboEstilo").Enabled = False
                        FormularioSBO.Items.Item("cboModelo").Enabled = False
                        FormularioSBO.DataSources.UserDataSources.Item("udEstilo").Value = ""
                        FormularioSBO.DataSources.UserDataSources.Item("udModelo").Value = ""
                    End If
                    
                Case "cboEstilo"
                    If oComboEstilo.Selected IsNot Nothing Then
                        FormularioSBO.Items.Item("cboModelo").Enabled = True
                        FormularioSBO.DataSources.UserDataSources.Item("udModelo").Value = ""
                        strCode = oComboEstilo.Selected.Value
                        CargarValidValues(oDbSCGD_MODELO, oComboModelo, strCode)
                    Else
                        FormularioSBO.Items.Item("cboModelo").Enabled = False
                        FormularioSBO.DataSources.UserDataSources.Item("udModelo").Value = ""
                    End If

            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Caprtura el evento Itempressed del botón imprimir
    ''' </summary>
    ''' <param name="p_Val"></param>
    ''' <param name="p_bubbleEvent"></param>
    ''' <remarks></remarks>
    Public Sub ManejadorEventoItemPressed(p_Val As ItemEvent, p_bubbleEvent As Boolean)
        Dim strParametros As String
        Try
            If p_Val.ItemUID = "btnprint" Then
                If CargarParametros(strParametros) Then
                    ImprimirReporte(My.Resources.Resource.rptVehiculoRecurrentesTaller, My.Resources.Resource.rptVehiculoRecurrentesTallerTITULO, strParametros)
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
#End Region

#Region "Metodos"
    ''' <summary>
    ''' Método para cargar los Combos con valores válidos
    ''' </summary>
    ''' <param name="p_oDb"></param>
    ''' <param name="p_oCombo"></param>
    ''' <param name="p_strCode"></param>
    ''' <remarks></remarks>
    Private Sub CargarValidValues(p_oDb As DBDataSource, ByRef p_oCombo As ComboBox, Optional p_strCode As String = Nothing)

        Try
            Dim oConditions As SAPbouiCOM.Conditions
            Dim oCondition As SAPbouiCOM.Condition

            If p_oCombo.ValidValues.Count > 0 Then
                While p_oCombo.ValidValues.Count > 0
                    p_oCombo.ValidValues.Remove(p_oCombo.ValidValues.Item(0).Value, BoSearchKey.psk_ByValue)
                End While
            End If

            If Not String.IsNullOrEmpty(p_strCode) Then
                oConditions = ApplicationSBO.CreateObject(BoCreatableObjectType.cot_Conditions)
                oCondition = oConditions.Add()
                If p_oDb.TableName = "@SCGD_ESTILO" Then
                    oCondition.Alias = "U_Cod_Marc"
                Else
                    oCondition.Alias = "U_Cod_Esti"
                End If
                oCondition.Operation = BoConditionOperation.co_EQUAL
                oCondition.CondVal = p_strCode

                p_oDb.Query(oConditions)
                Else
                p_oDb.Query()
            End If

            p_oCombo.ValidValues.Add(" ", " ")
            
            If p_oDb.Size > 0 Then
                For i As Integer = 0 To p_oDb.Size - 1
                    p_oCombo.ValidValues.Add(p_oDb.GetValue(0, i).ToString().Trim(), p_oDb.GetValue(1, i).ToString().Trim())
                Next
            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Cargar los Parámetros para el reporte
    ''' </summary>
    ''' <param name="strParametros"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CargarParametros(ByRef strParametros As String) As Boolean
        Dim strMarca As String
        Dim strEstilo As String
        Dim strModelo As String
        Dim strAnoVehi As String
        Dim dtFi As Date
        Dim dtFf As Date

        Try
            strMarca = " AND VEHI.U_Cod_Marc = {0}"
            strEstilo = " AND VEHI.U_Cod_Esti = {0}"
            strModelo = " AND VEHI.U_Cod_Mode = {0}"
            strAnoVehi = " AND VEHI.U_Ano_Vehi ={0}"


            If Not String.IsNullOrEmpty(FormularioSBO.DataSources.UserDataSources.Item("udFi").Value) And Not String.IsNullOrEmpty(FormularioSBO.DataSources.UserDataSources.Item("udFf").Value) Then
                dtFi = FormularioSBO.DataSources.UserDataSources.Item("udFi").Value.Trim()
                dtFf = FormularioSBO.DataSources.UserDataSources.Item("udFf").Value.Trim()
                If dtFi <= dtFf Then

                    If oComboMarca.Selected IsNot Nothing Then
                        strParametros = String.Format(strMarca, oComboMarca.Selected.Value.Trim())

                        If oComboEstilo.Selected IsNot Nothing Then
                            strParametros = strParametros + String.Format(strEstilo, oComboEstilo.Selected.Value.Trim())

                            If oComboModelo.Selected IsNot Nothing Then
                                strParametros = strParametros + String.Format(strModelo, oComboModelo.Selected.Value.Trim())
                            End If
                        End If
                    End If
                    If Not String.IsNullOrEmpty(FormularioSBO.DataSources.UserDataSources.Item("udAno").Value.Trim()) Then
                        strParametros = strParametros + String.Format(strAnoVehi, FormularioSBO.DataSources.UserDataSources.Item("udAno").Value.Trim())
                    End If

                    strParametros = strParametros & "," & dtFf.ToString() & "," & dtFi.ToString()
                Else
                    ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.ErrorFechaInicioMenoraFechaFin, BoMessageTime.bmt_Short, False)
                    Return False
                End If

                Return True
            Else
                ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.ErrorIngresarFechaFinFechaInicio, BoMessageTime.bmt_Short, False)
                Return False
            End If


        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try

    End Function

    'Imprimir reportes
    <System.CLSCompliant(False)> _
    Public Sub ImprimirReporte(ByVal strDireccionReporte As String, _
                               ByVal strBarraTitulo As String, _
                               ByVal strParametros As String)
        Try

            Dim strPathExe As String = String.Empty

            objConfiguracionGeneral = Nothing

            Configuracion.CrearCadenaDeconexion(CompanySBO.Server, CompanySBO.CompanyDB, m_strConectionString)
            If m_cn_Coneccion.State = ConnectionState.Open Then
                m_cn_Coneccion.Close()
            End If
            m_cn_Coneccion.ConnectionString = m_strConectionString

            objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(m_cn_Coneccion)

            strDireccionReporte = objConfiguracionGeneral.DireccionReportes & strDireccionReporte
            strDireccionReporte = strDireccionReporte.Replace(" ", "°")
            strParametros = strParametros.Replace(" ", "°")

            strBarraTitulo = strBarraTitulo.Replace(" ", "°")

            strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

            strPathExe &= strBarraTitulo & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & CompanySBO.Server & "," & CompanySBO.CompanyDB & " " & strParametros
            Shell(strPathExe, AppWinStyle.MaximizedFocus)

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
#End Region

End Class
