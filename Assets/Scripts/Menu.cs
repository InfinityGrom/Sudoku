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
}
