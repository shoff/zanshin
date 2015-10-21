namespace Zanshin.Domain.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using Newtonsoft.Json;

    using Zanshin.Domain.Exceptions;
    using Zanshin.Domain.Factories;

    /// <summary></summary>
    /// <typeparam name="T"></typeparam>
    [DataContract(Namespace = "")]
    public sealed class SerializablePagination<T> : BasePagination
    {

        [NonSerialized, JsonIgnore]
        private readonly IList<T> dataSource;
        private string[] pageArray;
        [NonSerialized, JsonIgnore]
        private PageArrayBuilderForSerializablePagination<T> pageArrayBuilder;
        private int pageArraySize;
        private int pageSize;
        private int pageNumber;
        private int startingIndex;
        //private int totalPages;

        /// <summary>
        ///   Initializes a new instance of the <see cref="SerializablePagination&lt;T&gt;" /> class.
        /// </summary>
        public SerializablePagination()
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CustomPagination&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="dataSource"> The data source. </param>
        public SerializablePagination(ICollection<T> dataSource)
            : this(dataSource, 1, 10)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CustomPagination&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="dataSource"> The data source. </param>
        /// <param name="pageNumber"> The page number. </param>
        public SerializablePagination(ICollection<T> dataSource, int pageNumber)
            : this(dataSource, pageNumber, 10)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CustomPagination&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="dataSource"> The data source. </param>
        /// <param name="pageNumber"> The page number. </param>
        /// <param name="pageSize"> Size of the page. </param>
        public SerializablePagination(ICollection<T> dataSource, int pageNumber, int pageSize)
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            // NOTE we don't check for an empty collection to allow for a default parameterless constructor
            if (dataSource == null)
            {
                throw new TypeInitializationException(this.GetType().ToString(), new ParameterNullException("dataSource"));
            }
            this.CurrentPage = new List<T>();
            this.dataSource = dataSource.ToList();
            this.TotalItems = this.dataSource.Count();

            // DO NOT CHANGE THIS ORDER
            this.pageSize = this.ValidatePageSize(pageSize, this.TotalItems);
            this.TotalPages = this.CalculateTotalPages();
            this.PageNumber = this.ValidatePageNumber(pageNumber);
            this.SetupCurrentPage();
            this.HasNextPage = this.PageNumber < this.TotalPages;
            this.HasPreviousPage = this.PageNumber > 1;
            // make sure that array of links (not the number of links per page, but the number of links to pages
            // ie 1 _ 2 _ 3 _ 4  shown on the bottom for paging, is not greater than our total pages.
            this.pageArraySize = this.TotalPages > 11 ? 11 : this.TotalPages;

            this.EnsurePageArrayBuilder();

            this.BuildPageArray();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        ///   Gets the index of the first item at the current page number. This is based upon the page size.
        /// </summary>
        /// <value> The index of the starting. </value>
        [DataMember]
        [JsonProperty("startingIndex")]
        public int StartingIndex
        {
            get
            {
                this.startingIndex = this.PageNumber > 1 ? ((this.PageNumber - 1) * this.pageSize) : 0;
                return this.startingIndex;
            }
            set { this.startingIndex = value; }
        }

        /// <summary>
        ///   Gets the page array.
        /// </summary>
        /// <value> The page array. </value>
        [DataMember]
        [JsonProperty("pageArray")]
        public string[] PageArray
        {
            get
            {
                this.EnsurePageArray();
                return this.pageArray;
            }
            set { this.pageArray = value; }
        }

        /// <summary>
        ///   Gets or sets the size of the page array.
        /// </summary>
        /// <value> The size of the page array. </value>
        /// <remarks>
        ///   If you override the setter of this property in a derived class, then you MUST either recreate the logic in this base class or ensure that call base.PageArraySize = x in your derived class, failure to do so <b>WILL</b> result in unpredictable results.
        /// </remarks>
        [DataMember]
        [JsonProperty("pageArraySize")]
        public override int PageArraySize
        {
            get
            {
                // if the pageArraySize is 0 then it has not been set, so use the default. Otherwise,
                // use the one provided.
                if (this.pageArraySize == 0)
                {
                    // ok pageArraySize hasn't been set for some reason.
                    // make sure that default page array size is less than or equal to totalPages
                    this.pageArraySize = 11 <= this.TotalPages ? 11 : this.TotalPages;
                }
                return this.pageArraySize;
            }
            set
            {
                if (value == this.pageArraySize)
                {
                    // do nothing as nothing has changed.
                    return;
                }

                // do the easiest most obvious first, if it's less than or equal to the 
                // total pages, then accept it.
                if (value <= this.TotalPages)
                {
                    this.pageArraySize = value;
                    return;
                }
                // ok so to get here simply means that it is greater than total pages, 
                // so simply use the total pages as the page array size.
                this.pageArraySize = this.TotalPages;
            }
        }

        /// <summary>
        ///   The current page number
        /// </summary>
        /// <value> </value>
        /// <remarks>
        ///   When setting the page number value, if the specified value is greater than the total number of pages, then the total page number is used. If the specified value is less than one, then one is used.
        /// </remarks>
        [DataMember]
        [JsonProperty("pageNumber")]
        public override int PageNumber
        {
            get { return this.pageNumber; }
            set
            {
                this.pageNumber = value > this.TotalPages ? this.TotalPages : value < 1 ? 1 : value;

                // clear any page array we have this will cause the pageBuilder's build array method to be
                // invoked if the PageArray getter in this object is requested.
                this.pageArray = null;
                this.EnsurePageArrayBuilder();
                this.pageArrayBuilder.PageSizeChanged(currentPage: this.pageNumber);
            }
        }

        /// <summary>
        ///   Gets the current page.
        /// </summary>
        /// <value> The current page. </value>
        [DataMember]
        public List<T> CurrentPage { get; set; }

        /// <summary>
        ///   The number of items in each page.
        /// </summary>
        /// <value> </value>
        /// <remarks>
        ///   If page size is not set then the <c>DefaultPageSize</c> (10) is returned.
        /// </remarks>
        [DataMember]
        [JsonProperty("pageSize")]
        public int PageSize
        {
            get { return this.pageSize; }
            set
            {
                // do nothing if the value isn't changed.
                if (value == this.pageSize)
                {
                    return;
                }

                this.pageSize = this.ValidatePageSize(value, this.TotalItems);

                // need to reset the total pages.
                this.CalculateTotalPages();
            }
        }

        /// <summary>
        ///   The total number of items.
        /// </summary>
        /// <value> </value>
        [DataMember]
        public int TotalItems { get; set; }
        
        /// <summary>
        ///   The total number of pages.
        /// </summary>
        /// <value> </value>
        /// <remarks>
        ///   The setter is only on this property to allow the <see cref="DataContractSerializer" />
        ///  to serialize this object. If the setter is explicitly used in consumers, then all bets are off!
        /// </remarks>
        [DataMember]
        [JsonProperty("totalPages")]       
        public override int TotalPages
        {
            get { return this.CalculateTotalPages(); }
            set {}
        }

        /// <summary>
        ///   Whether there are pages before the current page.
        /// </summary>
        /// <value> </value>
        [DataMember]
        public bool HasPreviousPage { get; set; }

        /// <summary>
        ///   Whether there are pages after the current page.
        /// </summary>
        /// <value> </value>
        [DataMember]
        public bool HasNextPage { get; set; }

        /// <summary>
        ///   Ensures the page array.
        /// </summary>
        private void EnsurePageArray()
        {
            if (this.pageArray == null)
            {
                this.pageArray = this.BuildPageArray();
            }
        }

        /// <summary>
        ///   Builds the page array.
        /// </summary>
        /// <returns> </returns>
        private string[] BuildPageArray()
        {
            this.EnsurePageArrayBuilder();
            return this.pageArrayBuilder.BuildPageArray().ToArray();
        }

        // only need to do this if pagesize changes.
        private int CalculateTotalPages()
        {
            // avoid divide by zero
            return this.pageSize <= 0 ? 1 : (int)Math.Ceiling(((double)this.TotalItems) / this.pageSize);
        }

        private int ValidatePageNumber(int value)
        {
            return value > this.TotalPages ? this.TotalPages : value < 1 ? 1 : value;
        }

        private int ValidatePageSize(int value, int dataCount)
        {
            int tempPageSize = value > dataCount ? dataCount : value;
            return tempPageSize <= -1 ? 0 : tempPageSize;
        }

        private void SetupCurrentPage()
        {
            int start = this.StartingIndex;
            int end = this.StartingIndex + this.pageSize;

            while ((start < end) && (start < this.TotalItems))
            {
                var source = this.dataSource[start];

                this.CurrentPage.Add(source);
                start++;
            }
        }

        private void EnsurePageArrayBuilder()
        {
            if (this.pageArrayBuilder == null)
            {
                this.pageArrayBuilder = new PageArrayBuilderForSerializablePagination<T>(this.TotalPages, this.PageArraySize,
                    this.PageNumber);
            }
        }
    }
}