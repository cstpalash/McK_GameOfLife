using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCK.GameOfLife.Infrastructure.Rule;
using MCK.GameOfLife.Infrastructure.Grid;

namespace MCK.GameOfLife.Engine.Rule
{
    /// <summary>
    /// RuleOfLiveCell
    /// </summary>
    public class RuleOfLiveCell : IRule<IGrid<ICell>,ICell>
    {
        #region IRule members
        /// <summary>
        /// Applies rule for dead cell. If it has less than 2 or greater that 3 live cells, it will die.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="cell">The cell.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        public ICell Apply(IGrid<ICell> grid, ICell cell)
        {
            if (grid == null)
                throw new ArgumentNullException("grid");
            if (cell == null)
                throw new ArgumentNullException("cell");
            if (!cell.IsAlive)
                throw new InvalidOperationException("RuleOfLiveCell is valid for only live cell");

            ICell result = cell.Clone();

            int aliveNeighbourCount = grid.GetNeighbours(cell).Count(item => item.IsAlive);

            if (aliveNeighbourCount < 2 || aliveNeighbourCount > 3)
                result.IsAlive = false;

            return result;
        }
        #endregion
    }
}
