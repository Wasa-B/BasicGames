using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public void MoveGame()
    {
        SceneManager.LoadScene(1);
    }
    public void MoveTitle()
    {
        SceneManager.LoadScene(0);

    }
}
