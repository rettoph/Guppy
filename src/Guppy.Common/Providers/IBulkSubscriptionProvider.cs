﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    public interface IBulkSubscriptionProvider
    {
        void Subscribe(IEnumerable<object> instances);
        void Unsubscribe(IEnumerable<object> instances);
    }
}
