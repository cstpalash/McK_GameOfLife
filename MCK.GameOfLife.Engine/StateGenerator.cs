using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCK.GameOfLife.Infrastructure.Grid;
using MCK.GameOfLife.Infrastructure.Rule;
using MCK.GameOfLife.Engine.Rule;
using MCK.GameOfLife.Infrastructure;
using Microsoft.Practices.Unity;

namespace MCK.GameOfLife.Engine
{
    /// <summary>
    /// StateGenerator
    /// </summary>
    public class StateGenerator : IStateGenerator
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        private IUnityContainer Container { get; set; }

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StateGenerator" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="currentState">State of the current.</param>
        public StateGenerator(IUnityContainer container, IGrid<ICell> currentState)
        {
            Container = container;
            CurrentState = currentState;
        }
        #endregion

        #region IStateGenerator members
        /// <summary>
        /// Gets the state of the current.
        /// </summary>
        /// <value>
        /// The state of the current.
        /// </value>
        public IGrid<ICell> CurrentState { get; private set; }

        /// <summary>
        /// Goes the state of to next.
        /// </summary>
        public void GoToNextState()
        {
            IGrid<ICell> temp = Container.Resolve<IGrid<ICell>>(
                    new ParameterOverride("totalRows", CurrentState.TotalRows),
                    new ParameterOverride("totalColumns", CurrentState.TotalColumns));
            
            for (int row = 0; row < CurrentState.TotalRows; row++)
            {
                for (int col = 0; col < CurrentState.TotalColumns; col++)
                {
                    temp[row, col] = GetRule(CurrentState[row, col]).Apply(CurrentState, CurrentState[row, col]);
                }
            }

            CurrentState = temp;
        }
        #endregion

        /// <summary>
        /// Gets the rule.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns></returns>
        private IRule<IGrid<ICell>, ICell> GetRule(ICell cell)
        {
            //These are singleton rules, no harm to resolve multiple times
            if (cell.IsAlive)
                return Container.Resolve<IRule<IGrid<ICell>, ICell>>("Live");
            else
                return Container.Resolve<IRule<IGrid<ICell>, ICell>>("Dead");
        }
    }
}
