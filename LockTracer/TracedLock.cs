using System;
using System.Threading;

namespace LockTracer
{
	public class TracedLock : IDisposable
	{
		private readonly TracedLockStatus _status;
		private readonly TraceableLock _traceableLock;

		internal TraceableLock TraceableLock { get { return _traceableLock; }}

		public static TracedLock Lock(TraceableLock traceableLock, TimeSpan? maxWaitTime = null)
		{
			return new TracedLock(traceableLock, maxWaitTime);
		}

		public static TraceableLock Register(string name)
		{
			return new TraceableLock(name);
		}
		
		public TracedLock(TraceableLock traceableLock, TimeSpan? timeout)
		{
			if (timeout == null)
				timeout = TimeSpan.FromSeconds(30);

			_status = TracedLockStatus.Acquiring; //useful for detecting dead-lock
			_traceableLock = traceableLock;

			LockAnalyzer.Lock(traceableLock);

			//collect useful information about the context such 
			//as stacktrace, time to acquire the lock(T1)
			if (Monitor.TryEnter(_traceableLock, timeout.Value))
			{
				_status = TracedLockStatus.Acquired;
			}
			else
			{
				_status = TracedLockStatus.Timedout;
			}
			//lock is acuired, so collect acquired-time(T2)
			//[T2-T1 = time taken to acquire lock]
		}

		public void Dispose()
		{
			LockAnalyzer.Release(TraceableLock);

			if (_status == TracedLockStatus.Acquired)
			{
				Monitor.Exit(_traceableLock);
			}
			//T3: activity in a lock is over
		}
	}
}