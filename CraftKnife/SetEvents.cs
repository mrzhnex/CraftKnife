using Exiled.API.Features;
using Exiled.Events.EventArgs;
using RemoteAdmin;
using System.Linq;
using UnityEngine;

namespace CraftKnife
{
    public class SetEvents
    {
        internal void OnRoundStarted()
        {
            Global.can_use_commands = true;
        }

        internal void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player.GameObject.GetComponent<KnifeHolder>())
                Object.Destroy(ev.Player.GameObject.GetComponent<KnifeHolder>());
        }

        internal void OnSendingConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            if (!Global.can_use_commands)
            {
                ev.ReturnMessage = "Дождитесь начала раунда!";
                return;
            }

            if (ev.Name.ToLower().Contains("craft") && ev.Name.ToLower().Contains("knife"))
            {
                if (ev.Player.GameObject.GetComponent<CraftProccess>() != null)
                {
                    ev.ReturnMessage = Global._already_craft;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<KnifeHolder>() != null)
                {
                    ev.ReturnMessage = Global._already_have_knife;
                    return;
                }
                if (ev.Player.Inventory.items.Where(x => x.id == ItemType.Flashlight).FirstOrDefault() == default)
                {
                    ev.ReturnMessage = Global._not_have_flashlight;
                    return;
                }
                ev.Player.Inventory.items.Remove(ev.Player.Inventory.items.Where(x => x.id == ItemType.Flashlight).FirstOrDefault());
                ev.Player.GameObject.AddComponent<CraftProccess>();
                ev.ReturnMessage = Global._success_start + Global.time_to_craft.ToString();
                return;
            }
            if (ev.Name.ToLower() == "stab")
            {
                if (ev.Player.GameObject.GetComponent<KnifeHolder>() == null)
                {
                    ev.ReturnMessage = Global._nothaveknife;
                    return;
                }
                Player target = null;
                if (Physics.Raycast((ev.Player.PlayerCamera.forward * 1.001f) + ev.Player.GameObject.transform.position, ev.Player.PlayerCamera.forward, out RaycastHit hit, Global.distance_to_cut))
                {
                    if (hit.transform.GetComponent<QueryProcessor>() == null)
                    {
                        ev.ReturnMessage = Global._toolong;
                        return;
                    }
                    else
                    {
                        target = Player.Get(hit.transform.gameObject);
                        Object.Destroy(ev.Player.GameObject.GetComponent<KnifeHolder>());
                        if (target.Team == Team.SCP)
                        {
                            ev.ReturnMessage = Global._isscp;
                            return;
                        }
                        target.ReferenceHub.playerStats.HurtPlayer(new PlayerStats.HitInfo(Global.damage, ev.Player.Nickname, DamageTypes.Scp939, ev.Player.Id), target.GameObject);
                        ev.ReturnMessage = Global._success_cut + target.Nickname;
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

        public void OnWaitingForPlayers()
        {
            Global.can_use_commands = false;
        }
    }
}