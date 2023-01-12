using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    #region METHODS
    public void PlayGame()
	{
		SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
