using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LockTracer.Test
{
	/// <summary>
	/// Summary description for ValidDeadlockOrderWithRepeatedUsage
	/// </summary>
	[TestClass]
	public class ValidDeadlockOrderWithRepeatedUsage
	{
		[TestMethod]
		public void TestValidOrderMixing1()
		{
			var lockObj1 = TracedLock.Register("TestValidOrderMixing1.lockObj1");
			var lockObj2 = TracedLock.Register("TestValidOrderMixing1.lockObj2");
			var lockObj3 = TracedLock.Register("TestValidOrderMixing1.lockObj3");

			int value = 0;

			using (TracedLock.Lock(lockObj1))
			using (TracedLock.Lock(lockObj1))
			using (TracedLock.Lock(lockObj2))
			{
				value++;
			}

			using (TracedLock.Lock(lockObj1))
			using (TracedLock.Lock(lockObj3))
			using (TracedLock.Lock(lockObj1))
			{
				value++;
			}

			using (TracedLock.Lock(lockObj2))
			using (TracedLock.Lock(lockObj3))
			{
				value++;
			}

			var orders = LockAnalyzer.DeadlockOrders;
		}
	}
}
