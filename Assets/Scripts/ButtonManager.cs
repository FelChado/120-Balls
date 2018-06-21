using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    [SerializeField]
    Animator pauseAnimator;
    [SerializeField]
    int sceneToChange;

    public void ChangeScene()
    {
        StopAllCoroutines();
        Time.timeScale = 1;
        SceneManager.LoadScene(this.sceneToChange);
    }

    public void Pause()
    {
        if (Time.timeScale == 1)
        {
            this.pauseAnimator.SetTrigger("Pressed");
            Time.timeScale = 0;
        }
        else if(Time.timeScale == 0)
        {
            this.pauseAnimator.SetTrigger("Normal");
            Time.timeScale = 1;
        }
    }

    public void PausePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }
}
