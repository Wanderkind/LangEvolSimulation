module FsLib.BasicFsLib

// open Root

type CT = Root.CommonTypes
type temperature = CT.Environment.Temperature


let relu (x: float32) : float32 = if x < 0.f then 0.f else x

type r_slope =
| Sym
| Mnl of float32

type thermo_condition = {
    t_b: float32 // t_lc < t_b is a must
    t_lc: float32 // t_lc < t_uc is a must
    t_uc: float32 // t_uc < t_b is encouraged
    basal: float32 // calories per tick
    d_slope: r_slope
}

type WorldConfig =
    
    struct
        
        val Version: string // "Pilot-0.1"
        val Size_Horizontal: uint16
        val Size_Vertical: uint16
        val Ambient_Temperature: temperature
        val Seasons: bool // Seasons give delta on ambient temp
        val Population: uint32
        val Ticks_Per_Day: uint32
        val MutationRate: uint32 // never if 0, else one mutation occurence in x genes passed on
        val Genome_Length: uint16
        val Inner_Neurons: uint16

        val Sex_Differences: string
        val Environment_Fields: string array
        val CellType_Fields: string array
        val Contamination_Fields: uint16
        val Cell_Oriented_Contamination_Fields: uint16
        val Nutrition_Fields: uint16
        val Vital_Fields: string array // health, calories, etc.
        val Thermo_Condition: thermo_condition
        val Health_Mechanism: string
        val Features_To_Add: string // protein, immunity, contamination sensitivity, etc.

        new(ver: string, sh: uint16, sv: uint16, at: temperature, ssn: bool,
            pop: uint32, tpd: uint32, mr: uint32, gl: uint16, inr: uint16,
            sd: string, ef: string array, ctf: string array, cf: uint16, ccf: uint16,
            nf: uint16, vf: string array, tc: thermo_condition, hm: string, fta: string) =
            {
                Version = ver; Size_Horizontal = sh; Size_Vertical = sv; Ambient_Temperature = at;
                Seasons = ssn; Population = pop; Ticks_Per_Day = tpd; MutationRate = mr;
                Genome_Length = gl; Inner_Neurons = inr; Sex_Differences = sd; Environment_Fields = ef;
                CellType_Fields = ctf; Contamination_Fields = cf; Cell_Oriented_Contamination_Fields = ccf; Nutrition_Fields = nf;
                Vital_Fields = vf; Thermo_Condition = tc; Health_Mechanism = hm; Features_To_Add = fta
            }
    end

let world_config = WorldConfig(
    "Pilot-0.1", 300us, 300us, temperature(30.f), false, 60u, 500u, 0u, 16us, 4us,
    "None", [|"(environment_fields)"|], [|"(celltype_fields)"|],
    1us, 3us, 3us, [|"(vital_fields)"|],
    {t_b = 36.f; t_lc = 28.f; t_uc = 34.f; basal = 1600.f; d_slope = Sym},
    "(health_mechansim)", "(features_to_add)"
)

let get_wc () = world_config

let thermo_metabolism (temp: temperature) : float32 =
    let cond = world_config.Thermo_Condition
    let l, u, b, s = cond.t_lc, cond.t_uc, cond.t_b, cond.basal
    let func (t: float32) : float32 =
        if t < l then
            (b - t)*s/(b - l)
        else if u < t then
            match cond.d_slope with
            | Sym -> s + (t - u)*s/(b - l)
            | Mnl f -> s + (t - u)*f
        else
            s
    func temp.Val

let health_mechanism : (CT.Contamination -> CT.Cell_Oriented_Contamination -> CT.Nutrition -> float32) = // reduction from health
    let func (intrc: CT.Contamination) (celor: CT.Cell_Oriented_Contamination) (ntrtn: CT.Nutrition) : float32 =
        let a, b, c = celor.A, celor.B, celor.C
        let i, j, k = ntrtn.I, ntrtn.J, ntrtn.K
        intrc.Val * relu (a + b - k) * relu (b + c - i) * relu (c + a - j) + intrc.Val
    func