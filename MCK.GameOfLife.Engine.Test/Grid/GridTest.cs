using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Microsoft.Practices.Unity;
using MCK.GameOfLife.Infrastructure.Grid;
using MCK.GameOfLife.Engine.Grid;

namespace MCK.GameOfLife.Engine.Test.Grid
{
    [TestFixture]
    public class GridTest
    {
        IUnityContainer container;

        [SetUp]
        public void SetUp()
        {
            container = new UnityContainer();
            container.RegisterType<ICell, Cell>();
            container.RegisterType<IGrid<ICell>, MCK.GameOfLife.Engine.Grid.Grid>();
        }

        [TearDown]
        public void TearDown()
        {
            if (container != null)
            {
                container.Dispose();
                container = null;
            }
        }

        [Test]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void TestTotalRowsColumns()
        {
            ICell cell = container.Resolve<ICell>(new ParameterOverride("rowIndex", 0), new ParameterOverride("colIndex", 0));
            IGrid<ICell> grid = container.Resolve<IGrid<ICell>>(
                new ParameterOverride("totalRows", 1),
                new ParameterOverride("totalColumns", 1),
                new ParameterOverride("initialActiveCells", new ICell[] { cell }));
        }

        [Test]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void TestInitialActiveCells()
        {
            ICell cell = container.Resolve<ICell>(new ParameterOverride("rowIndex", 3), new ParameterOverride("colIndex", 0));
            IGrid<ICell> grid = container.Resolve<IGrid<ICell>>(
                new ParameterOverride("totalRows", 3),
                new ParameterOverride("totalColumns", 3),
                new ParameterOverride("initialActiveCells", new ICell[] { cell }));
        }

        [Test]
        public void TestIndexer()
        {
            ICell cell1 = container.Resolve<ICell>(new ParameterOverride("rowIndex", 0), new ParameterOverride("colIndex", 0));
            ICell cell2 = container.Resolve<ICell>(new ParameterOverride("rowIndex", 1), new ParameterOverride("colIndex", 1));
            ICell cell3 = container.Resolve<ICell>(new ParameterOverride("rowIndex", 2), new ParameterOverride("colIndex", 2));
            IGrid<ICell> grid = container.Resolve<IGrid<ICell>>(
                new ParameterOverride("totalRows", 3),
                new ParameterOverride("totalColumns", 3),
                new ParameterOverride("initialActiveCells", new ICell[] { cell1, cell2, cell3 }));

            Assert.Throws<ArgumentOutOfRangeException>(() => grid[3, 3].IsAlive = false);
            Assert.AreEqual(cell1, grid[0, 0]);
            Assert.AreEqual(cell2, grid[1, 1]);
            Assert.AreEqual(cell3, grid[2, 2]);
        }

        [Test]
        public void TestGetNeighbours()
        {
            ICell cell1 = container.Resolve<ICell>(new ParameterOverride("rowIndex", 0), new ParameterOverride("colIndex", 0));
            ICell cell2 = container.Resolve<ICell>(new ParameterOverride("rowIndex", 1), new ParameterOverride("colIndex", 1));
            ICell cell3 = container.Resolve<ICell>(new ParameterOverride("rowIndex", 2), new ParameterOverride("colIndex", 2));
            IGrid<ICell> grid = container.Resolve<IGrid<ICell>>(
                new ParameterOverride("totalRows", 3),
                new ParameterOverride("totalColumns", 3),
                new ParameterOverride("initialActiveCells", new ICell[] { cell1, cell2, cell3 }));

            ICell outOfRangeCell = container.Resolve<ICell>(new ParameterOverride("rowIndex", 3), new ParameterOverride("colIndex", 3));
            Assert.Throws<ArgumentNullException>(() => grid.GetNeighbours(null).Count());
            Assert.Throws<ArgumentOutOfRangeException>(() => grid.GetNeighbours(outOfRangeCell).Count());

            IEnumerable<ICell> neighbours = grid.GetNeighbours(cell1);
            Assert.That(neighbours.Count(), Is.EqualTo(3), "Must have 3 neighbours");

            CollectionAssert.Contains(neighbours, grid[0, 1]);
            CollectionAssert.Contains(neighbours, grid[1, 1]);
            CollectionAssert.Contains(neighbours, grid[1, 0]);
        }
    }
}
