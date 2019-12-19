using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[JsonConverter(typeof(DishRecipeJsonConverter))]
public class DishRecipe
{
	private Dictionary<String, ICondition> propertyConditionDictionary = new Dictionary<String, ICondition>();

	public String name { get; private set; }

	public DishRecipe(String name)
	{
		this.name = name;
	}

	public void AddPropertyWithCondition(String propertyName, ICondition condition)
	{
		if(String.IsNullOrEmpty(propertyName) || condition == null)
		{
			return;
		}

		if(!propertyConditionDictionary.ContainsKey(propertyName))
		{
			propertyConditionDictionary.Add(propertyName, condition);
		}
	}

	public int Match(DishSummary dishSummary)
	{
		int weight = 0;

		foreach(KeyValuePair<String, ICondition> kvp in propertyConditionDictionary)
		{
			String propertyName = kvp.Key;
			ICondition condition = kvp.Value;
 
			IDishProperty dishProperty = dishSummary.GetDishProperty(propertyName);

			bool propertyNotFound = (dishProperty == null);

			if(propertyNotFound)
			{
				return 0;
			}

			bool valueMatched = condition.IsTrue(dishProperty.value);

			if(valueMatched)
			{
				weight += (dishProperty.value > 0) ? dishProperty.value : 1;
			}
			else
			{
				return 0;
			}
		}

		return weight;
	}

	public override String ToString()
	{
		String result = name + "\n";

		foreach(KeyValuePair<String, ICondition> kvp in propertyConditionDictionary)
        {
            result += String.Format("{0} {1}\n", kvp.Key, kvp.Value);
        }

        return result;
	}
}
