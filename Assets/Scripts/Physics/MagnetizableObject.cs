using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class MagnetizableObject : MonoBehaviour
{
	private Rigidbody2D rb2D;
	private Vector2 	magnetizePosition;
	private Vector2 	force;
	private float 		forceMultiplier;

	public bool magnetized { get; private set; }

	void Awake()
	{
		rb2D = GetComponent<Rigidbody2D>();

		forceMultiplier = rb2D.gravityScale / 3;
	}

	void FixedUpdate()
	{
		if(magnetized)
		{
			 rb2D.MovePosition(magnetizePosition);
		}
	}

	public void UpdateMagnetizePosition(Vector2 magnetizePosition)
	{
		Vector2 previousPosition = this.magnetizePosition;

		this.magnetizePosition = magnetizePosition;

		CalculateForce(previousPosition, this.magnetizePosition);
	}

	public void SetMagnetized(bool value)
	{
		magnetized = value;

		if(!magnetized)
		{
			rb2D.AddForce(force, ForceMode2D.Impulse);
		}
	}

	private void CalculateForce(Vector2 oldPosition, Vector2 newPosition)
	{
		force = newPosition - oldPosition;

		force *= forceMultiplier;
	}
}
