using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCK.GameOfLife.Infrastructure.Grid;
using MCK.GameOfLife.Infrastructure.Rule;
using MCK.GameOfLife.Engine.Rule;
using MCK.GameOfLife.Infrastructure;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using MCK.GameOfLife.Engine.Grid;

namespace MCK.GameOfLife.Engine.Test.Rule
{
    [TestFixture]
    public class RuleOfLiveCellTest
    {
        IUnityContainer container;

        [SetUp]
        public void SetUp()
        {
            container = new UnityContainer();
            container.RegisterType<ICell, Cell>();
            container.RegisterType<IGrid<ICell>, MCK.GameOfLife.Engine.Grid.Grid>();

            //Wanted rules to be singleton
            container.RegisterType<IRule<IGrid<ICell>, ICell>, RuleOfLiveCell>("Live", new ContainerControlledLifetimeManager());
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
        public void TestApply()
        {
            ICell cell1 = container.Resolve<ICell>(new ParameterOverride("rowIndex", 0), new ParameterOverride("colIndex", 1));
            ICell cell2 = container.Resolve<ICell>(new ParameterOverride("rowIndex", 1), new ParameterOverride("colIndex", 1));
            ICell cell3 = container.Resolve<ICell>(new ParameterOverride("rowIndex", 2), new ParameterOverride("colIndex", 1));
            cell1.IsAlive = cell2.IsAlive = cell3.IsAlive = true;
            IGrid<ICell> grid = container.Resolve<IGrid<ICell>>(
                new ParameterOverride("totalRows", 3),
                new ParameterOverride("totalColumns", 3),
                new ParameterOverride("initialActiveCells", new ICell[] { cell1, cell2, cell3 }));

            IRule<IGrid<ICell>, ICell> rule = container.Resolve<IRule<IGrid<ICell>, ICell>>("Live");

            Assert.Throws<ArgumentNullException>(() => rule.Apply(null, cell1));
            Assert.Throws<ArgumentNullException>(() => rule.Apply(grid, null));
            Assert.Throws<InvalidOperationException>(() => rule.Apply(grid, grid[0,0]));

            ICell result = rule.Apply(grid, grid[0,1]);
            Assert.AreEqual(result.IsAlive, false);

            result = rule.Apply(grid, grid[1,1]);
            Assert.AreEqual(result.IsAlive, true);
        }
    }
}
