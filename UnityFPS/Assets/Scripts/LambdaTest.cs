using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BTState01
{
    Failure,
    Success,
    Continue,
    Abort
}

public static class BTtest
{
    public static Root01 Root() { return new Root01(); }
    public static Action01 Call(System.Action fn) { return new Action01(fn); }

    public static ConditionalBranch01 If(System.Func<bool> fn) { return new ConditionalBranch01(fn); }

}


public abstract class BTNode01
{
    public abstract BTState01 Tick();
}

public abstract class Branch01 : BTNode01
{
    protected int activeChild;
    protected List<BTNode01> children = new List<BTNode01>();

    public virtual Branch01 OpenBranch(params BTNode01[] children)
    {
        for (int i = 0; i < children.Length; i++)
        {
            this.children.Add(children[i]);
        }
        return this;
    }
}

public abstract class Block01 : Branch01
{
    public override BTState01 Tick()
    {
        switch (children[activeChild].Tick())
        {
            case BTState01.Continue:
                return BTState01.Continue;
            default:
                activeChild++;
                if (activeChild==children.Count)
                {
                    activeChild = 0;
                    return BTState01.Success;
                }
                return BTState01.Continue;
        }
    }

}


public class ConditionalBranch01 : Block01
{
    public System.Func<bool> fn;
    bool tested = false;

    public ConditionalBranch01(System.Func<bool> fn)
    {
        this.fn = fn;
    }
    public override BTState01 Tick()
    {
        if (!tested)
        {
            tested = fn();
        }
        if (tested)
        {
            var result = base.Tick();
            if (result == BTState01.Continue)
            {
                return BTState01.Continue;
            }
            else
            {
                tested = false;
                return result;
            }
        }
        else
        {
            return BTState01.Failure;
        }
    }
}


public class Root01 : Block01
{
    public bool isTerminated = false;
    public override BTState01 Tick()
    {
        if (isTerminated)
        {
            
            return BTState01.Abort;
        }

        while (true)
        {
            switch (children[activeChild].Tick())
            {
                case BTState01.Continue:                  
                    return BTState01.Continue;

                case BTState01.Abort:
                    isTerminated = true;                 
                    return BTState01.Abort;

                default:
                    activeChild++;
                    if (activeChild == children.Count)
                    {
                        activeChild = 0;
                        
                        return BTState01.Success;
                    }
                    continue;
            }
        }
    }
}

public class Action01 : BTNode01
{
    System.Action fn;
    public Action01(System.Action fn)
    {
        this.fn = fn;
    }

    public override BTState01 Tick()
    {
        if (fn != null)
        {
            fn();
            return BTState01.Success;
        }
        else
        {
            return BTState01.Failure;
        }
    }
}

