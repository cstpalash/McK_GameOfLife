using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCK.GameOfLife.Infrastructure.Grid
{
    /// <summary>
    /// ICell
    /// </summary>
    public interface ICell
    {
        /// <summary>
        /// Gets the index of the row.
        /// </summary>
        /// <value>
        /// The index of the row.
        /// </value>
        int RowIndex { get; }
        /// <summary>
        /// Gets the index of the col.
        /// </summary>
        /// <value>
        /// The index of the col.
        /// </value>
        int ColIndex { get; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is alive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is alive; otherwise, <c>false</c>.
        /// </value>
        bool IsAlive { get; set; }
        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        ICell Clone();
    }
}
