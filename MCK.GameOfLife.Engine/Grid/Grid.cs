using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCK.GameOfLife.Infrastructure.Grid;
using System.Globalization;
using Microsoft.Practices.Unity;

namespace MCK.GameOfLife.Engine.Grid
{
    public class Grid : IGrid<ICell>
    {
        #region Private members
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        private IUnityContainer Container { get; set; }
        /// <summary>
        /// storage
        /// </summary>
        private readonly ICell[,] storage;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Grid" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="totalRows">The total rows.</param>
        /// <param name="totalColumns">The total columns.</param>
        /// <param name="initialActiveCells">The initial active cells.</param>
        public Grid(IUnityContainer container, int totalRows, int totalColumns, ICell[] initialActiveCells)
        {
            Container = container;
            ValidateGrid(totalRows, totalColumns, initialActiveCells);

            TotalRows = totalRows;
            TotalColumns = totalColumns;

            storage = new ICell[TotalRows, TotalColumns];

            Initialize(initialActiveCells);
        }
        #endregion

        #region IGrid members
        /// <summary>
        /// Gets the total rows.
        /// </summary>
        /// <value>
        /// The total rows.
        /// </value>
        public int TotalRows { get; private set; }
        /// <summary>
        /// Gets the total columns.
        /// </summary>
        /// <value>
        /// The total columns.
        /// </value>
        public int TotalColumns { get; private set; }
        /// <summary>
        /// Gets the neighbours.
        /// </summary>
        /// <param name="currentCell">The current cell.</param>
        /// <returns></returns>
        public IEnumerable<ICell> GetNeighbours(ICell currentCell)
        {
            ValidateCell(currentCell);

            for (int row = currentCell.RowIndex - 1; row <= currentCell.RowIndex + 1; row++)
            {
                if (row < 0 || row >= TotalRows) continue;
                for (int col = currentCell.ColIndex - 1; col <= currentCell.ColIndex + 1; col++)
                {
                    if (col < 0 || col >= TotalColumns) continue;
                    if (row == currentCell.RowIndex && col == currentCell.ColIndex) continue;
                    yield return storage[row, col];
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ICell" /> with the specified i.
        /// </summary>
        /// <value>
        /// The <see cref="ICell" />.
        /// </value>
        /// <param name="i">The i.</param>
        /// <param name="j">The j.</param>
        /// <returns></returns>
        public ICell this[int i, int j]
        {
            get
            {
                ValidateIndexer(i, j);
                return storage[i, j];
            }
            set
            {
                ValidateIndexer(i, j);
                storage[i, j] = value;
            }
        }
        #endregion

        #region Validation
        /// <summary>
        /// Validates the grid.
        /// </summary>
        /// <param name="totalRows">The total rows.</param>
        /// <param name="totalColumns">The total columns.</param>
        /// <param name="initialActiveCells">The initial active cells.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void ValidateGrid(int totalRows, int totalColumns, ICell[] initialActiveCells)
        {
            if (totalRows <= 1 || totalColumns <= 1)
                throw new ArgumentOutOfRangeException("totalRows and totalColumns", "Should be more than 1");

            if (initialActiveCells != null)
            {
                foreach (var item in initialActiveCells)
                {
                    if (item.RowIndex < 0 || item.ColIndex < 0 || item.RowIndex >= totalRows || item.ColIndex >= totalColumns)
                        throw new ArgumentOutOfRangeException("initialActiveCells",
                            string.Format(CultureInfo.CurrentCulture,
                            "Valid ranges of RowIndex and ColumnIndex should be {0}-{1} and {2}-{3} respectively",
                            0, totalRows -1 , 0, totalColumns - 1));
                }
            }
        }

        /// <summary>
        /// Validates the cell.
        /// </summary>
        /// <param name="currentCell">The current cell.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void ValidateCell(ICell currentCell)
        {
            if (currentCell == null)
                throw new ArgumentNullException("currentCell");
            if (currentCell.RowIndex < 0 || currentCell.ColIndex < 0 || currentCell.RowIndex >= TotalRows || currentCell.ColIndex >= TotalColumns)
                throw new ArgumentOutOfRangeException("currentCell",
                    string.Format(CultureInfo.CurrentCulture,
                    "Valid ranges of RowIndex and ColumnIndex should be {0}-{1} and {2}-{3} respectively",
                    0, TotalRows - 1, 0, TotalColumns - 1));
        }

        /// <summary>
        /// Validates the indexer.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void ValidateIndexer(int rowIndex, int colIndex)
        {
            if (rowIndex < 0 || colIndex < 0 || rowIndex >= TotalRows || colIndex >= TotalColumns)
                throw new ArgumentOutOfRangeException("Indexer",
                    string.Format(CultureInfo.CurrentCulture,
                    "Valid ranges of RowIndex and ColumnIndex should be {0}-{1} and {2}-{3} respectively",
                    0, TotalRows, 0, TotalColumns));
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the specified initial active cells.
        /// </summary>
        /// <param name="initialActiveCells">The initial active cells.</param>
        private void Initialize(ICell[] initialActiveCells)
        {
            for (int row = 0; row < TotalRows; row++)
                for (int col = 0; col < TotalColumns; col++)
                    storage[row, col] = Container.Resolve<ICell>(new ParameterOverride("rowIndex", row), new ParameterOverride("colIndex", col));

            if (initialActiveCells != null)
                foreach (var item in initialActiveCells)
                    storage[item.RowIndex, item.ColIndex] = item;
        }
        #endregion
    }
}
