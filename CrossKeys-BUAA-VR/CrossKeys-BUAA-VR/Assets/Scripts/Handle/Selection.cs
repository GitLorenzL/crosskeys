using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WindowsInput;

using System;
using System.Text;
using System.IO;
                 
public class Selection : MonoBehaviour {
    ////////////////////////////////////////////////////////////////////////////////////////    USER INFO FORM
    ///**********************************************************************************///
    private const string userName = "Xiaolong Liu";
    private const string userOrganization = "SCSE, BUAA";
    private const int userAge = 29;
    private const int userGender = 1;                   // 1 for male, 0 for female
    private const bool userExperience = true;           // if experienced in VR
    private const bool userDominantHandRight = true;    // if dominant hand is on the right
    private const string type = "Alpha";                // Alpha, Qwerty, Freq
    private const int testDuration = 120000;                // if unlimited, assign -1, else assign time in millisecond
    ///**********************************************************************************///
    ////////////////////////////////////////////////////////////////////////////////////////    FINISH TO READY TO GO

    private float CentralThreshold = 0.07f;
    private float Left1Threshold = 0.06f;
    private float Left2Threshold = 0.06f;
    private float Right2Threshold = 0.06f;
    private float Right1Threshold = 0.06f;
    private float FrontThreshold = 0.06f;
    private float BehindThreshold = 0.06f;
    private ushort Vibration = 4000;
    private Color commonColor = Color.black;
    private Color highlightedColor = Color.yellow;
    
    private int currentBlockID = 0;
    private int previousBlockID = 0;
    
    public GameObject Left1Dot, Left2Dot, CentralDot, Right2Dot, Right1Dot, BehindDot, FrontDot;
    public GameObject Left1Block, Left2Block, CentralBlock, Right2Block, Right1Block, BehindBlock, FrontBlock;
    public GameObject TrackerSphere;
    public GameObject PWord, SWord, TWord;
    public GameObject TimeAlarm;
    public StringBuilder typingSentence = new StringBuilder();
    public String typingWord;
    public int delTimes = 0;

    private StreamWriter sw;
    private int startTime = 0;
    private bool timerStarted = false;
    public GameObject InputField;
    public GameObject TestText;

    private SteamVR_TrackedObject trackedObject;
	private SteamVR_Controller.Device device;

    private WindowsInput.VirtualKeyCode[,] Chars = new WindowsInput.VirtualKeyCode[7, 4]{ 
                                            {VirtualKeyCode.VK_A, VirtualKeyCode.VK_D, VirtualKeyCode.VK_B, VirtualKeyCode.VK_C}, 
                                            {VirtualKeyCode.VK_E, VirtualKeyCode.VK_H, VirtualKeyCode.VK_F, VirtualKeyCode.VK_G}, 
                                            {VirtualKeyCode.OEM_COMMA, VirtualKeyCode.OEM_PERIOD, VirtualKeyCode.VK_I, VirtualKeyCode.VK_J}, 
                                            {VirtualKeyCode.VK_K, VirtualKeyCode.VK_N, VirtualKeyCode.VK_L, VirtualKeyCode.VK_M}, 
                                            {VirtualKeyCode.VK_O, VirtualKeyCode.VK_R, VirtualKeyCode.VK_P, VirtualKeyCode.VK_Q}, 
                                            {VirtualKeyCode.VK_S, VirtualKeyCode.VK_V, VirtualKeyCode.VK_T, VirtualKeyCode.VK_U}, 
                                            {VirtualKeyCode.VK_W, VirtualKeyCode.VK_Z, VirtualKeyCode.VK_X, VirtualKeyCode.VK_Y}}; // Alpha
    
    
    // private WindowsInput.VirtualKeyCode[,] Chars = new WindowsInput.VirtualKeyCode[7, 4]{ 
    //                                         {VirtualKeyCode.VK_F, VirtualKeyCode.VK_K, VirtualKeyCode.VK_V, VirtualKeyCode.VK_W}, 
    //                                         {VirtualKeyCode.VK_A, VirtualKeyCode.VK_N, VirtualKeyCode.VK_S, VirtualKeyCode.VK_R}, 
    //                                         {VirtualKeyCode.OEM_COMMA, VirtualKeyCode.OEM_PERIOD, VirtualKeyCode.VK_E, VirtualKeyCode.VK_I}, 
    //                                         {VirtualKeyCode.VK_T, VirtualKeyCode.VK_C, VirtualKeyCode.VK_O, VirtualKeyCode.VK_L}, 
    //                                         {VirtualKeyCode.VK_X, VirtualKeyCode.VK_Q, VirtualKeyCode.VK_J, VirtualKeyCode.VK_Z}, 
    //                                         {VirtualKeyCode.VK_G, VirtualKeyCode.VK_Y, VirtualKeyCode.VK_H, VirtualKeyCode.VK_B}, 
    //                                         {VirtualKeyCode.VK_D, VirtualKeyCode.VK_M, VirtualKeyCode.VK_P, VirtualKeyCode.VK_U}}; // Freq
    
    
    // private WindowsInput.VirtualKeyCode[,] Chars = new WindowsInput.VirtualKeyCode[7, 4]{ 
    //                                         {VirtualKeyCode.VK_Q, VirtualKeyCode.VK_Z, VirtualKeyCode.VK_A, VirtualKeyCode.VK_S}, 
    //                                         {VirtualKeyCode.VK_W, VirtualKeyCode.VK_X, VirtualKeyCode.VK_D, VirtualKeyCode.VK_F}, 
    //                                         {VirtualKeyCode.OEM_COMMA, VirtualKeyCode.OEM_PERIOD, VirtualKeyCode.VK_E, VirtualKeyCode.VK_I}, 
    //                                         {VirtualKeyCode.VK_O, VirtualKeyCode.VK_N, VirtualKeyCode.VK_H, VirtualKeyCode.VK_J}, 
    //                                         {VirtualKeyCode.VK_P, VirtualKeyCode.VK_M, VirtualKeyCode.VK_K, VirtualKeyCode.VK_L}, 
    //                                         {VirtualKeyCode.VK_G, VirtualKeyCode.VK_V, VirtualKeyCode.VK_C, VirtualKeyCode.VK_B}, 
    //                                         {VirtualKeyCode.VK_T, VirtualKeyCode.VK_Y, VirtualKeyCode.VK_R, VirtualKeyCode.VK_U}}; // Qwerty
    
    
    private String[,] CharsActual = new String[7, 4] {  {"a", "d", "b", "c"}, 
                                                        {"e", "h", "f", "g"}, 
                                                        {",", ".", "i", "j"}, 
                                                        {"k", "n", "l", "m"}, 
                                                        {"o", "r", "p", "q"}, 
                                                        {"s", "v", "t", "u"}, 
                                                        {"w", "z", "x", "y"}}; // Alpha
    
    
    // private String[,] CharsActual = new String[7, 4] {  {"f", "k", "v", "w"}, 
    //                                                     {"a", "n", "s", "r"}, 
    //                                                     {",", ".", "e", "i"}, 
    //                                                     {"t", "c", "o", "l"}, 
    //                                                     {"x", "q", "j", "z"}, 
    //                                                     {"g", "y", "h", "b"}, 
    //                                                     {"d", "m", "p", "u"}}; // Freq
    
    
    // private String[,] CharsActual = new String[7, 4] {  {"q", "z", "a", "s"}, 
    //                                                     {"w", "x", "d", "f"}, 
    //                                                     {",", ".", "e", "i"}, 
    //                                                     {"o", "n", "h", "j"}, 
    //                                                     {"p", "m", "k", "l"}, 
    //                                                     {"g", "v", "c", "b"}, 
    //                                                     {"t", "y", "r", "u"}}; // Qwerty
    

    private const string DEL = "<Del>";
    private const string SPACE = "<Space>";
    private const string PRIMARY = "<PC>";
    private const string SECOND = "<SC>";
    private const string THIRD = "<TC>";
    
    void Start() {
        string filePath = @".\Assets\Scripts\InputField\log.txt";
        sw = File.AppendText(filePath);
        Left1Dot = GameObject.Find("Left1Dot");
        Left2Dot = GameObject.Find("Left2Dot");
        CentralDot = GameObject.Find("CentralDot");
        Right2Dot = GameObject.Find("Right2Dot");
        Right1Dot = GameObject.Find("Right1Dot");
        FrontDot = GameObject.Find("FrontDot");
        BehindDot = GameObject.Find("BehindDot");
        Left1Block = GameObject.Find("Left1Block");
        Left2Block = GameObject.Find("Left2Block");
        CentralBlock = GameObject.Find("CentralBlock");
        Right2Block = GameObject.Find("Right2Block");
        Right1Block = GameObject.Find("Right1Block");
        FrontBlock = GameObject.Find("FrontBlock");
        BehindBlock = GameObject.Find("BehindBlock");
        TrackerSphere = GameObject.Find("TrackSphere");
        PWord = GameObject.Find("PWord");
        SWord = GameObject.Find("SWord");
        TWord = GameObject.Find("TWord");
        TimeAlarm = GameObject.Find("TimeAlarm");
        InputField = GameObject.Find("InputField");
        TestText = GameObject.Find("TestText");

        trackedObject = GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input ((int)trackedObject.index);

        initLog();
        colorInitiation();
        lockCurrentBlock();
        highlightCurrentBlock();
    }

    String getTypingWord() {
        for (int i = typingSentence.Length - 1; i >=0; --i) {
            if (typingSentence[i] == ' ')
                return typingSentence.ToString().Substring(i + 1, typingSentence.Length - i - 1);
            if (i == 0)
                return typingSentence.ToString();
        }
        return "";
    }

    void Update() {
        typingWord = getTypingWord();
        
        string[] res = predict(typingWord);
        
        updateCandidate(res);
        lockCurrentBlock();
        highlightCurrentBlock();
        listenButtons();

        ifTerminateTiming();
    }

    void initLog() {
        sw.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        sw.WriteLine("Date: " + DateTime.Now.ToString());
        sw.WriteLine("User: " + userName + ", " + userAge + ", " + (userGender == 0 ? "female" : "male") + ", " + (userExperience ? "" : "in") + "experienced" + ", " + (userDominantHandRight ? "right-" : "left-") + "handed" + ", from " + userOrganization);
        sw.WriteLine("Duration: " + (testDuration > 0 ? testDuration.ToString()  + "ms": "unlimited"));
        sw.WriteLine("Type: " + type);
        sw.WriteLine("Entry Tokens: ");
    }

    void colorInitiation() {
        Left1Block.GetComponent<MeshRenderer>().material.color = commonColor;
        Left2Block.GetComponent<MeshRenderer>().material.color = commonColor;
        Right1Block.GetComponent<MeshRenderer>().material.color = commonColor;
        Right2Block.GetComponent<MeshRenderer>().material.color = commonColor;
        CentralBlock.GetComponent<MeshRenderer>().material.color = commonColor;
        BehindBlock.GetComponent<MeshRenderer>().material.color = commonColor;
        FrontBlock.GetComponent<MeshRenderer>().material.color = commonColor;
    }

    void updateCandidate(string[] res) {
        PWord.GetComponent<Text>().text = res[0];
        SWord.GetComponent<Text>().text = res[1];
        TWord.GetComponent<Text>().text = res[2];
    }

    string[] returnVals = new string[3];
    string[] predict(string readIn) {
        FileStream inFile = new FileStream(@".\Assets\Scripts\Handle\30k.txt", FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(inFile, Encoding.Default);

        int subLen = readIn.Length, returnCnt = 0;
        string aLine;
        while ((aLine = reader.ReadLine()) != null) {
            if (aLine.Length < subLen)
                continue;
            if (String.Equals(aLine.Substring(0, subLen), readIn))
                returnVals[returnCnt++] = aLine;
            if (returnCnt == 3) 
                break;
        }
        return returnVals;
    }

    WindowsInput.VirtualKeyCode getVirtualKeycode(int dir) {
        return Chars[currentBlockID, dir];
    }
    
    void lockCurrentBlock() {
        previousBlockID = currentBlockID;
        float left1Dist, left2Dist, centralDist, right2Dist, right1Dist, frontDist, behindDist;
        left1Dist = (Left1Dot.transform.position - TrackerSphere.transform.position).magnitude;
        left2Dist = (Left2Dot.transform.position - TrackerSphere.transform.position).magnitude; 
        centralDist = (CentralDot.transform.position - TrackerSphere.transform.position).magnitude;
        right2Dist = (Right2Dot.transform.position - TrackerSphere.transform.position).magnitude;
        right1Dist = (Right1Dot.transform.position - TrackerSphere.transform.position).magnitude;
        frontDist = (FrontDot.transform.position - TrackerSphere.transform.position).magnitude;
        behindDist = (BehindDot.transform.position - TrackerSphere.transform.position).magnitude;

        // print("left1Dist: " + left1Dist + ", left2Dist: " + left2Dist + ", centralDist: " + centralDist + ", right2Dist: " + right2Dist + ", right1Dist: " + right1Dist + ", frontDist: " + frontDist + ", behindDist: " + behindDist);
        // sw.WriteLine("left1Dist: {0}, left2Dist: {1}, centralDist: {2}, right2Dist: {3}, right1Dist: {4}, frontDist: {5}, behindDist: {6}", 
        //                     left1Dist, left2Dist, centralDist, right2Dist, right1Dist, frontDist, behindDist);// Log Write

        if (left1Dist <= Left1Threshold) {
            currentBlockID = 0;
        } else if (left2Dist <= Left2Threshold) {
            currentBlockID = 1;
        } else if (centralDist <= CentralThreshold) {
            currentBlockID = 2;
        } else if (right2Dist <= Right2Threshold) {
            currentBlockID = 3;
        } else if (right1Dist <= Right1Threshold) {
            currentBlockID = 4;
        } else if (frontDist <= FrontThreshold) {
            currentBlockID = 5;
        } else if (behindDist <= BehindThreshold) {
            currentBlockID = 6;
        } else {
            currentBlockID = 7;
        }
    }

    bool touched = false;
    bool followAuto = false;

    void writeLog(string str) {
        sw.Write(str);
    }

    void ifStartTiming() {
        if (!timerStarted) {
            timerStarted = true;
            startTime = System.Environment.TickCount;
        }
    }

    void ifTerminateTiming() {
        if (timerStarted) {
            int currentRunningTime = System.Environment.TickCount - startTime;
            if (currentRunningTime >= testDuration - 5000 && testDuration >= 0) {
                TimeAlarm.GetComponent<Image>().color = Color.red;
            }
            if (currentRunningTime >= testDuration - 4250 && testDuration >= 0) {
                TimeAlarm.GetComponent<Image>().color = Color.white;
            }
            if (currentRunningTime >= testDuration - 4000 && testDuration >= 0) {
                TimeAlarm.GetComponent<Image>().color = Color.red;
            }
            if (currentRunningTime >= testDuration - 3250 && testDuration >= 0) {
                TimeAlarm.GetComponent<Image>().color = Color.white;
            }
            if (currentRunningTime >= testDuration - 3000 && testDuration >= 0) {
                TimeAlarm.GetComponent<Image>().color = Color.red;
            }
            if (currentRunningTime >= testDuration - 2250 && testDuration >= 0) {
                TimeAlarm.GetComponent<Image>().color = Color.white;
            }
            if (currentRunningTime >= testDuration - 2000 && testDuration >= 0) {
                TimeAlarm.GetComponent<Image>().color = Color.red;
            }
            if (currentRunningTime >= testDuration - 1250 && testDuration >= 0) {
                TimeAlarm.GetComponent<Image>().color = Color.white;
            }
            if (currentRunningTime >= testDuration - 1000 && testDuration >= 0) {
                TimeAlarm.GetComponent<Image>().color = Color.red;
            }
            if (currentRunningTime >= testDuration - 250 && testDuration >= 0) {
                TimeAlarm.GetComponent<Image>().color = Color.white;
            }
            if (currentRunningTime >= testDuration && testDuration >= 0) {
                string realTextEntered = InputField.GetComponent<InputField>().text;
                string testText = TestText.GetComponent<Text>().text;
                sw.WriteLine("\nAssigned Phrase: ");
                sw.WriteLine("\"" + testText + "\"");
                sw.WriteLine("\nReal Text Entered: ");
                sw.WriteLine("\"" + realTextEntered + "\"");
                sw.WriteLine("WPM: " + (realTextEntered.Length / 5.0) / (testDuration / 60000.0));
                ErrorRate ERate = new ErrorRate(delTimes);
                sw.WriteLine("TER: " + ERate.TER(testText, realTextEntered.Substring(0, realTextEntered.Length)));
                sw.WriteLine("NCER: " + ERate.NCER(testText, realTextEntered.Substring(0, realTextEntered.Length)));
                sw.Close();
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }
    }

    int terminate = 0;  // Press SPACE 2 times to terminate.

    void listenButtons() {
        if (currentBlockID < 7) {
			if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) {
                if (device.GetAxis ().y > device.GetAxis ().x && device.GetAxis ().y > -device.GetAxis ().x && !touched) {
                    if ((Chars[currentBlockID, 0] == VirtualKeyCode.OEM_COMMA) && followAuto) 
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.BACK);
                    followAuto = false;
                    device.TriggerHapticPulse(Vibration);
                    InputSimulator.SimulateKeyPress(getVirtualKeycode(0));
                    writeLog("<" + getVirtualKeycode(0).ToString() + ">");
                    terminate = 0;
                    ifStartTiming();
                    typingSentence.Append(CharsActual[currentBlockID, 0]);
                    if ((Chars[currentBlockID, 0] == VirtualKeyCode.OEM_COMMA)) {
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.SPACE);
                        typingSentence.Append(' ');
                    }
                    touched = true;
                } else if (device.GetAxis ().y < -device.GetAxis ().x && device.GetAxis ().y < device.GetAxis ().x && !touched) {
                    if ((Chars[currentBlockID, 1] == VirtualKeyCode.OEM_PERIOD) && followAuto)
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.BACK);
                    followAuto = false;
                    device.TriggerHapticPulse(Vibration);
                    InputSimulator.SimulateKeyPress(getVirtualKeycode(1));
                    writeLog("<" + getVirtualKeycode(1).ToString() + ">");
                    terminate = 0;
                    ifStartTiming();
                    typingSentence.Append(CharsActual[currentBlockID, 1]);
                    if ((Chars[currentBlockID, 1] == VirtualKeyCode.OEM_PERIOD)) {
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.SPACE);
                        typingSentence.Append(' ');
                    }
                    touched = true;
                } else if (device.GetAxis ().y < -device.GetAxis ().x && device.GetAxis ().y > device.GetAxis ().x && !touched) {
                    device.TriggerHapticPulse(Vibration);
                    InputSimulator.SimulateKeyPress(getVirtualKeycode(2));
                    writeLog("<" + getVirtualKeycode(2).ToString() + ">");
                    terminate = 0;
                    ifStartTiming();
                    typingSentence.Append(CharsActual[currentBlockID, 2]);
                    touched = true;
                } else if (device.GetAxis ().y > -device.GetAxis ().x && device.GetAxis ().y < device.GetAxis ().x && !touched) {
                    device.TriggerHapticPulse(Vibration);
                    InputSimulator.SimulateKeyPress(getVirtualKeycode(3));
                    writeLog("<" + getVirtualKeycode(3).ToString() + ">");
                    terminate = 0;
                    ifStartTiming();
                    typingSentence.Append(CharsActual[currentBlockID, 3]);
                    touched = true;
                }
            } else {
                touched = false;
            }
		} else {
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
            if (device.GetAxis ().x < -0.33f) {
                if(device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
                    simuTextWord(returnVals[1], typingWord);
                    writeLog(SECOND);
                    terminate = 0;
                }
            }
            else if (device.GetAxis ().x > -0.33f && device.GetAxis ().x < 0.33f) {
                if(device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
                    simuTextWord(returnVals[0], typingWord);
                    writeLog(PRIMARY);
                    terminate = 0;
                }
            }
            else if (device.GetAxis ().x > 0.33f) {
                if(device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
                    simuTextWord(returnVals[2], typingWord);
                    writeLog(THIRD);
                    terminate = 0;
                }
            }
        }
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {   // BlankSpace
            ++terminate;
            print(terminate);

            if (terminate == 2 && testDuration < 0) {
                string realTextEntered = InputField.GetComponent<InputField>().text;
                string testText = TestText.GetComponent<Text>().text;
                sw.WriteLine("\nAssigned Phrase: ");
                sw.WriteLine("\"" + testText + "\"");
                sw.WriteLine("Real Text Entered: ");
                sw.WriteLine("\"" + realTextEntered + "\"");
                sw.WriteLine("WPM: " + (realTextEntered.Length / 5.0) / ((System.Environment.TickCount - startTime) / 60000.0));
                ErrorRate ERate = new ErrorRate(delTimes);
                sw.WriteLine("TER: " + ERate.TER(testText, realTextEntered.Substring(0, realTextEntered.Length)));
                sw.WriteLine("NCER: " + ERate.NCER(testText, realTextEntered.Substring(0, realTextEntered.Length)));
                sw.Close();
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
            InputSimulator.SimulateKeyPress(VirtualKeyCode.SPACE);
            typingSentence.Append(' ');
            writeLog(SPACE);
            ifStartTiming();
        }
        if (device.GetHairTriggerDown()) {   // Delete
            InputSimulator.SimulateKeyPress(VirtualKeyCode.BACK);
            writeLog(DEL);
            ++delTimes;
            terminate = 0;
            ifStartTiming();
            if (typingSentence.Length > 0)
                typingSentence.Remove(typingSentence.Length - 1, 1);
        }
    }

    void highlightCurrentBlock() {
        switch (previousBlockID) {
            case 0:
                Left1Dot.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                Left1Block.GetComponent<MeshRenderer>().material.color = commonColor;  
                Left1Block.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                break;
            case 1:
                Left2Dot.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                Left2Block.GetComponent<MeshRenderer>().material.color = commonColor;  
                Left2Block.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                break;
            case 2:
                CentralDot.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                CentralBlock.GetComponent<MeshRenderer>().material.color = commonColor;  
                CentralBlock.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                break;
            case 3:
                Right2Dot.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                Right2Block.GetComponent<MeshRenderer>().material.color = commonColor;  
                Right2Block.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                break;
            case 4:
                Right1Dot.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                Right1Block.GetComponent<MeshRenderer>().material.color = commonColor;  
                Right1Block.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                break;
            case 5:
                FrontDot.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                FrontBlock.GetComponent<MeshRenderer>().material.color = commonColor;  
                FrontBlock.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                break;
            case 6:
                BehindDot.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                BehindBlock.GetComponent<MeshRenderer>().material.color = commonColor;  
                BehindBlock.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                break;
            default:
                break;
        }
        switch (currentBlockID) {
            case 0:
                Left1Dot.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                Left1Block.GetComponent<MeshRenderer>().material.color = highlightedColor;  
                Left1Block.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case 1:
                Left2Dot.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                Left2Block.GetComponent<MeshRenderer>().material.color = highlightedColor;  
                Left2Block.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case 2:
                CentralDot.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                CentralBlock.GetComponent<MeshRenderer>().material.color = highlightedColor;  
                CentralBlock.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case 3:
                Right2Dot.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                Right2Block.GetComponent<MeshRenderer>().material.color = highlightedColor;  
                Right2Block.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case 4:
                Right1Dot.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                Right1Block.GetComponent<MeshRenderer>().material.color = highlightedColor;  
                Right1Block.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case 5:
                FrontDot.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                FrontBlock.GetComponent<MeshRenderer>().material.color = highlightedColor;  
                FrontBlock.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case 6:
                BehindDot.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                BehindBlock.GetComponent<MeshRenderer>().material.color = highlightedColor;  
                BehindBlock.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            default:
                break;
        }
    } 

    void simuTextWord(string word, string typingWord) {
        for (int i = 0; i < word.Length; ++i) {
            if (i < typingWord.Length) continue;
            if (word[i] == 'a')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A);
            if (word[i] == 'b')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_B);
            if (word[i] == 'c')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_C);
            if (word[i] == 'd')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_D);
            if (word[i] == 'e')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E);
            if (word[i] == 'f')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_F);
            if (word[i] == 'g')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_G);
            if (word[i] == 'h')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_H);
            if (word[i] == 'i')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I);
            if (word[i] == 'j')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_J);
            if (word[i] == 'k')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_K);
            if (word[i] == 'l')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L);
            if (word[i] == 'm')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_M);
            if (word[i] == 'n')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_N);
            if (word[i] == 'o')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O);
            if (word[i] == 'p')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_P);
            if (word[i] == 'q')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_Q);
            if (word[i] == 'r')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_R);
            if (word[i] == 's')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S);
            if (word[i] == 't')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_T);
            if (word[i] == 'u')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_U);
            if (word[i] == 'v')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_V);
            if (word[i] == 'w')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_W);
            if (word[i] == 'x')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_X);
            if (word[i] == 'y')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_Y);
            if (word[i] == 'z')
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_Z);
            typingSentence.Append(word[i]);
        }
        InputSimulator.SimulateKeyPress(VirtualKeyCode.SPACE);
        typingSentence.Append(' ');
        ifStartTiming();
        followAuto = true;
    }
}