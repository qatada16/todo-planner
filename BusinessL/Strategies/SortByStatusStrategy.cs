using todo_planner.Models;
using Task = todo_planner.Models.Task;
using TaskStatus = todo_planner.Models.TaskStatus;

namespace todo_planner.BusinessL.Strategies
{
    /// <summary>
    /// Concrete Strategy - Sorts tasks by status (Pending → InProgress → Completed)
    /// </summary>
    public class SortByStatusStrategy : ITaskSortStrategy
    {
        public string StrategyName => "Status";

        public IEnumerable<Task> Sort(IEnumerable<Task> tasks)
        {
            var statusOrder = new Dictionary<TaskStatus, int>
            {
                { TaskStatus.Pending, 1 },
                { TaskStatus.InProgress, 2 },
                { TaskStatus.Completed, 3 }
            };

            return tasks.OrderBy(t => statusOrder[t.Status])
                       .ThenBy(t => t.DueDate);
        }
    }
}