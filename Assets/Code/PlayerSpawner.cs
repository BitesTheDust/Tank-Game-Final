using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
	public class PlayerSpawner : MonoBehaviour 
	{
		[SerializeField] private PlayerUnit _playerPrefab;
		[SerializeField, Range( 0.1f, 100.0f )] private float _waitToSpawn = 1.0f;
		
		private Pool< PlayerUnit > _playerUnit;
		private Vector3 _respawnPosition;
		private Vector3 _respawnRotation;

		private IEnumerator Spawn() 
		{
			yield return new WaitForSeconds( _waitToSpawn );

			PlayerUnit unit = _playerUnit.GetPooledObject();
			if( unit != null && unit.Lives > 0 )
			{
				PlacePlayer( unit, _respawnPosition, _respawnRotation );
			}
		}

		public void Init() 
		{
			_playerUnit = new Pool< PlayerUnit >( 1, false, _playerPrefab, InitPlayer );

			PlayerUnit unit = _playerUnit.GetPooledObject();
			if( unit != null )
			{
				PlacePlayer( unit, Vector3.zero );
			}
		}

		private void PlacePlayer( PlayerUnit unit, Vector3 position, Vector3 rotation = default(Vector3) ) 
		{
			unit.transform.position = position;
			unit.transform.localEulerAngles = rotation;
			GameManager.Instance.AddUnit( unit );

			Debug.Log( "Spawned Player");
		}

		private void InitPlayer( PlayerUnit unit )
		{
			unit.SpawnerInit( PlayerDestroyed );
		}

		private void PlayerDestroyed( PlayerUnit unit )
		{
			_respawnPosition = unit.transform.position;
			_respawnRotation = unit.transform.localEulerAngles;

			if ( !_playerUnit.ReturnObject( unit ) )
			{
				Debug.LogError( "Could not return the player back to the pool!" );
			}

			StartCoroutine( Spawn() );
		}
	}
} 
