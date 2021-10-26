using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private bool isGameActive = false;
    private int score;
    
    public Button startButton;
    public TextMeshProUGUI scoreText;
    public GameObject titleScreen;
    public GameObject pieces;
    public GameObject thowingpiece;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        isGameActive = true;
        score = 0;
        titleScreen.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        thowingpiece.gameObject.SetActive(true);
        pieces.gameObject.SetActive(true);
    }
}
