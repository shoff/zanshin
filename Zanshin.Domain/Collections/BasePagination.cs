namespace Zanshin.Domain.Collections
{
    using System.Runtime.Serialization;

    /// <summary></summary>
    [DataContract(Namespace = "")]
    public abstract class BasePagination 
    {
        /// <summary>
        ///   Gets or sets the total pages.
        /// </summary>
        /// <value> The total pages. </value>
        [DataMember]
        public abstract int TotalPages { get; set; }

        /// <summary>
        ///   Gets or sets the size of the page array.
        /// </summary>
        /// <value> The size of the page array. </value>
        [DataMember]
        public abstract int PageArraySize { get; set; }

        /// <summary>
        ///   Gets or sets the page number.
        /// </summary>
        /// <value> The page number. </value>
        [DataMember]
        public abstract int PageNumber { get; set; }
    }
}