using TankGame.Localization;
using TankGame.Messaging;
using UnityEngine;
using UnityEngine.UI;
using l10n = TankGame.Localization.Localization;

namespace TankGame.UI
{
	public class LivesUI : MonoBehaviour
	{
		[SerializeField] private Text _text;

		private PlayerUnit _unit;

		private const string LivesKey = "lives";

		protected void OnDestroy()
		{
			UnregisterEventListeners();
		}

		public void Init()
		{
			Debug.Log( "Lives UI initialized" );
		}

		public void SetUnit( Unit unit ) 
		{
			l10n.LanguageLoaded += OnLanguageLoaded; 
			_unit = unit as PlayerUnit;
			_unit.LivesLost += OnLivesLost;
			SetText( _unit.Lives );
		}

		private void OnLanguageLoaded( LangCode currentLanguage)
		{
			SetText( _unit.Lives );
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

		private void SetText( int lives )
		{
			string translation = l10n.CurrentLanguage.GetTranslation( LivesKey );

			_text.text = string.Format( translation, lives );
		}
	}
}
