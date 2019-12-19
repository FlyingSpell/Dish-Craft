using System;
using System.Collections.Generic;

public abstract class DishPropertyDataExtractor<T>
{
	private List<IDishProperty> preparedDishPropertyList = new List<IDishProperty>();

	public T ExtractDataFrom(IReadOnlyCollection<IDishProperty> allDishProperties)
	{
		if(allDishProperties == null)
		{
			return default(T);
		}

		preparedDishPropertyList.Clear();	

		AddIngredientPropertiesToList(allDishProperties);

		RemoveValuelessPropertiesFromList();

		return ExtractData(preparedDishPropertyList.AsReadOnly());
	}

	private void AddIngredientPropertiesToList(IReadOnlyCollection<IDishProperty> allDishProperties)
	{
		foreach(IDishProperty dishProperty in allDishProperties)
		{
			if(IsIngredientProperty(dishProperty))
			{
				preparedDishPropertyList.Add(dishProperty);
			}
		}
	}

	private void RemoveValuelessPropertiesFromList()
	{
		Predicate<IDishProperty> valuelessPredicate = (IDishProperty dishProperty) =>
		{
			return dishProperty.value == 0;
		};

		preparedDishPropertyList.RemoveAll(valuelessPredicate);
	}

	private bool IsIngredientProperty(IDishProperty dishProperty)
	{
		return dishProperty.ingredientInfo != null;
	}

	protected abstract T ExtractData(IReadOnlyCollection<IDishProperty> preparedDishProperties);
}
