Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmReporteOrdenesxTipo

        Private WithEvents m_buOrdenes As New Buscador.SubBuscador

        Private Sub picRepuesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picRepuesto.Click
            CargarBuscador(sender)
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

        Private Sub m_buOrdenes_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_buOrdenes.AppAceptar
            txtIdOrden.Text = Campo_Llave
            txtDescripcionOrden.Text = Arreglo_Campos(1)
        End Sub

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        Private Sub btncerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncerrar.Click
            Me.Close()
        End Sub

        Public Overridable Sub CargaReporte()

            Dim rptTiempo As New ComponenteCristalReport.SubReportView

            Dim strParametros As String = ""

            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion
            Dim strMoneda As String

            Try
                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                If chkTipoOt.Checked = True Then
                    strParametros = txtDescripcionOrden.Text & ","
                Else
                    strParametros = " " & ","
                End If


                If chkTipoOt.Checked = True Then
                    strParametros = strParametros & Trim(txtIdOrden.Text) & ","
                Else
                    strParametros = strParametros & "0" & ","
                End If

                '********************************************************************************************************

                'strParametros = strParametros & dtpDesde.Value.ToString & ","
                'strParametros = strParametros & DateAdd(DateInterval.Second, -1, DateAdd(DateInterval.Day, 1, dtpHasta.Value)).ToString & ","

                'Manejo de la fecha obteniendo el formato de la maquina
                Dim strFechaDesde As String
                Dim strFechaHasta As String

                strFechaDesde = Utilitarios.RetornaFechaFormatoRegional(dtpDesde.Value.Date)
                strFechaHasta = Utilitarios.RetornaFechaFormatoRegional(dtpHasta.Value.Date)

                strParametros = strParametros & strFechaDesde & "," & strFechaHasta & ","

                '********************************************************************************************************

                If optMonedaLocal.Checked = True Then
                    strMoneda = Trim(Utilitarios.ObtenerMonedaLocal)
                Else
                    strMoneda = Trim(Utilitarios.ObtenerMonedaSistema)
                End If

                strParametros = strParametros & strMoneda & ","

                If chkTipoOt.Checked = True Then
                    strParametros = strParametros & "0"
                Else
                    strParametros = strParametros & "1"
                End If


                With rptTiempo
                    .P_BarraTitulo = My.Resources.ResourceUI.rptBarraTituoOrdenesXTipo
                    .P_WorkFolder = PATH_REPORTES
                    .P_Filename = My.Resources.ResourceUI.rptNombreOrdenesxTipo
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

            If chkTipoOt.Checked = True Then
                If Trim(txtIdOrden.Text) <> "" Then
                    CargaReporte()
                Else
                    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeCompletarCampos)
                End If
            Else
                CargaReporte()
            End If

        End Sub

        Private Sub frmReporteOrdenesxTipo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            dtpDesde.Value = Today.Date
            dtpHasta.Value = Today.Date
            optMonedaLocal.Checked = True
        End Sub

        Private Sub chkTipoOt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTipoOt.CheckedChanged
            If chkTipoOt.Checked = True Then
                txtIdOrden.Enabled = True
                txtDescripcionOrden.Enabled = True
                picRepuesto.Enabled = True
            Else
                txtIdOrden.Enabled = False
                txtDescripcionOrden.Enabled = False
                picRepuesto.Enabled = False
            End If
        End Sub
    End Class
End Namespace