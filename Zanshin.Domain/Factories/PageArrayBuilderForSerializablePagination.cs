namespace Zanshin.Domain.Factories
{
    using System.Collections.Generic;
    using System.Globalization;

    public sealed class PageArrayBuilderForSerializablePagination<T>
    {
        private int boundSize;
        private bool allowCenter;
        private readonly List<string> errorMessages = new List<string>();
        private int totalPages;
        private int pageArraySize;
        private int pageNumber;

        /// <summary>
        ///   Initializes a new instance of the <see cref="PageArrayBuilder&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="totalPages"> The total pages. </param>
        /// <param name="pageArraySize"> The page link count. </param>
        /// <param name="pageNumber"> The current page. </param>
        public PageArrayBuilderForSerializablePagination(int totalPages, int pageArraySize, int pageNumber)
        {
            // avoid demeter
            this.totalPages = totalPages;
            this.pageArraySize = pageArraySize;
            this.pageNumber = pageNumber;
            this.CalculateBounds();
        }

        /// <summary>
        ///   Creates the specified custom pagination.
        /// </summary>
        /// <param name="totalPages"> The total pages. </param>
        /// <param name="pageArraySize"> The page link count. </param>
        /// <param name="pageNumber"> The current page. </param>
        /// <returns> </returns>
        internal static PageArrayBuilderForSerializablePagination<T> Create(int totalPages, int pageArraySize, int pageNumber)
        {
            return new PageArrayBuilderForSerializablePagination<T>(totalPages, pageArraySize, pageNumber);
        }

        /// <summary>
        /// Pages the size changed.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">The total pages.</param>
        /// <param name="pageArraySize">Size of the page array.</param>
        /// <param name="currentPage">The current page.</param>
        public void PageSizeChanged(int pageSize = 0, int totalPages = 0, int pageArraySize = 0, int currentPage = 0)
        {
            if (currentPage > 0)
            {
                this.pageNumber = currentPage;
            }

            if (pageArraySize > 0)
            {
                this.pageArraySize = pageArraySize;
                // and then we need to recalculate the bounds.
                this.CalculateBounds();
            }

            if (totalPages > 0)
            {
                this.totalPages = totalPages;
            }
        }

        private void CalculateBounds()
        {
            // so our totalLinks is set by the caller as either a variable, or if none was given
            // then the default would be used. If the default is more than the total pages, 
            // then the total pages are used.
            this.boundSize = (this.pageArraySize / 2);

            // if even don't put in center otherwise do, we need to know how to divide up 
            // the lower and upper boundaries of the array.
            this.allowCenter = this.pageArraySize % 2 != 0;
        }

        /// <summary>
        ///   Gets the error messages.
        /// </summary>
        /// <value> The error messages. </value>
        public IEnumerable<string> ErrorMessages
        {
            get { return this.errorMessages; }
        }

        /// <summary>
        ///   Gets the size of the bound.
        /// </summary>
        /// <value> The size of the bound. </value>
        internal int BoundSize
        {
            get { return this.boundSize; }
        }

        /// <summary>
        ///   Gets a value indicating whether [in lower bounds].
        /// </summary>
        /// <value> <c>true</c> if [in lower bounds]; otherwise, <c>false</c> . </value>
        internal bool InLowerBounds
        {
            get { return this.pageNumber <= this.boundSize; }
        }

        /// <summary>
        ///   Gets a value indicating whether [in upper bounds].
        /// </summary>
        /// <value> <c>true</c> if [in upper bounds]; otherwise, <c>false</c> . </value>
        internal bool InUpperBounds
        {
            get { return this.pageNumber >= (this.totalPages - this.boundSize); }
        }

        /// <summary>
        ///   Builds the page array.
        /// </summary>
        /// <returns> </returns>
        public IEnumerable<string> BuildPageArray()
        {
            if (this.pageArraySize == 1)
            {
                // just return it
                return new[] { "1" };
            }

            // so if this is true, then our building just became VERY easy,
            // otherwise we run through the whole array assembly.
            return this.totalPages == this.pageArraySize ?
                this.CreateSimpleLinkArray() : this.AssembleArray();
        }

        private List<string> AssembleArray()
        {
            List<string> pageNumbers = new List<string>();

            var lower = this.BuildLowerBounds();

            var upper = this.BuildUpperBounds();

            foreach (var x in lower)
            {
                pageNumbers.Add(x.ToString(CultureInfo.InvariantCulture));
            }

            // this is only true if we have an odd number.
            if (this.allowCenter)
            {
                pageNumbers.Add(((lower[this.boundSize - 1]) + 1).ToString(CultureInfo.InvariantCulture));
            }

            foreach (var x in upper)
            {
                if (x <= this.totalPages)
                {
                    pageNumbers.Add(x.ToString(CultureInfo.InvariantCulture));
                }
            }

            return pageNumbers;
        }

        private List<string> CreateSimpleLinkArray()
        {
            List<string> pageNumbers = new List<string>();

            for (int i = 1; i <= this.totalPages; i++)
            {
                pageNumbers.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            return pageNumbers;
        }

        /// <summary>
        /// Builds the lower bounds.
        /// </summary>
        /// <returns></returns>
        internal List<int> BuildLowerBounds()
        {
            if (this.InLowerBounds)
            {
                // then the current page is within the the lower bounds
                // if it equals this.boundSize then  it's at the spot just 
                // to the right of lower bounds.
                return this.BuildCurrentInLower();
            }

            if (this.InUpperBounds)
            {
                // not in lowerbounds as proved by if starting up there ^
                // our starting is determined by the size of our lower array  - the page number
                // so figure out how far into the upper bound we are
                return this.BuildLowerWithCurrentInUpper();
            }

            // ok not in upper or lower bounds so just build the array starting at 
            // PageNumber - boundSize +1
            List<int> lower = new List<int>();
            var x = this.pageNumber - this.boundSize;
            if (x < 1)
            {
                x = 1;
            }

            for (int i = 0; i < this.boundSize; i++, x++)
            {
                lower.Add(x);
            }
            return lower;
        }

        // in upper means that we are near or at our last page.
        private List<int> BuildLowerWithCurrentInUpper()
        {
            List<int> lower = new List<int>();
            // subtract the difference +6 from starting and that becomes our 
            // actual starting point
            // ok so upper bound start lets say we had 72 pages and our totalLinks == 11.
            // our array should look like  
            // lower:  62 63 64 65 66
            // center: 67
            // upper:  68 69 70 71 72
            // 1  2  3  4  5  6  7  8  9  10 11
            // 62 63 64 65 66 67 68 69 70 71 72

            var start = (this.totalPages - this.pageArraySize) + 1; // should be 62
            int end = start + this.boundSize;
            for (; start < end; start++)
            {
                lower.Add(start);
            }
            return lower;
        }

        private List<int> BuildCurrentInLower()
        {
            List<int> lower = new List<int>();

            for (int i = 1; i <= this.boundSize; i++)
            {
                lower.Add(i);
            }
            return lower;
        }

        /// <summary>
        /// Builds the upper bounds.
        /// </summary>
        /// <returns></returns>
        internal List<int> BuildUpperBounds()
        {
            // so if we have 890 total pages and we have a default array size then upperBoundsStart would be 885
            // meaning if our current page is 885 or greater, we're in the upper bound
            List<int> upper = new List<int>();

            // do last page
            if (this.InUpperBounds)
            {
                int start = this.totalPages - (this.boundSize - 1);
                int end = start + this.boundSize;

                // changed this from PageArraySize to TotalPages to PageArraySize back to TotalPages 
                // if its fubar its heidi's fault.
                for (; start < end; start++)
                {
                    upper.Add(start);
                }
            }
            else if (this.InLowerBounds)
            {
                // OK the current page is in the lower bounds.
                // So all we have to do is grab the last item of
                // lowerBounds add 2 and that's our starting location
                // for upper

                int starting = this.boundSize + (this.allowCenter ? 2 : 1);

                // this.lowerBounds.FindLast(y => y > 0) + (this.allowCenter ? 2 : 1);

                int lastItemInUpperBounds = starting + this.boundSize;

                for (; starting < lastItemInUpperBounds; starting++)
                {
                    upper.Add(starting);
                }
            }
            else
            {
                // this should be the most commonly called branch
                int x = this.pageNumber + 1;
                for (int i = 0; i < this.boundSize; i++, x++)
                {
                    upper.Add(x);
                }
            }
            return upper;
        }
    }
}