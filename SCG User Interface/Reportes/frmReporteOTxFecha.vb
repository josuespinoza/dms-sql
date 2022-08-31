Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmReporteOTxFecha

        Private WithEvents m_buOrdenes As New Buscador.SubBuscador

        Public Sub New(ByVal p_blnEstado As Boolean)

            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()


        End Sub

        Private Sub frmReporteOTxFecha_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            dtpDesde.Value = Today.Date
            dtpHasta.Value = Today.Date
        End Sub

        Private Sub btncerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncerrar.Click
            Me.Close()
        End Sub

        Private Sub btnCargar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCargar.Click

            Dim strParametros As String = ""
            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion
            Try

                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                If Trim(txtIDMarca.Text) = "" Then
                    strParametros = "-2,"
                Else
                    strParametros = Trim(txtIDMarca.Text) & ","
                End If

                If Trim(txtDescripcionOrden.Text) = "" Then
                    strParametros = strParametros & "-2,"
                Else
                    strParametros = strParametros & Trim(txtDescripcionOrden.Text) & ","
                End If

                strParametros = strParametros & dtpDesde.Value.ToString & "," 'CDate(Format(dtpDesde.Value, "yyyyMMdd") & " 00:00:00.000" & "
                strParametros = strParametros & DateAdd(DateInterval.Second, -1, DateAdd(DateInterval.Day, 1, dtpHasta.Value)).ToString

                With rptReporte
                    .P_BarraTitulo = My.Resources.ResourceUI.rptBarraTituloListadoOrdenesTrabajo
                    .P_WorkFolder = PATH_REPORTES

                    If chkSolosinFacturar.Checked = False Then
                        .P_Filename = My.Resources.ResourceUI.rptNombreOTXFecha
                    Else
                        .P_Filename = My.Resources.ResourceUI.rptNombreOTSinFacturar
                    End If

                    .P_Server = Server
                    .P_DataBase = strDATABASESCG
                    .P_CompanyName = COMPANIA
                    .P_User = UserSCGInternal
                    .P_Password = Password
                    .P_ParArray = strParametros


                End With

                rptReporte.VerReporte()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
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


        Private Sub CargarBuscadorMarcas(ByVal sender As System.Object)
            Try
                With m_buOrdenes

                    'Me.Cursor = Cursors.WaitCursor
                    Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTitulosBuscadorMarcas
                    .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Descripcion
                    .Criterios = "code, name"
                    .Tabla = " SCGTA_VW_Marcas"
                    .Where = ""
                    .Activar_Buscador(sender)

                End With
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub Label6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label6.Click

        End Sub

        Private Sub picRepuesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picTipoOT.Click
            CargarBuscador(sender)
        End Sub

        Private Sub m_buOrdenes_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_buOrdenes.AppAceptar
            Select Case CType(sender, System.Windows.Forms.PictureBox).Name
                Case "picTipoOT"
                    txtIdOrden.Text = Campo_Llave
                    txtDescripcionOrden.Text = Arreglo_Campos(1)
                Case "picMarca"
                    txtIDMarca.Text = Campo_Llave
                    txtDescripcionMarca.Text = Arreglo_Campos(1)
            End Select
        End Sub



        Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picMarca.Click
            CargarBuscadorMarcas(sender)
        End Sub
    End Class
End Namespace