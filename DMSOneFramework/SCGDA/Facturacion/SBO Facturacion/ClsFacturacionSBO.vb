Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.BLSBO
Imports System.Data.SqlClient

Namespace SCGDataAccess
    Public Class ClsFacturacionSBO

#Region "Declaraciones"
        Private Const mc_strOrdenTrabajo As String = "U_OT"
        Private Const mc_strEstadofactura As String = "u_est_fac"
        Private Const mc_strFacturada As String = "u_facturad"

        Private cmdFactura As SqlCommand
        Private Shared m_cnnSCGTaller As SqlClient.SqlConnection
        Private m_adpFactura As SqlClient.SqlDataAdapter

        Dim objDAConexion As DAConexion
#End Region


#Region "Constructor"
        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpFactura = New SqlClient.SqlDataAdapter
        End Sub


#End Region

        


        Public Function CrearFactura(ByVal srtCardname As String, ByVal strCardCode As String, ByVal strOT As String, _
                                     ByVal suministros As Boolean, ByVal repuestos As Boolean, ByVal manoobra As Boolean, _
                                     Optional ByVal strCodManoObra As String = "", Optional ByVal decTotalMO As Decimal = 0, _
                                     Optional ByVal strImpMO As String = "", Optional ByVal strCodRep As String = "", _
                                     Optional ByVal decTotalRep As Decimal = 0, Optional ByVal strImpRep As String = "", _
                                     Optional ByVal strCodSum As String = "", Optional ByVal decTotalSum As Decimal = 0, _
                                     Optional ByVal strImpSum As String = "") As Integer
            Dim oInvoice As SAPbobsCOM.Documents
            Dim strError As String = ""
            Dim intError As Integer
            Dim strDocumento As Integer
            Dim numFactura As Integer

            oCompany.StartTransaction()

            ''Encabezado Factura
            oInvoice = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)
            oInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items
            oInvoice.DocObjectCode = SAPbobsCOM.BoObjectTypes.oInvoices
            oInvoice.CardCode = strCardCode
            oInvoice.CardName = srtCardname
            oInvoice.DocDate = Today
            'oInvoice.DocDueDate = Today

            oInvoice.UserFields.Fields.Item(mc_strOrdenTrabajo).Value = strOT
            ' oInvoice.UserFields.Fields.Item(mc_strFacturada).Value = objfacRep.U_Facturad
            'oInvoice.UserFields.Fields.Item(mc_strEstadofactura).Value = objfacRep.U_Est_Fac

            'Lineas de Factura
            If suministros Then
                oInvoice.Lines.ItemCode = strCodSum
                oInvoice.Lines.Quantity = decTotalSum
                oInvoice.Lines.Price = 1
                oInvoice.Lines.TaxCode = strImpSum
            End If

            If repuestos Then
                If suministros Then
                    oInvoice.Lines.Add()
                End If
                oInvoice.Lines.ItemCode = strCodRep
                oInvoice.Lines.Quantity = decTotalRep
                oInvoice.Lines.Price = 1
                oInvoice.Lines.TaxCode = strImpRep

            End If

            If manoobra Then
                If suministros Or repuestos Then
                    oInvoice.Lines.Add()
                End If
                oInvoice.Lines.ItemCode = strCodManoObra
                oInvoice.Lines.Quantity = decTotalMO
                oInvoice.Lines.Price = 1
                oInvoice.Lines.TaxCode = strImpMO
            End If




            If oInvoice.Add <> 0 Then
                oCompany.GetLastError(intError, strError)

                'Agregado 30/05/06. Alejandra
                If intError <> 0 Then
                    Throw New SCGCommon.ExceptionsSBO(intError, strError)
                End If

                If oCompany.InTransaction Then
                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                End If
                CrearFactura = 0
            Else
                If oCompany.InTransaction Then
                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                End If
                oCompany.GetNewObjectCode(strDocumento)
                numFactura = GetDocNum(strDocumento)

                CrearFactura = numFactura

            End If
        End Function

        Public Function Lee_Tipo_Cambio(ByVal dtProc As DateTime) As Boolean

            Dim oSBObob As SAPbobsCOM.SBObob

            Dim lTCHoy As Long
            Dim sLocalCurr As String
            Dim sSystemCurr As String
            Dim sToday As String

            Dim oRecordset As SAPbobsCOM.Recordset
            Dim oRSTC As SAPbobsCOM.Recordset

            Dim adpDocMarketing As New SCGDataAccess.AccesoSBODataAdapter


            Try
                oSBObob = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)

                sToday = dtProc

                '// Get an initialized Recordset object
                oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                oRecordset = oSBObob.GetLocalCurrency()
                sLocalCurr = oRecordset.Fields.Item(0).Value

                oRecordset = oSBObob.GetSystemCurrency()

                sSystemCurr = oRecordset.Fields.Item(0).Value
                '// catch exceptions
                '// can be used instead of Company.GetLastError method



                '// Executing the GetCurrencyRate method
                '// You can use this method to query the exchange rate
                '// between any currency and the local currency.
                '// For example, assume that the local currency is US dollars,
                '// and you use this method to query EUR on
                '// January 10, 2002. You can then use
                '// GetCurrencyRate("eur", Date("10.01.2002")).
                '// The result 0.98 from the returned Recordset object means
                '// that on January 10, 2002 the exchange rate was 1 EUR = 0.98 USD.

                oRSTC = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                If sLocalCurr.Trim = sSystemCurr.Trim Then

                    lTCHoy = 1

                Else

                    oRSTC = oSBObob.GetCurrencyRate(sSystemCurr, CDate(sToday))
                    lTCHoy = CDec(oRSTC.Fields.Item(0).Value)

                End If

                'oMoneda = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                'oMoneda = oSBObob.GetCurrencyRate("EUR", CDate(sToday))
                'lTCHoy = CDec(oMoneda.Fields.Item(0).Value)

                Return True


                Exit Function

            Catch ex As Exception
                Throw ex

                'MsgBox(ex.Message, MsgBoxStyle.OKOnly)
                Return False

            End Try

        End Function

        Public Function ModificarFactura_Repuestos(ByVal docnum As String, ByVal NoFactura As Integer, ByVal est_fact As String) As Integer

            Dim objFactura As SAPbobsCOM.Documents

            Dim objUFields As SAPbobsCOM.UserFields

            Dim objFields As SAPbobsCOM.Fields

            Dim objField As SAPbobsCOM.Field

            Dim strArrayDocNum() As String

            Dim intError As Integer

            Dim strError As String = ""
            Dim i As Integer


            Try


                objFactura = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices)

                strArrayDocNum = Split(docnum, "/")

                For i = 0 To UBound(strArrayDocNum) - 1

                    If objFactura.GetByKey(CInt(strArrayDocNum(i))) Then

                        objUFields = objFactura.UserFields

                        objFields = objUFields.Fields


                        For Each objField In objFields

                            If objField.Name = "U_NFactura" Then

                                If NoFactura <> 0 Then
                                    objField.Value = NoFactura
                                Else
                                    objField.Value = 0
                                End If

                            End If

                            If objField.Name = "U_Facturad" Then
                                If est_fact <> "" Then
                                    objField.Value = est_fact
                                Else
                                    objField.Value = "2"
                                End If
                            End If

                        Next

                        intError = objFactura.Update()

                        ModificarFactura_Repuestos = 0


                        If intError <> 0 Then

                            oCompany.GetLastError(intError, strError)

                            ModificarFactura_Repuestos = -1

                            Throw New SCGCommon.ExceptionsSBO(intError, strError)

                        End If

                    End If
                Next i

            Catch ex As Exception

                ModificarFactura_Repuestos = -1

                Throw ex

            End Try

        End Function

        Public Function AnulaFactura_Repuestos(ByVal v_numfactura As Integer, ByVal est_fact As String) As Integer

            Dim objFactura As SAPbobsCOM.Documents

            Dim objUFields As SAPbobsCOM.UserFields

            Dim objFields As SAPbobsCOM.Fields

            Dim objField As SAPbobsCOM.Field

            Dim intError As Integer

            Dim DrdFacturas As SqlClient.SqlDataReader =  Nothing

            Dim strError As String = ""

            Dim v_docnum As Integer

            cmdFactura = New SqlCommand

            Try

                'Se abre la conexion
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    m_cnnSCGTaller.Open()
                End If


                With cmdFactura
                    .Connection = m_cnnSCGTaller
                    .CommandType = CommandType.Text
                    .CommandText = "Select docentry From scgta_vw_opch where u_nfactura = " & v_numfactura
                    DrdFacturas = cmdFactura.ExecuteReader()
                End With

                While DrdFacturas.Read
                    v_docnum = DrdFacturas.Item(0)

                    objFactura = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices)


                    If objFactura.GetByKey(v_docnum) Then

                        objUFields = objFactura.UserFields

                        objFields = objUFields.Fields


                        For Each objField In objFields

                            If objField.Name = "U_NFactura" Then
                                objField.Value = 0
                            End If

                            If objField.Name = "U_Facturad" Then
                                If est_fact <> "" Then
                                    objField.Value = est_fact
                                Else
                                    objField.Value = "2"
                                End If
                            End If

                        Next

                        intError = objFactura.Update()

                        AnulaFactura_Repuestos = 0

                        If intError <> 0 Then

                            oCompany.GetLastError(intError, strError)

                            AnulaFactura_Repuestos = -1

                            Throw New SCGCommon.ExceptionsSBO(intError, strError)

                        End If

                    End If

                End While
                
            Catch ex As Exception

                AnulaFactura_Repuestos = -1

                Throw ex

            Finally
                ' Se cierra la conexión
                DrdFacturas.Close()
                m_cnnSCGTaller.Close()

            End Try

        End Function

'        Public Function Modifica_Reversarfactura(ByVal v_numfactura As Integer) As Integer
            'Dim strArrayDocNum() As String
'
            'Dim intError As Integer
'
            'Dim strError As String
            'Dim i As Integer
            'Dim n As Integer
'
'            Dim rs As SAPbobsCOM.Recordset
'
'            Try
'
'                rs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
'
'                rs.DoQuery("UPDATE OINV set U_Anula_F = '2' WHERE DocNum = " & CInt(v_numfactura))
'
'
'
'            Catch ex As Exception
'
'                Modifica_Reversarfactura = -1
'
'                Throw ex
'
'            Finally
'
'                Call m_cnnSCGTaller.Close()
'
'            End Try
'
'
'
'        End Function
'        Public Sub AnulaFactura_Suministros(ByVal v_numfactura As Integer, ByVal est_fact As String)
'
'
'            Dim objFactura As SAPbobsCOM.Documents
'
            'Dim strArrayDocNum() As String
'
            'Dim intError As Integer
'
            'Dim strError As String
            'Dim i As Integer
            'Dim n As Integer
'
'            Dim rs As SAPbobsCOM.Recordset
'
'            Try
'                objFactura = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)
'
'                rs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
'
'                rs.DoQuery("UPDATE IGE1 set U_NFactura =" & 0 & ", u_facturad = '2'" & "WHERE U_NFactura = " & CInt(v_numfactura))
'
'            Catch ex As Exception
'
                'AnulaFactura_Suministros = -1
'
'                Throw ex
'
'            Finally
'
'                Call m_cnnSCGTaller.Close()
'
'            End Try
'
'        End Sub
        Public Function GetDocNum(ByVal intNFactura As Integer) As Integer
            Dim objFactura As SAPbobsCOM.Documents
            Dim blnOK As Boolean
            Dim intResult As Integer
            Dim strErrMessage As String = ""

            objFactura = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)

            Try
                blnOK = objFactura.GetByKey(intNFactura)

                If blnOK Then
                    GetDocNum = objFactura.DocNum
                    Exit Function
                Else
                    oCompany.GetLastError(intResult, strErrMessage)
                    GetDocNum = -1
                End If
            Catch ex As Exception
                GetDocNum = -1
                Throw ex
            End Try
        End Function

'        Public Function ModificarFactura_Suministros(ByVal docnum As String, ByVal NoFactura As Integer, ByVal linenum As String) As Integer
'
'            Dim objFactura As SAPbobsCOM.Documents
'
            'Dim objUFields As SAPbobsCOM.UserFields
'
            'Dim objFields As SAPbobsCOM.Fields
'
            'Dim objField As SAPbobsCOM.Field
'
'            Dim strArrayDocNum() As String
'            Dim strArrayLineNum() As String
'
            'Dim intError As Integer
'
            'Dim strError As String
'            Dim i As Integer
            'Dim n As Integer
'
'
'            Dim rs As SAPbobsCOM.Recordset
'
'            Try
'                objFactura = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)
'
'                strArrayDocNum = Split(docnum, "/")
'                strArrayLineNum = Split(linenum, "/")
'
'                For i = 0 To UBound(strArrayDocNum) - 1
'
'                    rs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
'
'                    rs.DoQuery("UPDATE IGE1 set U_NFactura =" & NoFactura & ", u_facturad = '1'" & "WHERE docentry = " & CInt(strArrayDocNum(i)) & "and linenum =" & CInt(strArrayLineNum(i)))
'
'
'                Next i
'
'
'            Catch ex As Exception
'
'                ModificarFactura_Suministros = -1
'
'                Throw ex
'
'            Finally
'                Call m_cnnSCGTaller.Close()
'
'            End Try
'
'        End Function

'        Public Sub ActualizarKitReparacion(ByVal p_dstSuministros As SuministrosFullDataset, ByVal p_intNoFactura As Integer)
            'Actualiza el user define field U_KitRepar, que determina si los suministros a facturar forman parte de un kit de reparacion 
'            Try
'                Dim drwSum As SuministrosFullDataset.SCGTA_SP_SelSuministrosFullRow
'                Dim rs As SAPbobsCOM.Recordset
'                Dim intKit As Integer
'
'                For Each drwSum In p_dstSuministros.SCGTA_SP_SelSuministrosFull
'                    If drwSum.Check = True And drwSum.facturada = "No Facturada" Then
'
'                        rs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
'                        If drwSum.U_KitRepar Then
'                            intKit = 1
'                        Else
'                            intKit = 0
'                        End If
'
'                        rs.DoQuery("UPDATE IGE1 set U_KitRepar = " & intKit & " WHERE docentry = " & drwSum.docentry & " and linenum = " & drwSum.LineNum & " and U_NFactura = " & p_intNoFactura)
'
'                    End If
'
'                Next drwSum
'
'            Catch ex As Exception
'                Throw ex
'
'            Finally
'                Call m_cnnSCGTaller.Close()
'            End Try
'        End Sub




    End Class
End Namespace
