using Guppy.Gaming.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Guppy.Gaming
{
    public static class IAsyncableExtensions
    {
        /// <summary>
        /// Create a new task to update the asyncable.
        /// </summary>
        /// <param name="asyncable"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="interval"></param>
        /// <param name="draw"></param>
        /// <returns></returns>
        public static Task StartAsync(this IAsyncable asyncable, CancellationToken cancellationToken, int interval = 16, bool draw = false)
        {
            if(draw)
            {
                return TaskHelper.CreateLoop(gt =>
                {
                    asyncable.Update(gt);
                    asyncable.Draw(gt);
                }, interval, cancellationToken);
            }

            return TaskHelper.CreateLoop(gt => asyncable.Update(gt), interval, cancellationToken);
        }
    }
}
