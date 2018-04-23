using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using JetBrains.Annotations;
using UnityEngine;

namespace TankGame
{
	public class PlayerUnit : Unit, INotifyPropertyChanged
	{
		[SerializeField]
		private string _horizontalAxis = "Horizontal";
		[SerializeField]
		[Tooltip("The name of the vertical axis")]
		private string _verticalAxis = "Vertical";

		[SerializeField] private int _lives = 3;

		public int Lives 
		{
			get { return _lives; }
			protected set 
			{
				_lives = value;
				if( Lives < 0 ) 
				{
					_lives = 0;
				}
				if( LivesLost != null ) 
				{
					LivesLost( _lives );
				}
				OnPropertyChanged( () => Lives );
			}
		}

		public event Action< int > LivesLost;

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
			Lives -= 1;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged< T >( Expression< Func< T > > propertyLambda )
		{
			if ( PropertyChanged != null )
			{
				PropertyChanged( this,
					new PropertyChangedEventArgs( Utils.Utils.GetPropertyName( propertyLambda ) ) );
			}
		}
	}
}
