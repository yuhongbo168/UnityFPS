using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BTStateA
{
    Succeed,
    Failed,
    Abort,
    Continue
}

public class BTA
{
    public static RootA Root() { return new RootA(); }
    public static ConditionalBruch If(System.Func<bool> fn) { return new ConditionalBruch(fn); }

    public static ActionA Call(System.Action ac) { return new ActionA(ac); }
}


public abstract class BTNodeA
{
   public abstract BTStateA Tick();

}

public abstract class BranchA : BTNodeA
{
    protected int activeChildren;
    protected List<BTNodeA> children = new List<BTNodeA>();
    public BranchA OpenBranch(params BTNodeA[] child)
    {
        for (int i = 0; i < child.Length; i++)
        {
            children.Add(child[i]);
        }

        return this;
    }
}

public abstract class BlockA : BranchA
{
    public override BTStateA Tick()
    {
        switch (children[activeChildren].Tick())
        {
            case  BTStateA.Continue:
                return BTStateA.Continue;

            default:
                activeChildren++;
                if (activeChildren==children.Count)
                {
                    activeChildren = 0;
                    return BTStateA.Succeed;
                }

                return BTStateA.Succeed;
        }
    }
}

public class ConditionalBruch : BlockA
{
    System.Func<bool> fn;
    bool tested;

    public ConditionalBruch(System.Func<bool> fn)
    {
        this.fn = fn;
    }

    public override BTStateA Tick()
    {
        if (!tested)
        {
            tested = fn();
        }
        if (tested)
        {
            var result = base.Tick();
            if (result == BTStateA.Continue)
            {
                return BTStateA.Continue;
            }
            else
            {
                tested = false;
                return result;
            }
        }
        else
        {
            return BTStateA.Failed;
        }
    }
}

public class ActionA:BTNodeA
{
    System.Action active;

    public ActionA (System.Action at)
    {
        active = at;
    }
    public override BTStateA Tick()
    {
        if (active!=null)
        {
            active();
        
            return BTStateA.Succeed;
        }
        else
        {
            
            return BTStateA.Failed;
        }
    }
}

public class RootA : BlockA
{
    public bool isTerminated = false;
    public override BTStateA Tick()
    {
        if (isTerminated)
        {
            return BTStateA.Abort;
        }

        while (true)
        {
            switch (children[activeChildren].Tick())
            {
                
                case BTStateA.Continue:
                    return BTStateA.Continue;
                    
                case BTStateA.Abort:
                    isTerminated = true;
                    return BTStateA.Abort;

                default:
                    activeChildren++;
                    if (activeChildren == children.Count)
                    {
                       
                        activeChildren = 0;
                        return BTStateA.Succeed;
                    }

                continue;
            }
        }
        
    }
}