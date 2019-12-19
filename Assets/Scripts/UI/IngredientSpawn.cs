using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientSpawn : MonoBehaviour, IPointerDownHandler
{
	#pragma warning disable 649

	[SerializeField]
	private GameObject ingredientPrefab;

	[SerializeField]
	[Tooltip("Game object which will contain all spawned ingredients")]
	private GameObject ingredientParent;

	#pragma warning restore 649

	public void OnPointerDown(PointerEventData pointerEventData)
	{
		SpawnIngredient(pointerEventData.position);
	}

	private void SpawnIngredient(Vector2 position)
	{
		if(ingredientPrefab == null || ingredientParent == null)
		{
			return;
		}

		Instantiate(ingredientPrefab, position, Quaternion.identity, ingredientParent.transform);
	}
}
