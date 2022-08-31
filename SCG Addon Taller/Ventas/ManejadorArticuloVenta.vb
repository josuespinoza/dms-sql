Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports DMSOneFramework.ConfiguracionArticuloVentaTableAdapters
Imports DMSOneFramework.ConfiguracionArticuloVenta

Namespace Ventas

    Public Class ManejadorArticuloVenta

        Private _conexion As String

        Public Sub New(ByVal conexion As String)
            _conexion = conexion
        End Sub

        Public Function CargaConfiguraciones() As List(Of ConfiguracionArticuloVenta)
            Dim configuracionesAdapter As ConfiguracionesArticuloVentaTableAdapter = New ConfiguracionesArticuloVentaTableAdapter()
            Dim configuracionesArticuloVentaDataTable As ConfiguracionesArticuloVentaDataTable
            Dim configuracionesArticuloVenta As List(Of ConfiguracionArticuloVenta)

            configuracionesAdapter.CadenaConexion = _conexion
            configuracionesArticuloVentaDataTable = configuracionesAdapter.GetData()
            configuracionesArticuloVenta = New List(Of ConfiguracionArticuloVenta)(configuracionesArticuloVentaDataTable.Rows.Count)

            For Each configuracionesArticuloVentaRow As ConfiguracionesArticuloVentaRow In configuracionesArticuloVentaDataTable
                Dim conf As ConfiguracionArticuloVenta = New ConfiguracionArticuloVenta(configuracionesArticuloVentaRow.Code, configuracionesArticuloVentaRow.Name, configuracionesArticuloVentaRow.ArticuloVenta)
                configuracionesArticuloVenta.Add(conf)
            Next

            Return configuracionesArticuloVenta
        End Function
    End Class

End Namespace