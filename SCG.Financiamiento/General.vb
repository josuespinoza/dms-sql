Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

'Clase que contiene funciones y metodos genéricos necesarios en diferentes partes del proyecto de Financiamiento

Public Class General

    Friend Shared DBPassword As String
    Friend Shared DBUser As String

    Shared Function EjecutarConsulta(ByRef p_strConsulta As String, _
                                     ByVal strConectionString As String) As String
        Return DMS_Connector.Helpers.EjecutarConsulta(p_strConsulta)
    End Function

    ''' <summary>
    ''' Funcion que valida si existe un formulario abierto
    ''' </summary>
    ''' <param name="SBO_Application">SBO Aplication</param>
    ''' <param name="strFormUID">Id de formulario</param>
    ''' <param name="blnselectIfOpen"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function ValidarSiFormularioAbierto(ByRef SBO_Application As SAPbouiCOM.IApplication, ByVal strFormUID As String, _
                                                ByVal blnselectIfOpen As Boolean) As Boolean

        Dim intI As Integer = 0
        Dim blnFound As Boolean = False
        Dim frmForma As SAPbouiCOM.Form

        Dim a As Integer = SBO_Application.Forms.Count

        While (Not blnFound AndAlso intI < SBO_Application.Forms.Count)

            frmForma = SBO_Application.Forms.Item(intI)

            If frmForma.UniqueID = strFormUID Then
                blnFound = True
                If (blnselectIfOpen) Then
                    If Not (frmForma.Selected) Then
                        SBO_Application.Forms.Item(strFormUID).Select()
                    End If
                End If
            Else

                intI += 1
            End If

        End While

        If (blnFound) Then
            Return True
        Else
            Return False
        End If

    End Function

    'Valida si el formulario que se está abriendo ya está abierto o no, para que no de error al abrirlo de nuevo

    Shared Function FormularioAbierto(ByVal formulario As IFormularioSBO, ByVal activarSiEstaAbierto As Boolean, ByVal _sboApplication As SAPbouiCOM.Application) As Boolean
        Dim sboForm As Form

        For indice As Integer = 0 To _sboApplication.Forms.Count - 1
            sboForm = _sboApplication.Forms.Item(indice)
            If sboForm.TypeEx = formulario.FormType Then
                If activarSiEstaAbierto Then sboForm.Select()
                Return True
            End If
        Next
        Return False

    End Function

    'Carga de valores válidos en los Combo Box mediante consultas a tablas de base de datos, si el Combo tiene valores los elimina
    'para luego cargar los valores consultados

    Public Overloads Shared Sub CargarValidValuesEnCombos(ByRef oValidValues As SAPbouiCOM.ValidValues, ByVal strQuery As String,
                                                          ByVal p_oCompany As SAPbobsCOM.Company)

        Dim intRecIndex As Integer
        'Dim cboCombo As SAPbouiCOM.ComboBox
        'Dim oItem As SAPbouiCOM.Item

        Dim drdResultadoConsulta As SqlClient.SqlDataReader
        Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Try
           
            CrearCadenaDeconexion(p_oCompany.Server, p_oCompany.CompanyDB, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString

            cn_Coneccion.Open()
            cmdEjecutarConsulta.Connection = cn_Coneccion
            cmdEjecutarConsulta.CommandType = CommandType.Text
            cmdEjecutarConsulta.CommandText = strQuery
            drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()

            'Borra los ValidValues
            If oValidValues.Count > 0 Then
                For intRecIndex = 0 To oValidValues.Count - 1
                    oValidValues.Remove(oValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            ''Agrega los ValidValues
            Do While drdResultadoConsulta.Read
                If Not drdResultadoConsulta.IsDBNull(0) AndAlso Not drdResultadoConsulta.IsDBNull(1) Then

                    oValidValues.Add(drdResultadoConsulta.Item(0).ToString.Trim, drdResultadoConsulta.Item(1).ToString.Trim)
                End If
            Loop

            drdResultadoConsulta.Close()
            cn_Coneccion.Close()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function CrearCadenaDeconexion(ByVal strNombreServidor As String, _
                                                ByVal BasedeDatosSCG As String, _
                                                ByRef p_strCadenaDeConexion As String) As Boolean

        'Dim strConectionString As String

        Try
            'Verifica si la conexión utiliza autenticación de windows
            'Si utiliza Windows Autentication crea el string sin el Usuario y Password
            'Si No envia el Usuario y Password de Conexión

            p_strCadenaDeConexion = "Data Source=" & strNombreServidor.ToLower() & ";" & _
                                    "Initial Catalog=" & BasedeDatosSCG.ToLower() & ";" & _
                                    "Connect Timeout=120;" & _
                                    "User ID=" & General.DBUser & ";" & _
                                    "pwd=" & General.DBPassword & _
                                    ";Pooling=False"

            'If oCompany.WinAuthentication Then
            '    strConectionString &= ";Trusted_Connection=Yes"
            'Else
            '    
            'End If
            Return True

        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        End Try

    End Function

    Shared Sub DestruirObjeto(ByRef objDocumento As Object)
        If Not objDocumento Is Nothing Then
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objDocumento)
            objDocumento = Nothing
        End If
    End Sub


    'Retorna la moneda de sistema de SBO

    Shared Function RetornarMonedaSistema(ByVal oCompany As SAPbobsCOM.Company) As String

        Dim oSBObob As SAPbobsCOM.SBObob
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim strResult As String

        oSBObob = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
        oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

        oRecordset = oSBObob.GetSystemCurrency()
        strResult = oRecordset.Fields.Item(0).Value

        Return strResult

    End Function

    'Retorna la moneda local de SBO

    Shared Function RetornarMonedaLocal(ByVal oCompany As SAPbobsCOM.Company) As String

        Dim oSBObob As SAPbobsCOM.SBObob
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim strResult As String

        oSBObob = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
        oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

        oRecordset = oSBObob.GetLocalCurrency()
        strResult = oRecordset.Fields.Item(0).Value

        Return strResult

    End Function

    'Imprimir los reportes del modulo de financiamiento según los diferentes parámetros, y el reporte que se esté abriendo

    Public Shared Sub ImprimirReporte(ByVal p_oCompany As SAPbobsCOM.Company, ByVal p_strDireccionReporte As String, ByVal p_strBarraTitulo As String, ByVal p_strParametros As String, _
                               ByVal p_strUsuarioBD As String, ByVal p_strContraseñaBD As String)

        Dim strPathExe As String

        p_strBarraTitulo = p_strBarraTitulo.Replace(" ", "°")

        p_strDireccionReporte = p_strDireccionReporte.Replace(" ", "°")

        p_strParametros = p_strParametros.Replace(" ", "°")

        strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

        strPathExe &= p_strBarraTitulo & " " & p_strDireccionReporte & " " & p_strUsuarioBD & "," & p_strContraseñaBD & "," & p_oCompany.Server & "," & p_oCompany.CompanyDB & " " & p_strParametros

        Shell(strPathExe, AppWinStyle.MaximizedFocus)

    End Sub

    Shared Function ConvierteDecimal(ByVal strDecimal As String, _
                                     ByVal n As NumberFormatInfo) As Decimal
        'Variable a obtener el valor final
        Dim dcDecimal As Decimal

        'Elimino espacios al string 
        strDecimal = strDecimal.Trim()

        'Convierto el valor del string a decimal
        If Not String.IsNullOrEmpty(strDecimal) Then
            dcDecimal = Decimal.Parse(strDecimal, n)
        Else
            dcDecimal = 0
        End If

        'Retorna el decimal
        Return dcDecimal

    End Function

    'Retorna el pago recibido como objeto de SAP según el número de pago indicado

    Public Shared Function CargarPagoRecibido(ByVal p_intPagoRecibido As Integer, ByVal p_oCompany As SAPbobsCOM.Company, ByVal intTipoPago As Integer)

        Dim oPagoRecibido As SAPbobsCOM.Payments = Nothing

        Try

            oPagoRecibido = p_oCompany.GetBusinessObject(intTipoPago)

            If oPagoRecibido.GetByKey(p_intPagoRecibido) Then

                Return oPagoRecibido

            End If

        Catch ex As Exception

            Throw ex

        End Try

        Return Nothing

    End Function

    Public Shared Function ObtieneFormatoFecha(ByVal SBO_Application As SAPbouiCOM.Application, ByVal p_oCompany As SAPbobsCOM.Company) As String
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oCompanyAdminInfo As AdminInfo
        Dim separador As String
        Dim formato As String

        oCompanyService = p_oCompany.GetCompanyService()
        oCompanyAdminInfo = oCompanyService.GetAdminInfo()
        separador = oCompanyAdminInfo.DateSeparator
        Select Case oCompanyAdminInfo.DateTemplate
            Case BoDateTemplate.dt_DDMMYY
                formato = String.Format("dd{0}MM{0}yy", separador)
            Case BoDateTemplate.dt_DDMMCCYY
                formato = String.Format("dd{0}MM{0}yyyy", separador)
            Case BoDateTemplate.dt_MMDDYY
                formato = String.Format("MM{0}dd{0}yy", separador)
            Case BoDateTemplate.dt_MMDDCCYY
                formato = String.Format("MM{0}dd{0}yyyy", separador)
            Case BoDateTemplate.dt_CCYYMMDD
                formato = String.Format("yyyy{0}MM{0}dd", separador)
            Case BoDateTemplate.dt_DDMonthYYYY
                formato = String.Format("dd{0}MMMM{0}yy", separador)
            Case Else
                Throw New InvalidOperationException("El formato de la fecha especificada para la compañia no es válido.")
        End Select
        Return formato
    End Function

    Shared Function ValidaExisteDataTable(ByRef p_form As SAPbouiCOM.Form, ByVal strDtName As String) As Boolean
        Dim ExisteDataTable As Boolean = False
        If p_form.DataSources.DataTables.Count > 0 Then
            For i As Integer = 0 To p_form.DataSources.DataTables.Count - 1
                If p_form.DataSources.DataTables.Item(i).UniqueID = strDtName Then
                    ExisteDataTable = True
                End If
            Next
        End If
        Return ExisteDataTable
    End Function

End Class
