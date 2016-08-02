using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class Stunable : MonoBehaviour 
{
	//Stun duration 
    public enum StunType { NotStunned, ShockStun, GasStun };
	private Stopwatch stunTimer = new Stopwatch();
    public StunType mTypeofStun = StunType.NotStunned;
	public float stunDuration = 1f;
	public float invincibleDuration = 2f;
	public bool IsStunned = false;
	private bool bInvincible = false;
    public bool bStayStunned = false;
	public void UpdateStunnedTimer()
	{
		if (!bStayStunned)
        {
            if (stunTimer.Elapsed.Seconds >= stunDuration)
            {
                IsStunned = false;
                bInvincible = true;
            }
        }
        
	}
	public void UpdateStunInvinciblity()
	{
		if (stunTimer.Elapsed.Seconds >= invincibleDuration)
		{
			bInvincible = false;
			stunTimer.Reset();
		}
	}
	public void Stun()
	{
		if (!bInvincible && !IsStunned)
		{
			IsStunned = true;
			stunTimer.Start();
		}
	}
}
