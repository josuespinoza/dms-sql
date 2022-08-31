Imports DMSOneFramework
Imports DMSOneFramework.OtrosGastosDataSetTableAdapters

Public Class DetalleOtrosGastos

    Private _concepto As String
    Private _numOt As String
    Private _resumido As Boolean = True

'    Public Sub New(ByVal concepto As String, ByVal numOt As String, ByVal dstOtrosGastos As OtrosGastosDataSet)
    Public Sub New(ByVal concepto As String, ByVal dstOtrosGastos As OtrosGastosDataSet)
        InitializeComponent()
        Me._concepto = concepto
        '        Me._numOt = numOt

        m_bsrcOtrosGastos.DataSource = dstOtrosGastos
        m_bsrcOtrosGastos.DataMember = dstOtrosGastos.SCGTA_VW_OtrosGastos.TableName
        m_bsrcOtrosGastos.Filter = String.Format("Concepto = '{0}'", concepto)
        txtConcepto.Text = concepto
        ActualizaTotal()
    End Sub

    Private Sub DetalleOtrosGastos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub ActualizaTotal()
        Dim sum As Double = 0
        Dim otrosGastosRow As OtrosGastosDataSet.SCGTA_VW_OtrosGastosRow
        For Each rv As DataRowView In m_bsrcOtrosGastos
            otrosGastosRow = DirectCast(rv.Row, OtrosGastosDataSet.SCGTA_VW_OtrosGastosRow)
            sum += otrosGastosRow.PrecioConDescuento
        Next
        txtTotal.Text = String.Format("{0:0.00}", sum)
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        If (_resumido) Then
            _resumido = False
            PictureBox1.Image = My.Resources.triangulo2
            Me.Height = DataGridView1.Bottom + 5
            DataGridView1.Visible = True
        Else
            _resumido = True
            PictureBox1.Image = My.Resources.triangulo1
            Me.Height = txtConcepto.Bottom + 5
            DataGridView1.Visible = False
        End If
    End Sub
End Class
