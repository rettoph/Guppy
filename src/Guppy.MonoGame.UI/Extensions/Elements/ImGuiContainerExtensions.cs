using Guppy.MonoGame.UI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Extensions.Elements
{
    public static class ImGuiContainerExtensions
    {
        public static TContainer AddText<TContainer>(this TContainer container, string text, out Text child)
            where TContainer : Container
        {
            child = new Text(text);

            return container.AddChild(child);
        }

        public static TContainer AddText<TContainer>(this TContainer container, string text, Action<Text> setup)
            where TContainer : Container
        {
            container.AddText(text, out Text child);
            setup(child);
            return container;
        }

        public static TContainer AddText<TContainer>(this TContainer container, string text)
            where TContainer : Container
        {

            return container.AddText(text, out _);
        }

        public static TContainer AddTextWrapped<TContainer>(this TContainer container, string text, out TextWrapped child)
            where TContainer : Container
        {
            child = new TextWrapped(text);

            return container.AddChild(child);
        }

        public static TContainer AddTextWrapped<TContainer>(this TContainer container, string text, Action<TextWrapped> setup)
            where TContainer : Container
        {
            container.AddTextWrapped(text, out TextWrapped child);
            setup(child);
            return container;
        }

        public static TContainer AddTextWrapped<TContainer>(this TContainer container, string text)
            where TContainer : Container
        {
            return container.AddTextWrapped(text, out _);
        }

        public static TContainer AddChild<TContainer>(this TContainer container, Element child)
            where TContainer : Container
        {
            container.Children.Add(child);

            return container;
        }

        public static TContainer AddChild<TContainer>(this TContainer container, Func<Element> childFactory)
            where TContainer : Container
        {
            return container.AddChild(childFactory());
        }

        public static TContainer RemoveChild<TContainer>(this TContainer container, Element child, out bool result)
            where TContainer : Container
        {
            result = container.Children.Remove(child);

            return container;
        }

        public static TContainer RemoveChild<TContainer>(this TContainer container, Element child)
            where TContainer : Container
        {
            return container.RemoveChild(child, out _);
        }

        public static TContainer SetName<TContainer>(this TContainer container, string name)
            where TContainer : Container
        {
            container.Name = name;

            return container;
        }

        public static TContainer SetFlags<TContainer, TFlags>(this TContainer container, TFlags flags)
            where TContainer : Container<TFlags>
            where TFlags : struct, Enum
        {
            container.Flags = flags;

            return container;
        }
    }
}
