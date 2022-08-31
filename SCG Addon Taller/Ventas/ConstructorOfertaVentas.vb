Imports SAPbouiCOM

Module ConstructorOfertaVentas

    ''' <summary>
    ''' Carga los controles de DMS en el formulario de oferta de ventas
    ''' </summary>
    ''' <param name="FormUID">ID única del formulario</param>
    ''' <param name="pVal">Variable con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar o no con el evento</param>
    ''' <remarks></remarks>
    Public Sub CargarControles(ByVal FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oFormulario As SAPbouiCOM.Form
        Dim DocumentoXML As Xml.XmlDataDocument
        Dim InnerXML As String = String.Empty
        Dim Folder As SAPbouiCOM.Folder
        Dim Path As String = String.Empty
        Dim Nodo As Xml.XmlNode
        Dim UsaInterfazFord As String = String.Empty
        Dim oCombo As ComboBox
        Dim oItem As SAPbouiCOM.Item
        Dim oMatrix As Matrix
        Dim BloquearColumnaAprobacion As String = String.Empty

        Try
            If pVal.FormTypeEx = "149" AndAlso pVal.BeforeAction Then
                oFormulario = ObtenerFormulario(FormUID)
                '---------------------------------------
                'Paso 1 Preparar el XML
                '---------------------------------------
                AddChooseFromList(oFormulario)
                DocumentoXML = New Xml.XmlDataDocument
                Path = String.Format("{0}{1}", System.Windows.Forms.Application.StartupPath, My.Resources.Resource.XMLOfertaVentas)
                'Carga el XML con la información de los controles que se van a cargar en la oferta de ventas
                'el XML debe tener la propiedad UPDATE para poder actualizar el formulario
                DocumentoXML.Load(Path)

                'Los valores válidos deben agregarse directamente al XML antes de cargarlo, esto por un tema de rendimiento
                'que puede ser hasta un 85% superior respecto a realizarlo por medio de los objetos posterior a la carga del XML
                CargarValoresValidos(oFormulario, DocumentoXML)
                'Las rutas de las imagenes que se utilizan en el XML deben actualizarse por las rutas reales
                ActualizarUbicacionImagenes(DocumentoXML)

                'Se debe actualizar el UID en el XML para que coincida con el formulario en el cual vamos agregar los controles
                Nodo = DocumentoXML.SelectSingleNode("/Application/forms/action/form")
                Nodo.Attributes.ItemOf("uid").Value = FormUID
                InnerXML = DocumentoXML.InnerXml

                'Actualiza la oferta de ventas con base al XML
                DMS_Connector.Company.ApplicationSBO.LoadBatchActions(InnerXML)

                '----------------------------------------------
                'Paso 2 Acciones posteriores a la carga del XML
                '----------------------------------------------

                'Se agrega el folder "Recepción" a la agrupación
                Folder = oFormulario.Items.Item("SCGD_FdN").Specific
                Folder.GroupWith("138")
                Folder.Item.AffectsFormMode = False
                AgregaControlesTabContenido(oFormulario)

                'Verifica si se utiliza la interfaz FORD
                UsaInterfazFord = DMS_Connector.Configuracion.ParamGenAddon.U_Usa_IFord.Trim
                If Not UsaInterfazFord = "Y" Then
                    oFormulario.Items.Item("stTipoPago").Visible = False
                    oFormulario.Items.Item("cboTipPago").Visible = False
                    oFormulario.Items.Item("stDptoSrv").Visible = False
                    oFormulario.Items.Item("cboDptoSrv").Visible = False
                End If

                'Verifica si se utiliza solicitud de OT especial
                If Utilitarios.MostrarMenu("SCGD_SOE", DMS_Connector.Company.ApplicationSBO.Company.UserName) Then
                    oFormulario.Items.Item("btnSotE").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 6, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    oFormulario.Items.Item("btnSotE").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 9, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    oFormulario.Items.Item("btnSotE").Visible = False
                    oFormulario.Items.Item("btnSotE").Enabled = False
                End If

                oFormulario.DataSources.UserDataSources.Item("btnAsMul").Value = "Y"

                oMatrix = oFormulario.Items.Item("38").Specific

                BloquearColumnaAprobacion = DMS_Connector.Helpers.EjecutarConsulta(" SELECT U_BloqApro FROM [@SCGD_ADMIN] with (nolock) ")

                If BloquearColumnaAprobacion.ToUpper() = "Y" Then
                    'Inhabilita la columna aprobado de la cotización para los usuarios indicados.
                    If DMS_Connector.Helpers.PermisosMenu("SCGD_BEA") Then
                        'Desactiva la columna aprobado
                        oMatrix.Columns.Item("U_SCGD_Aprobado").Editable = False
                    End If
                End If

                If Utilitarios.MostrarMenu("SCGD_SOE", DMS_Connector.Company.ApplicationSBO.Company.UserName) Then
                    oFormulario.DataSources.UserDataSources.Item("btnSolOT").ValueEx = "Y"
                End If

                

                'Boton Asingnacion Multiple
                'If AgregaBTNAsigMul(oForm, SBO_Application) Then
                '    userDS.Item("btnAsMul").Value = "Y"
                '    oItem = oForm.Items.Item(mc_strBtnAsigMult)
                '    oItem.Visible = True
                '    oItem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
                'End If

                'Carga cualquier valor predeterminado en los controles
                CargarValoresPredeterminados(oFormulario)
                SetAutoManageAttributes(oFormulario)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Define los estados de los controles de acuerdo al modo del formulario
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub SetAutoManageAttributes(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oItem As SAPbouiCOM.Item
        Try
            'Estado en modo vista, OK y crear
            oFormulario.Items.Item("SCGD_etCOT").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etNOT").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etOT").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("SCGD_etEst").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("SCGD_etAño").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("SCGD_etFOT").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("SCGD_etNVi").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("SCGD_TxLS").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("SCGD_etMar").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("SCGD_etMod").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("SCGD_etVIN").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("SCGD_etHOT").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("SCGD_cbEst").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("SCGD_etNSe").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("SCGD_etNCi").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("SCGD_etFC").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("SCGD_etHC").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("SCGD_etNoU").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oFormulario.Items.Item("btnAsM").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("btnAsM").Enabled = False
            oFormulario.Items.Item("btnAsM").Visible = True

            'Estado en el modo búsqueda
            oFormulario.Items.Item("SCGD_etCOT").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etNOT").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etOT").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etEst").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etAño").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etFOT").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etNVi").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_TxLS").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etMar").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etMod").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etVIN").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etHOT").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_cbEst").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etNSe").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etNCi").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etFC").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etHC").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oFormulario.Items.Item("SCGD_etNoU").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 4, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Reemplaza las ubicaciones de las imagenes por las ubicaciones reales en el ambiente productivo del cliente
    ''' </summary>
    ''' <param name="DocumentoXML">Documento XML que contiene la información de los controles que se van a agregar a la oferta de ventas</param>
    ''' <remarks></remarks>
    Private Sub ActualizarUbicacionImagenes(ByRef DocumentoXML As Xml.XmlDataDocument)
        Dim Nodo As Xml.XmlNode
        Try
            Nodo = DocumentoXML.SelectSingleNode("/Application/forms/action/form/items/action/item[@uid=""btnSN""]/specific")
            Nodo.Attributes.ItemOf("image").Value = System.Windows.Forms.Application.StartupPath.ToString & "\CFL.BMP"

            Nodo = DocumentoXML.SelectSingleNode("/Application/forms/action/form/items/action/item[@uid=""SCGD_btDVe""]/specific")
            Nodo.Attributes.ItemOf("image").Value = System.Windows.Forms.Application.StartupPath.ToString & "\Flecha.BMP"

            Nodo = DocumentoXML.SelectSingleNode("/Application/forms/action/form/items/action/item[@uid=""SCGD_btpPl""]/specific")
            Nodo.Attributes.ItemOf("image").Value = System.Windows.Forms.Application.StartupPath.ToString & "\CFL.BMP"

            Nodo = DocumentoXML.SelectSingleNode("/Application/forms/action/form/items/action/item[@uid=""SCGD_btRec""]/specific")
            Nodo.Attributes.ItemOf("image").Value = System.Windows.Forms.Application.StartupPath.ToString & "\CFL.BMP"
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de agregar los datos necesarios por la matriz del formulario de oferta de ventas
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub AgregaControlesTabContenido(ByVal oFormulario As SAPbouiCOM.Form)
        Dim oitem As SAPbouiCOM.Item
        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            Call AddChooseFromListColaboradores(oFormulario)

            oitem = oFormulario.Items.Item("38")
            oMatrix = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)

            oMatrix.Columns.Item("U_SCGD_EmpAsig").ChooseFromListUID = "CFL_Col"
            oMatrix.Columns.Item("U_SCGD_EmpAsig").ChooseFromListAlias = "empID"
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Agrega los ChooseFromList utilizados por las líneas de la matriz
    ''' </summary>
    ''' <param name="oFormulario"></param>
    ''' <remarks></remarks>
    Private Sub AddChooseFromListColaboradores(ByVal oFormulario As Form)
        Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
        Dim oCons As SAPbouiCOM.Conditions
        Dim oCon As SAPbouiCOM.Condition
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
        Try
            oCFLs = oFormulario.ChooseFromLists
            oCFLCreationParams = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            ' Adding 2 CFL, one for the button and one for the edit text.
            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = "171"
            oCFLCreationParams.UniqueID = "CFL_Col"
            oCFL = oCFLs.Add(oCFLCreationParams)

            ' Adding Conditions to CFL1
            oCons = oCFL.GetConditions()
            oCon = oCons.Add()
            oCon.Alias = "U_SCGD_T_Fase"
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
            oCFL.SetConditions(oCons)

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Agrega los ChooseFromList utilizados por los distintos controles de la oferta de ventas
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub AddChooseFromList(ByVal oFormulario As SAPbouiCOM.Form)
        Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
        Dim oCons As SAPbouiCOM.Conditions
        Dim oCon As SAPbouiCOM.Condition
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
        Try
            oCFLs = oFormulario.ChooseFromLists
            oCFLCreationParams = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            ' Adding 2 CFL, one for the button and one for the edit text.
            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = "SCGD_VEH"
            oCFLCreationParams.UniqueID = "CFL1"
            oCFL = oCFLs.Add(oCFLCreationParams)

            ' Adding Conditions to CFL1
            oCons = oCFL.GetConditions()
            oCon = oCons.Add()
            oCon.Alias = "U_CardCode"
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCFL.SetConditions(oCons)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = "SCGD_VEH"
            oCFLCreationParams.UniqueID = "CFL5"
            oCFL = oCFLs.Add(oCFLCreationParams)

            ' Adding Conditions to CFL5
            oCons = oCFL.GetConditions()
            oCon = oCons.Add()
            oCon.Alias = "U_CardCode"
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCFL.SetConditions(oCons)


            oCFLCreationParams.UniqueID = "CFL2"
            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = SAPbouiCOM.BoLinkedObject.lf_Employee
            oCFL = oCFLs.Add(oCFLCreationParams)

            oCFLCreationParams.UniqueID = "CFL3"
            oCFLCreationParams.MultiSelection = False
            'oCFLCreationParams.ObjectType = "23"
            oCFLCreationParams.ObjectType = "SCGD_OT"
            oCFL = oCFLs.Add(oCFLCreationParams)

            oCons = oCFL.GetConditions()
            oCon = oCons.Add()
            'oCon.Alias = "U_SCGD_Num_Vehiculo"
            oCon.Alias = "U_NoUni"
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCFL.SetConditions(oCons)

            oCFLCreationParams.UniqueID = "CFL4"
            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = SAPbobsCOM.ServiceTypes.ProjectsService
            oCFL = oCFLs.Add(oCFLCreationParams)

            AplicaValorDeCondicion(0, oFormulario, "CFL3", "")

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = "2"
            oCFLCreationParams.UniqueID = "CFL6"
            oCFL = oCFLs.Add(oCFLCreationParams)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Define los parámetros de los oConditions para algunos ChooseFromList específicos
    ''' </summary>
    ''' <param name="intIndice"></param>
    ''' <param name="oform"></param>
    ''' <param name="idCFL"></param>
    ''' <param name="strValor"></param>
    ''' <remarks></remarks>
    Private Sub AplicaValorDeCondicion(ByVal intIndice As Integer, ByVal oform As SAPbouiCOM.Form, ByVal idCFL As String, ByVal strValor As String)


        Dim oCons As SAPbouiCOM.Conditions
        Dim oCon As SAPbouiCOM.Condition
        Dim oCFL As SAPbouiCOM.ChooseFromList

        Try
            oCFL = oform.ChooseFromLists.Item(idCFL)
            oCons = oform.ChooseFromLists.Item(idCFL).GetConditions
            oCon = oCons.Item(intIndice)

            'oCon.BracketOpenNum = 1
            If strValor <> "" Then
                oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCon.CondVal = strValor
            Else
                oCon.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                oCon.CondVal = "'" & CStr(-1) & ""
            End If

            If (idCFL = "CFL1" Or idCFL = "CFL5") And oCons.Count < 2 Then
                If oCons.Count = 1 And strValor = "" Then
                    oCon.BracketOpenNum = 1
                    oCon.Relationship = BoConditionRelationship.cr_OR
                    oCon = oCons.Add()
                    oCon.Alias = "U_CardCode"
                    oCon.Operation = SAPbouiCOM.BoConditionOperation.co_IS_NULL
                    oCon.BracketCloseNum = 1
                End If

                oCon.Relationship = BoConditionRelationship.cr_AND
                oCon = oCons.Add
                oCon.Alias = "U_Activo"
                oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCon.CondVal = "Y"
            End If

            oCFL.SetConditions(oCons)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los valores predeterminados cuando se abre una nueva instancia del formulario oferta de ventas
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub CargarValoresPredeterminados(ByRef oFormulario As SAPbouiCOM.Form)
        Dim NumeroOT As String = String.Empty
        Dim NoOTReferencia As String = String.Empty
        Dim NumeroCita As String = String.Empty
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim oEditText As SAPbouiCOM.EditText
        Dim HoraActual As DateTime = DateTime.Now()
        Try
            NumeroOT = oFormulario.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).TrimEnd()
            NoOTReferencia = oFormulario.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_NoOtRef", 0).Trim()
            NumeroCita = oFormulario.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_NoCita", 0).Trim()

            'Carga el valor predeterminado de la sucursal cuando los campos están en blanco (Documento nuevo)
            If String.IsNullOrEmpty(NumeroOT) AndAlso String.IsNullOrEmpty(NoOTReferencia) AndAlso String.IsNullOrEmpty(NumeroCita) Then
                oComboBox = oFormulario.Items.Item("SCGD_cbSuc").Specific
                If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                    oComboBox.Select(oFormulario.DataSources.DBDataSources.Item("OQUT").GetValue("BPLId", 0).Trim(), BoSearchKey.psk_ByValue)
                Else
                    oComboBox.Select(Utilitarios.ObtieneIdSucursal(DMS_Connector.Company.ApplicationSBO).ToString, BoSearchKey.psk_ByDescription)
                End If
            End If

            'Oculta los campos de la sucursal cuando las sucursales de SAP están activadas sin importar si el documento
            'es nuevo o uno existente
            If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                'Dependiendo de la configuración del tipo de sucursal utilizada (SAP o DMS), se ocultan los campos
                oFormulario.Items.Item("SCGD_stSuc").Visible = False
                oFormulario.Items.Item("SCGD_cbSuc").Visible = False
            End If

            'Fecha y hora de recepción
            If String.IsNullOrEmpty(oFormulario.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Fech_Recep", 0)) Then
                oEditText = oFormulario.Items.Item("SCGD_etFec").Specific
                oEditText.Value = HoraActual.ToString("yyyyMMdd")
            End If
            
            If String.IsNullOrEmpty(oFormulario.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Hora_Recep", 0)) Then
                oEditText = oFormulario.Items.Item("SCGD_etHor").Specific
                oEditText.Value = HoraActual.ToString("HHmm")
            End If
            
            'Fecha y hora de compromiso
            If String.IsNullOrEmpty(oFormulario.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Fech_Comp", 0)) Then
                oEditText = oFormulario.Items.Item("SCGD_etFeC").Specific
                oEditText.Value = HoraActual.ToString("yyyyMMdd")
            End If
            
            If String.IsNullOrEmpty(oFormulario.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Hora_Comp", 0)) Then
                oEditText = oFormulario.Items.Item("SCGD_etHoC").Specific
                oEditText.Value = HoraActual.ToString("HHmm")
            End If
            

            If Not Utilitarios.MostrarMenu("SCGD_BBL", DMS_Connector.Company.ApplicationSBO.Company.UserName) Then
                oFormulario.Items.Item("SCGD_btBal").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            Else
                oFormulario.Items.Item("SCGD_btBal").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub



    ''' <summary>
    ''' Obtiene el formulario desde el cual se ejecutó el evento
    ''' </summary>
    ''' <param name="FormUID">ID única de la instancia del formulario</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenerFormulario(ByVal FormUID As String) As SAPbouiCOM.Form
        Try
            Return DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Agrega los valores válidos al XML
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="DocumentoXML">Documento XML con la información de los controles que se van a cargar en el formulario
    ''' oferta de ventas</param>
    ''' <remarks></remarks>
    Public Sub CargarValoresValidos(ByRef oFormulario As SAPbouiCOM.Form, ByRef DocumentoXML As Xml.XmlDataDocument)
        Try
            AgregarValoresValidosSucursal(oFormulario, DocumentoXML)
            AgregarValoresValidosTiposOrden(oFormulario, DocumentoXML)
            AgregarValoresValidosEstadosOT(oFormulario, DocumentoXML)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Agrega el listado de estados válidos de orden de trabajo al XML que será cargado en el formulario
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="DocumentoXML">Documento XML con la información de los controles que se van a cargar en el formulario oferta de ventas</param>
    ''' <remarks></remarks>
    Private Sub AgregarValoresValidosEstadosOT(ByRef oFormulario As SAPbouiCOM.Form, ByRef DocumentoXML As Xml.XmlDataDocument)
        Dim Nodo As Xml.XmlNode
        Try
            Nodo = DocumentoXML.SelectSingleNode("/Application/forms/action/form/items/action/item[@uid=""SCGD_cbEst""]/specific/ValidValues/action")

            'Obtiene los valores válidos y los agrega uno a uno al Nodo XML que posteriormente podrá ser utilizado
            'para actualizar el formulario
            CrearValorValidoXML(DocumentoXML, Nodo, My.Resources.Resource.EstadoOrdenNoIniciada, My.Resources.Resource.EstadoOrdenNoIniciada)
            CrearValorValidoXML(DocumentoXML, Nodo, My.Resources.Resource.EstadoOrdenEnproceso, My.Resources.Resource.EstadoOrdenEnproceso)
            CrearValorValidoXML(DocumentoXML, Nodo, My.Resources.Resource.EstadoOrdenFinalizada, My.Resources.Resource.EstadoOrdenFinalizada)
            CrearValorValidoXML(DocumentoXML, Nodo, My.Resources.Resource.EstadoOrdenSuspendida, My.Resources.Resource.EstadoOrdenSuspendida)
            CrearValorValidoXML(DocumentoXML, Nodo, My.Resources.Resource.EstadoOrdenCancelada, My.Resources.Resource.EstadoOrdenCancelada)
            CrearValorValidoXML(DocumentoXML, Nodo, My.Resources.Resource.EstadoOrdenFacturada, My.Resources.Resource.EstadoOrdenFacturada)
            CrearValorValidoXML(DocumentoXML, Nodo, My.Resources.Resource.EstadoOrdenEntregada, My.Resources.Resource.EstadoOrdenEntregada)
            CrearValorValidoXML(DocumentoXML, Nodo, My.Resources.Resource.EstadoOrdenCerrada, My.Resources.Resource.EstadoOrdenCerrada)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Agrega un valor válido para un nodo de tipo ComboBox 
    ''' </summary>
    ''' <param name="DocumentoXML">Documento XML con la información de los controles</param>
    ''' <param name="Nodo">Nodo del control que se le desean agregar valores válidos</param>
    ''' <param name="Valor">Valor</param>
    ''' <param name="Descripcion">Descripción</param>
    ''' <remarks></remarks>
    Private Sub CrearValorValidoXML(ByRef DocumentoXML As Xml.XmlDataDocument, ByRef Nodo As Xml.XmlNode, ByVal Valor As String, ByVal Descripcion As String)
        Dim ValorValido As Xml.XmlNode
        Dim Atributo As Xml.XmlAttribute
        Try
            If Not String.IsNullOrEmpty(Valor) Then
                'Crea un nuevo valor válido
                ValorValido = DocumentoXML.CreateElement("ValidValue")
                'Asigna el atributo "Valor"
                Atributo = DocumentoXML.CreateAttribute("value")
                Atributo.Value = Valor
                ValorValido.Attributes.Append(Atributo)
                'Asigna el atributo "Descripción"
                Atributo = DocumentoXML.CreateAttribute("description")
                Atributo.Value = Descripcion
                ValorValido.Attributes.Append(Atributo)
                'Agrega el nuevo valor válido recien creado al nodo
                Nodo.AppendChild(ValorValido)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Agrega el listado de sucursales al XML que será cargado en el formulario
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="DocumentoXML">Documento XML con la información de los controles que se van a cargar en el formulario oferta de ventas</param>
    ''' <remarks></remarks>
    Private Sub AgregarValoresValidosSucursal(ByRef oFormulario As SAPbouiCOM.Form, ByRef DocumentoXML As Xml.XmlDataDocument)
        Dim Nodo As Xml.XmlNode
        Dim Query As String = " SELECT Code, Name FROM [@SCGD_SUCURSALES] with (nolock) "
        Dim oRecordset As SAPbobsCOM.Recordset

        Try
            Nodo = DocumentoXML.SelectSingleNode("/Application/forms/action/form/items/action/item[@uid=""SCGD_cbSuc""]/specific/ValidValues/action")

            'Obtiene los valores válidos y los agrega uno a uno al Nodo XML que posteriormente podrá ser utilizado
            'para actualizar el formulario
            oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordset.DoQuery(Query)

            While Not oRecordset.EoF
                CrearValorValidoXML(DocumentoXML, Nodo, oRecordset.Fields.Item("Code").Value.ToString(), oRecordset.Fields.Item("Name").Value.ToString())
                oRecordset.MoveNext()
            End While


        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Agrega el listado de tipos de orden de trabajo al XML que será cargado en el formulario
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="DocumentoXML">Documento XML con la información de los controles que se van a cargar en el formulario oferta de ventas</param>
    ''' <remarks></remarks>
    Private Sub AgregarValoresValidosTiposOrden(ByRef oFormulario As SAPbouiCOM.Form, ByRef DocumentoXML As Xml.XmlDataDocument)
        Dim Nodo As Xml.XmlNode
        Dim Query As String = String.Empty
        Dim oRecordset As SAPbobsCOM.Recordset
        Try
            Nodo = DocumentoXML.SelectSingleNode("/Application/forms/action/form/items/action/item[@uid=""SCGD_cbTOT""]/specific/ValidValues/action")
            'Obtiene los valores válidos y los agrega uno a uno al Nodo XML que posteriormente podrá ser utilizado
            'para actualizar el formulario

            'If DMS_Connector.Configuracion.ParamGenAddon.U_OT_SAP = "Y" Then
            '    Query = "SELECT [@SCGD_CONF_TIP_ORDEN].U_Code AS Code, [@SCGD_CONF_TIP_ORDEN].U_Name AS Name FROM [@SCGD_CONF_SUCURSAL] INNER JOIN [@SCGD_CONF_TIP_ORDEN] ON [@SCGD_CONF_SUCURSAL].DocEntry = [@SCGD_CONF_TIP_ORDEN].DocEntry "
            'Else
            '    Query = "Select Code, Name from [@SCGD_TIPO_ORDEN] Order By Code"
            'End If

            Query = "Select Code, Name from [@SCGD_TIPO_ORDEN] Order By Code"

            oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordset.DoQuery(Query)

            While Not oRecordset.EoF
                CrearValorValidoXML(DocumentoXML, Nodo, oRecordset.Fields.Item("Code").Value.ToString(), oRecordset.Fields.Item("Name").Value.ToString())
                oRecordset.MoveNext()
            End While
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
End Module
