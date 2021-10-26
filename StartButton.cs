using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private GameManager gameManager;
    private Button startButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton = GetComponent<Button>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        startButton.onClick.AddListener(gameManager.StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
