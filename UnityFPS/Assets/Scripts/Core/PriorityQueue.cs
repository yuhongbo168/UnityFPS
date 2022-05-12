using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> where T : System.IComparable<T>
{
    public readonly List<T> items;

    public PriorityQueue()
    {
        items = new List<T>();
    }

    public bool Empty
    {
        get { return items.Count == 0; }
    }

    public T First
    {
        get
        {
            if (items.Count>0)
            {
                return items[0];
            }
            return items[items.Count - 1];
        }
    }

    int Compare(T a,T b)
    {
        return a.CompareTo(b);
    }

    public void Push(T item)
    {
        items.Add(item);
        SiftDown(0, items.Count - 1);
    }

    void SiftDown(int startpos,int pos)
    {
        var newitem = items[pos];
        while (pos>startpos)
        {
            var parentpos = (pos - 1) >> 1;
            var parent = items[parentpos];
            if (Compare(parent,newitem)<=0)
            {
                break;
            }
            items[pos] = parent;
            pos = parentpos;

        }
        items[pos] = newitem;
    }

    void SiftUp(int pos)
    {
        var endpos = items.Count;
        var startpos = pos;
        var newitem = items[pos];
        var childPos = 2 * pos + 1;
        while (childPos < endpos)
        {
            var rightpos = childPos + 1;
            if (rightpos<endpos && Compare(items[rightpos],items[childPos]) <= 0)
            {
                childPos = rightpos;
            }
            items[pos] = items[childPos];
            pos = childPos;
            childPos = 2 * pos + 1;
        }
        items[pos] = newitem;
        SiftDown(startpos, pos);
    }

    public T Pop()
    {
        lock(this)
        {
            T item;
            var last = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            if (items.Count  > 0)
            {
                item = items[0];
                items[0] = last;
                SiftUp(0);
            }
            else
            {
                item = last;
            }
            return item;
        }
    }
}
