using UnityEngine;


namespace WasabiGame
{
    [CreateAssetMenu(menuName = "Command/Move/FixedMove")]
    public class FixedMoveCommand : MoveCommand
    {
        public Vector2[] positions;
        int currentIndex = 0;

        

        public FixedMoveCommand() : base()
        {
            
        }

        public override void MoveTo(Vector2 pos , bool accReset = true)
        {
            currentIndex++;
            if(currentIndex >= positions.Length) currentIndex = 0;

            base.MoveTo(positions[currentIndex], accReset);
        }

    }
}