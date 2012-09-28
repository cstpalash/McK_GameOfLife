using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCK.GameOfLife.Infrastructure.View;
using MCK.GameOfLife.Infrastructure;

namespace MCK.GameOfLife.View
{
    /// <summary>
    /// GridView
    /// </summary>
    public class GridView : IGridView
    {
        #region Private members
        private StringBuilder builder;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="GridView" /> class.
        /// </summary>
        /// <param name="stateGenerator">The state generator.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public GridView(IStateGenerator stateGenerator)
        {
            if (stateGenerator == null)
                throw new ArgumentNullException("stateGenerator");
            StateGenerator = stateGenerator;
            builder = new StringBuilder();
        }
        #endregion

        #region IGridView members
        /// <summary>
        /// Gets the state generator.
        /// </summary>
        /// <value>
        /// The state generator.
        /// </value>
        public IStateGenerator StateGenerator
        {
            get;
            private set;
        }

        /// <summary>
        /// Renders the state of the next.
        /// </summary>
        /// <exception cref="System.InvalidOperationException"></exception>
        public void RenderNextState()
        {
            if (StateGenerator == null)
                throw new InvalidOperationException("StateGenerator should be initialized first");
            StateGenerator.GoToNextState();
            Render();
        }

        /// <summary>
        /// Renders this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException"></exception>
        public void Render()
        {
            if (StateGenerator == null)
                throw new InvalidOperationException("StateGenerator should be initialized first");

            Console.WriteLine();
            builder.Clear();
            for (var rowIndex = 0; rowIndex < StateGenerator.CurrentState.TotalRows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < StateGenerator.CurrentState.TotalColumns; columnIndex++)
                {
                    builder.Append(StateGenerator.CurrentState[rowIndex, columnIndex].IsAlive ? "X" : "-");
                    builder.Append(" ");
                }
                builder.Append(Environment.NewLine);
            }

            Console.WriteLine(builder.ToString());
        }
        #endregion
    }
}
