using System;
using Newtonsoft.Json;
using UnityEngine;

[JsonConverter(typeof(IngredientInfoJsonConverter))]
public class IngredientInfo
{
	public String name { get; private  set; }
	public String rusName { get; private set; }
	public String spritePath { get; private set; }
	public int 	  score { get; private set; }
	public int 	  rotation { get; private set; }
	public Sprite sprite { get; private set; }

	public IngredientInfo(String name, String rusName, String spritePath, int score, int rotation)
	{
		this.name = name;
		this.rusName = rusName;
		this.spritePath = spritePath;
		this.score = score;
		this.rotation = rotation;

		sprite = Resources.Load<Sprite>(spritePath);
	}

	public override String ToString()
	{
		return $"{name} - {score}";
	}
}
