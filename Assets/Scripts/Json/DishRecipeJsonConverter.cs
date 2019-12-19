using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DishRecipeJsonConverter : JsonConverter
{
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
    	if(reader.TokenType != JsonToken.StartObject)
    	{
    		throw new JsonReaderException($"Expected token 'StartObject', got '{reader.TokenType}' with value '{reader.Value}'");
    	}

    	JObject jobject = JObject.Load(reader);
    	
    	return MakeDishRecipeFromJObject(jobject);
    }

  	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException("Not implemented yet");
    }

    public override bool CanWrite
    {
        get
        {
        	return false;
        }
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(DishRecipe).IsAssignableFrom(objectType);
    }

    private DishRecipe MakeDishRecipeFromJObject(JObject jobject)
    {
    	String recipeName = jobject.GetValue("Name")?.ToObject<String>() ?? String.Empty;

		DishRecipe dishRecipe = new DishRecipe(recipeName);

		JEnumerable<JToken> propertyTokens = jobject.Children();

		foreach(JToken propertyToken in propertyTokens)
		{
			ParsePropertyValueToken(dishRecipe, propertyToken.First);
		}

		return dishRecipe;
    }

    private void ParsePropertyValueToken(DishRecipe dishRecipe, JToken propertyValueToken)
    {
    	ExtractValuesFromToken(propertyValueToken, out IList<JToken> values, out JTokenType valueType);

		switch(valueType)
		{
			case JTokenType.Integer:
			{
				String propertyName = propertyValueToken.Parent.Path;

				AddIntProperty(dishRecipe, propertyName, values);

				break;
			}
			case JTokenType.Object:
			{
				AddObjectProperty(dishRecipe, values);

				break;
			}
			default:
			{
				return;
			}
		}
    }

    private void ExtractValuesFromToken(JToken propertyValueToken, out IList<JToken> values, out JTokenType valueType)
    {
		switch(propertyValueToken.Type)
		{
			case JTokenType.Integer:
			{
				values = new List<JToken>(1);
				values.Add(propertyValueToken);

				valueType = JTokenType.Integer;
				
				break;
			}
			case JTokenType.Array:
			{
				ExtractValuesFromArray(propertyValueToken, out values, out valueType);

				break;
			}
			default:
			{
				values = null;
				valueType = JTokenType.None;

				break;
			}
		}
    }

    private void ExtractValuesFromArray(JToken arrayToken, out IList<JToken> values, out JTokenType valueType)
    {
    	if(!arrayToken.HasValues)
    	{
    		values = null;
    		valueType = JTokenType.None;

    		return;
    	}

    	valueType = arrayToken.First.Type;

    	values = new List<JToken>();

    	foreach(JToken arrayValueToken in arrayToken.Children())
		{
			values.Add(arrayValueToken);
		}
    }

    private void AddIntProperty(DishRecipe dishRecipe, String propertyName, IList<JToken> intValues)
    {
    	ICondition condition = CreateConditionFromIntValues(intValues);

    	dishRecipe.AddPropertyWithCondition(propertyName, condition);
    }

    private void AddObjectProperty(DishRecipe dishRecipe, IList<JToken> objectValues)
	{
		if(objectValues == null)
		{
			return;
		}

		foreach(JToken objectToken in objectValues)
		{
			JObject jobject = objectToken as JObject;

			String propertyName = jobject.First.First.ToObject<String>();

			JToken valueToken = jobject.Last.First;

			ExtractValuesFromToken(valueToken, out IList<JToken> values, out JTokenType valueType);

			AddIntProperty(dishRecipe, propertyName, values);
		}
	}

	private ICondition CreateConditionFromIntValues(IList<JToken> intValues)
	{
		switch(intValues.Count)
		{
			case 1:
			{
				int value = intValues[0].ToObject<int>();

				return new EqualityCondition(value);
			}
			case 2:
			{
				int leftValue = intValues[0].ToObject<int>();
				int rightValue = intValues[1].ToObject<int>();				

				return new IntervalCondition(leftValue, rightValue);
			}
			default:
			{
				return null;
			}
		}
	} 
}
