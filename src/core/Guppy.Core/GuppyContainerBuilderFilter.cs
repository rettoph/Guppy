using Guppy.Core.Builders;
using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Delegates;

namespace Guppy.Core
{
    public class GuppyContainerBuilderFilter<TBuilder>(GuppyContainerFilterDelegate<TBuilder>? filter, Action<TBuilder> build) : IGuppyContainerBuilderFilter<TBuilder>
        where TBuilder : IGuppyContainerBuilder
    {
        private readonly GuppyContainerFilterDelegate<TBuilder>? _filter = filter;
        private readonly Action<TBuilder> _build = build;

        public void Build(TBuilder builder)
        {
            this._build(builder);
        }

        public bool Filter(TBuilder builder)
        {
            if (this._filter is null)
            {
                return true;
            }

            bool result = true;
            foreach (GuppyContainerFilterDelegate<TBuilder> del in this._filter.GetInvocationList())
            {
                result &= del.Invoke(builder);
            }

            return result;
        }

        public static GuppyContainerBuilderFilter<TBuilder> Create(
            Action<IGuppyContainerBuilderFilterBuilder<TBuilder>> filter,
            Action<TBuilder> build)
        {
            GuppyContainerBuilderFilterBuilder<TBuilder> containerBuilderFilterBuilder = new();
            filter(containerBuilderFilterBuilder);

            GuppyContainerBuilderFilter<TBuilder> containerBuilderFilter = new(containerBuilderFilterBuilder.Build(), build);

            return containerBuilderFilter;
        }
    }
}
