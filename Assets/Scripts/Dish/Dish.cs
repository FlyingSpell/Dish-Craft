using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dish", menuName = "Game/Dish", order = 51)]
public class Dish : ScriptableObject, IEnumerable<IngredientInfo>
{
	private List<IngredientInfo> 	   ingredientInfoList = new List<IngredientInfo>();

	public delegate void 			   IngredientInfoHandler(IngredientInfo ingredientInfo);
	public event IngredientInfoHandler NewIngredientInfoAdded;
	public event Action 			   AllIngredientsCleared;

	public int ingredientCount
	{
		get
		{
			return ingredientInfoList.Count;
		}
	}

	public void AddIngredientInfo(IngredientInfo ingredientInfo)
	{
		ingredientInfoList.Add(ingredientInfo);

		NewIngredientInfoAdded?.Invoke(ingredientInfo);
	}

	public void Clear()
	{
		ingredientInfoList.Clear();

		AllIngredientsCleared?.Invoke();
	}

	public IEnumerator<IngredientInfo> GetEnumerator()
    {
    	foreach(IngredientInfo ingredientInfo in ingredientInfoList)
    	{
    		yield return ingredientInfo;
    	}
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
