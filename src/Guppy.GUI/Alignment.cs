namespace Guppy.GUI
{
    public struct Alignment
    {
        public VerticalAlignment Vertical;
        public HorizontalAlignment Horizontal;

        public void Align(in RectangleF container, ref RectangleF element)
        {
            Alignment.Align(in container, ref element, this.Vertical, this.Horizontal);
        }

        public PointF Align(SizeF container, SizeF element)
        {
            return Alignment.Align(container, element, this.Vertical, this.Horizontal);
        }

        public static void Align(
            in RectangleF container,
            ref RectangleF element,
            VerticalAlignment vertical = VerticalAlignment.Center,
            HorizontalAlignment horizontal = HorizontalAlignment.Center)
        {
            PointF offset = Alignment.Align(container.Size, element.Size, vertical, horizontal);

            element.Location = new PointF()
            {
                X = container.Left + offset.X,
                Y = container.Top + offset.Y
            };
        }

        public static PointF Align(
            SizeF container,
            SizeF element, 
            VerticalAlignment vertical = VerticalAlignment.Center, 
            HorizontalAlignment horizontal = HorizontalAlignment.Center)
        {
            PointF result = PointF.Empty;

            switch(vertical)
            {
                case VerticalAlignment.Top:
                    result.Y = 0;
                    break;

                case VerticalAlignment.Center:
                    result.Y = (container.Height + element.Height) / 2;
                    break;

                case VerticalAlignment.Bottom:
                    result.Y = container.Height - element.Height;
                    break;
            }

            switch (horizontal)
            {
                case HorizontalAlignment.Left:
                    result.X = 0;
                    break;

                case HorizontalAlignment.Center:
                    result.X = (container.Width + element.Width) / 2;
                    break;

                case HorizontalAlignment.Right:
                    result.X = container.Width - element.Width;
                    break;
            }

            return result;
        }
    }
}
