Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports System.Data.SqlClient

Namespace SCG_User_Interface

    Partial Class frmOrden
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

#Region "Declaraciones"

        Private m_adpSuministros As New SuministrosDataAdapter
        Private m_dstSuministros As New SuministrosDataset

        'Constantes
        Private Const mc_strNoSuministro As String = "NoSuministro"
        Private Const mc_strCantidad As String = "Cantidad"
        Private Const mc_strAdicional As String = "Adicional"
        Private Const mc_strItemName As String = "itemname"
        Private Const mc_strEstadoLinea As String = "EstadoLinea"
        Private Const mc_strEstadoLineaResources As String = "EstadoLinearesources"
        Private Const mc_strCheck As String = "check"

#End Region

#Region "Procedimientos"

        Private Sub EstiloGridSuministros()
            Const intColumnaCondicional As Integer = 10
            'Dim mensaje As String
            'Esta funciön pone las propiedadSCGes del datagrid por código con el objetivo de que cumpla los estándares.

            'Declaraciones generales
            Dim tsSuministros As New DataGridTableStyle

            Call dtgSuministros.TableStyles.Clear()

            Dim tcId As New DataGridTextBoxColumn
            Dim tcNoSuministro As New DataGridTextBoxColumn
            Dim tcNoOrden As New DataGridTextBoxColumn
            Dim tcDescripcion As New DataGridConditionalColumn
            Dim tcEstado As New DataGridConditionalColumn
            Dim tcEstadoResources As New DataGridConditionalColumn
            Dim tcAdicional As New DataGridConditionalColumn
            Dim tcObservaciones As New DataGridValidatedTextColumn
            Dim tclinenum As New DataGridTextBoxColumn
            Dim tcCantidad As New DataGridTextBoxColumn
            Dim tcCantidadPendiente As New DataGridTextBoxColumn
            Dim tcCantidadRecibida As New DataGridTextBoxColumn
            Dim tcCheck As New DataGridCheckColumn
            Dim tcBodega As New DataGridCheckColumn
            Dim tcEstadoTras As New DataGridConditionalColumn

            '
            Dim tcResultado As New DataGridValidatedTextColumn
            'Dos columnas posteriormente agregadas --26-04-06 dorian
            Dim tcNoAdicional As New DataGridConditionalColumn
            Dim tcFecha_Solicitud As New DataGridConditionalColumn
            Dim tcFechaInsercion As New DataGridConditionalColumn

            Dim tcLineNumOriginal As New DataGridTextBoxColumn

            tsSuministros.MappingName = m_dstSuministros.SCGTA_VW_Suministros.TableName

            With tcNoOrden
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.NoOrden  '"No Orden"
                .MappingName = mc_strNoOrden
                .Format = "###"
                .ReadOnly = True
            End With

            tcResultado.Width = 300
            tcResultado.HeaderText = My.Resources.ResourceUI.Resultados
            tcResultado.MappingName = "ResultadoActividad"
            tcResultado.ReadOnly = False
            tcResultado.NullText = ""
            AddHandler tcResultado.Cambio_Valor, AddressOf CambiaResultadoSuministros

            tcFechaInsercion.Width = 100
            tcFechaInsercion.HeaderText = My.Resources.ResourceUI.FechaInsercion
            tcFechaInsercion.MappingName = "FechaInsercion"
'            tcFechaInsercion.P_Formato = "{0:d}"
            tcFechaInsercion.ReadOnly = True
            tcFechaInsercion.NullText = ""
            tcFechaInsercion.P_ColumnaCondicional = intColumnaCondicional
            tcFechaInsercion.P_ColorCondicional = Color.Maroon


            With tcNoSuministro
                .Width = 100
                .HeaderText = My.Resources.ResourceUI.NoSuministro '"No Suministro"
                .MappingName = mc_strNoSuministro
                .Format = "###"
                .ReadOnly = True
            End With

            With tcCantidad
                .Width = 75
                .HeaderText = My.Resources.ResourceUI.Cantidad  '"Cantidad"
                .MappingName = mc_strCantidad
                '.Format = "###"
                .ReadOnly = True
            End With

            With tcCantidadRecibida
                .Width = 100
                .HeaderText = My.Resources.ResourceUI.Cantidadrecibida  '"Cantidad"
                .MappingName = "CantidadRecibida"
                '.Format = "###"
                .ReadOnly = True
            End With

            With tcCantidadPendiente
                .Width = 100
                .HeaderText = My.Resources.ResourceUI.CantidadPendiente  '"Cantidad"
                .MappingName = "CantidadPendiente"
                '.Format = "###"
                .ReadOnly = True
            End With

            With tcDescripcion
                .Width = 300
                .HeaderText = My.Resources.ResourceUI.Suministro  '"Suministro"
                .MappingName = mc_strItemName
                .NullText = ""
                .ReadOnly = True
                .P_ColumnaCondicional = intColumnaCondicional
                .P_ColorCondicional = Color.Maroon
            End With

            With tcObservaciones
                .Width = 300
                .HeaderText = My.Resources.ResourceUI.Observaciones ' "Observaciones"
                .MappingName = mc_strObservaciones
                .NullText = ""
'                .ReadOnly = True
'                .P_ColumnaCondicional = intColumnaCondicional
'                .P_ColorCondicional = Color.Maroon
                AddHandler .Cambio_Valor, AddressOf CambiaObservacionSuministros
            End With

            With tcEstado
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.Aprobacion  '"Aprobación"
                .MappingName = mc_strEstadoLinea
                .NullText = ""
                .ReadOnly = True
                .P_ColumnaCondicional = intColumnaCondicional
                .P_ColorCondicional = Color.Maroon
            End With

            With tcEstadoResources
                .Width = 110
                .HeaderText = My.Resources.ResourceUI.Aprobacion  '"Aprobación"
                .MappingName = mc_strEstadoLineaResources
                .NullText = ""
                .ReadOnly = True
                .P_ColumnaCondicional = intColumnaCondicional
                .P_ColorCondicional = Color.Maroon
            End With

            With tcAdicional
                .Width = 0
                .MappingName = mc_strAdicional
                .ReadOnly = True
                .P_ColumnaCondicional = intColumnaCondicional
                .P_ColorCondicional = Color.Maroon
            End With


            With tcCheck
                .MappingName = mc_strCheck
                .Width = 30
                .AllowNull = False
            End With

            With tcBodega
                .HeaderText = My.Resources.ResourceUI.Almacen  '"Almacén"
                .MappingName = mc_blnBodega
                .Width = 50
                .AllowNull = False

            End With

            With tcEstadoTras
                .HeaderText = My.Resources.ResourceUI.Traslado  '"Traslado"
                .MappingName = "DescTrasladada"
                .Width = 100
                .NullText = " "
                .P_ColumnaCondicional = intColumnaCondicional
                .P_ColorCondicional = Color.Maroon

            End With

            '*******************************
            'tcLineNumOriginal.Width = 0
            'tcLineNumOriginal.HeaderText = "LineNumOriginal"
            'tcLineNumOriginal.MappingName = "LineNumOriginal"
            'tcLineNumOriginal.ReadOnly = True
            'tcLineNumOriginal.NullText = String.Empty

            'Visibles

            tsSuministros.GridColumnStyles.Add(tcCheck)
            tsSuministros.GridColumnStyles.Add(tcDescripcion)
            tsSuministros.GridColumnStyles.Add(tcNoSuministro)
            tsSuministros.GridColumnStyles.Add(tcEstado)
            tsSuministros.GridColumnStyles.Add(tcEstadoResources)
            tsSuministros.GridColumnStyles.Add(tcCantidad)
            tsSuministros.GridColumnStyles.Add(tcCantidadRecibida)
            tsSuministros.GridColumnStyles.Add(tcCantidadPendiente)
            tsSuministros.GridColumnStyles.Add(tcBodega)
            tsSuministros.GridColumnStyles.Add(tcNoOrden)
            tsSuministros.GridColumnStyles.Add(tcAdicional)
            tsSuministros.GridColumnStyles.Add(tcObservaciones)
            tsSuministros.GridColumnStyles.Add(tcResultado)
            tsSuministros.GridColumnStyles.Add(tcEstadoTras)
            tsSuministros.GridColumnStyles.Add(tcFechaInsercion)

            'Establece propiedades del datagrid (colores estándares).
            tsSuministros.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
            tsSuministros.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
            tsSuministros.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
            tsSuministros.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
            tsSuministros.RowHeadersVisible = False
            tsSuministros.PreferredRowHeight = 50

            'Hace que el datagrid adopte las propiedades del TableStyle.
            dtgSuministros.TableStyles.Add(tsSuministros)

        End Sub

        Private Sub CargaGridSuministros1(ByVal intAdicional As Integer)

            Try

                Call m_dstSuministros.Clear()

                Call m_adpSuministros.Fill(m_dstSuministros, m_strNoOrden, -1, intAdicional)

                CargarEstadoLineaResources(m_dstSuministros)

                m_dstSuministros.SCGTA_VW_Suministros.DefaultView.AllowDelete = False
                'm_dstSuministros.SCGTA_VW_Suministros.DefaultView.AllowEdit = False
                m_dstSuministros.SCGTA_VW_Suministros.DefaultView.AllowNew = False

                dtgSuministros.DataSource = m_dstSuministros.SCGTA_VW_Suministros

                Call EstiloGridSuministros()


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try

        End Sub

        Private Sub EliminarSuministros()

            Dim objDA As New DMSOneFramework.SCGDataAccess.SuministrosDataAdapter
            Dim drwSuministros As DMSOneFramework.SuministrosDataset.SCGTA_VW_SuministrosRow
            'Dim IntCodEstado As Integer
            'Dim intCodFase As Integer
            Dim strMensaje As String = ""
            Dim blnEliminarPaquetes As Boolean = False
            Try

                MetodosCompartidosSBOCls.IniciaTransaccion()

                MetodosCompartidosSBOCls.IniciarCotizacion(m_drdOrdenCurrent.NoCotizacion)

                For Each drwSuministros In m_dstSuministros.SCGTA_VW_Suministros.Rows

                    If Not drwSuministros.Check Then

                        drwSuministros.RejectChanges()

                    Else
                        If drwSuministros.LineNumFather = -1 Then
                            If drwSuministros.CodEstadoLinea = SCGEstadoLinea.scgFaltaAprobacion Then
                                MetodosCompartidosSBOCls.EliminarItemCotizacion(drwSuministros.LineNum)

                                drwSuministros.Delete()
                                g_AgregaAdicionales = True
                            Else
                                If strMensaje = "" Then
                                    strMensaje = "'" & drwSuministros.itemName & "'"
                                Else
                                    strMensaje = strMensaje & ", '" & drwSuministros.itemName & "'"
                                End If
                                drwSuministros.RejectChanges()
                            End If
                        Else
                            blnEliminarPaquetes = True
                            If MessageBox.Show(My.Resources.ResourceUI.PreguntaItemPertenecePaqueteEliminar, My.Resources.ResourceUI.EliminarItems, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                If EliminarPaquete(drwSuministros.LineNumFather, strMensaje) Then
                                    MessageBox.Show(My.Resources.ResourceUI.MensajeLosSiguientesItems & ": " & strMensaje & " " & My.Resources.ResourceUI.MensajeFueronEliminadosCorrectamente)
                                    Exit For
                                Else
                                    MessageBox.Show(My.Resources.ResourceUI.MensajePaqueteNoEliminadoPuesLosItems & " " & strMensaje + " " & My.Resources.ResourceUI.MensajeNoPuedenEliminarse)
                                End If
                            End If
                        End If
                    End If
                Next
                If Not blnEliminarPaquetes Then
                    If strMensaje <> "" Then

                        objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajelosItems & " " & strMensaje & " " & My.Resources.ResourceUI.MensajeNoEliminadosXAprobados)

                    End If

                    Call objDA.EliminarSuministros(m_dstSuministros)

                    CargaGridSuministros1(0)
                End If

                MetodosCompartidosSBOCls.ActualizarCotizacion()

                MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Commit)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                Throw ex
            End Try



        End Sub

#End Region

#Region "Eventos"

        Private Sub btnAgregaSum_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregaSum.Click

            Dim frmAdicionales As New frmAdicionales1(3, m_strNoOrden, m_drdOrdenCurrent.NoCotizacion, m_blnAgregaAdicional, m_drdVisitaCurrent.NoVisita)
            Call frmAdicionales.ShowDialog()
            Call CargaGridSuministros1(IIf(chkAdicionalesSu.Checked, -1, 0))

        End Sub

        Private Sub chkAdicionalesSu_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAdicionalesSu.CheckedChanged

            Try

                Call CargaGridSuministros1(IIf(chkAdicionalesSu.Checked, -1, 0))


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex

            End Try


        End Sub

        Private Sub btnEliminaSum_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminaSum.Click
            Try
                Me.MdiParent.Cursor = Cursors.WaitCursor

                EliminarSuministros()

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally

                Call CargaGridSuministros1(IIf(chkAdicionalesSu.Checked, -1, 0))

            End Try
        End Sub

        Private Sub dtgSuministros_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgSuministros.GotFocus
            Try

                G_CancelarEditColumnDataGrid(Me, dtgSuministros)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub CambiaObservacionSuministros(ByRef sender As DataGridTextBox)

            Dim intFila As Integer
            Dim strObservacion As String
            Dim dtbItems As SuministrosDataset.SCGTA_VW_SuministrosDataTable

            Dim drwSum As SuministrosDataset.SCGTA_VW_SuministrosRow
            Dim objDA As New BLSBO.GlobalFunctionsSBO
            Dim cnxFecha As New SqlConnection(DAConexion.ConnectionString)
            Dim comFecha As New SqlCommand
            comFecha = cnxFecha.CreateCommand


            Try

                dtbItems = m_dstSuministros.SCGTA_VW_Suministros
                intFila = dtgSuministros.CurrentCell.RowNumber

                drwSum = dtbItems.Rows(intFila)
                If CType(sender, DataGridTextBox).Text <> "" Then
                    strObservacion = CType(sender, DataGridTextBox).Text
                    objDA.ActualizaObservacionLinea(m_drdOrdenCurrent.NoCotizacion, strObservacion, drwSum.LineNum)

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub CambiaResultadoSuministros(ByRef sender As DataGridTextBox)

            Dim intFila As Integer
            Dim strResultado As String
            Dim dtbItems As SuministrosDataset.SCGTA_VW_SuministrosDataTable

            Dim drwSum As SuministrosDataset.SCGTA_VW_SuministrosRow
            Dim objDA As New BLSBO.GlobalFunctionsSBO
            Dim cnxFecha As New SqlConnection(DAConexion.ConnectionString)
            Dim comFecha As New SqlCommand
            comFecha = cnxFecha.CreateCommand

            Try

                dtbItems = m_dstSuministros.SCGTA_VW_Suministros
                intFila = dtgSuministros.CurrentCell.RowNumber

                drwSum = dtbItems.Rows(intFila)

                If CType(sender, DataGridTextBox).Text <> "" Then
                    strResultado = CType(sender, DataGridTextBox).Text
                    objDA.ActualizaResultado(m_drdOrdenCurrent.NoCotizacion, strResultado, drwSum.LineNum)

                    If cnxFecha.State = ConnectionState.Closed Then
                        cnxFecha.Open()
                        comFecha.CommandText = "Update SCGTA_TB_SuministroxOrden set fechaSync =  GETDATE() Where ID = " & drwSum.ID
                        comFecha.ExecuteNonQuery()
                        cnxFecha.Close()
                    Else
                        comFecha.CommandText = "Update SCGTA_TB_SuministroxOrden set fechaSync =  GETDATE() Where ID = " & drwSum.ID
                        comFecha.ExecuteNonQuery()
                        cnxFecha.Close()
                    End If

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

#End Region

    End Class

End Namespace
