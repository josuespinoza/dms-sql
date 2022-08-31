Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmConfBodegasporCentroCosto

        Public Sub New(ByVal p_blnEstado As Boolean)

            InitializeComponent()

        End Sub

#Region "Metodos"

        Private Sub CargarSourceBodegas()
            Dim adpBodegasXCC As New DMSOneFramework.SCGDataAccess.ConfBodegasXCCDataAdapter

            adpBodegasXCC.FillBodegasLista(BodegSBODataset)

        End Sub

        Private Sub CargarSourceCentrosCosto()
            Dim adpBodegasXCC As New DMSOneFramework.SCGDataAccess.ConfBodegasXCCDataAdapter

            adpBodegasXCC.FillCentrosCostoLista(CCDataset)

        End Sub

        Private Sub CargarSourceBXCC()
            Dim adpBodegasXCC As New DMSOneFramework.SCGDataAccess.ConfBodegasXCCDataAdapter

            adpBodegasXCC.FillBXCC(ConfBodXCCDataSet)

        End Sub

        Private Sub ActualizarBXCC()
            Dim adpBodegasXCC As New DMSOneFramework.SCGDataAccess.ConfBodegasXCCDataAdapter

            adpBodegasXCC.UpdateBXCC(ConfBodXCCDataSet)

        End Sub

#End Region

#Region "Eventos"

        Private Sub frmConfBodegasporCentroCosto_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                CargarSourceBXCC()

                CargarSourceBodegas()

                CargarSourceCentrosCosto()

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click

            Try

                If ConfBodXCCDataSet.HasChanges Then

                    ActualizarBXCC()

                    objSCGMSGBox.msgInserModiElim()

                    Me.Close()

                End If

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub dtgBodegasConf_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dtgBodegasConf.DataError
            Try
                'Debe seleccionar un centro de costo valido y que no este duplicado
                'You must select a valid profit center that is not duplicated

                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeErrorConfBxCC)

                'e.Cancel = True

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

            Try

                Me.Close()

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

#End Region

        Private Sub frmConfBodegasporCentroCosto_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
            Dim objSize As Size

            If Me.WindowState = FormWindowState.Maximized Then
                Me.WindowState = FormWindowState.Normal
                Me.Dock = DockStyle.Fill
                objSize = Me.Size
                Me.Dock = DockStyle.None
                Me.Top = 0
                Me.Left = 0
                Me.Size = objSize
            End If

        End Sub
    End Class

End Namespace

