Imports System.Xml
Imports SCG.DMSOne.Framework.MenuManager
Imports System.IO
Imports System.Security.Cryptography
Imports System.Security.Cryptography.Xml
Imports System.Collections.Generic
Imports System.Text

Module AdministradorLicencias

    Private IV As Byte() = Convert.FromBase64String("XUmRrerGCRRMhqyTjlP13w==")
    Private Key As Byte() = Convert.FromBase64String("aJvmRjePI72X1R+6FFCuvuO6M2DQ0d+WbcUjIsjNRH4=")
    Private Licencia As License
    Private LicenciaNueva As License
    Private LicenciasAsignadas As AsignacionLicencias

    Sub New()
        Try
            LicenciasAsignadas = New AsignacionLicencias()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Function EncriptarTexto(ByVal Texto As String, ByVal Key() As Byte, ByVal IV() As Byte) As String
        Dim BytesEncriptados() As Byte
        Dim Encriptador As ICryptoTransform
        Try
            If String.IsNullOrEmpty(Texto) Then
                Return String.Empty
            Else
                Using oAES As Aes = Aes.Create()
                    oAES.Key = Key
                    oAES.IV = IV
                    Encriptador = oAES.CreateEncryptor(oAES.Key, oAES.IV)

                    Using oMemoryStream As New MemoryStream()
                        Using oCryptoStream As New CryptoStream(oMemoryStream, Encriptador, CryptoStreamMode.Write)
                            Using oStreamWriter As New StreamWriter(oCryptoStream)
                                oStreamWriter.Write(Texto)
                            End Using
                            BytesEncriptados = oMemoryStream.ToArray()
                        End Using
                    End Using
                End Using
                Return Convert.ToBase64String(BytesEncriptados)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return String.Empty
        End Try
    End Function

    Private Function DesencriptarTexto(ByVal Texto As String, ByVal Key() As Byte, ByVal IV() As Byte) As String
        Dim TextoDesencriptado As String = String.Empty
        Dim TextoCifrado() As Byte
        Dim Desencriptador As ICryptoTransform
        Try
            If String.IsNullOrEmpty(Texto) Then
                Return String.Empty
            Else
                TextoCifrado = Convert.FromBase64String(Texto)
                Using oAES As Aes = Aes.Create()
                    oAES.Key = Key
                    oAES.IV = IV
                    Desencriptador = oAES.CreateDecryptor(oAES.Key, oAES.IV)

                    Using oMemoryStream As New MemoryStream(TextoCifrado)
                        Using oCryptoStream As New CryptoStream(oMemoryStream, Desencriptador, CryptoStreamMode.Read)
                            Using oStreamReader As New StreamReader(oCryptoStream)
                                TextoDesencriptado = oStreamReader.ReadToEnd()
                            End Using
                        End Using
                    End Using
                End Using

                Return TextoDesencriptado
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return String.Empty
        End Try
    End Function

    Private Function DesencriptarArchivoLicencias(ByVal Path As String) As XmlDocument
        Dim Clave As RijndaelManaged
        Dim Documento As XmlDocument
        Dim Nodes As XmlNodeList
        Dim Element As XmlElement
        Dim oEncryptedData As EncryptedData
        Dim oEncryptedXml As EncryptedXml
        Dim Resultado As Byte()
        Try
            Documento = New XmlDocument()
            Clave = New RijndaelManaged()
            oEncryptedData = New EncryptedData()
            oEncryptedXml = New EncryptedXml()
            Clave.IV = IV
            Clave.Key = Key
            Documento.PreserveWhitespace = True
            Documento.Load(Path)
            Element = CType(Documento.GetElementsByTagName("EncryptedData").Item(0), XmlElement)
            oEncryptedData.LoadXml(Element)
            Resultado = oEncryptedXml.DecryptData(oEncryptedData, Clave)
            oEncryptedXml.ReplaceData(Element, Resultado)
            Return Documento
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return Nothing
        End Try
    End Function

    Public Sub AbrirFormulario()
        Dim PaqueteCreacion As SAPbouiCOM.FormCreationParams
        Dim Documento As XmlDocument
        Dim Path As String = String.Empty
        Dim Formulario As SAPbouiCOM.Form
        Try
            Documento = New XmlDocument()
            Path = String.Format("{0}{1}", Application.StartupPath, My.Resources.Resource.XMLAdministradorLicencias)
            Documento.Load(Path)
            PaqueteCreacion = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            PaqueteCreacion.XmlData = Documento.InnerXml
            Formulario = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(PaqueteCreacion)
            InicializarFormulario(Formulario.UniqueID)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub AgregarMenu()
        Dim strMenuPadre As String = "SCGD_CFG"
        Dim strTitulo As String = My.Resources.Resource.TituloLicenseManager
        Dim strIDMenu As String = "SCGD_OLAD"
        Dim intPosicion As Integer = 18
        Try
            If PermisosValidos() Then
                GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(strIDMenu, SAPbouiCOM.BoMenuType.mt_STRING, strTitulo, intPosicion, False, True, strMenuPadre))
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Valida si el usuario tiene permisos para abrir el formulario
    ''' </summary>
    ''' <returns>True = Tiene autorizaciones. False = No tiene autorizaciones</returns>
    ''' <remarks></remarks>
    Private Function PermisosValidos() As Boolean
        Dim blnPermisoValido As Boolean = False
        Try
            If Not Utilitarios.MostrarMenu("SCGD_OLAD", DMS_Connector.Company.ApplicationSBO.Company.UserName) Then
                blnPermisoValido = True
            End If

            Return blnPermisoValido
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Manejador de eventos ItemEvent
    ''' </summary>
    ''' <param name="FormUID">ID único del formulario</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Public Sub ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.FormTypeEx = "SCGD_OLAD" Then
                Select Case pVal.EventType
                    Case SAPbouiCOM.BoEventTypes.et_FORM_LOAD
                        FormLoad(FormUID, pVal, BubbleEvent)
                    Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                        ItemPressed(FormUID, pVal, BubbleEvent)
                    Case SAPbouiCOM.BoEventTypes.et_CLICK
                        Click(FormUID, pVal, BubbleEvent)
                    Case SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK
                        Click(FormUID, pVal, BubbleEvent)
                End Select
            End If

            If pVal.FormTypeEx = "SCGD_LCOM" Then
                Select Case pVal.EventType
                    Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                        ItemPressedComparacion(FormUID, pVal, BubbleEvent)
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de evento ItemPressed
    ''' </summary>
    ''' <param name="FormUID">ID único del formulario</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub Click(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub



    ''' <summary>
    ''' Manejador de eventos FormLoad
    ''' </summary>
    ''' <param name="FormUID">ID único del formulario</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub FormLoad(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.BeforeAction Then

            Else

            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub InicializarFormulario(ByVal FormUID As String)
        Dim Formulario As SAPbouiCOM.Form
        Dim Query As String = String.Empty
        Dim Matriz As SAPbouiCOM.Matrix
        Dim Tabla As SAPbouiCOM.DataTable
        Dim FechaExpiracion As SAPbouiCOM.EditText
        Try
            LicenciasAsignadas = New AsignacionLicencias()
            Formulario = DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID)
            Tabla = Formulario.DataSources.DataTables.Item("Users")
            Query = "SELECT ""USERID"" AS ""ID"", ""USER_CODE"" AS ""CODE"" FROM ""OUSR"""
            Tabla.ExecuteQuery(Query)
            Matriz = Formulario.Items.Item("Users").Specific
            Matriz.LoadFromDataSource()

            If CargarArchivoLicencias() Then
                CargarTiposLicencia(Formulario)
                ObtenerLicenciasAsignadas()
                Formulario.DataSources.UserDataSources.Item("Date").ValueEx = Licencia.FechaVencimiento.ToString("yyyyMMdd")
                CalcularLicenciasDisponibles(Formulario)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub CalcularLicenciasDisponibles(ByRef Formulario As SAPbouiCOM.Form)
        Dim Tabla As SAPbouiCOM.DataTable
        Dim TipoLicencia As String = String.Empty
        Dim CantidadDisponible As Integer
        Dim CantidadAsignada As Integer
        Dim Matriz As SAPbouiCOM.Matrix
        Try
            Tabla = Formulario.DataSources.DataTables.Item("Selected")
            For i As Integer = 0 To Tabla.Rows.Count - 1
                TipoLicencia = Tabla.GetValue("Code", i)
                For Each Componente As LicenseComponent In Licencia.LicenseComponents
                    If Componente.Tipo = TipoLicencia Then
                        CantidadDisponible = Componente.Cantidad
                        If LicenciasAsignadas.AsignacionPorTipo.ContainsKey(TipoLicencia) Then
                            CantidadAsignada = LicenciasAsignadas.AsignacionPorTipo.Item(TipoLicencia)
                            CantidadDisponible = CantidadDisponible - CantidadAsignada
                        End If
                        If CantidadDisponible < 0 Then
                            CantidadDisponible = 0
                        End If
                        Tabla.SetValue("Quantity", i, CantidadDisponible)
                    End If

                Next
            Next

            Matriz = Formulario.Items.Item("Selected").Specific
            Matriz.LoadFromDataSource()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub



    Private Sub RefrescarResumenAsignacion()
        Try

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub RefrescarTiposLicencia()
        Try

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de evento ItemPressed
    ''' </summary>
    ''' <param name="FormUID">ID único del formulario</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ItemPressedComparacion(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim Formulario As SAPbouiCOM.Form
        Try
            Formulario = DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID)
            If pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case "Import"
                        ValidarReasignacion(FormUID, pVal, BubbleEvent)
                End Select
            Else
                Select Case pVal.ItemUID
                    Case "Import"
                        ProcesarLicencia(FormUIDPadre, DocumentoTemporal)
                        Formulario.Close()
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub ValidarReasignacion(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim Resultado As Integer
        Dim Formulario As SAPbouiCOM.Form
        Try
            If RequiereReasignacion Then
                Formulario = DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUIDPadre)
                Resultado = DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.MsjEliminarAsignacionLicencias, 2, My.Resources.Resource.Si, My.Resources.Resource.No)
                Select Case Resultado
                    Case 1
                        Licencia = LicenciaNueva
                        EliminarAsignacionLicencias(Formulario)
                    Case 2
                        BubbleEvent = False
                        DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ImportacionCancelada, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                End Select
            Else
                Licencia = LicenciaNueva
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            BubbleEvent = False
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de evento ItemPressed
    ''' </summary>
    ''' <param name="FormUID">ID único del formulario</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case "Users"
                        If Not EsFilaValida(FormUID, pVal, BubbleEvent) Then
                            BubbleEvent = False
                        End If
                    Case "Selected"
                        If Not EsFilaValida(FormUID, pVal, BubbleEvent) Then
                            BubbleEvent = False
                        End If
                End Select
            Else
                Select Case pVal.ItemUID
                    Case "Import"
                        ImportarArchivoLicencias(FormUID, pVal, BubbleEvent)
                        CargarMatrizResumenAsignacion(FormUID, pVal, BubbleEvent)
                    Case "OpenFile"
                        AbrirArchivoLicencia(FormUID, pVal, BubbleEvent)
                    Case "Users"
                        ManejadorMatrizUsuarios(FormUID, pVal, BubbleEvent)
                    Case "Selected"
                        ManejadorMatrizAsignacion(FormUID, pVal, BubbleEvent)
                    Case "1"
                        GuardarLicenciasAsignadas()
                    Case "TSummary"
                        CargarMatrizResumenAsignacion(FormUID, pVal, BubbleEvent)
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Function EsFilaValida(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean) As Boolean
        Dim Matriz As SAPbouiCOM.Matrix
        Dim Formulario As SAPbouiCOM.Form
        Try
            EsFilaValida = True

            If pVal.Row <= 0 Then
                Return False
            End If

            Formulario = DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID)
            Matriz = Formulario.Items.Item(pVal.ItemUID).Specific

            If pVal.Row > Matriz.RowCount Then
                Return False
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            EsFilaValida = False
        End Try
    End Function

    Private Sub AbrirArchivoLicencia(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oThread As Threading.Thread
        Try
            oThread = New System.Threading.Thread(AddressOf AbrirBuscadorArchivos)
            oThread.SetApartmentState(Threading.ApartmentState.STA)
            oThread.IsBackground = True
            oThread.Start(FormUID)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub AbrirBuscadorArchivos(ByVal FormUID As Object)
        Dim Formulario As SAPbouiCOM.Form
        Dim Path As String = String.Empty
        Dim OpenFileDialog As OpenFileDialog

        Try
            Formulario = DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID.ToString())
            OpenFileDialog = New OpenFileDialog()
            OpenFileDialog.InitialDirectory = "c:\"
            OpenFileDialog.Filter = "License Files |*.xml; *.lic"
            OpenFileDialog.FilterIndex = 2
            OpenFileDialog.Title = My.Resources.Resource.TituloSeleccionarLicencia
            OpenFileDialog.Multiselect = False
            OpenFileDialog.RestoreDirectory = True

            Using WinForm As System.Windows.Forms.Form = New System.Windows.Forms.Form()
                WinForm.TopLevel = True
                WinForm.TopMost = True
                If (OpenFileDialog.ShowDialog(WinForm) = Windows.Forms.DialogResult.OK) Then
                    Path = OpenFileDialog.FileName
                End If
            End Using

            If Not String.IsNullOrEmpty(Path) Then
                Formulario.DataSources.UserDataSources.Item("Path").ValueEx = Path
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


    Private Sub ObtenerLicenciasAsignadas()
        Dim Query As String = "SELECT T0.""Code"", T0.""U_Data"" FROM ""@SCGD_ULIC"" T0 "
        Dim Texto As String = String.Empty
        Dim oRecordSet As SAPbobsCOM.Recordset
        Dim Codigo As String = String.Empty
        Dim XmlString As String = String.Empty
        Try
            oRecordSet = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery(Query)

            While Not oRecordSet.EoF
                Codigo = oRecordSet.Fields.Item("Code").Value.ToString()
                XmlString += oRecordSet.Fields.Item("U_Data").Value.ToString()
                oRecordSet.MoveNext()
            End While
            XmlString = DesencriptarTexto(XmlString, Key, IV)
            CargarLicenciasUsuario(XmlString)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub CargarMatrizResumenAsignacion(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim Formulario As SAPbouiCOM.Form
        Try
            Formulario = DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID)
            CargarMatrizResumenAsignacion(Formulario)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub CargarMatrizResumenAsignacion(ByRef Formulario As SAPbouiCOM.Form)
        Dim Tabla As SAPbouiCOM.DataTable
        Dim Grid As SAPbouiCOM.Grid
        Dim CodigoUsuario As String = String.Empty
        Dim Query As String = String.Empty
        Dim Format As String = ", '' AS ""{0}"" "
        Dim Columns As String = String.Empty
        Try
            Grid = Formulario.Items.Item("Summary").Specific
            Tabla = Formulario.DataSources.DataTables.Item("Summary")
            Query = "SELECT ""USERID"" AS ""UserID"", ""USER_CODE"" AS ""UserCode"" {0} FROM ""OUSR"""

            For i As Integer = 0 To Licencia.LicenseComponents.Count - 1
                Columns += String.Format(Format, Licencia.LicenseComponents.Item(i).Tipo)
            Next

            Query = String.Format(Query, Columns)

            Tabla.ExecuteQuery(Query)

            If LicenciasAsignadas.AsignacionPorUsuario.Count > 0 Then
                For i As Integer = 0 To Tabla.Rows.Count - 1
                    CodigoUsuario = Tabla.GetValue("UserID", i)
                    For Each Valor As KeyValuePair(Of String, List(Of String)) In LicenciasAsignadas.AsignacionPorUsuario
                        If CodigoUsuario = Valor.Key Then
                            For j As Integer = 0 To Tabla.Columns.Count - 1
                                For Each Tipo As String In Valor.Value
                                    If Tipo = Tabla.Columns.Item(j).Name Then
                                        Tabla.SetValue(Tipo, i, 1)
                                    End If
                                Next
                            Next
                        End If
                    Next
                Next
            Else

            End If
            Grid.DataTable = Tabla

            For i As Integer = 0 To Grid.Columns.Count - 1
                For j As Integer = 0 To Licencia.LicenseComponents.Count - 1
                    If Grid.Columns.Item(i).UniqueID = Licencia.LicenseComponents.Item(j).Tipo Then
                        Grid.Columns.Item(i).TitleObject.Caption = Licencia.LicenseComponents.Item(j).Descripcion
                    End If
                Next
            Next

            Grid.AutoResizeColumns()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub CargarLicenciasUsuario(ByRef XmlString As String)
        Dim Documento As XmlDocument
        Dim NodeList As XmlNodeList
        Dim UserID As String = String.Empty
        Dim Componente As String = String.Empty
        Try
            Documento = New XmlDocument()
            If Not String.IsNullOrEmpty(XmlString) Then
                Documento.LoadXml(XmlString)
                NodeList = Documento.SelectNodes("UserLicenses/UserLicense")
                For Each NodoLicencia As XmlNode In NodeList
                    UserID = NodoLicencia("UserID").InnerText
                    For Each Component As XmlNode In NodoLicencia("LicenseComponents").ChildNodes
                        Componente = Component("IDLicencia").InnerText
                        For Each Value As LicenseComponent In Licencia.LicenseComponents
                            If Value.Tipo = Componente Then
                                LicenciasAsignadas.AsignarLicencia(UserID, Componente)
                            End If
                        Next
                    Next
                Next
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Function LicenciaUsuarioValida(ByVal UserSignature As String, ByVal MenuUID As String) As Boolean
        Dim intCont As Integer = 1
        Dim Tabla As SAPbobsCOM.UserTable
        Dim XmlString As String = String.Empty
        Dim Documento As XmlDocument
        Dim UserID As String = String.Empty
        Dim FechaExpiracion As DateTime
        Dim LicenciasUsuario As XmlNodeList
        Dim Nodo As XmlNode
        Dim Componente As String = String.Empty
        Try
            'Se devuelve true para no validar las licencias, hasta realizar la implementación completa
            'del nuevo modelo de licenciamiento
            Return True

            If MenuUID = "SCGD_OLAD" Or MenuUID = "SCGD_PRM" Or MenuUID = "SCGD_OCDE" Or MenuUID = "SCGD_OTDI" Then
                Return True
            End If

            If Not String.IsNullOrEmpty(MenuUID) AndAlso MenuUID.Contains("SCGD") Then
                Documento = New XmlDocument()
                Tabla = DMS_Connector.Company.CompanySBO.UserTables.Item("SCGD_ULIC")

                While Tabla.GetByKey(intCont.ToString())
                    intCont += 1
                    XmlString = XmlString + Tabla.UserFields.Fields.Item("U_Data").Value
                End While

                If Not String.IsNullOrEmpty(XmlString) Then
                    'If Tabla.GetByKey("1") Then
                    'XmlString = Tabla.UserFields.Fields.Item("U_Data").Value
                    XmlString = AdministradorLicencias.DesencriptarTexto(XmlString, Key, IV)
                    If String.IsNullOrEmpty(XmlString) Then
                        Return False
                    End If

                    Documento.LoadXml(XmlString)

                    Nodo = Documento.SelectSingleNode("UserLicenses/ExpirationDate")
                    FechaExpiracion = DateTime.ParseExact(Nodo.InnerText, "yyyyMMdd", Nothing)
                    If FechaExpiracion < DateTime.Today Then
                        Return False
                    End If

                    LicenciasUsuario = Documento.SelectNodes("UserLicenses/UserLicense")
                    For Each LicenciaUsuario As XmlNode In LicenciasUsuario
                        UserID = LicenciaUsuario("UserID").InnerText
                        If UserID = UserSignature Then
                            For Each Component As XmlNode In LicenciaUsuario("LicenseComponents").ChildNodes
                                'If Not String.IsNullOrEmpty(Component("IDLicencia").InnerText) Then
                                '    Return True
                                'End If

                                'Pendiente activar cuando se hayan definido los formularios para cada tipo de licencia
                                For Each Formulario As XmlNode In Component("Forms").ChildNodes
                                    If MenuUID = Formulario.InnerText Or Formulario.InnerText = "SCGD_ALL" Then
                                        Return True
                                    End If
                                Next
                            Next
                        End If
                    Next
                    Return False
                Else
                    Return False
                End If
                Return False
            End If

            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Private Sub GuardarLicenciasAsignadas()
        Dim Documento As XmlDocument
        Dim DeclaracionXML As XmlNode
        Dim UserLicenses As XmlNode
        Dim UserLicense As XmlNode
        Dim IDLicencia As XmlNode
        Dim LicenseComponents As XmlNode
        Dim Componente As XmlNode
        Dim Node As XmlNode
        Dim Tabla As SAPbobsCOM.UserTable
        Dim TextoEncriptado As String = String.Empty
        Dim intCon As Integer = 1
        Dim intCon2 As Integer = 1
        Dim listTextoEncriptado As List(Of String)
        Dim tamaño As Integer
        Dim tamano2 As Integer
        Dim cadenas1 As String
        Dim cadenas2 As String

        Try
            Tabla = DMS_Connector.Company.CompanySBO.UserTables.Item("SCGD_ULIC")
            LimpiarTabla(Tabla)
            listTextoEncriptado = New List(Of String)
            Documento = New XmlDocument()
            DeclaracionXML = Documento.CreateXmlDeclaration("1.0", "UTF-8", "yes")
            Documento.AppendChild(DeclaracionXML)
            UserLicenses = Documento.CreateElement("UserLicenses")
            Documento.AppendChild(UserLicenses)
            Node = Documento.CreateElement("ExpirationDate")
            Node.InnerText = Licencia.FechaVencimiento.ToString("yyyyMMdd")
            UserLicenses.AppendChild(Node)
            For Each ComponentList As KeyValuePair(Of String, List(Of String)) In LicenciasAsignadas.AsignacionPorUsuario
                UserLicense = Documento.CreateElement("UserLicense")
                UserLicenses.AppendChild(UserLicense)
                Node = Documento.CreateElement("UserID")
                Node.InnerText = ComponentList.Key
                UserLicense.AppendChild(Node)
                LicenseComponents = Documento.CreateElement("LicenseComponents")
                UserLicense.AppendChild(LicenseComponents)
                For Each Component As String In ComponentList.Value
                    Componente = Documento.CreateElement("Component")
                    IDLicencia = Documento.CreateElement("IDLicencia")
                    IDLicencia.InnerText = Component
                    Componente.AppendChild(IDLicencia)
                    AgregarListaFormularios(Documento, Componente, Component)
                    LicenseComponents.AppendChild(Componente)
                Next
            Next

            TextoEncriptado = EncriptarTexto(Documento.OuterXml, Key, IV)
            tamaño = TextoEncriptado.Length
            tamano2 = Integer.Parse(Math.Round(tamaño / 2))

            While Tabla.GetByKey(intCon.ToString())
                intCon += 1
                Tabla.Remove()
                Tabla.Update()
            End While

            cadenas1 = TextoEncriptado.Substring(0, tamano2)
            cadenas2 = TextoEncriptado.Substring(tamano2, tamaño - tamano2)
            listTextoEncriptado.Add(cadenas1)
            listTextoEncriptado.Add(cadenas2)

            For Each element As String In listTextoEncriptado
                If Not String.IsNullOrEmpty(element) Then
                    Tabla.Code = intCon2.ToString()
                    Tabla.Name = intCon2.ToString()
                    Tabla.UserFields.Fields.Item("U_Data").Value = element
                    Tabla.Add()
                    intCon2 += 1
                End If
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub AgregarListaFormularios(ByRef Documento As XmlDocument, ByRef Componente As XmlNode, ByVal IDComponente As String)
        Dim Formulario As XmlNode
        Dim Formularios As XmlNode
        Try
            Formularios = Documento.CreateElement("Forms")
            Componente.AppendChild(Formularios)
            For Each ComponenteLicencia As LicenseComponent In Licencia.LicenseComponents
                If ComponenteLicencia.Tipo = IDComponente Then
                    For Each kvp As KeyValuePair(Of String, String) In ComponenteLicencia.Formularios
                        Formulario = Documento.CreateElement("FormUID")
                        Formulario.InnerText = kvp.Value
                        Formularios.AppendChild(Formulario)
                    Next
                End If
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub LimpiarTabla(ByRef Tabla As SAPbobsCOM.UserTable)
        Dim Query As String = "SELECT T0.""Code"" FROM ""@SCGD_ULIC"" T0"
        Dim oRecordSet As SAPbobsCOM.Recordset
        Dim Codigo As String = String.Empty
        Try
            oRecordSet = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery(Query)
            While Not oRecordSet.EoF
                Codigo = oRecordSet.Fields.Item("Code").Value.ToString()
                Tabla.GetByKey(Codigo)
                Tabla.Remove()
                oRecordSet.MoveNext()
            End While
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub ManejadorMatrizUsuarios(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim CodigoUsuario As String = String.Empty
        Dim Matriz As SAPbouiCOM.Matrix
        Dim Tabla As SAPbouiCOM.DataTable
        Dim Formulario As SAPbouiCOM.Form
        Try
            Formulario = DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID)
            Tabla = Formulario.DataSources.DataTables.Item("Users")
            Matriz = Formulario.Items.Item("Users").Specific
            Matriz.FlushToDataSource()
            If Not Matriz.IsRowSelected(pVal.Row) Then
                Matriz.SelectRow(pVal.Row, True, False)
            End If
            CodigoUsuario = Tabla.GetValue("ID", pVal.Row - 1)
            CargarLicenciasAsignadas(Formulario, CodigoUsuario)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub CargarLicenciasAsignadas(ByRef Formulario As SAPbouiCOM.Form, ByVal CodigoUsuario As String)
        Dim Tabla As SAPbouiCOM.DataTable
        Dim Matriz As SAPbouiCOM.Matrix
        Dim TipoLicencia As String = String.Empty
        Dim CantidadDisponible As Integer = 0
        Dim CantidadAsignada As Integer = 0
        Try
            Matriz = Formulario.Items.Item("Selected").Specific
            Tabla = Formulario.DataSources.DataTables.Item("Selected")
            If Tabla.Rows.Count > 0 Then
                For i As Integer = 0 To Tabla.Rows.Count - 1
                    CantidadDisponible = 0
                    TipoLicencia = Tabla.GetValue("Code", i)
                    For Each Componente As LicenseComponent In Licencia.LicenseComponents
                        If Componente.Tipo = TipoLicencia Then
                            CantidadDisponible = Componente.Cantidad
                        End If
                    Next

                    If LicenciasAsignadas.AsignacionPorTipo.ContainsKey(TipoLicencia) Then
                        CantidadAsignada = LicenciasAsignadas.AsignacionPorTipo.Item(TipoLicencia)
                        CantidadDisponible = CantidadDisponible - CantidadAsignada
                    End If
                    If CantidadDisponible < 0 Then
                        CantidadDisponible = 0
                    End If
                    Tabla.SetValue("Quantity", i, CantidadDisponible)

                    If LicenciasAsignadas.AsignacionPorUsuario.ContainsKey(CodigoUsuario) Then
                        If LicenciasAsignadas.AsignacionPorUsuario.Item(CodigoUsuario).Contains(TipoLicencia) Then
                            Tabla.SetValue("Assigned", i, "Y")
                        Else
                            Tabla.SetValue("Assigned", i, "N")
                        End If
                    Else
                        Tabla.SetValue("Assigned", i, "N")
                    End If
                Next
            End If
            Matriz.LoadFromDataSource()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub ManejadorMatrizAsignacion(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim CodigoUsuario As String = String.Empty
        Dim TipoLicencia As String = String.Empty
        Dim LicenciaAsignada As String = String.Empty
        Dim Matriz As SAPbouiCOM.Matrix
        Dim MatrizUsuarios As SAPbouiCOM.Matrix
        Dim Tabla As SAPbouiCOM.DataTable
        Dim TablaUsuarios As SAPbouiCOM.DataTable
        Dim Formulario As SAPbouiCOM.Form
        Dim LineaUsuario As Integer
        Dim CantidadDisponible As Integer
        Try
            Formulario = DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID)
            Tabla = Formulario.DataSources.DataTables.Item("Selected")
            TablaUsuarios = Formulario.DataSources.DataTables.Item("Users")
            Matriz = Formulario.Items.Item("Selected").Specific
            MatrizUsuarios = Formulario.Items.Item("Users").Specific
            Matriz.FlushToDataSource()
            TipoLicencia = Tabla.GetValue("Code", pVal.Row - 1)
            LicenciaAsignada = Tabla.GetValue("Assigned", pVal.Row - 1)
            CantidadDisponible = Tabla.GetValue("Quantity", pVal.Row - 1)
            LineaUsuario = MatrizUsuarios.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder) - 1
            If LineaUsuario >= 0 Then
                CodigoUsuario = TablaUsuarios.GetValue("ID", LineaUsuario)
                If LicenciaAsignada = "Y" Then
                    If CantidadDisponible = 0 Then
                        Tabla.SetValue("Assigned", pVal.Row - 1, "N")
                    Else
                        LicenciasAsignadas.AsignarLicencia(CodigoUsuario, TipoLicencia)
                    End If
                Else
                    LicenciasAsignadas.RemoverAsignacion(CodigoUsuario, TipoLicencia)
                End If
            End If
            If Not String.IsNullOrEmpty(CodigoUsuario) Then
                CargarLicenciasAsignadas(Formulario, CodigoUsuario)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


    Private Sub ImportarArchivoLicencias(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim Path As String = String.Empty
        Dim Formulario As SAPbouiCOM.Form
        Dim Documento As XmlDocument
        Dim ListaDiferencias As List(Of ComparacionLicencia)
        Try
            ListaDiferencias = New List(Of ComparacionLicencia)
            Formulario = DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID)
            Path = Formulario.DataSources.UserDataSources.Item("Path").ValueEx
            If Not String.IsNullOrEmpty(Path) Then
                Documento = DesencriptarArchivoLicencias(Path)
                If Documento IsNot Nothing Then
                    LicenciaNueva = New License(Documento)
                End If
                If LicenciaNueva.FechaVencimiento >= DateTime.Now() Then
                    If LicenciasCompatibles(Licencia, LicenciaNueva, ListaDiferencias) Then
                        Licencia = LicenciaNueva
                        If Not ListaDiferencias.Count > 0 Then
                            ProcesarLicencia(FormUID, Documento)
                        Else
                            MostrarDiferencias(FormUID, Documento, ListaDiferencias, False)
                        End If
                    Else
                        MostrarDiferencias(FormUID, Documento, ListaDiferencias, True)
                    End If
                Else
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.LicenciaExpirada, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ProcesarLicencia(ByRef FormUID As String, ByRef Documento As XmlDocument)
        Dim Formulario As SAPbouiCOM.Form
        Try
            Formulario = DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID)
            GuardarArchivoLicencias(Documento)
            CargarTiposLicencia(Formulario)
            ObtenerLicenciasAsignadas()
            Formulario.DataSources.UserDataSources.Item("Date").ValueEx = Licencia.FechaVencimiento.ToString("yyyyMMdd")
            CalcularLicenciasDisponibles(Formulario)
            CargarMatrizResumenAsignacion(Formulario)
            Formulario.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    'Private Function AceptarCambioLicencia(ByRef Formulario As SAPbouiCOM.Form, ByRef LicenciaActual As License, ByRef LicenciaNueva As License) As Boolean
    '    Dim Resultado As Integer
    '    Dim ListaDiferencias As List(Of ComparacionLicencia)
    '    Try
    '        ListaDiferencias = New List(Of ComparacionLicencia)
    '        If LicenciaActual IsNot Nothing Then
    '            If Not LicenciasCompatibles(LicenciaActual, LicenciaNueva, ListaDiferencias) Then
    '                MostrarDiferencias(ListaDiferencias)
    '                Resultado = DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.MsjEliminarAsignacionLicencias, 2, My.Resources.Resource.Si, My.Resources.Resource.No)
    '                Select Case Resultado
    '                    Case 1
    '                        LicenciaActual = LicenciaNueva
    '                        EliminarAsignacionLicencias(Formulario)
    '                    Case 2
    '                        DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ImportacionCancelada, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
    '                        Return False
    '                End Select
    '            Else
    '                MostrarDiferencias(FormUID, Documento, ListaDiferencias)
    '                LicenciaActual = LicenciaNueva
    '            End If
    '        Else
    '            LicenciaActual = LicenciaNueva
    '        End If
    '        Return True
    '    Catch ex As Exception
    '        DMS_Connector.Helpers.ManejoErrores(ex)
    '        Return False
    '    End Try
    'End Function

    Private DocumentoTemporal As XmlDocument
    Private FormUIDPadre As String = String.Empty
    Private RequiereReasignacion As Boolean = False

    Private Sub MostrarDiferencias(ByVal FormUID As String, ByRef DocumentoLicencia As XmlDocument, ByRef ListaDiferencias As List(Of ComparacionLicencia), ByVal EliminarAsignacion As Boolean)
        Dim FormularioComparacion As SAPbouiCOM.Form
        Dim PaqueteCreacion As SAPbouiCOM.FormCreationParams
        Dim Documento As XmlDocument
        Dim Path As String = String.Empty
        Dim TablaDiferencias As SAPbouiCOM.DataTable
        Try
            FormUIDPadre = FormUID
            DocumentoTemporal = DocumentoLicencia
            RequiereReasignacion = EliminarAsignacion
            If ListaDiferencias.Count > 0 Then
                Documento = New XmlDocument()
                Path = String.Format("{0}{1}", Application.StartupPath, My.Resources.Resource.XMLComparacionLicencias)
                Documento.Load(Path)
                PaqueteCreacion = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
                PaqueteCreacion.XmlData = Documento.InnerXml
                FormularioComparacion = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(PaqueteCreacion)
                TablaDiferencias = FormularioComparacion.DataSources.DataTables.Item("Dif")
                For Each Diferencia As ComparacionLicencia In ListaDiferencias
                    TablaDiferencias.Rows.Add()
                    TablaDiferencias.SetValue("Type", TablaDiferencias.Rows.Count - 1, Diferencia.Type)
                    TablaDiferencias.SetValue("Dsc", TablaDiferencias.Rows.Count - 1, Diferencia.Description)
                    TablaDiferencias.SetValue("CQty", TablaDiferencias.Rows.Count - 1, Diferencia.CurrentQuantity)
                    TablaDiferencias.SetValue("NQty", TablaDiferencias.Rows.Count - 1, Diferencia.NewQuantity)
                    TablaDiferencias.SetValue("Remarks", TablaDiferencias.Rows.Count - 1, Diferencia.Remarks)
                Next
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Function LicenciasCompatibles(ByRef LicenciaActual As License, ByRef LicenciaNueva As License, ByRef ListaDiferencias As List(Of ComparacionLicencia)) As Boolean
        Dim ExisteLicencia As Boolean = False
        Dim NuevoTipoLicencia As Boolean = True
        Try
            If LicenciaActual IsNot Nothing Then
                ListaDiferencias = New List(Of ComparacionLicencia)
                LicenciasCompatibles = True
                For Each Componente As LicenseComponent In LicenciaActual.LicenseComponents
                    ExisteLicencia = False
                    For Each Valor As LicenseComponent In LicenciaNueva.LicenseComponents
                        If Componente.Tipo = Valor.Tipo Then
                            ExisteLicencia = True
                            If Valor.Cantidad < Componente.Cantidad Then
                                ListaDiferencias.Add(New ComparacionLicencia(Componente.Tipo, Componente.Descripcion, Componente.Cantidad, Valor.Cantidad, My.Resources.Resource.CantidadReducida))
                                LicenciasCompatibles = False
                            End If

                            If Valor.Cantidad > Componente.Cantidad Then
                                ListaDiferencias.Add(New ComparacionLicencia(Componente.Tipo, Componente.Descripcion, Componente.Cantidad, Valor.Cantidad, My.Resources.Resource.CantidadAumentada))
                            End If
                        End If
                    Next
                    If Not ExisteLicencia Then
                        LicenciasCompatibles = False
                        ListaDiferencias.Add(New ComparacionLicencia(Componente.Tipo, Componente.Descripcion, Componente.Cantidad, "0", My.Resources.Resource.LicenciaEliminada))
                    End If
                Next

                For Each Componente As LicenseComponent In LicenciaNueva.LicenseComponents
                    NuevoTipoLicencia = True
                    For Each Valor As LicenseComponent In LicenciaActual.LicenseComponents
                        If Componente.Tipo = Valor.Tipo Then
                            NuevoTipoLicencia = False
                            Exit For
                        End If
                    Next
                    If NuevoTipoLicencia Then
                        ListaDiferencias.Add(New ComparacionLicencia(Componente.Tipo, Componente.Descripcion, "0", Componente.Cantidad, My.Resources.Resource.LicenciaAgregada))
                    End If
                Next
            Else
                LicenciasCompatibles = True
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Private Sub EliminarAsignacionLicencias(ByRef Formulario As SAPbouiCOM.Form)
        Dim LicenciasUsuario As SAPbobsCOM.UserTable
        Dim TablaAsignacion As SAPbouiCOM.DataTable
        Dim MatrizAsignacion As SAPbouiCOM.Matrix
        Try
            TablaAsignacion = Formulario.DataSources.DataTables.Item("Selected")
            LicenciasUsuario = DMS_Connector.Company.CompanySBO.UserTables.Item("SCGD_ULIC")
            LimpiarTabla(LicenciasUsuario)
            LicenciasAsignadas = New AsignacionLicencias()

            For i As Integer = 0 To TablaAsignacion.Rows.Count - 1
                TablaAsignacion.SetValue("Assigned", i, "N")
            Next

            MatrizAsignacion = Formulario.Items.Item("Selected").Specific
            MatrizAsignacion.LoadFromDataSource()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Guardar el archivo completo del XML de licencias en sistema
    ''' </summary>
    ''' <param name="Documento">Archivo de licencia en formato XML</param>
    ''' <remarks></remarks>
    Private Sub GuardarArchivoLicencias(ByRef Documento As XmlDocument)
        Dim TablaUsuario As SAPbobsCOM.UserTable
        Dim XmlEncriptado As String = String.Empty
        Try
            TablaUsuario = DMS_Connector.Company.CompanySBO.UserTables.Item("SCGD_OLIC")
            XmlEncriptado = EncriptarTexto(Documento.OuterXml, Key, IV)
            If TablaUsuario.GetByKey(1) Then
                TablaUsuario.UserFields.Fields.Item("U_File").Value = XmlEncriptado
                'TablaUsuario.UserFields.Fields.Item("U_File").Value = Documento.OuterXml
                TablaUsuario.Update()
            Else
                TablaUsuario.Code = 1
                TablaUsuario.Name = 1
                TablaUsuario.UserFields.Fields.Item("U_File").Value = XmlEncriptado
                'TablaUsuario.UserFields.Fields.Item("U_File").Value = Documento.OuterXml
                TablaUsuario.Add()
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


    Private Function CargarArchivoLicencias() As Boolean
        Dim TablaUsuario As SAPbobsCOM.UserTable
        Dim Documento As XmlDocument
        Dim XmlDesencriptado As String = String.Empty
        Dim DatosEncriptados As String = String.Empty
        Try
            TablaUsuario = DMS_Connector.Company.CompanySBO.UserTables.Item("SCGD_OLIC")
            Documento = New XmlDocument()
            If TablaUsuario.GetByKey(1) Then
                DatosEncriptados = TablaUsuario.UserFields.Fields.Item("U_File").Value
                XmlDesencriptado = DesencriptarTexto(DatosEncriptados, Key, IV)
                If String.IsNullOrEmpty(XmlDesencriptado) Then
                    Return False
                Else
                    'Documento.LoadXml(TablaUsuario.UserFields.Fields.Item("U_File").Value)
                    Documento.LoadXml(XmlDesencriptado)
                    If Documento IsNot Nothing Then
                        Licencia = New License(Documento)
                        Return True
                    End If
                End If
            Else
                'Error no existe un archivo de licencias en sistema
                Return False
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Private Sub CargarTiposLicencia(ByRef Formulario As SAPbouiCOM.Form)
        Dim TablaAsignacion As SAPbouiCOM.DataTable
        Dim Matriz As SAPbouiCOM.Matrix
        Dim TablaTipos As SAPbouiCOM.DataTable
        Try
            TablaAsignacion = Formulario.DataSources.DataTables.Item("Selected")
            TablaAsignacion.Rows.Clear()
            TablaTipos = Formulario.DataSources.DataTables.Item("Types")
            TablaTipos.Rows.Clear()
            For Each Componente As LicenseComponent In Licencia.LicenseComponents
                TablaAsignacion.Rows.Add()
                TablaAsignacion.SetValue("Code", TablaAsignacion.Rows.Count - 1, Componente.Tipo)
                TablaAsignacion.SetValue("Name", TablaAsignacion.Rows.Count - 1, Componente.Descripcion)
                TablaAsignacion.SetValue("Assigned", TablaAsignacion.Rows.Count - 1, "N")
                TablaAsignacion.SetValue("Quantity", TablaAsignacion.Rows.Count - 1, Componente.Cantidad)

                TablaTipos.Rows.Add()
                TablaTipos.SetValue("Code", TablaTipos.Rows.Count - 1, Componente.Tipo)
                TablaTipos.SetValue("Type", TablaTipos.Rows.Count - 1, Componente.Descripcion)
                TablaTipos.SetValue("Quantity", TablaTipos.Rows.Count - 1, Componente.Cantidad)
            Next

            Matriz = Formulario.Items.Item("Selected").Specific
            Matriz.LoadFromDataSource()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para notificar si está pronto a expirar la licencia
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub NotificacionExpiracionLicencia()
        Dim fechaActual As Date
        Dim FechaVencimiento As Date
        Dim ts As TimeSpan
        Try
            If CargarArchivoLicencias() Then
                ObtenerLicenciasAsignadas()
                fechaActual = DateTime.Now()
                FechaVencimiento = Licencia.FechaVencimiento
                ts = FechaVencimiento - fechaActual
                If ts.Days <= 30 Then
                    DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.NoticacionVencimientoLicencia + FechaVencimiento.ToString("dd-MM-yyyy") + My.Resources.Resource.NotificacionVencimientoLicenciaFin, Btn1Caption:="OK")
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

End Module
