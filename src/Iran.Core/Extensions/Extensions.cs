using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Iran.Core.Extensions
{
    public static class Extensions {

        public static async Task<List<T>> ToListAsync<T>(
            this IAsyncEnumerable<T> source, 
            CancellationToken cancellationToken = default) {
            var list = new List<T>();
            await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)) {
                list.Add(item);
            }

            return list;
        }
    }
}
