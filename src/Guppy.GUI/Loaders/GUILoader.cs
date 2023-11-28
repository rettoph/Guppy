using Autofac;
using Guppy.Attributes;
using Guppy.Common.Helpers;
using Guppy.GUI.Constants;
using Guppy.GUI.Messages;
using Guppy.GUI.Serialization.Json.Converters;
using Guppy.GUI.Styling.StyleValueResources;
using Guppy.Input.Enums;
using Guppy.Loaders;
using Guppy.Resources.Serialization.Json.Converters;
using ImGuiNET;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.GUI.Loaders
{
    internal sealed class GUILoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            string nativesDirectory = Path.Combine(Directory.GetCurrentDirectory(), NativeConstants.Directory);
            NativeHelper.Load(nativesDirectory, NativeConstants.cImGui, NativeConstants.cImPlot);

            services.RegisterType<ImGuiBatch>().As<ImGuiBatch>().SingleInstance();
            services.RegisterType<Gui>().As<IGui>().SingleInstance();

            services.RegisterType<StyleConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<PolymorphicConverter<StyleValue>>().As<JsonConverter>().SingleInstance();
            services.RegisterType<StyleVarFloatValueConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<StyleVarVector2ValueConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<StyleColorValueConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<StyleFontValueConverter>().As<JsonConverter>().SingleInstance();

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
            services.AddInput(
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
            services.AddInput(
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
