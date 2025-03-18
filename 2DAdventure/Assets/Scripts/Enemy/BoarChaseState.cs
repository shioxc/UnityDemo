using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void LogicUpdate()
    {
        if(currentEnemy.lostTimeCounter <=0 )
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        if ((currentEnemy.pc.touchLeftWall && currentEnemy.faceDir.x < 0) | (currentEnemy.pc.touchRightWall && currentEnemy.faceDir.x > 0) | !currentEnemy.pc.isGround)
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
            if (currentEnemy.faceDir.x < 0)
                currentEnemy.pc.buttomOffset = new Vector2(currentEnemy.pc.rightOffset.x, currentEnemy.pc.buttomOffset.y);
            else currentEnemy.pc.buttomOffset = new Vector2(currentEnemy.pc.leftOffset.x, currentEnemy.pc.buttomOffset.y);
        }
    }

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.anim.SetBool("isRun", true);
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("isRun",false);
    }

    public override void PhysicsUpdate()
    {
        throw new System.NotImplementedException();
    }
}
