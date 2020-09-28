using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_UI
    : MonoBehaviour
{
    public GameObject Button_E;
    public GameObject Button_Space;
    public Text textLabel;
    public Image faceImage;
    public TextAsset textfile;
    public int index;
    public float textSpeed;
    public Sprite face01;
    public Sprite face02;

    public bool showStatus=false;
    public bool started=false;
    bool cancelTyping;
    bool textFinished;
    List<string> textList = new List<string>();

    void Awake()
    {
        GetTextFromFile(textfile);
        index = 0;
        Button_E.SetActive(true);
    }

    private void OnEnable()
    {
        textFinished = true;
        StartCoroutine(SetTextUI());
    }

    void Update()
    {
        if (started == false)
        {
            if (showStatus == false)
            {
                showStatus = GetComponent<Dialogue_UI_Movement>().isShow;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && index == textList.Count)
        {
            
            index = 0;
            Button_E.SetActive(false);
            Button_Space.SetActive(true);
            showStatus = false;
            started = true;

            return;
        }


        if (showStatus == true&& Input.GetKeyDown(KeyCode.E))
        {
            if (textFinished && !cancelTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!textFinished && !cancelTyping)
            {
                cancelTyping = true;
            }
            
        }

    }

    void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        index = 0;
        var lineData = file.text.Split(new char[] { '\r', '\n'},System.StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";

        switch (textList[index])
        {
            case "A":
                faceImage.sprite = face01;
                index++;
                break;

            case "B":
                faceImage.sprite = face02;
                index++;
                break;
        }

        int letter = 0;
        while (!cancelTyping && letter < textList[index].Length - 1)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];
        cancelTyping = false;
        textFinished = true;
        index++;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            
   
        }
    }
}
