Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface
    Public Class frmTiemposRealesVSStandar

        Private WithEvents m_buOrdenes As New Buscador.SubBuscador
        Private WithEvents m_buTiposOT As New Buscador.SubBuscador

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        Private Sub chkMecanico_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMecanico.CheckedChanged
            If chkMecanico.Checked = False Then
                txtEmpleado.Clear()
                txtIdEmpleado.Clear()
                txtEmpleado.Enabled = False
                txtIdEmpleado.Enabled = False
                picEmpleado.Enabled = False
                lblMecanico.Enabled = False
            Else
                txtEmpleado.Enabled = True
                txtIdEmpleado.Enabled = True
                picEmpleado.Enabled = True
                lblMecanico.Enabled = True
            End If
        End Sub

        Private Sub chkRangoFechas_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRangoFechas.CheckedChanged

            Dim datTiempo As New Date(1900, 1, 1)

            If chkRangoFechas.Checked = False Then
                dtpDesde.Value = datTiempo
                dtpHasta.Value = datTiempo
                gbxRangoFechas.Enabled = False
            Else
                gbxRangoFechas.Enabled = True
                dtpDesde.Value = Today.Date
                dtpHasta.Value = Today.Date
            End If
        End Sub


        Private Sub picEmpleado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picEmpleado.Click
            CargarBuscadorMecanicos(sender)
        End Sub

        Private Sub CargarBuscadorMecanicos(ByVal sender As System.Object)
            Try
                With m_buOrdenes

                    'Me.Cursor = Cursors.WaitCursor
                    Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                    .SQL_Cnn = DATemp.ObtieneConexion

                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorEmpleados
                    .Titulos = My.Resources.ResourceUI.ID & "," & My.Resources.ResourceUI.Nombre & _
                    "," & My.Resources.ResourceUI.SegundoNombre & "," & My.Resources.ResourceUI.PrimerApellido

                    '"Id Empleado, Nombre ,Primer Apellido,Segundo Apellido"
                    .Criterios = "empid, firstName,middleName, lastName"
                    .Tabla = "SCGTA_VW_OHEM"
                    .Where = ""
                    .Activar_Buscador(sender)

                End With
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub btncerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncerrar.Click
            Me.Close()
        End Sub

        Private Sub m_buOrdenes_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_buOrdenes.AppAceptar
            txtIdEmpleado.Text = Campo_Llave
            txtEmpleado.Text = Arreglo_Campos(1) & " " & Arreglo_Campos(2) & " " & Arreglo_Campos(3)
        End Sub

        Private Sub btnCargar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCargar.Click
            CargaReporte()
        End Sub

        Public Overridable Sub CargaReporte()
            Dim rptTiempos As New ComponenteCristalReport.SubReportView

            Dim strParametros As String = ""

            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion

            Try
                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                If Trim(txtIdOrden.Text) <> String.Empty Then
                    strParametros = txtIdOrden.Text
                Else
                    strParametros = "-1"
                End If
                
                '********************************************************************************************************

                'strParametros = strParametros & "," & dtpDesde.Value.ToString & ","
                'strParametros = strParametros & DateAdd(DateInterval.Second, -1, DateAdd(DateInterval.Day, 1, dtpHasta.Value)).ToString

                'Manejo de la fecha obteniendo el formato de la maquina
                Dim strFechaDesde As String
                Dim strFechaHasta As String

                strFechaDesde = Utilitarios.RetornaFechaFormatoRegional(dtpDesde.Value.Date)
                strFechaHasta = Utilitarios.RetornaFechaFormatoRegional(dtpHasta.Value.Date)

                strParametros = strParametros & "," & strFechaDesde & "," & strFechaHasta

                '********************************************************************************************************

                If Trim(txtIdEmpleado.Text) <> String.Empty Then
                    strParametros = strParametros & "," & txtIdEmpleado.Text
                Else
                    strParametros = strParametros & "," & "-1"
                End If


                With rptTiempos
                    .P_BarraTitulo = My.Resources.ResourceUI.rptBarraTituloReporteTiempoRealvsEstandar
                    .P_WorkFolder = PATH_REPORTES
                    .P_Filename = My.Resources.ResourceUI.rptNombreTiempoRealvsEstandar
                    .P_Server = Server
                    .P_DataBase = strDATABASESCG
                    .P_CompanyName = COMPANIA
                    .P_User = UserSCGInternal
                    .P_Password = Password
                    .P_ParArray = strParametros
                End With

                rptTiempos.VerReporte()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'objSCGMSGBox.msgInformationCustom(ex.Message)
            End Try
        End Sub

        Private Sub CargarBuscadorTiposOT(ByVal sender As System.Object)

            Try
                With m_buTiposOT


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

        Private Sub chkTipoOrden_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTipoOrden.CheckedChanged
            If chkTipoOrden.Checked = False Then
                txtDescripcionOrden.Enabled = False
                txtIdOrden.Clear()
                txtIdOrden.Enabled = False
                txtDescripcionOrden.Clear()
                picTipoOT.Enabled = False
                lblTipoOT.Enabled = False
            Else
                txtIdOrden.Enabled = True
                txtDescripcionOrden.Enabled = True
                picTipoOT.Enabled = True
                lblTipoOT.Enabled = True
            End If
        End Sub

        Private Sub picTipoOT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picTipoOT.Click
            CargarBuscadorTiposOT(sender)
        End Sub

        Private Sub m_buTiposOT_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_buTiposOT.AppAceptar
            txtIdOrden.Text = Campo_Llave
            txtDescripcionOrden.Text = Arreglo_Campos(1)
        End Sub

        Private Sub frmTiemposRealesVSStandar_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim datTiempo As New Date(1900, 1, 1)
            dtpDesde.Value = datTiempo
            dtpHasta.Value = datTiempo
        End Sub
    End Class
End Namespace