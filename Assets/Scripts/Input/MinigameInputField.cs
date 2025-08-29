using TMPro;
using UnityEngine.EventSystems;

namespace SwedishApp.Input
{
    public class MinigameInputField : TMP_InputField
    {
        //This is a remnant from trying to solve mobile keyboards disappearing
        //after selecting a new input field via code. It is not possible to stop
        //from happening in this way and you should refer instead to the method
        //described in the handover documentation to avoid repeating my folly.
        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
        }
    }
}