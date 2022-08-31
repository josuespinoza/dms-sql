Option Strict On
Option Explicit On

Imports Microsoft.Win32

#Const AUTOMOTRIZ = False
#Const YAMAHA = False
#Const CHILE = False
#Const DITEC = False
#Const SAIS = False
#Const Motores = False

Public Class frmPrincipal
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Private _idioma As String = "es-CR"

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents txtDest As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkDefaultFolder As System.Windows.Forms.CheckBox
    Friend WithEvents chkRestart As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNombreAddon As System.Windows.Forms.TextBox
    Friend WithEvents btnInstalar As System.Windows.Forms.Button
    Friend WithEvents FileWatcher As System.IO.FileSystemWatcher
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPrincipal))
        Me.txtDest = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.chkDefaultFolder = New System.Windows.Forms.CheckBox
        Me.chkRestart = New System.Windows.Forms.CheckBox
        Me.txtNombreAddon = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnInstalar = New System.Windows.Forms.Button
        Me.FileWatcher = New System.IO.FileSystemWatcher
        CType(Me.FileWatcher, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtDest
        '
        Me.txtDest.BackColor = System.Drawing.Color.White
        Me.txtDest.Location = New System.Drawing.Point(8, 40)
        Me.txtDest.Name = "txtDest"
        Me.txtDest.ReadOnly = True
        Me.txtDest.Size = New System.Drawing.Size(472, 20)
        Me.txtDest.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(8, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(256, 23)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Installation path suggested by SBO"
        '
        'chkDefaultFolder
        '
        Me.chkDefaultFolder.Checked = True
        Me.chkDefaultFolder.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDefaultFolder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.chkDefaultFolder.Location = New System.Drawing.Point(8, 128)
        Me.chkDefaultFolder.Name = "chkDefaultFolder"
        Me.chkDefaultFolder.Size = New System.Drawing.Size(264, 24)
        Me.chkDefaultFolder.TabIndex = 2
        Me.chkDefaultFolder.Text = "Use installation path suggested by SBO"
        '
        'chkRestart
        '
        Me.chkRestart.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.chkRestart.Location = New System.Drawing.Point(8, 160)
        Me.chkRestart.Name = "chkRestart"
        Me.chkRestart.Size = New System.Drawing.Size(104, 24)
        Me.chkRestart.TabIndex = 3
        Me.chkRestart.Text = "Restart"
        '
        'txtNombreAddon
        '
        Me.txtNombreAddon.BackColor = System.Drawing.Color.White
        Me.txtNombreAddon.Location = New System.Drawing.Point(8, 96)
        Me.txtNombreAddon.Name = "txtNombreAddon"
        Me.txtNombreAddon.ReadOnly = True
        Me.txtNombreAddon.Size = New System.Drawing.Size(472, 20)
        Me.txtNombreAddon.TabIndex = 1
        Me.txtNombreAddon.Text = "SCG.DMSOne.AddonTaller"
        '
        'Label1
        '
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(8, 72)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(280, 23)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Executable AddOn(.exe) File Name"
        '
        'btnInstalar
        '
        Me.btnInstalar.BackgroundImage = CType(resources.GetObject("btnInstalar.BackgroundImage"), System.Drawing.Image)
        Me.btnInstalar.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!)
        Me.btnInstalar.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.btnInstalar.Location = New System.Drawing.Point(8, 192)
        Me.btnInstalar.Name = "btnInstalar"
        Me.btnInstalar.Size = New System.Drawing.Size(70, 20)
        Me.btnInstalar.TabIndex = 4
        Me.btnInstalar.Text = "&Install"
        '
        'FileWatcher
        '
        Me.FileWatcher.EnableRaisingEvents = True
        Me.FileWatcher.SynchronizingObject = Me
        '
        'frmPrincipal
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(488, 213)
        Me.Controls.Add(Me.btnInstalar)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtNombreAddon)
        Me.Controls.Add(Me.txtDest)
        Me.Controls.Add(Me.chkDefaultFolder)
        Me.Controls.Add(Me.chkRestart)
        Me.Controls.Add(Me.Label2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPrincipal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SCG AddOn Installer"
        CType(Me.FileWatcher, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Declaraciones"

    Private strAddonName As String 'nombre del addon    
    Private strDll As String ' The path of "AddOnInstallAPI.dll"
    Private strDest As String ' Installation target path    

    ' Declaring the functions inside "AddOnInstallAPI.dll"

    'EndInstall - Signals SBO that the installation is complete.
    Declare Function EndInstall Lib "AddOnInstallAPI.dll" () As Int32
    'SetAddOnFolder - Use it if you want to change the installation folder.
    Declare Function SetAddOnFolder Lib "AddOnInstallAPI.dll" (ByVal srrPath As String) As Int32
    'RestartNeeded - Use it if your installation requires a restart, it will cause
    'the SBO application to close itself after the installation is complete.
    Declare Function RestartNeeded Lib "AddOnInstallAPI.dll" () As Int32

#End Region

#Region "Funciones"

    ' Read the addon path from the registry
    Public Function ReadPath() As String

        Dim strAns As String
        Dim strErr As String = ""

        strAns = RegValue(RegistryHive.LocalMachine, "SOFTWARE", strAddonName, strErr)
        If Not (strAns <> "") Then
            MessageBox.Show("This error occurred: " & strErr)
        End If

        Return strAns

    End Function

    ' This Function reads values to the registry
    Public Function RegValue(ByVal Hive As RegistryHive, _
          ByVal strKey As String, ByVal strValueName As String, _
          Optional ByRef strErrInfo As String = "") As String

        Dim objParent As RegistryKey = Nothing
        Dim objSubkey As RegistryKey
        Dim strAns As String = String.Empty
        Select Case Hive
            Case RegistryHive.ClassesRoot
                objParent = Registry.ClassesRoot
            Case RegistryHive.CurrentConfig
                objParent = Registry.CurrentConfig
            Case RegistryHive.CurrentUser
                objParent = Registry.CurrentUser
            Case RegistryHive.DynData
                objParent = Registry.DynData
            Case RegistryHive.LocalMachine
                objParent = Registry.LocalMachine
            Case RegistryHive.PerformanceData
                objParent = Registry.PerformanceData
            Case RegistryHive.Users
                objParent = Registry.Users

        End Select

        objSubkey = objParent.OpenSubKey(strKey)
        'if can't be found, object is not initialized
        If Not objSubkey Is Nothing Then
            strAns = CStr(objSubkey.GetValue(strValueName))
        End If

        Return strAns
    End Function

    ' This Function writes values to the registry
    Public Function WriteToRegistry(ByVal _
    ParentKeyHive As RegistryHive, _
    ByVal strSubKeyName As String, _
    ByVal strValueName As String, _
    ByVal objValue As Object) As Boolean

        Dim objSubKey As RegistryKey
        Dim objParentKey As RegistryKey = Nothing

        Select Case ParentKeyHive
            Case RegistryHive.ClassesRoot
                objParentKey = Registry.ClassesRoot
            Case RegistryHive.CurrentConfig
                objParentKey = Registry.CurrentConfig
            Case RegistryHive.CurrentUser
                objParentKey = Registry.CurrentUser
            Case RegistryHive.DynData
                objParentKey = Registry.DynData
            Case RegistryHive.LocalMachine
                objParentKey = Registry.LocalMachine
            Case RegistryHive.PerformanceData
                objParentKey = Registry.PerformanceData
            Case RegistryHive.Users
                objParentKey = Registry.Users
        End Select

        'Open 
        objSubKey = objParentKey.OpenSubKey(strSubKeyName, True)
        'create if doesn't exist
        If objSubKey Is Nothing Then
            objSubKey = objParentKey.CreateSubKey(strSubKeyName)
        End If


        objSubKey.SetValue(strValueName, objValue)


        Return True

    End Function

    ' This function extracts the given add-on into the path specified
    Private Sub ExtractFile(ByVal strPath As String, ByVal strNombreArchivo As String)
        Dim AddonExeFile As IO.FileStream
        Dim thisExe As System.Reflection.Assembly
        thisExe = System.Reflection.Assembly.GetExecutingAssembly()
        Dim sTargetPath As String = strPath & "\" & strNombreArchivo
        Dim sSourcePath As String = strPath & "\" & strNombreArchivo & ".tmp"

        Dim file As System.IO.Stream


        Try
            file = thisExe.GetManifestResourceStream(thisExe.GetManifestResourceNames(0).Split(CChar("."))(0) & "." & strNombreArchivo)

            ' Create a tmp file first, after file is extracted change to exe
            If IO.File.Exists(sSourcePath) Then
                IO.File.Delete(sSourcePath)
            End If
            AddonExeFile = IO.File.Create(sSourcePath)

            Dim buffer() As Byte
            ReDim buffer(CInt(file.Length))
            file.Read(buffer, 0, CInt(file.Length))
            AddonExeFile.Write(buffer, 0, CInt(file.Length))
            AddonExeFile.Close()

            If IO.File.Exists(sTargetPath) Then
                IO.File.Delete(sTargetPath)
            End If
            ' Change file extension to exe
            IO.File.Move(sSourcePath, sTargetPath)

        Catch ex As Exception
            MsgBox("Falta el archivo " & sSourcePath)
        End Try
    End Sub

    ' This procedure delets the addon files
    Private Sub UnInstall()
        Dim strMensaje As String
        Dim strPath As String

        strPath = ReadPath() ' Reads the addon path from the registry
        If strPath <> "" Then

#If AUTOMOTRIZ = True Then
            strMensaje = Chr(13) & borrarArchivo(strPath, strAddonName & ".Skoda.exe") ' Extract aExtract add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Skoda.exe.config")
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Nasa.exe") ' Extract adct add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Nasa.exe.config")
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Automotriz.exe") ' Extrct add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Automotriz.exe.config")
#ElseIf YAMAHA = True Then
            strMensaje = Chr(13) & borrarArchivo(strPath, strAddonName & ".Yamaha.exe") ' Extract aExtract add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Yamaha.exe.config")
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Europa.exe") ' Extract adct add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Europa.exe.config")

#ElseIf CHILE = True Then
            strMensaje = Chr(13) & borrarArchivo(strPath, strAddonName & ".Portezuelo.exe") ' Extract aExtract add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Portezuelo.exe.config")
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Piramide.exe") ' Extract adct add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Piramide.exe.config")
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Portillo.exe") ' Extract aExtract add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Portillo.exe.config")

#ElseIf DITEC = True Then
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Ditec.exe") ' Extract adct add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Ditec.exe.config")
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".RaulLabbe.exe") ' Extract adct add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".RaulLabbe.exe.config")

#ElseIf SAIS = True Then
            strMensaje = Chr(13) & borrarArchivo(strPath, strAddonName & ".Sais.exe") ' Extract aExtract add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Sais.exe.config")
            strMensaje = Chr(13) & borrarArchivo(strPath, strAddonName & ".Cecor.exe") ' Extract aExtract add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".Cecor.exe.config")

#ElseIf Motores = True Then
             strMensaje = Chr(13) & borrarArchivo(strPath, strAddonName & ".AutoPartes.exe") ' Extract aExtract add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".AutoPartes.exe.config")
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".RoyalMotors.exe") ' Extract adct add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".RoyalMotors.exe.config")
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".SoloCuadras.exe") ' Extract aExtract add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".SoloCuadras.exe.config")
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".CasaMaya.exe") ' Extract aExtract add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".CasaMaya.exe.config")
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".MotoresBritanicos.exe") ' Extract aExtract add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".MotoresBritanicos.exe.config")

#Else
            strMensaje = Chr(13) & borrarArchivo(strPath, strAddonName & ".exe") ' Extract add-on to installation folder
            strMensaje &= Chr(13) & borrarArchivo(strPath, strAddonName & ".exe.config")
#End If
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Imagenes", "Mensajeria.gif")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Imagenes", "S_B_INTE.gif")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Imagenes", "sap_cerrar.gif")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Imagenes", "sbo_cuadro.GIF")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Imagenes", "sbo_humanRe.gif")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Imagenes", "sbo_humanRe.ico")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Imagenes", "sbo_procesos2.gif")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Imagenes", "sbo_produccion.gif")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Imagenes", "sbo_produccion.jpg")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Imagenes", "sbo_reportes.gif")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Imagenes", "sbo_reportes.jpg")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Imagenes", "sbo_seguridad.gif")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Imagenes", "sbo_valoracion.gif")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Imagenes", "sbo_valoracion.ico")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "ControlUDF.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "ControlUDT.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "CreacionUDF.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "DMS_Addon.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "DMSOneFramework.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "Proyecto SCGMSGBox.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "Proyecto SCGToolBar.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "SCG DMS One.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "SCG New Buscador.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "SCG User Interface.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "SCG.License.UX.Windows.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "SCG.Seguridad.2005.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "SCG.UX.Windows.GeneradorConsultas.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "SCG.UX.Windows.ManejoDeArchivosDigitales.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "ManipuladorClienteDLL.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "SCGExceptionHandler.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "SCG.Financiamiento.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "SCG.Placas.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "SCG.Requisiciones.resources.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\en-US", "SCG.ServicioPostVenta.resources.dll")


            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "AgendaCitas.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "AgendaCitas.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CerrarOrdenesTrabajo.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CerrarOrdenesTrabajo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CerrarOrdenesTrabajoInternas.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CerrarOrdenesTrabajoInternas.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfDesglosePago.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfDesglosePago.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfiguracionAccesos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfiguracionAccesos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfiguracionPropsVehiculos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfiguracionPropsVehiculos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfLineasFactura.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfLineasFactura.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfTransaccionesCompraVeh.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfTransaccionesCompraVeh.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ContratosVentaTramite.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ContratosVentaTramite.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ContratoVenta.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ContratoVenta.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "EntradasSinProcesar.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "EntradasSinProcesar.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "FacturaInterna.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "FacturaInterna.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "IngresoContableVehiculo.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "IngresoContableVehiculo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "InventarioVehiculos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "InventarioVehiculos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ListadoContratos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ListadoContratos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "MaestroVehiculos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "MaestroVehiculos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ParamGenAddon.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ParamGenAddon.xml")
            '            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Recepcion.enUS.xml")
            '            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Recepcion.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Recosteos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Recosteos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReportesCosteoVehiculo.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReportesCosteoVehiculo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SalidaContableVehiculo.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SalidaContableVehiculo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "VehiculosSinCostear.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "VehiculosSinCostear.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ContratosReversados.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ContratosReversados.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ListadoContratosAReversar.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ListadoContratosAReversar.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Presupuestos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Presupuestos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SCGD_Solicitudes.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SCGD_Solicitudes.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "TrasladoCostos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "TrasladoCostos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ListaContXUnidad.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ListaContXUnidad.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "PlanPagos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "PlanPagos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfFinanciamiento.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfFinanciamiento.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CuotasVencidas.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CuotasVencidas.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "EstadosCuenta.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "EstadosCuenta.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "HistoricoPagos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "HistoricoPagos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Prestamo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Prestamo.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Saldos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Saldos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "TrasladoCostos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "TrasladoCostos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ExpedientedePlacas.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ExpedientedePlacas.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "IngresoEventosGrupo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "IngresoEventosGrupo.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ContratoTraspasoVehiculo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ContratoTraspasoVehiculo.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "VehiculosTipoEvento.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "VehiculosTipoEvento.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BalanceContratoVentas.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BalanceContratoVentas.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReportesContratoVenta.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReportesContratoVenta.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "GastosAdicionales.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "GastosAdicionales.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Refacturacion.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Refacturacion.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CitasXTipoFecha.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CitasXTipoFecha.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "EntregaVehiculosOT.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "EntregaVehiculosOT.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "VehiculosProblemas.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "VehiculosProblemas.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CosteoMultiplesUnidades.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CosteoMultiplesUnidades.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ComisionPlacas.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ComisionPlacas.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "MensajeriaAprobacion.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "MensajeriaAprobacion.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "UsuariosPorNivel.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "UsuariosPorNivel.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SalidaMultiplesUnidades.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SalidaMultiplesUnidades.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteUnidadesVendidas.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteUnidadesVendidas.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "VendedoresXTipoInv.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "VendedoresXTipoInv.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BusquedaOrdenesTrabajo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BusquedaOrdenesTrabajo.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "VehiculosEspecificacionesPorModelo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "VehiculosEspecificacionesPorModelo.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BalanceOrdenesTrabajo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BalanceOrdenesTrabajo.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "AsociacionArticuloEspecificacion.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "AsociacionArticuloEspecificacion.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "AgendaConfiguracion.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "AgendaConfiguracion.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BusquedaCitas.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BusquedaCitas.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CitasRecepcion.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CitasRecepcion.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "NumeracionSeries.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "NumeracionSeries.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ParametrosDeAplicacion.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ParametrosDeAplicacion.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CargarPanelCitas.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CargarPanelCitas.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionaRepuestosOT.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionaRepuestosOT.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "IncluirRepuestosOT.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "IncluirRepuestosOT.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "AgendaSuspension.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "AgendaSuspension.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "EmbarqueVehiculos.xml")
            'strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "EmbarqueVehiculos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionArticulosVenta.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionArticulosVenta.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionColorVehiculo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionColorVehiculo.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SolicitudOrdenEspecial.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SolicitudOrdenEspecial.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteBodegaProceso.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteBodegaProceso.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteFacturacionVehiculo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteFacturacionVehiculo.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteOrdenesEspeciales.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteOrdenesEspeciales.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BusquedaControlProceso.xml")
            'strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BusquedaControlProceso.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ContratosReversadosReal.xml")
            'strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ContratosReversadosReal.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ControlCrearVisita.xml")
            'strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ControlCrearVisita.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ControlVisita.xml")
            'strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ControlVisita.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "OfertaVentas.xml")
            'strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "OfertaVentas.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Visita.xml")
            'strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Visita.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfiguracionDimensionesContables.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfiguracionDimensionesContables.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CrearDocumentosGastoCostos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CrearDocumentosGastoCostos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "IncluirGastosOT.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "IncluirGastosOT.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionaCostosOT.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionaCostosOT.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CostosDeImportacion.xml")
            'strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CostosDeImportacion.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "EntradaDeVehiculos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "EntradaDeVehiculos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "PedidoDeUnidades.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "PedidoDeUnidades.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "TipoOtInterna.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "TipoOtInterna.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfiguracionDimensionesContablesOT.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfiguracionDimensionesContablesOT.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SolicitaOTEsp.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SolicitaOTEsp.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CosteoDeEntradas.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "CosteoDeEntradas.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "OrdenesDeTrabajoPorEstado.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "OrdenesDeTrabajoPorEstado.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "HistorialVehiculo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "HistorialVehiculo.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "AsignacionMultipleTareas.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "AsignacionMultipleTareas.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "reporteFacturacionFI.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "reporteFacturacionFI.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteSExOT.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteSExOT.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteFacturacionOT.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteFacturacionOT.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "AsignacionMultiple.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "AsignacionMultiple.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BuscadorAdicionales.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BuscadorAdicionales.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "OrdenTrabajo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "OrdenTrabajo.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionListaPrecios.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionListaPrecios.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "DocumentoCompra.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "DocumentoCompra.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BuscadorProveedores.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BuscadorProveedores.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ComentarioHistCV.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ComentarioHistCV.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "OTEspecial.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "OTEspecial.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Mensajeria.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Mensajeria.enUs.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "RazonSuspension.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "RazonSuspension.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionListaEmpleados.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionListaEmpleados.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "DevolucionDeVehiculos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "DevolucionDeVehiculos.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionUnidadesDevolucion.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionUnidadesDevolucion.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "KardexInventarioVehiculos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "KardexInventarioVehiculos.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BuscardorCitasArt.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BuscardorCitasArt.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BuscardorCitasArt.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "BuscardorCitasArt.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionLineasPedido.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionLineasPedido.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionLineasRecepcion.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionLineasRecepcion.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfInterfaceFord.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ConfInterfaceFord.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionMarcaEstiloModelo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionMarcaEstiloModelo.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "TrackingRep.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "TrackingRep.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "PagosPrestamo.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "PagosPrestamo.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionListaUbicaciones.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SeleccionListaUbicaciones.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ListadoRequisiciones.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ListadoRequisiciones.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "FinalizaActividades.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "FinalizaActividades.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ListadoContratosSeguroPostVenta.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ListadoContratosSeguroPostVenta.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SolicitudEspecificos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "SolicitudEspecificos.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ListadoSolicitudEspecificos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ListadoSolicitudEspecificos.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "FacturacionMecanicos.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "FacturacionMecanicos.enUS.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "AvaluoUsados.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "AvaluoUsados.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Configuracion_ADX_IC.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Configuracion_ADX_IC.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Configuracion_TSD_IC.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "Configuracion_TSD_IC.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteFinanciamientoCV.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteFinanciamientoCV.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "TrackingSolEspecific.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "TrackingSolEspecific.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteAntiguedadVehiculos.enUS.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath & "\Formularios", "ReporteAntiguedadVehiculos.xml")

            strMensaje &= Chr(13) & borrarArchivo(strPath, "CornerImage.png")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "archivos.bmp")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "CFL.bmp")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "cont.bmp")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "DMSOne.bmp")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "etiqueta.bmp")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "Flecha.bmp")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "sbo.bmp")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "setup.bmp")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "financ.bmp")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "placas.bmp")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "InfDMS.bmp")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "imgBack.bmp")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "Imagenes.txt")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "ConfiguracionSBO_SoporteCritico.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "citas.bmp")

            strMensaje &= Chr(13) & borrarArchivo(strPath, "GeneradorHash.exe")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG DMS One.exe")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG DMS One.exe.config")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG Visualizador de Reportes.exe")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "AccesoDatos.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "AccesoDatosUDF.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "AccesoDatosUDT.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "ComponenteCristalReport.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "ControlUDF.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "ControlUDT.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "CreacionUDF.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "DataSets.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "DataSetsUDF.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "DataSetsUDT.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "DeKlaritLibrary.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "DMSONEDKFrameworkBusinessFramework.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "DMSOneFramework.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "DMS_Addon.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "DMS_Connector.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "Interop.Outlook.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "Interop.SAPbobsCOM.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "Interop.SAPbouiCOM.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "Microsoft.SqlServer.BatchParser.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "Microsoft.SqlServer.Replication.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "NEWTEXTBOX.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "PDSACryptography.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "Proyecto SCGMSGBox.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "Proyecto SCGToolBar.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "RegionMasterControls.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG ComponenteImagenes.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG New Buscador.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG Produccion Controls.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG User Interface.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.AccesoDatos.Conexion.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.Cifrado.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.Controls.Windows.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.GenLic.Seguridad.Xml.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.License.UX.Windows.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.Seguridad.2005.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.ServidorLic.LogicaNegocios.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.UX.Windows.CitasAutomaticas.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.UX.Windows.GeneradorConsultas.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.UX.Windows.ManejoDeArchivosDigitales.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.UX.Windows.SAP.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.UX.Windows.TextBox.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCGComboBox.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "ManipuladorClienteDLL.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCGExceptionHandler.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "XML_Direccion_WebService.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "IrisSkin2.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "sbo8.8.ssk")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "sbo2007.ssk")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "Skins.xml")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.SkinManager.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.Requisiciones.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.SBOFramework.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.DMSOne.Framework.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.Financiamiento.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.Placas.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.WinFormsSAP.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "smagentapi.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "smcommonutil.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "smerrlog.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "Microsoft.Practices.EnterpriseLibrary.Common.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "Microsoft.Practices.EnterpriseLibrary.Data.dll")
            strMensaje &= Chr(13) & borrarArchivo(strPath, "SCG.ServicioPostVenta.dll")

            'MessageBox.Show(strMensaje, "SCG AddOn", MessageBoxButtons.OK, MessageBoxIcon.Information)
            MessageBox.Show("AddOn desinstalado correctamente", "SCG AddOn", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Path not found")
        End If
        ' Terminate the application
        GC.Collect()
    End Sub

    Private Function borrarArchivo(ByVal strPath As String, ByVal strNombreArchivo As String) As String
        Dim strResult As String = ""

        Try

            If IO.File.Exists(strPath & "\" & strNombreArchivo) Then
                IO.File.Delete(strPath & "\" & strNombreArchivo)
                strResult = strPath & "\" & strNombreArchivo & " was deleted"
            Else
                strResult = strPath & "\" & strNombreArchivo & " was not found"
            End If

            Return strResult

        Catch ex As Exception
            strResult = ex.Message
            Return strResult
        End Try

    End Function

    ' This procedure copies the addon exe file to the installation folder        
    Private Sub Install()
        Dim blnAns As Boolean

        Environment.CurrentDirectory = strDll ' For Dll function calls will work

        If chkDefaultFolder.Checked = False Then ' Change the installation folder
            SetAddOnFolder(txtDest.Text)
            strDest = txtDest.Text
        End If

        If Not (IO.Directory.Exists(strDest)) Then
            IO.Directory.CreateDirectory(strDest) ' Create installation folder
            '            IO.Directory.CreateDirectory(strDest & "\es-CR") 'Crea carpeta para el idioma
            '            IO.Directory.CreateDirectory(strDest & "\es-CR") 'Crea carpeta para el idioma
            IO.Directory.CreateDirectory(strDest & "\en-US") 'Crea carpeta para el idioma
            IO.Directory.CreateDirectory(strDest & "\Imagenes")
            IO.Directory.CreateDirectory(strDest & "\Formularios")
        End If

        FileWatcher.Path = strDest
        FileWatcher.EnableRaisingEvents = True

#If AUTOMOTRIZ = True Then
        ExtractFile(strDest, strAddonName & ".Skoda.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".Skoda.exe.config")
        ExtractFile(strDest, strAddonName & ".Nasa.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".Nasa.exe.config")
        ExtractFile(strDest, strAddonName & ".Naranjo.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".Naranjo.exe.config")
        ExtractFile(strDest, strAddonName & ".Automotriz.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".Automotriz.exe.config")
#ElseIf YAMAHA = True Then
        ExtractFile(strDest, strAddonName & ".Yamaha.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".Yamaha.exe.config")
        ExtractFile(strDest, strAddonName & ".Europa.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".Europa.exe.config")

#ElseIf CHILE = True Then
        ExtractFile(strDest, strAddonName & ".Portezuelo.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".Portezuelo.exe.config")
        ExtractFile(strDest, strAddonName & ".Piramide.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".Piramide.exe.config")
        ExtractFile(strDest, strAddonName & ".Portillo.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".Portillo.exe.config")

#ElseIf DITEC = True Then
        ExtractFile(strDest, strAddonName & ".Ditec.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".Ditec.exe.config")
        ExtractFile(strDest, strAddonName & ".RaulLabbe.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".RaulLabbe.exe.config")

#ElseIf SAIS = True Then
        ExtractFile(strDest, strAddonName & ".Sais.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".Sais.exe.config")
        ExtractFile(strDest, strAddonName & ".Cecor.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".Cecor.exe.config")

#ElseIf Motores = True Then
        ExtractFile(strDest, strAddonName & ".AutoPartes.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".AutoPartes.exe.config")
        ExtractFile(strDest, strAddonName & ".RoyalMotors.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".RoyalMotors.exe.config")
        ExtractFile(strDest, strAddonName & ".SoloCuadras.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".SoloCuadras.exe.config")
        ExtractFile(strDest, strAddonName & ".CasaMaya.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".CasaMaya.exe.config")
        ExtractFile(strDest, strAddonName & ".MotoresBritanicos.exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".MotoresBritanicos.exe.config")
#Else
        ExtractFile(strDest, strAddonName & ".exe") ' Extract add-on to installation folder
        ExtractFile(strDest, strAddonName & ".exe.config")
#End If


        ExtractFile(strDest & "\Imagenes", "Mensajeria.gif")
        ExtractFile(strDest & "\Imagenes", "S_B_INTE.gif")
        ExtractFile(strDest & "\Imagenes", "sap_cerrar.gif")
        ExtractFile(strDest & "\Imagenes", "sbo_cuadro.GIF")
        ExtractFile(strDest & "\Imagenes", "sbo_humanRe.gif")
        ExtractFile(strDest & "\Imagenes", "sbo_humanRe.ico")
        ExtractFile(strDest & "\Imagenes", "sbo_procesos2.gif")
        ExtractFile(strDest & "\Imagenes", "sbo_produccion.gif")
        ExtractFile(strDest & "\Imagenes", "sbo_produccion.jpg")
        ExtractFile(strDest & "\Imagenes", "sbo_reportes.gif")
        ExtractFile(strDest & "\Imagenes", "sbo_reportes.jpg")
        ExtractFile(strDest & "\Imagenes", "sbo_seguridad.gif")
        ExtractFile(strDest & "\Imagenes", "sbo_valoracion.gif")
        ExtractFile(strDest & "\Imagenes", "sbo_valoracion.ico")

        ExtractFile(strDest & "\en-US", "ControlUDF.resources.dll")
        ExtractFile(strDest & "\en-US", "ControlUDT.resources.dll")
        ExtractFile(strDest & "\en-US", "CreacionUDF.resources.dll")
        ExtractFile(strDest & "\en-US", "DMS_Addon.resources.dll")
        ExtractFile(strDest & "\en-US", "DMSOneFramework.resources.dll")
        ExtractFile(strDest & "\en-US", "Proyecto SCGMSGBox.resources.dll")
        ExtractFile(strDest & "\en-US", "Proyecto SCGToolBar.resources.dll")
        ExtractFile(strDest & "\en-US", "SCG DMS One.resources.dll")
        ExtractFile(strDest & "\en-US", "SCG New Buscador.resources.dll")
        ExtractFile(strDest & "\en-US", "SCG User Interface.resources.dll")
        ExtractFile(strDest & "\en-US", "SCG.License.UX.Windows.resources.dll")
        ExtractFile(strDest & "\en-US", "SCG.Seguridad.2005.resources.dll")
        ExtractFile(strDest & "\en-US", "SCG.UX.Windows.GeneradorConsultas.resources.dll")
        ExtractFile(strDest & "\en-US", "SCG.UX.Windows.ManejoDeArchivosDigitales.resources.dll")
        ExtractFile(strDest & "\en-US", "ManipuladorClienteDLL.resources.dll")
        ExtractFile(strDest & "\en-US", "SCGExceptionHandler.resources.dll")
        ExtractFile(strDest & "\en-US", "SCG.Financiamiento.resources.dll")
        ExtractFile(strDest & "\en-US", "SCG.Placas.resources.dll")
        ExtractFile(strDest & "\en-US", "SCG.Requisiciones.resources.dll")
        ExtractFile(strDest & "\en-US", "SCG.ServicioPostVenta.resources.dll")

        ExtractFile(strDest & "\Formularios", "AgendaCitas.enUS.xml")
        ExtractFile(strDest & "\Formularios", "AgendaCitas.xml")
        ExtractFile(strDest & "\Formularios", "CerrarOrdenesTrabajo.enUS.xml")
        ExtractFile(strDest & "\Formularios", "CerrarOrdenesTrabajo.xml")
        ExtractFile(strDest & "\Formularios", "CerrarOrdenesTrabajoInternas.enUS.xml")
        ExtractFile(strDest & "\Formularios", "CerrarOrdenesTrabajoInternas.xml")
        ExtractFile(strDest & "\Formularios", "ConfDesglosePago.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ConfDesglosePago.xml")
        ExtractFile(strDest & "\Formularios", "ConfiguracionAccesos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ConfiguracionAccesos.xml")
        ExtractFile(strDest & "\Formularios", "ConfiguracionPropsVehiculos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ConfiguracionPropsVehiculos.xml")
        ExtractFile(strDest & "\Formularios", "ConfLineasFactura.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ConfLineasFactura.xml")
        ExtractFile(strDest & "\Formularios", "ConfTransaccionesCompraVeh.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ConfTransaccionesCompraVeh.xml")
        ExtractFile(strDest & "\Formularios", "ContratosVentaTramite.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ContratosVentaTramite.xml")
        ExtractFile(strDest & "\Formularios", "ContratoVenta.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ContratoVenta.xml")
        ExtractFile(strDest & "\Formularios", "EntradasSinProcesar.enUS.xml")
        ExtractFile(strDest & "\Formularios", "EntradasSinProcesar.xml")
        ExtractFile(strDest & "\Formularios", "FacturaInterna.enUS.xml")
        ExtractFile(strDest & "\Formularios", "FacturaInterna.xml")
        ExtractFile(strDest & "\Formularios", "IngresoContableVehiculo.enUS.xml")
        ExtractFile(strDest & "\Formularios", "IngresoContableVehiculo.xml")
        ExtractFile(strDest & "\Formularios", "InventarioVehiculos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "InventarioVehiculos.xml")
        ExtractFile(strDest & "\Formularios", "ListadoContratos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ListadoContratos.xml")
        ExtractFile(strDest & "\Formularios", "MaestroVehiculos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "MaestroVehiculos.xml")
        ExtractFile(strDest & "\Formularios", "ParamGenAddon.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ParamGenAddon.xml")
        ExtractFile(strDest & "\Formularios", "Recosteos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "Recosteos.xml")
        ExtractFile(strDest & "\Formularios", "ReportesCosteoVehiculo.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ReportesCosteoVehiculo.xml")
        ExtractFile(strDest & "\Formularios", "SalidaContableVehiculo.enUS.xml")
        ExtractFile(strDest & "\Formularios", "SalidaContableVehiculo.xml")
        ExtractFile(strDest & "\Formularios", "VehiculosSinCostear.enUS.xml")
        ExtractFile(strDest & "\Formularios", "VehiculosSinCostear.xml")
        ExtractFile(strDest & "\Formularios", "ContratosReversados.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ContratosReversados.xml")
        ExtractFile(strDest & "\Formularios", "ListadoContratosAReversar.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ListadoContratosAReversar.xml")
        ExtractFile(strDest & "\Formularios", "Presupuestos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "Presupuestos.xml")
        ExtractFile(strDest & "\Formularios", "SCGD_Solicitudes.xml")
        ExtractFile(strDest & "\Formularios", "SCGD_Solicitudes.enUS.xml")
        ExtractFile(strDest & "\Formularios", "TrasladoCostos.xml")
        ExtractFile(strDest & "\Formularios", "TrasladoCostos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ListaContXUnidad.xml")
        ExtractFile(strDest & "\Formularios", "ListaContXUnidad.enUS.xml")
        ExtractFile(strDest & "\Formularios", "PlanPagos.xml")
        ExtractFile(strDest & "\Formularios", "PlanPagos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ConfFinanciamiento.xml")
        ExtractFile(strDest & "\Formularios", "ConfFinanciamiento.enUS.xml")
        ExtractFile(strDest & "\Formularios", "CuotasVencidas.xml")
        ExtractFile(strDest & "\Formularios", "CuotasVencidas.enUS.xml")
        ExtractFile(strDest & "\Formularios", "EstadosCuenta.xml")
        ExtractFile(strDest & "\Formularios", "EstadosCuenta.enUS.xml")
        ExtractFile(strDest & "\Formularios", "HistoricoPagos.xml")
        ExtractFile(strDest & "\Formularios", "HistoricoPagos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "Prestamo.xml")
        ExtractFile(strDest & "\Formularios", "Prestamo.enUS.xml")
        ExtractFile(strDest & "\Formularios", "Saldos.xml")
        ExtractFile(strDest & "\Formularios", "Saldos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "TrasladoCostos.xml")
        ExtractFile(strDest & "\Formularios", "TrasladoCostos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ExpedientedePlacas.xml")
        ExtractFile(strDest & "\Formularios", "ExpedientedePlacas.enUS.xml")
        ExtractFile(strDest & "\Formularios", "IngresoEventosGrupo.xml")
        ExtractFile(strDest & "\Formularios", "IngresoEventosGrupo.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ContratoTraspasoVehiculo.xml")
        ExtractFile(strDest & "\Formularios", "ContratoTraspasoVehiculo.enUS.xml")
        ExtractFile(strDest & "\Formularios", "VehiculosTipoEvento.xml")
        ExtractFile(strDest & "\Formularios", "VehiculosTipoEvento.enUS.xml")
        ExtractFile(strDest & "\Formularios", "BalanceContratoVentas.xml")
        ExtractFile(strDest & "\Formularios", "BalanceContratoVentas.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ReportesContratoVenta.xml")
        ExtractFile(strDest & "\Formularios", "ReportesContratoVenta.enUS.xml")
        ExtractFile(strDest & "\Formularios", "GastosAdicionales.xml")
        ExtractFile(strDest & "\Formularios", "GastosAdicionales.enUS.xml")
        ExtractFile(strDest & "\Formularios", "Refacturacion.xml")
        ExtractFile(strDest & "\Formularios", "Refacturacion.enUS.xml")
        ExtractFile(strDest & "\Formularios", "CitasXTipoFecha.xml")
        ExtractFile(strDest & "\Formularios", "CitasXTipoFecha.enUS.xml")
        ExtractFile(strDest & "\Formularios", "EntregaVehiculosOT.xml")
        ExtractFile(strDest & "\Formularios", "EntregaVehiculosOT.enUS.xml")
        ExtractFile(strDest & "\Formularios", "VehiculosProblemas.xml")
        ExtractFile(strDest & "\Formularios", "VehiculosProblemas.enUS.xml")
        ExtractFile(strDest & "\Formularios", "CosteoMultiplesUnidades.xml")
        ExtractFile(strDest & "\Formularios", "CosteoMultiplesUnidades.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ComisionPlacas.xml")
        ExtractFile(strDest & "\Formularios", "ComisionPlacas.enUS.xml")
        ExtractFile(strDest & "\Formularios", "MensajeriaAprobacion.xml")
        ExtractFile(strDest & "\Formularios", "MensajeriaAprobacion.enUS.xml")
        ExtractFile(strDest & "\Formularios", "UsuariosPorNivel.xml")
        ExtractFile(strDest & "\Formularios", "UsuariosPorNivel.enUS.xml")
        ExtractFile(strDest & "\Formularios", "SalidaMultiplesUnidades.xml")
        ExtractFile(strDest & "\Formularios", "SalidaMultiplesUnidades.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ReporteUnidadesVendidas.xml")
        ExtractFile(strDest & "\Formularios", "ReporteUnidadesVendidas.enUS.xml")
        ExtractFile(strDest & "\Formularios", "VendedoresXTipoInv.xml")
        ExtractFile(strDest & "\Formularios", "VendedoresXTipoInv.enUS.xml")
        ExtractFile(strDest & "\Formularios", "BusquedaOrdenesTrabajo.xml")
        ExtractFile(strDest & "\Formularios", "BusquedaOrdenesTrabajo.enUS.xml")
        ExtractFile(strDest & "\Formularios", "VehiculosEspecificacionesPorModelo.xml")
        ExtractFile(strDest & "\Formularios", "VehiculosEspecificacionesPorModelo.enUS.xml")
        ExtractFile(strDest & "\Formularios", "BalanceOrdenesTrabajo.xml")
        ExtractFile(strDest & "\Formularios", "BalanceOrdenesTrabajo.enUS.xml")
        ExtractFile(strDest & "\Formularios", "AsociacionArticuloEspecificacion.xml")
        ExtractFile(strDest & "\Formularios", "AsociacionArticuloEspecificacion.enUS.xml")
        ExtractFile(strDest & "\Formularios", "AgendaConfiguracion.xml")
        ExtractFile(strDest & "\Formularios", "AgendaConfiguracion.enUS.xml")
        ExtractFile(strDest & "\Formularios", "BusquedaCitas.xml")
        ExtractFile(strDest & "\Formularios", "BusquedaCitas.enUS.xml")
        ExtractFile(strDest & "\Formularios", "CitasRecepcion.xml")
        ExtractFile(strDest & "\Formularios", "CitasRecepcion.enUS.xml")
        ExtractFile(strDest & "\Formularios", "NumeracionSeries.xml")
        ExtractFile(strDest & "\Formularios", "NumeracionSeries.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ParametrosDeAplicacion.xml")
        ExtractFile(strDest & "\Formularios", "ParametrosDeAplicacion.enUS.xml")
        ExtractFile(strDest & "\Formularios", "CargarPanelCitas.xml")
        ExtractFile(strDest & "\Formularios", "CargarPanelCitas.enUS.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionaRepuestosOT.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionaRepuestosOT.enUS.xml")
        ExtractFile(strDest & "\Formularios", "IncluirRepuestosOT.xml")
        ExtractFile(strDest & "\Formularios", "IncluirRepuestosOT.enUS.xml")
        ExtractFile(strDest & "\Formularios", "AgendaSuspension.xml")
        ExtractFile(strDest & "\Formularios", "AgendaSuspension.enUS.xml")
        ExtractFile(strDest & "\Formularios", "EmbarqueVehiculos.xml")
        'ExtractFile(strDest & "\Formularios", "EmbarqueVehiculos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionArticulosVenta.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionArticulosVenta.enUS.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionColorVehiculo.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionColorVehiculo.enUS.xml")
        ExtractFile(strDest & "\Formularios", "SolicitudOrdenEspecial.xml")
        ExtractFile(strDest & "\Formularios", "SolicitudOrdenEspecial.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ReporteBodegaProceso.xml")
        ExtractFile(strDest & "\Formularios", "ReporteBodegaProceso.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ReporteFacturacionVehiculo.xml")
        ExtractFile(strDest & "\Formularios", "ReporteFacturacionVehiculo.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ReporteOrdenesEspeciales.xml")
        ExtractFile(strDest & "\Formularios", "ReporteOrdenesEspeciales.enUS.xml")
        ExtractFile(strDest & "\Formularios", "BusquedaControlProceso.xml")
        'ExtractFile(strDest & "\Formularios", "BusquedaControlProceso.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ContratosReversadosReal.xml")
        'ExtractFile(strDest & "\Formularios", "ContratosReversadosReal.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ControlCrearVisita.xml")
        'ExtractFile(strDest & "\Formularios", "ControlCrearVisita.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ControlVisita.xml")
        'ExtractFile(strDest & "\Formularios", "ControlVisita.enUS.xml")
        ExtractFile(strDest & "\Formularios", "OfertaVentas.xml")
        'ExtractFile(strDest & "\Formularios", "OfertaVentas.enUS.xml")
        ExtractFile(strDest & "\Formularios", "Visita.xml")
        'ExtractFile(strDest & "\Formularios", "Visita.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ConfiguracionDimensionesContables.xml")
        ExtractFile(strDest & "\Formularios", "ConfiguracionDimensionesContables.enUS.xml")
        ExtractFile(strDest & "\Formularios", "CrearDocumentosGastoCostos.xml")
        ExtractFile(strDest & "\Formularios", "CrearDocumentosGastoCostos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "IncluirGastosOT.xml")
        ExtractFile(strDest & "\Formularios", "IncluirGastosOT.enUS.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionaCostosOT.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionaCostosOT.enUS.xml")
        ExtractFile(strDest & "\Formularios", "CostosDeImportacion.xml")
        'ExtractFile(strDest & "\Formularios", "CostosDeImportacion.enUS.xml")
        ExtractFile(strDest & "\Formularios", "EntradaDeVehiculos.xml")
        ExtractFile(strDest & "\Formularios", "EntradaDeVehiculos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "PedidoDeUnidades.xml")
        ExtractFile(strDest & "\Formularios", "PedidoDeUnidades.enUS.xml")
        ExtractFile(strDest & "\Formularios", "TipoOtInterna.xml")
        ExtractFile(strDest & "\Formularios", "TipoOtInterna.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ConfiguracionDimensionesContablesOT.xml")
        ExtractFile(strDest & "\Formularios", "ConfiguracionDimensionesContablesOT.enUS.xml")
        ExtractFile(strDest & "\Formularios", "SolicitaOTEsp.xml")
        ExtractFile(strDest & "\Formularios", "SolicitaOTEsp.enUS.xml")
        ExtractFile(strDest & "\Formularios", "CosteoDeEntradas.xml")
        ExtractFile(strDest & "\Formularios", "CosteoDeEntradas.enUS.xml")
        ExtractFile(strDest & "\Formularios", "OrdenesDeTrabajoPorEstado.xml")
        ExtractFile(strDest & "\Formularios", "OrdenesDeTrabajoPorEstado.enUS.xml")
        ExtractFile(strDest & "\Formularios", "HistorialVehiculo.xml")
        ExtractFile(strDest & "\Formularios", "HistorialVehiculo.enUS.xml")
        ExtractFile(strDest & "\Formularios", "AsignacionMultipleTareas.xml")
        ExtractFile(strDest & "\Formularios", "AsignacionMultipleTareas.enUS.xml")
        ExtractFile(strDest & "\Formularios", "reporteFacturacionFI.xml")
        ExtractFile(strDest & "\Formularios", "reporteFacturacionFI.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ReporteSExOT.xml")
        ExtractFile(strDest & "\Formularios", "ReporteSExOT.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ReporteFacturacionOT.xml")
        ExtractFile(strDest & "\Formularios", "ReporteFacturacionOT.enUS.xml")
        ExtractFile(strDest & "\Formularios", "AsignacionMultiple.xml")
        ExtractFile(strDest & "\Formularios", "AsignacionMultiple.enUS.xml")
        ExtractFile(strDest & "\Formularios", "BuscadorAdicionales.xml")
        ExtractFile(strDest & "\Formularios", "BuscadorAdicionales.enUS.xml")
        ExtractFile(strDest & "\Formularios", "OrdenTrabajo.xml")
        ExtractFile(strDest & "\Formularios", "OrdenTrabajo.enUS.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionListaPrecios.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionListaPrecios.enUS.xml")
        ExtractFile(strDest & "\Formularios", "DocumentoCompra.xml")
        ExtractFile(strDest & "\Formularios", "DocumentoCompra.enUS.xml")
        ExtractFile(strDest & "\Formularios", "BuscadorProveedores.xml")
        ExtractFile(strDest & "\Formularios", "BuscadorProveedores.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ComentarioHistCV.xml")
        ExtractFile(strDest & "\Formularios", "ContratoVenta.enUS.xml")
        ExtractFile(strDest & "\Formularios", "OTEspecial.xml")
        ExtractFile(strDest & "\Formularios", "OTEspecial.enUS.xml")

        ExtractFile(strDest & "\Formularios", "Mensajeria.xml")
        ExtractFile(strDest & "\Formularios", "Mensajeria.enUs.xml")
        ExtractFile(strDest & "\Formularios", "RazonSuspension.xml")
        ExtractFile(strDest & "\Formularios", "RazonSuspension.enUS.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionListaEmpleados.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionListaEmpleados.enUS.xml")

        ExtractFile(strDest & "\Formularios", "DevolucionDeVehiculos.xml")
        ExtractFile(strDest & "\Formularios", "DevolucionDeVehiculos.enUS.xml")

        ExtractFile(strDest & "\Formularios", "SeleccionUnidadesDevolucion.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionUnidadesDevolucion.enUS.xml")

        ExtractFile(strDest & "\Formularios", "KardexInventarioVehiculos.xml")
        ExtractFile(strDest & "\Formularios", "KardexInventarioVehiculos.enUS.xml")

        ExtractFile(strDest & "\Formularios", "BuscardorCitasArt.xml")
        ExtractFile(strDest & "\Formularios", "BuscardorCitasArt.enUS.xml")

        ExtractFile(strDest & "\Formularios", "SeleccionLineasPedido.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionLineasPedido.enUS.xml")

        ExtractFile(strDest & "\Formularios", "SeleccionLineasRecepcion.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionLineasRecepcion.enUS.xml")

        ExtractFile(strDest & "\Formularios", "ConfInterfaceFord.xml")
        ExtractFile(strDest & "\Formularios", "ConfInterfaceFord.enUS.xml")

        ExtractFile(strDest & "\Formularios", "SeleccionMarcaEstiloModelo.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionMarcaEstiloModelo.enUS.xml")

        ExtractFile(strDest & "\Formularios", "TrackingRep.xml")
        ExtractFile(strDest & "\Formularios", "TrackingRep.enUS.xml")

        ExtractFile(strDest & "\Formularios", "PagosPrestamo.xml")
        ExtractFile(strDest & "\Formularios", "PagosPrestamo.enUS.xml")

        ExtractFile(strDest & "\Formularios", "SeleccionListaUbicaciones.xml")
        ExtractFile(strDest & "\Formularios", "SeleccionListaUbicaciones.enUS.xml")

        ExtractFile(strDest & "\Formularios", "ListadoRequisiciones.xml")
        ExtractFile(strDest & "\Formularios", "ListadoRequisiciones.enUS.xml")

        ExtractFile(strDest & "\Formularios", "FinalizaActividades.xml")
        ExtractFile(strDest & "\Formularios", "FinalizaActividades.enUS.xml")

        ExtractFile(strDest & "\Formularios", "ListadoContratosSeguroPostVenta.xml")
        ExtractFile(strDest & "\Formularios", "ListadoContratosSeguroPostVenta.enUS.xml")

        ExtractFile(strDest & "\Formularios", "SolicitudEspecificos.xml")
        ExtractFile(strDest & "\Formularios", "SolicitudEspecificos.enUS.xml")

        ExtractFile(strDest & "\Formularios", "ListadoSolicitudEspecificos.xml")
        ExtractFile(strDest & "\Formularios", "ListadoSolicitudEspecificos.enUS.xml")

        ExtractFile(strDest & "\Formularios", "FacturacionMecanicos.xml")
        ExtractFile(strDest & "\Formularios", "FacturacionMecanicos.enUS.xml")

        ExtractFile(strDest & "\Formularios", "AvaluoUsados.enUS.xml")
        ExtractFile(strDest & "\Formularios", "AvaluoUsados.xml")

        ExtractFile(strDest & "\Formularios", "Configuracion_ADX_IC.enUS.xml")
        ExtractFile(strDest & "\Formularios", "Configuracion_ADX_IC.xml")

        ExtractFile(strDest & "\Formularios", "Configuracion_TSD_IC.enUS.xml")
        ExtractFile(strDest & "\Formularios", "Configuracion_TSD_IC.xml")

        ExtractFile(strDest & "\Formularios", "ReporteFinanciamientoCV.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ReporteFinanciamientoCV.xml")

        ExtractFile(strDest & "\Formularios", "TrackingSolEspecific.enUS.xml")
        ExtractFile(strDest & "\Formularios", "TrackingSolEspecific.xml")

        ExtractFile(strDest & "\Formularios", "ReporteAntiguedadVehiculos.enUS.xml")
        ExtractFile(strDest & "\Formularios", "ReporteAntiguedadVehiculos.xml")

        ExtractFile(strDest, "CornerImage.png")
        ExtractFile(strDest, "archivos.bmp")
        ExtractFile(strDest, "CFL.bmp")
        ExtractFile(strDest, "cont.bmp")
        ExtractFile(strDest, "DMSOne.bmp")
        ExtractFile(strDest, "etiqueta.bmp")
        ExtractFile(strDest, "Flecha.bmp")
        ExtractFile(strDest, "sbo.bmp")
        ExtractFile(strDest, "setup.bmp")
        ExtractFile(strDest, "financ.bmp")
        ExtractFile(strDest, "placas.bmp")
        ExtractFile(strDest, "InfDMS.bmp")
        ExtractFile(strDest, "imgBack.bmp")
        ExtractFile(strDest, "Imagenes.txt")
        ExtractFile(strDest, "ConfiguracionSBO_SoporteCritico.xml")
        ExtractFile(strDest, "citas.bmp")

        ExtractFile(strDest, "GeneradorHash.exe")
        ExtractFile(strDest, "SCG DMS One.exe")
        ExtractFile(strDest, "SCG DMS One.exe.config")
        ExtractFile(strDest, "SCG Visualizador de Reportes.exe")
        ExtractFile(strDest, "AccesoDatos.dll")
        ExtractFile(strDest, "AccesoDatosUDF.dll")
        ExtractFile(strDest, "AccesoDatosUDT.dll")
        ExtractFile(strDest, "ComponenteCristalReport.dll")
        ExtractFile(strDest, "ControlUDF.dll")
        ExtractFile(strDest, "ControlUDT.dll")
        ExtractFile(strDest, "CreacionUDF.dll")
        ExtractFile(strDest, "DataSets.dll")
        ExtractFile(strDest, "DataSetsUDF.dll")
        ExtractFile(strDest, "DataSetsUDT.dll")
        ExtractFile(strDest, "DeKlaritLibrary.dll")
        ExtractFile(strDest, "DMSONEDKFrameworkBusinessFramework.dll")
        ExtractFile(strDest, "DMSOneFramework.dll")
        ExtractFile(strDest, "DMS_Addon.dll")
        ExtractFile(strDest, "DMS_Connector.dll")
        ExtractFile(strDest, "Interop.Outlook.dll")
        ExtractFile(strDest, "Interop.SAPbobsCOM.dll")
        ExtractFile(strDest, "Interop.SAPbouiCOM.dll")
        ExtractFile(strDest, "Microsoft.SqlServer.BatchParser.dll")
        ExtractFile(strDest, "Microsoft.SqlServer.Replication.dll")
        ExtractFile(strDest, "NEWTEXTBOX.dll")
        ExtractFile(strDest, "PDSACryptography.dll")
        ExtractFile(strDest, "Proyecto SCGMSGBox.dll")
        ExtractFile(strDest, "Proyecto SCGToolBar.dll")
        ExtractFile(strDest, "RegionMasterControls.dll")
        ExtractFile(strDest, "SCG ComponenteImagenes.dll")
        ExtractFile(strDest, "SCG New Buscador.dll")
        ExtractFile(strDest, "SCG Produccion Controls.dll")
        ExtractFile(strDest, "SCG User Interface.dll")
        ExtractFile(strDest, "SCG.AccesoDatos.Conexion.dll")
        ExtractFile(strDest, "SCG.Cifrado.dll")
        ExtractFile(strDest, "SCG.Controls.Windows.dll")
        ExtractFile(strDest, "SCG.GenLic.Seguridad.Xml.dll")
        ExtractFile(strDest, "SCG.License.UX.Windows.dll")
        ExtractFile(strDest, "SCG.Seguridad.2005.dll")
        ExtractFile(strDest, "SCG.ServidorLic.LogicaNegocios.dll")
        ExtractFile(strDest, "SCG.UX.Windows.CitasAutomaticas.dll")
        ExtractFile(strDest, "SCG.UX.Windows.GeneradorConsultas.dll")
        ExtractFile(strDest, "SCG.UX.Windows.ManejoDeArchivosDigitales.dll")
        ExtractFile(strDest, "SCG.UX.Windows.SAP.dll")
        ExtractFile(strDest, "SCG.UX.Windows.TextBox.dll")
        ExtractFile(strDest, "SCGComboBox.dll")
        ExtractFile(strDest, "ManipuladorClienteDLL.dll")
        ExtractFile(strDest, "SCGExceptionHandler.dll")
        ExtractFile(strDest, "XML_Direccion_WebService.xml")
        ExtractFile(strDest, "IrisSkin2.dll")
        ExtractFile(strDest, "sbo8.8.ssk")
        ExtractFile(strDest, "sbo2007.ssk")
        ExtractFile(strDest, "Skins.xml")
        ExtractFile(strDest, "SCG.SkinManager.dll")
        ExtractFile(strDest, "SCG.Requisiciones.dll")
        ExtractFile(strDest, "SCG.SBOFramework.dll")
        ExtractFile(strDest, "SCG.DMSOne.Framework.dll")
        ExtractFile(strDest, "SCG.Financiamiento.dll")
        ExtractFile(strDest, "SCG.Placas.dll")
        ExtractFile(strDest, "SCG.WinFormsSAP.dll")
        ExtractFile(strDest, "smagentapi.dll")
        ExtractFile(strDest, "smcommonutil.dll")
        ExtractFile(strDest, "smerrlog.dll")
        ExtractFile(strDest, "Microsoft.Practices.EnterpriseLibrary.Common.dll")
        ExtractFile(strDest, "Microsoft.Practices.EnterpriseLibrary.Data.dll")
        ExtractFile(strDest, "SCG.ServicioPostVenta.dll")


        'WFR


        'While blnFileCreated = False
        '    Application.DoEvents()
        '    'Don't continue running until the file is copied...
        'End While


        If chkRestart.Checked Then
            RestartNeeded() ' Inform SBO the restart is needed
        End If
        EndInstall() ' Inform SBO the installation ended
        'Write installation Folder to registry


        'WriteToRegistry(RegistryHive.LocalMachine, "SOFTWARE", "path", "c:\folder")
        blnAns = WriteToRegistry(RegistryHive.LocalMachine, "SOFTWARE", strAddonName, strDest)

        MessageBox.Show("Finished Installing", "Installation ended", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Windows.Forms.Application.Exit() ' Exit the installer

    End Sub

#End Region

#Region "Eventos"

    Private Sub chkDefaultFolder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDefaultFolder.CheckedChanged
        If chkDefaultFolder.Checked Then
            txtDest.ReadOnly = True
        Else
            txtDest.ReadOnly = False
        End If
    End Sub

    Private Sub btnInstalar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInstalar.Click

        Dim NumOfParams As Integer 'The number of parameters in the command line (should be 2)

        Try

            If txtNombreAddon.Text <> "" Then

                strAddonName = txtNombreAddon.Text

                'Dim strAppPath As String

                NumOfParams = Environment.GetCommandLineArgs.Length

                If NumOfParams = 1 Then
                    'desinstalando
                    'strCmdLine = CStr(Environment.GetCommandLineArgs.GetValue(0))
                    UnInstall()
                    Windows.Forms.Application.Exit()

                Else
                    If NumOfParams = 2 Then
                        Install()
                        Windows.Forms.Application.Exit()
                    End If

                End If

            Else
                Dim msj As String = ""

                If _idioma = "es-CR" Then
                    msj = "Please enter the installation package"
                Else
                    msj = "No introdujo el nombre del archivo ejecutable del AddOn"
                End If

                MsgBox(msj, MsgBoxStyle.Exclamation, "<SCG> AddOn Installer")
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "<SCG> AddOn Installer")
        End Try
    End Sub

    Private Sub frmPrincipal_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim NumOfParams As Integer 'The number of parameters in the command line (should be 2)
        Dim strCmdLineElements(2) As String
        Dim strCmdLine As String ' The whole command line
        Dim textoBotonInstall As String = ""
        Dim textoBotonDesinstall As String = ""

        If _idioma = "es-CR" Then
            textoBotonInstall = "Install"
            textoBotonDesinstall = "Uninstall"
        Else
            textoBotonInstall = "Instalar"
            textoBotonDesinstall = "Desinstalar"
        End If

        Try
            NumOfParams = Environment.GetCommandLineArgs.Length

            If NumOfParams = 2 Then

                strCmdLine = CStr(Environment.GetCommandLineArgs.GetValue(1))

                strCmdLineElements = strCmdLine.Split(CType("|", Char))
                ' Get Install destination Folder
                strDest = CStr(strCmdLineElements.GetValue(0))
                txtDest.Text = strDest

                ' Get the "AddOnInstallAPI.dll" path
                strDll = CStr(strCmdLineElements.GetValue(1))
                strDll = strDll.Remove((strDll.Length - 19), 19) ' Only the path is needed

                Me.btnInstalar.Text = textoBotonInstall
            Else
                Me.btnInstalar.Text = textoBotonDesinstall
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "<SCG> AddOn Installer")
        End Try

    End Sub

#End Region


End Class
