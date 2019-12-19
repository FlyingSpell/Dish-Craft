using System;
using UnityEngine;

public class Pan : MonoBehaviour
{
	#pragma warning disable 649

	[SerializeField]
	private Dish 	 currentDish;
	[SerializeField]
	private Cookbook cookbook;

	#pragma warning restore 649

	void OnTriggerEnter2D(Collider2D ingredientCollider2D)
	{
		GameObject ingredientGameObject = ingredientCollider2D.gameObject;

		AddIngredientToDish(ingredientGameObject);

        Destroy(ingredientGameObject);
    }

    private void AddIngredientToDish(GameObject ingredientGameObject)
    {
    	String ingredientName = ExtractNameFromGameObject(ingredientGameObject);

		IngredientInfo ingredientInfo = cookbook.GetIngredientInfo(ingredientName);

        currentDish.AddIngredientInfo(ingredientInfo);
    }

    private String ExtractNameFromGameObject(GameObject gameObject)
    {
    	return gameObject.name.Replace("(Clone)", String.Empty);
    }
}
