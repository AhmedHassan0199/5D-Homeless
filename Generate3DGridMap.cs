using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate3DGridMap : MonoBehaviour {

    public Vector3 StartPosition;
    public int size = 256;
    byte[,] TilesGrid;

    public int WallsCount;
    public Material WallMaterial;
    public GameObject Wall;
    public Material TileMaterial;
    public GameObject Tile;
    [HideInInspector]
    public List<GameObject> Tiles;
    GameObject TilesParent;

    public GameObject player;
    public GameObject enemy;
    //public Vector3 target;
    int setTarget;
    int resetTarget;

    Pair<int, int> start, end, oldstart, oldend;

    void Awake()
    {
        TilesGrid = new byte[size, size];
        Tiles = new List<GameObject>();
        TilesParent = new GameObject("TilesParent");

        HashSet<Pair<int, int>> pairs = new HashSet<Pair<int, int>>();
        Pair<int, int> Tile = new Pair<int, int>();
        int cb, ca;

        for (int i = 0; i != WallsCount; i++)
        {
            Tile.First = Random.Range(0, size);
            Tile.Second = Random.Range(0, size);

            if ((Tile.First == 0 && Tile.Second == 0) || (Tile.First == size/2 && Tile.Second == size/2))
                continue;

            cb = pairs.Count;
            pairs.Add(Tile);
            ca = pairs.Count;

            if (cb == ca)
                TilesGrid[Tile.First, Tile.Second] = 1;
        }

        for (int j = 0; j != size; j++)
            for (int i = 0; i != size; i++)
            {
                if (TilesGrid[i, j] == 1)
                {
                    GameObject NewTile = Instantiate(Wall, StartPosition + new Vector3(i, 1, j), Quaternion.identity);
                    NewTile.transform.parent = TilesParent.transform;
                    Tiles.Add(NewTile);
                }

                GameObject New = Instantiate(this.Tile, StartPosition + new Vector3(i, -.75f, j), Quaternion.identity);
                New.transform.parent = TilesParent.transform;
            }

        player = Instantiate(player, new Vector3(size / 2, 1, size / 2), Quaternion.identity);
        enemy = Instantiate(enemy, new Vector3(0, 1, 0), Quaternion.identity);

        setTarget = 0;
        resetTarget = 25;

        oldend = new Pair<int, int>();
        end = new Pair<int, int>();
        oldend.First = 1;
        oldend.Second = 2;
        end.First = 0;
        end.Second = 0;
    }

    void Update()
    {
        Vector3 pointer = new Vector3();
        //pointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //pointer = target;
        pointer = player.transform.position;

        setTarget++;
        if (setTarget > resetTarget)
            setTarget = 0;

        print((int)pointer.x + " " + (int)pointer.y);

        if (end != oldend && setTarget == 0)
        {
            if ((int)pointer.x >= 0 && (int)pointer.x < size && (int)pointer.z >= 0 && (int)pointer.z < size && TilesGrid[(int)pointer.x, (int)pointer.z] != 1)
            {
                StopAllCoroutines();
                print("oldend " + oldend.First + " " + oldend.Second);
                print("end " + end.First + " " + end.Second);
                oldend.First = end.First;
                oldend.Second = end.Second;
                end.First = (int)pointer.x;
                end.Second = (int)pointer.z;
                print("oldend " + oldend.First + " " + oldend.Second);
                print("end " + end.First + " " + end.Second);
                StartCoroutine(Fallow());
            }
        }
    }

    IEnumerator Fallow()
    {
        //GridTopDownAI algorithm = new GridTopDownAI(size, size, 1, 1, 2, oldend.First, oldend.Second, end.First, end.Second, Tiles);    
        GridTopDownAI algorithm = new GridTopDownAI(size, size, 1, 1, 2,
            (int)enemy.transform.position.x, (int)enemy.transform.position.z, end.First, end.Second, Tiles);
        List<Pair<int, int>> path = algorithm.run_Algorithm();
        yield return StartCoroutine(Follow_Path(enemy, path));
    }

    IEnumerator Follow_Path(GameObject obj, List<Pair<int, int>> path)
    {
        while (path.Count > 0)
        {
            obj.transform.position = new Vector3(path[path.Count - 1].First, obj.transform.position.y, path[path.Count - 1].Second);

            path.RemoveAt(path.Count - 1);
            yield return new WaitForSeconds(.1f);

            if (end != oldend && Input.GetKeyUp(KeyCode.S))
            {
                print("yes");
                yield break;
            }
        }
    }
}