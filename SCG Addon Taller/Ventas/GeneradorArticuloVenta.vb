Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports DMSOneFramework.ConfiguracionArticuloVentaTableAdapters
Imports DMSOneFramework.ConfiguracionArticuloVenta
Imports DMS_Addon.LlamadaServicio
Imports System.Data.SqlClient

Namespace Ventas

    Public Class GeneradorArticuloVenta

        Private _conexion As String
        Private _conexionSql As SqlConnection
        Private _configuracionesArticuloVenta As SortedList(Of Integer, ConfiguracionArticuloVenta)

        Public Sub New(ByVal conexion As String)
            _conexion = conexion
        End Sub

        Private Sub CargaConfiguraciones()
            Dim configuracionesAdapter As ConfiguracionesArticuloVentaTableAdapter = New ConfiguracionesArticuloVentaTableAdapter()
            Dim configuracionesArticuloVentaDataTable As ConfiguracionesArticuloVentaDataTable

            configuracionesAdapter.CadenaConexion = _conexion
            configuracionesArticuloVentaDataTable = configuracionesAdapter.GetData()
            _configuracionesArticuloVenta = New SortedList(Of Integer, ConfiguracionArticuloVenta)(configuracionesArticuloVentaDataTable.Rows.Count)

            For Each configuracionesArticuloVentaRow As ConfiguracionesArticuloVentaRow In configuracionesArticuloVentaDataTable
'                Dim conf As ConfiguracionArticuloVenta = New ConfiguracionArticuloVenta(configuracionesArticuloVentaRow.Code)
'
'                With configuracionesArticuloVentaRow
'
'                    conf.ArticuloVenta = .ArticuloVenta
'                    conf.Estilo = .Estilo
'                    conf.Marca = .Marca
'                    conf.Modelo = .Modelo
'                    conf.Name = .Name
'                    conf.Prioridad = .Prioridad
'                    conf.TipoVehiculo = .TipoVehiculo
'
'                End With
'
'                _configuracionesArticuloVenta.Add(conf.Prioridad, conf)

            Next

        End Sub

        Public Function ObtieneArticuloVenta(ByVal vehiculo As DatosVehiculo) As String
            CargaConfiguraciones()
            For Each configuraciones As KeyValuePair(Of Integer, ConfiguracionArticuloVenta) In _configuracionesArticuloVenta
'                If configuraciones.Value.IsMatch(vehiculo) Then Return configuraciones.Value.ArticuloVenta
            Next

            Return String.Empty

        End Function

    End Class

End Namespace