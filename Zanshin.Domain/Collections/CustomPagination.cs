
namespace Zanshin.Domain.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Zanshin.Domain.Exceptions;
    using Zanshin.Domain.Extensions;
    using Zanshin.Domain.Factories;

    /// <summary>
    /// A collection of objects that has been split into pages.
    /// </summary>
    public interface IPagination : IEnumerable
    {
        /// <summary>
        /// The current page number
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// The number of items in each page.
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// The total number of items.
        /// </summary>
        int TotalItems { get; }

        /// <summary>
        /// The total number of pages.
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// The index of the first item in the page.
        /// </summary>
        int FirstItem { get; }

        /// <summary>
        /// The index of the last item in the page.
        /// </summary>
        int LastItem { get; }

        /// <summary>
        /// Whether there are pages before the current page.
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Whether there are pages after the current page.
        /// </summary>
        bool HasNextPage { get; }
    }

    /// <summary>
    /// Generic form of <see cref="IPagination"/>
    /// </summary>
    /// <typeparam name="T">Type of object being paged</typeparam>
    public interface IPagination<out T> : IPagination, IEnumerable<T>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CustomPagination<T> : BasePagination, IPagination<T>
    {
        [NonSerialized]
        private readonly object syncRoot = new object();

        /// <summary>The default page size</summary>
        public const int DefaultPageSize = 10;
        /// <summary>The default page array size</summary>
        public const int DefaultPageArraySize = 11;
        private int pageArraySize;
        private readonly List<T> currentPage;

        [NonSerialized]
        [JsonIgnore]
        private readonly IList<T> dataSource;

        private int pageSize;
        private int pageNumber;
        private string[] pageArray;
        private int firstItem;
        private int lastItem;
        private bool hasPreviousPage;
        private bool hasNextPage;
        private int totalPages;
        private int startingIndex;
        private readonly bool useDataSourceAsCurrentPage;

        [NonSerialized]
        [JsonIgnore]
        private readonly PageArrayBuilder<T> pageArrayBuilder;


        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPagination&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        public CustomPagination(ICollection<T> dataSource)
            : this(dataSource, 1, DefaultPageSize, dataSource.Count(), false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPagination&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <exception cref="TypeInitializationException">new ParameterNullException(dataSource)</exception>
        public CustomPagination(ICollection<T> dataSource, int pageNumber)
            : this(dataSource, pageNumber, DefaultPageSize, dataSource.Count(), false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPagination&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <exception cref="TypeInitializationException">new ParameterNullException(dataSource)</exception>
        public CustomPagination(ICollection<T> dataSource, int pageNumber, int pageSize)
            : this(dataSource, pageNumber, pageSize, dataSource.Count(), false)
        {
        }

        /// <summary>
        ///   Creates a new instance of CustomPagination
        /// </summary>
        /// <param name="dataSource"> A pre-paged slice of data </param>
        /// <param name="pageNumber"> The current page number </param>
        /// <param name="pageSize"> The number of items per page </param>
        /// <param name="totalItems"> The total number of items in the overall datasource </param>
        /// <exception cref="TypeInitializationException">new ParameterNullException(dataSource)</exception>
        public CustomPagination(ICollection<T> dataSource, int pageNumber, int pageSize, int totalItems)
            : this(dataSource, pageNumber, pageSize, totalItems, false)
        {
        }

        /// <summary>
        /// Creates a new instance of CustomPagination
        /// </summary>
        /// <param name="dataSource">A pre-paged slice of data</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per page</param>
        /// <param name="totalItems">The total number of items in the overall datasource</param>
        /// <param name="useDataSourceAsCurrentPage">if set to <c>true</c> [use data source as current page].</param>
        /// <exception cref="System.TypeInitializationException">new ParameterNullException(dataSource)</exception>
        /// <exception cref="ParameterNullException">dataSource</exception>
        public CustomPagination(ICollection<T> dataSource, int pageNumber,
            int pageSize, int totalItems, bool useDataSourceAsCurrentPage)
        {
            if (dataSource == null)
            {
                throw new TypeInitializationException(this.GetType().ToString(),
                    new ParameterNullException("dataSource"));
            }

            this.dataSource = dataSource.ToList();
            this.TotalItems = totalItems;
            this.pageNumber = pageNumber;
            this.pageSize = pageSize > totalItems ? totalItems : pageSize;
            this.currentPage = new List<T>();
            this.useDataSourceAsCurrentPage = useDataSourceAsCurrentPage;

            this.CalculateTotalPages();

            // make sure that array of links (not the number of links per page, but the number of links to pages
            // ie 1 _ 2 _ 3 _ 4  shown on the bottom for paging, is not greater than our total pages.
            this.pageArraySize = this.totalPages > DefaultPageArraySize ? DefaultPageArraySize : this.totalPages;
            this.pageArrayBuilder = PageArrayBuilder<T>.Create(this);
            this.BeginCreatePageArray();
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection. </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.dataSource.GetEnumerator();
        }

        /// <summary>
        ///   Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns> An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection. </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <value>
        /// The item
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public T this[int index]
        {
            get { return this.dataSource[index]; }
        }

        /// <summary>
        /// The current page number
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        /// <remarks>
        /// When setting the page number value, if the specified value is greater than the total 
        /// number of pages, then the total page number is used. If the specified value is less than one, then one is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException" accessor="set">syncRoot</exception>
        /// <exception cref="LockTimeoutException" accessor="set">Condition.</exception>
        [DataMember(Name = "PageNumber")]
        public override int PageNumber
        {
            get { return this.pageNumber; }
            set
            {
                using (TimedLock.Lock(this.syncRoot))
                {
                    this.pageNumber = value > this.TotalPages ? this.TotalPages : value < 1 ? 1 : value;

                    // clear any page array we have this will cause the pageBuilder's build array method to be
                    // invoked if the PageArray getter in this object is requested.
                    this.pageArray = null;
                }
            }
        }

        /// <summary>
        ///   Gets the index of the first item at the current page number. This is based upon the page size.
        /// </summary>
        /// <value> The index of the starting. </value>
        [DataMember(Name = "StartingIndex")]
        public int StartingIndex
        {
            get
            {
                this.SetStartingIndex();
                return this.startingIndex;
            }
            set { this.startingIndex = value; }
        }

        /// <summary>
        ///   Gets the current page.
        /// </summary>
        /// <value> The current page. </value>
        public T[] CurrentPage
        {
            get
            {
                this.SetCurrentPage();

                return this.currentPage.ToArray();
            }
            set
            {
                foreach (var t in value)
                {
                    this.currentPage.Add(t);
                }
            }
        }

        /// <summary>
        ///   The number of items in each page.
        /// </summary>
        /// <value> </value>
        /// <remarks>
        ///   If page size is not set then the <c>DefaultPageSize</c> (10) is returned.
        /// </remarks>
        [DataMember(Name = "PageSize")]
        public int PageSize
        {
            get { return this.pageSize > 0 ? this.pageSize : DefaultPageSize; }
            set
            {
                // do nothing if the value isn't changed.
                if (value == this.pageSize)
                {
                    return;
                }

                if ((value > this.TotalItems) || (value <= 0))
                {
                    this.pageSize = this.TotalItems;
                }
                else
                {
                    this.pageSize = value;
                }

                // need to reset the total pages.
                this.CalculateTotalPages();
            }
        }

        /// <summary>
        ///   The total number of items.
        /// </summary>
        /// <value> </value>
        [DataMember(Name = "TotalItems")]
        public int TotalItems { get; set; }


        /// <summary>
        ///   The total number of pages.
        /// </summary>
        /// <value> </value>
        /// <remarks>
        ///   The setter is only on this property to allow the <see cref="DataContractSerializer" /> to serialize this object. If the setter is explicitly used in consumers, then all bets are off!
        /// </remarks>
        [DataMember(Name = "TotalPages")]
        public override int TotalPages
        {
            get { return this.totalPages; }
            set { this.totalPages = value; }
        }

        /// <summary>
        ///   The index of the first item in the page.
        /// </summary>
        /// <value> </value>
        /// <exception cref="ArgumentNullException" accessor="get">syncRoot</exception>
        /// <exception cref="LockTimeoutException" accessor="get">Condition.</exception>
        [DataMember(Name = "FirstItem"), Obsolete("Use 'StartingIndex' to get the first paged item.")]
        public int FirstItem
        {
            get
            {
                this.firstItem = ((this.PageNumber - 1) * this.PageSize) + 1;
                return this.firstItem;
            }
            set { this.firstItem = value; }
        }

        /// <summary>
        ///   The index of the last item in the page.
        /// </summary>
        /// <value> </value>
        /// <remarks>
        ///   This property is NOT returning the value one might expect.
        /// </remarks>
        [DataMember(Name = "LastItem")]
        public int LastItem
        {
            get
            {
                this.lastItem = this.StartingIndex + this.dataSource.Count - 1;

                //this.lastItem = this.FirstItem + this.dataSource.Count - 1;
                return this.lastItem;
            }
            set { this.lastItem = value; }
        }

        /// <summary>
        ///   Whether there are pages before the current page.
        /// </summary>
        /// <value> </value>
        [DataMember(Name = "HasPreviousPage")]
        public bool HasPreviousPage
        {
            get
            {
                this.hasPreviousPage = this.pageNumber > 1;
                return this.hasPreviousPage;
            }
            set { this.hasPreviousPage = value; }
        }

        /// <summary>
        ///   Whether there are pages after the current page.
        /// </summary>
        /// <value> </value>
        [DataMember(Name = "HasNextPage")]
        public bool HasNextPage
        {
            get
            {
                this.hasNextPage = this.pageNumber < this.TotalPages;
                return this.hasNextPage;
            }
            set { this.hasNextPage = value; }
        }

        /// <summary>
        ///   Gets the page array.
        /// </summary>
        /// <value> The page array. </value>
        /// <exception cref="ArgumentNullException" accessor="get">syncRoot</exception>
        /// <exception cref="LockTimeoutException" accessor="get">Condition.</exception>
        /// <exception cref="OverflowException" accessor="get"><paramref>
        ///   <name>value</name>
        /// </paramref>
        /// is less than <see cref="F:System.TimeSpan.MinValue" />
        /// or greater than <see cref="F:System.TimeSpan.MaxValue" />.-or-<paramref><name>value</name></paramref>
        /// is <see cref="F:System.Double.PositiveInfinity" />.
        /// -or-<paramref><name>value</name></paramref>
        /// is <see cref="F:System.Double.NegativeInfinity" />.</exception>
        [DataMember(Name = "PageArray")]
        public string[] PageArray
        {
            get
            {
                using (TimedLock.Lock(this.syncRoot))
                {
                    this.EnsurePageArray();
                    return this.pageArray;
                }
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
        [DataMember(Name = "PageArraySize")]
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
                    this.pageArraySize = DefaultPageArraySize <= this.TotalPages ? DefaultPageArraySize : this.TotalPages;
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
        ///   Gets the page array builder.
        /// </summary>
        /// <value> The page array builder. </value>
        internal PageArrayBuilder<T> PageArrayBuilder
        {
            get { return this.pageArrayBuilder; }
        }

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
            return this.pageArrayBuilder.BuildPageArray().ToArray();
        }

        // only need to do this if pagesize changes.
        private void CalculateTotalPages()
        {
            // avoid divide by zero
            this.totalPages = this.pageSize <= 0 ? 1 : (int)Math.Ceiling(((double)this.TotalItems) / this.pageSize);
        }

        private void SetCurrentPage()
        {
            // this will be the starting point into the collection
            // for our current page.
            int start = this.StartingIndex;
            int end = this.StartingIndex + this.pageSize;

            while ((start < end) && (start < this.dataSource.Count()))
            {
                this.currentPage.Add(this.dataSource[start]);
                start++;
            }
        }

        private void SetStartingIndex()
        {
            if (this.useDataSourceAsCurrentPage)
            {
                this.startingIndex = 0;
            }
            else
            {
                this.startingIndex = this.pageNumber > 1 ? ((this.pageNumber - 1) * this.pageSize) : 0;
            }
        }

        private void BeginCreatePageArray()
        {
            // now create a builder, we could just call the method already on the class,
            // but since it is now virtual, we want to avoid a virtual member call in the constructor.
            Func<IEnumerable<string>> builder = this.BuildPageArray;
            builder.BeginInvoke(this.BuildPageArrayComplete, null);
        }


        // TODO use async await
        private void BuildPageArrayComplete(IAsyncResult iar)
        {
            if (iar == null)
            {
                throw new ArgumentNullException("iar");
            }

            AsyncResult ar = (AsyncResult)iar;
            Func<IEnumerable<string>> builder = (Func<IEnumerable<string>>)ar.AsyncDelegate;
            this.pageArray = (string[])builder.EndInvoke(iar);
        }

    }
}