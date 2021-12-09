using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent{

    public PlayerController player;

    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform workTrans;
    [SerializeField] private Transform playTrans;
    [SerializeField] private Transform eatTrans;
    [SerializeField] private Transform sleepTrans;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMesh;

    bool wantSleep = false;
    bool wantEat = false;
    bool wantPlay = false;
    bool wantWork = false;

    int moveForward;
    int rotate;
    int startJob;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = Vector3.zero;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(targetTransform.position);
        sensor.AddObservation(workTrans.position);
        sensor.AddObservation(playTrans.position);
        sensor.AddObservation(eatTrans.position);
        sensor.AddObservation(sleepTrans.position);
        sensor.AddObservation(player.energy);
        sensor.AddObservation(player.mood);
        sensor.AddObservation(player.hungry);
        sensor.AddObservation(player.money);
        sensor.AddObservation(player.inSleep);
        sensor.AddObservation(player.inWork);
        sensor.AddObservation(player.inLunch);
        sensor.AddObservation(player.inPlay);
        sensor.AddObservation(player.working);
        sensor.AddObservation(player.playing);
        sensor.AddObservation(player.sleeping);
        sensor.AddObservation(player.eating);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        moveForward = actions.DiscreteActions[0];
        rotate = actions.DiscreteActions[1];
        startJob = actions.DiscreteActions[2];

        player.commandMovement = moveForward;
        player.commandRotate = rotate;
        player.commandWork = startJob;

        CheckIfOutsideTargetZone(moveForward);
        //CheckIfInsideTargetZone() //It's handle by the onTriggerEnter()

        CheckIfDoingTargetJob();

        CheckPriorityTask();

        if (player.dead)
        {
            AddReward(-10f);
            EndEpisode();
        }

        if(player.money > 250)
        {
            AddReward(10f);
            EndEpisode();
        }
    }

    void CheckIfOutsideTargetZone(int movement_command) // Incentivar a se mover até chegar chegar na zona objetivo
    {
        if (CheckTargetZone(workTrans) && !player.inWork)
        {
            if (movement_command != 1) AddReward(-0.1f);
            else AddReward(0.5f);
        }
        else if (CheckTargetZone(eatTrans) && !player.inLunch)
        {
            if (movement_command != 1) AddReward(-0.1f);
            else AddReward(0.5f);
        }
        else if (CheckTargetZone(sleepTrans) && !player.inSleep)
        {
            if (movement_command != 1) AddReward(-0.1f);
            else AddReward(0.5f);
        }
        else if (CheckTargetZone(playTrans) && !player.inPlay)
        {
            if (movement_command != 1) AddReward(-0.1f);
            else AddReward(0.5f);
        }
    }
    bool CheckTargetZone(Transform current_zone)
    {
        if (targetTransform == current_zone) return true;
        else return false;
    }
    private void OnTriggerEnter(Collider other) // Incentivar a ficar parado na zona objetivo
    {
        if (other.tag == "Work" && CheckTargetZone(workTrans))
        {
            if (moveForward != 0) AddReward(-0.1f);
            else AddReward(0.5f);
        }
        else if (other.tag == "Play" && CheckTargetZone(playTrans))
        {
            if (moveForward != 0) AddReward(-0.1f);
            else AddReward(0.5f);
        }
        else if (other.tag == "Eat" && CheckTargetZone(eatTrans))
        {
            if (moveForward != 0) AddReward(-0.1f);
            else AddReward(0.5f);
        }
        else if (other.tag == "Sleep" && CheckTargetZone(sleepTrans))
        {
            if (moveForward != 0) AddReward(-0.1f);
            else AddReward(0.5f);
        }
    }

    private void OnCollisionEnter(Collision collision) // Incentivar a evitar as paredes
    {
        if(collision.collider.tag == "Wall")
        {
            if (rotate == 0) AddReward(-10f);
            else AddReward(1f);
        }
    }

    private void CheckIfDoingTargetJob()
    {
        if (wantSleep)
        {
            if (player.working || player.playing) AddReward(-1f); // Ruim, pois diminui mais
            else if (player.eating) AddReward(-0.1f); // Ruim, mas aceitável
            else if (player.sleeping) AddReward(1f); // Bom
        }

        if (wantEat && player.money > 50)
        {
            if (player.playing) AddReward(-1f); // Ruim, pois diminui mais
            else if (player.working || player.sleeping) AddReward(-0.5f); // Ruim
            else if (player.eating) AddReward(1f); // Bom
        }
        else if (wantEat && player.money < 50)
        {
            if (player.playing) AddReward(-1f); // Ruim, pois diminui mais
            else if (player.sleeping) AddReward(-0.8f); // Ruim
            else if (player.working) AddReward(-0.5f); // Ruim, mas está dando uma recompensa
            else if (player.eating) AddReward(1f); // Bom
        }

        if(wantPlay && player.money > 50)
        {
            if (player.working) AddReward(-1f);
            else if (player.eating) AddReward(0.1f);
            else if (player.sleeping) AddReward(0.5f);
            else if (player.playing) AddReward(1f);
        }
        else if(wantPlay && player.money < 50)
        {
            if (player.working) AddReward(-1f);
            else if (player.eating) AddReward(0.05f);
            else if (player.sleeping) AddReward(0.5f);
            else if (player.playing) AddReward(1f);
        }

        if (wantWork)
        {
            if (player.working) AddReward(1f);
            else AddReward(-0.5f);
        }
    }

    private void CheckPriorityTask()
    {
        if (player.energy < 40)
        {
            wantSleep = true;
            targetTransform = sleepTrans;

            wantEat = false;
            wantPlay = false;
            wantWork = false;
        }
        else
        {
            if(player.hungry > 60)
            {
                wantEat = true;
                targetTransform = eatTrans;
                wantSleep = false;
                wantPlay = false;
                wantWork = false;
            }
            else
            {
                if(player.mood < 40)
                {
                    wantPlay = true;
                    targetTransform = playTrans;
                    wantSleep = false;
                    wantEat = false;
                    wantWork = false;
                }
                else
                {
                    wantWork = true;
                    targetTransform = workTrans;
                    wantSleep = false;
                    wantEat = false;
                    wantPlay = false;
                }
            }
        }

    }
}
