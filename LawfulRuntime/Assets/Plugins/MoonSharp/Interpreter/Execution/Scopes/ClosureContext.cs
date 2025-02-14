using System.Collections.Generic;
using System.Linq;

namespace MoonSharp.Interpreter.Execution
{
	/// <summary>
	/// The scope of a closure (container of upvalues)
	/// </summary>
	internal class ClosureContext : List<DynValueAccessor>
	{
		/// <summary>
		/// Gets the symbols.
		/// </summary>
		public string[] Symbols { get; private set; }

		internal ClosureContext(SymbolRef[] symbols, IEnumerable<DynValueAccessor> values)
		{
			Symbols = symbols.Select(s => s.i_Name).ToArray();
			this.AddRange(values);
		}

		internal ClosureContext()
		{
			Symbols = new string[0];
		}

	}
}
