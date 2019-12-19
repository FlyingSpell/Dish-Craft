using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class IDishPropertyJsonConverter : JsonConverter
{
  	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
    	if(reader.TokenType != JsonToken.StartObject)
    	{
    		throw new JsonReaderException($"Expected token 'StartObject', got '{reader.TokenType}' with value '{reader.Value}'");
    	}

    	JObject jobject = JObject.Load(reader);

    	String name = jobject.GetValue("Name")?.ToObject<String>() ?? String.Empty;
    	int value  = jobject.GetValue("Value")?.ToObject<int>() ?? 0;
		IngredientInfo ingredientInfo = jobject.GetValue("IngredientInfo")?.ToObject<IngredientInfo>();

    	return new DishProperty(name, ingredientInfo, value);
    }

  	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        IDishProperty dishProperty = value as IDishProperty;

        JObject jobject = new JObject();

        jobject.Add("Name", new JValue(dishProperty.name));
        jobject.Add("Value", new JValue(dishProperty.value));

        bool needToWriteIngredientInfo = (dishProperty.ingredientInfo != null);

        if(needToWriteIngredientInfo)
        {
        	jobject.Add("IngredientInfo", JObject.FromObject(dishProperty.ingredientInfo));
        }

        jobject.WriteTo(writer);
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(IDishProperty).IsAssignableFrom(objectType);
    }
}
