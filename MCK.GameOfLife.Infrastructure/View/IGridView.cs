using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCK.GameOfLife.Infrastructure.View
{
    /// <summary>
    /// IGridView
    /// </summary>
    public interface IGridView : IMainView
    {
        /// <summary>
        /// Gets the state generator.
        /// </summary>
        /// <value>
        /// The state generator.
        /// </value>
        IStateGenerator StateGenerator { get; }

        /// <summary>
        /// Renders the state of the next.
        /// </summary>
        void RenderNextState();
    }
}
