Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmReporteHistorialResumido

        Private WithEvents m_buOrdenes As New Buscador.SubBuscador


        Private Sub btncerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncerrar.Click
            Me.Close()
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
                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorVehiculos

                    .Titulos = My.Resources.ResourceUI.ID & "," & My.Resources.ResourceUI.Unidad & _
                    "," & My.Resources.ResourceUI.Placa & "," & My.Resources.ResourceUI.Propietario & _
                    "," & My.Resources.ResourceUI.VIN & "," & My.Resources.ResourceUI.Marca & _
                    "," & My.Resources.ResourceUI.Estilo & "," & My.Resources.ResourceUI.Modelo & _
                    "," & My.Resources.ResourceUI.Año

                    '"ID,Número Unidad, Placa ,Propietario,VIN,Marca,Estilo,Modelo,Año"
                    .Criterios = "TOP 100 IDVehiculo,NoVehiculo,Placa,Cliente,VIN,DescMarca,DescEstilo,DescModelo,AnoVehiculo"
                    .Tabla = "SCGTA_VW_Vehiculos"
                    .Where = ""
                    .ConsultarDBPorFiltrado = True
                    .Criterios_OcultosEx = "1"
                    .Activar_Buscador(sender)

                End With
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
            CargaReporte()
        End Sub


        Private Sub picRepuesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picRepuesto.Click
            CargarBuscador(sender)
        End Sub

        Private Sub m_buOrdenes_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_buOrdenes.AppAceptar
            txtPlaca.Tag = Campo_Llave
            txtPlaca.Text = Arreglo_Campos(2)
            txtMarca.Text = Arreglo_Campos(5)
            txtEstilo.Text = Arreglo_Campos(6)
            txtModelo.Text = Arreglo_Campos(7)
            txtUnidad.Text = Arreglo_Campos(1)
        End Sub


        Public Overridable Sub CargaReporte()
            Dim rptTiempo As New ComponenteCristalReport.SubReportView

            Dim strParametros As String = ""

            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion

            Try
                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                With rptTiempo
                    .P_BarraTitulo = My.Resources.ResourceUI.rptBarraTituloHistorialResumido
                    .P_WorkFolder = PATH_REPORTES

                    If rbtResumido.Checked = True Then
                        .P_Filename = My.Resources.ResourceUI.rptNombreHistorialResumido
                        strParametros = txtUnidad.Text
                        .P_DataBase = strDATABASESCG
                    Else
                        .P_Filename = My.Resources.ResourceUI.rptNombreHistorialDetallado
                        strParametros = txtUnidad.Text
                        .P_DataBase = strDATABASE
                    End If

                    .P_Server = Server
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

    End Class
End Namespace
