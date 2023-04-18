using Guppy.Common;
using Guppy.GUI.Loaders;
using Guppy.Input.Providers;
using Guppy.Resources.Providers;

namespace Guppy.GUI.Providers
{
    internal class StageProvider : IStageProvider
    {
        private readonly IScreen _screen;
        private readonly ICursorProvider _cursors;
        private readonly IBus _bus;
        private readonly IResourceProvider _resources;
        private readonly IStageLoader[] _loaders;

        public StageProvider(
            IScreen screen,
            ICursorProvider cursors,
            IBus bus,
            IResourceProvider resources,
            IFiltered<IStageLoader> loaders)
        {
            _screen = screen;
            _cursors = cursors;
            _bus = bus;
            _resources = resources;
            _loaders = loaders.Instances.ToArray();
        }

        public Stage Create(params string[] names)
        {
            IStyleSheet styleSheet = new StyleSheet(_resources);

            Stage stage = new Stage(names, styleSheet, _screen, _cursors, _bus);

            foreach (IStageLoader loader in _loaders)
            {
                if (loader.StageBlockList.Allows(names))
                {
                    loader.Load(stage);
                }
            }

            return stage;
        }
    }
}
