using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Statistics", menuName = "Game/Statistics", order = 51)]
public class Statistics : ScriptableObject
{
	private Tuple<String, IReadOnlyCollection<IDishProperty>, int> lastDishTuple;
	private Tuple<String, IReadOnlyCollection<IDishProperty>, int> bestDishTuple;

	private int 				   			  	totalScore = 0;
	private bool   				   		  	  	memorizeLastDish = true;
	private bool   				   		  	  	memorizeBestDish = true;
	private static readonly String 			  	scoreText = "Счёт: {0}";
	private static readonly String 			  	lastDishText = "\nПоследнее блюдо: {0}";
	private static readonly String 			  	bestDishText = "\nЛучшее блюдо: {0}";
	private DishPropertyDataExtractor<String[]> ingredientStringArrayExtractor = new IngredientStringArrayExtractor();

	public String 				   		  	  	scoreString { get; private set; } = String.Format(scoreText, 0);
	public DishPropertyDataExtractor<int> 	  	scoreExtractor { get; set; }

	public delegate void 						ScoreStringHandler(String scoreString);
	public event ScoreStringHandler 			ScoreStringChanged;
	public event Action<int> 					ScoreChanged;

	public void UpdateFrom(String recipeName, IReadOnlyCollection<IDishProperty> dishProperties)
	{
		if(scoreExtractor == null)
		{
			return;
		}

		int dishScore = scoreExtractor.ExtractDataFrom(dishProperties);

		if(memorizeLastDish)
		{
			lastDishTuple = Tuple.Create(recipeName, dishProperties, dishScore);
		}

		if(memorizeBestDish)
		{
			int bestDishScore = bestDishTuple?.Item3 ?? 0;

			if(bestDishScore < dishScore)
			{
				bestDishTuple = Tuple.Create(recipeName, dishProperties, dishScore);
			}
		}

		totalScore += dishScore;

		ScoreChanged?.Invoke(totalScore);

		UpdateScoreString();
	}

	public void Reset()
	{
		totalScore = 0;

		lastDishTuple = null;
		bestDishTuple = null;

		UpdateScoreString();
	}

	public void MemorizeLastDish(bool value)
	{
		memorizeLastDish = value;

		UpdateScoreString();
	}

	public void MemorizeBestDish(bool value)
	{
		memorizeBestDish = value;

		UpdateScoreString();
	}

	public void SaveTo(out String jsonString)
	{
		JObject jobject = new JObject();

		jobject.Add("TotalScore", new JValue(totalScore));

		jobject.Add("MemorizeLastDish", new JValue(memorizeLastDish));
		jobject.Add("MemorizeBestDish", new JValue(memorizeBestDish));

		if(memorizeLastDish)
		{
			WriteDishTupleToJsonObject(lastDishTuple, "LastDish", jobject);
		}

		if(memorizeBestDish)
		{
			WriteDishTupleToJsonObject(bestDishTuple, "BestDish", jobject);
		}

		jsonString = jobject.ToString(Formatting.None);
	}

	public void LoadFrom(String jsonString)
	{
		JObject jobject = JObject.Parse(jsonString);

		totalScore = jobject.GetValue("TotalScore").ToObject<int>();

		memorizeLastDish = jobject.GetValue("MemorizeLastDish").ToObject<bool>();
		memorizeBestDish = jobject.GetValue("MemorizeBestDish").ToObject<bool>();

		if(memorizeLastDish)
		{
			ReadDishTupleFromJsonObject(ref lastDishTuple, "LastDish", jobject);
		}

		if(memorizeBestDish)
		{
			ReadDishTupleFromJsonObject(ref bestDishTuple, "BestDish", jobject);
		}

		UpdateScoreString();
	}	

	private void UpdateScoreString()
	{
		scoreString = String.Format(scoreText, totalScore);

		if(memorizeLastDish)
		{
			scoreString += GetDishString(lastDishText, lastDishTuple);
		}

		if(memorizeBestDish)
		{
			scoreString += GetDishString(bestDishText, bestDishTuple);
		}

		ScoreStringChanged?.Invoke(scoreString);
	}

	private String GetDishString(String dishText, Tuple<String, IReadOnlyCollection<IDishProperty>, int> dishTuple)
	{
		if(dishTuple == null)
		{
			return String.Format(dishText, String.Empty);
		}

		String[] ingredientStringArray = ingredientStringArrayExtractor.ExtractDataFrom(dishTuple.Item2);

		String ingredientString = String.Join(", ", ingredientStringArray);

		String dishData = String.Format("{0} ({1}) [{2}]", dishTuple.Item1, ingredientString, dishTuple.Item3);

		return String.Format(dishText, dishData);
	}

	private void WriteDishTupleToJsonObject(Tuple<String, IReadOnlyCollection<IDishProperty>, int> dishTuple, String dishTupleName, JObject jobject)
	{
		JObject dishTupleJObject = new JObject();

		dishTupleJObject.Add("RecipeName", new JValue(dishTuple.Item1));
		dishTupleJObject.Add("DishPropertyCount", new JValue(dishTuple.Item2.Count));

		JArray dishPropertyJArray = new JArray();

		foreach(IDishProperty dishProperty in lastDishTuple.Item2)
		{
			JObject dishPropertyJObject = JObject.FromObject(dishProperty);

			dishPropertyJArray.Add(dishPropertyJObject);
		}

		dishTupleJObject.Add("DishProperties", dishPropertyJArray);
		dishTupleJObject.Add("DishScore", new JValue(dishTuple.Item3));

		jobject.Add(dishTupleName, dishTupleJObject);
	}

	private void ReadDishTupleFromJsonObject(ref Tuple<String, IReadOnlyCollection<IDishProperty>, int> dishTuple, String dishTupleName, JObject jobject)
	{
		JObject dishTupleJObject = jobject.GetValue(dishTupleName).ToObject<JObject>();

		String recipeName = dishTupleJObject.GetValue("RecipeName").ToObject<String>();
		int dishPropertyCount = dishTupleJObject.GetValue("DishPropertyCount").ToObject<int>();
		JArray dishPropertyJArray = dishTupleJObject.GetValue("DishProperties").ToObject<JArray>();

		List<IDishProperty> dishPropertyList = new List<IDishProperty>(dishPropertyCount);

		foreach(JToken dishPropertyToken in dishPropertyJArray.Children())
		{
			IDishProperty dishProperty = dishPropertyToken.ToObject<IDishProperty>();

			dishPropertyList.Add(dishProperty);
		}

		IReadOnlyCollection<IDishProperty> dishProperties = dishPropertyList;

		int dishScore = dishTupleJObject.GetValue("DishScore").ToObject<int>();

		dishTuple = Tuple.Create(recipeName, dishProperties, dishScore);
	}
}
