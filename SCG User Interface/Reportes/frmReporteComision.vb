Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmReporteComision
        Inherits frmRangoFechas

        Private WithEvents m_buOrdenes As New Buscador.SubBuscador

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New(p_blnEstado)

            InitializeComponent()

        End Sub

        Private Sub CargarBuscador(ByVal sender As System.Object)
            Try
                With m_buOrdenes

                    'Me.Cursor = Cursors.WaitCursor
                    Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                    .SQL_Cnn = DATemp.ObtieneConexion

                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorEmpleados
                    .Titulos = My.Resources.ResourceUI.ID & "," & My.Resources.ResourceUI.Nombre & _
                    "," & My.Resources.ResourceUI.SegundoNombre & "," & My.Resources.ResourceUI.PrimerApellido


                    '********************************************************************************************
                    'Agregado 01/03/2012: Agregar configuración validación de tiempo estándar
                    'Autor: José Soto
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

        Public Overrides Sub CargaReporte()
            Dim rptTiempo As New ComponenteCristalReport.SubReportView

            Dim strParametros As String = ""

            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion

            Try
                If txtIdEmpleado.Text = String.Empty Then
                    txtIdEmpleado.Text = 0
                End If

                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)
                
                '********************************************************************************************************
                'strParametros = strParametros & dtpDesde.Value.ToString & ","

                'strParametros = strParametros & dtpHasta.Value.ToString & ","

                'Manejo de la fecha obteniendo el formato de la maquina
                Dim strFechaDesde As String
                Dim strFechaHasta As String

                strFechaDesde = Utilitarios.RetornaFechaFormatoRegional(dtpDesde.Value.Date)
                strFechaHasta = Utilitarios.RetornaFechaFormatoRegional(dtpHasta.Value.Date)

                strParametros = strParametros & strFechaDesde & "," & strFechaHasta & ","

                '********************************************************************************************************

                strParametros = strParametros & txtIdEmpleado.Text

                With rptTiempo
                    .P_BarraTitulo = My.Resources.ResourceUI.rptTituloVentasxMecanicoComision
                    .P_WorkFolder = PATH_REPORTES
                    .P_Filename = My.Resources.ResourceUI.rptNombreVentasxMecanicoComision
                    .P_Server = Server
                    .P_DataBase = strDATABASESCG
                    .P_CompanyName = COMPANIA
                    .P_User = UserSCGInternal
                    .P_Password = Password
                    .P_ParArray = strParametros
                End With

                rptTiempo.VerReporte()
                Me.txtEmpleado.Text = String.Empty
                Me.txtIdEmpleado.Text = String.Empty

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                ' objSCGMSGBox.msgInformationCustom(ex.Message)
            End Try
        End Sub
        Private Sub picRepuesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picRepuesto.Click

            Me.CargarBuscador(sender)
        End Sub


        Private Sub InitializeComponent()
            Me.GroupBox1.SuspendLayout()
            Me.SuspendLayout()
            '
            'dtpDesde
            '
            Me.dtpDesde.Value = New Date(2010, 7, 13, 0, 0, 0, 0)
            '
            'GroupBox1
            '
            Me.GroupBox1.Size = New System.Drawing.Size(293, 66)
            '
            'btnBuscar
            '
            Me.btnBuscar.Location = New System.Drawing.Point(8, 108)
            '
            'btncerrar
            '
            Me.btncerrar.Location = New System.Drawing.Point(86, 108)
            '

            'Agregado 17/11/2010: Acomodar componentes interfaz
            Me.lblMecanico.Location = New System.Drawing.Point(10, 98)
            Me.txtIdEmpleado.Location = New System.Drawing.Point(75, 92)
            Me.txtEmpleado.Location = New System.Drawing.Point(105, 92)
            Me.picRepuesto.Location = New System.Drawing.Point(263, 95)

            'frmReporteComision
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.ClientSize = New System.Drawing.Size(306, 137)
            Me.Name = "frmReporteComision"
            Me.Text = "Reporte Comisiones"
            Me.GroupBox1.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Private Sub frmReporteComision_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Me.Text = My.Resources.ResourceUI.rptTituloVentasxMecanicoComision
            Me.txtEmpleado.Visible = True
            Me.txtIdEmpleado.Visible = True
            Me.picRepuesto.Visible = True
            Me.lblMecanico.Visible = True
            Me.Size = New Size(320, 200)
        End Sub

        Private Sub m_buOrdenes_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_buOrdenes.AppAceptar
            txtIdEmpleado.Text = Campo_Llave
            txtEmpleado.Text = Arreglo_Campos(1) & " " & Arreglo_Campos(2) & " " & Arreglo_Campos(3)
        End Sub
    End Class
End Namespace