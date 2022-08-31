Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports DMSOneFramework
Imports DMSOneFramework.PresupuestosDataSet
Imports DMSOneFramework.PresupuestosDataSetTableAdapters

Namespace Ventas
    Public Class ManejadorClasificacionPresupuesto
        Private _conexion As String

        Public Sub New(ByVal conexion As String)
            _conexion = conexion
        End Sub

        Public Function CargaClasificacion(ByVal query As String) As List(Of ClasificacionPresupuesto)
            Dim dataReader As SqlDataReader
            Dim conexionSql As SqlConnection
            Dim command As SqlCommand

            conexionSql = New SqlConnection(_conexion)
            Try
                conexionSql.Open()
                command = New SqlCommand(query, conexionSql)
                command.CommandType = CommandType.Text
                dataReader = command.ExecuteReader()
                CargaClasificacion = New List(Of ClasificacionPresupuesto)()

                Dim clasif As ClasificacionPresupuesto
                While dataReader.Read
                    If Not dataReader.IsDBNull(0) AndAlso Not dataReader.IsDBNull(1) Then
                        clasif = New ClasificacionPresupuesto(dataReader.Item(0).ToString(), dataReader.Item(1).ToString())
                        CargaClasificacion.Add(clasif)
                    End If
                End While
                If (dataReader IsNot Nothing) Then dataReader.Close()
            Catch
                Return Nothing
            Finally
                conexionSql.Close()
            End Try

        End Function

        Public Function CargaConfiguracionesClasificacion1() As Dictionary(Of String, ConfiguracionPresupuesto)
            Dim configPrespAdapter As ConfiguracionesPresupuestosTableAdapter = New ConfiguracionesPresupuestosTableAdapter()
            Dim configPrespDataSet As PresupuestosDataSet = New PresupuestosDataSet()

            configPrespAdapter.CadenaConexion = _conexion
            configPrespAdapter.Fill(configPrespDataSet.ConfiguracionesPresupuestos)
            CargaConfiguracionesClasificacion1 = New Dictionary(Of String, ConfiguracionPresupuesto)(configPrespDataSet.ConfiguracionesPresupuestos.Rows.Count)
            For Each configuracionesPresupuestosRow As ConfiguracionesPresupuestosRow In configPrespDataSet.ConfiguracionesPresupuestos
                If Not configuracionesPresupuestosRow.IsU_QueryNull Then
                    With configuracionesPresupuestosRow
                        CargaConfiguracionesClasificacion1.Add(.Code, New ConfiguracionPresupuesto(.Code, .Name, .U_Query))
                    End With
                End If
            Next
        End Function
    End Class
End Namespace