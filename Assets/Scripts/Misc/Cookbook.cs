using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cookbook", menuName = "Game/Cookbook", order = 51)]
public class Cookbook : ScriptableObject
{
	private Dictionary<String, IngredientInfo> ingredientInfoDictionary = new Dictionary<String, IngredientInfo>();
	private List<DishRecipe> 	 			   dishRecipeList = new List<DishRecipe>();

	public IReadOnlyCollection<IngredientInfo> ingredients
	{
		get
		{
			return ingredientInfoDictionary.Values;
		}
	}

	public IReadOnlyCollection<DishRecipe> recipes
	{
		get
		{
			return dishRecipeList.AsReadOnly();
		}
	}

	public IngredientInfo GetIngredientInfo(String name)
	{
		ingredientInfoDictionary.TryGetValue(name, out IngredientInfo ingredientInfo);

		return ingredientInfo;
	}

	public void InitializeWith(List<IngredientInfo> ingredientInfoList)
	{
		if(ingredientInfoList == null)
		{
			return;
		}

		foreach(IngredientInfo ingredientInfo in ingredientInfoList)
		{
			if(!ingredientInfoDictionary.ContainsKey(ingredientInfo.name))
			{
				ingredientInfoDictionary.Add(ingredientInfo.name, ingredientInfo);
			}
		}
	}

	public void InitializeWith(List<DishRecipe> dishRecipeList)
	{
		if(dishRecipeList == null)
		{
			return;
		}

		this.dishRecipeList.AddRange(dishRecipeList);
	}
}
