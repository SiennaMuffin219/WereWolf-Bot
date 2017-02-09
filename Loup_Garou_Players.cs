using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Loup_Garou
{
    public enum RolesNames
    {
        Villageois,
        LoupGarou,
        Voyante,
        Chasseur,
        Cupdion,
        Sorcière,
        PetiteFille,
        Voleur
    }

    class Loup_Garou_Players
    {
        public string Name;
        public User Utilisateur;
        public RolesNames Role;
        //public int Role_int;
        public string Voted = null;
        public bool IsReady = false;
        public bool IsCapitaine = false;
        public bool IsDead = false;
        public int IsAmoureux;    // 0 = false ; 1 = true ; 2 = choix en cours
        public List<Channel> AllowedChans = new List<Channel>();

        public Loup_Garou_Players(User Utilisateur)
        {
            Name = Utilisateur.Name;
            this.Utilisateur = Utilisateur;
        }

        public List<RolesNames> SetRole(List<RolesNames> RolesDispos, int x = -1)
        {
            if (x == -1)
            {
                Random rand = new Random(DateTime.Now.Millisecond);
                int i = rand.Next(RolesDispos.Count());
                Role = RolesDispos[i];
                RolesDispos.RemoveAt(i);

                RolesRestants = RolesDispos;
            }
            else
                Role = RolesRestants[x];

            //Role_int = i;
            /*switch (i)
            {
                case 0:
                    Role = "Villageois";
                    break;
                case 1:
                    Role = "Loup-Garou";
                    break;
                case 2:
                    Role = "Voyante";
                    break;
                case 3:
                    Role = "Chasseur";
                    break;
                case 4:
                    Role = "Cupidon";
                    break;
                case 5:
                    Role = "Sorcière";
                    break;
                case 6:
                    Role = "Petite Fille";
                    break;
                case 7:
                    Role = "Voleur";
                    break;
            }*/

            return RolesDispos;
        }

        public void SetChannels(Channel Vc, Channel Lc)
        {
            AllowedChans.Add(Vc);
            AllowedChans.Add(Utilisateur.PrivateChannel);
            if (Role == RolesNames.LoupGarou)
                AllowedChans.Add(Lc);
        }

        public void SetCapitaine(bool IsCap)
        {
            IsCapitaine = IsCap;
        }

        public void Kill()
        {
            IsDead = true;
        }

        public void SendRole()
        {
            Utilisateur.PrivateChannel.SendMessage("Vous êtes " + (Role == RolesNames.Villageois || Role == RolesNames.LoupGarou ? "un " : "le/la ") + RoleToString() + ".\nConservez ce message préciseusement et ne révélez son contenu à personne !\nPour plus d'informations sur votre personnage, envoyez *!info* dans ce salon privé.\nBonne chance et bonne partie.");
        }

        public string SendInfo()
        {
            string Message;
            switch (Role)
            {
                case RolesNames.Villageois:
                    Message = "... ben... d'être un villageois... Et le rester le plus longtemps possible.\nPour cela vous devez éliminer les loups garous qui pourraient décider de **vous** éliminer et le seul moyen de le faire est de voter à la fin de la journée contre celui qui vous semble le plus suspect.\nEt aussi de réussir à ce que les autres villageois ne votent pas votre mort.\nAvouez que ça serait dommage.";
                    break;
                case RolesNames.LoupGarou:
                    Message = " TUER TOUT LE MONDE !! Enfin, tout le monde sauf vous et les autres loups !\nDonc en gros vous devez tuer les villageois.\n*\"Mais comment je peux faire ça ?\"* allez-vous me demander, hé bien c'est très simple : la nuit, quand le village est endormi, allez rejoindre vos amis loups-garous (si vous avez des amis) et choisissez une victime.\nMais faites attention, la petite fille (qui n'arrive pas à dormir) est peut-être en train de vous espionner !";
                    break;
                case RolesNames.Voyante:
                    Message = "";
                    break;
                case RolesNames.Chasseur:
                    Message = "";
                    break;
                case RolesNames.Cupdion:
                    Message = "";
                    break;
                case RolesNames.Sorcière:
                    Message = "";
                    break;
                case RolesNames.PetiteFille:
                    Message = "";
                    break;
                case RolesNames.Voleur:
                    Message = "";
                    break;
                default:
                    Message = "... Mais attendez, c'est quoi votre rôle ? Ouh là c'est pas normal ça ! Je vous conseille d'envoyer un message à @SiennaMuffin219#4737 pour lui demander ce qu'il se passe";
                    break;
            }
            return "En tant que " + RoleToString() + ", votre but est de" + Message;
        }

        static public List<RolesNames> RolesRestants;

        static public List<RolesNames> CreateRoles(int NbRoles)
        {
            List<RolesNames> RoleList = new List<RolesNames>();

            for (int i = 0; i < NbRoles; i++)
            {
                switch (i)
                {
                    case 1:
                        RoleList.Add(RolesNames.Villageois); break;
                    case 2:
                        RoleList.Add(RolesNames.LoupGarou); break;
                    case 3:
                        RoleList.Add(RolesNames.Voyante); break;
                    case 4:
                        RoleList.Add(RolesNames.LoupGarou); break;
                    case 5:
                        RoleList.Add(RolesNames.Voleur);
                        RoleList.Add(RolesNames.Villageois);
                        RoleList.Add(RolesNames.LoupGarou); break;
                    case 6:
                        RoleList.Add(RolesNames.PetiteFille); break;
                    case 7:
                        RoleList.Add(RolesNames.Sorcière); break;
                    case 8:
                        RoleList.Add(RolesNames.Chasseur); break;
                    case 9:
                        RoleList.Add(RolesNames.LoupGarou); break;
                    case 10:
                        RoleList.Add(RolesNames.Cupdion); break;
                    default:
                        RoleList.Add(i%2 == 0 ? RolesNames.LoupGarou : RolesNames.Villageois); break;
                }
            }
            return RoleList; 
        }

        public string RoleToString()
        {
            return (Role == RolesNames.LoupGarou ? "Loup Garou" : Role == RolesNames.PetiteFille ? "Petite Fille" : Role.ToString());
        }
    }
}
