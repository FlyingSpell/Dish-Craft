using System;
using Newtonsoft.Json;

[JsonConverter(typeof(IDishPropertyJsonConverter))]
public interface IDishProperty
{
    String 		   name { get; }
	int 		   value { get; }
    IngredientInfo ingredientInfo { get; }
}
