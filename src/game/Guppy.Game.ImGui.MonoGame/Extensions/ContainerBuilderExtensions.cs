using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Common.Helpers;
using Guppy.Core.Files.Common.Helpers;
using Guppy.Core.Serialization.Json.Converters;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Constants;
using Guppy.Game.ImGui.Common.Serialization.Json.Converters;
using Guppy.Game.ImGui.Common.Services;
using Guppy.Game.ImGui.Common.Styling.StyleValueResources;
using Guppy.Game.ImGui.MonoGame.Common.Messages;
using Guppy.Game.Input.Common.Enums;
using Microsoft.Xna.Framework.Input;
using System.Text.Json.Serialization;

namespace Guppy.Game.ImGui.MonoGame.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterGameMonoGameImGuiServices(this ContainerBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterGameMonoGameImGuiServices), builder =>
            {
                string nativesDirectory = DirectoryHelper.Combine(DirectoryHelper.GetEntryDirectory(), NativeConstants.Directory);
                NativeHelper.Load(nativesDirectory, NativeConstants.cImGui, NativeConstants.cImPlot);

                builder.RegisterType<MonoGameImGuiBatch>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<ImGui>().As<IImGui>().SingleInstance();
                builder.RegisterType<ImGuiObjectExplorerService>().As<IImGuiObjectExplorerService>().SingleInstance();
                builder.RegisterType<DefaultImGuiObjectExplorer>().AsSelf().As<ImGuiObjectExplorer>().SingleInstance();

                builder.RegisterType<ImStyleConverter>().As<JsonConverter>().SingleInstance();
                builder.RegisterType<PolymorphicConverter<ImStyleValue>>().As<JsonConverter>().SingleInstance();
                builder.RegisterType<ImStyleVarFloatValueConverter>().As<JsonConverter>().SingleInstance();
                builder.RegisterType<ImStyleVarVector2ValueConverter>().As<JsonConverter>().SingleInstance();
                builder.RegisterType<ImStyleColorValueConverter>().As<JsonConverter>().SingleInstance();
                builder.RegisterType<ImStyleFontValueConverter>().As<JsonConverter>().SingleInstance();

                AddImGuiKeyEvent(builder, InputConstants.UI_Tab, Keys.Tab, ImGuiKey.Tab);
                AddImGuiKeyEvent(builder, InputConstants.UI_LeftArrow, Keys.Left, ImGuiKey.LeftArrow);
                AddImGuiKeyEvent(builder, InputConstants.UI_RightArrow, Keys.Right, ImGuiKey.RightArrow);
                AddImGuiKeyEvent(builder, InputConstants.UI_UpArrow, Keys.Up, ImGuiKey.UpArrow);
                AddImGuiKeyEvent(builder, InputConstants.UI_DownArrow, Keys.Down, ImGuiKey.DownArrow);
                AddImGuiKeyEvent(builder, InputConstants.UI_PageUp, Keys.PageUp, ImGuiKey.PageUp);
                AddImGuiKeyEvent(builder, InputConstants.UI_PageDown, Keys.PageDown, ImGuiKey.PageDown);
                AddImGuiKeyEvent(builder, InputConstants.UI_Home, Keys.Home, ImGuiKey.Home);
                AddImGuiKeyEvent(builder, InputConstants.UI_End, Keys.End, ImGuiKey.End);
                AddImGuiKeyEvent(builder, InputConstants.UI_Delete, Keys.Delete, ImGuiKey.Delete);
                AddImGuiKeyEvent(builder, InputConstants.UI_Backspace, Keys.Back, ImGuiKey.Backspace);
                AddImGuiKeyEvent(builder, InputConstants.UI_Enter, Keys.Enter, ImGuiKey.Enter);
                AddImGuiKeyEvent(builder, InputConstants.UI_Escape, Keys.Escape, ImGuiKey.Escape);
                AddImGuiKeyEvent(builder, InputConstants.UI_Space, Keys.Space, ImGuiKey.Space);
                AddImGuiKeyEvent(builder, InputConstants.UI_A, Keys.A, ImGuiKey.A);
                AddImGuiKeyEvent(builder, InputConstants.UI_C, Keys.C, ImGuiKey.C);
                AddImGuiKeyEvent(builder, InputConstants.UI_V, Keys.V, ImGuiKey.V);
                AddImGuiKeyEvent(builder, InputConstants.UI_X, Keys.X, ImGuiKey.X);
                AddImGuiKeyEvent(builder, InputConstants.UI_Y, Keys.Y, ImGuiKey.Y);
                AddImGuiKeyEvent(builder, InputConstants.UI_Z, Keys.Z, ImGuiKey.Z);
                AddImGuiKeyEvent(builder, InputConstants.UI_LeftShift, Keys.LeftShift, ImGuiKey.LeftShift);
                AddImGuiKeyEvent(builder, InputConstants.UI_RightShift, Keys.RightShift, ImGuiKey.RightShift);
                AddImGuiKeyEvent(builder, InputConstants.UI_LeftCtrl, Keys.LeftControl, ImGuiKey.LeftCtrl);
                AddImGuiKeyEvent(builder, InputConstants.UI_RightCtrl, Keys.RightControl, ImGuiKey.RightCtrl);
                AddImGuiKeyEvent(builder, InputConstants.UI_LeftAlt, Keys.LeftAlt, ImGuiKey.LeftAlt);
                AddImGuiKeyEvent(builder, InputConstants.UI_RightAlt, Keys.RightAlt, ImGuiKey.RightAlt);
                AddImGuiKeyEvent(builder, InputConstants.UI_LeftSuper, Keys.LeftWindows, ImGuiKey.LeftSuper);
                AddImGuiKeyEvent(builder, InputConstants.UI_RightSuper, Keys.RightWindows, ImGuiKey.RightSuper);

                AddImGuiMouseButtonEvent(builder, InputConstants.UI_MouseButton01, CursorButtons.Left, 0);
                AddImGuiMouseButtonEvent(builder, InputConstants.UI_MouseButton02, CursorButtons.Middle, 1);
                AddImGuiMouseButtonEvent(builder, InputConstants.UI_MouseButton03, CursorButtons.Right, 2);
            });
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
