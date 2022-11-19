using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Num = System.Numerics;

namespace Guppy.MonoGame.UI.Elements
{
    public sealed class Table : Container<ImGuiTableFlags>
    {
        public int Columns;
        public Num.Vector2 OuterSize;
        public float InnerWidth;

        public Table(string name, int columns) : base(name)
        {
            this.Columns = columns;
        }

        protected override bool BeginDrawContainer()
        {
            return ImGui.BeginTable(this.Name, this.Columns, this.Flags, this.OuterSize, this.InnerWidth);
        }

        protected override void EndDrawContainer()
        {
            ImGui.EndTable();
        }

        protected override void DrawChildren(GameTime gameTime)
        {
            int currentColumn = 0;

            foreach(Element child in this.Children)
            {
                if (currentColumn == this.Columns)
                {
                    ImGui.TableNextRow();
                    currentColumn = 0;
                }

                ImGui.TableNextColumn();
                child.Draw(gameTime);
                currentColumn++;
            }
        }
    }
}
