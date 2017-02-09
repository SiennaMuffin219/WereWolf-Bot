using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;


namespace Loup_Garou
{
    

    class MyBot
    {
        bool PlayingLoups = false; int StartingLoups = 0;
        Server Serv;
        Channel Chan, VillageoisChan, LoupsChan;
        int Etape;

        List<Loup_Garou_Players> Players = new List<Loup_Garou_Players>();
        
        Invite loupinvite;
        Server loupserv;

        DiscordClient discord;
        CommandService commands;
        bool repeat = false;

        public MyBot()
        {
            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Debug;
                x.LogHandler = Log;
                x.AppName = "WereWolf The Game";
            })
            .UsingCommands(x =>
            {
                x.AllowMentionPrefix = true;
                x.PrefixChar = '!';
                x.ExecuteHandler = Executed;
            });

            /*discord.Ready += async (s, e) =>
            {
                Console.WriteLine(discord.Servers.Any());
                discord.SetGame("42");
            };*/

            commands = discord.GetService<CommandService>();



            MessageRecu();

            commands.CreateCommand("get")
                .Parameter("Variable", ParameterType.Optional)
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage(Etape.ToString());
                });

            commands.CreateCommand("help")
                .Parameter("HelpFunc", ParameterType.Optional)
                .Do(async (e) =>
                {
                    string Func = e.GetArg("HelpFunc").ToLower();
                    if (Func.Length > 0 && !(Func[0] > 47 && Func[0] < 58) || Func == "42" || Func == "1337")
                    {
                        if (Func[0] == '!')
                            Func = Func.Remove(0, 1);
                        switch (Func)
                        {
                            case "help":
                                await e.Channel.SendMessage("!help : (+ 1 paramètre [numéro de page] optionnel) Affiche une page de la liste des fonctions d'*Alpha test 42* disponibles");
                                break;
                            case "42":
                                await e.Channel.SendMessage("!42 : (+ 1 paramètre [nombre]) Affiche *x* fois \"42\", puis la Grande Réponse");
                                break;
                            case "leet":
                            case "1337":
                                await e.Channel.SendMessage("!1337 / !leet (+ 1 paramètre [phrase] optionnel) Affiche \"Leet\", puis votre message en *Leet speak*");
                                break;
                            case "pascal":
                            case "sierp":
                                await e.Channel.SendMessage("!sierp / !pascal : Affiche un triangle de Sierpinski de très grande beauté");
                                break;
                            case "hello":
                                await e.Channel.SendMessage("!hello : *Alpha test 42* vous répond avec grande dignité");
                                break;
                            case "repeat":
                                await e.Channel.SendMessage("!repeat : Active le mode \"Repeat\" : *Alpha test 42* répète ce que tout le monde dit");
                                break;
                            case "time":
                                await e.Channel.SendMessage("!time : Affiche la date et l'heure (de mon PC, donc pas forcément très à l'heure)");
                                break;
                            case "caca":
                                await e.Channel.SendMessage("!caca / !poop / !shitty / !prout : Prout");
                                break;

                            default:
                                await e.Channel.SendMessage("Commande non trouvée (maybe *coming soon*)");
                                break;
                        }
                    }
                    else
                    {
                        if (Func.Length == 0)
                            Func = "1";

                        await e.Channel.SendMessage("`Commandes disponibles :`\n`### Page " + Func + " ###`");

                        switch (Func)
                        {
                            case "1":                       // Nombre de commandes par page max = 8
                                await e.Channel.SendMessage("```\n- !help\n- !1337\n- !42\n- !hello\n- !prim\n- !repeat\n- !sierp\n- !time\n```");
                                break;
                            case "2":
                                await e.Channel.SendMessage("```\n- !caca\n- *More coming soon*```");
                                break;
                            default:
                                await e.Channel.SendMessage("`Error 404: Page not Found`");
                                break;
                        }
                    }
                });



            commands.CreateCommand("hello")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Yes, my Lord");
                    await e.Channel.SendFile("C:/Users/Olivier/Pictures/Bot.png");
                    
                });
            

            commands.CreateCommand("repeat")
                .Parameter("Param", ParameterType.Optional)
                .Do(async (e) =>
                {
                    if (e.GetArg("Param") == "?")
                        await e.Channel.SendMessage("Repeat mode is *" + (repeat ? "activated*" : "deactivated*"));
                    else
                    {
                        repeat = !repeat;
                        if (repeat)
                            await e.Channel.SendMessage("*Repeat Mode Activated!*");
                        else
                            await e.Channel.SendMessage("*Repeat Mode " + (repeat ? "A" : "Dea") + "ctivated*");
                    }
                });

            
            

            commands.CreateCommand("time")
                .Description("Affiche la date et l'heure (de mon PC, donc pas forcément très à l'heure)")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage(DateTime.Now.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo));
                    await e.Channel.SendMessage(DateTime.Now.ToString("HH:mm:ss", DateTimeFormatInfo.InvariantInfo));
                });


            commands.CreateCommand("test")
                .Do(async (e) =>
                {
                    var ro = e.Server.FindRoles("Master").LastOrDefault();
                    await e.Server.Users.FirstOrDefault().AddRoles(ro);
                });
            commands.CreateCommand("test42")
                .Do(async (e) =>
                {
                    var ro = e.Server.FindRoles("Master").LastOrDefault();
                    await e.Server.Users.FirstOrDefault().RemoveRoles(ro);
                });

            commands.CreateCommand("test2")
                .Do(async (e) =>
                {
                    loupserv = await discord.CreateServer("WereWolf The Game", e.Server.Region);
                    try
                    {
                        loupinvite = await loupserv.CreateInvite(1800,null,false,true);
                    }
                    catch (Exception x)
                    {
                        await e.Channel.SendMessage("1 " + x.Message);
                    }
                    try
                    {
                        await e.Channel.SendMessage("https://discord.gg/" + loupinvite.Code);
                    }
                    catch (Exception x)
                    {
                        await e.Channel.SendMessage("2 " + x.Message);
                    }
                });
            commands.CreateCommand("test3")
                .Do(async (e) =>
                {
                    try
                    {
                        await e.Channel.SendMessage(loupserv.Users.ElementAt(0).ServerPermissions.CreateInstantInvite.ToString());
                    }
                    catch (Exception x)
                    {
                        await e.Channel.SendMessage("1 " + x.Message);
                    }
                });
            commands.CreateCommand("test4")
                .Do(async (e) =>
                {
                    await loupserv.Delete();
                    await e.Channel.SendMessage("Server deleted");
                });
            commands.CreateCommand("test5")
                .Do(async (e) =>
                {
                    await loupserv.Delete();
                    await e.Channel.SendMessage("Server deleted");
                });

            commands.CreateCommand("disconnect")
                .Alias("dis", "dc")
                .Do(async (e) =>
                {
                    Console.WriteLine("Disconnecting...");
                    await e.Channel.SendMessage("Disconnecting...");
                    discord.SetStatus(UserStatus.Invisible);
                    if (e.User.Name == "SiennaMuffin219")
                    {
                        Thread.Sleep(500);
                        await discord.Disconnect();
                    }
                });

            commands.CreateCommand("salons")
                .Do(async (e) =>
                {
                    try
                    {
                        if (e.Message.User.Name == "SiennaMuffin219")
                        {
                            await VillageoisChan.Delete();
                            await LoupsChan.Delete();
                        }
                    }
                    catch (Exception a)
                    {
                        await e.Channel.SendMessage(a.Message);
                    }
                });

            commands.CreateCommand("Loup")
                .Do(async (e) =>
                {
                    Chan = e.Channel;
                    Serv = e.Server;
                    await Chan.SendMessage("Commencer une partie de Loups Garous ?");
                    StartingLoups = 2;
                });

            commands.CreateCommand("info")
                .Do(async (e) =>
                {
                    try
                    {
                        if (e.Channel.IsPrivate && PlayingLoups && Players.Any(x => x.Name == e.User.Name))
                        {
                            await e.Channel.SendMessage(Players.Find(x => x.Name == e.User.Name).SendInfo());
                        }
                    }
                    catch (Exception a)
                    {
                        await e.Channel.SendMessage(a.Message);
                    }
                });


            commands.CreateCommand("new")
                .Parameter("Nb", ParameterType.Required)
                .Do(async (e) =>
                {
                    if (e.Message.User.Name == "SiennaMuffin219")
                    {
                        try
                        {
                            Random rand = new Random(DateTime.Now.Millisecond);
                            for (int i = 0; i < System.Convert.ToInt32(e.GetArg(0)); i++)
                            {
                                Players.Add(new Loup_Garou_Players(e.User));
                                Players.Last().Name = rand.Next().ToString();
                                await Chan.SendMessage($"Recherche de participants : {Players.Count}{(Players.Count > 1 ? " joueurs" : " joueur")}");
                                Players.Last().IsReady = true;
                            }
                            Etape = 1;
                            Players.ForEach(async (x) => await Chan.SendMessage(x.Name));
                        }
                        catch (Exception a)
                        {
                            await e.Channel.SendMessage(a.Message);
                        }
                    }
                });


            discord.ExecuteAndWait(async () =>
            {
                try
                {
                    await discord.Connect("Mjc3NTk0NzIzNjI1OTkyMTkz.C3gB0w.XVskkSNdjbdtpZhxEv8s0FDb17w", TokenType.Bot);
                    discord.SetGame("Être pas un MJ");
                }
                catch (Exception x)
                {
                    Console.WriteLine(x.Message);
                }
            });
        }
        

        private async void Loups(Message e)
        {
            int Etape2;
            do
            {
                Etape2 = Etape;
                switch (Etape)
                {
                    case 0:     // Recherche de joueurs
                        if (e.Text.ToLower().Contains("joue") && e.Channel == Chan && !Players.Any(x => x.Name == e.User.Name))
                        {
                            Players.Add(new Loup_Garou_Players(e.User));
                            await Chan.SendMessage("Recherche de participants : " + Players.Count.ToString() + (Players.Count > 1 ? " joueurs" : " joueur"));
                        }
                        if (Players.Any(x => x.Name == e.User.Name) && e.Text.ToLower().Contains("go") && e.Channel == Chan)
                        {
                            Etape = 1;
                        }
                        break;

                    case 1:     // Attente pour que tous les joueurs soient prêts
                        if (Players.Any(x => x.Name == e.User.Name) && e.Text.ToLower().Contains("no go") && e.Channel == Chan)
                            Players.Find(x => x.Name == e.User.Name).IsReady = false;

                        if (Players.Any(x => x.Name == e.User.Name) && e.Text.ToLower().Contains("go") && e.Channel == Chan)
                            Players.Find(x => x.Name == e.User.Name).IsReady = true;

                        await Chan.SendMessage(Players.Count(x => x.IsReady == true).ToString() + " joueur" + (Players.Count(x => x.IsReady == true) > 1 ? "s " : " ") + "prêt" + (Players.Count(x => x.IsReady == true) > 1 ? "s " : " ") + "sur " + Players.Count);
                        if (!Players.Any(x => x.IsReady == false))
                        {
                            await e.Channel.SendMessage("Tous les joueurs sont prêts, lancement de la partie en cours.");
                            List<RolesNames> Roles = new List<RolesNames>();
                            try { Roles = Loup_Garou_Players.CreateRoles(Players.Count); }
                            catch (Exception a) { await e.Channel.SendMessage(a.Message); }
                            /*foreach (var i in Players)
                                i.SetRole(Roles);*/
                            try { Players.ForEach(x => x.SetRole(Roles)); }
                            catch (Exception a) { await e.Channel.SendMessage(a.Message); }
                            //Chan.SendMessage("!salons");
                            Etape = 2;
                            try
                            {
                                Players.ForEach(async (x) => { await x.Utilisateur.SendMessage("Lancement de la partie de Loups Garous..."); });
                                Players.ForEach(x => x.SendRole());
                            }
                            catch (Exception a)
                            {
                                await e.Channel.SendMessage("Erreur lors de l'envoie des MP : " + a.Message);
                            }
                        }


                        break;

                    case 2:

                        try
                        {
                            VillageoisChan = await Serv.CreateChannel("Le_Village", ChannelType.Text);
                            await e.Channel.SendMessage(VillageoisChan.Mention);
                            LoupsChan = await Serv.CreateChannel("La_Nuit", ChannelType.Text);
                            await e.Channel.SendMessage(LoupsChan.Mention);
                            ChannelPermissions allow = new ChannelPermissions(false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
                            ChannelPermissions deny = new ChannelPermissions(true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true);
                            foreach (var Joue in Players.FindAll(x => x.Role != RolesNames.LoupGarou))
                            {
                                await LoupsChan.AddPermissionsRule(Joue.Utilisateur, allow, deny);
                            }
                        }
                        catch (Exception a)
                        {
                            Etape = -1;
                            await e.Channel.SendMessage("1 " + a.Message);
                        }
                        
                            //Players.ForEach(x => x.SetChannels(VillageoisChan, LoupsChan));

                        await LoupsChan.SendMessage("Si vous avez accès à ce salon, c'est que vous êtes parmi les loups.\nAttendez la nuit pour choisir votre prochaine victime et faites votre possible pour ne pas vous faire démasquer la journée.\nBonne chance.");
                        await VillageoisChan.SendMessage("Ce salon est la place du village, vous y avez accès la journée, pour discuter et débattre de choses et d'autres entre villageois.\nMais depuis quelques temps un malheur s'abat sur votre village : certain villageois les plus dignes de confiance se réveillent la nuit et se transforment en loups garous, pour tuer d'autres villageois !\n" +
                            "Faites votre possible pour éliminer ceux qui pourraient vous trahir d'une nuit à l'autre et utilisez votre pouvoir (si vous avez la chance d'en avoir un) pour trouver qui dit la vérité et qui ment.\nEt souvenez-vous, la confiance est une chose dont il vaut parfois mieux se douter :unamused:");

                        Thread.Sleep(7500);
                        await VillageoisChan.SendMessage("C'est à présent la nuit, tous les habitants s'endorment... :sleeping:");

                        try
                        {
                            if (Players.Any(x => x.Role == RolesNames.Voleur))
                            {
                                Etape = 3;
                                Thread.Sleep(750);

                                await VillageoisChan.SendMessage("Le voleur se réveille.");
                                var vol = Players.Find(x => x.Role == RolesNames.Voleur);
                                await vol.Utilisateur.PrivateChannel.SendMessage("Vous êtes le voleur : cela vous permet de prendre connaissance des rôles restants au début de la partie (c'est à dire maintenant) pour en changer.\nVoici les 2 rôles restants :\n" + Loup_Garou_Players.RolesRestants[0] + "\n" + Loup_Garou_Players.RolesRestants[1]);
                                if (Loup_Garou_Players.RolesRestants.Count(x => x == RolesNames.LoupGarou) == 2)
                                {
                                    await vol.Utilisateur.PrivateChannel.SendMessage("Les 2 cartes restantes étant des loups garous, vous êtes obligé de vous transformer en l'un d'eux.");
                                    vol.SetRole(Loup_Garou_Players.RolesRestants, 0);
                                    Thread.Sleep(5000);
                                    vol.SendRole();
                                    await VillageoisChan.SendMessage("Le voleur se rendort.");
                                    Etape = 4;
                                }
                                else
                                {
                                    await vol.Utilisateur.PrivateChannel.SendMessage("Vous avez le choix entre rester le voleur (votre pouvoir n'aura alors plus d'utilité pour le reste de la partie) en envoyant \"Rester\", ou bien prendre un des deux rôles restants en envoyant son nom.");
                                }
                            }
                            else
                                Etape = 4;
                        }
                        catch (Exception a)
                        {
                            Etape = -1;
                            await e.Channel.SendMessage("3 " + a.Message);
                        }

                        break;

                    case 3:     // Voleur
                        if (e.Channel == Players.Find(x => x.Role == RolesNames.Voleur).Utilisateur.PrivateChannel)
                        {
                            var vol = Players.Find(x => x.Role == RolesNames.Voleur);
                            if (e.Text.ToLower().Contains(Loup_Garou_Players.RolesRestants[0].ToString()) || e.Text.ToLower().Contains(Loup_Garou_Players.RolesRestants[1].ToString()))
                            {
                                vol.SetRole(Loup_Garou_Players.RolesRestants, Loup_Garou_Players.RolesRestants.FindIndex(x => e.Text.ToLower().Contains(x.ToString())));
                                vol.SendRole();
                                await VillageoisChan.SendMessage("Le voleur se rendort.");
                                Etape = 4;
                            }
                            else
                            {
                                if (e.Text.ToLower().Contains("rester"))
                                {
                                    await VillageoisChan.SendMessage("Le voleur se rendort.");
                                    Etape = 4;
                                }
                                else
                                    await vol.Utilisateur.PrivateChannel.SendMessage("Aucun rôle ne correspond à votre message.");
                            }
                        }
                        else
                        {
                            if (e.Channel == VillageoisChan)
                            {
                                Random rand = new Random(DateTime.Now.Millisecond);
                                if (rand.Next(2) == 0)
                                    await e.Edit("Chut ! C'est la nuit !");
                                else
                                    await e.Edit("J'ai dit \"C'est la nuit\" !");
                                Thread.Sleep(1000);
                                await e.Delete();
                            }
                        }
                        break;

                    case 4:
                        if (Players.Any(x => x.Role == RolesNames.Cupdion))
                        {
                            Thread.Sleep(750);
                            await VillageoisChan.SendMessage("Cupidon se réveille");
                            var cup = Players.Find(x => x.Role == RolesNames.Cupdion);
                            string personnes = "";
                            Players.ForEach(x => personnes += x.Name + "\n");
                            await cup.Utilisateur.PrivateChannel.SendMessage("Vous êtes Cupidon : vous devez choisir 2 personnes pour les faire tomber amoureuses jusqu'à la fin de la partie.\n```" + personnes + "```\nChoisissez 2 personnes en envoyant leur nom l\'un après l\'autre.");
                            Etape = 5;
                        }
                        else
                        {
                            Etape = 6;
                        }
                        break;

                    case 5:
                        if (e.Channel == Players.Find(x => x.Role == RolesNames.Cupdion).Utilisateur.PrivateChannel)
                        {
                            var cup = Players.Find(x => x.Role == RolesNames.Cupdion);
                            if (Players.Any(x => x.Name.ToLower() == e.Text.ToLower()))
                            {
                                if (Players.Any(x => x.IsAmoureux == 2))
                                {
                                    var amour1 = Players.Find(x => x.Name.ToLower() == e.Text.ToLower()); amour1.IsAmoureux = 1;
                                    var amour2 = Players.Find(x => x.IsAmoureux == 2); amour2.IsAmoureux = 1;
                                    await amour1.Utilisateur.PrivateChannel.SendMessage("Vous avez été choisi par Cupidon pour brûler d'un amour fou avec " + amour2.Name + " !\nVotre but est à présent de survivre avec lui/elle.");
                                    await amour2.Utilisateur.PrivateChannel.SendMessage("Vous avez été choisi par Cupidon pour brûler d'un amour fou avec " + amour1.Name + " !\nVotre but est à présent de survivre avec lui/elle\nS'il/si elle meurt, vous mourez avec lui/elle.");
                                    await cup.Utilisateur.PrivateChannel.SendMessage("Vous avez fait votre travail, les amoureux se sont rencontrés et ils ne se quitteront plus.");
                                    Etape = 6;
                                    await VillageoisChan.SendMessage("Cupidon se rendort.");
                                }
                                else
                                {
                                    Players.Find(x => x.Name.ToLower() == e.Text.ToLower()).IsAmoureux = 2;
                                }
                            }
                            else
                            {
                                await cup.Utilisateur.PrivateChannel.SendMessage("Aucun nom ne correspond à votre message.");
                            }
                        }
                        else
                        {
                            if (e.Channel == VillageoisChan)
                            {
                                Random rand = new Random(DateTime.Now.Millisecond);
                                if (rand.Next(2) == 0)
                                    await e.Edit("Chut ! C'est la nuit !");
                                else
                                    await e.Edit("J'ai dit \"C'est la nuit\" !");
                                Thread.Sleep(1000);
                                await e.Delete();
                            }
                        }
                        break;

                    case 6:
                        if (Players.Any(x => x.Role == RolesNames.Voyante))
                        {
                            Thread.Sleep(750);
                            await VillageoisChan.SendMessage("La voyante se réveille");
                            var voy = Players.Find(x => x.Role == RolesNames.Voyante);
                            string personnes = "";
                            Players.ForEach(x => personnes += x.Name + "\n");
                            await voy.Utilisateur.PrivateChannel.SendMessage("Vous êtes la voyante : vous pouvez découvrir le rôle d'un habitant.\n```" + personnes + "```\nChoisissez 1 personne en envoyant son nom.");
                            Etape = 7;
                        }
                        else
                        {
                            Etape = 8;
                        }
                        break;

                    case 7:
                        if (e.Channel == Players.Find(x => x.Role == RolesNames.Voyante).Utilisateur.PrivateChannel)
                        {
                            var voy = Players.Find(x => x.Role == RolesNames.Voyante);
                            if (Players.Any(x => x.Name.ToLower() == e.Text.ToLower()))
                            {
                                await voy.Utilisateur.SendMessage("Votre boule de cristal magique (ou électronique) vous permet de découvrir le rôle de " + Players.Find(x => x.Name.ToLower() == e.Text.ToLower()).Name + " : il/elle est " + Players.Find(x => x.Name.ToLower() == e.Text.ToLower()).Role);
                                if (Players.Find(x => x.Name.ToLower() == e.Text.ToLower()).IsDead)
                                    await voy.Utilisateur.SendMessage("Et d'ailleurs il est mort");
                                Etape = 8;
                                await VillageoisChan.SendMessage("La voyante se rendort.");
                            }
                            else
                            {
                                await voy.Utilisateur.PrivateChannel.SendMessage("Aucun nom ne correspond à votre message.");
                            }
                        }
                        else
                        {
                            if (e.Channel == VillageoisChan)
                            {
                                Random rand = new Random(DateTime.Now.Millisecond);
                                if (rand.Next(2) == 0)
                                    await e.Edit("Chut ! C'est la nuit !");
                                else
                                    await e.Edit("J'ai dit \"C'est la nuit\" !");
                                Thread.Sleep(1000);
                                await e.Delete();
                            }
                        }
                        break;

                    case 8:
                        if (Players.Any(x => x.Role == RolesNames.LoupGarou))
                        {
                            if (!Players.Any(x => x.Role != RolesNames.LoupGarou))
                            {
                                await VillageoisChan.SendMessage("Tous les villageois sont morts, les loups garous ont gagné !");
                                Etape = 10;
                                break;
                            }
                            Thread.Sleep(750);
                            await VillageoisChan.SendMessage("Les loups garous se réveillent !");
                            string personnes = "";
                            Players.FindAll(x => x.Role != RolesNames.LoupGarou).ForEach(x => personnes += (x.IsDead ? "~~" + x.Name + "~~ (mort)\n": x.Name + "\n"));
                            await LoupsChan.SendMessage("C'est enfin la nuit, vous allez pouvoir décider d'une nouvelle victime !\n```" + personnes + "```\nVous pouvez débattre entre loups ; quand vous aurez trouvé votre victime, envoyez *!vote* puis son nom.\nVous pouvez revoir la liste en envoyant *!list*");
                            Etape = 9;
                        }
                        else
                        {
                            await VillageoisChan.SendMessage("Tous les loups garous sont morts, les villageois ont gagné !");
                            Etape = 10;
                        }
                        break;

                    case 9:
                        if (e.Channel == LoupsChan && (e.RawText == "!vote" || e.RawText == "! vote" || e.RawText == "! Vote" || e.RawText == "!Vote"))
                        {
                            await LoupsChan.SendMessage("Début du vote");
                            Etape = 10; Etape2 = 10;
                        }
                        break;

                    case 10:
                        if (e.Channel == LoupsChan)
                        {
                            if (Players.Any(x => x.Role != RolesNames.LoupGarou && x.Name.ToLower() == e.Text.ToLower()))
                            {
                                await LoupsChan.SendMessage($"");
                                Etape = 8;
                                await VillageoisChan.SendMessage("Les loups garous se rendorment.");
                            }
                            else
                            {
                                await LoupsChan.SendMessage("Aucun nom ne correspond à votre message.");
                            }
                        }
                        else
                        {
                            if (e.Channel == VillageoisChan)
                            {
                                Random rand = new Random(DateTime.Now.Millisecond);
                                if (rand.Next(2) == 0)
                                    await e.Edit("Chut ! C'est la nuit !");
                                else
                                    await e.Edit("J'ai dit \"C'est la nuit\" !");
                                Thread.Sleep(1000);
                                await e.Delete();
                            }
                        }
                        break;

                    case 11:

                        break;

                    case -1:
                        await VillageoisChan.SendMessage("Hum... Y a un petit problème on dirai, je vais demander à @SiennaMuffin219#4737 ce qu'il se passe et je reviens.");
                        break;
                }
            } while (Etape != Etape2);
        }



        private void MessageRecu()
        {
            discord.MessageReceived += async (s, e) =>
            {

                if (PlayingLoups && !e.Message.IsAuthor)
                    Loups(e.Message);

                if (StartingLoups == 1)
                {
                    if (!e.Message.IsAuthor && e.Channel == Chan)
                    {
                        if (e.Message.Text.Contains("Oui"))
                        {
                            await Chan.SendMessage("*Let's go!!*");
                            StartingLoups = 0; PlayingLoups = true;
                            Etape = 0;
                        }
                        else
                        {
                            StartingLoups = 0;
                        }
                    }
                }
                if (StartingLoups == 2)
                    StartingLoups = 1;



                /*if (e.Message.IsAuthor && e.Message.Text == "!salons")
                {
                    try
                    {
                        VillageoisChan = await Serv.CreateChannel("Le_Village", ChannelType.Text);
                        await e.Channel.SendMessage(VillageoisChan.Mention);
                        LoupsChan = await Serv.CreateChannel("La_Nuit", ChannelType.Text);
                        await e.Channel.SendMessage(LoupsChan.Mention);
                        ChannelPermissions allow = new ChannelPermissions(false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
                        ChannelPermissions deny = new ChannelPermissions(true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true);
                        foreach (var Joue in Players.FindAll(x => x.Role != "Loup Garou"))
                        {
                            await LoupsChan.AddPermissionsRule(Joue.Utilisateur, allow, deny);
                        }
                    }
                    catch (Exception a)
                    {
                        Etape = -1;
                        await e.Channel.SendMessage("1" + a.Message);
                    }

                    //try
                    {
                        Players.ForEach(x => x.SetChannels(VillageoisChan, LoupsChan));

                        Players.ForEach(async (x) => { await x.Utilisateur.SendMessage(" "); });
                        Players.ForEach(x => x.SendRole());
                        await LoupsChan.SendMessage("Si vous avez accès à ce salon, c'est que vous êtes parmi les loups.\nAttendez la nuit pour choisir votre prochaine victime et faites votre possible pour ne pas vous faire démasquer la journée.\nBonne chance.");
                        await VillageoisChan.SendMessage("Ce salon est la place du village, vous y avez accès la journée, pour discuter et débattre de choses et d'autres entre villageois.\nMais depuis quelques temps un malheur s'abat sur votre village : certain villageois les plus dignes de confiance se réveillent la nuit et se transforment en loups garous, pour tuer d'autres villageois !\nFaites votre possible pour éliminer ceux qui pourraient vous trahir d'une nuit à l'autre et utilisez votre pouvoir (si vous avez la chance d'en avoir un) pour trouver qui dit la vérité et qui ment.\nEt souvenez-vous, la confiance est une chose dont il vaut parfois mieux se douter :unamused:");

                        Thread.Sleep(5000);
                        await VillageoisChan.SendMessage("C'est à présent la nuit, tous les habitants s'endorment... :sleeping:");
                    }
                    /*catch (Exception a)
                    {
                        Etape = -1;
                        await e.Channel.SendMessage("2" + a.Message);
                    }

                    try
                    {
                        if (Players.Any(x => x.Role == "Voleur"))
                        {
                            Etape = 2;
                            Thread.Sleep(750);

                            await VillageoisChan.SendMessage("Le voleur se réveille.");
                            var vol = Players.Find(x => x.Role == "Voleur");
                            await vol.Utilisateur.PrivateChannel.SendMessage("Vous êtes le voleur : cela vous permet de prendre connaissance des rôles restants au début de la partie (c'est à dire maintenant) pour en changer.\nVoici les 2 rôles restants :\n" + Loup_Garou_Players.RolesRestants[0] + "\n" + Loup_Garou_Players.RolesRestants[1]);
                            if (Loup_Garou_Players.RolesRestants.Count(x => x == "Loup Garou") == 2)
                            {
                                await vol.Utilisateur.PrivateChannel.SendMessage("Les 2 cartes restantes étant des loups garous, vous êtes obligé de vous transformer en l'un d'eux.");
                                vol.SetRole(Loup_Garou_Players.RolesRestants, 0);
                                Thread.Sleep(5000);
                                vol.SendRole();
                                await VillageoisChan.SendMessage("Le voleur se rendort.");
                                Etape = 3;
                            }
                            else
                            {
                                await vol.Utilisateur.PrivateChannel.SendMessage("Vous avez le choix entre rester le voleur (votre pouvoir n'aura alors plus d'utilité pour le reste de la partie) en envoyant \"Rester\", ou bien prendre un des deux rôles restants en envoyant son nom.");
                            }
                        }
                        else
                            Etape = 4;
                    }
                    catch (Exception a)
                    {
                        Etape = -1;
                        await e.Channel.SendMessage("3" + a.Message);
                    }
                }*/

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss", DateTimeFormatInfo.InvariantInfo) + " - " + e.User + "  on  " + e.Message.Channel);
                Console.WriteLine("         \"" + e.Message.Text + "\"");

                

                if (!e.Message.IsAuthor && repeat)
                    await e.Channel.SendMessage(e.Message.Text);
            };
        }


        

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            ConsoleColor color = ConsoleColor.Yellow;
            Console.ForegroundColor = color;
            Console.Write(DateTime.Now.ToString("HH:mm:ss", DateTimeFormatInfo.InvariantInfo));

            Console.BackgroundColor = ConsoleColor.Black;
            switch (e.Severity)
            {
                case LogSeverity.Error: color = ConsoleColor.Red; break;
                case LogSeverity.Warning: color = ConsoleColor.Yellow; break;
                case LogSeverity.Info: color = ConsoleColor.White; break;
                case LogSeverity.Verbose: color = ConsoleColor.Gray; break;
                case LogSeverity.Debug: default: color = ConsoleColor.DarkGray; break;
            }
            Console.ForegroundColor = color;

            Console.WriteLine(" - " + e.Source + " - " + e.Message);
        }
        
        private void Executed(object sender, CommandEventArgs e)
        {
            Console.WriteLine(42);
        }
    }
}