using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DB.War.Stack;

namespace DB.War.Stickman
{
    public class SquadManager : MonoBehaviour
    {
        int wpindex = 0;

        [Button]
        public void AddPerson(Stickman person)
        {
            _crowd.Add(person);
            if (person.isTank)
            {
                person.mainGoalT.parent = WPs[wpindex++];
                person.mainGoalT.localPosition = Vector3.zero;
                
                return;
            }
            else
            {
                person.GetComponent<StickPathTraveler>().OnLock += () =>
                {
                    person.ReadSquadRB(rb);
                };
            }

            PlaceCrowd();
        }

        [SerializeField] private List<Stickman> _crowd = new List<Stickman>();
        [SerializeField] private Transform _leader;
        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private float rainHeight = 10f;
        [SerializeField] private PowerUps powerups;
        [SerializeField] private Stacker stacker;
        [SerializeField] private Rigidbody rb;

        [FoldoutGroup("Placor")]
        [SerializeField] private float _radiusIncrease = 0.3f;
        [FoldoutGroup("Placor")]
        [SerializeField] private int _density = 4;

        [SerializeField] private Transform[] WPs;

        [Button]
        private void PlaceCrowd()
        {
            int i = 0;
            for (int rimIndex = 1; i < _crowd.Count; rimIndex++)
            {
                int density = _density * rimIndex;
                int inrimIndex = 0;

                while (true)
                {
                    // set values
                    int rimOffset = inrimIndex++ % density;
                    float time = (float)rimOffset / (float)density;
                    // calculate offset
                    Quaternion rotator = Quaternion.Euler(0, time * 360f, 0);
                    Vector3 offset = rotator * (transform.forward * _radiusIncrease * rimIndex);

                    // place them
                    _crowd[i].mainGoalT.parent = transform;
                    _crowd[i].mainGoalT.position = offset + transform.position;

                    // continue loop
                    i++;
                    if (inrimIndex >= density || i >= _crowd.Count)
                    {
                        break;
                    }
                }
            }
        }

        private void Start()
        {
            transform.parent = null;
        }
    }
}