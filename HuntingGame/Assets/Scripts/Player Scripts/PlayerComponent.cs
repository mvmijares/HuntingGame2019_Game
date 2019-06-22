using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    Player _player;

    public void InitializeComponent(Player player)
    {
        _player = player;
    }
}
