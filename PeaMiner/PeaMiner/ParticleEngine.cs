using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PeaMiner
{
    public class ParticleEngine
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        public List<Particle> particals;
        private List<Texture2D> textures;

        //private Vector2 Station;
        public ParticleEngine(List<Texture2D> textures, Vector2 location)
        {

            EmitterLocation = location;
            this.textures = textures;
            this.particals = new List<Particle>();
            random = new Random();
        }
        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(0.5f * (float)(random.NextDouble()) * 2.5f + (-2 + random.Next(5)), -(0.5f * (float)(random.NextDouble()) * 2.5f + (-2 + random.Next(5))));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            //Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 20 + random.Next(40);

            return new Particle(texture, position, velocity, angle, angularVelocity, size, ttl);
        }
        public void Update()
        {
            int total = 50;

            for (int i = 0; i < total; i++)
            {
                particals.Add(GenerateNewParticle());
            }

            for (int partical = 0; partical < particals.Count; partical++)
            {
                particals[partical].Update();
                if (particals[partical].TTL <= 0)
                {
                    particals.RemoveAt(partical);
                    partical--;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particals.Count; index++)
            {
                particals[index].Draw(spriteBatch);
            }
        }
    }
}
