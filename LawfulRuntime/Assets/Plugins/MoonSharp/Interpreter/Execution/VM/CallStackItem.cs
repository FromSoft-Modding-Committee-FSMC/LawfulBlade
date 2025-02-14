using MoonSharp.Interpreter.Debugging;

namespace MoonSharp.Interpreter.Execution.VM
{
	internal class CallStackItem
	{
		public SymbolRef[] Debug_Symbols;

		public SourceRef CallingSourceRef;

		public CallbackFunction ClrFunction;
		public CallbackFunction Continuation;
		public CallbackFunction ErrorHandler;
		public DynValue ErrorHandlerBeforeUnwind;

		public DynValueAccessor[] LocalScope;
		public ClosureContext ClosureScope;

		public int BasePointer;
		public int ReturnAddress;
		public int Debug_EntryPoint;
		public CallStackItemFlags Flags;
	}

}
