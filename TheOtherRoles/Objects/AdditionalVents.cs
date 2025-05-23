using System.Collections.Generic;
using System.Linq;
using TheOtherRoles.CustomGameModes;
using TheOtherRoles.Utilities;
using UnityEngine;

namespace TheOtherRoles
{

    public class AdditionalVents
    {
        public Vent vent;
        public static System.Collections.Generic.List<AdditionalVents> AllVents = new();
        public static bool flag = false;
        public AdditionalVents(Vector3 p)
        {
            // Create the vent
            var referenceVent = UnityEngine.Object.FindObjectOfType<Vent>();
            vent = UnityEngine.Object.Instantiate<Vent>(referenceVent);
            vent.transform.position = p;
            vent.Left = null;
            vent.Right = null;
            vent.Center = null;
            Vent tmp = MapUtilities.CachedShipStatus.AllVents[0];
            vent.EnterVentAnim = tmp.EnterVentAnim;
            vent.ExitVentAnim = tmp.ExitVentAnim;
            vent.Offset = new Vector3(0f, 0.25f, 0f);
            vent.Id = MapUtilities.CachedShipStatus.AllVents.Select(x => x.Id).Max() + 1; // Make sure we have a unique id
            var allVentsList = MapUtilities.CachedShipStatus.AllVents.ToList();
            allVentsList.Add(vent);
            MapUtilities.CachedShipStatus.AllVents = allVentsList.ToArray();
            vent.gameObject.SetActive(true);
            vent.name = "AdditionalVent_" + vent.Id;
            AllVents.Add(this);
        }

        public static void AddAdditionalVents()
        {
            if (flag) return;
            flag = true;
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started ||
                HideNSeek.isHideNSeekGM || GameOptionsManager.Instance.currentGameOptions.GameMode == AmongUs.GameOptions.GameModes.HideNSeek) return;

            // Polus Vents
            if (GameOptionsManager.Instance.currentNormalGameOptions.MapId == 2 && CustomOptionHolder.additionalVents.getBool())
            {
                AdditionalVents vents1 = new(new Vector3(36.54f, -21.77f, PlayerControl.LocalPlayer.transform.position.z + 1f)); // Specimen
                AdditionalVents vents2 = new(new Vector3(16.64f, -2.46f, PlayerControl.LocalPlayer.transform.position.z + 1f)); // InitialSpawn
                AdditionalVents vents3 = new(new Vector3(26.67f, -17.54f, PlayerControl.LocalPlayer.transform.position.z + 1f)); // Vital
                vents1.vent.Left = vents3.vent; // Specimen - Vital
                vents2.vent.Center = vents3.vent; // InitialSpawn - Vital
                vents3.vent.Right = vents1.vent; // Vital - Specimen
                vents3.vent.Left = vents2.vent; // Vital - InitialSpawn
            }
        }

        public static void clearAndReload()
        {
            flag = false;
            AllVents = new List<AdditionalVents>();
        }
    }
}
