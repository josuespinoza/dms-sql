namespace SCG.SBOFramework.DI
{
    /// <summary>
    /// Database server types.
    /// </summary>
    public enum DataBaseServerType
    {
        /// <summary>
        /// Microsoft SQL Server 2000 (Not Supported for 8.8)
        /// </summary>
        MicrosoftSql2000 = 1,
        /// <summary>
        /// DB2 (Not Supported for 8.8)
        /// </summary>
        Db2 = 2,
        /// <summary>
        /// Sybase (Not Supported for 8.8)
        /// </summary>
        Sybase = 3,
        /// <summary>
        /// Microsoft SQL Server 2005
        /// </summary>
        MicrosoftSql2005 = 4,
        /// <summary>
        /// MaxDB (Not Supported for 8.8)
        /// </summary>
        MaxDb = 5,
        /// <summary>
        /// Microsoft SQL Server 2008
        /// </summary>
        MicrosoftSql2008 = 6
    }
}