using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    [SerializeField]
    Animator animator;
    [SerializeField]
    Color initColor, endColor;
    [SerializeField]
    SpriteRenderer sprite;
    int life;
    public TextMesh lifeText;

    public int Life 
    {
        get { return this.life; }
        set { this.life = value; }
    }
	// Use this for initialization
    void Start()
    {
        lifeText.text = (life).ToString();
        SetColor();
	}

    void Update()
    {
        if (life <= 0)
        {
            // SOM DE DESTRUIR
            this.animator.SetTrigger("Destroy");
        }
    }
	

    public void GetDamaged()
    {
        life -= 1;
        if (life >= 0)
        {
            lifeText.text = (life).ToString();
            // SOM DE BATER
        }
        if(life <= 0)
            this.GetComponent<BoxCollider2D>().enabled = false;
        else
            this.animator.SetTrigger("Hit");
        SetColor();
    }

    void SetColor()
    {
        this.sprite.color = Color.Lerp(this.endColor, this.initColor, (50 - (float)this.life) / 60);
    }
}
