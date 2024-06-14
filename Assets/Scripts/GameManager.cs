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
    BoardManager boardManager;
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

    public void PlacePiece(Vector3 position, int x, int z)
    {
        if (x < 0 || x >= 3 || z < 0 || z >= 3 || board[x, z] != 0) return;
        if (boardManager == null) boardManager = BoardManager.Instance;

        GameObject prefab = playerOnesTurn ? prefabX : prefabO;
        GameObject obj = Instantiate(prefab, position, prefab.transform.rotation);

        pieces.Add(obj);

        StartCoroutine(AnimatePiece(obj, position));

        obj.GetComponent<Renderer>().material = playerOnesTurn ? playerOneMaterial : playerTwoMaterial;

        board[x, z] = playerOnesTurn ? 1 : 2;

        if (CheckWinner())
        {
            StartCoroutine(DelayedResults(true));
        }
        else if (CheckDraw())
        {
            StartCoroutine(DelayedResults(false));
        } else {
            if (singlePlayer && playerOnesTurn)
            {
                StartCoroutine(AIMove());
            }
        }

        playerOnesTurn = !playerOnesTurn;

    }
    public bool CheckWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] != 0 && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2]) return true;
            if (board[0, i] != 0 && board[0, i] == board[1, i] && board[1, i] == board[2, i]) return true;
        }
        if (board[0, 0] != 0 && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) return true;
        if (board[0, 2] != 0 && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]) return true;
        return false;
    }

    public bool CheckDraw()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == 0) return false;
            }
        }
        return true;
    }

    public void ResetGame()
    {
        board = new int[3, 3];
        playerOnesTurn = true;
        foreach (GameObject obj in pieces)
        {
            Destroy(obj);
        }
    }

    IEnumerator DelayedResults(bool hasWinner)
    {
        yield return new WaitForSeconds(0.3f);
        if (hasWinner)
        {
            boardManager.DeclareWinner(!playerOnesTurn);
        }
        else
        {
            boardManager.DeclareDraw();
        }
    }

    IEnumerator AnimatePiece(GameObject obj, Vector3 position)
    {
        float elapsedTime = 0;
        float raiseDuration = 0.3f;
        float raiseHeight = 3f;
        float lowerDuration = 0.3f;

        Vector3 startPosition = position - new Vector3(0f, 1f, 0f);
        Vector3 endPosition = position + new Vector3(0f, 1f, 0f);

        while (elapsedTime < raiseDuration)
        {
            elapsedTime += Time.deltaTime;
            obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / raiseDuration);
            obj.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsedTime / raiseDuration);

            yield return null;
        }

        SoundManager.Instance?.PlayAddPieceSound();

        elapsedTime = 0;
        while (elapsedTime < lowerDuration)
        {
            elapsedTime += Time.deltaTime;
            obj.transform.position = Vector3.Lerp(endPosition, position, elapsedTime / lowerDuration);
            yield return null;
        }
    }

    IEnumerator AIMove()
    {
        yield return new WaitForSeconds(0.2f);

        int[] gridPositions = { -2, 0, 2 };
        List<Vector3> availablePositions = new List<Vector3>();
        List<(int, int)> availableIndices = new List<(int, int)>();

        // Collect all available positions
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == 0)
                {
                    availablePositions.Add(new Vector3(gridPositions[i], 0, gridPositions[j]));
                    availableIndices.Add((i, j));
                }
            }
        }

        // Choose a random available position
        if (availablePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector3 position = availablePositions[randomIndex];
            (int x, int z) = availableIndices[randomIndex];
            PlacePiece(position, x, z);
        }
    }



}