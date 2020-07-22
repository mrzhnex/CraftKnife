using EXILED;
using EXILED.Extensions;
using RemoteAdmin;
using System.Linq;
using UnityEngine;

namespace CraftKnife
{
    public class SetEvents
    {
        public void OnCallCommand(ConsoleCommandEvent ev)
        {
            if (!Global.can_use_commands)
            {
                ev.ReturnMessage = "Дождитесь начала раунда!";
                return;
            }
            string command = ev.Command.Split(new char[]
            {
                ' '
            })[0].ToLower();
            if (ev.Command.ToLower().Contains("craft") && ev.Command.ToLower().Contains("knife"))
            {
                if (ev.Player.gameObject.GetComponent<CraftProccess>() != null)
                {
                    ev.ReturnMessage = Global._already_craft;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<KnifeHolder>() != null)
                {
                    ev.ReturnMessage = Global._already_have_knife;
                    return;
                }
                if (ev.Player.inventory.items.Where(x => x.id == ItemType.Flashlight).FirstOrDefault() == default)
                {
                    ev.ReturnMessage = Global._not_have_flashlight;
                    return;
                }
                ev.Player.inventory.items.Remove(ev.Player.inventory.items.Where(x => x.id == ItemType.Flashlight).FirstOrDefault());
                ev.Player.gameObject.AddComponent<CraftProccess>();
                ev.ReturnMessage = Global._success_start + Global.time_to_craft.ToString();
                return;
            }
            if (command == "stab")
            {
                if (ev.Player.gameObject.GetComponent<KnifeHolder>() == null)
                {
                    ev.ReturnMessage = Global._nothaveknife;
                    return;
                }
                ReferenceHub target = null;
                if (Physics.Raycast((ev.Player.gameObject.GetComponent<Scp049PlayerScript>().plyCam.transform.forward * 1.001f) + ev.Player.gameObject.transform.position, ev.Player.gameObject.GetComponent<Scp049PlayerScript>().plyCam.transform.forward, out RaycastHit hit, Global.distance_to_cut))
                {
                    if (hit.transform.GetComponent<QueryProcessor>() == null)
                    {
                        ev.ReturnMessage = Global._toolong;
                        return;
                    }
                    else
                    {
                        target = Player.GetPlayer(hit.transform.gameObject);
                        UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<KnifeHolder>());
                        if (target.GetTeam() == Team.SCP)
                        {
                            ev.ReturnMessage = Global._isscp;
                            return;
                        }
                        if (target.gameObject.GetComponent<BleedOutPlugin.BleedOutComponent>() == null)
                        {
                            target.gameObject.AddComponent<BleedOutPlugin.BleedOutComponent>();
                            target.gameObject.GetComponent<BleedOutPlugin.BleedOutComponent>().SetSettings(BleedOutPlugin.BleedOutType.Normal);
                        }
                        else
                        {
                            target.gameObject.GetComponent<BleedOutPlugin.BleedOutComponent>().SetSettings(BleedOutPlugin.BleedOutType.Major);
                        }
                        target.playerStats.HurtPlayer(new PlayerStats.HitInfo(Global.damage, ev.Player.nicknameSync.Network_myNickSync, DamageTypes.Scp939, ev.Player.GetPlayerId()), target.gameObject);
                        ev.ReturnMessage = Global._success_cut + target.nicknameSync.Network_myNickSync;
                        return;
                    }
                }
                else
                {
                    ev.ReturnMessage = Global._toolong;
                    return;
                }
            }
        }

        internal void OnPlayerSpawn(PlayerSpawnEvent ev)
        {
            if (ev.Player.gameObject.GetComponent<KnifeHolder>())
                Object.Destroy(ev.Player.gameObject.GetComponent<KnifeHolder>());
        }

        internal void OnPlayerDeath(ref PlayerDeathEvent ev)
        {
            if (ev.Player.gameObject.GetComponent<KnifeHolder>())
                Object.Destroy(ev.Player.gameObject.GetComponent<KnifeHolder>());
        }

        public void OnRoundStart()
        {
            Global.can_use_commands = true;
        }

        public void OnWaitingForPlayers()
        {
            Global.can_use_commands = false;
        }
    }
}