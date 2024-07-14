using Ghost.States;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Ghost
{
    public class Ghost : MonoBehaviour
    {
        public float _suckingDifficulty;
        public Transform _player;

        public Animator _anim;

        protected AudioSource _audioSource;

        public Material _ghostMaterial;

        #region State Machine Variables
        //All the potential states
        public GhostStateMachine StateMachine { get; set; }
        public IdleState IdleState { get; set; }
        public AttackingState AttackingState { get; set; }
        public RunningState RunningState { get; set; }
        public SearchingState SearchingState { get; set; }
        public StrugglingState StrugglingState { get; set; }
        #endregion

        #region Animation Triggers
        private void AnimationTriggerEvent(AnimationTriggerType triggerType)
        {
            StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
        }

        public enum AnimationTriggerType
        {
            Idle,
            Struggling,
        }
        #endregion

        #region ScriptableObject Variables

        //The variables where you put the behaviours in, in the inspector
        [SerializeField] private GhostIdleSOB IdleSOB;
        [SerializeField] private GhostAttackSOB AttackSOB;
        [SerializeField] private GhostRunningSOB RunningSOB; 
        [SerializeField] private GhostSearchingSOB SearchingSOB;
        [SerializeField] private GhostStrugglingSOB StrugglingSOB;

        //Instances of those variables
        public GhostIdleSOB GhostIdleSOBInstance { get; set; }
        public GhostAttackSOB GhostAttackSOBInstance { get; set; }
        public GhostRunningSOB GhostRunningSOBInstance { get; set; }
        public GhostSearchingSOB GhostSearchingSOBInstance { get; set; }
        public GhostStrugglingSOB GhostStrugglingSOBInstance { get; set; }
        #endregion

        #region Start/Awake
        void Awake()
        {
            _ghostMaterial = transform.GetChild(0).GetChild(1).GetComponent<Renderer>().sharedMaterial;

            _audioSource = GetComponent<AudioSource>();

            if (IdleSOB != null)
            {
                GhostIdleSOBInstance = Instantiate(IdleSOB);
                IdleState = new IdleState(this, StateMachine);
                GhostIdleSOBInstance.Initialize(gameObject, this, GetComponent<NavMeshAgent>());
            }
            if(AttackSOB != null)
            {
                GhostAttackSOBInstance = Instantiate(AttackSOB);
                AttackingState = new AttackingState(this, StateMachine);
                GhostAttackSOBInstance.Initialize(gameObject, this);
            }
            if (RunningSOB != null)
            {
                GhostRunningSOBInstance = Instantiate(RunningSOB);
                RunningState = new RunningState(this, StateMachine);
                GhostRunningSOBInstance.Initialize(gameObject, this, GetComponent<NavMeshAgent>());
            }
            if (SearchingSOB != null)
            {
                GhostSearchingSOBInstance = Instantiate(SearchingSOB);
                SearchingState = new SearchingState(this, StateMachine);
                GhostSearchingSOBInstance.Initialize(gameObject, this, GetComponent<NavMeshAgent>());
            }
            if (StrugglingSOB != null)
            {
                GhostStrugglingSOBInstance = Instantiate(StrugglingSOB);
                StrugglingState = new StrugglingState(this, StateMachine);
                GhostStrugglingSOBInstance.Initialize(gameObject, this, GetComponent<NavMeshAgent>());
            }

            StateMachine = new GhostStateMachine();
        }
        void Start()
        {
            StateMachine.Initialize(SearchingState);

            _player = GameObject.Find("Player").transform;
        } 
        #endregion

        #region Updates
        void Update()
        {
            StateMachine.CurrentEnemyState.FrameUpdate();
        }
        void FixedUpdate()
        {
            StateMachine.CurrentEnemyState.PhysicsUpdate();
        }  
        #endregion


        
    }
}
