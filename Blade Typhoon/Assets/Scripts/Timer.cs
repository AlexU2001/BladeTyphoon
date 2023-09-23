using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    private List<TimeSlot> _timeSlots = new List<TimeSlot>();
    public Action<string> TimerStarted;
    public Action<string> TimerOver;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void StartTimer(string tag, float length)
    {
        if (CheckForTag(tag) == null)
            NewTimer(tag, length);
        else
            ResetTimer(CheckForTag(tag));
    }
    private TimeSlot CheckForTag(string tag)
    {
        foreach (var slot in _timeSlots)
        {
            if (slot.tag.Equals(tag))
                return slot;
        }
        return null;
    }
    private void NewTimer(string tag, float length)
    {
        TimeSlot newSlot = new TimeSlot(tag, length);
        TimerStarted?.Invoke(tag);
        _timeSlots.Add(newSlot);
        if (_timeSlots.Count <= 1)
            StartCoroutine(decreaseTime());
    }
    private void ResetTimer(TimeSlot slot)
    {
        TimerStarted?.Invoke(slot.tag);
        slot.Reset();
    }
    private void EndTimer(TimeSlot slot)
    {
        Debug.Log("Removing...");
        _timeSlots.Remove(slot);
        Debug.Log("Removed...");
        TimerOver?.Invoke(slot.tag);
    }

    private IEnumerator decreaseTime()
    {
        WaitForSeconds oneSecond = new WaitForSeconds(1);
        while (_timeSlots.Count > 0)
        {
            yield return oneSecond;
            int count = _timeSlots.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                TimeSlot slot = _timeSlots[i];
                //Debug.Log(slot.ToString());
                if (slot.value > 0)
                    slot.Subtract(1);
                else
                    EndTimer(slot);
            }
        }
    }
    class TimeSlot
    {
        public TimeSlot(string tag, float length)
        {
            this.tag = tag;
            value = length;
            initialValue = length;
        }

        public string tag { get; private set; }
        public float value { get; private set; }
        public float initialValue { get; private set; }
        public void Subtract(float value) { this.value -= value; }
        public void setTime(float value) { this.value = value; this.initialValue = value; }
        public void Reset() { setTime(initialValue); }
        public override string ToString() { return tag + " Value: " + value + " Initial Value"; }
    }

}