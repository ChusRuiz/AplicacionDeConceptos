using System;
using System.Collections.Generic;
using System.Threading;

public class TaskManager : ITaskManager
{
    private List<Task> tasks = new List<Task>();
    private ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

    public void AddTask(string taskName)
    {
        rwLock.EnterWriteLock();
        try
        {
            tasks.Add(new Task { Name = taskName, Status = TaskStatus.Pending });
        }
        finally
        {
            rwLock.ExitWriteLock();
        }
    }

    public void CompleteRandomTask()
    {
        rwLock.EnterUpgradeableReadLock();
        try
        {
            var pendingTasks = tasks.FindAll(t => t.Status == TaskStatus.Pending);
            if (pendingTasks.Count > 0)
            {
                int index = new Random().Next(pendingTasks.Count);
                rwLock.EnterWriteLock();
                try
                {
                    pendingTasks[index].Status = TaskStatus.Completed;
                }
                finally
                {
                    rwLock.ExitWriteLock();
                }
            }
        }
        finally
        {
            rwLock.ExitUpgradeableReadLock();
        }
    }

    public void DisplayTasks()
    {
        rwLock.EnterReadLock();
        try
        {
            Console.WriteLine("Lista de Tareas:");
            foreach (var task in tasks)
            {
                Console.WriteLine($"- {task.Name}: {task.Status}");
            }
            Console.WriteLine();
        }
        finally
        {
            rwLock.ExitReadLock();
        }
    }
}