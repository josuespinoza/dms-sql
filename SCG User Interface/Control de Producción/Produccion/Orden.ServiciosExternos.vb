Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
'Imports SCG_ComponenteImagenes.SCG_Imagenes

Namespace SCG_User_Interface

    Partial Class frmOrden
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

#Region "Declaraciones"

        Private m_dstServiciosExternos As RepuestosxOrdenDataset
        Private Const mc_strServicioExterno As String = "Servicio Externo"

#End Region

#Region "Eventos"

        Private Sub btnAgregarSE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregarSE.Click
            Dim intCodEstado As Integer

            Try
                Dim frmAdicionales As New frmAdicionales1(enTipoArticulo.ServicioExterno, m_strNoOrden, m_drdOrdenCurrent.NoCotizacion, m_blnAgregaAdicional, m_drdVisitaCurrent.NoVisita)
                Call frmAdicionales.ShowDialog()

                IntCodEstado = CInt(Busca_Codigo_Texto(cboEstadoRep2.Text, True))
                Call CargarGridRepuesto(intCodEstado, IIf(chkAdicionalesSE.Checked, 1, 0), _
                                        enTipoArticulo.ServicioExterno, m_dstServiciosExternos, _
                                        dtgSE, My.Resources.ResourceUI.ServiciosExternos)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub cbEstadoSE_SelectedIndexChanged(ByVal sender As Object, _
                                                    ByVal e As System.EventArgs) Handles cbEstadoSE.SelectedIndexChanged
            Dim intCodigoEstado As Integer
            Try

                If Not Me.MdiParent Is Nothing Then
                    Me.MdiParent.Cursor = Cursors.WaitCursor
                End If


                intCodigoEstado = CInt(Busca_Codigo_Texto(cbEstadoSE.Text, True))

                Call CargarGridRepuesto(intCodigoEstado, IIf(chkAdicionalesSE.Checked, 1, 0), _
                                        enTipoArticulo.ServicioExterno, m_dstServiciosExternos, _
                                        dtgSE, mc_strServicioExterno)

                'If dtgSE.TableStyles.Count > 0 Then


                '    If intCodigoEstado = 3 Then
                '        dtgSE.TableStyles(0).GridColumnStyles(mc_blnBodega).ReadOnly = False
                '    Else
                '        dtgSE.TableStyles(0).GridColumnStyles(mc_blnBodega).ReadOnly = True
                '    End If

                'End If

                'Agregado. 26/05/06. Alejandra. Se permite generar una orden de compra sólo si el estado
                'del repuesto es Pendiente o Pendiente por Devolución

                If m_drdOrdenCurrent.Estado <> mc_NumEstado_Finalizada And m_drdOrdenCurrent.Estado <> mc_NumEstado_Cancelada Then

                    If intCodigoEstado = 0 Or intCodigoEstado = 1 Or intCodigoEstado = 4 Then
                        btnOrdenCompraSE.Enabled = True
                    Else
                        btnOrdenCompraSE.Enabled = False
                    End If

                Else



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

        Private Sub chkAdicionalesSE_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAdicionalesSE.CheckedChanged
            Dim intCodigoEstado As Integer

            Try

                intCodigoEstado = CInt(Busca_Codigo_Texto(cbEstadoSE.Text, True))

                Call CargarGridRepuesto(intCodigoEstado, IIf(chkAdicionalesSE.Checked, 1, 0), _
                                        enTipoArticulo.ServicioExterno, m_dstServiciosExternos, _
                                        dtgSE, mc_strServicioExterno)

                'If chkAdicionalesSE.Checked = True Then
                '    m_bolAdicional = True
                'Else
                '    m_bolAdicional = False
                'End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub btnEliminarSE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminarSE.Click

            Try
                Me.MdiParent.Cursor = Cursors.WaitCursor

                Call EliminarRepuestos(m_dstServiciosExternos, _
                                       cbEstadoSE, _
                                       chkAdicionalesSE, _
                                       dtgSE, _
                                       enTipoArticulo.ServicioExterno, _
                                       mc_strServicioExterno)

                Me.MdiParent.Cursor = Cursors.Arrow
            Catch ex As Exception
                Me.MdiParent.Cursor = Cursors.Arrow
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally

                Call CargarGridRepuesto(0, IIf(chkAdicionalesSE.Checked, 1, 0), _
                enTipoArticulo.ServicioExterno, m_dstServiciosExternos, _
                dtgSE, mc_strServicioExterno)

            End Try

        End Sub

        Private Sub dtgSE_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgSE.GotFocus

            Try

                G_CancelarEditColumnDataGrid(Me, dtgSE)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

#End Region

    End Class

End Namespace
