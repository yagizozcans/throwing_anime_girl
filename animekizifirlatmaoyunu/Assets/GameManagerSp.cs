using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerSp : MonoBehaviour
{
    [SerializeField] int gravity;

    public AnimeKizlari[] characters;

    GameObject player;

    void Start()
    {
        Physics.gravity = new Vector3(0,gravity,0);
    }

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
        player = GameObject.FindGameObjectWithTag("Player");
        CreateAnimeKizi();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    void CreateAnimeKizi()
    {
        Destroy(player.GetComponentInChildren<Animator>().gameObject);
        int randomAnimeKizi = Random.Range(0, characters.Length);
        GameObject animekiziIns = Instantiate(characters[randomAnimeKizi].animekizi, player.transform.GetChild(0).transform.localPosition, Quaternion.identity, player.transform);
        animekiziIns.transform.localScale = characters[randomAnimeKizi].animekiziscale * Vector3.one;
        animekiziIns.transform.localEulerAngles = new Vector3(0, 180, 0);
    }

    [System.Serializable]
    public class AnimeKizlari
    {
        public GameObject animekizi;
        public float animekiziscale;
    }
}
