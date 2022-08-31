using System;

namespace SCG.SBOFramework.DI
{
    public class UDOBindAttribute : Attribute
    {
        public string Tabla { get; set; }
        public string Columna { get; set; }
        public bool SoloLectura { get; set; }
        public string ValorPredeterminado { get; set; }
        public bool Key { get; set; }
        
        public UDOBindAttribute(string columna)
        {
            Columna = columna;
            SoloLectura = false;
        }
    }
}