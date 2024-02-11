using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace LES
{
    /*
    public interface INeuron
    {
        uint Neuron_ID { get; set; }
    }

    // public interface ISynapse
    // public interface IProtein
    // ...
    // public interface IGene
    // public interface IGenome

    // public class immunity
    // contamination sensitivity
    // ...

    */
    public class Creature
    {
        //public Tuple<ushort, ushort> Coord { get; set; }
        public uint ID { get; set; }

    }

    public class Creature_List
    {
        private List<Creature> Creatures { get; }
        public Creature_List()
        {
            Creatures = new List<Creature>();
        }
        public void AddCreature(Creature creature)
        {
            Creatures.Add(creature);
        }
        /*public Creature FindCreature_Coord(Tuple<ushort, ushort> coord)
        {
            return Creatures.Find(coord);
        }*/
    }
}