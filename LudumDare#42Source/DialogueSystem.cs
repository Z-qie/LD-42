using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine. UI;

public class DialogueSystem : MonoBehaviour
{
    public GameObject talkUI;
    public GameObject Button;
    public GameObject Button2;
    bool inside;

    [Header("UI组件")]
    public Text textLabel;
    public Image faceImage;

    [Header("文本文档")]
    public TextAsset textfile;
    public int index;
    public float textSpeed;

    [Header("头像")]
    public Sprite face01, face02;

    bool cancelTyping;
    bool textFinished;
    List<string> textList = new List<string>();
    
    void Awake()
    {
        GetTextFromFile(textfile);
        index = 0;
    }

    private void OnEnable()
    {
        //textLabel.text = textList[index];
        //index++;
        textFinished = true;
        StartCoroutine(SetTextUI());
    }
     
    // Update is called once per frame
    void Update()
    {
        if (Button.activeSelf && Input.GetKeyDown(KeyCode.E) && inside == true)
        {
            talkUI.SetActive(true);
            Button.SetActive(false);
            Button2.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.E) && index == textList.Count)
        {
            talkUI.SetActive (false);
            index = 0;
            Button2.SetActive(false);
            if (inside == true)
            {
                Button.SetActive(true);
            }
            if (gameObject.tag == "collection")
            {
                Destroy(gameObject);
                //other.GetComponent<???>().enabled = true;
            }
            return;
        }
        /* (Input.GetKeyDown(KeyCode.E)&& textFinished )
        {
            //textLabel.text = textList[index];
            //index++;
            StartCoroutine(SetTextUI());
        }*/

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (textFinished && !cancelTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!textFinished&&!cancelTyping )
            {
                cancelTyping = true;
            }
        }

    }

   public void OnTriggerEnter2D(Collider2D other)
    {
        inside = true;
        if (Button2.activeSelf == false)
        {
            Button.SetActive(true);
        }
        if (gameObject.tag == "collection")
        {
            //other.GetComponent<???>().enabled = false;
        }
    }

    public  void OnTriggerExit2D(Collider2D other)
    {
        inside = false;
        Button.SetActive(false);
    }

    void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        index = 0;
        var lineData = file.text.Split('\n');
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

        /*for (int i = 0; i < textList[index].Length; i++)
          {
              textLabel.text += textList[index][i];
              yield return new WaitForSeconds(textSpeed); 
          }*/

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
}
