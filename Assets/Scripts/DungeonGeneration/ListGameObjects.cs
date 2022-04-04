using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListGameObjects", menuName = "ScriptableObject/ListGameObjects", order = 0)]
public class ListGameObjects : ScriptableObject
{
    public GameObject[] objects;
}
