using RemoteAdmin;
using Smod2.API;
using UnityEngine;

namespace CraftKnife
{
    class BleedComponent : MonoBehaviour
    {
        private float timer = 0f;
        private float timeIsUp = 1.0f;
        public int damage = 1;
        public float progress = Global.time_to_craft;
        private Player bleeder;
        private string badge;
        private string color;

        public void Start()
        {
            foreach (Player p in Global.plugin.Server.GetPlayers())
            {
                if (p.PlayerId == gameObject.GetComponent<QueryProcessor>().PlayerId)
                {
                    bleeder = p;
                    break;
                }
            }
            badge = gameObject.GetComponent<ServerRoles>().NetworkMyText;
            color = gameObject.GetComponent<ServerRoles>().NetworkMyColor;
            gameObject.GetComponent<ServerRoles>().NetworkMyText = Global.badge;
            gameObject.GetComponent<ServerRoles>().NetworkMyColor = Global.color;
        }

        public void Update()
        {
            timer = timer + Time.deltaTime;
            if (timer >= timeIsUp)
            {
                timer = 0f;
                progress = progress - timeIsUp;
                bleeder.Damage(damage, DamageType.SCP_939);
            }

            if (progress <= 0f)
            {
                Destroy(gameObject.GetComponent<BleedComponent>());
            }

        }
        public void OnDestroy()
        {
            gameObject.GetComponent<ServerRoles>().NetworkMyText = badge;
            gameObject.GetComponent<ServerRoles>().NetworkMyColor = color;
        }
    }
}
