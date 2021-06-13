using System;
using UnityEngine;
using UnityEngine.Events;

namespace Controller
{
    public class RopeController : MonoBehaviour
    {
        public event Action OnJointBreak;
        
        private void OnJointBreak2D(Joint2D brokenJoint)
        {
            OnJointBreak?.Invoke();
        }
    }
}