using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.Entities
{
    public readonly ref struct CellPair
    {
        public readonly ref Cell Input;
        public readonly ref Cell Output;

        public CellTypeEnum OutputType => this.Output.Updated ? this.Output.Type : this.Input.Type;

        private CellPair(ref Cell input, ref Cell output)
        {
            this.Input = ref input;
            this.Output = ref output;
        }

        public bool Either(CellTypeEnum type) => this.Input.Type == type || this.Output.Type == type;

        public bool Both(CellTypeEnum type) => this.Input.Type == type && this.Output.Type == type;

        public bool BothIn(CellTypeEnum flags)
        {
            bool result = flags.HasFlag(this.Input.Type) && flags.HasFlag(this.Output.Type);
            return result;
        }

        public static void Create(Grid input, Grid output, int index, out CellPair pair) => pair = new CellPair(ref input.GetCell(index), ref output.GetCell(index));
    }
}