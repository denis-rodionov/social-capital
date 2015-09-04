using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SocialCapital.Common
{
	public interface IAwaitable<out TResult>
	{
		IAwaiter<TResult> GetAwaiter();
	}

	public interface IAwaiter<out TResult> : INotifyCompletion // or ICriticalNotifyCompletion
	{
		bool IsCompleted { get; }

		TResult GetResult();
	}

	public struct FuncAwaiter<TResult> : IAwaiter<TResult>
	{
		private readonly Task<TResult> task;

		public FuncAwaiter(Func<TResult> function)
		{
			this.task = new Task<TResult>(function);
			this.task.Start();
		}

		bool IAwaiter<TResult>.IsCompleted
		{
			get
			{
				return this.task.IsCompleted;
			}
		}

		TResult IAwaiter<TResult>.GetResult()
		{
			return this.task.Result;
		}

		void INotifyCompletion.OnCompleted(Action continuation)
		{
			new Task(continuation).Start();
		}
	}

	public struct FuncAwaitable<TResult> : IAwaitable<TResult>
	{
		private readonly Func<TResult> function;

		public FuncAwaitable(Func<TResult> function)
		{
			this.function = function;
		}

		public IAwaiter<TResult> GetAwaiter()
		{
			return new FuncAwaiter<TResult>(this.function);
		}
	}
}

