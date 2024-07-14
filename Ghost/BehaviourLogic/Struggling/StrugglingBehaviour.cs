using Ghost.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Ghost
{
    [CreateAssetMenu(fileName = "Struggling", menuName = "Ghost-States/Struggling/StrugglingBehaviour")]
    public class StrugglingBehaviour : GhostStrugglingSOB
    {
        //IArtifactOriented possesiveGhost;
        public Texture2D[] _strugglingFaces;
        public Texture2D _normalFace;


        public override void DoAnimationTriggerEventLogic(Ghost.AnimationTriggerType animationTriggerType)
        {
            base.DoAnimationTriggerEventLogic(animationTriggerType);
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();

            AudioManager.instance.Play("GhostStruggling");
            _ghost._anim.SetBool("IsStruggling", true);

            if(Random.Range(0,6) != 2)
                _ghost._ghostMaterial.SetTexture("_FaceTexture", _strugglingFaces[0]);
            else
                _ghost._ghostMaterial.SetTexture("_FaceTexture", _strugglingFaces[1]);

            //possesiveGhost = (IArtifactOriented)_ghost;


            _agent.enabled = false;
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();

            _ghost._anim.SetBool("IsStruggling", false);
            _ghost._ghostMaterial.SetTexture("_FaceTexture", _normalFace);
            AudioManager.instance.Stop("GhostStruggling");

            _agent.enabled = true;
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
        }

        public override void DoPhyiscsLogic()
        {
            base.DoPhyiscsLogic();
        }

        public override void Initialize(GameObject gameObject, Ghost ghost, NavMeshAgent agent)
        {
            base.Initialize(gameObject, ghost, agent);
        }

        public override void ResetValues()
        {
            base.ResetValues();
        }
    }
}
