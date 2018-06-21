using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
    [SerializeField]
    SpriteRenderer ballImage;
    [SerializeField]
    GameObject popUp, newBulletPrefab, ring;
    List<GameObject> newBulletList = new List<GameObject>();
    public static float newPositionX;
    public GameObject bullet;
    public static bool firstBulletDestroyed;
    public static int bulletsShot = 0, bulletsMax = 2, bulletsDestroyed = 0, extraBullets;
    Vector2 mousePosition;
    public BlockManager blockManager;

    void Start()
    {
        newPositionX = this.transform.position.x;
        PlayerInfo.score = 0;
        BlockManager.gameState = BlockManager.GameState.Waiting;
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonUp(0) && BlockManager.gameState == BlockManager.GameState.Waiting && Time.timeScale == 1 && !this.blockManager.gameOver)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(mousePosition.y < -0.3f && mousePosition.y >= -9.3f)
                StartCoroutine(Shooting());
        }
        if(bulletsDestroyed >= bulletsMax)
        {
            this.transform.position = new Vector2(newPositionX, this.transform.position.y);
            bulletsMax += extraBullets;
            ExtraBallsNotification();
            extraBullets = 0;
            bulletsShot = 0;
            bulletsDestroyed = 0;
            BlockManager.gameState = BlockManager.GameState.Waiting;
            blockManager.SpawnBlocks();
            firstBulletDestroyed = false;
            this.ballImage.enabled = true;
            ClearBulletList();
        }
        this.transform.GetChild(1).transform.position = new Vector2(newPositionX, this.transform.position.y);
        if(bulletsDestroyed > 0)
            this.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
    }

    void ExtraBallsNotification()
    {
        if (extraBullets > 0)
        {
            GameObject popUp = GameObject.Instantiate(this.popUp, Camera.main.WorldToScreenPoint(this.transform.position), Quaternion.identity, GameObject.Find("Canvas").transform);
            popUp.transform.GetChild(0).GetComponent<Text>().text = "+" + extraBullets;
        }
    }

    void Shoot()
    {
        GameObject b = Instantiate(bullet, gameObject.transform.position, Quaternion.identity);
        bulletsShot++;
        b.GetComponent<Rigidbody2D>().velocity = (mousePosition - (Vector2)transform.position).normalized * 10;
        BlockManager.gameState = BlockManager.GameState.Shooting;
    }

    IEnumerator Shooting()
    {
        this.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        while(bulletsShot < bulletsMax)
        {
            yield return new WaitForSeconds(0.1f);
            // SOM DE ATIRAR BALA
            Shoot();
        }
        this.ballImage.enabled = false;
    }

    public void NewBulletAnim(Vector2 pos)
    {
        this.newBulletList.Add(GameObject.Instantiate(this.newBulletPrefab, pos, Quaternion.identity));
        GameObject.Instantiate(this.ring, pos, Quaternion.identity);
        this.newBulletList[this.newBulletList.Count - 1].GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }

    void ClearBulletList()
    {
        for (int i = 0; i < this.newBulletList.Count; i++)
        {
            GameObject.Destroy(this.newBulletList[i]);
        }
        this.newBulletList = new List<GameObject>();
    }
}
