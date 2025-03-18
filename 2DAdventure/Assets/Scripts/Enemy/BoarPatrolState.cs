using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BoarPatrolState : BaseState
{   
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.anim.SetBool("isWalk", true);
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }
    
    public override void PhysicsUpdate()
    {
        throw new System.NotImplementedException();
    }
    public override void LogicUpdate()
    {
        

        if(currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
        if ((currentEnemy.pc.touchLeftWall && currentEnemy.faceDir.x < 0) | (currentEnemy.pc.touchRightWall && currentEnemy.faceDir.x > 0) | !currentEnemy.pc.isGround)
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
            if (currentEnemy.faceDir.x < 0)
                currentEnemy.pc.buttomOffset = new Vector2(currentEnemy.pc.rightOffset.x, currentEnemy.pc.buttomOffset.y);
            else currentEnemy.pc.buttomOffset = new Vector2(currentEnemy.pc.leftOffset.x, currentEnemy.pc.buttomOffset.y);
        }
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("isWalk", false);
    }

    
}
