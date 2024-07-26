using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        ITaskManager taskManager = new TaskManager();

        // Crear hilos para simular múltiples usuarios
        Thread user1 = new Thread(() => UserSimulation(taskManager, "Usuario 1"));
        Thread user2 = new Thread(() => UserSimulation(taskManager, "Usuario 2"));

        user1.Start();
        user2.Start();

        user1.Join();
        user2.Join();

        Console.WriteLine("Presione cualquier tecla para salir...");
        Console.ReadKey();
    }

    static void UserSimulation(ITaskManager taskManager, string userName)
    {
        Random random = new Random();

        for (int i = 0; i < 5; i++)
        {
            int action = random.Next(3);
            switch (action)
            {
                case 0:
                    string taskName = $"Tarea {random.Next(100)} de {userName}";
                    taskManager.AddTask(taskName);
                    Console.WriteLine($"{userName} agregó: {taskName}");
                    break;
                case 1:
                    taskManager.CompleteRandomTask();
                    Console.WriteLine($"{userName} completó una tarea aleatoria");
                    break;
                case 2:
                    taskManager.DisplayTasks();
                    break;
            }
            Thread.Sleep(1000); // Simular tiempo entre acciones
        }
    }
}