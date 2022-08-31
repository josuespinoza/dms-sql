using System;
using System.Collections.Generic;
using System.Reflection;
using DMS_Connector.Business_Logic.Queries;

namespace DMS_Connector
{
    public partial class Queries
    {
        private static bool blnIniciado;
        private static Dictionary<String, String> dQueries;
        private static QueriesRecursos queriesRecursos;

        /// <summary>
        /// Función que retorna el Query solicitada formateada al tipo de servidor conectado
        /// </summary>
        /// <param name="p_strQuery">Query solicitada</param>
        /// <returns>String con Query formateada</returns>
        public static string GetStrQueryFormat(string p_strQuery)
        {
            try
            {
                if (!blnIniciado) InicializarValores();
                p_strQuery = string.Format(Company.CompanySBO.DbServerType != SAPbobsCOM.BoDataServerTypes.dst_HANADB ? "SQL_{0}" : "HANA_{0}", p_strQuery);
                if (!dQueries.ContainsKey(p_strQuery)) AgregarNewQueries(p_strQuery);
                return dQueries[p_strQuery];
            }
            catch (Exception ex)
            {
                Helpers.ManejoErrores(new Exception(p_strQuery));
                throw;
            }
        }

        /// <summary>
        /// Retorna una Query específica
        /// </summary>
        /// <param name="p_strQuery">Query a retorar</param>
        /// <returns>String con query solicitada</returns>
        public static string GetStrSpecificQuery(string p_strQuery)
        {
            try
            {
                if (!blnIniciado) InicializarValores();
                if (!dQueries.ContainsKey(p_strQuery)) AgregarNewQueries(p_strQuery);
                return dQueries[p_strQuery].Replace("¿#?", GetStrQueryFormat("strNoLock"));
            }
            catch (Exception)
            {
                Helpers.ManejoErrores(new Exception(p_strQuery));
                throw;
            }
        }


        /// <summary>
        /// Metodo que inicializa Queries de recursos
        /// </summary>
        private static void InicializarValores()
        {
            blnIniciado = true;
            queriesRecursos = new QueriesRecursos();
            dQueries = new Dictionary<string, string>();
            foreach (PropertyInfo propertyInfo in queriesRecursos.GetType().GetProperties())
                if (!dQueries.ContainsKey(propertyInfo.Name))
                    dQueries.Add(propertyInfo.Name, propertyInfo.GetValue(queriesRecursos, null).ToString());
        }

        /// <summary>
        /// Agrega Query a utilizar al diccionario de Queries
        /// </summary>
        /// <param name="p_strKey">Query a agregar</param>
        private static void AgregarNewQueries(string p_strKey)
        {
            string strQuery = new Queries().GetType().GetField(p_strKey, BindingFlags.NonPublic | BindingFlags.Static).GetValue(new Queries()).ToString();
            dQueries.Add(p_strKey, strQuery);
        }
    }
}