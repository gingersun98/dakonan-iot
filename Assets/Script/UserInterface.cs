using EasyTransition;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    public TransitionSettings transition;
    public void TravelScene(int sceneIndex)
    {
        Time.timeScale = 1f;
        TransitionManager.Instance().Transition(sceneIndex, transition, 0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
