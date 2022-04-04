using UnityEngine;

public class Dialog : MonoBehaviour
{
    [SerializeField] private DialogValue value = null;
    [SerializeField] private TextAsset text_Asset = null;
    [SerializeField] private Sprite image = null;
    
    public void Chat() => value.Activate(text_Asset, image);
}
