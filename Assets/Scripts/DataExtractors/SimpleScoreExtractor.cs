using System.Collections.Generic;

public class SimpleScoreExtractor : DishPropertyDataExtractor<int>
{
	protected override int ExtractData(IReadOnlyCollection<IDishProperty> preparedDishProperties)
	{
		int score = 0;

		foreach(IDishProperty dishProperty in preparedDishProperties)
		{
			score += dishProperty.value * dishProperty.ingredientInfo.score;
		}

		return score;
	}
}
