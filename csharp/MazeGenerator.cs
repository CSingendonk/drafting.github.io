using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Author: Christopher Singendonk
//Date: January 17 2024
public class MazeGenerator : MonoBehaviour
{
    public int mazeWidth;
    public int mazeHeight;
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject startPrefab;
    public GameObject endPrefab;
    public GameObject playerCapsule;
    public Color floorColour;
    public Material wallMaterial;
    public Color wallColour;
    public Color startColour;
    public Color endColour;
    public GameObject supplyPrefab;
    private bool[,] maze;
    private (int, int) secondEndYX;
    public List<(int, int)> deadEnds;

    void Start()
    {
        if (mazeHeight <= 10)
        {
            mazeHeight = 11;
        }
        if (mazeWidth <= 10)
        {
            mazeWidth = 11;
        }
        if (mazeHeight % 2 == 0)
        {
            mazeHeight++;
        }
        if (mazeWidth % 2 == 0)
        {
            mazeWidth++;
        }
        maze = new bool[mazeWidth, mazeHeight];

        int startX = 1;
        int startY = 0;
        secondEndYX = (new System.Random().Next(mazeHeight / 2, mazeHeight), maze.Length / 2 - mazeWidth);
        FuckIt(startX.ToString(), startY, this);
        ReadySteadyGo(playerCapsule, this, WallszCount);
    }

    private Vector3 fuck = new Vector3(0,0,0);
    private Vector3 you = new Vector3(0f, (float)(new System.Random().NextDouble()), 0f);
    public GameObject line;
    void Update()
    {
        LineRenderer j = line.GetComponent<LineRenderer>();
        j.startColor = Color.red;
        j.useWorldSpace = true;
        j.startWidth = j.startWidth + 1 * 2;
        j.endWidth = j.startWidth * 2;
        j.textureMode = LineTextureMode.Stretch;
        j.allowOcclusionWhenDynamic = false;
        j.bounds = new Bounds(Vector3.zero, fuck);
        j.enabled = true;
        fuck.y = (1 / you.y * 10);
        you = playerCapsule.transform.position;
        fuck.x = (float)(((float)(fuck.x++) + (new System.Random().NextDouble())));
        Debug.DrawLine(fuck, you, floorColour, float.PositiveInfinity, false);
    }



    protected void GenerateMaze((int, int) startXCustom, (int, int) startYCustom)
    {
        if (startXCustom.Item1 <= startYCustom.Item1 && startXCustom.Item2 <= startYCustom.Item2)
        {
            return;
        }
        int sX = startXCustom.Item2;
        int sY = startXCustom.Item1;
        int eX = startYCustom.Item2;
        int eY = startYCustom.Item1;
        maze[sY, sX] = true;

        int[][] directions = new int[][]
        {
            new int[] {-2, 0},
            new int[] {2, 0},
            new int[] {0, -2},
            new int[] {0, 2}
        };

        System.Random rng = new System.Random();

        directions = directions.OrderBy(dir => rng.Next()).ToArray();

        foreach (int[] direction in directions)
        {
            int dy = direction[0];
            int dx = direction[1];

            int newX = sX - dx;
            int newY = sY - dy;

            if (IsValid(newY, newX) && !maze[newY, newX])
            {
                maze[sY - dy / 2, sX - dx / 2] = true;
                GenerateMaze((newY, newX), (eY, eX));
            }
        }
    }
    private static void ReadySteadyGo(GameObject who, MazeGenerator what, int where)
    {
        if (who != null && what != null && where != 0)
        {
            int i = 0;
            int j = 0;
            foreach (bool tf in what.maze)
            {

                i++;
                j = i >= what.mazeWidth ? (j < what.mazeHeight ? j++ : j) : j;
                i = i >= what.mazeWidth ? i = 0 : i++;
                if (tf)
                {
                    if (who.transform.position.x != j * where && who.transform.position.z != i * where)
                    {
                        who.transform.Translate(new Vector3(j * where, who.transform.position.y, i * where));
                        break;
                    }
                }
            }
        }
    }
    [Range(1,10)]
    public int WallszCount = 4;
    protected void PresentMaze()
    {
        for (int xx = 0; xx < mazeHeight; xx++)
        {
            for (int yx = 0; yx < mazeWidth; yx++)
            {
                foreach ((int, int) pos in deadEnds)
                {
                    if ((xx, yx) == pos)
                    {
                        SpawnObject(pos, supplyPrefab);
                        break;
                    }
                }
                if (xx == 0)
                {
                    GameObject wall = Instantiate(wallPrefab, new Vector3((xx - 1) * 4, 2f, yx * 4), Quaternion.identity, transform);
                    wall.GetComponent<Renderer>().material.color = wallColour;
                }
                if (xx+1 >= maze.GetUpperBound(1))
                {

                    GameObject wall = Instantiate(wallPrefab, new Vector3((xx + 1) * 4, 2f, yx * 4), Quaternion.identity, transform);
                    wall.GetComponent<Renderer>().material.color = wallColour;
                }
                if (yx == 0)
                {
                    GameObject wall = Instantiate(wallPrefab, new Vector3(xx * 4, 2f,  (yx - 1) * 4 ), Quaternion.identity, transform);
                    wall.GetComponent<Renderer>().material.color = wallColour;
                }
                if (yx >= maze.GetUpperBound(0))
                {

                    GameObject wall = Instantiate(wallPrefab, new Vector3(xx * 4, 2f, (yx + 1) * 4), Quaternion.identity, transform);
                    wall.GetComponent<Renderer>().material.color = wallColour;
                }
                if (xx == 1 && yx == 1)
                {
                    GameObject specialCube = Instantiate(startPrefab, new Vector3(xx * 4, 0.5f, yx * 4), Quaternion.identity, transform);
                    specialCube.GetComponent<Renderer>().material.color = startColour;
                    continue;
                }
                if (xx + 1 >= maze.GetUpperBound(0) && yx + 1 >= maze.GetUpperBound(1))
                {
                    GameObject specialCube = Instantiate(endPrefab, new Vector3(xx * 4, 0.5f, yx * 4), Quaternion.identity, transform);
                    specialCube.GetComponentInChildren<Renderer>().material.color = endColour;
                    continue;
                }
                if (xx == secondEndYX.Item1 && yx == secondEndYX.Item2)
                {
                    if (!maze[secondEndYX.Item1, secondEndYX.Item2])
                    {
                        var tenant = gameObject.transform.GetChild(xx-1 * yx-1);
                        if (tenant != null)
                        {
                            if ((int)tenant.position.x + (int)tenant.position.z == (int)xx+(int)yx)
                            Destroy(tenant.gameObject);
                        }
                    }
                    GameObject specialCube = Instantiate(endPrefab, new Vector3(xx, 0.75f, yx), Quaternion.identity, transform);
                    specialCube.GetComponentInChildren<Renderer>().material.color = endColour;
                    specialCube.transform.parent.gameObject.transform.position = new Vector3(xx, 0.5f, yx);
                    specialCube.GetComponentsInChildren<Rigidbody>()[0].useGravity = false;

                    continue;
                }

                if (maze[xx, yx])
                {
                    GameObject floor = Instantiate(floorPrefab, new Vector3(xx * 4, 0f, yx * 4), Quaternion.identity, transform);
                    continue;
                }
                if (!maze[xx, yx])
                {
                    GameObject wall = Instantiate(wallPrefab, new Vector3(xx * 4, 2f, yx * 4), Quaternion.identity, transform);
                    wall.GetComponent<Renderer>().material.color = wallColour;
                    continue;
                }
            }
        }

    }

    private List<(int, int)> FindDeadEnds()
    {
        List<(int, int)> ends = new List<(int, int)>();

        for (int xx = 0; xx < mazeHeight; xx++)
        {
            for (int yx = 0; yx < mazeWidth; yx++)
            {
                if (maze[xx, yx] && CountNeighboringWalls(xx, yx) >= 0b111)
                {
                    ends.Add((xx, yx));
                }
            }
        }

        return ends;
    }

    private int CountNeighboringWalls(int x, int y)
    {
        int wallCount = 0;

        for (int dx = -1; dx < 2; dx++)
        {
            for (int dy = -1; dy < 2; dy++)
            {
                int newX = x + dx;
                int newY = y + dy;

                if (IsValid(newX, newY) && !maze[newX, newY])
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }



    void SpawnObject((int, int) position, GameObject thing)
    {
        GameObject spawnedObject = Instantiate(thing, new Vector3(position.Item1 * WallszCount, 0f, position.Item2 * WallszCount), Quaternion.identity, transform);
    }

    private void GenerateMaze(int x, int y)
    {
        maze[x, y] = true;

        int[][] directions = new int[][]
        {
            new int[] {-2, 0},
            new int[] {2, 0},
            new int[] {0, -2},
            new int[] {0, 2}
        };

        System.Random rng = new System.Random();

        directions = directions.OrderBy(dir => rng.Next()).ToArray();

        foreach (int[] direction in directions)
        {
            int dx = direction[0];
            int dy = direction[1];

            int newX = x + dx;
            int newY = y + dy;

            if (IsValid(newX, newY) && !maze[newX, newY])
            {
                maze[x + dx / 2, y + dy / 2] = true;
                GenerateMaze(newX, newY);
            }
        }
    }

    bool IsValid(int x, int y)
    {
        return x >= 0 && x < mazeHeight && y >= 0 && y < mazeWidth;
    }


	private protected static void FuckIt(string x, int y, MazeGenerator fuckThat)
	{
		var startX = int.Parse(x);
		var startY = y;
		fuckThat.GenerateMaze(startX, startY);
		fuckThat.deadEnds = (fuckThat.FindDeadEnds());
		PlayerScript.SetCratesTotal(fuckThat.deadEnds.Count, fuckThat.playerCapsule.GetComponent<PlayerScript>());
		fuckThat.PresentMaze();
		//for (int i = 0; i == 0; i += i - i)
		//{
		//    // nothing to see here.
		//    Debug.LogError("em llet uoy -_o.O_- ton ro erehwemos rorre a ereht saw");
		//    ScreenCapture.CaptureScreenshot("Say Cheese!", i + 1);
		//}
	}

	public MazeGenerator(char x)
	{
		var fuckThat = new MazeGenerator(' ');
		int y = 0;
		var startX = (int)x;
        var startY = y;
        fuckThat.GenerateMaze(secondEndYX, secondEndYX);
        fuckThat.deadEnds = (fuckThat.FindDeadEnds());
        PlayerScript.SetCratesTotal(fuckThat.deadEnds.Count, fuckThat.playerCapsule.GetComponent<PlayerScript>());
        //fuckThat.PresentMaze();
    }
}
