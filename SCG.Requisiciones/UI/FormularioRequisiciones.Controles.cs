using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using SAPbouiCOM;
using SCG.SBOFramework;
using SCG.SBOFramework.UI;
using ICompany = SAPbobsCOM.ICompany;

namespace SCG.Requisiciones.UI
{
    public delegate string LocalizationNeededHandler(
        InformacionLineaRequisicion informacionLineaRequisicion, TipoMensaje tipoMensaje);

    public delegate void TrasladoRealizadoHandler(List<TransferenciaLineasBase> lineasTransferidas, string strTipoReq, ref int codigoError, ref string mensajeError);

    public delegate void AjusteCantidaRealizadoHandler(List<TransferenciaLineasBase> lineasTransferidas);

    public delegate void LineasCanceladasHandler(List<InformacionLineaRequisicion> lineas, EncabezadoRequisicion encabezadoRequisicion, ref int codigoError, ref string mensajeError);

    public delegate InformacionLineaRequisicion CopiarLineasMatrizHandler(
        IDBDataSource dbDataSource, int offset);

    public partial class FormularioRequisiciones
    {
        private struct LineasCotizacion
        {
            public int LineNum;
            public int Aprobado;
            public int Trasladado;
        }

        public EditTextSBO EditTextCodigoCliente;
        public EditTextSBO EditTextEstado;
        public EditTextSBO EditTextFecha;
        public EditTextSBO EditTextHora;
        public EditTextSBO EditTextNoOrden;
        public EditTextSBO EditTextNoRequisicion;
        public EditTextSBO EditTextNombreCliente;
        public EditTextSBO EditTextTipoDocumento;
        public EditTextSBO EditTextTipoRequisicion;
        public EditTextSBO EditTextUsuario;
        public EditTextSBO EditTextComentariosUsuario;
        public FolderSBO FolderMovimientos;
        public FolderSBO FolderRequisiciones;
        public MatrixSBOMovimientosRequisiciones MatrixMovimientos;
        public MatrixSBOLineasRequisiciones MatrixRequisiciones;
        public StaticTextSBO StaticTextCodigoCliente;
        public StaticTextSBO StaticTextEstado;
        public StaticTextSBO StaticTextFecha;
        public StaticTextSBO StaticTextNoOrden;
        public StaticTextSBO StaticTextNoRequisicion;
        public StaticTextSBO StaticTextNombreCliente;
        public StaticTextSBO StaticTextTipoDocumento;
        public StaticTextSBO StaticTextTipoRequisicion;
        public StaticTextSBO StaticTextUsuario;
        public ButtonSBO ButtonSBOTrasladar;
        public ButtonSBO ButtonSBOAjusteCantidad;
        public ButtonSBO ButtonOk;
        public ButtonSBO ButtonCancelar;
        //boton Generar reportes
        public ButtonSBO ButtonGenerarReporte;

        public CheckBoxSBO CheckBoxSelTodo;
        public CheckBoxSBO CheckBoxEntregado;
        public UserDataSource udsForm;

        public SAPbouiCOM.DataTable dtLocal;
        public Boolean m_blnActualizaCot ;
        public Boolean m_blnValidaEntregado ;

        public Requisicion Requisicion { get; private set; }
        public event LocalizationNeededHandler LocalizationNeeded;
        public event TrasladoRealizadoHandler TrasladoRealizado;
        public event AjusteCantidaRealizadoHandler AjusteCantidadRealizado;
        public event LineasCanceladasHandler LineasCanceladas;
        public event CopiarLineasMatrizHandler CopiarLineasMatriz;

        public ComboBoxSBO ComboBoxSucursal;
        public SAPbouiCOM.DataTable dtCantidadesUbicacion;

        private GestorFormularios oGestorFormularios;
        private ListaUbicaciones oFormListaUbi;
        private const string strFormListaUbi = "SCGD_SLUB";

        public FormularioRequisiciones(Application applicationSBO, ICompany companySBO, Requisicion requisicion)
        {
            ApplicationSBO = applicationSBO;
            CompanySBO = companySBO;
            Requisicion = requisicion;
        }

        public bool UsaUbicaciones { get; set; }

        public virtual void ApplicationSBOOnFormDataEvent(ref BusinessObjectInfo businessObjectInfo,
                                                             ref bool bubbleEvent)
        {
            //bubbleEvent = true;
            if (businessObjectInfo.FormTypeEx != FormType) return;
            if (businessObjectInfo.BeforeAction == false && businessObjectInfo.ActionSuccess)
            {
                switch (businessObjectInfo.EventType)
                {
                    case BoEventTypes.et_FORM_DATA_LOAD:
                        DataLoadEvent(businessObjectInfo, ref bubbleEvent);
                        break;

                    case BoEventTypes.et_FORM_LOAD:
                        //LlenarComboSucursal();
                        break;
                }
            }
        }

        public virtual void ApplicationSBOOnItemEvent(string formUid, ref ItemEvent pVal, ref bool bubbleEvent, ref ListaUbicaciones m_oFormSeleccionUbicaciones)
        {
            
            //bubbleEvent = true;
            if (pVal.FormTypeEx != FormType ) return;

            if (!string.IsNullOrEmpty(pVal.ItemUID))
            {
                Item item = FormularioSBO.Items.Item(pVal.ItemUID);
                if ( item != null && !item.Enabled) return;
            }
            switch (pVal.EventType)
            {
                case BoEventTypes.et_CHOOSE_FROM_LIST:
                    if (pVal.ItemUID == EditTextCodigoCliente.UniqueId)
                        CodigoClienteCFLEvent(formUid, pVal, ref bubbleEvent);
                    else if (pVal.ItemUID == EditTextNombreCliente.UniqueId)
                        NombreClienteCFLEvent(formUid, pVal, ref bubbleEvent);
                    else if (pVal.ItemUID == "mtxReq")
                    {

                        if (pVal.ColUID == "colDeUbic" || pVal.ColUID ==  "colAUbic")
                        {
                            DeUbicacionCFLEvent(formUid, pVal, ref bubbleEvent, ref m_oFormSeleccionUbicaciones);
                        }
                        //else
                        //{
                        //    if (pVal.ColUID == "colAUbic")
                        //    {
                        //        AUbicacionCFLEvent(formUid, pVal, ref bubbleEvent);
                        //    }
                        //}
                    }
                    break;
                  
                case BoEventTypes.et_ITEM_PRESSED:

                    FormularioSBO.Freeze(true);

                    if (pVal.ItemUID == FolderMovimientos.UniqueId || pVal.ItemUID == FolderRequisiciones.UniqueId)
                        FolderMovimientosItemPressed(formUid, pVal, ref bubbleEvent);
                    else if (pVal.ItemUID == ButtonSBOTrasladar.UniqueId)
                    {
                        ButtonSBOTrasladarItemPressed(formUid, pVal, ref bubbleEvent);
                    }
                    else if (pVal.ItemUID == ButtonCancelar.UniqueId)
                    {
                        ButtonSBOCancelarItemPressed(formUid, pVal, ref bubbleEvent);
                    }
                    else if (pVal.ItemUID == ButtonGenerarReporte.UniqueId )
                    {
                        ButtonSBOGenerarReporteItemPressed(formUid, pVal, ref bubbleEvent);
                    }
                    else if (pVal.ItemUID == CheckBoxSelTodo.UniqueId)
                    {
                         SeleccionaTodo(formUid, pVal, ref bubbleEvent);
                    }
                    else if (pVal.ItemUID == "btnAjuste")
                    {
                        ButtonDeAjusteCantidades(formUid, pVal, ref bubbleEvent);     
                    }
                    else if (pVal.ItemUID == "1")
                    {
                        ButtonPrincipal(formUid, pVal, ref bubbleEvent);
                    }
                    else if (pVal.ItemUID == CheckBoxEntregado.UniqueId || pVal.ColUID  == MatrixRequisiciones .ColumnaLineaObservcacion .UniqueId )
                    {
                        if(pVal .ActionSuccess ) m_blnActualizaCot = true;
                    }


                    FormularioSBO.Freeze(false);
                    break;

                case BoEventTypes.et_CLICK:

                    if (pVal.ItemUID == ComboBoxSucursal.UniqueId)
                    {
                        //LlenarComboSucursal();

                    }
                    break;


            }
           
        }
       
        /*
        public void ButtonPrincipal(string formUid, ItemEvent pVal, ref bool bubbleEvent)
        {
            if (pVal.BeforeAction)
            {

                FormularioSBO.Items.Item("cboSucur").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False );
                
            }
            else if (pVal.ActionSuccess)
            {

                if (m_blnActualizaCot)
                {
                    m_blnActualizaCot = false;
                    ActualizaCotizacion();
                    ActualizaTansferenciaS();
                }
            }
        }*/

        public void ButtonPrincipal(string formUid, ItemEvent pVal, ref bool bubbleEvent)
        {
            if (pVal.BeforeAction)
            {

                FormularioSBO.Items.Item("cboSucur").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False);

            }
            else if (pVal.ActionSuccess)
            {

                if (m_blnActualizaCot)
                {
                    m_blnActualizaCot = false;
                    //ActualizaCotizacion();
                    //ActualizaTansferenciaS();
                }
            }
        }


        #region IFormularioSBO Members

        public string FormType { get; set; }

        public string NombreXml { get; set; }
        public string Titulo { get; set; }

        public IForm FormularioSBO { get; set; }

        public bool Inicializado { get; set; }

        public void InicializarControles()
        {
            if (FormularioSBO != null)
            {
                dtLocal = FormularioSBO.DataSources.DataTables.Add("dtConsulta");
                dtLocal = FormularioSBO.DataSources.DataTables.Add("dtLocal");
                dtCantidadesUbicacion = FormularioSBO.DataSources.DataTables.Add("dtCantidadesUbicacion"); 

                FolderRequisiciones = new FolderSBO("fldReq");
                FolderMovimientos = new FolderSBO("fldMov");

                StaticTextNoOrden = new StaticTextSBO("stNoOrden");
                StaticTextCodigoCliente = new StaticTextSBO("stCodCl");
                StaticTextNombreCliente = new StaticTextSBO("stNombCl");
                StaticTextTipoRequisicion = new StaticTextSBO("stTipoReq");
                StaticTextTipoDocumento = new StaticTextSBO("stTipoDoc");
                StaticTextNoRequisicion = new StaticTextSBO("stNoReq");
                StaticTextFecha = new StaticTextSBO("stFecha");
                StaticTextUsuario = new StaticTextSBO("stUsuario");
                StaticTextEstado = new StaticTextSBO("stEstado");

                ButtonOk = new ButtonSBO("1", FormularioSBO);
                ButtonCancelar = new ButtonSBO("btnCanc", FormularioSBO);
                ButtonSBOTrasladar = new ButtonSBO("btnTrasl", FormularioSBO);

                ButtonSBOAjusteCantidad = new ButtonSBO("btnAjuste", FormularioSBO);

                ButtonGenerarReporte = new ButtonSBO("btnGnRpt", FormularioSBO);

                EditTextNoOrden = new EditTextSBO("edtNoOrden", true, UDORequisiciones.TablaEncabezado, "U_SCGD_NoOrden",
                                                  FormularioSBO);
                EditTextCodigoCliente = new EditTextSBO("edtCodCl", true, UDORequisiciones.TablaEncabezado,
                                                        "U_SCGD_CodCliente", FormularioSBO);
                EditTextNombreCliente = new EditTextSBO("edtNombCl", true, UDORequisiciones.TablaEncabezado,
                                                        "U_SCGD_NombCliente", FormularioSBO);
                EditTextTipoRequisicion = new EditTextSBO("edtTipoReq", true, UDORequisiciones.TablaEncabezado,
                                                          "U_SCGD_TipoReq", FormularioSBO);
                EditTextTipoDocumento = new EditTextSBO("edtTipoDoc", true, UDORequisiciones.TablaEncabezado,
                                                        "U_SCGD_TipoDoc", FormularioSBO);
                EditTextNoRequisicion = new EditTextSBO("edtNoReq", true, UDORequisiciones.TablaEncabezado,
                                                        "DocNum", FormularioSBO);
                EditTextFecha = new EditTextSBO("edtFecha", true, UDORequisiciones.TablaEncabezado, "CreateDate",
                                                FormularioSBO);
                EditTextHora = new EditTextSBO("edtHora", true, UDORequisiciones.TablaEncabezado, "CreateTime",
                                               FormularioSBO);
                EditTextUsuario = new EditTextSBO("edtUsuario", true, UDORequisiciones.TablaEncabezado, "U_SCGD_Usuario",
                                                  FormularioSBO);
                EditTextComentariosUsuario = new EditTextSBO("txtComen", true, UDORequisiciones.TablaEncabezado, "U_SCGD_Comen",
                                                  FormularioSBO);
                EditTextEstado = new EditTextSBO("edtEstado", true, UDORequisiciones.TablaEncabezado, "U_SCGD_Est", FormularioSBO);

                CheckBoxEntregado = new CheckBoxSBO("chkEnt", true, UDORequisiciones.TablaEncabezado, "U_SCGD_Entregado",FormularioSBO);

                udsForm = FormularioSBO.DataSources.UserDataSources.Add("Sel", BoDataType.dt_SHORT_TEXT, 10);
                CheckBoxSelTodo = new CheckBoxSBO("chkSelTodo", true, "", "Sel", FormularioSBO);

                ComboBoxSucursal = new ComboBoxSBO("cboSucur", FormularioSBO, true, UDORequisiciones.TablaEncabezado, "U_SCGD_IDSuc");                 
                
                var numberFormatInfo = DIHelper.GetNumberFormatInfo(CompanySBO);
                MatrixRequisiciones = new MatrixSBOLineasRequisiciones("mtxReq", FormularioSBO)
                                          {TablaLigada = UDORequisiciones.TablaLineas, NumberFormatInfo = numberFormatInfo};
                if (CopiarLineasMatriz != null)
                    MatrixRequisiciones.CopiarLineasMatriz += CopiarLineasMatriz;
                MatrixRequisiciones.CreaColumnas();
                MatrixRequisiciones.LigaColumnas();
                CargarValidValuesEnCombos();
                
                MatrixRequisiciones.Especifico.SelectionMode = BoMatrixSelect.ms_None;
                
                MatrixMovimientos = new MatrixSBOMovimientosRequisiciones("mtxMov", FormularioSBO)
                                        {TablaLigada = UDORequisiciones.TablaMovimientos, NumberFormatInfo = numberFormatInfo};
                MatrixMovimientos.CreaColumnas();                               
                MatrixMovimientos.LigaColumnas();

                ILinkedButton linked = (ILinkedButton) MatrixMovimientos.ColumnaCodigoDocumento.Columna.ExtendedObject;
                linked.LinkedObjectType = Requisicion.TipoDocumentoMovimiento;

                EditTextNoOrden.AsignaBinding();
                EditTextCodigoCliente.AsignaBinding();
                EditTextNombreCliente.AsignaBinding();
                EditTextTipoRequisicion.AsignaBinding();
                EditTextTipoDocumento.AsignaBinding();
                EditTextNoRequisicion.AsignaBinding();
                EditTextFecha.AsignaBinding();
                EditTextHora.AsignaBinding();
                EditTextEstado.AsignaBinding();
                EditTextUsuario.AsignaBinding();
                EditTextComentariosUsuario.AsignaBinding();

                CheckBoxSelTodo.AsignaBinding();
                CheckBoxEntregado.AsignaBinding();

                EditTextNoOrden.HabilitarBuscar();
                EditTextCodigoCliente.HabilitarBuscar();
                EditTextNombreCliente.HabilitarBuscar();
                EditTextNoRequisicion.HabilitarBuscar();
                EditTextFecha.HabilitarBuscar();
                EditTextUsuario.HabilitarBuscar();
                EditTextEstado.HabilitarBuscar();

                ButtonSBOTrasladar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable,
                                                                   (int) BoAutoFormMode.afm_Find,
                                                                   BoModeVisualBehavior.mvb_False);
                ButtonCancelar.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable,
                                                                   (int) BoAutoFormMode.afm_Find,
                                                                   BoModeVisualBehavior.mvb_False);
                ButtonGenerarReporte.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable,
                                                                   (int)BoAutoFormMode.afm_Find,
                                                                   BoModeVisualBehavior.mvb_False);

                ButtonSBOAjusteCantidad.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable,
                                                                   (int)BoAutoFormMode.afm_Find,
                                                                   BoModeVisualBehavior.mvb_False);

                ComboBoxSucursal.AsignaBinding();

                dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal");
                dtLocal.Clear();
                string l_strSQL = "select U_Entrega_Rep from [@SCGD_CONF_SUCURSAL] " +
                                  " where U_Sucurs = (select branch from OUSR where User_Code = '{0}')";

                dtLocal.ExecuteQuery(string.Format(l_strSQL, ApplicationSBO.Company.UserName));

                if (! string.IsNullOrEmpty((string)dtLocal.GetValue("U_Entrega_Rep", 0)))
                {
                    if (dtLocal.GetValue("U_Entrega_Rep", 0).Equals("Y"))
                    {
                        FormularioSBO.Items.Item(CheckBoxEntregado.UniqueId).Visible = true;
                        m_blnValidaEntregado = true;
                    }
                    else if (dtLocal.GetValue("U_Entrega_Rep", 0).Equals("N"))
                    {
                        FormularioSBO.Items.Item(CheckBoxEntregado.UniqueId).Visible = false;
                        m_blnValidaEntregado = false;
                    }
                }
                LlenarComboSucursal();

                //Valida si usa ubicaciones
                var query = "select U_UsaUbicD from [@SCGD_ADMIN] where code = 'DMS'";
                dtLocal.Clear();
                dtLocal.ExecuteQuery(query);
                if (dtLocal.Rows.Count > 0)
                {
                    //si no usa ubicaciones
                    if(dtLocal.GetValue("U_UsaUbicD", 0).ToString()!="Y")
                    {
                        MatrixRequisiciones.ColumnaDeUbicacion.Columna.Visible = false;
                        MatrixRequisiciones.ColumnaDeUbicacion.Columna.Editable = false;

                        MatrixRequisiciones.ColumnaAUbicacion.Columna.Visible = false;
                        MatrixRequisiciones.ColumnaAUbicacion.Columna.Editable = false;
                    }
                    else
                    {
                        //si usa pero tiene sap 8 o inferior
                        if (CompanySBO.Version < 900000)
                        {
                            MatrixRequisiciones.ColumnaDeUbicacion.Columna.Visible = false;
                            MatrixRequisiciones.ColumnaDeUbicacion.Columna.Editable = false;

                            MatrixRequisiciones.ColumnaAUbicacion.Columna.Visible = false;
                            MatrixRequisiciones.ColumnaAUbicacion.Columna.Editable = false;
                        }
                    }
                }
            }
        }

        public void CargarValidValuesEnCombos()
        {
            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal");
            dtLocal.ExecuteQuery(" SELECT Code, Name FROM [@SCGD_OBSER_REQ] WITH (NOLOCK) ");
            for (int index = 0; index < dtLocal.Rows .Count ; index++)
            {
                MatrixRequisiciones.ColumnaLineaObservcacion.Columna.ValidValues.Add(dtLocal.GetValue("Code", index).ToString() .Trim( ), dtLocal.GetValue("Name", index).ToString() .Trim( ));    
            }
            
        }
               
        public void InicializaFormulario()
        {
            if (FormularioSBO != null)
            {
                FormType = FormularioSBO.TypeEx;
                FormularioSBO.DataBrowser.BrowseBy = "edtNoReq";
                FormularioSBO.PaneLevel = 1;
                FormularioSBO.Mode = BoFormMode.fm_FIND_MODE;
                FormularioSBO.Title = Titulo;

                foreach (SAPbouiCOM.Item oItem in FormularioSBO.Items)
                {

                    if (oItem.UniqueID == "chkSelTodo")
                    {
                        oItem.AffectsFormMode = false;
                    }
                } 
            }
        }

        public ICompany CompanySBO { get; private set; }

        public IApplication ApplicationSBO { get; private set; }

        #endregion

        #region IUsaMenu Members

        public string IdMenu { get; set; }
        public string MenuPadre { get; set; }
        public int Posicion { get; set; }
        public string Nombre { get; set; }

        //Manejo de reportes
        public string DireccionReportes { get; set; }
        public string BDUser { get; set; }
        public string BDPass { get; set; }

        #endregion

        protected virtual string Localize(InformacionLineaRequisicion informacionLineaRequisicion, TipoMensaje tipoMensaje, string  mensaje)
        {
            string m = string.Empty;
            if (LocalizationNeeded != null)
                m = LocalizationNeeded(informacionLineaRequisicion, tipoMensaje);
            return string.IsNullOrEmpty(m) ? mensaje : m;
        }
    }
}
