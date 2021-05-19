using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField]
    GameObject exitPanel;

    public void continueGame()
    {
        if (ProgressSerial.getInstance().checkExist())
        {
            ProgressSerial.getInstance().loadData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            restartGame();
        }
    }

    public void restartGame()
    {
        ProgressSerial.getInstance().restartData();
        ProgressSerial.getInstance().saveData();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void nextLevel()
    {
        GameplayCounter.getInstance().restartCounter();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void backToLobby()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void showDialog()
    {
        if (exitPanel)
        {
            exitPanel.SetActive(true);
        }
    }

    public void onUserClickYesNo(int choice)
    {
        if(choice == 1)
        {
            ProgressSerial.getInstance().saveData();
            Application.Quit();
        }
        else
        {
            exitPanel.SetActive(false);
        }
    }
}
