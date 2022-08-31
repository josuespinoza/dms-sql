namespace SCG.SBOFramework.UI
{
    public interface ISBOBindable
    {
        bool Ligada { get; set; }
        string ColumnaLigada { get; set; }
        string TablaLigada { get; set; }
        void AsignaBinding();
        void AsignaValorUI(string valor);
        void AsignaValorDataSource(string valor);
        string ObtieneValorDataSource();
        string ObtieneValorUI();
        void AsignaValorUserDataSource(string valor);
        string ObtieneValorUserDataSource();
    }
}