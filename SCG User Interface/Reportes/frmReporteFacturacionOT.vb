Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmReporteFacturacionOT



        Public Sub New(ByVal p_blnEstado As Boolean)

            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()


        End Sub

        Private Sub frmReporteFacturacionOT_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            cboMarcas.Items.Clear()
            cboMarcas.Items.Add("FORD")
            cboMarcas.Items.Add("VW")
            cboMarcas.Items.Add("SKODA")
            cboMarcas.Items.Add("OTRAS MARCAS")
            cboMarcas.Items.Add("TODAS")
            cboMarcas.SelectedIndex = 0
            dtpDesde.Value = Today.Date
            dtpHasta.Value = Today.Date

        End Sub

        Private Sub btncerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncerrar.Click
            Me.Close()
        End Sub

        Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
            Dim strParametros As String = ""
            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion
            Try
                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                'strParametros = strParametros & Format(dtpDesde.Value, "dd/MM/yyyy") & " 00:00:00.000" & ","

                strParametros = strParametros & dtpDesde.Value.ToString & "," 'CDate(Format(dtpDesde.Value, "yyyyMMdd") & " 00:00:00.000" & ",")

                'strParametros = strParametros & Format(dtpHasta.Value, "dd/MM/yyyy") & " 23:59:59.999" & ","

                strParametros = strParametros & DateAdd(DateInterval.Second, -1, DateAdd(DateInterval.Day, 1, dtpHasta.Value)).ToString & ","

                ''CDate(Format(dtpHasta.Value, "yyyyMMdd") & " 23:59:59.999" & ",")

                Select Case Trim(cboMarcas.Text)
                    Case "FORD"
                        strParametros = strParametros & "117"
                    Case "VW"
                        strParametros = strParametros & "98"
                    Case "SKODA"
                        strParametros = strParametros & "118"
                    Case "OTRAS MARCAS"
                        strParametros = strParametros & "-1"
                    Case "TODAS"
                        strParametros = strParametros & "-2"
                End Select


                With rptReporte
                    .P_BarraTitulo = My.Resources.ResourceUI.repBarraTituloFactOrdenesTrabajo
                    .P_WorkFolder = PATH_REPORTES
                    .P_Filename = My.Resources.ResourceUI.rptNombreReporteFacturacionOT
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