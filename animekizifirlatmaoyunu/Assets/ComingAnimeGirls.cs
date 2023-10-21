using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComingAnimeGirls : MonoBehaviour
{
    //Attach this script to the object you want to move

    //Set this value in the inspector
    public RectTransform targetPosition;

    RectTransform mineRT;

    public float moveSpeed; //You can change this value to change the speed the object moves at

    Vector3 position;

    public float transparentSizer;

    public Sprite[] myAnimeGirlSprites;

    private void Awake()
    {
        mineRT = GetComponent<RectTransform>();
        position = mineRT.anchoredPosition3D;
        GetComponent<Image>().sprite = myAnimeGirlSprites[Random.Range(0, myAnimeGirlSprites.Length)];
        mineRT.sizeDelta = new Vector2(GetComponent<Image>().sprite.texture.width, GetComponent<Image>().sprite.texture.height);
        targetPosition = GameObject.Find("AnimeGirlPosition").GetComponent<RectTransform>();

        if(mineRT.sizeDelta.y < 1000)
        {
            mineRT.sizeDelta = new Vector2(GetComponent<Image>().sprite.texture.width * 2, GetComponent<Image>().sprite.texture.height * 2);
        }
    }

    private void Update()
    {
        position = Vector3.Lerp(position, targetPosition.anchoredPosition3D, 0.1f);

        mineRT.anchoredPosition3D = position;

        float newColor = GetComponent<Image>().color.a -  transparentSizer * Time.deltaTime;

        GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, newColor);

        if(GetComponent<Image>().color.a == 0)
        {
            Destroy(gameObject);
        }
    }
}
