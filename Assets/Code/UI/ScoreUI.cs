using TankGame.Localization;
using TankGame.Messaging;
using UnityEngine;
using UnityEngine.UI;
using l10n = TankGame.Localization.Localization;

namespace TankGame.UI
{
	public class ScoreUI : MonoBehaviour
	{
		[SerializeField]
		private Text _text;

		private const string ScoreKey = "score";

		protected void OnDestroy()
		{
			UnregisterEventListeners();
		}

		public void Init()
		{
			Debug.Log( "Score UI initialized" );
			
			l10n.LanguageLoaded += OnLanguageLoaded;
			GameManager.Instance.ScoringSystem.ScoreChanged += OnScoreChanged;
			SetText( GameManager.Instance.ScoringSystem.CurrentScore );
		}

		private void OnLanguageLoaded( LangCode currentLanguage)
		{
			SetText( GameManager.Instance.ScoringSystem.CurrentScore );
			// _text.text = 
			// 	Localization.Localization.CurrentLanguage.GetTranslation( _key );
		}

		private void UnregisterEventListeners()
		{
			l10n.LanguageLoaded -= OnLanguageLoaded;
			GameManager.Instance.ScoringSystem.ScoreChanged -= OnScoreChanged;
			// if ( !GameManager.IsClosing )
			// 	GameManager.Instance.MessageBus.Unsubscribe( _unitDiedSubscription );
			//_unit.Health.UnitDied -= OnUnitDied;
		}

		private void OnScoreChanged( int score )
		{
			SetText( score );
		}

		private void SetText( int score )
		{
			string translation = l10n.CurrentLanguage.GetTranslation( ScoreKey );

			_text.text = string.Format( translation, score );
		}
	}
}
