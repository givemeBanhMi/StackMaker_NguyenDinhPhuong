using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Level_1;
    public GameObject People;
    public GameObject[] levels;
    public int currentLevel = 0;

    public Transform playerSpawnPoint;
    private void Start()
    {
        UIManager.Instance.OpenUI<CanvasMainMenu>();
        Instantiate(levels[currentLevel], Vector3.zero, Quaternion.identity);
        //playerSpawnPoint = GameObject.Find("PlayerSpawnPoint").transform;
        //LoadMap();
    }
    public void LoadLevel(int levelIndex)
    {
        currentLevel = levelIndex;

        Destroy(GameObject.Find("Level"));
        Instantiate(levels[currentLevel], Vector3.zero, Quaternion.identity);

        playerSpawnPoint = GameObject.Find("PlayerSpawnPoint").transform;

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            player.transform.position = playerSpawnPoint.position;
        }
    }
}
