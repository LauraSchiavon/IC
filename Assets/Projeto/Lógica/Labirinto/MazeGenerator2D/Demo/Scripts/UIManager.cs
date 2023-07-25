using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MazeSkeleton;

public class UIManager : MonoBehaviour
{
    public GameObject indicator;
    public GameObject greenIndicator;
    public GameObject blueIndicator;
    public GameObject pinkIndicator;
    public GameObject blackIndicator;
    public MazeDrawer2D MazeDrawer;

    private List<GameObject> indicators= new List<GameObject>();
    void Start()
    {
        
    }
    private void DrawIndicators(List<MazeCell> list, GameObject pref)
    {
        foreach (MazeCell c in list)
        {
            GameObject ind = Instantiate(pref, c.Position, Quaternion.identity) as GameObject;
            indicators.Add(ind);
        }
    }
    private void DrawIndicators(List<Vector2> list, GameObject pref)
    {
        foreach (Vector2 c in list)
        {
            GameObject ind = Instantiate(pref, c, Quaternion.identity) as GameObject;
            indicators.Add(ind);
        }
    }
    public void ShowMazeGrid()
    {
        if(MazeDrawer)
        {
            List<MazeCell> grid = MazeDrawer.GetMazeCellsList();
            DrawIndicators(grid,indicator);
        }
    }

    public void ShowMazeTunnel()
    {
        if (MazeDrawer)
        {
            List<MazeCell> tunnels = MazeDrawer.GetTunnels();
            DrawIndicators(tunnels, blueIndicator);
        }
    }

    public void ShowMazeImpasses()
    {
        if (MazeDrawer)
        {
            List<MazeCell> imp = MazeDrawer.GetImpassess();
            DrawIndicators(imp, pinkIndicator);
        }
    }

    public void ShowMazeCrossRoads()
    {
        if (MazeDrawer)
        {
            List<MazeCell> cross = MazeDrawer.GetCrossRoads();
            DrawIndicators(cross, blackIndicator);
        }
    }

    public void ShowMazeRooms()
    {
        if (MazeDrawer)
        {
            List<MazeRoom> rooms = MazeDrawer.GetMazeRooms();
            foreach(MazeRoom r in rooms)
            {
                DrawIndicators(r.roomCells, indicator);
            }
        }
    }

    public void ShowMazeRoomEnter()
    {
        if (MazeDrawer)
        {
            List<MazeRoom> rooms = MazeDrawer.GetMazeRooms();
            foreach (MazeRoom r in rooms)
            {
                DrawIndicators(r.roomEntrances, greenIndicator);
            }
        }

    }

    public void ShowMazeRandomWay()
    {
        if (MazeDrawer)
        {
            List<MazeCell> imp = MazeDrawer.GetImpassess();
            MazeCell c1 = imp[Random.Range(0, imp.Count)];
            MazeCell c2 = c1;
            while (c2==c1)
            {
                c2 = imp[Random.Range(0, imp.Count)];
            }

            List<MazeCell> way = MazeDrawer.FindWayFromTo(c1, c2);
            DrawIndicators(way, indicator);

        }
    }
    public void ShowMazeLongWay()
    {
        if (MazeDrawer)
        {
            List<MazeCell> way = MazeDrawer.GetMazeLongWayCells();
            DrawIndicators(way, indicator);
        }
    }

    public void ShowMazeWayFromStartToExit()
    {
        if (MazeDrawer)
        {
            List<MazeCell> way = MazeDrawer.FindWayFromStartToExit();
            DrawIndicators(way, indicator);
        }
    }
    public void ShowStartExit()
    {
        if (MazeDrawer)
        {
            List<MazeCell> inds = new List<MazeCell>();
            
            GameObject ind = Instantiate(greenIndicator, MazeDrawer.GetStartCell().Position, Quaternion.identity) as GameObject;
            indicators.Add(ind);

            GameObject ind2 = Instantiate(indicator, MazeDrawer.GetExitCell().Position, Quaternion.identity) as GameObject;
            indicators.Add(ind);
        }
    }
    public void GenerateNew()
    {
        if (MazeDrawer)
        {
            MazeDrawer.mazeWidth = 15;
            MazeDrawer.mazeHeigh = 15;
            MazeDrawer.GenerateNewMaze();
        }
    }
    public void GenerateMazeWithRandomStart()
    {
        if (MazeDrawer)
        {            
            MazeDrawer.GenerateMazeWithRandomStart();
        }
    }
    public void DeleteMaze()
    {
        if (MazeDrawer)
        {
            MazeDrawer.DeleteMaze();
        }
    }

    public void DeleteIndicators()
    {
        foreach(GameObject obj in indicators)
        {
            Destroy(obj);
        }
        indicators.Clear();
    }
}
