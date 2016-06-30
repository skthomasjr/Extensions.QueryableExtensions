using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Extensions.QueryableExtensions
{
    public static partial class QueryableExtensions
    {
        /// <summary>
        ///     Creates an array from an <see cref="IQueryable{T}" /> by enumerating it asynchronously.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of <paramref name="source" />.
        /// </typeparam>
        /// <param name="source">The source <see cref="IQueryable{T}" />.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains an array that contains elements from the input sequence.
        /// </returns>
        public static Task<TSource[]> ToArrayAsync<TSource>(this IQueryable<TSource> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<TSource[]>();
            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(() => tcs.TrySetCanceled(), false);
            }
            try
            {
                tcs.SetResult(source.ToArray());
            }
            catch (Exception exc)
            {
                tcs.SetException(exc);
            }

            return tcs.Task;
        }
    }
}