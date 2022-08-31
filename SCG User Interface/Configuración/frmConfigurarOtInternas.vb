Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface
    Public Class frmConfigurarOtInternas


#Region "Declareciones"
        Dim adpOTIternasAdapter As New DMSONEDKFramework.Conf_Ot_IternaDataAdapter
        Private WithEvents m_objBuscador As New Buscador.SubBuscador
        Public objSCGMSGBox As New Proyecto_SCGMSGBox.SCGMSGBox("DMS ONE")
        Private objDAConexion As DAConexion
        Private drwConfOTInterna As DMSONEDKFramework.Conf_Ot_IternaDataSet.SCGTA_TB_Conf_Ot_IternaRow

#End Region

#Region "Constructor"
        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            InitializeComponent()
        End Sub
#End Region

#Region "Eventos"

        Private Sub frmConfigurarOtInternas_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


            Try
                objDAConexion = New DAConexion
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Exportar).Visible = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Imprimir).Visible = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Buscar).Visible = False
                ScgToolBar1.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Visible = False
                dtgOtInternas.AllowUserToAddRows = False
                dtgOtInternas.AllowUserToDeleteRows = False
                dtgOtInternas.AllowUserToResizeRows = False

                m_adpTransacciones.Connection = objDAConexion.ObtieneConexion
                m_adpTransacciones.Fill(m_dtsTransacciones.SCGTA_VW_Tran_Comp)
                cboTransaccion.SelectedItem = Nothing
                cboTransaccion.SelectedItem = Nothing

                dstOTIntertasDataSet.SCGTA_TB_Conf_Ot_Iterna.Columns.Add("Descripcion")
                dstOTIntertasDataSet.SCGTA_TB_Conf_Ot_Iterna.Columns.Add("NombreCuenta")
                dtgOtInternas.DataSource = dstOTIntertasDataSet.SCGTA_TB_Conf_Ot_Iterna
                adpOTIternasAdapter.Fill(dstOTIntertasDataSet)
                CargarDescripcionesTipoOrdenes()
                CargarDescripcionesCuentasContables()
                EstiloGrid()

            Catch ex As Exception               
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, "SCG DMS ONE")
            End Try


        End Sub

        Private Sub m_objBuscador_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_objBuscador.AppAceptar
            Try
                Select Case sender.name
                    Case picBuscadorTiposOrdenes.Name
                        txtTipoOrden.Tag = Arreglo_Campos(0)
                        txtTipoOrden.Text = Arreglo_Campos(1)
                    Case piCuentasContables.Name
                        txtNumeroCuenta.Text = Arreglo_Campos(0)
                        txtNombreCuenta.Text = Arreglo_Campos(1)
                End Select
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, "SCG DMS ONE")
            End Try
        End Sub

#End Region

#Region "Metodos"

        Private Sub CargarBuscadorTiposOrdenes(ByVal sender As Object)
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscador = New Buscador.SubBuscador
                m_objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscador.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorTipoOrdenes
                m_objBuscador.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Descripcion
                m_objBuscador.Criterios = "CodTipoOrden, Descripcion"
                m_objBuscador.Tabla = "SCGTA_TB_TipoOrden"
                m_objBuscador.Activar_Buscador(sender)

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, "SCG DMS ONE")
            End Try

        End Sub

        Private Sub CargarBuscadorCuentasContables(ByVal sender As Object)
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscador = New Buscador.SubBuscador
                m_objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscador.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorCuentasContables
                m_objBuscador.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Descripcion
                m_objBuscador.Criterios = "ACCTCODE, ACCTNAME"
                m_objBuscador.Where = "postable = 'Y' and fixed = 'N'"
                m_objBuscador.Tabla = "SCGTA_VW_OACT"
                m_objBuscador.Activar_Buscador(sender)

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, "SCG DMS ONE")
            End Try

        End Sub

        Private Sub CargarDescripcionesTipoOrdenes()
            Dim intIndice As Integer

            For intIndice = 0 To dstOTIntertasDataSet.SCGTA_TB_Conf_Ot_Iterna.Count - 1
                dstOTIntertasDataSet.SCGTA_TB_Conf_Ot_Iterna.Rows(intIndice)("Descripcion") = Utilitarios.RetornaDescripcionOT(dstOTIntertasDataSet.SCGTA_TB_Conf_Ot_Iterna.Rows(intIndice)("ID_TIpo_OT"))
            Next

        End Sub

#End Region

        Private Sub picBuscadorTiposOrdenes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picBuscadorTiposOrdenes.Click
            CargarBuscadorTiposOrdenes(sender)
        End Sub

        Private Sub piCuentasContables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles piCuentasContables.Click
            CargarBuscadorCuentasContables(sender)
        End Sub

        Private Sub EstiloGrid()
            dtgOtInternas.Columns("ID_Conf_Ot_Interna").Visible = False
            dtgOtInternas.Columns("ID_Tipo_Ot").Visible = False
            dtgOtInternas.Columns("Descripcion").HeaderText = My.Resources.ResourceUI.TipoOrden
            dtgOtInternas.Columns("Numero_Cuenta_Contable").HeaderText = My.Resources.ResourceUI.CuentaContable
            dtgOtInternas.Columns("NombreCuenta").HeaderText = My.Resources.ResourceUI.NombreCuenta
            dtgOtInternas.Columns("NombreCuenta").Width = 120
            dtgOtInternas.Columns("Descripcion").DisplayIndex = 0
            dtgOtInternas.Columns("Descripcion").Width = 180
            dtgOtInternas.Columns("Numero_Cuenta_Contable").DisplayIndex = 1
            dtgOtInternas.Columns("NombreCuenta").DisplayIndex = 2
            dtgOtInternas.Columns(dstOTIntertasDataSet.SCGTA_TB_Conf_Ot_Iterna.Tran_CompColumn.ColumnName).Visible = False

            dstOTIntertasDataSet.SCGTA_TB_Conf_Ot_Iterna.Constraints.Add("IDTipoOT", dstOTIntertasDataSet.SCGTA_TB_Conf_Ot_Iterna.Columns("ID_Tipo_OT"), True)
        End Sub

        Private Sub dtgOtInternas_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgOtInternas.CellClick

            Dim strID As String

            Try


                txtTipoOrden.Tag = dtgOtInternas.Rows.Item(e.RowIndex).Cells.Item("ID_Tipo_Ot").Value
                txtTipoOrden.Text = dtgOtInternas.Rows.Item(e.RowIndex).Cells.Item("Descripcion").Value
                cboTransaccion.SelectedValue = dtgOtInternas.Rows.Item(e.RowIndex).Cells.Item("Tran_Comp").Value
                txtNumeroCuenta.Text = dtgOtInternas.Rows.Item(e.RowIndex).Cells.Item("Numero_Cuenta_Contable").Value
                txtNombreCuenta.Text = dtgOtInternas.Rows.Item(e.RowIndex).Cells.Item("NombreCuenta").Value
                strID = dtgOtInternas.Rows.Item(e.RowIndex).Cells.Item("ID_Conf_Ot_Interna").Value
                For Each drwConfOTInterna In dstOTIntertasDataSet.SCGTA_TB_Conf_Ot_Iterna.Rows
                    If drwConfOTInterna.ID_Conf_Ot_Interna = strID Then
                        Exit For
                    End If
                Next
                If Not drwConfOTInterna.ID_Conf_Ot_Interna = strID Then
                    drwConfOTInterna = Nothing
                End If
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, "SCG DMS ONE")

            End Try
        End Sub

        Private Sub dtgOtInternas_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dtgOtInternas.DataError
            objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeTipoOrdenYaConfigurada)
        End Sub

        Private Sub ScgToolBar1_Click_Cancelar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Cancelar

            Call LimpiarFormulario()

        End Sub

        Private Sub ScgToolBar1_Click_Eliminar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Eliminar
            If MessageBox.Show(My.Resources.ResourceUI.PreguntaDeseaEliminarRegistro, My.Resources.ResourceUI.TituloEliminarregistro, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                dstOTIntertasDataSet.SCGTA_TB_Conf_Ot_Iterna.Rows(dtgOtInternas.CurrentRow.Index).Delete()
                adpOTIternasAdapter.Update(dstOTIntertasDataSet)
                Call LimpiarFormulario()
            End If
        End Sub

        Private Sub ScgToolBar1_Click_Guardar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Guardar
            Try
                Dim strTransaccion As String = ""
                If cboTransaccion.SelectedValue IsNot Nothing Then
                    strTransaccion = cboTransaccion.SelectedValue
                End If

                If Trim(txtNombreCuenta.Text) <> String.Empty And Trim(txtNumeroCuenta.Text) <> String.Empty And Trim(txtTipoOrden.Text) <> String.Empty Then
                    If drwConfOTInterna Is Nothing Then
                        dstOTIntertasDataSet.SCGTA_TB_Conf_Ot_Iterna.AddSCGTA_TB_Conf_Ot_IternaRow(txtTipoOrden.Tag, txtNumeroCuenta.Text, strTransaccion)
                    Else
                        drwConfOTInterna.ID_Tipo_Ot = txtTipoOrden.Tag
                        drwConfOTInterna.Numero_Cuenta_Contable = txtNumeroCuenta.Text
                        If Not String.IsNullOrEmpty(strTransaccion) Then
                            drwConfOTInterna.Tran_Comp = strTransaccion
                        End If
                    End If
                    CargarDescripcionesTipoOrdenes()

                    txtNombreCuenta.Clear()
                    txtNumeroCuenta.Clear()
                    txtTipoOrden.Clear()

                    adpOTIternasAdapter.Update(dstOTIntertasDataSet)
                    CargarDescripcionesCuentasContables()
                    Call LimpiarFormulario()
                End If


            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeTipoOrdenYaConfigurada)
            End Try

        End Sub

        Private Sub ScgToolBar1_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.ButtonClick

        End Sub

        Private Sub ScgToolBar1_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Cerrar
            Me.Close()
        End Sub

        Private Sub CargarDescripcionesCuentasContables()
            Dim intIndice As Integer

            For intIndice = 0 To dstOTIntertasDataSet.SCGTA_TB_Conf_Ot_Iterna.Count - 1
                dstOTIntertasDataSet.SCGTA_TB_Conf_Ot_Iterna.Rows(intIndice)("NombreCuenta") = Utilitarios.RetornaDescripcionCuentaContable(dstOTIntertasDataSet.SCGTA_TB_Conf_Ot_Iterna.Rows(intIndice)("Numero_Cuenta_Contable"))
            Next

        End Sub

        Private Sub LimpiarFormulario()
            txtNombreCuenta.Clear()
            txtNumeroCuenta.Clear()
            txtTipoOrden.Clear()
            cboTransaccion.SelectedItem = Nothing
            cboTransaccion.SelectedItem = Nothing
            drwConfOTInterna = Nothing
        End Sub

    End Class
End Namespace