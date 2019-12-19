using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonDataParser : MonoBehaviour
{
	#pragma warning disable 0649

	[SerializeField]
	private String 	 ingredientsFileName;
	[SerializeField]
	private String 	 recipesFileName;
	[SerializeField]
	private Cookbook cookbook;

	#pragma warning restore 0649

	void Awake()
	{
		List<IngredientInfo> ingredientInfoList = GetDataListFromJsonFile<IngredientInfo>(ingredientsFileName);
		List<DishRecipe> dishRecipeList = GetDataListFromJsonFile<DishRecipe>(recipesFileName);

		cookbook.InitializeWith(ingredientInfoList);
		cookbook.InitializeWith(dishRecipeList);
	}

	private List<T> GetDataListFromJsonFile<T>(String fileName)
	{
		String jsonData = ReadJsonDataFromFile(fileName);

		if(jsonData == null)
		{
			return null;
		}

		return ParseJsonArray<T>(jsonData);
	}

	private List<T> ParseJsonArray<T>(String jsonArray)
	{
		try
		{
			JArray jarray = JArray.Parse(jsonArray);

			return jarray.ToObject<List<T>>();
		}
		catch(JsonReaderException jsonReaderException)
		{
			Debug.Log($"JsonDataParser error: Failed to parse json - {jsonReaderException.Message}");

			return null;
		}
	}

	private String ReadJsonDataFromFile(String fileName)
	{
		if(String.IsNullOrEmpty(fileName))
		{
			Debug.Log("JsonDataParser error: File name is null or empty");

			return null;
		}

		if(!File.Exists(fileName))
		{
			Debug.Log($"JsonDataParser error: File \"{fileName}\" does not exist");

			return null;
		}

		String jsonData = "";

		using(StreamReader streamReader = new StreamReader(fileName))
        {
            while(!streamReader.EndOfStream)
		    {
		        jsonData += streamReader.ReadLine();
		    }
        }

        return jsonData;
	}
}
