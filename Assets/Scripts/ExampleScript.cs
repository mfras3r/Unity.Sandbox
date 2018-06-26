using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class ExampleScript : MonoBehaviour
    {
        public float Speed = 0f;
        public float Distance = 0f;
        public float Time = 0f;

        public float MinimumSpeedLimit = 40f;

        public float MaximumSpeedLimit = 70f;

        public string SpeedUnit = "mph";

        // Use this for initialization
        void Start ()
        {
            Distance = 100;
            Time = 1;
        }
	
        // Update is called once per frame
        void Update ()
        {
            Speed = Time != 0
                ? Distance / Time
                : 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckSpeed();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                EnemySearch();
            }
        }

        public void CheckSpeed()
        {
            if (MinimumSpeedLimit > MaximumSpeedLimit)
            {
                print(string.Format(
                    "Illogical speed limits set. Minimum speed limit {0:F1}{2} is greater than Maximum speed limit {1:F1}{2}.",
                    MinimumSpeedLimit, MaximumSpeedLimit, SpeedUnit));
            }

            if (Speed > 70)
            {
                print(string.Format("You are breaking the law! You're doing {0:F1}{2} in a {1:F1}{2} maximum zone.", Speed, MaximumSpeedLimit, SpeedUnit));
            }
            else if (Speed < MinimumSpeedLimit)
            {
                print(string.Format("You are breaking the law! You're doing {0:F1}{2} in a {1:F1}{2} minimum zone.", Speed, MinimumSpeedLimit, SpeedUnit));
            }
        }

        public void EnemySearch()
        {
            var enemyDistances = Enumerable.Range(1, 5)
                .Select(i => new { Enemy = i, Distance = Random.Range(1, 10) });
            var groupedEnemies = enemyDistances
                .Select(e =>
                {
                    if (e.Distance < 4)
                    {
                        return new {e.Enemy, e.Distance, DistanceCategory = EnemyDistance.Close};
                    }
                    else if (e.Distance < 8)
                    {
                        return new {e.Enemy, e.Distance, DistanceCategory = EnemyDistance.Medium};
                    }
                    else
                    {
                        return new {e.Enemy, e.Distance, DistanceCategory = EnemyDistance.Far};
                    }
                })
                .GroupBy(e => e.DistanceCategory)
                .ToDictionary(g => g.Key, g => g.Select(e => e.Enemy.ToString()).ToArray());

            if (groupedEnemies.ContainsKey(EnemyDistance.Close))
            {
                print(string.Format("Enemies {0} are close by!", string.Join(", ", groupedEnemies[EnemyDistance.Close])));
            }

            if (groupedEnemies.ContainsKey(EnemyDistance.Medium))
            {
                print(string.Format("Enemies {0} are at medium range.", string.Join(", ", groupedEnemies[EnemyDistance.Medium])));
            }

            if (groupedEnemies.ContainsKey(EnemyDistance.Far))
            {
                print(string.Format("Enemies {0} are far away.", string.Join(", ", groupedEnemies[EnemyDistance.Far])));
            }

        }
    }

    public enum EnemyDistance
    {
        Close,
        Medium,
        Far
    }
}
