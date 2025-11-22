using todo_planner.Models;
using Task = todo_planner.Models.Task;

namespace todo_planner.BusinessL.Strategies
{
    /// <summary>
    /// Strategy Pattern Interface - Defines contract for task sorting algorithms
    /// Allows runtime selection of different sorting strategies
    /// </summary>
    public interface ITaskSortStrategy
    {
        /// <summary>
        /// Sorts a collection of tasks according to the strategy's algorithm
        /// </summary>
        IEnumerable<Task> Sort(IEnumerable<Task> tasks);
        
        /// <summary>
        /// Gets the display name of the sorting strategy
        /// </summary>
        string StrategyName { get; }
    }
}