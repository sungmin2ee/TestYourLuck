using UnityEngine;

public class ResultCheck : MonoBehaviour
{
   void Result()
    {
        if(Compare.checkResult() == "Win") { 
            //reset and add money
        }else if(Compare.checkResult() == "Draw")
        {
            //reset and do nothing
        }else if(Compare.checkResult() == "Lose")
        {
            //reset and spend money
        }
    }
}
