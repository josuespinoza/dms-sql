using System;
using System.Reflection;
using System.Collections;
using SAPbobsCOM;

namespace SCG.SBOFramework.DI
{
    /// <summary>
    /// Delegate para pasar a la clase UDO un método.
    /// </summary>
    /// <param name="udoId">ID del UDO</param>
    /// <returns></returns>
    public delegate int GetAutoKeyMethod(string udoId);

    /// <summary>
    /// Clase abstracta para el manejo de UDOS.
    /// </summary>
    public abstract class UDO
    {
        private int _lastErrorCode;
        private string _lastErrorDescription;

        /// <summary>
        /// Constructor para redefinir en la clase heredera.
        /// </summary>
        /// <param name="company">Objeto Company de SBO</param>
        /// <param name="udoId">ID del UDO</param>
        protected UDO(Company company, string udoId)
            : this(company, udoId, null)
        {
        }

        /// <summary>
        /// Constructor para redefinir en la clase heredera.
        /// </summary>
        /// <param name="company">Objeto Company de SBO</param>
        /// <param name="udoId">ID del UDO</param>
        /// <param name="getAutoKeyMethod">Delegate a un método para el cálculo del campo llave.</param>
        protected UDO(Company company, string udoId, GetAutoKeyMethod getAutoKeyMethod)
        {
            Company = company;
            UDOId = udoId;
            GetAutoKeyMethod = getAutoKeyMethod;
        }

        //protected UDO(Company company, string udoId)
        //{
        //    Company = company;
        //    UDOId = udoId;
        //    g_companyService = Company.GetCompanyService();
        //    g_generalService = companyService.GetGeneralService(UDOId);
        //                var encabezadoGeneralDataParams =
        //                    (GeneralDataParams)
        //                    generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

        //                SetPropertiesKeyOnly(encabezadoGeneralDataParams);
        //    g_generalService = generalService.GetByParams(encabezadoGeneralDataParams);
        //}

        /// <summary>
        /// Obejto Company de SBO
        /// </summary>
        public Company Company { get; set; }

        /// <summary>
        /// ID del UDO.
        /// </summary>
        public string UDOId { get; set; }

        /// <summary>
        /// Devuelve True si hubo error. False en caso contrario
        /// </summary>
        public virtual bool Error { get; protected set; }


        public virtual CompanyService g_companyService { get; set; }

        public virtual GeneralService g_generalService { get; set; }

        public virtual GeneralData g_generalData { get; set; }

        /// <summary>
        /// Si hubo error devuelve el código (Error de SBO)
        /// </summary>
        public virtual int LastErrorCode
        {
            get { return _lastErrorCode; }
            protected set { _lastErrorCode = value; }
        }

        /// <summary>
        /// Si hubo error devuelve la descripción (Error de SBO)
        /// </summary>
        public virtual string LastErrorDescription
        {
            get { return _lastErrorDescription; }
            protected set { _lastErrorDescription = value; }
        }

        /// <summary>
        /// Delegate del método para obtener la columna llave.
        /// </summary>
        public GetAutoKeyMethod GetAutoKeyMethod { get; private set; }

        /// <summary>
        /// Lee las propiedades del UDO y las asigna a un objeto de tipo <see cref="GeneralDataParams"/>
        /// </summary>
        /// <param name="generalDataParams">Objeto GeneralDataParams</param>
        /// <seealso cref="UDO.SetPropertiesEncabezado"/>
        protected virtual void SetPropertiesKeyOnly(GeneralDataParams generalDataParams)
        {
            //Obtiene las propiedades de la instancia del objeto
            PropertyInfo[] propiedadesPublicas = GetType().GetProperties();

            foreach (PropertyInfo propiedad in propiedadesPublicas)
            {
                //Por cada propiedad si implementa la interface IEncabezadoUDO entonces
                if (propiedad.PropertyType.GetInterface("IEncabezadoUDO") != null)
                {
                    //Obtiene las propiedades del objeto que implementa la interface IEncabezadoUDO
                    PropertyInfo[] propiedadesEncabezado = propiedad.PropertyType.GetProperties();

                    //Obtiene la instancia del encabezado
                    object encabezado = propiedad.GetValue(this, null);
                    object[] attributes = null;

                    //Procesa cada propiedad del encabezado
                    foreach (PropertyInfo cpi in propiedadesEncabezado)
                    {
                        attributes = cpi.GetCustomAttributes(typeof(UDOBindAttribute), true);

                        if (attributes.Length != 0)
                        {
                            var ba = (UDOBindAttribute)attributes[0];
                            if (ba.Key)
                            {
                                string field = ba.Columna;
                                object value = cpi.GetValue(encabezado, null);
                                generalDataParams.SetProperty(field, value ?? ba.ValorPredeterminado ?? string.Empty);
                            }
                        }
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Lee las propiedades del encabezado del UDO y las asigna a un objeto de tipo <see cref="GeneralData"/>
        /// </summary>
        /// <param name="generalData">Objeto GeneralData</param>
        /// <seealso cref="SetPropertiesKeyOnly"/>
        protected virtual void SetPropertiesEncabezado(ref GeneralData generalData)
        {
            //Obtiene las propiedades de la instancia del objeto
            PropertyInfo[] propiedadesPublicas = GetType().GetProperties();

            foreach (PropertyInfo propiedad in propiedadesPublicas)
            {
                //Por cada propiedad si implementa la interface IEncabezadoUDO entonces
                if (propiedad.PropertyType.GetInterface("IEncabezadoUDO") != null)
                {
                    //Obtiene las propiedades del objeto que implementa la interface IEncabezadoUDO
                    PropertyInfo[] propiedadesEncabezado = propiedad.PropertyType.GetProperties();

                    //Obtiene la instancia del encabezado
                    object encabezado = propiedad.GetValue(this, null);
                    object[] attributes = null;

                    //Procesa cada propiedad del encabezado
                    foreach (PropertyInfo cpi in propiedadesEncabezado)
                    {
                        attributes = cpi.GetCustomAttributes(typeof(UDOBindAttribute), true);

                        if (attributes.Length != 0)
                        {
                            var ba = (UDOBindAttribute)attributes[0];
                            if (!ba.SoloLectura)
                            {
                                string field = ba.Columna;
                                object value = cpi.GetValue(encabezado, null);
                                if (ba.Key && GetAutoKeyMethod != null)
                                {
                                    string autoKey = GetAutoKeyMethod(UDOId).ToString();
                                    generalData.SetProperty(field, autoKey);
                                }
                                else
                                    generalData.SetProperty(field, value ?? ba.ValorPredeterminado ?? string.Empty);
                            }
                        }
                    }
                    return;
                }
            }
        }

        protected virtual void GetPropertiesEncabezado(GeneralData generalData)
        {
            //Obtiene las propiedades de la instancia del objeto
            PropertyInfo[] propiedadesPublicas = GetType().GetProperties();

            foreach (PropertyInfo propiedad in propiedadesPublicas)
            {
                //Por cada propiedad si implementa la interface IEncabezadoUDO entonces
                if (propiedad.PropertyType.GetInterface("IEncabezadoUDO") != null)
                {
                    //Obtiene las propiedades del objeto que implementa la interface IEncabezadoUDO
                    PropertyInfo[] propiedadesEncabezado = propiedad.PropertyType.GetProperties();

                    //Obtiene la instancia del encabezado
                    object encabezado = propiedad.GetValue(this, null);
                    object[] attributes = null;

                    //Procesa cada propiedad del encabezado
                    foreach (PropertyInfo cpi in propiedadesEncabezado)
                    {
                        attributes = cpi.GetCustomAttributes(typeof(UDOBindAttribute), true);

                        if (attributes.Length != 0)
                        {
                            var ba = (UDOBindAttribute)attributes[0];

                            string field = ba.Columna;
                            object value = generalData.GetProperty(field);
                            cpi.SetValue(encabezado, value, null);
                        }
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Lee las lineas del UDO y las asigna a un objeto de tipo <see cref="GeneralData"/>
        /// </summary>
        /// <param name="generalData">Objeto GeneralData</param>
        protected virtual void SetPropertiesLineas(ref GeneralData generalData)
        {
            PropertyInfo[] propiedadesPublicas = GetType().GetProperties();
            GeneralDataCollection lineasGeneralDataCollection;

            foreach (PropertyInfo propiedad in propiedadesPublicas)
            {
                if (propiedad.PropertyType.GetInterface("ILineasUDO") != null)
                {
                    PropertyInfo[] propiedadesLineasUDO = propiedad.PropertyType.GetProperties();

                    var lineasUDO = (ILineasUDO)propiedad.GetValue(this, null);
                    if (lineasUDO == null)
                        continue;

                    lineasGeneralDataCollection = generalData.Child(lineasUDO.TablaLigada);

                    foreach (ILineaUDO lineaUDO in lineasUDO.LineasUDO)
                    {
                        GeneralData lineaGeneralData = lineasGeneralDataCollection.Add();
                        object[] attributes = null;
                        foreach (PropertyInfo lineaUDOPropertyInfo in lineaUDO.GetType().GetProperties())
                        {
                            attributes = lineaUDOPropertyInfo.GetCustomAttributes(typeof(UDOBindAttribute), true);
                            if (attributes.Length != 0)
                            {
                                var ba = (UDOBindAttribute)attributes[0];
                                if (!ba.SoloLectura)
                                {
                                    string field = ba.Columna;
                                    object value = lineaUDOPropertyInfo.GetValue(lineaUDO, null);
                                    lineaGeneralData.SetProperty(field, value ?? ba.ValorPredeterminado ?? string.Empty);
                                }
                            }
                        }
                    }
                    int i = lineasGeneralDataCollection.Count;

                    i = generalData.Child(lineasUDO.TablaLigada).Count;

                    i = i + 0;

                }

            }
        }

        protected virtual void GetPropertiesLineas(GeneralData generalData)
        {
            PropertyInfo[] propiedadesPublicas = GetType().GetProperties();
            GeneralDataCollection lineasGeneralDataCollection;
            int intContador = 0;
            foreach (PropertyInfo propiedad in propiedadesPublicas)
            {
                if (propiedad.PropertyType.GetInterface("ILineasUDO") != null)
                {
                    var lineasUDO = (ILineasUDO)propiedad.GetValue(this, null);
                    if (lineasUDO == null)
                        continue;
                    lineasGeneralDataCollection = generalData.Child(lineasUDO.TablaLigada);

                    intContador = 0;

                    foreach (ILineaUDO lineaUDO in lineasUDO.LineasUDO)
                    {
                        foreach (GeneralData lineaGeneralData in lineasGeneralDataCollection)
                        {
                            ILineaUDO objNuevaLinea;
                            objNuevaLinea = lineaUDO;
                            PropertyInfo[] lineaUDOPropertyInfo = objNuevaLinea.GetType().GetProperties();

                            object lineaEncabezado = propiedad.GetValue(this, null);
                            object[] attributes = null;

                            foreach (PropertyInfo property in lineaUDOPropertyInfo)
                            {
                                attributes = property.GetCustomAttributes(typeof(UDOBindAttribute), true);

                                if (attributes.Length != 0)
                                {
                                    var ba = (UDOBindAttribute)attributes[0];
                                    string field = ba.Columna;
                                    object value = lineaGeneralData.GetProperty(field);
                                    property.SetValue(objNuevaLinea, value, null);
                                }
                            }

                            intContador += 1;
                            lineasUDO.LineasUDO.Add(objNuevaLinea);
                        }

                        lineasUDO.LineasUDO.Remove(lineaUDO);
                        break;
                    }
                }
            }
        }

        public virtual bool Insert()
        {
            if (Company != null)
            {
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(UDOId);
                var encabezadoGeneralData =
                    (GeneralData)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                
                SetPropertiesEncabezado(ref encabezadoGeneralData);
                SetPropertiesLineas(ref encabezadoGeneralData);

                GeneralDataParams generalDataParams = generalService.Add(encabezadoGeneralData);
                if (generalDataParams == null)
                {
                    Company.GetLastError(out _lastErrorCode, out _lastErrorDescription);
                    Error = true;
                    return false;
                }
                GetKeyProperties(generalDataParams);

                Error = false;
                return true;
            }
            Error = true;
            return false;
        }

        public virtual bool Insert(Hashtable headerProperties)
        {
            if (Company != null)
            {
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(UDOId);
                var encabezadoGeneralData =
                    (GeneralData)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                SetPropertiesEncabezado(ref encabezadoGeneralData);

                foreach (DictionaryEntry de in headerProperties)
                {
                    encabezadoGeneralData.SetProperty(de.Key.ToString(), de.Value);
                }

                SetPropertiesLineas(ref encabezadoGeneralData);

                GeneralDataParams generalDataParams = generalService.Add(encabezadoGeneralData);

                if (generalDataParams == null)
                {
                    Company.GetLastError(out _lastErrorCode, out _lastErrorDescription);
                    Error = true;
                    return false;
                }
                GetKeyProperties(generalDataParams);
            }

            Error = false;
            return true;
        }

        protected virtual void GetKeyProperties(GeneralDataParams generalDataParams)
        {
            PropertyInfo[] propiedadesPublicas = GetType().GetProperties();

            foreach (PropertyInfo propiedad in propiedadesPublicas)
            {
                if (propiedad.PropertyType.GetInterface("IEncabezadoUDO") != null)
                {
                    PropertyInfo[] propiedadesEncabezado = propiedad.PropertyType.GetProperties();

                    object encabezado = propiedad.GetValue(this, null);
                    object[] attributes = null;

                    foreach (PropertyInfo cpi in propiedadesEncabezado)
                    {
                        attributes = cpi.GetCustomAttributes(typeof(UDOBindAttribute), true);

                        if (attributes.Length != 0)
                        {
                            var ba = (UDOBindAttribute)attributes[0];
                            if (ba.Key)
                            {
                                string field = ba.Columna;
                                object value = generalDataParams.GetProperty(field);
                                cpi.SetValue(encabezado, value, null);
                            }
                        }
                    }
                }
            }
        }

        public virtual bool Update()
        {
            if (Company != null)
            {
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(UDOId);
                var encabezadoGeneralData =
                    (GeneralData)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                SetPropertiesEncabezado(ref encabezadoGeneralData);
                SetPropertiesLineas(ref encabezadoGeneralData);

                generalService.Update(encabezadoGeneralData);

                Company.GetLastError(out _lastErrorCode, out _lastErrorDescription);

                Error = LastErrorCode != 0;
            }
            return Error;

        }

        /// <summary>
        /// Actualiza la línea actual del udo en la base de datos utilizando solo los valores definidos en la tabla hash
        /// </summary>
        /// <param name="headerProperties">Tabla hash en la cual se debe especificar la propiedad y el valor correspondiente</param>
        /// <returns>Retorna verdadero si la actualización se realizo de forma exitosa</returns>
        public virtual bool Update(Hashtable headerProperties)
        {
            if (Company != null)
            {
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(UDOId);
                var encabezadoGeneralDataParams =
                    (GeneralDataParams)
                    generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                SetPropertiesKeyOnly(encabezadoGeneralDataParams);
                var generalData = generalService.GetByParams(encabezadoGeneralDataParams);

                foreach (DictionaryEntry de in headerProperties)
                {
                    generalData.SetProperty(de.Key.ToString(), de.Value);
                }

                generalService.Update(generalData);

                Company.GetLastError(out _lastErrorCode, out _lastErrorDescription);
                Error = LastErrorCode != 0;
            }
            return !Error;
        }

        public virtual bool Delete()
        {
            if (Company != null)
            {
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(UDOId);
                var encabezadoGeneralDataParamas =
                    (GeneralDataParams)
                    generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                SetPropertiesKeyOnly(encabezadoGeneralDataParamas);

                generalService.Delete(encabezadoGeneralDataParamas);

                Company.GetLastError(out _lastErrorCode, out _lastErrorDescription);
                Error = LastErrorCode != 0;
            }
            return !Error;
        }

        public virtual bool Load()
        {
            if (Company != null)
            {
                g_companyService = Company.GetCompanyService();
                g_generalService = g_companyService.GetGeneralService(UDOId);
                var encabezadoGeneralDataParams =
                    (GeneralDataParams)
                    g_generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                SetPropertiesKeyOnly(encabezadoGeneralDataParams);

                g_generalData = g_generalService.GetByParams(encabezadoGeneralDataParams);

                GetPropertiesEncabezado(g_generalData);
                GetPropertiesLineas(g_generalData);
                //generalService.Update(generalData);

                //Company.GetLastError(out _lastErrorCode, out _lastErrorDescription);
                //Error = LastErrorCode != 0;
            }
            return Error;
        }

    }
}