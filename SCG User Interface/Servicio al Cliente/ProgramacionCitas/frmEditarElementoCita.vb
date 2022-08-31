Option Explicit On
Option Strict On

Public Class frmEditarElementoCita

    Private _vehiculo As Vehiculo

    Public Property Vehiculo() As Vehiculo
        Get
            Return _vehiculo
        End Get
        Set(ByVal value As Vehiculo)
            _vehiculo = value
        End Set
    End Property

    Public Sub CargaPropiedades()

        textBoxMarca.Text = _vehiculo.DescMarca
        textBoxEstilo.Text = _vehiculo.DescEstilo
        textBoxModelo.Text = _vehiculo.DescModelo
        textBoxCliente.Text = _vehiculo.CardName
        textBoxVin.Text = _vehiculo.Vin

        If _vehiculo.FechaProximoServicio.HasValue Then
            dateTimePickerFechaPS.Value = _vehiculo.FechaProximoServicio.Value
'            dateTimePickerFechaPS.Enabled = True
'        Else
'            dateTimePickerFechaPS.Enabled = False
        End If

        If _vehiculo.FechaUltimoServicio.HasValue Then
            dateTimePickerFechaUS.Value = _vehiculo.FechaUltimoServicio.Value
'            dateTimePickerFechaUS.Enabled = True
'        Else
'            dateTimePickerFechaUS.Enabled = False
        End If

    End Sub

    Private Sub frmEditarElementoCita_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click
        Vehiculo.FechaProximoServicio = dateTimePickerFechaPS.Value
    End Sub
End Class