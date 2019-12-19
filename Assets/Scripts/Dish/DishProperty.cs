using System;

public class DishProperty : IDishProperty
{
	public String 		  name { get; private set; }
    public int            value { get; private set; }
    public IngredientInfo ingredientInfo { get; private set; }
    
    public DishProperty(String name) : this(name, null, 0)
    {
    }

    public DishProperty(String name, int value) : this(name)
    {
        this.value = value;
    }

    public DishProperty(String name, IngredientInfo ingredientInfo, int value = 0)
    {
    	this.name = name;
        this.ingredientInfo = ingredientInfo;
        this.value = value;
    }

    public void IncreaseValue()
    {
    	value++;
    }

    public override String ToString()
	{
        String ingredientInfoString = ingredientInfo?.ToString() ?? name;

		return $"{ingredientInfoString}, value - {value}";
	}
}
