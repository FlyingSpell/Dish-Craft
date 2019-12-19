using System;
using System.Linq;
using System.Collections.Generic;

public class DishSummary
{
	private String 							 totalIngredientsName = "TotalIngredients";
	private Cookbook 						 cookbook;
	private Dictionary<String, DishProperty> dishPropertyDictionary;
	private List<Tuple<int, String>> 		 matchedRecipeNamesList;

	public DishSummary(Cookbook cookbook)
	{
		if(cookbook == null)
		{
			throw new ArgumentNullException(nameof(cookbook));
		}

		this.cookbook = cookbook;

		InitializeDishPropertyDictionary();

		matchedRecipeNamesList = new List<Tuple<int, String>>(cookbook.recipes.Count);
	}

	public String recipeName
	{
		get
		{
			return matchedRecipeNamesList.FirstOrDefault()?.Item2;
		}
	}

	public IReadOnlyCollection<IDishProperty> dishProperties
	{
		get
		{
			return new List<IDishProperty>(dishPropertyDictionary.Values);
		}
	}

	public IDishProperty GetDishProperty(String name)
	{
		dishPropertyDictionary.TryGetValue(name, out DishProperty dishProperty);

		return dishProperty;
	}

	public void UpdateFrom(Dish dish)
	{
		Reset();

		UpdateDishPropertyFrom(dish);

		AddMissingProperties();

		MatchRecipes();

		SortMatchedRecipes();
	}
	
	public override String ToString()
	{
		String result = "DishSummary:\n";

		foreach(KeyValuePair<String, DishProperty> kvp in dishPropertyDictionary)
        {
        	result += $"{kvp.Key}, {kvp.Value.value}\n";
        }

		return result;
	}

	private void InitializeDishPropertyDictionary()
	{
		int size = cookbook.ingredients.Count + 1;

		dishPropertyDictionary = new Dictionary<String, DishProperty>(size);
	}

	private void AddMissingProperties()
	{
		foreach(IngredientInfo ingredientInfo in cookbook.ingredients)
		{
			if(!dishPropertyDictionary.ContainsKey(ingredientInfo.name))
			{
				DishProperty newDishProperty = new DishProperty(ingredientInfo.name, ingredientInfo);

				dishPropertyDictionary.Add(ingredientInfo.name, newDishProperty);
			}
		}
	}

	private void UpdateDishProperty(String name, IngredientInfo ingredientInfo)
	{
		if(dishPropertyDictionary.ContainsKey(name))
		{
			dishPropertyDictionary[name].IncreaseValue();	
		}
		else
		{
			DishProperty newDishProperty = new DishProperty(ingredientInfo.name, ingredientInfo, value: 1);

			dishPropertyDictionary.Add(name, newDishProperty);
		}
	}

	private void UpdateDishPropertyFrom(Dish dish)
	{
		foreach(IngredientInfo ingredientInfo in dish)
		{
			UpdateDishProperty(ingredientInfo.name, ingredientInfo);
		}

		DishProperty newDishProperty = new DishProperty(totalIngredientsName, dish.ingredientCount);

		dishPropertyDictionary.Add(totalIngredientsName, newDishProperty);
	}

	private void Reset()
	{
		dishPropertyDictionary.Clear();

		matchedRecipeNamesList.Clear();
	}

	private void MatchRecipes()
	{
		foreach(DishRecipe dishRecipe in cookbook.recipes)
		{
			int weight = dishRecipe.Match(this);

			bool recipeMatched = weight > 0;

			if(recipeMatched)
			{
				matchedRecipeNamesList.Add(Tuple.Create(weight, dishRecipe.name));
			}
		}
	}

	private void SortMatchedRecipes()
	{
		Comparison<Tuple<int, String>> comparisonByWeight = (Tuple<int, String> first, Tuple<int, String> second) =>
		{
			return second.Item1.CompareTo(first.Item1);
		};

		matchedRecipeNamesList.Sort(comparisonByWeight);
	}
}
