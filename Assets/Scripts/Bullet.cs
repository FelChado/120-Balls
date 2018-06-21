using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
    bool objective;
	
	void Update () 
    {
        if (GetComponent<Rigidbody2D>().velocity.y < 1 && GetComponent<Rigidbody2D>().velocity.y > -1)
        {
            if(GetComponent<Rigidbody2D>().velocity.y > 0)
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 1f);
            else
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -1f);
        }

        if (transform.position.y <= -9.3)
        {
            if (!PlayerController.firstBulletDestroyed)
            {
                PlayerController.newPositionX = this.transform.position.x;
                PlayerController.firstBulletDestroyed = true;
                PlayerController.bulletsDestroyed++;
                Destroy(gameObject);
            }
            else
                this.objective = true;
        }

        if (this.transform.position.x > 5.4f || this.transform.position.x < -0.4f)
        {
            PlayerController.bulletsDestroyed++;
            Destroy(gameObject);
        }

        if (this.objective)
            GoToObjective();
	}

    void GoToObjective()
    {
        if (PlayerController.newPositionX > this.transform.position.x)
            GetComponent<Rigidbody2D>().velocity = new Vector2(10, 0);
        else
            GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 0);

        this.transform.position = new Vector2(this.transform.position.x, -9.3f);

        if ((Mathf.Abs(this.transform.position.x - PlayerController.newPositionX) < 0.2f && Time.timeScale == 1) ||
            (Mathf.Abs(this.transform.position.x - PlayerController.newPositionX) < 0.3f && Time.timeScale != 1))
        {
            PlayerController.bulletsDestroyed++;
            Destroy(gameObject);
        }
    }


    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Block")
        {
            col.gameObject.GetComponent<Block>().GetDamaged();
        }

    }

}
