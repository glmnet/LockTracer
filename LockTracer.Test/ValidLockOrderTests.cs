using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LockTracer.Test
{
	[TestClass]
	public class ValidLockOrderTests
	{
		[TestMethod]
		public void TestValidOrderMixing1()
		{
			var lockObj1 = TracedLock.Register("TestValidOrderMixing1.lockObj1");
			var lockObj2 = TracedLock.Register("TestValidOrderMixing1.lockObj2");
			var lockObj3 = TracedLock.Register("TestValidOrderMixing1.lockObj3");

			int value = 0;

			using (TracedLock.Lock(lockObj1))
			using (TracedLock.Lock(lockObj2))
			{
				value++;
			}

			using (TracedLock.Lock(lockObj1))
			using (TracedLock.Lock(lockObj3))
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

		[TestMethod]
		public void TestValidOrderMixing2()
		{
			var lockObj1 = TracedLock.Register("TestValidOrderMixing2.lockObj1");
			var lockObj2 = TracedLock.Register("TestValidOrderMixing2.lockObj2");
			var lockObj3 = TracedLock.Register("TestValidOrderMixing2.lockObj3");

			int value = 0;

			using (TracedLock.Lock(lockObj1))
			using (TracedLock.Lock(lockObj3))
			{
				value++;
			}

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

			Assert.AreEqual(3, value);

			var orders = LockAnalyzer.DeadlockOrders;
		}

		[TestMethod]
		public void TestValidOrderMixing3()
		{
			var lockObj1 = TracedLock.Register("TestValidOrderMixing3.lockObj1");
			var lockObj2 = TracedLock.Register("TestValidOrderMixing3.lockObj2");
			var lockObj3 = TracedLock.Register("TestValidOrderMixing3.lockObj3");

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

			using (TracedLock.Lock(lockObj1))
			using (TracedLock.Lock(lockObj3))
			{
				value++;
			}

			var orders = LockAnalyzer.DeadlockOrders;
		}

		[TestMethod]
		public void TestValidOrderMixing4()
		{
			var lockObj1 = TracedLock.Register("TestValidOrderMixing4.lockObj1");
			var lockObj2 = TracedLock.Register("TestValidOrderMixing4.lockObj2");
			var lockObj3 = TracedLock.Register("TestValidOrderMixing4.lockObj3");
			var lockObj4 = TracedLock.Register("TestValidOrderMixing4.lockObj4");

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

			using (TracedLock.Lock(lockObj2))
			using (TracedLock.Lock(lockObj4))
			{
				value++;
			}

			var orders = LockAnalyzer.DeadlockOrders;
		}
	}
}
