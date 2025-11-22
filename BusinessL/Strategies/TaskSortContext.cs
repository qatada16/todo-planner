using todo_planner.Models;
using Task = todo_planner.Models.Task;

namespace todo_planner.BusinessL.Strategies
{
    /// <summary>
    /// Context - Manages the current sorting strategy and applies it
    /// Follows Strategy Pattern - allows switching strategies at runtime
    /// </summary>
    public class TaskSortContext
    {
        private ITaskSortStrategy _strategy;

        public TaskSortContext(ITaskSortStrategy strategy)
        {
            _strategy = strategy;
        }

        /// <summary>
        /// Changes the sorting strategy at runtime
        /// </summary>
        public void SetStrategy(ITaskSortStrategy strategy)
        {
            _strategy = strategy;
        }

        /// <summary>
        /// Applies the current strategy to sort tasks
        /// </summary>
        public IEnumerable<Task> ExecuteStrategy(IEnumerable<Task> tasks)
        {
            return _strategy.Sort(tasks);
        }

        public string CurrentStrategyName => _strategy.StrategyName;
    }
}