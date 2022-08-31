Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface
    Public Class frmReporteResumenFacturacionXMecanico

        Private Sub btncerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncerrar.Click
            Me.Close()
        End Sub

        Public Sub New(ByVal p_blnEstado As Boolean)

            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()


        End Sub


        Private Sub frmReporteResumenFacturacionXMecanico_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            dtpDesde.Value = Now.Date
            dtpHasta.Value = Now.Date
        End Sub

        Sub CargaReporte()
            Dim strParametros As String = ""
            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion
            Try

                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)
                strParametros = Trim(txtDiasTrabajados.Text) & ","
                strParametros = strParametros & dtpDesde.Value.ToString & ","
                strParametros = strParametros & DateAdd(DateInterval.Second, -1, DateAdd(DateInterval.Day, 1, dtpHasta.Value)).ToString

                With rptReporte
                    .P_BarraTitulo = My.Resources.ResourceUI.rptBarraTituloResumenFacturacionXMecanico
                    .P_WorkFolder = PATH_REPORTES
                    .P_Filename = My.Resources.ResourceUI.rptNombreResumenFacturacionXMecanico
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

        Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
            If Trim(txtDiasTrabajados.Text) <> "" Then

                If CInt(Trim(txtDiasTrabajados.Text)) > 0 Then
                    CargaReporte()
                Else
                    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeNumeroDiasMayorCero)
                End If

            Else
                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeCompletarCampos)
            End If

        End Sub
    End Class
End Namespace