using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeEntity : Gimmic
{
    public bool open = false;
    public readonly string Label = "Spike";
    private Animator animator;
    private AudioSource audioSource;
    private GameObject player;
    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        if (open) {
            animator.Play("spike_open_idle");
        } else {
            animator.Play("spike_close_idle");
        }
        animator.SetBool("open", open);
    }

    public override void Action()
    {
        GameManager.Instance.AddAction(new SpikeAction(this));
        
        player = GameObject.FindWithTag("Player");
        float distance = Vector2.Distance(transform.position, player.transform.position);
        // Check if the player is within the hearing distance
        if (distance <= 5f)
        {
                audioSource.Play();
        }
    }

    public void SetOpen(bool open)
    {
        this.open = open;
        animator.SetBool("open", open);
    }

}

class SpikeAction : IReversibleAction
{
    public SpikeEntity spike;
    public bool open;
    public SpikeAction(SpikeEntity spike)
    {
        this.spike = spike;
        this.open = spike.open;
    }

    public void Perform()
    {
        spike.SetOpen(!open);
    }

    public void Undo()
    {
        spike.SetOpen(open);
    }
}
