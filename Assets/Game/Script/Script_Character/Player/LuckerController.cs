using UnityEngine;

public class LuckerController : PlayerController
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void UseBasicAttack()
    {
        base.UseBasicAttack();
    }

    protected override void UseSpSkill()
    {
        base.UseSpSkill();

        if (spSkill)
        {
           
        }
        else
        {
            
        }
    }
}
