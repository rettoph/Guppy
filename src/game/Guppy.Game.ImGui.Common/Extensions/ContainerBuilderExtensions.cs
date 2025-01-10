using System.Text.Json.Serialization;
using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Common.Helpers;
using Guppy.Core.Files.Common.Helpers;
using Guppy.Core.Serialization.Common.Extensions;
using Guppy.Core.Serialization.Json.Converters;
using Guppy.Game.ImGui.Common.Constants;
using Guppy.Game.ImGui.Common.Messages;
using Guppy.Game.ImGui.Common.Serialization.Json.Converters;
using Guppy.Game.ImGui.Common.Services;
using Guppy.Game.ImGui.Common.Styling.StyleValues;
using Guppy.Game.Input.Common.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Game.ImGui.Common.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCommonImGuiServices(this ContainerBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCommonImGuiServices), builder =>
                                                                                                              {
                                                                                                                  string nativesDirectory = DirectoryHelper.Combine(DirectoryHelper.GetEntryDirectory(), ImGuiNatives.Directory);
                                                                                                                  NativeHelper.Load(nativesDirectory, ImGuiNatives.cImGui, ImGuiNatives.cImPlot);

                                                                                                                  builder.RegisterType<ImGuiObjectExplorerService>().As<IImGuiObjectExplorerService>().SingleInstance();
                                                                                                                  builder.RegisterType<DefaultImGuiObjectExplorer>().AsSelf().As<ImGuiObjectExplorer>().SingleInstance();

                                                                                                                  builder.RegisterJsonConverter<ImStyleConverter>();
                                                                                                                  builder.RegisterJsonConverter<PolymorphicConverter<ImStyleValue>>();
                                                                                                                  builder.RegisterJsonConverter<ImStyleVarFloatValueConverter>();
                                                                                                                  builder.RegisterJsonConverter<ImStyleVarVector2ValueConverter>();
                                                                                                                  builder.RegisterJsonConverter<ImStyleColorValueConverter>();
                                                                                                                  builder.RegisterJsonConverter<ImStyleFontValueConverter>();

                                                                                                                  builder.RegisterPolymorphicJsonType<ImStyleColorValue, ImStyleValue>(nameof(Color));
                                                                                                                  builder.RegisterPolymorphicJsonType<ImStyleFontValue, ImStyleValue>(nameof(TrueTypeFont));
                                                                                                                  builder.RegisterPolymorphicJsonType<ImStyleVarFloatValue, ImStyleValue>(nameof(Single));
                                                                                                                  builder.RegisterPolymorphicJsonType<ImStyleVarVector2Value, ImStyleValue>(nameof(Vector2));

                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_Tab, Keys.Tab, ImGuiKey.Tab);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_LeftArrow, Keys.Left, ImGuiKey.LeftArrow);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_RightArrow, Keys.Right, ImGuiKey.RightArrow);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_UpArrow, Keys.Up, ImGuiKey.UpArrow);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_DownArrow, Keys.Down, ImGuiKey.DownArrow);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_PageUp, Keys.PageUp, ImGuiKey.PageUp);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_PageDown, Keys.PageDown, ImGuiKey.PageDown);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_Home, Keys.Home, ImGuiKey.Home);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_End, Keys.End, ImGuiKey.End);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_Delete, Keys.Delete, ImGuiKey.Delete);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_Backspace, Keys.Back, ImGuiKey.Backspace);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_Enter, Keys.Enter, ImGuiKey.Enter);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_Escape, Keys.Escape, ImGuiKey.Escape);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_Space, Keys.Space, ImGuiKey.Space);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_A, Keys.A, ImGuiKey.A);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_C, Keys.C, ImGuiKey.C);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_V, Keys.V, ImGuiKey.V);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_X, Keys.X, ImGuiKey.X);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_Y, Keys.Y, ImGuiKey.Y);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_Z, Keys.Z, ImGuiKey.Z);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_LeftShift, Keys.LeftShift, ImGuiKey.LeftShift);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_RightShift, Keys.RightShift, ImGuiKey.RightShift);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_LeftCtrl, Keys.LeftControl, ImGuiKey.LeftCtrl);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_RightCtrl, Keys.RightControl, ImGuiKey.RightCtrl);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_LeftAlt, Keys.LeftAlt, ImGuiKey.LeftAlt);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_RightAlt, Keys.RightAlt, ImGuiKey.RightAlt);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_LeftSuper, Keys.LeftWindows, ImGuiKey.LeftSuper);
                                                                                                                  AddImGuiKeyEvent(builder, ImGuiInputs.UI_RightSuper, Keys.RightWindows, ImGuiKey.RightSuper);

                                                                                                                  AddImGuiMouseButtonEvent(builder, ImGuiInputs.UI_MouseButton01, CursorButtonsEnum.Left, 0);
                                                                                                                  AddImGuiMouseButtonEvent(builder, ImGuiInputs.UI_MouseButton02, CursorButtonsEnum.Middle, 1);
                                                                                                                  AddImGuiMouseButtonEvent(builder, ImGuiInputs.UI_MouseButton03, CursorButtonsEnum.Right, 2);
                                                                                                              });
        }

        private static void AddImGuiKeyEvent(ContainerBuilder services, string key, Keys defaultKey, ImGuiKey mapping)
        {
            services.RegisterInput(
                key,
                defaultKey,
                [
                    (ButtonState.Pressed, new ImGuiKeyEvent(mapping, true)),
                    (ButtonState.Released, new ImGuiKeyEvent(mapping, false))
                ]);
        }

        private static void AddImGuiMouseButtonEvent(ContainerBuilder services, string key, CursorButtonsEnum defaultButton, int mapping)
        {
            services.RegisterInput(
                key,
                defaultButton,
                [
                    (ButtonState.Pressed, new ImGuiMouseButtonEvent(mapping, true)),
                    (ButtonState.Released, new ImGuiMouseButtonEvent(mapping, false))
                ]);
        }
    }
}