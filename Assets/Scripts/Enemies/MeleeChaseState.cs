using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeChaseState : IEnemyMeleeState
{
    private EnemyMeleeController _controller; // Referencia al controlador del enemigo

    public MeleeChaseState(EnemyMeleeController controller)
    {
        this._controller = controller;
    }

    // Actualiza el estado de persecución
    public void UpdateState()
    {
        _controller.Chase(); // Persigue al jugador
        if (!_controller.IsPlayerInRange())
        {
            _controller.ChangeState(new MeleePatrolState(_controller)); // Vuelve a patrullar si el jugador se aleja
        }
    }
}
