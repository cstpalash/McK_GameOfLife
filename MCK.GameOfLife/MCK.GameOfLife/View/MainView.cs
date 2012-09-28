using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using MCK.GameOfLife.Infrastructure.View;
using MCK.GameOfLife.Infrastructure.Grid;
using MCK.GameOfLife.Infrastructure;
using System.Globalization;

namespace MCK.GameOfLife.View
{
    public class MainView : IMainView
    {
        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        private IUnityContainer Container { get; set; }

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MainView" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public MainView(IUnityContainer container)
        {
            Container = container;
        }
        #endregion

        #region IMainView members
        /// <summary>
        /// Renders this instance.
        /// </summary>
        public void Render()
        {
            try
            {
                RenderHeader();
                PromptAndTakeInputs();

                InitializeGrid();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "Error : {0}", ex.Message));
                Console.Write("Press <enter> to quit : ");
                Console.ReadLine();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "Error : {0}", ex.Message));
                Console.Write("Press <enter> to quit : ");
                Console.ReadLine();
            }
            catch (ResolutionFailedException)
            {
                Console.WriteLine("Can not create/initialize grid with provided inputs");
                Console.Write("Press <enter> to quit : ");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "Error : {0}", ex.Message));
                Console.Write("Press <enter> to quit : ");
                Console.ReadLine();
            }
        }

        #endregion

        #region Private members
        /// <summary>
        /// Renders the header.
        /// </summary>
        private void RenderHeader()
        {
            Console.WriteLine("*************************Game Of Life*************************");
            Console.WriteLine("Date : 28 Sept 2012");
            Console.WriteLine("Rules: 1. Live cell with less than 2 live neighbours dies => under-population.\n" +
                              "       2. Live cell with 2 or 3 live neighbours lives.\n" +
                              "       3. Live cell with more than 3 live neighbours dies => overcrowding.\n" +
                              "       4. Dead cell with 3 live neighbours becomes a live cell => reproduction.");
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Live cell = X");
            Console.WriteLine("Dead cell = -");

            Console.WriteLine("**************************************************************");
            Console.WriteLine(Environment.NewLine);
        }

        private int totalRows = 0;
        private int totalColumns = 0;
        private ICell[] initialActiveCells;

        /// <summary>
        /// Prompts the and take inputs.
        /// </summary>
        private void PromptAndTakeInputs()
        {
            while (!InputSize())
                ShowInvalidInput();

            while (!InputInitialLiveCells())
                ShowInvalidInput();

        }

        /// <summary>
        /// Initializes the grid.
        /// </summary>
        private void InitializeGrid()
        {
            IGrid<ICell> grid = Container.Resolve<IGrid<ICell>>(
                new ParameterOverride("totalRows", totalRows),
                new ParameterOverride("totalColumns", totalColumns),
                new ParameterOverride("initialActiveCells", initialActiveCells));


            IStateGenerator stateGenerator = Container.Resolve<IStateGenerator>(new ParameterOverride("currentState", grid));

            IGridView gridView = Container.Resolve<IGridView>(new ParameterOverride("stateGenerator", stateGenerator));
            gridView.Render();

            Console.Write("Press <enter> to see next state or 'exit' to quit : ");
            string result = Console.ReadLine();

            while (string.Compare(result, "exit", StringComparison.OrdinalIgnoreCase) != 0)
            {
                gridView.RenderNextState();

                Console.Write("Press <enter> to see next state or 'exit' to quit : ");
                result = Console.ReadLine();
            }
        }

        /// <summary>
        /// Inputs the size.
        /// </summary>
        /// <returns></returns>
        private bool InputSize()
        {
            Console.Write("Please enter number of rows and columns (e.g. 3,3) : ");
            string result = Console.ReadLine();
            string[] segments = result.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            if (segments.Count() == 2)
            {
                int.TryParse(segments[0], out totalRows);
                int.TryParse(segments[1], out totalColumns);

                if (totalRows > 0 && totalColumns > 0)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Inputs the initial live cells.
        /// </summary>
        /// <returns></returns>
        private bool InputInitialLiveCells()
        {
            Console.WriteLine("Please enter initial live cells [0 based index] (e.g. 0,1; 1,0; 1,2; 2,1) : ");
            string result = Console.ReadLine();
            string[] segments = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            if (segments.Count() == 0) return false;

            initialActiveCells = new ICell[segments.Count()];
            for (int i = 0; i < segments.Count(); i++)
            {
                string item = segments[i];
                string[] fragments = item.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                if (fragments.Count() != 2) return false;

                int r, c;
                bool isRowIndexValid = int.TryParse(fragments[0], out r);
                bool isColIndexValid = int.TryParse(fragments[1], out c);

                if (isRowIndexValid && isColIndexValid)
                {
                    ICell cell = Container.Resolve<ICell>(new ParameterOverride("rowIndex", r), new ParameterOverride("colIndex", c));
                    cell.IsAlive = true;
                    initialActiveCells[i] = cell;
                }
                else
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Shows the invalid input.
        /// </summary>
        private void ShowInvalidInput()
        {
            Console.WriteLine("Invalid input / wrong format.");
        }
        #endregion
    }
}
