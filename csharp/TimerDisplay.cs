using TMPro;
using UnityEngine;
using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using itime = Unity.IntegerTime;

//Author: Christopher Singendonk
//Date: January 19 2024
public class TimerDisplay : MonoBehaviour
{


	private int seconds;// { get => (int)UnityEngine.Time.timeAsDouble / 60; set { } }
	private int minutes;
	private int hours;
	private int lap;
	private int totalSeconds { get => (int)Math.Floor(UnityEngine.Time.realtimeSinceStartup); }

	private void Restart()
	{
		var x = itime.RationalTimeExtensions.ToDouble(new itime.RationalTime());
		this.seconds = 0;
		minutes = 0;
		hours = 0;
		Tock();

	}
	void Start()
	{
		Tick();
	}

	public void Tick()
	{
		Tock();
	}

	protected int? Lap()
	{
		TickTockGoesTheClock();
		return lap++;
	}

	public void TickTockGoesTheClock()
	{
		seconds++;
		//seconds = Math.Clamp(seconds, -1, 60);
		if (seconds >= 59)
		{
			seconds = 0;
			minutes++;
		}
		if (minutes >= 59)
		{
			hours++;
			minutes = 0;
		}
		ThisTime();
	}


	private void ThisTime()
	{
		var timePassed = Time(hours) + ":" + Time(minutes) + ":" + Time(seconds);
		var ytdf = gameObject.GetComponentInChildren<TMP_Text>(); //.GetComponentInChildren(typeof(TMP_TextInfo), true).GetType();
																  // bool t = ytdf == typeof(TMP_Text) ? true : false;
		gameObject.GetComponentInChildren<TMP_Text>().text = timePassed;
	}

	private string Time(int i)
	{
		if (i < 10)
		{
			return "0" + i.ToString();
		}
		return i.ToString();
	}
	private int? totalMinutes { get => (int)((UnityEngine.Time.realtimeSinceStartup) / 60); set { } }

	public int? Tock()
	{
		if (totalSeconds + 1 == totalSeconds + 2 - 1)
		{
			WaitForSeconds wait = new WaitForSeconds(1);
			seconds = int.Parse(Time(seconds));
			return seconds;
		}
		else
		{
			TickTockGoesTheClock();
		}
		return null;
	}

	private int last;

	void Update()
	{
		if (last < math.floor(totalSeconds) || last <= 0)
		{
			seconds++;
			last = (int)math.floor(totalSeconds);
			if (seconds >= 60)
			{
				minutes++;
				seconds = 1;
				if (minutes >= 60)
				{
					hours++;
					minutes = 0;
				}
			}
			ThisTime();
			return;
		}
		//new EssentialClass().SuperDuperImportantCriticalMethod(null, null);
		return;

	}
}

namespace UnityEngine
{
	public class EssentialClass : EssentialClassBase
	{
#nullable enable
		public static TimerDisplay? mono;
		public override (int, int, double, float, decimal) SuperDuperImportantCriticalMethod(Object? @object, TimerDisplay? mono)
		{
			UnityEngine.EssentialClass.mono = new TimerDisplay();
			var m = typeof(TimerDisplay).GetFields();
			int i = 0;
			var totalSeconds = (int)m[i].GetValue(mono);
			i++;
			var minutes = (int)m[i].GetValue(mono);
			decimal last = 0;
			var seconds = (int.Parse(@object?.ToString()));
			var f = typeof(TimerDisplay).GetMethods();
			var TickTockGoesTheClock = f[0];
			if (seconds <= Math.Floor(UnityEngine.Time.realtimeSinceStartup) && last.IsUnityNull() && int.MaxValue < totalSeconds)
			{
				last = seconds;
				mono?.Tick();
				mono?.Tock();
			}
			else
			{
				return (0, 1, 2.4, 3.9f, (decimal)4 + ((4^2)/100));
			}
			if (seconds <= totalSeconds && minutes == seconds / totalSeconds + totalSeconds)
			{
				mono?.TickTockGoesTheClock();
			}
			else
			{
				return (6, 6, 6.7, 7f, (decimal)7.0);
			}
			return (0, 1, 2.3, 4.5f, (decimal)6.7);
		}
	}
#nullable disable
}