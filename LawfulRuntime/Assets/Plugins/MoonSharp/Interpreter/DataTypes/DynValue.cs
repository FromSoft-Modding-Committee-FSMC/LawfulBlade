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
	/// A class representing a value in a Lua/MoonSharp script.
	/// </summary>
	public class DynValue
	{
		private static Dictionary<string, DynValue> stringTable = new Dictionary<string, DynValue>();

        private static KeyValuePair<double, DynValue> GetConst(double number)
        {
			return new KeyValuePair<double, DynValue>(number, new DynValue(number, DataType.Number));
        }

		private readonly double m_Number;
		private readonly object m_Object;

		///// <summary>
		///// Gets the type of the value.
		///// </summary>
		public readonly DataType Type;

		private DynValue(DataType type)
        {
			Type = type;
        }

		private DynValue(double number, DataType type)
		{
			Type = type;
			m_Number = number;
		}

		private DynValue(object @object, DataType type)
        {
			Type = type;
			m_Object = @object;
        }


        /// <summary>
        /// Gets the function (valid only if the <see cref="System.Type"/> is <see cref="DataType.Function"/>)
        /// </summary>
        public Closure Function { get { return m_Object as Closure; } }
        /// <summary>
        /// Gets the numeric value (valid only if the <see cref="System.Type"/> is <see cref="DataType.Number"/>)
        /// </summary>
        public double Number { get { return m_Number; } }
        /// <summary>
        /// Gets the values in the tuple (valid only if the <see cref="System.Type"/> is Tuple).
        /// This field is currently also used to hold arguments in values whose <see cref="System.Type"/> is <see cref="DataType.TailCallRequest"/>.
        /// </summary>
        public DynValue[] Tuple { get { return m_Object as DynValue[]; } }
        /// <summary>
        /// Gets the coroutine handle. (valid only if the <see cref="System.Type"/> is Thread).
        /// </summary>
        public Coroutine Coroutine { get { return m_Object as Coroutine; } }
        /// <summary>
        /// Gets the table (valid only if the <see cref="System.Type"/> is <see cref="DataType.Table"/>)
        /// </summary>
        public Table Table { get { return m_Object as Table; } }
        /// <summary>
        /// Gets the boolean value (valid only if the <see cref="System.Type"/> is <see cref="DataType.Boolean"/>)
        /// </summary>
        public bool Boolean { get { return m_Number != 0; } }
        /// <summary>
        /// Gets the string value (valid only if the <see cref="System.Type"/> is <see cref="DataType.String"/>)
        /// </summary>
        public string String { get { return m_Object as string; } }
        /// <summary>
        /// Gets the CLR callback (valid only if the <see cref="System.Type"/> is <see cref="DataType.ClrFunction"/>)
        /// </summary>
        public CallbackFunction Callback { get { return m_Object as CallbackFunction; } }
		/// <summary>
		/// Gets the tail call data.
		/// </summary>
		public TailCallData TailCallData { get { return m_Object as TailCallData; } }
		/// <summary>
		/// Gets the yield request data.
		/// </summary>
		public YieldRequest YieldRequest { get { return m_Object as YieldRequest; } }
		/// <summary>
		/// Gets the tail call data.
		/// </summary>
		public UserData UserData { get { return m_Object as UserData; } }

		/// <summary>
		/// Creates a new writable value initialized to Nil.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewNil()
		{
			return Nil;
		}

		/// <summary>
		/// Creates a new writable value initialized to the specified boolean.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewBoolean(bool v)
		{
			return v ? True : False;
		}


		//private static Dictionary<double, int> numCount = new();
		//private static double mostUsed;

		//private static int newNumberSaved;
		/// <summary>
		/// Creates a new writable value initialized to the specified number.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewNumber(double num)
        {
            if (num == 0.0)
            {
                return Zero;
            }
            if (num == 1.0)
            {
                return One;
            }

            return new DynValue(num, DataType.Number);
        }

		/// <summary>
		/// Creates a new writable value initialized to the specified number.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewLiteralNumber(double num)
		{
			if (num == 0.0)
			{
				return Zero;
			}
			if (num == 1.0)
			{ 
				return One;
			}

			var value = new DynValue(num, DataType.Number);
			return value;
		}

		/// <summary>
		/// Creates a new writable value initialized to the specified string.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewString(string str)
		{
			if (stringTable.TryGetValue(str, out var value))
            {
				return value;
            }
			value = new DynValue(str, DataType.String);
			stringTable.Add(str, value);
			return value;
		}

		/// <summary>
		/// Creates a new writable value initialized to the specified StringBuilder.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewString(StringBuilder sb)
		{
			return NewString(sb.ToString());
		}

		/// <summary>
		/// Creates a new writable value initialized to the specified string using String.Format like syntax
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewString(string format, params object[] args)
		{
			return new DynValue(string.Format(format, args), DataType.String);
		}

		/// <summary>
		/// Creates a new writable value initialized to the specified coroutine.
		/// Internal use only, for external use, see Script.CoroutineCreate
		/// </summary>
		/// <param name="coroutine">The coroutine object.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewCoroutine(Coroutine coroutine)
		{
			return new DynValue(coroutine, DataType.Thread);
		}

		/// <summary>
		/// Creates a new writable value initialized to the specified closure (function).
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewClosure(Closure function)
		{
			return new DynValue(function, DataType.Function);
		}

		/// <summary>
		/// Creates a new writable value initialized to the specified CLR callback.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewCallback(Func<ScriptExecutionContext, CallbackArguments, DynValue> callBack, string name = null)
		{
			return new DynValue(new CallbackFunction(callBack, name), DataType.ClrFunction);
		}

		/// <summary>
		/// Creates a new writable value initialized to the specified CLR callback.
		/// See also CallbackFunction.FromDelegate and CallbackFunction.FromMethodInfo factory methods.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewCallback(CallbackFunction function)
		{
			return new DynValue(function, DataType.ClrFunction);
		}

		/// <summary>
		/// Creates a new writable value initialized to the specified table.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewTable(Table table)
		{
			return new DynValue(table, DataType.Table);
		}

		/// <summary>
		/// Creates a new writable value initialized to an empty prime table (a 
		/// prime table is a table made only of numbers, strings, booleans and other
		/// prime tables).
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewPrimeTable()
		{
			return NewTable(new Table(null));
		}

		/// <summary>
		/// Creates a new writable value initialized to an empty table.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewTable(Script script)
		{
			return NewTable(new Table(script));
		}

		/// <summary>
		/// Creates a new writable value initialized to with array contents.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewTable(Script script, params DynValue[] arrayValues)
		{
			return NewTable(new Table(script, arrayValues));
		}

		/// <summary>
		/// Creates a new request for a tail call. This is the preferred way to execute Lua/MoonSharp code from a callback,
		/// although it's not always possible to use it. When a function (callback or script closure) returns a
		/// TailCallRequest, the bytecode processor immediately executes the function contained in the request.
		/// By executing script in this way, a callback function ensures it's not on the stack anymore and thus a number
		/// of functionality (state savings, coroutines, etc) keeps working at full power.
		/// </summary>
		/// <param name="tailFn">The function to be called.</param>
		/// <param name="args">The arguments.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewTailCallReq(DynValue tailFn, params DynValue[] args)
		{
			return new DynValue(new TailCallData() { Args = args, Function = tailFn }, DataType.TailCallRequest);
		}

		/// <summary>
		/// Creates a new request for a tail call. This is the preferred way to execute Lua/MoonSharp code from a callback,
		/// although it's not always possible to use it. When a function (callback or script closure) returns a
		/// TailCallRequest, the bytecode processor immediately executes the function contained in the request.
		/// By executing script in this way, a callback function ensures it's not on the stack anymore and thus a number
		/// of functionality (state savings, coroutines, etc) keeps working at full power.
		/// </summary>
		/// <param name="tailCallData">The data for the tail call.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewTailCallReq(TailCallData tailCallData)
		{
			return new DynValue(tailCallData, DataType.TailCallRequest);
		}



		/// <summary>
		/// Creates a new request for a yield of the current coroutine.
		/// </summary>
		/// <param name="args">The yield argumenst.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewYieldReq(DynValue[] args)
		{
			return new DynValue(new YieldRequest() { ReturnValues = args }, DataType.YieldRequest);
		}

		/// <summary>
		/// Creates a new request for a yield of the current coroutine.
		/// </summary>
		/// <param name="args">The yield argumenst.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static DynValue NewForcedYieldReq()
		{
			return new DynValue(new YieldRequest() { Forced = true }, DataType.YieldRequest);
		}

		/// <summary>
		/// Creates a new tuple initialized to the specified values.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewTuple(params DynValue[] values)
		{
			if (values.Length == 0)
				return DynValue.NewNil();

			if (values.Length == 1)
				return values[0];

			return new DynValue(values, DataType.Tuple);
		}

		/// <summary>
		/// Creates a new tuple initialized to the specified values - which can be potentially other tuples
		/// </summary>
		public static DynValue NewTupleNested(params DynValue[] values)
		{
			if (!values.Any(v => v.Type == DataType.Tuple))
				return NewTuple(values);

			if (values.Length == 1)
				return values[0];

			List<DynValue> vals = new List<DynValue>();

			foreach (var v in values)
			{
				if (v.Type == DataType.Tuple)
					vals.AddRange(v.Tuple);
				else
					vals.Add(v);
			}

			return new DynValue(vals.ToArray(), DataType.Tuple);
		}


		/// <summary>
		/// Creates a new userdata value
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue NewUserData(UserData userData)
		{
			return new DynValue(userData, DataType.UserData);
		}

		/// <summary>
		/// A preinitialized, readonly instance, equaling Void
		/// </summary>
		public static DynValue Void { get; private set; }
		/// <summary>
		/// A preinitialized, readonly instance, equaling Nil
		/// </summary>
		public static DynValue Nil { get; private set; }
		/// <summary>
		/// A preinitialized, readonly instance, equaling True
		/// </summary>
		public static DynValue True { get; private set; }
		/// <summary>
		/// A preinitialized, readonly instance, equaling False
		/// </summary>
		public static DynValue False { get; private set; }

		/// <summary>
		/// A preinitialized, readonly instance, equaling Zero
		/// </summary>
		public static DynValue Zero { get; private set; }
		/// <summary>
		/// A preinitialized, readonly instance, equaling One
		/// </summary>
		public static DynValue One { get; private set; }


		static DynValue()
		{
			Nil = new DynValue(DataType.Nil);
			Void = new DynValue(DataType.Void);
			True = new DynValue(1.0, DataType.Boolean);
			False = new DynValue(0.0, DataType.Boolean);
			Zero = new DynValue(0.0, DataType.Number);
			One = new DynValue(1.0, DataType.Number);
		}

		/// <summary>
		/// Returns a string which is what it's expected to be output by the print function applied to this value.
		/// </summary>
		public string ToPrintString()
		{
			if (this.m_Object != null && this.m_Object is RefIdObject)
			{
				RefIdObject refid = (RefIdObject)m_Object;

				string typeString = this.Type.ToLuaTypeString();

				if (m_Object is UserData)
				{
					UserData ud = (UserData)m_Object;
					string str = ud.Descriptor.AsString(ud.Object);
					if (str != null)
						return str;
				}

				return refid.FormatTypeString(typeString);
			}

			switch (Type)
			{
				case DataType.String:
					return String;
				case DataType.Tuple:
					return string.Join("\t", Tuple.Select(t => t.ToPrintString()).ToArray());
				case DataType.TailCallRequest:
					return "(TailCallRequest -- INTERNAL!)";
				case DataType.YieldRequest:
					return "(YieldRequest -- INTERNAL!)";
				default:
					return ToString();
			}
		}

		/// <summary>
		/// Returns a string which is what it's expected to be output by debuggers.
		/// </summary>
		public string ToDebugPrintString()
		{
			if (this.m_Object != null && this.m_Object is RefIdObject)
			{
				RefIdObject refid = (RefIdObject)m_Object;

				string typeString = this.Type.ToLuaTypeString();

				if (m_Object is UserData)
				{
					UserData ud = (UserData)m_Object;
					string str = ud.Descriptor.AsString(ud.Object);
					if (str != null)
						return str;
				}

				return refid.FormatTypeString(typeString);
			}

			switch (Type)
			{
				case DataType.Tuple:
					return string.Join("\t", Tuple.Select(t => t.ToPrintString()).ToArray());
				case DataType.TailCallRequest:
					return "(TailCallRequest)";
				case DataType.YieldRequest:
					return "(YieldRequest)";
				default:
					return ToString();
			}
		}


		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			switch (Type)
			{
				case DataType.Void:
					return "void";
				case DataType.Nil:
					return "nil";
				case DataType.Boolean:
					return Boolean.ToString().ToLower();
				case DataType.Number:
					return Number.ToString(CultureInfo.InvariantCulture);
				case DataType.String:
					return "\"" + String + "\"";
				case DataType.Function:
					return string.Format("(Function {0:X8})", Function.EntryPointByteCodeLocation);
				case DataType.ClrFunction:
					return string.Format("(Function CLR)", Function);
				case DataType.Table:
					return "(Table)";
				case DataType.Tuple:
					return string.Join(", ", Tuple.Select(t => t.ToString()).ToArray());
				case DataType.TailCallRequest:
					return "Tail:(" + string.Join(", ", Tuple.Select(t => t.ToString()).ToArray()) + ")";
				case DataType.UserData:
					return "(UserData)";
				case DataType.Thread:
					return string.Format("(Coroutine {0:X8})", this.Coroutine.ReferenceID);
				default:
					return "(???)";
			}
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			int baseValue = ((int)Type) << 27;

			switch (Type)
			{
				case DataType.Void:
				case DataType.Nil:
					return baseValue;
				case DataType.Boolean:
					return baseValue ^ (Boolean ? 1 : 0);
				case DataType.Number:
					return baseValue ^ m_Number.GetHashCode();
				case DataType.String:
				case DataType.Function:
				case DataType.ClrFunction:
				case DataType.Table:
				case DataType.Tuple:
				case DataType.TailCallRequest:
				case DataType.UserData:
				case DataType.Thread:
					return baseValue ^ m_Object.GetHashCode();
				default:
					return baseValue;
			}
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			DynValue other = obj as DynValue;

			if (other == null) return false;

			if ((other.Type == DataType.Nil && this.Type == DataType.Void)
				|| (other.Type == DataType.Void && this.Type == DataType.Nil))
				return true;

			if (other.Type != this.Type) return false;


			switch (Type)
			{
				case DataType.Void:
				case DataType.Nil:
					return true;
				case DataType.Boolean:
					return Boolean == other.Boolean;
				case DataType.Number:
					return Number == other.Number;
				case DataType.String:
					return String == other.String;
				case DataType.Function:
					return Function == other.Function;
				case DataType.ClrFunction:
					return Callback == other.Callback;
				case DataType.Table:
					return Table == other.Table;
				case DataType.Tuple:
				case DataType.TailCallRequest:
					return Tuple == other.Tuple;
				case DataType.Thread:
					return Coroutine == other.Coroutine;
				case DataType.UserData:
					{
						UserData ud1 = this.UserData;
						UserData ud2 = other.UserData;

						if (ud1 == null || ud2 == null)
							return false;

						if (ud1.Descriptor != ud2.Descriptor)
							return false;

						if (ud1.Object == null && ud2.Object == null)
							return true;

						if (ud1.Object != null && ud2.Object != null)
							return ud1.Object.Equals(ud2.Object);

						return false;
					}
				default:
					return object.ReferenceEquals(this, other);
			}
		}


		/// <summary>
		/// Casts this DynValue to string, using coercion if the type is number.
		/// </summary>
		/// <returns>The string representation, or null if not number, not string.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string CastToString()
		{
			DynValue rv = ToScalar();
			if (rv.Type == DataType.Number)
			{
				return rv.Number.ToString();
			}
			else if (rv.Type == DataType.String)
			{
				return rv.String;
			}
			return null;
		}

		/// <summary>
		/// Casts this DynValue to a double, using coercion if the type is string.
		/// </summary>
		/// <returns>The string representation, or null if not number, not string or non-convertible-string.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double? CastToNumber()
		{
			DynValue rv = ToScalar();
			if (rv.Type == DataType.Number)
			{
				return rv.Number;
			}
			else if (rv.Type == DataType.String)
			{
				double num;
				if (double.TryParse(rv.String, NumberStyles.Any, CultureInfo.InvariantCulture, out num))
					return num;
			}
			return null;
		}


		/// <summary>
		/// Casts this DynValue to a bool
		/// </summary>
		/// <returns>False if value is false or nil, true otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool CastToBool()
		{
			DynValue rv = ToScalar();
			if (rv.Type == DataType.Boolean)
				return rv.Boolean;
			else return (rv.Type != DataType.Nil && rv.Type != DataType.Void);
		}

		/// <summary>
		/// Returns this DynValue as an instance of <see cref="IScriptPrivateResource"/>, if possible,
		/// null otherwise
		/// </summary>
		/// <returns>False if value is false or nil, true otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IScriptPrivateResource GetAsPrivateResource()
		{
			return m_Object as IScriptPrivateResource;
		}


		/// <summary>
		/// Converts a tuple to a scalar value. If it's already a scalar value, this function returns "this".
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DynValue ToScalar()
		{
			if (Type != DataType.Tuple)
				return this;

			if (Tuple.Length == 0)
				return DynValue.Void;

			return Tuple[0].ToScalar();
		}

		/// <summary>
		/// Gets the length of a string or table value.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ScriptRuntimeException">Value is not a table or string.</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DynValue GetLength()
		{
			if (this.Type == DataType.Table)
				return DynValue.NewNumber(this.Table.Length);
			if (this.Type == DataType.String)
				return DynValue.NewNumber(this.String.Length);

			throw new ScriptRuntimeException("Can't get length of type {0}", this.Type);
		}

		/// <summary>
		/// Determines whether this instance is nil or void
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsNil()
		{
			return this.Type == DataType.Nil || this.Type == DataType.Void;
		}

		/// <summary>
		/// Determines whether this instance is not nil or void
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsNotNil()
		{
			return this.Type != DataType.Nil && this.Type != DataType.Void;
		}

		/// <summary>
		/// Determines whether this instance is void
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsVoid()
		{
			return this.Type == DataType.Void;
		}

		/// <summary>
		/// Determines whether this instance is not void
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsNotVoid()
		{
			return this.Type != DataType.Void;
		}

		/// <summary>
		/// Determines whether is nil, void or NaN (and thus unsuitable for using as a table key).
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsNilOrNan()
		{
			return (this.Type == DataType.Nil) || (this.Type == DataType.Void) || (this.Type == DataType.Number && double.IsNaN(this.Number));
		}

		/// <summary>
		/// Creates a new DynValue from a CLR object
		/// </summary>
		/// <param name="script">The script.</param>
		/// <param name="obj">The object.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynValue FromObject(Script script, object obj)
		{
			return MoonSharp.Interpreter.Interop.Converters.ClrToScriptConversions.ObjectToDynValue(script, obj);
		}

		/// <summary>
		/// Converts this MoonSharp DynValue to a CLR object.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object ToObject()
		{
			return MoonSharp.Interpreter.Interop.Converters.ScriptToClrConversions.DynValueToObject(this);
		}

		/// <summary>
		/// Converts this MoonSharp DynValue to a CLR object of the specified type.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object ToObject(Type desiredType)
		{
			//Contract.Requires(desiredType != null);
			return MoonSharp.Interpreter.Interop.Converters.ScriptToClrConversions.DynValueToObjectOfType(this, desiredType, null, false);
		}

		/// <summary>
		/// Converts this MoonSharp DynValue to a CLR object of the specified type.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T ToObject<T>()
		{
			return (T)ToObject(typeof(T));
		}

#if HASDYNAMIC
		/// <summary>
		/// Converts this MoonSharp DynValue to a CLR object, marked as dynamic
		/// </summary>
		public dynamic ToDynamic()
		{
			return MoonSharp.Interpreter.Interop.Converters.ScriptToClrConversions.DynValueToObject(this);
		}
#endif

		/// <summary>
		/// Checks the type of this value corresponds to the desired type. A propert ScriptRuntimeException is thrown
		/// if the value is not of the specified type or - considering the TypeValidationFlags - is not convertible
		/// to the specified type.
		/// </summary>
		/// <param name="funcName">Name of the function requesting the value, for error message purposes.</param>
		/// <param name="desiredType">The desired data type.</param>
		/// <param name="argNum">The argument number, for error message purposes.</param>
		/// <param name="flags">The TypeValidationFlags.</param>
		/// <returns></returns>
		/// <exception cref="ScriptRuntimeException">Thrown
		/// if the value is not of the specified type or - considering the TypeValidationFlags - is not convertible
		/// to the specified type.</exception>
		public DynValue CheckType(string funcName, DataType desiredType, int argNum = -1, TypeValidationFlags flags = TypeValidationFlags.Default)
		{
			if (this.Type == desiredType)
				return this;

			bool allowNil = ((int)(flags & TypeValidationFlags.AllowNil) != 0);

			if (allowNil && this.IsNil())
				return this;

			bool autoConvert = ((int)(flags & TypeValidationFlags.AutoConvert) != 0);

			if (autoConvert)
			{
				if (desiredType == DataType.Boolean)
					return DynValue.NewBoolean(this.CastToBool());

				if (desiredType == DataType.Number)
				{
					double? v = this.CastToNumber();
					if (v.HasValue)
						return DynValue.NewNumber(v.Value);
				}

				if (desiredType == DataType.String)
				{
					string v = this.CastToString();
					if (v != null)
						return DynValue.NewString(v);
				}
			}

			if (this.IsVoid())
				throw ScriptRuntimeException.BadArgumentNoValue(argNum, funcName, desiredType);

			throw ScriptRuntimeException.BadArgument(argNum, funcName, desiredType, this.Type, allowNil);
		}

		/// <summary>
		/// Checks if the type is a specific userdata type, and returns it or throws.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="funcName">Name of the function.</param>
		/// <param name="argNum">The argument number.</param>
		/// <param name="flags">The flags.</param>
		/// <returns></returns>
		public T CheckUserDataType<T>(string funcName, int argNum = -1, TypeValidationFlags flags = TypeValidationFlags.Default)
		{
			DynValue v = this.CheckType(funcName, DataType.UserData, argNum, flags);
			bool allowNil = ((int)(flags & TypeValidationFlags.AllowNil) != 0);

			if (v.IsNil())
				return default(T);

			object o = v.UserData.Object;
			if (o != null && o is T)
				return (T)o;

			throw ScriptRuntimeException.BadArgumentUserData(argNum, funcName, typeof(T), o, allowNil);
		}
	}
}
