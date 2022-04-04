using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class BookUI : BookBase
{

    [SerializeField] private Image img = null;
    [SerializeField] private Text spell_name = null;
    [SerializeField] private Text description = null;
    [SerializeField] private Text strength = null;
    [SerializeField] private Text cost = null;
    [SerializeField] private Text cooldown = null;
    [SerializeField] private Text cast_time = null;

    protected override void SetMaxIndex() => max_index = inventory.magics.Count;

    protected override void SetInfo()
    {
        Magic spell = inventory.magics[index];
        img.sprite = spell.image;
        spell_name.text = spell.spell_name;
        description.text = spell.descricption;

        strength.text = "Strength: " + spell.strength;
        cost.text = "Cost: " + spell.cost;
        cooldown.text = "Cooldown: " + spell.cooldown+ " s";
        cast_time.text = "Cast Time: " + spell.cast_time + " s";

        if (index >= max_index - 1) nxtBtn.SetActive(false);
        if (index <= 0) bkBtn.SetActive(false);
    }

 
    
}
