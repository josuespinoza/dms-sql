Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGCommon


Namespace SCG_User_Interface


    Public Class frmFiltroClientes

        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

#Region "Declaraciones"

        Private WithEvents m_buClientes As New Buscador.SubBuscador
        ' Private m_dstCliente As New DataSet

        Private oFormulario As FrmPublicidadClientes

        Friend WithEvents gbEncabezado As System.Windows.Forms.GroupBox
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents lblTipoCliente As System.Windows.Forms.Label
        Friend WithEvents cboTipoCliente As SCGComboBox.SCGComboBox
        Friend WithEvents ScgTbClientes As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents chkUsaSucursal As System.Windows.Forms.CheckBox

        Friend WithEvents EPPublicidad As System.Windows.Forms.ErrorProvider




        Private m_oCompany As SAPbobsCOM.Company

#End Region

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            InicializarCombo()


            'cboTipoCliente.Items.Add("Hola")

            'Add any initialization after the InitializeComponent() call

        End Sub

        Public Sub New(ByRef oform As FrmPublicidadClientes)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            InicializarCombo()

            oFormulario = oform


            'cboTipoCliente.Items.Add("Hola")

            'Add any initialization after the InitializeComponent() call

        End Sub

#End Region

#Region "Inicializar pantalla"

        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFiltroClientes))
            Me.gbEncabezado = New System.Windows.Forms.GroupBox()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.lblTipoCliente = New System.Windows.Forms.Label()
            Me.cboTipoCliente = New SCGComboBox.SCGComboBox()
            Me.ScgTbClientes = New Proyecto_SCGToolBar.SCGToolBar()
            Me.chkUsaSucursal = New System.Windows.Forms.CheckBox()
            Me.gbEncabezado.SuspendLayout()
            Me.SuspendLayout()
            '
            'gbEncabezado
            '
            resources.ApplyResources(Me.gbEncabezado, "gbEncabezado")
            Me.gbEncabezado.Controls.Add(Me.Label8)
            Me.gbEncabezado.Controls.Add(Me.lblTipoCliente)
            Me.gbEncabezado.Controls.Add(Me.cboTipoCliente)
            Me.gbEncabezado.Name = "gbEncabezado"
            Me.gbEncabezado.TabStop = False
            '
            'Label8
            '
            resources.ApplyResources(Me.Label8, "Label8")
            Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.Label8.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label8.Name = "Label8"
            '
            'lblTipoCliente
            '
            resources.ApplyResources(Me.lblTipoCliente, "lblTipoCliente")
            Me.lblTipoCliente.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblTipoCliente.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblTipoCliente.Name = "lblTipoCliente"
            '
            'cboTipoCliente
            '
            resources.ApplyResources(Me.cboTipoCliente, "cboTipoCliente")
            Me.cboTipoCliente.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboTipoCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboTipoCliente.EstiloSBO = True
            Me.cboTipoCliente.FormattingEnabled = True
            Me.cboTipoCliente.Name = "cboTipoCliente"
            '
            'ScgTbClientes
            '
            resources.ApplyResources(Me.ScgTbClientes, "ScgTbClientes")
            Me.ScgTbClientes.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.SoloLectura
            Me.ScgTbClientes.Name = "ScgTbClientes"
            '
            'chkUsaSucursal
            '
            resources.ApplyResources(Me.chkUsaSucursal, "chkUsaSucursal")
            Me.chkUsaSucursal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.chkUsaSucursal.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.chkUsaSucursal.Name = "chkUsaSucursal"
            Me.chkUsaSucursal.UseVisualStyleBackColor = False
            '
            'frmFiltroClientes
            '
            resources.ApplyResources(Me, "$this")
            Me.Controls.Add(Me.chkUsaSucursal)
            Me.Controls.Add(Me.ScgTbClientes)
            Me.Controls.Add(Me.gbEncabezado)
            Me.Name = "frmFiltroClientes"
            Me.gbEncabezado.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region
       

#Region "Métodos"

        Private Sub ScgTbClientes_Click_Buscar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgTbClientes.Click_Buscar

            Try

                If cboTipoCliente.SelectedItem = My.Resources.ResourceUI.S_Post_Venta Then

                    Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion

                    With m_buClientes

                        .SQL_Cnn = DATemp.ObtieneConexion
                        .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorClientes
                        '.Titulos = My.Resources.ResourceUI.CodCliente & "," & My.Resources.ResourceUI.NombreCliente & "," & My.Resources.ResourceUI.Fecha & "," & My.Resources.ResourceUI.Marca  '"Publicidad,Fecha Envio,Hora Envio,IdEnvioPublicidad,enviado"
                        .Titulos = My.Resources.ResourceUI.CodCliente & "," & My.Resources.ResourceUI.NombreCliente & "," & My.Resources.ResourceUI.Marca & "," & My.Resources.ResourceUI.Fecha
                        '.Criterios = "CardCode,CardName,DocDate,U_SCGD_Des_Marc"
                        .Criterios = "distinct CardCode,CardName,U_SCGD_Des_Marc,DocDate"
                        .Criterios_Ocultos = 0
                        .Tabla = "SCGTA_VW_OQUT"
                        .MultiSeleccion = True
                        .Where = "CardCode <> '' "
                        .Activar_Buscador(sender)

                    End With

                    Exit Sub

                End If


                If cboTipoCliente.SelectedItem = My.Resources.ResourceUI.P_Venta_Vehiculo Then

                    Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion

                    With m_buClientes

                        .SQL_Cnn = DATemp.ObtieneConexion
                        .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorClientes


                        If chkUsaSucursal.Checked = True Then

                            .Titulos = My.Resources.ResourceUI.CodCliente & "," & My.Resources.ResourceUI.NombreCliente & "," & My.Resources.ResourceUI.Marca & "," & My.Resources.ResourceUI.Fecha & "," & My.Resources.ResourceUI.Sucursal '"Publicidad,Fecha Envio,Hora Envio,IdEnvioPublicidad,enviado"
                            .Criterios = "distinct U_CardCode,U_CardName,U_Des_Marc,CreateDate, NameSucursal"
                            .Criterios_Ocultos = 0
                            .Tabla = "SCGTA_VW_Filt_CVENTA"

                        Else
                            .Titulos = My.Resources.ResourceUI.CodCliente & "," & My.Resources.ResourceUI.NombreCliente & "," & My.Resources.ResourceUI.Marca & "," & My.Resources.ResourceUI.Fecha  '"Publicidad,Fecha Envio,Hora Envio,IdEnvioPublicidad,enviado"
                            .Criterios = "distinct U_CardCode,U_CardName,U_Des_Marc,CreateDate"
                            .Criterios_Ocultos = 0
                            .Tabla = "SCGTA_VW_CVENTA"

                        End If

                        .MultiSeleccion = True
                        .Where = "U_Estado <> " & TraerNivelSuperior() & " and U_CardCode <> '' "
                        .Activar_Buscador(sender)

                    End With

                    Exit Sub

                End If

                If cboTipoCliente.SelectedItem = My.Resources.ResourceUI.V_Vehiculos Then


                    Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion

                    With m_buClientes

                        .SQL_Cnn = DATemp.ObtieneConexion
                        .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorClientes


                        If chkUsaSucursal.Checked = True Then

                            .Titulos = My.Resources.ResourceUI.CodCliente & "," & My.Resources.ResourceUI.NombreCliente & "," & My.Resources.ResourceUI.Marca & "," & My.Resources.ResourceUI.Fecha & "," & My.Resources.ResourceUI.Sucursal '"Publicidad,Fecha Envio,Hora Envio,IdEnvioPublicidad,enviado"
                            .Criterios = "distinct U_CardCode,U_CardName,U_Des_Marc,CreateDate, NameSucursal"
                            .Criterios_Ocultos = 0
                            .Tabla = "SCGTA_VW_Filt_CVENTA"

                        Else
                            .Titulos = My.Resources.ResourceUI.CodCliente & "," & My.Resources.ResourceUI.NombreCliente & "," & My.Resources.ResourceUI.Marca & "," & My.Resources.ResourceUI.Fecha '"Publicidad,Fecha Envio,Hora Envio,IdEnvioPublicidad,enviado"
                            .Criterios = "distinct U_CardCode,U_CardName,U_Des_Marc,CreateDate"
                            .Criterios_Ocultos = 0
                            .Tabla = "SCGTA_VW_CVENTA"

                        End If

                        .MultiSeleccion = True
                        .Where = "U_Estado =  " & TraerNivelSuperior() & " and U_CardCode <> '' "
                        .Activar_Buscador(sender)

                    End With

                    Exit Sub

                End If

                If cboTipoCliente.SelectedItem = Nothing Then

                    'Mensaje de error para el usuario
                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeSeleccionTipCliente)

                    Exit Sub

                End If

            Catch ex As Exception

            End Try


        End Sub

        Private Sub ScgTbClientes_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgTbClientes.Click_Cerrar
            Me.Close()
        End Sub


        Private Function TraerNivelSuperior() As Integer

            Dim intValorLeido As Integer

            Dim objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

            intValorLeido = objUtilitarios.TraerNiveles

            Return intValorLeido

        End Function

        Private Sub m_buClientes_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_buClientes.AppAceptar


            oFormulario.m_dstCliente.Tables.Clear()


            Dim objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)
            Dim contador As Integer = 0

            m_buClientes.Titulos.Remove(2)
            m_buClientes.Titulos.Remove(3)



            m_buClientes.Criterios.Remove(2)
            m_buClientes.Criterios.Remove(3)


            Try

                If oFormulario.m_dstCliente.Tables.Count = 0 Then

                    oFormulario.m_dstCliente.Tables.Add(m_buClientes.OUT_DataTable)
                    oFormulario.m_dstCliente.Tables(0).DefaultView.AllowNew = False

                Else

                    oFormulario.m_dstCliente.Merge(m_buClientes.OUT_DataTable)
                End If

                'Tenemos que ir agregando manualmente los correos de todas las personas

                oFormulario.m_dstCliente.Tables(0).Columns(2).ColumnName = "e_mail"

                For Each row As DataRow In oFormulario.m_dstCliente.Tables(0).Rows

                    If objUtilitarios.TraerEmail(oFormulario.m_dstCliente.Tables(0).Rows(contador).Item(0)) = Nothing Then
                        oFormulario.m_dstCliente.Tables(0).Rows(contador).Item(2) = "----"

                    Else

                        oFormulario.m_dstCliente.Tables(0).Rows(contador).Item(2) = objUtilitarios.TraerEmail(oFormulario.m_dstCliente.Tables(0).Rows(contador).Item(0))

                    End If


                    contador += 1

                Next

                oFormulario.dtgClientes.DataSource = oFormulario.m_dstCliente.Tables(0)

                estiloGrid("Table")



                'Se filtra el dataset para que exclusividad de clientes
                Dim datasetFilt As DataSet
                If cboTipoCliente.SelectedItem = My.Resources.ResourceUI.V_Vehiculos Or cboTipoCliente.SelectedItem = My.Resources.ResourceUI.P_Venta_Vehiculo Then
                    datasetFilt = delDupRows(oFormulario.m_dstCliente, "U_CardCode")
               
                Else
                    datasetFilt = delDupRows(oFormulario.m_dstCliente, "CardCode")

                End If

                oFormulario.dtgClientes.DataSource = datasetFilt.Tables(0)



            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub estiloGrid(ByVal NombredeTabla As String)

            'Dim mensaje As String
            'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.
            'Declaraciones generales
            Dim tsReprocesos As New DataGridTableStyle

            Call oFormulario.dtgClientes.TableStyles.Clear()

            Dim tcCardCode As New DataGridTextBoxColumn
            Dim tcCardName As New DataGridTextBoxColumn
            Dim tcE_mail As New DataGridTextBoxColumn
            Dim tcSelectCheck As New DataGridBoolColumn

            Try

                tsReprocesos.MappingName = NombredeTabla '"Table"


                With tcCardCode
                    .Width = 70
                    .HeaderText = My.Resources.ResourceUI.CodCliente

                    If cboTipoCliente.SelectedItem = My.Resources.ResourceUI.V_Vehiculos Or cboTipoCliente.SelectedItem = My.Resources.ResourceUI.P_Venta_Vehiculo Then
                        .MappingName = "U_CardCode"

                    Else
                        .MappingName = "CardCode"
                    End If

                    .ReadOnly = True
                End With

                With tcCardName
                    .Width = 150
                    .HeaderText = My.Resources.ResourceUI.Cliente
                    If cboTipoCliente.SelectedItem = My.Resources.ResourceUI.V_Vehiculos Or cboTipoCliente.SelectedItem = My.Resources.ResourceUI.P_Venta_Vehiculo Then
                        .MappingName = "U_CardName"

                    Else
                        .MappingName = "CardName"
                    End If
                    .ReadOnly = True
                End With

                With tcE_mail
                    .Width = 150
                    .HeaderText = My.Resources.ResourceUI.Email
                    .MappingName = "e_mail"
                    .NullText = "----"
                    .ReadOnly = True
                End With

                With tcSelectCheck
                    .Width = 30 '60
                    '.HeaderText = "Check"
                    .MappingName = "SelectCheck"
                    .ReadOnly = False
                    .AllowNull = False
                End With

                'Agrega las columnas al tableStyle
                ' tsReprocesos.GridColumnStyles.Add(tcNoReprocesoxOrden)

                With tsReprocesos

                    .GridColumnStyles.Add(tcSelectCheck)
                    .GridColumnStyles.Add(tcCardCode)
                    .GridColumnStyles.Add(tcCardName)
                    .GridColumnStyles.Add(tcE_mail)

                End With

                'tsReprocesos.GridColumnStyles.Add(tcSelectCheck)

                'Establece propiedades del datagrid (colores estándares).
                tsReprocesos.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
                tsReprocesos.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                tsReprocesos.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
                tsReprocesos.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
                tsReprocesos.RowHeadersVisible = False
                'Hace que el datagrid adopte las propiedades del TableStyle.

                oFormulario.dtgClientes.TableStyles.Add(tsReprocesos)



            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'objSCGMSGBox.msgExclamationCustom(ex.Message)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub InicializarCombo()


            cboTipoCliente.Items.Add(My.Resources.ResourceUI.P_Venta_Vehiculo)
            cboTipoCliente.Items.Add(My.Resources.ResourceUI.S_Post_Venta)
            cboTipoCliente.Items.Add(My.Resources.ResourceUI.V_Vehiculos)

        End Sub

        Private Function delDupRows(ByVal dTable As DataSet, ByVal colName As String) As DataSet
            Try
                Dim hTable As Hashtable = New Hashtable()
                Dim duplicateList As ArrayList = New ArrayList

                For Each drow As DataRow In dTable.Tables(0).Rows
                    If (hTable.Contains(drow(colName))) Then
                        duplicateList.Add(drow)
                    Else
                        hTable.Add(drow(colName), String.Empty)
                    End If
                Next drow

                For Each drow As DataRow In duplicateList
                    dTable.Tables(0).Rows.Remove(drow)
                Next drow

                Return dTable
            Catch ex As Exception
                Return Nothing
            End Try
        End Function


        Private Sub cboTipoCliente_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTipoCliente.SelectedIndexChanged

            If cboTipoCliente.SelectedItem = My.Resources.ResourceUI.S_Post_Venta Then

                chkUsaSucursal.Checked = False

                chkUsaSucursal.Enabled = False

            Else

                chkUsaSucursal.Enabled = True

            End If

        End Sub


#End Region

       
    End Class


End Namespace