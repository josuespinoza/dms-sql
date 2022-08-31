' Clase para manejo de las compras en el proceso de ventas
' manejo de compra de accesorios asi como tramites desde 
' el contrato de ventas

Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports System.Collections.Generic
Imports SAPbouiCOM

Public Class ComprasEnProcesoVentas

#Region "Definiciones"
    
    Private m_oCompany As SAPbobsCOM.Company
    Private m_SBOApplication As SAPbouiCOM.Application

    Private mc_intOrdenDeCompra As Integer = 142
    Private dtOrdenesCompra As SAPbouiCOM.DataTable
    
    Dim strCampoOrden As String = ""
    Dim strCodArticulo As String = ""
    Private PermiteBorrarLinea As Boolean
    Private intLineas As Integer = 0

    'variables para propiedades 
    Private _CancelarOC As Boolean
    Private _FormOrdenesCompra As SAPbouiCOM.Form
    Private _ListaAccesorios As List(Of String)
    Private _NumLinea As Integer
    Private _htAccesorios As New Hashtable

#End Region

#Region "Propiedades"

    Public Property CancelarOc As Boolean
        Get
            Return _CancelarOC
        End Get
        Set(ByVal value As Boolean)
            _CancelarOC = value
        End Set
    End Property

    Public Property FormOrdenesCompra As Form
        Get
            Return _FormOrdenesCompra
        End Get
        Set(ByVal value As Form)
            _FormOrdenesCompra = value
        End Set
    End Property

    Public Property ListaAccesorios As List(Of String)
        Get
            Return _ListaAccesorios
        End Get
        Set(ByVal value As List(Of String))
            _ListaAccesorios = value
        End Set
    End Property

    Public Property NumLinea As Integer
        Get
            Return _NumLinea
        End Get
        Set(ByVal value As Integer)
            _NumLinea = value
        End Set
    End Property

    Public Property htAccesorios As Hashtable
        Get
            Return _htAccesorios
        End Get
        Set(ByVal value As Hashtable)
            _htAccesorios = value
        End Set
    End Property

#End Region

#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company,
                   ByVal SBO_Application As Application)
        'objeto Compania de SAP
        m_oCompany = ocompany
        m_SBOApplication = SBO_Application
        CancelarOc = False

    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Manejo del evento Carga del formulario
    ''' </summary>
    ''' <param name="FormUID">Identificador para el formulario de ordenes de cmpra</param>
    ''' <param name="pVal">Evento Item Press para multiples manejos </param>
    ''' <param name="BubbleEvent">Variable  para detencion de la ejecucion de la aplicacion</param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub ManejaEventoLoad(ByVal FormUID As String, _
                                 ByRef pVal As SAPbouiCOM.ItemEvent, _
                                 ByRef BubbleEvent As Boolean)
        Try
            Dim strNoContratoVentas As String = ""
            'valida qeu el formulario se de tipo orden de compra 
            If pVal.FormTypeEx = mc_intOrdenDeCompra Then

                'Before Action
                If pVal.BeforeAction Then
                    'obtengo el form de ordenes de compra 
                    FormOrdenesCompra = m_SBOApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                    'hastable para manejo de eliminacion 
                    htAccesorios = New Hashtable
                    For x As Integer = 1 To FormOrdenesCompra.DataSources.DBDataSources.Item("POR1").Size - 1
                        htAccesorios.Add(x, FormOrdenesCompra.DataSources.DBDataSources.Item("POR1").GetValue("ItemCode", x - 1).Trim)
                    Next

                    strNoContratoVentas = FormOrdenesCompra.DataSources.DBDataSources.Item("OPOR").GetValue("U_SCGD_NoCVta", 0)
                    strNoContratoVentas = strNoContratoVentas.Trim()

                    If Not String.IsNullOrEmpty(strNoContratoVentas) Then PermiteBorrarLinea = True Else PermiteBorrarLinea = False
                    'cantidad de lineas en orden de compra 
                    intLineas = htAccesorios.Count
                    'agrego datatable en el Form 
                    dtOrdenesCompra = FormOrdenesCompra.DataSources.DataTables.Add("dtOC")

                End If 'Before Action 
            End If 'form Orden Compra

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try

    End Sub

    ''' <summary>
    ''' Manejo de los eventos de tipo Item Pressed 
    ''' </summary>
    ''' <param name="FormUID">Identificador para el formulario de ordenes de cmpra</param>
    ''' <param name="pVal">Evento Item Press para multiples manejos </param>
    ''' <param name="BubbleEvent">Variable  para detencion de la ejecucion de la aplicacion</param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)>
    Public Sub ManejadorEventosItemPressed(ByVal FormUID As String, _
                                           ByVal pVal As ItemEvent, _
                                           ByVal BubbleEvent As Boolean)
        Try
            Dim oForm As SAPbouiCOM.Form
            oForm = m_SBOApplication.Forms.Item(FormUID)

            If pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case "1"
                        If oForm IsNot Nothing _
                            And ListaAccesorios IsNot Nothing Then
                            If oForm.Mode = BoFormMode.fm_UPDATE_MODE _
                                And ListaAccesorios.Count > 0 Then
                                'elimina los articulos ingresados en la lista
                                BorraLineasOrdenCompra(oForm, BubbleEvent)
                            End If
                        End If
                    Case "38"
                        NumLinea = pVal.Row
                End Select 'ItemUID
            End If 'Action Success 

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
        End Try
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Cancela la referencia de un accesorio del contrtao de ventas con 
    ''' una orden de compra
    ''' </summary>
    ''' <param name="oForm">Objeto Form de la Orden de Compra</param>
    ''' <remarks></remarks>
    Public Sub BorraReferenciaOrdenConContVta()

        Try

            Dim strNoContratoVentas As String
            Dim strNoOrdenCompra As String
            Dim strConsulta As String = ""
            Dim strDocEntryOC As String = ""
            Dim strTipo As String = ""

            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oGeneralDataParams As SAPbobsCOM.GeneralDataParams
            Dim oDataChilds As SAPbobsCOM.GeneralDataCollection
            Dim oChild As SAPbobsCOM.GeneralData

            'docNum de la orden de compra
            strNoOrdenCompra = FormOrdenesCompra.DataSources.DBDataSources.Item("OPOR").GetValue("DocNum", 0)
            strNoOrdenCompra.Trim()

            strTipo = FormOrdenesCompra.DataSources.DBDataSources.Item("OPOR").GetValue("U_SCGD_TipoArt", 0)
            strTipo = strTipo.Trim()

            strConsulta = String.Format("SELECT DocEntry FROM OPOR  WHERE DocNum = '{0}'", strNoOrdenCompra)
            dtOrdenesCompra.ExecuteQuery(strConsulta)
            'docentry de la orden de compra, 
            'para buscarla en las lineas del contrato de ventas
            strDocEntryOC = dtOrdenesCompra.GetValue("DocEntry", 0)
            strDocEntryOC = strDocEntryOC.Trim()

            'No de contrato de ventas asociado en la Orden de Compra
            strNoContratoVentas = FormOrdenesCompra.DataSources.DBDataSources.Item("OPOR").GetValue("U_SCGD_NoCVta", 0)
            strNoContratoVentas = strNoContratoVentas.Trim()

            If Not String.IsNullOrEmpty(strNoContratoVentas) Then

                'Company service y general service  
                oCompanyService = m_oCompany.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_CVT")

                'Get UDO record
                oGeneralDataParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                'Obtengo el contrato de ventas pro medio del docentry
                oGeneralDataParams.SetProperty("DocEntry", strNoContratoVentas)
                oGeneralData = oGeneralService.GetByParams(oGeneralDataParams)
                oDataChilds = Nothing

                Select Case strTipo
                    Case "7"
                        oDataChilds = oGeneralData.Child("SCGD_ACCXCONT")
                        strCampoOrden = "U_Ord_Acc"
                    Case "9"
                        oDataChilds = oGeneralData.Child("SCGD_TRAMXCONT")
                        strCampoOrden = "U_Ord_Comp"
                End Select

                'si se cancela toda la orden o unicamente una linea 

                If oDataChilds IsNot Nothing And
                    Not String.IsNullOrEmpty(strCampoOrden) Then
                    'agrego la linea
                    For Each oChild In oDataChilds
                        'DocEntry de la Orden de Compra
                        If oChild.GetProperty(strCampoOrden) = strDocEntryOC Then
                            oChild.SetProperty(strCampoOrden, String.Empty)
                            oChild.SetProperty("U_Comprar", "Y")
                        End If
                    Next
                End If


                'Add the new row, including children, to database
                oGeneralService.Update(oGeneralData)

            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
        Finally
            Me.CancelarOc = False
            strCampoOrden = String.Empty
        End Try

    End Sub

    ''' <summary>
    ''' Borra los accesorios ingresados en la lista previa 
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <remarks></remarks>
    Public Sub BorraLineasOrdenCompra(ByVal oForm As SAPbouiCOM.Form, ByVal BubbleEvent As Boolean)

        Try

            Dim strNoContratoVentas As String
            Dim strNoOrdenCompra As String
            Dim strConsulta As String = ""
            Dim strDocEntryOC As String = ""
            'tipo Accesorio o Tramite
            Dim strTipo As String = ""

            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oGeneralDataParams As SAPbobsCOM.GeneralDataParams
            Dim oDataChilds As SAPbobsCOM.GeneralDataCollection
            Dim oChild As SAPbobsCOM.GeneralData

            'docNum de la orden de compra
            strNoOrdenCompra = oForm.DataSources.DBDataSources.Item("OPOR").GetValue("DocNum", 0)
            strNoOrdenCompra.Trim()
            'tipo de OC = Tramite o Accesorio
            strTipo = oForm.DataSources.DBDataSources.Item("OPOR").GetValue("U_SCGD_TipoArt", 0)
            strTipo = strTipo.Trim()

            strConsulta = String.Format("SELECT DocEntry FROM OPOR  WHERE DocNum = '{0}'", strNoOrdenCompra)
            dtOrdenesCompra.ExecuteQuery(strConsulta)
            'docentry de la orden de compra, para buscarla en las lineas del contrato de ventas
            strDocEntryOC = dtOrdenesCompra.GetValue("DocEntry", 0)
            strDocEntryOC = strDocEntryOC.Trim()

            'No de contrato de ventas asociado en la Orden de Compra
            strNoContratoVentas = oForm.DataSources.DBDataSources.Item("OPOR").GetValue("U_SCGD_NoCVta", 0)
            strNoContratoVentas = strNoContratoVentas.Trim()

            If Not String.IsNullOrEmpty(strNoContratoVentas) Then

                'Company service y general service  
                oCompanyService = m_oCompany.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_CVT")

                'Get UDO record
                oGeneralDataParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                'Obtengo el contrato de ventas pro medio del docentry
                oGeneralDataParams.SetProperty("DocEntry", strNoContratoVentas)
                oGeneralData = oGeneralService.GetByParams(oGeneralDataParams)
                oDataChilds = Nothing

                Select Case strTipo
                    Case "7"
                        oDataChilds = oGeneralData.Child("SCGD_ACCXCONT")
                        strCampoOrden = "U_Ord_Acc"
                        strCodArticulo = "U_Acc"
                    Case "9"
                        oDataChilds = oGeneralData.Child("SCGD_TRAMXCONT")
                        strCampoOrden = "U_Ord_Comp"
                        strCodArticulo = "U_Cod_Tram"
                End Select

                If oDataChilds IsNot Nothing And
                    Not String.IsNullOrEmpty(strCampoOrden) And
                    Not String.IsNullOrEmpty(strCodArticulo) Then
                    For Each strAccesorio As String In ListaAccesorios
                        'agrego la linea
                        For Each oChild In oDataChilds
                            'DocEntry de la Orden de Compra
                            If oChild.GetProperty(strCampoOrden) = strDocEntryOC And
                                oChild.GetProperty(strCodArticulo) = strAccesorio Then
                                oChild.SetProperty(strCampoOrden, String.Empty)
                                oChild.SetProperty("U_Comprar", "Y")
                            End If
                        Next
                    Next
                End If

                'Add the new row, including children, to database
                oGeneralService.Update(oGeneralData)
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
        Finally
            Me.CancelarOc = False
            Me.ListaAccesorios = Nothing
            strCampoOrden = String.Empty
            strCodArticulo = String.Empty
        End Try

    End Sub

    ''' <summary>
    ''' Ingresa el codigo del accesorio a eliminar en una lista que 
    ''' luego se recorre para realizar la eliminacion de ordenes en el 
    ''' contrato de ventas
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub IngresaListaAccEliminar(ByVal BubbleEvent As Boolean)
        Try
            Dim strArticulo As String
            'valida que existan mas de una linea a eliminar
            If NumLinea > 0 Then

                strArticulo = htAccesorios(NumLinea)
                If ListaAccesorios Is Nothing Then
                    ListaAccesorios = New List(Of String)
                End If
                'agrego el accesorio o tramite a la lista a eliminar
                ListaAccesorios.Add(strArticulo)
                intLineas = intLineas - 1
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBOApplication)
        Finally
            NumLinea = 0
        End Try
    End Sub

    ''' <summary>
    ''' Valida si la orden de compra posee un contrato de ventas asociado
    ''' para validar que no pueda borrar la ultima linea de la orden de compra
    ''' </summary>
    ''' <param name="oForm">Formulario de la Orden de Compra</param>
    ''' <returns>False = Si posee un contrato asociado y no posee mas de 2 lineas</returns>
    ''' <remarks></remarks>
    Public Function ValidaNumeroLineas(ByVal oForm As SAPbouiCOM.Form) As Boolean
        Try
            Dim strNoContratoVentas As String = ""
            'obtiene el numero de contrato de venta 
            strNoContratoVentas = oForm.DataSources.DBDataSources.Item("OPOR").GetValue("U_SCGD_NoCVta", 0)
            strNoContratoVentas = strNoContratoVentas.Trim()
            'si posee un contrato de venta asociado si permite borrar linea 
            If Not String.IsNullOrEmpty(strNoContratoVentas) Then PermiteBorrarLinea = True Else PermiteBorrarLinea = False

            If intLineas < 2 And PermiteBorrarLinea Then
                m_SBOApplication.StatusBar.SetText(My.Resources.Resource.NumLineasMinimoOC, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                Return False
            End If
            Return True

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBOApplication)
        End Try
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

#End Region


End Class
