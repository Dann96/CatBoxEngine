using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class DialogueBehaviour : PlayableBehaviour
{
    public DialogueFlavor dialogueFlavor;
    public string dialogueTextKey;

    public bool pause = false;

    private bool clipPlayed = false;
    private bool pauseScheduled = false;

    private PlayableDirector director;

    public override void OnPlayableCreate(Playable playable)
    {
        director = (playable.GetGraph().GetResolver() as PlayableDirector);
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (!clipPlayed && info.weight > 0f)
        {
            //set dialogue panel here

            if (Application.isPlaying)
            {
                if (pause)
                {
                    pauseScheduled = true;
                }
            }
            clipPlayed = true;
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (pauseScheduled)
        {
            pauseScheduled = false;
            director.Pause();
        }
        else
        {

        }
    }
}
