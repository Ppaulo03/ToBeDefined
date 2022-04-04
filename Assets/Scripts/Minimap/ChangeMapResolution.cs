using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMapResolution : MonoBehaviour
{
    [SerializeField] private float original_resolution = -3f;
    [SerializeField] private float  max_resolution = -3f;
    [SerializeField] private float min_resolution = -10f;
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float wheelSpeed = 1f;

    [SerializeField] private Vector2 maxPos = Vector2.zero;
    [SerializeField] private Vector2 minPos = Vector2.zero;
    [SerializeField] private float moveSpeed = 0.01f;
    [SerializeField] private float dragSpeed = 0.01f;
    private Vector3 dragOrigin;

    private float resolution;
    private bool big_map = false;

    void Start()
    {
        resolution = (max_resolution + min_resolution)/2f;
        transform.position = new Vector3(transform.position.x, transform.position.y, original_resolution);
    }

    private void Update() 
    {
        if(big_map)
        {
            if(Input.GetKey("=")) ZoomIn(zoomSpeed);
            else if(Input.GetKey("-")) ZoomOut(zoomSpeed);
            else if (Input.GetAxis("Mouse ScrollWheel") != 0) Zoom(Input.GetAxis("Mouse ScrollWheel")*wheelSpeed);

            if (Input.GetMouseButtonDown(0)) dragOrigin = Input.mousePosition;
            else if (!Input.GetMouseButton(0)) Move();
            else Drag();
        } 
    }

    public void ChangeMap()
    {
        if(big_map) transform.localPosition = new Vector3(0, 0, original_resolution);
        else transform.localPosition = new Vector3(0, 0, resolution);
        big_map = !big_map;
    }

    private void Zoom(float i)
    {
        if(i<0) ZoomOut(-i);
        else ZoomIn(i);
    }

    private void ZoomIn(float i)
    { 
        resolution += i;
        if(resolution >= max_resolution) resolution = max_resolution;
        transform.position = new Vector3(transform.position.x, transform.position.y, resolution);
    }
    private void ZoomOut(float i)
    {
        resolution -= 1;
        if(resolution <= min_resolution) resolution = min_resolution;
        transform.position = new Vector3(transform.position.x, transform.position.y, resolution);
    }

    private void Move()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        direction = direction.normalized;

        transform.Translate(direction*moveSpeed);
        CheckPos();
    }

    private void Drag()
    {
        Vector3 mouse_pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        dragOrigin = Input.mousePosition;
        Vector2 direction = new Vector2(mouse_pos.x, mouse_pos.y);

        transform.Translate(direction*-dragSpeed);
        CheckPos();
    }

    private void CheckPos()
    {
        if(transform.position.x > maxPos.x) transform.position = new Vector3(maxPos.x, transform.position.x, resolution);
        else if(transform.position.x < minPos.x) transform.position = new Vector3(minPos.x, transform.position.x, resolution);
        
        if(transform.position.y > maxPos.y) transform.position = new Vector3(transform.position.x, maxPos.y, resolution);
        else if(transform.position.y < minPos.y) transform.position = new Vector3(transform.position.x, minPos.y, resolution);
    }
}
