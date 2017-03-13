/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class PacmanController : MonoBehaviour,
                                                ITrackableEventHandler
    {
		public Pacman trigger;
		public GameObject virtualBricks;
		public bool debugVisual = true;
		public bool manualDebug = true;

        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;
    
        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS
    
        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
			if(manualDebug){
				OnTrackingFound();
			}
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
			if(manualDebug) return;
			if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
			trigger.triggerAnimation();
			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");

			Collider[] colliderComponents = virtualBricks.GetComponentsInChildren<Collider>(true);
			foreach (Collider component in colliderComponents)
				component.enabled = true;
			
			if(debugVisual){
				Renderer[] rendererComponents = virtualBricks.GetComponentsInChildren<Renderer>(true);
				foreach (Renderer component in rendererComponents)
                	component.enabled = true;
            }
        }


        private void OnTrackingLost()
        {
			Collider[] colliderComponents = virtualBricks.GetComponentsInChildren<Collider>(true);
			foreach (Collider component in colliderComponents)
				component.enabled = false;
			
			Renderer[] rendererComponents = virtualBricks.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer component in rendererComponents)
				component.enabled = false;
			
			trigger.pause();
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        #endregion // PRIVATE_METHODS
    }
}
