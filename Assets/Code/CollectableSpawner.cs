using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0436

namespace TankGame.Collectable 
{
	public class CollectableSpawner : MonoBehaviour 
	{
		[SerializeField] private Collectable _collectablePrefab;
		[SerializeField] private float _xAreaLimits = 10.0f;
		[SerializeField] private float _zAreaLimits = 10.0f;
		[SerializeField] private float _distanceFromPlayer = 3.0f;
		[SerializeField] private LayerMask _playerMask;
		[SerializeField, Range( 0.1f, 100.0f )] private float _waitToSpawn = 1.0f;
		[SerializeField] private float _turnSpeed = 3.0f;

		private Pool< Collectable > _collectables;

		private GameManager _owner;

		private IEnumerator Spawn() 
		{
			while( !GameManager.IsClosing ) 
			{
				yield return new WaitForSeconds( _waitToSpawn );

				bool validSpawn = false;
				Vector3 spawnPosition = RandomizePosition();
				Collider[] player = Physics.OverlapSphere( spawnPosition, _distanceFromPlayer, _playerMask ); //, QueryTriggerInteraction.Collide );
				//Debug.DrawLine( )
				if( player == null || player.Length < 1 ) 
				{
					validSpawn = true;
				}
					
				// while( !validSpawn ) 
				// {
				// 	spawnPosition = RandomizePosition();

				// 	Collider[] player = Physics.OverlapSphere( spawnPosition, _distanceFromPlayer, _playerMask );

				// 	if( player == null || player.Length > 0 )
				// 		validSpawn = true;
				// }

				if( validSpawn ) 
				{
					Collectable collectable = _collectables.GetPooledObject();
					if( collectable != null )
					{
						collectable.transform.position = spawnPosition;
						collectable.Rigidbody.isKinematic = true;
						
						Debug.Log( "Spawned Collectible");
					}
				}
			}
		}

		public void Init() 
		{
			_owner = GameManager.Instance;
			_collectables = new Pool< Collectable >( 12, false, _collectablePrefab, InitCollectable );

			StartCoroutine( Spawn() );
		}

		private void InitCollectable( Collectable collectable )
		{
			collectable.Init( _turnSpeed, CollectableCollected );
		}

		private Vector3 RandomizePosition() 
		{
			float x = Random.Range( -_xAreaLimits, _xAreaLimits );
			float z = Random.Range( -_zAreaLimits, _zAreaLimits );

			return new Vector3( x, _collectablePrefab.transform.localPosition.y, z );
		}

		private void CollectableCollected( Collectable collectable )
		{
			if ( !_collectables.ReturnObject( collectable ) )
			{
				Debug.LogError( "Could not return the collectable back to the pool!" );
			}
		}
	}
} 

#pragma warning restore 0436
