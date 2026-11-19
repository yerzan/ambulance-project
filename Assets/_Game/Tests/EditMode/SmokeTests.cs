using System.Linq;
using LastCall.Core;
using NUnit.Framework;
using UnityEditor;

namespace LastCall.Tests.EditMode
{
    /// <summary>Step 0 smoke tests: the test pipeline runs and core project invariants hold.</summary>
    public class SmokeTests
    {
        [Test]
        public void MedicCountRange_MatchesGdd()
        {
            Assert.AreEqual(1, GameInfo.MinMedics);
            Assert.AreEqual(2, GameInfo.MinCoopMedics);
            Assert.AreEqual(4, GameInfo.MaxMedics);
        }

        [Test]
        public void PlayerControlledPatient_IsDisabledInFirstRelease()
        {
            Assert.IsFalse(GameInfo.PlayerControlledPatientEnabled);
        }

        [Test]
        public void BootScene_IsFirstEnabledSceneInBuildSettings()
        {
            var first = EditorBuildSettings.scenes.FirstOrDefault(s => s.enabled);
            Assert.IsNotNull(first, "No enabled scenes in Build Settings.");
            Assert.AreEqual("Assets/_Game/Scenes/Boot/Boot.unity", first.path);
        }
    }
}
