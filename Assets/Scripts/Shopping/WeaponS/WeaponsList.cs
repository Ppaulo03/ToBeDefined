using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponsList", menuName = "ScriptableObject/WeaponsList", order = 0)]
public class WeaponsList : ScriptableObject
{
    public List<Weaponry> weapons;
}