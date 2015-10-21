namespace Zanshin.Domain.Extensions
{
    using System;
    using System.Threading;

    /// <summary>
    /// Class provides a nice way of obtaining a lock that will time out with a cleaner syntax than using the whole Monitor.TryEnter() method.
    /// </summary>
    /// <remarks>
    /// Adapted from Ian Griffiths article http://www.interact-sw.co.uk/iangblog/2004/03/23/locking and incorporating suggestions
    ///  by Marek Malowidzki as outlined in this blog post http://www.interact-sw.co.uk/iangblog/2004/05/12/timedlockstacktrace
    /// </remarks>
    /// <example>
    /// Instead of: <code>lock(obj)
    /// {
    /// //Thread safe operation
    /// }
    /// do this:
    /// using(TimedLock.Lock(obj))
    /// {
    /// //Thread safe operations
    /// }
    /// or this:
    /// try
    /// {
    /// TimedLock timeLock = TimedLock.Lock(obj);
    /// //Thread safe operations
    /// timeLock.Dispose();
    /// }
    /// catch(LockTimeoutException e)
    /// {
    /// Console.WriteLine("Couldn't get a lock!");
    /// StackTrace otherStack = e.GetBlockingThreadStackTrace(5000);
    /// if(otherStack == null)
    /// {
    /// Console.WriteLine("Couldn't get other stack!");
    /// }
    /// else
    /// {
    /// Console.WriteLine("Stack trace of thread that owns lock!");
    /// }
    /// }</code></example>
    public struct TimedLock : IDisposable
    {
        /// <summary>
        /// Attempts to obtain a lock on the specified object for up to 10 seconds.
        /// </summary>
        /// <param name="syncRoot">The synchronize root.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">syncRoot</exception>
        /// <exception cref="LockTimeoutException">Condition.</exception>
        /// <exception cref="OverflowException"><paramref>
        ///   <name>value</name>
        /// </paramref>
        /// is less than <see cref="F:System.TimeSpan.MinValue" />
        /// or greater than <see cref="F:System.TimeSpan.MaxValue" />.-or-<paramref><name>value</name></paramref>
        /// is <see cref="F:System.Double.PositiveInfinity" />.
        /// -or-<paramref><name>value</name></paramref>
        /// is <see cref="F:System.Double.NegativeInfinity" />.</exception>
        /// <exception cref="ArgumentNullException">The value of 'o' cannot be null.</exception>
        public static TimedLock Lock(object syncRoot)
        {
            if (syncRoot == null)
            {
                throw new ArgumentNullException("syncRoot");
            }
            return Lock(syncRoot, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Attempts to obtain a lock on the specified object for up to the specified timeout.
        /// </summary>
        /// <param name="syncRoot">The synchronize root.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">syncRoot</exception>
        /// <exception cref="LockTimeoutException"></exception>
        /// <exception cref="LockTimeoutException">Condition.</exception>
        /// <exception cref="ArgumentNullException">The value of 'syncRoot' cannot be null.</exception>
        public static TimedLock Lock(object syncRoot, TimeSpan timeout)
        {
            if (syncRoot == null)
            {
                throw new ArgumentNullException("syncRoot");
            }
            Thread.BeginCriticalRegion();
            var tl = new TimedLock(syncRoot);
            if (!Monitor.TryEnter(syncRoot, timeout))
            {
                // Failed to acquire lock.
#if DEBUG
                GC.SuppressFinalize(tl.leakDetector);
                throw new LockTimeoutException();
#else
                throw new LockTimeoutException();
#endif
            }
            return tl;
        }

        private TimedLock(object syncRoot)
        {
            this.target = syncRoot;
#if DEBUG
            this.leakDetector = new Sentinel();
#endif
        }

        private readonly object target;

        /// <summary>
        /// Disposes of this lock.
        /// </summary>
        /// <exception cref="SynchronizationLockException">The current thread does not own the lock for the specified object.</exception>
        public void Dispose()
        {
            // Owning thread is done.
#if DEBUG
            try
            {
                // This shouldn't throw an exception.
                LockTimeoutException.ReportStackTraceIfError(this.target);
            }
            finally
            {
                // But just in case...
                Monitor.Exit(this.target);
            }
#else
            Monitor.Exit(target);
#endif
#if DEBUG
            // It's a bad error if someone forgets to call Dispose,
            // so in Debug builds, we put a finalizer in to detect
            // the error. If Dispose is called, we suppress the
            // finalizer.
            GC.SuppressFinalize(this.leakDetector);
#endif
            Thread.EndCriticalRegion();
        }

#if DEBUG
        // (In Debug mode, we make it a class so that we can add a finalizer
        // in order to detect when the object is not freed.)
        private sealed class Sentinel
        {
            ~Sentinel()
            {
                // If this finalizer runs, someone somewhere failed to
                // call Dispose, which means we've failed to leave a monitor!
                // System.Diagnostics.Debug.Fail("Undisposed lock");
                throw new UndisposedLockException("Undisposed lock");
            }
        }

        private readonly Sentinel leakDetector;
#endif
    }

    public sealed class UndisposedLockException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndisposedLockException"/> class.
        /// </summary>
        /// <param name="undisposedLock">The undisposed lock.</param>
        public UndisposedLockException(string undisposedLock)
            : base(undisposedLock)
        {
        }
    }

    public sealed class LockTimeoutException : ApplicationException
    {
        /// <summary>
        /// Reports the stack trace if error.
        /// </summary>
        /// <param name="target">The target.</param>
        public static void ReportStackTraceIfError(object target)
        {
        }
    }
}