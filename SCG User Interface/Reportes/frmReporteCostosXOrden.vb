Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmReporteCostosXOrden

        Public Sub New(ByVal p_blnEstado As Boolean)

            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()


        End Sub

        Private Sub frmReporteOTxFecha_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            dtpDesde.Value = Today.Date
            dtpHasta.Value = Today.Date
        End Sub

        Private Sub btncerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncerrar.Click
            Me.Close()
        End Sub

        Private Sub btnCargar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCargar.Click

            Dim strParametros As String = ""
            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion
            Try

                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)
                
                '********************************************************************************************************
                
                'strParametros = dtpDesde.Value.ToString & "," 'CDate(Format(dtpDesde.Value, "yyyyMMdd") & " 00:00:00.000" & "
                'strParametros = strParametros & DateAdd(DateInterval.Second, -1, DateAdd(DateInterval.Day, 1, dtpHasta.Value)).ToString

                'Manejo de la fecha obteniendo el formato de la maquina
                Dim strFechaDesde As String
                Dim strFechaHasta As String

                strFechaDesde = Utilitarios.RetornaFechaFormatoRegional(dtpDesde.Value.Date)
                strFechaHasta = Utilitarios.RetornaFechaFormatoRegional(dtpHasta.Value.Date)

                strParametros = strParametros & strFechaDesde & "," & strFechaHasta

                '********************************************************************************************************

                With rptReporte
                    .P_WorkFolder = PATH_REPORTES

                    If optDetallado.Checked = True Then
                        .P_BarraTitulo = My.Resources.ResourceUI.rptTituloCostoPorOrdenEntreFechas
                        .P_Filename = My.Resources.ResourceUI.rptNombreCostoPorOrdenEntreFechas
                    Else
                        .P_BarraTitulo = My.Resources.ResourceUI.rptTituloCostoPorOrdenEntreFechasResumido
                        .P_Filename = My.Resources.ResourceUI.rptNombreCostoPorOrdenEntreFechasResumido
                    End If

                    .P_Server = Server
                    .P_DataBase = strDATABASESCG
                    .P_CompanyName = COMPANIA
                    .P_User = UserSCGInternal
                    .P_Password = Password
                    .P_ParArray = strParametros


                End With

                rptReporte.VerReporte()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

    End Class

End Namespace
