using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBall : MonoBehaviour
{
    PlayerController controller;

    void Start()
    {
        controller = GameObject.Find("PlayerPos").GetComponent<PlayerController>();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            // SOM DE PEGAR BOLA NOVA
            this.controller.NewBulletAnim(this.transform.position);
            PlayerController.extraBullets += 1;
            Destroy(this.gameObject);
        }
    }
}
