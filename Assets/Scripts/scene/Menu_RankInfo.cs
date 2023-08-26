using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_RankInfo : MonoBehaviour
{
    [SerializeField]
    private Sprite[] RankImgs;

    void Start()
    {
        if (!PlayerPrefs.HasKey(this.gameObject.name)) return;
        gameObject.GetComponent<SpriteRenderer>().sprite = RankImgs[PlayerPrefs.GetInt(this.gameObject.name)];
    }
}
