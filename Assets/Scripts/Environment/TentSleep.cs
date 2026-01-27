using System.Collections;
using UnityEngine;

public class TentSleep : MonoBehaviour
{
    public void TryToSleep()
    {
        DayNightCycle daytime = DayNightCycle.instance;


        if (daytime.Cycle > 310 || daytime.Cycle < 60)
        {
            StartCoroutine("SleepUI");
            daytime.Cycle = 90f;
        }
        else
        {
            print("You cant sleep right now");
        }
    }

    IEnumerator SleepUI()
    {
        UIManager.instance.SleepBG.SetActive(true);
        UIManager.instance.Locked = true;
        yield return new WaitForSeconds(3);
        UIManager.instance.SleepBG.SetActive(false);
        UIManager.instance.Locked = false;
    }
}
