Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmUnidadesTiempo
#Region "Declaraciones"

        Dim adpUnidadesTiempoAdapter As New DMSONEDKFramework.UnidadTiempoDataAdapter

#End Region


#Region "Constructor"
        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub
#End Region

#Region "Eventos"

        Private Sub frmUnidadesTiempo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'Se ocultan los botones del toolbar que no se van utilizar
            tlbUnidadesTiempo.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Exportar).Visible = False
            tlbUnidadesTiempo.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Imprimir).Visible = False
            tlbUnidadesTiempo.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Buscar).Visible = False
            tlbUnidadesTiempo.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cancelar).Visible = True
            tlbUnidadesTiempo.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = True
            tlbUnidadesTiempo.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = False

            adpUnidadesTiempoAdapter.Fill(dstUnidadesTiempoDataSet)
            dtgUnidadesTiempo.DataSource = dstUnidadesTiempoDataSet.SCGTA_TB_UnidadTiempo

        End Sub

        Private Sub tlbUnidadesTiempo_Click_Cancelar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbUnidadesTiempo.Click_Cancelar
            Try
                txtDescripcion.Clear()
                txtTiempo.Clear()
                txtDescripcion.ReadOnly = True
                txtTiempo.ReadOnly = True
                dstUnidadesTiempoDataSet.RejectChanges()
                'adpUnidadesTiempoAdapter.Fill(dstUnidadesTiempoDataSet)
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try

        End Sub

        Private Sub tlbUnidadesTiempo_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbUnidadesTiempo.Click_Cerrar
            dstUnidadesTiempoDataSet.RejectChanges()
            Me.Close()
        End Sub

        Private Sub tlbUnidadesTiempo_Click_Eliminar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbUnidadesTiempo.Click_Eliminar
            If dstUnidadesTiempoDataSet.SCGTA_TB_UnidadTiempo.Rows(dtgUnidadesTiempo.CurrentRow.Index)("CodigoUnidadTiempo") <> g_intUnidadTiempo Then
                dstUnidadesTiempoDataSet.SCGTA_TB_UnidadTiempo.Rows(dtgUnidadesTiempo.CurrentRow.Index).Delete()
                'tlbUnidadesTiempo.Buttons(tlbUnidadesTiempo.enumButton.Guardar).Enabled = True
                adpUnidadesTiempoAdapter.Update(dstUnidadesTiempoDataSet)
            Else
                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajenoPuedeEliminarUnidadTiempoPredefinida)

            End If

        End Sub

        Private Sub tlbUnidadesTiempo_Click_Guardar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbUnidadesTiempo.Click_Guardar
            Agregar_Unidad()
            adpUnidadesTiempoAdapter.Update(dstUnidadesTiempoDataSet)
        End Sub

        Private Sub tlbUnidadesTiempo_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbUnidadesTiempo.Click_Nuevo
            txtDescripcion.ReadOnly = False
            txtTiempo.ReadOnly = False
        End Sub

        Private Sub txtTiempo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTiempo.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then
                Agregar_Unidad()
                adpUnidadesTiempoAdapter.Update(dstUnidadesTiempoDataSet)
            End If
        End Sub

        Private Sub txtDescripcion_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDescripcion.KeyPress
            If Asc(e.KeyChar) = Keys.Enter Then
                Agregar_Unidad()
                txtDescripcion.ReadOnly = True
                txtTiempo.ReadOnly = True
            End If
        End Sub

        Private Sub txtTiempo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTiempo.TextChanged

        End Sub

        Private Sub dtgUnidadesTiempo_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgUnidadesTiempo.RowEnter
            tlbUnidadesTiempo.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Enabled = True
        End Sub
#End Region

#Region "Metodos"
        Sub Agregar_Unidad()

            If Trim(txtDescripcion.Text) = "" Or Trim(txtTiempo.Text) = "" Then

                'SCGExceptionHandler.clsExceptionHandler.mostrarErrorCustomizado(My.Resources.ResourceUI.MensajeDebeCompletarCampos, "SCG DMS ONE")
            Else
                Dim strDescripcion As String
                Dim dblMinutos As Double
                strDescripcion = CStr(Trim(txtDescripcion.Text))
                dblMinutos = CDbl(Trim(txtTiempo.Text))
                dstUnidadesTiempoDataSet.SCGTA_TB_UnidadTiempo.AddSCGTA_TB_UnidadTiempoRow(strDescripcion, dblMinutos)
                txtDescripcion.Text = String.Empty
                txtTiempo.Text = String.Empty
            End If

        End Sub
#End Region

    End Class
End Namespace
