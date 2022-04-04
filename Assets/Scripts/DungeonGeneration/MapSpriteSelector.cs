using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpriteSelector : MonoBehaviour 
{	
	[SerializeField] private Vector2 dimensions = Vector2.zero;
	[SerializeField] private Transform minimap = null;
	[SerializeField] private Transform rootGrid;

	[Header("Room")]
	public GameObject spU;
	public GameObject spD, spR, spL,
			spUD, spRL, spUR, spUL, spDR, spDL,
			spULD, spRUL, spDRU, spLDR, spUDRL;

	[Header("Minimap")]
	public GameObject mpU;
	public GameObject mpD, mpR, mpL,
			mpUD, mpRL, mpUR, mpUL, mpDR, mpDL,
			mpULD, mpRUL, mpDRU, mpLDR, mpUDRL;

	
	[System.NonSerialized] public bool up = false, down = false, left = false, right = false;
	[System.NonSerialized] public int type = -1; // 0: Enter, -2: Boss; Others: distance from entrance
	
	private void Start () 
	{
		if(type == 0) down = true;
		PickSprite();
		Destroy(gameObject);	
	}

	private void PickSprite()
	{ 
		if (up)
		{
			if (down)
			{
				if (right)
				{
					if (left) InstantiateRooms(spUDRL, mpUDRL);
					else InstantiateRooms(spDRU, mpDRU);
				}
				else if (left) InstantiateRooms(spULD, mpULD);
				else InstantiateRooms(spUD, mpUD);	
			}
			else
			{
				if (right)
				{
					if (left)InstantiateRooms(spRUL, mpRUL);
					else InstantiateRooms(spUR, mpUR);
				}
				else if (left) InstantiateRooms(spUL, mpUL);
				else InstantiateRooms(spU, mpU);
			}
		}

		else if (down)
		{
			if (right)
			{
				if(left) InstantiateRooms(spLDR, mpLDR);
				else InstantiateRooms(spDR, mpDR);
			}
			else if (left) InstantiateRooms(spDL, mpDL);
			else InstantiateRooms(spD, mpD);
		}

		else if (right)
		{
			if (left) InstantiateRooms(spRL, mpRL);
			else InstantiateRooms(spR, mpR);
		}

		else if (left) InstantiateRooms(spL, mpL);
		
	}

	private void InstantiateRooms(GameObject sprite, GameObject minimap_sprite)
	{
		Vector2 roomPos = transform.position * dimensions;
		GameObject room = Instantiate(sprite, roomPos, Quaternion.identity);
		room.GetComponent<RoomController>().type = type;
		if(rootGrid != null) room.transform.parent = rootGrid;	
		Instantiate(minimap_sprite, transform.position, Quaternion.identity).transform.SetParent(minimap);
	}

}
