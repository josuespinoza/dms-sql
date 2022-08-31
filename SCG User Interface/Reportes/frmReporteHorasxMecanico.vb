Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface
    Public Class frmReporteHorasxMecanico


        Private WithEvents m_buOrdenes As New Buscador.SubBuscador

        Private Sub picRepuesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picRepuesto.Click
            CargarBuscador(sender)
        End Sub

        Public Sub New(ByVal p_blnEstado As Boolean)

            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()


        End Sub

        Private Sub CargarBuscador(ByVal sender As System.Object)
            Try
                With m_buOrdenes

                    'Me.Cursor = Cursors.WaitCursor
                    Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                    .SQL_Cnn = DATemp.ObtieneConexion


                    '********************************************************************************************
                    'Agregado 01/03/2012: Agregar configuración validación de tiempo estándar
                    'Autor: José Soto

                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorEmpleados
                    .Titulos = My.Resources.ResourceUI.ID & "," & My.Resources.ResourceUI.Nombre & _
                    "," & My.Resources.ResourceUI.SegundoNombre & "," & My.Resources.ResourceUI.PrimerApellido

                    '.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorEmpleados
                    '.Titulos = My.Resources.ResourceUI.ID & "," & My.Resources.ResourceUI.PrimerApellido & _
                    '"," & My.Resources.ResourceUI.Nombre & "," & My.Resources.ResourceUI.SegundoApellido




                    '"Id Empleado, Nombre ,Primer Apellido,Segundo Apellido"
                    .Criterios = "empid, firstName, middleName, lastName"
                    .Tabla = "SCGTA_VW_OHEM"
                    .Where = ""
                    .Activar_Buscador(sender)
                    '********************************************************************************************

                End With
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub frmReporteHorasxMecanico_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            dtpDesde.Value = Today.Date
            dtpHasta.Value = Today.Date
        End Sub

        Public Overridable Sub CargaReporte()
            Dim rptTiempo As New ComponenteCristalReport.SubReportView

            Dim strParametros As String = ""

            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion

            Try
                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                strParametros = txtDiasTrabajados.Text & "," & Trim(txtIdEmpleado.Text) & ","

                strParametros = strParametros & dtpDesde.Value.ToString & ","
                strParametros = strParametros & DateAdd(DateInterval.Second, -1, DateAdd(DateInterval.Day, 1, dtpHasta.Value)).ToString

                With rptTiempo
                    .P_BarraTitulo = My.Resources.ResourceUI.rptBarraTituloHorasXMecanico
                    .P_WorkFolder = PATH_REPORTES
                    .P_Filename = My.Resources.ResourceUI.rptNombreHorasXMecanico
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

        Private Sub m_buOrdenes_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_buOrdenes.AppAceptar
            txtIdEmpleado.Text = Campo_Llave
            txtEmpleado.Text = Arreglo_Campos(1) & " " & Arreglo_Campos(2) & " " & Arreglo_Campos(3)
        End Sub


        Private Sub btncerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncerrar.Click
            Me.Close()
        End Sub

    End Class
End Namespace
