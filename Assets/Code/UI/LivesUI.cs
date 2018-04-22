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

		private const string LivesKey = "lives";

		protected void OnDestroy()
		{
			UnregisterEventListeners();
		}

		public void Init()
		{
			Debug.Log( "Lives UI initialized" );
			
			l10n.LanguageLoaded += OnLanguageLoaded;
			// Unit unit 
			// _unit = GameManager.Instance.PlayerUnit as PlayerUnit;
			GameManager.Instance.LivesLost += OnLivesLost;
			SetText( GameManager.Instance.Lives );
		}

		private void OnLanguageLoaded( LangCode currentLanguage)
		{
			SetText( GameManager.Instance.ScoringSystem.CurrentScore );
		}

		private void UnregisterEventListeners()
		{
			l10n.LanguageLoaded -= OnLanguageLoaded;
			GameManager.Instance.LivesLost -= OnLivesLost;
		}

		private void OnLivesLost( int lives )
		{
			SetText( lives );
		}

		private void SetText( int lives )
		{
			string translation = l10n.CurrentLanguage.GetTranslation( LivesKey );

			_text.text = string.Format( translation, lives );
		}
	}
}
