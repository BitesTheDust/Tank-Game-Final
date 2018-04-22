using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0436

namespace TankGame.Collectable 
{
	public class Collectable : MonoBehaviour 
	{
		[SerializeField] private int _pointValue;

		private float _turnSpeed;
		private Rigidbody _rigidbody;
		private System.Action< Collectable > _collisionCallback;

		public Rigidbody Rigidbody
		{
			get
			{
				if ( _rigidbody == null )
				{
					_rigidbody = gameObject.GetOrAddComponent< Rigidbody >();
				}
				return _rigidbody;
			}
		}

		public void Init( float turnSpeed, System.Action< Collectable > collisionCallback ) 
		{
			_turnSpeed = turnSpeed;
			_collisionCallback = collisionCallback;
		}

		private void Turn() 
		{
			Vector3 rotation = transform.localEulerAngles;
			rotation.y += _turnSpeed * Time.deltaTime;
			rotation.z += _turnSpeed / 2 * Time.deltaTime;
			transform.localEulerAngles = rotation;
		}
		
		private void Update() 
		{
			Turn();
		}

		protected void OnTriggerEnter( Collider collider ) 
		{
			_collisionCallback( this );
			GameManager.Instance.ScoringSystem.AddScore( _pointValue );
		}
	}
} 

#pragma warning restore 0436
