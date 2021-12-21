using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ApplyInput : MonoBehaviour {
    private bool fatigueTest = false;
    private String[] MacKenzie_Sets = new String[555];
    private String testText;

    public GameObject InputField;
    public GameObject Camera;
    public InputField field;
    public GameObject PrimaryCandidate, SecondCandidate, ThirdCandidate;
    public GameObject TestBoard, TestText;
    public GameObject TimeAlarm;

    void resetRotationAndPosition() {
        InputField.transform.rotation = Camera.transform.rotation;
        InputField.transform.TransformDirection(Camera.transform.forward);
        InputField.transform.position = Camera.transform.position;
        InputField.transform.Translate(new Vector3(0, 0, 80f));
        
        PrimaryCandidate.transform.rotation = Camera.transform.rotation;
        PrimaryCandidate.transform.TransformDirection(Camera.transform.forward);
        PrimaryCandidate.transform.position = Camera.transform.position;
        PrimaryCandidate.transform.Translate(new Vector3(0, -6, 80f));
        
        SecondCandidate.transform.rotation = Camera.transform.rotation;
        SecondCandidate.transform.TransformDirection(Camera.transform.forward);
        SecondCandidate.transform.position = Camera.transform.position;
        SecondCandidate.transform.Translate(new Vector3(-18, -6, 80f));
        
        ThirdCandidate.transform.rotation = Camera.transform.rotation;
        ThirdCandidate.transform.TransformDirection(Camera.transform.forward);
        ThirdCandidate.transform.position = Camera.transform.position;
        ThirdCandidate.transform.Translate(new Vector3(18, -6, 80f));

        TestBoard.transform.rotation = Camera.transform.rotation;
        TestBoard.transform.TransformDirection(Camera.transform.forward);
        TestBoard.transform.position = Camera.transform.position;
        if (!fatigueTest) {
            TestBoard.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 80);
            TestBoard.transform.Translate(new Vector3(0, 9.4f, 80f));
        } else {
            TestBoard.transform.Translate(new Vector3(0, 13.4f, 80f));
        }

        TimeAlarm.transform.rotation = Camera.transform.rotation;
        TimeAlarm.transform.TransformDirection(Camera.transform.forward);
        TimeAlarm.transform.position = Camera.transform.position;
        TimeAlarm.transform.Translate(new Vector3(0, -10, 80f));
    }

    IEnumerator MoveCursorToTheTail() 
    {
        yield return new WaitForEndOfFrame();
        field.MoveTextEnd(true);
    }
    void activateField() {
        if (!field.isFocused) {
            field.ActivateInputField();
        }
        StartCoroutine(MoveCursorToTheTail());
    }

    void Start() { 
        FileStream inFile = new FileStream(@".\Assets\Scripts\InputField\mackenzie-phrases.txt", FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(inFile, Encoding.Default);
        String temLine;
        int lineCnt = 0;
        while ((temLine = reader.ReadLine()) != null)
            MacKenzie_Sets[lineCnt++] = temLine;

        field = GameObject.Find("InputField").GetComponent<InputField>();
        Camera = GameObject.Find("Camera (eye)");
        InputField = GameObject.Find("InputField");
        PrimaryCandidate = GameObject.Find("PrimaryCandidate");
        SecondCandidate = GameObject.Find("SecondCandidate");
        ThirdCandidate = GameObject.Find("ThirdCandidate");
        TestBoard = GameObject.Find("TestBoard");
        TestText = GameObject.Find("TestText");
        TimeAlarm = GameObject.Find("TimeAlarm");

        System.Random rd = new System.Random();

        if (!fatigueTest) {
            testText = MacKenzie_Sets[rd.Next(0, 500)];
        } else {
            for (int i = 0; i < 8; ++i) 
                testText += MacKenzie_Sets[rd.Next(0, 500)] + (i == 7 ? ". " : ", ");
        }

        TestText.GetComponent<Text>().text = testText;

        resetRotationAndPosition();
        activateField();
    }

    void Update() {
        TestText.GetComponent<Text>().text = testText;

        resetRotationAndPosition();
        activateField();
    }
}