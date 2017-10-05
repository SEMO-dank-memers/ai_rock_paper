using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpriteChange : MonoBehaviour {

    public Button btnRock, btnPaper, btnScissors;
    public Sprite spriteRock, spritePaper, spriteScissors;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == null) spriteRenderer.sprite = spriteRock;
        btnRock.onClick.AddListener(RockyRoad);
        btnPaper.onClick.AddListener(PaperMoon);
        btnScissors.onClick.AddListener(DontRunWithThis);
    }

    void RockyRoad()
    {
        spriteRenderer.sprite = spriteRock;
    }

    void PaperMoon()
    {
        spriteRenderer.sprite = spritePaper;
    }

    void DontRunWithThis()
    {
        spriteRenderer.sprite = spriteScissors;
    }
}