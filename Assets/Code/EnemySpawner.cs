using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TankGame.WaypointSystem;

namespace TankGame
{
	public class EnemySpawner : MonoBehaviour 
	{
		[SerializeField] private EnemyUnit _enemyPrefab;
		[SerializeField] private float _xAreaLimits = 10.0f;
		[SerializeField] private float _zAreaLimits = 10.0f;
		[SerializeField] private float _distanceFromPlayer = 10.0f;
		[SerializeField] private LayerMask _playerMask;
		[SerializeField, Range( 0.1f, 100.0f )] private float _waitToSpawn = 1.0f;
		
		private Pool< EnemyUnit > _enemyUnits;

		private IEnumerator Spawn() 
		{
			while( !GameManager.IsClosing ) 
			{
				yield return new WaitForSeconds( _waitToSpawn );

				bool validSpawn = false;
				Vector3 spawnPosition = RandomizePosition();
				Collider[] player = Physics.OverlapSphere( spawnPosition, _distanceFromPlayer, _playerMask ); 

				if( player == null || player.Length < 1 ) 
				{
					validSpawn = true;
				}

				if( validSpawn ) 
				{
					EnemyUnit unit = _enemyUnits.GetPooledObject();
					if( unit != null )
					{
						PlaceEnemy( unit, spawnPosition );
					}
				}
			}
		}

		public void Init() 
		{
			_enemyUnits = new Pool< EnemyUnit >( 8, false, _enemyPrefab, InitEnemy );

			Transform[] spawns = GetComponentsInChildren< Transform >();
			foreach( Transform spawn in spawns ) 
			{
				if( spawn.position == transform.position )
					continue;

				EnemyUnit unit = _enemyUnits.GetPooledObject();
				if( unit != null )
				{
					PlaceEnemy( unit, spawn.position, spawn.localEulerAngles );
				}
			}

			StartCoroutine( Spawn() );
		}

		private void PlaceEnemy( EnemyUnit unit, Vector3 position, Vector3 rotation = default(Vector3) ) 
		{
			unit.transform.position = position;
			unit.transform.localEulerAngles = rotation;
			GameManager.Instance.AddUnit( unit );

			Debug.Log( "Spawned Enemy");
		}

		private void InitEnemy( EnemyUnit unit )
		{
			unit.SpawnerInit( EnemyDestroyed );
		}

		private Vector3 RandomizePosition() 
		{
			float x = Random.Range( -_xAreaLimits, _xAreaLimits );
			float z = Random.Range( -_zAreaLimits, _zAreaLimits );

			return new Vector3( x, _enemyPrefab.transform.localPosition.y, z );
		}

		private void EnemyDestroyed( EnemyUnit unit )
		{
			if ( !_enemyUnits.ReturnObject( unit ) )
			{
				Debug.LogError( "Could not return the enemyunit back to the pool!" );
			}
		}
	}
} 
