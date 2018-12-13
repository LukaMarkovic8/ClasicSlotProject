using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuMenager : MonoBehaviour {
    [SerializeField]
    public Text totalCreditsText;

    public static int totalCredits = 0;

    public void Start()
    {
        totalCreditsText.text = " ";
        UpdateText();
    }
    //uptadtes credits by amount of ButtonValue.buttonValue
    public void IncreaseCredits(int amount)
    {
        totalCredits += amount;

    }

    public void LoadGame()
    {
        //loading the actual game scene
        SceneManager.LoadScene("Slot");
    }

    public void UpdateText()
    {
        totalCreditsText.text = "Balance: " + MenuMenager.totalCredits;
    }
}
