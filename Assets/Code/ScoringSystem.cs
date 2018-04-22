using System;
using System.Linq.Expressions;
using System.ComponentModel;
using JetBrains.Annotations;
using UnityEngine;

namespace TankGame 
{
    public class ScoringSystem : INotifyPropertyChanged
    {
		private int _currentScore;

		public event Action Victory;

		public event Action< int > ScoreChanged;

		public int CurrentScore
		{
			get { return _currentScore; }
			protected set
			{
				_currentScore = value;
				// ScoreChanged event is raised every time CurrentScore is changed
				if ( ScoreChanged != null )
				{
					ScoreChanged( _currentScore );
				}
				OnPropertyChanged( () => CurrentScore );
			}
		}

		public ScoringSystem()
		{
			CurrentScore = 0;
		}

		/// <summary>
		/// Increases score.
		/// </summary>
		/// <param name="score">Amount of points</param>
		/// <returns>True, if the unit dies. False otherwise</returns>
		public virtual bool AddScore( int score )
		{
			CurrentScore += score;
			bool win = CurrentScore >= GameManager.WinningScore;
			if ( win )
			{
				RaiseVictoryEvent();
			}
			return win;
		}

		protected void RaiseVictoryEvent()
		{
			if ( Victory != null )
			{
				Victory();
			}
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
