using todo_planner.Models;
using Task = todo_planner.Models.Task;

namespace todo_planner.BusinessL.Strategies
{
    /// <summary>
    /// Concrete Strategy - Sorts tasks by priority (High → Medium → Low)
    /// </summary>
    public class SortByPriorityStrategy : ITaskSortStrategy
    {
        public string StrategyName => "Priority";

        public IEnumerable<Task> Sort(IEnumerable<Task> tasks)
        {
            var priorityOrder = new Dictionary<TaskPriority, int>
            {
                { TaskPriority.High, 1 },
                { TaskPriority.Medium, 2 },
                { TaskPriority.Low, 3 }
            };

            return tasks.OrderBy(t => priorityOrder[t.Priority])
                       .ThenBy(t => t.DueDate);
        }
    }
}