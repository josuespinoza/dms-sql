using System.Collections.Generic;

namespace SCG.SBOFramework.DI
{
    public interface ILineasUDO
    {
        List<ILineaUDO> LineasUDO { get; }
        string TablaLigada { get; }
    }
}