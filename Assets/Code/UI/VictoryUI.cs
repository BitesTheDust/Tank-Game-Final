using TankGame.Localization;
using TankGame.Messaging;
using UnityEngine;
using UnityEngine.UI;
using l10n = TankGame.Localization.Localization;

namespace TankGame.UI
{
	public class VictoryUI : MonoBehaviour
	{
		[SerializeField]
		private Text _text;

		private const string VictoryKey = "victory";

		protected void OnDestroy()
		{
			UnregisterEventListeners();
		}

		public void Init()
		{
			Debug.Log( "Victory UI initialized" );
			
			l10n.LanguageLoaded += OnLanguageLoaded;
			GameManager.Instance.ScoringSystem.Victory += OnVictory;
		}

		private void OnLanguageLoaded( LangCode currentLanguage)
		{
			//SetText();
		}

		private void UnregisterEventListeners()
		{
			l10n.LanguageLoaded -= OnLanguageLoaded;
			GameManager.Instance.ScoringSystem.Victory -= OnVictory;
			// if ( !GameManager.IsClosing )
			// 	GameManager.Instance.MessageBus.Unsubscribe( _unitDiedSubscription );
			//_unit.Health.UnitDied -= OnUnitDied;
		}

		private void OnVictory()
		{
			SetText();
		}

		private void SetText()
		{
			string translation = l10n.CurrentLanguage.GetTranslation( VictoryKey );

			_text.text = string.Format( translation );
		}
	}
}
