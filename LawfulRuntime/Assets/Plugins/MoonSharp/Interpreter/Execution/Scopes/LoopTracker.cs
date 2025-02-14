using MoonSharp.Interpreter.DataStructs;
using MoonSharp.Interpreter.Execution.VM;

namespace MoonSharp.Interpreter.Execution
{
	interface ILoop
	{
		void CompileBreak(ByteCode bc);
		bool IsBoundary();
	}


	internal class LoopTracker
	{
		private const int LOOP_DEPTH = 16;
		
		public FastStack<ILoop> Loops = new FastStack<ILoop>(LOOP_DEPTH);
	}
}
