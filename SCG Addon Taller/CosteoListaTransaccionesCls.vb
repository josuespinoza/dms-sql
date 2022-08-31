
Partial Public Class CosteoCls

    Public Function CrearAsiento(m_strUnidad As String, ByVal usaTransaction As Boolean, ByVal p_fecha As Date, ByVal p_strTipoVehiculo As String) As Integer

        Dim oJournalEntry As SAPbobsCOM.JournalEntries

        Dim strMensajeError As String = String.Empty
        Dim strMonedaLocal As String = String.Empty

        Dim strNoAsiento As String = String.Empty

        Dim decTotal As Decimal
        Dim strCuenta As String = String.Empty
        Dim strContraCuenta As String = String.Empty
        Dim strTipoVehiculo As String = String.Empty
        Dim blnPrimeraCuenta As Boolean = True
        Dim strInvFacturado As String = String.Empty

        ''manejo para validacion de importes negativos 
        Dim strImpNeg As String = String.Empty
        Dim CreaAsientoNormal As Boolean = False

        Try
            strNoAsiento = 0

            oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

            strMonedaLocal = g_strMonedaLocal
            Dim p_fechaDocumento As String = p_fecha


            If Not String.IsNullOrEmpty(m_strUnidad) Then

                strInvFacturado = objConfiguracionGeneral.InventarioVehiculoVendido

                If Not String.IsNullOrEmpty(p_strTipoVehiculo) Then
                    strTipoVehiculo = p_strTipoVehiculo
                Else
                    strTipoVehiculo = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Tipo FROM [@SCGD_VEHICULO] WITH (nolock) where U_Cod_Unid = '{0}'", m_strUnidad), m_oCompany.CompanyDB, m_oCompany.Server).Trim
                End If

                ' Comparo el inventario de la Unidad con el Inventario "Post Venta"
                If strInvFacturado = strTipoVehiculo Then
                    strTipoVehiculo = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Tipo_Ven FROM [@SCGD_VEHICULO] WITH (nolock) where U_Cod_Unid = '{0}'", m_strUnidad), m_oCompany.CompanyDB, m_oCompany.Server).Trim
                End If

                strCuenta = objConfiguracionGeneral.CuentaInventarioTransito(strTipoVehiculo)
                strContraCuenta = objConfiguracionGeneral.CuentaStock(strTipoVehiculo)

                oJournalEntry.Reference = m_strUnidad

                If p_fechaDocumento <> Nothing Then
                    oJournalEntry.ReferenceDate = p_fechaDocumento
                Else
                    oJournalEntry.ReferenceDate = Date.Now
                End If

                oJournalEntry.Memo = My.Resources.Resource.RegistroDiarioMemoEntrada & " " & m_strUnidad
                oJournalEntry.UserFields.Fields.Item("U_SCGD_AplVal").Value = "0"
                decTotal = 0

                If Not blnPrimeraCuenta Then
                    oJournalEntry.Lines.Add()
                Else
                    blnPrimeraCuenta = False
                End If

                If Not String.IsNullOrEmpty(strCuenta) Then
                    oJournalEntry.Lines.AccountCode = strCuenta
                End If

                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                decTotal = decCostoTotalMonedaLocal

                CreaAsientoNormal = False

                If decTotal < 0 Then

                    If String.IsNullOrEmpty(strImpNeg) Then
                        'obtengo configuracion para importes negativos 
                        strImpNeg = Utilitarios.EjecutarConsulta("SELECT NegAmount FROM OADM WITH (nolock)", m_oCompany.CompanyDB, m_oCompany.Server)
                    End If

                    If strImpNeg = "N" Then
                        'cuando se reciben valores negativos se invierten las cuentas
                        oJournalEntry.Lines.Debit = decCostoTotalMonedaLocal * -1
                        oJournalEntry.Lines.FCDebit = 0
                        oJournalEntry.Lines.ContraAccount = strContraCuenta
                        oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "Y"

                        If blnAgregarDimension Then
                            ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                        End If

                        'Cuenta
                        oJournalEntry.Lines.Add()
                        oJournalEntry.Lines.AccountCode = strContraCuenta
                        oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "Y"
                        oJournalEntry.Lines.Credit = decCostoTotalMonedaLocal * -1
                        oJournalEntry.Lines.FCCredit = 0

                        If blnAgregarDimension Then
                            ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                        End If

                    ElseIf strImpNeg = "Y" Then
                        CreaAsientoNormal = True
                    End If

                Else
                    ' cuando los valores son positivos
                    CreaAsientoNormal = True
                End If

                If CreaAsientoNormal Then

                    oJournalEntry.Lines.Credit = decCostoTotalMonedaLocal
                    oJournalEntry.Lines.FCCredit = 0
                    oJournalEntry.Lines.ContraAccount = strContraCuenta
                    oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "N"
                    If blnAgregarDimension Then
                        ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                    End If

                    'Cuenta
                    oJournalEntry.Lines.Add()
                    oJournalEntry.Lines.AccountCode = strContraCuenta
                    oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "N"
                    oJournalEntry.Lines.Debit = decCostoTotalMonedaLocal
                    oJournalEntry.Lines.FCDebit = 0

                    If blnAgregarDimension Then
                        ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                    End If
                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

                End If
                Dim error2 As Integer = oJournalEntry.Add

                If error2 <> 0 Then
                    If decTotal = 0 Then

                    Else
                        strNoAsiento = "0"
                        m_oCompany.GetLastError(error2, strMensajeError)
                        If m_oCompany.InTransaction Then
                            m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                        End If
                    End If
                Else

                    m_oCompany.GetNewObjectCode(strNoAsiento)

                End If
            Else

            End If

            Return CInt(strNoAsiento)

        Catch ex As Exception

            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If

        End Try
    End Function

    Public Class ListaValoresCosteoLocal_Sistema

        Private strTransaccion As String
        Public Property Transaccion() As String
            Get
                Return strTransaccion

            End Get
            Set(value As String)
                strTransaccion = value
            End Set
        End Property


        Private decValorLocal As Decimal
        Public Property ValorLocal() As Decimal
            Get
                Return decValorLocal

            End Get
            Set(value As Decimal)
                decValorLocal = value
            End Set
        End Property


        Private decValorSistema As Decimal
        Public Property ValorSistema() As Decimal
            Get
                Return decValorSistema

            End Get
            Set(value As Decimal)
                decValorSistema = value
            End Set
        End Property

        Private strMonedaRegistro As String
        Public Property MonedaRegistro() As String
            Get
                Return strMonedaRegistro

            End Get
            Set(value As String)
                strMonedaRegistro = value
            End Set
        End Property

        Private strNombreTransaccion As String
        Public Property NombreTransaccion() As String
            Get
                Return strNombreTransaccion

            End Get
            Set(value As String)
                strNombreTransaccion = value
            End Set
        End Property

        Private decValorLocal_S As Decimal
        Public Property ValorLocal_S() As Decimal
            Get
                Return decValorLocal_S

            End Get
            Set(value As Decimal)
                decValorLocal_S = value
            End Set
        End Property


        Private decValorSistema_S As Decimal
        Public Property ValorSistema_S() As Decimal
            Get
                Return decValorSistema_S

            End Get
            Set(value As Decimal)
                decValorSistema_S = value
            End Set
        End Property
    End Class

    Public Class ListaUnidad

        Private strUnidad As String
        Public Property Unidad() As String
            Get
                Return strUnidad

            End Get
            Set(value As String)
                strUnidad = value
            End Set
        End Property


        Private strMarca As String
        Public Property Marca() As String
            Get
                Return strMarca

            End Get
            Set(value As String)
                strMarca = value
            End Set
        End Property

        Private strEstilo As String
        Public Property Estilo() As String
            Get
                Return strEstilo

            End Get
            Set(value As String)
                strEstilo = value
            End Set
        End Property

        Private strModelo As String
        Public Property Modelo() As String
            Get
                Return strModelo

            End Get
            Set(value As String)
                strModelo = value
            End Set
        End Property

        Private strVIN As String
        Public Property VIN() As String
            Get
                Return strVIN

            End Get
            Set(value As String)
                strVIN = value
            End Set
        End Property

        Private strIDVehiculo As String
        Public Property IDVehiculo() As String
            Get
                Return strIDVehiculo

            End Get
            Set(value As String)
                strIDVehiculo = value
            End Set
        End Property

        Private strDocRecepcion As String
        Public Property DocRecepcion() As String
            Get
                Return strDocRecepcion

            End Get
            Set(value As String)
                strDocRecepcion = value
            End Set
        End Property

        Private strtipoVehiculo As String
        Public Property TipoVehiculo() As String
            Get
                Return strtipoVehiculo

            End Get
            Set(value As String)
                strtipoVehiculo = value
            End Set
        End Property
    End Class

End Class
