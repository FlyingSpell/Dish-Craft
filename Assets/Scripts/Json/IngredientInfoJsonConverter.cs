using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class IngredientInfoJsonConverter : JsonConverter
{
  	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
    	if(reader.TokenType != JsonToken.StartObject)
    	{
    		throw new JsonReaderException($"Expected token 'StartObject', got '{reader.TokenType}' with value '{reader.Value}'");
    	}

    	JObject jobject = JObject.Load(reader);

    	String name = jobject.GetValue("Name")?.ToObject<String>() ?? String.Empty;
    	String rusName  = jobject.GetValue("RusName")?.ToObject<String>() ?? String.Empty;
		String spritePath = jobject.GetValue("SpritePath")?.ToObject<String>() ?? String.Empty;
		int score  = jobject.GetValue("Score")?.ToObject<int>() ?? 0;
		int rotation  = jobject.GetValue("Rotation")?.ToObject<int>() ?? 0;

    	return new IngredientInfo(name, rusName, spritePath, score, rotation);
    }

  	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        IngredientInfo ingredientInfo = value as IngredientInfo;

        JObject jobject = new JObject();

        jobject.Add("Name", new JValue(ingredientInfo.name));
        jobject.Add("RusName", new JValue(ingredientInfo.rusName));
        jobject.Add("Score", new JValue(ingredientInfo.score));
        jobject.Add("SpritePath", new JValue(ingredientInfo.spritePath));

        bool needToWriteRotation = (ingredientInfo.rotation != 0);

        if(needToWriteRotation)
        {
        	jobject.Add("Rotation", new JValue(ingredientInfo.rotation));
        }

        jobject.WriteTo(writer);
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(IngredientInfo).IsAssignableFrom(objectType);
    }
}
