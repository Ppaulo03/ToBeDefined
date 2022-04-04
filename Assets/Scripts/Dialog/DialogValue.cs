using UnityEngine;

[CreateAssetMenu(fileName = "Magic", menuName = "ScriptableObject/DialogValue", order = 0)]
public class DialogValue : ScriptableObject
{
    public TextAsset text_Asset = null;
    public Sprite image = null;
    [SerializeField] private Signal change = null;

    public void Activate(TextAsset _text_Asset, Sprite _image)
    {
        if(!DialogController.isEnabled)
        {
            text_Asset = _text_Asset;
            image = _image;
            change.Raise();
        }  
    }


}
