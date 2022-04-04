using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable] public class Cutscenes
{
    public List<string> conditions;
    public PlayableAsset animation;
}

public class TimelineManager : MonoBehaviour
{
    private PlayableDirector director;
    [SerializeField] private List<Cutscenes> cutscenes;
    public static Dictionary<string,bool> animaion_flags = new Dictionary<string,bool>
    {
        {"Dead", false},
    };

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        foreach( Cutscenes cut in cutscenes)
        {
            if(cut.conditions.TrueForAll(c => animaion_flags[c]))
            {
                foreach(string conditions in cut.conditions) animaion_flags[conditions] = false;
                director.playableAsset = cut.animation;
                director.Play();
            }
        }
    }

}
