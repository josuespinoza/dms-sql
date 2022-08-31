namespace SCG.SBOFramework.DI
{
    public enum UserTableType
    {
        /// <summary>
        /// A No Object type refers to a user table that cannot be linked to a user defined object. The default table includes Code and 
        /// Name fields only.
        /// </summary>
        NoObject,
        /// <summary>
        /// A Master Data type table refers to a collection of information about a person or an object, such as a cost object, business 
        /// partner, or G/L account. For example, a business partner master record contains not only general information such as the 
        /// business partner's name and address, but also specific information, such as payment terms and delivery instructions. 
        /// Generally for end-users, master data is reference data that you will look up and use, but not create or change. 
        /// </summary>
        MasterData,
        /// <summary>
        /// A Master Data Lines type refers as a child of Master Data type. For example, list of addresses related to a business partner.
        /// </summary>
        MasterDataLines,
        /// <summary>
        /// A Document type table refers to transactional data, which is data related to a single business event such as a purchase
        /// requisition or a request for payment. When you create a requisition, for example, SAP creates an electronic document for that
        /// particular transaction. SAP gives the transaction a document number and adds the document to the transaction data that is 
        /// already in the system. Whenever you complete a transaction in SAP, that is, when you create, change, or print a document in 
        /// SAP, this document number appears at the bottom of the screen.
        /// </summary>
        Document,
        /// <summary>
        /// A Document Lines type refers as a child of Document type. For example, Content tab in Invoice document.
        /// </summary>
        DocumentLines
    }
}