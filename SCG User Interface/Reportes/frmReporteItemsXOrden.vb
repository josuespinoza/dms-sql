Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface
    Public Class frmReporteItemsXOrden

        Private Sub btncerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncerrar.Click
            Me.Close()
        End Sub


        Public Overridable Sub CargaReporte()

            Dim rptTiempo As New ComponenteCristalReport.SubReportView

            Dim strParametros As String = ""

            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion

            Try
                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                strParametros = txtNumeroOT.Text & ","

                If chkRepuestos.Checked = True Then
                    strParametros = strParametros & "1" & ","
                Else
                    strParametros = strParametros & "-1" & ","
                End If

                If chkServicios.Checked = True Then
                    strParametros = strParametros & "2" & ","
                Else
                    strParametros = strParametros & "-1" & ","
                End If

                If chkSuminstros.Checked = True Then
                    strParametros = strParametros & "3" & ","
                Else
                    strParametros = strParametros & "-1" & ","
                End If

                If chkServiciosExternos.Checked = True Then
                    strParametros = strParametros & "4" & ","
                Else
                    strParametros = strParametros & "-1" & ","
                End If

                If chkPaquetes.Checked = True Then
                    strParametros = strParametros & "5" & ","
                Else
                    strParametros = strParametros & "-1" & ","
                End If

                If chkOtros.Checked = True Then
                    strParametros = strParametros & "6"
                Else
                    strParametros = strParametros & "-1"
                End If


                With rptTiempo
                    .P_BarraTitulo = My.Resources.ResourceUI.rptBarraTituloItemsPorOrden
                    .P_WorkFolder = PATH_REPORTES
                    .P_Filename = My.Resources.ResourceUI.rptNombreItemsXOrden
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
        End Sub

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()
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
            txtNumeroOT.Text = Campo_Llave
        End Sub

        Private Sub picRepuesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picRepuesto.Click
            CargarBuscador(sender)
        End Sub
    End Class
End Namespace