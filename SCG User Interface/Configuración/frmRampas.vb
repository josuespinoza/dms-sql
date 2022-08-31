Imports Proyecto_SCGToolBar.SCGToolBar
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmRampas

#Region "Declaraciones"

        Private m_adpRampas As RampasDataAdapter
        Private m_dstRampas As RampasDataSet

        Private Const mc_strIDRampa As String = "IDRampa"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strEstadoLogico As String = "EstadoLogico"
        Private Const mc_strEtiqueta As String = "Etiqueta"
#End Region

#Region "Contructor"
        Sub New(ByVal cargaforma As Boolean)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()


            ' Add any initialization after the InitializeComponent() call.

        End Sub
#End Region

#Region "Eventos"

        Private Sub frmRampas_Load(ByVal sender As Object, _
                                   ByVal e As System.EventArgs) Handles Me.Load

            Try

                With ScgTbRampas

                    .Buttons(enumButton.Exportar).Visible = False
                    .Buttons(enumButton.Imprimir).Visible = False
                    .Buttons(enumButton.Buscar).Visible = False
                    .Buttons(enumButton.Cancelar).Visible = False
                    .Buttons(enumButton.Eliminar).Visible = False
                    .Buttons(enumButton.Nuevo).Visible = False

                End With



                If CargarRampasActivas(m_adpRampas, _
                                       m_dstRampas) Then

                    dtgRampas.DataSource = m_dstRampas.SCGTA_TB_Rampas

                End If

                m_dstRampas.SCGTA_TB_Rampas.DefaultView.AllowDelete = False

                'Call EstiloGrid()

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub ScgTbRampas_Click_Guardar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgTbRampas.Click_Guardar

            Try

                Call m_adpRampas.Update(m_dstRampas)

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Function ValidaFilasBorradas(ByRef dtbRampas As RampasDataSet.SCGTA_TB_RampasDataTable) As Boolean

            Dim drwRampas As RampasDataSet.SCGTA_TB_RampasRow

            Try

                For Each drwRampas In dtbRampas.Rows

                    If drwRampas.RowState = DataRowState.Deleted Then

                        Call drwRampas.RejectChanges()
                        drwRampas.EstadoLogico = False

                    End If

                Next drwRampas

                Return True
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Return False
            End Try

        End Function

        Private Sub ScgTbRampas_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgTbRampas.Click_Cerrar
            Call Me.Close()
        End Sub

        'Private Sub dtgrampas_gotfocus(ByVal sender As Object, ByVal e As System.EventArgs)
        '    Try

        '        G_CancelarEditColumnDataGrid(Me, dtgRampas)

        '    Catch ex As Exception
        '        SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
        '    End Try
        'End Sub

#End Region

#Region "Metodos"

        Private Function CargarRampasActivas(ByRef adpRampas As RampasDataAdapter, _
                                             ByRef dstRampas As RampasDataSet) As Boolean

            Try
                adpRampas = New RampasDataAdapter
                dstRampas = New RampasDataSet

                Call adpRampas.Fill(dstRampas)

                Return True

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
                Return False
            End Try

        End Function


        'Private Sub EstiloGrid()

        '    'Declaraciones generales
        '    Dim tsRampas As New DataGridTableStyle
        '    Call dtgRampas.TableStyles.Clear()

        '    Dim tcCodigo As New DataGridTextBoxColumn
        '    Dim tcDescripcion As New DataGridTextBoxColumn
        '    Dim tcEstadoLogico As New DataGridCheckColumn

        '    tsRampas.MappingName = m_dstRampas.SCGTA_TB_Rampas.TableName

        '    With tcCodigo
        '        .Width = 0
        '        .HeaderText = My.Resources.ResourceUI.IdRampa
        '        .MappingName = mc_strIDRampa
        '        '.ReadOnly = True
        '    End With

        '    With tcDescripcion
        '        .Width = 184
        '        .HeaderText = mc_strDescripcion
        '        .MappingName = mc_strDescripcion
        '        '.ReadOnly = True
        '    End With

        '    With tcEstadoLogico
        '        .Width = 50
        '        .HeaderText = My.Resources.ResourceUI.Estado
        '        .MappingName = mc_strEstadoLogico
        '        .AllowNull = False
        '        .NullValue = False
        '        '.ReadOnly = True
        '    End With


        '    tsRampas.GridColumnStyles.Add(tcCodigo)
        '    tsRampas.GridColumnStyles.Add(tcDescripcion)
        '    tsRampas.GridColumnStyles.Add(tcEstadoLogico)


        '    tsRampas.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
        '    tsRampas.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
        '    tsRampas.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
        '    tsRampas.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))

        '    dtgRampas.TableStyles.Add(tsRampas)
        '    'dtgRampas.ReadOnly = True

        'End Sub

#End Region

    End Class

End Namespace

