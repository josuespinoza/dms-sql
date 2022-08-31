Imports SAPbouiCOM
Imports SAPbobsCOM
Imports SCG.DMSOne.Framework.MenuManager
Imports System.Globalization
Imports SCG.SBOFramework
Imports System.Xml
Imports System.IO
Imports System.Collections.Generic
Imports SCG.Cifrado
Imports SCG.Integration.InterfaceDPM
Imports SCG.Integration.InterfaceDPM.Entities
Imports System.Reflection
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany
Imports RestSharp

Module InterfaceJohnDeereModulo
    Private WithEvents oApplication As SAPbouiCOM.Application
    Private oCompany As SAPbobsCOM.Company
    Private oFormulario As SAPbouiCOM.Form
    Private n As NumberFormatInfo
    Private oForm As SAPbouiCOM.Form
    Private oTimer As System.Timers.Timer
    Private formID As String = "SCGD_IJD"
    Private gridJD As Grid
    Public dtMatrix As DataTable
    Private _udsFormulario As UserDataSources
    Public cboTipoCarga As ComboBoxSBO
    Private _rbtnDelta As OptionBtnSBO
    Private _rbtnInit As OptionBtnSBO
    Public _txtResul As EditTextSBO
    Public _txtRutaC As EditTextSBO
    Public _txtRutaD As EditTextSBO
    Public _txtResulD As EditTextSBO
    Public _txtORD As EditTextSBO
    Public _txtPORD As EditTextSBO
    Public _txtXFER As EditTextSBO
    Public _txtRXFER As EditTextSBO
    Private strRuta As String = String.Empty

    Enum Accion
        CargarArchivo
        DescargarArchivo
    End Enum
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        Try
            oApplication = DMS_Connector.Company.ApplicationSBO
            oCompany = DMS_Connector.Company.CompanySBO
            'oForm = oApplication.Forms.Item("SCGD_IJD")
            n = DIHelper.GetNumberFormatInfo(oCompany)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


#Region "Eventos"
    Public Sub SBO_Application_ItemEvent(ByVal FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Handles oApplication.ItemEvent
        Dim oInterfaceJohnDeere As InterfaceJohnDeere
        Dim oInterfaceJohnDeerePMM As InterfaceJohnDeere_PMM
        Dim oInterfaceJohnDeereDTFAPI As InterfaceJohnDeere_DTFAPI
        Dim oInterfaceJohnDeereORD As InterfaceJohnDeere_ORD
        Dim oInterfaceJohnDeereXFER As InterfaceJohnDeere_XFER
        Dim strTipoCarga As String = String.Empty
        Dim strDocEntry As String = String.Empty
        Try
            If pVal.FormTypeEx = formID Then
                If pVal.EventType <> BoEventTypes.et_FORM_UNLOAD Then
                    If pVal.Before_Action Then
                    Else
                        Select Case pVal.EventType
                            Case BoEventTypes.et_ITEM_PRESSED
                                Select Case pVal.ItemUID
                                    Case "btnCargar"
                                        dtMatrix.Rows.Clear()
                                        gridJD = oForm.Items.Item("gridJD").Specific
                                        gridJD.DataTable = Nothing
                                        oForm.Freeze(True)
                                        InicializarTimer()

                                        If _rbtnDelta.ObtieneValorUserDataSource = "Y" Then
                                            strTipoCarga = "D"
                                        Else
                                            strTipoCarga = "I"
                                        End If
                                        oInterfaceJohnDeere = New InterfaceJohnDeere(oApplication, oCompany, oForm)
                                        oInterfaceJohnDeere.ManejaInterfaceJohnDeere_JDPRISM(dtMatrix, strTipoCarga)

                                        gridJD.DataTable = dtMatrix
                                        DetenerTimer()
                                        oForm.Freeze(False)
                                    Case "rbtnDelta"
                                        ManejarEventoRadioButton(pVal)
                                    Case "rbtnInit"
                                        ManejarEventoRadioButton(pVal)
                                    Case "btnPMM"
                                        oInterfaceJohnDeerePMM = New InterfaceJohnDeere_PMM(oApplication, oCompany, oForm)
                                        oInterfaceJohnDeerePMM.ManejaInterfaceJohnDeere_PMM()
                                    Case "btnSelC"
                                        strRuta = String.Empty
                                        IniciarProcesoFileDialog()
                                        oForm.Items.Item("txtRutaC").Specific.String = strRuta
                                    Case "btnUbi"
                                        strRuta = String.Empty
                                        IniciarProcesoFolderDialog()
                                        oForm.Items.Item("txtRutaD").Specific.String = strRuta
                                    Case "btnCA"
                                        oInterfaceJohnDeereDTFAPI = New InterfaceJohnDeere_DTFAPI(oApplication, oCompany, oForm)
                                        oInterfaceJohnDeereDTFAPI.ManejaInterfaceJohnDeere_DTFAPI("CargarArchivo", _txtRutaC.ObtieneValorUserDataSource().ToString())
                                    Case "btnORD"
                                        strRuta = String.Empty
                                        IniciarProcesoFileDialog()
                                        oForm.Items.Item("txtORD").Specific.String = strRuta
                                    Case "btnPORD"
                                        oInterfaceJohnDeereORD = New InterfaceJohnDeere_ORD(oApplication, oCompany, oForm)
                                        oInterfaceJohnDeereORD.ManejaInterfaceJohnDeere_ORD(_txtORD.ObtieneValorUserDataSource().ToString(), strDocEntry)
                                        If Not String.IsNullOrEmpty(strDocEntry) Then oForm.Items.Item("txtPORD").Specific.String = strDocEntry
                                    Case "btnXFER"
                                        strRuta = String.Empty
                                        IniciarProcesoFileDialog()
                                        oForm.Items.Item("txtXFER").Specific.String = strRuta
                                    Case "btnPXFER"
                                        oInterfaceJohnDeereXFER = New InterfaceJohnDeere_XFER(oApplication, oCompany, oForm)
                                        oInterfaceJohnDeereXFER.ManejaInterfaceJohnDeere_XFER(_txtXFER.ObtieneValorUserDataSource().ToString(), strDocEntry)
                                        If Not String.IsNullOrEmpty(strDocEntry) Then oForm.Items.Item("txtRXFER").Specific.String = strDocEntry
                                End Select
                        End Select
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
#End Region

#Region "Metodos"
    Public Sub InicializarControles(ByRef p_oForm As SAPbouiCOM.Form)
        Try
            p_oForm.DataSources.DataTables.Add("dtJDPRISM")
            dtMatrix = p_oForm.DataSources.DataTables.Item("dtJDPRISM")
            CrearColumnasDataTable(dtMatrix)

            'CargarCombos(p_oForm)

            '** User data source ***
            _udsFormulario = p_oForm.DataSources.UserDataSources

            _udsFormulario.Add("rbtnDelta", BoDataType.dt_LONG_TEXT, 150)
            _udsFormulario.Add("rbtnInit", BoDataType.dt_LONG_TEXT, 150)
            _udsFormulario.Add("txtResul", BoDataType.dt_LONG_TEXT, 250)
            _udsFormulario.Add("txtRutaC", BoDataType.dt_LONG_TEXT, 250)
            _udsFormulario.Add("txtRutaD", BoDataType.dt_LONG_TEXT, 250)
            _udsFormulario.Add("txtResulD", BoDataType.dt_LONG_TEXT, 250)
            _udsFormulario.Add("txtORD", BoDataType.dt_LONG_TEXT, 250)
            _udsFormulario.Add("txtPORD", BoDataType.dt_LONG_TEXT, 250)
            _udsFormulario.Add("txtXFER", BoDataType.dt_LONG_TEXT, 250)
            _udsFormulario.Add("txtRXFER", BoDataType.dt_LONG_TEXT, 250)

            _rbtnDelta = New OptionBtnSBO("rbtnDelta", True, "", "rbtnDelta", p_oForm)
            _rbtnDelta.AsignaBinding()
            _rbtnDelta.AsignaValorUserDataSource("Y")

            _rbtnInit = New OptionBtnSBO("rbtnInit", True, "", "rbtnInit", p_oForm)
            _rbtnInit.AsignaBinding()
            _rbtnInit.AsignaValorUserDataSource("N")

            _txtResul = New EditTextSBO("txtResul", True, "", "txtResul", p_oForm)
            _txtResul.AsignaBinding()

            _txtRutaC = New EditTextSBO("txtRutaC", True, "", "txtRutaC", p_oForm)
            _txtRutaC.AsignaBinding()

            _txtRutaD = New EditTextSBO("txtRutaD", True, "", "txtRutaD", p_oForm)
            _txtRutaD.AsignaBinding()

            _txtResulD = New EditTextSBO("txtResulD", True, "", "txtResulD", p_oForm)
            _txtResulD.AsignaBinding()

            _txtORD = New EditTextSBO("txtORD", True, "", "txtORD", p_oForm)
            _txtORD.AsignaBinding()

            _txtPORD = New EditTextSBO("txtPORD", True, "", "txtPORD", p_oForm)
            _txtPORD.AsignaBinding()

            _txtXFER = New EditTextSBO("txtXFER", True, "", "txtXFER", p_oForm)
            _txtXFER.AsignaBinding()

            _txtRXFER = New EditTextSBO("txtRXFER", True, "", "txtRXFER", p_oForm)
            _txtRXFER.AsignaBinding()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub AbrirFormulario()
        Dim oFormCreationParams As FormCreationParams
        Dim Path As String = String.Empty
        'Dim oForm As SAPbouiCOM.Form
        Dim oMatrix As Matrix

        Try
            oFormCreationParams = DMS_Connector.Company.ApplicationSBO.CreateObject(BoCreatableObjectType.cot_FormCreationParams)
            oFormCreationParams.BorderStyle = BoFormBorderStyle.fbs_Sizable
            oFormCreationParams.FormType = "SCGD_IJD"

            Path = My.Resources.Resource.XMLInterfaceJohnDeere
            oFormCreationParams.XmlData = CargarDesdeXML(Path)

            oForm = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(oFormCreationParams)

            InicializarControles(oForm)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ManejarEventoRadioButton(ByRef pVal As SAPbouiCOM.ItemEvent)
        Try
            Select Case pVal.ItemUID
                Case _rbtnDelta.UniqueId
                    _rbtnDelta.AsignaValorUserDataSource("Y")
                    _rbtnInit.AsignaValorUserDataSource("N")
                Case _rbtnInit.UniqueId
                    _rbtnInit.AsignaValorUserDataSource("Y")
                    _rbtnDelta.AsignaValorUserDataSource("N")
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub CrearColumnasDataTable(ByRef p_dtMatrix As DataTable)
        Dim oRecord As DetailJDPRISM
        Dim properties As PropertyInfo()
        Try
            oRecord = New DetailJDPRISM

            properties = GetType(DetailJDPRISM).GetProperties()

            For Each p As PropertyInfo In properties
                p_dtMatrix.Columns.Add(p.Name, BoFieldsType.ft_AlphaNumeric)
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para cargar las formas desde el archivo XML
    ''' </summary>
    ''' <param name="strFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CargarDesdeXML(ByRef strFileName As String) As String
        Dim oXMLDoc As XmlDocument
        Dim strPath As String

        strPath = Windows.Forms.Application.StartupPath & strFileName
        oXMLDoc = New XmlDocument()

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml
    End Function
    ''' <summary>
    ''' Metodo para agregar el menú de Tareas de Implementación a SAP
    ''' </summary>
    ''' <param name="pIndependiente"> True = Menu dentro del estándar de SAP - False = Menu dentro de las Configuraciones de DMS</param>
    ''' <remarks></remarks>
    Public Sub AgregarMenu()
        Dim strTitulo As String = "Interface John Deere (DPM)"
        Dim strIDMenu As String = "SCGD_IJD"
        Dim intPosicion As Integer = 18
        Dim oMenuItem As SAPbouiCOM.MenuItem
        Dim oMenus As SAPbouiCOM.Menus
        Dim oCreationPackage As SAPbouiCOM.MenuCreationParams
        Try
            If PermisosValidos() Then
                GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(strIDMenu, SAPbouiCOM.BoMenuType.mt_STRING, strTitulo, intPosicion, False, True, "SCGD_IND"))
            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para Validar el Permiso SCGD_OTDI
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PermisosValidos() As Boolean
        Dim blnPermisoValido As Boolean = False
        Try
            If Utilitarios.MostrarMenu("SCGD_IJD", DMS_Connector.Company.ApplicationSBO.Company.UserName) Then
                blnPermisoValido = True
            End If
            Return blnPermisoValido
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Private Sub InicializarTimer()
        Try
            'Inicializa un timer que se ejecuta cada 30 segundos
            'y llama al método LimpiarColaMensajes
            oTimer = New System.Timers.Timer(30000)
            RemoveHandler oTimer.Elapsed, AddressOf LimpiarColaMensajes
            AddHandler oTimer.Elapsed, AddressOf LimpiarColaMensajes
            oTimer.AutoReset = True
            oTimer.Enabled = True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub DetenerTimer()
        Try
            oTimer.Stop()
            oTimer.Dispose()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub LimpiarColaMensajes()
        Try
            'En las operaciones muy largas, la cola de mensajes se llena ocasionando que el add-on se desconecte y genere errores como
            'RPC Server call o similares. Para solucionarlo se debe ejecutar este método cada cierto tiempo (30 o 60 segundos) para limpiar
            'la cola de mensajes
            DMS_Connector.Company.ApplicationSBO.RemoveWindowsMessage(SAPbouiCOM.BoWindowsMessageType.bo_WM_TIMER, True)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub IniciarProcesoFileDialog()
        Dim threadGetFile As Threading.Thread
        Try
            threadGetFile = New Threading.Thread(New Threading.ThreadStart(AddressOf MostrarFileDialog))
            threadGetFile.SetApartmentState(Threading.ApartmentState.STA)

            threadGetFile.Start()
            While threadGetFile.IsAlive
                Threading.Thread.Sleep(1)
                threadGetFile.Join()
            End While
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            threadGetFile = Nothing
        End Try
    End Sub

    Private Sub IniciarProcesoFolderDialog()
        Dim threadGetFile As Threading.Thread
        Try
            threadGetFile = New Threading.Thread(New Threading.ThreadStart(AddressOf MostrarFolderDialog))
            threadGetFile.SetApartmentState(Threading.ApartmentState.STA)

            threadGetFile.Start()
            While threadGetFile.IsAlive
                Threading.Thread.Sleep(1)
                threadGetFile.Join()
            End While
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            threadGetFile = Nothing
        End Try
    End Sub

    <MTAThread()> _
    Private Sub MostrarFileDialog()
        Dim nw As New NativeWindow
        Dim openFileDialog As OpenFileDialog = New OpenFileDialog()
        Dim result As DialogResult
        Dim strSelectedPath As String = String.Empty
        Try
            nw.AssignHandle(System.Diagnostics.Process.GetProcessesByName("SAP Business One")(DMS_Connector.Company.ApplicationSBO.AppId).MainWindowHandle)

            result = openFileDialog.ShowDialog()

            If result = Windows.Forms.DialogResult.OK Then
                strSelectedPath = openFileDialog.FileName
                If Not String.IsNullOrEmpty(strSelectedPath) Then
                    strRuta = strSelectedPath
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    <MTAThread()> _
    Private Sub MostrarFolderDialog()
        Dim strSelectedPath As String
        Dim objFolderDialog As New Windows.Forms.FolderBrowserDialog
        Dim nw As New NativeWindow
        Try
            nw.AssignHandle(System.Diagnostics.Process.GetProcessesByName("SAP Business One")(DMS_Connector.Company.ApplicationSBO.AppId).MainWindowHandle)

            objFolderDialog.ShowNewFolderButton = False
            objFolderDialog.ShowDialog(nw)
            strSelectedPath = objFolderDialog.SelectedPath
            If Not String.IsNullOrEmpty(strSelectedPath) Then
                strRuta = strSelectedPath
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
#End Region
End Module
