using Guppy.Core.Builders;
using Guppy.Core.Common.Builders;

namespace Guppy.Core.Utilities
{
    public class GuppyBuilderFilterManager<TBuilder>(IEnumerable<GuppyContainerBuilderFilter<TBuilder>>? guppyContainerBuilderFilter = null)
        where TBuilder : IGuppyContainerBuilder
    {
        private readonly List<GuppyContainerBuilderFilter<TBuilder>> _list = new(guppyContainerBuilderFilter ?? []);

        /// <summary>
        /// Add a new filter, run it, and return it.
        /// 
        /// If the filter is not ran succesfully then
        /// we will cache it and attempt to re-run the next
        /// time a filter is added and ran
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="filter"></param>
        /// <param name="build"></param>
        /// <returns></returns>
        public GuppyContainerBuilderFilter<TBuilder> Add(
            Action<IGuppyContainerBuilderFilterBuilder<TBuilder>> filter,
            Action<TBuilder> build)
        {
            GuppyContainerBuilderFilterBuilder<TBuilder> containerBuilderFilterBuilder = new();
            filter(containerBuilderFilterBuilder);

            GuppyContainerBuilderFilter<TBuilder> containerBuilderFilter = GuppyContainerBuilderFilter<TBuilder>.Create(filter, build);
            this._list.Add(containerBuilderFilter);

            return containerBuilderFilter;
        }

        /// <summary>
        /// Excecute all internal filters.
        /// </summary>
        public void ExcecuteAll(TBuilder builder)
        {
            bool dirty = false;

            do
            {
                dirty = false;

                for (int i = 0; i < this._list.Count; i++)
                {
                    if (this._list[i].Filter(builder) == false)
                    {
                        continue;
                    }


                    this._list[i].Build(builder);

                    dirty = true;
                    this._list.RemoveAt(i);
                    break;
                }
            } while (dirty == true);
        }
    }
}
