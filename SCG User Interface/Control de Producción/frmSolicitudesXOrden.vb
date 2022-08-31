Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmSolicitudesXOrden

#Region "Declaraciones"

        Private m_strNoOrden As String

        Private m_adpSolicitudEspecificos As New SolicitudEspecificosDataAdapter

        Private WithEvents m_objfrmOpenSolicitud As frmSolicitudEspecificos

#End Region

#Region "Constructor"


        Public Sub New(ByVal p_strNoOrden As String)

            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()

            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

            m_strNoOrden = p_strNoOrden

        End Sub

#End Region

#Region "Eventos"

        Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click

            Me.Close()

        End Sub

        Private Sub frmSolicitudesXOrden_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            m_adpSolicitudEspecificos.Fill(m_dtsSolicitudEspecificos, , m_strNoOrden)
            GlobalesUI.LlenarEstadoSolicitudEspecificosResources(m_dtsSolicitudEspecificos)
            txtNoOrden.Text = m_strNoOrden

        End Sub

        Private Sub dtgDetalles_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgDetalles.DoubleClick

            Dim Forma_Nueva As Form
            Dim blnExisteForm As Boolean
            Dim adpOrdenTrabajo As New OrdenTrabajoDataAdapter
            Dim dstOrdenTrabajo As New OrdenTrabajoDataset
            Dim intNoSolicitud As Integer

            Try
                blnExisteForm = False

                If dtgDetalles.CurrentRow IsNot Nothing Then

                    For Each Forma_Nueva In Me.MdiParent.MdiChildren
                        If Forma_Nueva.Name = "frmSolicitudEspecificos" Then
                            blnExisteForm = True
                        End If
                    Next

                    If Not blnExisteForm Then
                        intNoSolicitud = CInt(dtgDetalles.CurrentRow.Cells.Item(1).Value)


                        m_objfrmOpenSolicitud = New frmSolicitudEspecificos(intNoSolicitud)

                        If Not Me.MdiParent Is Nothing Then
                            m_objfrmOpenSolicitud.MdiParent = Me.MdiParent
                        End If

                        m_objfrmOpenSolicitud.Show()

                    Else
                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeErrorCargarSolicitud)
                    End If
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

#End Region

        Private Sub m_objfrmOpenSolicitud_eSolicitudCreada(ByVal p_intNoOslicitud As Integer) Handles m_objfrmOpenSolicitud.eSolicitudCreada
            Try

                m_adpSolicitudEspecificos.Fill(m_dtsSolicitudEspecificos, , m_strNoOrden)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub


        Private Sub btnCancelarSolicitud_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelarSolicitud.Click
            Dim a_drwSolicitudes As SolicitudEspecificosDataset.SCGTA_SP_SelSolicitudEspecificoRow()
            Dim drwSolicitudes As SolicitudEspecificosDataset.SCGTA_SP_SelSolicitudEspecificoRow
            Dim cnConection As SqlClient.SqlConnection = Nothing
            Dim tnTransation As SqlClient.SqlTransaction = Nothing

            Try

                If MessageBox.Show(My.Resources.ResourceUI.MensajeCancelarSolicitud, G_strCompaniaSCG, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    a_drwSolicitudes = m_dtsSolicitudEspecificos.SCGTA_SP_SelSolicitudEspecifico.Select("Check = true")
                    For Each drwSolicitudes In a_drwSolicitudes
                        If drwSolicitudes.Estado = 0 Then
                            drwSolicitudes.Estado = 2
                            drwSolicitudes.RespondidoPor = USUARIO_SISTEMA
                        End If
                    Next
                    m_adpSolicitudEspecificos.Update(m_dtsSolicitudEspecificos, cnConection, tnTransation, True)
                    m_adpSolicitudEspecificos.Fill(m_dtsSolicitudEspecificos, , m_strNoOrden)
                    GlobalesUI.LlenarEstadoSolicitudEspecificosResources(m_dtsSolicitudEspecificos)
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw
            Finally
                'Agregado 06072010
                Call cnConection.Close()
            End Try
        End Sub
    End Class

End Namespace