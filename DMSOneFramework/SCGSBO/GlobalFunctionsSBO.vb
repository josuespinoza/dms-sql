Imports System.Data.SqlClient
Imports DMSOneFramework.SCGCommon
Namespace BLSBO
    Public Class GlobalFunctionsSBO
        'Carga los datos de las monedas que se manejan en el sistema de SBO y retorna un recordset
'        Public Function CargarMonedasSBO() As SAPbobsCOM.Recordset
'            Dim objMonedas As SAPbobsCOM.Recordset
'            Dim intResult As Integer
'            Dim strErrMessage As String = ""
'
'            Try
'                objMonedas = BLSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
'                objMonedas.DoQuery("SELECT * FROM OCRN")
                'oCompany.GetLastError(intResult, strErrMessage)
'
                'If intResult <> 0 Then
                '    Throw New SCGCommon.ExceptionsSBO(intResult)
                'End If
'            Catch ex As Exception
'                Throw ex
'            End Try
'
'            CargarMonedasSBO = objMonedas
'
'        End Function

        Public Sub AgregarPrecioAcordado(ByVal p_intNoCotizacion As Integer, _
                                              ByVal p_dblPrecio As Double, _
                                              ByVal p_intLineNum As Integer)
            Dim objCotizacion As SAPbobsCOM.Documents
            Dim intResult As Integer
            Dim strErrMessage As String = ""

            Try
                objCotizacion = BLSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

                If Not objCotizacion.GetByKey(p_intNoCotizacion) Then
                    oCompany.GetLastError(intResult, strErrMessage)
                    Throw New SCGCommon.ExceptionsSBO(intResult, strErrMessage)
                End If

                objCotizacion.Lines.SetCurrentLine(p_intLineNum)
                'If objCotizacion.Lines.UnitPrice = 0 Then
                objCotizacion.Lines.UnitPrice = p_dblPrecio
                'End If
                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PrecioAcordad").Value = CInt(p_dblPrecio)
                intResult = objCotizacion.Update()

                If intResult <> 0 Then
                    oCompany.GetLastError(intResult, strErrMessage)
                    Throw New SCGCommon.ExceptionsSBO(intResult, strErrMessage)
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End Sub

        Public Function ActualizaResultado(ByVal p_intNoCotizacion As Integer, _
                                              ByVal p_strResultado As String, _
                                              ByVal p_intLineNum As Integer) As Integer
            Dim objCotizacion As SAPbobsCOM.Documents
            Dim intResult As Integer = 0
            Dim strErrMessage As String = ""
            Dim intReturnValue As Integer = 0
            Dim strResultado As String = String.Empty


            Try
                objCotizacion = BLSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

                If Not objCotizacion.GetByKey(p_intNoCotizacion) Then
                    oCompany.GetLastError(intResult, strErrMessage)
                    Throw New SCGCommon.ExceptionsSBO(intResult, strErrMessage)
                End If


                'For i As Integer = 0 To objCotizacion.Lines.Count - 1

                '    objCotizacion.Lines.SetCurrentLine(i)
                '    If p_intLineNum = objCotizacion.Lines.LineNum Then
                '        Dim a As Integer = objCotizacion.Lines.LineNum
                '    End If

                'Next

                objCotizacion.Lines.SetCurrentLine(p_intLineNum)

               

                '100 es el tamaño del UDF U_Resultado
                strResultado = p_strResultado.Substring(0, Math.Min(p_strResultado.Length, 100))

                If CStr(objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value) <> strResultado Then
                    objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value = strResultado
                    intResult = objCotizacion.Update()

                Else

                    intReturnValue = -1

                End If

                If intResult <> 0 Then

                    oCompany.GetLastError(intResult, strErrMessage)

                    Throw New SCGCommon.ExceptionsSBO(intResult, strErrMessage)

                Else

                    Return intReturnValue

                End If

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ActualizaObservacionLinea(ByVal p_intNoCotizacion As Integer, _
                                              ByVal p_strObservacion As String, _
                                              ByVal p_intLineNum As Integer) As Integer
            Dim objCotizacion As SAPbobsCOM.Documents
            Dim intResult As Integer = 0
            Dim strErrMessage As String = ""
            Dim intReturnValue As Integer = 0

            Try
                objCotizacion = BLSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                If p_strObservacion.Length > 100 Then
                    p_strObservacion = p_strObservacion.Substring(0, 100).Trim
                End If
                If Not objCotizacion.GetByKey(p_intNoCotizacion) Then
                    oCompany.GetLastError(intResult, strErrMessage)
                    Throw New SCGCommon.ExceptionsSBO(intResult, strErrMessage)
                End If

                For index As Integer = 0 To objCotizacion.Lines.Count - 1
                    objCotizacion.Lines.SetCurrentLine(index)
                    If objCotizacion.Lines.LineNum = p_intLineNum Then
                        If objCotizacion.Lines.FreeText <> p_strObservacion Then

                            objCotizacion.Lines.FreeText = p_strObservacion
                            intResult = objCotizacion.Update()

                        Else

                            intReturnValue = -1

                        End If

                        If intResult <> 0 Then

                            oCompany.GetLastError(intResult, strErrMessage)

                            Throw New SCGCommon.ExceptionsSBO(intResult, strErrMessage)

                        Else

                            Return intReturnValue

                        End If
                    End If
                Next
                
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ActualizarCantidadAct(ByVal p_intNoCotizacion As Integer, _
                                              ByVal p_dblCantidad As Double, _
                                              ByVal p_intLineNum As Integer) As Integer

            Dim objCotizacion As SAPbobsCOM.Documents
            Dim intResult As Integer = 0
            Dim strErrMessage As String = ""
            Dim intReturnValue As Integer = 0

            Try
                objCotizacion = BLSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

                If Not objCotizacion.GetByKey(p_intNoCotizacion) Then
                    oCompany.GetLastError(intResult, strErrMessage)
                    Throw New SCGCommon.ExceptionsSBO(intResult, strErrMessage)
                End If

                objCotizacion.Lines.SetCurrentLine(p_intLineNum)

                If p_dblCantidad > objCotizacion.Lines.Quantity Then

                    objCotizacion.Lines.Quantity = p_dblCantidad
                    intResult = objCotizacion.Update()

                Else

                    intReturnValue = -1

                End If

                If intResult <> 0 Then

                    oCompany.GetLastError(intResult, strErrMessage)

                    Throw New SCGCommon.ExceptionsSBO(intResult, strErrMessage)

                Else

                    Return intReturnValue

                End If

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Sub AgregarCodProblema(ByVal p_intNoCotizacion As Integer, _
                                            ByVal strCodigo As String, _
                                            ByVal p_intLineNum As Integer)
            Dim objCotizacion As SAPbobsCOM.Documents
            Dim intResult As Integer
            Dim strErrMessage As String = ""

            Try
                objCotizacion = BLSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

                If Not objCotizacion.GetByKey(p_intNoCotizacion) Then
                    oCompany.GetLastError(intResult, strErrMessage)
                    Throw New SCGCommon.ExceptionsSBO(intResult, strErrMessage)
                End If

                objCotizacion.Lines.SetCurrentLine(p_intLineNum)
                'If objCotizacion.Lines.UnitPrice = 0 Then
                'objCotizacion.Lines.UnitPrice = strCodProblema
                'End If

                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CodProblema").Value = strCodigo


                intResult = objCotizacion.Update()

                If intResult <> 0 Then
                    oCompany.GetLastError(intResult, strErrMessage)
                    Throw New SCGCommon.ExceptionsSBO(intResult, strErrMessage)
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End Sub

        Public Sub AgregarCodOperacion(ByVal p_intNoCotizacion As Integer, _
                                          ByVal strCodigo As String, _
                                          ByVal p_intLineNum As Integer)
            Dim objCotizacion As SAPbobsCOM.Documents
            Dim intResult As Integer
            Dim strErrMessage As String = ""

            Try
                objCotizacion = BLSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

                If Not objCotizacion.GetByKey(p_intNoCotizacion) Then
                    oCompany.GetLastError(intResult, strErrMessage)
                    Throw New SCGCommon.ExceptionsSBO(intResult, strErrMessage)
                End If

                objCotizacion.Lines.SetCurrentLine(p_intLineNum)
                'If objCotizacion.Lines.UnitPrice = 0 Then
                'objCotizacion.Lines.UnitPrice = strCodProblema
                'End If

                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CodOperacion").Value = strCodigo


                intResult = objCotizacion.Update()

                If intResult <> 0 Then
                    oCompany.GetLastError(intResult, strErrMessage)
                    Throw New SCGCommon.ExceptionsSBO(intResult, strErrMessage)
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End Sub


        'Public Sub AgregarPrecioAcordado(ByVal p_intNoCotizacion As Integer, _
        '                                      ByVal p_dblPrecio As Double, _
        '                                      ByVal p_strItemCode As String)
        '    Dim objCotizacion As SAPbobsCOM.Documents
        '    Dim intResult As Integer
        '    Dim strErrMessage As String = ""
        '    Dim intLineNum As Integer
        '    Try
        '        objCotizacion = BLSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
        '        If Not objCotizacion.GetByKey(p_intNoCotizacion) Then
        '            oCompany.GetLastError(intResult, strErrMessage)
        '            Throw New SCGCommon.ExceptionsSBO(intResult)
        '        End If
        '        For intLineNum = 0 To objCotizacion.Lines.Count - 1

        '            objCotizacion.Lines.SetCurrentLine(intLineNum)
        '            If objCotizacion.Lines.ItemCode = p_strItemCode Then
        '                If objCotizacion.Lines.UnitPrice = 0 Then
        '                    objCotizacion.Lines.UnitPrice = p_dblPrecio
        '                End If
        '                objCotizacion.Lines.UserFields.Fields.Item("U_PrecioAcordado").Value = CInt(p_dblPrecio)
        '            End If

        '        Next

        '        intResult = objCotizacion.Update()

        '        If intResult <> 0 Then
        '            oCompany.GetLastError(intResult, strErrMessage)
        '            Throw New SCGCommon.ExceptionsSBO(intResult)
        '        End If

        '    Catch ex As Exception
        '        Throw ex
        '    End Try

        'End Sub


        '*Metodo Documentado, sustituido por utilitarios.asignarempleado
        'Public Sub AgregarEmpleadoRealiza(ByVal p_intNoCotizacion As Integer, _
        '                                              ByVal p_strIDEmpleado As String, _
        '                                              ByVal p_intLineNum As Integer, _
        '                                              ByVal p_strNombreEmpleado As String)


        '    ''*********************************************************************
        '    ''*Metodo documentado, se cambió para evitar el uso de objetos de SDK *
        '    ''*********************************************************************

        '    'Dim objCotizacion As SAPbobsCOM.Documents
        '    'Dim intResult As Integer
        '    'Dim strErrMessage As String = ""
        '    ''Dim strValorAnteriorCampo As String
        '    ''Dim strValores() As String
        '    ''Dim blnYaEsta As Boolean = False
        '    'Dim strValor As String

        '    'Try
        '    '    objCotizacion = BLSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
        '    '    If Not objCotizacion.GetByKey(p_intNoCotizacion) Then
        '    '        oCompany.GetLastError(intResult, strErrMessage)
        '    '        Throw New SCGCommon.ExceptionsSBO(intResult)
        '    '    End If

        '    '    objCotizacion.Lines.SetCurrentLine(p_intLineNum)
        '    '    'strValorAnteriorCampo = objCotizacion.Lines.UserFields.Fields.Item("U_Emp_Realiza").Value
        '    '    'strValores = strValorAnteriorCampo.Split(",")
        '    '    'For Each strValor In strValores
        '    '    '    If strValor = p_strIDEmpleado Then
        '    '    '        blnYaEsta = True
        '    '    '        Exit For
        '    '    '    End If
        '    '    'Next
        '    '    'If Not blnYaEsta Then
        '    '    'If strValorAnteriorCampo = "" Then
        '    '    objCotizacion.Lines.UserFields.Fields.Item("U_Emp_Realiza").Value = p_strIDEmpleado
        '    '    objCotizacion.Lines.UserFields.Fields.Item("U_NombEmpleado").Value = p_strNombreEmpleado
        '    '    'Else
        '    '    '    objCotizacion.Lines.UserFields.Fields.Item("U_Emp_Realiza").Value = strValorAnteriorCampo & ", " & p_strIDEmpleado
        '    '    'End If

        '    '    intResult = objCotizacion.Update()

        '    '    If intResult <> 0 Then
        '    '        BLSBO.oCompany.GetLastError(intResult, strErrMessage)
        '    '        Throw New Exception(strErrMessage)
        '    '    End If

        '    '    'End If
        '    'Catch ex As Exception
        '    '    Throw ex
        '    'End Try

        '    '    '**************************************************************
        '    '    '*                FIN DEL METODO DOCUMENTADO                  *
        '    '    '**************************************************************




        'End Sub

'        Public Function AllEmpByID(ByVal intCodEmp As Integer) As SAPbobsCOM.Recordset
'            Dim objEmpleado As SAPbobsCOM.Recordset
'            Dim intResult As Integer
'            Dim strErrMessage As String = ""
'
'            Try
'                objEmpleado = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
'                objEmpleado.DoQuery("SELECT * FROM ohem WHERE EMPID = " & intCodEmp)
                'oCompany.GetLastError(intResult, strErrMessage)
                'If intResult <> 0 Then
                '    Throw New SCGCommon.ExceptionsSBO(intResult)
                'End If
'            Catch ex As Exception
'                Throw ex
'            End Try
'
'            AllEmpByID = objEmpleado
'
'        End Function

'        Public Function CargarPeriodoSBO() As SAPbobsCOM.Recordset
'            Dim objPeriodo As SAPbobsCOM.Recordset
'            Dim intResult As Integer
'            Dim strErrMessage As String = ""
'
'            Try
'                objPeriodo = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
'                objPeriodo.DoQuery("SELECT * FROM OFPR WHERE ACTIV = 'Y'")
                'oCompany.GetLastError(intResult, strErrMessage)
                'If intResult <> 0 Then
                '    Throw New SCGCommon.ExceptionsSBO(intResult)
                'End If
'            Catch ex As Exception
'                Throw ex
'            End Try
'
'            CargarPeriodoSBO = objPeriodo
'
'        End Function

        'Carga el codigo y el nombre de los departamentos
        'que se manejan en el sistema de SBO y retorna un recordset
'        Public Function CargarDepartamentosSBO() As SAPbobsCOM.Recordset
'
'            Dim objRecordset As SAPbobsCOM.Recordset
'            Dim intResult As Integer
'            Dim strErrMessage As String = ""
'
'            Try
'
'                objRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
'                objRecordset.DoQuery("SELECT code, name FROM OUDP")
                'oCompany.GetLastError(intResult, strErrMessage)
'
                'If intResult <> 0 Then
                '    Throw New SCGCommon.ExceptionsSBO(intResult)
                'End If
'
'            Catch ex As Exception
'                Throw ex
'            End Try
'
'            CargarDepartamentosSBO = objRecordset
'
'        End Function

        Public Sub Set_Compania(ByVal objCIA As SAPbobsCOM.Company)
            oCompany = objCIA
        End Sub

        Public Sub MonedasSistema(ByRef strMonedaLocal As String, ByRef strMonedaSistema As String)
            Dim oRecordset As SAPbobsCOM.Recordset
            Dim oSBObob As SAPbobsCOM.SBObob
            Dim intResult As Integer
            Dim strErrMessage As String = ""

            Try
                oSBObob = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
                oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                oRecordset = oSBObob.GetLocalCurrency()
                oRecordset.MoveFirst()
                If oRecordset.EoF Then
                    Throw New Exception(My.Resources.ResourceFrameWork.MensajeMonedaLocalNoConfigurada)
                Else
                    strMonedaLocal = oRecordset.Fields.Item(0).Value
                End If

                oRecordset = oSBObob.GetSystemCurrency()
                oRecordset.MoveFirst()
                If oRecordset.EoF Then
                    Throw New Exception(My.Resources.ResourceFrameWork.MensajeMonedaSistemaNoConfigurada)
                Else
                    strMonedaSistema = oRecordset.Fields.Item(0).Value
                End If

                'oCompany.GetLastError(intResult, strErrMessage)
                'If intResult <> 0 Then
                '    Throw New ExceptionsSBO(intResult)
                'End If
            Catch ex As Exception
                Throw ex
            End Try

        End Sub

        Public Function RetornarMonedaLocal() As String
            Dim oSBObob As SAPbobsCOM.SBObob
            Dim oRecordset As SAPbobsCOM.Recordset
            Dim strResult As String

            Try

                oSBObob = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
                oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                oRecordset = oSBObob.GetLocalCurrency()
                strResult = oRecordset.Fields.Item(0).Value

                Return strResult

            Catch ex As Exception
                Return -1
            End Try

        End Function

        Public Function RetornarMonedaSistema() As String
            Dim oSBObob As SAPbobsCOM.SBObob
            Dim sToday As String
            Dim oRecordset As SAPbobsCOM.Recordset
            Dim strResult As String

            'Try

            oSBObob = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
            oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            oRecordset = oSBObob.GetSystemCurrency()
            strResult = oRecordset.Fields.Item(0).Value

            Return strResult


        End Function

        Public Function RetornarTipoCambioMonedaRS(ByVal Moneda As String, ByVal p_Hoy As Date) As Double
            Dim oSBObob As SAPbobsCOM.SBObob
            Dim sToday As String
            Dim oRecordset As SAPbobsCOM.Recordset
            Dim dblResult As Double
            Dim query As String

            oSBObob = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
            oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            sToday = p_Hoy
            query = "SELECT Rate FROM ORTT WHERE Currency='" & Moneda & "'" & _
                              " AND RateDate='" & CDate(sToday).ToString("yyyyMMdd") & "'"
            oRecordset.DoQuery(query)
            dblResult = -1
            If oRecordset IsNot Nothing Then
                dblResult = FormatNumber(oRecordset.Fields.Item(0).Value, 2)
                If dblResult = 0 Then dblResult = -1
            End If
            Return dblResult

        End Function

        Public Function RetornarTipoCambioMoneda (ByVal Moneda As String, ByVal p_Hoy As Date, ByVal strConectionString As String, ByVal blnBDExterna As Boolean) As Decimal
            Dim drdResultadoConsulta As SqlClient.SqlDataReader
            Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
            Dim cn_Coneccion As New SqlClient.SqlConnection
            Dim sToday As String
            Dim dblResult As Double = -1

            Try
                cn_Coneccion.ConnectionString = strConectionString
                cn_Coneccion.Open()
                sToday = p_Hoy
                cmdEjecutarConsulta.Connection = cn_Coneccion

                cmdEjecutarConsulta.CommandType = CommandType.Text
                If blnBDExterna then
                    cmdEjecutarConsulta.CommandText = "SELECT Rate FROM SCGTA_VW_ORTT with (nolock) WHERE Currency='" & Moneda & "'" & _
                                      " AND RateDate='" & CDate(sToday).ToString("yyyyMMdd") & "'"
                    Else
                    cmdEjecutarConsulta.CommandText = "SELECT Rate FROM ORTT with (nolock) WHERE Currency='" & Moneda & "'" & _
                                      " AND RateDate='" & CDate(sToday).ToString("yyyyMMdd") & "'"

                    end if
                drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()
                Do While drdResultadoConsulta.Read
                    If drdResultadoConsulta.Item(0) IsNot DBNull.Value Then
                        'dblResult = FormatNumber(drdResultadoConsulta.Item(0), 2)
                        dblResult = drdResultadoConsulta.GetDecimal(0)
                        If dblResult = 0 Then dblResult = -1
                        Exit Do
                    End If
                Loop
            Catch
                Throw
            Finally
                drdResultadoConsulta.Close()
                cmdEjecutarConsulta.Connection.Close()
            End Try
            Return dblResult
        End Function

        Public Function ValidarTipoCambioMonedaFecha(ByVal Moneda As String, ByVal p_fecha As Date, ByVal strConectionString As String, ByVal blnBDExterna As Boolean) As Decimal
            Dim drdResultadoConsulta As SqlClient.SqlDataReader
            Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
            Dim cn_Coneccion As New SqlClient.SqlConnection

            Dim n As System.Globalization.NumberFormatInfo

            Dim strValor As String = ""
            Dim sToday As String
            Dim dblResult As Double = -1

            Try
                cn_Coneccion.ConnectionString = strConectionString
                cn_Coneccion.Open()
                sToday = p_fecha
                cmdEjecutarConsulta.Connection = cn_Coneccion

                cmdEjecutarConsulta.CommandType = CommandType.Text
                If blnBDExterna Then
                    cmdEjecutarConsulta.CommandText = "SELECT Rate FROM SCGTA_VW_ORTT WHERE Currency='" & Moneda & "'" & _
                                  " AND RateDate='" & CDate(sToday).ToString("yyyyMMdd") & "'"
                Else
                    cmdEjecutarConsulta.CommandText = "SELECT Rate FROM ORTT WHERE Currency='" & Moneda & "'" & _
                                  " AND RateDate='" & CDate(sToday).ToString("yyyyMMdd") & "'"

                End If
                drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()
                Do While drdResultadoConsulta.Read
                    If drdResultadoConsulta.Item(0) IsNot DBNull.Value Then
                        'dblResult = FormatNumber(drdResultadoConsulta.Item(0), 2)
                        dblResult = drdResultadoConsulta.GetDecimal(0)
                        If dblResult = 0 Then dblResult = -1
                        Exit Do
                    End If
                Loop
            Catch
                Throw
            Finally
                drdResultadoConsulta.Close()
                cmdEjecutarConsulta.Connection.Close()
            End Try
            Return dblResult
        End Function



'        Public Function RetornarTipoCambioMoneda(ByVal Moneda As String, ByVal p_Hoy As Date) As Double
'            Dim oSBObob As SAPbobsCOM.SBObob
'            Dim sToday As String
'            Dim oRecordset As SAPbobsCOM.Recordset
'            Dim dblResult As Double
'            ' Dim bla As String
'            'Try
'
'            oSBObob = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
'            oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
'
'            sToday = p_Hoy
'            oRecordset.DoQuery("SELECT Rate FROM ORTT WHERE Currency='" & Moneda & "'" & _
'                              " AND RateDate='" & CDate(sToday).ToString("yyyyMMdd") & "'")
'
'            'oRecordset = oSBObob.GetCurrencyRate(Moneda, CDate(sToday))
'            If oRecordset IsNot Nothing Then
'                dblResult = FormatNumber(oRecordset.Fields.Item(0).Value, 2)
'                If dblResult = 0 Then
'                    Throw New Exception(My.Resources.ResourceFrameWork.TipoCambioNoActualizado)
'                End If
'            Else
'                Throw New Exception(My.Resources.ResourceFrameWork.TipoCambioNoActualizado)
'            End If
'            Return dblResult
'
'            'Catch ex As Exception
'            '    ''bla = oCompany.GetLastErrorCode
'            '    Return -1
'            'End Try
'
'        End Function

        'Public Function RetornarTipoCambioMoneda(ByVal p_Hoy As Date, ByRef p_oCompany As SAPbobsCOM.Company) As Double
        '    Dim oSBObob As SAPbobsCOM.SBObob
        '    Dim sToday As String
        '    Dim oRecordset As SAPbobsCOM.Recordset
        '    Dim dblResult As Double
        '    Dim strSysCurrency As String

        '    Try
        '        oSBObob = p_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
        '        oRecordset = p_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        '        sToday = p_Hoy

        '        oRecordset = oSBObob.GetSystemCurrency
        '        strSysCurrency = oRecordset.Fields.Item(0).Value

        '        oRecordset = oSBObob.GetCurrencyRate(strSysCurrency, CDate(sToday))
        '        dblResult = FormatNumber(oRecordset.Fields.Item(0).Value, 2)

        '        Return dblResult

        '    Catch ex As Exception
        '        Return -1
        '    End Try

        'End Function

        'Public Function RetornarTipoCambioMoneda(ByVal Moneda As String) As Double
        '    Dim oSBObob As SAPbobsCOM.SBObob
        '    Dim sToday As String
        '    Dim oRecordset As SAPbobsCOM.Recordset
        '    Dim dblResult As Double
        '    Dim errCode As Integer
        '    Dim errMesg As String = ""


        '    Try
        '        oSBObob = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
        '        oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        '        sToday = Now.Date

        '        oRecordset = oSBObob.GetCurrencyRate(Moneda, CDate(sToday))
        '        dblResult = FormatNumber(oRecordset.Fields.Item(0).Value, 2)

        '        'oCompany.GetLastError(errCode, errMesg)

        '        'If errCode <> 0 Then
        '        '    Throw New ExceptionsSBO(errCode)
        '        'Else
        '        RetornarTipoCambioMoneda = dblResult
        '        'Exit Function
        '        'End If
        '    Catch ex As Exception
        '        Throw ex
        '    End Try

        'End Function

        Public Sub Set_DB_SCG(ByVal strDB As String)
            DBSCG = strDB
        End Sub


 
    End Class
End Namespace
