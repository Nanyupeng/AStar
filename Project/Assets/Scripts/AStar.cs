using System;
using System.Collections.Generic;

public class Node
{
    public int X, Y;         // 坐标
    public float G, H;       // G: 起点到当前节点的代价, H: 启发代价
    public float F => G + H; // F: 总代价 F = G + H
    public bool IsWalkable;  // 是否可通行
    public Node Parent;      // 父节点，用于路径回溯

    public Node(int x, int y, bool isWalkable = true)
    {
        X = x;
        Y = y;
        IsWalkable = isWalkable;
    }

    public override string ToString()
    {
        return $"({X},{Y})";
    }
}

public class AStarUtil
{
  public static List<Node> FindPath(Node start, Node goal, Node[,] grid)
    {
        var openList = new List<Node>();      // 开放列表
        var closedList = new HashSet<Node>(); // 封闭列表

        openList.Add(start);

        while (openList.Count > 0)
        {
            // 从开放列表中取出 F 值最小的节点
            openList.Sort((a, b) => a.F.CompareTo(b.F));
            var current = openList[0];

            // 如果找到目标节点，返回路径
            if (current == goal)
            {
                return RetracePath(start, goal);
            }

            openList.Remove(current);
            closedList.Add(current);

            // 遍历邻居节点
            foreach (var neighbor in GetNeighbors(current, grid))
            {
                if (!neighbor.IsWalkable || closedList.Contains(neighbor)) continue;

                float newG = current.G + GetDistance(current, neighbor);
                if (newG < neighbor.G || !openList.Contains(neighbor))
                {
                    neighbor.G = newG;
                    neighbor.H = GetDistance(neighbor, goal);
                    neighbor.Parent = current;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null; // 无路径可达
    }

    // 获取邻居节点
    private static List<Node> GetNeighbors(Node node, Node[,] grid)
    {
        var neighbors = new List<Node>();
        int rows = grid.GetLength(0), cols = grid.GetLength(1);
        int[][] directions =
        {
            new[]
            {
                0, 1
            },
            new[]
            {
                1, 0
            },
            new[]
            {
                0, -1
            },
            new[]
            {
                -1, 0
            }
        };
        foreach (var dir in directions)
        {
            int nx = node.X + dir[0], ny = node.Y + dir[1];
            if (nx >= 0 && ny >= 0 && nx < rows && ny < cols)
            {
                neighbors.Add(grid[nx, ny]);
            }
        }

        return neighbors;
    }

    // 计算两个节点之间的距离
    private static float GetDistance(Node a, Node b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y); // 曼哈顿距离
    }

    // 回溯路径
    private static List<Node> RetracePath(Node start, Node goal)
    {
        var path = new List<Node>();
        Node current = goal;
        while (current != start)
        {
            path.Add(current);
            current = current.Parent;
        }
        path.Reverse();
        return path;
    }
}
