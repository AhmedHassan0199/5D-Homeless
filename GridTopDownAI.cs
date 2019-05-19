using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTopDownAI : MonoBehaviour {

    public Node[,] Grid;

    Pair<int, int> start;
    Pair<int, int> end;
    List<Node> openList;
    List<Node> closedList;

    int horizontalCost;
    int verticalCost;
    int diagonalCost;

    int gridWidth;
    int gridHeight;

    bool finished;

    public GridTopDownAI(int width, int height, int h_cost, int v_cost, int d_cost, int si, int sj, int ei, int ej, List<GameObject> obsticals)
    {
        finished = false;

        gridWidth = width;
        gridHeight = height;

        Grid = new Node[gridWidth, gridHeight];
        start = new Pair<int, int>();
        end = new Pair<int, int>();
        openList = new List<Node>();
        closedList = new List<Node>();

        for (int i = 0; i != gridWidth; i++)
            for (int j = 0; j != gridHeight; j++)
            {
                Grid[i, j] = new Node();
                Grid[i, j].current_Cell.First = i;
                Grid[i, j].current_Cell.Second = j;
                Grid[i, j].parent_Cell.First = -1;
                Grid[i, j].parent_Cell.Second = -1;
            }

        horizontalCost = h_cost;
        verticalCost = v_cost;
        diagonalCost = d_cost;

        start.First = si;
        start.Second = sj;
        end.First = ei;
        end.Second = ej;

        set_Obsticals(obsticals);
        calc_Heuristic_Grid();
    }

    void set_Obsticals(List<GameObject> obsticals)
    {
        for (int i = 0; i != obsticals.Count; i++)
            Grid[(int)obsticals[i].transform.position.x, (int)obsticals[i].transform.position.z].obstical = true;
    }

    void calc_Heuristic_Grid()
    {
        for (int i = 0; i != gridWidth; i++)
            for (int j = 0; j != gridHeight; j++)
                calc_H_Node(i, j);
    }

    void calc_H_Node(int i, int j)
    {
        if (!Grid[i, j].obstical) Grid[i, j].H = (Mathf.Abs(i - end.First)) + (Mathf.Abs(j - end.Second));
        else Grid[i, j].H = -1;
    }

    bool check(int i, int j)
    {
        return (i >= 0 && i < gridWidth) && (j >= 0 && j < gridHeight) && !Grid[i, j].obstical;
    }

    void calc_G_F(int i, int j, int cost, Node node)
    {
        if (i == end.First && j == end.Second)
        {
            finished = true;
            Grid[i, j].parent_Cell.First = node.current_Cell.First;
            Grid[i, j].parent_Cell.Second = node.current_Cell.Second;
        }

        if (Grid[i, j].F != -1)
        {
            if (Grid[i, j].H + Grid[i, j].G < Grid[i, j].F)
            {
                Grid[i, j].G += cost;
                Grid[i, j].F = Grid[i, j].H + Grid[i, j].G;
                Grid[i, j].parent_Cell.First = node.current_Cell.First;
                Grid[i, j].parent_Cell.Second = node.current_Cell.Second;
            }
        }
        else
        {
            Grid[i, j].G += cost;
            Grid[i, j].F = Grid[i, j].H + Grid[i, j].G;
            Grid[i, j].parent_Cell.First = node.current_Cell.First;
            Grid[i, j].parent_Cell.Second = node.current_Cell.Second;
        }
    }

    Node node(int i, int j)
    {
        return Grid[i, j];
    }

    bool check_In_Closed_List(int i, int j)
    {
        if (check(i, j))
            return !closedList.Contains(Grid[i, j]);

        return false;
    }

    bool check_In_Open_List(int i, int j)
    {
        if (check(i, j))
            return !openList.Contains(Grid[i, j]);

        return false;
    }

    void set_Open_List(int i, int j, Node _node)
    {
        //if (check_In_Closed_List(i + 1, j + 1) && check_In_Open_List(i + 1, j + 1))
        //{
        //    calc_G_F(i + 1, j + 1, diagonalCost, _node);
        //    openList.Add(node(i + 1, j + 1));
        //}

        //if (check_In_Closed_List(i - 1, j - 1) && check_In_Open_List(i - 1, j - 1))
        //{
        //    calc_G_F(i - 1, j - 1, diagonalCost, _node);
        //    openList.Add(node(i - 1, j - 1));
        //}

        //if (check_In_Closed_List(i + 1, j - 1) && check_In_Open_List(i + 1, j - 1))
        //{
        //    calc_G_F(i + 1, j - 1, diagonalCost, _node);
        //    openList.Add(node(i + 1, j - 1));
        //}

        //if (check_In_Closed_List(i - 1, j + 1) && check_In_Open_List(i - 1, j + 1))
        //{
        //    calc_G_F(i - 1, j + 1, diagonalCost, _node);
        //    openList.Add(node(i - 1, j + 1));
        //}

        if (check_In_Closed_List(i, j + 1) && check_In_Open_List(i, j + 1))
        {
            calc_G_F(i, j + 1, verticalCost, _node);
            openList.Add(node(i, j + 1));
        }

        if (check_In_Closed_List(i, j - 1) && check_In_Open_List(i, j - 1))
        {
            calc_G_F(i, j - 1, verticalCost, _node);
            openList.Add(node(i, j - 1));
        }

        if (check_In_Closed_List(i + 1, j) && check_In_Open_List(i + 1, j))
        {
            calc_G_F(i + 1, j, horizontalCost, _node);
            openList.Add(node(i + 1, j));
        }

        if (check_In_Closed_List(i - 1, j) && check_In_Open_List(i - 1, j))
        {
            calc_G_F(i - 1, j, horizontalCost, _node);
            openList.Add(node(i - 1, j));
        }
    }

    public List<Pair<int, int>> run_Algorithm()
    {
        Grid[start.First, start.Second].F = 0 + Grid[start.First, start.Second].H;
        openList.Add(node(start.First, start.Second));
        Node least_Node = new Node();
        Node current_Node = node(start.First, start.Second);

        while (!finished)
        {
            set_Open_List(current_Node.current_Cell.First, current_Node.current_Cell.Second, current_Node);
            openList.Remove(current_Node);
            closedList.Add(current_Node);

            least_Node.F = 1000;
            for (int i = 0; i != openList.Count; i++)
                if (openList[i].F < least_Node.F)
                    least_Node = openList[i];

            openList.Remove(least_Node);
            current_Node = least_Node;
        }

        List<Pair<int, int>> path;
        path = new List<Pair<int, int>>();

        Node traverse = node(end.First, end.Second);

        //while (traverse.current_Cell.First != start.First && traverse.current_Cell.Second != start.Second)
        //{
        //    print(traverse.current_Cell.First + " " + traverse.current_Cell.Second);
        //    path.Add(traverse.current_Cell);
        //    traverse = node(traverse.parent_Cell.First, traverse.parent_Cell.Second);
        //}


        while (traverse.parent_Cell.First != -1 && traverse.parent_Cell.Second != -1)
        {
            print(traverse.current_Cell.First + " " + traverse.current_Cell.Second);
            path.Add(traverse.current_Cell);
            traverse = node(traverse.parent_Cell.First, traverse.parent_Cell.Second);
        }
      
        return path;
    }
}

public class Pair<T, U>
{
    public Pair()
    {
    }

    public Pair(T first, U second)
    {
        this.First = first;
        this.Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }
};

public class Node
{
    public int G;
    public int H;
    public int F;
    public Pair<int, int> current_Cell;
    public Pair<int, int> parent_Cell;
    public bool obstical;

    public Node()
    {
        current_Cell = new Pair<int, int>();
        parent_Cell = new Pair<int, int>();
        obstical = false;
        G = 0;
        F = -1;
    }
}