Imports System.Collections.Generic
Imports DMSOneFramework
Imports DMSOneFramework.PresupuestosDataSetTableAdapters

Namespace Ventas

    Public Class ManejadorMarcasPresupuestos
        Private _conexion As String

        Public Sub New(ByVal conexion As String)
            _conexion = conexion
        End Sub

        Public Function CargaMarcasPresupuestos() As Dictionary(Of String, MarcasPresupuestos)
            Dim marcasPresupuestoDataAdapater As MarcasPresupuestoTableAdapter = New MarcasPresupuestoTableAdapter()
            Dim configPrespDataSet As PresupuestosDataSet = New PresupuestosDataSet()

            marcasPresupuestoDataAdapater.CadenaConexion = _conexion
            marcasPresupuestoDataAdapater.Fill(configPrespDataSet.MarcasPresupuesto)
            CargaMarcasPresupuestos = New Dictionary(Of String, MarcasPresupuestos)(configPrespDataSet.ConfiguracionesPresupuestos.Rows.Count)
            For Each configuracionesPresupuestosRow As PresupuestosDataSet.MarcasPresupuestoRow In configPrespDataSet.MarcasPresupuesto
                With configuracionesPresupuestosRow
                    CargaMarcasPresupuestos.Add(.Code, New MarcasPresupuestos(.Code, .Name))
                End With
            Next
        End Function
    End Class
End Namespace