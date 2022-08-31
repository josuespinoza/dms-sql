Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmFactVtaNocturna

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

        End Sub

#End Region

#Region "Declaraciones"

#End Region

#Region "Procedimientos"

        Private Sub EstiloGrid()
            'Dim tcCheck As DataGridViewCheckBoxColumn
            'Dim tcNoMensaje As DataGridViewTextBoxColumn
            'Dim tcDetalle As New DataGridViewTextBoxColumn
            'Dim tcNoOrden As New DataGridViewTextBoxColumn
            'Dim tcNoCotizacion As New DataGridViewTextBoxColumn
            'Dim tcFechaApertura As New DataGridViewTextBoxColumn
            'Dim tcFechaCompromiso As New DataGridViewTextBoxColumn
            'Dim tcHoraApertura As New DataGridViewTextBoxColumn
            'Dim tcHoraCompromiso As New DataGridViewTextBoxColumn
            'Dim tcTipoMensaje As New DataGridViewTextBoxColumn
            'Dim tcNoSolicitud As New DataGridViewTextBoxColumn

            dtgActividades.DefaultCellStyle = GetEstiloCellNormal()
            dtgActividades.ColumnHeadersDefaultCellStyle = GetEstiloCellHeader()

            'tcCheck = New DataGridViewCheckBoxColumn
            'With tcCheck
            '    '.Name = mc_strCheck
            '    .ReadOnly = False
            '    '.DataPropertyName = mc_strCheck
            '    .HeaderText = ""
            '    .Width = 30
            '    .ThreeState = False
            '    .Frozen = True
            '    .SortMode = DataGridViewColumnSortMode.NotSortable
            'End With

            'tcNoMensaje = New DataGridViewTextBoxColumn
            'With tcNoMensaje
            '    '.Name = mc_intNoMensaje
            '    .ReadOnly = True
            '    '.DataPropertyName = mc_intNoMensaje
            '    .HeaderText = ""
            '    .Visible = False
            '    .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            'End With

            'tcDetalle = New DataGridViewTextBoxColumn
            'With tcDetalle
            '    '.Name = mc_strDetalle
            '    .ReadOnly = True
            '    '.DataPropertyName = mc_strDetalle
            '    .HeaderText = "Detalle"
            '    .Width = 160
            '    .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            'End With

            'tcNoSolicitud = New DataGridViewTextBoxColumn
            'With tcNoSolicitud
            '    '.Name = mc_strNoSolicitud
            '    .ReadOnly = True
            '    '.DataPropertyName = mc_strNoSolicitud
            '    .HeaderText = "No. Solicitud"
            '    .Width = 160
            '    .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            '    .Visible = False
            'End With

            'tcTipoMensaje = New DataGridViewTextBoxColumn
            'With tcTipoMensaje
            '    '.Name = mc_strTipoMensaje
            '    .ReadOnly = True
            '    '.DataPropertyName = mc_strTipoMensaje
            '    .HeaderText = "Tipo Mensaje"
            '    .Width = 160
            '    .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            '    .Visible = False
            'End With

            'tcNoOrden = New DataGridViewTextBoxColumn
            'With tcNoOrden
            '    '.Name = mc_strNoOrden
            '    .ReadOnly = True
            '    '.DataPropertyName = mc_strNoOrden
            '    .HeaderText = "No.Orden"
            '    .Width = 70
            '    .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            'End With

            'tcNoCotizacion = New DataGridViewTextBoxColumn
            'With tcNoCotizacion
            '    '.Name = mc_strNoCotizacion
            '    .ReadOnly = True
            '    '.DataPropertyName = mc_strNoCotizacion
            '    .HeaderText = "No. Cotización"
            '    .Width = 80
            '    .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            'End With

            'tcHoraApertura = New DataGridViewTextBoxColumn
            'With tcHoraApertura
            '    '.Name = mc_strHoraApertura
            '    .ReadOnly = True
            '    '.DataPropertyName = mc_strHoraApertura
            '    .HeaderText = "Hora Recepción"
            '    .Width = 86
            '    .DefaultCellStyle.Format = "MM:HH"
            '    .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            'End With

            'tcHoraCompromiso = New DataGridViewTextBoxColumn
            'With tcHoraCompromiso
            '    '.Name = mc_strHoraCompromiso
            '    .ReadOnly = True
            '    '.DataPropertyName = mc_strHoraCompromiso
            '    .HeaderText = "Hora Compromiso"
            '    .Width = 96
            '    .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            'End With

            'tcFechaApertura = New DataGridViewTextBoxColumn
            'With tcFechaApertura
            '    '.Name = mc_strFechaApertura
            '    .ReadOnly = True
            '    '.DataPropertyName = mc_strFechaApertura
            '    .HeaderText = "Fecha Recepción"
            '    .Width = 94
            '    .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            '    .DefaultCellStyle.Format = "dd/MM/yyyy"
            'End With

            'tcFechaCompromiso = New DataGridViewTextBoxColumn
            'With tcFechaCompromiso
            '    '.Name = mc_strFechaCompromiso
            '    .ReadOnly = True
            '    '.DataPropertyName = mc_strFechaCompromiso
            '    .HeaderText = "Fecha Compromiso"
            '    .Width = 94
            '    .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            '    .DefaultCellStyle.Format = "dd/MM/yyyy"
            'End With

            With dtgActividades

                '    .Columns.Add(tcCheck)
                '    .Columns.Add(tcNoMensaje)
                '    .Columns.Add(tcDetalle)
                '    .Columns.Add(tcNoOrden)
                '    .Columns.Add(tcNoCotizacion)
                '    .Columns.Add(tcNoSolicitud)
                '    .Columns.Add(tcFechaApertura)
                '    .Columns.Add(tcHoraApertura)
                '    .Columns.Add(tcFechaCompromiso)
                '    .Columns.Add(tcHoraCompromiso)
                '    .Columns.Add(tcTipoMensaje)

                '    .AutoGenerateColumns = False
                '    .AllowUserToAddRows = False
                '    .AllowUserToDeleteRows = False
                '    .AllowUserToOrderColumns = False
                '    .RowHeadersVisible = False
                '    .MultiSelect = False
                '    .SelectionMode = DataGridViewSelectionMode.FullRowSelect

                .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(244, 244, 240)

            End With

        End Sub

        Private Function GetEstiloCellHeader() As DataGridViewCellStyle
            Dim objEstiloCell As DataGridViewCellStyle

            objEstiloCell = New DataGridViewCellStyle

            With objEstiloCell
                .Font = New Font("Arial Unicode MS", 9, FontStyle.Regular)
                .Alignment = DataGridViewContentAlignment.MiddleLeft
                .BackColor = Color.FromArgb(222, 223, 206)
                .ForeColor = Color.FromArgb(77, 77, 77)
                .SelectionBackColor = Color.FromArgb(222, 223, 206)
            End With

            Return objEstiloCell

        End Function

        Private Function GetEstiloCellNormal() As DataGridViewCellStyle
            Dim objEstiloCell As DataGridViewCellStyle

            objEstiloCell = New DataGridViewCellStyle

            With objEstiloCell
                .Font = New Font("Arial Unicode MS", 8, FontStyle.Regular)
                .Alignment = DataGridViewContentAlignment.MiddleLeft
                .BackColor = Color.FromArgb(253, 253, 253)
                .ForeColor = Color.FromArgb(77, 77, 77)
                .SelectionBackColor = Color.Beige
                .SelectionForeColor = Color.FromArgb(0, 53, 106)
            End With

            Return objEstiloCell

        End Function

        Private Sub BuscarLineasFact()

            Dim adpFacts As New DMSOneFramework.SCGDataAccess.FacturasEspecialesDataAdapter

            dstFacturasEspeciales = New DMSOneFramework.FacturasEspecialesDataset

            adpFacts.Fill(dstFacturasEspeciales, txtNoFactura.Tag)

            dtgActividades.DataSource = dstFacturasEspeciales

        End Sub

        Private Sub AsignarColaborador(ByVal p_intRowIndex As Integer, ByVal p_intColIndex As Integer)
            Try
                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion

                dtgActividades.CurrentCell = dtgActividades.Item(p_intColIndex, p_intRowIndex)

                With SubBEmpleados

                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarratitulosBuscadorEmpleados
                    .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre
                    .Criterios = "Cod_Empleado,Nombre"
                    .Tabla = "SCGTA_VW_Empleados"
                    .Where = "branch = " & G_strIDSucursal & " and U_SCGD_T_FASE <> ''"
                    .Activar_Buscador(dtgActividades.CurrentRow)

                End With
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub AgregarColaboradorFact()

            Dim adpFacts As New DMSOneFramework.SCGDataAccess.FacturasEspecialesDataAdapter

            If adpFacts.Update(dstFacturasEspeciales) Then
                objSCGMSGBox.msgInserModiElim()
            End If

        End Sub

#End Region

#Region "Eventos"

        Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
            Me.Close()
        End Sub

        Private Sub frmFactVtaNocturna_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                EstiloGrid()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub picFacturas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picFacturas.Click
            Try
                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion

                With SubBFacturas

                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorFacturasEspeciales

                    .Titulos = My.Resources.ResourceUI.NumeroInterno & "," & My.Resources.ResourceUI.NoFactura & _
                    "," & My.Resources.ResourceUI.CodCliente & "," & My.Resources.ResourceUI.Nombre & _
                    "," & My.Resources.ResourceUI.Fecha
                    '"DocEntry,# Factura,Cod. Cliente,Nombre,Fecha"

                    .Criterios = "OINV.DocEntry,DocNum,CardCode,CardName,OINV.DocDate"
                    .Tabla = "SCGTA_VW_OINV OINV " ''& _
                    ''"INNER JOIN SCGTA_VW_INV1 INV1 " & _
                    ''"ON OINV.DocEntry=INV1.DocEntry"
                    .Criterios_OcultosEx = "1"
                    .Where = "U_SCGD_TipoVenta=1 AND OINV.DocType='I'"
                    .Activar_Buscador(sender)

                End With
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub SubBFacturas_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles SubBFacturas.AppAceptar
            Try

                txtNoFactura.Tag = Campo_Llave
                txtNoFactura.Text = Arreglo_Campos(1)
                txtCliente.Text = Arreglo_Campos(2) & " - " & Arreglo_Campos(3)
                txtFecha.Text = Arreglo_Campos(4)

                BuscarLineasFact()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub dtgActividades_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgActividades.CellClick
            Try

                If dtgActividades.Columns(e.ColumnIndex).Name = "Buscar" Then

                    If e.RowIndex >= 0 Then

                        AsignarColaborador(e.RowIndex, e.ColumnIndex)

                    End If

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub SubBEmpleados_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles SubBEmpleados.AppAceptar
            Dim intDocEntry As Integer
            Dim strItemCode As String
            Dim intLineNum As Integer

            Dim objDataGridRow As Windows.Forms.DataGridViewRow

            Try

                objDataGridRow = CType(sender, Windows.Forms.DataGridViewRow)

                intDocEntry = txtNoFactura.Tag
                strItemCode = objDataGridRow.Cells(1).Value
                intLineNum = objDataGridRow.Cells(0).Value

                With dstFacturasEspeciales.FacturasEspecialesDataTable.FindByDocEntryItemCodeLineNum(intDocEntry, strItemCode, intLineNum)

                    .EmpID = Arreglo_Campos(0)
                    .Colaborador = Arreglo_Campos(1)

                End With

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
            Try

                If txtNoFactura.Tag <> 0 Then

                    AgregarColaboradorFact()

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

#End Region

    End Class

End Namespace
