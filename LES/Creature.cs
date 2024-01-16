﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LES
{
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