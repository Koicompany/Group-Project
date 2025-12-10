using UnityEngine;

public class _rockDebug : DefaultObserverEventHandler
{
    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        Debug.Log("Dog found!");
        // Custom behavior for Dog here
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        Debug.Log("Dog lost!");
    }

}
