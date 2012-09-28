using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MCK.GameOfLife.Infrastructure.Grid;
using MCK.GameOfLife.Engine.Grid;
using Microsoft.Practices.Unity;

namespace MCK.GameOfLife.Engine.Test.Grid
{
    [TestFixture]
    public class CellTest
    {
        IUnityContainer container;

        [SetUp]
        public void SetUp()
        {
            container = new UnityContainer();
            container.RegisterType<ICell, Cell>();
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
        public void Test()
        {
            ICell original = container.Resolve<ICell>(new ParameterOverride("rowIndex", -1), new ParameterOverride("colIndex", 1));
        }

        [Test]
        public void CloneTest()
        {
            ICell original = container.Resolve<ICell>(new ParameterOverride("rowIndex", 0), new ParameterOverride("colIndex", 0));
            ICell clone = original.Clone();

            Assert.AreEqual(original.RowIndex, clone.RowIndex);
            Assert.AreEqual(original.ColIndex, clone.ColIndex);
            Assert.AreEqual(original.IsAlive, clone.IsAlive);

            Assert.AreNotEqual(original, clone);
        }
    }
}
