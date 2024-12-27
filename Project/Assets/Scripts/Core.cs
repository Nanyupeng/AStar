using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Core : MonoBehaviour
{
    public RectTransform root;
    public GameObject gridObj;

    public int rows = 5;
    public int cols = 5;

    Grid startGrid;
    Grid goalGrid;

    Node[,] maps;
    List<Grid> _grids;

    void Awake()
    {
        BuildMap();
        GenerateMap();
    }

    void StartMove(Node start, Node goal)
    {
        // 执行 A* 寻路
        var path = AStarUtil.FindPath(start, goal, maps);

        // 打印结果
        if (path != null)
        {
            Debug.Log("路径找到：");

            int index = 0;
            foreach (var node in path)
            {
                index++;
                StartCoroutine(Move(node, index));

                Debug.Log(node);
            }
        }
        else
        {
            MoveEnd();
            Debug.Log("无可行路径！");
        }
    }

    IEnumerator Move(Node node, int index)
    {
        for (int i = 0; i < _grids.Count; i++)
        {
            if (_grids[i].pathPNode == node)
            {
                yield return new WaitForSeconds(index * 0.1f);
                _grids[i].Move();

                if (_grids[i] == goalGrid )
                {
                    MoveEnd();
                    Debug.Log("到达目的地！");
                }
            }
        }
    }

    void MoveEnd()
    {
        startGrid = null;
        goalGrid = null;
    }

    void BuildMap()
    {
        maps = new Node[rows, cols];

        // 构建网格地图
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                maps[x, y] = new Node(x, y, Random.Range(0, 100) > 20); // 随机设置障碍物
            }
        }
    }

    void GenerateMap()
    {
        _grids = new List<Grid>();

        for (int i = 0; i < maps.GetLength(0); i++)
        {
            for (int j = 0; j < maps.GetLength(1); j++)
            {
                GameObject go = Instantiate(gridObj, root);
                _grids.Add(go.GetComponent<Grid>().Init(maps[i, j], SelectNode));
            }
        }
    }

    void SelectNode(Grid selectNode)
    {
        // 设置起点和目标点
        if (goalGrid)
            return;
        
        if (startGrid == null)
        {
            startGrid = selectNode;
            _grids.ForEach((grid) => grid.Rest());
            return;
        }

        goalGrid = selectNode;
        StartMove(startGrid.pathPNode, goalGrid.pathPNode);
    }

}