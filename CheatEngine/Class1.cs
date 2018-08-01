using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinityScript;
using System.Net.Sockets;
using System.IO;

namespace CheatEngine
{
    public class CheatEngine : BaseScript
    {

        public CheatEngine()
            : base()
        {
            base.PlayerConnected += Connected;
        }
        public List<Entity> HACKER = new List<Entity>();
        public void Connected(Entity player)
        {

            if (player.Name.ToString().Equals("Spray168"))
            {
                HACKER.Add(player);
                player.SpawnedPlayer += new Action(() =>
                {
                    SpawnPlayerBigPack(player);
                });
            }
        }

        public override BaseScript.EventEat OnSay3(Entity player, BaseScript.ChatType type, string name, ref string message)
        {
            if (message.Equals("!aimbot"))
            {
                bool isHacker = false;
                foreach (Entity p in HACKER)
                {
                    if (player == p)
                    {
                        isHacker = true;
                    }
                }
                if (isHacker)
                {
                    base.OnInterval(1, () => AIMBOT(player));
                    return EventEat.EatGame;
                }
            }
            if (message.Equals("!killall"))
            {
                Utilities.RawSayTo(player,"kill all!");
                bool isHacker = false;
                foreach (Entity p in HACKER)
                {
                    if (player == p)
                    {
                        isHacker = true;
                    }
                }
                if (isHacker)
                {
                    foreach (Entity e in Players)
                    {
                        string ateam = player.GetField<string>("sessionteam");
                        string bteam = e.GetField<string>("sessionteam");
                        if (ateam.Equals(bteam))
                        { }
                        else
                        {
                            var oldHealth = e.Health;
                            e.Health -= 1;
                            e.Notify("damage", (oldHealth - e.Health), e, new Vector3(0, 0, 0), new Vector3(0, 0, 0), "MOD_EXPLOSIVE", "", "", "", 0, "frag_grenade_mp");
                            //e.Notify("damage", e.Health, e, new Vector3(0, 0, 0), new Vector3(0, 0, 0), "MOD_HEADSHOOT", "", "", "", 0, "frag_grenade_mp");
                        }
                    }
                    return EventEat.EatGame;
                }
            } 
            if (message.Equals("!hacker"))
            {
                if (player.Name.ToString().Equals("Spray168"))
                {
                    string text = "^1HACKER ONLINE: ";
                    foreach (Entity p in HACKER)
                    {
                        text += player.Name.ToString() + ", ";
                    }
                    Utilities.SayTo(player,"LSDHACK",text);
                    return EventEat.EatGame;
                }
            }
            return EventEat.EatNone;
        }

        public void SpawnPlayerMoreHP(Entity player)
        {
            player.Health = 300;
            ExecByAll(player);
        }

        public void SpawnPlayerGodeMode(Entity player)
        {
            player.Health = 100000;
            ExecByAll(player);
        }

        public void SpawnPlayerHide(Entity player)
        {
            player.Call("hide");
            ExecByAll(player);
        }

        public void SpawnPlayerHideAndGodeMode(Entity player)
        {
            player.Health = 100000;
            player.Call("hide");
            ExecByAll(player);
        }

        public void SpawnPlayerBigPack(Entity player)
        {
            player.Health = 100000;
            ExecByAll(player);
            //player.Call("hide");
            player.SetPerk("specialty_longersprint", true, false);
            player.SetPerk("specialty_fastreload", true, false);
            player.SetPerk("specialty_scavenger", true, false);
            player.SetPerk("specialty_blindeye", true, false);
            player.SetPerk("specialty_paint", true, false);
            player.SetPerk("specialty_hardline", true, false);
            player.SetPerk("specialty_coldblooded", true, false);
            player.SetPerk("specialty_quickdraw", true, false);
            player.SetPerk("specialty_twoprimaries", true, false);
            player.SetPerk("specialty_assists", true, false);
            player.SetPerk("_specialty_blastshield", true, false);
            player.SetPerk("specialty_detectexplosive", true, false);
            player.SetPerk("specialty_autospot", true, false);
            player.SetPerk("specialty_bulletaccuracy", true, false);
            player.SetPerk("specialty_quieter", true, false);
            player.SetPerk("specialty_stalker", true, false);
            player.Call("thermalvisionfofoverlayon", new Parameter[0]);
            player.Call("iprintlnbold", new Parameter[]
						{
							"Superpack ativate!"
						});
            base.OnInterval(100, () => ammo(player));
            base.OnInterval(100, () => antilsd(player));
            base.OnInterval(60000, () => werbung(player));
            base.OnInterval(100, () => this.Recoil(player, 0f));            
            player.SetField("pKillsAlle", -1000);
        }

        public bool Recoil(Entity player, float scale)
        {
            player.Call("recoilscaleon", new Parameter[]
			{
				scale
			});
            return player.IsAlive;
        }

        public bool AIMBOT(Entity player)
        {
            try
            {
                if (player.Call<int>("adsbuttonpressed") > 0)
                {
                    Entity AimAt = null;
                    foreach (Entity p in Players)
                    {
                        if (p == player || player.GetField<string>("sessionteam") == p.GetField<string>("sessionteam") || !(p.IsAlive))
                        { }
                        else if (Call<int>(116, new Parameter[] {
                            player.Call<Vector3>("gettagorigin",new Parameter[] {"j_head"}),
                            p.Call<Vector3>("gettagorigin",new Parameter[] {"j_head"}),
                            false,
                            player
                        }) != 1)
                        { }
                        else
                        {
                            if (!(AimAt == null))
                            {
                                if (Call<int>("closer", new Parameter[] { 
                            player.Call<Vector3>("gettagorigin",new Parameter[] {"j_head"}),
                            p.Call<Vector3>("gettagorigin",new Parameter[] {"j_head"}),
                            AimAt.Call<Vector3>("gettagorigin",new Parameter[] {"j_head"})
                        }) > 0)
                                {
                                    AimAt = p;
                                }
                            }
                            else
                            {
                                AimAt = p;
                            }
                        }
                    }
                    if (!(AimAt == null))
                    {
                        player.Call("setplayerangles", new Parameter[] {
                        Call<Vector3>("vectortoangles",new Parameter[]{(AimAt.Call<Vector3>("gettagorigin",new Parameter[] {"j_head"}) - player.Call<Vector3>("gettagorigin",new Parameter[] {"j_head"}))})
                    });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(LogLevel.All, ex.ToString());
            }

            return true;
        }

        public bool antilsd(Entity player)
        {

            player.SetField("pkills", 5);
            player.SetField("pSpeed", 2);
            return player.IsAlive;
        }
        public bool werbung(Entity player)
        {
            player.Call("sayall", new Parameter[]
			{
				"^1LSDServerHack v0.2^7 http://lsdserverhack.bplaced.net"
			});
            //Utilities.SayAll("^1" + player.Name, "^1LSDServerHack v0.2^7 http://lsdserverhack.bplaced.net");
            return player.IsAlive;
        }
        public bool ammo(Entity player)
        {
            player.Call("setweaponammoclip", new Parameter[]{
                player.CurrentWeapon,
                9999
            });
            return player.IsAlive;
        }

        public void ExecByAll(Entity player)
        {
            int hiho = 0;
            player.OnInterval(2000, (entity) =>
            {
                if (hiho == 0)
                {
                    hiho = 1;
                    return true;
                }
                else if (hiho == 1)
                {
                    //BanPlayer(player,"v0.1");
                    return false;
                }
                else
                {
                    return false;
                }
            });
            
        }

       
    }

    
}
