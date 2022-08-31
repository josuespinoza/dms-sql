Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmRptOrdenGarantia

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)

            MyBase.New()

            InitializeComponent()

        End Sub

#End Region

#Region "Procedimientos"

        Private Sub CargarBuscador(ByVal sender As System.Object)

            Try

                With SubBOTs

                    Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorOrden

                    .Titulos = My.Resources.ResourceUI.NoOrden & "," & My.Resources.ResourceUI.Cotizacion & _
                    "," & My.Resources.ResourceUI.FechaApertura & "," & My.Resources.ResourceUI.Cliente
                    '"Orden,Cotización,Fecha apertura,Cód. Cliente"

                    .Criterios = "TOP 100 NoOrden,NoCotizacion,Fecha_apertura,ClienteFacturar"
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

#End Region

#Region "Eventos"

        Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
            Me.Close()
        End Sub

        Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
            Dim strParametros As String = ""

            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion

            Try
                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                strParametros = strParametros & txtNoOrden.Text

                With SubReportsOrdenes

                    If optFord.Checked Then
                        .P_BarraTitulo = My.Resources.ResourceUI.repBarraTituloOrdenesTrabajoGarantia
                        .P_Filename = My.Resources.ResourceUI.rptNombreOTGarantiaNASA
                    Else
                        .P_BarraTitulo = "<SCG> Ordenes de Trabajo - Garantía - VW"
                        .P_Filename = "rptOrdenTrabajoGarantiaAutomotriz.rpt"
                    End If
                    .P_WorkFolder = PATH_REPORTES
                    .P_Server = Server
                    .P_DataBase = strDATABASESCG
                    .P_User = UserSCGInternal
                    .P_Password = Password
                    .P_ParArray = strParametros
                End With

                SubReportsOrdenes.VerReporte()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub picRepuesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picRepuesto.Click

            Try

                Call CargarBuscador(sender)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub SubBOTs_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles SubBOTs.AppAceptar

            txtNoOrden.Text = Campo_Llave

        End Sub

#End Region

    End Class

End Namespace

