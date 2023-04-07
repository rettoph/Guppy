using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public class Padding
    {
        public readonly Unit Top;
        public readonly Unit Right;
        public readonly Unit Bottom;
        public readonly Unit Left;

        public Padding(Unit top, Unit right, Unit bottom, Unit left)
        {
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.Left = left;
        }

        public void AddPadding(in Rectangle bounds, out Rectangle result)
        {
            int top = this.Top.Calculate(bounds.Height);
            int right = this.Right.Calculate(bounds.Width);
            int bottom = this.Bottom.Calculate(bounds.Height);
            int left = this.Left.Calculate(bounds.Width);

            result.Y = bounds.Y + top;
            result.X = bounds.X + left;
            result.Height = bounds.Height - top - bottom;
            result.Width = bounds.Width - left - right;
        }

        public void AddPadding(ref Rectangle bounds)
        {
            this.AddPadding(in bounds, out bounds);
        }

        public void RemovePadding(in Rectangle bounds, out Rectangle result)
        {
            int top = this.Top.Calculate(bounds.Height);
            int right = this.Right.Calculate(bounds.Width);
            int bottom = this.Bottom.Calculate(bounds.Height);
            int left = this.Left.Calculate(bounds.Width);

            result.Y = bounds.Y - top;
            result.X = bounds.X - left;
            result.Height = bounds.Height + top + bottom;
            result.Width = bounds.Width + left + right;
        }

        public void RemovePadding(ref Rectangle bounds)
        {
            this.RemovePadding(in bounds, out bounds);
        }

        public int Horizontal(int parent)
        {
            return this.Left.Calculate(parent) + this.Right.Calculate(parent);
        }

        public int Vertical(int parent)
        {
            return this.Top.Calculate(parent) + this.Bottom.Calculate(parent);
        }
    }
}
