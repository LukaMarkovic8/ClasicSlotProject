using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetMenager : MonoBehaviour
{
    //Game objects and texts
    [SerializeField]
    private Text displayText;
    [SerializeField]
    private Text linesText;
    [SerializeField]
    private Text betOnLineText;
    [SerializeField]
    private Text totalBetText;
    [SerializeField]
    private GameObject spinButton;

    //values that hold the amount of credits left for playing the game
    public int totalCredits = 0;

    public int totalBet = 1;

    public int minBet = 1;

    public int maxBet = 110;

    public int betOnLine = 1;

    public int numberOfLines = 1;

    public void Start()
    {
        //Set total credits to vale of credits bought in menu scene
        totalCredits = MenuMenager.totalCredits;
        totalBet = betOnLine * numberOfLines;
    }
    public void IncreaseBetPerLine()
    {
        //checking if bet is allowed
        if (totalBet < maxBet)
        {
            betOnLine += 1;
            UpdateDisplayText("You are betting  " + betOnLine + " per line");
            betOnLineText.text = betOnLine.ToString();
        }
        else
        {
            UpdateDisplayText("You've reached the limit for maximum bet ");
        }
        //update the total bet value and text
        UpdateTotalBet();
    }

    public void DecreaseBetPerLine()
    {
        //checking if bet is allowed
        if (minBet == totalBet || totalBet < minBet)
        {
            UpdateDisplayText("The minimum bet is: " + minBet.ToString());
            betOnLine = 1;
            betOnLineText.text = betOnLine.ToString();
        }
        else
        {
            betOnLine -= 1;
            UpdateDisplayText("You are betting " + betOnLine + " per line");
            betOnLineText.text = betOnLine.ToString();
        }
        //using this method to the total bet value and text
        UpdateTotalBet();
    }
   
    private void UpdateDisplayText(string text)
    {
        displayText.text = text;
    }

    public void IncreaseNumberOfLines()
    {
        //checkig if bet is allowed
        if (numberOfLines > 10)
        {
            UpdateDisplayText("Max number of lines reached");
        }
        else
        {
            numberOfLines++;
            linesText.text = numberOfLines.ToString();
            displayText.text = "You bet on " + numberOfLines + " lines";
        }
        //using this method to the total bet value and text
        UpdateTotalBet();
    }
    public void DecreaseNumberOfLines()
    {
        //checkig if bet is allowed
        if (numberOfLines < 2)
        {
            UpdateDisplayText("Minimum number of lines reached");
        }
        else
        {
            numberOfLines--;
            linesText.text = numberOfLines.ToString();
            UpdateDisplayText("You bet on " + numberOfLines + " lines");
        }
        //using this method to the total bet value and text
        UpdateTotalBet();
    }
    //Updating the total bet
    public void UpdateTotalBet()
    {
        totalBet = betOnLine * numberOfLines;
        totalBetText.text = totalBet.ToString();
        //checkig if toal bet is allowed depending on total credits
        if (totalBet > totalCredits)
        {
            spinButton.gameObject.SetActive(false);
        }
        else
        {
            spinButton.gameObject.SetActive(true);
        }
    }
}
