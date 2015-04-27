using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace LockTracer
{
	public static class LockAnalyzer
	{
		private static readonly object Sync = new object();
		private static readonly List<List<TraceableLock>> LockingOrders = new List<List<TraceableLock>>();
		private static readonly Dictionary<int, List<TraceableLock>> ActiveLockChains = new Dictionary<int, List<TraceableLock>>();

		public static string DeadlockOrders
		{
			get
			{
				return string.Join("; ", LockingOrders.Select(
					lockingOrder => String.Join(", ", lockingOrder.Select(order => order.Name))));
			}
		}

		public static void Lock(TraceableLock traceableLock)
		{
			lock (Sync)
			{
				var threadId = Thread.CurrentThread.ManagedThreadId;
				if (!ActiveLockChains.ContainsKey(threadId))
					ActiveLockChains[threadId] = new List<TraceableLock>();

				if (ActiveLockChains[threadId].Contains(traceableLock))
					return; // Same thread using the same lock

				ActiveLockChains[threadId].Add(traceableLock);
				if (!VerifyLockChain(ActiveLockChains[threadId]))
				{
					var copy = ActiveLockChains[threadId].ToList();
					ActiveLockChains[threadId].Remove(ActiveLockChains[threadId].Last());
					PossibleDeadlock(copy);
				}
			}
		}

		public static void Release(TraceableLock traceableLock)
		{
			lock (Sync)
			{
				var threadId = Thread.CurrentThread.ManagedThreadId;
				
				if (ActiveLockChains[threadId].Count > 0 && ActiveLockChains[threadId].Last() == traceableLock)
					ActiveLockChains[threadId].Remove(traceableLock);
			}
		}

		private static readonly Dictionary<TraceableLock, List<TraceableLock>> Requires = new Dictionary<TraceableLock, List<TraceableLock>>();

		static IEnumerable<TraceableLock> RequiredLocks(TraceableLock traceableLock)
		{
			if (Requires.ContainsKey(traceableLock))
				foreach (var tl in Requires[traceableLock])
				{
					yield return tl;
					foreach (TraceableLock tl2 in RequiredLocks(tl))
					{
						yield return tl2;
					}
				}
		}

		private static bool VerifyLockChain(IEnumerable<TraceableLock> thisLockChain)
		{
			var chainRequredLocks = new List<TraceableLock>();
			TraceableLock prevLock = null;
			foreach (TraceableLock traceableLock in thisLockChain)
			{
				if (chainRequredLocks.Contains(traceableLock))
					return false;

				if (Requires.ContainsKey(traceableLock))
					chainRequredLocks.AddRange(RequiredLocks(traceableLock));

				if (prevLock != null)
				{
					if (Requires.ContainsKey(traceableLock))
					{
						if (!Requires[traceableLock].Contains(prevLock))
							Requires[traceableLock].Add(prevLock);
					}
					else
					{
						Requires[traceableLock] = new List<TraceableLock> {prevLock};
					}
				}
				prevLock = traceableLock;
			}

			return true;
		}


		private static void PossibleDeadlock(IEnumerable<TraceableLock> offendingChain)
		{
			throw new ApplicationException(string.Format("Posible deadlock order found: {0} Current valid orders: {1}", 
				String.Join(", ", offendingChain.Select(x => x.Name)), DeadlockOrders));
			
		}

	}
}