Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports System.Math
Imports SCG.UX.Windows.SAP
Imports System.Data.SqlClient
'Imports System.Windows.Forms.VisualStyles

Namespace SCG_User_Interface

    Partial Class frmOrden
        Inherits frmPlantillaSAP

#Region "Declaraciones"

#Region "Enums"

        Public Enum SCGEstadoLinea
            scgAprobado = 1
            scgNoAprobado = 2
            scgFaltaAprobacion = 3
        End Enum

#End Region

#Region "Constantes"

        'Actividades
        Private Const mcact_strNoOrdenAct As String = "NoOrden"
        Private Const mcact_intNoFaseAct As String = "NoFase"
        Private Const mcact_intNoActividadAct As String = "NoActividad"
        Private Const mcact_strEstadoAct As String = "Estado"
        Private Const mcact_intAdicional As String = "Adicional"
        Private Const mcact_strDescripcionAct As String = "ItemName"
        Private Const mc_strObservaciones As String = "Observaciones"
        Private Const mcact_strColaborasAsignados As String = "ColaborasAsignados"
        Private Const mcact_blnCheck As String = "Check"
        Private Const mcact_strTableNameAct As String = "SCGTA_TB_ActividadesXOrden"
        Private Const mcact_dtFecha_Solicitud As String = "Fecha_Solicitud"
        Private Const mcact_intNoAdicional As String = "NoAdicional"
        Private Const mc_strNoIniciada As String = "No iniciada"
        Private Const mc_strDescripEstadoResources As String = "DescripcionActividadResources"
        Private Const mc_strTiempoEstandar As String = "Duracion"
        Private Const mcact_strNombreFase As String = "Fase"
        Private Const mcact_strCurrency As String = "Currency"

       'Dim resource As New Resources.ResourceManager("SCG_User_Interface.ResourceUI", _
        '                                            Me.GetType.Assembly)



#End Region

#Region "Objetos"

#Region "Datasets"

        Public m_dstAct As ActividadesXFaseDataset

#End Region

#Region "Adapters"

        Private m_adpAct As SCGDataAccess.ActividadesXFaseDataAdapter

#End Region

#Region "DataRows"

        Private drwAct As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow

#End Region

#End Region

#End Region

#Region "Procedimientos"

        Private Overloads Sub CargarEstadoLineaResources(ByVal dstActividades As ActividadesXFaseDataset)

            Dim intIndice As Integer
            For intIndice = 0 To dstActividades.SCGTA_TB_ActividadesxOrden.Rows.Count - 1

                Select Case (dstActividades.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("EstadoLinea")).ToString.ToLower
                    Case "aprobada", "aprobado", "si"
                        dstActividades.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("EstadolineaResources") = My.Resources.ResourceUI.Si
                    Case "no aprobada", "no aprobado", "no"
                        dstActividades.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("EstadolineaResources") = My.Resources.ResourceUI.No
                    Case "falta aprobacion", "falta aprobación)"
                        dstActividades.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("EstadolineaResources") = My.Resources.ResourceUI.FaltaAprobacion
                    Case Else
                        dstActividades.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("EstadolineaResources") = dstActividades.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("EstadoLinea")
                End Select
            Next

            dstActividades.AcceptChanges()
        End Sub

        Private Overloads Sub CargarEstadoLineaResources(ByVal dstdataset As RepuestosxOrdenDataset)

            Dim intIndice As Integer
            For intIndice = 0 To dstdataset.SCGTA_TB_RepuestosxOrden.Rows.Count - 1

                Select Case (dstdataset.SCGTA_TB_RepuestosxOrden.Rows(intIndice)("EstadoLinea")).ToString.ToLower
                    Case "aprobada", "aprobado", "si"
                        dstdataset.SCGTA_TB_RepuestosxOrden.Rows(intIndice)("EstadolineaResources") = My.Resources.ResourceUI.Si
                    Case "no aprobada", "no aprobado", "no"
                        dstdataset.SCGTA_TB_RepuestosxOrden.Rows(intIndice)("EstadolineaResources") = My.Resources.ResourceUI.No
                    Case "falta aprobacion", "falta aprobación)"
                        dstdataset.SCGTA_TB_RepuestosxOrden.Rows(intIndice)("EstadolineaResources") = My.Resources.ResourceUI.FaltaAprobacion
                    Case Else
                        dstdataset.SCGTA_TB_RepuestosxOrden.Rows(intIndice)("EstadolineaResources") = dstdataset.SCGTA_TB_RepuestosxOrden.Rows(intIndice)("EstadoLinea")
                End Select
            Next
            dstdataset.AcceptChanges()
        End Sub

        Private Overloads Sub CargarEstadoLineaResources(ByVal dstdataset As SuministrosDataset)

            Dim intIndice As Integer
            For intIndice = 0 To dstdataset.SCGTA_VW_Suministros.Rows.Count - 1

                Select Case (dstdataset.SCGTA_VW_Suministros.Rows(intIndice)("EstadoLinea")).ToString.ToLower
                    Case "aprobada", "aprobado", "si"
                        dstdataset.SCGTA_VW_Suministros.Rows(intIndice)("EstadolineaResources") = My.Resources.ResourceUI.Si
                    Case "no aprobada", "no aprobado", "no"
                        dstdataset.SCGTA_VW_Suministros.Rows(intIndice)("EstadolineaResources") = My.Resources.ResourceUI.No
                    Case "falta aprobacion", "falta aprobación)"
                        dstdataset.SCGTA_VW_Suministros.Rows(intIndice)("EstadolineaResources") = My.Resources.ResourceUI.FaltaAprobacion
                    Case Else
                        dstdataset.SCGTA_VW_Suministros.Rows(intIndice)("EstadolineaResources") = dstdataset.SCGTA_VW_Suministros.Rows(intIndice)("EstadoLinea")
                End Select
            Next
            dstdataset.AcceptChanges()
        End Sub



        Private Sub EstiloGridActividades()
            Const intColumnaCondicional As Integer = 15

            'Declaraciones generales
            Dim tsConfiguracion As New DataGridTableStyle

            dtgActividades.TableStyles.Clear()



            Dim tcNoOrden As New DataGridTextBoxColumn
            Dim tcNoActividad As New DataGridTextBoxColumn
            Dim tcNoFase As New DataGridTextBoxColumn
            Dim tcDescripcion As New DataGridConditionalColumn
            Dim tcEstadoLinea As New DataGridConditionalColumn
            Dim tcEstado As New DataGridConditionalColumn
            Dim tcAdicional As New DataGridConditionalColumn
            Dim tcObservaciones As New DataGridValidatedTextColumn
            Dim tcCheck As New DataGridCheckColumn
            Dim tcCantidad As New DataGridValidatedTextColumn
            Dim tcColAsignados As New DataGridConditionalColumn
            Dim tcCurrency As New DataGridTextBoxColumn
            Dim tcPrecioAcordado As New DataGridValidatedTextColumn
            Dim tcDescipcionEstadoResources As New DataGridConditionalColumn
            Dim tcTiempoEstandar As New DataGridValidatedTextColumn
            Dim tcEstadoLineaResources As New DataGridConditionalColumn
            Dim tcNombreFase As New DataGridConditionalColumn


            Dim tcResultado As New DataGridValidatedTextColumn
            Dim tcFechaInsercion As New DataGridConditionalColumn
            'Dos columnas posteriormente agregadas --26-04-06 dorian
            Dim tcNoAdicional As New DataGridConditionalColumn
            Dim tcFecha_Solicitud As New DataGridConditionalColumn

            tsConfiguracion.MappingName = m_dstAct.SCGTA_TB_ActividadesxOrden.TableName()

            With tcNoOrden
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.NoOrden   '"No Orden"
                .MappingName = m_dstAct.SCGTA_TB_ActividadesxOrden.Columns(mcact_strNoOrdenAct).ColumnName
                .Format = "###"
                .ReadOnly = True
            End With

            tcResultado.Width = 300
            tcResultado.HeaderText = My.Resources.ResourceUI.Resultados
            tcResultado.MappingName = "ResultadoActividad"
            tcResultado.ReadOnly = False
            tcResultado.NullText = ""
            AddHandler tcResultado.Cambio_Valor, AddressOf CambiaResultadoActividades

            tcFechaInsercion.Width = 100
            tcFechaInsercion.HeaderText = My.Resources.ResourceUI.FechaInsercion
            tcFechaInsercion.MappingName = "FechaInsercion"
            '            tcFechaInsercion.P_Formato = "{0:d}"
            tcFechaInsercion.ReadOnly = True
            tcFechaInsercion.NullText = ""
            tcFechaInsercion.P_ColumnaCondicional = intColumnaCondicional
            tcFechaInsercion.P_ColorCondicional = Color.Maroon

            With tcNoActividad
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.NoActividad '"No Actividad"
                .MappingName = m_dstAct.SCGTA_TB_ActividadesxOrden.Columns(mcact_intNoActividadAct).ColumnName
                .Format = "###"
                .ReadOnly = True
            End With

            With tcNoFase
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.NoFase  '"No Fase"
                .MappingName = m_dstAct.SCGTA_TB_ActividadesxOrden.Columns(mcact_intNoFaseAct).ColumnName
                .Format = "###"
                .ReadOnly = True
            End With


            With tcDescripcion
                .Width = 222
                .HeaderText = My.Resources.ResourceUI.Actividad  '"Actividad"
                .MappingName = m_dstAct.SCGTA_TB_ActividadesxOrden.Columns(mcact_strDescripcionAct).ColumnName
                .NullText = ""
                .ReadOnly = False
                .P_ColumnaCondicional = intColumnaCondicional
                .P_ColorCondicional = Color.Maroon
            End With

            With tcObservaciones
                .Width = 300
                .HeaderText = My.Resources.ResourceUI.Observaciones  '"Observaciones"
                .MappingName = m_dstAct.SCGTA_TB_ActividadesxOrden.Columns(mc_strObservaciones).ColumnName
                .NullText = ""

                '                .ReadOnly = True
                AddHandler tcObservaciones.Cambio_Valor, AddressOf CambiaObservacionActividades
                '                .P_ColumnaCondicional = intColumnaCondicional
                '                .P_ColorCondicional = Color.Maroon
            End With

            With tcEstadoLinea
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.Aprobacion  '"Aprobación"
                .MappingName = m_dstAct.SCGTA_TB_ActividadesxOrden.Columns(mc_strEstadoLinea).ColumnName
                .NullText = ""
                .ReadOnly = True
                .P_ColumnaCondicional = intColumnaCondicional
                .P_ColorCondicional = Color.Maroon
            End With

            With tcEstadoLineaResources
                .Width = 110
                .HeaderText = My.Resources.ResourceUI.Aprobacion  '"Aprobación"
                .MappingName = m_dstAct.SCGTA_TB_ActividadesxOrden.Columns(mc_strEstadoLineaResources).ColumnName
                .NullText = ""
                .ReadOnly = True
                .P_ColumnaCondicional = intColumnaCondicional
                .P_ColorCondicional = Color.Maroon
            End With

            With tcColAsignados
                .Width = 200
                .HeaderText = My.Resources.ResourceUI.Colaboradores '"Colaboradores"
                .MappingName = m_dstAct.SCGTA_TB_ActividadesxOrden.Columns(mcact_strColaborasAsignados).ColumnName
                .NullText = ""
                .ReadOnly = True
                .P_ColumnaCondicional = intColumnaCondicional
                .P_ColorCondicional = Color.Maroon
                .P_TipoColabora = True
            End With

            With tcEstado
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.Estado  '"Estado"
                .MappingName = m_dstAct.SCGTA_TB_ActividadesxOrden.Columns(mcact_strEstadoAct).ColumnName
                .NullText = ""
                .ReadOnly = True
                .P_ColumnaCondicional = intColumnaCondicional
                .P_ColorCondicional = Color.Maroon
            End With

            With tcDescipcionEstadoResources
                .Width = 85
                .HeaderText = My.Resources.ResourceUI.Estado
                .MappingName = m_dstAct.SCGTA_TB_ActividadesxOrden.Columns(mc_strDescripEstadoResources).ColumnName
                .NullText = ""
                .ReadOnly = True
                .P_ColumnaCondicional = intColumnaCondicional
                .P_ColorCondicional = Color.Maroon
            End With

            With tcAdicional
                .Width = 0
                .MappingName = m_dstAct.SCGTA_TB_ActividadesxOrden.Columns(mcact_intAdicional).ColumnName
                .ReadOnly = True
                .P_ColumnaCondicional = intColumnaCondicional
                .P_ColorCondicional = Color.Maroon
            End With

            With tcCheck
                .MappingName = m_dstAct.SCGTA_TB_ActividadesxOrden.Columns(mcact_blnCheck).ColumnName
                .Width = 30
                .AllowNull = False
            End With

            With tcCantidad
                .Width = 60
                .HeaderText = My.Resources.ResourceUI.Cantidad  '"Cant."
                .MappingName = mc_strCantidad
                .ReadOnly = False
                .NullText = "0"
                .TextBox.MaxLength = 10

                AddHandler tcCantidad.Cambio_Valor, _
                    AddressOf CambioCantidadAct

            End With

            With tcCurrency
                .Width = 60
                .HeaderText = My.Resources.ResourceUI.Moneda  'Moneda"
                .MappingName = m_dstAct.SCGTA_TB_ActividadesxOrden.Columns(mcact_strCurrency).ColumnName
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
                'AddHandler tcPrecioAcordado.TextBox.LostFocus, _
                '    AddressOf CambioPrecioAcordadoAct
                AddHandler tcPrecioAcordado.Cambio_Valor, _
                    AddressOf CambioPrecioAcordadoAct

            End With

            With tcTiempoEstandar
                .Width = 75
                .HeaderText = My.Resources.ResourceUI.TiempoEstandar
                If g_intUnidadTiempo = -1 Then
                    .MappingName = mc_strTiempoEstandar
                Else
                    .MappingName = "TotalUnidadTiempo"
                End If

                .ReadOnly = False
                .NullText = ""
                .TextBox.MaxLength = 5

                AddHandler tcTiempoEstandar.Cambio_Valor, _
                    AddressOf CambioTiempoEstandarAct

            End With

            With tcNombreFase
                .Width = 200
                .HeaderText = My.Resources.ResourceUI.NombreFase  '"Nombre de Fase"
                .MappingName = m_dstAct.SCGTA_TB_ActividadesxOrden.Columns(mcact_strNombreFase).ColumnName
                .NullText = ""
                .ReadOnly = True
                .P_ColumnaCondicional = intColumnaCondicional
                .P_ColorCondicional = Color.Maroon
            End With

            'Visibles
            If g_blnCampoVisible Then
                tsConfiguracion.GridColumnStyles.Add(tcCheck)
                tsConfiguracion.GridColumnStyles.Add(tcDescripcion)
                tsConfiguracion.GridColumnStyles.Add(tcResultado)
                tsConfiguracion.GridColumnStyles.Add(tcEstadoLinea)
                tsConfiguracion.GridColumnStyles.Add(tcEstadoLineaResources)
                tsConfiguracion.GridColumnStyles.Add(tcTiempoEstandar)
                tsConfiguracion.GridColumnStyles.Add(tcColAsignados)
                tsConfiguracion.GridColumnStyles.Add(tcEstado)
                tsConfiguracion.GridColumnStyles.Add(tcDescipcionEstadoResources)
                tsConfiguracion.GridColumnStyles.Add(tcCantidad)
                tsConfiguracion.GridColumnStyles.Add(tcFechaInsercion)
                tsConfiguracion.GridColumnStyles.Add(tcNombreFase)
            Else
                tsConfiguracion.GridColumnStyles.Add(tcCheck)
                tsConfiguracion.GridColumnStyles.Add(tcDescripcion)
                tsConfiguracion.GridColumnStyles.Add(tcEstadoLinea)
                tsConfiguracion.GridColumnStyles.Add(tcEstadoLineaResources)
                tsConfiguracion.GridColumnStyles.Add(tcTiempoEstandar)
                tsConfiguracion.GridColumnStyles.Add(tcColAsignados)
                tsConfiguracion.GridColumnStyles.Add(tcEstado)
                tsConfiguracion.GridColumnStyles.Add(tcDescipcionEstadoResources)
                tsConfiguracion.GridColumnStyles.Add(tcCantidad)
                tsConfiguracion.GridColumnStyles.Add(tcResultado)
                tsConfiguracion.GridColumnStyles.Add(tcFechaInsercion)
                tsConfiguracion.GridColumnStyles.Add(tcNombreFase)
            End If



            'No visibles
            tsConfiguracion.GridColumnStyles.Add(tcNoOrden)
            tsConfiguracion.GridColumnStyles.Add(tcNoActividad)
            tsConfiguracion.GridColumnStyles.Add(tcNoFase)
            tsConfiguracion.GridColumnStyles.Add(tcAdicional)
            tsConfiguracion.GridColumnStyles.Add(tcCurrency)
            tsConfiguracion.GridColumnStyles.Add(tcPrecioAcordado)
            tsConfiguracion.GridColumnStyles.Add(tcObservaciones)

            'Establece propiedades del datagrid (colores estándares).
            tsConfiguracion.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
            tsConfiguracion.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
            tsConfiguracion.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
            tsConfiguracion.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
            tsConfiguracion.RowHeadersVisible = False
            tsConfiguracion.PreferredRowHeight = 50

            'Hace que el datagrid adopte las propiedades del TableStyle.
            dtgActividades.TableStyles.Add(tsConfiguracion)

        End Sub

        Private Sub CargarGridActividades(ByVal noFase As Integer, ByVal intAdicional As Integer)

            Dim dtvActividades As New DataView

            m_adpAct = New SCGDataAccess.ActividadesXFaseDataAdapter

            m_strNoOrdenAct = m_strNoOrden

            m_dstAct = Nothing

            m_dstAct = New ActividadesXFaseDataset

            Call EstiloGridActividades()

            Call m_adpAct.FillbyFilters(m_dstAct, m_strNoOrden, noFase, intAdicional)
            CargarTiempoUnidadesDatasetActividades()

            CargarEstadoLineaResources(m_dstAct)

            GlobalesUI.CargarEstadosActividadesResurces(m_dstAct)

            With dtvActividades
                .AllowDelete = False
                .AllowNew = False
                .Table = m_dstAct.SCGTA_TB_ActividadesxOrden
            End With

            dtgActividades.DataSource = dtvActividades

        End Sub

        Private Sub CambiaObservacionActividades(ByRef sender As DataGridTextBox)

            Dim intFila As Integer
            Dim strResultado As String
            Dim dtbItems As DMSOneFramework.ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable

            Dim drwAct As DMSOneFramework.ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow
            Dim objDA As New BLSBO.GlobalFunctionsSBO

            Dim visOrder As Integer
            Dim cadenaConexion As String = String.Empty
            Dim nombreTabla As String = "QUT1"
            Dim cnxFecha As New SqlConnection(DAConexion.ConnectionString)
            Dim comFecha As New SqlCommand
            comFecha = cnxFecha.CreateCommand

            Try

                dtbItems = m_dstAct.SCGTA_TB_ActividadesxOrden
                intFila = dtgActividades.CurrentCell.RowNumber

                drwAct = dtbItems.Rows(intFila)
                If CType(sender, DataGridTextBox).Text <> "" Then
                    strResultado = CType(sender, DataGridTextBox).Text

                    'visOrder = ObtieneVisOrder(BLSBO.oCompany, nombreTabla, cadenaConexion, drwAct.LineNum, drwAct.NoActividad, m_drdOrdenCurrent.NoCotizacion)

                    objDA.ActualizaObservacionLinea(m_drdOrdenCurrent.NoCotizacion, strResultado, drwAct.LineNum)

                   
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub


        Private Sub CambiaResultadoActividades(ByRef sender As DataGridTextBox)

            Dim intFila As Integer
            Dim strResultado As String
            Dim dtbItems As DMSOneFramework.ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable

            Dim drwAct As DMSOneFramework.ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow
            Dim objDA As New BLSBO.GlobalFunctionsSBO

            Dim visOrder As Integer
            Dim cadenaConexion As String = String.Empty
            Dim nombreTabla As String = "QUT1"
            Dim cnxFecha As New SqlConnection(DAConexion.ConnectionString)
            Dim comFecha As New SqlCommand
            comFecha = cnxFecha.CreateCommand
            Try

                dtbItems = m_dstAct.SCGTA_TB_ActividadesxOrden
                intFila = dtgActividades.CurrentCell.RowNumber

                drwAct = dtbItems.Rows(intFila)
                If CType(sender, DataGridTextBox).Text <> "" Then
                    strResultado = CType(sender, DataGridTextBox).Text

                    visOrder = ObtieneVisOrder(BLSBO.oCompany, nombreTabla, cadenaConexion, drwAct.LineNum, drwAct.NoActividad, m_drdOrdenCurrent.NoCotizacion)

                    objDA.ActualizaResultado(m_drdOrdenCurrent.NoCotizacion, strResultado, visOrder) 'drwAct.LineNum)

                    If cnxFecha.State = ConnectionState.Closed Then
                        cnxFecha.Open()
                        comFecha.CommandText = "Update SCGTA_TB_ActividadesxOrden set fechaSync =  GETDATE() Where ID = " & drwAct.ID
                        comFecha.ExecuteNonQuery()
                        cnxFecha.Close()
                    Else
                        comFecha.CommandText = "Update SCGTA_TB_ActividadesxOrden set fechaSync =  GETDATE() Where ID = " & drwAct.ID
                        comFecha.ExecuteNonQuery()
                        cnxFecha.Close()
                    End If

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub


        Private Sub CambiarEstadoActividades(ByVal strEstado As String)
            Dim objDA As New DMSOneFramework.SCGDataAccess.ActividadesXFaseDataAdapter
            Dim drdActividadesDV As DMSOneFramework.ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow
            Dim IntCodFase As Integer
            Dim strMensaje As String = ""

            For Each drdActividadesDV In CType(dtgActividades.DataSource, DataView).Table.Rows
                If Not drdActividadesDV.Check Then
                    drdActividadesDV.RejectChanges()

                ElseIf drdActividadesDV.EstadoLinea = "Falta Aprobación" Then
                    drdActividadesDV.RejectChanges()
                    If strMensaje = "" Then
                        strMensaje = "'" & drdActividadesDV.ItemName & "'"
                    Else
                        strMensaje = strMensaje & ", '" & drdActividadesDV.ItemName & "'"
                    End If
                End If


            Next
            If strMensaje <> "" Then
                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeNosePuedeCambiarEstadoActividades & " " & strMensaje & " " & My.Resources.ResourceUI.MensajeNoHansidoAprobadas)
            End If
            objDA.Update(CType(dtgActividades.DataSource, DataView).Table, strEstado)

            IntCodFase = CInt(Busca_Codigo_Texto(cboFasesProdF.Text, True))
            CargarGridActividades(IntCodFase, IIf(chkAdicionalAct.Checked, 1, 0))
        End Sub

        Private Sub EliminarActividades()

            Dim objDA As New DMSOneFramework.SCGDataAccess.ActividadesXFaseDataAdapter
            Dim drwActividades As DMSOneFramework.ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow
            Dim IntCodFase As Integer
            Dim strMensaje As String = ""
            Dim blnEliminarPaquetes As Boolean = False

            Try

                MetodosCompartidosSBOCls.IniciaTransaccion()

                MetodosCompartidosSBOCls.IniciarCotizacion(m_drdOrdenCurrent.NoCotizacion)

                For Each drwActividades In m_dstAct.SCGTA_TB_ActividadesxOrden.Rows

                    If Not drwActividades.Check Then

                        drwActividades.RejectChanges()
                    ElseIf drwActividades.LineNumFather <> -1 Then
                        blnEliminarPaquetes = True
                        If MessageBox.Show(My.Resources.ResourceUI.PreguntaItemPertenecePaqueteEliminar, My.Resources.ResourceUI.EliminarItems, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            If EliminarPaquete(drwActividades.LineNumFather, strMensaje) Then
                                MessageBox.Show(My.Resources.ResourceUI.MensajeLosSiguientesItems & ": " & strMensaje & " " & My.Resources.ResourceUI.MensajeFueronEliminadosCorrectamente)
                                Exit For
                            Else
                                MessageBox.Show(My.Resources.ResourceUI.MensajePaqueteNoEliminadoPuesLosItems & " " & strMensaje & " " & My.Resources.ResourceUI.MensajeNoPuedenEliminarse)
                            End If
                        End If

                    ElseIf drwActividades.Estado <> mc_strNoIniciada Then

                        drwActividades.RejectChanges()
                        If strMensaje = "" Then
                            strMensaje = "'" & drwActividades.ItemName & "'"
                        Else
                            strMensaje = strMensaje & ", '" & drwActividades.ItemName & "'"
                        End If

                    Else
                        If (drwActividades.CodEstadoLinea = SCGEstadoLinea.scgFaltaAprobacion) Or (drwActividades.Estado = mc_strNoIniciada) Then
                            MetodosCompartidosSBOCls.EliminarItemCotizacion(drwActividades.LineNum)
                            drwActividades.Delete()
                            g_AgregaAdicionales = True
                        Else

                            If strMensaje = "" Then
                                strMensaje = "'" & drwActividades.ItemName & "'"
                            Else
                                strMensaje = strMensaje & ", '" & drwActividades.ItemName & "'"
                            End If

                        End If

                    End If
                Next

                MetodosCompartidosSBOCls.ActualizarCotizacion()
                MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Commit)

                If Not blnEliminarPaquetes Then
                    If strMensaje <> "" Then
                        objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeLasActividades & " '" & strMensaje & "' " & My.Resources.ResourceUI.MensajeNosepuedenEliminarXEstado)
                    End If

                    Call objDA.UpdateEliminar(m_dstAct.SCGTA_TB_ActividadesxOrden)
                    IntCodFase = CInt(Busca_Codigo_Texto(cboFasesProdF.Text, True))
                    CargarGridActividades(IntCodFase, IIf(chkAdicionalAct.Checked, 1, 0))
                End If
            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

                MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)

            End Try

        End Sub

        Private Sub ShowToolTipInfo(ByVal p_objPoint As Point)
            Dim objHTI As DataGrid.HitTestInfo
            Dim strTextoShow As String = String.Empty

            Dim CurrentCell As DataGridCell

            objHTI = dtgActividades.HitTest(p_objPoint)

            If objHTI.Type = DataGrid.HitTestType.Cell Then

                If objHTI.Column = 5 Then

                    CurrentCell.ColumnNumber = objHTI.Column
                    CurrentCell.RowNumber = objHTI.Row

                    If Not IsDBNull(dtgActividades.Item(CurrentCell)) Then
                        strTextoShow = CStr(dtgActividades.Item(CurrentCell))
                    End If
                    
                    TTColaboras.SetToolTip(dtgActividades, strTextoShow) 'm_strTextoInfo)

                Else

                    TTColaboras.SetToolTip(dtgActividades, "")

                End If

            Else

                TTColaboras.SetToolTip(dtgActividades, "")

            End If

        End Sub

        Private Sub CambioPrecioAcordadoAct(ByRef p_txtNewValue As DataGridTextBox)

            Dim intFila As Integer
            Dim dblPrecio As Double
            Dim dtbItems As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable

            Dim drwActividad As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow
            Dim objDA As New BLSBO.GlobalFunctionsSBO

            Dim visOrder As Integer
            Dim cadenaConexion As String = String.Empty
            Dim nombreTabla As String = "QUT1"

            Try

                dtbItems = m_dstAct.SCGTA_TB_ActividadesxOrden
                intFila = dtgActividades.CurrentCell.RowNumber

                drwActividad = dtbItems.Rows(intFila)
                'If CType(sender, DataGridTextBox).Text <> "" Then
                If p_txtNewValue.Text <> "" Then
                    'dblPrecio = IIf(IsNumeric(CType(sender, DataGridTextBox).Text), Abs(CDbl(CType(sender, DataGridTextBox).Text)), -1)
                    dblPrecio = IIf(IsNumeric(p_txtNewValue.Text), Abs(CDbl(p_txtNewValue.Text)), -1)
                    If dblPrecio > -1 Then

                        visOrder = ObtieneVisOrder(BLSBO.oCompany, nombreTabla, cadenaConexion, drwActividad.LineNum, drwActividad.NoActividad, m_drdOrdenCurrent.NoCotizacion)

                        objDA.AgregarPrecioAcordado(m_drdOrdenCurrent.NoCotizacion, dblPrecio, visOrder) 'drwActividad.LineNum)
                        'objDA.AgregarPrecioAcordado(m_drdOrdenCurrent.NoCotizacion, dblPrecio, drwActividad.NoActividad)
                        m_intFilaAnterior = intFila

                    Else
                        MessageBox.Show(My.Resources.ResourceUI.MensajePrecioAcordadoDebeSerNumerico)
                        drwActividad.RejectChanges()
                    End If
                End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub CambioCantidadAct(ByRef p_txtNewValue As DataGridTextBox)

            Dim intFila As Integer
            Dim dblCantidad As Double
            Dim dtbItems As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable

            Dim drwActividad As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow
            Dim objDA As New BLSBO.GlobalFunctionsSBO

            Dim visOrder As Integer
            Dim cadenaConexion As String = String.Empty
            Dim nombreTabla As String = "QUT1"


            Try

                dtbItems = m_dstAct.SCGTA_TB_ActividadesxOrden
                intFila = dtgActividades.CurrentCell.RowNumber

                drwActividad = dtbItems.Rows(intFila)
                If p_txtNewValue.Text <> "" Then
                    dblCantidad = IIf(IsNumeric(p_txtNewValue.Text), Abs(CDbl(p_txtNewValue.Text)), -1)
                    If dblCantidad > -1 Then

                        visOrder = ObtieneVisOrder(BLSBO.oCompany, nombreTabla, cadenaConexion, drwActividad.LineNum, drwActividad.NoActividad, m_drdOrdenCurrent.NoCotizacion)

                        If objDA.ActualizarCantidadAct(m_drdOrdenCurrent.NoCotizacion, dblCantidad, visOrder) = 0 Then 'drwActividad.LineNum) = 0 Then


                            ActualizarCantidadActDMSDB(m_drdOrdenCurrent.NoOrden, drwActividad.NoActividad, drwActividad.LineNum, _
                                    drwActividad.NoFase, drwActividad.Duracion, dblCantidad)
                            m_intFilaAnterior = intFila
                        Else

                            MessageBox.Show(My.Resources.ResourceUI.MensajeLaCantidadDebeSerMayor)
                            drwActividad.RejectChanges()

                        End If

                    Else

                        MessageBox.Show(My.Resources.ResourceUI.MensajeCantidadActDebeSerNumerico)
                        drwActividad.RejectChanges()

                    End If
                End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub CambioTiempoEstandarAct(ByRef p_txtNewValue As DataGridTextBox)
            CargarUnidadesTiempoGlobales()
            Dim intFila As Integer
            Dim dblTiempo As Double
            Dim dtbItems As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenDataTable

            Dim drwActividad As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow
            Dim objDA As New DMSOneFramework.SCGDataAccess.ActividadesXFaseDataAdapter

            Dim oCotizacion As SAPbobsCOM.Documents

            Try
                dtbItems = m_dstAct.SCGTA_TB_ActividadesxOrden
                intFila = dtgActividades.CurrentCell.RowNumber

                drwActividad = dtbItems.Rows(intFila)
                If p_txtNewValue.Text <> "" Then

                    If g_intUnidadTiempo = -1 Then
                        dblTiempo = IIf(IsNumeric(p_txtNewValue.Text), Abs(CDbl(p_txtNewValue.Text)), -1)
                    Else

                        dblTiempo = IIf(IsNumeric(p_txtNewValue.Text), Abs(CDbl(p_txtNewValue.Text)), -1)

                        If dblTiempo > -1 Then
                            dblTiempo = dblTiempo * m_dblValorUnidadTiempo
                        End If

                    End If

                    If dblTiempo > -1 Then
                        objDA.UpdateTiempoEstandarActividades(m_drdOrdenCurrent.NoOrden, dblTiempo, drwActividad.ID)
                        m_intFilaAnterior = intFila

                        oCotizacion = CType(BLSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations),  _
                                                                SAPbobsCOM.Documents)
                        oCotizacion.GetByKey(m_drdOrdenCurrent.NoCotizacion)

                        'Recorro las lineas de la Oferta Ventas y asigno el nuevo tiempo a la linea indicada deacuerdo al ID
                        For i As Integer = 0 To oCotizacion.Lines.Count - 1
                            oCotizacion.Lines.SetCurrentLine(i)
                            If oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = drwActividad.ID Then
                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = dblTiempo.ToString()
                            End If
                        Next
                        oCotizacion.Update()

                    Else
                        MessageBox.Show(My.Resources.ResourceUI.MensajeTiempoEstandarDebeSerNumerico)
                        drwActividad.RejectChanges()
                    End If
                End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub


        Private Sub CargarTiempoUnidadesDatasetActividades()

            Dim intIndice As Integer
            For intIndice = 0 To m_dstAct.SCGTA_TB_ActividadesxOrden.Rows.Count - 1
                If m_dblValorUnidadTiempo > 0 Then
                    If Not m_dstAct.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("Duracion") Is System.DBNull.Value Then
                        m_dstAct.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("TotalUnidadTiempo") = Math.Round(m_dstAct.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("Duracion") / m_dblValorUnidadTiempo, 4)
                    Else
                        m_dstAct.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("TotalUnidadTiempo") = System.DBNull.Value
                    End If
                Else
                    m_dstAct.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("TotalUnidadTiempo") = 0
                End If

            Next
        End Sub

        Private Sub ActualizarCantidadActDMSDB(ByVal p_strNoOrden As String, ByVal p_strNoActividad As String, _
                ByVal p_intLineNum As Integer, ByVal p_intNoFase As Integer, ByVal p_dblDuracion As Double, _
                ByVal p_dblCantidad As Double)

            Dim adpActXOrden As New DMSOneFramework.SCGDataAccess.ActividadesXFaseDataAdapter

            If adpActXOrden.ActualizarCantidadActXOrden(p_strNoOrden, p_strNoActividad, _
                 p_intLineNum, p_intNoFase, p_dblDuracion, _
                 p_dblCantidad) <> 0 Then

                m_dstAct.AcceptChanges()

            End If

        End Sub

        Public Function ObtieneVisOrder(ByVal company As SAPbobsCOM.Company, ByVal nombreTabla As String, ByVal cadenaConexion As String, ByVal lineNum As Integer, ByVal itemCode As String, ByVal docEntry As Integer) As Integer
            Dim visOrder As Nullable(Of Integer)

            Dim rSet As SAPbobsCOM.Recordset = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            Try
                rSet.DoQuery(String.Format("SELECT VisOrder FROM {0} WHERE  DocEntry = {3} AND LineNum = {1} AND ItemCode = '{2}'", nombreTabla, lineNum, itemCode, docEntry))
                If rSet.RecordCount <> 0 Then
                    visOrder = rSet.Fields.Item(0).Value
                    If visOrder.HasValue Then Return visOrder.Value
                End If
                Throw New ApplicationException("Error obteniendo visorder")
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw
            End Try

        End Function

#End Region

#Region "Eventos"

        Private Sub cboFasesProdF_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFasesProdF.SelectedIndexChanged
            Dim intFase As Integer

            Try

                If Not Me.MdiParent Is Nothing Then
                    Me.MdiParent.Cursor = Cursors.WaitCursor
                End If

                intFase = CInt(Busca_Codigo_Texto(Me.cboFasesProdF.Text, True))
                CargarGridActividades(intFase, IIf(chkAdicionalAct.Checked, 1, 0))

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

        Private Sub btnActividad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActividad.Click
            Dim strParametros As String = ""

            Dim objBLConexion As DMSOneFramework.SCGDataAccess.DAConexion

            Try

                Me.MdiParent.Cursor = Cursors.WaitCursor

                objBLConexion = New DMSOneFramework.SCGDataAccess.DAConexion

                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)
                If txtNoOrden.Text <> "" Then


                    strParametros = strParametros & txtNoOrden.Text.Trim


                    With rptorden
                        .P_BarraTitulo = My.Resources.ResourceUI.rptTituloDocumentoActividades
                        .P_WorkFolder = PATH_REPORTES
                        .P_Filename = My.Resources.ResourceUI.rptNombreDocumentoActividades
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

        Private Sub btnActAdicional_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActAdicional.Click
            Dim strParametros As String = ""

            Dim objBLConexion As DMSOneFramework.SCGDataAccess.DAConexion

            Try

                Me.MdiParent.Cursor = Cursors.WaitCursor

                objBLConexion = New DMSOneFramework.SCGDataAccess.DAConexion

                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)
                If txtNoOrden.Text <> "" Then


                    strParametros = strParametros & txtNoOrden.Text.Trim


                    With rptorden
                        .P_BarraTitulo = My.Resources.ResourceUI.rptTituloDocumentoActividadesAdicionales
                        .P_WorkFolder = PATH_REPORTES
                        .P_Filename = My.Resources.ResourceUI.rptNombreDocumentoActividadesAdicionales
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

        Private Sub btnCambiarEstadoActividad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCambiarEstadoActividad.Click
            Try
                Me.MdiParent.Cursor = Cursors.WaitCursor

                If cboEstado.Text <> "" Then
                    CambiarEstadoActividades(cboEstado.Text)
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

        Private Sub chkAdicionalAct_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAdicionalAct.CheckedChanged
            Dim intFase As Integer

            Try

                intFase = CInt(Busca_Codigo_Texto(Me.cboFasesProdF.Text, True))
                CargarGridActividades(intFase, IIf(chkAdicionalAct.Checked, 1, 0))

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub btnEliminarAct_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarAct.Click
            Try
                Me.MdiParent.Cursor = Cursors.WaitCursor

                EliminarActividades()

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally

                If cboFases_Producción.SelectedIndex > -1 Then
                    CargarGridActividades(CInt(cboFases_Producción.SelectedValue), IIf(chkAdicionalAct.Checked, 1, 0))
                Else
                    CargarGridActividades(0, IIf(chkAdicionalAct.Checked, 1, 0))
                End If

            End Try
        End Sub

        Private Sub btnAgregarAct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregarAct.Click

            Dim frmAdicionales As New frmAdicionales1(2, m_strNoOrden, m_drdOrdenCurrent.NoCotizacion, m_blnAgregaAdicional, m_drdVisitaCurrent.NoVisita)
            Call frmAdicionales.ShowDialog()
            If cboFases_Producción.SelectedIndex > -1 Then
                CargarGridActividades(CInt(cboFases_Producción.SelectedValue), IIf(chkAdicionalAct.Checked, 1, 0))
            Else
                CargarGridActividades(0, IIf(chkAdicionalAct.Checked, 1, 0))
            End If

        End Sub

        Private Sub dtgActividades_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgActividades.GotFocus
            Try

                G_CancelarEditColumnDataGrid(Me, dtgActividades)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtgActividades_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dtgActividades.MouseMove
            Try

                ShowToolTipInfo(New Point(e.X, e.Y))

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub


        '********************************************************************************************
        'Agregado 29/02/2012: Agregar validación de tiempo estándar
        'Autor: José Soto
        Public Function ValidarGridActividades() As Boolean


            'Dim intIndice As Integer

            'For intIndice = 0 To m_dstAct.SCGTA_TB_ActividadesxOrden.Rows.Count - 1

            '    If m_dstAct.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("TotalUnidadTiempo") <= 0 Then

            '        MessageBox.Show(m_dstAct.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("ItemName"))

            '    End If

            '    If Not m_dstAct.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("Duracion") <= 0 Then

            '        MessageBox.Show(m_dstAct.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("ItemName"))

            '    End If


            'If m_dblValorUnidadTiempo > 0 Then
            '    If Not m_dstAct.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("Duracion") Is System.DBNull.Value Then
            '        m_dstAct.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("TotalUnidadTiempo") = Math.Round(m_dstAct.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("Duracion") / m_dblValorUnidadTiempo, 4)
            '    Else
            '        m_dstAct.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("TotalUnidadTiempo") = System.DBNull.Value
            '    End If
            'Else
            '    m_dstAct.SCGTA_TB_ActividadesxOrden.Rows(intIndice)("TotalUnidadTiempo") = 0
            'End If

            'Next



            Dim objDA As New DMSOneFramework.SCGDataAccess.ActividadesXFaseDataAdapter
            Dim drdActividadesDV As DMSOneFramework.ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow

            Dim IntCodFase As Integer
            Dim strMensaje As String = ""

            For Each drdActividadesDV In CType(dtgActividades.DataSource, DataView).Table.Rows
                If Not drdActividadesDV.Check Then
                    drdActividadesDV.RejectChanges()

                ElseIf drdActividadesDV.TotalUnidadTiempo <= 0 Then
                    MessageBox.Show("Error")
                    If strMensaje = "" Then
                        strMensaje = "'" & drdActividadesDV.ItemName & "'"
                    Else
                        strMensaje = strMensaje & ", '" & drdActividadesDV.ItemName & "'"
                    End If
                End If


            Next

            '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            'For i As Integer = 0 To dtgActividades.VisibleRowCount - 1

            '    '    'If dtgActividades.Item(i, 2).ToString = "0" Then

            '    If dtgActividades.Item(i, 3).value <= 0 Then

            '        MessageBox.Show(CStr(dtgActividades.Item(i, 3).value))


            '    End If


            '    '    Return True
            '    '    Exit For

            '    '    'End If

            'Next

        End Function

        '********************************************************************************************


#End Region

    End Class

End Namespace
