using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public string InteractText { get; }

    public void InteractE();

    public void InteractQ();

    public void InteractF();

    public void MouseHoverEnter();

    public void MouseHoverExit();
}