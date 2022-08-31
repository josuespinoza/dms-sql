
Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmOrdenesEspeciales

#Region "Declaraciones"

        Private m_adpOrdenesEspeciales As New OQUTDataAdapter

        Private m_strOTPadre As String

        Private WithEvents m_objOrdenesEspeciales As frmDetalleOrdenesEspeciales

        Private m_drwOrdenPadre As OrdenTrabajoDataset.SCGTA_TB_OrdenRow

        Private m_strCardCodeOrig As String
        Private m_strCardNameOrig As String
        Private m_strValidaRepPendientes As String

#End Region

#Region "Constructor"

        Public Sub New(ByVal p_strOTPadre As String, _
                       ByVal p_drwOrdenPadre As OrdenTrabajoDataset.SCGTA_TB_OrdenRow, _
                       ByVal p_strCardCodeOrig As String, _
                       ByVal p_strCardNameOrig As String, _
                       ByVal p_strValidaRepPendientes As String)

            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()

            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
            m_strOTPadre = p_strOTPadre

            m_drwOrdenPadre = p_drwOrdenPadre
            m_strCardCodeOrig = p_strCardCodeOrig
            m_strCardNameOrig = p_strCardNameOrig
            m_strValidaRepPendientes = p_strValidaRepPendientes

        End Sub

#End Region

#Region "Eventos"

        Private Sub frmOrdenesEspeciales_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            m_adpOrdenesEspeciales.Fill(m_dtsOrdenTrabajo, , m_strOTPadre)

        End Sub

        Private Sub btnNueva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNueva.Click

            Dim Forma_Nueva As Form
            Dim blnExisteForm As Boolean

            For Each Forma_Nueva In Me.MdiParent.MdiChildren
                If Forma_Nueva.Name = "frmDetalleOrdenesEspeciales" Then
                    blnExisteForm = True
                End If
            Next

            If blnExisteForm Then

                m_objOrdenesEspeciales = Nothing

            End If

            m_objOrdenesEspeciales = New frmDetalleOrdenesEspeciales(m_drwOrdenPadre.NoCotizacion, m_drwOrdenPadre, m_strCardCodeOrig, m_strCardNameOrig, m_strValidaRepPendientes)

            With m_objOrdenesEspeciales
                .MdiParent = Me.MdiParent
                .Show()
            End With

        End Sub

        Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
            Me.Close()
        End Sub

        Private Sub m_objOrdenesEspeciales_eOrdenGenerada(ByVal p_intNoCotizacion As Integer) Handles m_objOrdenesEspeciales.eOrdenGenerada

            m_objOrdenesEspeciales.Close()
            m_dtsOrdenTrabajo.SCGTA_TB_Orden.Rows.Clear()
            m_adpOrdenesEspeciales.Fill(m_dtsOrdenTrabajo, , m_strOTPadre)

        End Sub

#End Region

#Region "Métodos"



#End Region


    End Class

End Namespace