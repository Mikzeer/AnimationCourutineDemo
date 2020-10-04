using UnityEngine;

namespace PositionerDemo
{
    public class ShieldCard : Card
    {
        public ShieldCard(int ID, Player ownerPlayer, CardScriptableObject cardSO) : base(ID, ownerPlayer, cardSO)
        {
            //CardFiltters.Add(new PlayerFriendCardFiltter());
            //CardFiltters.Add(new PlayerCardAmountCardFiltter());
        }

        public ShieldCard(int ID, CardScriptableObject CardSO) : base(ID, CardSO)
        {
            //CardFiltters.Add(new PlayerFriendCardFiltter());
            //CardFiltters.Add(new PlayerCardAmountCardFiltter());
        }


        //public override void OnDropCard()
        //{
        //    base.OnDropCard();
        //    Debug.Log("ShieldCard");
                
        //    //if (gameCreator.actualPlayerTurn == ownerPlayer)
        //    //{
        //    //    ownerPlayer.useCardAbility.Set(this);

        //    //    if (ownerPlayer.useCardAbility.OnTryExecute())
        //    //    {
        //    //        gameCreator.ChangeState(new GameStateMachine.UseCardState(ownerPlayer, gameCreator, gameCreator.currentState));
        //    //    }
        //    //}
        //}
    }
}

