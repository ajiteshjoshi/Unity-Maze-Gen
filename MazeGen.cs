using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public struct Room
{
    public int x1;
    public int x2;
    public int y1;
    public int y2;

    public Room(int a, int b, int c, int d)
    {
        x1 = a;
        x2 = b;
        y1 = c;
        y2 = d;
    }
}

public class MazeGen : MonoBehaviour
{
    public int w, h = 0;
    public int roomSize;
    public int noOfRooms;
    public bool[,] mazeH;
    public bool[,] mazeV;
    public GameObject wallH;
    public GameObject wallV;
    public GameObject roomSprite;

    private List<Room> rooms;
    void Start()
    {
        mazeH = new bool[w, h];
        mazeV = new bool[w, h];
        rooms = new List<Room>();
        GenerateMaze(0, w, 0, h);
        DrawMaze();

/**
        foreach (Room r in rooms)
        {
            Debug.Log("Room Start");
            Debug.Log(r.x1);
            Debug.Log(r.x2);
            Debug.Log(r.y1);
            Debug.Log(r.y2);
        }
**/

/**
        for (int i = 0; i < w; i++)
        {
            string logStr = "";
            for (int j = 0; j < h; j++)
            {
                logStr += maze[i, j] ? "1" : "0";
            }
            Debug.Log(logStr);
        }
**/
    }

    void GenerateMaze(int x1, int x2, int y1, int y2)
    {
        if (x2 - x1 <= 1 || y2 - y1 <= 1)
        {
            return;
        }

        if (x2 - x1 <  roomSize && y2 -y1 < roomSize && Random.Range(0, 2 * noOfRooms) < noOfRooms && noOfRooms > 0) 
        {
            noOfRooms--;
            rooms.Add(new Room(x1, x2, y1, y2));
            return;
        }

        if (x2 - x1 > y2 - y1)
        {
            int wall = Random.Range(x1 + 1, x2);
            int passage = Random.Range(y1, y2);
            for (int i = y1; i < y2; ++i)
            {
                if (i != passage)
                {
                    mazeV[wall, i] = true;
                }
            }
            GenerateMaze(x1, wall, y1, y2);
            GenerateMaze(wall, x2, y1, y2);
        }
        else
        {
            int wall = Random.Range(y1 + 1, y2);
            int passage = Random.Range(x1, x2);
            for (int i = x1; i < x2; ++i)
            {
                if (i != passage)
                {
                    mazeH[i, wall] = true;
                }
            }
            GenerateMaze(x1, x2, y1, wall);
            GenerateMaze(x1, x2, wall, y2);
        }
    }

    void DrawMaze()
    {
        for (int i = 0; i < w; ++i)
        {
            for(int j = 0; j < h; ++j)
            {
                if (mazeV[i, j])
                {
                    Instantiate(wallV, new Vector3(i, -1 * j - 0.5f, 0), Quaternion.identity);
                }
            }
        }
        for (int j = 0;j < h; ++j)
        {
            for (int i = 0; i < w; i++)
            {
                if (mazeH[i, j])
                {
                    Instantiate(wallH, new Vector3(i + 0.5f, -1 * j, 0), Quaternion.identity);
                }
            }
        }

        for(int i =0; i < w; i++)
        {
            Instantiate(wallH, new Vector3(i + 0.5f, 0f, 0), Quaternion.identity);
            Instantiate(wallH, new Vector3(i + 0.5f, -1 * h, 0), Quaternion.identity);
        }
        for (int j = 0; j < h + 0; j++)
        {
            Instantiate(wallV, new Vector3(0, -1 * j - 0.5f, 0), Quaternion.identity);
            Instantiate(wallV, new Vector3(w, -1 * j - 0.5f, 0), Quaternion.identity);
        }
        
        foreach (Room r in rooms)
        {
            GameObject g = Instantiate(roomSprite, new Vector3((r.x1 + r.x2) / 2f, -1 * (r.y1 + r.y2) / 2f, 0), Quaternion.identity);
            g.transform.localScale = new Vector3((r.x2 -  r.x1 - 0.1f), (r.y2 - r.y1 - 0.1f), 0);
        }
    }
}
