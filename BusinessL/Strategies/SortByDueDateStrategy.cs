using todo_planner.Models;
using Task = todo_planner.Models.Task;

namespace todo_planner.BusinessL.Strategies
{
    /// <summary>
    /// Concrete Strategy - Sorts tasks by due date (earliest first)
    /// </summary>
    public class SortByDueDateStrategy : ITaskSortStrategy
    {
        public string StrategyName => "Due Date";

        public IEnumerable<Task> Sort(IEnumerable<Task> tasks)
        {
            return tasks.OrderBy(t => t.DueDate);
        }
    }
}