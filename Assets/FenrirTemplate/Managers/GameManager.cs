using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fenrir.Actors;
using Fenrir.EventBehaviour;

namespace Fenrir.Managers
{
    [RequireComponent(typeof(DataManager))]
    public class GameManager : EventBehaviour<GameManager>
    {
        public Runtime runtime;

        private void Start()
        {
            FirstLoad();
        }

        private void FirstLoad()
        {
            runtime.currentLevelIndex = PlayerPrefs.GetInt("level",0);
            LoadLevel();
        }

        private void LoadLevel()
        {
            if (runtime.currentLevel)
            {
                Destroy(runtime.currentLevel.gameObject);
            }
            GameObject createdLevel = Instantiate(DataManager.Instance.levelCapsule.LevelPrefab(runtime.currentLevelIndex));
            if (createdLevel.TryGetComponent(out LevelActor levelActor))
            {
                runtime.currentLevel = levelActor;
                levelActor.SetupLevel();
                PushEvent(BaseGameEvents.LevelLoaded);
            }
        }

        public void StartLevel()
        {
            if (!runtime.isGameStarted)
            {
                runtime.isGameStarted = true;
                PushEvent(BaseGameEvents.StartGame);
            }
            else
            {
                throw new System.ApplicationException("Level Already Started");
            }
        }
        public void FinishLevel(bool status)
        {
            runtime.isGameStarted = false;
            runtime.isGameOver = true;
            PushEvent(BaseGameEvents.FinishGame);
            PushEvent(status ? BaseGameEvents.WinGame : BaseGameEvents.LoseGame);

            if (status)
            {
                runtime.currentLevelIndex++;
                PlayerPrefs.SetInt("level", runtime.currentLevelIndex);
            }
        }
        public void NextLevel()
        {
            PushEvent(BaseGameEvents.NextLevel);
            LoadLevel();
            
        }
        public void RestartLevel()
        {
            PushEvent(BaseGameEvents.RestartGame);
            LoadLevel();
        }

        [System.Serializable]
        public struct Runtime
        {
            public bool isGameStarted;
            public bool isGameOver;
            public int currentLevelIndex;

            public LevelActor currentLevel;
        }

    }
}