Option Explicit On

Imports System.Collections.Generic
Imports System.Linq
Imports DMS_Connector.Business_Logic

Public Class AgregarDimensionLineasDocumentosCls

    Private m_oCompany As SAPbobsCOM.Company
    Private m_SBO_Application As SAPbouiCOM.Application
    
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                      ByVal SBOAplication As SAPbouiCOM.Application)

        m_oCompany = ocompany
        m_SBO_Application = SBOAplication


    End Sub
    
    Public Function DatatableConfiguracionDocumentosDimensiones() As Hashtable

        Dim ListaDocumentosConfiguracion As Hashtable = New Hashtable
        
        For Each row As DataRow In Utilitarios.EjecutarConsultaDataTable("Select ""Code"", ""U_Valor"" from ""@SCGD_DIMEN_CONF"" ").Rows
            ListaDocumentosConfiguracion.Add(row.Item("Code").ToString.Trim(), row.Item("U_Valor").ToString.Trim)
        Next

        Return ListaDocumentosConfiguracion

    End Function

    Public Function DatatableConfiguracionDocumentosDimensionesOT(ByVal p_form As SAPbouiCOM.Form) As List(Of LineasConfiguracionOT)

        Dim ListaDocumentosConfiguracionOrdenTrabajo As List(Of LineasConfiguracionOT) = New List(Of LineasConfiguracionOT)()

        For Each tipoOt As TipoOT In DMS_Connector.Configuracion.TipoOt.OrderBy(Function(tipOT) tipOT.Code)
            ListaDocumentosConfiguracionOrdenTrabajo.Add(New LineasConfiguracionOT() With {
                                                         .TipoOT = tipoOt.Code,
                                                         .UsaDim = tipoOt.U_UsaDim, _
                                                         .UsaDimAEM = tipoOt.U_UsaDimAEM,
                                                         .UsaDimAFP = tipoOt.U_UsaDimAFP
                                                     })
        Next
        
        Return ListaDocumentosConfiguracionOrdenTrabajo

    End Function

    Public Function DatatableDimensionesContables(ByVal p_form As SAPbouiCOM.Form, ByVal p_TipoInventario As String, ByVal p_Marca As String, ByRef p_DT As SAPbouiCOM.DataTable) As SAPbouiCOM.DataTable
        Dim mc_strDataTableDimensiones As String = "DimensionesContablesDMS"

        p_DT = p_form.DataSources.DataTables.Item(mc_strDataTableDimensiones)

        If p_DT.Rows.Count > 0 Then
            p_DT.Rows.Clear()
        End If

        If p_DT.Columns.Count > 0 Then
            For index As Integer = 0 To p_DT.Columns.Count-1
                p_DT.Columns.Remove(0)
            Next
        End If
        For index As Integer = 1 To 5
            p_DT.Columns.Add(String.Format("U_Dim{0}", index), SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 80)
        Next
        If DMS_Connector.Configuracion.Dimensiones.Any(Function(dimensiones) dimensiones.U_Tip_Inv.Trim.Equals(p_TipoInventario)) Then
            If DMS_Connector.Configuracion.Dimensiones.FirstOrDefault(Function(dimensiones) dimensiones.U_Tip_Inv.Trim.Equals(p_TipoInventario)).Dimensiones_Lineas.Any(Function(lineas) lineas.U_CodMar.Trim.Equals(p_Marca)) Then
                p_DT.Rows.Add()
                With DMS_Connector.Configuracion.Dimensiones.FirstOrDefault(Function(dimensiones) dimensiones.U_Tip_Inv.Trim.Equals(p_TipoInventario)).Dimensiones_Lineas.FirstOrDefault(Function(lineas) lineas.U_CodMar.Trim.Equals(p_Marca))
                    p_DT.SetValue("U_Dim1", p_DT.Rows.Count - 1, .U_Dim1)
                    p_DT.SetValue("U_Dim2", p_DT.Rows.Count - 1, .U_Dim2)
                    p_DT.SetValue("U_Dim3", p_DT.Rows.Count - 1, .U_Dim3)
                    p_DT.SetValue("U_Dim4", p_DT.Rows.Count - 1, .U_Dim4)
                    p_DT.SetValue("U_Dim5", p_DT.Rows.Count - 1, .U_Dim5)
                End With
            End If
        End If

        Return p_DT

    End Function
    Public Function DatatableDimensionesContablesOrdenTrabajo(ByVal p_form As SAPbouiCOM.Form, ByVal p_CodigoSucursal As String, ByVal p_Marca As String, ByRef p_DT As SAPbouiCOM.DataTable) As SAPbouiCOM.DataTable
        Dim mc_strDataTableDimensionesOT As String = "DimensionesContablesDMSOT"
        p_DT = p_form.DataSources.DataTables.Item(mc_strDataTableDimensionesOT)
        If p_DT.Columns.Count > 0 Then
            p_DT.Rows.Clear()
            For index As Integer = 0 To p_DT.Columns.Count - 1
                p_DT.Columns.Remove(0)
            Next
        End If
        For index As Integer = 1 To 5
            p_DT.Columns.Add(String.Format("U_Dim{0}", index), SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 80)
        Next
        If DMS_Connector.Configuracion.DimensionesOT.Any(Function(dimensiones) dimensiones.U_CodSuc.Trim.Equals(p_CodigoSucursal)) Then
            If DMS_Connector.Configuracion.DimensionesOT.FirstOrDefault(Function(dimensiones) dimensiones.U_CodSuc.Trim.Equals(p_CodigoSucursal)).DimensionesOT_Lineas.Any(Function(lineas) lineas.U_CodMar.Trim.Equals(p_Marca)) Then
                p_DT.Rows.Add()
                With DMS_Connector.Configuracion.DimensionesOT.FirstOrDefault(Function(dimensiones) dimensiones.U_CodSuc.Trim.Equals(p_CodigoSucursal)).DimensionesOT_Lineas.FirstOrDefault(Function(lineas) lineas.U_CodMar.Trim.Equals(p_Marca))
                    p_DT.SetValue("U_Dim1", p_DT.Rows.Count - 1, .U_Dim1)
                    p_DT.SetValue("U_Dim2", p_DT.Rows.Count - 1, .U_Dim2)
                    p_DT.SetValue("U_Dim3", p_DT.Rows.Count - 1, .U_Dim3)
                    p_DT.SetValue("U_Dim4", p_DT.Rows.Count - 1, .U_Dim4)
                    p_DT.SetValue("U_Dim5", p_DT.Rows.Count - 1, .U_Dim5)
                End With
            End If
        End If

        Return p_DT

    End Function

    Public Function DatatableDimensionesContablesDMS(ByVal p_TipoInventario As String, ByVal p_Marca As String) As System.Data.DataTable

        Dim oDataTableDimensiones As DataTable
        oDataTableDimensiones = New DataTable()
        Dim drRow As DataRow
        For index As Integer = 1 To 5
            oDataTableDimensiones.Columns.Add(New DataColumn(String.Format("U_Dim{0}", index), Type.GetType("System.String")))
        Next
        If DMS_Connector.Configuracion.Dimensiones.Any(Function(dimensiones) dimensiones.U_Tip_Inv.Trim.Equals(p_TipoInventario)) Then
            If DMS_Connector.Configuracion.Dimensiones.FirstOrDefault(Function(dimensiones) dimensiones.U_Tip_Inv.Trim.Equals(p_TipoInventario)).Dimensiones_Lineas.Any(Function(lineas) lineas.U_CodMar.Trim.Equals(p_Marca)) Then
                drRow = oDataTableDimensiones.NewRow()
                With DMS_Connector.Configuracion.Dimensiones.FirstOrDefault(Function(dimensiones) dimensiones.U_Tip_Inv.Trim.Equals(p_TipoInventario)).Dimensiones_Lineas.FirstOrDefault(Function(lineas) lineas.U_CodMar.Trim.Equals(p_Marca))
                    drRow("U_Dim1") = .U_Dim1
                    drRow("U_Dim2") = .U_Dim2
                    drRow("U_Dim3") = .U_Dim3
                    drRow("U_Dim4") = .U_Dim4
                    drRow("U_Dim5") = .U_Dim5
                End With
                oDataTableDimensiones.Rows.Add(drRow)
            End If
        End If

        Return oDataTableDimensiones

    End Function


    Public Sub AgregarDimensionesLineasDocumentos(ByRef p_LineasDocumentos As SAPbobsCOM.Document_Lines, ByRef p_DataTableDimension As SAPbouiCOM.DataTable)
        p_LineasDocumentos.CostingCode = p_DataTableDimension.GetValue(0, 0)
        p_LineasDocumentos.CostingCode2 = p_DataTableDimension.GetValue(1, 0)
        p_LineasDocumentos.CostingCode3 = p_DataTableDimension.GetValue(2, 0)
        p_LineasDocumentos.CostingCode4 = p_DataTableDimension.GetValue(3, 0)
        p_LineasDocumentos.CostingCode5 = p_DataTableDimension.GetValue(4, 0)
    End Sub


    Public Sub AgregarDimensionesLineasAsiento(ByRef p_LineasJournalEntries As SAPbobsCOM.JournalEntries_Lines, ByRef p_DataTableDimension As System.Data.DataTable, _
                                               Optional ByVal p_DataTableDimensionSAP As SAPbouiCOM.DataTable = Nothing)

        Dim row As System.Data.DataRow

        If p_DataTableDimensionSAP Is Nothing Then

            row = p_DataTableDimension.Rows(0)

            If Not row.IsNull(0) Then
                p_LineasJournalEntries.CostingCode = row.Item(0)
            Else
                p_LineasJournalEntries.CostingCode = String.Empty
            End If

            If Not row.IsNull(1) Then
                p_LineasJournalEntries.CostingCode2 = row.Item(1)
            Else
                p_LineasJournalEntries.CostingCode2 = String.Empty

            End If

            If Not row.IsNull(2) Then
                p_LineasJournalEntries.CostingCode3 = row.Item(2)
            Else
                p_LineasJournalEntries.CostingCode3 = String.Empty

            End If

            If Not row.IsNull(3) Then
                p_LineasJournalEntries.CostingCode4 = row.Item(3)
            Else
                p_LineasJournalEntries.CostingCode4 = String.Empty
            End If

            If Not row.IsNull(4) Then
                p_LineasJournalEntries.CostingCode5 = row.Item(4)
            Else
                p_LineasJournalEntries.CostingCode5 = String.Empty
            End If

        Else

            p_LineasJournalEntries.CostingCode = p_DataTableDimensionSAP.GetValue(0, 0)
            p_LineasJournalEntries.CostingCode2 = p_DataTableDimensionSAP.GetValue(1, 0)
            p_LineasJournalEntries.CostingCode3 = p_DataTableDimensionSAP.GetValue(2, 0)
            p_LineasJournalEntries.CostingCode4 = p_DataTableDimensionSAP.GetValue(3, 0)
            p_LineasJournalEntries.CostingCode5 = p_DataTableDimensionSAP.GetValue(4, 0)
        End If

    End Sub

    Public Function ValidacionAsientosDimensiones(ByVal p_ListaConfiguraciones As List(Of LineasConfiguracionOT), ByVal p_tipoOT As String, Optional ByVal p_AsientoEM As Boolean = False, Optional ByVal p_AsientoFP As Boolean = False) As String
        Dim strValorDimension As String
        Dim m_strValorDimensionSi As String = "Y"
        Dim m_strValorDimensionNo As String = "N"

        For k As Integer = 0 To p_ListaConfiguraciones.Count - 1

            Dim strTipoOTLista As String = p_ListaConfiguraciones.Item(k).TipoOT

            If p_tipoOT = strTipoOTLista Then

                If Not String.IsNullOrEmpty(p_ListaConfiguraciones.Item(k).UsaDim) Then

                    If p_ListaConfiguraciones.Item(k).UsaDim = "Y" Then

                        If p_AsientoEM Then

                            If p_ListaConfiguraciones.Item(k).UsaDimAEM = "Y" Then
                                Return m_strValorDimensionSi
                            Else
                                Return m_strValorDimensionNo
                            End If

                        ElseIf p_AsientoFP Then

                            If p_ListaConfiguraciones.Item(k).UsaDimAFP = "Y" Then
                                Return m_strValorDimensionSi
                            Else
                                Return m_strValorDimensionNo
                            End If

                        Else

                            strValorDimension = p_ListaConfiguraciones.Item(k).UsaDim
                            Return strValorDimension

                        End If

                    Else
                        Return m_strValorDimensionNo

                    End If
                Else

                    Return m_strValorDimensionNo

                End If
            End If
        Next
    End Function


    Public Function CargarDimensionesOrdenTrabajo(ByVal p_oForm As SAPbouiCOM.Form, ByRef p_oListaNoOrden As Generic.List(Of String), ByRef p_oListaServExterno As List(Of ListaLineasDocumento))
        
        Dim blnAsignaDimension As Boolean = False
        Dim ListaDocumentosConfiguracionOrdenTrabajo = New List(Of LineasConfiguracionOT)()
        Dim strAsignaDimension As String = "N"
        Dim blnAsignaDimenPreviaCargada As Boolean = False
        Dim oDataTableDimensionesContablesDMS As SAPbouiCOM.DataTable
        Try
            ListaDocumentosConfiguracionOrdenTrabajo = DatatableConfiguracionDocumentosDimensionesOT(p_oForm)
            For Each row1 As String In p_oListaNoOrden
                'Valida aplica aiento por tipo OT y tipo transacción
                blnAsignaDimenPreviaCargada = False
                strAsignaDimension = "N"

                For Each row2 As ListaLineasDocumento In p_oListaServExterno
                    If row1 = row2.NoOrden And row2.AplicadoCargaDimensiones = False Then
                        If Not blnAsignaDimenPreviaCargada Then
                            strAsignaDimension = ValidacionAsientosDimensiones(ListaDocumentosConfiguracionOrdenTrabajo, row2.TipoOT, False, True)
                            If Not String.IsNullOrEmpty(strAsignaDimension) Then
                                If strAsignaDimension = "Y" Then
                                    oDataTableDimensionesContablesDMS = DatatableDimensionesContablesOrdenTrabajo(p_oForm, row2.IdSucursal, row2.CodMarca, oDataTableDimensionesContablesDMS)
                                    If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                                        blnAsignaDimension = True
                                    End If
                                End If
                            End If
                            blnAsignaDimenPreviaCargada = True
                        End If
                        If blnAsignaDimension Then
                            If Not String.IsNullOrEmpty(oDataTableDimensionesContablesDMS.GetValue(0, 0)) Then
                                row2.CostingCode = oDataTableDimensionesContablesDMS.GetValue(0, 0)
                            End If
                            If Not String.IsNullOrEmpty(oDataTableDimensionesContablesDMS.GetValue(1, 0)) Then
                                row2.CostingCode2 = oDataTableDimensionesContablesDMS.GetValue(1, 0)
                            End If
                            If Not String.IsNullOrEmpty(oDataTableDimensionesContablesDMS.GetValue(2, 0)) Then
                                row2.CostingCode3 = oDataTableDimensionesContablesDMS.GetValue(2, 0)
                            End If
                            If Not String.IsNullOrEmpty(oDataTableDimensionesContablesDMS.GetValue(3, 0)) Then
                                row2.CostingCode4 = oDataTableDimensionesContablesDMS.GetValue(3, 0)
                            End If
                            If Not String.IsNullOrEmpty(oDataTableDimensionesContablesDMS.GetValue(4, 0)) Then
                                row2.CostingCode5 = oDataTableDimensionesContablesDMS.GetValue(4, 0)
                            End If
                            row2.AplicadoCargaDimensiones = True
                        End If
                    End If
                Next
            Next
        Catch ex As Exception
        End Try
    End Function
#Region "Nuevos metodos"
    Public Sub ObtieneConfiguracionDimensionesOT(ByRef p_oConfiguracionOrdenTrabajoList As ConfiguracionOrdenTrabajo_List)
        Try
            Dim oDataTableConfiguracionOT As System.Data.DataTable = Nothing
            Dim oDataRowConfiguracionOT As System.Data.DataRow
            Dim oConfiguracionOrdenTrabajo As ConfiguracionOrdenTrabajo
            '******************************************************************************
            '******************** Carga Configuración de tabla Tipo Orden*******
            '******************************************************************************
            oDataTableConfiguracionOT = Utilitarios.EjecutarConsultaDataTable(String.Format("Select Code, U_UsaDim , U_UsaDimAEM , U_UsaDimAFP from dbo.[@SCGD_TIPO_ORDEN] with (nolock)"),
                                                                                    m_oCompany.CompanyDB,
                                                                                    m_oCompany.Server)
            '******************************************************************************
            '******************** Recorre configuraciones y agrega a objeto list*******
            '******************************************************************************
            For Each oDataRowConfiguracionOT In oDataTableConfiguracionOT.Rows
                oConfiguracionOrdenTrabajo = New ConfiguracionOrdenTrabajo()
                With oConfiguracionOrdenTrabajo
                    '************************ Carga Codigo OT**********************
                    If Not IsDBNull(oDataRowConfiguracionOT.Item("Code")) Then
                        .TipoOT = oDataRowConfiguracionOT.Item("Code").ToString.Trim()
                    End If
                    '************************Valida si usa dimensiones**********************
                    If Not IsDBNull(oDataRowConfiguracionOT.Item("U_UsaDim")) Then
                        If oDataRowConfiguracionOT.Item("U_UsaDim") = "Y" Then
                            .UsaDimensiones = True
                        Else
                            .UsaDimensiones = False
                        End If
                    Else
                        .UsaDimensiones = False
                    End If
                    '************************Valida si genera asientos para entradas de mercancia**********************
                    If Not IsDBNull(oDataRowConfiguracionOT.Item("U_UsaDimAEM")) Then
                        If oDataRowConfiguracionOT.Item("U_UsaDimAEM") = "Y" Then
                            .UsaDimensionAsientoEntradaMercancia = True
                        Else
                            .UsaDimensionAsientoEntradaMercancia = False
                        End If
                    Else
                        .UsaDimensionAsientoEntradaMercancia = False
                    End If
                    '************************Valida si genera asientos para facturaproveedor**********************
                    If Not IsDBNull(oDataRowConfiguracionOT.Item("U_UsaDimAFP")) Then
                        If oDataRowConfiguracionOT.Item("U_UsaDimAFP") = "Y" Then
                            .UsaDimensionAsientoFacturaProveedor = True
                        Else
                            .UsaDimensionAsientoFacturaProveedor = False
                        End If
                    Else
                        .UsaDimensionAsientoFacturaProveedor = False
                    End If
                End With
                p_oConfiguracionOrdenTrabajoList.Add(oConfiguracionOrdenTrabajo)
            Next
            If oDataTableConfiguracionOT.Rows.Count > 0 Then
                oDataTableConfiguracionOT.Clear()
                oDataTableConfiguracionOT = Nothing
            End If
        Catch ex As Exception
            m_SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub CargaCentrosCostoDimensionesOT(ByVal p_oSucursalList As Generic.List(Of String), _
                                              ByVal p_oCodigoMarcaList As Generic.List(Of String), _
                                              ByRef p_oDimensionesContablesList As DimensionesContables_List)
        Try
            '*************Declaración DataContract********
            Dim oDimensionesContables As DimensionesContables
            '*************Declaración variables********
            Dim oDataTableDimensionesContablesOT As System.Data.DataTable = Nothing
            Dim oDataRowDimensionesContablesOT As System.Data.DataRow
            Dim strIDSucursales As String = String.Empty
            Dim intContSucursalList As Integer = 0
            Dim intContSucursalTemporal As Integer = 0
            Dim strCodigoMarca As String = String.Empty
            Dim intContCodigoMarcaList As Integer = 0
            Dim intContCodigoMarcaTemporal As Integer = 0

            '******************************************************************************
            '******************** Carga Configuración de tabla Tipo Orden*******
            '******************************************************************************

            intContSucursalList = p_oSucursalList.Count()
            For Each rowSucursal As String In p_oSucursalList
                intContSucursalTemporal += 1
                If Not strIDSucursales.Contains(rowSucursal) Then
                    If intContSucursalTemporal = intContSucursalList Then
                        strIDSucursales = strIDSucursales & String.Format("'{0}'", rowSucursal)
                    Else
                        strIDSucursales = strIDSucursales & String.Format("'{0}', ", rowSucursal)
                    End If
                End If
            Next

            intContCodigoMarcaList = p_oCodigoMarcaList.Count()
            For Each rowCodigoMarca As String In p_oCodigoMarcaList
                intContCodigoMarcaTemporal += 1
                If Not strCodigoMarca.Contains(rowCodigoMarca) Then
                    If intContCodigoMarcaTemporal = intContCodigoMarcaList Then
                        strCodigoMarca = strCodigoMarca & String.Format("'{0}'", rowCodigoMarca)
                    Else
                        strCodigoMarca = strCodigoMarca & String.Format("'{0}', ", rowCodigoMarca)
                    End If
                End If
            Next

            If (strIDSucursales.Length > 0 And strCodigoMarca.Length > 0) Then
                strIDSucursales = strIDSucursales.Substring(0, strIDSucursales.Length - 0)
                strCodigoMarca = strCodigoMarca.Substring(0, strCodigoMarca.Length - 0)
                oDataTableDimensionesContablesOT = Utilitarios.EjecutarConsultaDataTable(String.Format("Select D.U_CodSuc,LD.U_CodMar , LD.U_Dim1 , LD.U_Dim2, LD.U_Dim3 , LD.U_Dim4 , LD.U_Dim5  from dbo.[@SCGD_DIMENSION_OT] D inner join dbo.[@SCGD_LINEAS_DIMENOT] LD on " & _
                                                                                                             "d.DocEntry = ld.DocEntry  where D.U_CodSuc in ({0})  And LD.U_CodMar in ({1})",
                                                                                                             strIDSucursales, strCodigoMarca),
                                                                                                              m_oCompany.CompanyDB,
                                                                                                              m_oCompany.Server)
            End If
            '******************************************************************************
            '******************** Recorre configuraciones y agrega a objeto list*******
            '******************************************************************************
            If Not oDataTableDimensionesContablesOT Is Nothing Then
                For Each oDataRowDimensionesContablesOT In oDataTableDimensionesContablesOT.Rows
                    oDimensionesContables = New DimensionesContables
                    With oDimensionesContables
                        '************************ Sucursal**********************
                        If Not IsDBNull(oDataRowDimensionesContablesOT.Item("U_CodSuc")) Then
                            .Sucursal = oDataRowDimensionesContablesOT.Item("U_CodSuc").ToString.Trim()
                        End If
                        '************************ Codigo Marca**********************
                        If Not IsDBNull(oDataRowDimensionesContablesOT.Item("U_CodMar")) Then
                            .CodigoMarca = oDataRowDimensionesContablesOT.Item("U_CodMar").ToString.Trim()
                        End If
                        '************************ Code1**********************
                        If Not IsDBNull(oDataRowDimensionesContablesOT.Item("U_Dim1")) Then
                            .CostingCode = oDataRowDimensionesContablesOT.Item("U_Dim1").ToString.Trim()
                        End If
                        '************************ Code2**********************
                        If Not IsDBNull(oDataRowDimensionesContablesOT.Item("U_Dim2")) Then
                            .CostingCode2 = oDataRowDimensionesContablesOT.Item("U_Dim2").ToString.Trim()
                        End If
                        '************************ Code3**********************
                        If Not IsDBNull(oDataRowDimensionesContablesOT.Item("U_Dim3")) Then
                            .CostingCode3 = oDataRowDimensionesContablesOT.Item("U_Dim3").ToString.Trim()
                        End If
                        '************************ Code4**********************
                        If Not IsDBNull(oDataRowDimensionesContablesOT.Item("U_Dim4")) Then
                            .CostingCode4 = oDataRowDimensionesContablesOT.Item("U_Dim4").ToString.Trim()
                        End If
                        '************************ Code5**********************
                        If Not IsDBNull(oDataRowDimensionesContablesOT.Item("U_Dim5")) Then
                            .CostingCode5 = oDataRowDimensionesContablesOT.Item("U_Dim5").ToString.Trim()
                        End If
                    End With
                    p_oDimensionesContablesList.Add(oDimensionesContables)
                Next
                If oDataTableDimensionesContablesOT.Rows.Count > 0 Then
                    oDataTableDimensionesContablesOT.Clear()
                    oDataTableDimensionesContablesOT = Nothing
                End If
            End If
        Catch ex As Exception
            m_SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub AsignaDimensionesOTAsiento(ByRef p_oAsiento As Asiento,
                                          ByVal p_strIDSucursal As String,
                                          ByVal p_strCodMarca As String)
        Try
            If DMS_Connector.Configuracion.DimensionesOT.Any(Function(dimensiones) dimensiones.U_CodSuc.Trim.Equals(p_strIDSucursal)) Then
                If DMS_Connector.Configuracion.DimensionesOT.FirstOrDefault(Function(dimensiones) dimensiones.U_CodSuc.Trim.Equals(p_strIDSucursal)).DimensionesOT_Lineas.Any(Function(lineas) lineas.U_CodMar.Trim.Equals(p_strCodMarca)) Then
                    With DMS_Connector.Configuracion.DimensionesOT.FirstOrDefault(Function(dimensiones) dimensiones.U_CodSuc.Trim.Equals(p_strIDSucursal)).DimensionesOT_Lineas.FirstOrDefault(Function(lineas) lineas.U_CodMar.Trim.Equals(p_strCodMarca))
                        If Not String.IsNullOrEmpty(.U_Dim1) Then p_oAsiento.CostingCode = .U_Dim1
                        If Not String.IsNullOrEmpty(.U_Dim2) Then p_oAsiento.CostingCode2 = .U_Dim2
                        If Not String.IsNullOrEmpty(.U_Dim3) Then p_oAsiento.CostingCode3 = .U_Dim3
                        If Not String.IsNullOrEmpty(.U_Dim4) Then p_oAsiento.CostingCode4 = .U_Dim4
                        If Not String.IsNullOrEmpty(.U_Dim5) Then p_oAsiento.CostingCode5 = .U_Dim5
                    End With
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub AsignaDimensionesOTDocumento(ByRef p_LineasDocumentos As SAPbobsCOM.Document_Lines,
                                         ByVal p_strIDSucursal As String,
                                         ByVal p_strCodMarca As String)
        Try
            If DMS_Connector.Configuracion.DimensionesOT.Any(Function(dimensiones) dimensiones.U_CodSuc.Trim.Equals(p_strIDSucursal)) Then
                If DMS_Connector.Configuracion.DimensionesOT.FirstOrDefault(Function(dimensiones) dimensiones.U_CodSuc.Trim.Equals(p_strIDSucursal)).DimensionesOT_Lineas.Any(Function(lineas) lineas.U_CodMar.Trim.Equals(p_strCodMarca)) Then
                    With DMS_Connector.Configuracion.DimensionesOT.FirstOrDefault(Function(dimensiones) dimensiones.U_CodSuc.Trim.Equals(p_strIDSucursal)).DimensionesOT_Lineas.FirstOrDefault(Function(lineas) lineas.U_CodMar.Trim.Equals(p_strCodMarca))
                        If Not String.IsNullOrEmpty(.U_Dim1) Then p_LineasDocumentos.CostingCode = .U_Dim1
                        If Not String.IsNullOrEmpty(.U_Dim2) Then p_LineasDocumentos.CostingCode2 = .U_Dim2
                        If Not String.IsNullOrEmpty(.U_Dim3) Then p_LineasDocumentos.CostingCode3 = .U_Dim3
                        If Not String.IsNullOrEmpty(.U_Dim4) Then p_LineasDocumentos.CostingCode4 = .U_Dim4
                        If Not String.IsNullOrEmpty(.U_Dim5) Then p_LineasDocumentos.CostingCode5 = .U_Dim5
                    End With
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
#End Region
End Class

