Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework
Imports DMSOneFramework.BLSBO.GlobalFunctionsSBO
Imports DMSOneFramework.BLSBO
Imports DMSOneFramework.SCGCommon
'Imports SCG_ComponenteImagenes.SCG_Imagenes

Namespace SCG_User_Interface

    Partial Class frmOrden
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

#Region "Declaraciones"

#Region "Constantes"

        'Produccion
        Private Const mccol_intID As String = "ID"
        Private Const mccol_intEmpId As String = "EmpID"
        Private Const mccol_dtFechaInicio As String = "FechaInicio"
        Private Const mccol_dtFechaFin As String = "fechaFin"
        Private Const mccol_intReproceso As String = "Reproceso"
        Private Const mccol_dblCosto As String = "Costo"
        Private Const mccol_dblTiempoHoras As String = "TiempoHoras"
        Private Const mccol_strNoOrden As String = "NoOrden"
        Private Const mccol_intNoFase As String = "NoFase"
        Private Const mccol_strEmpNombre As String = "EmpNombre"
        Private Const mccol_strEstado As String = "Estado"
        Private Const mccol_intReferencia As String = "Referencia"
        Private Const mccol_intIndicador As String = "Indicador"
        Private Const mccol_blnCheck As String = "Check"
        Private Const mccol_strRazon As String = "Razon"
        Private Const mccol_intNoRazon As String = "NoRazon"
        Private Const mccol_strIDActividad As String = "IDActividad"
        Private Const mccol_strActividadDesc As String = "ActividadDesc"
        Private Const mccol_strTableName As String = "SCGTA_TB_CONTROLCOLABORADOR"
        Private Const mc_strProcesoManual As String = "Manual"
        Private Const mc_strColUnidadTiempo As String = "TotalUnidadTiempo"
        Private Const mc_strColTiempoEstandar As String = "TiempoEstandar"
        Private Const mc_strDescripcionActividadResources As String = "DescripcionActividadResources"
        Private Const mc_strReAsignado As String = "ReAsignado"

        'Por Fase
        Private Const mc_Estado_Finalizado As String = "Finalizado"
        Private Const mc_Estado_Suspendido As String = "Suspendido"
        Private Const mc_Estado_NoIniciado As String = "No iniciado"
        Private Const mc_Estado_Iniciado As String = "Iniciado"

        Private objUtilitarios2 As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)
#End Region

#Region "Objetos"

#Region "Datasets"

        Public m_dstCol As ColaboradorDataset
        
#End Region

#Region "Adapters"

        Private m_adpCol As SCGDataAccess.ColaboradorDataAdapter

#End Region

#Region "DataRows"

        Private drwCol As ColaboradorDataset.SCGTA_TB_ControlColaboradorRow

#End Region

#Region "Formularios"

        Private WithEvents m_objFrmAsignarActividades As frmAsignarActividades
        Private WithEvents m_objFrmAsignarHoras As frmTrabajoActividad
        Private WithEvents m_objFrmAsignacionTiempos As frmAsignacionTiempos
        Private WithEvents m_objFrmAsignarTiempo As frmAsignacionTiempos

#End Region

#End Region

#End Region

#Region "Procedimientos"

        Private Sub AsignarColaborador(ByVal intNuevaAsignacion As Integer, ByVal intFase As Integer, ByVal p_intIDActividad As Integer,
                                       ByVal p_intDocEntry As Integer)


            If ValidarColaborador() Then
                Exit Sub
            End If


            If ValidarAsignacionUnicaMO(p_intIDActividad) = True Then
                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.ValidarAsignacionUnicaMOSel)
                Exit Sub
            End If


            If intNuevaAsignacion = 0 Then
                AsinacionNueva(intFase, p_intIDActividad, p_intDocEntry)
            End If

            If p_intIDActividad > 0 Then
                ActualizaReAsignacion(p_intIDActividad)
            End If


            CargarGridColaborador(Busca_Codigo_Texto(cboFases_Producción.Text))

            intFase = CInt(Busca_Codigo_Texto(Me.cboFasesProdF.Text, True))
            CargarGridActividades(intFase, IIf(chkAdicionalAct.Checked, 1, 0))

        End Sub

        Private Sub EstiloGridColaborador()
            Dim m_ColumnaCondicional As Integer = 18
            Dim m_ColorCondicional As Color

            'Dim mensaje As String
            'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.

            'Declaraciones generales
            Dim tsConfiguracion As New DataGridTableStyle
            dtgcolaborador.TableStyles.Clear()

            Dim tcID As New DataGridConditionalColumn
            Dim tcNoFase As New DataGridTextBoxColumn
            Dim tcNoOrden As New DataGridTextBoxColumn
            Dim tcReproceso As New DataGridBoolColumn
            Dim tcEmpID As New DataGridConditionalColumn
            Dim tcEmpNombre As New DataGridConditionalColumn
            Dim tcFechaInicio As New DataGridConditionalColumn
            Dim tcFechaFin As New DataGridConditionalColumn
            Dim tcTiempoHoras As New DataGridConditionalColumn
            Dim tcEstado As New DataGridConditionalColumn
            Dim tcCosto As New DataGridConditionalColumn
            Dim tcReferencia As New DataGridConditionalColumn
            Dim tcIndicador As New DataGridConditionalColumn
            Dim tcRazon As New DataGridConditionalColumn
            Dim tcCheck As New DataGridCheckColumn
            Dim tcNoRazon As New DataGridTextBoxColumn
            Dim tcIDActividad As New DataGridConditionalColumn
            Dim tcActividadDesc As New DataGridConditionalColumn
            Dim tcUnidadTiempo As New DataGridConditionalColumn
            Dim tcTiempoEstandar As New DataGridConditionalColumn
            Dim tcEstadoResources As New DataGridConditionalColumn

            Dim tcReAsignado As New DataGridBoolColumn

            tsConfiguracion.MappingName = m_dstCol.SCGTA_TB_ControlColaborador.TableName()

            m_ColorCondicional = Color.Gray

            If g_intUnidadTiempo <> -1 Then
                With tcUnidadTiempo
                    .Width = 60
                    'If String.IsNullOrEmpty(m_strDescripcionUnidadTiempo) Then
                    'm_strDescripcionUnidadTiempo = ""
                    'End If
                    .HeaderText = My.Resources.ResourceUI.TiempoReal 'm_strDescripcionUnidadTiempo
                    .MappingName = mc_strColUnidadTiempo
                    .ReadOnly = True
                    .P_ColorCondicional = m_ColorCondicional
                    .P_ColumnaCondicional = m_ColumnaCondicional
                End With
            End If



            With tcTiempoEstandar
                .Width = 65
                .HeaderText = My.Resources.ResourceUI.DuracionEstandar
                .MappingName = mc_strColTiempoEstandar
                .ReadOnly = True
                .P_ColorCondicional = m_ColorCondicional
                .P_ColumnaCondicional = m_ColumnaCondicional
            End With


            With tcID
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.ID
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_intID).ColumnName
                .P_ColorCondicional = m_ColorCondicional
                .P_ColumnaCondicional = m_ColumnaCondicional
                .ReadOnly = True
            End With

            With tcNoFase
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.NoFase  '"No Fase"
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_intNoFase).ColumnName
                .Format = "###"
                .ReadOnly = True
            End With

            With tcNoOrden
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.NoOrden  '"No Orden"
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_strNoOrden).ColumnName
                .Format = "###"
                .ReadOnly = True
            End With

            With tcNoRazon
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.NoRazon  '"No Razón"
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_intNoRazon).ColumnName
                .Format = "###"
                .ReadOnly = True
            End With

            With tcReproceso
                .Width = 40
                .HeaderText = My.Resources.ResourceUI.Reproceso
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_intReproceso).ColumnName
                .ReadOnly = True
                .AllowNull = False
            End With

            With tcEmpID
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.ID
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_intEmpId).ColumnName
                .P_ColorCondicional = m_ColorCondicional
                .P_ColumnaCondicional = m_ColumnaCondicional
                .ReadOnly = True
            End With

            With tcEmpNombre
                .Width = 242
                .HeaderText = My.Resources.ResourceUI.Colaboradores  '"Colaborador"
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_strEmpNombre).ColumnName
                .P_ColorCondicional = m_ColorCondicional
                .P_ColumnaCondicional = m_ColumnaCondicional
                .ReadOnly = True
            End With

            With tcFechaInicio
                .Width = 140
                .HeaderText = My.Resources.ResourceUI.FechaInicio '"Fecha inicio"
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_dtFechaInicio).ColumnName
                .NullText = ""
                .P_ColorCondicional = m_ColorCondicional
                .P_ColumnaCondicional = m_ColumnaCondicional
                .ReadOnly = True
            End With

            With tcFechaFin
                .Width = 140
                .HeaderText = My.Resources.ResourceUI.FechaFin
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_dtFechaFin).ColumnName
                .NullText = ""
                .P_ColorCondicional = m_ColorCondicional
                .P_ColumnaCondicional = m_ColumnaCondicional
                .ReadOnly = True
            End With

            With tcRazon
                .Width = 250
                .HeaderText = My.Resources.ResourceUI.Suspendidopor   '"Suspendido por"
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_strRazon).ColumnName
                .P_ColorCondicional = m_ColorCondicional
                .P_ColumnaCondicional = m_ColumnaCondicional
                .ReadOnly = True
            End With


            With tcTiempoHoras

                If g_intUnidadTiempo = -1 Then
                    .Width = 85
                Else
                    .Width = 0
                End If

                .HeaderText = My.Resources.ResourceUI.TotalMinutos
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_dblTiempoHoras).ColumnName
                .P_ColorCondicional = m_ColorCondicional
                .P_ColumnaCondicional = m_ColumnaCondicional
                .ReadOnly = True
            End With

            With tcEstado
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.Estado  '"Estado"
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_strEstado).ColumnName
                .P_ColorCondicional = m_ColorCondicional
                .P_ColumnaCondicional = m_ColumnaCondicional
                .ReadOnly = True
            End With

            With tcEstadoResources
                .Width = 70
                .HeaderText = My.Resources.ResourceUI.Estado
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mc_strDescripcionActividadResources).ColumnName
                .P_ColorCondicional = m_ColorCondicional
                .P_ColumnaCondicional = m_ColumnaCondicional
                .ReadOnly = True
            End With

            With tcCosto
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.Costo  '"Costo"
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_dblCosto).ColumnName
                .P_ColorCondicional = m_ColorCondicional
                .P_ColumnaCondicional = m_ColumnaCondicional
                .ReadOnly = True
            End With

            With tcReferencia
                .Width = 40
                .HeaderText = My.Resources.ResourceUI.Referencia
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_intReferencia).ColumnName
                .P_ColorCondicional = m_ColorCondicional
                .P_ColumnaCondicional = m_ColumnaCondicional
                .NullText = ""
                .ReadOnly = True
            End With

            With tcIndicador
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.indicador  '"Indicador"
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_intIndicador).ColumnName
                .P_ColorCondicional = m_ColorCondicional
                .P_ColumnaCondicional = m_ColumnaCondicional
                .ReadOnly = True
            End With

            With tcCheck
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_blnCheck).ColumnName
                .Width = 30
                .AllowNull = False
            End With

            With tcIDActividad
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.NoActividad   '"IDActividad"
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_strIDActividad).ColumnName
                .P_ColorCondicional = m_ColorCondicional
                .P_ColumnaCondicional = m_ColumnaCondicional
                .ReadOnly = True
            End With

            With tcActividadDesc
                .Width = 250
                .HeaderText = My.Resources.ResourceUI.actividadasignada   '"Actividad asignada"
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mccol_strActividadDesc).ColumnName
                .P_ColorCondicional = m_ColorCondicional
                .P_ColumnaCondicional = m_ColumnaCondicional
                .ReadOnly = True
            End With


            With tcReAsignado
                .Width = 80
                .HeaderText = My.Resources.ResourceUI.ReAsignado
                .MappingName = m_dstCol.SCGTA_TB_ControlColaborador.Columns(mc_strReAsignado).ColumnName
                .ReadOnly = True
                .AllowNull = False
            End With

            'Agrega las columnas al tableStyle
            tsConfiguracion.GridColumnStyles.Add(tcCheck)
            tsConfiguracion.GridColumnStyles.Add(tcNoFase)
            tsConfiguracion.GridColumnStyles.Add(tcNoOrden)
            tsConfiguracion.GridColumnStyles.Add(tcID)
            tsConfiguracion.GridColumnStyles.Add(tcEmpID)
            tsConfiguracion.GridColumnStyles.Add(tcEmpNombre)
            tsConfiguracion.GridColumnStyles.Add(tcFechaInicio)
            tsConfiguracion.GridColumnStyles.Add(tcFechaFin)
            tsConfiguracion.GridColumnStyles.Add(tcTiempoHoras)
            tsConfiguracion.GridColumnStyles.Add(tcUnidadTiempo)
            tsConfiguracion.GridColumnStyles.Add(tcTiempoEstandar)
            tsConfiguracion.GridColumnStyles.Add(tcReproceso)
            tsConfiguracion.GridColumnStyles.Add(tcEstado)
            tsConfiguracion.GridColumnStyles.Add(tcEstadoResources)
            tsConfiguracion.GridColumnStyles.Add(tcCosto)
            tsConfiguracion.GridColumnStyles.Add(tcIDActividad)
            tsConfiguracion.GridColumnStyles.Add(tcActividadDesc)
            tsConfiguracion.GridColumnStyles.Add(tcReferencia)
            tsConfiguracion.GridColumnStyles.Add(tcIndicador)
            tsConfiguracion.GridColumnStyles.Add(tcNoRazon)
            tsConfiguracion.GridColumnStyles.Add(tcRazon)

            tsConfiguracion.GridColumnStyles.Add(tcReAsignado)

            'Establece propiedades del datagrid (colores estándares).
            tsConfiguracion.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
            tsConfiguracion.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
            tsConfiguracion.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
            tsConfiguracion.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
            tsConfiguracion.RowHeadersVisible = False

            'Hace que el datagrid adopte las propiedades del TableStyle.
            dtgcolaborador.TableStyles.Add(tsConfiguracion)

        End Sub

        Private Sub CargarGridColaborador(ByVal codfase As Integer)

            Dim intIndicador As Integer
            Dim objDTVCol As DataView = Nothing



            m_adpCol = New SCGDataAccess.ColaboradorDataAdapter

            If Not IsNothing(m_dstCol) Then
                m_dstCol.Dispose()
                m_dstCol = Nothing
            End If

            m_dstCol = New ColaboradorDataset

            EstiloGridColaborador()

            If chkRefSuperiores.Checked Then
                intIndicador = 1
            Else
                intIndicador = 0
            End If

            Call m_adpCol.Fill(m_dstCol, txtNoOrden.Text, codfase, intIndicador)
            GlobalesUI.CargarEstadosActividadesResurces(m_dstCol)
            Call CargarUnidadesTiempoDataset()
            Call m_adpCol.CargarDuracionEstandar(m_dstCol, g_intUnidadTiempo, m_dblValorUnidadTiempo)

            If Not IsNothing(objDTVCol) Then
                objDTVCol.Dispose()
            End If

            objDTVCol = New DataView

            With objDTVCol
                .AllowDelete = False
                .AllowNew = False
                .Table = m_dstCol.SCGTA_TB_ControlColaborador
            End With

            dtgcolaborador.DataSource = objDTVCol

        End Sub

        Private Sub CargarDatasetEstadoFaseXOrden(ByVal p_intFaseActual As Integer, ByVal p_strNoOrden As String)
            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
            Dim objDataRow() As FaseXOrdenEstadosDataset.SCGTA_TB_FasesxOrden_EstadosRow
            Dim intEstadoFase As Integer = 0
            
            If Not IsNothing(m_dtsCurrentFaseXOrdenEstado) Then
                m_dtsCurrentFaseXOrdenEstado.Dispose()
                m_dtsCurrentFaseXOrdenEstado = Nothing
            End If

            'intHayRechazo = objDA.EsEstadoFaseRechazo(p_strNoOrden, p_intFaseActual)

            If m_drdOrdenCurrent.Estado <> mc_NumEstado_Finalizada And m_drdOrdenCurrent.Estado <> mc_NumEstado_Cancelada Then

                m_dtsCurrentFaseXOrdenEstado = New FaseXOrdenEstadosDataset

                objDA.Fill(m_dtsCurrentFaseXOrdenEstado, p_intFaseActual, p_strNoOrden)

                With m_dtsCurrentFaseXOrdenEstado.SCGTA_TB_FasesxOrden_Estados

                    If .Rows.Count <> 0 Then

                        objDataRow = .Select("", "id desc")

                        If objDataRow.Length <> 0 Then

                            Select Case objDataRow(0).Estado

                                Case "Proceso"
                                    intEstadoFase = 1
                                Case "Suspendida"
                                    intEstadoFase = 2
                                Case "Finalizada"
                                    intEstadoFase = 3
                            End Select
                            'Agregado 22/08/06. Alejandra
                            'If (intHayRechazo = 1) Then 'Si la fase está Rechazada
                            '    intEstadoFase = 4
                            'End If
                            ''''''''''''
                            ProdCambiarEstadoFase(intEstadoFase)

                        End If
                    Else
                        'Agregado 22/08/06. Alejandra
                        'If (intHayRechazo = 1) Then 'Si la fase está Rechazada
                        '    intEstadoFase = 4
                        'End If
                        ''''''''''''
                        ProdCambiarEstadoFase(intEstadoFase)
                    End If

                End With

            End If


        End Sub

        Private Sub AsinacionNueva(ByVal p_intNoFase As Integer, ByVal p_intIDActividad As Integer, ByVal p_intDocEntry As Integer)
            Dim objDA As New SCGDataAccess.ColaboradorDataAdapter
            Dim dtsAsignados As New ColaboradorDataset
            Dim dtrAsignando As ColaboradorDataset.SCGTA_TB_ControlColaboradorRow
            Dim m_objCotizacion As SAPbobsCOM.Documents
            Dim oLineasCotizacion As SAPbobsCOM.Document_Lines
            Dim m_strValorId As String
            Dim m_strValorIdEmp As String


            dtrAsignando = dtsAsignados.SCGTA_TB_ControlColaborador.NewSCGTA_TB_ControlColaboradorRow

            With dtrAsignando
                .NoFase = p_intNoFase 'Busca_Codigo_Texto(cboFases_Producción.Text)
                .NoOrden = txtNoOrden.Text
                If chkReproceso.Checked Then
                    .Reproceso = 1
                Else
                    .Reproceso = 0
                End If
                .EmpID = Busca_Codigo_Texto(cbocolaborador.Text)
                .EmpNombre = Busca_Codigo_Texto(cbocolaborador.Text, False)
                '.FechaInicio = Nothing
                ' .FechaFin = Nothing
                .TiempoHoras = 0
                .Estado = mc_Estado_NoIniciado
                .Costo = 0
                .IDActividad = p_intIDActividad
            End With




            dtsAsignados.SCGTA_TB_ControlColaborador.AddSCGTA_TB_ControlColaboradorRow(dtrAsignando)

            objDA.InsertarNuevo(dtsAsignados)

            ''Inserta Mecanico en Cotizacion 09/05/2014
            m_objCotizacion = G_objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
            m_objCotizacion.GetByKey(p_intDocEntry)
            oLineasCotizacion = m_objCotizacion.Lines

            For i As Integer = 0 To oLineasCotizacion.Count - 1

                oLineasCotizacion.SetCurrentLine(i)
                m_strValorId = oLineasCotizacion.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString.Trim()
                m_strValorIdEmp = oLineasCotizacion.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim()
                If (p_intIDActividad = m_strValorId) Then

                    If (String.IsNullOrEmpty(m_strValorIdEmp)) Then
                        oLineasCotizacion.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = Busca_Codigo_Texto(cbocolaborador.Text)
                        oLineasCotizacion.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = Busca_Codigo_Texto(cbocolaborador.Text, False)


                    Else

                        oLineasCotizacion.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = ""
                        oLineasCotizacion.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = "Varios"


                    End If
                End If


            Next
            
            m_objCotizacion.Update()
        End Sub



        Private Sub ActualizaReAsignacion(ByVal p_intIDActividad As Integer)
            Dim objDA As New SCGDataAccess.ColaboradorDataAdapter

            Try
                objDA.UpdateReAsignarColaborador(p_intIDActividad)

            Catch ex As Exception
                Throw ex
            End Try

        End Sub

        Private Sub IniciarProceso()
            Dim objDA As New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
            Dim drdColaboradorDV As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorRow
            Dim IntCodFase As Integer
            Dim objBLSBO As BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal

            objBLSBO = New BLSBO.GlobalFunctionsSBO

            If m_dstCol.SCGTA_TB_ControlColaborador.Select("Check=1").Length <> 0 Then
                If objBLSBO.RetornarMonedaLocal <> objBLSBO.RetornarMonedaSistema Then
                    decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(objBLSBO.RetornarMonedaSistema, Today, strConectionString, True)
                Else
                    decTipoCambio = 1
                End If

                If decTipoCambio <> -1 Then

                    If SCGDataAccess.Utilitarios.GetPostingPeriod(Today).Trim <> "" Then

                        'Agregado 29/06/06. Alejandra
                        Dim adpFaseXOrdenEstados As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
                        Dim adpTiempos As New DMSOneFramework.SCGDataAccess.TiemposMuertosDataAdapter
                        '''''''

                        For Each drdColaboradorDV In CType(dtgcolaborador.DataSource, DataView).Table.Rows
                            If Not drdColaboradorDV.Check Then
                                drdColaboradorDV.RejectChanges()

                            Else

                                drdColaboradorDV.SetFechaInicioNull()

                            End If
                        Next

                        objDA.UpdateIniciar(CType(dtgcolaborador.DataSource, DataView).Table, mc_strProcesoManual)

                        'If Not btnSuspension.Enabled And Not btnFinalizar.Enabled Then
                        '    ProcesoIniciarFaseXOrden()
                        'End If
                        If mf_strEstado <> mc_Estado_Finalizado Then

                            'Agregado 29/06/06. Alejandra. Si la fase estaba suspendida, antes de iniciarla
                            'establece la fecha en que finalizó la suspensión
                            'adpFaseXOrdenEstados.EstablecerFinSuspension(txtNoOrden.Text, CInt(Busca_Codigo_Texto(cboFases_Producción.SelectedItem)))

                            'Agregado 17/08/06. Alejandra. Calcula tiempos muertos
                            If (objUtilitarios.HayFasesIniciadas(txtNoOrden.Text) = 0) Then
                                ProcesoIniciarFaseXOrden()
                                'adpTiempos.UPDTiemposMuertosIniciarOrden(txtNoOrden.Text, Busca_Codigo_Texto(cboFases_Producción.SelectedItem))
                            Else
                                'adpTiempos.UPDTiemposMuertosIniciarFase(txtNoOrden.Text, Busca_Codigo_Texto(cboFases_Producción.SelectedItem))
                                ProcesoIniciarFaseXOrden()
                            End If
                            ''''''''
                            'ProcesoIniciarFaseXOrden()
                        End If

                        IntCodFase = CInt(Busca_Codigo_Texto(cboFases_Producción.Text, True))
                        CargarGridColaborador(IntCodFase)
                        CargarGridActividades(0, IIf(chkAdicionalAct.Checked, 1, 0))

                    Else

                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOPeriodoFiscalInvalido)

                    End If
                Else

                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOActualizarTipoCambio)

                End If

            End If

        End Sub

        Private Sub SuspenderProceso(ByVal dtFecha As Date)
            Dim objDA As New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
            Dim drdColaboradorDV As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorRow
            Dim IntCodFase As Integer
            Dim dtbColaborasModif As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorDataTable
            Dim objBLSBO As BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal
            Dim strMonedaLocal As String
            Dim strMonedaSistema As String

            objBLSBO = New BLSBO.GlobalFunctionsSBO
            strMonedaSistema = objBLSBO.RetornarMonedaSistema
            strMonedaLocal = objBLSBO.RetornarMonedaLocal
            If strMonedaSistema <> strMonedaLocal Then
                decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(strMonedaSistema, Today, strConectionString, true)
            Else
                decTipoCambio = 1
            End If
            If decTipoCambio <> -1 Then

                If SCGDataAccess.Utilitarios.GetPostingPeriod(Today).Trim <> "" Then

                    For Each drdColaboradorDV In CType(dtgcolaborador.DataSource, DataView).Table.Rows
                        If Not drdColaboradorDV.Check Then
                            drdColaboradorDV.RejectChanges()
                        End If
                    Next

                    dtbColaborasModif = CType(dtgcolaborador.DataSource, DataView).Table.GetChanges

                    objDA.UpdateSuspender(CType(dtgcolaborador.DataSource, DataView).Table, G_intNoRazon, mc_strProcesoManual, dtFecha)

                    'CalculoCostosDtD(CType(dtgcolaborador.DataSource, DataView).Table, dtbColaborasModif)

                    IntCodFase = CInt(Busca_Codigo_Texto(cboFases_Producción.Text, True))

                    CargarGridColaborador(IntCodFase)

                Else

                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOPeriodoFiscalInvalido)

                End If
            Else

                objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOActualizarTipoCambio)

            End If

        End Sub

        Private Sub FinalizarProceso()
            Dim objDA As New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
            Dim drdColaboradorDV As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorRow
            Dim IntCodFase As Integer
            Dim dtbColaborasModif As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorDataTable
            Dim objBLSBO As BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal
            Dim strMonedaLocal As String
            Dim strMonedaSistema As String

            objBLSBO = New BLSBO.GlobalFunctionsSBO

            strMonedaSistema = objBLSBO.RetornarMonedaSistema
            strMonedaLocal = objBLSBO.RetornarMonedaLocal
            If strMonedaSistema <> strMonedaLocal Then
                decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(strMonedaSistema, Today, strConectionString, true)
            Else
                decTipoCambio = 1
            End If
            If decTipoCambio <> -1 Then

                If SCGDataAccess.Utilitarios.GetPostingPeriod(Today).Trim <> "" Then

                    For Each drdColaboradorDV In CType(dtgcolaborador.DataSource, DataView).Table.Rows
                        If Not drdColaboradorDV.Check Then
                            drdColaboradorDV.RejectChanges()
                        End If
                    Next

                    dtbColaborasModif = CType(dtgcolaborador.DataSource, DataView).Table.GetChanges

                    objDA.UpdateFinalizar(CType(dtgcolaborador.DataSource, DataView).Table, mc_strProcesoManual)

                    'CalculoCostosDtD(CType(dtgcolaborador.DataSource, DataView).Table, dtbColaborasModif)

                    IntCodFase = CInt(Busca_Codigo_Texto(cboFases_Producción.Text, True))

                    CargarGridColaborador(IntCodFase)
                    CargarGridActividades(0, IIf(chkAdicionalAct.Checked, 1, 0))

                Else

                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOPeriodoFiscalInvalido)

                End If
            Else

                objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOActualizarTipoCambio)

            End If

        End Sub

        Private Sub SuspenderFase(ByVal p_intNoFase As Integer, ByVal p_intNoSuspension As Integer)
            Dim objDA As New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
            Dim dstColaborador As New ColaboradorDataset

            'objDA.SuspenderFase(p_intNoFase, txtNoOrden.Text, p_intNoSuspension, mc_strProcesoManual)
            'Suspende los colaboradores de la fase

            objDA.SelColabIniciadosXOrdenXFase(dstColaborador, txtNoOrden.Text, p_intNoFase)
            ModificarDataSet(dstColaborador)
            objDA.UpdateSuspender(dstColaborador.SCGTA_TB_ControlColaborador, 0, mc_strProcesoManual, Nothing)

            'Calcula Costos
            'CalculoCostosDtD(dstColaborador.SCGTA_TB_ControlColaborador, dstColaborador.SCGTA_TB_ControlColaborador)

            If cboFases_Producción.Text.Trim <> "" Then
                If p_intNoFase = CInt(Busca_Codigo_Texto(cboFases_Producción.Text, True)) Then
                    CargarGridColaborador(p_intNoFase)
                End If
            End If

            ProcesoSuspenderFaseXOrden(p_intNoFase)

        End Sub

        Private Sub IniciarTBProduccionDocs()

            'btnDocumentos.Visible = False
            'btnReproceso.Visible = False
            'btnSuspension.Visible = False
            'btnRechazar.Visible = False

            btnDocumentos.Text = dropmnuProduccion.Text
            btnDocumentos.Tag = dropmnuProduccion.Index

        End Sub

        Private Sub ActivarTBProduccionDocs(ByVal intIndexSelected As Integer)

            With btnDocumentos

                Select Case intIndexSelected

                    Case dropmnuProduccion.Index

                        .Text = dropmnuProduccion.Text
                        .Tag = dropmnuProduccion.Index

                        ShowReporteProduccion()

                    Case dropmnuOficina.Index

                        .Text = dropmnuOficina.Text
                        .Tag = dropmnuOficina.Index

                        ShowReporteOficina()

                    Case dropmnuReprocesos.Index

                        .Text = dropmnuReprocesos.Text
                        .Tag = dropmnuReprocesos.Index

                        ShowReporteReprocesos()

                    Case dropmnuSuspenciones.Index

                        .Text = dropmnuSuspenciones.Text
                        .Tag = dropmnuSuspenciones.Index

                        ShowReporteSuspenciones()

                    Case dropmnuCostos.Index

                        .Text = dropmnuCostos.Text
                        .Tag = dropmnuCostos.Index

                        ShowReporteCostos()

                    Case dropmnuItemsNoAprobados.Index
                        .Text = dropmnuItemsNoAprobados.Text
                        .Tag = dropmnuItemsNoAprobados.Tag

                        ShowReporteItemsNoAprobados()

                        'Balance Orden Trabajo
                    Case dropmnuBalanceOT.Index
                        .Text = dropmnuBalanceOT.Text
                        .Tag = dropmnuBalanceOT.Tag

                        ShowReporteBalanceOT()


                End Select

            End With

        End Sub

        Private Sub ShowReporteProduccion()
            Dim strParametros As String = ""
            Dim objBLConexion As DMSOneFramework.SCGDataAccess.DAConexion

            objBLConexion = New DMSOneFramework.SCGDataAccess.DAConexion

            PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)
            If txtNoOrden.Text <> "" Then

                strParametros = strParametros & txtNoOrden.Text.Trim '& ","

                'strParametros = strParametros & txtNoVisita.Text.Trim

                With rptorden
                    .P_BarraTitulo = My.Resources.ResourceUI.rptTituloOrdenDeTrabajo
                    .P_WorkFolder = PATH_REPORTES
                    .P_Filename = My.Resources.ResourceUI.rptNombreOrdenTrabajo
                    .P_Server = Server
                    .P_DataBase = strDATABASESCG
                    .P_CompanyName = COMPANIA
                    .P_User = UserSCGInternal
                    .P_Password = Password
                    .P_ParArray = strParametros
                End With

                rptorden.VerReporte()
            Else
                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarOT)
            End If

        End Sub

        Private Sub ShowReporteItemsNoAprobados()
            Dim strParametros As String = ""
            Dim objBLConexion As DMSOneFramework.SCGDataAccess.DAConexion

            objBLConexion = New DMSOneFramework.SCGDataAccess.DAConexion

            PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)
            If txtNoOrden.Text <> "" Then

                strParametros = strParametros & txtNoOrden.Text.Trim '& ","

                'strParametros = strParametros & txtNoVisita.Text.Trim

                With rptorden
                    .P_BarraTitulo = My.Resources.ResourceUI.rptTituloItemsNoAprobados
                    .P_WorkFolder = PATH_REPORTES
                    .P_Filename = My.Resources.ResourceUI.rptNombreItemsNoAprobados
                    .P_Server = Server
                    .P_DataBase = strDATABASESCG
                    .P_User = UserSCGInternal
                    .P_Password = Password
                    .P_ParArray = strParametros
                End With

                rptorden.VerReporte()
            Else
                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarOT)
            End If

        End Sub

        'Reporte Balance OT
        Private Sub ShowReporteBalanceOT()
 
            Dim strParametros As String = ""
            Dim objBLConexion As DMSOneFramework.SCGDataAccess.DAConexion

            objBLConexion = New DMSOneFramework.SCGDataAccess.DAConexion

            PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)
            If txtNoOrden.Text <> "" Then

                strParametros = strParametros & txtNoOrden.Text.Trim

                With rptorden
                    .P_BarraTitulo = My.Resources.ResourceUI.TituloBalanceOT
                    .P_WorkFolder = PATH_REPORTES
                    .P_Filename = My.Resources.ResourceUI.rptBalanceOT
                    .P_Server = Server
                    .P_DataBase = strDATABASESCG
                    .P_User = UserSCGInternal
                    .P_Password = Password
                    .P_ParArray = strParametros
                End With

                rptorden.VerReporte()
            Else
                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarOT)
            End If

        End Sub

        Private Sub ShowReporteOficina()
            Dim strParametros As String = ""
            Dim objBLConexion As DMSOneFramework.SCGDataAccess.DAConexion

            objBLConexion = New DMSOneFramework.SCGDataAccess.DAConexion

            PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)
            If txtNoOrden.Text <> "" Then

                strParametros = strParametros & txtNoOrden.Text.Trim & ","

                strParametros = strParametros & txtNoVisita.Text.Trim

                With rptorden
                    .P_BarraTitulo = My.Resources.ResourceUI.rptTituloDocumentoOficina
                    .P_WorkFolder = PATH_REPORTES
                    .P_Filename = My.Resources.ResourceUI.rptNombreDocumentoOficina
                    .P_Server = Server
                    .P_DataBase = strDATABASESCG
                    .P_User = UserSCGInternal
                    .P_Password = Password
                    .P_ParArray = strParametros
                End With

                rptorden.VerReporte()
            Else
                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarOT)
            End If

        End Sub

        Private Sub ShowReporteReprocesos()
            Dim strParametros As String = ""
            Dim objBLConexion As DMSOneFramework.SCGDataAccess.DAConexion

            objBLConexion = New DMSOneFramework.SCGDataAccess.DAConexion

            PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

            strParametros = strParametros & m_strNoOrden.Trim & ","

            strParametros = strParametros & G_strCompaniaSCG & ","

            strParametros = strParametros & gc_strAplicacion & ","

            strParametros = strParametros & G_strUser

            With rptReprocesos
                .P_BarraTitulo = My.Resources.ResourceUI.rptTituloDocumentoReproceso
                .P_WorkFolder = PATH_REPORTES
                .P_Filename = My.Resources.ResourceUI.rptnombreDocumentoReproceso
                .P_Server = Server
                .P_DataBase = strDATABASESCG
                .P_User = UserSCGInternal
                .P_Password = Password
                .P_ParArray = strParametros
            End With

            rptReprocesos.VerReporte()

        End Sub

        Private Sub ShowReporteSuspenciones()
            Dim strParametros As String = ""
            Dim objBLConexion As DMSOneFramework.SCGDataAccess.DAConexion

            objBLConexion = New DMSOneFramework.SCGDataAccess.DAConexion

            PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

            strParametros = strParametros & m_strNoOrden.Trim & ","

            strParametros = strParametros & G_strCompaniaSCG & ","

            strParametros = strParametros & gc_strAplicacion & ","

            strParametros = strParametros & G_strUser

            With rptSuspensiones
                .P_BarraTitulo = My.Resources.ResourceUI.rptTituloDocumentoSuspensiones
                .P_WorkFolder = PATH_REPORTES
                .P_Filename = My.Resources.ResourceUI.rptNombreDocumentoSuspensiones
                .P_Server = Server
                .P_DataBase = strDATABASESCG
                .P_User = UserSCGInternal
                .P_Password = Password
                .P_ParArray = strParametros
            End With

            rptSuspensiones.VerReporte()

        End Sub

        Private Sub ShowReporteCostos()
            Dim strParametros As String = ""
            Dim objBLConexion As DMSOneFramework.SCGDataAccess.DAConexion

            objBLConexion = New DMSOneFramework.SCGDataAccess.DAConexion

            PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

            strParametros = strParametros & m_strNoOrden.Trim ''& ","

            ''strParametros = strParametros & G_strCompaniaSCG & ","

            ''strParametros = strParametros & gc_strAplicacion & ","

            ''strParametros = strParametros & G_strUser

            With rptSuspensiones
                .P_BarraTitulo = My.Resources.ResourceUI.rptTituloCostoPorOrden
                .P_WorkFolder = PATH_REPORTES
                .P_Filename = My.Resources.ResourceUI.rptNombreCostoPorOrden
                .P_Server = Server
                .P_DataBase = strDATABASESCG
                .P_User = UserSCGInternal
                .P_Password = Password
                .P_ParArray = strParametros
            End With

            rptSuspensiones.VerReporte()

        End Sub

        Private Sub ProcesoIniciarFaseXOrden()
            Dim intFase As Integer
            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
            Dim intUpdateResult As Integer

            If Not OrdenIniciada() Then
                IniciarOrden()
            End If

            intFase = Busca_Codigo_Texto(cboFases_Producción.SelectedItem)

            intUpdateResult = objDA.IniciarFase(txtNoOrden.Text, intFase)

            ''Cálculo costo del Día a Día
            If intUpdateResult <> 0 Then
                'CalculoCostosInicioFase(txtNoOrden.Text, intFase)
            End If
            ''Costos

            'If m_drdOrdenCurrent.IsNoFaseActualNull Then
            '    m_drdOrdenCurrent.NoFaseActual = intFase
            '    m_drdOrdenCurrent.FaseDescripcion = Busca_Codigo_Texto(cboFases_Producción.SelectedItem, False)
            '    m_drdOrdenCurrent.AcceptChanges()
            'ElseIf m_drdOrdenCurrent.NoFaseActual < intFase Then
            '    m_drdOrdenCurrent.NoFaseActual = intFase
            '    m_drdOrdenCurrent.FaseDescripcion = Busca_Codigo_Texto(cboFases_Producción.SelectedItem, False)
            '    m_drdOrdenCurrent.AcceptChanges()
            'End If

            CargarDatasetEstadoFaseXOrden(intFase, txtNoOrden.Text)

        End Sub

        Private Function VerificarOrdenXSuspender(ByVal p_intFase As Integer, ByVal p_strNoOrden As String) As Boolean
            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
            Dim blnResult As Boolean

            blnResult = objDA.VerificarOrdenXSuspender(p_strNoOrden, p_intFase)

            Return blnResult

        End Function

        Private Sub ProcesoSuspenderFaseXOrden(ByVal p_intNoFase As Integer)
            Dim intFase As Integer
            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter

            intFase = p_intNoFase 'Busca_Codigo_Texto(cboFases_Producción.SelectedItem)

            If VerificarOrdenXSuspender(intFase, txtNoOrden.Text) Then
                SuspenderOrden()
            End If

            objDA.SuspenderFase(txtNoOrden.Text, intFase)

            CargarDatasetEstadoFaseXOrden(intFase, txtNoOrden.Text)

        End Sub

        Private Sub ProcesoFinalizarFaseXOrden(ByVal intFase As Integer)
            'Dim intFase As Integer
            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
            Dim objDACol As DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
            Dim dstColaborador As New ColaboradorDataset


            If ValidarDatosSAP() Then 'Valida los datos que se necesitan en SAP para evitar error al calcular costos

                'intFase = Busca_Codigo_Texto(cboFases_Producción.SelectedItem)

                If VerificarColaboradoresPendientes(txtNoOrden.Text, intFase) Then

                    If objSCGMSGBox.msgPregunta(My.Resources.ResourceUI.PreguntaColaboradFasenoTerminada & "," & Chr(13) & _
                    My.Resources.ResourceUI.PreguntaDeseaContinuaryFinalizarActividades) = MsgBoxResult.Yes Then

                        'Agregado 29/06/06. Alejandra. Si la fase estaba suspendida, antes de finalizarla
                        'establece la fecha fin de la suspensión
                        'objDA.EstablecerFinSuspension(txtNoOrden.Text, intFase)

                        objDACol = New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
                        'objDACol.FinalizarFase(intFase, txtNoOrden.Text, mc_strProcesoManual)
                        'Finaliza los colaboradores de la fase
                        objDACol.SelColaboradoresAFinalizar(dstColaborador, txtNoOrden.Text, intFase)
                        ModificarDataSet(dstColaborador)
                        objDACol.UpdateFinalizar(dstColaborador.SCGTA_TB_ControlColaborador, mc_strProcesoManual)
                        'Calcula Costos
                        'CalculoCostosDtD(dstColaborador.SCGTA_TB_ControlColaborador, dstColaborador.SCGTA_TB_ControlColaborador)

                        'Finaliza la fase
                        objDA.FinalizarFase(txtNoOrden.Text, intFase)
                        'Actualiza el costo promedio por panel
                        'objDA.ActualizarCostoPromedioPanel(txtNoOrden.Text, intFase)
                        ''''

                        CargarGridColaborador(intFase)
                        CargarDatasetEstadoFaseXOrden(intFase, txtNoOrden.Text)

                    End If
                Else

                    'Agregado 29/06/06. Alejandra. Si la fase estaba suspendida, antes de finalizarla
                    'establece la fecha fin de la suspensión
                    'objDA.EstablecerFinSuspension(txtNoOrden.Text, intFase)

                    objDA.FinalizarFase(txtNoOrden.Text, intFase)
                    'Agregado 08/08/06. Alejandra. Actualiza el costo promedio por panel
                    'objDA.ActualizarCostoPromedioPanel(txtNoOrden.Text, intFase)
                    ''''
                    CargarDatasetEstadoFaseXOrden(intFase, txtNoOrden.Text)

                End If
            End If
        End Sub

        Private Sub CambiarEstadoTabProduccion()

            ''ToolBar
            btnIniciar.Enabled = True
            btnRechazar.Enabled = True
            btnReproceso.Enabled = True
            btnSuspension.Enabled = True
            btnCalidad.Enabled = True
            btnFinalizar.Enabled = True

            ''General
            btnInicioFecha.Enabled = True
            btnSuspende.Enabled = True
            btnFinaliza.Enabled = True
            btnAsignar.Enabled = True

        End Sub

        Private Sub CambiarEstadoOrdenXFase(ByVal intProcedimiento As Integer)
            Select Case intProcedimiento

                Case 1 ''Estado proceso
                    CambEstOrdenXFase_Proceso()
                Case 2 ''Estado suspendida
                    CambEstOrdenXFase_Suspendida()
                Case 3 ''Estado finalizada
                    CambEstOrdenXFase_Finalizada()

            End Select
        End Sub

        Private Sub CambEstOrdenXFase_Proceso()

        End Sub

        Private Sub CambEstOrdenXFase_Suspendida()

        End Sub

        Private Sub CambEstOrdenXFase_Finalizada()

        End Sub

        Private Sub ProdCambiarEstadoFase(ByVal intEstado As Integer)

            If m_drdOrdenCurrent.Estado <> mc_PriEstado_Finalizada Then

                CambiarEstadoTabProduccion()

                Select Case intEstado
                    Case 0 ''No Iniciada

                        picEstado.Image = Nothing
                        btnReproceso.Enabled = False
                        btnSuspension.Enabled = False
                        btnCalidad.Enabled = False
                        btnFinalizar.Enabled = False

                        mf_strEstado = mc_PriEstado_NoIniciada

                    Case 1 ''Proceso
                        picEstado.Image = imglst_ProcProd.Images(0)
                        btnIniciar.Enabled = False
                        btnRechazar.Enabled = False

                        mf_strEstado = mc_PriEstado_Proceso

                    Case 2 ''Suspendida
                        picEstado.Image = imglst_ProcProd.Images(3)
                        btnRechazar.Enabled = False
                        btnSuspension.Enabled = False

                        mf_strEstado = mc_PriEstado_Suspendida

                    Case 3 ''Finalizada
                        picEstado.Image = imglst_ProcProd.Images(9)
                        btnIniciar.Enabled = True

                        btnRechazar.Enabled = False
                        btnReproceso.Enabled = False
                        btnSuspension.Enabled = False
                        btnCalidad.Enabled = False
                        btnFinalizar.Enabled = False

                        btnInicioFecha.Enabled = False
                        btnSuspende.Enabled = False
                        btnFinaliza.Enabled = False
                        btnAsignar.Enabled = False

                        mf_strEstado = mc_PriEstado_Finalizada

                        'Agregado 22/08/06. Alejandra
                    Case 4 ''Rechazada
                        picEstado.Image = imglst_ProcProd.Images(1)
                        btnReproceso.Enabled = False
                        btnSuspension.Enabled = False
                        btnCalidad.Enabled = False
                        btnFinalizar.Enabled = False
                        btnRechazar.Enabled = False

                        btnInicioFecha.Enabled = False
                        btnSuspende.Enabled = False
                        btnFinaliza.Enabled = False
                        btnAsignar.Enabled = False

                End Select

            End If

        End Sub

        Private Function VerificarColaboradoresPendientes(ByVal p_NoOrden As String, _
                                                          ByVal p_NoFase As Integer, _
                                                          Optional ByVal p_intValidarSuspencion As Integer = 0) As Boolean
            Dim objDA As New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
            Dim blnResult As Boolean

            blnResult = objDA.VerificarColPendi(p_NoOrden, p_NoFase, p_intValidarSuspencion)

            Return blnResult

        End Function

        Private Sub EliminarColaborador(ByRef dtbControlColaborador As ColaboradorDataset.SCGTA_TB_ControlColaboradorDataTable)
            Dim intFase As Integer
            Dim drwColaborador As ColaboradorDataset.SCGTA_TB_ControlColaboradorRow
            Dim objDACol As New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter

            'Solo debe poder eliminar los colaboradores con estado  = No inciado, así que revierte la seleccion
            'de colaboradores que no tienen ese estado
            If Not dtbControlColaborador Is Nothing Then
                For Each drwColaborador In dtbControlColaborador.Rows
                    If drwColaborador.Estado <> "No iniciado" Then
                        drwColaborador.RejectChanges()
                    Else
                        If drwColaborador.Check = True Then
                            If drwColaborador.RowState <> DataRowState.Added Then
                                drwColaborador.Delete()

                            Else
                                drwColaborador.Delete()
                                drwColaborador.AcceptChanges()
                            End If
                        Else
                            drwColaborador.RejectChanges()
                        End If
                    End If
                Next


                objDACol.EliminarColaborador(m_dstCol)

                intFase = CInt(Busca_Codigo_Texto(Me.cboFasesProdF.Text, True))
                CargarGridActividades(intFase, IIf(chkAdicionalAct.Checked, 1, 0))

            End If
        End Sub

        Private Function ValidarColaborador() As Boolean
            Dim strCosteoServicios As String
            Dim IDColaborador As String
            Dim idCol As Integer

            Try

                'Validar salario del colaborador

                strCosteoServicios = objUtilitarios2.TraerConfiguracionServicios()

                If strCosteoServicios <> "0" Then

                    IDColaborador = Busca_Codigo_Texto(cbocolaborador.Text)

                    'idCol = CType(cbocolaborador.SelectedValue, Integer)


                    If objUtilitarios2.TraerSalarioColaborador(IDColaborador) Then
                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.ValidarSalario)
                        Return True

                    Else
                        Return False

                    End If

                End If



            Catch ex As Exception
                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Function

        Public Sub Asignar(ByVal sender As Object, ByVal e As System.EventArgs)


            'Agregado 03/07/06. Alejandra. Asigna el colaborador a la fase seleccionada en el context menu 
            Dim intFase As Integer
            'Dim strCosteoServicios As String
            'Dim IDColaborador As String
            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter

            Try

                'Validar salario del colaborador

                'strCosteoServicios = objUtilitarios2.TraerConfiguracionServicios()

                'If strCosteoServicios <> "0" Then
                '    IDColaborador = cbocolaborador.SelectedValue

                '    If objUtilitarios2.TraerSalarioColaborador(IDColaborador) Then
                '        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.ValidarSalario)
                '        Exit Sub

                '    End If

                'End If



                If Not m_alstFases Is Nothing Then

                    intFase = m_alstFases.Item(CType(sender, MenuItem).Index)
                    'intHayRechazo = objDA.EsEstadoFaseRechazo(txtNoOrden.Text, intFase)

                    Me.MdiParent.Cursor = Cursors.WaitCursor

                    If cbocolaborador.Text.Trim <> "" Then
                        If Not cboActividadesAsignables.SelectedValue Is Nothing Then

                            'No se debe permitir asignar un colaborador a una fase que está Finalizada
                            If objUtilitarios.retornaEstadoFase(txtNoOrden.Text, intFase) <> "Finalizada" Then

                                'El tiempo asignado a la fase de produccion debe ser mayor que 0 si se quiere asignar un colaborador
                                If objUtilitarios.DevuelveTiempoAprobado(txtNoOrden.Text, intFase) > 0 Then
                                    AsignarColaborador(0, intFase, cboActividadesAsignables.SelectedValue, m_drdOrdenCurrent.NoCotizacion)
                                Else
                                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeTiempoOtorgado0Horas)
                                End If 'Tiempo aprobado

                            Else
                                objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeFaseFinalizadaNoAsigna)
                            End If 'Fase Finalizada

                        Else
                            objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarActividad)
                        End If

                    Else
                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarcolaborador)
                    End If 'cboColaborador


                    Me.MdiParent.Cursor = Cursors.Arrow

                End If
            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Public Sub CargarMenuFases(ByVal strNoOrden As String)

            'Agregado 03/07/06. Alejandra. Carga el Context menu con las fases de producción 
            Dim drd As SqlClient.SqlDataReader = Nothing


            Try
                drd = objUtilitarios.ReaderFasesProd(strNoOrden)

                m_alstFases = New ArrayList

                While drd.Read

                    mnuFases.MenuItems.Add(drd.Item(0), AddressOf Asignar)
                    m_alstFases.Add(drd.Item(1)) 'Carga el array list con los codigos de las fases

                End While
                drd.Close()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw
            Finally
                'Agregado 01072010
                If drd IsNot Nothing Then
                    If Not drd.IsClosed Then
                        drd.Close()
                    End If
                End If
            End Try
        End Sub

        Private Function FinalizarTodasFasesOrden(ByVal p_blnCancelarOrden As Boolean)
            'Agregado 10/08/06. Alejandra. Finaliza las fases que tienen estado "En Proceso" o "Suspendida"

            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
            Dim strEstadoFase As String
            Dim intIndice As Integer
            Dim blnFinalizar As Boolean

            Try
                blnFinalizar = False

                If p_blnCancelarOrden OrElse VerificaExistenPendientes() Then


                    If HayColaboradoresPendientesEnAlgunaFase() Then

                        If objSCGMSGBox.msgPregunta(My.Resources.ResourceUI.PreguntaColaboradFasenoTerminada & "," & Chr(13) & _
                        My.Resources.ResourceUI.PreguntaDeseaContinuaryFinalizarActividades) = MsgBoxResult.Yes Then

                            For intIndice = 0 To m_alstFasesProduccion.Count - 1

                                strEstadoFase = objUtilitarios.retornaEstadoFase(txtNoOrden.Text, m_alstFasesProduccion(intIndice))

                                If (strEstadoFase = mc_PriEstado_Suspendida Or strEstadoFase = mc_PriEstado_Proceso) Then
                                    FinalizarFase(m_alstFasesProduccion(intIndice))
                                End If

                            Next intIndice
                            blnFinalizar = True

                        End If

                    Else 'No hay colaboradores pendientes

                        For intIndice = 0 To m_alstFasesProduccion.Count - 1

                            strEstadoFase = objUtilitarios.retornaEstadoFase(txtNoOrden.Text, m_alstFasesProduccion(intIndice))

                            If (strEstadoFase = mc_PriEstado_Suspendida Or strEstadoFase = mc_PriEstado_Proceso) Then
                                FinalizarFase(m_alstFasesProduccion(intIndice))
                            End If

                        Next intIndice

                        blnFinalizar = True

                    End If

                    'Else

                    '    objSCGMSGBox.msgExclamationCustom("No es posible finalizar una Orden de Trabajo con Items Pendientes" & Chr(13) & _
                    '                                        ", sin colaboradores asignados o con solicitudes pendientes")

                End If

                Return blnFinalizar

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

                Throw ex

            End Try
        End Function



        Private Function SuspenderTodasFasesOrden()
            'Agregado 10/08/06. Alejandra. Finaliza las fases que tienen estado "En Proceso" o "Suspendida"

            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
            Dim strEstadoFase As String
            Dim intIndice As Integer
            Dim blnSuspender As Boolean

            Try
                blnSuspender = False

                If HayColaboradoresPendientesEnAlgunaFase(1) Then

                    If objSCGMSGBox.msgPregunta(My.Resources.ResourceUI.PreguntaColaboradFasenoTerminada & "," & Chr(13) & _
                    My.Resources.ResourceUI.PreguntaDeseaContinuarySuspenderActividades) = MsgBoxResult.Yes Then

                        For intIndice = 0 To m_alstFasesProduccion.Count - 1

                            strEstadoFase = objUtilitarios.retornaEstadoFase(txtNoOrden.Text, m_alstFasesProduccion(intIndice))

                            If (strEstadoFase = mc_PriEstado_Proceso) Then
                                SuspenderFase(m_alstFasesProduccion(intIndice), 0)
                            End If

                        Next intIndice
                        blnSuspender = True

                    End If

                Else 'No hay colaboradores pendientes

                    For intIndice = 0 To m_alstFasesProduccion.Count - 1

                        strEstadoFase = objUtilitarios.retornaEstadoFase(txtNoOrden.Text, m_alstFasesProduccion(intIndice))

                        If (strEstadoFase = mc_PriEstado_Proceso) Then
                            'FinalizarFase(m_alstFasesProduccion(intIndice))
                            SuspenderFase(m_alstFasesProduccion(intIndice), 0)
                        End If

                    Next intIndice
                    blnSuspender = True
                End If

                Return blnSuspender
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

                Throw ex
            Finally

            End Try
        End Function

        Private Function HayColaboradoresPendientesEnAlgunaFase(Optional ByVal p_intValidarSuspencion As Integer = 0) As Boolean
            'Agregado 10/08/06. Alejandra. Determina si hay colaboradores iniciados en alguna de todas las fases
            Dim blnColaboradores As Boolean
            Dim drdFases As SqlClient.SqlDataReader = Nothing

            Try
                m_alstFasesProduccion = New ArrayList
                blnColaboradores = False

                drdFases = objUtilitarios.ReaderFasesProd(txtNoOrden.Text)

                While drdFases.Read
                    m_alstFasesProduccion.Add(drdFases.Item(1))
                    If VerificarColaboradoresPendientes(txtNoOrden.Text, drdFases.Item(1), p_intValidarSuspencion) Then
                        blnColaboradores = True
                    End If
                End While

                Return blnColaboradores

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

                Throw ex
            Finally
                drdFases.Close()
            End Try
        End Function

        Private Sub FinalizarFase(ByVal intFase As Integer)
            'Agregado 10/08/06. Alejandra
            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
            Dim objDACol As DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
            Dim dstColaborador As New ColaboradorDataset

            If VerificarColaboradoresPendientes(txtNoOrden.Text, intFase) Then


                'Si la fase estaba suspendida, antes de finalizarla establece la fecha fin de la suspensión
                'objDA.EstablecerFinSuspension(txtNoOrden.Text, intFase)

                objDACol = New DMSOneFramework.SCGDataAccess.ColaboradorDataAdapter
                'objDACol.FinalizarFase(intFase, txtNoOrden.Text, mc_strProcesoManual)
                'Finaliza los colaboradores de la fase
                objDACol.SelColaboradoresAFinalizar(dstColaborador, txtNoOrden.Text, intFase)
                ModificarDataSet(dstColaborador)
                objDACol.UpdateFinalizar(dstColaborador.SCGTA_TB_ControlColaborador, mc_strProcesoManual)
                'Calcula Costos
                'CalculoCostosDtD(dstColaborador.SCGTA_TB_ControlColaborador, dstColaborador.SCGTA_TB_ControlColaborador)


                objDA.FinalizarFase(txtNoOrden.Text, intFase)
                'Actualiza el costo promedio por panel
                'objDA.ActualizarCostoPromedioPanel(txtNoOrden.Text, intFase)



            Else 'No hay colaboradores Pendientes

                'Si la fase estaba suspendida, antes de finalizarla establece la fecha fin de la suspensión
                'objDA.EstablecerFinSuspension(txtNoOrden.Text, intFase)

                objDA.FinalizarFase(txtNoOrden.Text, intFase)
                'Actualiza el costo promedio por panel
                'objDA.ActualizarCostoPromedioPanel(txtNoOrden.Text, intFase)



            End If
        End Sub

        Private Function ValidarDatosSAP()

            'Valida que el tipo de cambio y el periodo fiscal sean validos antes de realizar calculo de costos

            Dim blnValido As Boolean = True
            Dim objBLSBO As BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal
            Dim strMonedaLocal As String
            Dim strMonedaSistema As String

            Try

                objBLSBO = New BLSBO.GlobalFunctionsSBO
                strMonedaSistema = objBLSBO.RetornarMonedaSistema
                strMonedaLocal = objBLSBO.RetornarMonedaLocal
                If strMonedaSistema <> strMonedaLocal Then
                    decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(strMonedaSistema, Today, strConectionString, true)
                Else
                    decTipoCambio = 1
                End If
                If decTipoCambio <> -1 Then

                    If SCGDataAccess.Utilitarios.GetPostingPeriod(Today).Trim <> "" Then

                    Else

                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOPeriodoFiscalInvalido)
                        blnValido = False


                    End If
                Else

                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOActualizarTipoCambio)
                    blnValido = False
                End If

                Return blnValido

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try
        End Function

        Private Sub ModificarDataSet(ByRef p_dstColab As ColaboradorDataset)
            'Establece el campo Check en True para que los rows cambien al estado Modified y puedan
            'ser detectados por el Update
            Dim drw As ColaboradorDataset.SCGTA_TB_ControlColaboradorRow

            Try

                For Each drw In p_dstColab.SCGTA_TB_ControlColaborador.Rows
                    drw.Check = True
                Next

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try

        End Sub

        Private Sub CargarActividadesCombo(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer)
            Dim adpACtividadesXOrden As New SCGDataAccess.ActividadesXFaseDataAdapter
            Dim drdAxO As SqlClient.SqlDataReader

            drdAxO = adpACtividadesXOrden.GetActividadesByNoFaseToReader(p_strNoOrden, p_intNoFase)

            Utilitarios.CargarComboSourceByReader(cboActividadesAsignables, drdAxO)

            drdAxO.Close()

        End Sub

#End Region

#Region "Eventos"

        Private Sub tbr_SCG_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tbr_SCG.ButtonClick
            Dim objBLSBO As BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal
            Dim strMonedaLocal As String
            Dim strMonedaSistema As String
            Dim caso As String
            Dim blnEstadoASuspender As Boolean = False


            'Agregado 29/06/06. Alejandra
            Dim objDA As New DMSOneFramework.SCGDataAccess.FaseXOrdenEstadosDataAdapter
            Dim adpTiempos As New DMSOneFramework.SCGDataAccess.TiemposMuertosDataAdapter
            '''''''

            Try
                caso = e.Button.Text

                Me.MdiParent.Cursor = Cursors.WaitCursor

                Select Case caso

                    'llama al formulario de reprocesos
                    Case Is = btnReproceso.Text



                        'verifica si el combo de fases tiene texto seleccionado
                        If cboFases_Producción.Text.Trim <> "" Then

                            ObjfrmReprocesos = ValidarFormularios("frmReprocesos")

                            If IsNothing(ObjfrmReprocesos) Then
                                ObjfrmReprocesos = New frmReprocesos(m_strNoOrden, _
                                                         Busca_Codigo_Texto(cboFases_Producción.Text), _
                                                         Busca_Codigo_Texto(cboFases_Producción.Text, False))
                                ObjfrmReprocesos.MdiParent = Me.MdiParent
                            End If

                            'carga el formulario
                            ObjfrmReprocesos.Show()
                        Else
                            Call objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarFase)
                        End If



                    Case Is = btnSuspension.Text

                        objBLSBO = New BLSBO.GlobalFunctionsSBO
                        strMonedaSistema = objBLSBO.RetornarMonedaSistema
                        strMonedaLocal = objBLSBO.RetornarMonedaLocal
                        If strMonedaSistema <> strMonedaLocal Then
                            decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(strMonedaSistema, Today, strConectionString, true)
                        Else
                            decTipoCambio = 1
                        End If
                        If decTipoCambio <> -1 Then

                            If SCGDataAccess.Utilitarios.GetPostingPeriod(Today).Trim <> "" Then


                                If cboFases_Producción.Text.Trim <> "" Then
                                    frmSuspensiones = ValidarFormularios("frmSuspensiones")

                                    If IsNothing(frmSuspensiones) Then
                                        If mf_strEstado = mc_PriEstado_Proceso Then
                                            blnEstadoASuspender = True
                                        End If
                                        frmSuspensiones = New frmSuspensiones(m_strNoOrden, _
                                                                Busca_Codigo_Texto(cboFases_Producción.Text), _
                                                                Busca_Codigo_Texto(cboFases_Producción.Text, False), blnEstadoASuspender)
                                        frmSuspensiones.MdiParent = Me.MdiParent
                                    End If

                                    frmSuspensiones.Show()

                                Else
                                    Call objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarFase)
                                End If

                            Else

                                objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOPeriodoFiscalInvalido)

                            End If
                        Else

                            objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOActualizarTipoCambio)

                        End If

                    Case Is = btnCalidad.Text
                        ' Dim form As frmCtrlCalidad

                        If cboFases_Producción.Text.Trim <> "" Then

                            'form = ValidarFormularios("frmBoletaCalidad")
                            ' If IsNothing(form) Then
                            ' form = New frmCtrlCalidad(Busca_Codigo_Texto(cboFases_Producción.Text), Busca_Codigo_Texto(cboFases_Producción.Text, False), txtNoOrden.Text)
                            'form.MdiParent = Me.MdiParent
                            'End If
                            'form.Show()

                        Else
                            objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarFase)
                        End If

                        'Case Is = btnAdicionales.Text
                        '    Dim form As frmAdicionales
                        '    form = ValidarFormularios("frmAdicionales")
                        '    If IsNothing(form) Then
                        '        form = New frmAdicionales
                        '        form.MdiParent = Me.MdiParent
                        '    End If
                        '    form.Show()

                    Case Is = btnIniciar.Text

                        objBLSBO = New BLSBO.GlobalFunctionsSBO

                        strMonedaSistema = objBLSBO.RetornarMonedaSistema
                        strMonedaLocal = objBLSBO.RetornarMonedaLocal
                        If strMonedaSistema <> strMonedaLocal Then
                            decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(strMonedaSistema, Today, strConectionString, True)
                        Else
                            decTipoCambio = 1
                        End If

                        If decTipoCambio <> -1 Then

                            If SCGDataAccess.Utilitarios.GetPostingPeriod(Today).Trim <> "" Then

                                If cboFases_Producción.Text.Trim <> "" Then
                                    'El tiempo asignado a la fase de produccion debe ser mayor que 0 si se quiere iniciar la fase
                                    If txtFSalida.Text > 0 Then

                                        'Agregado 29/06/06. Alejandra. Si la fase estaba suspendida, antes de iniciarla
                                        'establece la fecha en que finalizó la suspensión
                                        'objDA.EstablecerFinSuspension(txtNoOrden.Text, CInt(Busca_Codigo_Texto(cboFases_Producción.SelectedItem)))

                                        'Agregado 17/08/06. Alejandra. Calcula tiempos muertos
                                        If (objUtilitarios.HayFasesIniciadas(txtNoOrden.Text) = 0) Then
                                            ProcesoIniciarFaseXOrden()
                                            'adpTiempos.UPDTiemposMuertosIniciarOrden(txtNoOrden.Text, Busca_Codigo_Texto(cboFases_Producción.SelectedItem))
                                        Else
                                            'adpTiempos.UPDTiemposMuertosIniciarFase(txtNoOrden.Text, Busca_Codigo_Texto(cboFases_Producción.SelectedItem))
                                            ProcesoIniciarFaseXOrden()
                                        End If

                                        ''''''''
                                        'ProcesoIniciarFaseXOrden()
                                    Else
                                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeTiempoOtorgado0Horas)
                                    End If

                                Else
                                    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarFase)
                                End If

                            Else

                                objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOPeriodoFiscalInvalido)

                            End If
                        Else

                            objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOActualizarTipoCambio)

                        End If

                    Case Is = btnFinalizar.Text

                        objBLSBO = New BLSBO.GlobalFunctionsSBO

                        strMonedaSistema = objBLSBO.RetornarMonedaSistema
                        strMonedaLocal = objBLSBO.RetornarMonedaLocal
                        If strMonedaSistema <> strMonedaLocal Then
                            decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(strMonedaSistema, Today, strConectionString, True)
                        Else
                            decTipoCambio = 1
                        End If

                        If decTipoCambio <> -1 Then

                            If SCGDataAccess.Utilitarios.GetPostingPeriod(Today).Trim <> "" Then

                                If cboFases_Producción.Text.Trim <> "" Then

                                    ProcesoFinalizarFaseXOrden(Busca_Codigo_Texto(cboFases_Producción.SelectedItem))

                                Else
                                    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarFase)
                                End If

                            Else

                                objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOPeriodoFiscalInvalido)

                            End If
                        Else

                            objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOActualizarTipoCambio)

                        End If

                    Case Is = btnDocumentos.Text



                        Select Case btnDocumentos.Text 'btnDocumentos.Tag

                            Case dropmnuProduccion.Text 'dropmnuProduccion.Index

                                ShowReporteProduccion()

                            Case dropmnuOficina.Text 'dropmnuOficina.Index

                                ShowReporteOficina()

                            Case dropmnuReprocesos.Text 'dropmnuReprocesos.Index

                                ShowReporteReprocesos()

                            Case dropmnuSuspenciones.Text 'dropmnuSuspenciones.Index

                                ShowReporteSuspenciones()

                            Case dropmnuCostos.Text 'dropmnuCostos.Index

                                ShowReporteCostos()

                            Case dropmnuItemsNoAprobados.Text
                                ShowReporteItemsNoAprobados()

                            Case dropmnuBalanceOT.Text 'dropmnuBalanceOT.Index
                                ShowReporteBalanceOT()

                        End Select

                        'Agregado 17/08/06. Alejandra.
                    Case Is = btnRechazar.Text

                        If cboFases_Producción.Text.Trim <> "" Then

                            adpTiempos.UPDTiemposMuertosRechazarFase(txtNoOrden.Text)
                            objDA.RechazarFase(txtNoOrden.Text, Busca_Codigo_Texto(cboFases_Producción.SelectedItem))
                            CargarDatasetEstadoFaseXOrden(Busca_Codigo_Texto(cboFases_Producción.SelectedItem), txtNoOrden.Text)

                        Else
                            objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarFase)
                        End If
                        '''''''''

                    Case Is = btnAsignacionMultiple.Text
                        'If cboFases_Producción.SelectedItem Is Nothing Then
                        '    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarFase)
                        '    Return
                        'End If

                        Dim Forma_Nueva As Form
                        Dim blnExisteForm As Boolean

                        For Each Forma_Nueva In Me.MdiParent.MdiChildren
                            If Forma_Nueva.Name = "frmAsignarActividades" Then
                                blnExisteForm = True
                            End If
                        Next

                        If Not blnExisteForm Then

                            If m_objFrmAsignarActividades IsNot Nothing Then
                                m_objFrmAsignarActividades.Dispose()
                                m_objFrmAsignarActividades = Nothing
                            End If

                            m_objFrmAsignarActividades = New frmAsignarActividades(m_strNoOrdenAct, m_drdOrdenCurrent.NoCotizacion, m_drdOrdenCurrent.Estado)
                            m_objFrmAsignarActividades.MdiParent = Me.MdiParent
                            m_objFrmAsignarActividades.Show()
                        End If

                    Case Is = btnAsignarTiempos.Text
                        Dim Forma_Nueva As Form
                        Dim blnExisteForm As Boolean

                        For Each Forma_Nueva In Me.MdiParent.MdiChildren
                            If Forma_Nueva.Name = "frmAsignarActividades" Then
                                blnExisteForm = True
                            End If
                        Next

                        If Not blnExisteForm Then

                            If m_objFrmAsignacionTiempos IsNot Nothing Then
                                m_objFrmAsignacionTiempos.Dispose()
                                m_objFrmAsignacionTiempos = Nothing
                            End If

                            m_objFrmAsignacionTiempos = New frmAsignacionTiempos(m_strNoOrdenAct, m_drdOrdenCurrent.NoCotizacion, m_drdOrdenCurrent.Estado)
                            m_objFrmAsignacionTiempos.MdiParent = Me.MdiParent
                            m_objFrmAsignacionTiempos.Show()
                        End If


                End Select

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub cboFases_Producción_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFases_Producción.SelectedIndexChanged
            Dim intFase As Integer

            Try
                Me.MdiParent.Cursor = Cursors.WaitCursor

                intFase = Busca_Codigo_Texto(cboFases_Producción.SelectedItem, True)

                objUtilitarios.CargarCombos(cbocolaborador, 16, intFase, G_strIDSucursal)

                If cbocolaborador.Items.Count <> 0 Then
                    cbocolaborador.SelectedIndex = 0
                End If

                If g_intUnidadTiempo = -1 Then
                    txtFSalida.Text = objUtilitarios.DevuelveTiempoAprobado(txtNoOrden.Text, intFase)
                Else
                    If m_dblValorUnidadTiempo > 0 Then
                        txtFSalida.Text = objUtilitarios.DevuelveTiempoAprobado(txtNoOrden.Text, intFase) / m_dblValorUnidadTiempo
                    Else
                        txtFSalida.Text = 0
                    End If
                End If


                CargarGridColaborador(intFase)
                CargarDatasetEstadoFaseXOrden(intFase, txtNoOrden.Text)
                CargarActividadesCombo(txtNoOrden.Text, intFase)

                'If cboFases_Producción.SelectedItem <> "" Then

                '    'verifica que existan reprocesos por la fase seleccionad
                '    If ConsultarCantidadReprocesosPorFase(intFase, txtNoOrden.Text) > 0 Then

                '        'CHEQUEA EL REPROCESO
                '        chkReproceso.Enabled = True
                '        chkReproceso.Checked = True

                '    Else

                '        'Importante las 2.
                '        'deshabilita  y desmarca el checkBox de proceso
                '        chkReproceso.Checked = False
                '        chkReproceso.Enabled = False


                '    End If
                'End If


                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Function ConsultarCantidadReprocesosPorFase(ByVal intFase As Integer, ByVal strNoOrden As String) As Integer

            Dim adp_reprocesosXOrden As SCGDataAccess.ReprocesosxOrdenDataAdapter

            Try
                'Crea  el objeto DataAdapter para reprocesos por orden
                adp_reprocesosXOrden = New SCGDataAccess.ReprocesosxOrdenDataAdapter

                'Utiliza el objeto dataadapter para consultar la cantidad de reprocesos por fase
                'Y Retorna esta cantidad.
                Return adp_reprocesosXOrden.ConsultarCantidadReprocesosPorFase(intFase, strNoOrden)

            Catch ex As Exception
                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally

                'DEstruye el objeto dataAdapter
                adp_reprocesosXOrden = Nothing

            End Try



        End Function

        Private Sub ObjfrmReprocesos_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles ObjfrmReprocesos.Closed

            Dim intFase As Integer

            Try

                'verifica que exista un item seleccionado
                If cboFases_Producción.SelectedItem <> "" Then

                    'extrae el codigo de la fase seleccionada
                    intFase = Busca_Codigo_Texto(cboFases_Producción.SelectedItem, True)

                    'verifica que existan reprocesos por la fase seleccionad
                    If ConsultarCantidadReprocesosPorFase(intFase, txtNoOrden.Text) > 0 Then

                        'CHEQUEA EL REPROCESO
                        chkReproceso.Enabled = True
                        chkReproceso.Checked = True

                    Else

                        'Importante las 2.
                        'deshabilita  y desmarca el checkBox de proceso
                        chkReproceso.Checked = False
                        chkReproceso.Enabled = False


                    End If
                End If




            Catch ex As Exception
                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally



            End Try
        End Sub

        Private Sub chkRefSuperiores_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRefSuperiores.CheckedChanged
            Dim intFase As Integer

            Try
                If Not IsNothing(Me.MdiParent) Then

                    Me.MdiParent.Cursor = Cursors.WaitCursor

                    intFase = Busca_Codigo_Texto(cboFases_Producción.SelectedItem, True)
                    CargarGridColaborador(intFase)

                    Me.MdiParent.Cursor = Cursors.Arrow

                End If

            Catch ex As Exception

                If Not IsNothing(Me.MdiParent) Then
                    Me.MdiParent.Cursor = Cursors.Arrow
                End If

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnAsignar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsignar.Click
            Try

                Dim objSBOCommons As New GlobalFunctionsSBO()
                Dim drwActividad As ActividadesXFaseDataset.SCGTA_TB_ActividadesxOrdenRow
                Me.MdiParent.Cursor = Cursors.WaitCursor

                If cbocolaborador.Text.Trim <> "" Then
                    If Not cboActividadesAsignables.SelectedValue Is Nothing Then
                        'El tiempo asignado a la fase de produccion debe ser mayor que 0 si se quiere asignar un colaborador
                        If txtFSalida.Text > 0 Then

                            AsignarColaborador(0, CInt(Busca_Codigo_Texto(cboFases_Producción.Text)), CType(cboActividadesAsignables.SelectedValue, Integer), m_drdOrdenCurrent.NoCotizacion)
                            drwActividad = m_dstAct.SCGTA_TB_ActividadesxOrden.FindByID(CType(cboActividadesAsignables.SelectedValue, Integer))
                            If drwActividad IsNot Nothing Then
                                'objSBOCommons.AgregarEmpleadoRealiza(m_drdOrdenCurrent.NoCotizacion, Busca_Codigo_Texto(cbocolaborador.Text, True), drwActividad.LineNum, Busca_Codigo_Texto(cbocolaborador.Text, False))
                                'Utilitarios.AsignarEmpleado(m_drdOrdenCurrent.NoCotizacion, Busca_Codigo_Texto(cbocolaborador.Text, True), drwActividad.LineNum, Busca_Codigo_Texto(cbocolaborador.Text, False))
                            End If
                        Else
                            objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeTiempoOtorgado0Horas)
                        End If
                    Else
                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarActividad)
                    End If
                Else
                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarcolaborador)
                End If


                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub btnInicioFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInicioFecha.Click

            Try

                Me.MdiParent.Cursor = Cursors.WaitCursor

                IniciarProceso()
                CargarGridActividades(0, IIf(chkAdicionalAct.Checked, 1, 0))

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnFinaliza_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFinaliza.Click
            Try
                Me.MdiParent.Cursor = Cursors.WaitCursor

                FinalizarProceso()
                CargarGridActividades(0, IIf(chkAdicionalAct.Checked, 1, 0))
                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnSuspende_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuspende.Click

            Try

                Me.MdiParent.Cursor = Cursors.WaitCursor

                Dim m_intnofase As Integer = Busca_Codigo_Texto(cboFases_Producción.Text)
                Dim drdColaboradorDV As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorRow
                Dim m_strDescripcionFase As String = Busca_Codigo_Texto(cboFases_Producción.Text, False)
                Dim dtFecha As Date

                Dim blnNoSuspender As Boolean = False


                If m_dstCol.SCGTA_TB_ControlColaborador.Select("Check=1").Length <> 0 Then


                    dtFecha = Nothing
                    For Each drdColaboradorDV In CType(dtgcolaborador.DataSource, DataView).Table.Rows
                        If Not drdColaboradorDV.Check Then
                            drdColaboradorDV.RejectChanges()
                        Else
                            If drdColaboradorDV.Estado <> mc_Estado_NoIniciado And drdColaboradorDV.Estado <> mc_Estado_Finalizado Then
                                If dtFecha = Nothing Then
                                    dtFecha = drdColaboradorDV.FechaInicio
                                Else
                                    If dtFecha < drdColaboradorDV.FechaInicio Then
                                        dtFecha = drdColaboradorDV.FechaInicio
                                    End If
                                End If

                            Else
                                drdColaboradorDV.RejectChanges()
                                blnNoSuspender = True
                            End If

                        End If
                    Next

                    If blnNoSuspender = False Then
                        frmChild = New frmCtrlSuspension(m_strNoOrden, m_intnofase, m_strDescripcionFase, dtFecha, 2)
                        frmChild.Owner = Me
                        Call frmChild.ShowDialog()

                        RaiseEvent NuevaSuspension(frmChild.Ok, Me)
                        CargarGridActividades(0, IIf(chkAdicionalAct.Checked, 1, 0))
                        'SuspenderFase(Busca_Codigo_Texto(cboFases_Producción.Text), 1)

                    Else
                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeSuspender)
                    End If



                End If
                CargarGridActividades(0, IIf(chkAdicionalAct.Checked, 1, 0))
                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub btnImprimirListaCalidad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimirListaCalidad.Click
            Dim strParametros As String = ""
            Dim objBLConexion As DMSOneFramework.SCGDataAccess.DAConexion

            Try

                Me.MdiParent.Cursor = Cursors.WaitCursor

                objBLConexion = New DMSOneFramework.SCGDataAccess.DAConexion

                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)
                If txtNoOrden.Text <> "" And cboFases_Producción.SelectedItem <> "" Then
                    strParametros = strParametros & txtNoOrden.Text.Trim & ","
                    Dim fase As Integer

                    fase = Busca_Codigo_Texto(cboFases_Producción.SelectedItem, True)

                    strParametros = strParametros & fase.ToString
                    With rptCalidad
                        .P_BarraTitulo = My.Resources.ResourceUI.rptTituloDocumentoCalidadSinAplicar
                        .P_WorkFolder = PATH_REPORTES
                        .P_Filename = My.Resources.ResourceUI.rptnombreDocumentoCalidadSinAplicar
                        .P_Server = Server
                        .P_DataBase = strDATABASESCG
                        .P_User = UserSCGInternal
                        .P_Password = Password
                        .P_ParArray = strParametros
                    End With

                    rptCalidad.VerReporte()
                Else
                    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarFase)
                End If

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception
                Me.MdiParent.Cursor = Cursors.Arrow
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dropmnuS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dropmnuProduccion.Click, dropmnuOficina.Click, dropmnuReprocesos.Click, dropmnuSuspenciones.Click, dropmnuCostos.Click, dropmnuBalanceOT.Click
            Try

                Me.MdiParent.Cursor = Cursors.WaitCursor

                ActivarTBProduccionDocs(CType(sender, MenuItem).Index)

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub



        Private Sub frmSuspensiones_NuevaSuspension(ByVal ok As Boolean, ByVal NoSuspension As Integer, ByVal sender As Object) Handles frmSuspensiones.NuevaSuspension
            Try
                If ok Then
                    If ValidarDatosSAP() Then 'La siguiente llamada calcula algunos costos, por eso se validan antes los datos de SAP que podrian ocasionar un error
                        Call SuspenderFase(Busca_Codigo_Texto(cboFases_Producción.Text), NoSuspension)
                    End If
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub frmChild_RetornaCodigo(ByVal intCodigo As Integer, ByVal dtFecha As Date) Handles frmChild.RetornaCodigo
            'Global codigo asignar = intcodigo
            G_intNoRazon = intCodigo

            SuspenderProceso(dtFecha)

        End Sub

        Private Sub btnEliminarColaborador_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarColaborador.Click
            Try
                EliminarColaborador(m_dstCol.SCGTA_TB_ControlColaborador)
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnMenuFases_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMenuFases.Click
            Try
                mnuFases.Show(btnMenuFases, btnMenuFases.PointToClient(MousePosition))

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub dtgcolaborador_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgcolaborador.DoubleClick

            Dim intIdAsignacion As Integer
            Dim drwActividadAsignada As ColaboradorDataset.SCGTA_TB_ControlColaboradorRow
            Dim intFase As Integer
            Dim blnActividadSuspendida As Boolean
            Dim blnActividadYaIniciada As Boolean

            If dtgcolaborador.CurrentRowIndex <> -1 Then

                intIdAsignacion = dtgcolaborador.Item(dtgcolaborador.CurrentCell.RowNumber, 3)
                'Agregado 06/07/06. Alejandra. Para evitar problemas si el nombreRepuesto está en Null
                drwActividadAsignada = m_dstCol.SCGTA_TB_ControlColaborador.FindByID(intIdAsignacion)
                If drwActividadAsignada IsNot Nothing Then
                    If drwActividadAsignada.Estado = "No iniciado" Or drwActividadAsignada.Estado = "Suspendido" Or drwActividadAsignada.Estado = "Iniciado" Then
                        Dim Forma_Nueva As Form
                        Dim blnExisteForm As Boolean

                        For Each Forma_Nueva In Me.MdiParent.MdiChildren
                            If Forma_Nueva.Name = "frmTrabajoActividad" Then
                                blnExisteForm = True
                            End If
                        Next

                        If Not blnExisteForm Then

                            If m_objFrmAsignarHoras IsNot Nothing Then
                                m_objFrmAsignarHoras.Dispose()
                                m_objFrmAsignarHoras = Nothing
                            End If

                            intFase = Busca_Codigo_Texto(cboFases_Producción.SelectedItem)
                            If drwActividadAsignada.Estado <> "Iniciado" Then
                                blnActividadYaIniciada = False
                            Else
                                blnActividadYaIniciada = True
                            End If
                            If drwActividadAsignada.Estado <> "Suspendido" Then
                                blnActividadSuspendida = False
                            Else
                                blnActividadSuspendida = True
                            End If
                            m_objFrmAsignarHoras = New frmTrabajoActividad(drwActividadAsignada.EmpID, drwActividadAsignada.EmpNombre, drwActividadAsignada.IDActividad, drwActividadAsignada.ActividadDesc, drwActividadAsignada.ID, m_drdOrdenCurrent.NoOrden, m_drdOrdenCurrent.Estado, intFase, m_dstCol, blnActividadYaIniciada, blnActividadSuspendida)
                            m_objFrmAsignarHoras.MdiParent = Me.MdiParent
                            m_objFrmAsignarHoras.Show()

                        End If
                    End If
                End If
            End If

        End Sub

        Private Sub dtgcolaborador_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgcolaborador.GotFocus
            Try

                G_CancelarEditColumnDataGrid(Me, dtgcolaborador)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub txtFSalida_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFSalida.LostFocus

            Try

                If cboFases_Producción.SelectedIndex > -1 Then
                    If cboActividadesAsignables.Items.Count > 0 Then
                        Dim adpFases As New FasesXOrdenDataAdapter
                        Dim dtsFases As New FasesXOrdenDataset
                        Dim drwFase As FasesXOrdenDataset.SCGTA_TB_FasesxOrdenRow
                        drwFase = dtsFases.SCGTA_TB_FasesxOrden.NewSCGTA_TB_FasesxOrdenRow
                        drwFase.NoOrden = m_drdOrdenCurrent.NoOrden
                        drwFase.NoFase = Busca_Codigo_Texto(cboFases_Producción.SelectedItem, True)

                        If g_intUnidadTiempo <> -1 And Trim(txtFSalida.Text) <> String.Empty Then
                            drwFase.DuracionHorasAprobadas = txtFSalida.Text() * m_dblValorUnidadTiempo
                        Else
                            drwFase.DuracionHorasAprobadas = txtFSalida.Text()
                        End If

                        dtsFases.SCGTA_TB_FasesxOrden.AddSCGTA_TB_FasesxOrdenRow(drwFase)
                        drwFase.AcceptChanges()
                        drwFase.NoOrden = m_drdOrdenCurrent.NoOrden
                        adpFases.Update(dtsFases)
                    End If
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try


        End Sub

        Private Sub m_objFrmAsignarHoras_AsignacionRealizada(ByVal p_blnOrdenIniciada As Boolean) Handles m_objFrmAsignarHoras.e_AsignacionRealizada

            Dim intFase As Integer

            Try
                If Not IsNothing(Me.MdiParent) Then

                    Me.MdiParent.Cursor = Cursors.WaitCursor

                    intFase = Busca_Codigo_Texto(cboFases_Producción.SelectedItem, True)
                    CargarGridColaborador(intFase)

                    If p_blnOrdenIniciada Then
                        cboEstadoOrden.SelectedText = "Proceso"
                    End If
                    Me.MdiParent.Cursor = Cursors.Arrow

                End If

            Catch ex As Exception

                If Not IsNothing(Me.MdiParent) Then
                    Me.MdiParent.Cursor = Cursors.Arrow
                End If

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub m_objFrmAsignarTiempos_AsignacionRealizada(ByVal p_blnOrdenIniciada As Boolean) Handles m_objFrmAsignacionTiempos.e_AsignacionRealizada

            Dim intFase As Integer

            Try
                If Not IsNothing(Me.MdiParent) Then

                    Me.MdiParent.Cursor = Cursors.WaitCursor

                    intFase = Busca_Codigo_Texto(cboFases_Producción.SelectedItem, True)
                    CargarGridColaborador(intFase)

                    If p_blnOrdenIniciada Then
                        cboEstadoOrden.SelectedText = "Proceso"
                    End If
                    Me.MdiParent.Cursor = Cursors.Arrow

                End If

            Catch ex As Exception

                If Not IsNothing(Me.MdiParent) Then
                    Me.MdiParent.Cursor = Cursors.Arrow
                End If

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub m_objFrmAsignarActividades_e_AsignacionRealizada(ByVal p_blnOrdenIniciada As Boolean) Handles m_objFrmAsignarActividades.e_AsignacionRealizada

            'Dim intFase As Integer

            Try
                If Not IsNothing(Me.MdiParent) Then

                    ' Me.MdiParent.Cursor = Cursors.WaitCursor

                    m_objFrmAsignarActividades.Close()

                    'intFase = Busca_Codigo_Texto(cboFases_Producción.SelectedItem, True)
                    CargarGridColaborador(0)

                    CargarGridActividades(0, IIf(chkAdicionalAct.Checked, 1, 0))

                    If p_blnOrdenIniciada Then
                        cboEstadoOrden.SelectedValue = "Proceso"
                    End If

                    '    Me.MdiParent.Cursor = Cursors.Arrow

                End If

            Catch ex As Exception

                If Not IsNothing(Me.MdiParent) Then
                    Me.MdiParent.Cursor = Cursors.Arrow
                End If

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub CargarUnidadesTiempoDataset()

            CargarUnidadesTiempoGlobales()

            Dim intIndice As Integer
            For intIndice = 0 To m_dstCol.SCGTA_TB_ControlColaborador.Rows.Count - 1
                If m_dblValorUnidadTiempo > 0 Then
                    If Not m_dstCol.SCGTA_TB_ControlColaborador.Rows(intIndice)("TiempoHoras") Is System.DBNull.Value Then
                        m_dstCol.SCGTA_TB_ControlColaborador.Rows(intIndice)("TotalUnidadTiempo") = Math.Round(m_dstCol.SCGTA_TB_ControlColaborador.Rows(intIndice)("TiempoHoras") / m_dblValorUnidadTiempo, 4)
                    Else
                        m_dstCol.SCGTA_TB_ControlColaborador.Rows(intIndice)("TotalUnidadTiempo") = System.DBNull.Value
                    End If

                Else
                    m_dstCol.SCGTA_TB_ControlColaborador.Rows(intIndice)("TotalUnidadTiempo") = 0
                End If

            Next
        End Sub

#End Region

        Private Sub dropmnuItemsNoAprobados_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dropmnuItemsNoAprobados.Click
            Try

                Me.MdiParent.Cursor = Cursors.WaitCursor

                ActivarTBProduccionDocs(CType(sender, MenuItem).Index)

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub


    End Class

End Namespace
