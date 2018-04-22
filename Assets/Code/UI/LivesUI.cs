using TankGame.Localization;
using TankGame.Messaging;
using UnityEngine;
using UnityEngine.UI;
using l10n = TankGame.Localization.Localization;

namespace TankGame.UI
{
	public class LivesUI : MonoBehaviour
	{
		[SerializeField]
		private Text _text;

		private PlayerUnit _unit;

		private const string LivesKey = "lives";

		protected void OnDestroy()
		{
			UnregisterEventListeners();
		}

		public void Init()
		{
			Debug.Log( "Lives UI initialized" );
			
			l10n.LanguageLoaded += OnLanguageLoaded;
			_unit = GameManager.Instance.PlayerUnit as PlayerUnit;
			_unit.LivesLost += OnLivesLost;
			SetText( _unit.Lives );
		}

		private void OnLanguageLoaded( LangCode currentLanguage)
		{
			SetText( GameManager.Instance.ScoringSystem.CurrentScore );
		}

		private void UnregisterEventListeners()
		{
			l10n.LanguageLoaded -= OnLanguageLoaded;
			_unit.LivesLost -= OnLivesLost;
		}

		private void OnLivesLost( int lives )
		{
			SetText( lives );
		}

		private void SetText( int score )
		{
			string translation = l10n.CurrentLanguage.GetTranslation( ScoreKey );

			_text.text = string.Format( translation, score );
		}
	}
}
