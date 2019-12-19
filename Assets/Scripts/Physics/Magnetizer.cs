using UnityEngine;

public class Magnetizer : MonoBehaviour
{
	#pragma warning disable 649

	[SerializeField]
	[Tooltip("Allows objects magnetizing to cursor in given radius")]
	[Range(0, 150)]
	private int 			   magnetizeRadius;
	
	#pragma warning restore 649

	private MagnetizableObject magnetizableObject;

    void Update()
    {
    	bool startMagnetizing = Input.GetMouseButtonDown(0);
    	bool continueMagnetizing = Input.GetMouseButton(0);
    	bool stopMagnetizing = Input.GetMouseButtonUp(0);

        if(startMagnetizing)
        {
			magnetizableObject = FindNearestMagnetizableObject(Input.mousePosition, magnetizeRadius);

			magnetizableObject?.SetMagnetized(true);
        }

        if(continueMagnetizing)
        {
			magnetizableObject?.UpdateMagnetizePosition(Input.mousePosition);
        }

        if(stopMagnetizing)
        {
        	if(magnetizableObject != null)
        	{
        		magnetizableObject.SetMagnetized(false);
        	}
        }
    }

    private MagnetizableObject FindNearestMagnetizableObject(Vector2 point, float radius)
    {
		Collider2D[] overlappedCollider2DArray = Physics2D.OverlapCircleAll(point, radius);

		foreach(Collider2D overlappedCollider2D in overlappedCollider2DArray)
		{
			MagnetizableObject magnetizableObject = overlappedCollider2D.gameObject.GetComponent<MagnetizableObject>();

			if(magnetizableObject != null)
			{
				return magnetizableObject;
			}
		}

		return null;
    }
}
