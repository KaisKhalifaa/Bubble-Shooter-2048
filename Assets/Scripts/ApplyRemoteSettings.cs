using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig; // supposé hedha fih class ConfigManager
using Unity.Services.RemoteConfig;

public class ApplyRemoteSettings : MonoBehaviour
{
public int NumberOfStartingRows, MaximumSpawnedBallTag, MinimumSpawnedBallTag, NumberOfShotsBeforeNextRow;

    public struct userAttributes { }
    public struct appAttributes { }

    void Awake()
    {
        //FetchRemoteConfig();
    }

    void FetchRemoteConfig()
    {
        //ConfigManager.FetchCompleted += SetSettings; // l classe ConfigManager mahouch ya9ra feha mawjouda f namespace
        //ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    }

    void SetSettings(ConfigResponse response)
    {
        // if (response.status == ConfigRequestStatus.Success)
        // {
        //     NumberOfStartingRows = ConfigManager.appConfig.GetInt("NumberOfStartingRows");
        //     MaximumSpawnedBallTag = ConfigManager.appConfig.GetInt("MaximumSpawnedBallTag");
        //     MinimumSpawnedBallTag = ConfigManager.appConfig.GetInt("MinimumSpawnedBallTag");
        //     NumberOfShotsBeforeNextRow = ConfigManager.appConfig.GetInt("NumberOfShotsBeforeNextRow");
        // }
        BallSpawner.Instance.StartingNumberOfRows = NumberOfStartingRows;
        ObjectPools.Instance.MaximumSpawnedBallTag = MaximumSpawnedBallTag;
        ObjectPools.Instance.MinimumSpawnedBallTag = MinimumSpawnedBallTag;
        BallInteractionManager.Instance.NumberOfShotsBeforeNextRow = NumberOfShotsBeforeNextRow;
    }
}
