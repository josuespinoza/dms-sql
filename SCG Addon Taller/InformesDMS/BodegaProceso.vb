Option Explicit On

Imports System.Globalization
Imports System.IO
Imports DMSOneFramework.CitasTableAdapters
Imports SCG.DMSOne.Framework.MenuManager
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SAPbouiCOM
Imports DMSOneFramework


Partial Public Class BodegaProceso

#Region "Declariones"
    'declaracion de objetos generales 
    Private m_oCompany As SAPbobsCOM.Company
    Private m_SBO_Application As SAPbouiCOM.Application
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon

    'objeto form 
    Private oForm As SAPbouiCOM.Form
    Public n As NumberFormatInfo
    Private m_strDireccionConfiguracion As String

    'objeto datatable 
    Private _strParametros As String

    Private m_strConectionString As String
    Dim m_cn_Coneccion As New SqlClient.SqlConnection

    Public g_strBodxSucursinCOMA As String = String.Empty
    
    Public g_strDBBodegas As String = String.Empty

#End Region

#Region "Properties"

    <System.CLSCompliant(False)> _
    Public Property SAPCompany() As SAPbobsCOM.Company
        Get
            Return m_oCompany
        End Get
        Set(ByVal value As SAPbobsCOM.Company)
            m_oCompany = value
        End Set
    End Property

    Public Property StrParametros As String
        Get
            Return _strParametros
        End Get
        Set(ByVal value As String)
            _strParametros = value
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Sub CargarFormulario()

        cbxBodega.AsignaValorUserDataSource("N")

    End Sub

    Public Sub CargarCombos()
        Try
            Dim sboItem As SAPbouiCOM.Item
            Dim sboCombo As SAPbouiCOM.ComboBox

            sboItem = FormularioSBO.Items.Item(cboSucursal.UniqueId)
            sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
            Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, "SELECT Code, Name FROM dbo.[@SCGD_SUCURSALES] with (nolock) ORDER BY U_BDSucursal")

            SucursalUsuario()

            sboItem = FormularioSBO.Items.Item(cboBodega.UniqueId)
            sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)

            If sboCombo.ValidValues.Count > 0 Then
                sboCombo.Select(0, BoSearchKey.psk_Index)
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    Public Sub SucursalUsuario()
        Try
            Dim sboItem As SAPbouiCOM.Item
            Dim sboCombo As SAPbouiCOM.ComboBox
            Dim strUsuario As String = String.Empty
            Dim strConsulta As String = String.Empty
            Dim strSucursalTaller As String = String.Empty

            strUsuario = m_SBO_Application.Company.UserName.ToString.Trim()

            strConsulta = "Select SUC.Code " &
                            "From dbo.OUSR USR Inner Join [dbo].[@SCGD_SUCURSALES] SUC " &
                            "On USR.Branch=SUC.Code " &
                            "Where USR.USER_CODE='" & strUsuario & "'"

            strSucursalTaller = Utilitarios.EjecutarConsulta(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)

            'Asigno la sucursal que tiene el usuario conectado por defecto en el combo
            sboItem = FormularioSBO.Items.Item(cboSucursal.UniqueId)
            sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
            sboCombo.Select(strSucursalTaller, SAPbouiCOM.BoSearchKey.psk_ByValue)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    Public Sub ManejoEventosCombo(ByVal formUID As String, _
                                      ByVal pval As SAPbouiCOM.ItemEvent, _
                                      ByRef BubbleEvent As Boolean)
        Try

            If pval.ActionSuccess = True Then
                If pval.ItemUID = cboSucursal.UniqueId Then
                    CargaComboBodegas()
                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    Private Sub CargaComboBodegas()

        Dim sboCombo As ComboBox
        Dim sboItem As Item
        Dim strCodSucursal As String = String.Empty

        Try
            strCodSucursal = cboSucursal.ObtieneValorUserDataSource()

            sboItem = FormularioSBO.Items.Item(cboBodega.UniqueId)
            sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)

            Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, _
                                                        String.Format("SELECT DISTINCT CBXC.U_Pro, CBXC.U_Pro as upro FROM [@SCGD_CONF_BODXCC] CBXC with (nolock) " &
                                                                      "INNER JOIN [@SCGD_CONF_SUCURSAL] CSU with (nolock) on CSU.DocEntry = CBXC.DocEntry " &
                                                                      "WHERE CSU.U_Sucurs = '{0}'", strCodSucursal))
            sboCombo.ValidValues.Add("", "")

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    'Imprimir reportes
    <System.CLSCompliant(False)> _
    Public Sub ImprimirReporte(ByVal strDireccionReporte As String, _
                               ByVal strBarraTitulo As String, _
                               ByVal strParametros As String)
        Try

            Dim strPathExe As String = String.Empty

            objConfiguracionGeneral = Nothing

            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, m_strConectionString)
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

            strPathExe &= strBarraTitulo & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & m_oCompany.Server & "," & m_oCompany.CompanyDB & " " & strParametros
            Shell(strPathExe, AppWinStyle.MaximizedFocus)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

#Region "Eventos"

    Public Sub ValidarDatos(ByRef BubbleEvent As Boolean)
        Try

            Dim strSucursal As String = String.Empty
            Dim strBodega As String = String.Empty

            strSucursal = cboSucursal.ObtieneValorUserDataSource()
            strBodega = cboBodega.ObtieneValorUserDataSource()

            If IsDBNull(strSucursal) OrElse String.IsNullOrEmpty(strSucursal) Then
                BubbleEvent = False
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRptBodegasinSucur, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub

            ElseIf (IsDBNull(strBodega) OrElse String.IsNullOrEmpty(strBodega)) AndAlso cbxBodega.ObtieneValorUserDataSource() = "N" Then
                BubbleEvent = False
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRprtBodegasinBodega, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
            End If

        Catch ex As Exception
            BubbleEvent = False
            Throw ex
        End Try
    End Sub

    Public Sub CargarReporte(ByRef BubbleEvent As Boolean)
        Try

            StrParametros = String.Empty

            Dim strNumeroOT As String = String.Empty
            Dim strBodega As String = String.Empty

            strNumeroOT = txtNumeroOT.ObtieneValorUserDataSource()
            strBodega = cboBodega.ObtieneValorUserDataSource()

            If strBodega = String.Empty Then
                strBodega = "ALL"
            End If

            StrParametros = String.Format("{0},{1}", strBodega, strNumeroOT)

            If Not String.IsNullOrEmpty(StrParametros) Or BubbleEvent = False Then

                Call ImprimirReporte(My.Resources.Resource.rptBodegaProceso, My.Resources.Resource.TituloReporteBodegaProcesos, StrParametros)

            Else
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRptBodegasParametros, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
            End If

        Catch ex As Exception
            BubbleEvent = False
            Throw ex
        End Try
    End Sub


    Public Sub ManejadorEventoItemPress(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim sboItem As Item
            Dim sboCombo As ComboBox

            Dim l_TodoOT As String = String.Empty

            If pVal.ItemUID = BtnPrintSbo.UniqueId Then
                If pVal.BeforeAction Then
                    ValidarDatos(BubbleEvent)
                ElseIf pVal.ActionSuccess Then
                    CargarReporte(BubbleEvent)
                End If


            ElseIf pVal.ItemUID = cbxBodega.UniqueId Then

                If pVal.ActionSuccess Then
                    _formularioSbo.Freeze(True)

                    l_TodoOT = cbxBodega.ObtieneValorUserDataSource()

                    sboItem = FormularioSBO.Items.Item(cboBodega.UniqueId)
                    sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)

                    If l_TodoOT = "N" Then
                        _formularioSbo.Items.Item(cboBodega.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
                        sboCombo.Select(0, BoSearchKey.psk_Index)
                    ElseIf l_TodoOT = "Y" Then
                        _formularioSbo.Items.Item(cboBodega.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_False)
                        sboCombo.Select(sboCombo.ValidValues.Count - 1, BoSearchKey.psk_Index)

                    End If
                    _formularioSbo.Freeze(False)
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#End Region

End Class
