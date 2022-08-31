Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports System.Math
Imports System.Data.SqlClient

Namespace SCG_User_Interface

    Partial Public Class frmOrden
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

#Region "Declaraciones"

#Region "Constantes"

        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strID As String = "ID"
        Private Const mc_intNoRepuesto As String = "NoRepuesto"
        Private Const mc_intNoPiezaPrincipal As String = "NoPiezaPrincipal"
        Private Const mc_intNoSeccion As String = "NoSeccion"
        Private Const mc_strComponente As String = "ItemName"
        Private Const mc_strSeccion As String = "Seccion"
        Private Const mc_strPiezaPrincipal As String = "PiezaPrincipal"
        Private Const mc_strEstado As String = "EstadoRep"
        Private Const mc_strInformacion As String = "Informacion"
        Private Const mc_strPrecioAcordado As String = "PrecioAcordado"
        Private Const mc_intCantEstado As String = "CantidadEstado"
        Private Const mc_intCodEstado As String = "codEstadoRep"
        Private Const mc_strTableName As String = "SCGTA_TB_RepuestosXOrden"
        Private Const mc_blnCheck As String = "Check"
        Private Const mc_blnBodega As String = "Bodega"
        Private Const mc_intAdicional As String = "Adicional"
        Private Const mc_intNoAdicional As String = "NoAdicional"
        Private Const mc_dtFecha_Solicitud As String = "Fecha_Solicitud"
        Private Const mc_dtFecha_Compromiso As String = "Fecha_Compromiso"
        Private Const mc_strCantidadPendiente As String = "CantidadPendiente"
        Private Const mc_strCantidadSolicitada As String = "CantidadSolicitada"
        Private Const mc_strCantidadRecibida As String = "CantidadRecibida"
        Private Const mc_strCantidadPendienteTraslado As String = "CantidadPendienteTraslado"
        Private Const mc_strCodigoProblema As String = "Codigo_Problema"
        Private Const mc_strCodigoOperacion As String = "Codigo_Operacion"


        Private Const mc_strCurrency As String = "Currency"
        Private Const mc_strPendienteBodega As String = "PendienteBodega"



        Public Enum enTipoArticulo

            Repuesto = 1
            Servicio = 2
            Suministro = 3
            ServicioExterno = 4
        End Enum
#End Region

#Region "Variables"

        Private m_intFilaAnterior As Integer
        'Private m_dblPrecioAnterior As Double
        Private m_intCantidadPermPorOrden As Integer

#End Region

#Region "Objetos"

#Region "Datasets"

        Public m_dstRep As RepuestosxOrdenDataset
        Private m_dstRepuestosProveeduria As New RepuestosProveduriaDataset
        Private mc_strComponenteEtiqueta As String = My.Resources.ResourceUI.Repuesto

#End Region

#Region "Adapters"

        Private m_adpRep As SCGDataAccess.RepuestosxOrdenDataAdapter
        Private m_adpRepuestosProveeduria As New RepuestosProveeduriaDataAdapter

#End Region

#Region "Datarows"

        Private drwRep As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow

#End Region

#End Region

#Region "Formularios"

        Private WithEvents m_objFrmAsignacionRepuestos As frmAsignacionRepuestos

#End Region

#End Region

#Region "Procedimientos"

        Private Sub CargarGridRepuesto(ByVal codestado As Integer, _
                                       ByVal intAdicional As Integer, _
                                       ByVal udtTipoArticulo As enTipoArticulo, _
                                       ByRef dstArticulo As RepuestosxOrdenDataset, _
                                       ByRef dtgArticulo As DataGrid, _
                                       ByVal Etiqueta As String)

            Dim dtvRepuestos As New DataView

            m_adpRep = New SCGDataAccess.RepuestosxOrdenDataAdapter

            dstArticulo = Nothing

            dstArticulo = New RepuestosxOrdenDataset

            If codestado = 0 Then
                Call EstiloGridRepuestosEstadoTodos(dtgArticulo, Etiqueta)
            Else
                Call EstiloGridRepuestos(dtgArticulo, Etiqueta)
            End If

            Call m_adpRep.Fill(dstArticulo, m_strNoOrden, codestado, udtTipoArticulo, intAdicional)

            CargarEstadoLineaResources(dstArticulo)

            With dtvRepuestos
                .AllowDelete = False
                '.AllowEdit = False
                .AllowNew = False
                .Table = dstArticulo.SCGTA_TB_RepuestosxOrden
            End With

            dtgArticulo.DataSource = dtvRepuestos
            dtgArticulo.Text = ""

        End Sub

        Private Sub CambiarEstadoRepuestos(ByVal intEstado As Integer)
            Dim objDA As New DMSOneFramework.SCGDataAccess.RepuestosxOrdenDataAdapter
            Dim adpRepuestosProveeduria As New RepuestosProveeduriaDataAdapter

            Dim drdRepuestosDV As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
            Dim IntCodEstado As Integer
            Dim strUsuario As String
            Dim strMensaje As String = ""
            strUsuario = G_strUser ' objUtilitarios.obtenerNombreUsuario(G_strUser, G_strCompaniaSCG, gc_strAplicacion)

            For Each drdRepuestosDV In CType(dtgRepuestos.DataSource, DataView).Table.Rows
                If Not drdRepuestosDV.Check Then
                    drdRepuestosDV.RejectChanges()
                Else
                    If drdRepuestosDV.CodEstadoLinea = 3 Then
                        If strMensaje = "" Then
                            strMensaje = "'" & drdRepuestosDV.Itemname & "'"
                        Else
                            strMensaje = strMensaje & ", '" & drdRepuestosDV.Itemname & "'"
                        End If
                    End If
                End If
            Next
            If strMensaje <> "" Then
                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeAlosItems & " " & strMensaje & " " & _
                My.Resources.ResourceUI.MensajeNosepuedeCambiarEstadoxNoAprobados)
            End If
            objDA.Update(CType(dtgRepuestos.DataSource, DataView).Table, intEstado)

            'Para cada repuesto cuyo estado ha cambiado, inserta una nueva línea en el tracking de repuestos para registrar el cambio manual
            For Each drdRepuestosDV In CType(dtgRepuestos.DataSource, DataView).Table.Rows
                If drdRepuestosDV.Check Then
                    adpRepuestosProveeduria.InsertarLineaTracking(Busca_Codigo_Texto(cboEstadoRep.Text, False), drdRepuestosDV.EstadoRep, drdRepuestosDV.CantidadEstado, drdRepuestosDV.NoRepuesto, drdRepuestosDV.NoOrden, strUsuario)
                End If
            Next
            '''''''''''''''''''''

            IntCodEstado = CInt(Busca_Codigo_Texto(cboEstadoRep2.Text, True))
            CargarGridRepuesto(IntCodEstado, IIf(chkAdicionalRep.Checked, 1, 0), _
                               enTipoArticulo.Repuesto, m_dstRep, dtgRepuestos, mc_strComponenteEtiqueta)

        End Sub

        Private Sub EstiloGridRepuestos(ByRef dtgArticulos As DataGrid, _
                                        ByVal strEtiquetaDescripcion As String)
            Const intColumnaCondicional As Integer = 15

            'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.

            'Declaraciones generales
            Dim tsConfiguracion As New DataGridTableStyle
            Try

                dtgArticulos.TableStyles.Clear()

                Dim tcID As New DataGridTextBoxColumn
                Dim tcNoOrden As New DataGridTextBoxColumn
                Dim tcCodEstado As New DataGridTextBoxColumn
                Dim tcNoRepuesto As New DataGridConditionalColumn
                Dim tcCantidad As New DataGridConditionalColumn
                Dim tcComponente As New DataGridConditionalColumn
                Dim tcEstado As New DataGridConditionalColumn
                Dim tcEstadoLinea As New DataGridConditionalColumn
                Dim tcEstadoLineaResources As New DataGridConditionalColumn
                Dim tcCantidadEstado As New DataGridConditionalColumn
                Dim tcObservaciones As New DataGridValidatedTextColumn
                Dim tcAdicional As New DataGridConditionalColumn
                Dim tcSeleccion As New DataGridCheckColumn
                Dim tcNoAdicional As New DataGridConditionalColumn
                Dim tcFecha_Solicitud As New DataGridConditionalColumn
                Dim tcBodega As New DataGridCheckColumn
                Dim tcCurrency As New DataGridTextBoxColumn
                Dim tcPrecioAcordado As New DataGridValidatedTextColumn
                Dim tcCodEspecifico As New DataGridConditionalColumn
                Dim tcNomEspecifico As New DataGridConditionalColumn
                Dim tcCodigoProblema As New DataGridValidatedTextColumn
                Dim tcCodigoOperacion As New DataGridValidatedTextColumn

                Dim tcResultadoActividas As New DataGridValidatedTextColumn

                'Dim txtPrecio As New TextBox
                Dim tcInformacion As New DataGridConditionalColumn
                '''''''
                Dim tcResultado As New DataGridValidatedTextColumn
                Dim tcRespondidoPor As New DataGridValidatedTextColumn
                Dim tcFechaInsercion As New DataGridConditionalColumn
                '''''''
                Dim tcFecha_Compromiso As New DataGridColumnDate



                '******************para factura interna************************

                Dim tcLineNumOriginal As New DataGridTextBoxColumn
                '**************************************************************


                'm_tcFecha_Compromiso = New DataGridTextBoxColumn

                tsConfiguracion.MappingName = "SCGTA_TB_RepuestosxOrden"

                'm_dstRep.SCGTA_TB_RepuestosxOrden.Rows(0). 

                With tcID
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.ID
                    .MappingName = mc_strID
                    .Format = "###"
                    .NullText = ""
                End With

                tcResultado.Width = 300
                tcResultado.HeaderText = My.Resources.ResourceUI.Resultados
                tcResultado.MappingName = "ResultadoActividad"
                tcResultado.ReadOnly = False
                tcResultado.NullText = String.Empty

                tcRespondidoPor.Width = 100
                tcRespondidoPor.HeaderText = My.Resources.ResourceUI.RespondidaPor
                tcRespondidoPor.MappingName = "RespondidoPor"
                tcRespondidoPor.ReadOnly = False
                tcRespondidoPor.NullText = String.Empty

                '*******************************
                tcLineNumOriginal.Width = 0
                tcLineNumOriginal.HeaderText = "LineNumOriginal"
                tcLineNumOriginal.MappingName = "LineNumOriginal"
                tcLineNumOriginal.ReadOnly = True
                tcLineNumOriginal.NullText = String.Empty

                '*******************************

                tcFechaInsercion.Width = 100
                tcFechaInsercion.HeaderText = My.Resources.ResourceUI.FechaInsercion
                tcFechaInsercion.MappingName = "FechaInsercion"
                '            tcFechaInsercion.P_Formato = "{0:d}"
                tcFechaInsercion.ReadOnly = True
                tcFechaInsercion.NullText = ""
                tcFechaInsercion.P_ColumnaCondicional = intColumnaCondicional
                tcFechaInsercion.P_ColorCondicional = Color.Maroon

                With tcObservaciones
                    .Width = 300
                    .HeaderText = My.Resources.ResourceUI.Observaciones  'resource.GetString("Observaciones")  '"Observaciones"
                    .MappingName = mc_strObservaciones
                    .NullText = ""
                    .ReadOnly = False
                    '                    .P_ColumnaCondicional = intColumnaCondicional
                    '                    .P_ColorCondicional = Color.Maroon
                End With

                With tcCodEspecifico
                    .Width = 80
                    .HeaderText = My.Resources.ResourceUI.CodEspecifico ' resource.GetString("CodEspecifico") '"Cod. Especif."
                    .MappingName = "ItemCodeEspecifico"
                    .NullText = ""
                    .ReadOnly = True
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcNomEspecifico
                    .Width = 150
                    .HeaderText = My.Resources.ResourceUI.NombreEspecifico  'resource.GetString("NombreEspecifico") '"Nombre Especifico"
                    .MappingName = "ItemNameEspecifico"
                    .NullText = ""
                    .ReadOnly = True
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcNoOrden
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.NoOrden  'resource.GetString("noOrden") '"No Orden"
                    .MappingName = mc_strNoOrden
                    .Format = "###"
                    .NullText = ""
                End With

                With tcCodEstado
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.NoEstado  'resource.GetString("noEstado") '"No Estado"
                    .MappingName = mc_intCodEstado
                    .Format = "###"
                    .NullText = ""
                End With

                With tcNoRepuesto
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.NoRepuesto  'resource.GetString("NoRepuesto") '"Repuesto"
                    .MappingName = mc_intNoRepuesto
                    '.Format = "###"
                    .NullText = ""
                    .ReadOnly = True
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcCantidad
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.CantOriginal  'resource.GetString("CantOriginal")  '"Cant.Original"
                    .MappingName = mc_strCantidad
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcComponente
                    .Width = 222
                    .HeaderText = strEtiquetaDescripcion
                    .MappingName = mc_strComponente
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcEstado
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.Estado  'resource.GetString("Estado") '"Estado"
                    .MappingName = mc_strEstado
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcEstadoLinea
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.Aprobacion  'resource.GetString("Aprobacion") '"Aprobación"
                    .MappingName = mc_strEstadoLinea
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcEstadoLineaResources
                    .Width = 110
                    .HeaderText = My.Resources.ResourceUI.Aprobacion  'resource.GetString("Aprobacion") '"Aprobación"
                    .MappingName = mc_strEstadoLineaResources
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcCantidadEstado
                    .Width = 60
                    .HeaderText = My.Resources.ResourceUI.Cantidad  'resource.GetString("Cantidad") '"Cant."
                    .MappingName = mc_intCantEstado
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcAdicional
                    .Width = 0
                    .MappingName = mc_intAdicional
                    .ReadOnly = True
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcNoAdicional
                    .Width = 80
                    .HeaderText = My.Resources.ResourceUI.NoAdicional  'resource.GetString("NoAdicional") '"N° Adicional"
                    .MappingName = mc_intNoAdicional
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcFecha_Solicitud
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.FechaSolicitud 'resource.GetString("FechaSolicitud") '"Fecha Solicitud"
                    .MappingName = mc_dtFecha_Solicitud
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With m_tcFecha_Compromiso
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.FechaCompromiso  'resource.GetString("FechaCompromiso") '"Fecha Compromiso"
                    .MappingName = mc_dtFecha_Compromiso
                    '.ReadOnly = True
                    .NullText = ""
                    .Format = "dd/MM/yyyy"

                End With

                With tcFecha_Compromiso
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.FechaCompromiso  'resource.GetString("FechaCompromiso") '"Fecha Compromiso"
                    .MappingName = mc_dtFecha_Compromiso
                    '.ReadOnly = True
                    .NullText = ""
                    .Format = "dd/MM/yyyy"
                    .CalendarForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                    .CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(253, Byte), CType(253, Byte), CType(243, Byte))
                    .CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(253, Byte), CType(243, Byte))
                    .CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                    .CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))

                    AddHandler tcFecha_Compromiso.CambiaValor, _
                        AddressOf cambiaFechaCompromiso

                End With

                With tcSeleccion
                    .MappingName = mc_blnCheck
                    .Width = 30
                    .AllowNull = False
                End With

                With tcBodega
                    .HeaderText = My.Resources.ResourceUI.Almacen
                    .MappingName = mc_blnBodega
                    .Width = 50
                    .AllowNull = False

                End With

                With tcCurrency
                    .Width = 60
                    .HeaderText = My.Resources.ResourceUI.Moneda  'Moneda"
                    .MappingName = mc_strCurrency
                    .ReadOnly = True
                    .NullText = ""
                End With

                With tcPrecioAcordado
                    .Width = 70
                    .HeaderText = My.Resources.ResourceUI.Precio
                    .MappingName = mc_strPrecioAcordado
                    .ReadOnly = False
                    .NullText = ""
                    .TextBox.MaxLength = 10
                    .Format = "n2"
                    If strEtiquetaDescripcion = mc_strComponenteEtiqueta Then

                        AddHandler tcPrecioAcordado.Cambio_Valor, _
                            AddressOf CambioPrecioAcordadoRepuesto

                    Else

                        AddHandler tcPrecioAcordado.Cambio_Valor, _
                            AddressOf CambioPrecioAcordadoSer

                    End If

                End With

                With tcInformacion
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.Informacion
                    .MappingName = mc_strInformacion
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcCodigoProblema
                    .Width = 85
                    .HeaderText = My.Resources.ResourceUI.CodigoProblema
                    .MappingName = mc_strCodigoProblema
                    .NullText = ""

                    AddHandler tcCodigoProblema.Cambio_Valor, _
                             AddressOf CambioCodigoProblema

                End With

                With tcCodigoOperacion
                    .Width = 85
                    .HeaderText = My.Resources.ResourceUI.CodigoOperacion
                    .MappingName = mc_strCodigoOperacion
                    .NullText = ""

                    AddHandler tcCodigoOperacion.Cambio_Valor, _
                             AddressOf CambioCodigoOperacion
                End With

                'Agrega las columnas al tableStyle
                tsConfiguracion.GridColumnStyles.Add(tcID)
                tsConfiguracion.GridColumnStyles.Add(tcSeleccion)
                tsConfiguracion.GridColumnStyles.Add(tcCantidadEstado)
                tsConfiguracion.GridColumnStyles.Add(tcComponente)
                tsConfiguracion.GridColumnStyles.Add(tcNoRepuesto)
                tsConfiguracion.GridColumnStyles.Add(tcEstadoLinea)
                tsConfiguracion.GridColumnStyles.Add(tcEstadoLineaResources)
                tsConfiguracion.GridColumnStyles.Add(tcFecha_Solicitud)
                'tsConfiguracion.GridColumnStyles.Add(m_tcFecha_Compromiso)
                tsConfiguracion.GridColumnStyles.Add(tcFecha_Compromiso)
                tsConfiguracion.GridColumnStyles.Add(tcBodega)
                tsConfiguracion.GridColumnStyles.Add(tcResultado)
                tsConfiguracion.GridColumnStyles.Add(tcFechaInsercion)
                tsConfiguracion.GridColumnStyles.Add(tcRespondidoPor)


                '******************************
                tsConfiguracion.GridColumnStyles.Add(tcLineNumOriginal)
                '******************************

                'No visibles
                tsConfiguracion.GridColumnStyles.Add(tcNoOrden)
                tsConfiguracion.GridColumnStyles.Add(tcAdicional)
                tsConfiguracion.GridColumnStyles.Add(tcCantidad)
                tsConfiguracion.GridColumnStyles.Add(tcObservaciones)
                tsConfiguracion.GridColumnStyles.Add(tcCurrency)
                tsConfiguracion.GridColumnStyles.Add(tcPrecioAcordado)
                tsConfiguracion.GridColumnStyles.Add(tcEstado)
                tsConfiguracion.GridColumnStyles.Add(tcInformacion)
                tsConfiguracion.GridColumnStyles.Add(tcCodEspecifico)
                tsConfiguracion.GridColumnStyles.Add(tcNomEspecifico)
                tsConfiguracion.GridColumnStyles.Add(tcCodigoProblema)
                tsConfiguracion.GridColumnStyles.Add(tcCodigoOperacion)



                'Establece propiedades del datagrid (colores estándares).
                'tsConfiguracion.RowHeadersVisible = False
                tsConfiguracion.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
                tsConfiguracion.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                tsConfiguracion.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
                tsConfiguracion.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
                tsConfiguracion.RowHeadersVisible = False
                tsConfiguracion.PreferredRowHeight = 50

                'Hace que el datagrid adopte las propiedades del TableStyle.
                dtgArticulos.TableStyles.Add(tsConfiguracion)

                'Agregado 10/07/06. Alejandra. Se permite seleccionar los repuestos sin importar el estado en el que estén
                dtgArticulos.TableStyles(0).GridColumnStyles(mc_blnCheck).ReadOnly = False

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MessageBox.Show(ex.Message)

            End Try
        End Sub

        Private Sub EstiloGridRepuestosEstadoTodos(ByRef dtgArticulos As DataGrid, _
                                        ByVal strEtiquetaDescripcion As String)
            Const intColumnaCondicional As Integer = 22
            'Dim mensaje As String
            'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.

            'Declaraciones generales
            Dim tsConfiguracion As New DataGridTableStyle
            Try

                dtgArticulos.TableStyles.Clear()

                Dim tcID As New DataGridTextBoxColumn
                Dim tcNoOrden As New DataGridTextBoxColumn
                Dim tcCodEstado As New DataGridTextBoxColumn
                Dim tcNoRepuesto As New DataGridConditionalColumn
                Dim tcCantidad As New DataGridConditionalColumn
                Dim tcComponente As New DataGridConditionalColumn
                Dim tcEstado As New DataGridConditionalColumn
                Dim tcEstadoLinea As New DataGridConditionalColumn
                Dim tcEstadoLineaResources As New DataGridConditionalColumn
                Dim tcCantidadEstado As New DataGridConditionalColumn
                Dim tcObservaciones As New DataGridValidatedTextColumn
                Dim tcAdicional As New DataGridConditionalColumn
                Dim tcSeleccion As New DataGridCheckColumn
                Dim tcNoAdicional As New DataGridConditionalColumn
                Dim tcFecha_Solicitud As New DataGridConditionalColumn
                Dim tcBodega As New DataGridCheckColumn
                Dim tcCurrency As New DataGridTextBoxColumn
                Dim tcPrecioAcordado As New DataGridValidatedTextColumn
                Dim tcCantidadPendiente As New DataGridConditionalColumn
                Dim tcCantidadSolicitada As New DataGridConditionalColumn
                Dim tcCantidadRecibida As New DataGridConditionalColumn
                Dim tcCantidadPendienteTraslado As New DataGridConditionalColumn
                ''''para documentos Draft''''''''''
                Dim tcPendienteBodega As New DataGridConditionalColumn

                Dim tcCodEspecifico As New DataGridConditionalColumn
                Dim tcNomEspecifico As New DataGridConditionalColumn
                Dim tcNomColAsignado As New DataGridConditionalColumn
                Dim tcCodigoProblema As New DataGridValidatedTextColumn
                Dim tcCodigoOperacion As New DataGridValidatedTextColumn

                'Dim txtPrecio As New TextBox
                Dim tcInformacion As New DataGridConditionalColumn
                '''''''
                Dim tcFecha_Compromiso As New DataGridColumnDate

                '''''''
                Dim tcResultado As New DataGridValidatedTextColumn
                Dim tcFechaInsercion As New DataGridConditionalColumn
                Dim tcRespondidoPor As New DataGridValidatedTextColumn
                '''''''



                '***************************************************
                Dim tcLineNumOriginal As New DataGridTextBoxColumn
                '***************************************************

                'm_tcFecha_Compromiso = New DataGridTextBoxColumn

                tsConfiguracion.MappingName = "SCGTA_TB_RepuestosxOrden"

                'm_dstRep.SCGTA_TB_RepuestosxOrden.Rows(0). 

                With tcID
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.ID
                    .MappingName = mc_strID
                    .Format = "###"
                    .NullText = ""
                End With

                tcResultado.Width = 300
                tcResultado.HeaderText = My.Resources.ResourceUI.Resultados
                tcResultado.ReadOnly = False
                tcResultado.NullText = String.Empty
                tcResultado.MappingName = "ResultadoActividad"
                AddHandler tcResultado.Cambio_Valor, AddressOf CambiaResultado

                tcRespondidoPor.Width = 100
                tcRespondidoPor.HeaderText = My.Resources.ResourceUI.RespondidaPor
                tcRespondidoPor.ReadOnly = True
                tcRespondidoPor.NullText = String.Empty
                tcRespondidoPor.MappingName = "RespondidoPor"

                '***************************************************
                tcLineNumOriginal.Width = 0
                tcLineNumOriginal.HeaderText = "LineNumOriginal"
                tcLineNumOriginal.ReadOnly = True
                tcLineNumOriginal.NullText = String.Empty
                tcLineNumOriginal.MappingName = "LineNumOriginal"
                '***************************************************

                tcFechaInsercion.Width = 100
                tcFechaInsercion.HeaderText = My.Resources.ResourceUI.FechaInsercion
                tcFechaInsercion.MappingName = "FechaInsercion"
                '            tcFechaInsercion.P_Formato = "{0:d}"
                tcFechaInsercion.ReadOnly = True
                tcFechaInsercion.NullText = ""
                tcFechaInsercion.P_ColumnaCondicional = intColumnaCondicional
                tcFechaInsercion.P_ColorCondicional = Color.Maroon

                With tcObservaciones
                    .Width = 300
                    .HeaderText = My.Resources.ResourceUI.Observaciones 'resource.GetString("Observaciones")  '"Observaciones"
                    .MappingName = mc_strObservaciones
                    .NullText = ""
                    .ReadOnly = False
                    AddHandler tcObservaciones.Cambio_Valor, AddressOf CambiaObservacion

                    '                    .P_ColumnaCondicional = intColumnaCondicional
                    '                    .P_ColorCondicional = Color.Maroon
                End With

                With tcNoOrden
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.NoOrden  'resource.GetString("NoOrden") '"No Orden"
                    .MappingName = mc_strNoOrden
                    .Format = "###"
                    .NullText = ""
                End With

                With tcCodEstado
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.NoEstado  'resource.GetString("Noestado")  '"No Estado"
                    .MappingName = mc_intCodEstado
                    .Format = "###"
                    .NullText = ""
                End With

                With tcCodEspecifico
                    .Width = 80
                    .HeaderText = My.Resources.ResourceUI.CodEspecifico  'resource.GetString("CodEspecifico")  '"Cod. Especif."
                    .MappingName = "ItemCodeEspecifico"
                    .NullText = ""
                    .ReadOnly = True
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcNomEspecifico
                    .Width = 150
                    .HeaderText = My.Resources.ResourceUI.NombreEspecifico  'resource.GetString("NombreEspecifico")  '"Nombre Especif."
                    .MappingName = "ItemNameEspecifico"
                    .NullText = ""
                    .ReadOnly = True
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcNoRepuesto
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.NoRepuesto  '"Repuesto"
                    .MappingName = mc_intNoRepuesto
                    '.Format = "###"
                    .NullText = ""
                    .ReadOnly = True
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcCantidad
                    .Width = 60
                    .HeaderText = My.Resources.ResourceUI.Cantidad  'resource.GetString("cantidad")  '"Cant."
                    .MappingName = mc_strCantidad
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcComponente
                    .Width = 222
                    .HeaderText = strEtiquetaDescripcion
                    .MappingName = mc_strComponente
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcEstado
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.Estado  '"Estado"
                    .MappingName = mc_strEstado
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcEstadoLinea
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.Aprobacion  '"Aprobación"
                    .MappingName = mc_strEstadoLinea
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcEstadoLineaResources
                    .Width = 110
                    .HeaderText = My.Resources.ResourceUI.Aprobacion  '"Aprobación"
                    .MappingName = mc_strEstadoLineaResources
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcCantidadEstado
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.CantEstado  '"Cant. Estado"
                    .MappingName = mc_intCantEstado
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcAdicional
                    .Width = 0
                    .MappingName = mc_intAdicional
                    .ReadOnly = True
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcNoAdicional
                    .Width = 80
                    .HeaderText = My.Resources.ResourceUI.NoAdicional  '"N° Adicional"
                    .MappingName = mc_intNoAdicional
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcFecha_Solicitud
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.FechaSolicitud  '"Fecha Solicitud"
                    .MappingName = mc_dtFecha_Solicitud
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With m_tcFecha_Compromiso
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.FechaCompromiso  '"Fecha Compromiso"
                    .MappingName = mc_dtFecha_Compromiso
                    '.ReadOnly = True
                    .NullText = ""
                    .Format = "dd/MM/yyyy"

                End With

                With tcFecha_Compromiso
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.FechaCompromiso  '"Fecha Compromiso"
                    .MappingName = mc_dtFecha_Compromiso
                    '.ReadOnly = True
                    .NullText = ""
                    .Format = "dd/MM/yyyy"
                    .CalendarForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                    .CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(253, Byte), CType(253, Byte), CType(243, Byte))
                    .CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(253, Byte), CType(243, Byte))
                    .CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                    .CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))

                    AddHandler tcFecha_Compromiso.CambiaValor, _
                        AddressOf cambiaFechaCompromiso

                End With

                With tcSeleccion
                    .MappingName = mc_blnCheck
                    .Width = 30
                    .AllowNull = False
                End With

                With tcBodega
                    .HeaderText = My.Resources.ResourceUI.Almacen  '"Almacén"
                    .MappingName = mc_blnBodega
                    .Width = 50
                    .AllowNull = False

                    '.ReadOnly = True

                    'AddHandler tcBodega.CambioValueSingle, _
                    '    AddressOf CambioColumBodega
                End With

                With tcCurrency
                    .Width = 60
                    .HeaderText = My.Resources.ResourceUI.Moneda  'Moneda"
                    .MappingName = mc_strCurrency
                    .ReadOnly = True
                    .NullText = ""
                End With

                With tcPrecioAcordado
                    .Width = 70
                    .HeaderText = My.Resources.ResourceUI.Precio  '"Precio"
                    .MappingName = mc_strPrecioAcordado
                    .ReadOnly = False
                    .NullText = ""
                    .TextBox.MaxLength = 10
                    .Format = "n2"
                    If strEtiquetaDescripcion = mc_strComponenteEtiqueta Then

                        AddHandler tcPrecioAcordado.Cambio_Valor, _
                            AddressOf CambioPrecioAcordadoRepuesto

                    Else

                        AddHandler tcPrecioAcordado.Cambio_Valor, _
                            AddressOf CambioPrecioAcordadoSer

                    End If

                End With

                With tcInformacion
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.Informacion  '"Información"
                    .MappingName = mc_strInformacion
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcCantidadPendiente
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.CantidadPendiente  '"Cant. Pendiente"
                    .MappingName = mc_strCantidadPendiente
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcCantidadSolicitada
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.CantidadSolicitada  '"Cant. Solicitada"
                    .MappingName = mc_strCantidadSolicitada
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcCantidadRecibida
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.Cantidadrecibida  '"Cant. Recibida"
                    .MappingName = mc_strCantidadRecibida
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcCantidadPendienteTraslado
                    .Width = 120
                    .HeaderText = My.Resources.ResourceUI.Cantiadadpendtraslado  '"Cant. Pendiente Trasl"
                    .MappingName = mc_strCantidadPendienteTraslado
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcNomColAsignado
                    .Width = 150
                    .HeaderText = My.Resources.ResourceUI.ColaboradorAsignado  '"Colaborador Asignado"
                    .MappingName = "NombEmpleado"
                    .NullText = ""
                    .ReadOnly = True
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon
                End With

                With tcCodigoProblema
                    .Width = 85
                    .HeaderText = My.Resources.ResourceUI.CodigoProblema
                    .MappingName = mc_strCodigoProblema
                    .NullText = ""

                    AddHandler tcCodigoProblema.Cambio_Valor, _
                               AddressOf CambioCodigoProblema
                End With

                With tcCodigoOperacion
                    .Width = 85
                    .HeaderText = My.Resources.ResourceUI.CodigoOperacion
                    .MappingName = mc_strCodigoOperacion
                    .NullText = ""

                    AddHandler tcCodigoOperacion.Cambio_Valor, _
                             AddressOf CambioCodigoOperacion
                End With

                With tcPendienteBodega
                    .Width = 140
                    .HeaderText = My.Resources.ResourceUI.PendienteBodega '"PendienteBodega"
                    .MappingName = mc_strPendienteBodega
                    .ReadOnly = True
                    .NullText = ""
                    .P_ColumnaCondicional = intColumnaCondicional
                    .P_ColorCondicional = Color.Maroon

                End With

                'Agrega las columnas al tableStyle
                tsConfiguracion.GridColumnStyles.Add(tcID)
                tsConfiguracion.GridColumnStyles.Add(tcSeleccion)
                tsConfiguracion.GridColumnStyles.Add(tcCantidad)
                tsConfiguracion.GridColumnStyles.Add(tcComponente)
                tsConfiguracion.GridColumnStyles.Add(tcNoRepuesto)
                tsConfiguracion.GridColumnStyles.Add(tcCantidadPendiente)
                tsConfiguracion.GridColumnStyles.Add(tcCantidadSolicitada)
                tsConfiguracion.GridColumnStyles.Add(tcCantidadRecibida)
                tsConfiguracion.GridColumnStyles.Add(tcCantidadPendienteTraslado)


                ''para documentos Draft'''''
                tsConfiguracion.GridColumnStyles.Add(tcPendienteBodega)

                tsConfiguracion.GridColumnStyles.Add(tcEstadoLinea)
                tsConfiguracion.GridColumnStyles.Add(tcEstadoLineaResources)
                tsConfiguracion.GridColumnStyles.Add(tcFecha_Solicitud)
                'tsConfiguracion.GridColumnStyles.Add(m_tcFecha_Compromiso)
                tsConfiguracion.GridColumnStyles.Add(tcFecha_Compromiso)
                tsConfiguracion.GridColumnStyles.Add(tcBodega)
                tsConfiguracion.GridColumnStyles.Add(tcCodigoProblema)
                tsConfiguracion.GridColumnStyles.Add(tcCodigoOperacion)
                tsConfiguracion.GridColumnStyles.Add(tcResultado)
                tsConfiguracion.GridColumnStyles.Add(tcFechaInsercion)
                tsConfiguracion.GridColumnStyles.Add(tcRespondidoPor)
                '*****************************************************
                tsConfiguracion.GridColumnStyles.Add(tcLineNumOriginal)

                '*****************************************************


                'No visibles
                tsConfiguracion.GridColumnStyles.Add(tcNoOrden)
                tsConfiguracion.GridColumnStyles.Add(tcAdicional)
                tsConfiguracion.GridColumnStyles.Add(tcCantidadEstado)
                tsConfiguracion.GridColumnStyles.Add(tcObservaciones)
                tsConfiguracion.GridColumnStyles.Add(tcCurrency)
                tsConfiguracion.GridColumnStyles.Add(tcPrecioAcordado)
                tsConfiguracion.GridColumnStyles.Add(tcEstado)
                tsConfiguracion.GridColumnStyles.Add(tcInformacion)
                tsConfiguracion.GridColumnStyles.Add(tcCodEspecifico)
                tsConfiguracion.GridColumnStyles.Add(tcNomEspecifico)
                tsConfiguracion.GridColumnStyles.Add(tcNomColAsignado)



                'Establece propiedades del datagrid (colores estándares).
                'tsConfiguracion.RowHeadersVisible = False
                tsConfiguracion.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
                tsConfiguracion.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                tsConfiguracion.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
                tsConfiguracion.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
                tsConfiguracion.RowHeadersVisible = False
                tsConfiguracion.PreferredRowHeight = 50

                'Hace que el datagrid adopte las propiedades del TableStyle.
                dtgArticulos.TableStyles.Add(tsConfiguracion)

                'Agregado 10/07/06. Alejandra. Se permite seleccionar los repuestos sin importar el estado en el que estén
                dtgArticulos.TableStyles(0).GridColumnStyles(mc_blnCheck).ReadOnly = False

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MessageBox.Show(ex.Message)

            End Try
        End Sub

        Private Sub EliminarRepuestos(ByRef dstArticulo As RepuestosxOrdenDataset, _
                                      ByVal cboEstado As SCGComboBox.SCGComboBox, _
                                      ByVal chkAdicionales As CheckBox, _
                                      ByVal dtgArticulo As DataGrid, _
                                      ByVal udtTipoArticulo As enTipoArticulo, _
                                      ByVal strEtiqueta As String)

            Dim objDA As New DMSOneFramework.SCGDataAccess.RepuestosxOrdenDataAdapter
            Dim drwRepuestos As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
            Dim IntCodEstado As Integer
            'Dim intCodFase As Integer
            Dim strMensaje As String = ""
            Dim blnEliminarLinea As Boolean = False
            Dim intEstadoCombo As Integer
            Dim blnEliminarPaquetes As Boolean = False

            Try

                MetodosCompartidosSBOCls.IniciaTransaccion()

                MetodosCompartidosSBOCls.IniciarCotizacion(m_drdOrdenCurrent.NoCotizacion)

                For Each drwRepuestos In dstArticulo.SCGTA_TB_RepuestosxOrden.Rows

                    If Not drwRepuestos.Check Then

                        drwRepuestos.RejectChanges()
                    ElseIf drwRepuestos.LineNumFather <> -1 AndAlso drwRepuestos.Check Then
                        blnEliminarPaquetes = True
                        If MessageBox.Show(My.Resources.ResourceUI.PreguntaItemPertenecePaqueteEliminar, My.Resources.ResourceUI.EliminarItems, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            If EliminarPaquete(drwRepuestos.LineNumFather, strMensaje) Then
                                MessageBox.Show(My.Resources.ResourceUI.MensajeLosSiguientesItems & ": " & strMensaje & " " & My.Resources.ResourceUI.MensajeFueronEliminadosCorrectamente)
                                Exit For
                            Else
                                MessageBox.Show(My.Resources.ResourceUI.MensajePaqueteNoEliminadoPuesLosItems & " " & strMensaje & " " & My.Resources.ResourceUI.MensajeNoPuedenEliminarse)
                            End If
                        End If
                    ElseIf drwRepuestos.Check And Not VerificarEstadoRepPend(drwRepuestos.ID) Then

                        drwRepuestos.RejectChanges()
                        If strMensaje = "" Then
                            strMensaje = "'" & drwRepuestos.Itemname & "'"
                        Else
                            strMensaje = strMensaje & ", '" & drwRepuestos.Itemname & "'"
                        End If

                    Else

                        If (drwRepuestos.CodEstadoLinea = SCGEstadoLinea.scgFaltaAprobacion) _
                            Or Not (drwRepuestos.IsCantidadPendienteNull) _
                            Or Not (drwRepuestos.IsCantidadPendienteTrasladoNull) Then

                            If (drwRepuestos.CantidadPendiente = drwRepuestos.Cantidad) _
                            Or (drwRepuestos.CantidadPendienteTraslado = drwRepuestos.Cantidad) Then

                                blnEliminarLinea = True

                            End If

                        Else
                            If Not drwRepuestos.IsCantidadEstadoNull Then
                                If udtTipoArticulo = enTipoArticulo.Repuesto Then
                                    intEstadoCombo = CInt(Busca_Codigo_Texto(cboEstadoRep2.Text, True))
                                ElseIf udtTipoArticulo = enTipoArticulo.ServicioExterno Then
                                    intEstadoCombo = CInt(Busca_Codigo_Texto(cboEstadoRep.Text, True))
                                Else
                                    intEstadoCombo = 0
                                End If
                                If (drwRepuestos.CantidadEstado = drwRepuestos.Cantidad And _
                                    (intEstadoCombo = 1 Or intEstadoCombo = 5)) Then

                                    blnEliminarLinea = True

                                End If
                            Else
                                blnEliminarLinea = False
                            End If
                        End If

                        If blnEliminarLinea Then
                            blnEliminarLinea = False
                            MetodosCompartidosSBOCls.EliminarItemCotizacion(drwRepuestos.LineNum)
                            drwRepuestos.Delete()
                            g_AgregaAdicionales = True
                        Else
                            If strMensaje = "" Then
                                strMensaje = "'" & drwRepuestos.Itemname & "'"
                            Else
                                strMensaje = strMensaje & ", '" & drwRepuestos.Itemname & "'"
                            End If

                            drwRepuestos.RejectChanges()
                            'Exit For
                        End If
                    End If

                Next
                If Not blnEliminarPaquetes Then

                    If strMensaje <> "" Then
                        objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajelosItems & " " & strMensaje & " " & My.Resources.ResourceUI.MensajeNosepuedenEliminarXEstado)
                    End If
                    Call objDA.UpdateEliminar(dstArticulo.SCGTA_TB_RepuestosxOrden)

                    IntCodEstado = CInt(Busca_Codigo_Texto(cboEstado.Text, True))
                    CargarGridRepuesto(IntCodEstado, IIf(chkAdicionales.Checked, 1, 0), _
                                       udtTipoArticulo, dstArticulo, dtgArticulo, strEtiqueta)
                End If

                MetodosCompartidosSBOCls.ActualizarCotizacion()
                MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Commit)


            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

                MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                Throw ex

            End Try

        End Sub

        Private Sub GenerarCambiodeFechaenTrackingRepuestos(ByVal NoOrden As String, _
                                                                 ByVal NoRepuesto As Integer, _
                                                                 ByVal NewFechaCompromiso As Date, _
                                                                 ByVal dstRepuestosProveeduria As RepuestosProveduriaDataset, _
                                                                 ByVal adpRepuestosProveeduria As RepuestosProveeduriaDataAdapter, _
                                                                 ByVal p_intID As Integer)

            'Dim drwUltRegistroRepuestosProveeduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow

            Try

                '    Call dstRepuestosProveeduria.Clear()

                '    NewFechaCompromiso = NewFechaCompromiso.Date

                '    If adpRepuestosProveeduria.Fill(dstRepuestosProveeduria, _
                '                                    NoOrden, _
                '                                    NoRepuesto, _
                '                                      True) = 1 Then

                '        'TODO llama al metodo que copia la fila del repuesto

                '        If Not dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Rows(0) Is Nothing Then

                '            drwUltRegistroRepuestosProveeduria = DirectCast(dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Rows(0), _
                '                                                            RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow)

                '            If drwUltRegistroRepuestosProveeduria.IsFechaCompromisoNull Then

                '                drwUltRegistroRepuestosProveeduria.FechaCompromiso = NewFechaCompromiso

                '            Else

                '                Call AgregaNuevoregistroRepuestosProveeduria(dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria, _
                '                                                             drwUltRegistroRepuestosProveeduria, _
                '                                                             NewFechaCompromiso)

                '            End If

                '        End If

                '        If dstRepuestosProveeduria.HasChanges Then

                '            Call adpRepuestosProveeduria.Update(m_dstRepuestosProveeduria)

                '        End If

                '    End If


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex

            Finally

            End Try
        End Sub

        Private Function AgregaNuevoregistroRepuestosProveeduria(ByRef dtbRepuestosProveeduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaDataTable, _
                                                                 ByVal drwOldRepuestosProveduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow, _
                                                                 ByVal newFechaCompromiso As Date) As Boolean

            Dim drwNewRepuestosProveduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow
            Dim dtcRepuestosProveduria As DataColumn
            Dim strObservacion As String

            Try

                drwNewRepuestosProveduria = dtbRepuestosProveeduria.NewSCGTA_TB_RepuestosxOrden_ProveduriaRow

                For Each dtcRepuestosProveduria In dtbRepuestosProveeduria.Columns

                    drwNewRepuestosProveduria(dtcRepuestosProveduria.ColumnName) = drwOldRepuestosProveduria(dtcRepuestosProveduria.ColumnName)

                Next dtcRepuestosProveduria

                strObservacion = My.Resources.ResourceUI.MensajeNuevaFechaCompromiso & newFechaCompromiso & "."

                drwNewRepuestosProveduria.Observaciones = strObservacion

                drwNewRepuestosProveduria.FechaCompromiso = newFechaCompromiso

                Call dtbRepuestosProveeduria.AddSCGTA_TB_RepuestosxOrden_ProveduriaRow(drwNewRepuestosProveduria)

                Return True

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Return False
            End Try
        End Function

        Private Function ExisteAlgunRepuestoSeleccionado(ByVal dtbRepuestosxOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable) As Boolean

            Dim drwRepuestosxOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
            Dim blnRepuestoNoAprobado As Boolean = False
            If Not dtbRepuestosxOrden Is Nothing Then
                For Each drwRepuestosxOrden In dtbRepuestosxOrden.Rows

                    If drwRepuestosxOrden.Check Then
                        If drwRepuestosxOrden.CodEstadoLinea <> 3 Then
                            Return True
                        End If
                    End If
                Next
            End If

            Return False
        End Function

        Private Function VerificarCantidadLineas(ByVal dtbRepuestosxOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable) As Boolean

            Dim drwRepuestosxOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
            Dim intCantSelecc As Integer = 0

            If Not dtbRepuestosxOrden Is Nothing Then

                For Each drwRepuestosxOrden In dtbRepuestosxOrden.Rows

                    If drwRepuestosxOrden.Check Then

                        If drwRepuestosxOrden.CodEstadoLinea <> 3 Then
                            intCantSelecc += 1
                        End If

                    End If

                Next

            End If

            If intCantSelecc <= m_intCantidadPermPorOrden Then
                Return True
            Else
                Return False
            End If

        End Function

        Private Sub ActualizarDataSetRepuestos(ByRef dtbRepuestosxOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable)

            Dim drwRepuestosxOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow

            If Not dtbRepuestosxOrden Is Nothing Then
                For Each drwRepuestosxOrden In dtbRepuestosxOrden.Rows

                    drwRepuestosxOrden.Check = True

                Next
            End If
        End Sub

        Private Function VerificarEstadoRepPend(ByVal p_intID As Integer) As Boolean

            Dim adpRepuestos As New SCGDataAccess.RepuestosxOrdenDataAdapter

            Return adpRepuestos.VerificarEstadoRepPend(p_intID)

        End Function

        Private Sub establecerFechaCompromiso()

            Try
                Dim drwRepuesto As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow

                If dtgRepuestos.CurrentRowIndex <> -1 Then

                    For Each drwRepuesto In m_dstRep.SCGTA_TB_RepuestosxOrden
                        If drwRepuesto.Check Then

                            drwRepuesto.Fecha_Compromiso = dtpFechaCompromiso.Text
                            Call GenerarCambiodeFechaenTrackingRepuestos(drwRepuesto.NoOrden, drwRepuesto.NoRepuesto, drwRepuesto.Fecha_Compromiso, _
                                                                 m_dstRepuestosProveeduria, m_adpRepuestosProveeduria, drwRepuesto.ID)

                        End If

                    Next
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try


        End Sub

        Private Sub cambiaFechaCompromiso(ByVal intNoRow As Integer)

            Try
                Me.Cursor = Cursors.WaitCursor

                ValidarCambioFechaCompro(intNoRow)

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally

                Me.Cursor = Cursors.Arrow

            End Try
        End Sub

        Private Sub ValidarCambioFechaCompro(ByVal intNoRow As Integer)

            Dim s_strNoOrden As String
            Dim s_intNoRepuesto As Integer
            Dim s_intNoPieza As Integer
            Dim s_intNoSeccion As Integer
            Dim s_intID As Integer
            Dim drwRepuesto As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
            'Dim dtFechaAnterior As Date
            Dim dtFechaNueva As Date

            Try
                If Busca_Codigo_Texto(cboEstadoRep2.Text) = 2 Then

                    s_strNoOrden = dtgRepuestos.Item(intNoRow, 10)
                    s_intNoRepuesto = dtgRepuestos.Item(intNoRow, 11)
                    s_intNoPieza = dtgRepuestos.Item(intNoRow, 12)
                    s_intNoSeccion = dtgRepuestos.Item(intNoRow, 13)
                    s_intID = dtgRepuestos.Item(intNoRow, mc_intIDCol)

                    drwRepuesto = CType(CType(dtgRepuestos.DataSource, DataView).Table, DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable).FindByID(s_intID)


                    dtFechaNueva = CDate(dtgRepuestos.Item(intNoRow, 8))



                    Call GenerarCambiodeFechaenTrackingRepuestos(s_strNoOrden, s_intNoRepuesto, dtFechaNueva, _
                                                                 m_dstRepuestosProveeduria, m_adpRepuestosProveeduria, s_intID)

                Else
                    dtgRepuestos.Item(intNoRow, 8) = System.DBNull.Value

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try


        End Sub

        Private Sub CambiaResultado(ByRef sender As DataGridTextBox)

            Dim intFila As Integer
            Dim p_strResultado As String
            Dim dtbItems As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable

            Dim drwRep As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
            Dim objDA As New BLSBO.GlobalFunctionsSBO
            Dim dtg As DataGrid
            Dim cnxFecha As New SqlConnection(DAConexion.ConnectionString)
            Dim comFecha As New SqlCommand
            comFecha = cnxFecha.CreateCommand

            Try

                dtg = DirectCast(sender.Parent, DataGrid)
                If (dtg Is dtgSE) Then
                    dtbItems = m_dstServiciosExternos.SCGTA_TB_RepuestosxOrden
                Else
                    dtbItems = m_dstRep.SCGTA_TB_RepuestosxOrden

                End If
                intFila = dtg.CurrentCell.RowNumber

                drwRep = dtbItems.Rows(intFila)
                If CType(sender, DataGridTextBox).Text <> "" Then
                    p_strResultado = CType(sender, DataGridTextBox).Text
                    objDA.ActualizaResultado(m_drdOrdenCurrent.NoCotizacion, p_strResultado, drwRep.LineNum)

                    If cnxFecha.State = ConnectionState.Closed Then
                        cnxFecha.Open()
                        comFecha.CommandText = "Update SCGTA_TB_RepuestosxOrden set fechaSync =  GETDATE() Where ID = " & drwRep.ID
                        comFecha.ExecuteNonQuery()
                        cnxFecha.Close()
                    Else
                        comFecha.CommandText = "Update SCGTA_TB_RepuestosxOrden set fechaSync =  GETDATE() Where ID = " & drwRep.ID
                        comFecha.ExecuteNonQuery()
                        cnxFecha.Close()
                    End If

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub


        Private Sub CambiaObservacion(ByRef sender As DataGridTextBox)

            Dim intFila As Integer
            Dim strObservacion As String
            Dim dtbItems As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable

            Dim drwRep As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
            Dim objDA As New BLSBO.GlobalFunctionsSBO
            Dim dtg As DataGrid
            Dim cnxFecha As New SqlConnection(DAConexion.ConnectionString)
            Dim comFecha As New SqlCommand
            comFecha = cnxFecha.CreateCommand

            Try

                dtg = DirectCast(sender.Parent, DataGrid)
                If (dtg Is dtgSE) Then
                    dtbItems = m_dstServiciosExternos.SCGTA_TB_RepuestosxOrden
                Else
                    dtbItems = m_dstRep.SCGTA_TB_RepuestosxOrden

                End If
                intFila = dtg.CurrentCell.RowNumber

                drwRep = dtbItems.Rows(intFila)
                If CType(sender, DataGridTextBox).Text <> "" Then
                    strObservacion = CType(sender, DataGridTextBox).Text
                    objDA.ActualizaObservacionLinea(m_drdOrdenCurrent.NoCotizacion, strObservacion, drwRep.LineNum)
                    
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub CambioPrecioAcordadoRepuesto(ByRef sender As DataGridTextBox)

            Dim intFila As Integer
            Dim dblPrecio As Double
            Dim dtbItems As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable

            Dim drwRep As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
            Dim objDA As New BLSBO.GlobalFunctionsSBO

            Try

                dtbItems = m_dstRep.SCGTA_TB_RepuestosxOrden
                intFila = dtgRepuestos.CurrentCell.RowNumber

                drwRep = dtbItems.Rows(intFila)
                If CType(sender, DataGridTextBox).Text <> "" Then
                    dblPrecio = IIf(IsNumeric(CType(sender, DataGridTextBox).Text), Abs(CDbl(CType(sender, DataGridTextBox).Text)), -1)
                    If dblPrecio > -1 Then

                        If drwRep.IsEstadoRepNull Then

                            If drwRep.CantidadPendiente <> 0 Then

                                objDA.AgregarPrecioAcordado(m_drdOrdenCurrent.NoCotizacion, dblPrecio, drwRep.LineNum)
                                m_intFilaAnterior = intFila

                            Else

                                drwRep.RejectChanges()

                            End If

                        ElseIf drwRep.EstadoRep = "Pendiente" Then

                            objDA.AgregarPrecioAcordado(m_drdOrdenCurrent.NoCotizacion, dblPrecio, drwRep.LineNum)
                            m_intFilaAnterior = intFila

                        Else

                            drwRep.RejectChanges()


                        End If

                    Else
                        MessageBox.Show(My.Resources.ResourceUI.MensajePrecioAcordadoDebeSerNumerico)
                        'CType(sender, DataGridTextBox).Text = drwRep.PrecioAcordado
                        drwRep.RejectChanges()
                    End If

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub CambioPrecioAcordadoSer(ByRef sender As DataGridTextBox)

            Dim intFila As Integer
            Dim dblPrecio As Double
            Dim dtbItems As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable

            Dim drwRep As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
            Dim objDA As New BLSBO.GlobalFunctionsSBO

            Try

                dtbItems = m_dstServiciosExternos.SCGTA_TB_RepuestosxOrden
                intFila = dtgSE.CurrentCell.RowNumber

                drwRep = dtbItems.Rows(intFila)
                If CType(sender, DataGridTextBox).Text <> "" Then
                    dblPrecio = IIf(IsNumeric(CType(sender, DataGridTextBox).Text), Abs(CDbl(CType(sender, DataGridTextBox).Text)), -1)
                    If dblPrecio > -1 Then
                        If drwRep.IsEstadoRepNull Then
                            '  If drwRep.CantidadPendiente > 0 Then

                            objDA.AgregarPrecioAcordado(m_drdOrdenCurrent.NoCotizacion, dblPrecio, drwRep.LineNum)
                            m_intFilaAnterior = intFila

                            'Else

                            '    drwRep.RejectChanges()

                            'End If
                        Else
                            If drwRep.EstadoRep = "Pendiente" Then

                                objDA.AgregarPrecioAcordado(m_drdOrdenCurrent.NoCotizacion, dblPrecio, drwRep.LineNum)
                                m_intFilaAnterior = intFila

                            Else

                                drwRep.RejectChanges()

                            End If
                        End If

                    Else
                        MessageBox.Show(My.Resources.ResourceUI.MensajePrecioAcordadoDebeSerNumerico)
                        drwRep.RejectChanges()
                    End If
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try

        End Sub

        Private Sub CambioCodigoProblema(ByRef sender As DataGridTextBox)

            Dim intFila As Integer
            Dim strCodigoProblema As String
            Dim dtbItems As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable

            Dim drwRep As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
            Dim objDA As New BLSBO.GlobalFunctionsSBO

            Try

                dtbItems = m_dstRep.SCGTA_TB_RepuestosxOrden
                intFila = dtgRepuestos.CurrentCell.RowNumber
                drwRep = dtbItems.Rows(intFila)
                strCodigoProblema = CType(sender, DataGridTextBox).Text
                objDA.AgregarCodProblema(m_drdOrdenCurrent.NoCotizacion, strCodigoProblema, drwRep.LineNum)
                m_dstRep.SCGTA_TB_RepuestosxOrden.Rows(intFila).AcceptChanges()
                m_intFilaAnterior = intFila

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try


        End Sub

        Private Sub CambioCodigoOperacion(ByRef sender As DataGridTextBox)

            Dim intFila As Integer
            Dim strCodigoProblema As String
            Dim dtbItems As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable

            Dim drwRep As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
            Dim objDA As New BLSBO.GlobalFunctionsSBO

            Try

                dtbItems = m_dstRep.SCGTA_TB_RepuestosxOrden
                intFila = dtgRepuestos.CurrentCell.RowNumber
                drwRep = dtbItems.Rows(intFila)
                strCodigoProblema = CType(sender, DataGridTextBox).Text
                objDA.AgregarCodOperacion(m_drdOrdenCurrent.NoCotizacion, strCodigoProblema, drwRep.LineNum)
                m_dstRep.SCGTA_TB_RepuestosxOrden.Rows(intFila).AcceptChanges()
                m_intFilaAnterior = intFila


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try


        End Sub

        Private Sub LlamaFormadeOrdendeCompra(ByRef dstArticulo As RepuestosxOrdenDataset, _
                                              ByVal udtTipoArticulo As enTipoArticulo, _
                                              ByVal dtgArticulo As DataGrid, _
                                              ByVal strEtiqueta As String, _
                                              ByVal cbEstados As SCGComboBox.SCGComboBox, _
                                              ByVal chkAdicionales As CheckBox)

            Dim form As frmOrdenCompra = Nothing
            Dim IntCodEstado As Integer
            'Const strCriterio As String = "check=True"

            Try

                m_intCantidadPermPorOrden = 35

                If ExisteAlgunRepuestoSeleccionado(dstArticulo.SCGTA_TB_RepuestosxOrden) Then

                    If VerificarCantidadLineas(dstArticulo.SCGTA_TB_RepuestosxOrden) Then

                        TieneRepuestosPendientes(dstArticulo.SCGTA_TB_RepuestosxOrden, CInt(Busca_Codigo_Texto(cbEstados.Text, True)))

                        Me.MdiParent.Cursor = Cursors.WaitCursor


                        If IsNothing(form) Then

                            form = New frmOrdenCompra(dstArticulo.SCGTA_TB_RepuestosxOrden, _
                                                      txtMarca.Text, _
                                                      m_drdOrdenCurrent.DescModelo, _
                                                      m_intAnio, _
                                                      m_strNoChasis, _
                                                      udtTipoArticulo, _
                                                      m_drdOrdenCurrent.NoOrden, _
                                                      CInt(Busca_Codigo_Texto(cbEstados.Text, True)), _
                                                      m_drdOrdenCurrent.NoCotizacion, m_drdOrdenCurrent.CodMarca, _
                                                      m_drdOrdenCurrent.DescEstilo, m_drdOrdenCurrent.Placa, m_drdVisitaCurrent.Asesor, m_intTipo)

                        End If

                        form.ShowInTaskbar = False

                        Call form.ShowDialog(Me.MdiParent)

                        If form.Ok Then


                            IntCodEstado = CInt(Busca_Codigo_Texto(cbEstados.Text, True))

                            Call CargarGridRepuesto(IntCodEstado, IIf(chkAdicionales.Checked, 1, 0), _
                                                    udtTipoArticulo, dstArticulo, _
                                                    dtgArticulo, strEtiqueta)

                        End If

                        Me.MdiParent.Cursor = Cursors.Arrow

                    Else

                        objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeCantidadItemsSeleccionadosNoMayorDe & CStr(m_intCantidadPermPorOrden))

                    End If

                Else

                    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarAlMenosUnitemParaOC)

                End If

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub TieneRepuestosPendientes(ByVal p_dtbRepuestosxOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable, ByVal p_intEstadoSelec As Integer)

            Dim drwRepuestosxOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
            Dim blnRepuestoNoAprobado As Boolean = False

            If p_intEstadoSelec = 0 Then
                If Not p_dtbRepuestosxOrden Is Nothing Then
                    For Each drwRepuestosxOrden In p_dtbRepuestosxOrden.Rows

                        If drwRepuestosxOrden.Check Then
                            If Not drwRepuestosxOrden.IsCantidadPendienteNull Then
                                If drwRepuestosxOrden.CantidadPendiente = 0 Then
                                    drwRepuestosxOrden.Check = False
                                End If
                            Else
                                If Not (drwRepuestosxOrden.CantidadEstado > 0 And drwRepuestosxOrden.CodEstadoRep = 1) Then
                                    drwRepuestosxOrden.Check = False
                                End If
                            End If
                        End If

                    Next
                End If

            End If

        End Sub

#End Region


#Region "Eventos"

        Private Sub btnCambiarEstadoRepuesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCambiarEstadoRepuesto.Click
            Try
                'Dim intNoRepuesto As Integer
                'Dim strNoOrden As String
                'Dim strEstadoActual As String
                'Dim strEstadoNuevo As String
                'Dim intCantidad As Integer
                'Dim strUsuario As String

                Dim adpRepuestosProveeduria As New RepuestosProveeduriaDataAdapter


                Me.MdiParent.Cursor = Cursors.WaitCursor

                If cboEstadoRep.Text <> "" And cboEstadoRep.Text <> cboEstadoRep2.Text Then
                    CambiarEstadoRepuestos(CInt(Busca_Codigo_Texto(cboEstadoRep.Text)))
                Else
                    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeNoHaElegidoEstado)
                End If

                If cboEstadoRep.Text <> "" Then

                    If cboEstadoRep.Text <> cboEstadoRep2.Text Then

                        CambiarEstadoRepuestos(CInt(Busca_Codigo_Texto(cboEstadoRep.Text)))

                    Else
                        objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeNuevoEstadoDebeSerDiferente)
                    End If

                Else
                    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeNoHaElegidoEstado)
                End If

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnOrdenCompra_Click(ByVal sender As System.Object, _
                                         ByVal e As System.EventArgs) Handles btnOrdenCompra.Click, btnOrdenCompraSE.Click

            Try

                If sender.name = btnOrdenCompra.Name Then

                    Call LlamaFormadeOrdendeCompra(m_dstRep, _
                                                  enTipoArticulo.Repuesto, _
                                                  dtgRepuestos, _
                                                  mc_strComponenteEtiqueta, _
                                                  cboEstadoRep2, _
                                                  chkAdicionalRep)

                ElseIf sender.name = btnOrdenCompraSE.Name Then

                    Call LlamaFormadeOrdendeCompra(m_dstServiciosExternos, _
                                                 enTipoArticulo.ServicioExterno, _
                                                 dtgSE, _
                                                 mc_strServicioExterno, _
                                                 cboEstadoRep2, _
                                                 chkAdicionalesSE)
                End If


            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnRepuesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRepuesto.Click
            Dim strParametros As String = ""
            Dim strNombreReport As String
            Dim objBLConexion As DMSOneFramework.SCGDataAccess.DAConexion

            Try

                If m_bolAdicional = True Then
                    strNombreReport = My.Resources.ResourceUI.rptNombreRepuestosAdic
                Else
                    strNombreReport = My.Resources.ResourceUI.rptNombreRepuestos
                End If
                Me.MdiParent.Cursor = Cursors.WaitCursor

                objBLConexion = New DMSOneFramework.SCGDataAccess.DAConexion

                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)
                If txtNoOrden.Text <> "" Then


                    strParametros = strParametros & txtNoOrden.Text.Trim


                    With rptorden
                        .P_BarraTitulo = My.Resources.ResourceUI.rptTituloDocumentoRepuestos
                        .P_WorkFolder = PATH_REPORTES
                        .P_Filename = strNombreReport
                        .P_Server = Server
                        .P_DataBase = strDATABASESCG
                        .P_User = UserSCGInternal
                        .P_Password = Password
                        .P_ParArray = strParametros
                    End With

                    rptorden.VerReporte()
                Else
                    MsgBox(My.Resources.ResourceUI.MensajeDebeSeleccionarOT)
                End If

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnAdicional_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdicional.Click
            Dim strParametros As String = ""

            Dim objBLConexion As DMSOneFramework.SCGDataAccess.DAConexion

            Try
                Me.MdiParent.Cursor = Cursors.WaitCursor

                objBLConexion = New DMSOneFramework.SCGDataAccess.DAConexion

                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)
                If txtNoOrden.Text <> "" Then


                    strParametros = strParametros & txtNoOrden.Text.Trim


                    With rptorden
                        .P_BarraTitulo = My.Resources.ResourceUI.rptTituloDocumentoRepAdicionales
                        .P_WorkFolder = PATH_REPORTES
                        .P_Filename = My.Resources.ResourceUI.rptNombreRepuestosAdicionales
                        .P_Server = Server
                        .P_DataBase = strDATABASESCG
                        .P_User = UserSCGInternal
                        .P_Password = Password
                        .P_ParArray = strParametros
                    End With

                    rptorden.VerReporte()
                Else
                    MsgBox(My.Resources.ResourceUI.MensajeDebeSeleccionarOT)
                End If

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.WaitCursor

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub cboEstadoRep2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEstadoRep2.SelectedIndexChanged
            Dim codestado As Integer

            Try
                If Not Me.MdiParent Is Nothing Then
                    Me.MdiParent.Cursor = Cursors.WaitCursor
                End If


                codestado = CInt(Busca_Codigo_Texto(cboEstadoRep2.Text, True))

                CargarGridRepuesto(codestado, IIf(chkAdicionalRep.Checked, 1, 0), _
                                        enTipoArticulo.Repuesto, m_dstRep, _
                                        dtgRepuestos, mc_strComponenteEtiqueta)


                If m_drdOrdenCurrent.Estado <> mc_NumEstado_Finalizada And m_drdOrdenCurrent.Estado <> mc_NumEstado_Cancelada Then

                    'Agregado. 26/05/06. Alejandra. Se permite generar una orden de compra sólo si el estado
                    'del repuesto es Pendiente o Pendiente por Devolución
                    If codestado = 0 Or codestado = 1 Or codestado = 4 Then
                        btnOrdenCompra.Enabled = True
                    Else
                        btnOrdenCompra.Enabled = False
                    End If

                Else

                    btnEliminarRep.Enabled = False

                End If

                If Not Me.MdiParent Is Nothing Then
                    Me.MdiParent.Cursor = Cursors.Arrow
                End If

            Catch ex As Exception

                If Not Me.MdiParent Is Nothing Then
                    Me.MdiParent.Cursor = Cursors.Arrow
                End If

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub chkAdicionalRep_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAdicionalRep.CheckedChanged
            Dim codestado As Integer

            Try

                codestado = CInt(Busca_Codigo_Texto(cboEstadoRep2.Text, True))
                CargarGridRepuesto(codestado, IIf(chkAdicionalRep.Checked, 1, 0), _
                                    enTipoArticulo.Repuesto, m_dstRep, dtgRepuestos, mc_strComponenteEtiqueta)

                If chkAdicionalRep.Checked = True Then
                    m_bolAdicional = True
                Else
                    m_bolAdicional = False
                End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub dtgRepuestos_DoubleClick(ByVal sender As Object, _
                                             ByVal e As System.EventArgs) Handles dtgRepuestos.DoubleClick, dtgSE.DoubleClick
            Try

                If sender.name = dtgRepuestos.Name Then

                    LlamaTrackingArticulos(dtgRepuestos, enTipoArticulo.Repuesto)

                ElseIf sender.name = dtgSE.Name Then

                    LlamaTrackingArticulos(dtgSE, enTipoArticulo.ServicioExterno)
                End If


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally
            End Try
        End Sub

        Private Sub LlamaTrackingArticulos(ByVal dtgArticulos As DataGrid, _
                                            ByVal TipoArticulo As Integer)
            Try
                Dim idRepuesto As Integer
                Dim strNombreArticulo As String
                'Dim Seccion As String

                'validacion de que existan registros en el datagrid
                If dtgArticulos.CurrentRowIndex <> -1 Then

                    idRepuesto = dtgArticulos.Item(dtgArticulos.CurrentCell.RowNumber, 0)
                    'Agregado 06/07/06. Alejandra. Para evitar problemas si el nombreRepuesto está en Null
                    If (dtgArticulos.Item(dtgArticulos.CurrentCell.RowNumber, 3)) Is System.DBNull.Value Then
                        strNombreArticulo = ""
                    Else
                        strNombreArticulo = CStr(dtgArticulos.Item(dtgArticulos.CurrentCell.RowNumber, 3))
                    End If

                    Dim forma As New frmTrackingRepuestos(m_strNoOrden, _
                                                         idRepuesto, _
                                                        strNombreArticulo, _
                                                        txtMarca.Text, _
                                                        txtEstilo.Text, _
                                                        m_intAnio, _
                                                        m_strNoChasis, _
                                                        TipoArticulo)

                    forma.ShowInTaskbar = False

                    Call forma.ShowDialog(Me.MdiParent)

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally
            End Try

        End Sub

        Private Sub btnEliminarRep_Click(ByVal sender As Object, _
                                         ByVal e As System.EventArgs) Handles btnEliminarRep.Click
            Try

                Me.MdiParent.Cursor = Cursors.WaitCursor



                Call EliminarRepuestos(m_dstRep, _
                                       cboEstadoRep2, _
                                       chkAdicionalRep, _
                                       dtgRepuestos, _
                                       enTipoArticulo.Repuesto, _
                                       mc_strComponenteEtiqueta)

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub btnCheckAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheckAll.Click
            Dim intCodEstado As Integer

            Try
                Me.Cursor = Cursors.WaitCursor

                intCodEstado = CInt(Busca_Codigo_Texto(cboEstadoRep2.Text, True))

                'Modificado 10/07/06. Alejandra. Se permite seleccionar los repuestos, sin importar su estado
                ActualizarDataSetRepuestos(m_dstRep.SCGTA_TB_RepuestosxOrden)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally
                Me.Cursor = Cursors.Arrow
            End Try
        End Sub

        Private Sub btnFechaComp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFechaComp.Click

            Try
                Dim intCodEstado As Integer
                intCodEstado = CInt(Busca_Codigo_Texto(cboEstadoRep2.Text, True))

                If intCodEstado = 2 Then 'Esta funcionalidad es solo para los repuestos con estado "Solicitado"
                    establecerFechaCompromiso()
                End If


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub btnAgregarRep_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregarRep.Click
            Dim intCodEstado As Integer

            Try
                Dim frmAdicionales As New frmAdicionales1(enTipoArticulo.Repuesto, m_strNoOrden, m_drdOrdenCurrent.NoCotizacion, m_blnAgregaAdicional, m_drdVisitaCurrent.NoVisita)
                Call frmAdicionales.ShowDialog()

                intCodEstado = CInt(Busca_Codigo_Texto(cboEstadoRep2.Text, True))
                CargarGridRepuesto(intCodEstado, IIf(chkAdicionalRep.Checked, 1, 0), _
                                    enTipoArticulo.Repuesto, m_dstRep, dtgRepuestos, mc_strComponenteEtiqueta)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub dtgRepuestos_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgRepuestos.GotFocus
            Try

                G_CancelarEditColumnDataGrid(Me, dtgRepuestos)

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub btnSolicitar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSolicitar.Click

            Dim intCodEstado As Integer

            Try
                Dim frmAdicionales As New frmAdicionales1(enTipoArticulo.Repuesto, m_strNoOrden, m_drdOrdenCurrent.NoCotizacion, m_blnAgregaAdicional, m_drdVisitaCurrent.NoVisita)
                Call frmAdicionales.GenerarSolicitudDesdeAfuera(m_dstRep)

                intCodEstado = CInt(Busca_Codigo_Texto(cboEstadoRep2.Text, True))
                CargarGridRepuesto(intCodEstado, IIf(chkAdicionalRep.Checked, 1, 0), _
                                    enTipoArticulo.Repuesto, m_dstRep, dtgRepuestos, mc_strComponenteEtiqueta)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try

        End Sub

        Private Sub btnAsignarARepuesto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAsignarARepuesto.Click
            Try

                Dim Forma_Nueva As Form
                Dim blnExisteForm As Boolean

                For Each Forma_Nueva In Me.MdiParent.MdiChildren
                    If Forma_Nueva.Name = "frmAsignacionRepuestos" Then
                        blnExisteForm = True
                    End If
                Next

                If Not blnExisteForm Then

                    If m_objFrmAsignacionRepuestos IsNot Nothing Then
                        m_objFrmAsignacionRepuestos.Dispose()
                        m_objFrmAsignacionRepuestos = Nothing
                    End If

                    m_objFrmAsignacionRepuestos = m_objFrmAsignacionRepuestos

                    m_objFrmAsignacionRepuestos = New frmAsignacionRepuestos(m_strNoOrdenAct, m_drdOrdenCurrent.NoCotizacion, m_drdOrdenCurrent.Estado)
                    m_objFrmAsignacionRepuestos.MdiParent = Me.MdiParent
                    m_objFrmAsignacionRepuestos.Show()
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub m_objFrmAsignacionRepuestos_e_AsignacionRealizada() Handles m_objFrmAsignacionRepuestos.e_AsignacionRealizada

            Try
                Dim intCodEstadoRep As Integer

                If Not IsNothing(Me.MdiParent) Then

                    m_objFrmAsignacionRepuestos.Close()

                    intCodEstadoRep = CInt(Busca_Codigo_Texto(cboEstadoRep2.Text, True))

                    CargarGridRepuesto(intCodEstadoRep, IIf(chkAdicionalRep.Checked, 1, 0), _
                                            enTipoArticulo.Repuesto, m_dstRep, _
                                            dtgRepuestos, mc_strComponenteEtiqueta)

                End If

            Catch ex As Exception

                If Not IsNothing(Me.MdiParent) Then
                    Me.MdiParent.Cursor = Cursors.Arrow
                End If

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

#End Region

    End Class

End Namespace
