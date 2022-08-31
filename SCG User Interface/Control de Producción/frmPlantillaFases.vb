Imports System.Data.SqlClient
Imports DMSOneFramework.SCGCommon
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework

Namespace SCG_User_Interface
    Public Class frmPlantillaFases
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        Public Sub New(ByVal CargaForma As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents cboFasesProdF As SCGComboBox.SCGComboBox
        Friend WithEvents btnVerReporte As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPlantillaFases))
            Me.cboFasesProdF = New SCGComboBox.SCGComboBox
            Me.btnVerReporte = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'cboFasesProdF
            '
            Me.cboFasesProdF.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboFasesProdF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboFasesProdF.EstiloSBO = True
            Me.cboFasesProdF.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cboFasesProdF.ItemHeight = 15
            Me.cboFasesProdF.Location = New System.Drawing.Point(5, 10)
            Me.cboFasesProdF.Name = "cboFasesProdF"
            Me.cboFasesProdF.Size = New System.Drawing.Size(248, 23)
            Me.cboFasesProdF.TabIndex = 2
            '
            'btnVerReporte
            '
            Me.btnVerReporte.BackgroundImage = CType(resources.GetObject("btnVerReporte.BackgroundImage"), System.Drawing.Image)
            Me.btnVerReporte.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnVerReporte.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnVerReporte.Image = CType(resources.GetObject("btnVerReporte.Image"), System.Drawing.Image)
            Me.btnVerReporte.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnVerReporte.Location = New System.Drawing.Point(174, 37)
            Me.btnVerReporte.Name = "btnVerReporte"
            Me.btnVerReporte.Size = New System.Drawing.Size(80, 20)
            Me.btnVerReporte.TabIndex = 827
            Me.btnVerReporte.Text = "   Aceptar"
            '
            'frmPlantillaFases
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.ClientSize = New System.Drawing.Size(256, 69)
            Me.Controls.Add(Me.btnVerReporte)
            Me.Controls.Add(Me.cboFasesProdF)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmPlantillaFases"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "<SCG>Boletas de calidad por fase"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region "Declaraciones"

        Private Const mc_strbackSlash As String = "\"
        Private  mc_strTitulodeReporte As String = My.Resources.ResourceUI.rptTituloReporteBoletasCalidadXFase
        Private mc_strNombreReporte As String = My.Resources.ResourceUI.rptNombrePlantillaCalidad
        Private mc_strTodas As String = My.Resources.ResourceUI.Todas
        Private m_Mostrada As Boolean = True
        Private m_strParametros As String
        Private objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)
#End Region

#Region "Metodos"
        Public Function LlamaReporte(ByVal strParametros As String, _
                                    ByVal TituloDeReporte As String, _
                                    ByVal FileName As String, _
                                    ByVal Server As String, _
                                    ByVal BD As String, _
                                    ByVal PathReporte As String, _
                                    ByVal User As String, _
                                    ByVal Password As String) As Boolean

            Dim rptGeneral As ComponenteCristalReport.SubReportView

            Try
                'Se carga el path donde se encuentra el reporte desde la base de datos de Configuración


                If System.IO.Directory.Exists(PathReporte) Then

                    If Not PathReporte.EndsWith(mc_strbackSlash) Then

                        PathReporte &= mc_strbackSlash
                    End If

                    If System.IO.File.Exists(PathReporte & FileName) Then

                        rptGeneral = New ComponenteCristalReport.SubReportView

                        With rptGeneral

                            .P_BarraTitulo = TituloDeReporte
                            .P_WorkFolder = PathReporte
                            .P_Filename = FileName
                            .P_Server = Server
                            .P_DataBase = BD
                            .P_User = User
                            .P_Password = Password
                            .P_ParArray = strParametros

                        End With

                        Call rptGeneral.VerReporte()

                    Else
                        MsgBox(My.Resources.ResourceUI.MensajeProblemaCargaReporte)
                        Return False
                    End If
                Else
                    MsgBox(My.Resources.ResourceUI.MensajeProblemaCargaReporte)
                    Return False
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Function
#End Region

        Private Sub btnVerReporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVerReporte.Click

            If cboFasesProdF.Text = mc_strTodas Then
                m_strParametros = -1
            Else
                m_strParametros = Busca_Codigo_Texto(cboFasesProdF.Text)
            End If

            Call LlamaReporte(m_strParametros, _
                              mc_strTitulodeReporte, _
                              mc_strNombreReporte, _
                              Server, _
                              strDATABASESCG, _
                              PATH_REPORTES, _
                              UserSCGInternal, _
                              Password)

        End Sub

        Private Sub frmPlantillaFases_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                Call objUtilitarios.CargarCombos(cboFasesProdF, 1)
                Call cboFasesProdF.Items.Add(mc_strTodas)
                cboFasesProdF.Text = mc_strTodas

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            Finally
            End Try
        End Sub
        ''B
        Private Const mc_strpathFuenteMixit As String = "pathFuenteMixit"
        Private Const mc_strpathDestinoMixit As String = "pathDestinoMixit"
        Private Const mc_strIdCentroCostoPintura As String = "CentrodeCostosPintura"
        Private Const mc_strTiempoEnMinutos As String = "TiempoEnMinutos"

    End Class
End Namespace

















