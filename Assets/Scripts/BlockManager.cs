using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour {

    [SerializeField]
    List<int> blocksPerRow, blockHealthMin, blockHealthMax;

    [SerializeField]
    Animator ffAnimator, canvasAnimator;
    [SerializeField]
    Image stopSign;
    public bool autoFastForward, gameOver;

    public GameObject blockPrefab, extraBallPrefab;
    public GameObject[,] gridBlocks;

    public Text textScore, textBalls, bestScore;

    int currentRow, extraDifficulty;

    public enum GameState
    {
        Waiting,
        Shooting,
    }

    public static GameState gameState;

	void Start () 
    {
        if(PlayerPrefs.HasKey("Best"))
            this.bestScore.text = PlayerPrefs.GetInt("Best").ToString();
        gridBlocks = new GameObject[11, 20];
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < gridBlocks.GetLength(0); x++)
            {
                int r = Random.Range(1, 20);
                if(r < 5)
                {
                    GameObject block = Instantiate(blockPrefab, new Vector3(x * 0.5f, y * -0.5f - 1.2f, 0), Quaternion.identity);
                    gridBlocks[x, y] = block;
                    gridBlocks[x, y].GetComponent<Block>().Life = Random.Range(3, 5);
                    gridBlocks[x, y].name = "Pos: " + x + "," + y;
                }

            }
        }

    }
	
	void Update () 
    {
        if (Time.timeScale == 3 && gameState == GameState.Waiting)
        {
            Time.timeScale = 1;
            this.stopSign.enabled = false;
        }

        if (gameState == GameState.Shooting && this.autoFastForward && Time.timeScale != 0)
        {
            Time.timeScale = 3;
            this.stopSign.enabled = true;
        }

        textScore.text = PlayerInfo.score.ToString();
        textBalls.text = PlayerController.bulletsMax.ToString();
	}



    public void SpawnBlocks()
    {
        PlayerInfo.score += 1;
        for (int x = gridBlocks.GetLength(0) - 1; x >= 0; x--)
        {
            for (int y = gridBlocks.GetLength(1) - 2; y >= 0; y--)
            {
                if (gridBlocks[x, y] != null)
                {
                    gridBlocks[x, y].name = "Pos: " + x + "," + (y + 1);
                    gridBlocks[x, y].transform.position = new Vector3(x * 0.5f, (y * -0.5f) -0.5f - 1.2f, 0);
                    gridBlocks[x, y + 1] = gridBlocks[x, y];
                    gridBlocks[x, y] = null;
                    if (y + 1 == 16)
                        GameOver();
                }
            }
        }
        if (!this.gameOver)
        {

            List<int> takenPlaces = new List<int>();
            int r = Random.Range(0, gridBlocks.GetLength(0));
            int rowBlockNum = 0;
            if (this.blocksPerRow.Count > this.currentRow)
                rowBlockNum = this.blocksPerRow[this.currentRow];
            else
                rowBlockNum = this.blocksPerRow[this.blocksPerRow.Count - 1];
            for (int i = 0; i < rowBlockNum + Random.Range(-2, 3); i++)
            {
                while (takenPlaces.Contains(r))
                    r = Random.Range(0, gridBlocks.GetLength(0));
                takenPlaces.Add(r);
                GameObject block = Instantiate(blockPrefab, new Vector3(0.5f * r, 0 - 1.2f, 0), Quaternion.identity);
                gridBlocks[r, 0] = block;
                gridBlocks[r, 0].name = "Pos: " + r + "," + 0;

                int blockHealth = 0;
                if (this.blockHealthMin.Count > this.currentRow)
                    blockHealth = Random.Range(this.blockHealthMin[this.currentRow], this.blockHealthMax[this.currentRow]);
                else
                    blockHealth = Random.Range(Mathf.Clamp(this.blockHealthMin[this.blockHealthMin.Count - 1] + extraDifficulty, 0, 99), Mathf.Clamp(this.blockHealthMax[this.blockHealthMax.Count - 1] + extraDifficulty, 0, 100));

                block.GetComponent<Block>().Life = blockHealth;
            }

            if (PlayerController.bulletsMax + ExtraBulletNum() < 120)
            {
                while (takenPlaces.Contains(r))
                    r = Random.Range(0, gridBlocks.GetLength(0));

                GameObject extraBall = Instantiate(extraBallPrefab, new Vector3(0.5f * r, 0 - 1.2f, 0), Quaternion.identity);
                gridBlocks[r, 0] = extraBall;
                gridBlocks[r, 0].name = "Pos: " + r + "," + 0;
            }

            if (this.blockHealthMin.Count <= this.currentRow)
                extraDifficulty++;
            this.currentRow++;
        }

    }

    public void FastForward()
    {
        if (Time.timeScale != 0)
        {
            this.autoFastForward = !this.autoFastForward;
            if (autoFastForward)
            {
                this.ffAnimator.SetTrigger("Pressed");
            }
            else
            {
                this.ffAnimator.SetTrigger("Normal");
            }

            if (gameState == GameState.Shooting)
            {
                if (Time.timeScale == 1)
                {
                    Time.timeScale = 3;
                    this.stopSign.enabled = true;
                }
                else
                {
                    Time.timeScale = 1;
                    this.stopSign.enabled = false;
                }
            }
        }
    }

    void GameOver()
    {
        PlayerPrefs.SetInt("Best", PlayerInfo.score);
        this.gameOver = true;
        Time.timeScale = 1;
        PlayerController.extraBullets = 0;
        PlayerController.bulletsMax = 2;
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in blocks)
        {
            block.GetComponent<Animator>().ResetTrigger("Hit");
            block.GetComponent<Animator>().ResetTrigger("Destroy");
            block.GetComponent<Animator>().SetTrigger("Disappear");
        }
        GameObject[] extraBalls = GameObject.FindGameObjectsWithTag("ExtraBall");
        foreach (GameObject ball in extraBalls)
        {
            ball.GetComponent<Animator>().SetTrigger("Disappear");
        }
        this.canvasAnimator.SetTrigger("Disappear");
        //SceneManager.LoadScene(2);
    }

    int ExtraBulletNum()
    {
        GameObject[] extraBalls = GameObject.FindGameObjectsWithTag("ExtraBall");
        return extraBalls.Length;
    }
}
