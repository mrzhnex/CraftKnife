using UnityEngine;
using EXILED.Extensions;

namespace CraftKnife
{
    class CraftProccess : MonoBehaviour
    {
        private float timer = 0f;
        private readonly float timeIsUp = 1.0f;
        public float progress = Global.time_to_craft;
        public Vector3 startpos;
        private ReferenceHub crafter;

        public void Start()
        {
            crafter = Player.GetPlayer(gameObject);
            startpos = gameObject.transform.position;
        }

        public void Update()
        {
            timer += Time.deltaTime;
            if (timer > timeIsUp)
            {
                timer = 0f;
                progress -= timeIsUp;
                crafter.ClearBroadcasts();
                crafter.Broadcast(1, "<color=#228b22>Вы мастерите заточку. Осталось времени: " + progress.ToString() + "</color>", true);
            }

            if (progress < 0f)
            {
                crafter.ClearBroadcasts();
                crafter.Broadcast(10, "<color=#228b22>Вы смастерили заточку. Пропишите команду '.stab', смотря на того, кого вы хотите порезать</color>", true);
                gameObject.AddComponent<KnifeHolder>();
                Destroy(gameObject.GetComponent<CraftProccess>());
            }
            if (Vector3.Distance(gameObject.transform.position, startpos) > Global.distance_to_cut)
            {
                crafter.ClearBroadcasts();
                crafter.Broadcast(10, "<color=#228b22>Вы сдвинулись с места, крафт прекращен</color>", true);
                Destroy(gameObject.GetComponent<CraftProccess>());
            }
        }
    }
}