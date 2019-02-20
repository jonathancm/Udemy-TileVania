using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	// Configurable Parameters
	[SerializeField] float movementSpeed = 1f;

	// Cached References
	Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
		rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
		Move();
    }

	private void Move()
	{
		Vector2 newVelocity;

		if(IsFacingRight())
		{
			newVelocity = new Vector2(movementSpeed, rigidBody.velocity.y);
		}
		else
		{
			newVelocity = new Vector2( (-1f) * movementSpeed, rigidBody.velocity.y);
		}
		rigidBody.velocity = newVelocity;
	}

	private bool IsFacingRight()
	{
		return transform.localScale.x > 0;
	}

	private void OnTriggerExit2D(Collider2D otherCollider)
	{
		float tempScaleX = transform.localScale.x;
		float tempScaleY = transform.localScale.y;
		float scaleX = (-1f) * Mathf.Abs(tempScaleX) * Mathf.Sign(rigidBody.velocity.x);

		transform.localScale = new Vector2(scaleX, tempScaleY);
	}
}
