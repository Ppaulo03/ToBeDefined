using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BountyList", menuName = "ScriptableObject/BountyList", order = 0)]
public class BountyList : ScriptableObject
{
    public List<Bounty> bounties;
}
