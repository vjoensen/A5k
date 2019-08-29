using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5k
{
    class AI
    {
        List<Ship> minions;
        List<SpaceObject> world;
        PlayerShip player;
        int frameFreq = 20;
        int frameCount;
        public AI(List<SpaceObject> world, PlayerShip player)
        {
            this.world = world;
            this.player = player;
            minions = new List<Ship>();
        }
        public void Update()
        {
            frameCount--;
            if (frameCount < 0)
            {
                frameCount = frameFreq;
                foreach (Ship minion in minions)
                {
                    minion.setTarget(player);
                }
            }
        }



        public void takeControl(Ship newMinion)
        {
            minions.Add(newMinion);
        }
    }
}
