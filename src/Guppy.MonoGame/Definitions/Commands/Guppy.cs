using Guppy.Attributes;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Binding;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuppyData = Guppy.MonoGame.Commands.Guppy;

namespace Guppy.MonoGame.Definitions.Commands
{
    [AutoLoad]
    public class Guppy : CommandDefinition<GuppyData>
    {
        public Option<int> Age { get; } = new Option<int>("--age");

        public override GuppyData BindData(BindingContext context)
        {
            var data =  new GuppyData(
                age: context.ParseResult.GetValueForOption(this.Age));

            return data;
        }
    }
}
