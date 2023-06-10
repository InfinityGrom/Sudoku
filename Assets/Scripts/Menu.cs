using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LoadScene(int number)
    {
        SceneManager.LoadScene(number);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ClickOnEasy()
    {
        Difficulty.Difficulty_Number = 1;
    }

    public void ClickOnMedium()
    {
        Difficulty.Difficulty_Number = 2;
    }

    public void ClickOnHard()
    {
        Difficulty.Difficulty_Number = 3;
    }
}
