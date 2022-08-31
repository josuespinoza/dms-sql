Option Explicit On

Imports System.Globalization
Imports System.IO
Imports DMSOneFramework.CitasTableAdapters
Imports SCG.DMSOne.Framework.MenuManager
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SAPbouiCOM
Imports DMSOneFramework


Partial Public Class ReporteSociosNegocios

#Region "Declaraciones"
    'declaracion de objetos generales 
    Private m_oCompany As SAPbobsCOM.Company
    Private m_SBO_Application As SAPbouiCOM.Application
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon
    Private m_strConectionString As String
    'objeto form 
    Private oForm As SAPbouiCOM.Form
    Private _strParametros As String
    Dim m_cn_Coneccion As New SqlClient.SqlConnection
    Private m_strDireccionConfiguracion As String
#End Region

#Region "Propiedades"

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

#Region "Metodos"

    Public Sub CargarFormulario()
    End Sub

    Public Sub CargarComboMarcas()
        Dim oItem As SAPbouiCOM.Item
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim strQueryMarcas As String = "SELECT ""Code"", ""Name"" FROM ""@SCGD_MARCA"" Order By ""Name"""

        Try
            'LimpiarCombo("cboMake")
            LimpiarCombo("cboEstil")
            LimpiarCombo("cboModel")

            oItem = FormularioSBO.Items.Item("cboMake")
            oCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)

            If oCombo.ValidValues.Count = 0 Then
                Utilitarios.CargarValidValuesEnCombos(oCombo.ValidValues, strQueryMarcas)
                oCombo.ValidValues.Add("", "")
            End If
            
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub CargarComboEstilo()
        Dim oItem As SAPbouiCOM.Item
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim strQueryEstilos As String = "SELECT ""Code"", ""Name"" FROM ""@SCGD_ESTILO"" WHERE ""U_Cod_Marc"" = '{0}' Order By ""Name"""
        Dim strMarcaSeleccionada As String = String.Empty
        Try
            LimpiarCombo("cboEstil")
            LimpiarCombo("cboModel")

            oItem = FormularioSBO.Items.Item("cboMake")
            oCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
            strMarcaSeleccionada = oCombo.Selected.Value

            If Not String.IsNullOrEmpty(strMarcaSeleccionada) Then
                oItem = FormularioSBO.Items.Item("cboEstil")
                oCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
                strQueryEstilos = String.Format(strQueryEstilos, strMarcaSeleccionada)
                Utilitarios.CargarValidValuesEnCombos(oCombo.ValidValues, strQueryEstilos)
                oCombo.ValidValues.Add("", "")
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub CargarComboModelo()
        Dim oItem As SAPbouiCOM.Item
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim strQueryModelos As String = "SELECT ""Code"", ""Name"" FROM ""@SCGD_MODELO"" WHERE ""U_Cod_Esti"" = '{0}' Order By ""Name"""
        Dim strEstiloSeleccionado As String = String.Empty
        Try
            LimpiarCombo("cboModel")

            oItem = FormularioSBO.Items.Item("cboEstil")
            oCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
            strEstiloSeleccionado = oCombo.Selected.Value

            If Not String.IsNullOrEmpty(strEstiloSeleccionado) Then
                oItem = FormularioSBO.Items.Item("cboModel")
                oCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
                strQueryModelos = String.Format(strQueryModelos, strEstiloSeleccionado)
                Utilitarios.CargarValidValuesEnCombos(oCombo.ValidValues, strQueryModelos)
                oCombo.ValidValues.Add("", "")
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub LimpiarCombo(ByVal p_strUID As String)
        Dim oItem As SAPbouiCOM.Item
        Dim oCombo As SAPbouiCOM.ComboBox
        Try
            oItem = FormularioSBO.Items.Item(p_strUID)
            oCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)

            If oCombo.ValidValues.Count > 0 Then
                For i As Integer = 0 To oCombo.ValidValues.Count - 1
                    oCombo.ValidValues.Remove(0, BoSearchKey.psk_Index)
                Next
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ManejoEventosCombo(ByVal formUID As String, ByVal pval As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try

            'If pval.ActionSuccess = True Then
            '    If pval.ItemUID = cboSucursal.UniqueId Then
            '        CargaComboBodegas()
            '    End If
            'End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
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
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

#Region "Eventos"

    Public Sub CargarReporte(ByRef BubbleEvent As Boolean)
        Dim strParametros As String = String.Empty

        Try

            strParametros = ObtenerParametros()

            Call ImprimirReporte(My.Resources.Resource.rptSociosNegocios, My.Resources.Resource.TituloReporteSociosNegocios, strParametros)


        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            BubbleEvent = False
        End Try
    End Sub

    Public Function ObtenerParametros() As String
        Dim strMarca As String = String.Empty
        Dim strEstilo As String = String.Empty
        Dim strModelo As String = String.Empty
        Dim strYear As String = String.Empty
        Dim oItem As SAPbouiCOM.Item
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim oEditText As SAPbouiCOM.EditText
        Dim strWhere As String = String.Empty

        Try
            oItem = FormularioSBO.Items.Item("cboMake")
            oCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
            If oCombo.Selected IsNot Nothing Then
                strMarca = oCombo.Selected.Value
            End If

            oItem = FormularioSBO.Items.Item("cboEstil")
            oCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
            If oCombo.Selected IsNot Nothing Then
                strEstilo = oCombo.Selected.Value
            End If

            oItem = FormularioSBO.Items.Item("cboModel")
            oCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
            If oCombo.Selected IsNot Nothing Then
                strModelo = oCombo.Selected.Value
            End If

            oItem = FormularioSBO.Items.Item("txtYear")
            oEditText = DirectCast(oItem.Specific, SAPbouiCOM.EditText)
            strYear = oEditText.Value

            If Not String.IsNullOrEmpty(strMarca) Or Not String.IsNullOrEmpty(strEstilo) Or Not String.IsNullOrEmpty(strModelo) Or Not String.IsNullOrEmpty(strYear) Then
                If Not String.IsNullOrEmpty(strMarca) Then
                    strWhere += String.Format(" WHERE VEHICULO.U_Cod_Marc = '{0}' ", strMarca)

                    If Not String.IsNullOrEmpty(strEstilo) Then
                        strWhere += String.Format(" AND VEHICULO.U_Cod_Esti = '{0}' ", strEstilo)
                    End If

                    If Not String.IsNullOrEmpty(strModelo) Then
                        strWhere += String.Format(" AND VEHICULO.U_Cod_Mode = '{0}' ", strModelo)
                    End If
                End If

                If Not String.IsNullOrEmpty(strYear) Then
                    If String.IsNullOrEmpty(strWhere) Then
                        strWhere += String.Format(" WHERE VEHICULO.U_Ano_Vehi = '{0}' ", strYear)
                    Else
                        strWhere += String.Format(" AND VEHICULO.U_Ano_Vehi = '{0}' ", strYear)
                    End If
                End If
            Else
                strWhere = "--"
            End If

            Return strWhere
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function


    Public Sub ManejadorEventoItemPress(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.ActionSuccess Then
                Select Case pVal.EventType
                    Case BoEventTypes.et_COMBO_SELECT
                        Select Case pVal.ItemUID
                            Case "cboMake"
                                CargarComboEstilo()
                            Case "cboEstil"
                                CargarComboModelo()
                        End Select
                    Case BoEventTypes.et_CLICK
                        Select Case pVal.ItemUID
                            Case "btnPrint"
                                CargarReporte(BubbleEvent)
                        End Select
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

#End Region

#End Region


End Class
