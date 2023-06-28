using System;
using System.Collections.Generic;

public class Graph
{
    private int V; // Количество вершин
    private List<Tuple<int, int>>[] adjList; // Список смежности

    public Graph(int vertices)
    {
        V = vertices;
        adjList = new List<Tuple<int, int>>[V];
        for (int i = 0; i < V; i++)
        {
            adjList[i] = new List<Tuple<int, int>>();
        }
    }

    public void AddEdge(int u, int v, int weight)
    {
        adjList[u].Add(new Tuple<int, int>(v, weight));
        adjList[v].Add(new Tuple<int, int>(u, weight));
    }

    public List<List<int>> FindShortestPaths(int source, int destination)
    {
        List<List<int>> shortestPaths = new List<List<int>>();
        List<int> currentPath = new List<int>();

        // Создаем массив для хранения кратчайших расстояний до каждой вершины
        int[] distances = new int[V];
        for (int i = 0; i < V; i++)
        {
            distances[i] = int.MaxValue;
        }

        // Создаем очередь для обхода графа в ширину
        Queue<List<int>> queue = new Queue<List<int>>();

        // Добавляем исходную вершину в очередь и устанавливаем ее расстояние равным 0
        currentPath.Add(source);
        queue.Enqueue(currentPath);
        distances[source] = 0;

        while (queue.Count > 0)
        {
            currentPath = queue.Dequeue();
            int lastVertex = currentPath[currentPath.Count - 1];

            // Если достигнута конечная вершина, добавляем текущий путь в список кратчайших путей
            if (lastVertex == destination)
            {
                shortestPaths.Add(new List<int>(currentPath));
            }

            // Просматриваем все смежные вершины
            foreach (var neighbor in adjList[lastVertex])
            {
                int nextVertex = neighbor.Item1;
                int weight = neighbor.Item2;

                // Если новое расстояние до смежной вершины меньше текущего расстояния,
                // обновляем расстояние и добавляем новый путь в очередь
                if (distances[lastVertex] + weight < distances[nextVertex])
                {
                    distances[nextVertex] = distances[lastVertex] + weight;
                    List<int> newPath = new List<int>(currentPath);
                    newPath.Add(nextVertex);
                    queue.Enqueue(newPath);
                }
            }
        }

        // Сортируем кратчайшие пути по возрастанию расстояния
        shortestPaths.Sort((x, y) => distances[x[x.Count - 1]] - distances[y[y.Count - 1]]);

        // Ограничиваем список кратчайших путей только первыми 2 путями
        if (shortestPaths.Count > 2)
        {
            shortestPaths = shortestPaths.GetRange(0, 5);
        }

        return shortestPaths;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Создаем граф с 6 вершинами
        Graph graph = new Graph(6);

        // Добавляем ребра между вершинами и их весами
        graph.AddEdge(0, 1, 2);
        graph.AddEdge(0, 2, 4);
        graph.AddEdge(1, 2, 1);
        graph.AddEdge(1, 3, 7);
        graph.AddEdge(2, 4, 3);
        graph.AddEdge(3, 4, 1);
        graph.AddEdge(3, 5, 5);
        graph.AddEdge(4, 5, 2);
        // Находим 5 кратчайших путей от вершины 0 до вершины 5
        List<List<int>> shortestPaths = graph.FindShortestPaths(0, 3);

        // Выводим найденные пути
        Console.WriteLine("5 кратчайших путей от вершины 0 до вершины 5:");
        foreach (var path in shortestPaths)
        {
            Console.WriteLine(string.Join(" -> ", path));
        }
    }
}