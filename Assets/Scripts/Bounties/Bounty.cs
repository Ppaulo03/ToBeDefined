using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BountyStatus
{
        Inativa,
        Ativa,
        Spawned,
        AguardandoClaim,
        Completa,
}

[CreateAssetMenu(fileName = "Bounty", menuName = "ScriptableObject/Bounty", order = 0)]
public class Bounty : ScriptableObject
{
    public BountyStatus status = BountyStatus.Inativa;
    public string title = "";
    public string descricao = "";
    public Sprite image = null;
    public GameObject target = null;
}
