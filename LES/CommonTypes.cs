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
using System.Runtime.InteropServices;
using System.ComponentModel.Design.Serialization;


namespace LES
{
    public class CommonTypes
    {
        public class DirException : Exception { public DirException() :
                base("#### DirException: illegal direction parameter ####") { } }

        public class MoveException : Exception { public MoveException() :
                base("#### MoveException: unable to move to expected location ####") { } }

        public struct Coord
        {
            public ushort Row { get; set; }
            public ushort Col { get; set; }
            public Coord(ushort r, ushort c) { Row = r; Col = c; }

            public void E() { Col++; }
            public void NE() { Row--; Col++; }
            public void N() { Row--; }
            public void NW() { Row--; Col--; }
            public void W() { Col--; }
            public void SW() { Row++; Col--; }
            public void S() { Row++; }
            public void SE() { Row++; Col++; }
        }

        public struct Creature_Location
        {
            public Coord Coords { get; set; }
            public byte Dir { get; set; }
            public Creature_Location(Coord c, byte d) { Coords = c; Dir = d; }

            public void Turn(byte factor) { Dir += factor; }
            public void Move() // assumes move is legal
            {
                try
                {
                    byte rem = (byte)(Dir % 8);
                    if (rem == 0) { Coords.E(); }
                    else if (rem == 1) { Coords.NE(); }
                    else if (rem == 2) { Coords.N(); }
                    else if (rem == 3) { Coords.NW(); }
                    else if (rem == 4) { Coords.W(); }
                    else if (rem == 5) { Coords.SW(); }
                    else if (rem == 6) { Coords.S(); }
                    else if (rem == 7) { Coords.SE(); }
                    else throw new DirException();
                }
                catch (Exception) { throw new MoveException(); }
            }
        }

        public static float Distance(Coord mycoord, Coord theircoord)
        {
            float x = mycoord.Row - theircoord.Row;
            float y = mycoord.Col - theircoord.Col;
            return MathF.Sqrt(x * x + y * y);
        }

        public struct Season // [0, 3) winter, [3, 6) spring, [6, 9) summer, [9, 12) autumn
        {
            public float Val { get; set; }
            public Season(float v) { Val = v; }
        }

        public class Environment
        {
            public struct Temperature
            {
                public float Val { get; set; } // Celcius
                public Temperature(float v) { Val = v; }
            }

            public struct Humidity
            {
                public float Val { get; set; } // [0, 100]
                public Humidity(float v) { Val = v; }
            }

            public class Fire
            {
                public struct IsFire
                {
                    public Coord Location { get; set; }
                    public IsFire(Coord loc) { Location = loc; }
                    public Temperature TempInc(Coord target_loc) // increment of Temp caused by fire
                    {
                        float d = Distance(target_loc, Location);
                        return new Temperature(10 / d / d);
                    }
                }
                public struct NoFire { }
            }
        }

        public struct Contamination // intercreature
        {
            public float Val { get; set; }
            public Contamination(float v) { Val = v; }
            public float ICont_Val() { return Val; }
        }

        public class Cell_Oriented_Contamination
        {
            public struct A // air
            {
                public float Val { get; set; }
                public A(float v) { Val = v; }
                public float ICont_Val() { return Val; }
            }
            public struct B // soil and water
            {
                public float Val { get; set; }
                public B(float v) { Val = v; }
                public float ICont_Val() { return Val; }
            }
            public struct C // grass
            {
                public float Val { get; set; }
                public C(float v) { Val = v; }
                public float ICont_Val() { return Val; }
            }
        }

        public class CellType
        {
            public struct Soil
            {
                public float Soil_Hum { get; set; } // seperate from Env.Humidity
                public Cell_Oriented_Contamination.B Cont_B { get; set; }
                public Soil(float h, Cell_Oriented_Contamination.B c) { Soil_Hum = h; Cont_B = c; }
            }
            public struct Grass
            { // harvested as crops if 90+ fresh at season, turns to soil if 0 fresh
                public float Freshness { get; set; } // [0, 100]
                public Cell_Oriented_Contamination.C Cont_C { get; set; }
                public Grass(float f, Cell_Oriented_Contamination.C c) { Freshness = f; Cont_C = c; }
            }
            public struct Tree // unoccupiable
            { // harvested as crops if 90+ fresh at season, dies if 0 fresh
                public float Freshness { get; set; } // [0, 100]
                public Tree(float f) { Freshness = f; }
            }
            public struct Water // unoccupiable
            {
                public Cell_Oriented_Contamination.B Cont_B { get; set; }
                public Water(Cell_Oriented_Contamination.B c) { Cont_B = c; }
            }
            public struct Rock { } // just a barrier // unoccupiable
            public struct Sand { } // just a blank cell
        }

        public class CellInfo // only used for admin inspection, not for info retrieval during simulation
        {
            public Coord Coord { get; }
            public CellType CellType { get; }
            public Environment.Temperature Temperature { get; set; }
            public Environment.Humidity Humidity { get; set; }
            public Environment.Fire Fire { get; set; }
            public Cell_Oriented_Contamination Air_Cont { get; set; }
            public Cell_Oriented_Contamination CT_Dependant_Cont { get; set; }
            public bool Occupied { get; set; }

            public CellInfo(Coord cd, CellType ct, Environment.Temperature et, Environment.Humidity eh,
                Environment.Fire ef, Cell_Oriented_Contamination coc_ac, Cell_Oriented_Contamination coc_cdc, bool oc)
            {
                Coord = cd; CellType = ct; Temperature = et; Humidity = eh;
                Fire = ef; Air_Cont = coc_ac; CT_Dependant_Cont = coc_cdc; Occupied = oc;
            }
        }
    }

    /*
    class Program
    {
        #pragma warning disable CS8321
        static void Main()
        {
            void inspect()
            {
                Console.WriteLine($"Size of Coord struct: {Marshal.SizeOf(typeof(CommonTypes.Coord))} bytes");
                Console.WriteLine($"Size of Creature_Location struct: {Marshal.SizeOf(typeof(CommonTypes.Creature_Location))} bytes");
                Console.WriteLine($"Size of Season struct: {Marshal.SizeOf(typeof(CommonTypes.Season))} bytes");
                Console.WriteLine($"Size of Temperature struct: {Marshal.SizeOf(typeof(CommonTypes.Environment.Temperature))} bytes");
                Console.WriteLine($"Size of Humidity struct: {Marshal.SizeOf(typeof(CommonTypes.Environment.Humidity))} bytes");
                Console.WriteLine($"Size of Fire.IsFire struct: {Marshal.SizeOf(typeof(CommonTypes.Environment.Fire.IsFire))} bytes");
                Console.WriteLine($"Size of Contamination struct: {Marshal.SizeOf(typeof(CommonTypes.Contamination))} bytes");
                Console.WriteLine($"Size of Cell_Oriented_Contamination.A struct: {Marshal.SizeOf(typeof(CommonTypes.Cell_Oriented_Contamination.A))} bytes");
                //Console.WriteLine($"Size of CellType class: {Marshal.SizeOf(typeof(CommonTypes.CellType))} bytes");
                Console.WriteLine($"Size of CellType.Grass struct: {Marshal.SizeOf(typeof(CommonTypes.CellType.Grass))} bytes");
                Console.WriteLine($"Size of CellInfo struct: {Marshal.SizeOf(typeof(CommonTypes.CellInfo))} bytes");
            }
            //inspect();
        }
    }
    */
}