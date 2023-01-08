using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private GameObject _slide1;
    [SerializeField] private GameObject _slide2;
    [SerializeField] private GameObject _buttonNext;
    [SerializeField] private GameObject _buttonPrev;
    #endregion

    #region METHODS
    public void PlayGame()
	{
		SceneManager.LoadScene("Main");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Slide1()
    {
        _buttonNext.SetActive(true);
        _buttonPrev.SetActive(false);
        _slide1.SetActive(true);
        _slide2.SetActive(false);
    }

    public void Slide2()
    {
        _buttonNext.SetActive(false);
        _buttonPrev.SetActive(true);
        _slide1.SetActive(false);
        _slide2.SetActive(true);
    }
    #endregion
}
