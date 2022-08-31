
Partial Public Class BalanceOrdenesTrabajo


#Region "Métodos"

    ''' <summary>
    ''' Método que se ejecuta al cargar el formulario
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargaFormulario()
        rbtDet.AsignaValorUserDataSource("Y")
        rbtRes.AsignaValorUserDataSource("N")
    End Sub


    Public Sub Imprimir()

        Dim strTipoRpt, strDirRpt, strBDSAP, strParametros As String
        Dim dtFechaDesde As String
        Dim dtFechaHasta As String
        Dim strNoOT As String

        strBDSAP = _companySbo.CompanyDB

        If rbtDet.ObtieneValorUserDataSource = "Y" And rbtRes.ObtieneValorUserDataSource = "N" Then strTipoRpt = "Det"
        If rbtDet.ObtieneValorUserDataSource = "N" And rbtRes.ObtieneValorUserDataSource = "Y" Then strTipoRpt = "Res"

        dtFechaDesde = txtFDesde.ObtieneValorUserDataSource()
        dtFechaHasta = txtFHasta.ObtieneValorUserDataSource()
        strNoOT = txtNoOt.ObtieneValorUserDataSource()

        If String.IsNullOrEmpty(strNoOT) Then
            strNoOT = "ALL"
        End If

        'Concateno los parametros
        strParametros = String.Format("{0},{1},{2}", dtFechaDesde, dtFechaHasta, strNoOT)

        If strTipoRpt = "Res" Then
            strDirRpt = DireccionReportes & My.Resources.Resource.rptBalanceOTResumido
        ElseIf strTipoRpt = "Det" Then
            strDirRpt = DireccionReportes & My.Resources.Resource.rptBalanceOTDetallado
        End If

        If Not String.IsNullOrEmpty(strParametros) Then

            Call Utilitarios.ImprimirReporte(strDirRpt, My.Resources.Resource.TituloBalanceOT, strParametros, UsuarioBd, ContraseñaBd, strBDSAP, CompanySBO.Server)

        End If

    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Manejo del evento item pressed
    ''' </summary>
    ''' <param name="FormUID">UID del formulario</param>
    ''' <param name="pVal">Objeto Evento</param>
    ''' <param name="BubbleEvent">Bubble event</param>
    ''' <remarks></remarks>
    Private Sub ManejadorEventosItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByVal BubbleEvent As Boolean)

        Dim strDet As String
        Dim strRes As String

        FormularioSBO.Freeze(True)

        If pVal.ActionSuccess Then

            Select Case pVal.ItemUID

                Case "rbtDet"

                    strDet = rbtDet.ObtieneValorUserDataSource()

                    If strDet = "N" Then
                        rbtDet.AsignaValorUserDataSource("N")
                        rbtRes.AsignaValorUserDataSource("Y")
                    ElseIf strDet = "Y" Then
                        rbtDet.AsignaValorUserDataSource("Y")
                        rbtRes.AsignaValorUserDataSource("N")
                    End If

                Case "rbtRes"

                    strRes = rbtRes.ObtieneValorUserDataSource()

                    If strRes = "N" Then
                        rbtRes.AsignaValorUserDataSource("N")
                        rbtDet.AsignaValorUserDataSource("Y")
                    ElseIf strRes = "Y" Then
                        rbtDet.AsignaValorUserDataSource("N")
                        rbtRes.AsignaValorUserDataSource("Y")
                    End If

                Case "btnImp"

                    Imprimir()

            End Select

        End If

        FormularioSBO.Freeze(False)

    End Sub

#End Region

End Class
