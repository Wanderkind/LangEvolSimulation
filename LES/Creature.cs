using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//using Root;
using FsLib;
using CT = Root.CommonTypes;
using Env = Root.CommonTypes.Environment;


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

    // Creature.Calories에 대해 Metabolism 함수 필요

    public class Creature
    {
        //public Tuple<ushort, ushort> Coord { get; set; }
        public uint ID { get; set; }


        /*
        static float Metabolism(Env.Temperature temp)
        {
            Func<Env.Temperature, float> metab = BasicFsLib.thermo_metabolism(BasicFsLib.thermo_condition t_c);
            return 
        }
        */
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