using UnityEngine;
using Exiled.API.Features;

namespace CraftKnife
{
    class CraftProccess : MonoBehaviour
    {
        private float Timer = 0f;
        private readonly float TimeIsUp = 1.0f;
        public float progress = Global.time_to_craft;
        public Vector3 startpos;
        private Player crafter;

        public void Start()
        {
            crafter = Player.Get(gameObject);
            startpos = gameObject.transform.position;
        }

        public void Update()
        {
            Timer += Time.deltaTime;
            if (Timer > TimeIsUp)
            {
                Timer = 0f;
                progress -= TimeIsUp;
                crafter.ClearBroadcasts();
                crafter.Broadcast(1, "<color=#228b22>Вы мастерите заточку. Осталось времени: " + progress.ToString() + "</color>", Broadcast.BroadcastFlags.Normal);
            }

            if (progress < 0f)
            {
                crafter.ClearBroadcasts();
                crafter.Broadcast(10, "<color=#228b22>Вы смастерили заточку. Пропишите команду '.stab', смотря на того, кого вы хотите порезать</color>", Broadcast.BroadcastFlags.Normal);
                gameObject.AddComponent<KnifeHolder>();
                Destroy(gameObject.GetComponent<CraftProccess>());
            }
            if (Vector3.Distance(gameObject.transform.position, startpos) > Global.distance_to_cut)
            {
                crafter.ClearBroadcasts();
                crafter.Broadcast(10, "<color=#228b22>Вы сдвинулись с места, крафт прекращен</color>", Broadcast.BroadcastFlags.Normal);
                Destroy(gameObject.GetComponent<CraftProccess>());
            }
        }
    }
}