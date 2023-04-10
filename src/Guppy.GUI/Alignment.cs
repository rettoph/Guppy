using System.Diagnostics;

namespace Guppy.GUI
{
    public struct Alignment
    {
        public static readonly Alignment TopLeft = new Alignment(VerticalAlignment.Top, HorizontalAlignment.Left);
        public static readonly Alignment TopCenter = new Alignment(VerticalAlignment.Top, HorizontalAlignment.Center);
        public static readonly Alignment TopRight = new Alignment(VerticalAlignment.Top, HorizontalAlignment.Right);
        public static readonly Alignment CenterLeft = new Alignment(VerticalAlignment.Center, HorizontalAlignment.Left);
        public static readonly Alignment Center = new Alignment(VerticalAlignment.Center, HorizontalAlignment.Center);
        public static readonly Alignment CenterRight = new Alignment(VerticalAlignment.Center, HorizontalAlignment.Right);
        public static readonly Alignment BottomLeft = new Alignment(VerticalAlignment.Bottom, HorizontalAlignment.Left);
        public static readonly Alignment BottomCenter = new Alignment(VerticalAlignment.Bottom, HorizontalAlignment.Center);
        public static readonly Alignment BottomRight = new Alignment(VerticalAlignment.Bottom, HorizontalAlignment.Right);


        public VerticalAlignment Vertical;
        public HorizontalAlignment Horizontal;

        public Alignment(VerticalAlignment vertical, HorizontalAlignment horizontal)
        {
            this.Vertical = vertical;
            this.Horizontal = horizontal;
        }

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
            return new PointF()
            {
                X = Alignment.AlignHorizontal(container.Width, element.Width, horizontal),
                Y = Alignment.AlignVertical(container.Height, element.Height, vertical),
            };
        }

        public static float AlignVertical(float container, float element, VerticalAlignment alignment)
        {
            switch (alignment)
            {
                case VerticalAlignment.Top:
                    return 0;

                case VerticalAlignment.Center:
                    return (container - element) / 2;

                case VerticalAlignment.Bottom:
                    return container - element;

                default:
                    throw new UnreachableException();
            }
        }


        public static float AlignHorizontal(float container, float element, HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Left:
                    return 0;

                case HorizontalAlignment.Center:
                    return (container - element) / 2;

                case HorizontalAlignment.Right:
                    return container - element;

                case HorizontalAlignment.LeftFit:
                    return container < element ? container - element : 0;

                default:
                    throw new UnreachableException();
            }
        }
    }
}
