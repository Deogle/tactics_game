using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour {

    private const int GRASS_TILE = 0;
    private const int WOOD_TILE = 1;

    private const float TILE_OFFSET = .63f;

    public TileType[] tileTypes;
    public GameObject unit;

    public GameObject selectedUnit;

    int[,] tiles;
    public Node[,] graph;

    public int sizeX;
    public int sizeY;

    void Start()
    {
        GenerateMapData();
        GeneratePathGraph();
        GenerateMapVisual();

        //Setup unit variables
        selectedUnit.GetComponent<Unit>().unitX = selectedUnit.transform.position.x;
        selectedUnit.GetComponent<Unit>().unitY = selectedUnit.transform.position.y;
        selectedUnit.GetComponent<Unit>().map = this;

    }//Start


    void GenerateMapData()
    {
        Debug.Log("making map data");
        //Allocate map tiles
        tiles = new int[sizeX, sizeY];

        //Initial map tiles 
        for (int x = 0; x < sizeX; ++x)
        {
            for (int y = 0; y < sizeY; ++y)
            {
                tiles[x, y] = 0;
            }
        }

        //Instantiate Board
        for (int x = 0; x < sizeX; ++x)
        {
            for (int y = 0; y < sizeY; ++y)
            {
                tiles[x, y] = WOOD_TILE;
            }
        }
    }//GenerateMapData

    [System.Serializable]
    public class Node {
        public List<Node> edges;
        public float x;
        public float y;

        public Node(){
            edges = new List<Node>();
        }

        public float DistanceTo(Node n)
        {
            return Vector2.Distance(
                    new Vector2(x, y),
                    new Vector2(n.x, n.y)
                    );
        }
    }

    void GeneratePathGraph()
    {
        Debug.Log("Generating Path Graph");
        //Init array
        graph = new Node[sizeX, sizeY];

        //Init a node for each tile being added to the graph 
        //Problem is that this is also including the grass tiles which shouldn't be part of the graph
        for(int x= 0; x < sizeX; x++)
        {
            for(int y= 0; y < sizeY; y++)
            {
                graph[x, y] = new Node
                {
                    x = x * TILE_OFFSET,
                    y = y * TILE_OFFSET
                };
            }
        }

        for (int x = 0; x < sizeX; x++)
        {
            for(int y= 0; y < sizeY; y++)
            {
                //4 edges per node
                if(x > 0)                
                    graph[x, y].edges.Add(graph[x - 1, y]); //connect via left
                if(x < sizeX-1)                
                    graph[x, y].edges.Add(graph[x + 1, y]); //connect via right
                if(y > 0)
                    graph[x, y].edges.Add(graph[x, y - 1]); //connect via bottom
                if (y < sizeY - 1)
                    graph[x, y].edges.Add(graph[x, y + 1]); //connect via top
            }
        }
    }

    void GenerateMapVisual()
    {
        Debug.Log("Generating Map Visual");
        int startX = 1;
        int startY = 1;
        float offsetX = 0;
        float offsetY = 0;

        for (int x = 0; x < sizeX; x++)
        {
            for(int y=0; y < sizeY; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];

                offsetX = x * TILE_OFFSET;
                offsetY = y * TILE_OFFSET;

                GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(offsetX, offsetY, 0), Quaternion.identity);
                
                if(tiles[x,y] != GRASS_TILE)
                {
                    ClickableTile ct = go.GetComponent<ClickableTile>();
                    ct.tileX = offsetX;
                    ct.tileY = offsetY;
                    ct.map = this;
                }
                
            }
        }

        selectedUnit = (GameObject)  Instantiate(unit, new Vector3(startY * TILE_OFFSET, startX * TILE_OFFSET, 0), Quaternion.identity);

    }//GenerateMapVisual

    public Vector3 TileCoordToWorldCoord(float x, float y)
    {
        return new Vector3(x, y, 0);
    }

    public void GeneratePathTo(float x, float y)
    {
        Debug.Log("Generating Path to " +x+ "," + y);
        //Clear old path
        selectedUnit.GetComponent<Unit>().currentPath = null;

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        //list of unvisited nodes
        List<Node> unvisited = new List<Node>();

        Node source = graph[
            (int)selectedUnit.GetComponent<Unit>().unitX,
            (int)selectedUnit.GetComponent<Unit>().unitY
            ];

        Node target = graph[(int)x,(int)y];

        dist[source] = 0;
        prev[source] = null;

        foreach (Node v in graph)
        {
            if(v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while(unvisited.Count > 0)
        {
            //u is going to be the unvisited node with the smallest distance
            Node u = null;

            foreach(Node possibleU in unvisited)
            {
                if(u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            unvisited.Remove(u);

            foreach(Node v in u.edges)
            {
                float alt = dist[u] + u.DistanceTo(v);
                if(alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        if(prev[target] == null)
        {
            //No route
            return;
        }

        List<Node> currentPath = new List<Node>();
        Node current = target;

        //Step back through path
        while(current != null)
        {
            currentPath.Add(current);
            current = prev[current];
        }

        currentPath.Reverse();

        selectedUnit.GetComponent<Unit>().currentPath = currentPath;

    }//Generate Path using Dijkstra's pathfinding algorithm

}
