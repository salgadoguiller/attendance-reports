using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2._0
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase se encarga de almacenar la información necesaria para 
    //          generar los reportes.
    // Propiedades:
    //          Nombre                  -> nombre del empleado del que se genera el reporte.
    //          FechaEntradaMonday      -> hora de entrada lunes.
    //          FechaEntradaTuesday     -> hora de entrada martes.
    //          FechaEntradaWendnsday   -> hora de entrada miercoles.
    //          FechaEntradaThursday    -> hora de entrada jueves.
    //          FechaEntradaFriday      -> hora de entrada viernes.
    //          FechaEntradaSaturday    -> hora de entrada sabado.
    //          FechaEntradaSunday      -> hora de entrada domingo.
    //          FechaSalidaMonday       -> hora de salida lunes.
    //          FechaSalidaTuesday      -> hora de salida martes.
    //          FechaSalidaWendnsday    -> hora de salida miercoles.
    //          FechaSalidaThursday     -> hora de salida jueves.
    //          FechaSalidaFriday       -> hora de salida viernes.
    //          FechaSalidaSaturday     -> hora de salida sabado.
    //          FechaSalidaSunday       -> hora de salida domingo.
    //          HorasMonday             -> total de horas trabajadas el lunes.
    //          HorasTuesday            -> total de horas trabajadas el martes.
    //          HorasWendnsday          -> total de horas trabajadas el miercoles.
    //          HorasThursday           -> total de horas trabajadas el jueves.
    //          HorasFriday             -> total de horas trabajadas el viernes.
    //          HorasSaturday           -> total de horas trabajadas el sabado.
    //          HorasSunday             -> total de horas trabajadas el domingo.
    //          SensorInMonday          -> representa si la marcacion de entrada se realizo de manera presencial o remota.
    //          SensorOutMonday         -> representa si la marcacion de salida se realizo de manera presencial o remota.
    //          SensorInTuesday         -> representa si la marcacion de entrada se realizo de manera presencial o remota.
    //          SensorOutTuesday        -> representa si la marcacion de salida se realizo de manera presencial o remota.
    //          SensorInWednesday       -> representa si la marcacion de entrada se realizo de manera presencial o remota.
    //          SensorOutWednesday      -> representa si la marcacion de salida se realizo de manera presencial o remota.
    //          SensorInThursday        -> representa si la marcacion de entrada se realizo de manera presencial o remota.
    //          SensorOutThursday       -> representa si la marcacion de salida se realizo de manera presencial o remota.
    //          SensorInFriday          -> representa si la marcacion de entrada se realizo de manera presencial o remota.
    //          SensorOutFriday         -> representa si la marcacion de salida se realizo de manera presencial o remota.
    //          SensorInSaturday        -> representa si la marcacion de entrada se realizo de manera presencial o remota.
    //          SensorOutSaturday       -> representa si la marcacion de salida se realizo de manera presencial o remota.
    //          SensorInSunday          -> representa si la marcacion de entrada se realizo de manera presencial o remota.
    //          SensorOutSunday         -> representa si la marcacion de salida se realizo de manera presencial o remota.
    // Metodos: no tiene ningun metodo.
    // ------------------------------------------------------------------------------
    class InfoReport
    {
        public string Nombre { get; set; }
        public DateTime? FechaEntradaMonday { get; set; }
        public DateTime? FechaEntradaTuesday { get; set; }
        public DateTime? FechaEntradaWendnsday { get; set; }
        public DateTime? FechaEntradaThursday { get; set; }
        public DateTime? FechaEntradaFriday { get; set; }
        public DateTime? FechaEntradaSaturday { get; set; }
        public DateTime? FechaEntradaSunday { get; set; }
        public DateTime? FechaSalidaMonday { get; set; }
        public DateTime? FechaSalidaTuesday { get; set; }
        public DateTime? FechaSalidaWendnsday { get; set; }
        public DateTime? FechaSalidaThursday { get; set; }
        public DateTime? FechaSalidaFriday { get; set; }
        public DateTime? FechaSalidaSaturday { get; set; }
        public DateTime? FechaSalidaSunday { get; set; }
        public Decimal? HorasMonday { get; set; }
        public Decimal? HorasTuesday { get; set; }
        public Decimal? HorasWendnsday { get; set; }
        public Decimal? HorasThursday { get; set; }
        public Decimal? HorasFriday { get; set; }
        public Decimal? HorasSaturday { get; set; }
        public Decimal? HorasSunday { get; set; }
        public int? SensorInMonday { get; set; }
        public int? SensorOutMonday { get; set; }
        public int? SensorInTuesday { get; set; }
        public int? SensorOutTuesday { get; set; }
        public int? SensorInWednesday { get; set; }
        public int? SensorOutWednesday { get; set; }
        public int? SensorInThursday { get; set; }
        public int? SensorOutThursday { get; set; }
        public int? SensorInFriday { get; set; }
        public int? SensorOutFriday { get; set; }
        public int? SensorInSaturday { get; set; }
        public int? SensorOutSaturday { get; set; }
        public int? SensorInSunday { get; set; }
        public int? SensorOutSunday { get; set; }
    }
}
