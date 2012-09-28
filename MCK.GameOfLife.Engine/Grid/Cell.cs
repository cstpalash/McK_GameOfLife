using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCK.GameOfLife.Infrastructure.Grid;
using MCK.GameOfLife.Infrastructure.Rule;
using MCK.GameOfLife.Engine.Rule;
using Microsoft.Practices.Unity;

namespace MCK.GameOfLife.Engine.Grid
{
    public class Cell : ICell
    {
        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        private IUnityContainer Container { get; set; }

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Cell" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public Cell(IUnityContainer container, int rowIndex, int colIndex)
        {
            Container = container;
            if (rowIndex < 0 || colIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex and colIndex", "Should not be negative");

            RowIndex = rowIndex;
            ColIndex = colIndex;
        }
        #endregion

        #region ICell members
        /// <summary>
        /// Gets the index of the row.
        /// </summary>
        /// <value>
        /// The index of the row.
        /// </value>
        public int RowIndex { get; private set; }
        /// <summary>
        /// Gets the index of the col.
        /// </summary>
        /// <value>
        /// The index of the col.
        /// </value>
        public int ColIndex { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is alive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is alive; otherwise, <c>false</c>.
        /// </value>
        public bool IsAlive { get; set; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public ICell Clone()
        {
            ICell result = Container.Resolve<ICell>(new ParameterOverride("rowIndex", RowIndex), new ParameterOverride("colIndex", ColIndex));
            result.IsAlive = IsAlive;
            return result;
        }
        #endregion
    }
}
