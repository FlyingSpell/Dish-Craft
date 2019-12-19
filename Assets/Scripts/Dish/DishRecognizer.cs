using UnityEngine;

public class DishRecognizer : MonoBehaviour
{
	#pragma warning disable 0649

	[SerializeField]
	[Tooltip("Required amount of ingredients to start recognition")]
	private int  	 	ingredientsThreshold;
	[SerializeField]
	private Dish 	 	currentDish;
	[SerializeField]
	private Cookbook 	cookbook;
	[SerializeField]
	private Statistics  statistics;

	#pragma warning restore 0649

	private DishSummary dishSummary;
	private bool 		dishIsReady = false;

	void Start()
	{
		dishSummary = new DishSummary(cookbook);
	}

    void Update()
    {	
    	dishIsReady = (currentDish.ingredientCount == ingredientsThreshold);

        if(dishIsReady)
        {
        	dishSummary.UpdateFrom(currentDish);

        	statistics.UpdateFrom(dishSummary.recipeName, dishSummary.dishProperties);

        	currentDish.Clear();

        	dishIsReady = false;
        }
    }
}
