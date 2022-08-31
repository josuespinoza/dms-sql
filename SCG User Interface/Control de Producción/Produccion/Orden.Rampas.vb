Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
'Imports SCG_ComponenteImagenes.SCG_Imagenes

Namespace SCG_User_Interface

    Partial Class frmOrden
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

#Region "Declaraciones"

#Region "Constantes"

        Const mc_strColID As String = "ID"
        Const mc_strColNoOrden As String = "NoOrden"
        Const mc_strColIDRampa As String = "IDRampa"
        Const mc_strColRampaDesc As String = "RampaDesc"
        Const mc_strColFechaHora As String = "FechaHora"
        Const mc_strColFecha As String = "FechaSola"
        Const mc_strColHora As String = "HoraSola"
        Const mc_strColDuracion As String = "Duracion"
        Const mc_strColCheck As String = "Check"


#End Region

#Region "Objetos"

#Region "Datasets"

        Dim m_dstRampasXOrden As New RampasXOrdenDataset

#End Region

#End Region

#End Region

#Region "Procedimientos"

        Private Sub PrepararInfoRampas()
            Dim adpRampasXOrden As New SCGDataAccess.RampasXOrdenAdapter
            Dim adpFasesXOrden As New SCGDataAccess.FasesXOrdenDataAdapter
            Dim drdRampas As SqlClient.SqlDataReader = Nothing
            Dim m_adtRampas As New RampasDataAdapter
            Try
                dtpRampaFecha.Value = Now.Date

                dtpRampaHora.Value = Now

                drdRampas = m_adtRampas.Fill(New Date(dtpRampaFecha.Value.Year, dtpRampaFecha.Value.Month, dtpRampaFecha.Value.Day, 23, 59, 59))
                Utilitarios.CargarComboSourceByReader(cboRampas, drdRampas)
                drdRampas.Close()

                CargarUnidadesTiempoGlobales()

                Dim dblTiempoAsignadoRampa As Double


                If g_intUnidadTiempo = -1 Then
                    txtRampaDuracion.Text = adpFasesXOrden.GetTiempoTotalAsignado(txtNoOrden.Text)
                Else
                    dblTiempoAsignadoRampa = adpFasesXOrden.GetTiempoTotalAsignado(txtNoOrden.Text)
                    If m_dblValorUnidadTiempo > 0 Then
                        dblTiempoAsignadoRampa = dblTiempoAsignadoRampa / m_dblValorUnidadTiempo
                    Else
                        dblTiempoAsignadoRampa = 0
                    End If
                    txtRampaDuracion.Text = dblTiempoAsignadoRampa
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw
            Finally
                'Agregado 01072010
                If drdRampas IsNot Nothing Then
                    If Not drdRampas.IsClosed Then
                        Call drdRampas.Close()
                    End If
                End If
            End Try


        End Sub

        Private Sub CargarTiempoUnidadesDatasetRampas()
            Dim intIndice As Integer
            For intIndice = 0 To m_dstRampasXOrden.SCGTA_TB_RampasXOrden.Rows.Count - 1
                If m_dblValorUnidadTiempo > 0 Then
                    If Not m_dstRampasXOrden.SCGTA_TB_RampasXOrden.Rows(intIndice)("Duracion") Is System.DBNull.Value Then
                        m_dstRampasXOrden.SCGTA_TB_RampasXOrden.Rows(intIndice)("DuracionUnidadTiempo") = Math.Round(m_dstRampasXOrden.SCGTA_TB_RampasXOrden.Rows(intIndice)("Duracion") / m_dblValorUnidadTiempo, 4)
                    Else
                        m_dstRampasXOrden.SCGTA_TB_RampasXOrden.Rows(intIndice)("DuracionUnidadTiempo") = System.DBNull.Value
                    End If
                Else
                    m_dstRampasXOrden.SCGTA_TB_RampasXOrden.Rows(intIndice)("DuracionUnidadTiempo") = 0
                End If

            Next
        End Sub

        Private Sub CargarInfoRampas()
            Dim adpRampasXOrden As New SCGDataAccess.RampasXOrdenAdapter
            'Dim drwRampasXOrden As RampasXOrdenDataset.SCGTA_TB_RampasXOrdenRow

            m_dstRampasXOrden.Clear()

            adpRampasXOrden.CargaRampasXOrdenByNoOrden(m_dstRampasXOrden, txtNoOrden.Text)
            CargarUnidadesTiempoGlobales()
            CargarTiempoUnidadesDatasetRampas()

            With m_dstRampasXOrden.SCGTA_TB_RampasXOrden.DefaultView
                .AllowDelete = False
                .AllowEdit = True
                .AllowNew = False
            End With

            dtgRampas.DataSource = m_dstRampasXOrden.SCGTA_TB_RampasXOrden

        End Sub

        Private Sub AsignacionOTRampa()
            Dim drwRampasXOrden As RampasXOrdenDataset.SCGTA_TB_RampasXOrdenRow
            Dim adpRampasXOrden As SCGDataAccess.RampasXOrdenAdapter

            CargarUnidadesTiempoGlobales()

            For Each drwRampasXOrden In m_dstRampasXOrden.SCGTA_TB_RampasXOrden.Rows

                drwRampasXOrden.RejectChanges()

            Next

            If cboRampas.SelectedIndex <> -1 Then
                adpRampasXOrden = New SCGDataAccess.RampasXOrdenAdapter

                drwRampasXOrden = m_dstRampasXOrden.SCGTA_TB_RampasXOrden.NewSCGTA_TB_RampasXOrdenRow

                With drwRampasXOrden

                    .NoOrden = txtNoOrden.Text
                    .IDRampa = cboRampas.SelectedValue
                    .FechaHora = New Date(dtpRampaFecha.Value.Year, dtpRampaFecha.Value.Month, dtpRampaFecha.Value.Day, _
                                    dtpRampaHora.Value.Hour, dtpRampaHora.Value.Minute, 0)

                    If g_intUnidadTiempo = -1 Then
                        .Duracion = CDec(txtRampaDuracion.Text)
                    Else
                        .Duracion = CDec(txtRampaDuracion.Text) * m_dblValorUnidadTiempo
                    End If



                End With

                m_dstRampasXOrden.SCGTA_TB_RampasXOrden.Rows.Add(drwRampasXOrden)

                adpRampasXOrden.InsertRampasXOrden(m_dstRampasXOrden)

                CargarInfoRampas()

            Else

                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeDefinirRampas)

            End If

        End Sub

        Private Sub ElimarAsignacionRampa()
            Dim drwRampas As RampasXOrdenDataset.SCGTA_TB_RampasXOrdenRow
            Dim adpRampasXOrden As New SCGDataAccess.RampasXOrdenAdapter

            For Each drwRampas In m_dstRampasXOrden.SCGTA_TB_RampasXOrden.Rows

                If Not drwRampas.Check Then
                    drwRampas.RejectChanges()
                Else
                    drwRampas.Delete()
                End If
            Next

            adpRampasXOrden.DeleteRampasXOrden(m_dstRampasXOrden)

            CargarInfoRampas()

        End Sub

        Private Sub EstiloGridRampas()
            Dim tsConfiguracion As New DataGridTableStyle
            Dim tcID As New DataGridLabelColumn
            Dim tcNoOrden As New DataGridLabelColumn
            Dim tcIDRampa As New DataGridLabelColumn
            Dim tcRampaDesc As New DataGridLabelColumn
            Dim tcFecha As New DataGridLabelColumn
            Dim tcHora As New DataGridLabelColumn
            Dim tcDuracion As New DataGridLabelColumn
            Dim tcCheck As New DataGridCheckColumn
            Dim tcDuracionUnidadesTiempo As New DataGridLabelColumn

            tsConfiguracion.MappingName = m_dstRampasXOrden.SCGTA_TB_RampasXOrden.TableName

            With tcID
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.ID
                .MappingName = mc_strColID
                .ReadOnly = True
            End With

            With tcNoOrden
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.NoOrden '"NoOrden"
                .MappingName = mc_strColNoOrden
                .ReadOnly = True
            End With

            With tcIDRampa
                .Width = 0
                .HeaderText = My.Resources.ResourceUI.NoRampa   '"IDRampa"
                .MappingName = mc_strColIDRampa
                .ReadOnly = True
            End With

            With tcRampaDesc
                .Width = 175
                .HeaderText = My.Resources.ResourceUI.Rampa   '"Rampa"
                .MappingName = mc_strColRampaDesc
                .ReadOnly = True
            End With

            With tcFecha
                .Width = 100
                .HeaderText = My.Resources.ResourceUI.Fecha
                .MappingName = mc_strColFecha
                .Format = "dd/MM/yyyy"
                .ReadOnly = True
            End With


            With tcHora
                .Width = 100
                .HeaderText = My.Resources.ResourceUI.Hora  '"Hora"
                .MappingName = mc_strColHora
                .Format = "hh:mm tt"
                .ReadOnly = True
            End With

            With tcDuracion
                If g_intUnidadTiempo = -1 Then
                    .Width = 100
                Else
                    .Width = 0
                End If

                .HeaderText = My.Resources.ResourceUI.Duracion
                .MappingName = mc_strColDuracion
                .ReadOnly = True
            End With

            With tcDuracionUnidadesTiempo
                If g_intUnidadTiempo <> -1 Then
                    .Width = 100
                Else
                    .Width = 0
                End If

                .HeaderText = My.Resources.ResourceUI.Duracion
                .MappingName = "DuracionUnidadTiempo"
                .ReadOnly = True
            End With

            With tcCheck
                .MappingName = mc_strColCheck
                .Width = 30
                .AllowNull = False
            End With

            'Visibles
            tsConfiguracion.GridColumnStyles.Add(tcCheck)
            tsConfiguracion.GridColumnStyles.Add(tcID)
            tsConfiguracion.GridColumnStyles.Add(tcNoOrden)
            tsConfiguracion.GridColumnStyles.Add(tcIDRampa)
            tsConfiguracion.GridColumnStyles.Add(tcRampaDesc)
            tsConfiguracion.GridColumnStyles.Add(tcFecha)
            tsConfiguracion.GridColumnStyles.Add(tcHora)
            tsConfiguracion.GridColumnStyles.Add(tcDuracion)
            tsConfiguracion.GridColumnStyles.Add(tcDuracionUnidadesTiempo)

            'Establece propiedades del datagrid (colores estándares).
            tsConfiguracion.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
            tsConfiguracion.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
            tsConfiguracion.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
            tsConfiguracion.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
            tsConfiguracion.RowHeadersVisible = False

            'Hace que el datagrid adopte las propiedades del TableStyle.
            dtgRampas.TableStyles.Add(tsConfiguracion)

        End Sub

#End Region

#Region "Eventos"

        Private Sub btnAsignarRampa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsignarRampa.Click
            Try
                If dtpRampaFecha.Value >= Now.Date Then
                    If cboEstadoOrden.SelectedValue <> mc_PriEstado_Suspendida And cboEstadoOrden.SelectedValue <> mc_PriEstado_Finalizada Then
                        AsignacionOTRampa()
                    Else
                        MessageBox.Show(My.Resources.ResourceUI.MensajeOrdenNoFinalizParaAsignarRampa)
                    End If
                Else
                    MessageBox.Show(My.Resources.ResourceUI.MensajeFechaAsignacionSupFechaActual)
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnQuitarRampa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuitarRampa.Click
            Try

                ElimarAsignacionRampa()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtgRampas_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgRampas.GotFocus
            Try

                G_CancelarEditColumnDataGrid(Me, dtgRampas)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

#End Region

    End Class

End Namespace
