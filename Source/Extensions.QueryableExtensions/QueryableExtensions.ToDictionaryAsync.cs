using System;
using System.Collections.Generic;
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
        ///     Creates a <see cref="Dictionary{TKey, TValue}" /> from a <see cref="IQueryable{T}" /> by enumerating it asynchronously.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of <paramref name="source" />.
        /// </typeparam>
        /// <typeparam name="TKey">
        ///     The type of the key returned by <paramref name="keySelector" /> .
        /// </typeparam>
        /// <param name="source">The source <see cref="IQueryable{T}" />.</param>
        /// <param name="keySelector"> A function to extract a key from each element. </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains a <see cref="Dictionary{TKey, TSource}" /> that contains selected keys and values.
        /// </returns>
        public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TKey, TSource>(this IQueryable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<Dictionary<TKey, TSource>>();
            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(() => tcs.TrySetCanceled(), false);
            }
            try
            {
                tcs.SetResult(source.ToDictionary(keySelector));
            }
            catch (Exception exc)
            {
                tcs.SetException(exc);
            }

            return tcs.Task;
        }
    }
}