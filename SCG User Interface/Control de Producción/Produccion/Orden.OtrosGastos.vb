Imports DMSOneFramework
Imports DMSOneFramework.OtrosGastosDataSetTableAdapters
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Partial Class frmOrden
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

        Private m_adpOtrosGastosResumido As SCGTA_VW_OtrosGastosResumidoTableAdapter = Nothing
        Private m_adpOtrosGastos As SCGTA_VW_OtrosGastosTableAdapter = Nothing
        Private Delegate Sub Back(f as FlowLayoutPanel)

        Private Sub CargaOtrosGastos()

            If m_adpOtrosGastos Is Nothing Then
                m_adpOtrosGastos = New SCGTA_VW_OtrosGastosTableAdapter()
                m_adpOtrosGastos.CadenaConexion = GlobalesDA.strConexionADO
'                m_adpOtrosGastos.FillByNumOT(m_dstOtrosGastos.SCGTA_VW_OtrosGastos, m_drdOrdenCurrent.NoOrden)

                m_adpOtrosGastosResumido = New SCGTA_VW_OtrosGastosResumidoTableAdapter()
                m_adpOtrosGastosResumido.CadenaConexion = GlobalesDA.strConexionADO

'                m_adpOtrosGastosResumido.FillByNumOT(m_dstOtrosGastos.SCGTA_VW_OtrosGastosResumido, m_drdOrdenCurrent.NoOrden)
                FlowLayoutPanel1.Controls.Clear()
                CargaSec(FlowLayoutPanel1)
'                Dim b As Back = New Back(AddressOf CargaSec)
'                Dim ar As IAsyncResult = b.BeginInvoke(FlowLayoutPanel1,AddressOf FinCargaSec, Nothing)
'                b.EndInvoke(ar)
            End If
        End Sub

        Public Sub CargaSec(f as FlowLayoutPanel)
            m_adpOtrosGastos.FillByNumOT(m_dstOtrosGastos.SCGTA_VW_OtrosGastos, m_drdOrdenCurrent.NoOrden)
            m_adpOtrosGastosResumido.FillByNumOT(m_dstOtrosGastos.SCGTA_VW_OtrosGastosResumido, m_drdOrdenCurrent.NoOrden)

            Dim sum As Double = 0
            For Each row As OtrosGastosDataSet.SCGTA_VW_OtrosGastosResumidoRow In m_dstOtrosGastos.SCGTA_VW_OtrosGastosResumido
                Dim t As DetalleOtrosGastos = New DetalleOtrosGastos(row.Concepto, m_dstOtrosGastos)
                t.Width = f.Width - 30
                f.Controls.Add(t)
                sum += row.Gasto
            Next
            txtTotalOtrosGastos.Text = String.Format("{0:0.00}", sum)
        End Sub

        Public Sub FinCargaSec(ByVal ar As IAsyncResult)
        End Sub

        Private Sub btnActualizarOtrosGastos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnActualizarOtrosGastos.Click
            FlowLayoutPanel1.Controls.Clear()
            CargaSec(FlowLayoutPanel1)
        End Sub
    End Class

End Namespace