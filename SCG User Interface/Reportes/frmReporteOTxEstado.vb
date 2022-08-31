Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface
    Public Class frmReporteOTxEstado

        Private WithEvents m_buOrdenes As New Buscador.SubBuscador

        Private Sub picRepuesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picRepuesto.Click
            CargarBuscador(sender)
        End Sub

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        Private Sub CargarBuscador(ByVal sender As System.Object)
            Try
                With m_buOrdenes

                    'Me.Cursor = Cursors.WaitCursor
                    Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorTipoOrdenes
                    .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Descripcion
                    .Criterios = "CodTipoOrden, Descripcion"
                    .Tabla = "SCGTA_TB_TipoOrden"
                    .Where = ""
                    .Activar_Buscador(sender)

                End With
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub chkRangoFechas_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRangoFechas.CheckedChanged
            If chkRangoFechas.Checked = True Then
                gbxRangoFechas.Enabled = True
                dtpDesde.Value = Now.Date
                dtpHasta.Value = Now.Date
            Else
                gbxRangoFechas.Enabled = False
            End If
        End Sub

        Private Sub chkTipoOrden_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTipoOrden.CheckedChanged
            If chkTipoOrden.Checked = True Then
                txtDescripcionOrden.Enabled = True
                txtIdOrden.Enabled = True
                lblTipoOrden.Enabled = True
                picRepuesto.Enabled = True
            Else
                txtDescripcionOrden.Enabled = False
                txtIdOrden.Enabled = False
                lblTipoOrden.Enabled = False
                picRepuesto.Enabled = False
            End If
        End Sub

        Private Sub btncerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncerrar.Click
            Me.Close()
        End Sub

        Public Overridable Sub CargaReporte()

            Dim rptTiempo As New ComponenteCristalReport.SubReportView

            Dim strParametros As String = ""

            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion
            'Dim strMoneda As String

            Try
                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                If chkRangoFechas.Checked = True Then
                    strParametros = "1,"
                Else
                    strParametros = "-1,"
                End If

                strParametros = strParametros & dtpDesde.Value.ToString & ","
                strParametros = strParametros & DateAdd(DateInterval.Second, -1, DateAdd(DateInterval.Day, 1, dtpHasta.Value)).ToString & ","

                If chkTipoOrden.Checked Then
                    strParametros = strParametros & Trim(txtIdOrden.Text) & ","
                Else
                    strParametros = strParametros & "-1,"
                End If

                If chkTipoOrden.Checked Then
                    strParametros = strParametros & "1,"
                Else
                    strParametros = strParametros & "-1,"
                End If

                If chkEstado.Checked = True Then
                    strParametros = strParametros & cboEstadoOT.SelectedValue
                Else
                    strParametros = strParametros & "-1"
                End If


                With rptTiempo
                    .P_BarraTitulo = My.Resources.ResourceUI.rptBarraTituloOrdenesXEstado
                    .P_WorkFolder = PATH_REPORTES
                    If rbtEstandar.Checked = True Then
                        .P_Filename = My.Resources.ResourceUI.rptNombreOrdenesXEstado
                    Else
                        .P_Filename = My.Resources.ResourceUI.rptNombreOrdenesXEstadoDetallado
                    End If
                    .P_Server = Server
                    .P_DataBase = strDATABASESCG
                    .P_CompanyName = COMPANIA
                    .P_User = UserSCGInternal
                    .P_Password = Password
                    .P_ParArray = strParametros
                End With

                rptTiempo.VerReporte()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'objSCGMSGBox.msgInformationCustom(ex.Message)
            End Try
        End Sub

        Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
            CargaReporte()
            dtpDesde.Value = Now.Date
            dtpHasta.Value = Now.Date
        End Sub

        Private Sub m_buOrdenes_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_buOrdenes.AppAceptar
            txtIdOrden.Text = Campo_Llave
            txtDescripcionOrden.Text = Arreglo_Campos(1)
        End Sub

        Private Sub frmReporteOTxEstado_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            CargarCombo()
        End Sub

        Sub CargarCombo()
            cboEstadoOT.Items.Clear()
            clsUtilidadCombos.CargarComboEstadoOT(cboEstadoOT)
        End Sub



        Private Sub chkEstado_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEstado.CheckedChanged
            If chkEstado.Checked = True Then
                lblEstado.Enabled = True
                cboEstadoOT.Enabled = True
            Else
                lblEstado.Enabled = False
                cboEstadoOT.Enabled = False
            End If

        End Sub
    End Class
End Namespace