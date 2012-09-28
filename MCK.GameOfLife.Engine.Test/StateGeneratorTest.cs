using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Microsoft.Practices.Unity;
using MCK.GameOfLife.Infrastructure.Grid;
using MCK.GameOfLife.Engine.Grid;
using MCK.GameOfLife.Infrastructure;
using MCK.GameOfLife.Infrastructure.Rule;
using MCK.GameOfLife.Engine.Rule;

namespace MCK.GameOfLife.Engine.Test
{
    [TestFixture]
    public class StateGeneratorTest
    {
        IUnityContainer container;

        [SetUp]
        public void SetUp()
        {
            container = new UnityContainer();
            container.RegisterType<ICell, Cell>();
            container.RegisterType<IGrid<ICell>, MCK.GameOfLife.Engine.Grid.Grid>();
            container.RegisterType<IStateGenerator, StateGenerator>();

            //Wanted rules to be singleton
            container.RegisterType<IRule<IGrid<ICell>, ICell>, RuleOfLiveCell>("Live", new ContainerControlledLifetimeManager());
            container.RegisterType<IRule<IGrid<ICell>, ICell>, RuleOfDeadCell>("Dead", new ContainerControlledLifetimeManager());
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
        public void TestGoToNextState()
        {
            ICell cell1 = container.Resolve<ICell>(new ParameterOverride("rowIndex", 0), new ParameterOverride("colIndex", 1));
            ICell cell2 = container.Resolve<ICell>(new ParameterOverride("rowIndex", 1), new ParameterOverride("colIndex", 1));
            ICell cell3 = container.Resolve<ICell>(new ParameterOverride("rowIndex", 2), new ParameterOverride("colIndex", 1));
            cell1.IsAlive = cell2.IsAlive = cell3.IsAlive = true;
            IGrid<ICell> grid = container.Resolve<IGrid<ICell>>(
                new ParameterOverride("totalRows", 3),
                new ParameterOverride("totalColumns", 3),
                new ParameterOverride("initialActiveCells", new ICell[] { cell1, cell2, cell3 }));

            IStateGenerator stateGenerator = container.Resolve<IStateGenerator>(new ParameterOverride("currentState", grid));
            stateGenerator.GoToNextState();
            
            Assert.AreEqual(stateGenerator.CurrentState[1, 0].IsAlive, true);
            Assert.AreEqual(stateGenerator.CurrentState[1, 1].IsAlive, true);
            Assert.AreEqual(stateGenerator.CurrentState[1, 2].IsAlive, true);

            Assert.AreEqual(stateGenerator.CurrentState[0, 0].IsAlive, false);
            Assert.AreEqual(stateGenerator.CurrentState[0, 1].IsAlive, false);
            Assert.AreEqual(stateGenerator.CurrentState[0, 2].IsAlive, false);

            Assert.AreEqual(stateGenerator.CurrentState[2, 0].IsAlive, false);
            Assert.AreEqual(stateGenerator.CurrentState[2, 1].IsAlive, false);
            Assert.AreEqual(stateGenerator.CurrentState[2, 2].IsAlive, false);

            stateGenerator.GoToNextState();

            Assert.AreEqual(stateGenerator.CurrentState[0, 1].IsAlive, true);
            Assert.AreEqual(stateGenerator.CurrentState[1, 1].IsAlive, true);
            Assert.AreEqual(stateGenerator.CurrentState[2, 1].IsAlive, true);

            Assert.AreEqual(stateGenerator.CurrentState[0, 0].IsAlive, false);
            Assert.AreEqual(stateGenerator.CurrentState[0, 2].IsAlive, false);

            Assert.AreEqual(stateGenerator.CurrentState[1, 0].IsAlive, false);
            Assert.AreEqual(stateGenerator.CurrentState[1, 2].IsAlive, false);

            Assert.AreEqual(stateGenerator.CurrentState[2, 0].IsAlive, false);
            Assert.AreEqual(stateGenerator.CurrentState[2, 2].IsAlive, false);
        }
    }
}
