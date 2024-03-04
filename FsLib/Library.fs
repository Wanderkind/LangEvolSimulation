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

let thermo_metabolism (cond: thermo_condition) : (temperature -> float32) =
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
    (fun temp -> func (temp.Val))

let health_mechanism : (CT.Contamination -> CT.Cell_Oriented_Contamination -> CT.Nutrition -> float32) = // reduction from health
    let func (intrc: CT.Contamination) (celor: CT.Cell_Oriented_Contamination) (ntrtn: CT.Nutrition) : float32 =
        let a, b, c = celor.A, celor.B, celor.C
        let i, j, k = ntrtn.I, ntrtn.J, ntrtn.K
        intrc.Val * relu (a + b - k) * relu (b + c - i) * relu (c + a - j) + intrc.Val
    func

type WorldConfig =
    
    struct
        
        val Version: string // "Pilot-0.1"
        val Size_Horizontal: uint16
        val Size_Vertical: uint16
        val Ambient_Temperature: temperature
        val Seasons: bool // Seasons give delta on ambient temp
        val Population: uint32
        val Ticks_Per_Day: uint32
        val MutationRate: uint32 // one mutation occurence in x genes passed on
        val Genome_Length: uint16
        val Inner_Neurons: uint16

        val Sex_Differences: string
        val Environment_Fields: string array
        val CellType_Fields: string list
        val Contamination_Fields: uint16
        val Cell_Oriented_Contamination_Fields: uint16
        val Nutrition_Fields: uint16
        val Vital_Fields: string array // health, calories, etc.
        val Thermo_Condition: thermo_condition
        val Health_Mechanism: string
        val Features_To_Add: string // protein, immunity, contamination sensitivity, etc.

    end