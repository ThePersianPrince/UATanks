using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapSeed;
    public int rows;
    public int columns;
    public GameObject[] gridPrefabs;
    public bool isMapOfTheDay;
    public bool isRandomMap;

    private readonly float roomWidth = 50.0f;
    private readonly float roomHeight = 50.0f;
    private Room[,] grid;
    private readonly string mapTypeKey = "MapType";

    // Use this for initialization
    void Start ()
    {
        if (PlayerPrefs.HasKey(mapTypeKey))
        {
            if (PlayerPrefs.GetString(mapTypeKey) == "Random Map")
            {
                isMapOfTheDay = false;
                isRandomMap = true;
            }
            else if (PlayerPrefs.GetString(mapTypeKey) == "Map Of The Day")
            {
                isMapOfTheDay = true;
                isRandomMap = false;
            }
        }

        if (isMapOfTheDay)
        {
            mapSeed = DateToInt(DateTime.Now.Date);
        }
        else if (isRandomMap)
        {
            mapSeed = DateToInt(DateTime.Now);
        }
        else if (mapSeed == 0)
        {
            mapSeed = DateToInt(DateTime.Now);
        }

        
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        

        UnityEngine.Random.InitState(mapSeed);

        
        grid = new Room[columns, rows];

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                float xPosition = roomWidth * column;
                float zPosition = roomHeight * row;
                Vector3 newPosition = new Vector3(xPosition, 0.0f, zPosition);

                GameObject tempRoomObj = Instantiate(RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject;

                tempRoomObj.transform.parent = this.transform;

                tempRoomObj.name = "Room_" + column + "," + row;

                Room tempRoom = tempRoomObj.GetComponent<Room>();

                if(row == 0)
                {
                    tempRoom.doorNorth.SetActive(false);
                }
                else if (row == rows - 1)
                {
                    tempRoom.doorSouth.SetActive(false);
                }
                else
                {
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }

                if (column == 0)
                {
                    tempRoom.doorEast.SetActive(false);
                }
                else if (column == columns - 1)
                {
                    tempRoom.doorWest.SetActive(false);
                }
                else
                {
                    tempRoom.doorEast.SetActive(false);
                    tempRoom.doorWest.SetActive(false);
                }

                grid[column, row] = tempRoom;
            }
        }
    }

    public GameObject RandomRoomPrefab()
    {
        return gridPrefabs[UnityEngine.Random.Range(0, gridPrefabs.Length)];
    }

    public int DateToInt(DateTime dateToUse)
    {
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }
}
