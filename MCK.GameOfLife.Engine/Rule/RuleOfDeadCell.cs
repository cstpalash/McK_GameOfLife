using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCK.GameOfLife.Infrastructure.Rule;
using MCK.GameOfLife.Infrastructure.Grid;

namespace MCK.GameOfLife.Engine.Rule
{
    /// <summary>
    /// RuleOfDeadCell
    /// </summary>
    public class RuleOfDeadCell : IRule<IGrid<ICell>, ICell>
    {
        #region IRule members
        /// <summary>
        /// Applies rule for dead cell. If it has exactly 3 live cell, it will get alive.
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
            if (cell.IsAlive)
                throw new InvalidOperationException("RuleOfDeadCell is valid for only dead cell");

            ICell result = cell.Clone();

            int aliveNeighbourCount = grid.GetNeighbours(cell).Count(item => item.IsAlive);

            if (aliveNeighbourCount == 3)
                result.IsAlive = true;

            return result;
        }
        #endregion
    }
}
