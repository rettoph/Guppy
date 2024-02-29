// using Guppy.Example.Client.Entities;
// using Guppy.Example.Client.Enums;
// 
// namespace Guppy.Example.Client.CellTypes
// {
//     internal abstract class BaseParticleCellType : BaseGravityCellType
//     {
//         protected override bool Step(ref Cell input, Grid output)
//         {
//             if (base.Step(ref input, output) == false)
//             {
//                 // Try to swap with cell below if it is water/liquid
//                 ref Cell belowInput = ref input.GetNeighbor(0, 1);
//                 ref Cell belowOutput = ref output.GetCell(belowInput.Index);
//                 if (belowInput.CurrentType == CellTypeEnum.Water && (belowOutput.CurrentType == CellTypeEnum.Water || belowOutput.CurrentType == CellTypeEnum.Air))
//                 {
//                     output.Cells[input.Index].Update(CellTypeEnum.Water, true);
//                     belowOutput.Update(input.CurrentType, true);
// 
//                     return true;
//                 }
// 
//                 return false;
//             }
// 
//             return true;
//         }
//     }
// }
