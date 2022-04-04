using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopSetValue : MonoBehaviour
{
    [SerializeField] private Slider slider = null;
    [SerializeField] private InputField input = null;
    [SerializeField] private Button shop = null;
    public Text price_text = null;
    [SerializeField] ItemShop itemShop = null;
    [SerializeField] int slot = 0;

    public float percentage = 1f;

    private int max_value = 24;

    public Item merchandise;

    private void Start() 
    {
        input.onEndEdit.AddListener(delegate {InputValueChangeCheck(); });
        slider.onValueChanged.AddListener(delegate {SliderValueChangeCheck(); });
        shop.onClick.AddListener(delegate {BuyOrSell(); });
    }
    
    public void SetMax(int max, Item _merchandise, float new_percentage)
    {
        percentage = new_percentage;

        merchandise = _merchandise;
        price_text = shop.transform.GetChild(0).GetComponent<Text>();
        price_text.text = "$" + ((int) Mathf.Ceil(merchandise.price * percentage)).ToString();

        max_value = max;
        slider.maxValue = max_value;
        slider.value = 1;
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
        if(merchandise != null) price_text.text = "$" + ((int) Mathf.Ceil(merchandise.price * value * percentage)).ToString();
    }

    public void SliderValueChangeCheck()
    {
        int value = (int) slider.value;
        input.text = value.ToString();
        if(merchandise != null) price_text.text = "$" + ((int) Mathf.Ceil(merchandise.price * value * percentage)).ToString();
    }

    public void BuyOrSell() => itemShop.BuyOrSell(slot, (int) slider.value);
        

    
}
