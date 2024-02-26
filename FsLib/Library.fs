module FsLib.BasicFsLib

// open Root

type CT = Root.CommonTypes
type temperature = CT.Environment.Temperature

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

let metabolism (cond: thermo_condition) : (temperature -> float32) =
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