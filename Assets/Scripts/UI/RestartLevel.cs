using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour, IPointerClickHandler
{
	#pragma warning disable 0649

	[SerializeField]
	private Dish 	   currentDish;
	[SerializeField]
	private Statistics statistics;

	#pragma warning restore 0649

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
        	Restart();
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
	{
		Restart();
	}

	private void Restart()
    {
        ResetScriptableObjects();

        RestartCurrentScene();
    }

    private void RestartCurrentScene()
    {
		Scene activeScene = SceneManager.GetActiveScene();

		SceneManager.LoadScene(activeScene.name);
    }

    private void ResetScriptableObjects()
    {
    	currentDish.Clear();

    	statistics.Reset();
    }
}
