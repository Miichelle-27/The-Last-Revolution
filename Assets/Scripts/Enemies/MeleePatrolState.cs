using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePatrolState : IEnemyMeleeState
{
    private EnemyMeleeController _controller; // Referencia al controlador del enemigo

    public MeleePatrolState(EnemyMeleeController controller)
    {
        this._controller = controller;
    }

    // Actualiza el estado de patrullaje
    public void UpdateState()
    {
        _controller.Patrol(); // Mueve al enemigo entre puntos de patrullaje
        if (_controller.IsPlayerInRange())
        {
            _controller.ChangeState(new MeleeChaseState(_controller)); // Cambia a persecución si el jugador está cerca
        }
    }
}
