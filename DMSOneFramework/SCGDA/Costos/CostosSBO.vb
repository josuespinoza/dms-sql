Namespace SCGDataAccess

    Public Class CostosSBO

#Region "Declaraciones"

        Public Structure G_Type_EsquemaAsiento
            Public strNoOrden As String
            Public arrLineas As ArrayList
        End Structure

        Public Structure G_Type_EsquemaCuenta
            Public strNoOrden As String
            Public strNoCuenta As String
            Public strNoContraCuenta As String
            Public decCredit As Decimal
            Public decDebit As Decimal
            Public strRef2 As String
            Public strCostingCode As String
        End Structure

#End Region

#Region "Propiedades"

#End Region

#Region "Procedimientos Publicos"

        Public Function CrearAsientoContable(ByRef p_strucAsiento As G_Type_EsquemaAsiento) As Integer
            Dim boJournalEntry As SAPbobsCOM.JournalEntries
            Dim objEsquemaCuenta As G_Type_EsquemaCuenta
            Dim decDebit As Decimal = 0
            Dim decCredit As Decimal = 0
            Dim intCont As Integer
            Dim intResult As Integer = -1
            Dim strErrorMessage As String
            Dim ex As Exception

            boJournalEntry = G_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

            boJournalEntry.Reference = p_strucAsiento.strNoOrden
            boJournalEntry.TaxDate = Now.Date

            For intCont = 0 To p_strucAsiento.arrLineas.Count - 1

                objEsquemaCuenta = CType(p_strucAsiento.arrLineas(intCont), G_Type_EsquemaCuenta)

                If objEsquemaCuenta.decCredit <> 0 Or objEsquemaCuenta.decDebit <> 0 Then

                    decDebit += objEsquemaCuenta.decDebit
                    decCredit += objEsquemaCuenta.decCredit

                    CargarLineaCuenta(boJournalEntry.Lines, objEsquemaCuenta)

                End If

            Next

            If decCredit = decDebit And (decCredit + decDebit) <> 0 Then

                intResult = boJournalEntry.Add

                If intResult <> 0 Then

                    G_objCompany.GetLastError(intResult, strErrorMessage)

                    ex = New SCGCommon.ExceptionsSBO(intResult, strErrorMessage)

                    ex.Source = "Creación de Asiento contable"

                    Throw ex

                End If

                Return 0

            Else
                Return 1
            End If

        End Function

#End Region

#Region "Procedimientos Privados"

        Private Sub CargarLineaCuenta(ByRef p_boJournalEntryLines As SAPbobsCOM.JournalEntries_Lines, ByRef p_strucDatos As G_Type_EsquemaCuenta)
            Dim dtCurrentDate As Date

            dtCurrentDate = Now.Date

            With p_boJournalEntryLines

                .AccountCode = p_strucDatos.strNoCuenta
                .ContraAccount = p_strucDatos.strNoContraCuenta
                .Credit = p_strucDatos.decCredit
                .Debit = p_strucDatos.decDebit
                .DueDate = dtCurrentDate
                .ShortName = p_strucDatos.strNoCuenta
                .TaxDate = dtCurrentDate
                .Reference1 = p_strucDatos.strNoOrden
                .Reference2 = p_strucDatos.strRef2
                .CostingCode = p_strucDatos.strCostingCode
            End With

            p_boJournalEntryLines.Add()

        End Sub

#End Region

    End Class

End Namespace