using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace MazeSkeleton
{
    public struct RoomParams
    {
        public int roomCount, roomWidth, roomHeight;
    }
    public enum SIDE
    {
        TOP,
        LEFT,
        BOT,
        RIGHT
    }
    public enum WALL_TYPES
    {
        HORIZONTAL,
        VERTICAL,
        TEE_LEFT,
        TEE_RIGHT,
        TEE_TOP,
        TEE_BOT,
        CROSSROAD,
        VERTICAL_IMPASS_TOP,
        VERTICAL_IMPASS_BOT,
        HORIZONTAL_IMPASS_LEFT,
        HORIZONTAL_IMPASS_RIGHT,
        CORNER_TOP_LEFT,
        CORNER_BOT_LEFT,
        CORNER_TOP_RIGHT,
        CORNER_BOT_RIGHT,
        NONE
    }

    /// <summary>
    /// Main class, which generating virtual maze in 2D space
    /// </summary>
    public class MazeSkeleton2D
    {       

        #region private variables
        private List<MazeCell> cells = new List<MazeCell>();
        private int mazeWidth, mazeHeight, wallCountPerCell;
        private float cellWidth, cellStep, wallWidth, mazeTunnelWidth;
        private Vector2 leftBotPos;
        private float mazeWorldWidth, mazeWorldHeight;

        private MazeCell startCell, endCell;
        private int mazeSeed = 1000;
        private bool useSeed = false;
        private bool isGen = false;
        private bool useLongWay = true;
        private bool generateRooms = true;
        private List<MazeCell> longWay = new List<MazeCell>();
        private List<Vector2> wallPosArray = new List<Vector2>(), startWallPos = new List<Vector2>(), endWallPos = new List<Vector2>();

        private List<MazeCell> impasses = new List<MazeCell>();
        private List<MazeCell> crossRoads = new List<MazeCell>();
        private List<MazeCell> tunnelRoads = new List<MazeCell>();
        private List<MazeWall> walls = new List<MazeWall>();
        private List<MazeRoom> rooms = new List<MazeRoom>();

        private RoomParams roomParams;
        private Rect mazeRect;
        #endregion

        #region Public Events
        /// <summary>
        /// Event, triggered when maze is generated success
        /// </summary>
        public Action MazeGenerated = null;

        /// <summary>
        /// Event, triggered when error was in generation
        /// </summary>
        public Action GenerationIsFault = null;

        /// <summary>
        /// Debug Event, not used
        /// </summary>
        public Action<Vector2> DebugPoint=null;
        #endregion

        #region Constructors
        public MazeSkeleton2D(int width, int height, float tunnelWidth, int wallsPerCell, Vector2 leftBottomPosition)
        {
            mazeWidth = width;
            mazeHeight = height;
            mazeTunnelWidth = tunnelWidth;
            wallCountPerCell = wallsPerCell;
            leftBotPos = leftBottomPosition;

            roomParams.roomCount = 2;
            roomParams.roomHeight = 2;
            roomParams.roomWidth = 2;

            CalcMazeParameters();
            CalcMazeGrid();

        }
        #endregion

        #region Public variables
        public float WallWidth
        {
            get { return wallWidth; }
        }
        #endregion
        #region Public Functions
        /// <summary>
        /// Returns true, if position in wall
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsThereWall(Vector2 position)
        {
            bool toRet = false;

            if (isGen)
            {
                float dist = float.MaxValue;
                for (int i = 0; i < wallPosArray.Count; i++)
                {
                    dist = Vector2.Distance(position, wallPosArray[i]);

                    if (dist <= wallWidth / 2)
                    {
                        toRet = true;
                        break;
                    }

                }
            }
            return toRet;
        }
        /// <summary>
        /// Returns list of Vector2 positions of walls on start position
        /// </summary>
        /// <returns></returns>
        public List<Vector2> GetStartWallsPosition()
        {
            return startWallPos;
        }
        /// <summary>
        /// Returns list of Vector2 positions of walls on exit position
        /// </summary>
        /// <returns></returns>
        public List<Vector2> GetExitWallsPosition()
        {
            return endWallPos;
        }
        /// <summary>
        /// Generates new maze
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="tunnelWidth"></param>
        /// <param name="wallsPerCell"></param>
        /// <param name="leftBottomPosition"></param>
        public void GenerateNewMaze(int width, int height, float tunnelWidth, int wallsPerCell, Vector2 leftBottomPosition)
        {
            mazeWidth = width;
            mazeHeight = height;
            mazeTunnelWidth = tunnelWidth;
            wallCountPerCell = wallsPerCell;
            leftBotPos = leftBottomPosition;           
            DeleteMaze();
            
            CalcMazeParameters();
            CalcMazeGrid();
            GenerateMaze();
        }

        /// <summary>
        /// If set true, maze will use fixed seed, you can use it for same mazes
        /// </summary>
        /// <param name="use"></param>
        /// <param name="value"></param>
        public void SetMazeSeed(bool use, int value=1000)
        {
            useSeed = use;
            mazeSeed = value;
        }

        /// <summary>
        /// If set true, end position will be setted to end cell of long way in maze from start cell
        /// </summary>
        /// <param name="use"></param>
        public void SetUseLongWay(bool use)
        {
            useLongWay = use;
        }

        /// <summary>
        /// Set room generation mode, if createRooms is false, rooms will be not created
        /// </summary>
        /// <param name="createRooms"></param>
        /// <param name="pars"></param>
        public void SetRoomParams(bool createRooms, RoomParams pars)
        {
            generateRooms = createRooms;
            roomParams = pars;
        }

        /// <summary>
        /// Will delete old maze and set new params
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="tunnelWidth"></param>
        /// <param name="wallsPerCell"></param>
        /// <param name="leftBottomPosition"></param>
        public void SetMazeParams(int width, int height, float tunnelWidth, int wallsPerCell, Vector2 leftBottomPosition)
        {
            DeleteMaze();

            mazeWidth = width;
            mazeHeight = height;
            mazeTunnelWidth = tunnelWidth;
            wallCountPerCell = wallsPerCell;
            leftBotPos = leftBottomPosition;
            
            CalcMazeParameters();
            CalcMazeGrid();
        }
        /// <summary>
        /// Returns start cell
        /// </summary>
        /// <returns></returns>
        public MazeCell GetStartCell()
        {
            return startCell;
        }
        /// <summary>
        /// Returns exit cell
        /// </summary>
        /// <returns></returns>
        public MazeCell GetExitCell()
        {
            return endCell;
        }
        /// <summary>
        /// Retuns list of maze walls
        /// </summary>
        /// <returns></returns>
        public List<MazeWall> GetWalls()
        {
            return walls;
        }

        /// <summary>
        /// Retuns list of maze rooms
        /// </summary>
        /// <returns></returns>
        public List<MazeRoom> GetMazeRooms()
        {
            return rooms;
        }

        /// <summary>
        /// Returns list of Maze cells, each cell is a tunnel cell
        /// </summary>
        /// <returns></returns>
        public List<MazeCell> GetTunnels()
        {
            return tunnelRoads;
        }

        /// <summary>
        /// Returns list of Maze cells, each cell is a cross road cell
        /// </summary>
        /// <returns></returns>
        public List<MazeCell> GetCrossRoads()
        {
            return crossRoads;
        }
        /// <summary>
        /// Returns list of Maze cells, each cell is a impasse cell
        /// </summary>
        /// <returns></returns>
        public List<MazeCell> GetImpassess()
        {
            return impasses;
        }

        /// <summary>
        /// Returns way from c1 to c2
        /// </summary>
        /// <returns></returns>
        public List<MazeCell> FindWayFromStartToExit(MazeCell c1, MazeCell c2)
        {
            return FindWayFromTo(c1, c2);
        }

        /// <summary>
        /// Returns way from start to exit
        /// </summary>
        /// <returns></returns>
        public List<MazeCell> FindWayFromStartToExit()
        {
            if(useLongWay)
            {
                return longWay;
            }
            else return FindWayFromTo(startCell, endCell);
        }
        /// <summary>
        /// Clears maze skeleton data
        /// </summary>
        public void DeleteMaze()
        {
            cells.Clear();
            longWay.Clear();
            wallPosArray.Clear();
            startWallPos.Clear();
            endWallPos.Clear();
            impasses.Clear();
            crossRoads.Clear();
            tunnelRoads.Clear();
            rooms.Clear();
            walls.Clear();
            isGen = false;
        }
        /// <summary>
        /// Returns maze rect
        /// </summary>
        /// <returns></returns>
        public Rect GetMazeRect()
        {
            return mazeRect;
        }

        /// <summary>
        /// Generates new maze
        /// </summary>
        public void GenerateMaze()
        {
            if (isGen) DeleteMaze();
            if(generateRooms)
            {
                GenerateRooms();                
            }
            GenerateMazeByBackTrackMethod();
        }
        /// <summary>
        /// Returns List of all cells in the way from start position to 
        /// </summary>
        /// <returns></returns>
        public List<MazeCell> GetMazeLongWayCells()
        {
            return longWay;
        }
        /// <summary>
        /// Returns list of all maze cells
        /// </summary>
        /// <returns></returns>
        public List<MazeCell> GetMazeCellsList()
        {
            return cells;
        }
        /// <summary>
        /// Returns lists of all wall position for spawn
        /// </summary>
        /// <returns></returns>
        public List<Vector2> GetWallPositions()
        {
            return wallPosArray;
        }
        /// <summary>
        /// Sets start cell, if maze was not generated, will create impasse in this cell
        /// </summary>
        /// <param name="c"></param>
        public void SetStartCell(MazeCell c)
        {
            if(isGen)
            {
                startCell = c;
                Debug.Log("tyt");
            }
            else
            {
                Debug.Log("tyt2");
                Dictionary<SIDE, MazeCell> neigs = c.GetNeighbors();
                SIDE s= SIDE.BOT;
                MazeCell n = null;
                int side = 0;
                var rnd = new System.Random(mazeSeed);
                if (useSeed) side = rnd.Next(0, neigs.Count);
                else side = UnityEngine.Random.Range(0, neigs.Count);
                int index = 0;
                foreach(SIDE key in neigs.Keys)
                {
                    if(index == side)
                    {
                        s = key;
                        n = neigs[key];
                        break;
                    }
                    index++;
                }
                c.ClearNeighbors();
                c.AddNeighbor(s, n);
                startCell = c;
            }
           
        }
        /// <summary>
        /// Sets exit cell
        /// </summary>
        /// <param name="c"></param>
        public void SetExitCell(MazeCell c)
        {
            endCell = c;
        }
        #endregion

        #region private functions
        /// <summary>
        /// Calculates maze parameters: cell size, cell step and e.t.c
        /// </summary>
        private void CalcMazeParameters()
        {
            wallWidth = mazeTunnelWidth / (wallCountPerCell - 2);
            cellWidth = mazeTunnelWidth + wallWidth * 2;
            cellStep = mazeTunnelWidth + wallWidth;
            mazeWorldWidth = mazeWidth * mazeTunnelWidth + (mazeWidth+1)* wallWidth;
            mazeWorldHeight = mazeHeight * mazeTunnelWidth + (mazeHeight + 1) * wallWidth;
            mazeRect = new Rect(leftBotPos.x- mazeTunnelWidth / 2- wallWidth, leftBotPos.y - mazeTunnelWidth / 2 - wallWidth, mazeWorldWidth, mazeWorldHeight);
            
            // if left bot pos is pos of corner
            //leftBotPos = new Vector2(leftBotPos.x+ cellStep/2, leftBotPos.y+ cellStep/2);
        }
        /// <summary>
        /// Calculates grid of cells
        /// </summary>
        private void CalcMazeGrid()
        {
            Vector2 pos = Vector2.zero;
            cells = new List<MazeCell>();
            for (int i = 0; i < mazeWidth; i++)
            {
                for (int j = 0; j < mazeHeight; j++)
                {
                    pos = new Vector2(leftBotPos.x + i * cellStep, leftBotPos.y + j * cellStep);
                    MazeCell c = new MazeCell(pos, cells.Count, i, j);

                    if (j == 0 || j == (mazeHeight - 1) || i == 0 || i == (mazeWidth - 1)) c.IsCellBorder = true;
                    else c.IsCellBorder = false;
                    cells.Add(c);
                }
            }
            isGen = false;
            startCell = cells[0];
            endCell = cells[cells.Count - 1];
            CreateNeighbors();
        }
        /// <summary>
        /// Function will find all neightbors for each cell
        /// </summary>
        private void CreateNeighbors()
        {
            foreach (MazeCell c in cells)
            {
                Vector2 leftPosition = new Vector2(c.GridX - 1, c.GridY);
                Vector2 rightPosition = new Vector2(c.GridX + 1, c.GridY);
                Vector2 botPosition = new Vector2(c.GridX, c.GridY - 1);
                Vector2 topPosition = new Vector2(c.GridX, c.GridY + 1);
                foreach (MazeCell cell in cells)
                {
                    if ((cell.GridX == (int)leftPosition.x) && (cell.GridY == (int)leftPosition.y))
                    {
                        c.AddNeighbor(SIDE.LEFT, cell);
                    }
                    if ((cell.GridX == (int)rightPosition.x) && (cell.GridY == (int)rightPosition.y))
                    {
                        c.AddNeighbor(SIDE.RIGHT, cell);
                    }
                    if ((cell.GridX == (int)botPosition.x) && (cell.GridY == (int)botPosition.y))
                    {
                        c.AddNeighbor(SIDE.BOT, cell);
                    }
                    if ((cell.GridX == (int)topPosition.x) && (cell.GridY == (int)topPosition.y))
                    {
                        c.AddNeighbor(SIDE.TOP, cell);
                    }                    
                }                
            }
        }

        /// <summary>
        /// Function will generate maze, using back track method
        /// </summary>
        private void GenerateMazeByBackTrackMethod()
        {
            MazeCell currentCell = startCell;
            MazeCell nextCell = null;
            List<MazeCell> neig = null;
            
            bool done = false;
            var rnd = new System.Random(mazeSeed);
            int count = 0;
            int side;
            List<MazeCell> pointsToReturn = new List<MazeCell>();

            List<List<MazeCell>> ways = new List<List<MazeCell>>();
            Dictionary<int, List<MazeCell>> tempPoints = new Dictionary<int, List<MazeCell>>();
            List<MazeCell> temp = new List<MazeCell>();
            MazeCell curTempPoint = null;

            while (!done)
            {
                count++;
                nextCell = null;
                if (count > (mazeWidth * mazeHeight * 20))
                {
                    Debug.Log("Generation is fault!");
                    GenerationIsFault?.Invoke();
                    break;
                }
                if (GetDontVisitedNeighbors(currentCell, out neig))
                {
                    if (neig.Count > 1)
                    {
                        pointsToReturn.Add(currentCell);
                        if (curTempPoint != currentCell)
                        {
                            if (!tempPoints.ContainsKey(currentCell.ID))
                            {
                                tempPoints.Add(currentCell.ID, temp);
                            }
                            curTempPoint = currentCell;
                            temp = new List<MazeCell>();
                        }
                    }
                    if (useSeed) side = rnd.Next(0, neig.Count);
                    else side = UnityEngine.Random.Range(0, neig.Count);
                    nextCell = neig[side];
                    RemoveWallBetweenCells(currentCell, nextCell);

                    currentCell.IsCellVisited = true;
                    nextCell.IsCellVisited = true;
                    currentCell = nextCell;
                    temp.Add(currentCell);                    
                    neig.Clear();
                }
                else
                {
                    if (pointsToReturn.Count > 0)
                    {
                        currentCell = pointsToReturn[pointsToReturn.Count - 1];
                        pointsToReturn.RemoveAt(pointsToReturn.Count - 1);

                        if (tempPoints.ContainsKey(curTempPoint.ID))
                        {
                            List<MazeCell> w1 = new List<MazeCell>();

                            foreach (int wKey in tempPoints.Keys)
                            {
                                List<MazeCell> tempWay = tempPoints[wKey];
                                foreach (MazeCell c in tempWay) w1.Add(c);
                            }
                            foreach (MazeCell c in temp) w1.Add(c);
                            ways.Add(w1);
                            temp = new List<MazeCell>();

                        }

                        if (!GetDontVisitedNeighbors(currentCell, out neig))
                        {
                            tempPoints.Remove(currentCell.ID);
                        }

                    }
                    else
                    {

                    }
                }
                done = IsAllCellsIsVisited();
                if (done)
                {

                    isGen = true;

                    int max = 0;
                    int wayID = -1;
                    for (int i = 0; i < ways.Count; i++)
                    {
                        if (ways[i].Count > max)
                        {
                            wayID = i;
                            max = ways[i].Count;
                        }
                    }

                    if (wayID > 0)
                    {
                        longWay = ways[wayID];
                        if (useLongWay)
                        {
                            endCell = longWay[longWay.Count - 1];
                        }
                        SortWay(longWay);
                    }

                    WriteWallPositions(startCell);
                    WriteWallPositions(endCell);
                    for (int i = 0; i < cells.Count; i++)
                    {
                        WriteWallPositions(cells[i]);
                    }
                    CalcMazeCellTypes();
                    CaclWallTypes();                   

                    
                    MazeGenerated?.Invoke();
                }

            }
           

        }
        private bool FindWays(MazeCell c, out List<MazeCell> ways)
        {
            ways = new List<MazeCell>();
            Dictionary<SIDE, MazeCell> neigs = c.GetNeighbors();
            foreach(SIDE key in neigs.Keys)
            {
                if(!neigs[key].IsCellVisited)
                {
                    switch(key)
                    {
                        case SIDE.BOT:
                            if(!neigs[key].TopWall && !c.BotWall)
                            {
                                ways.Add(neigs[key]);
                            }
                            break;
                        case SIDE.TOP:
                            if (!neigs[key].BotWall && !c.TopWall)
                            {
                                ways.Add(neigs[key]);
                            }
                            break;
                        case SIDE.LEFT:
                            if (!neigs[key].RightWall && !c.LeftWall)
                            {
                                ways.Add(neigs[key]);
                            }
                            break;
                        case SIDE.RIGHT:
                            if (!neigs[key].LeftWall && !c.RightWall)
                            {
                                ways.Add(neigs[key]);
                            }
                            break;
                    }
                }
            }
            return ways.Count>0;
        }
        /// <summary>
        /// Returns List of cells, from first position to last position;
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private List<MazeCell> FindWayFromTo(MazeCell from, MazeCell to)
        {
            SetAllCellsNotVisited();
            List<MazeCell> way = new List<MazeCell>();
            MazeCell currentCell = from;
            MazeCell nextCell = null;
            List<MazeCell> neig = null;
            bool done = false;            
            int count = 0;            
            List<MazeCell> pointsToReturn = new List<MazeCell>();

            while (!done)
            {
                count++;
                
                if (count > (mazeWidth * mazeHeight * 20))
                {
                    Debug.Log("Path finding is fault!");

                    break;
                }
                if (FindWays(currentCell, out neig))
                {
                    if (neig.Count > 1)
                    {
                        pointsToReturn.Add(currentCell);                        
                    }                 
                    
                    currentCell.IsCellVisited = true;
                   
                    currentCell = neig[0];
                    
                    way.Add(currentCell);
                    neig.Clear();
                    currentCell.IsCellVisited = true;
                }
                else
                {
                    if (pointsToReturn.Count > 0)
                    {
                        currentCell = pointsToReturn[pointsToReturn.Count - 1];
                        pointsToReturn.RemoveAt(pointsToReturn.Count - 1);
                        
                        while (way[way.Count-1]!= currentCell)
                        {
                            way.RemoveAt(way.Count - 1);
                            if (way.Count == 0) break;
                        }

                    }
                    else
                    {
                        Debug.Log("No points to return");
                    }
                }

                done = (currentCell == to);
                

            }
            
            return way;
        }

        private void CalcMazeCellTypes()
        {
            int cnt = 0;
            foreach(MazeCell cell in cells)
            {
                if (cell.BotWall) cnt++;
                if (cell.TopWall) cnt++;
                if (cell.LeftWall) cnt++;
                if (cell.RightWall) cnt++;

                if(cnt==1)
                {
                    if (!cell.IsCellRoomCell) crossRoads.Add(cell);
                }
                if (cnt == 2)
                {
                    if (!cell.IsCellRoomCell) tunnelRoads.Add(cell);
                }
                if (cnt >=3)
                {                    
                    if (!cell.IsCellRoomCell) impasses.Add(cell);
                }

                cnt = 0;

            }
        }
        /// <summary>
        /// Function will delete errors in way
        /// </summary>
        /// <param name="way"></param>
        private void SortWay(List<MazeCell> way)
        {
            MazeCell cur=null, next=null;
            int i = 0;
            while(i< way.Count)
            {
                cur = way[i];
                if (i != (way.Count - 1))
                {
                    next = way[i + 1];
                    if (!cur.GetNeighbors().ContainsValue(next))
                    {
                        way.RemoveAt(i + 1);
                    }
                    else
                    {
                        i++;
                    }
                }
                else i++;
            }
        }
        private void SetAllCellsNotVisited()
        {
            foreach(MazeCell c in cells)
            {
                c.IsCellVisited = false;
            }
        }
        
        private WALL_TYPES CheckWallByPos(Vector2 pos)
        {
            WALL_TYPES type = WALL_TYPES.NONE;
            bool wallTop = false, wallBot = false, wallLeft = false, wallRight = false;

            Vector2 topPos = new Vector2(pos.x, pos.y + wallWidth);
            Vector2 botPos = new Vector2(pos.x, pos.y - wallWidth);
            Vector2 leftPos = new Vector2(pos.x - wallWidth, pos.y);
            Vector2 rightPos = new Vector2(pos.x + wallWidth, pos.y);

            wallTop = IsThereWall(topPos);
            wallBot = IsThereWall(botPos);
            wallLeft = IsThereWall(leftPos);
            wallRight = IsThereWall(rightPos);

            if (wallTop && wallBot && wallLeft && wallRight) type = WALL_TYPES.CROSSROAD;
            if (!wallTop && wallBot && wallLeft && wallRight) type = WALL_TYPES.TEE_BOT;
            if (wallTop && !wallBot && wallLeft && wallRight) type = WALL_TYPES.TEE_TOP;
            if (wallTop && wallBot && !wallLeft && wallRight) type = WALL_TYPES.TEE_RIGHT;
            if (wallTop && wallBot && wallLeft && !wallRight) type = WALL_TYPES.TEE_LEFT;
            if (!wallTop && !wallBot && wallLeft && wallRight) type = WALL_TYPES.HORIZONTAL;
            if (wallTop && wallBot && !wallLeft && !wallRight) type = WALL_TYPES.VERTICAL;
            if (!wallTop && !wallBot && wallLeft && !wallRight) type = WALL_TYPES.HORIZONTAL_IMPASS_RIGHT;
            if (!wallTop && !wallBot && !wallLeft && wallRight) type = WALL_TYPES.HORIZONTAL_IMPASS_LEFT;
            if (wallTop && !wallBot && !wallLeft && !wallRight) type = WALL_TYPES.VERTICAL_IMPASS_BOT;
            if (!wallTop && wallBot && !wallLeft && !wallRight) type = WALL_TYPES.VERTICAL_IMPASS_TOP;

            if (!wallTop && wallBot && !wallLeft && wallRight) type = WALL_TYPES.CORNER_TOP_LEFT;
            if (wallTop && !wallBot && !wallLeft && wallRight) type = WALL_TYPES.CORNER_BOT_LEFT;
            if (!wallTop && wallBot && wallLeft && !wallRight) type = WALL_TYPES.CORNER_TOP_RIGHT;
            if (wallTop && !wallBot && wallLeft && !wallRight) type = WALL_TYPES.CORNER_BOT_RIGHT;
            
            return type;
        }
        private void CaclWallTypes()
        {
            foreach(MazeWall w in walls)
            {
                w.Type = CheckWallByPos(w.Position);
            }
        }
        private void CreateWall(MazeCell c, Vector2 pos)
        {                 
            
            if (!IsThereWall(pos))
            {
                MazeWall wall = new MazeWall(pos);
                wallPosArray.Add(pos);
                walls.Add(wall);

                if (c == endCell)
                {
                    endWallPos.Add(pos);
                    wall.isExit = true;
                }
                if (c == startCell)
                {
                    startWallPos.Add(pos);
                    wall.isStart = true;
                }
            }
            
        }
        private void WriteWallPositions(MazeCell c)
        {
            Vector2 wallStartPos = new Vector2(c.Position.x - cellStep / 2, c.Position.y - cellStep / 2);
            if (c.TopWall)
            {
                for (int i = 0; i < wallCountPerCell; i++)
                {
                    Vector2 pos = new Vector2(wallStartPos.x + i * wallWidth, wallStartPos.y + wallWidth + mazeTunnelWidth);
                    CreateWall(c, pos);
                }
            }

            if (c.BotWall)
            {
                for (int i = 0; i < wallCountPerCell; i++)
                {
                    Vector2 pos = new Vector2(wallStartPos.x + i * wallWidth, wallStartPos.y);
                    CreateWall(c, pos);
                }
            }
            if (c.LeftWall)
            {
                for (int i = 0; i < wallCountPerCell; i++)
                {
                    Vector2 pos = new Vector2(wallStartPos.x, wallStartPos.y + i * wallWidth);
                    CreateWall(c, pos);
                }
            }
            if (c.RightWall)
            {
                for (int i = 0; i < wallCountPerCell; i++)
                {
                    Vector2 pos = new Vector2(wallStartPos.x + +wallWidth + mazeTunnelWidth, wallStartPos.y + i * wallWidth);
                    CreateWall(c, pos);
                }
            }
        }
        private bool IsAllCellsIsVisited()
        {
            foreach (MazeCell c in cells)
            {
                if (!c.IsCellVisited) return false;
            }
            return true;
        }
        /// <summary>
        /// Removes wall between two cells c1 and c2
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        private SIDE RemoveWallBetweenCells(MazeCell c1, MazeCell c2)
        {
            SIDE side = SIDE.BOT;
            int dx, dy;
            dx = c1.GridX - c2.GridX;
            dy = c1.GridY - c2.GridY;
            if (dx == 0)
            {
                if (dy == 1)
                {
                    side = SIDE.BOT;
                    c1.BotWall = false;
                    c2.TopWall = false;
                }
                else if (dy == -1)
                {
                    side = SIDE.TOP;
                    c1.TopWall = false;
                    c2.BotWall = false;
                }

            }
            else
            {
                if (dx == 1)
                {
                    side = SIDE.LEFT;
                    c1.LeftWall = false;
                    c2.RightWall = false;
                }
                else if (dx == -1)
                {
                    side = SIDE.RIGHT;
                    c1.RightWall = false;
                    c2.LeftWall = false;
                }

            }
            return side;
        }

        private bool GetDontVisitedNeighbors(MazeCell c, out List<MazeCell> unvisNeighbors)
        {
            Dictionary<SIDE, MazeCell> neighbors = c.GetNeighbors();
            
            unvisNeighbors = new List<MazeCell>();
            foreach (SIDE key in neighbors.Keys)
            {
                if (!neighbors[key].IsCellVisited)
                {
                    unvisNeighbors.Add(neighbors[key]);
                    
                }
            }
            return unvisNeighbors.Count > 0;
        }

        private bool FindCellByGridPos(int x, int y, out MazeCell c)
        {
            c = null;
            foreach (MazeCell cell in cells)
            {
                if ((cell.GridX == x) && (cell.GridY == y))
                {
                    c = cell;
                    return true;
                }
            }
            return false;
        }

        private void GenerateRooms()
        {
            List<MazeCell> place = null;
            for (int i=0;i<roomParams.roomCount;i++)
            {
                if(FindPalaceForRoom(roomParams.roomWidth, roomParams.roomHeight,out place))
                {
                    new Rect(leftBotPos.x - mazeTunnelWidth / 2 - wallWidth, leftBotPos.y - mazeTunnelWidth / 2 - wallWidth, mazeWorldWidth, mazeWorldHeight);
                    Rect r = new Rect(  place[0].Position.x - mazeTunnelWidth / 2 - wallWidth, 
                                        place[0].Position.y - mazeTunnelWidth / 2 - wallWidth,
                                        roomParams.roomWidth * mazeTunnelWidth + (roomParams.roomWidth + 1) * wallWidth, 
                                        roomParams.roomHeight * mazeTunnelWidth + (roomParams.roomHeight + 1) * wallWidth);
                    MazeRoom room = new MazeRoom(roomParams.roomWidth, roomParams.roomHeight, place[0].Position, r, place,i);
                    CreateRoomEnter(room, place);
                    rooms.Add(room);
                    RemoveWallsInsideArea(place);
                }
            }
            Debug.Log(rooms.Count);
        }
        
        private void RemoveWallsToNearestCellsInArea(List<MazeCell> area, MazeCell cell)
        {

            MazeCell tmp;
            Vector2 pos = new Vector2(cell.Position.x + cellStep, cell.Position.y);
            Dictionary<SIDE, MazeCell> neigbors = cell.GetNeighbors();
            foreach (SIDE key in neigbors.Keys)
            {
                tmp = neigbors[key];
                if(area.Contains(tmp))
                {
                    RemoveWallBetweenCells(cell, tmp);
                }
            }            

        }
        private void RemoveWallsInsideArea(List<MazeCell> area)
        {
            
            for (int i = 0; i < area.Count; i++)
            {                
                RemoveWallsToNearestCellsInArea(area, area[i]);
            }
        }
        private void CreateRoomEnter(MazeRoom r, List<MazeCell> place)
        {
            bool ok = false;            
            List<MazeCell> neig;
            MazeCell roomTmp = null;
            MazeCell tmp = null;
            var rnd = new System.Random(mazeSeed);
            int id = 0;

            while (!ok)
            {
                if (useSeed) id = rnd.Next(0, place.Count);
                else id = UnityEngine.Random.Range(0, place.Count);

                roomTmp = place[id];

                if (GetDontVisitedNeighbors(roomTmp, out neig))
                {
                    if (neig.Count > 1)
                    {
                        if (useSeed) id = rnd.Next(0, neig.Count);
                        else id = UnityEngine.Random.Range(0, neig.Count);
                        tmp = neig[id];
                    }
                    else
                    {
                        tmp = neig[0];
                    }
                    if (tmp != null)
                    {
                        ok = true;
                        SIDE pos = RemoveWallBetweenCells(roomTmp, tmp);
                        Vector2 interPos = Vector2.zero;
                        Vector2 wallStartPos = new Vector2(roomTmp.Position.x - cellStep / 2, roomTmp.Position.y - cellStep / 2);
                        for (int j = 0; j < wallCountPerCell; j++)
                        {
                            switch (pos)
                            {
                                case SIDE.BOT:
                                    interPos = new Vector2(wallStartPos.x+j*wallWidth, wallStartPos.y);
                                    break;
                                case SIDE.TOP:
                                    interPos = new Vector2(wallStartPos.x + j * wallWidth, wallStartPos.y +wallWidth + mazeTunnelWidth);
                                    break;
                                case SIDE.LEFT:
                                    interPos = new Vector2(wallStartPos.x, wallStartPos.y + j * wallWidth);
                                    break;
                                case SIDE.RIGHT:
                                    interPos = new Vector2(wallStartPos.x + wallWidth + mazeTunnelWidth, wallStartPos.y + j * wallWidth);
                                    break;
                            }
                            r.roomEntrances.Add(interPos);
                        }
                        switch (pos)
                        {
                            case SIDE.BOT:
                                interPos = new Vector2(roomTmp.Position.x, wallStartPos.y);
                                break;
                            case SIDE.TOP:
                                interPos = new Vector2(roomTmp.Position.x, wallStartPos.y + wallWidth + mazeTunnelWidth);
                                break;
                            case SIDE.LEFT:
                                interPos = new Vector2(wallStartPos.x, roomTmp.Position.y);
                                break;
                            case SIDE.RIGHT:
                                interPos = new Vector2(wallStartPos.x +wallWidth + mazeTunnelWidth, roomTmp.Position.y);
                                break;
                        }
                        r.roomInterCenter = interPos;
                    }
                }
            }
        }
        private bool FindPalaceForRoom(int w, int h, out List<MazeCell> place)
        {
            place = new List<MazeCell>();
            bool toRet = false;
            bool ok = false;
            MazeCell tmp = null;
            Rect tempRect = new Rect();            
            Vector2 p1 = new Vector2(), p2 = new Vector2(), p3 = new Vector2(), p4 = new Vector2();
            int iterCount = 0;
            var rnd = new System.Random(mazeSeed);
            int cellID=0;
            while (!ok)
            {
                iterCount++;
                if (iterCount > 10000) return false;

                if (useSeed) cellID = rnd.Next(0, cells.Count);
                else cellID = UnityEngine.Random.Range(0, cells.Count);
                
                tmp = cells[cellID];
                if (tmp.Position != startCell.Position)
                {
                    tempRect.Set(tmp.Position.x, tmp.Position.y, (w - 1) * cellStep, (h - 1) * cellStep);

                    p1.Set(tempRect.x, tempRect.y);
                    p2.Set(tempRect.x + tempRect.width, tempRect.y);
                    p3.Set(tempRect.x + tempRect.width, tempRect.y + tempRect.height);
                    p4.Set(tempRect.x, tempRect.y + tempRect.height);

                    if (mazeRect.Contains(p1) && mazeRect.Contains(p2) && mazeRect.Contains(p3) && mazeRect.Contains(p4))
                    {
                        int containsCount = 0;
                        for (int i = 0; i < rooms.Count; i++)
                        {
                            if (rooms[i].RoomRect.Contains(p1) && rooms[i].RoomRect.Contains(p2) && rooms[i].RoomRect.Contains(p3) && rooms[i].RoomRect.Contains(p4))
                            {
                                containsCount++;
                            }
                        }
                        if (containsCount == 0)
                        {
                            ok = true;
                            MazeCell c = null;
                            for (int i = tmp.GridX; i < (tmp.GridX+w); i++)
                            {
                                for (int j = tmp.GridY; j < (tmp.GridY+h); j++)
                                {
                                    if (FindCellByGridPos(i, j, out c))
                                    {
                                        c.IsCellVisited = true;
                                        c.IsCellRoomCell = true;                                        
                                        place.Add(c);
                                    }
                                }
                            }
                            return true;
                        }
                    }
                }
            }

            return toRet;
        }
        #endregion
    }
    
    /// <summary>
    /// Class, which describes a cell in MazeSkeleton
    /// </summary>
    public class MazeCell
    {
        private bool left;
        private bool right;
        private bool top;
        private bool bot;
        private bool isBorder;
        private bool isVisited, isCellRoomCell;
        private Vector2 pos;
        private int gridX, gridY;
        private int id;
        private Dictionary<SIDE, MazeCell> cellNeighbors = new Dictionary<SIDE, MazeCell>();
        /// <summary>
        /// Creates Maze cell with grid position and world position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public MazeCell(Vector2 point, int index = 0, int x = 0, int y = 0)
        {
            left = true;
            right = true;
            top = true;
            bot = true;
            isBorder = false;
            isVisited = false;
            isCellRoomCell = false;
            pos = point;
            gridX = x;
            gridY = y;
            id = index;
        }
        public MazeCell()
        {
            left = true;
            right = true;
            top = true;
            bot = true;
            isBorder = false;
            isVisited = false;
            isCellRoomCell = false;
        }
        public void ClearNeighbors()
        {
            cellNeighbors.Clear();
        }
        public void AddNeighbor(SIDE s, MazeCell c)
        {
            if(!cellNeighbors.ContainsKey(s)) cellNeighbors.Add(s, c);
        }
        public Dictionary<SIDE, MazeCell> GetNeighbors()
        {
            return cellNeighbors;
        }
        #region Public Variables
        public bool IsCellRoomCell
        {
            get { return isCellRoomCell; }
            set { isCellRoomCell = value; }
        }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }
        /// <summary>
        /// Grid position on horizontal
        /// </summary>
        public int GridX
        {
            get { return gridX; }
            set { gridX = value; }
        }
        /// <summary>
        /// Grid position on vertical
        /// </summary>
        public int GridY
        {
            get { return gridY; }
            set { gridY = value; }
        }
        /// <summary>
        /// If true, cell contains Left wall
        /// </summary>
        public bool LeftWall
        {
            get { return left; }
            set { left = value; }
        }
        /// <summary>
        /// If true, cell contains Right wall
        /// </summary>
        public bool RightWall
        {
            get { return right; }
            set { right = value; }
        }
        /// <summary>
        /// If true, cell contains Top wall
        /// </summary>
        public bool TopWall
        {
            get { return top; }
            set { top = value; }
        }
        /// <summary>
        /// If true, cell contains Bottom wall
        /// </summary>
        public bool BotWall
        {
            get { return bot; }
            set { bot = value; }
        }

        public bool IsCellBorder
        {
            get { return isBorder; }
            set { isBorder = value; }
        }

        public bool IsCellVisited
        {
            get { return isVisited; }
            set { isVisited = value; }
        }

        #endregion
    }
    /// <summary>
    /// Class, which describes a wall in the MazeSkeleton
    /// </summary>
    public class MazeWall
    {

        public bool isStart = false;
        public bool isExit = false;
        private Vector2 position;
        private Vector3 rotationToCell;
        private WALL_TYPES type;
        private SIDE side;
        public MazeWall(Vector2 pos)
        {
            position = pos;
        }

        #region Public variables
        public Vector2 Position
        {
            get { return position; }
            set { if (value != null) position = value; }
        }
        public WALL_TYPES Type
        {
            get { return type; }
            set { type = value; }
        }
        #endregion
    }
    /// <summary>
    /// Decribes room in MazeSkeleton
    /// </summary>
    public class MazeRoom
    {
        /// <summary>
        /// Left bottom position of room
        /// </summary>
        public Vector3 leftBotPos;
        /// <summary>
        /// Cells in rooms
        /// </summary>
        public List<MazeCell> roomCells;
        /// <summary>
        /// Center of the room entrance
        /// </summary>
        public Vector2 roomInterCenter;
        /// <summary>
        /// List of all wall positions in room entrance
        /// </summary>
        public List<Vector2> roomEntrances = new List<Vector2>();
        /// <summary>
        /// Room size in cells count
        /// </summary>
        public int width, height;
        private int id;
        private Rect roomRect;
        private float cellW = 0;
        /// <summary>
        /// Width/Height in cells count, pos - left bottom cell in room position, rect - room rect, room cells
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="pos"></param>
        /// <param name="r"></param>
        /// <param name="cells"></param>
        public MazeRoom(int w, int h, Vector2 pos, Rect r, List<MazeCell> cells, int index)
        {
            width = w;
            height = h;
            leftBotPos = pos;
            roomCells = cells;
            roomRect = r;
            id = index;
        }

        public void AddRoomEnter(Vector2 pos)
        {
            roomEntrances.Add(pos);
        }

        #region public variables
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// Room rect
        /// </summary>
        public Rect RoomRect
        {
            get { return roomRect; }
            set { roomRect = value; }
        }
        #endregion

        #region private functions
        
        #endregion
    }
}