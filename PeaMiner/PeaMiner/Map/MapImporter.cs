using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PeaMiner
{
    public class MapImporter
    {
        public static void Import(string fileLocation, Map map, out Player player, out Bus bus)
        {
            TheGame.Instance.map.Clear();
            TheGame.Instance.gameObjectList.Clear();

            if (fileLocation == String.Empty)
                throw new Exception("invalid map file");

            map.setDefaultType(GameObjectType.Dirt);
            /*
            map.resize(3, 3);
            map.set(2, 2, GameObjectType.PlayerSpawner);
            map.set(0, 1, GameObjectType.Free);
            map.set(1, 1, GameObjectType.Pea);
            */

            Point busStart = new Point(0, 0);
            Point busEnd = new Point(0, 0);

            int counter = 0;
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(fileLocation);
            while ((line = file.ReadLine()) != null)
            {
                string[] lineData = line.Split(' ');
                int[] lineDataInt = new int[lineData.Length];
                for (int i = 0; i < lineData.Length; i++)
                {
                    lineDataInt[i] = int.Parse(lineData[i]);
                }

                if (counter == 0)
                {
                    map.resize(lineDataInt[0], lineDataInt[1]);
                }
                else
                {
                    for(int j = 0; j < map.getWidth(); j++)
                    {
                        map.set(counter-1, j, (GameObjectType)lineDataInt[j]);

                        if (((GameObjectType)lineDataInt[j]) == GameObjectType.BusStart)
                            busStart = new Point(j, counter - 1);
                        if (((GameObjectType)lineDataInt[j]) == GameObjectType.BusFinish)
                            busEnd = new Point(j, counter - 1);
                    }
                }

                counter++;
            }

            file.Close();


            Point playerPos = map.getPlayerPos();

            player = new Player(new Vector2(playerPos.X * map.getBlockRectangle().Width + 1, playerPos.Y * map.getBlockRectangle().Height + 1), new Vector2(map.getBlockRectangle().Width - 10, map.getBlockRectangle().Height - 10));

            if (busStart == new Point(0, 0) && busEnd == busStart)
            {
                bus = null;
            }
            else
            {
                bus = new Bus(
                    new Vector2(busStart.X * map.getBlockRectangle().Width + 1, busStart.Y * map.getBlockRectangle().Height + 1),
                    new Vector2(busEnd.X * map.getBlockRectangle().Width + 1, busEnd.Y * map.getBlockRectangle().Height + 1),
                    new Vector2(map.getBlockRectangle().Width - 10, map.getBlockRectangle().Height - 10));
            }
        }
    }
}
