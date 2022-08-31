Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq

Public Class ConfiguracionColores
    Private ColoresRazonCita As Dictionary(Of String, String)
    Private ColoresEstadoCita As Dictionary(Of String, ColorPorEstado)

    Public Enum TipoColor
        Almuerzo = 0
        Bloqueo = 1
        OrdenTrabajo = 2
        SuspensionHorario = 3
        Cita = 4
        Reprogramada = 5
        ServicioNoIniciado = 6
    End Enum

    Sub New()
        Try
            CargarConfiguracionColores()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub CargarConfiguracionColores()
        Dim Recordset As SAPbobsCOM.Recordset
        Dim QueryRazones As String = "SELECT T0.U_RazonCita, T0.U_Color FROM ""@SCGD_COLORESAGENDA"" T0 "
        Dim QueryEstados As String = "SELECT T0.Code, T0.U_Color, T0.U_ColorOT FROM ""@SCGD_CITA_ESTADOS"" T0 "
        Try
            ColoresRazonCita = New Dictionary(Of String, String)
            ColoresEstadoCita = New Dictionary(Of String, ColorPorEstado)
            Recordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Recordset.DoQuery(QueryRazones)
            If Recordset.RecordCount > 0 Then
                While Not Recordset.EoF
                    If Not ColoresRazonCita.ContainsKey(Recordset.Fields.Item("U_RazonCita").Value.ToString()) Then
                        ColoresRazonCita.Add(Recordset.Fields.Item("U_RazonCita").Value.ToString(), Recordset.Fields.Item("U_Color").Value.ToString())
                    End If
                    Recordset.MoveNext()
                End While
            End If

            Recordset.DoQuery(QueryEstados)
            If Recordset.RecordCount > 0 Then
                While Not Recordset.EoF
                    If Not ColoresEstadoCita.ContainsKey(Recordset.Fields.Item("Code").Value.ToString()) Then
                        ColoresEstadoCita.Add(Recordset.Fields.Item("Code").Value.ToString(), New ColorPorEstado(Recordset.Fields.Item("U_Color").Value.ToString(), Recordset.Fields.Item("U_ColorOT").Value.ToString()))
                    End If
                    Recordset.MoveNext()
                End While
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el color que se debe utilizar para graficar la celda de acuerdo a los parámetros indicados
    ''' </summary>
    ''' <param name="Tipo">Tipo de color que representa el tipo de documento (Cita, Orden de Trabajo, Asesor,...)</param>
    ''' <param name="NumeroCita">Número de cita</param>
    ''' <param name="CodigoRazon">Código de la razón de la cita. En caso de no tener puede ir en blanco.</param>
    ''' <param name="CodigoEstado">Código del estado de la cita. En caso de no tener puede ir en blanco.</param>
    ''' <param name="NumeroOT">Número de cita. En caso de no tener puede ir en blanco.</param>
    ''' <param name="CodigoSucursal">Código de la sucursal</param>
    ''' <param name="EsDiaPosterior">Valor que indica si el documento es de un día anterior, ya que esto cambie el color que se debe utilizar</param>
    ''' <returns>Color que se debe utilizar para el documento indicado</returns>
    ''' <remarks></remarks>
    Public Function ObtenerColor(ByVal Tipo As TipoColor, ByVal NumeroCita As String, ByVal CodigoRazon As String, ByVal CodigoEstado As String, ByVal NumeroOT As String, ByVal CodigoSucursal As String, ByVal EsDiaPosterior As Boolean) As Color
        Dim Color As Color
        Try
            Select Case Tipo
                Case TipoColor.Almuerzo
                    Color = Drawing.Color.DarkGray
                Case TipoColor.Bloqueo
                    Color = Drawing.Color.DarkGray
                Case TipoColor.OrdenTrabajo
                    If Not String.IsNullOrEmpty(NumeroCita) Then
                        Color = ObtenerColorCita(CodigoRazon, CodigoEstado, NumeroOT, CodigoSucursal, EsDiaPosterior)
                    Else
                        Color = Drawing.Color.DarkSeaGreen
                    End If
                    'Color = Drawing.Color.DarkSeaGreen
                Case TipoColor.SuspensionHorario
                    Color = Drawing.Color.DarkGray
                Case TipoColor.Cita
                    Color = ObtenerColorCita(CodigoRazon, CodigoEstado, NumeroOT, CodigoSucursal, EsDiaPosterior)
                Case TipoColor.Reprogramada
                    If Not String.IsNullOrEmpty(NumeroCita) Then
                        Color = ObtenerColorCita(CodigoRazon, CodigoEstado, NumeroOT, CodigoSucursal, EsDiaPosterior)
                    Else
                        Color = Drawing.Color.DarkSeaGreen
                    End If
                Case TipoColor.ServicioNoIniciado
                    If Not String.IsNullOrEmpty(NumeroCita) Then
                        Color = ObtenerColorCita(CodigoRazon, CodigoEstado, NumeroOT, CodigoSucursal, EsDiaPosterior)
                    Else
                        Color = Drawing.Color.DarkSeaGreen
                    End If
            End Select
            Return Color
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return Color
        End Try
    End Function

    Private Function ObtenerColorCita(ByVal CodigoRazon As String, ByVal CodigoEstado As String, ByVal NumeroOT As String, ByVal CodigoSucursal As String, ByVal EsDiaPosterior As Boolean) As Color
        Dim UsaColoresAgenda As String = String.Empty
        Dim MetodoGestionColor As frmListaCitas.GestionColor
        Dim Color As Color
        Try
            UsaColoresAgenda = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs = CodigoSucursal).U_AgendaColor

            If String.IsNullOrEmpty(UsaColoresAgenda) Then
                UsaColoresAgenda = "N"
            End If

            'Obtiene la configuración de color de la sucursal
            If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs = CodigoSucursal).U_ManageColorBy) Then
                MetodoGestionColor = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs = CodigoSucursal).U_ManageColorBy
            Else
                MetodoGestionColor = frmListaCitas.GestionColor.RazonCita
            End If

            If UsaColoresAgenda = "Y" Then
                Select Case MetodoGestionColor
                    Case frmListaCitas.GestionColor.EstadoCita
                        If ColoresEstadoCita.ContainsKey(CodigoEstado) Then
                            If String.IsNullOrEmpty(NumeroOT) Then
                                Color = Color.FromName(ColoresEstadoCita.Item(CodigoEstado).ColorSinOrdenTrabajo)
                            Else
                                Color = Color.FromName(ColoresEstadoCita.Item(CodigoEstado).ColorConOrdenTrabajo)
                            End If
                        Else
                            Color = Drawing.Color.DarkSeaGreen
                        End If
                    Case frmListaCitas.GestionColor.RazonCita
                        If ColoresRazonCita.ContainsKey(CodigoRazon) Then
                            Color = Color.FromName(ColoresRazonCita.Item(CodigoRazon))
                        Else
                            Color = Drawing.Color.DarkSeaGreen
                        End If
                End Select
            Else
                If EsDiaPosterior Then
                    Color = Drawing.Color.Aqua
                Else
                    Color = Drawing.Color.DarkSeaGreen
                End If
            End If

            Return Color
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

End Class
