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

            public static byte Dir = 0;

            public static void Turn(byte factor)
            {
                Dir += factor;
            }

            public void Move() // assumes move is legal
            {
                try
                {
                    byte rem = (byte)(Dir % 8);
                    if (rem == 0) { Col++; }
                    else if (rem == 1) { Row--; Col++; }
                    else if (rem == 2) { Row--; }
                    else if (rem == 3) { Row--; Col--; }
                    else if (rem == 4) { Col--; }
                    else if (rem == 5) { Row++; Col--; }
                    else if (rem == 6) { Row++; }
                    else if (rem == 7) { Row++; Col++; }
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

        public class Season
        { // [0, 3) winter, [3, 6) spring, [6, 9) summer, [9, 12) autumn
            public float Val { get; set; }
            public Season(float v) { Val = v; }
        }

        public class Environment
        {
            public class Temperature
            {
                public float Val { get; set; } // Celcius
                public Temperature(float v) { Val = v; }
            }

            public class Humidity
            {
                public float Val { get; set; } // [0, 100]
                public Humidity(float v) { Val = v; }
            }

            public class Fire
            {
                public class IsFire
                {
                    public Coord Location { get; set; }
                    public IsFire(Coord loc) { Location = loc; }
                    public Temperature TempInc(Coord myloc)
                    { // increment of Temp caused by fire
                        float d = Distance(myloc, Location);
                        return new Temperature(10 / d / d);
                    }
                }
                public class NoFire { }
            }
        }

        public class Contamination
        {
            public class A
            { // soil and water
                public float Val { get; set; }
                public A(float v) { Val = v; }
                public float ICont_Val() { return Val; }
            }
            public class B
            { // grass
                public float Val { get; set; }
                public B(float v) { Val = v; }
                public float ICont_Val() { return Val; }
            }
            public class C
            { // air
                public float Val { get; set; }
                public C(float v) { Val = v; }
                public float ICont_Val() { return Val; }
            }
            public class D
            { // intercreature
                public float Val { get; set; }
                public D(float v) { Val = v; }
                public float ICont_Val() { return Val; }
            }
        }

        public class GridType
        {
            public class Soil
            {
                public float Soil_Hum { get; set; } // seperate from IEnv Humidity
                public Contamination.A Cont_A { get; set; }
                public Soil(float h, Contamination.A c) { Soil_Hum = h; Cont_A = c; }
            }
            public class Grass
            { // harvested as crops if 90+ fresh at season, turns to soil if 0 fresh
                public float Freshness { get; set; } // [0, 100]
                public Contamination.B Cont_B { get; set; }
                public Grass(float f, Contamination.B c) { Freshness = f; Cont_B = c; }
            }
            public class Tree // unoccupiable
            { // harvested as crops if 90+ fresh at season, dies if 0 fresh
                public float Freshness { get; set; } // [0, 100]
                public Tree(float f) { Freshness = f; }
            }
            public class Water // unoccupiable
            {
                public Contamination.A Cont_A { get; set; }
                public Water(Contamination.A c) { Cont_A = c; }
            }
            public class Rock { } // just a barrier // unoccupiable
            public class Sand { } // just a blank grid
        }

        public struct GridInfo
        {
            public Coord Coords { get; }
            public Environment.Temperature Temperature { get; set; }
            public Environment.Humidity Humidity { get; set; }
            public Environment.Fire Fire { get; set; }
        }
    }
}