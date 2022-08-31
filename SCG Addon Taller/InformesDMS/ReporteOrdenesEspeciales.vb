Imports SAPbouiCOM
Imports DMSOneFramework

Partial Public Class ReporteOrdenesEspeciales


#Region "Declaraciones"

    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon

    Private m_strConectionString As String
    Dim m_cn_Coneccion As New SqlClient.SqlConnection

#End Region

    ''' <summary>
    ''' Maneja el evento ItemPress del Fomrulario
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ManejadorEventoItemPress(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim l_strNumOT As String

            If pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case BtnPrintSbo.UniqueId

                        CargarReporte(BubbleEvent)

                End Select
            ElseIf pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case BtnPrintSbo.UniqueId
                        l_strNumOT = EditTextNumOT.ObtieneValorUserDataSource()
                        If String.IsNullOrEmpty(l_strNumOT) Then
                            _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorReporteSinNumeroOT, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End If

                End Select
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub CargarFormulario()
        Try


        Catch ex As Exception

        End Try
    End Sub

    Public Sub CargarReporte(ByRef BubbleEvent As Boolean)

        Try
            Dim strNomSucur As String = "Suc Prueba"
            Dim strNomComp As String
            Dim l_strNumOT As String
            Dim l_strNumVisita As String = ""
            Dim l_arrVisita() As String

            Dim l_intTieneGuia As Integer = 0

            Dim strSQLSucursal As String = "SELECT Code, Name FROM [@SCGD_SUCURSALES] where Code = '{0}'"
            strNomComp = _companySbo.CompanyName
            
            Me.StrParametros = ""

            l_strNumOT = EditTextNumOT.ObtieneValorUserDataSource()

            If Not String.IsNullOrEmpty(l_strNumOT) Then

                l_arrVisita = Split(l_strNumOT, "-", CompareMethod.Binary)
                l_strNumVisita = l_arrVisita(0).Trim()

                l_intTieneGuia = InStr(l_strNumOT, "-")


                If l_intTieneGuia = 0 Then
                    l_strNumOT = l_strNumOT & "-01"
                End If
                
            End If

            StrParametros = l_strNumOT & "," & l_strNumVisita & "," & strNomComp

            If Not String.IsNullOrEmpty(StrParametros) Then

                Call ImprimirReporte(My.Resources.Resource.rptOrdenesEspeciales,
                                     My.Resources.Resource.TituloReporteTrazabilidadOT,
                                     StrParametros)



            Else
                _applicationSbo.StatusBar.SetText("Error al generar el reporte", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)

            End If


        Catch ex As Exception
            BubbleEvent = False
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub


    'Imprimir reporte
    ''' <summary>
    ''' Imprime el reporte, llamando al componente externo
    ''' </summary>
    ''' <param name="strDireccionReporte"> Direccion de RPT de reportes </param>
    ''' <param name="strBarraTitulo"> Titulo para el reportes</param>
    ''' <param name="strParametros">parametros que recibe el reporte Numero de OT y Numero de Visita </param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub ImprimirReporte(ByVal strDireccionReporte As String, _
                               ByVal strBarraTitulo As String, _
                               ByVal strParametros As String)
        Try

            Dim strPathExe As String
            Dim strParametrosEjecutar As String

            objConfiguracionGeneral = Nothing

            Configuracion.CrearCadenaDeconexion(_companySbo.Server, _companySbo.CompanyDB, m_strConectionString)
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

            strPathExe &= strBarraTitulo & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & _companySbo.Server & "," & _companySbo.CompanyDB & " " & strParametros
            Shell(strPathExe, AppWinStyle.MaximizedFocus)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub



End Class


