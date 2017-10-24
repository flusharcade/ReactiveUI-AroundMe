
namespace ReactiveUIAroundMe.Portable.Extensions
{
    using ReactiveUI;

    using System;
    using System.Linq;
    using System.Reactive.Linq;

    /// <summary>
    /// 
    /// </summary>
    public static class ReactiveUiExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static IObservable<bool> ExecuteIfPossible<TParam, TResult>(this ReactiveCommand<TParam, TResult> cmd) =>
            cmd.CanExecute.FirstAsync().Where(can => can).Do(async _ => await cmd.Execute());

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="cmd"></param>
        /// <returns></returns>
        /*public static IObservable<bool> ExecuteIfPossible<TParam, TResult>(this ReactiveCommand<TParam, TResult> cmd) =>
            (cmd.CanExecute.FirstAsync())
            .Where(can => can)
            .Do(async _ => await cmd.Execute(_);*/
    }
}
