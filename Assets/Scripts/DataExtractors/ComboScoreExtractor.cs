using System.Collections.Generic;

public class ComboScoreExtractor : DishPropertyDataExtractor<int>
{
	protected override int ExtractData(IReadOnlyCollection<IDishProperty> preparedDishProperties)
	{
		int score = 0;

		foreach(IDishProperty dishProperty in preparedDishProperties)
		{
			float multiplier = GetMultiplierFor(dishProperty);

			score += (int)(dishProperty.value * dishProperty.ingredientInfo.score * multiplier);
		}

		return score;
	}

	private float GetMultiplierFor(IDishProperty dishProperty)
	{
		int ingredientCount = dishProperty.value;

		switch(ingredientCount)
		{
			case 2:
			{
				return 2f;
			}
			case 3:
			{
				return 1.5f;
			}
			case 4:
			{
				return 1.25f;
			}
			default:
			{
				return 1f;
			}
		}
	}
}