using System;
using System.Collections.Generic;

public class IngredientStringArrayExtractor : DishPropertyDataExtractor<String[]>
{
	protected override String[] ExtractData(IReadOnlyCollection<IDishProperty> preparedDishProperties)
	{
		String[] ingredientStringArray = new String[preparedDishProperties.Count];

		int i = 0;

		foreach(IDishProperty dishProperty in preparedDishProperties)
		{
			ingredientStringArray[i] = $"{dishProperty.value} {dishProperty.ingredientInfo.rusName}";

			i++;
		}

		return ingredientStringArray;
	}
}
