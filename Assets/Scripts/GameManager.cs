using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Material playerOneMaterial, playerTwoMaterial;
    public GameObject prefabX, prefabO;
    bool singlePlayer = true;
    public bool playerOnesTurn = true;
    public int[,] board = new int[3, 3]; // 0 = empty, 1 = player 1, 2 = player 2

    List<GameObject> pieces = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSinglePlayer(bool singlePlayer)
    {
        this.singlePlayer = singlePlayer;
    }
}