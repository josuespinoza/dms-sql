Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.BLSBO
Imports System.Data.SqlClient

Namespace SCGDataAccess
    Public Class ClsClientesSBO

#Region "Declaraciones"

        Private Shared m_cnnSCGTaller As SqlClient.SqlConnection
        Private m_adpCliente As SqlClient.SqlDataAdapter

        Private objDAConexion As DAConexion
        'Constantes
        Private Const mc_strSelCodigo As String = "SCGTA_SP_SELCodigoCliente"

        Public Structure stcCliente

            Dim strCardCode As String
            Dim strCardName As String
            Dim strTelfCasa As String
            Dim strTelfOficina As String
            Dim strCelular As String
            Dim strFax As String
            Dim strCorreo As String
            Dim strDetalle As String
            Dim strRFC As String

        End Structure

#End Region

#Region "Constructor"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpCliente = New SqlClient.SqlDataAdapter
        End Sub


#End Region

#Region "Metodos"

        Public Function CrearUsuario(ByVal p_strCliente As String, ByVal p_strCodigo As String, ByVal p_strTelfCasa As String _
                                     , ByVal p_strTelfOficina As String, ByVal p_strCelular As String, ByVal p_strFax As String _
                                     , ByVal p_strCorreo As String, ByVal p_strDetalle As String, ByVal p_strfc As String, _
                                     ByVal p_strSitioWeb As String, ByVal p_strTipoSocio As String) As Long


            Dim oCliente As SAPbobsCOM.BusinessPartners
            Dim strError As String =  String.Empty
            Dim lngError As Long
            Dim lngResultado As Long


            oCliente = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
            oCliente.CardType = SAPbobsCOM.BoCardTypes.cCustomer
            If p_strTipoSocio = "Sociedades" Then
                oCliente.CompanyPrivate = SAPbobsCOM.BoCardCompanyTypes.cCompany
            Else
                oCliente.CompanyPrivate = SAPbobsCOM.BoCardCompanyTypes.cPrivate
            End If

            oCliente.CardCode = p_strCodigo
            oCliente.CardName = p_strCliente
            oCliente.Phone1 = p_strTelfCasa
            oCliente.Phone2 = p_strTelfOficina
            oCliente.Cellular = p_strCelular
            oCliente.Fax = p_strFax
            oCliente.EmailAddress = p_strCorreo
            oCliente.Notes = p_strDetalle
            oCliente.FederalTaxID = p_strfc
            oCliente.Website = p_strSitioWeb
            


            lngResultado = oCliente.Add()
            If (lngResultado <> 0) Then
                oCompany.GetLastError(lngError, strError)
                If (lngError <> -1) Then
                    'MsgBox("Error:" + Str(lngError) + "," + strError)
                    Throw New SCGCommon.ExceptionsSBO(lngError, strError)
                End If
                lngResultado = lngError

            End If
            Return lngResultado


        End Function

        Public Function ActualizarDatosUsuario(ByVal strCliente As String, ByVal strCodigo As String _
                                              , ByVal strTelfCasa As String, ByVal strTelfOficina As String _
                                              , ByVal strCelular As String, ByVal strFax As String _
                                              , ByVal strCorreo As String, ByVal strDetalle As String _
                                              , ByVal strRFC As String) As Long

            Dim oCliente As SAPbobsCOM.BusinessPartners
            Dim strError As String =  String.Empty
            Dim lngError As Long


            oCliente = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
            oCliente.CardType = SAPbobsCOM.BoCardTypes.cCustomer

            If (oCliente.GetByKey(strCodigo) = True) Then
                'Actualiza los campos
                oCliente.CardCode = strCodigo
                oCliente.CardName = strCliente
                oCliente.Phone1 = strTelfCasa
                oCliente.Phone2 = strTelfOficina
                oCliente.Cellular = strCelular
                oCliente.Fax = strFax
                oCliente.EmailAddress = strCorreo
                oCliente.Notes = strDetalle
                oCliente.FederalTaxID = strRFC

                Call oCliente.Update()
            End If

            'Revisa si ocurrió un error
            Call oCompany.GetLastError(lngError, strError)
            If (0 <> lngError) Then
                Throw New SCGCommon.ExceptionsSBO(lngError, strError)
                'MsgBox("Error:" + Str(lngError) + "," + strError)
            End If

            Return lngError

        End Function

        Public Function CargarCliente(ByVal p_strCardCode As String) As stcCliente

            Dim oCliente As SAPbobsCOM.BusinessPartners
            Dim strError As String =  String.Empty
            Dim lngError As Long
            Dim objCliente As New stcCliente


            oCliente = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
            oCliente.CardType = SAPbobsCOM.BoCardTypes.cCustomer

            If (oCliente.GetByKey(p_strCardCode) = True) Then
                'Actualiza los campos
                objCliente.strCardCode = oCliente.CardCode
                objCliente.strCardName = oCliente.CardName
                objCliente.strTelfCasa = oCliente.Phone1
                objCliente.strTelfOficina = oCliente.Phone2
                objCliente.strCelular = oCliente.Cellular
                objCliente.strFax = oCliente.Fax
                objCliente.strCorreo = oCliente.EmailAddress
                objCliente.strDetalle = oCliente.Notes
                objCliente.strRFC = oCliente.FederalTaxID

                'Call oCliente.Update()
            Else
                Call oCompany.GetLastError(lngError, strError)
                If (0 <> lngError) Then
                    Throw New SCGCommon.ExceptionsSBO(lngError, strError)
                    'MsgBox("Error:" + Str(lngError) + "," + strError)
                End If
            End If
            Return objCliente
            'Revisa si ocurrió un error
            

        End Function

        Public Function ObtenerCodCliente() As String

            Dim strCodCliente As String
            Dim cmd As New SqlClient.SqlCommand

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                cmd.CommandText = mc_strSelCodigo
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Connection = m_cnnSCGTaller

                strCodCliente = cmd.ExecuteScalar
                Return strCodCliente

            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

#End Region

    End Class
End Namespace
