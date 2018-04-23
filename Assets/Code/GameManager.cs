using System.Collections.Generic;
using System.IO;
using System.Linq;
using TankGame.Localization;
using TankGame.Messaging;
using TankGame.Persistence;
using TankGame.Collectable;
using UnityEngine;
using L10n = TankGame.Localization.Localization;
using waypoints = TankGame.WaypointSystem;
using System;

namespace TankGame
{
	public class GameManager : MonoBehaviour
	{
		#region Statics

		private static GameManager _instance;

		public static GameManager Instance
		{
			get
			{
				if ( _instance == null && !IsClosing )
				{
					GameObject gameManagerObject = new GameObject( typeof( GameManager ).Name );
					_instance = gameManagerObject.AddComponent< GameManager >();
				}
				return _instance;
			}
		}

		public static bool IsClosing { get; private set; }

		public static int WinningScore { get; private set; } 

		#endregion

		[SerializeField] private int _winningScore = 100;

		private List< waypoints.Path > _paths;
		public List< waypoints.Path > Paths { get { return _paths; } }

		private List< Unit > _enemyUnit = new List< Unit >();
		private Unit _playerUnit = null;
		public Unit PlayerUnit { get { return _playerUnit; } }

		private SaveSystem _saveSystem;

		public string SavePath
		{
			get { return Path.Combine( Application.persistentDataPath, "save" ); }
		}

		public MessageBus MessageBus { get; private set; }

		public ScoringSystem ScoringSystem { get; private set; }

		private CollectableSpawner _collectableSpawner;

		private PlayerSpawner _playerSpawner;

		private EnemySpawner _enemySpawner;

		protected void Awake()
		{
			if ( _instance == null )
			{
				_instance = this;
			}
			else if ( _instance != this )
			{
				Destroy( gameObject );
				return;
			}

			Init();
		}

		private void OnApplicationQuit()
		{
			IsClosing = true;
		}

		private void OnDestroy()
		{
			L10n.LanguageLoaded -= OnLanguageLoaded;
		}

		private void Init()
		{
			InitLocalization();

			IsClosing = false;

			WinningScore = _winningScore;

			MessageBus = new MessageBus();

			ScoringSystem = new ScoringSystem();

			var UI = FindObjectOfType< UI.UI >();
			UI.Init();

			_paths = new List< waypoints.Path >();
			waypoints.Path[] paths = FindObjectsOfType< waypoints.Path >();
			foreach( waypoints.Path path in paths ) 
			{
				_paths.Add( path );
			}

			Unit[] allUnits = FindObjectsOfType< Unit >();
			foreach ( Unit unit in allUnits )
			{
				AddUnit( unit );
			}

			_playerSpawner = FindObjectOfType< PlayerSpawner >();
			_playerSpawner.Init();

			_collectableSpawner = FindObjectOfType< CollectableSpawner >();
			_collectableSpawner.Init();

			_enemySpawner = FindObjectOfType< EnemySpawner >();
			_enemySpawner.Init();

			_saveSystem = new SaveSystem( new BinaryPersitence( SavePath ) );
		}

		private const string LanguageKey = "Language";

		private void InitLocalization()
		{
			LangCode currentLang =
				(LangCode) PlayerPrefs.GetInt( LanguageKey, (int) LangCode.EN );
			L10n.LoadLanguage( currentLang );
			L10n.LanguageLoaded += OnLanguageLoaded;
		}

		private void OnLanguageLoaded( LangCode currentLanguage )
		{
			PlayerPrefs.SetInt( LanguageKey,
				(int) currentLanguage );
		}

		protected void Update()
		{
			bool save = Input.GetKeyDown( KeyCode.F2 );
			bool load = Input.GetKeyDown( KeyCode.F3 );

			if ( save )
			{
				Save();
			}
			else if ( load )
			{
				Load();
			}
		}

		public void AddUnit( Unit unit )
		{
			unit.Init();

			if ( unit is EnemyUnit && !_enemyUnit.Contains( unit ) )
			{
				_enemyUnit.Add( unit );
			}
			// Adding a player unit after the initialization really makes no sense because
			// we can have a reference to only one player unit. Be carefull with this
			else if ( unit is PlayerUnit )
			{
				_playerUnit = unit;
				UI.UI.Current.LivesUI.SetUnit( unit );
			}

			// Add unit's health to the UI.
			UI.UI.Current.HealthUI.AddUnit( unit );
		}

		public void Save()
		{
			GameData data = new GameData();
			foreach ( Unit unit in _enemyUnit )
			{
				data.EnemyDatas.Add( unit.GetUnitData() );
			}
			data.PlayerData = _playerUnit.GetUnitData();

			_saveSystem.Save( data );
		}

		public void Load()
		{
			GameData data = _saveSystem.Load();
			foreach ( UnitData enemyData in data.EnemyDatas )
			{
				Unit enemy = _enemyUnit.FirstOrDefault( unit => unit.Id == enemyData.Id );
				if ( enemy != null )
				{
					enemy.SetUnitData( enemyData );
				}
			}

			_playerUnit.SetUnitData( data.PlayerData );
		}

		protected void HandleLivesLost( Unit unit ) 
		{
			
		}
	}
}
