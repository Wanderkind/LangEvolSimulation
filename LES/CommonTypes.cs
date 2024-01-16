using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LES
{
    public class CommonTypes
    {
        public class DirException : Exception
        {
            public DirException() : base("#### DirException: illegal direction parameter ####") { }
        }

        public class MoveException : Exception
        {
            public MoveException() : base("#### MoveException: unable to move to expected location ####") { }
        }

        public struct Coord
        {
            public ushort Row { get; set; }
            public ushort Col { get; set; }


            public Coord(ushort r, ushort c) { Row = r; Col = c; }

            public sbyte Dir = 0;

            public void Turn(sbyte factor)
            {
                Dir += factor;
                while (Dir < 0) Dir += 8;
                while (7 < Dir) Dir -= 8;
            }

            public void Move() // assumes move is legal
            {
                try
                {
                    if      (Dir == 0) { Col++; }
                    else if (Dir == 1) { Row--; Col++; }
                    else if (Dir == 2) { Row--; }
                    else if (Dir == 3) { Row--; Col--; }
                    else if (Dir == 4) { Col--; }
                    else if (Dir == 5) { Row++; Col--; }
                    else if (Dir == 6) { Row++; }
                    else if (Dir == 7) { Row++; Col++; }
                    else throw new DirException();
                }
                catch (Exception)
                {
                    throw new MoveException();
                }
            }
        }

        public static float Distance(Coord mycoord, Coord theircoord)
        {
            float x = mycoord.Row - theircoord.Row;
            float y = mycoord.Col - theircoord.Col;
            return MathF.Sqrt(x * x + y * y);
        }

        public interface IEnvironment { }
        public class Temperature : IEnvironment
        {
            public float Val { get; set; }
            public Temperature(float v) { Val = v; }
        }

        public class Humidity : IEnvironment
        {
            public float Val { get; set; }
            public Humidity(float v) { Val = v; }
        }

        public class Fire : IEnvironment
        {
            public Coord Location { get; set; }
            public Fire(Coord loc) { Location = loc; }
            public Temperature TempInc(Coord myloc) // increment of Temp caused by fire
            {
                float d = Distance(myloc, Location);
                return new Temperature(10 / d / d);
            }
        }

        // public class Contamination : IEnvironment

        public interface IGridType { }
        public class Soil : IGridType { }
        public class Grass : IGridType { }
        public class Tree : IGridType { }
        public class Water : IGridType { }
        public class Rock : IGridType { }

        //public class GridInfo

        public interface INeuron
        {
            uint Neuron_ID { get; set; }
        }

        // public interface ISynapse
        // public interface IProtein
        // ...
        // public interface IGene
        // public interface IGenome
    }
}