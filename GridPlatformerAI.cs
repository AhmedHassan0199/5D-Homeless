using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlatformerAI : MonoBehaviour {

    public Cell[,] Cells;

    Pair<int, int> Start;
    Pair<int, int> End;
    List<Cell> OpenList;
    List<Cell> ClosedList;

    int HorizontalCost;
    int JumpCost;

    int GridWidth;
    int GridHeight;

    bool finished;

    public GridPlatformerAI(int width, int height, int h_cost, int j_cost, int si, int sj, int ei, int ej, List<GameObject> tiles)
    {
        finished = false;

        GridWidth = width;
        GridHeight = height;

        Cells = new Cell[GridWidth, GridHeight];
        Start = new Pair<int, int>();
        End = new Pair<int, int>();
        OpenList = new List<Cell>();
        ClosedList = new List<Cell>();

        for (int i = 0; i != GridWidth; i++)
            for (int j = 0; j != GridHeight; j++)
            {
                Cells[i, j] = new Cell();
                Cells[i, j].CurrentCell.First = i;
                Cells[i, j].CurrentCell.Second = j;
                Cells[i, j].ParentCell.First = -1;
                Cells[i, j].ParentCell.Second = -1;
            }

        HorizontalCost = h_cost;
        JumpCost = j_cost;

        Start.First = si;
        Start.Second = sj;
        End.First = ei;
        End.Second = ej;

        SetTiles(tiles);
        CalcHeuristicCells();
    }

    void SetTiles(List<GameObject> Tiles)
    {
        for (int i = 0; i != Tiles.Count; i++)
            Cells[(int)Tiles[i].transform.position.x, (int)Tiles[i].transform.position.y].Ground = true;
    }

    void CalcHeuristicCells()
    {
        for (int i = 0; i != GridWidth; i++)
            for (int j = 0; j != GridHeight; j++)
                CalcHeuristicCell(i, j);
    }

    void CalcHeuristicCell(int i, int j)
    {
        if (!Cells[i, j].Ground && i + 1 < GridHeight && Cells[i + 1, j].Ground) Cells[i, j].H = (Mathf.Abs(i - End.First)) + (Mathf.Abs(j - End.Second));
        else Cells[i, j].H = -1;
    }

    bool check(int i, int j)
    {
        return (i >= 0 && i < GridWidth) && (j >= 0 && j < GridHeight) && !Cells[i, j].Ground;
    }

    int CheckGround(int x, int y, string direction)
    {
        if (direction == "U")
        {
            if (x - JumpCost >= 0 && Cells[x - JumpCost + 1, y].Ground && !Cells[x - JumpCost, y].Ground)
                return x - JumpCost;
        }
        else
        if (direction == "D")
        {
            for (int i = x; i < GridHeight; i++)
                if (i - 1 != x && Cells[i, y].Ground && !Cells[i - 1, y].Ground)
                    return i - 1;
        }
        else
        if (direction == "H")
        {
            for (int i = x; i < GridHeight; i++)
                if (Cells[i, y].Ground)
                    return i - 1;
        }
        else
        if (direction == "UH")
        {
            if (x - JumpCost >= 0)
                for (int i = x - JumpCost; i < GridHeight; i++)
                    if (Cells[i, y].Ground)
                        return i - 1;
        }
        else
        if (direction == "DH")
        {
            for (int i = x; i < GridHeight; i++)
                if (Cells[i, y].Ground)
                    return i - 1;
        }

        return -1;
    }

    void calc_G_F(int i, int j, int cost, Cell cell)
    {
        if (i == End.First && j == End.Second)
        {
            finished = true;
            Cells[i, j].ParentCell.First = cell.CurrentCell.First;
            Cells[i, j].ParentCell.Second = cell.CurrentCell.Second;
        }

        if (Cells[i, j].F != -1)
        {
            if (Cells[i, j].H + Cells[i, j].G < Cells[i, j].F)
            {
                Cells[i, j].G += cost;
                Cells[i, j].F = Cells[i, j].H + Cells[i, j].G;
                Cells[i, j].ParentCell.First = cell.CurrentCell.First;
                Cells[i, j].ParentCell.Second = cell.CurrentCell.Second;
            }
        }
        else
        {
            Cells[i, j].G += cost;
            Cells[i, j].F = Cells[i, j].H + Cells[i, j].G;
            Cells[i, j].ParentCell.First = cell.CurrentCell.First;
            Cells[i, j].ParentCell.Second = cell.CurrentCell.Second;
        }
    }

    Cell Cell(int i, int j)
    {
        return Cells[i, j];
    }

    bool check_In_Closed_List(int i, int j)
    {
        if (check(i, j))
            return !ClosedList.Contains(Cells[i, j]);

        return false;
    }

    bool check_In_Open_List(int i, int j)
    {
        if (check(i, j))
            return !OpenList.Contains(Cells[i, j]);

        return false;
    }

    void set_Open_List(int i, int j, Cell cell)
    {
        int index;

        index = CheckGround(i, j + 1, "H");
        if (index != -1 && check_In_Closed_List(index, j + 1) && check_In_Open_List(index, j + 1))
        {
            calc_G_F(index, j + 1, HorizontalCost, cell);
            OpenList.Add(Cell(index, j + 1));
        }

        index = CheckGround(i, j - 1, "H");
        if (index != -1 && check_In_Closed_List(index, j - 1) && check_In_Open_List(index, j - 1))
        {
            calc_G_F(index, j - 1, HorizontalCost, cell);
            OpenList.Add(Cell(index, j - 1));
        }

        index = CheckGround(i, j, "U");
        if (index != -1 && check_In_Closed_List(index, j) && check_In_Open_List(index, j))
        {
            calc_G_F(index, j, HorizontalCost, cell);
            OpenList.Add(Cell(index, j));
        }

        index = CheckGround(i, j, "D");
        if (index != -1 && check_In_Closed_List(index, j) && check_In_Open_List(index, j))
        {
            calc_G_F(index, j, HorizontalCost, cell);
            OpenList.Add(Cell(index, j));
        }

        index = CheckGround(i, j + 1, "UH");
        if (index != -1 && check_In_Closed_List(index, j + 1) && check_In_Open_List(index, j + 1))
        {
            calc_G_F(index, j + 1, HorizontalCost, cell);
            OpenList.Add(Cell(index, j + 1));
        }

        index = CheckGround(i, j - 1, "UH");
        if (index != -1 && check_In_Closed_List(index, j - 1) && check_In_Open_List(index, j - 1))
        {
            calc_G_F(index, j - 1, HorizontalCost, cell);
            OpenList.Add(Cell(index, j - 1));
        }

        index = CheckGround(i, j + 1, "DH");
        if (index != -1 && check_In_Closed_List(index, j + 1) && check_In_Open_List(index, j + 1))
        {
            calc_G_F(index, j + 1, HorizontalCost, cell);
            OpenList.Add(Cell(index, j + 1));
        }

        index = CheckGround(i, j - 1, "DH");
        if (index != -1 && check_In_Closed_List(index, j - 1) && check_In_Open_List(index, j - 1))
        {
            calc_G_F(index, j - 1, HorizontalCost, cell);
            OpenList.Add(Cell(index, j - 1));
        }
    }

    public List<Pair<int, int>> run_Algorithm()
    {
        Cells[Start.First, Start.Second].F = 0 + Cells[Start.First, Start.Second].H;
        OpenList.Add(Cell(Start.First, Start.Second));
        Cell LeastCell = new Cell();
        Cell CurrentCell = Cell(Start.First, Start.Second);

        while (!finished && OpenList.Count != 0)
        {
            set_Open_List(CurrentCell.CurrentCell.First, CurrentCell.CurrentCell.Second, CurrentCell);
            OpenList.Remove(CurrentCell);
            ClosedList.Add(CurrentCell);

            LeastCell.F = 1000;
            for (int i = 0; i != OpenList.Count; i++)
                if (OpenList[i].F < LeastCell.F)
                    LeastCell = OpenList[i];

            OpenList.Remove(LeastCell);
            CurrentCell = LeastCell;
        }

        List<Pair<int, int>> path;
        path = new List<Pair<int, int>>();

        Cell traverse = Cell(End.First, End.Second);
        do
        {
            print(traverse.CurrentCell.First + " " + traverse.CurrentCell.Second);
            path.Add(traverse.CurrentCell);
            traverse = Cell(traverse.ParentCell.First, traverse.ParentCell.Second);
        }
        while (traverse.CurrentCell.First != Start.First && traverse.CurrentCell.Second != Start.Second);
        print(traverse.CurrentCell.First + " " + traverse.CurrentCell.Second);
        path.Add(traverse.CurrentCell);

        return path;
    }
}

public class Cell
{
    public int G;
    public int H;
    public int F;
    public Pair<int, int> CurrentCell;
    public Pair<int, int> ParentCell;
    public bool Ground;
    //public string Direction;

    public Cell()
    {
        CurrentCell = new Pair<int, int>();
        ParentCell = new Pair<int, int>();
        Ground = false;
        G = 0;
        H = 0;
        F = -1;
    }
}