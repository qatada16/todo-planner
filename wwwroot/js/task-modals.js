function openEditPopup(taskId, title, description, dueDate, priority, status) {
    document.getElementById('editTaskId').value = taskId;
    document.getElementById('editTaskTitle').value = title;
    document.getElementById('editTaskDescription').value = description || "";
    document.getElementById('editTaskDueDate').value = dueDate;
    document.getElementById('editTaskPriority').value = priority;
    document.getElementById('editTaskStatus').value = status;

    document.getElementById('editTaskModal').classList.remove('hidden');
}

function closeEditPopup() {
    document.getElementById('editTaskModal').classList.add('hidden');
}

function openDeletePopup(taskId) {
    document.getElementById('deleteTaskId').value = taskId;
    document.getElementById('deleteTaskModal').classList.remove('hidden');
}

function closeDeletePopup() {
    document.getElementById('deleteTaskModal').classList.add('hidden');
}