using UnityEngine;

public class GameSettings : MonoBehaviour
{
	#pragma warning disable 0649

	[SerializeField]
	private Statistics statistics;
	[SerializeField]
	private bool 	   enableScoreCombos;
	[SerializeField]
	private bool 	   allowRestart;
	[SerializeField]
	private bool 	   showDishMiniIcons;
	[SerializeField]
	private bool 	   showLastDish;
	[SerializeField]
	private bool 	   showBestDish;

	#pragma warning restore 0649

	private GameObject restartButtonGameObject;
	private GameObject dishMiniIconsGameObject;

	void Awake()
	{
		restartButtonGameObject = GameObject.Find("RestartButton");

		dishMiniIconsGameObject = GameObject.Find("DishMiniIcons");		  	
	}

	void Start()
	{	
		statistics.scoreExtractor = enableScoreCombos ?
									new ComboScoreExtractor() as DishPropertyDataExtractor<int> :
									new SimpleScoreExtractor() as DishPropertyDataExtractor<int>;


		statistics.MemorizeLastDish(showLastDish);
		statistics.MemorizeBestDish(showBestDish);
		statistics.Reset();

		restartButtonGameObject.SetActive(allowRestart);
		dishMiniIconsGameObject.SetActive(showDishMiniIcons);
	}
}
