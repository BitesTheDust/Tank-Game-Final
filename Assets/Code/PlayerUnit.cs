using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using JetBrains.Annotations;
using UnityEngine;

namespace TankGame
{
	public class PlayerUnit : Unit 
	{
		[SerializeField]
		private string _horizontalAxis = "Horizontal";
		[SerializeField]
		[Tooltip("The name of the vertical axis")]
		private string _verticalAxis = "Vertical";

		private System.Action< PlayerUnit > _deathCallback;

		protected override void Update()
		{
			var input = ReadInput();
			Mover.Turn( input.x );
			Mover.Move( input.z );

			// TODO: Refactor me! Extract method.
			bool shoot = Input.GetButton( "Fire1" );
			if ( shoot )
			{
				Weapon.Shoot();
			}
		}

		private Vector3 ReadInput()
		{
			float movement = Input.GetAxis( _verticalAxis );
			float turn = Input.GetAxis( _horizontalAxis );
			return new Vector3(turn, 0, movement);
		}

		public void SpawnerInit( System.Action< PlayerUnit > deathCallback ) 
		{
			_deathCallback = deathCallback;
		}

		protected override void HandleUnitDied(Unit unit) 
		{
			base.HandleUnitDied( unit );
			_deathCallback( this );
		}
	}
}
