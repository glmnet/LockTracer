using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LockTracer.Test
{
	[TestClass]
	public class InValidLockOrderTests
	{
		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestInvalidOrder()
		{
			var lockObj1 = TracedLock.Register("TestInvalidOrder.lockObj1");
			var lockObj2 = TracedLock.Register("TestInvalidOrder.lockObj2");

			int value = 0;

			using (TracedLock.Lock(lockObj1))
			using (TracedLock.Lock(lockObj2))
			{
				value++;
			}

			using (TracedLock.Lock(lockObj2))
			using (TracedLock.Lock(lockObj1)) // <- I expect some "possible deadlock" detection here
			{
				value++;
			}
		}


		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestInvalidOrder2()
		{
			// T1 locks obj1
			// T2 locks obj2
			// T3 locks obj3

			var lockObj1 = TracedLock.Register("TestInvalidOrder2.lockObj1");
			var lockObj2 = TracedLock.Register("TestInvalidOrder2.lockObj2");
			var lockObj3 = TracedLock.Register("TestInvalidOrder2.lockObj3");

			int value = 0;

			using (TracedLock.Lock(lockObj2))
			using (TracedLock.Lock(lockObj3)) 
			{
				value++;
			}

			using (TracedLock.Lock(lockObj1))
			using (TracedLock.Lock(lockObj2))
			{
				value++;
			}

			using (TracedLock.Lock(lockObj3))
			using (TracedLock.Lock(lockObj1)) // <- I expect some "possible deadlock" detection here
			{
				value++;
			}
		}


		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestInvalidOrder3()
		{
			// T1 locks obj1
			// T2 locks obj2
			// T3 locks obj3
			// T4 locks obj4

			var lockObj1 = TracedLock.Register("TestInvalidOrder3.lockObj1");
			var lockObj2 = TracedLock.Register("TestInvalidOrder3.lockObj2");
			var lockObj3 = TracedLock.Register("TestInvalidOrder3.lockObj3");
			var lockObj4 = TracedLock.Register("TestInvalidOrder3.lockObj4");

			int value = 0;

			using (TracedLock.Lock(lockObj1))
			using (TracedLock.Lock(lockObj2))
			{
				value++;
			}

			using (TracedLock.Lock(lockObj2))
			using (TracedLock.Lock(lockObj3))
			{
				value++;
			}

			using (TracedLock.Lock(lockObj3))
			using (TracedLock.Lock(lockObj4)) 
			{
				value++;
			}

			using (TracedLock.Lock(lockObj4))
			using (TracedLock.Lock(lockObj1)) // <- I expect some "possible deadlock" detection here
			{
				value++;
			}
		}
	}
}