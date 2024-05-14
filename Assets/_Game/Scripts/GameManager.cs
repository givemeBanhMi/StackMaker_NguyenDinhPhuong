using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public GameObject Level_1;
    public GameObject People;
    public GameObject[] levels;
    public int currentLevel = 0;

    private void Start()
    {
        UIManager.Instance.OpenUI<CanvasMainMenu>();
        LoadLevel(0);

    }
    public void LoadLevel(int levelIndex)
    {
        currentLevel = levelIndex;

        Destroy(GameObject.Find("Level"));
        Instantiate(levels[currentLevel], Vector3.zero, Quaternion.identity);


        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            GameObject spawnedPeople = Instantiate(People, player.transform.position, Quaternion.identity);
            spawnedPeople.transform.SetParent(player.transform);
            cameraFollow.Target = spawnedPeople.transform;
            player.transform.position = new Vector3(1.5f, 2.8f, 6.5f);
            //player.transform.position = transform.position;
        }
    }
}