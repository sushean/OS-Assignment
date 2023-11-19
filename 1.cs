using System;
using System.Collections.Generic;
using System.Linq;

public class Process
{
    public int Pid { get; set; }
    public int ArrivalTime { get; set; }
    public int BurstTime { get; set; }
    public int RemainingTime { get; set; }
    public int Priority { get; set; }
    public int WaitingTime { get; set; }
    public int TurnaroundTime { get; set; }
    public int ResponseTime { get; set; }
}

class Program
{
    static void Main()
    {
        List<Process> processes = new List<Process>
        {
            new Process { Pid = 1, ArrivalTime = 0, BurstTime = 8, Priority = 3 },
            new Process { Pid = 2, ArrivalTime = 1, BurstTime = 4, Priority = 1 },
            new Process { Pid = 3, ArrivalTime = 2, BurstTime = 9, Priority = 4 },
            new Process { Pid = 4, ArrivalTime = 3, BurstTime = 5, Priority = 2 },
        };

        FCFS(processes.ToList());
        SJF(processes.ToList());
        RoundRobin(processes.ToList(), quantum: 2);
        PriorityScheduling(processes.ToList());
        Console.Read();
    }

    static void FCFS(List<Process> processes)
    {
        processes = processes.OrderBy(p => p.ArrivalTime).ToList();
        int currentTime = 0;

        foreach (var process in processes)
        {
            process.WaitingTime = currentTime - process.ArrivalTime;
            process.TurnaroundTime = process.WaitingTime + process.BurstTime;
            currentTime += process.BurstTime;
        }

        DisplayResults("First-Come-First-Served (FCFS)", processes);
    }

    static void SJF(List<Process> processes)
    {
        processes = processes.OrderBy(p => p.ArrivalTime).ThenBy(p => p.BurstTime).ToList();
        int currentTime = 0;

        foreach (var process in processes)
        {
            process.WaitingTime = currentTime - process.ArrivalTime;
            process.TurnaroundTime = process.WaitingTime + process.BurstTime;
            currentTime += process.BurstTime;
        }

        DisplayResults("Shortest Job First (SJF)", processes);
    }

    static void RoundRobin(List<Process> processes, int quantum)
    {
        Queue<Process> queue = new Queue<Process>(processes);
        int currentTime = 0;

        while (queue.Count > 0)
        {
            var process = queue.Dequeue();
            if (process.ResponseTime == 0)
            {
                process.ResponseTime = currentTime;
            }

            if (process.RemainingTime <= quantum)
            {
                currentTime += process.RemainingTime;
                process.TurnaroundTime = currentTime - process.ArrivalTime;
                process.WaitingTime = process.TurnaroundTime - process.BurstTime;
            }
            else
            {
                currentTime += quantum;
                process.RemainingTime -= quantum;
                queue.Enqueue(process);
            }
        }

        DisplayResults($"Round Robin (Quantum={quantum})", processes);
    }

    static void PriorityScheduling(List<Process> processes)
    {
        processes = processes.OrderBy(p => p.ArrivalTime).ThenBy(p => p.Priority).ToList();
        int currentTime = 0;

        foreach (var process in processes)
        {
            process.WaitingTime = currentTime - process.ArrivalTime;
            process.TurnaroundTime = process.WaitingTime + process.BurstTime;
            currentTime += process.BurstTime;
        }

        DisplayResults("Priority Scheduling", processes);
    }

    static void DisplayResults(string algorithm, List<Process> processes)
    {
        double avgTurnaroundTime = processes.Average(p => p.TurnaroundTime);
        double avgWaitingTime = processes.Average(p => p.WaitingTime);
        double avgResponseTime = processes.Average(p => p.ResponseTime);

        Console.WriteLine($"\n{algorithm} Results:");
        Console.WriteLine($"Average Turnaround Time: {avgTurnaroundTime:F2}");
        Console.WriteLine($"Average Waiting Time: {avgWaitingTime:F2}");
        Console.WriteLine($"Average Response Time: {avgResponseTime:F2}");
    }
}
