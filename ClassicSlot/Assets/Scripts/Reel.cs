using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Script used for randomizing elemnts and keeping track of results 
public class Reel : MonoBehaviour
{
    public bool spin;
    public float speed;
    public const int rows = 3;
    public const int columns = 5;

    private BetMenager betMenager;
    private Slot slot;
    //grid of names of results after spinning
    static public string[,] resultGrid = new string[rows, columns];
    static int columnCounter = 0;

    void Start()
    {
        if (betMenager == null)
        {
            betMenager = FindObjectOfType<BetMenager>();
            if (betMenager == null)
                Debug.LogError("CANT FIND BETMENAGER!");
        }
        if (slot == null)
        {
            slot = FindObjectOfType<Slot>();
        }
        spin = false;
    }

    void Update()
    {
        if (spin)
        {
            foreach (Transform image in transform)
            {  //moving image down untill a certain point
                image.transform.Translate(Vector3.down * Time.smoothDeltaTime * speed, Space.World);

                if (image.transform.position.y <= 0)
                {
                    //moving the image on top 
                    image.transform.position = new Vector3(image.transform.position.x, image.transform.position.y + 800, image.transform.position.z);
                }
            }
        }
    }
    //Updates a column in result grid
    public void UpdateColumn()
    {
        var columnResults = FindColumnElementsNames();

        for (int i = 0; i < rows; i++)
        {
            resultGrid[i, columnCounter] = columnResults[i];
        }
        columnCounter++;
        if (columnCounter == columns)
        {
            StartCoroutine(slot.CalculateWin());
            columnCounter = 0;
        }
    }
    //randomizing the positions of a column
    public void RandomPosition()
    {
        //tansform values
        List<int> parts = new List<int>
        {
            300,
            200,
            100,
            0,
            -100,
            -200,
            -300
        };

        foreach (Transform image in transform)
        {
            //taking a random position
            int rand = Random.Range(0, parts.Count);
            //setting the image to this randomized position
            image.transform.position = new Vector3(image.transform.position.x, parts[rand] + transform.parent.GetComponent<RectTransform>().transform.position.y, image.transform.position.z);
            parts.RemoveAt(rand);
        }
        UpdateColumn();
    }

    //find the elemnts names in one column
    public List<string> FindColumnElementsNames()
    {

        List<GameObject> elements = FindColumObjects();
        List<string> result = new List<string>();
        foreach (GameObject element in elements)
        {
            result.Add(element.name.ToString());
        }

        return result;
    }
    //list of colun game objects taht are used for paylines
    public List<GameObject> FindColumObjects()
    {
        List<Transform> elements = new List<Transform>();
        //adding all the elements
        foreach (Transform element in transform)
        {
            elements.Add(element);
        }
        //sorting them by transform position
        Transform[] sortedVectors = elements.OrderBy(v => v.transform.position.y).ToArray<Transform>();
        //elemnts used in paylines middle, line above and line below
        int midElemnt = elements.Count / 2;
        int upElemnt = midElemnt + 1;
        int downElemnt = midElemnt - 1;
        //creating the list of column elemnts used in result grid
        List<GameObject> result = new List<GameObject>();
        result.Add(sortedVectors[upElemnt].gameObject);
        result.Add(sortedVectors[midElemnt].gameObject);
        result.Add(sortedVectors[downElemnt].gameObject);

        return result;
    }
}
