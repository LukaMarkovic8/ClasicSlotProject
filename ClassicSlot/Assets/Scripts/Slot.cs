using System;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    //audio
    [SerializeField]
    private AudioSource slotStoping;
    [SerializeField]
    private AudioSource winningMoney;
    //Texts
    [SerializeField]
    private Text totalBalanceText;
    [SerializeField]
    private Text displayText;

    public TextAsset paylineFile;
    //= (TextAsset)Resources.Load("textfile", typeof(TextAsset));


    [SerializeField]
    public GameObject spinButton;

    public BetMenager betMenager;

    public Reel[] reel;

    //list of betting lines
    public GameObject[] lines;
    //dictionary  for symbol values
    Dictionary<string, int> symbolValues = new Dictionary<string, int>();

    bool startSpin;

    public List<int[]> paylines = new List<int[]>();

    public float timeBetweenAnimations = 1f;

    public void Start()
    {
        spinButton.gameObject.SetActive(true);
        betMenager = FindObjectOfType<BetMenager>();
        //Initializing the values of each symbol 
        InitializeSymbolValues();

        string allPaylinesString = paylineFile.text;
        string[] allLines = allPaylinesString.Split('\n');
        int numOfPaylines = allLines.Length / (Reel.rows + 1);

        for (int i = 0; i < numOfPaylines; i++)
        {
            paylines.Add(GetPaylineFromStrings(allLines, i));
        }
    }

    private int[] GetPaylineFromStrings(string[] lines, int position)
    {
        int[] payline = new int[Reel.columns];

        int startIndex = position * (Reel.rows + 1);
        //iterating thru lines to find the payout combinations
        for (int i = 0; i < Reel.rows; i++)
        {
            //reading lines
            string line = lines[startIndex + i];
            for (int j = 0; j < Reel.columns; j++)
            {
                char element = line[j];
                //seting the payline elemnts combination 
                if (element == '*')
                {
                    payline[j] = i;
                }
            }
            Console.WriteLine(line);
            Debug.Log(line);
        }

        return payline;
    }
    //spinning 
    IEnumerator Spinning()
    {
        //deactivating the spin button while the reels are spinning
        spinButton.gameObject.SetActive(false);
        //starting the spinning on all reels
        foreach (Reel spinner in reel)
        {
            spinner.spin = true;
        }
        //stoping the reels one by one
        for (int i = 0; i < reel.Length; i++)
        {
            //waitnig for each real to stop for 1 or 2 seconds
            yield return new WaitForSeconds(UnityEngine.Random.Range(1, 2));
            reel[i].spin = false;
            //randomizing the positions of elements 
            reel[i].RandomPosition();
            //playing the sound of reel stoping
            slotStoping.Play();
        }
        startSpin = false;
    }



    //Calculating the win afte each spin
    public IEnumerator CalculateWin()
    {

        List<List<int>> winningPaylinesIndexes = new List<List<int>>();
        List<bool> isWinningPayline = new List<bool>();
        int win = 0;
        //counter used for checking how many paylines should be claculated
        int counter = 0;

        //creating array for each payline with symbols in this spin
        foreach (var payline in paylines)
        {
            string[] paylineSymbols = new string[payline.Length];
            //filling the array with coresponding symbols
            for (int i = 0; i < payline.Length; i++)
            {
                int row = payline[i];
                string sybmol = Reel.resultGrid[row, i];
                paylineSymbols[i] = sybmol;
            }
            Debug.Log(counter + 1);

            List<int> winningIndexes = new List<int>();
            foreach (var element in payline)
            {
                winningIndexes.Add(-1);
            }

            //Icreasing the win for active payline
            int lineWin = CalculatePayline(paylineSymbols, ref winningIndexes);
            winningPaylinesIndexes.Add(winningIndexes);
            //adding only paylines that won something
            isWinningPayline.Add(lineWin > 0);
            //Icreasing the win for line win
            win += lineWin;
            betMenager.totalCredits += lineWin;
            totalBalanceText.text = betMenager.totalCredits.ToString();

            //If player spent all the credits The Main menu scene is loading and credits set 
            if (betMenager.totalCredits == 0)
            {
                MenuMenager.totalCredits = betMenager.totalCredits;
                SceneManager.LoadScene("MainMenu");
            }
            counter++;
            if (counter >= betMenager.numberOfLines)
            {
                break;
            }
        }
        UpdateText(win);
        //checking wich payline to animate
        for (int i = 0; i < counter; i++)
        {
            if (isWinningPayline[i])
            {
                AnimatePayline(paylines[i], winningPaylinesIndexes[i]);
                lines[i].SetActive(true);// activating the payline
                yield return new WaitForSeconds(timeBetweenAnimations);
                lines[i].SetActive(false);  // deactivating the payline
            }
        }
        //Avtivating the spin button after calculating the win
        spinButton.gameObject.SetActive(true);
    }

    //calculating the win in a payline 
    public int CalculatePayline(string[] paylineSymbols, ref List<int> winningIndexes)
    {
        int paylineWin = 0;
        //lists of matchig symbols that are next to each other in a winning payline, maximum is two because there are five columns
        List<string> firstSymbol = new List<string>();
        List<string> secondSymbol = new List<string>();
        //multiplyers that show how many of same symbols are in the list
        int firstMultiplyer = 0, secondMultiplyer = 0;
        //seting the seconf list to false so we always start filling first list
        bool secondList = false;
        //going thru all the symbols in a playline
        for (int i = 0; i < paylineSymbols.Length - 1; i++)
        {
            //checking if symbol matches the next symbol
            if (paylineSymbols[i] == paylineSymbols[i + 1])
            {   //adding to the second list
                if (secondList == true)
                {
                    //checking if list is empty
                    if (secondSymbol.Count == 0)
                    {
                        //adding both the first time
                        winningIndexes[i] = 1;
                        winningIndexes[i + 1] = 1;
                    }
                    else
                    {  //adding next symbol
                        winningIndexes[i + 1] = 1;
                    }
                    secondSymbol.Add(paylineSymbols[i]);
                }
                else
                {
                    //checking if list is empty
                    if (firstSymbol.Count == 0)
                    {
                        //adding both first time
                        winningIndexes[i] = 1;
                        winningIndexes[i + 1] = 1;
                    }
                    else
                    {
                        //adding next symbol
                        winningIndexes[i + 1] = 1;
                    }
                    firstSymbol.Add(paylineSymbols[i]);
                }
            }
            else
            {   //checking whether to start adding to second list
                if (firstSymbol.Count != 0 && secondList == false)
                {
                    secondList = true;
                }
                winningIndexes.Add(-1);
            }
        }

        //here we add one because of the first element;
        firstMultiplyer = firstSymbol.Count + 1;
        secondMultiplyer = secondSymbol.Count + 1;
        //adding to payline win for the first list of matching symbols
        if (firstSymbol.Count != 0)
        {
            paylineWin += firstMultiplyer * SymbolMultyplyer(firstSymbol[0]) * betMenager.betOnLine;
        }
        //adding to payline win for the second list of matching symbols
        if (secondSymbol.Count != 0)
        {

            paylineWin += secondMultiplyer * SymbolMultyplyer(secondSymbol[0]) * betMenager.betOnLine;
        }
        return paylineWin;
    }

    public int SymbolMultyplyer(string symbol)
    {

        return symbolValues[symbol];
    }
    public void UpdateText(int win)
    {
        if (displayText == null)
            Debug.Log("CANT FIND DISPLAY TEXT FROM BETMENAGER");

        displayText.text = "You won " + win + " credits ";
        displayText.GetComponent<ScaleAnimation>().StartAnimation();
    }
    //method use for starting the reels spinning process
    public void SpinPressed()
    {
        if (!startSpin)
        {
            startSpin = true;
            displayText.text = "Good luck!";
            //starting spinning
            StartCoroutine(Spinning());
            //decreasing total credits by value of total bet and setting the text
            betMenager.totalCredits -= betMenager.totalBet;
            totalBalanceText.text = betMenager.totalCredits.ToString();
        }
    }

    private void PrintGrid()
    {
        foreach (int[] payline in paylines)
        {
            //printing out differetnr combinations
            string paylineString = " ";
            foreach (int elment in payline)
            {
                paylineString += " " + elment;
            }
            Debug.Log(paylineString);
        }
    }
    //animating the winning symbols in the payline
    public void AnimatePayline(int[] payline, List<int> winningIndexes)
    {
        //going thru all the elements of a payline
        for (int i = 0; i < payline.Length; i++)
        {
            //checking if winning sybol and will not be animated
            if (winningIndexes[i] == -1)
            {
                continue;
            }
            //finding wich object to animate in column
            List<GameObject> images = reel[i].FindColumObjects();
            int row = payline[i];
            GameObject imageToAnimate = images[row];
            //getting the
            var animation = imageToAnimate.GetComponent<ScaleAnimation>();
            if (animation == null)
            {
                Debug.Log("Cant find scale animation object");
            }
            else
            {
                //playing the suond
                winningMoney.Play();
                //stariting the animation
                animation.StartAnimation();
            }
        }

    }
    //Initilizeing the sybol values in the dicitionary
    public void InitializeSymbolValues()
    {
        symbolValues.Add("Cherry", 2);
        symbolValues.Add("Plum", 3);
        symbolValues.Add("Orange", 5);
        symbolValues.Add("Lemon", 10);
        symbolValues.Add("Diamond", 20);
        symbolValues.Add("Bar", 50);
        symbolValues.Add("Seven", 100);
    }
    public void GoToMenu()
    {
        MenuMenager.totalCredits = betMenager.totalCredits;
        SceneManager.LoadScene("MainMenu");
    }
}
