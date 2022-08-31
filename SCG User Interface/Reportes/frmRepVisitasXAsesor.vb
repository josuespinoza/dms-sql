Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmRepVisitasXAsesor

        Private WithEvents m_buOrdenes As New Buscador.SubBuscador

        Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
            Try
                If Trim(txtAsesor.Text) <> "" And Trim(txtIdEmpleado.Text) <> "" Then
                    CargaReporte()
                Else
                    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarAsesor)
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btncerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncerrar.Click
            Try
                Me.Close()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'objSCGMSGBox.msgInformationCustom(ex.Message)
            End Try
        End Sub

        Public Overridable Sub CargaReporte()
            Dim rptTiempo As New ComponenteCristalReport.SubReportView

            Dim strParametros As String = ""

            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion

            Try
                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                strParametros = Trim(txtIdEmpleado.Text) & ","

                '********************************************************************************************************

                'strParametros = strParametros & dtpDesde.Value.ToString & ","
                'strParametros = strParametros & DateAdd(DateInterval.Second, -1, DateAdd(DateInterval.Day, 1, dtpHasta.Value)).ToString

                'Manejo de la fecha obteniendo el formato de la maquina
                Dim strFechaDesde As String
                Dim strFechaHasta As String

                strFechaDesde = Utilitarios.RetornaFechaFormatoRegional(dtpDesde.Value.Date)
                strFechaHasta = Utilitarios.RetornaFechaFormatoRegional(dtpHasta.Value.Date)

                strParametros = strParametros & strFechaDesde & "," & strFechaHasta

                '********************************************************************************************************

                With rptTiempo
                    .P_BarraTitulo = My.Resources.ResourceUI.rptBarraTitulosVisitasXAsesor
                    .P_WorkFolder = PATH_REPORTES
                    .P_Filename = My.Resources.ResourceUI.rptNombreVisistasXAsesor
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

#Region "Constructor"

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub
#End Region

        Private Sub frmRangoFechas_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            dtpDesde.Value = Today.Date
            dtpHasta.Value = Today.Date
        End Sub

        Private Sub picRepuesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picRepuesto.Click
            CargarBuscador(sender)
        End Sub

        Private Sub CargarBuscador(ByVal sender As System.Object)
            Try
                With m_buOrdenes

                    'Me.Cursor = Cursors.WaitCursor
                    Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarratitulosBuscadorEmpleados

                    .Titulos = My.Resources.ResourceUI.ID & "," & My.Resources.ResourceUI.Nombre & _
                    "," & My.Resources.ResourceUI.SegundoNombre & "," & My.Resources.ResourceUI.PrimerApellido
                    '"Id Empleado, Nombre ,Primer Apellido,Segundo Apellido"

                    .Criterios = "empid, firstName, middleName, lastName"
                    .Tabla = "SCGTA_VW_OHEM"
                    .Where = ""
                    .Activar_Buscador(sender)

                End With
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub m_buOrdenes_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_buOrdenes.AppAceptar
            txtIdEmpleado.Text = Campo_Llave
            txtAsesor.Text = Arreglo_Campos(1) & " " & Arreglo_Campos(2) & " " & Arreglo_Campos(3)
        End Sub


        Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click

        End Sub
    End Class
End Namespace
