using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCK.GameOfLife.Infrastructure.Grid
{
    /// <summary>
    /// IGrid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGrid<T> where T : ICell
    {
        /// <summary>
        /// Gets or sets the <see cref="ICell" /> with the specified i.
        /// </summary>
        /// <value>
        /// The <see cref="ICell" />.
        /// </value>
        /// <param name="i">The i.</param>
        /// <param name="j">The j.</param>
        /// <returns></returns>
        ICell this[int i, int j] { get; set; }
        /// <summary>
        /// Gets the total rows.
        /// </summary>
        /// <value>
        /// The total rows.
        /// </value>
        int TotalRows { get; }
        /// <summary>
        /// Gets the total columns.
        /// </summary>
        /// <value>
        /// The total columns.
        /// </value>
        int TotalColumns { get; }
        /// <summary>
        /// Gets the neighbours.
        /// </summary>
        /// <param name="currentCell">The current cell.</param>
        /// <returns></returns>
        IEnumerable<ICell> GetNeighbours(ICell currentCell);
    }
}
