using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Extensions.QueryableExtensions
{
    /// <summary>
    ///     Extension methods for use with <see cref="IQueryable{T}" />.
    /// </summary>
    public static partial class QueryableExtensions
    {
        /// <summary>
        ///     Asynchronously enumerates the query without the overhead of creating the list or array.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of <paramref name="source" />.
        /// </typeparam>
        /// <param name="source">The source <see cref="IQueryable{T}" />.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static Task LoadAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<bool>();
            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(() => tcs.TrySetCanceled(), false);
            }
            try
            {
                while (source.GetEnumerator().MoveNext())
                {
                }

                tcs.SetResult(true);
            }
            catch (Exception exc)
            {
                (source as IDisposable)?.Dispose();

                tcs.SetException(exc);
            }

            return tcs.Task;
        }
    }
}