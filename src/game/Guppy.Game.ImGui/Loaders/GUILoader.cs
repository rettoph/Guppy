using Autofac;
using Guppy.Engine.Attributes;
using Guppy.Engine.Common.Helpers;
using Guppy.Core.Files.Helpers;
using Guppy.Game.ImGui.Constants;
using Guppy.Game.ImGui.Messages;
using Guppy.Game.ImGui.Serialization.Json.Converters;
using Guppy.Game.ImGui.Services;
using Guppy.Game.ImGui.Styling.StyleValueResources;
using Guppy.Game.Input.Enums;
using Guppy.Engine.Loaders;
using Guppy.Core.Resources.Serialization.Json.Converters;
using Microsoft.Xna.Framework.Input;
using System.Text.Json.Serialization;

namespace Guppy.Game.ImGui.Loaders
{
    [AutoLoad]
    internal sealed class GUILoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            string nativesDirectory = DirectoryHelper.Combine(DirectoryHelper.GetEntryDirectory(), NativeConstants.Directory);
            NativeHelper.Load(nativesDirectory, NativeConstants.cImGui, NativeConstants.cImPlot);

            services.RegisterType<NotImplementedImguiBatch>().AsImplementedInterfaces().SingleInstance();
            services.RegisterType<ImGui>().As<IImGui>().SingleInstance();
            services.RegisterType<ImGuiObjectExplorerService>().As<IImGuiObjectExplorerService>().SingleInstance();
            services.RegisterType<DefaultImGuiObjectExplorer>().AsSelf().As<ImGuiObjectExplorer>().SingleInstance();

            services.RegisterType<ImStyleConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<PolymorphicConverter<ImStyleValue>>().As<JsonConverter>().SingleInstance();
            services.RegisterType<ImStyleVarFloatValueConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<ImStyleVarVector2ValueConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<ImStyleColorValueConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<ImStyleFontValueConverter>().As<JsonConverter>().SingleInstance();

            AddImGuiKeyEvent(services, InputConstants.UI_Tab, Keys.Tab, ImGuiKey.Tab);
            AddImGuiKeyEvent(services, InputConstants.UI_LeftArrow, Keys.Left, ImGuiKey.LeftArrow);
            AddImGuiKeyEvent(services, InputConstants.UI_RightArrow, Keys.Right, ImGuiKey.RightArrow);
            AddImGuiKeyEvent(services, InputConstants.UI_UpArrow, Keys.Up, ImGuiKey.UpArrow);
            AddImGuiKeyEvent(services, InputConstants.UI_DownArrow, Keys.Down, ImGuiKey.DownArrow);
            AddImGuiKeyEvent(services, InputConstants.UI_PageUp, Keys.PageUp, ImGuiKey.PageUp);
            AddImGuiKeyEvent(services, InputConstants.UI_PageDown, Keys.PageDown, ImGuiKey.PageDown);
            AddImGuiKeyEvent(services, InputConstants.UI_Home, Keys.Home, ImGuiKey.Home);
            AddImGuiKeyEvent(services, InputConstants.UI_End, Keys.End, ImGuiKey.End);
            AddImGuiKeyEvent(services, InputConstants.UI_Delete, Keys.Delete, ImGuiKey.Delete);
            AddImGuiKeyEvent(services, InputConstants.UI_Backspace, Keys.Back, ImGuiKey.Backspace);
            AddImGuiKeyEvent(services, InputConstants.UI_Enter, Keys.Enter, ImGuiKey.Enter);
            AddImGuiKeyEvent(services, InputConstants.UI_Escape, Keys.Escape, ImGuiKey.Escape);
            AddImGuiKeyEvent(services, InputConstants.UI_Space, Keys.Space, ImGuiKey.Space);
            AddImGuiKeyEvent(services, InputConstants.UI_A, Keys.A, ImGuiKey.A);
            AddImGuiKeyEvent(services, InputConstants.UI_C, Keys.C, ImGuiKey.C);
            AddImGuiKeyEvent(services, InputConstants.UI_V, Keys.V, ImGuiKey.V);
            AddImGuiKeyEvent(services, InputConstants.UI_X, Keys.X, ImGuiKey.X);
            AddImGuiKeyEvent(services, InputConstants.UI_Y, Keys.Y, ImGuiKey.Y);
            AddImGuiKeyEvent(services, InputConstants.UI_Z, Keys.Z, ImGuiKey.Z);
            AddImGuiKeyEvent(services, InputConstants.UI_LeftShift, Keys.LeftShift, ImGuiKey.LeftShift);
            AddImGuiKeyEvent(services, InputConstants.UI_RightShift, Keys.RightShift, ImGuiKey.RightShift);
            AddImGuiKeyEvent(services, InputConstants.UI_LeftCtrl, Keys.LeftControl, ImGuiKey.LeftCtrl);
            AddImGuiKeyEvent(services, InputConstants.UI_RightCtrl, Keys.RightControl, ImGuiKey.RightCtrl);
            AddImGuiKeyEvent(services, InputConstants.UI_LeftAlt, Keys.LeftAlt, ImGuiKey.LeftAlt);
            AddImGuiKeyEvent(services, InputConstants.UI_RightAlt, Keys.RightAlt, ImGuiKey.RightAlt);
            AddImGuiKeyEvent(services, InputConstants.UI_LeftSuper, Keys.LeftWindows, ImGuiKey.LeftSuper);
            AddImGuiKeyEvent(services, InputConstants.UI_RightSuper, Keys.RightWindows, ImGuiKey.RightSuper);

            AddImGuiMouseButtonEvent(services, InputConstants.UI_MouseButton01, CursorButtons.Left, 0);
            AddImGuiMouseButtonEvent(services, InputConstants.UI_MouseButton02, CursorButtons.Middle, 1);
            AddImGuiMouseButtonEvent(services, InputConstants.UI_MouseButton03, CursorButtons.Right, 2);
        }

        private static void AddImGuiKeyEvent(ContainerBuilder services, string key, Keys defaultKey, ImGuiKey mapping)
        {
            services.RegisterInput(
                key,
                defaultKey,
                new[]
                {
                    (ButtonState.Pressed, new ImGuiKeyEvent(mapping, true)),
                    (ButtonState.Released, new ImGuiKeyEvent(mapping, false))
                });
        }

        private static void AddImGuiMouseButtonEvent(ContainerBuilder services, string key, CursorButtons defaultButton, int mapping)
        {
            services.RegisterInput(
                key,
                defaultButton,
                new[]
                {
                    (ButtonState.Pressed, new ImGuiMouseButtonEvent(mapping, true)),
                    (ButtonState.Released, new ImGuiMouseButtonEvent(mapping, false))
                });
        }
    }
}
