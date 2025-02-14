using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace MoonSharp.Interpreter
{
	/// <summary>
	/// A class that allows a caller to pass a pair of get/set accessors for retrieving/updating closure values
	/// </summary>
	internal sealed class DynValueAccessor
	{
		public readonly Func<DynValue> Get;
		public readonly Action<DynValue> Assign;

		internal DynValueAccessor(Func<DynValue> getter, Action<DynValue> setter)
        {
			Get = getter;
			Assign = setter;
        }

		internal DynValueAccessor(DynValue value)
		{
			Get = () => value;
			Assign = i => { };
		}

		public static implicit operator DynValue(DynValueAccessor v)
        {
			return v.Get();
        }

		public static DynValueAccessor NewNil()
        {
			DynValue v = DynValue.Nil;
			return new DynValueAccessor(() => v, i => { v = i; });
        }
	}
}
