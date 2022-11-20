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
        private ImFontPtr _fontHeader;
        private bool _open;

        public string Label { get; }

        public bool Open
        {
            get => _open;
            set => _open = value;
        }

        public UsersDebugger(IUserProvider users)
        {
            _users = users;
            _open = false;

            this.Label = "Users";
        }

        public void Initialize(ImGuiBatch imGuiBatch)
        {
            _fontHeader = imGuiBatch.Fonts[ImGuiFontConstants.DiagnosticsFontHeader].Ptr;
        }

        public void Draw(GameTime gameTime)
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, new Num.Vector2(400, 0));
            if (ImGui.Begin($"Users ({_users.Count()})", ref _open))
            {
                foreach (User user in _users)
                {
                    if (ImGui.CollapsingHeader($"Id: {user.Id}, Claims: {user.Count()}{(_users.Current?.Id == user.Id ? " - Current User" : "")}"))
                    {
                        if (ImGui.BeginTable("claims", 4))
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
        }

        public void Update(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }
    }
}