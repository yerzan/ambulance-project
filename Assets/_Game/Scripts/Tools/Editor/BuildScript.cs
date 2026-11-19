using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace LastCall.Editor
{
    /// <summary>
    /// CLI build entry points.
    /// Usage: Unity.exe -batchmode -quit -projectPath . -executeMethod LastCall.Editor.BuildScript.BuildWindows
    /// Optional: -buildOutput &lt;path to exe&gt;
    /// </summary>
    public static class BuildScript
    {
        private const string DefaultWindowsOutput = "Builds/Windows/LastCall.exe";

        [MenuItem("LastCall/Build/Windows (x64)")]
        public static void BuildWindows()
        {
            var output = GetArg("-buildOutput") ?? DefaultWindowsOutput;
            var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
            if (scenes.Length == 0)
            {
                Fail("No enabled scenes in Build Settings.");
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(output) ?? ".");
            var options = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = output,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None,
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            BuildSummary summary = report.summary;
            Debug.Log($"[BuildScript] Result={summary.result} Errors={summary.totalErrors} Warnings={summary.totalWarnings} " +
                      $"Size={summary.totalSize / (1024 * 1024)}MB Time={summary.totalTime} Output={output}");

            if (summary.result != BuildResult.Succeeded)
            {
                Fail($"Build failed: {summary.result}");
            }
        }

        private static string GetArg(string name)
        {
            var args = Environment.GetCommandLineArgs();
            for (var i = 0; i < args.Length - 1; i++)
            {
                if (string.Equals(args[i], name, StringComparison.OrdinalIgnoreCase))
                {
                    return args[i + 1];
                }
            }
            return null;
        }

        private static void Fail(string message)
        {
            Debug.LogError("[BuildScript] " + message);
            if (Application.isBatchMode)
            {
                EditorApplication.Exit(1);
            }
        }
    }
}
