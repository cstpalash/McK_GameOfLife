using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCK.GameOfLife.Infrastructure.Grid;

namespace MCK.GameOfLife.Infrastructure.Rule
{
    /// <summary>
    /// IRule
    /// </summary>
    /// <typeparam name="TG">The type of the G.</typeparam>
    /// <typeparam name="TC">The type of the C.</typeparam>
    public interface IRule<TG, TC> where TG : IGrid<ICell> where TC : ICell
    {
        /// <summary>
        /// Applies the specified grid.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="cell">The cell.</param>
        /// <returns></returns>
        ICell Apply(TG grid, TC cell);
    }
}
