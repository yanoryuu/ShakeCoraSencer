using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    List<int> numbers = new List<int>();
    
    public void AddNum(int num)
    {
        numbers.Add(num);
    }
    
    public List<int> SearchNum(int num)
    {
        List<int> result = new List<int>();
        
        int diff = int.MaxValue;
        
        foreach (var n in numbers)
        {
           var tempDiff = Mathf.Abs(num - n);

           if (tempDiff < diff)
           {
               result.Clear();
               diff = tempDiff;
               result.Add(n);
           }else if (tempDiff == diff)
           {
               result.Add(n);
           }else {
               // do nothing
           }
        }
        return result;
    }
}
