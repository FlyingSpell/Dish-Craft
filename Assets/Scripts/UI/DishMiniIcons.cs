using UnityEngine;
using UnityEngine.UI;

public class DishMiniIcons : MonoBehaviour
{
	#pragma warning disable 0649

	[SerializeField]
	private Dish 	   dish;
	[SerializeField]
	private int 	   slotsAmount;
	[SerializeField]
	private GameObject slotPrefab;
	[SerializeField]
	private Sprite 	   emptySlotSprite;
	[SerializeField]
	private int 	   iconsScaleDivider;
	[SerializeField]
	[Range(0, 1)]
	private float 	   iconsTransparency;

	#pragma warning restore 0649

	private GameObject slotsGameObject;

    void Awake()
    {
        slotsGameObject = transform.Find("Slots").gameObject;

		GenerateSlots();

		ClearSlots();
    }

    void OnEnable()
    {
		dish.NewIngredientInfoAdded += OnNewIngredientInfoAdd;

		dish.AllIngredientsCleared += OnAllIngredientsClear;
    }

    void OnDisable()
    {
    	dish.NewIngredientInfoAdded -= OnNewIngredientInfoAdd;

    	dish.AllIngredientsCleared -= OnAllIngredientsClear;
    }

    private void OnNewIngredientInfoAdd(IngredientInfo ingredientInfo)
    {
    	int slotIndex = dish.ingredientCount - 1;

		ApplySpriteToSlot(ingredientInfo.sprite, slotIndex, iconsScaleDivider, iconsTransparency, ingredientInfo.rotation);
    }

    private void OnAllIngredientsClear()
    {
    	ClearSlots();
    }

    private void GenerateSlots()
    {
    	for(int i = 0; i < slotsAmount; i++)
    	{
    		Instantiate(slotPrefab, Vector2.zero, Quaternion.identity, slotsGameObject.transform);
    	}
    }

    private void ClearSlots()
    {
    	for(int i = 0; i < slotsAmount; i++)
    	{
    		ApplySpriteToSlot(emptySlotSprite, i);
    	}
    }

    private void ApplySpriteToSlot(Sprite sprite, int slotIndex, int scaleDivider = 0, float transparency = 1, int rotation = 0)
	{
		RectTransform slotRectTransform = slotsGameObject.transform.GetChild(slotIndex) as RectTransform;

		Image slotImage = slotRectTransform.gameObject.GetComponent<Image>();

		slotImage.sprite = null;

		slotImage.sprite = sprite;

		slotImage.SetNativeSize();

		if(scaleDivider != 0)
		{
			slotRectTransform.sizeDelta /= scaleDivider;
		}

		slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, transparency);

		if(rotation != 0)
		{
			slotRectTransform.Rotate(0.0f, 0.0f, rotation);
		}
		else
		{
			slotRectTransform.rotation = Quaternion.identity;
		}	
	}
}
