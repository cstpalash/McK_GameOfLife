using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCK.GameOfLife.Infrastructure;
using MCK.GameOfLife.Engine;
using MCK.GameOfLife.Engine.Grid;
using Microsoft.Practices.Unity;
using MCK.GameOfLife.Infrastructure.Grid;
using MCK.GameOfLife.Infrastructure.Rule;
using MCK.GameOfLife.Engine.Rule;
using MCK.GameOfLife.Infrastructure.View;
using MCK.GameOfLife.View;

namespace MCK.GameOfLife
{
    /// <summary>
    /// Program
    /// </summary>
    class Program
    {
        /// <summary>
        /// Mains the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        static void Main(string[] args)
        {
            //IoC container
            using (var container = new UnityContainer())
            {
                RegisterComponents(container);

                IMainView mainView = container.Resolve<IMainView>();
                mainView.Render();
            }
        }

        private static void RegisterComponents(IUnityContainer container)
        {
            container.RegisterType<ICell, Cell>();
            container.RegisterType<IGrid<ICell>, Grid>();

            //Wanted rules to be singleton
            container.RegisterType<IRule<IGrid<ICell>, ICell>, RuleOfLiveCell>("Live", new ContainerControlledLifetimeManager());
            container.RegisterType<IRule<IGrid<ICell>, ICell>, RuleOfDeadCell>("Dead", new ContainerControlledLifetimeManager());

            container.RegisterType<IStateGenerator, StateGenerator>();

            container.RegisterType<IMainView, MainView>();
            container.RegisterType<IGridView, GridView>();
        }
    }
}
