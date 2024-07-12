using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Scene1 : MonoBehaviour {
    public HashSet<string> words;
    public Material white;
    public Material lightGray;
    public Material mediumGray;
    public Material darkGray;
    public Material yellow;
    public Material green;
    public Material black;
    public int numRows;
    public int numCols;
    public int curRow;
    public int curCol;
    public string curAnswer;
    public bool stopUserInput;
    public TextAsset dictionary;

    // Start is called before the first frame update
    void Start() {
        // Create 'Restart with new word' button:
        GameObject restartButton = new GameObject("RestartButton");
        restartButton.transform.SetParent(GameObject.Find("Canvas").transform);
        RectTransform rectTransform_restartButton = restartButton.AddComponent<RectTransform>();
        rectTransform_restartButton.anchoredPosition = new Vector2(990, -665);
        rectTransform_restartButton.sizeDelta = new Vector2(550, 75);
        Button restartButton_button = restartButton.AddComponent<Button>();
        Image restartButton_image = restartButton.AddComponent<Image>();
        restartButton_image.color = Color.black;
        restartButton_button.targetGraphic = restartButton_image;
        GameObject restartButton_text = new GameObject("RestartButtonText");
        restartButton_text.transform.SetParent(GameObject.Find("RestartButton").transform);
        TextMeshProUGUI restartButton_text_text = restartButton_text.AddComponent<TextMeshProUGUI>();
        RectTransform rectTransform_restartButton_text_text = restartButton_text_text.GetComponent<RectTransform>();
        rectTransform_restartButton_text_text.anchoredPosition = new Vector2(0, 0);
        rectTransform_restartButton_text_text.sizeDelta = new Vector2(550, 75);
        restartButton_text_text.text = "Restart with new word";
        restartButton_text_text.color = Color.white;
        restartButton_text_text.fontSize = 45;
        restartButton_text_text.fontStyle = FontStyles.Bold;
        restartButton_text_text.alignment = TextAlignmentOptions.Center;
        restartButton_button.onClick.AddListener(() => restartButtonClicked());
        // Create 'Go back to main menu' button:
        GameObject gbtmmButton = new GameObject("gbtmmButton");
        gbtmmButton.transform.SetParent(GameObject.Find("Canvas").transform);
        RectTransform rectTransform_gbtmmButton = gbtmmButton.AddComponent<RectTransform>();
        rectTransform_gbtmmButton.anchoredPosition = new Vector2(990, -750);
        rectTransform_gbtmmButton.sizeDelta = new Vector2(560, 75);
        Button gbtmmButton_button = gbtmmButton.AddComponent<Button>();
        Image gbtmmButton_image = gbtmmButton.AddComponent<Image>();
        gbtmmButton_image.color = Color.black;
        gbtmmButton_button.targetGraphic = gbtmmButton_image;
        GameObject gbtmmButton_text = new GameObject("gbtmmButtonText");
        gbtmmButton_text.transform.SetParent(GameObject.Find("gbtmmButton").transform);
        TextMeshProUGUI gbtmmButton_text_text = gbtmmButton_text.AddComponent<TextMeshProUGUI>();
        RectTransform rectTransform_gbtmmButton_text_text = gbtmmButton_text_text.GetComponent<RectTransform>();
        rectTransform_gbtmmButton_text_text.anchoredPosition = new Vector2(0, 0);
        rectTransform_gbtmmButton_text_text.sizeDelta = new Vector2(560, 75);
        gbtmmButton_text_text.text = "Go back to main menu";
        gbtmmButton_text_text.color = Color.white;
        gbtmmButton_text_text.fontSize = 45;
        gbtmmButton_text_text.fontStyle = FontStyles.Bold;
        gbtmmButton_text_text.alignment = TextAlignmentOptions.Center;
        gbtmmButton_button.onClick.AddListener(() => gbtmmButtonClicked());
        stopUserInput = false;
        numCols = Scene0.numCols;
        words = new HashSet<string>(dictionary.text.Split("\n"));
        words.RemoveWhere(isWrongLength);
        List<string> wordsAsList = new List<string>(words);
        curAnswer = wordsAsList[UnityEngine.Random.Range(0, wordsAsList.Count)];
        // Initialize message box:
        GameObject message = new GameObject("message");
        message.transform.SetParent(GameObject.Find("Canvas").transform);
        TextMeshProUGUI message_text = message.AddComponent<TextMeshProUGUI>();
        RectTransform rectTransform_message = message.GetComponent<RectTransform>();
        rectTransform_message.anchoredPosition = new Vector3(0, 750, -1);
        rectTransform_message.sizeDelta = new Vector2(1500, 200);
        message_text.color = Color.white;
        message_text.fontSize = 40;
        message_text.alignment = TextAlignmentOptions.Center;
        message_text.enabled = false;
        GameObject messageBox = new GameObject("message_box");
        messageBox.transform.SetParent(GameObject.Find("Canvas").transform);
        RectTransform rectTransform_messageBox = messageBox.AddComponent<RectTransform>();
        rectTransform_messageBox.anchoredPosition = new Vector3(0, 725, -1);
        LineRenderer lineRenderer_messageBox = messageBox.AddComponent<LineRenderer>();
        lineRenderer_messageBox.useWorldSpace = false;
        lineRenderer_messageBox.positionCount = 2;
        Vector3[] positions_messageBox = {new Vector2(0, -15), new Vector2(0, 65)};
        lineRenderer_messageBox.SetPositions(positions_messageBox);
        lineRenderer_messageBox.material = black;
        lineRenderer_messageBox.enabled = false;
        curRow = 0;
        curCol = 0;
        float boxBorderThickness = 6;
        float gapSpace = 15;
        float upperGapSpace = 50;
        numRows = (int)Mathf.Round(26f / numCols) + 1;
        float boxSideLength = Mathf.Min((2560 - upperGapSpace*2)/numCols - boxBorderThickness - gapSpace, (1050 - upperGapSpace*2)/numRows - gapSpace - boxBorderThickness);
        float startingXpos;
        if (numCols % 2 == 0) {
            startingXpos = gapSpace/2 + boxBorderThickness/2 - (boxSideLength + gapSpace + boxBorderThickness)*(numCols/2);
        }
        else {
            startingXpos = -boxSideLength/2 - (boxSideLength + gapSpace + boxBorderThickness)*(numCols/2);
        }
        for (int row_index = 0; row_index < numRows; row_index++) {
            for (int col_index = 0; col_index < numCols; col_index++) {
                // square_outline
                GameObject square_outline = new GameObject("SquareOutline(" + row_index.ToString() + ", " + col_index.ToString() + ")");
                square_outline.transform.SetParent(GameObject.Find("Canvas").transform);
                RectTransform rectTransform_outline = square_outline.AddComponent<RectTransform>();
                rectTransform_outline.anchoredPosition = new Vector2(startingXpos + (boxSideLength + boxBorderThickness + gapSpace)*col_index, 745 - upperGapSpace - boxBorderThickness/2.0f - boxSideLength - (boxSideLength + boxBorderThickness + gapSpace)*row_index);
                LineRenderer lineRenderer_outline = square_outline.AddComponent<LineRenderer>();
                lineRenderer_outline.useWorldSpace = false;
                lineRenderer_outline.startWidth = boxBorderThickness;
                lineRenderer_outline.positionCount = 5;
                Vector3[] positions_outline = {new Vector2(0, 0), new Vector2(0, boxSideLength), new Vector2(boxSideLength, boxSideLength), new Vector2(boxSideLength, 0), new Vector2(-boxBorderThickness/2.0f, 0)};
                lineRenderer_outline.SetPositions(positions_outline);
                lineRenderer_outline.material = lightGray;
                // square_inside
                GameObject square_inside = new GameObject("SquareInside(" + row_index.ToString() + ", " + col_index.ToString() + ")");
                square_inside.transform.SetParent(GameObject.Find("SquareOutline(" + row_index.ToString() + ", " + col_index.ToString() + ")").transform);
                RectTransform rectTransform_inside = square_inside.AddComponent<RectTransform>();
                rectTransform_inside.anchoredPosition = new Vector2(0, 0);
                LineRenderer lineRenderer_inside = square_inside.AddComponent<LineRenderer>();
                lineRenderer_inside.useWorldSpace = false;
                lineRenderer_inside.startWidth = boxSideLength - boxBorderThickness;
                lineRenderer_inside.positionCount = 2;
                Vector3[] positions_inside = {new Vector2(boxSideLength/2.0f, boxBorderThickness/2.0f), new Vector2(boxSideLength/2.0f, boxSideLength - boxBorderThickness/2.0f)};
                lineRenderer_inside.SetPositions(positions_inside);
                lineRenderer_inside.material = white;
                // letter
                GameObject Letter = new GameObject("Letter(" + row_index.ToString() + ", " + col_index.ToString() + ")");
                Letter.transform.SetParent(GameObject.Find("SquareOutline(" + row_index.ToString() + ", " + col_index.ToString() + ")").transform);
                TextMeshProUGUI letter = Letter.AddComponent<TextMeshProUGUI>();
                RectTransform rectTransform = letter.GetComponent<RectTransform>(); 
                rectTransform.anchoredPosition = new Vector2(boxSideLength/2.0f, boxSideLength/2.0f);
                rectTransform.sizeDelta = new Vector2(boxSideLength, boxSideLength);
                letter.alignment = TextAlignmentOptions.Center;
                letter.text = "";
                letter.color = Color.black;
                letter.fontSize = 30/numCols + 0.8f*boxSideLength;
            }
        }
        List<string> keyboard = new List<string>{"QWERTYUIOP", "ASDFGHJKL", "ZXCVBNM"};
        float button_width = (600f - 2*upperGapSpace)/keyboard.Count - gapSpace;
        for (int i = 0; i < keyboard.Count; i++) {
            if (keyboard[i].Length % 2 == 0) {
                startingXpos = gapSpace/2 - (button_width + gapSpace)*(keyboard[i].Length/2);
            }
            else {
                startingXpos = -button_width/2 - (button_width + gapSpace)*(keyboard[i].Length/2);
            }
            if (i == 1) {
                startingXpos -= 35;
            }
            else if (i == 2) {
                startingXpos -= 115;
            }
            // letter buttons
            for (int j = 0; j < keyboard[i].Length; j++) {
                // rectangle
                GameObject LetterButtonRectangle = new GameObject("LetterButtonRectangle(" + keyboard[i][j] + ")");
                LetterButtonRectangle.transform.SetParent(GameObject.Find("Canvas").transform);
                RectTransform rectTransform_LBR = LetterButtonRectangle.AddComponent<RectTransform>();
                rectTransform_LBR.anchoredPosition = new Vector2(startingXpos + (button_width + gapSpace)*j, -275 - button_width - (button_width + gapSpace)*i);
                LineRenderer lineRenderer_LBR = LetterButtonRectangle.AddComponent<LineRenderer>();
                lineRenderer_LBR.useWorldSpace = false;
                lineRenderer_LBR.startWidth = button_width;
                lineRenderer_LBR.positionCount = 2;
                Vector3[] positions_LBR = {new Vector2(button_width/2.0f, 0), new Vector2(button_width/2.0f, button_width)};
                lineRenderer_LBR.SetPositions(positions_LBR);
                lineRenderer_LBR.material = lightGray;
                // letter
                GameObject LetterButtonLetter = new GameObject("LetterButtonText(" + keyboard[i][j] + ")");
                LetterButtonLetter.transform.SetParent(GameObject.Find("LetterButtonRectangle(" + keyboard[i][j] + ")").transform);
                TextMeshProUGUI LetterButtonLetterText = LetterButtonLetter.AddComponent<TextMeshProUGUI>();
                RectTransform rectTransform_LBLT = LetterButtonLetter.GetComponent<RectTransform>(); 
                rectTransform_LBLT.anchoredPosition = new Vector2(button_width/2.0f, button_width/2.0f);
                rectTransform_LBLT.sizeDelta = new Vector2(button_width, button_width);
                LetterButtonLetterText.alignment = TextAlignmentOptions.Center;
                LetterButtonLetterText.text = keyboard[i][j].ToString();
                LetterButtonLetterText.color = Color.black;
                LetterButtonLetterText.fontSize = 90;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        foreach (char c in "qwertyuiopasdfghjklzxcvbnm") {
            if (Input.GetKeyDown(c.ToString()) && (curCol < numCols - 1 || (curCol == numCols - 1 && GameObject.Find("Letter(" + curRow.ToString() + ", " + (numCols - 1).ToString() + ")").GetComponent<TextMeshProUGUI>().text == "")) && stopUserInput == false) {
                GameObject.Find("Letter(" + curRow.ToString() + ", " + curCol.ToString() + ")").GetComponent<TextMeshProUGUI>().text = c.ToString().ToUpper();
                GameObject.Find("SquareOutline(" + curRow.ToString() + ", " + curCol.ToString() + ")").GetComponent<LineRenderer>().material = mediumGray;
                if (curCol < numCols - 1) {
                    curCol++;
                }
            }
        }
        if (Input.GetKeyDown("backspace") && stopUserInput == false) {
            while (true) {
                if (GameObject.Find("Letter(" + curRow.ToString() + ", " + curCol.ToString() + ")").GetComponent<TextMeshProUGUI>().text != "" || curCol == 0) {
                    GameObject.Find("Letter(" + curRow.ToString() + ", " + curCol.ToString() + ")").GetComponent<TextMeshProUGUI>().text = "";
                    GameObject.Find("SquareOutline(" + curRow.ToString() + ", " + curCol.ToString() + ")").GetComponent<LineRenderer>().material = lightGray;
                    break;
                }
                else {
                    curCol--;
                }
            }
        }
        if (Input.GetKeyDown("return")) {
            if (curCol == numCols - 1 && GameObject.Find("Letter(" + curRow.ToString() + ", " + curCol.ToString() + ")").GetComponent<TextMeshProUGUI>().text != "") {
                string submissionAttempt = "";
                for (int i = 0; i < numCols; i++) {
                    submissionAttempt += GameObject.Find("Letter(" + curRow.ToString() + ", " + i.ToString() + ")").GetComponent<TextMeshProUGUI>().text;
                }
                if (words.Contains(submissionAttempt)) {
                    for (int i = 0; i < numCols; i++) {
                        GameObject.Find("Letter(" + curRow.ToString() + ", " + i.ToString() + ")").GetComponent<TextMeshProUGUI>().color = Color.white;
                        GameObject.Find("LetterButtonText(" + submissionAttempt[i] + ")").GetComponent<TextMeshProUGUI>().color = Color.white;
                        if (GameObject.Find("Letter(" + curRow.ToString() + ", " + i.ToString() + ")").GetComponent<TextMeshProUGUI>().text == curAnswer[i].ToString()) {
                            GameObject.Find("SquareInside(" + curRow.ToString() + ", " + i.ToString() + ")").GetComponent<LineRenderer>().material = green;
                            GameObject.Find("SquareOutline(" + curRow.ToString() + ", " + i.ToString() + ")").GetComponent<LineRenderer>().material = green;
                            GameObject.Find("LetterButtonRectangle(" + submissionAttempt[i] + ")").GetComponent<LineRenderer>().material = green;
                        }
                        else if (curAnswer.Contains(GameObject.Find("Letter(" + curRow.ToString() + ", " + i.ToString() + ")").GetComponent<TextMeshProUGUI>().text)) {
                            GameObject.Find("SquareInside(" + curRow.ToString() + ", " + i.ToString() + ")").GetComponent<LineRenderer>().material = yellow;
                            GameObject.Find("SquareOutline(" + curRow.ToString() + ", " + i.ToString() + ")").GetComponent<LineRenderer>().material = yellow;
                            GameObject.Find("LetterButtonRectangle(" + submissionAttempt[i] + ")").GetComponent<LineRenderer>().material = yellow;
                        }
                        else {
                            GameObject.Find("SquareOutline(" + curRow.ToString() + ", " + i.ToString() + ")").GetComponent<LineRenderer>().material = darkGray;
                            GameObject.Find("SquareInside(" + curRow.ToString() + ", " + i.ToString() + ")").GetComponent<LineRenderer>().material = darkGray;
                            GameObject.Find("LetterButtonRectangle(" + submissionAttempt[i] + ")").GetComponent<LineRenderer>().material = darkGray;
                        }
                    }
                    if (submissionAttempt == curAnswer) {
                        stopUserInput = true;
                        GameObject.Find("message").GetComponent<TextMeshProUGUI>().text = "Great!";
                        GameObject.Find("message").GetComponent<TextMeshProUGUI>().enabled = true;
                        GameObject.Find("message_box").GetComponent<LineRenderer>().startWidth = 165;
                        GameObject.Find("message_box").GetComponent<LineRenderer>().enabled = true;
                    }
                    else if (curRow == numRows - 1) {
                        stopUserInput = true;
                        GameObject.Find("message").GetComponent<TextMeshProUGUI>().text = "The answer was '" + curAnswer + "'";
                        GameObject.Find("message").GetComponent<TextMeshProUGUI>().enabled = true;
                        GameObject.Find("message_box").GetComponent<LineRenderer>().startWidth = 355 + 28f*numCols;
                        GameObject.Find("message_box").GetComponent<LineRenderer>().enabled = true;
                    }
                    else {
                        curCol = 0;
                        curRow++;
                    }
                }
                else {
                    GameObject.Find("message").GetComponent<TextMeshProUGUI>().text = "INVALID WORD";
                    GameObject.Find("message").GetComponent<TextMeshProUGUI>().enabled = true;
                    GameObject.Find("message_box").GetComponent<LineRenderer>().startWidth = 345;
                    GameObject.Find("message_box").GetComponent<LineRenderer>().enabled = true;
                    Invoke("removeMessage", 1);
                }
            }
            else {
                GameObject.Find("message").GetComponent<TextMeshProUGUI>().text = "INCOMPLETE WORD";
                GameObject.Find("message").GetComponent<TextMeshProUGUI>().enabled = true;
                GameObject.Find("message_box").GetComponent<LineRenderer>().startWidth = 435;
                GameObject.Find("message_box").GetComponent<LineRenderer>().enabled = true;
                Invoke("removeMessage", 1);
            }
        }
    }

    void removeMessage() {
        GameObject.Find("message").GetComponent<TextMeshProUGUI>().enabled = false;
        GameObject.Find("message_box").GetComponent<LineRenderer>().enabled = false;
    }

    bool isWrongLength(string word) {
        return word.Length != numCols;
    }

    void restartButtonClicked() {
        SceneManager.LoadScene(1);
    }

    void gbtmmButtonClicked() {
        SceneManager.LoadScene(0);
    }

}