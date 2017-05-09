using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoTextHandler : MonoBehaviour {
    private const float WAIT_TIME = 1.2f;
    private const int MAX_LINE = 10;

    private bool isEmpty = true;
    private bool countStarted = false;

    private ArrayList currentText = new ArrayList(MAX_LINE);

    private Text display;
	// Use this for initialization
	void Start () {
        display = GetComponent<Text>();
        display.text = "";
        InsertMessage("Nothing new");
	}
	
	// Update is called once per frame
	void Update () {
        if (!countStarted && !isEmpty)
            StartCoroutine(DeleteLast());

        //TODO do better.
        display.text = "";
        for (int i = 0; i < currentText.Count; i++)
        {
            display.text += currentText[i];
        }
	}

    private IEnumerator DeleteLast()
    {
        countStarted = true;
        yield return new WaitForSeconds(WAIT_TIME);
        if (currentText.Count == 1)
        {
            currentText.RemoveAt(0);
            isEmpty = true;
        }
        else
        {
            for (int i = 0; i < currentText.Count - 1; i++)
            {
                currentText[i] = currentText[i + 1];
            }
            currentText.RemoveAt(currentText.Count - 1);
        }
        countStarted = false;

    }

    public void InsertMessage(string msg)
    {
        if(currentText.Count + 1 > MAX_LINE)
        {
            for(int i = 0;i<MAX_LINE -1;i++)
            {
                currentText.Insert(i, currentText[i+1]);
            }
            currentText.Insert(currentText.Count, msg + "\n");
        }
        else
        {
            currentText.Add(msg + "\n");
        }
        isEmpty = false;
    }
}
