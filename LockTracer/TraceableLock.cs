using System.Diagnostics;
using System.Net.Configuration;
using System.Runtime.InteropServices;
using System.Text;

namespace LockTracer
{
	[DebuggerDisplay("{Name}")]
	public class TraceableLock
	{
		internal TraceableLock(string name)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}
}
