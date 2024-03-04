namespace Root
{
    public class CommonTypes
    {
        public class DirException : Exception
        {
            public DirException() :
            base("#### DirException: illegal direction parameter ####")
            { }
        }

        public class MoveException : Exception
        {
            public MoveException() :
            base("#### MoveException: unable to move to expected location ####")
            { }
        }

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
        }

        public struct Cell_Oriented_Contamination
        {
            public float A { get; set; } // air
            public float B { get; set; } // soil and water
            public float C { get; set; } // grass
            public Cell_Oriented_Contamination(float a, float b, float c)
            {
                A = a; B = b; C = c;
            }
        }

        public struct Nutrition
        {
            public float I { get; set; }
            public float J { get; set; }
            public float K { get; set; }
            public Nutrition(float i, float j, float k)
            {
                I = i; J = j; K = k;
            }
        }

        public struct Food
        {
            public float Calories { get; set; }
            public Nutrition Nutrition { get; set; }
            public Food(float cal, Nutrition nutr)
            {
                Calories = cal; Nutrition = nutr;
            }
        }

        public class CellType
        {
            public struct Soil
            {
                public float Soil_Hum { get; set; } // seperate from Env.Humidity
                public float Cont_B { get; set; } // Cell_Oriented_Contamination.B
                public Soil(float h, float b) { Soil_Hum = h; Cont_B = b; }
            }
            public struct Grass
            { // harvested as crops if 90+ fresh at season, turns to soil if 0 fresh
                public float Freshness { get; set; } // [0, 100]
                public float Cont_C { get; set; }
                public Grass(float f, float c) { Freshness = f; Cont_C = c; }
            }
            public struct Tree // unoccupiable
            { // harvested as crops if 90+ fresh at season, dies if 0 fresh
                public float Freshness { get; set; } // [0, 100]
                public Tree(float f) { Freshness = f; }
            }
            public struct Water // unoccupiable
            {
                public float Cont_B { get; set; }
                public Water(float b) { Cont_B = b; }
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
}