using Terraria.ModLoader;

namespace EvilBiomeMimicCounterpart
{
	public class EvilBiomeMimicCounterpart : Mod
	{
        public override void Load()
        {
            Terraria.On_NPC.BigMimicSummonCheck += NPC_BigMimicSummonCheck;
        }

        private bool NPC_BigMimicSummonCheck(Terraria.On_NPC.orig_BigMimicSummonCheck orig, int x, int y, Terraria.Player user)
        {
			if (Terraria.Main.netMode == Terraria.ID.NetmodeID.MultiplayerClient || !Terraria.Main.hardMode)
			{
				return false;
			}
			int num = Terraria.Chest.FindChest(x, y);
			if (num < 0)
			{
				return false;
			}
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < 40; i++)
			{
				ushort num5 = Terraria.Main.tile[Terraria.Main.chest[num].x, Terraria.Main.chest[num].y].TileType;
				int num6 = Terraria.Main.tile[Terraria.Main.chest[num].x, Terraria.Main.chest[num].y].TileFrameX / 36;
				if (Terraria.ID.TileID.Sets.BasicChest[num5] && (num5 != 21 || num6 < 5 || num6 > 6) && Terraria.Main.chest[num].item[i] != null && Terraria.Main.chest[num].item[i].type > Terraria.ID.ItemID.None)
				{
					if (Terraria.Main.chest[num].item[i].type == Terraria.ID.ItemID.LightKey)
					{
						num2 += Terraria.Main.chest[num].item[i].stack;
					}
					else if (Terraria.Main.chest[num].item[i].type == Terraria.ID.ItemID.NightKey)
					{
						num3 += Terraria.Main.chest[num].item[i].stack;
					}
					else
					{
						num4++;
					}
				}
			}
			if (num4 == 0 && num2 + num3 == 1)
			{
				_ = 1;
				if (Terraria.ID.TileID.Sets.BasicChest[Terraria.Main.tile[x, y].TileType])
				{
					if (Terraria.Main.tile[x, y].TileFrameX % 36 != 0)
					{
						x--;
					}
					if (Terraria.Main.tile[x, y].TileFrameY % 36 != 0)
					{
						y--;
					}
					int number = Terraria.Chest.FindChest(x, y);
					for (int j = 0; j < 40; j++)
					{
						Terraria.Main.chest[num].item[j] = new Terraria.Item();
					}
					Terraria.Chest.DestroyChest(x, y);
					for (int k = x; k <= x + 1; k++)
					{
						for (int l = y; l <= y + 1; l++)
						{
							if (Terraria.ID.TileID.Sets.BasicChest[Terraria.Main.tile[k, l].TileType])
							{
								Terraria.Main.tile[k, l].ClearTile();
							}
						}
					}
					int number2 = 1;
					if (Terraria.Main.tile[x, y].TileType == 467)
					{
						number2 = 5;
					}
					if (Terraria.Main.tile[x, y].TileType >= 625)
					{
						number2 = 101;
					}
					Terraria.NetMessage.SendData(Terraria.ID.MessageID.ChestUpdates, -1, -1, null, number2, x, y, 0f, number, Terraria.Main.tile[x, y].TileType);
					Terraria.NetMessage.SendTileSquare(-1, x, y, 3);
				}
				int num7 = Terraria.ID.NPCID.BigMimicHallow;
				if (num3 == 1)
				{
					if (user.ZoneCrimson)
                    {
						num7 = Terraria.ID.NPCID.BigMimicCrimson;
					}
					else if (user.ZoneCorrupt)
                    {
						num7 = Terraria.ID.NPCID.BigMimicCorruption;
					}
					else if (Terraria.WorldGen.crimson)
                    {
						num7 = Terraria.ID.NPCID.BigMimicCrimson;
					}
					else
                    {
						num7 = Terraria.ID.NPCID.BigMimicCorruption;
					}
				}

				int num8 = Terraria.NPC.NewNPC(user.GetSource_TileInteraction(x, y), x * 16 + 16, y * 16 + 32, num7);
				Terraria.Main.npc[num8].whoAmI = num8;
				Terraria.NetMessage.SendData(Terraria.ID.MessageID.SyncNPC, -1, -1, null, num8);
				Terraria.Main.npc[num8].BigMimicSpawnSmoke();
			}
			return false;
		}
    }
}