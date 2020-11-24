using UnityEngine;

namespace RPG.Core 
{
    public class ActionScheduler : MonoBehaviour
    {
        private IAction currentAction;

        public void ActionStart(IAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null) 
            {
                currentAction.Cancel();
            }
            currentAction = action;
        }

        public void CancelCurrentAction() 
        {
            ActionStart(null);
        }
    }
}