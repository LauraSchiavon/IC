using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeSkeleton;
/// <summary>
/// This class will draws virtual maze skeleton in 2D game space
/// </summary>
public class MazeDrawer2D : MonoBehaviour
{
    public GameObject wallPrefab, startWallPrefab, exitWallPrefab;
    public GameObject teeWallPrefab, crossWallPrefab, impWallPrefab, cornerWallPrefab;
    
    public Vector2 mazePosition = new Vector2();        
    public int mazeWidth = 10;    
    public int mazeHeigh = 10;    
    public float tunnelWigth = 0.4f;
    public int wallPerCellSide=3;
    public int mazeSeed = 1000;
    public bool useSeed = false;
    public int roomCount = 2;
    public int roomWidth = 2;
    public int roomHeight = 2;
    public bool genRooms = true;
    public bool scaleWall = true;
    public bool useCustomWalls = false;
    public bool useStartWall = false;
    public bool useEndtWall = false;
    public bool createOnStart = false;
    public bool useLongWay = false;
    private MazeSkeleton2D maze;
    private List<MazeCell> cells;
    private List<GameObject> objects = new List<GameObject>();
    private int coutn = 0;
    private RoomParams rPar = new RoomParams();
    void Start()
    {
        maze = new MazeSkeleton2D(mazeWidth, mazeHeigh, tunnelWigth, wallPerCellSide, mazePosition);

        AppendMazeParams();

        maze.MazeGenerated = OnMazeGenerated;
        maze.GenerationIsFault = OnMazeGenerationFault;
        if (createOnStart) maze.GenerateMaze();
        
        
    }


    #region Public variables
    /// <summary>
    /// Left Bottom Maze position in scene
    /// </summary>
    public Vector2 MazePosition
    {
        get { return mazePosition; }
        set { mazePosition = value; }
    }

    /// <summary>
    /// Maze width in MazeCells count
    /// </summary>
    public int MazeWidth
    {
        get { return mazeWidth; }
        set { if (value > 0) mazeWidth = value; }
    }

    /// <summary>
    /// Maze height in MazeCells count
    /// </summary>
    public int MazeHeigh
    {
        get { return mazeHeigh; }
        set { if (value > 0) mazeHeigh = value; }
    }

    /// <summary>
    /// Maze tunnel width(MazeCell width)
    /// </summary>
    public float TunnelWigth
    {
        get { return tunnelWigth; }
        set { if (value > 0) tunnelWigth = value; }
    }

    /// <summary>
    /// Wall count per cell side, set minimum value = 3
    /// </summary>
    public int WallPerCellSide
    {
        get { return wallPerCellSide; }
        set { if (value >= 3) wallPerCellSide = value; }
    }

    /// <summary>
    /// Main wall prefab
    /// </summary>
    public GameObject WallPrefab
    {
        get { return wallPrefab; }
        set { wallPrefab = value; }
    }

    /// <summary>
    /// Main wall prefab
    /// </summary>    public 
    GameObject TeeWallPrefab
    {
        get { return teeWallPrefab; }
        set { teeWallPrefab = value; }
    }

    /// <summary>
    /// Cross road wall prefab
    /// </summary>
    public GameObject CrossWallPrefab
    {
        get { return crossWallPrefab; }
        set { crossWallPrefab = value; }
    }

    /// <summary>
    /// Impasse wall prefab
    /// </summary>
    public GameObject ImpasseWallPrefab
    {
        get { return impWallPrefab; }
        set { impWallPrefab = value; }
    }

    /// <summary>
    /// Corner wall prefab
    /// </summary>
    public GameObject CornerWallPrefab
    {
        get { return cornerWallPrefab; }
        set { cornerWallPrefab = value; }
    }


    #endregion


    #region public functions
    /// <summary>
    /// Draws maze walls
    /// </summary>
    public void DrawMaze()
    {        
        if (maze!=null)
        {
            List<MazeWall> walls = maze.GetWalls();
            List<MazeCell> longWay = maze.GetMazeLongWayCells();
            List<Vector2> end = maze.GetExitWallsPosition(), start=maze.GetStartWallsPosition();
            float wallWidth = maze.WallWidth;
            GameObject prefab = wallPrefab;
            foreach (MazeWall w in walls)
            {
                if(useCustomWalls)
                {                   
                    switch (w.Type)
                    {
                        case WALL_TYPES.NONE:
                            prefab = wallPrefab;
                            break;
                        case WALL_TYPES.CORNER_BOT_LEFT:
                            prefab = cornerWallPrefab;
                            break;
                        case WALL_TYPES.CORNER_BOT_RIGHT:
                            prefab = cornerWallPrefab;
                            break;
                        case WALL_TYPES.CORNER_TOP_LEFT:
                            prefab = cornerWallPrefab;
                            break;
                        case WALL_TYPES.CORNER_TOP_RIGHT:
                            prefab = cornerWallPrefab;
                            break;
                        case WALL_TYPES.CROSSROAD:
                            prefab = crossWallPrefab;
                            break;
                        case WALL_TYPES.HORIZONTAL:
                            prefab = wallPrefab;
                            break;
                        case WALL_TYPES.HORIZONTAL_IMPASS_LEFT:
                            prefab = impWallPrefab;
                            break;
                        case WALL_TYPES.HORIZONTAL_IMPASS_RIGHT:
                            prefab = impWallPrefab;
                            break;
                        case WALL_TYPES.TEE_BOT:
                            prefab = teeWallPrefab;
                            break;
                        case WALL_TYPES.TEE_LEFT:
                            prefab = teeWallPrefab;
                            break;
                        case WALL_TYPES.TEE_RIGHT:
                            prefab = teeWallPrefab;
                            break;
                        case WALL_TYPES.TEE_TOP:
                            prefab = teeWallPrefab;
                            break;
                        case WALL_TYPES.VERTICAL:
                            prefab = wallPrefab;
                            break;
                        case WALL_TYPES.VERTICAL_IMPASS_BOT:
                            prefab = impWallPrefab;
                            break;
                        case WALL_TYPES.VERTICAL_IMPASS_TOP:
                            prefab = impWallPrefab;
                            break;                       

                    }

                    if (w.isExit) prefab = exitWallPrefab;
                    if (w.isStart) prefab = startWallPrefab;

                    CreateWall(w.Position, prefab, wallWidth);
                }
                else
                {
                    prefab = wallPrefab;
                    if (w.isExit) prefab = exitWallPrefab;
                    if (w.isStart) prefab = startWallPrefab;
                    CreateWall(w.Position, prefab, wallWidth);
                }

            }            

        }
    }
    /// <summary>
    /// Generates new maze with random start, if used long way, exit cell will be on last position of long way
    /// </summary>
    public void GenerateMazeWithRandomStart()
    {
        DeleteMaze();
        maze.SetMazeParams(mazeWidth, mazeHeigh, tunnelWigth, wallPerCellSide, mazePosition);

        AppendMazeParams();

        List<MazeCell> grid = maze.GetMazeCellsList();
        var rnd = new System.Random(mazeSeed);
        int index = 0;
        if (useSeed) index = rnd.Next(0, grid.Count);
        else index = UnityEngine.Random.Range(0, grid.Count);

        maze.SetStartCell(grid[index]);
        maze.GenerateMaze();
    }

    /// <summary>
    /// Generates new maze
    /// </summary>    
    public void GenerateNewMaze()
    {
        DeleteMaze();

        AppendMazeParams();

        maze.GenerateNewMaze(mazeWidth, mazeHeigh, tunnelWigth, wallPerCellSide, mazePosition);
    }

    /// <summary>
    /// If set true, maze will use fixed seed, you can use it for same mazes
    /// </summary>
    /// <param name="use"></param>
    /// <param name="value"></param>
    public void SetMazeSeed(bool use, int value = 1000)
    {
        maze.SetMazeSeed(use, value);
    }       

    /// <summary>
    /// Retuns list of maze rooms
    /// </summary>
    /// <returns></returns>
    public List<MazeRoom> GetMazeRooms()
    {
        return maze.GetMazeRooms();
    }

    /// <summary>
    /// Returns start cell
    /// </summary>
    /// <returns></returns>
    public MazeCell GetStartCell()
    {
        return maze.GetStartCell();
    }
    /// <summary>
    /// Returns exit cell
    /// </summary>
    /// <returns></returns>
    public MazeCell GetExitCell()
    {
        return maze.GetExitCell();
    }

    /// <summary>
    /// Returns list of Maze cells, each cell is a tunnel cell
    /// </summary>
    /// <returns></returns>
    public List<MazeCell> GetTunnels()
    {
        return maze.GetTunnels();
    }

    /// <summary>
    /// Returns list of Maze cells, each cell is a cross road cell
    /// </summary>
    /// <returns></returns>
    public List<MazeCell> GetCrossRoads()
    {
        return maze.GetCrossRoads();
    }
    /// <summary>
    /// Returns list of Maze cells, each cell is a impasse cell
    /// </summary>
    /// <returns></returns>
    public List<MazeCell> GetImpassess()
    {
        return maze.GetImpassess();
    }

    /// <summary>
    /// Returns way from c1 to c2
    /// </summary>
    /// <returns></returns>
    public List<MazeCell> FindWayFromTo(MazeCell c1, MazeCell c2)
    {
        return maze.FindWayFromStartToExit(c1, c2);
    }

    /// <summary>
    /// Returns way from start to exit
    /// </summary>
    /// <returns></returns>
    public List<MazeCell> FindWayFromStartToExit()
    {
        return maze.FindWayFromStartToExit();
    }
    /// <summary>
    /// Clears maze skeleton data
    /// </summary>
    public void DeleteMaze()
    {
        UnDrawMaze();
        maze.DeleteMaze();        
    }
    /// <summary>
    /// Returns maze rect
    /// </summary>
    /// <returns></returns>
    public Rect GetMazeRect()
    {
        return maze.GetMazeRect();
    }

    /// <summary>
    /// Generates new maze
    /// </summary>
    public void GenerateMaze()
    {
        maze.GenerateMaze();
    }
    /// <summary>
    /// Returns List of all cells in the way from start position to 
    /// </summary>
    /// <returns></returns>
    public List<MazeCell> GetMazeLongWayCells()
    {
        return maze.GetMazeLongWayCells(); ;
    }
    /// <summary>
    /// Returns list of all maze cells
    /// </summary>
    /// <returns></returns>
    public List<MazeCell> GetMazeCellsList()
    {
        return maze.GetMazeCellsList();
    }   

    #endregion

    #region
    private void CreateWall(Vector2 pos, GameObject prefab, float width)
    {
        GameObject wall = (GameObject)Instantiate(prefab, pos, Quaternion.identity, transform);
        if (scaleWall)
        {
            wall.transform.localScale = new Vector3(width, width, width);
        }
        objects.Add(wall);
    }
    private void AppendMazeParams()
    {
        rPar.roomCount = roomCount;
        rPar.roomHeight = roomHeight;
        rPar.roomWidth = roomWidth;

        maze.SetRoomParams(genRooms, rPar);
        maze.SetUseLongWay(useLongWay);
        maze.SetMazeSeed(useSeed, mazeSeed);
    }
    private void OnDebugPoint(Vector2 pos)
    {
        GameObject wall = (GameObject)Instantiate(wallPrefab, pos, Quaternion.identity, transform);
        wall.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        wall.name = "debugPoint";
    }
    private void UnDrawMaze()
    {
        foreach(GameObject obj in objects)
        {
            Destroy(obj);
        }
        objects.Clear();
    }
    private void OnMazeGenerated()
    {
        DrawMaze();
    }
    private void OnMazeGenerationFault()
    {
        DeleteMaze();
        GenerateMaze();
    }
    #endregion
}
