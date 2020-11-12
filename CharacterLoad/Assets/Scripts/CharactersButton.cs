using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactersButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Button hairButton;
    [SerializeField] private Slider colorSlider;
    [SerializeField] private Text text;

    
    private string nameObj;
    public void Setup(string id, Action<string> callbackButton, Action<string,float> callbackSlider,Action<string> callbackHairButton)
    {
        text.text = id;

        nameObj = id;
        button.onClick.AddListener(delegate
        {
            callbackButton?.Invoke(id);
        });
        
        
        colorSlider.onValueChanged.AddListener(delegate
        {
           callbackSlider?.Invoke(nameObj,colorSlider.value);
        });;
        
        hairButton.onClick.AddListener(delegate
        {
            callbackHairButton?.Invoke(id);
        });
    }

}
