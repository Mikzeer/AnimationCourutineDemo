﻿namespace PositionerDemo
{
    public abstract class TargetTeamFiltter : CardFiltter
    {
        bool isAlly;
        public TargetTeamFiltter(bool isAlly, int ID) : base(ID)
        {
            this.isAlly = isAlly;
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            //TODO: IsAlly no se setea desde ningun lado, asi que es una chotada
            IOcuppy ocuppier = cardTarget.GetOcuppy();
            if (ocuppier == null) return null;
            if (ocuppier.IsAlly != isAlly) return null;
            return base.CheckTarget(cardTarget);
        }
    }
}