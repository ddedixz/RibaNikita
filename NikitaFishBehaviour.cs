using UnityEngine;

namespace NikitaFishMod
{
    public class NikitaFishBehaviour : MonoBehaviour
    {
        private Creature creature;
        private float nextActionTime = 0f;
        private float actionInterval = 5f;

        void Start()
        {
            creature = GetComponent<Creature>();
            Debug.Log("[NikitaFish] Рыба Никита заспавнилась");
        }

        void Update()
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime = Time.time + actionInterval;
                DoRandomAction();
            }
        }

        void DoRandomAction()
        {
            int action = Random.Range(0, 3);
            
            switch (action)
            {
                case 0:
                    Debug.Log("[NikitaFish] Никита делает круг из кокаина ");
                    break;
                case 1:
                    Debug.Log("[NikitaFish] Никита куда-то торопится");
                    if (creature != null)
                    {
                    }
                    break;
            }
        }

        void OnDestroy()
        {
            Debug.Log("[NikitaFish] Никита уплыл нахуй");
        }
    }
}