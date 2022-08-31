Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon
Imports Proyecto_SCGToolBar

Namespace SCG_User_Interface

    Public Class frmConfTipoOrden

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)
            '            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

#End Region


#Region "Declaraciones"
        Friend Event RetornaDatos()
        Private m_adpTipo As SCGDataAccess.TipoOrdenDataAdapter
        Private m_dstTipo As TipoOrdenDataset

        '-- Constantes que guardan el nombre de las columnas 
        Private mc_strCodTipoOrden As String = "CodTipoOrden"
        Private mc_strDescripcion As String = "Descripcion"
        Private mc_strEstadoLogico As String = "EstadoLogico"
        Private v_intUltimoCodigo As Integer

        '-- Nombre de la constante de la tabla donde se consultan las fases de producción
        Private mc_strTableName As String = "SCGTA_TB_TipoOrden"

        '-- Se inicializa un objeto tipo Utilitarios que recibe el string de conexión con el objetivo de usarla en funciones como carga combos.
        Private objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

        Private m_adpCentrosCosto As New DMSOneFramework.SCGDataAccess.CentroCostoDataAdapter()
        '-- Tipo de inserción que se va a relizar si un update o un insert si el tipo de inserción es 1 es un insert de un nuevo objeto si no es un 2 es un update.
        Private intTipoInsercion As Integer

        Private drw As TipoOrdenDataset.SCGTA_TB_TipoOrdenRow

#End Region


#Region "Metodos"


        Private Sub guardar()

            Dim strCodCentroCosto As String

            Try
                If txtTipoOrden.Text <> "" Then
                    If intTipoInsercion = 1 Then 'Es una nueva fase de producción.

                        'Dim i As String

                        'Dim n As Integer

                        '-- Crea un objeto Datarow del objeto Dataset Fase
                        Dim drwTipo As TipoOrdenDataset.SCGTA_TB_TipoOrdenRow

                        'If m_dstTipo.SCGTA_TB_TipoOrden.Rows.Count = 0 Then

                        '    n = 1

                        'Else

                        '    n = m_dstTipo.SCGTA_TB_TipoOrden.Rows.Count

                        '    i = dtgTipoOrden.Item(n - 1, 0)

                        '    n = CInt(i) + 1

                        'End If

                        '-- Se declara un nuevo row
                        drwTipo = m_dstTipo.SCGTA_TB_TipoOrden.NewRow()


                        '-- Carga el row con los datos adecuados.
                        'drwTipo.CodTipoOrden = n
                        drwTipo.Descripcion = Me.txtTipoOrden.Text
                        drwTipo.EstadoLogico = 1
                        strCodCentroCosto = cboCentroCosto.SelectedValue
                        If strCodCentroCosto <> "" Then
                            drwTipo.CodCentroCosto = CInt(strCodCentroCosto)
                        Else
                            drwTipo.SetCodCentroCostoNull()
                        End If

                        '-- Inserta el row en el Dataset 
                        m_dstTipo.SCGTA_TB_TipoOrden.AddSCGTA_TB_TipoOrdenRow(drwTipo)

                        'Actualiza la base de datos todos los cambios hechos en el el dataset.
                        m_adpTipo.Update(m_dstTipo)

                        Me.txtTipoOrden.Clear()
                        txtTipoOrden.Focus()

                    ElseIf intTipoInsercion = 2 Then

                        drw.Descripcion = Me.txtTipoOrden.Text
                        strCodCentroCosto = cboCentroCosto.SelectedValue
                        If strCodCentroCosto <> "" Then
                            drw.CodCentroCosto = CInt(strCodCentroCosto)
                        Else
                            drw.SetCodCentroCostoNull()
                        End If
                        m_adpTipo.Update(m_dstTipo)

                        cargarTipos()

                        Me.txtTipoOrden.Clear()
                        cboCentroCosto.SelectedIndex = -1
                        cboCentroCosto.SelectedIndex = -1
                        txtTipoOrden.ReadOnly = True
                    End If
                Else
                    objSCGMSGBox.msgRequeridos()
                End If

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub


        Private Sub cargarTipos()

            Try
                m_adpTipo = New SCGDataAccess.TipoOrdenDataAdapter

                m_dstTipo = New TipoOrdenDataset

                'estiloGrid()

                Call m_adpTipo.Fill(m_dstTipo)

                With m_dstTipo.SCGTA_TB_TipoOrden.DefaultView
                    .AllowDelete = True
                    .AllowEdit = True
                    .AllowNew = True
                End With

                dtgTipoOrden.DataSource = m_dstTipo.SCGTA_TB_TipoOrden

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try




        End Sub


        'Private Sub estiloGrid()


        '    'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.

        '    'Declaraciones generales
        '    Dim tsConfiguracion As New DataGridTableStyle

        '    dtgTipoOrden.TableStyles.Clear()


        '    Dim tcCodTipoOrden As New DataGridTextBoxColumn
        '    Dim tcDescripcion As New DataGridTextBoxColumn
        '    Dim tcEstadoLogico As New DataGridTextBoxColumn


        '    tsConfiguracion.MappingName = m_dstTipo.SCGTA_TB_TipoOrden.TableName()


        '    'Carga la columna codigo con las propiedades
        '    With tcCodTipoOrden
        '        .Width = 0
        '        .HeaderText = My.Resources.ResourceUI.Cod
        '        .MappingName = m_dstTipo.SCGTA_TB_TipoOrden.Columns(mc_strCodTipoOrden).ColumnName
        '        .Format = "###"
        '        .ReadOnly = True
        '    End With

        '    'Carga la columna descripcion con las propiedades
        '    With tcDescripcion
        '        .Width = 234
        '        .HeaderText = My.Resources.ResourceUI.TipoOrden
        '        .MappingName = m_dstTipo.SCGTA_TB_TipoOrden.Columns(mc_strDescripcion).ColumnName
        '        .ReadOnly = True
        '    End With

        '    'Carga la columna descripcion con las propiedades
        '    With tcEstadoLogico
        '        .Width = 0
        '        .HeaderText = ""
        '        .MappingName = m_dstTipo.SCGTA_TB_TipoOrden.Columns(mc_strEstadoLogico).ColumnName
        '        .ReadOnly = True
        '    End With


        '    'Agrega las columnas al tableStyle
        '    tsConfiguracion.GridColumnStyles.Add(tcCodTipoOrden)
        '    tsConfiguracion.GridColumnStyles.Add(tcDescripcion)
        '    tsConfiguracion.GridColumnStyles.Add(tcEstadoLogico)


        '    'Establece propiedades del datagrid (colores estándares).
        '    tsConfiguracion.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
        '    tsConfiguracion.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
        '    tsConfiguracion.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
        '    tsConfiguracion.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))


        '    'Hace que el datagrid adopte las propiedades del TableStyle.
        '    dtgTipoOrden.TableStyles.Add(tsConfiguracion)
        '    dtgTipoOrden.ReadOnly = True

        'End Sub


#End Region


#Region "Eventos"


        Private Sub frmMantTipoOrden_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


            cargarTipos()
            'Dim drdCentrosCosto as dat
            'objUtilitarios.CargarCombos(Me.cboCentroCosto, 2)

            'If Me.cboCentroCosto.Items.Count <> 0 Then
            '    'Carga las fases de produccion en el dataset
            '    Me.cboCentroCosto.SelectedIndex = 0

            'End If
            m_adpCentrosCosto.Fill(dtsCentrosCosto)
            intTipoInsercion = 1 'Se inicializa el tipo de inserción en uno para realizar un ingreso de una nueva fase.

            'Se ocultan los botones del toolbar que no se van utilizar
            ScgToolBar1.Buttons(SCGToolBar.enumButton.Exportar).Visible = False
            ScgToolBar1.Buttons(SCGToolBar.enumButton.Imprimir).Visible = False
            ScgToolBar1.Buttons(SCGToolBar.enumButton.Buscar).Visible = False
            ScgToolBar1.Buttons(SCGToolBar.enumButton.Cancelar).Visible = False

            'Se inicializan los botones eliminar y guardar inhabilitados ya que no se puede almacenar nada vacio ni eliminar si no esta un row seleccionado
            ScgToolBar1.Buttons(SCGToolBar.enumButton.Eliminar).Enabled = False
            ScgToolBar1.Buttons(SCGToolBar.enumButton.Guardar).Enabled = False
            Me.cboCentroCosto.SelectedIndex = -1


        End Sub



        Private Sub ScgToolBar1_Click_Guardar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Guardar
            Try

                guardar()

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub


        Private Sub ScgToolBar1_Click_Eliminar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Eliminar
            Try

                Eliminar()

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub
        Private Sub Eliminar()

            Try

                Dim i As Integer  'Valor que va cargar el código de tipo de orden que está en el dataset.

                'Se carga i con el valor del código del tipo de orden.
                i = dtgTipoOrden.Rows.Item(dtgTipoOrden.CurrentRow.Index).Cells(0).Value

                'Se busca el row seleccionado, segun el código cargado en i
                drw = m_dstTipo.SCGTA_TB_TipoOrden.FindByCodTipoOrden(i)

                'Se asigna el valor de 0 al estado logico del tipo de orden, esto equivale a que se hace una eliminación lógica.
                drw.EstadoLogico = 0

                'Se ejecuta la sentencia Delete, en la capa de negocios.
                m_adpTipo.Delete(m_dstTipo)

                'Se refresca el grid.
                cargarTipos()

                'Se limpia el textfield
                Me.txtTipoOrden.Clear()

            Catch ex As Exception

                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub ScgToolBar1_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Cerrar
            Me.Close()
            m_dstTipo.Dispose()
        End Sub


        Private Sub ScgToolBar1_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgToolBar1.Click_Nuevo
            intTipoInsercion = 1
            Me.txtTipoOrden.Clear()
            Me.txtTipoOrden.Focus()
            txtTipoOrden.ReadOnly = False
        End Sub


        Private Sub txtTipoOrden_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

            If Asc(e.KeyChar) = Keys.Enter Then

                guardar()

            End If

        End Sub



#End Region

        Private Sub frmConfTipoOrden_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
            If Asc(e.KeyChar) = Keys.Escape Then Me.Close()
        End Sub




        Private Sub dtgTipoOrden_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgTipoOrden.CellClick
            Try
                Dim i As Integer
                'Se valida que cuando se selecciona algo exista al menos un row en el dataset.
                If m_dstTipo.SCGTA_TB_TipoOrden.Rows.Count <> 0 Then

                    intTipoInsercion = 2

                    txtTipoOrden.ReadOnly = False

                    i = dtgTipoOrden.Rows.Item(dtgTipoOrden.CurrentRow.Index).Cells(0).Value

                    drw = m_dstTipo.SCGTA_TB_TipoOrden.FindByCodTipoOrden(i)

                    Me.txtTipoOrden.Text = drw.Descripcion
                    If Not drw.IsCodCentroCostoNull Then
                        Me.cboCentroCosto.SelectedValue = drw.CodCentroCosto
                    Else
                        Me.cboCentroCosto.SelectedIndex = -1
                        Me.cboCentroCosto.SelectedIndex = -1
                    End If
                    'Se habilita tanto la modificación como la eliminación del row.
                    ScgToolBar1.Buttons(SCGToolBar.enumButton.Eliminar).Enabled = True
                    ScgToolBar1.Buttons(SCGToolBar.enumButton.Guardar).Enabled = True
                    ScgToolBar1.Buttons(SCGToolBar.enumButton.Nuevo).Enabled = True

                End If

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtgTipoOrden_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtgTipoOrden.KeyDown
            If e.KeyCode = Keys.Delete Then
                Eliminar()
            End If
        End Sub
    End Class
End Namespace