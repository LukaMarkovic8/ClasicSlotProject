using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script for setting button values and connecting buttons to the GameMenager
public class ButtonValue : MonoBehaviour
{
    //Value of how much a button increses total credits
    [SerializeField]
    public int buttonValue;
    //Button value text
    private Text buttonText;
    //particles on button click
    [SerializeField]
    private ParticleSystem onClickEffect;

    private MenuMenager mm;

    public void Start()
    {
        mm = FindObjectOfType<MenuMenager>();
        //finding the text on the Button gameObject
        buttonText = GetComponentInChildren<Text>();
        buttonText.text = "Buy " + buttonValue + "Credits";
    }
    //Buying credits 
    public void BuyCredits()
    {
        //increasing credit depending on button value
        mm.IncreaseCredits(buttonValue);
        FindObjectOfType<MenuMenager>().UpdateText();
 
    }
    public void PlayParticleEffect()
    {
        onClickEffect.Play();
    }
}
