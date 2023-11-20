using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildScript
{
    private const string BUILD_ROOT = "builds/";
    private const string BUILD_SERVER_PATH = BUILD_ROOT + "Server/";
    private const string BUILD_CLIENT_PATH = BUILD_ROOT + "Client/";

    [MenuItem("Builder/DedicatedServer")]
    public static void BuildDedicatedServer()
    {
        BuildPlayerOptions buildOptions = new BuildPlayerOptions();
        buildOptions.locationPathName = BUILD_SERVER_PATH + "server.exe";
        buildOptions.scenes = GetBuildSceneList();
        buildOptions.target = BuildTarget.StandaloneWindows64;
        buildOptions.subtarget = (int)StandaloneBuildSubtarget.Server;
        buildOptions.options = BuildOptions.AutoRunPlayer;

        BuildPipeline.BuildPlayer(buildOptions);
    }

    private static string[] GetBuildSceneList()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        List<string> listScenePath = new();
        for (int i = 0; i < scenes.Length; ++i)
        {
            if (scenes[i].enabled)
            {
                listScenePath.Add(scenes[i].path);
            }
        }

        return listScenePath.ToArray();
    }

    [MenuItem("Builder/Client")]
    public static void BuildClient()
    {
        BuildClientNumber(1);
    }

    [MenuItem("Builder/ServerAndClient")]
    public static void BuildServerAndClient()
    {
        BuildDedicatedServer();
        BuildClientNumber(1);
    }

    [MenuItem("Builder/ServerAnd2Client")]
    public static void BuildServerAnd2Client()
    {
        BuildDedicatedServer();
        BuildClientNumber(1);
        BuildClientNumber(2);
    }

    private static void BuildClientNumber(int number)
    {
        BuildPlayerOptions buildOptions = new BuildPlayerOptions();
        buildOptions.locationPathName = $"{BUILD_CLIENT_PATH}{number}/client{number}.exe";
        buildOptions.scenes = GetBuildSceneList();
        buildOptions.target = BuildTarget.StandaloneWindows64;
        buildOptions.options = BuildOptions.AutoRunPlayer;

        BuildPipeline.BuildPlayer(buildOptions);
    }


}
