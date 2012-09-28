using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCK.GameOfLife.Infrastructure.Grid;

namespace MCK.GameOfLife.Infrastructure
{
    /// <summary>
    /// IStateGenerator
    /// </summary>
    public interface IStateGenerator
    {
        /// <summary>
        /// Gets the state of the current.
        /// </summary>
        /// <value>
        /// The state of the current.
        /// </value>
        IGrid<ICell> CurrentState { get; }
        /// <summary>
        /// Goes the state of to next.
        /// </summary>
        void GoToNextState();
    }
}
