using System.Collections.Generic;
using System.Linq;

namespace GeneticSharp.Extensions
{
    /// <summary>
    /// This simple chromosome simply represents each cell by a gene with value between 1 and 9, accounting for the target mask if given
    /// </summary>
    /// 
    //sudokuboard -> sudokugrid
    public class SudokuCellsChromosome : SudokuChromosomeBase, ISudokuChromosome
    {

        public SudokuCellsChromosome() : this(null)
        {
        }

        /// <summary>
        /// Basic constructor with target sudoku to solve
        /// </summary>
        /// <param name="targetSudokuGrid">the target sudoku to solve</param>
        public SudokuCellsChromosome(SudokuGrid targetSudokuGrid) : this(targetSudokuGrid, null) { }

        /// <summary>
        /// Constructor with additional precomputed domains for faster cloning
        /// </summary>
        /// <param name="targetSudokuGrid">the target sudoku to solve</param>
        /// <param name="extendedMask">The cell domains after initial constraint propagation</param>
        public SudokuCellsChromosome(SudokuGrid targetSudokuGrid, Dictionary<int, List<int>> extendedMask) : base(targetSudokuGrid, extendedMask, 81)
        {
        }

        public override Gene GenerateGene(int geneIndex)
        {
            //If a target mask exist and has a digit for the cell, we use it.
            if (TargetSudokuGrid != null && TargetSudokuGrid.Cells[geneIndex] != 0)
            {
                return new Gene(TargetSudokuGrid.Cells[geneIndex]);
            }
            // otherwise we use a random digit amongts those permitted.
            var rnd = RandomizationProvider.Current;
            var targetIdx = rnd.GetInt(0, ExtendedMask[geneIndex].Count);
            return new Gene(ExtendedMask[geneIndex][targetIdx]);
        }

        public override IChromosome CreateNew()
        {
            return new SudokuCellsChromosome(TargetSudokuGrid, ExtendedMask);
        }

        /// <summary>
        /// Builds a single Sudoku from the 81 genes
        /// </summary>
        /// <returns>A Sudoku board built from the 81 genes</returns>
        public override IList<SudokuGrid> GetSudokus()
        {
            var sudoku = new SudokuGrid(GetGenes().Select(g => (int)g.Value));
            return new List<SudokuGrid>(new[] { sudoku });
        }
    }
}
