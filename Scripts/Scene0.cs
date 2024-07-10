using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Scene0 : MonoBehaviour {
    
    public static int numCols;
    // Start is called before the first frame update
    void Start() {
        // Create title:
        GameObject title = new GameObject("title");
        title.transform.SetParent(GameObject.Find("Canvas").transform);
        TextMeshProUGUI title_text = title.AddComponent<TextMeshProUGUI>();
        RectTransform rectTransform_title = title.GetComponent<RectTransform>();
        rectTransform_title.anchoredPosition = new Vector2(0, 575);
        rectTransform_title.sizeDelta = new Vector2(1550, 200);
        title_text.text = "Worble";
        title_text.color = Color.black;
        title_text.fontSize = 200;
        title_text.fontStyle = FontStyles.Bold;
        title_text.alignment = TextAlignmentOptions.Center;
        // Create buttons:
        int button_index = 2;
        for (int i = -370; i <= 370; i += 740) {
            for (int j = 250; j >= -600; j -= 130) {
                GameObject button = new GameObject("Button " + button_index);
                button.transform.SetParent(GameObject.Find("Canvas").transform);
                RectTransform rectTransform_button = button.AddComponent<RectTransform>();
                rectTransform_button.anchoredPosition = new Vector2(i, j);
                rectTransform_button.sizeDelta = new Vector2(650, 110);
                Button button_button = button.AddComponent<Button>();
                Image button_image = button.AddComponent<Image>();
                button_image.color = new Color32(107, 170, 100, 255);
                button_button.targetGraphic = button_image;
                GameObject button_text = new GameObject("ButtonText " + button_index);
                button_text.transform.SetParent(GameObject.Find("Button " + button_index).transform);
                TextMeshProUGUI button_text_text = button_text.AddComponent<TextMeshProUGUI>();
                RectTransform rectTransform_text = button_text_text.GetComponent<RectTransform>();
                rectTransform_text.anchoredPosition = new Vector2(0, 0);
                button_text_text.text = button_index.ToString();
                button_text_text.color = Color.black;
                button_text_text.fontSize = 90;
                button_text_text.fontStyle = FontStyles.Bold;
                button_text_text.alignment = TextAlignmentOptions.Center;
                int local_buttonIndex = button_index;
                button_button.onClick.AddListener(() => buttonClicked(local_buttonIndex));
                button_index++;
            }
        }
    }

    void buttonClicked(int num) {
        numCols = num;
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("escape")) {
            Application.Quit();
        }
    }
}

