using System.Collections.Generic;

namespace PositionerDemo
{
    public class PlayerManager
    {
        Player[] players;
        public ConfigurationData playerConfigurationData;
        List<UserDB> users;

        public PlayerManager()
        {
            users = new List<UserDB>();
            players = new Player[2];
        }

        public void CreatePlayers()
        {
            // 1b- CREAR A LOS DOS PLAYERS SEGUN LA CONFIGURATION DATA
            Player playerOne = new Player(0);
            playerOne.SetStatsAndAbilities(OccupierAbilityDatabase.CreatePlayerAbilities(playerOne), OccupierStatDatabase.CreatePlayerStat());

            Player playerTwo = new Player(1);
            playerTwo.SetStatsAndAbilities(OccupierAbilityDatabase.CreatePlayerAbilities(playerTwo), OccupierStatDatabase.CreatePlayerStat());

            // DEBERIAMOS TENER UN ABILITYMODIFIER MANAGER O ALGO SIMILIAR PARA ENCARGARSE DE ESTO TAL VEZ
            //SpawnAbility spw = (SpawnAbility)playerOne.Abilities[ABILITYTYPE.SPAWN];
            //ChangeUnitClassAbilityModifier ab = new ChangeUnitClassAbilityModifier(playerOne);
            //spw.AddAbilityModifier(ab);
            //CanceclSpawnAbilityModifier cancelSpawn = new CanceclSpawnAbilityModifier(playerOne);
            //spw.AddAbilityModifier(cancelSpawn);

            //TestAbilityModifier cnlEnemy = new TestAbilityModifier(playerTwo);
            //IAddSimpleAbilityActionModifierCommand simple = new IAddSimpleAbilityActionModifierCommand(playerTwo.GeneralModifiers, cnlEnemy);
            //Invoker.AddNewCommand(simple);
            //Invoker.ExecuteCommands();
            
            players[0] = playerOne;
            players[1] = playerTwo;
        }

        public void CreateNewUser(ConfigurationData configData)
        {
            //UserDB user = configData.user;
            //users.Add(user);

            
            // 1- CARGAR LA CONFIGURATION DATA DE CADA PLAYER
            HelperCardCollectionJsonKimboko helperCardCollectionJsonKimboko = new HelperCardCollectionJsonKimboko();
            playerConfigurationData = helperCardCollectionJsonKimboko.GetConfigurationDataFromJson();

            UserDB userOne = playerConfigurationData.user;
            UserDB userTestJson = new UserDB("ppp");
            userTestJson.ID = "ppp";
            UserDB userTwo = playerConfigurationData.user;
            users[0] = userOne;
            //users[1] = userTwo;
            users[1] = userTestJson;
        }

        public Player[] GetPlayer()
        {
            return players;
        }

        public List<UserDB> GetUsers()
        {
            return users;
        }

    }
}