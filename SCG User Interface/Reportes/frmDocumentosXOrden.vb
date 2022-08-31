Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface
    Public Class frmDocumentosXOrden

        Private WithEvents SubBOTs As New Buscador.SubBuscador

        Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
            CargaReporte()
        End Sub

        Public Overridable Sub CargaReporte()

            Dim rptTiempo As New ComponenteCristalReport.SubReportView
            Dim strParametros As String

            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion

            strParametros = Trim(txtOrdendeTrabajo.Text)

            Try
                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                'strParametros = Trim(txtOrdendeTrabajo.Text)
                With rptTiempo
                    .P_Filename = My.Resources.ResourceUI.rptNombreDocumentosXOrden
                    .P_BarraTitulo = My.Resources.ResourceUI.rptBarraTituloDocumentosXOrden
                    .P_WorkFolder = PATH_REPORTES
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

        Private Sub btncerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncerrar.Click
            Me.Close()
        End Sub

        Public Sub New(ByVal p_blnEstado As Boolean)

            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()


        End Sub


        Private Sub picRepuesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picRepuesto.Click
            CargarBuscador(sender)
        End Sub

        Private Sub CargarBuscador(ByVal sender As System.Object)

            Try

                With SubBOTs

                    Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorOrden

                    .Titulos = My.Resources.ResourceUI.NoOrden & "," & My.Resources.ResourceUI.Cotizacion & _
                    "," & My.Resources.ResourceUI.FechaApertura & "," & My.Resources.ResourceUI.CodCliente
                    '"Orden,Cotización,Fecha apertura,Cód. Cliente"

                    .Criterios = "TOP 100 NoOrden,NoCotizacion,Fecha_apertura,ClienteFacturar"
                    '.Criterios = "NoOrden,NoCotizacion,Fecha_apertura,ClienteFacturar"
                    .Tabla = "SCGTA_TB_Orden"
                    .Where = "1=1 ORDER BY NoVisita,NoCotizacion"
                    .ConsultarDBPorFiltrado = True
                    .Activar_Buscador(sender)


                End With

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try

        End Sub

        Private Sub SubBOTs_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles SubBOTs.AppAceptar
            txtOrdendeTrabajo.Text = Campo_Llave
        End Sub

    End Class
End Namespace