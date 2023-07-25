using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MazeDrawer2D))]
[CanEditMultipleObjects]
public class MazeDrawer2DEditorView : Editor
{
    MazeDrawer2D subject;
    SerializedProperty wall, wallCross, wallImp, wallTee, wallCorner, Mwidth, Mheight, TunnelWidth, wallPerCellSide, pos;
    SerializedProperty mazeSeed, useSeed, roomCount, roomWidth, roomHeight, genRooms, scaleWall, useCustomWalls, useStartWall;
    SerializedProperty useEndWall, createOnStart, startWallPrefab, exitWallPrefab, useLongWay;
    private Rect mainParameters = new Rect();
    private Rect roomParameters = new Rect();
    private Rect seedParameters = new Rect();
    private Rect startEndParameters = new Rect();
    void OnEnable()
    {
        subject = target as MazeDrawer2D;
        
        wall = serializedObject.FindProperty("wallPrefab");
        wallCross = serializedObject.FindProperty("crossWallPrefab");
        wallImp = serializedObject.FindProperty("teeWallPrefab");
        wallTee = serializedObject.FindProperty("impWallPrefab");
        wallCorner = serializedObject.FindProperty("cornerWallPrefab");

        Mwidth = serializedObject.FindProperty("mazeWidth");
        Mheight = serializedObject.FindProperty("mazeHeigh");
        TunnelWidth = serializedObject.FindProperty("tunnelWigth");
        wallPerCellSide = serializedObject.FindProperty("wallPerCellSide");
        pos = serializedObject.FindProperty("mazePosition");

        mazeSeed = serializedObject.FindProperty("mazeSeed");
        useSeed = serializedObject.FindProperty("useSeed");
        roomCount = serializedObject.FindProperty("roomCount");
        roomWidth = serializedObject.FindProperty("roomWidth");
        roomHeight = serializedObject.FindProperty("roomHeight");
        genRooms = serializedObject.FindProperty("genRooms");
        scaleWall = serializedObject.FindProperty("scaleWall");
        useCustomWalls = serializedObject.FindProperty("useCustomWalls");
        useStartWall = serializedObject.FindProperty("useStartWall");

        useEndWall = serializedObject.FindProperty("useEndtWall");
        createOnStart = serializedObject.FindProperty("createOnStart");
        startWallPrefab = serializedObject.FindProperty("startWallPrefab");
        exitWallPrefab = serializedObject.FindProperty("exitWallPrefab");
        useLongWay = serializedObject.FindProperty("useLongWay");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        

        EditorGUILayout.BeginVertical();

        mainParameters = EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(pos, new GUIContent("Maze Position", "Left Bottom Maze position in scene"));

        EditorGUILayout.PropertyField(Mwidth, new GUIContent("Maze Width", "Maze width in MazeCells count"));
        EditorGUILayout.PropertyField(Mheight, new GUIContent("Maze Height", "Maze height in MazeCells count"));
        EditorGUILayout.PropertyField(TunnelWidth, new GUIContent("Tunnel Wigth", "Maze tunnel width(MazeCell width)"));
        EditorGUILayout.PropertyField(wallPerCellSide, new GUIContent("Walls on Cell Side", "Walls count per cell side, set minimum value = 3"));

        EditorGUILayout.PropertyField(useCustomWalls, new GUIContent("Use custom walls", "If checked, will create custom walls"));
        if(subject.useCustomWalls)
        {
            GUILayout.Label("Wall prefabs for maze drawing:");
            EditorGUILayout.PropertyField(wall, new GUIContent("Main wall", "Main wall prefab"));
            EditorGUILayout.PropertyField(wallCross, new GUIContent("Cross wall", "Cross wall prefab"));
            EditorGUILayout.PropertyField(wallImp, new GUIContent("Impasse wall", "Impasse wall prefab"));
            EditorGUILayout.PropertyField(wallTee, new GUIContent("Tee wall", "Tee wall prefab"));
            EditorGUILayout.PropertyField(wallCorner, new GUIContent("Corner wall", "Corner wall prefab"));
        }
        else
        {
            EditorGUILayout.PropertyField(wall, new GUIContent("Wall prefab", "Main wall prefab"));
        }

        EditorGUILayout.PropertyField(scaleWall, new GUIContent("Scale wall", "Drawer will scale each wall object(use it, if wall is rectangle)"));
        EditorGUILayout.PropertyField(createOnStart, new GUIContent("Create on start", "If checked, maze will be generated on start"));
        EditorGUILayout.PropertyField(useLongWay, new GUIContent("Use long way", "If checked, maze will set exit position to end of long way in maze"));

        EditorGUILayout.Space(5);
        EditorGUILayout.EndVertical();
        Vector2 startPos = new Vector2(mainParameters.position.x, mainParameters.position.y+ mainParameters.height);
        Handles.DrawLine(startPos, new Vector2(startPos.x+ mainParameters.width, startPos.y));

        roomParameters = EditorGUILayout.BeginVertical();
        EditorGUILayout.Space(5);
        EditorGUILayout.PropertyField(genRooms, new GUIContent("Generate rooms", "If checked, will generate room with setted parameters"));
        if(subject.genRooms)
        {
            GUILayout.Label("Room parameters:");
            EditorGUILayout.PropertyField(roomCount , new GUIContent("Room count", "Count of the rooms in maze"));
            EditorGUILayout.PropertyField(roomWidth , new GUIContent("Room width", "Room width in cells count"));
            EditorGUILayout.PropertyField(roomHeight, new GUIContent("Room height", "Room height in cells count"));
        }
        EditorGUILayout.Space(5);
        startPos = new Vector2(roomParameters.position.x, roomParameters.position.y + roomParameters.height);
        Handles.DrawLine(startPos, new Vector2(startPos.x + roomParameters.width, startPos.y));

        EditorGUILayout.EndVertical();

        seedParameters = EditorGUILayout.BeginVertical();
        EditorGUILayout.Space(5);
        EditorGUILayout.PropertyField(useSeed, new GUIContent("Use seed", "If checked, maze will use fixed seed for random function"));
        if(subject.useSeed)
        {
            EditorGUILayout.PropertyField(mazeSeed, new GUIContent("Maze seed", "Value of seed for random function"));
        }
        EditorGUILayout.Space(5);
        startPos = new Vector2(seedParameters.position.x, seedParameters.position.y + seedParameters.height);
        Handles.DrawLine(startPos, new Vector2(startPos.x + seedParameters.width, startPos.y));
        EditorGUILayout.EndVertical();


        startEndParameters = EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(useEndWall, new GUIContent("Use exit wall", "If checked, drawer will create exit wall object on exit"));
        if(subject.useEndtWall)
        {
            EditorGUILayout.PropertyField(exitWallPrefab, new GUIContent("Exit wall prefab", "Prefab for exit walls"));
        }

        EditorGUILayout.PropertyField(useStartWall, new GUIContent("Use start wall", "If checked, drawer will create start wall object on exit"));
        if (subject.useStartWall)
        {
            EditorGUILayout.PropertyField(startWallPrefab, new GUIContent("Start wall prefab", "Prefab for start walls"));
        }

        EditorGUILayout.Space(5);
        startPos = new Vector2(startEndParameters.position.x, startEndParameters.position.y + startEndParameters.height);
        Handles.DrawLine(startPos, new Vector2(startPos.x + startEndParameters.width, startPos.y));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();
        
        serializedObject.ApplyModifiedProperties();
    }
}
