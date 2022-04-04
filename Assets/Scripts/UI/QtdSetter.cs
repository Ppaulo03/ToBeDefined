using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QtdSetter : MonoBehaviour
{
    [SerializeField] private InventoryUI UI = null;
    [SerializeField] private Inventory inventory = null;

    [SerializeField] private Slider slider = null;
    [SerializeField] private InputField input = null;

    [SerializeField] private Button drop = null;
    private int max_value;
    
    private void OnEnable()
    {
        max_value = inventory.items[UI.selected].qtd;
        slider.maxValue = max_value;
    }

    private void Start() 
    {
        input.onEndEdit.AddListener(delegate {InputValueChangeCheck(); });
        slider.onValueChanged.AddListener(delegate {SliderValueChangeCheck(); });
        drop.onClick.AddListener(delegate {Drop(); });
    }

    public void InputValueChangeCheck()
    {
        int value = int.Parse(input.text);
        if(value > max_value)
        {
            value = max_value;
            input.text = value.ToString();
        }
        slider.value = value;
    }

    public void SliderValueChangeCheck()
    {
        input.text = slider.value.ToString();
    }

    public void Drop()
    {
        UI.Drop((int) slider.value);
    }
    
}
