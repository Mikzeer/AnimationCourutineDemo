namespace PositionerDemo
{
    public class ShieldCard : Card
    {
        public ShieldCard(int ID, Player ownerPlayer) : base(ID, ownerPlayer)
        {
        }

        public override void OnDropCard()
        {
            base.OnDropCard();
            //if (gameCreator.actualPlayerTurn == ownerPlayer)
            //{
            //    ownerPlayer.useCardAbility.Set(this);

            //    if (ownerPlayer.useCardAbility.OnTryExecute())
            //    {
            //        gameCreator.ChangeState(new GameStateMachine.UseCardState(ownerPlayer, gameCreator, gameCreator.currentState));
            //    }
            //}
        }
    }
}