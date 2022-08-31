namespace SCG.UX.Windows.CitasAutomaticas
{
    public interface IAgenda
    {
        int IdAgenda { get; set; }
        string Agenda { get; set; }
        int Intervalo { get; set; }
        string Abreviatura { get; set; }
    }
}