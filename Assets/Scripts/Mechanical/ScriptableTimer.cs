using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableTimer : MonoBehaviour
{
    public void DoStartCoroutine(IEnumerator coroutine) => StartCoroutine(coroutine);

}
