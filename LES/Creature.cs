using FsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//using Root;
//using FsLib;
using CT = Root.CommonTypes;
using Env = Root.CommonTypes.Environment;

using FSC = Microsoft.FSharp.Core;


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

        BasicFsLib.WorldConfig worldConfig = BasicFsLib.get_wc();
        //public FSC.FSharpFunc<Env.Temperature, float> metab = BasicFsLib.thermo_metabolism();

        //public Tuple<ushort, ushort> Coord { get; set; }
        public uint ID { get; set; }

        public static float Metabolism(Env.Temperature temp)
        {
            return BasicFsLib.thermo_metabolism(temp);
        }

        //public float Func<in Env.Temperature, out float>(Env.Temperature temp);
        //public Func<Env.Temperature, float> Metabolism = temp => metab(temp);

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