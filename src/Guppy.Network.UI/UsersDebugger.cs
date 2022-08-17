using Guppy.MonoGame;
using Guppy.MonoGame.UI;
using Guppy.MonoGame.UI.Constants;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Providers;
using ImGuiNET;
using ImPlotNET;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;
using Num = System.Numerics;

namespace Guppy.Network.UI
{
    public class UsersDebugger : IImGuiDebugger
    {
        private IUserProvider _users;
        private Num.Vector4 _textColor;
        private ImFontPtr _font;
        private ImFontPtr _fontHeader;

        public UsersDebugger(IUserProvider users)
        {
            _users = users;
            _textColor = Color.White.ToNumericsVector4();
        }

        public void Initialize(ImGuiBatch imGuiBatch)
        {
            _fontHeader = imGuiBatch.Fonts[ImGuiFontConstants.DiagnosticsFontHeader].Ptr;
            _font = imGuiBatch.Fonts[ImGuiFontConstants.DiagnosticsFont].Ptr;
        }

        public void Draw(GameTime gameTime)
        {
            ImGui.PushStyleColor(ImGuiCol.Text, _textColor);
            ImGui.PushFont(_font);

            ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, new Num.Vector2(400, 0));
            if (ImGui.Begin($"Users ({_users.Count()})"))
            {
                foreach(User user in _users)
                {
                    if(ImGui.CollapsingHeader($"Id: {user.Id}, Claims: {user.Count()}{(_users.Current?.Id == user.Id ? " - Current User" : "")}"))
                    {
                        if(ImGui.BeginTable("claims", 4))
                        {
                            ImGui.PushFont(_fontHeader);
                            ImGui.TableNextColumn();
                            ImGui.Text("Key");
                            ImGui.TableNextColumn();
                            ImGui.Text("Value");
                            ImGui.TableNextColumn();
                            ImGui.Text("Type");
                            ImGui.TableNextColumn();
                            ImGui.Text("Accessibility");
                            ImGui.PopFont();

                            foreach (Claim claim in user)
                            {
                                ImGui.TableNextRow();
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{claim.Key}");
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{claim.GetValue()}");
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{claim.GetValue()?.GetType().Name ?? "null"}");
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{claim.Accessiblity}");
                            }
                        }
                        ImGui.EndTable();
                    }
                }
            }
            ImGui.End();
            ImGui.PopStyleVar();

            ImGui.PopFont();
            ImGui.PopStyleColor();
        }

        public void Update(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }
    }
}