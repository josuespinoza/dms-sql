Option Explicit On
Option Strict On

Imports System.Data.SqlClient

Namespace LlamadaServicio
    Public Class DatosVehiculo
        Public IdVehiculo As String
        Public CodMarca As String
        Public DescMarca As String
        Public CodEstilo As String
        Public DescEstilo As String
        Public CodModelo As String
        Public DescModelo As String
        Public Vin As String
        Public NoUnidad As String
        Public Placa As String
        Public CodTipo As String
        Public Tipo As String
        Public CodColor As String
        Public Color As String

        Public Sub CargaDesdeSBO(ByVal code As String, ByVal cadenaConexion As String)
            Dim conexion As SqlConnection = Nothing
            Try
                conexion = New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim consulta As String = "SELECT" & _
                                         "[@SCGD_VEHICULO].Code," & _
                                         "[@SCGD_VEHICULO].U_Cod_Unid AS NoUnidad," & _
                                         "[@SCGD_VEHICULO].U_Cod_Marc AS CodMarca," & _
                                         "[@SCGD_VEHICULO].U_Cod_Mode AS CodModelo," & _
                                         "[@SCGD_VEHICULO].U_Des_Mode AS Modelo," & _
                                         "[@SCGD_VEHICULO].U_Cod_Esti AS CodEstilo," & _
                                         "[@SCGD_VEHICULO].U_Des_Esti AS Estilo," & _
                                         "[@SCGD_VEHICULO].U_Ano_Vehi AS Ano," & _
                                         "[@SCGD_VEHICULO].U_Num_Plac AS Placa," & _
                                         "[@SCGD_VEHICULO].U_Cod_Col AS CodColor," & _
                                         "[@SCGD_VEHICULO].U_Num_VIN AS Vin," & _
                                         "[@SCGD_VEHICULO].U_Tipo AS CodTipo," & _
                                         "[@SCGD_VEHICULO].U_Des_Marc AS Marca," & _
                                         "[@SCGD_COLOR].Name AS Color," & _
                                         "[@SCGD_TIPOVEHICULO].Name AS Tipo" & _
                                         " FROM    [@SCGD_VEHICULO]" & _
                                         " INNER JOIN [@SCGD_TIPOVEHICULO] ON [@SCGD_VEHICULO].U_Tipo = [@SCGD_TIPOVEHICULO].Code" & _
                                         " LEFT OUTER JOIN [@SCGD_COLOR] ON [@SCGD_VEHICULO].U_Cod_Col = [@SCGD_COLOR].Code" & _
                                         " WHERE [@SCGD_VEHICULO].Code = '" & code & "'"

                Dim drdResultadoConsulta As SqlClient.SqlDataReader
                Dim command As New SqlClient.SqlCommand

                command.Connection = conexion
                command.CommandType = CommandType.Text
                command.CommandText = consulta
                drdResultadoConsulta = command.ExecuteReader()

                Me.IdVehiculo = code
                If (drdResultadoConsulta.HasRows) Then
                    drdResultadoConsulta.Read()
                    LeeColumna(drdResultadoConsulta, "CodMarca", Me.CodMarca)
                    LeeColumna(drdResultadoConsulta, "Marca", Me.DescMarca)
                    LeeColumna(drdResultadoConsulta, "CodEstilo", Me.CodEstilo)
                    LeeColumna(drdResultadoConsulta, "Estilo", Me.DescEstilo)
                    LeeColumna(drdResultadoConsulta, "CodModelo", Me.CodModelo)
                    LeeColumna(drdResultadoConsulta, "Modelo", Me.DescModelo)
                    LeeColumna(drdResultadoConsulta, "Vin", Me.Vin)
                    LeeColumna(drdResultadoConsulta, "Placa", Me.Placa)
                    LeeColumna(drdResultadoConsulta, "CodTipo", Me.CodTipo)
                    LeeColumna(drdResultadoConsulta, "NoUnidad", Me.NoUnidad)
                    LeeColumna(drdResultadoConsulta, "Tipo", Me.Tipo)
                    LeeColumna(drdResultadoConsulta, "CodColor", Me.CodColor)
                    LeeColumna(drdResultadoConsulta, "Color", Me.Color)
                End If
            Catch
                Throw
            Finally
               If Not conexion is Nothing then conexion.Close()
            End Try
        End Sub

        Private Sub LeeColumna(ByVal dataReader As SqlDataReader, ByVal columna As String, ByRef valor As String)
            valor = String.Empty
            If dataReader IsNot Nothing Then
                If dataReader.Item(columna) IsNot DBNull.Value Then valor = dataReader.Item(columna).ToString()
            End If
        End Sub

    End Class
End Namespace