using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsList", menuName = "ScriptableObject/ItemsList", order = 0)]
public class ItemsList : ScriptableObject
{
    public List<Item> items;
}
