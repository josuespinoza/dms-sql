Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports System
Imports SCG.SBOFramework.UI
Imports System.IO
Imports SCG.DMSOne.Framework
Imports SCG.SBOFramework.DI

Namespace GastosContratoVentas

    Partial Public Class GastosAdicionales

        Private Shared m_oCVenta As ContratoVentasCls
        Private Shared p_strUnidad As String
        Private Shared p_strContrato As String
        Private Shared p_strMoneda As String

        Public Sub CargarGastosVehiculo(ByVal strUnidad As String, ByVal strContrato As String, ByVal strMoneda As String, ByVal dtFecha As Date)

            Dim strUnidadDataTable As String
            Dim intLineasPant As Integer = 0
            Dim strConsulta As String
            Dim strGuardaDataTable As String
            Dim strDependeDataTable As String
            Dim strUnidadDepende As String
            Dim strDepGuarda As String
            Dim strCodItemDataTable As String
            Dim strCodItemDepende As String
            Dim blnDepende As Boolean = False
            Dim blnUnidadExiste As Boolean = False
            Dim decMonto As Decimal
            Dim n As NumberFormatInfo

            n = DIHelper.GetNumberFormatInfo(_companySbo)

            m_oCVenta = New ContratoVentasCls(_companySbo, _applicationSbo)

            p_strUnidad = strUnidad

            p_strContrato = strContrato

            p_strMoneda = strMoneda

            dataTablePantGastos.Rows.Clear()

            For i As Integer = 0 To ContratoVentasCls.oDataTableGastosUnidad.Rows.Count - 1

                strUnidadDataTable = ContratoVentasCls.oDataTableGastosUnidad.GetValue("unidad", i)

                If strUnidadDataTable = strUnidad Then

                    blnUnidadExiste = True

                    strGuardaDataTable = ContratoVentasCls.oDataTableGastosUnidad.GetValue("guarda", i)
                    strDependeDataTable = ContratoVentasCls.oDataTableGastosUnidad.GetValue("depende", i)

                    If strGuardaDataTable = "Y" Then

                        For intGuarda As Integer = 0 To ContratoVentasCls.oDataTableGastosUnidad.Rows.Count - 1

                            strUnidadDepende = ContratoVentasCls.oDataTableGastosUnidad.GetValue("unidad", intGuarda)
                            strDepGuarda = ContratoVentasCls.oDataTableGastosUnidad.GetValue("depende", intGuarda)

                            strCodItemDataTable = ContratoVentasCls.oDataTableGastosUnidad.GetValue("codItem", i)
                            strCodItemDepende = ContratoVentasCls.oDataTableGastosUnidad.GetValue("codItem", intGuarda)

                            If strUnidadDepende = strUnidadDataTable AndAlso strCodItemDepende = strCodItemDataTable AndAlso strDepGuarda = "Y" Then

                                decMonto = Utilitarios.CalcularCostosPorCambioMoneda(_companySbo, p_strMoneda, ContratoVentasCls.oDataTableGastosUnidad.GetValue("monto", intGuarda), _
                                                                                     ContratoVentasCls.oDataTableGastosUnidad.GetValue("moneda", intGuarda), 1, dtFecha)

                                dataTablePantGastos.Rows.Add()
                                dataTablePantGastos.SetValue("codigo", intLineasPant, ContratoVentasCls.oDataTableGastosUnidad.GetValue("codItem", intGuarda))
                                dataTablePantGastos.SetValue("descrip", intLineasPant, ContratoVentasCls.oDataTableGastosUnidad.GetValue("desItem", intGuarda))
                                dataTablePantGastos.SetValue("monto", intLineasPant, decMonto.ToString(n))
                                intLineasPant += 1
                                blnDepende = True
                                Exit For

                            End If

                        Next

                        If blnDepende = False Then

                            decMonto = Utilitarios.CalcularCostosPorCambioMoneda(_companySbo, p_strMoneda, ContratoVentasCls.oDataTableGastosUnidad.GetValue("monto", i), _
                                                                                     ContratoVentasCls.oDataTableGastosUnidad.GetValue("moneda", i), 1, dtFecha)

                            dataTablePantGastos.Rows.Add()
                            dataTablePantGastos.SetValue("codigo", intLineasPant, ContratoVentasCls.oDataTableGastosUnidad.GetValue("codItem", i))
                            dataTablePantGastos.SetValue("descrip", intLineasPant, ContratoVentasCls.oDataTableGastosUnidad.GetValue("desItem", i))
                            dataTablePantGastos.SetValue("monto", intLineasPant, decMonto.ToString(n))
                            intLineasPant += 1

                        End If

                    ElseIf strGuardaDataTable = "N" AndAlso Not strDependeDataTable = "Y" Then

                        decMonto = Utilitarios.CalcularCostosPorCambioMoneda(_companySbo, p_strMoneda, ContratoVentasCls.oDataTableGastosUnidad.GetValue("monto", i), _
                                                                                     ContratoVentasCls.oDataTableGastosUnidad.GetValue("moneda", i), 1, dtFecha)

                        dataTablePantGastos.Rows.Add()
                        dataTablePantGastos.SetValue("codigo", intLineasPant, ContratoVentasCls.oDataTableGastosUnidad.GetValue("codItem", i))
                        dataTablePantGastos.SetValue("descrip", intLineasPant, ContratoVentasCls.oDataTableGastosUnidad.GetValue("desItem", i))
                        dataTablePantGastos.SetValue("monto", intLineasPant, decMonto.ToString(n))
                        intLineasPant += 1

                    End If

                End If

            Next

            If blnUnidadExiste = False Then

                dataTableLineasSum.Rows.Clear()
                dataTableLineasSum = FormularioSBO.DataSources.DataTables.Item("LineasSum")

                strConsulta = "SELECT Code, Name FROM [@SCGD_CONFLINEASSUM] where Canceled = 'N' AND U_Gas_Veh = 'Y'"

                dataTableLineasSum.ExecuteQuery(strConsulta)

                If dataTableLineasSum.Rows.Count > 0 Then

                    For i As Integer = 0 To dataTableLineasSum.Rows.Count - 1

                        If Not String.IsNullOrEmpty(dataTableLineasSum.GetValue("Code", i)) Then

                            dataTablePantGastos.Rows.Add()
                            dataTablePantGastos.SetValue("codigo", intLineasPant, dataTableLineasSum.GetValue("Code", i))
                            dataTablePantGastos.SetValue("descrip", intLineasPant, dataTableLineasSum.GetValue("Name", i))
                            intLineasPant += 1

                        End If
                        
                    Next

                End If

            End If

            MatrixGastosPantalla.Matrix.LoadFromDataSource()

        End Sub

        Public Sub ButtonSBOOk(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent)

            Dim strUnidadDataTable As String
            Dim strCodItemDataTable As String
            Dim strCodItemPant As String
            Dim blnAgregado As Boolean = False
            Dim blnUnidadExiste As Boolean = False
            Dim intPosDataTable As Integer
            Dim strGuardaDataTable As String
            Dim decSumaGastos As Decimal
            Dim blnExiste As Boolean = False
            Dim n As NumberFormatInfo

            n = DIHelper.GetNumberFormatInfo(_companySbo)

            MatrixGastosPantalla.Matrix.FlushToDataSource()

            For i As Integer = 0 To dataTablePantGastos.Rows.Count - 1

                If ContratoVentasCls.oDataTableGastosUnidad.Rows.Count > 0 AndAlso blnAgregado = False Then

                    For intCV As Integer = 0 To ContratoVentasCls.oDataTableGastosUnidad.Rows.Count - 1

                        strUnidadDataTable = ContratoVentasCls.oDataTableGastosUnidad.GetValue("unidad", intCV)
                        
                        If strUnidadDataTable = p_strUnidad AndAlso blnExiste = False Then

                            strCodItemDataTable = ContratoVentasCls.oDataTableGastosUnidad.GetValue("codItem", intCV)
                            strCodItemPant = dataTablePantGastos.GetValue("codigo", i)

                            blnUnidadExiste = True

                            If strCodItemDataTable = strCodItemPant Then

                                strGuardaDataTable = ContratoVentasCls.oDataTableGastosUnidad.GetValue("guarda", intCV)

                                If strGuardaDataTable = "Y" Then

                                    intPosDataTable = ContratoVentasCls.oDataTableGastosUnidad.Rows.Count

                                    ContratoVentasCls.oDataTableGastosUnidad.Rows.Add()
                                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("codItem", intPosDataTable, dataTablePantGastos.GetValue("codigo", i))
                                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("desItem", intPosDataTable, dataTablePantGastos.GetValue("descrip", i))
                                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("monto", intPosDataTable, dataTablePantGastos.GetValue("monto", i))
                                    If Not String.IsNullOrEmpty(p_strContrato) Then
                                        ContratoVentasCls.oDataTableGastosUnidad.SetValue("cont", intPosDataTable, p_strContrato)
                                    End If
                                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("unidad", intPosDataTable, p_strUnidad)
                                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("guarda", intPosDataTable, "N")
                                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("depende", intPosDataTable, "Y")
                                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("moneda", intPosDataTable, p_strMoneda)

                                Else

                                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("monto", intCV, dataTablePantGastos.GetValue("monto", i))
                                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("moneda", intCV, p_strMoneda)
                                    Exit For

                                End If

                            End If

                        End If

                    Next

                    If blnUnidadExiste = False Then

                        intPosDataTable = ContratoVentasCls.oDataTableGastosUnidad.Rows.Count

                        ContratoVentasCls.oDataTableGastosUnidad.Rows.Add()
                        ContratoVentasCls.oDataTableGastosUnidad.SetValue("codItem", intPosDataTable, dataTablePantGastos.GetValue("codigo", i))
                        ContratoVentasCls.oDataTableGastosUnidad.SetValue("desItem", intPosDataTable, dataTablePantGastos.GetValue("descrip", i))
                        ContratoVentasCls.oDataTableGastosUnidad.SetValue("monto", intPosDataTable, dataTablePantGastos.GetValue("monto", i))
                        If Not String.IsNullOrEmpty(p_strContrato) Then
                            ContratoVentasCls.oDataTableGastosUnidad.SetValue("cont", intPosDataTable, p_strContrato)
                        End If
                        ContratoVentasCls.oDataTableGastosUnidad.SetValue("unidad", intPosDataTable, p_strUnidad)
                        ContratoVentasCls.oDataTableGastosUnidad.SetValue("guarda", intPosDataTable, "N")
                        ContratoVentasCls.oDataTableGastosUnidad.SetValue("depende", intPosDataTable, "N")
                        ContratoVentasCls.oDataTableGastosUnidad.SetValue("moneda", intPosDataTable, p_strMoneda)

                        blnExiste = True
                        
                    End If

                Else

                    ContratoVentasCls.oDataTableGastosUnidad.Rows.Add()
                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("codItem", i, dataTablePantGastos.GetValue("codigo", i))
                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("desItem", i, dataTablePantGastos.GetValue("descrip", i))
                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("monto", i, dataTablePantGastos.GetValue("monto", i))
                    If Not String.IsNullOrEmpty(p_strContrato) Then
                        ContratoVentasCls.oDataTableGastosUnidad.SetValue("cont", i, p_strContrato)
                    End If
                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("unidad", i, p_strUnidad)
                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("guarda", i, "N")
                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("depende", i, "N")
                    ContratoVentasCls.oDataTableGastosUnidad.SetValue("moneda", i, p_strMoneda)

                    blnAgregado = True

                End If

                decSumaGastos += dataTablePantGastos.GetValue("monto", i)

            Next

            FormContrato.DataSources.DBDataSources.Item("@SCGD_CVENTA").SetValue("U_OG_Temp", 0, decSumaGastos.ToString(n))

            m_oCVenta.FormateaPreciosBase(FormContrato)

        End Sub

    End Class

End Namespace