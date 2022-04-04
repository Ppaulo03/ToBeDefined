using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
	[SerializeField] private GameObject spriteSelector;
	[SerializeField] private Vector2 worldSize = new Vector2(4,4);

	[SerializeField] private Vector2 begginingRoom = Vector2.zero;
	[SerializeField] private int numberOfRooms = 20;

	private int gridSizeX, gridSizeY;
	private Room[,] rooms;
	private List<Vector2> takenPositions = new List<Vector2>();
	private int width, height;
	
    private void Start() 
	{
        if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2)) 
			numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
		
		gridSizeX = Mathf.RoundToInt(worldSize.x); 
		gridSizeY = Mathf.RoundToInt(worldSize.y);

		if(begginingRoom.x >= gridSizeX) begginingRoom.x = gridSizeX - 1;
		else if(begginingRoom.x < -gridSizeX ) begginingRoom.x = -gridSizeX;

		if(begginingRoom.y >= gridSizeY) begginingRoom.y = gridSizeY - 1;
		else if(begginingRoom.y < -gridSizeY) begginingRoom.y = -gridSizeY;

		CreateRooms(); 
		SetRoomDoors(); 
		SetLast();
		DrawMap();
		Destroy(gameObject);
    }

    private void CreateRooms()
    {
		//Setup
		rooms = new Room[gridSizeX * 2, gridSizeY * 2];
		rooms[(int) begginingRoom.x + gridSizeX, (int) begginingRoom.y + gridSizeY] = new Room(begginingRoom, 0);
		takenPositions.Insert(0, begginingRoom);
        Vector2 checkPos = Vector2.zero;

        //magic numbers
		float randomCompareStart = 0.2f, randomCompareEnd = 0.01f;

		//add rooms
		for (int i = 0; i < numberOfRooms -1; i++)
		{
			float randomPerc = ((float) i) / (((float)numberOfRooms - 1));
			float randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);

			//grab new position
			checkPos = NewPosition();

			//test new position
			if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
				int iterations = 0;
				do
                {
					checkPos = SelectiveNewPosition();
					iterations++;
				}while(NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
			}

			//finalize position
			rooms[(int) checkPos.x + gridSizeX, (int) checkPos.y + gridSizeY] = new Room(checkPos, getDistance(checkPos));
			takenPositions.Insert(0,checkPos);
		}
    }
	private void SetLast()
	{
		int max_dist = 0, min_num_doors = 0, x_max = 0, y_max = 0;
		for (int x = 0; x < ((gridSizeX * 2)); x++)
		{
			for (int y = 0; y < ((gridSizeY * 2)); y++)
			{
				if (rooms[x,y] == null) continue;
				if (rooms[x,y].type < max_dist) continue;
				else if(rooms[x,y].type > max_dist)
				{
					max_dist = rooms[x,y].type;
					x_max = x; y_max = y;

					min_num_doors = 0;
					if(rooms[x,y].doorBot) min_num_doors ++;
					if(rooms[x,y].doorTop) min_num_doors ++;
					if(rooms[x,y].doorLeft) min_num_doors ++;
					if(rooms[x,y].doorRight) min_num_doors ++;

				}
				else
				{
					int num_doors = 0;
					if(rooms[x,y].doorBot) num_doors ++;
					if(rooms[x,y].doorTop) num_doors ++;
					if(rooms[x,y].doorLeft) num_doors ++;
					if(rooms[x,y].doorRight) num_doors ++;

					if(num_doors <= min_num_doors)
					{
						min_num_doors = num_doors;
						x_max = x; y_max = y;
					}
				}
			}
		}
		rooms[x_max, y_max].type = -2;
	}
	
	private int getDistance(Vector2 pos)
	{
		float distance = Vector2.Distance(pos, begginingRoom);
		return (int) Mathf.Floor(distance);
	}
    
	private Vector2 NewPosition()
	{
		int x = 0, y = 0;
		Vector2 checkingPos = Vector2.zero;

		do
		{
			int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); // pick a random room
			x = (int) takenPositions[index].x;//capture its x, y position
			y = (int) takenPositions[index].y;
			
			bool UpDown = (Random.value < 0.5f); //randomly pick whether to look on hor or vert axis
			bool positive = (Random.value < 0.5f); //randomly pick whether to be positive or negative on that axis

			if (UpDown) //find the position
			{ 
				if (positive) y += 1;
				else y -= 1;
			}else
			{
				if (positive) x += 1;
				else x -= 1;
			}
			checkingPos = new Vector2(x,y);

		}while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
		
		return checkingPos;
	}
	
	private Vector2 SelectiveNewPosition()
	{ 
		int index = 0, inc = 0;
		int x = 0, y = 0;
		Vector2 checkingPos = Vector2.zero;

		do
		{
			inc = 0;
			do
			{ 
				/*instead of getting a room to find an adject empty space, we start with one that only 
				has one neighbor. This will make it more likely that it returns a room that branches out*/
				index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
				inc ++;

			}while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);
			
			x = (int) takenPositions[index].x;
			y = (int) takenPositions[index].y;

			bool UpDown = (Random.value < 0.5f);
			bool positive = (Random.value < 0.5f);

			if (UpDown)
			{
				if (positive) y += 1;
				else y -= 1;
			}else
			{
				if (positive) x += 1;
				else x -= 1;

			}
			
			checkingPos = new Vector2(x,y);

		} while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
		
		if (inc >= 100)  print("Error: could not find position with only one neighbor");
		return checkingPos;
	}

	private int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
	{
		int neighbors = 0; 
		if (usedPositions.Contains(checkingPos + Vector2.right)) neighbors++;
		if (usedPositions.Contains(checkingPos + Vector2.left)) neighbors++;
		if (usedPositions.Contains(checkingPos + Vector2.up)) neighbors++;
		if (usedPositions.Contains(checkingPos + Vector2.down)) neighbors++;
		return neighbors;
	}

	private void SetRoomDoors()
	{
		for (int x = 0; x < ((gridSizeX * 2)); x++)
		{
			for (int y = 0; y < ((gridSizeY * 2)); y++)
			{
				if (rooms[x,y] == null) continue;

				Vector2 gridPosition = new Vector2(x,y);

				if (y - 1 < 0) rooms[x,y].doorBot = false;
				else rooms[x,y].doorBot = (rooms[x,y-1] != null);
				
				if (y + 1 >= gridSizeY * 2)rooms[x,y].doorTop = false;
				else rooms[x,y].doorTop = (rooms[x,y+1] != null);
				
				if (x - 1 < 0) rooms[x,y].doorLeft = false;	
				else rooms[x,y].doorLeft = (rooms[x - 1,y] != null);
				
				if (x + 1 >= gridSizeX * 2) rooms[x,y].doorRight = false;
				else rooms[x,y].doorRight = (rooms[x+1,y] != null);
			}
		}
	}

	private void DrawMap()
	{
		foreach (Room room in rooms)
		{
			if (room == null) continue; 

			MapSpriteSelector mapper = Object.Instantiate(spriteSelector, room.gridPos, Quaternion.identity).
									GetComponent<MapSpriteSelector>();

			mapper.type = room.type;
			mapper.up = room.doorTop;
			mapper.down = room.doorBot;
			mapper.right = room.doorRight;
			mapper.left = room.doorLeft;
		}
	}
}
