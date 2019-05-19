using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate3DMap : MonoBehaviour {

    public GameObject []blockPrefab;
    GameObject newBlock;
    MeshFilter blockMesh;
    int width;
    int height;
    float frequency;
    float amplitude;
    float yCordinate;

    private void Start()
    {
         width = 100;
         height = 100;
         frequency = 10;
         amplitude = 10;
         GenerateChunk();
    }

    private void Update()
    {

    }

    private void GenerateChunk()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                yCordinate = Mathf.PerlinNoise((this.transform.position.x + x) / frequency, (this.transform.position.z + z) / frequency) * amplitude;

                yCordinate = Mathf.Floor(yCordinate);

                if (yCordinate > amplitude/2)
                    newBlock =  Instantiate(blockPrefab[0]);
                else
                    newBlock = Instantiate(blockPrefab[1]);

                newBlock.name = "Cube:" + x + ", " + z;
                newBlock.transform.position = new Vector3((this.transform.position.x + x), yCordinate, (this.transform.position.z + z));
                newBlock.transform.parent = gameObject.transform;
            }
        }
    }
}