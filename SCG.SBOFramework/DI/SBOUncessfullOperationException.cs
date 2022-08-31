using System;

namespace SCG.SBOFramework.DI
{
    /// <summary>
    /// The Exception that occurs when a SBO method did not succesfully execute.
    /// </summary>
    public class SboUncessfullOperationException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the SBOUncessfullOperationException class
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="description">Error description</param>
        /// <param name="methodName">Method name that was executed</param>
        public SboUncessfullOperationException(int code, string description, string methodName) : base(description)
        {
            Code = code;
            MethodName = methodName;
        }

        /// <summary>
        /// Initializes a new instance of the SBOUncessfullOperationException class
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="description">Error description</param>
        /// <param name="methodName">Method name that was executed</param>
        /// <param name="innerException">Inner exception</param>
        public SboUncessfullOperationException(int code, string description, string methodName, Exception innerException)
            : base(description, innerException)
        {
            Code = code;
            MethodName = methodName;
        }

        /// <summary>
        /// Com Error Code
        /// </summary>
        public int Code { get; protected set; }

        /// <summary>
        /// Name of the method that has thrown the Exception
        /// </summary>
        public string MethodName { get; set; }

    }
}