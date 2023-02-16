using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossAnimationCommands : MonoBehaviour
{
    private FinalMaxerBrain m_brain;

    public void SetBrain(FinalMaxerBrain _brain)
    {
        m_brain = _brain;
    }

    public void StartLaserCharge()
    {
        m_brain.ChargeLaser();
    }

    public void StartAirLaserCharge()
    {
        m_brain.ChargeAirLaser();
    }

    public void ShootAirLaser()
    {
        m_brain.ShootAirLaser();
    }

    public void StopAirLaser()
    {
        m_brain.StopAirLaser();
    }

    public void OnImpact()
    {
        if (!m_brain) return;
        m_brain.OnImpact();
    }

    public void OnAnimationEnded()
    {
        m_brain.OnAnimationFinished();
    }

    public void OnActionFinished()
    {
        m_brain.OnActionFinished();
    }

    public void EnterRestingState()
    {
        m_brain.EnterRestState();
    }

    public void PlayBerseckerChanneVFX()
    {
        m_brain.PlayBerserkerChannelVFX();
    }

    public void PlayBersekerBurstVFX()
    {
        m_brain.PlayBersekerBurstVFX();
    }

    public void OnBersekerAnimationFinished()
    {
        m_brain.OnBersekerStart();
    }

    public void OnIntroductionFinish()
    {
        m_brain.PerformAction();
    }
    
    public void CameraStart()
    {
        m_brain.BringCamera();
    }

    public void CameraEnd()
    {
        m_brain.ReturnCameraToPlayer();
    }
}
