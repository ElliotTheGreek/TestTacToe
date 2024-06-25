using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    public RectTransform resultsPanel, gameMenuPanel;
    public TextMeshProUGUI resultsText;
    GameManager gameManager;

    bool activeGame = true;
    bool canClick = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (!activeGame || !canClick) return; 
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Clickable"))
                {
                    Vector3 newPosition = CalculatePosition(hit.point, out int x, out int z);
                    gameManager.PlacePiece(newPosition, x, z);
                    StartCoroutine(ClickCooldown());
                }
            }
        }
    }

    Vector3 CalculatePosition(Vector3 hitPoint, out int x, out int z)
    {
        // Convert hitPoint to board indices
        x = Mathf.RoundToInt(hitPoint.x / 2) + 1; // -2 maps to 0, 0 maps to 1, 2 maps to 2
        z = Mathf.RoundToInt(hitPoint.z / 2) + 1; // -2 maps to 0, 0 maps to 1, 2 maps to 2

        // Clamp values to be between 0 and 2
        x = Mathf.Clamp(x, 0, 2);
        z = Mathf.Clamp(z, 0, 2);

        // Return the new position with Y set to 0
        return new Vector3((x - 1) * 2, 0, (z - 1) * 2);
    }

    IEnumerator ClickCooldown()
    {
        canClick = false; 
        yield return new WaitForSeconds(0.5f); 
        canClick = true;
    }

    public void DeclareWinner(bool playerOne)
    {
        activeGame = false;
        resultsPanel.gameObject.SetActive(true);
        gameMenuPanel.gameObject.SetActive(false);
        resultsText.text = "Player " + (playerOne ? "One" : "Two") + " wins!";
    }

    public void DeclareDraw()
    {
        activeGame = false;
        resultsPanel.gameObject.SetActive(true);
        gameMenuPanel.gameObject.SetActive(false);
        resultsText.text = "It's a draw!";
    }

    public void RestartGame()
    {
        activeGame = true;
        resultsPanel.gameObject.SetActive(false);
        gameMenuPanel.gameObject.SetActive(true);
        gameManager.ResetGame();
    }

    public void BackToMenu()
    {
        RestartGame();
        SceneTransitioner.Instance.TransitionToScene(0);
    }
}