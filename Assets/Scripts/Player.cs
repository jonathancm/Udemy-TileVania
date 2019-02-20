using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	// Configurable Parameters
	[Header("Player Stats")]
	[SerializeField] float movementSpeed = 5f;
	[SerializeField] float jumpSpeed = 5f;
	[SerializeField] float climbSpeed = 3f;
	[SerializeField] Vector2 deathKick = new Vector2(-20f, 20f);

	[Header("Player Warp")]
	[SerializeField] float warpDuration = 2f;
	[SerializeField] GameObject warpVFX = null;
	[Range(0, 1)]
	[SerializeField] float warpVolume = 0.5f;
	[SerializeField] AudioClip warpSFX = null;

	[Header("Player Death")]
	[Range(0, 1)]
	[SerializeField] float deathVolume = 0.3f;
	[SerializeField] AudioClip deathSFX = null;


	// Cached References
	Rigidbody2D rigidBody;
	Animator animator;
	CapsuleCollider2D bodyCollider;
	BoxCollider2D feetCollider;

	// State
	bool isAlive = true;
	bool controlsLocked = false;
	float gravityScaleAtStart;
	int ladderMask;
	int groundMask;
	int enemyMask;
	int hazardsMask;

	void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		bodyCollider = transform.Find("Body").GetComponent<CapsuleCollider2D>();
		feetCollider = transform.Find("Feet").GetComponent<BoxCollider2D>();

		gravityScaleAtStart = rigidBody.gravityScale;
		groundMask = LayerMask.GetMask("Ground");
		ladderMask = LayerMask.GetMask("Climbing");
		enemyMask = LayerMask.GetMask("Enemies");
		hazardsMask = LayerMask.GetMask("Hazards");
	}


	void Update()
	{
		float inputAxisX = Input.GetAxis("Horizontal");
		float inputAxisY = Input.GetAxis("Vertical");

		if(!isAlive)
			return;

		if(controlsLocked)
			return;

		Move(inputAxisX);
		ClimbLadder(inputAxisY);
		Jump();
		UpdateSpriteState(inputAxisX);
		Die();
	}

	public void SetControlLocking(bool lockEnable)
	{
		controlsLocked = lockEnable;
	}

	private void Move(float inputAxisX)
	{
		// Shield Statement
		if(rigidBody == null)
			return;

		Vector2 newVelocity = rigidBody.velocity;
		newVelocity.x = inputAxisX * movementSpeed;
		rigidBody.velocity = newVelocity;
	}

	private void Jump()
	{
		//if(!feetCollider.IsTouchingLayers(groundMask | ladderMask))
		if(!feetCollider.IsTouchingLayers(groundMask))
			return;

		if(!Input.GetButtonDown("Jump"))
			return;

		Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
		rigidBody.velocity += jumpVelocityToAdd;
	}

	private void ClimbLadder(float inputAxisY)
	{
		bool playerWantsToMove_AxisY = Mathf.Abs(inputAxisY) > Mathf.Epsilon;

		if(!feetCollider.IsTouchingLayers(ladderMask))
		{
			rigidBody.gravityScale = gravityScaleAtStart;
			animator.speed = 1;
			animator.SetBool("isClimbing", false);
			return;
		}

		// Set Movement
		Vector2 newVelocity = rigidBody.velocity;
		newVelocity.y = inputAxisY * climbSpeed;
		rigidBody.velocity = newVelocity;

		if(playerWantsToMove_AxisY)
		{
			// Moving up/down ladder
			rigidBody.gravityScale = 0f;
			animator.speed = 1;
			animator.SetBool("isClimbing", true);
			
		}
		else
		{
			// Stopped in ladder
			animator.speed = 0;
		}
	}


	private void UpdateSpriteState(float inputAxisX)
	{
		bool playerWantsToMove_AxisX = Mathf.Abs(inputAxisX) > Mathf.Epsilon;

		if(playerWantsToMove_AxisX)
		{
			FlipSprite(inputAxisX);
			animator.SetBool("isRunning", true);
		}
		else
		{
			animator.SetBool("isRunning", false);
		}
	}


	private void FlipSprite(float inputAxisX)
	{
		float scaleX, scaleY;

		scaleX = Mathf.Abs(transform.localScale.x);
		scaleY = transform.localScale.y;

		transform.localScale = new Vector2(Mathf.Sign(inputAxisX) * scaleX, scaleY);
	}

	private void Die()
	{
		if(!isAlive)
			return;

		if(!bodyCollider.IsTouchingLayers(enemyMask | hazardsMask))
			return;

		isAlive = false;
		gameObject.layer = LayerMask.NameToLayer("Non Interactables");
		animator.SetBool("isDead", true);
		PlayDeathSFX();

		// Generate Death Kick
		rigidBody.AddForce(deathKick, ForceMode2D.Impulse);

		bodyCollider.enabled = false;
		FindObjectOfType<GameSession>().ProcessPlayerDeath();
	}

	private void PlayDeathSFX()
	{
		var sfxPlayer = FindObjectOfType<SoundEffectPlayer>();
		if(sfxPlayer)
			sfxPlayer.PlaySoundEffect(deathSFX, deathVolume);
	}

	public void Warp()
	{
		PlayWarpSFX();
		PlayWarpVFX();
	}

	private void PlayWarpSFX()
	{
		var sfxPlayer = FindObjectOfType<SoundEffectPlayer>();

		if(sfxPlayer)
			sfxPlayer.PlaySoundEffect(warpSFX, warpVolume);
	}

	private void PlayWarpVFX()
	{
		GameObject warpVFXObject;

		if(!warpVFX)
			return;

		warpVFXObject = Instantiate(warpVFX, transform.position, transform.rotation);
		warpVFXObject.transform.parent = gameObject.transform;
		Destroy(warpVFXObject, warpDuration);
	}
}
